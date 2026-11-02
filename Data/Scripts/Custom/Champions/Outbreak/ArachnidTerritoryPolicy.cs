using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public static class ArachnidTerritoryPolicy
    {
        public static int TierAt(Point3D point, Point3D queen)
        {
            int x = point.X - queen.X, y = point.Y - queen.Y;
            if (x < -150 || x >= 150 || y < -150 || y >= 150) return 0;
            if (x >= -50 && x < 50 && y >= -50 && y < 50) return 4;
            if (x >= -100 && x < 100 && y >= -100 && y < 100) return 3;
            return 2;
        }

        public static bool HasEggSac(Point3D point)
        { return (point.X / 12) % 3 == 0 && (point.Y / 12) % 3 == 0; }

        public static bool HasFrontierSpider(Point3D point)
        { return ((point.X / 12) + 2 * (point.Y / 12)) % 4 == 0; }

        public static bool HasInnerNest(Point3D point, int innerPercent)
        { return ((point.X/12*31+point.Y/12*17)%100+100)%100 < innerPercent; }

        public static int PopulationTarget(ArachnidInfestationPatch patch, int tier)
        { return PopulationTarget(patch,tier,45); }

        public static int PopulationTarget(ArachnidInfestationPatch patch, int tier, int innerPercent)
        {
            if (tier == 0) return (HasEggSac(patch.Center) ? 1 : 0) + (HasFrontierSpider(patch.Center) ? 1 : 0);
            return Math.Max(1,(tier == 2 ? 3 : tier == 3 ? 5 : 7)*innerPercent/100);
        }
    }

    public partial class ArachnidOutbreakController
    {
        public Point3D InfestationQueenCenter
        { get { return m_Infestation == null ? Point3D.Zero : m_Infestation.QueenCenterKnown ? m_Infestation.QueenCenter : m_Infestation.Origin; } }

        internal bool IsOutskirtsEgg(ArachnidInfestationCreature creature)
        {
            ArachnidInfestationPatch patch = m_Infestation == null ? null : m_Infestation.Find(creature.PatchId);
            return patch != null && creature.Guardian && m_Infestation.Tier(patch) == 0 && ArachnidTerritoryPolicy.HasEggSac(patch.Center);
        }

        private bool NeedsGuardian(ArachnidInfestationPatch patch)
        { return m_Infestation.Tier(patch) > 0 ? patch.Id==0 || ArachnidTerritoryPolicy.HasInnerNest(patch.Center,InnerPopulationPercent) : ArachnidTerritoryPolicy.HasEggSac(patch.Center); }

        private bool OpenEggGround(Point3D point)
        {
            int open = 0;
            for (int x = -1; x <= 1; ++x)
                for (int y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0) continue;
                    Point3D ground;
                    if (ArachnidInfestationTerrain.TryPoint(point.X+x,point.Y+y,out ground,false)
                        && Math.Abs(ground.Z-point.Z) <= 2 && Map.Sosaria.CanSpawnMobile(ground)) ++open;
                }
            return open >= 5;
        }

        private void UpdateQueenCenter(ArachnidOutbreakChampion queen)
        {
            m_Infestation.QueenCenter = queen.Location; m_Infestation.QueenCenterKnown = true;
        }

        private void RemovePatchWebs(ArachnidInfestationPatch patch)
        {
            foreach (Item item in m_Signs.ToArray())
            {
                ArachnidInfestationWeb web = item as ArachnidInfestationWeb;
                if (web != null && !web.Deleted && web.PatchId == patch.Id && Owns(web,AttemptId)) web.Delete();
            }
        }

        private bool PruneQuietSurplus(ArachnidInfestationPatch patch, DateTime now)
        {
            int tier = m_Infestation.Tier(patch);
            int target = ArachnidTerritoryPolicy.PopulationTarget(patch,tier,InnerPopulationPercent);
            if (patch.Residents.Count <= target) return false;
            // Migration is limited to genuinely quiet records; never erase a visible or engaged actor.
            foreach (ArachnidInfestationResident resident in patch.Residents)
            {
                ArachnidInfestationCreature actor = resident.Actor as ArachnidInfestationCreature;
                if (actor != null && !actor.Deleted && actor.Alive
                    && (SpreadingEncounterScheduler.Interested(actor) || SpreadingEncounterScheduler.Busy(actor)
                        || now-actor.LastRelevant < TimeSpan.FromSeconds(SpreadingEncounterScheduler.QuietSeconds))) return false;
            }
            for (int i = patch.Residents.Count-1; i >= 0; --i)
            {
                ArachnidInfestationResident resident = patch.Residents[i];
                bool retain = resident.Slot == 0 ? NeedsGuardian(patch) : target > (NeedsGuardian(patch) ? 1 : 0);
                if (retain && patch.Residents.Count <= target) continue;
                if (resident.Slot == 0 && NeedsGuardian(patch)) continue;
                if (resident.Actor != null && !resident.Actor.Deleted) resident.Actor.Delete();
                if (resident.Inventory != null && !resident.Inventory.Deleted && resident.Inventory.Parent == null) resident.Inventory.Delete();
                patch.Residents.RemoveAt(i);
                if (resident.Slot == 0) patch.Guardian = null;
            }
            return true;
        }
    }
}
