using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public partial class ArachnidOutbreakController : ISpreadingEncounter
    {
        private ArachnidInfestationState m_Infestation;
        private PathFollower m_FounderPath;
        private ArachnidInfestationPatch m_PendingPatch;
        private readonly List<int> m_LivePatches = new List<int>();
        private readonly Dictionary<int,int> m_LiveSlots = new Dictionary<int,int>();
        private int m_LiveCursor;
        public bool Infestation { get { return m_Infestation != null; } }
        public int InfestationPopulation { get { return LivingInfestationPopulation(); } }
        public bool Scheduled { get { return !Deleted && m_State.Phase != ArachnidOutbreakPhase.Disabled; } }
        public bool OriginPending { get { return !Deleted && m_State.Phase == ArachnidOutbreakPhase.Searching; } }
        private static readonly Dictionary<Type,bool> m_CustomEnemies = new Dictionary<Type,bool>();
        public bool RespondsTo(Mobile witness)
        {
            if (witness == null || witness.Deleted) return false;
            if (witness.Player || witness is TownGuards || witness is Citizens || witness is BaseVendor) return true;
            BaseCreature creature = witness as BaseCreature;
            if (creature == null) return false;
            if (creature.Controlled || creature.Summoned || creature.ControlMaster != null || creature.SummonMaster != null) return true;
            if (creature.Team != 0 || creature.OppositionGroup != null) return true;
            bool custom;
            if (!m_CustomEnemies.TryGetValue(creature.GetType(),out custom))
            {
                // Some IsEnemy overrides perform actions. Classify conservatively without calling them in a background query.
                custom = creature.GetType().GetMethod("IsEnemy",new Type[]{typeof(Mobile)}).DeclaringType != typeof(BaseCreature);
                m_CustomEnemies.Add(creature.GetType(),custom);
            }
            return custom;
        }
        public void WorkDeferred(DateTime now) { if (OriginPending) m_NextSearch = now.AddSeconds(2); }
        void ISpreadingEncounter.Step(DateTime now) { Tick(now); }
        void ISpreadingEncounter.Admit(int id, DateTime now) { AdmitInfestationPatch(id,now); }

        private int LivingInfestationPopulation()
        {
            int count = 0;
            foreach (Mobile mobile in m_Creatures) if (mobile != null && !mobile.Deleted && mobile.Alive && Owns(mobile,AttemptId)) ++count;
            return count;
        }

        private void MarkLive(int id)
        {
            if (m_LiveSlots.ContainsKey(id)) return;
            m_LiveSlots.Add(id,m_LivePatches.Count); m_LivePatches.Add(id);
        }
        private void MarkDormant(int id)
        {
            int index;
            if (!m_LiveSlots.TryGetValue(id,out index)) return;
            int last = m_LivePatches[m_LivePatches.Count-1]; m_LivePatches[index] = last; m_LiveSlots[last] = index;
            m_LivePatches.RemoveAt(m_LivePatches.Count-1); m_LiveSlots.Remove(id);
            m_LiveCursor = m_LivePatches.Count == 0 ? 0 : m_LiveCursor%m_LivePatches.Count;
        }

        private bool TryRandomInfestation(DateTime now)
        {
            Mobile walker = new Mobile();
            try
            {
                for (int probe = 0; probe < 20; ++probe)
                {
                    Point3D origin;
                    if (!ArachnidInfestationTerrain.TryPoint(Utility.RandomMinMax(32,5087),Utility.RandomMinMax(32,3087),out origin)
                        || !Map.Sosaria.CanSpawnMobile(origin) || !SpreadingEncounterScheduler.CanReserve(this,Map.Sosaria,origin)) continue;
                    if (ArachnidInfestationTerrain.LootPlacement(origin).WalkableCount < 80) continue;
                    int[] dx = new int[] {0,12,0,-12}, dy = new int[] {-12,0,12,0};
                    for (int direction = 0; direction < 4; ++direction)
                    {
                        Point3D destination;
                        if (!SpreadingEncounterScheduler.Route()) throw new SpreadingEncounterScheduler.Deferred();
                        if (ArachnidInfestationTerrain.TryPoint(origin.X+dx[direction],origin.Y+dy[direction],out destination)
                            && ArachnidInfestationTerrain.Connected(walker,origin,destination))
                        {
                            if (!BeginInfestation(origin,now)) throw new SpreadingEncounterScheduler.Deferred();
                            return true;
                        }
                    }
                }
            }
            finally { walker.Delete(); }
            m_Status = "No suitable random origin in this search batch."; return false;
        }

        private bool BeginInfestation(Point3D origin, DateTime now)
        {
            Point3D point;
            if (!ArachnidInfestationTerrain.TryPoint(origin.X,origin.Y,out point) || !Map.Sosaria.CanSpawnMobile(point)
                || !SpreadingEncounterScheduler.CanReserve(this,Map.Sosaria,point) || !SpreadingEncounterScheduler.AdmitStructuralActor()) return false;
            ArachnidOutbreakPlacement placement = ArachnidInfestationTerrain.LootPlacement(point);
            if (!m_State.TryStartInfestation("infestation:"+point.X+":"+point.Y)) return false;
            SpreadingEncounterScheduler.Initialize();
            m_Infestation = new ArachnidInfestationState();
            m_Infestation.Origin = point; m_Infestation.Started = now; m_Infestation.NextGrowth = now.AddSeconds(45); m_Infestation.LastTick = now;
            m_Placement = placement; m_Ledger = new ArachnidOutbreakLedger(AttemptId,2000,ArachnidOutbreakChampion.ChampionHealth);
            m_Status = "Queen vulnerable; root brood forming.";
            ArachnidInfestationPatch root = new ArachnidInfestationPatch();
            root.Id = 0; root.Parent = -1; root.Center = point; root.Established = true; root.NextRefill = now; root.LastInterest = now;
            m_Infestation.Add(root); SpreadingEncounterScheduler.Reserve(this,Map.Sosaria,root.Id,point);
            ArachnidOutbreakChampion queen = new ArachnidOutbreakChampion(this);
            queen.Name = "Mephitis, the brood queen"; queen.Home = point; queen.RangeHome = 16;
            m_Creatures.Add(queen); queen.MoveToWorld(point,Map.Sosaria); m_BossCreated = true; ++m_Infestation.Births;
            SpreadingEncounterScheduler.Track(queen); if (queen.AIObject != null) queen.AIObject.Activate();
            EstablishPopulation(root,now); m_Status = "Queen vulnerable; root brood established."; return true;
        }

        private bool InfestationContains(Point3D point)
        {
            if (m_Infestation == null) return false;
            if (Math.Abs(point.X-m_Infestation.Origin.X)<=16 && Math.Abs(point.Y-m_Infestation.Origin.Y)<=16) return true;
            for (int x = point.X-16; x <= point.X+16+12; x += 12)
                for (int y = point.Y-16; y <= point.Y+16+12; y += 12)
                {
                    ArachnidInfestationPatch patch = m_Infestation.Cell(x,y);
                    if (patch != null && !patch.Retired && Math.Abs(point.X-patch.Center.X)<=16 && Math.Abs(point.Y-patch.Center.Y)<=16) return true;
                }
            return false;
        }

        private bool FindInfestationPoint(Point3D center, int range, out Point3D point)
        {
            point = Point3D.Zero;
            for (int i = 0; i < 24; ++i)
            {
                ++m_Infestation.TerrainProbes;
                if (ArachnidInfestationTerrain.TryPoint(center.X+Utility.RandomMinMax(-range,range),center.Y+Utility.RandomMinMax(-range,range),out point)
                    && Map.Sosaria.CanSpawnMobile(point)) return true;
            }
            return false;
        }

        private ArachnidInfestationCreature SpawnInfestationCreature(ArachnidInfestationPatch patch, bool guardian, bool founder, Point3D point)
        {
            if (!((guardian || founder) ? SpreadingEncounterScheduler.AdmitStructuralActor() : SpreadingEncounterScheduler.AdmitActor(SpreadingEncounterScheduler.PriorityAdmission)))
            { return null; }
            ArachnidInfestationCreature creature = new ArachnidInfestationCreature(this,patch.Id,founder ? 0 : m_Infestation.Tier(patch),guardian,founder);
            creature.Home = patch.Center; creature.RangeHome = founder ? 0 : 7; creature.LastRelevant = DateTime.UtcNow;
            m_Creatures.Add(creature); creature.MoveToWorld(point,Map.Sosaria); SpreadingEncounterScheduler.Track(creature);
            if (creature.AIObject != null) creature.AIObject.Activate();
            if (m_Infestation.Births < Int32.MaxValue) ++m_Infestation.Births;
            if (patch.Births < Int32.MaxValue) ++patch.Births;
            MarkLive(patch.Id); return creature;
        }

        private static ArachnidInfestationResident Capture(ArachnidInfestationCreature creature)
        {
            ArachnidInfestationResident resident = new ArachnidInfestationResident();
            resident.Identity = creature.Identity; resident.Slot = creature.Slot; resident.Kind = (byte)creature.Kind; resident.Tier = (byte)creature.Tier;
            resident.Hits = (ushort)Math.Max(0,Math.Min(UInt16.MaxValue,creature.Hits)); resident.MaximumHits = (ushort)Math.Min(UInt16.MaxValue,creature.HitsMax);
            resident.Stam = (ushort)Math.Max(0,Math.Min(UInt16.MaxValue,creature.Stam)); resident.Mana = (ushort)Math.Max(0,Math.Min(UInt16.MaxValue,creature.Mana));
            resident.Position = creature.Location; resident.Actor = creature; resident.Inventory = creature.Backpack; resident.NextSpecial = creature.NextSpecial;
            return resident;
        }

        private void CaptureInfestation()
        {
            if (m_Infestation == null) return;
            foreach (int id in m_LivePatches)
            {
                ArachnidInfestationPatch patch = m_Infestation.Find(id);
                for (int i = 0; i < patch.Residents.Count; ++i)
                {
                    ArachnidInfestationCreature actor = patch.Residents[i].Actor as ArachnidInfestationCreature;
                    if (actor != null && !actor.Deleted && actor.Alive) patch.Residents[i] = Capture(actor);
                    else if (actor != null)
                    { ArachnidInfestationResident resident = patch.Residents[i]; resident.Hits = 0; resident.Actor = null; resident.Inventory = null; patch.Residents[i] = resident; }
                }
            }
        }

        private void RecoverInfestation()
        {
            m_Infestation.RebuildIndex();
            foreach (Mobile mobile in m_Creatures)
            {
                ArachnidOutbreakChampion queen = mobile as ArachnidOutbreakChampion;
                if (queen != null && !queen.Deleted && queen.Alive) { UpdateQueenCenter(queen); break; }
            }
            foreach (Mobile mobile in m_Creatures)
            {
                ArachnidInfestationCreature actor = mobile as ArachnidInfestationCreature;
                // Refresh saved frontier policy for residents and founders without replenishing resources.
                if (actor != null && !actor.Deleted && actor.Alive && actor.Tier == 0 && Owns(actor,AttemptId)) actor.ApplyTier(0);
            }
            foreach (ArachnidInfestationPatch patch in m_Infestation.Patches)
            {
                if (!SpreadingEncounterScheduler.Reserve(this,Map.Sosaria,patch.Id,patch.Center)) throw new InvalidOperationException("Persisted infestation territories overlap.");
                patch.LastInterest = DateTime.UtcNow;
                if (!patch.Retired && !patch.Established) m_PendingPatch = patch;
                if (!patch.Initialized)
                {
                    byte slot = 1;
                    foreach (Mobile mobile in m_Creatures)
                    {
                        ArachnidInfestationCreature actor = mobile as ArachnidInfestationCreature;
                        if (actor == null || actor.Deleted || !actor.Alive || actor.PatchId != patch.Id || actor.Founder || !Owns(actor,AttemptId)) continue;
                        actor.Identify(actor.Identity,actor.Guardian ? (byte)0 : slot++,actor.Kind); patch.Residents.Add(Capture(actor));
                    }
                    patch.Initialized = patch.Residents.Count > 0;
                }
                foreach (ArachnidInfestationResident resident in patch.Residents)
                {
                    ArachnidInfestationCreature actor = resident.Actor as ArachnidInfestationCreature;
                    if (actor == null || actor.Deleted || !actor.Alive) continue;
                    if (!Owns(actor,AttemptId) || actor.Identity != resident.Identity) throw new InvalidOperationException("Resident identity mismatch.");
                    SpreadingEncounterScheduler.Track(actor); if (actor.AIObject != null) actor.AIObject.Activate(); MarkLive(patch.Id);
                }
            }
        }

        private void AdmitInfestationPatch(int id, DateTime now)
        {
            if (Deleted || m_Infestation == null || !m_State.Active) return;
            ArachnidInfestationPatch patch = m_Infestation.Find(id);
            if (patch == null || patch.Retired || !patch.Established) return;
            Point3D ground;
            if (!ArachnidInfestationTerrain.TryPoint(patch.Center.X,patch.Center.Y,out ground,false)) { RetireInfestationBranch(id); return; }
            patch.LastInterest = now;
            HydrateInfestationPatch(patch,now);
            if (SpreadingEncounterScheduler.PriorityAdmission) EstablishPopulation(patch,now);
            DecorateInfestationPatch(patch);
        }

        private void HydrateInfestationPatch(ArachnidInfestationPatch patch, DateTime now)
        { HydrateInfestationPatch(patch,now,false); }

        private void HydrateInfestationPatch(ArachnidInfestationPatch patch, DateTime now, bool structural)
        {
            // Apply density migration before dormant records become visible actors and acquire protection.
            if (!patch.Retired) PruneQuietSurplus(patch,now);
            for (int i = 0; i < patch.Residents.Count; ++i)
            {
                ArachnidInfestationResident resident = patch.Residents[i];
                if (structural && resident.Slot != 0) continue;
                ArachnidInfestationCreature actor = resident.Actor as ArachnidInfestationCreature;
                if (actor != null && (actor.Deleted || !actor.Alive))
                { resident.Hits = 0; resident.Actor = null; resident.Inventory = null; patch.Residents[i] = resident; continue; }
                if (actor != null && !actor.Deleted && actor.Alive)
                {
                    int desired = m_Infestation.Tier(patch);
                    if ((actor.Tier != desired || actor.EggSac != IsOutskirtsEgg(actor)) && (actor.Tier > desired || !SpreadingEncounterScheduler.Busy(actor)))
                    { actor.ApplyTier(desired); patch.Residents[i] = Capture(actor); }
                    if (SpreadingEncounterScheduler.PriorityAdmission) actor.LastRelevant = now; continue;
                }
                if (resident.Hits == 0 || !(structural ? SpreadingEncounterScheduler.AdmitStructuralActor() : SpreadingEncounterScheduler.AdmitActor(true))) continue;
                Point3D point;
                if (!ArachnidInfestationTerrain.TryPoint(resident.Position.X,resident.Position.Y,out point,false))
                    if (!FindInfestationPoint(patch.Center,7,out point)) continue;
                if (resident.Inventory != null && !resident.Inventory.Deleted && resident.Inventory.Parent != null)
                    throw new InvalidOperationException("Dormant inventory has another owner.");
                actor = new ArachnidInfestationCreature(this,patch.Id,resident.Tier,resident.Slot == 0,false);
                actor.Identify(resident.Identity,resident.Slot,resident.Kind); actor.HitsMaxSeed = resident.MaximumHits;
                actor.Hits = resident.Hits; actor.Stam = resident.Stam; actor.Mana = resident.Mana; actor.NextSpecial = resident.NextSpecial;
                if (actor.Tier != m_Infestation.Tier(patch)) actor.ApplyTier(m_Infestation.Tier(patch));
                actor.Home = patch.Center; actor.RangeHome = 7; actor.LastRelevant = now;
                if (resident.Inventory != null && !resident.Inventory.Deleted)
                { if (actor.Backpack != null) actor.Backpack.Delete(); actor.AddItem(resident.Inventory); }
                resident.Actor = actor; patch.Residents[i] = resident;
                if (resident.Slot == 0) patch.Guardian = actor;
                m_Creatures.Add(actor); actor.MoveToWorld(point,Map.Sosaria); SpreadingEncounterScheduler.Track(actor);
                if (actor.AIObject != null) actor.AIObject.Activate(); MarkLive(patch.Id);
            }
        }

        private bool CompactInfestationPatch(ArachnidInfestationPatch patch, DateTime now)
        {
            if (QueenProtectsPatch(patch)) return false;
            if (now-patch.LastInterest < TimeSpan.FromSeconds(SpreadingEncounterScheduler.QuietSeconds)
                || m_PendingPatch != null && m_PendingPatch.Parent == patch.Id) return false;
            if (patch.Id == 0)
                foreach (Mobile mobile in m_Creatures)
                {
                    ArachnidOutbreakChampion queen = mobile as ArachnidOutbreakChampion;
                    if (queen != null && !queen.Deleted && (SpreadingEncounterScheduler.Interested(queen) || SpreadingEncounterScheduler.Busy(queen))) return false;
                }
            foreach (ArachnidInfestationResident resident in patch.Residents)
            {
                ArachnidInfestationCreature actor = resident.Actor as ArachnidInfestationCreature;
                if (actor == null || actor.Deleted || !actor.Alive) continue;
                if (actor.Map != Map.Sosaria || !actor.InRange(patch.Center,8) || actor.Hidden
                    || actor.HitsMax > UInt16.MaxValue || actor.Stam > UInt16.MaxValue || actor.Mana > UInt16.MaxValue
                    || actor.Items.Count > (actor.Backpack == null ? 0 : 1) || now-actor.LastRelevant < TimeSpan.FromSeconds(SpreadingEncounterScheduler.QuietSeconds)
                    || SpreadingEncounterScheduler.Interested(actor) || SpreadingEncounterScheduler.Busy(actor)) return false;
            }
            for (int i = 0; i < patch.Residents.Count; ++i)
            {
                ArachnidInfestationCreature actor = patch.Residents[i].Actor as ArachnidInfestationCreature;
                if (actor == null || actor.Deleted || !actor.Alive) continue;
                ArachnidInfestationResident resident = Capture(actor);
                if (resident.Inventory != null) { actor.RemoveItem(resident.Inventory); resident.Inventory.Internalize(); }
                resident.Actor = null; patch.Residents[i] = resident; m_Creatures.Remove(actor); m_BroodAttackers.Remove(actor); actor.Delete();
            }
            patch.Guardian = null;
            if (patch.Retired)
            {
                foreach (ArachnidInfestationResident resident in patch.Residents)
                    if (resident.Inventory != null && !resident.Inventory.Deleted && resident.Inventory.Parent == null) resident.Inventory.Delete();
                patch.Residents.Clear();
            }
            foreach (Item item in m_Signs.ToArray())
            { ArachnidInfestationWeb web = item as ArachnidInfestationWeb; if (web != null && web.PatchId == patch.Id && Owns(web,AttemptId)) web.Delete(); }
            MarkDormant(patch.Id); return true;
        }

        private void EstablishPopulation(ArachnidInfestationPatch patch, DateTime now)
        {
            if (patch.Retired) return;
            if (patch.Initialized && NeedsGuardian(patch))
            {
                bool survivor = false;
                foreach (ArachnidInfestationResident resident in patch.Residents)
                    if (resident.Slot == 0 && resident.Hits > 0 && (resident.Actor == null || !resident.Actor.Deleted && resident.Actor.Alive)) survivor = true;
                if (!survivor && patch.Guardian != null) { RetireInfestationBranch(patch.Id); return; }
            }
            Point3D point;
            if (NeedsGuardian(patch) && patch.Guardian == null && FindInfestationPoint(patch.Center,2,out point))
            {
                if (m_Infestation.Tier(patch) == 0 && !OpenEggGround(point)) return;
                ArachnidInfestationCreature guardian = SpawnInfestationCreature(patch,true,false,point);
                if (guardian == null) return;
                guardian.Identify(guardian.Identity,0,guardian.Kind); patch.Guardian = guardian;
                patch.Residents.Add(Capture(guardian)); patch.Initialized = true;
            }
            if (!NeedsGuardian(patch)) patch.Initialized = true;
            if (!patch.Initialized) return;
            RefillInfestationPatch(patch,now); DecorateInfestationPatch(patch);
        }

        private void RefillInfestationPatch(ArachnidInfestationPatch patch, DateTime now)
        {
            if (patch.Retired || !patch.Initialized || now < patch.NextRefill) return;
            for (int i = patch.Residents.Count-1; i >= 0; --i)
            {
                ArachnidInfestationResident resident = patch.Residents[i];
                if (resident.Slot != 0 && (resident.Hits == 0 || resident.Actor != null && (resident.Actor.Deleted || !resident.Actor.Alive))) patch.Residents.RemoveAt(i);
            }
            int tier = m_Infestation.Tier(patch), slots = 0;
            for (int i = 0; i < patch.Residents.Count; ++i)
            {
                ArachnidInfestationResident resident = patch.Residents[i]; slots |= 1 << resident.Slot;
                ArachnidInfestationCreature actor = resident.Actor as ArachnidInfestationCreature;
                if (actor != null && !actor.Deleted && actor.Alive && (actor.Tier > tier || !SpreadingEncounterScheduler.Busy(actor))
                    && (actor.Tier != tier || actor.EggSac != IsOutskirtsEgg(actor))) { actor.ApplyTier(tier); patch.Residents[i] = Capture(actor); }
            }
            PruneQuietSurplus(patch,now);
            int target = ArachnidTerritoryPolicy.PopulationTarget(patch,tier);
            if (patch.Residents.Count >= target) { patch.NextRefill = now.AddSeconds(30); return; }
            byte slot = 1; while ((slots & (1 << slot)) != 0 && slot < 13) ++slot;
            if (slot >= 13) return;
            Point3D point; if (!FindInfestationPoint(patch.Center,7,out point)) return;
            ArachnidInfestationCreature child = SpawnInfestationCreature(patch,false,false,point); if (child == null) return;
            child.Identify(child.Identity,slot,child.Kind); patch.Residents.Add(Capture(child)); patch.NextRefill = now.AddSeconds(5);
        }

        private void DecorateInfestationPatch(ArachnidInfestationPatch patch)
        {
            List<Item> existing = new List<Item>();
            foreach (Item item in m_Signs) { ArachnidInfestationWeb web = item as ArachnidInfestationWeb; if (web != null && !web.Deleted && web.PatchId == patch.Id) existing.Add(web); }
            int tier = Math.Min(3,m_Infestation.Tier(patch)), family = patch.Id%6;
            if (tier == 0)
            {
                int sparseTarget = ArachnidTerritoryPolicy.HasEggSac(patch.Center) || ArachnidTerritoryPolicy.HasFrontierSpider(patch.Center) ? 1 : 0;
                if (existing.Count == sparseTarget) return;
                foreach (Item item in existing) item.Delete();
                if (sparseTarget == 0 || !SpreadingEncounterScheduler.AdmitScenery(1)) return;
                Point3D ground;
                if (!ArachnidInfestationTerrain.TryPoint(patch.Center.X+3,patch.Center.Y+3,out ground,false)) return;
                ArachnidInfestationWeb fringe = new ArachnidInfestationWeb(this,patch.Id,0x10D7);
                fringe.MoveToWorld(ground,Map.Sosaria); m_Signs.Add(fringe); SpreadingEncounterScheduler.Track(fringe); return;
            }
            int[] starts = new int[] {4280,4285,4290,4294,4298,4302}, sizes = new int[] {5,5,4,4,4,4};
            bool[] forwards = new bool[] {false,true,true,false,true,false};
            int target = 2; for (int shape = 0; shape < tier; ++shape) target += sizes[(family+shape)%6];
            if (existing.Count == target || !SpreadingEncounterScheduler.AdmitScenery(target)) return;
            List<Point3D> points = new List<Point3D>(); List<int> graphics = new List<int>();
            for (int small = 0; small < 2; ++small)
            {
                Point3D point;
                if (!ArachnidInfestationTerrain.TryPoint(patch.Center.X-4+small*7,patch.Center.Y-4+small*7,out point,false)) return;
                points.Add(point); graphics.Add(small == 0 ? 0x10D7 : 0x10DD);
            }
            for (int shape = 0; shape < tier; ++shape)
            {
                int index = (family+shape)%6; bool found = false;
                for (int attempt = 0; attempt < 8 && !found; ++attempt)
                {
                    List<Point3D> parts = new List<Point3D>();
                    for (int part = 0; part < sizes[index]; ++part)
                    {
                        int offset = forwards[index] ? part : sizes[index]-1-part; Point3D point;
                        if (!ArachnidInfestationTerrain.TryPoint(patch.Center.X-4+attempt%4+offset,patch.Center.Y-3+shape*3+attempt/4-offset,out point,false)) break;
                        parts.Add(point);
                    }
                    if (parts.Count != sizes[index]) continue;
                    for (int part = 0; part < parts.Count; ++part) { points.Add(parts[part]); graphics.Add(starts[index]+part); }
                    found = true;
                }
                if (!found) return;
            }
            foreach (Item item in existing) item.Delete();
            for (int i = 0; i < points.Count; ++i)
            { ArachnidInfestationWeb web = new ArachnidInfestationWeb(this,patch.Id,graphics[i]); web.MoveToWorld(points[i],Map.Sosaria); m_Signs.Add(web); SpreadingEncounterScheduler.Track(web); }
        }

        private void RetireInfestationBranch(int id)
        {
            ArachnidInfestationPatch patch = m_Infestation.Find(id); if (patch == null || patch.Retired) return;
            patch.Retired = true; m_Infestation.RemoveFrontier(id);
            RemovePatchWebs(patch);
            CancelNestResponse(patch.Id);
            // Established daughters are independent footholds. Existing combatants remain real.
            if (m_PendingPatch != null && m_PendingPatch.Parent == id) RetireInfestationBranch(m_PendingPatch.Id);
            if (m_PendingPatch == patch) { m_PendingPatch = null; m_FounderPath = null; }
            for (int i = patch.Residents.Count-1; i >= 0; --i)
            {
                ArachnidInfestationResident resident = patch.Residents[i];
                if (resident.Actor != null && !resident.Actor.Deleted && resident.Actor.Alive) continue;
                if (resident.Inventory != null && !resident.Inventory.Deleted && resident.Inventory.Parent == null) resident.Inventory.Delete();
                patch.Residents.RemoveAt(i);
            }
        }

        private int InfestationMilestone() { for (int i = 0; i < 3; ++i) if (!m_Ledger.IsSealed(i)) return i; return -1; }
        private bool ValidInfestationReference(Mobile mobile, ArachnidInfestationPatch patch, bool founder)
        {
            ArachnidInfestationCreature creature = mobile as ArachnidInfestationCreature;
            return creature != null && !creature.Deleted && creature.Alive && Owns(creature,AttemptId) && m_Creatures.Contains(creature)
                && creature.PatchId == patch.Id && (founder ? creature.Founder : creature.Guardian);
        }

        private void InfestationDeath(Mobile mobile)
        {
            ArachnidInfestationCreature creature = mobile as ArachnidInfestationCreature;
            if (creature != null)
            {
                ArachnidInfestationPatch patch = m_Infestation.Find(creature.PatchId); if (patch == null) return;
                RecordInfestationKill(creature,patch,DateTime.UtcNow);
                for (int i = patch.Residents.Count-1; i >= 0; --i) if (patch.Residents[i].Identity == creature.Identity) patch.Residents.RemoveAt(i);
                if (patch.Retired) return;
                if (patch.Founder == creature && !patch.Established) { RetireInfestationBranch(patch.Id); return; }
                if (patch.Guardian == creature && (creature.EggSac || m_Infestation.Tier(patch) > 0))
                {
                    int milestone = InfestationMilestone();
                    if (milestone >= 0 && m_Ledger.Qualified(milestone).Count > 0)
                    { QueueStage(milestone); m_State.CompleteMilestone(milestone,DateTime.UtcNow); m_Wounds.Clear(); m_Poisons.Clear(); m_Bards.Clear(); }
                    RetireInfestationBranch(patch.Id);
                }
                else if (patch.Guardian == creature) patch.Guardian = null;
                return;
            }
            ArachnidOutbreakChampion queen = mobile as ArachnidOutbreakChampion; if (queen == null) return;
            bool qualified = m_Ledger.Qualified(3).Count > 0; m_Placement = ArachnidInfestationTerrain.LootPlacement(queen.Location);
            if (qualified && !TryBossRewards(queen)) { Abort(DateTime.UtcNow,"Victory has no safe distinct loot ground."); return; }
            if (qualified) QueueStage(3); else m_Ledger.Seal(3);
            if (m_State.CompleteBoss(DateTime.UtcNow,Utility.RandomMinMax(30,60)))
            { Cleanup(AttemptId); m_Placement = null; m_Status = "Queen defeated; infestation ended."; }
        }

        internal void TrackInfestationQueen(ArachnidOutbreakChampion queen)
        {
            if (m_Infestation == null) return;
            SpreadingEncounterScheduler.Track(queen);

        }

        private void TickInfestation(DateTime now)
        {
            ArachnidInfestationState colony = m_Infestation;
            ArachnidInfestationTerrain.BeginWork(4096);
            try
            {
                colony.LastTick = now; colony.RouteRequests = colony.TerrainProbes = 0;
                m_Creatures.RemoveAll(delegate(Mobile mobile) { return mobile == null || mobile.Deleted; });
                m_Signs.RemoveAll(delegate(Item item) { return item == null || item.Deleted; });
                bool queen = false;
                ArachnidOutbreakChampion livingQueen = null;
                foreach (Mobile mobile in m_Creatures)
                    if (mobile is ArachnidOutbreakChampion && mobile.Alive) { queen = true; livingQueen = (ArachnidOutbreakChampion)mobile; break; }
                if (livingQueen != null) { UpdateQueenCenter(livingQueen); ServiceQueenShelter(livingQueen,now); }
                if (!queen) { Abort(now,"Queen removed without a death outcome."); return; }
                ServiceNestResponses(now);
                for (int work = 0; work < 2 && m_LivePatches.Count > 0; ++work)
                {
                    m_LiveCursor %= m_LivePatches.Count; ArachnidInfestationPatch patch = colony.Find(m_LivePatches[m_LiveCursor]);
                    m_LiveCursor = (m_LiveCursor+1)%m_LivePatches.Count;
                    if (!patch.Established)
                    {
                        ArachnidInfestationCreature founder = patch.Founder as ArachnidInfestationCreature;
                        if (patch.Retired && (founder == null || founder.Deleted)) MarkDormant(patch.Id);
                        else if (patch.Retired && now-founder.LastRelevant > TimeSpan.FromSeconds(SpreadingEncounterScheduler.QuietSeconds)
                            && !SpreadingEncounterScheduler.Interested(founder) && !SpreadingEncounterScheduler.Busy(founder))
                        { founder.Delete(); patch.Founder = null; MarkDormant(patch.Id); }
                        continue;
                    }
                    if (CompactInfestationPatch(patch,now)) continue;
                    if (patch.Retired) continue;
                    Point3D point;
                    if (!ArachnidInfestationTerrain.TryPoint(patch.Center.X,patch.Center.Y,out point,false)) { RetireInfestationBranch(patch.Id); continue; }
                    EstablishPopulation(patch,now);
                }
                if (m_PendingPatch != null)
                {
                    ArachnidInfestationPatch patch = m_PendingPatch, parent = colony.Find(patch.Parent);
                    if (patch.Retired || parent == null || parent.Retired || !ValidInfestationReference(patch.Founder,patch,true)) { RetireInfestationBranch(patch.Id); return; }
                    if (patch.Founder.InRange(patch.Center,1))
                    {
                        if (patch.EstablishAfter == DateTime.MinValue) patch.EstablishAfter = now.AddSeconds(45);
                        if (now >= patch.EstablishAfter)
                        {
                            patch.Founder.Delete(); patch.Founder = null; patch.Established = true; patch.Birth = ++colony.Epoch;
                            colony.AddFrontier(patch); m_PendingPatch = null; m_FounderPath = null; patch.LastInterest = now; EstablishPopulation(patch,now);
                        }
                    }
                    else if (SpreadingEncounterScheduler.Route())
                    {
                        TurnParticipant actor = ArachnidOutbreakWebItem.Actor(patch.Founder);
                        if (actor != null && (actor.Group.Current != actor || actor.Pending != null)) return;
                        if (m_FounderPath == null) m_FounderPath = new PathFollower(patch.Founder,patch.Center);
                        ++colony.RouteRequests; m_FounderPath.Follow(false,1);
                        // A blocked route can fail locally; it never exhausts the parent colony.
                        if (++patch.Failures > 48) { RetireInfestationBranch(patch.Id); }
                    }
                    return;
                }
                if (now < colony.NextGrowth) { return; }
                if (colony.Frontiers.Count == 0) { return; }
                colony.NextGrowth = now.AddSeconds(2);
                colony.FrontierCursor %= colony.Frontiers.Count;
                ArachnidInfestationPatch source = colony.Find(colony.Frontiers[colony.FrontierCursor]); colony.FrontierCursor = (colony.FrontierCursor+1)%colony.Frontiers.Count;
                HydrateInfestationPatch(source,now,true); EstablishPopulation(source,now);
                if (source.Retired || !source.Established) return;
                int direction = Utility.Random(4); int[] dx = new int[] {0,12,0,-12}, dy = new int[] {-12,0,12,0};
                Point3D destination; int x = source.Center.X+dx[direction], y = source.Center.Y+dy[direction];
                if (colony.Cell(x,y) != null)
                { source.ExpansionMask |= (byte)(1 << direction); if (source.ExpansionMask == 15) colony.RemoveFrontier(source.Id); return; }
                if (!ArachnidInfestationTerrain.TryPoint(x,y,out destination) || !SpreadingEncounterScheduler.CanReserve(this,Map.Sosaria,destination) || !SpreadingEncounterScheduler.Route()) return;
                ++colony.RouteRequests;
                if (!ArachnidInfestationTerrain.Connected(livingQueen,source.Center,destination)) return;
                Point3D start; if (!FindInfestationPoint(source.Center,2,out start)) return;
                ArachnidInfestationPatch child = new ArachnidInfestationPatch(); child.Id = colony.Patches.Count; child.Parent = source.Id; child.Center = destination;
                child.Birth = colony.Epoch; child.LastInterest = now; child.Founder = SpawnInfestationCreature(child,false,true,start); if (child.Founder == null) return;
                colony.Add(child); SpreadingEncounterScheduler.Reserve(this,Map.Sosaria,child.Id,destination);
                source.ExpansionMask |= (byte)(1 << direction); if (source.ExpansionMask == 15) colony.RemoveFrontier(source.Id);
                m_PendingPatch = child; m_FounderPath = null; colony.NextGrowth = now.AddSeconds(45);
            }
            finally { colony.TerrainProbes = ArachnidInfestationTerrain.EndWork(); }
        }
    }
}
