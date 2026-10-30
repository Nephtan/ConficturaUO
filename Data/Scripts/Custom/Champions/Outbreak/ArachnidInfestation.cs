using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public partial class ArachnidOutbreakController
    {
        private ArachnidInfestationState m_Infestation;
        private PathFollower m_FounderPath;
        public bool Infestation { get { return m_Infestation != null; } }
        public int InfestationPopulation { get { return LivingInfestationPopulation(); } }

        private int LivingInfestationPopulation()
        {
            int count = 0;
            foreach (Mobile mobile in m_Creatures) if (mobile != null && !mobile.Deleted && mobile.Alive && Owns(mobile,AttemptId)) ++count;
            return count;
        }

        private bool TryRandomInfestation(DateTime now)
        {
            // Bounded broad sampling, not a fixed site list or an unbounded whole-map scan.
            Mobile walker = new Mobile();
            try
            {
                for (int probe = 0; probe < 20; ++probe)
                {
                    Point3D origin;
                    if (!ArachnidInfestationTerrain.TryPoint(Utility.RandomMinMax(32,5087),Utility.RandomMinMax(32,3087),out origin)
                        || !Map.Sosaria.CanSpawnMobile(origin)) continue;
                    ArachnidOutbreakPlacement placement = ArachnidInfestationTerrain.LootPlacement(origin);
                    if (placement.WalkableCount < 80) continue;
                    bool exit = false;
                    int[] dx = new int[] { 0,12,0,-12 }, dy = new int[] { -12,0,12,0 };
                    for (int direction = 0; direction < 4 && !exit; ++direction)
                    {
                        Point3D destination;
                        exit = ArachnidInfestationTerrain.TryPoint(origin.X+dx[direction],origin.Y+dy[direction],out destination)
                            && ArachnidInfestationTerrain.Connected(walker,origin,destination);
                    }
                    if (!exit) continue;
                    return BeginInfestation(origin,now);
                }
            }
            finally { walker.Delete(); }
            m_Status = "No suitable random origin in this search batch.";
            return false;
        }

        private bool BeginInfestation(Point3D origin, DateTime now)
        {
            Point3D checkedOrigin;
            if (!ArachnidInfestationTerrain.TryPoint(origin.X,origin.Y,out checkedOrigin) || !Map.Sosaria.CanSpawnMobile(checkedOrigin)
                || !m_State.TryStartInfestation("infestation:"+origin.X+":"+origin.Y)) return false;
            m_Infestation = new ArachnidInfestationState();
            m_Infestation.Origin = checkedOrigin; m_Infestation.Started = now; m_Infestation.NextGrowth = now.AddSeconds(45);
            m_Infestation.LastTick = now;
            m_Placement = ArachnidInfestationTerrain.LootPlacement(checkedOrigin);
            m_Ledger = new ArachnidOutbreakLedger(AttemptId,2000,ArachnidOutbreakChampion.ChampionHealth);
            ArachnidOutbreakChampion queen = new ArachnidOutbreakChampion(this);
            queen.Name = "Mephitis, the brood queen"; queen.Home = checkedOrigin; queen.RangeHome = 16;
            m_Creatures.Add(queen); queen.MoveToWorld(checkedOrigin,Map.Sosaria); m_BossCreated = true; ++m_Infestation.Births;
            ArachnidInfestationPatch root = new ArachnidInfestationPatch();
            root.Id = 0; root.Parent = -1; root.Center = checkedOrigin; root.Established = true;
            root.NextRefill = now; m_Infestation.Patches.Add(root);
            EstablishPopulation(root,now);
            m_Status = "Queen vulnerable; root brood established.";
            return true;
        }

        private bool InfestationContains(Point3D point)
        {
            if (m_Infestation == null) return false;
            if (Math.Abs(point.X-m_Infestation.Origin.X)<=16 && Math.Abs(point.Y-m_Infestation.Origin.Y)<=16) return true;
            foreach (ArachnidInfestationPatch patch in m_Infestation.Patches)
                if (!patch.Retired && Math.Abs(point.X-patch.Center.X) <= 16 && Math.Abs(point.Y-patch.Center.Y) <= 16) return true;
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
            if (m_Infestation.Births >= ArachnidInfestationState.MaximumBirths
                || LivingInfestationPopulation() >= ArachnidInfestationState.MaximumPopulation) return null;
            ArachnidInfestationCreature creature = new ArachnidInfestationCreature(this,patch.Id,founder ? 0 : m_Infestation.Tier(patch),guardian,founder);
            creature.Home = patch.Center; creature.RangeHome = founder ? 0 : 7;
            m_Creatures.Add(creature); creature.MoveToWorld(point,Map.Sosaria);
            ++m_Infestation.Births; ++patch.Births;
            return creature;
        }

        private void EstablishPopulation(ArachnidInfestationPatch patch, DateTime now)
        {
            Point3D point;
            if (patch.Guardian == null && FindInfestationPoint(patch.Center,2,out point))
                patch.Guardian = SpawnInfestationCreature(patch,true,false,point);
            if (patch.Guardian == null) { RetireInfestationBranch(patch.Id); return; }
            RefillInfestationPatch(patch,now);
            DecorateInfestationPatch(patch);
        }

        private void RefillInfestationPatch(ArachnidInfestationPatch patch, DateTime now)
        {
            if (now < patch.NextRefill || patch.Births >= 32) return;
            patch.NextRefill = now.AddSeconds(30);
            int tier = m_Infestation.Tier(patch), population = 0;
            foreach (Mobile mobile in m_Creatures)
            {
                ArachnidInfestationCreature creature = mobile as ArachnidInfestationCreature;
                if (creature != null && !creature.Deleted && creature.Alive && creature.PatchId == patch.Id && !creature.Founder)
                {
                    ++population;
                    if (creature.Tier != tier) creature.ApplyTier(tier);
                }
            }
            int target = new int[] { 5,8,12 }[tier];
            for (int i = 0; i < 3 && population < target && patch.Births < 32; ++i)
            {
                Point3D point; if (!FindInfestationPoint(patch.Center,7,out point)) break;
                if (SpawnInfestationCreature(patch,false,false,point) == null) break;
                ++population;
            }
        }

        private void DecorateInfestationPatch(ArachnidInfestationPatch patch)
        {
            int count = 0, total = 0;
            List<Item> existing = new List<Item>();
            foreach (Item item in m_Signs)
                if (item != null && !item.Deleted && item is ArachnidInfestationWeb)
                {
                    ++total;
                    if (((ArachnidInfestationWeb)item).PatchId == patch.Id) { ++count; existing.Add(item); }
                }
            int tier = m_Infestation.Tier(patch), family = patch.Id % 6;
            int[] starts = new int[] {4280,4285,4290,4294,4298,4302}, sizes = new int[] {5,5,4,4,4,4};
            bool[] forwards = new bool[] {false,true,true,false,true,false};
            int target = 2 + (tier >= 1 ? sizes[family] : 0) + (tier == 2 ? sizes[(family+1)%6] : 0);
            bool smallOnly = true;
            foreach (Item item in existing) if (item.ItemID < 0x10D7 || item.ItemID > 0x10DD) smallOnly = false;
            if (count == target && (tier == 0 ? smallOnly : !smallOnly)) return;
            if (total-count+target > ArachnidInfestationState.MaximumScenery) return;
            List<Point3D> points = new List<Point3D>(); List<int> graphics = new List<int>();
            for (int i = 0; i < 2; ++i)
            {
                Point3D point = Point3D.Zero; bool found = false;
                for (int attempt = 0; attempt < 24 && !found; ++attempt)
                {
                    found = ArachnidInfestationTerrain.TryPoint(patch.Center.X+Utility.RandomMinMax(-4,4),patch.Center.Y+Utility.RandomMinMax(-4,4),out point)
                        && !points.Contains(point);
                }
                if (!found) return;
                points.Add(point); graphics.Add(i == 0 ? 0x10D7 : 0x10DD);
            }
            for (int shape = 0; shape < tier; ++shape)
            {
                int index = (family+shape)%6; bool found = false;
                for (int attempt = 0; attempt < 8 && !found; ++attempt)
                {
                    List<Point3D> parts = new List<Point3D>();
                    int baseX = patch.Center.X-4+(attempt%4), baseY = patch.Center.Y-1+shape*4+(attempt/4);
                    for (int part = 0; part < sizes[index]; ++part)
                    {
                        int offset = forwards[index] ? part : sizes[index]-1-part; Point3D point;
                        if (!ArachnidInfestationTerrain.TryPoint(baseX+offset,baseY-offset,out point)) break;
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
            {
                ArachnidInfestationWeb web = new ArachnidInfestationWeb(this,patch.Id,graphics[i]);
                web.MoveToWorld(points[i],Map.Sosaria); m_Signs.Add(web);
            }
        }

        private void RetireInfestationBranch(int id)
        {
            ArachnidInfestationPatch patch = m_Infestation.Find(id);
            if (patch == null || patch.Retired) return;
            patch.Retired = true;
            foreach (ArachnidInfestationPatch child in m_Infestation.Patches)
                if (child.Parent == id) RetireInfestationBranch(child.Id);
            foreach (Mobile mobile in m_Creatures.ToArray())
            {
                ArachnidInfestationCreature creature = mobile as ArachnidInfestationCreature;
                if (creature != null && !creature.Deleted && creature.PatchId == id && Owns(creature,AttemptId)) creature.Delete();
            }
            foreach (Item item in m_Signs.ToArray())
            {
                ArachnidInfestationWeb web = item as ArachnidInfestationWeb;
                if (web != null && !web.Deleted && web.PatchId == id && Owns(web,AttemptId)) web.Delete();
            }
        }

        private int InfestationMilestone()
        {
            for (int i = 0; i < 3; ++i) if (!m_Ledger.IsSealed(i)) return i;
            return -1;
        }

        private bool ValidInfestationReference(Mobile mobile, ArachnidInfestationPatch patch, bool founder)
        {
            ArachnidInfestationCreature creature = mobile as ArachnidInfestationCreature;
            return creature != null && !creature.Deleted && creature.Alive && Owns(creature,AttemptId)
                && m_Creatures.Contains(creature) && creature.PatchId == patch.Id
                && (founder ? creature.Founder : creature.Guardian);
        }

        private void InfestationDeath(Mobile mobile)
        {
            ArachnidInfestationCreature creature = mobile as ArachnidInfestationCreature;
            if (creature != null)
            {
                ArachnidInfestationPatch patch = m_Infestation.Find(creature.PatchId);
                if (patch == null || patch.Retired) return;
                if (patch.Founder == creature && !patch.Established)
                {
                    patch.Retired = true;
                    ArachnidInfestationPatch parent = m_Infestation.Find(patch.Parent); if (parent != null) ++parent.Failures;
                }
                else if (patch.Guardian == creature)
                {
                    int milestone = InfestationMilestone();
                    if (milestone >= 0 && m_Ledger.Qualified(milestone).Count > 0)
                    {
                        QueueStage(milestone); m_State.CompleteMilestone(milestone,DateTime.UtcNow);
                        m_Wounds.Clear(); m_Poisons.Clear(); m_Bards.Clear();
                    }
                    RetireInfestationBranch(patch.Id);
                }
                return;
            }
            ArachnidOutbreakChampion queen = mobile as ArachnidOutbreakChampion;
            if (queen == null) return;
            // NPC-only victories never mint ground gold or personal entitlements.
            bool qualified = m_Ledger.Qualified(3).Count > 0;
            m_Placement = ArachnidInfestationTerrain.LootPlacement(queen.Location);
            if (qualified && !TryBossRewards(queen)) { Abort(DateTime.UtcNow,"Victory has no safe distinct loot ground."); return; }
            if (qualified) QueueStage(3); else m_Ledger.Seal(3);
            if (m_State.CompleteBoss(DateTime.UtcNow,Utility.RandomMinMax(30,60)))
            {
                Cleanup(AttemptId); m_Placement = null; m_Status = "Queen defeated; infestation ended.";
            }
        }

        private void TickInfestation(DateTime now)
        {
            ArachnidInfestationState colony = m_Infestation;
            ArachnidInfestationTerrain.BeginWork(4096);
            try
            {
                if (m_Placement == null) m_Placement = ArachnidInfestationTerrain.LootPlacement(colony.Origin);
                if (now-colony.Started >= TimeSpan.FromHours(2)) { Abort(now,"Infestation lifetime expired."); return; }
                colony.LastTick = now;
                colony.RouteRequests = 0; colony.TerrainProbes = 0;
                // Dead references are not births and must not turn lists into an unbounded history.
                m_Creatures.RemoveAll(delegate(Mobile mobile) { return mobile == null || mobile.Deleted; });
                m_Signs.RemoveAll(delegate(Item item) { return item == null || item.Deleted; });
                bool queen = false;
                foreach (Mobile mobile in m_Creatures) if (mobile is ArachnidOutbreakChampion && mobile.Alive) queen = true;
                if (!queen) { Abort(now,"Queen removed without a death outcome."); return; }
                for (int work = 0; work < 2 && colony.Patches.Count > 0; ++work)
                {
                    ArachnidInfestationPatch patch = colony.Patches[colony.Cursor];
                    colony.Cursor = (colony.Cursor+1)%colony.Patches.Count;
                    if (patch.Retired) continue;
                    Point3D point;
                    if (!ArachnidInfestationTerrain.TryPoint(patch.Center.X,patch.Center.Y,out point,false)) { RetireInfestationBranch(patch.Id); continue; }
                    if (patch.Established)
                    {
                        if (!ValidInfestationReference(patch.Guardian,patch,false)) { RetireInfestationBranch(patch.Id); continue; }
                        RefillInfestationPatch(patch,now); DecorateInfestationPatch(patch);
                    }
                }
                foreach (ArachnidInfestationPatch patch in colony.Patches)
                {
                    if (patch.Retired || patch.Established) continue;
                    if (!ValidInfestationReference(patch.Founder,patch,true)) { RetireInfestationBranch(patch.Id); continue; }
                    ArachnidInfestationPatch parent = colony.Find(patch.Parent);
                    if (parent == null || parent.Retired || !ValidInfestationReference(parent.Guardian,parent,false)) { RetireInfestationBranch(patch.Id); continue; }
                    if (patch.Founder.InRange(patch.Center,1))
                    {
                        if (patch.EstablishAfter == DateTime.MinValue) patch.EstablishAfter = now.AddSeconds(45);
                        if (now >= patch.EstablishAfter)
                        {
                            patch.Founder.Delete(); patch.Founder = null; patch.Established = true;
                            patch.Birth = ++colony.Epoch; EstablishPopulation(patch,now); m_FounderPath = null;
                        }
                    }
                    else
                    {
                        TurnParticipant actor = ArachnidOutbreakWebItem.Actor(patch.Founder);
                        if (actor != null && (actor.Group.Current != actor || actor.Pending != null)) return;
                        if (m_FounderPath == null) m_FounderPath = new PathFollower(patch.Founder,patch.Center);
                        ++colony.RouteRequests;
                        m_FounderPath.Follow(false,1);
                        if (++patch.Failures > 24) { ++parent.Failures; RetireInfestationBranch(patch.Id); m_FounderPath = null; }
                    }
                    return;
                }
                if (now < colony.NextGrowth || colony.Patches.Count >= ArachnidInfestationState.MaximumPatches) return;
                colony.NextGrowth = now.AddSeconds(45);
                int offset = Utility.Random(colony.Patches.Count);
                for (int i = 0; i < colony.Patches.Count; ++i)
                {
                    ArachnidInfestationPatch parent = colony.Patches[(i+offset)%colony.Patches.Count];
                    if (parent.Retired || !parent.Established || parent.Failures >= 2 || !ValidInfestationReference(parent.Guardian,parent,false)) continue;
                    int direction = Utility.Random(4); int[] dx = new int[] {0,12,0,-12}, dy = new int[] {-12,0,12,0};
                    Point3D destination;
                    ++colony.TerrainProbes;
                    if (!ArachnidInfestationTerrain.TryPoint(parent.Center.X+dx[direction],parent.Center.Y+dy[direction],out destination)) continue;
                    if (Math.Max(Math.Abs(destination.X-colony.Origin.X),Math.Abs(destination.Y-colony.Origin.Y))>ArachnidInfestationState.MaximumRadius) continue;
                    bool occupied = false;
                    foreach (ArachnidInfestationPatch other in colony.Patches)
                        if (Math.Abs(destination.X-other.Center.X)<10 && Math.Abs(destination.Y-other.Center.Y)<10) occupied = true;
                    if (occupied) continue;
                    if (colony.RouteRequests >= 2) return;
                    ++colony.RouteRequests;
                    if (!ArachnidInfestationTerrain.Connected(parent.Guardian,parent.Center,destination)) continue;
                    Point3D start; if (!FindInfestationPoint(parent.Center,2,out start)) return;
                    ArachnidInfestationPatch child = new ArachnidInfestationPatch();
                    child.Id = colony.Patches.Count; child.Parent = parent.Id; child.Center = destination;
                    child.Birth = colony.Epoch;
                    child.Founder = SpawnInfestationCreature(child,false,true,start);
                    if (child.Founder == null) return;
                    colony.Patches.Add(child); m_FounderPath = null; return;
                }
            }
            finally { colony.TerrainProbes = Math.Min(4096,ArachnidInfestationTerrain.EndWork()); }
        }
    }
}
