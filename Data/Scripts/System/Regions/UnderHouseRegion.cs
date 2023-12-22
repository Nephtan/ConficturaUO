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
using Server.Spells.Seventh;
using Server.Spells.Sixth;

namespace Server.Regions
{
    public class UnderHouseRegion : BaseRegion
    {
        public UnderHouseRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent) { }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return true;
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            if (target.Region is HouseRegion)
                return false;
            else
                return base.AllowHarmful(from, target);
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = LightCycle.NightLevel;
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
            LoggingFunctions.LogRegions(m, this.Name, "enter");
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            LoggingFunctions.LogRegions(m, this.Name, "exit");
        }
    }
}
