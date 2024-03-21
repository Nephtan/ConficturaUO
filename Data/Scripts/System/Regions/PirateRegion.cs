using System;
using System.IO;
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
using Server.Targeting;

namespace Server.Regions
{
    public class PirateRegion : BaseRegion
    {
        public PirateRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent) { }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
            if (m is PlayerMobile)
            {
                LoggingFunctions.LogRegions(m, this.Name, "enter");
            }

            Server.Misc.RegionMusic.MusicRegion(m, this);
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            if (
                m is BaseCreature
                && (
                    m is Spectre
                    || m is Zombie
                    || m is Ghoul
                    || m is AncientLich
                    || m is SkeletalKnight
                    || m is SkeletalMage
                    || m is PirateCaptain
                    || m is ElfPirateCaptain
                    || m is PirateCrewBow
                    || m is PirateCrew
                    || m is ElfPirateCrew
                    || m is ElfPirateCrewBow
                    || m is PirateCrewMage
                    || m is ElfPirateCrewMage
                )
            )
            {
                BaseCreature pirates = (BaseCreature)m;
                Effects.SendLocationParticles(
                    EffectItem.Create(pirates.Location, pirates.Map, EffectItem.DefaultDuration),
                    0x3728,
                    10,
                    10,
                    2023
                );
                pirates.PlaySound(0x1FE);
                pirates.Location = pirates.Home;
                Effects.SendLocationParticles(
                    EffectItem.Create(pirates.Home, pirates.Map, EffectItem.DefaultDuration),
                    0x3728,
                    10,
                    10,
                    5023
                );
            }
            else
            {
                LoggingFunctions.LogRegions(m, this.Name, "exit");
            }
        }
    }
}
