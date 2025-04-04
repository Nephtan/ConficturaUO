using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Research
{
    public class ResearchDivination : ResearchSpell
    {
        public override int spellIndex
        {
            get { return 39; }
        }
        public int CirclePower = 5;
        public static int spellID = 39;
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(1.5); }
        }
        public override double RequiredSkill
        {
            get
            {
                return (double)(Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 8)));
            }
        }
        public override int RequiredMana
        {
            get { return Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 7)); }
        }

        private static SpellInfo m_Info = new SpellInfo(
            Server.Misc.Research.SpellInformation(spellID, 2),
            Server.Misc.Research.CapsCast(Server.Misc.Research.SpellInformation(spellID, 4)),
            215,
            9001,
            Reagent.Ginseng,
            Reagent.DaemonBlood,
            Reagent.EyeOfToad
        );

        public ResearchDivination(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this, spellID, Scroll);
            }
        }

        private class InternalTarget : Target
        {
            private ResearchDivination m_Owner;
            private int m_SpellIndex;
            private Item m_fromBook;

            public InternalTarget(ResearchDivination owner, int spellIndex, Item fromBook)
                : base(Core.ML ? 10 : 12, false, TargetFlags.None)
            {
                m_Owner = owner;
                m_fromBook = fromBook;
                m_SpellIndex = spellIndex;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                bool success = false;

                if (targeted is PlayerMobile)
                {
                    Mobile p = (Mobile)targeted;
                    from.CloseGump(typeof(StatsGump));
                    from.SendGump(new StatsGump(from, p, 2));
                    success = true;
                }
                else if (
                    targeted is HenchmanMonster
                    || targeted is HenchmanWizard
                    || targeted is HenchmanFighter
                    || targeted is HenchmanArcher
                )
                {
                    from.SendMessage(
                        "This spell can really tell you nothing of importance for this one."
                    );
                }
                else if (
                    targeted is BaseVendor
                    || targeted is BasePerson
                    || targeted is Citizens
                    || targeted is PackBeast
                    || targeted is FrankenPorter
                    || targeted is FrankenFighter
                    || targeted is HenchmanFamiliar
                    || targeted is AerialServant
                    || targeted is GolemPorter
                    || targeted is Robot
                    || targeted is GolemFighter
                    || targeted is HenchmanArcher
                    || targeted is HenchmanMonster
                    || targeted is HenchmanFighter
                    || targeted is HenchmanWizard
                )
                {
                    from.SendMessage(
                        "This spell can really tell you nothing of importance for this one."
                    );
                }
                else if (targeted is Mobile)
                {
                    Mobile m = (Mobile)targeted;

                    if (Server.Items.PlayersHandbook.IsPeople(m))
                    {
                        BaseCreature c = (BaseCreature)m;
                        from.CloseGump(typeof(Server.SkillHandlers.DruidismGump));
                        from.SendGump(new Server.SkillHandlers.DruidismGump(from, c, 4));
                        from.SendSound(0x0F9);
                        success = true;
                    }
                    else if (m is BaseCreature)
                    {
                        BaseCreature c = (BaseCreature)m;
                        from.CloseGump(typeof(Server.SkillHandlers.DruidismGump));
                        from.SendGump(new Server.SkillHandlers.DruidismGump(from, c, 3));
                        from.SendSound(0x0F9);
                        success = true;
                    }
                    else
                    {
                        from.SendMessage("This spell doesn't seem to work on that.");
                    }
                }
                else
                {
                    from.SendMessage("This spell doesn't seem to work on that.");
                }

                if (success)
                {
                    Server.Misc.Research.ConsumeScroll(from, true, m_SpellIndex, false, m_fromBook);
                    from.PlaySound(0xF7);
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
