using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Custom.Confictura.Champions
{
    public struct ArachnidInfestationResident
    {
        public byte Slot, Kind, Tier;
        public ushort Hits, MaximumHits, Stam, Mana;
        public Point3D Position;
        public Guid Identity;
        public Mobile Actor;
        public Container Inventory;
        public DateTime NextSpecial;

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);
            writer.Write(Slot); writer.Write(Kind); writer.Write(Tier);
            writer.Write(Hits); writer.Write(MaximumHits); writer.Write(Stam); writer.Write(Mana);
            writer.Write(Position); writer.Write(Identity.ToString("N")); writer.Write(Actor != null && !Actor.Deleted && Actor.Alive);
            writer.Write(Actor); writer.Write(Inventory); writer.Write(NextSpecial);
        }

        public void Deserialize(GenericReader reader)
        {
            if (reader.ReadInt() != 0) throw new InvalidOperationException("Unknown resident snapshot format.");
            Slot = reader.ReadByte(); Kind = reader.ReadByte(); Tier = reader.ReadByte();
            Hits = reader.ReadUShort(); MaximumHits = reader.ReadUShort(); Stam = reader.ReadUShort(); Mana = reader.ReadUShort();
            Position = reader.ReadPoint3D(); Identity = new Guid(reader.ReadString()); bool live = reader.ReadBool();
            Actor = reader.ReadMobile(); Inventory = reader.ReadItem() as Container; NextSpecial = reader.ReadDateTime();
            if (live && Actor == null) Hits = 0;
            // A live creature can briefly retain health above its current maximum after
            // a stat/control effect ends. Preserve that snapshot; ordinary Hits assignment
            // clamps any later dormant hydration. It is not a corrupt positional record.
            if (Slot > 12 || Kind > 1 || Tier > 4 || MaximumHits == 0 || Identity == Guid.Empty)
                throw new InvalidOperationException(String.Format("Invalid resident snapshot: slot={0}, kind={1}, tier={2}, hits={3}, max={4}, identity={5}.",Slot,Kind,Tier,Hits,MaximumHits,Identity));
        }
    }

    public sealed class ArachnidInfestationPatch
    {
        public int Id, Parent, Birth, Failures, Births;
        public int ResponseBirths;
        public Point3D Center;
        public Mobile Guardian, Founder;
        public bool Established, Retired;
        public DateTime EstablishAfter, NextRefill;
        public byte ExpansionMask;
        public bool Initialized;
        public DateTime LastInterest;
        public List<ArachnidInfestationResident> Residents = new List<ArachnidInfestationResident>();

        public void Serialize(GenericWriter writer)
        {
            writer.Write(2);
            writer.Write(Id); writer.Write(Parent); writer.Write(Birth); writer.Write(Failures); writer.Write(Births);
            writer.Write(Center); writer.Write(Guardian); writer.Write(Founder);
            writer.Write(Established); writer.Write(Retired); writer.Write(EstablishAfter); writer.Write(NextRefill);
            writer.Write(ExpansionMask); writer.Write(Initialized); writer.Write(Residents.Count);
            foreach (ArachnidInfestationResident resident in Residents) resident.Serialize(writer);
            writer.Write(ResponseBirths);
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            if (version < 0 || version > 2) throw new InvalidOperationException("Unknown infestation patch format.");
            Id = reader.ReadInt(); Parent = reader.ReadInt(); Birth = reader.ReadInt(); Failures = reader.ReadInt(); Births = reader.ReadInt();
            Center = reader.ReadPoint3D(); Guardian = reader.ReadMobile(); Founder = reader.ReadMobile();
            Established = reader.ReadBool(); Retired = reader.ReadBool(); EstablishAfter = reader.ReadDateTime(); NextRefill = reader.ReadDateTime();
            if (version >= 1)
            {
                ExpansionMask = reader.ReadByte(); Initialized = reader.ReadBool(); int count = reader.ReadInt();
                if (ExpansionMask > 15 || count < 0 || count > 13) throw new InvalidOperationException("Invalid resident count or expansion state.");
                int slots = 0;
                for (int i = 0; i < count; ++i)
                {
                    ArachnidInfestationResident resident = new ArachnidInfestationResident(); resident.Deserialize(reader);
                    if ((slots & (1 << resident.Slot)) != 0) throw new InvalidOperationException("Duplicate resident slot.");
                    slots |= 1 << resident.Slot; Residents.Add(resident);
                }
            }
            if (version >= 2) ResponseBirths = reader.ReadInt();
            if (ResponseBirths < 0 || ResponseBirths > 9) throw new InvalidOperationException("Invalid local response budget.");
        }
    }

    public sealed class ArachnidInfestationState
    {
        // Geography bounds records; actor/scenery values are admission targets, never eviction limits.
        public const int MapColumns = 426, MapRows = 260, MaximumPatches = MapColumns * MapRows;
        public const int MaximumPopulation = 160, MaximumScenery = 240, PatchSpacing = 12;
        public Point3D Origin;
        public Point3D QueenCenter;
        public bool QueenCenterKnown;
        public int Epoch, Births, Cursor, RouteRequests, TerrainProbes;
        public DateTime Started, NextGrowth, LastTick;
        public List<ArachnidInfestationPatch> Patches = new List<ArachnidInfestationPatch>();
        private Dictionary<int, ArachnidInfestationPatch> m_Cells = new Dictionary<int, ArachnidInfestationPatch>();
        public List<int> Frontiers = new List<int>();
        private Dictionary<int,int> m_FrontierSlots = new Dictionary<int,int>();
        public int FrontierCursor;

        private static int CellKey(int x, int y) { return (x / PatchSpacing) * MapRows + y / PatchSpacing; }
        public void RebuildIndex()
        {
            m_Cells.Clear(); Frontiers.Clear(); m_FrontierSlots.Clear();
            foreach (ArachnidInfestationPatch patch in Patches)
            {
                int key = CellKey(patch.Center.X,patch.Center.Y);
                if (m_Cells.ContainsKey(key)) throw new InvalidOperationException("Duplicate territory cell.");
                m_Cells.Add(key,patch);
                if (patch.Established && !patch.Retired && patch.ExpansionMask != 15) AddFrontier(patch);
            }
            FrontierCursor = Frontiers.Count == 0 ? 0 : Math.Max(0,FrontierCursor) % Frontiers.Count;
        }

        public ArachnidInfestationPatch Cell(int x, int y)
        {
            ArachnidInfestationPatch patch;
            return m_Cells.TryGetValue(CellKey(x,y),out patch) ? patch : null;
        }

        public void Add(ArachnidInfestationPatch patch)
        {
            if (patch.Id != Patches.Count || Cell(patch.Center.X,patch.Center.Y) != null || Patches.Count >= MaximumPatches)
                throw new InvalidOperationException("Territory identity or geography bound violated.");
            Patches.Add(patch); m_Cells.Add(CellKey(patch.Center.X,patch.Center.Y),patch);
            if (patch.Established && !patch.Retired) AddFrontier(patch);
        }

        public void AddFrontier(ArachnidInfestationPatch patch)
        {
            if (m_FrontierSlots.ContainsKey(patch.Id)) return;
            m_FrontierSlots.Add(patch.Id,Frontiers.Count); Frontiers.Add(patch.Id);
        }

        public void RemoveFrontier(int id)
        {
            int index;
            if (!m_FrontierSlots.TryGetValue(id,out index)) return;
            int last = Frontiers[Frontiers.Count-1]; Frontiers[index] = last;
            m_FrontierSlots[last] = index; Frontiers.RemoveAt(Frontiers.Count-1); m_FrontierSlots.Remove(id);
            FrontierCursor = Frontiers.Count == 0 ? 0 : Math.Max(0,FrontierCursor)%Frontiers.Count;
        }

        public int Tier(ArachnidInfestationPatch patch)
        {
            return ArachnidTerritoryPolicy.TierAt(patch.Center,QueenCenterKnown ? QueenCenter : Origin);
        }

        public ArachnidInfestationPatch Find(int id)
        {
            return id >= 0 && id < Patches.Count ? Patches[id] : null;
        }

        public void Resume(DateTime now)
        {
            // No elapsed-time catch-up. Establishment must survive fresh online time.
            if (LastTick != DateTime.MinValue && now > LastTick) Started += now-LastTick;
            LastTick = now;
            NextGrowth = now.AddSeconds(45);
            foreach (ArachnidInfestationPatch patch in Patches)
            {
                patch.NextRefill = now.AddSeconds(30);
                if (!patch.Established) patch.EstablishAfter = DateTime.MinValue;
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(1); writer.Write(Origin); writer.Write(Epoch); writer.Write(Births); writer.Write(Cursor);
            writer.Write(Started); writer.Write(NextGrowth); writer.Write(LastTick); writer.Write(Patches.Count);
            foreach (ArachnidInfestationPatch patch in Patches) patch.Serialize(writer);
            writer.Write(FrontierCursor);
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            if (version < 0 || version > 1) throw new InvalidOperationException("Unknown infestation format.");
            Origin = reader.ReadPoint3D(); Epoch = reader.ReadInt(); Births = reader.ReadInt(); Cursor = reader.ReadInt();
            Started = reader.ReadDateTime(); NextGrowth = reader.ReadDateTime(); LastTick = reader.ReadDateTime(); int count = reader.ReadInt();
            if (count < 1 || count > (version == 0 ? 16 : MaximumPatches) || Epoch < 0 || Epoch > MaximumPatches || Births < 1)
                throw new InvalidOperationException("Invalid persisted infestation budget.");
            Cursor = Cursor < 0 ? 0 : Cursor % count;
            Dictionary<int, bool> ids = new Dictionary<int, bool>();
            Dictionary<Guid,bool> residents = new Dictionary<Guid,bool>();
            for (int i = 0; i < count; ++i)
            {
                ArachnidInfestationPatch patch = new ArachnidInfestationPatch(); patch.Deserialize(reader);
                if (patch.Id != i || ids.ContainsKey(patch.Id) || patch.Parent >= patch.Id || patch.Parent < -1
                    || (i == 0 ? patch.Parent != -1 : patch.Parent < 0)
                    || patch.Birth < 0 || patch.Birth > Epoch || patch.Births < 0
                    || patch.Center.X < 16 || patch.Center.X >= 5104 || patch.Center.Y < 16 || patch.Center.Y >= 3112)
                    throw new InvalidOperationException("Invalid infestation patch identity.");
                ids.Add(patch.Id, true); Patches.Add(patch);
                foreach (ArachnidInfestationResident resident in patch.Residents)
                {
                    if (residents.ContainsKey(resident.Identity)) throw new InvalidOperationException("Duplicate logical survivor identity.");
                    residents.Add(resident.Identity,true);
                }
            }
            if (version >= 1) FrontierCursor = reader.ReadInt();
            RebuildIndex();
        }
    }
}
