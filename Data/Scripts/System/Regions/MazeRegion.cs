using System;
using System.IO;
using System.Text;
using System.Xml;
using Server;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Chivalry;
using Server.Spells.Fourth;
using Server.Spells.Herbalist;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Undead;

namespace Server.Regions
{
    public class MazeRegion : BaseRegion
    {
        public MazeRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent) { }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
            if (m is PlayerMobile)
            {
                LoggingFunctions.LogRegions(m, this.Name, "enter");
            }
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            if (m is PlayerMobile)
            {
                LoggingFunctions.LogRegions(m, this.Name, "exit");
            }
        }
    }
}
