using System;
using Server;
using Server.Mobiles;
using Server.Regions;

namespace Server.Engines.CannedEvil
{
    public class ChampionSpawnRegion : BaseRegion
    {
        public override bool YoungProtected
        {
            get { return false; }
        }

        private ChampionSpawn m_Spawn;

        public ChampionSpawn ChampionSpawn
        {
            get { return m_Spawn; }
        }

        public ChampionSpawnRegion(ChampionSpawn spawn)
            : base(null, spawn.Map, Region.Find(spawn.Location, spawn.Map), spawn.SpawnArea)
        {
            m_Spawn = spawn;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            base.AlterLightLevel(m, ref global, ref personal);
            global = Math.Max(global, 1 + m_Spawn.Level); //This is a guesstimate.  TODO: Verify & get exact values // OSI testing: at 2 red skulls, light = 0x3 ; 1 red = 0x3.; 3 = 8; 9 = 0xD 8 = 0xD 12 = 0x12 10 = 0xD
        }
    }
}
