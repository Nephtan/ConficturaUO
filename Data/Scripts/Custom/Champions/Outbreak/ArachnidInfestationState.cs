using System;
using System.Collections.Generic;

namespace Server.Custom.Confictura.Champions
{
    public sealed class ArachnidInfestationPatch
    {
        public int Id, Parent, Birth, Failures, Births;
        public Point3D Center;
        public Mobile Guardian, Founder;
        public bool Established, Retired;
        public DateTime EstablishAfter, NextRefill;

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);
            writer.Write(Id); writer.Write(Parent); writer.Write(Birth); writer.Write(Failures); writer.Write(Births);
            writer.Write(Center); writer.Write(Guardian); writer.Write(Founder);
            writer.Write(Established); writer.Write(Retired); writer.Write(EstablishAfter); writer.Write(NextRefill);
        }

        public void Deserialize(GenericReader reader)
        {
            if (reader.ReadInt() != 0) throw new InvalidOperationException("Unknown infestation patch format.");
            Id = reader.ReadInt(); Parent = reader.ReadInt(); Birth = reader.ReadInt(); Failures = reader.ReadInt(); Births = reader.ReadInt();
            Center = reader.ReadPoint3D(); Guardian = reader.ReadMobile(); Founder = reader.ReadMobile();
            Established = reader.ReadBool(); Retired = reader.ReadBool(); EstablishAfter = reader.ReadDateTime(); NextRefill = reader.ReadDateTime();
        }
    }

    public sealed class ArachnidInfestationState
    {
        public const int MaximumPatches = 16, MaximumPopulation = 160, MaximumBirths = 400, MaximumScenery = 240;
        public const int PatchSpacing = 12, MaximumRadius = 160;
        public Point3D Origin;
        public int Epoch, Births, Cursor, RouteRequests, TerrainProbes;
        public DateTime Started, NextGrowth, LastTick;
        public List<ArachnidInfestationPatch> Patches = new List<ArachnidInfestationPatch>();

        public int Tier(ArachnidInfestationPatch patch)
        {
            return patch.Id == 0 ? 2 : Math.Min(2, Math.Max(0, (Epoch - patch.Birth) / 3));
        }

        public ArachnidInfestationPatch Find(int id)
        {
            foreach (ArachnidInfestationPatch patch in Patches) if (patch.Id == id) return patch;
            return null;
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
            writer.Write(0); writer.Write(Origin); writer.Write(Epoch); writer.Write(Births); writer.Write(Cursor);
            writer.Write(Started); writer.Write(NextGrowth); writer.Write(LastTick); writer.Write(Patches.Count);
            foreach (ArachnidInfestationPatch patch in Patches) patch.Serialize(writer);
        }

        public void Deserialize(GenericReader reader)
        {
            if (reader.ReadInt() != 0) throw new InvalidOperationException("Unknown infestation format.");
            Origin = reader.ReadPoint3D(); Epoch = reader.ReadInt(); Births = reader.ReadInt(); Cursor = reader.ReadInt();
            Started = reader.ReadDateTime(); NextGrowth = reader.ReadDateTime(); LastTick = reader.ReadDateTime(); int count = reader.ReadInt();
            if (count < 1 || count > MaximumPatches || Epoch < 0 || Epoch > MaximumPatches || Births < 1 || Births > MaximumBirths)
                throw new InvalidOperationException("Invalid persisted infestation budget.");
            Cursor = Cursor < 0 ? 0 : Cursor % count;
            Dictionary<int, bool> ids = new Dictionary<int, bool>();
            for (int i = 0; i < count; ++i)
            {
                ArachnidInfestationPatch patch = new ArachnidInfestationPatch(); patch.Deserialize(reader);
                if (patch.Id != i || ids.ContainsKey(patch.Id) || patch.Parent >= patch.Id || patch.Parent < -1
                    || (i == 0 ? patch.Parent != -1 : patch.Parent < 0)
                    || patch.Birth < 0 || patch.Birth > Epoch || patch.Births < 0 || patch.Births > MaximumBirths)
                    throw new InvalidOperationException("Invalid infestation patch identity.");
                ids.Add(patch.Id, true); Patches.Add(patch);
            }
        }
    }
}
