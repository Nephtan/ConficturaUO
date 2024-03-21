using System;
using System.IO;
using System.Text;
using System.Xml;
using Server;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Regions
{
    public class SkyHomeDwelling : BaseRegion
    {
        public SkyHomeDwelling(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent) { }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
            LoggingFunctions.LogRegions(m, "a Dwelling in the Sky", "enter");
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            LoggingFunctions.LogRegions(m, "a Dwelling in the Sky", "exit");
        }
    }
}
