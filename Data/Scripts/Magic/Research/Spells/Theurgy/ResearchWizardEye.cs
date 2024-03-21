using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Research
{
    public class ResearchWizardEye : ResearchSpell
    {
        public override int spellIndex
        {
            get { return 23; }
        }
        public int CirclePower = 1;
        public static int spellID = 23;
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(1.0); }
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
            Reagent.EyeOfToad,
            Reagent.SilverWidow,
            Reagent.GargoyleEar,
            Reagent.BlackPearl
        );

        public ResearchWizardEye(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this, spellID, Scroll, alwaysConsume);
            }
        }

        private class InternalTarget : Target
        {
            private ResearchWizardEye m_Owner;
            private int m_SpellIndex;
            private Item m_fromBook;
            private bool m_alwaysConsume;

            public InternalTarget(
                ResearchWizardEye owner,
                int spellIndex,
                Item fromBook,
                bool alwaysConsume
            )
                : base(Core.ML ? 10 : 12, false, TargetFlags.None)
            {
                m_Owner = owner;
                m_fromBook = fromBook;
                m_SpellIndex = spellIndex;
                m_alwaysConsume = alwaysConsume;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (Server.Items.ArtifactManual.LookupTheItem(from, targeted))
                {
                    Server.Misc.Research.ConsumeScroll(
                        from,
                        true,
                        m_SpellIndex,
                        m_alwaysConsume,
                        m_fromBook
                    );
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
