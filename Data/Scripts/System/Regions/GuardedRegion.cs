using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Spells;

namespace Server.Regions
{
    public class GuardedRegion : BaseRegion
    {
        // Existing constructor
        public GuardedRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent) { }

        // New constructor
        public GuardedRegion(string name, Map map, int priority, Rectangle3D[] area)
            : base(name, map, priority, area) { }
    }
}
