using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Chivalry;
using Server.Spells.Fourth;
using Server.Spells.Herbalist;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Undead;

namespace Server.Regions
{
    public class DungeonHomeRegion : BaseRegion
    {
        public DungeonHomeRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent) { }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = LightCycle.JailLevel;
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            LoggingFunctions.LogRegions(m, "a Dungeon Dwelling", "enter");

            Server.Misc.RegionMusic.MusicRegion(m, this);
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            LoggingFunctions.LogRegions(m, "a Dungeon Dwelling", "exit");
        }
    }
}
