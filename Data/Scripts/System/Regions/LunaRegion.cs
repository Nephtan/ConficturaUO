using System;
using System.IO;
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
    public class LunaRegion : BaseRegion
    {
        public LunaRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent) { }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            if (target.Region is HouseRegion)
                return false;
            else
                return base.AllowHarmful(from, target);
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
            if (m is PlayerMobile)
            {
                if (
                    m.Skills[SkillName.Elementalism].Base >= 80.0
                    || m.Skills[SkillName.Magery].Base >= 80.0
                    || m.Skills[SkillName.Necromancy].Base >= 80.0
                )
                {
                    LoggingFunctions.LogRegions(m, this.Name, "enter");
                }
                else
                {
                    BaseCreature.TeleportPets(m, new Point3D(3696, 523, 5), Map.Sosaria, false);
                    m.MoveToWorld(new Point3D(3696, 523, 5), Map.Sosaria);
                    m.PlaySound(0x1FE);
                    m.SendMessage("You lack the magical essence to remain on the moon.");
                }
            }
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            if (m is PlayerMobile)
            {
                if (
                    m.Skills[SkillName.Elementalism].Base >= 80.0
                    || m.Skills[SkillName.Magery].Base >= 80.0
                    || m.Skills[SkillName.Necromancy].Base >= 80.0
                )
                {
                    LoggingFunctions.LogRegions(m, this.Name, "exit");
                }
            }
        }
    }
}
