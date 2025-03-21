using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Spells.Song
{
    public class SinewyEtudeSong : Song
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sinewy Etude",
            "*plays a sinewy etude*",
            //SpellCircle.First,
            //212,9041
            -1
        );

        private SongBook m_Book;

        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(2); }
        }
        public override double RequiredSkill
        {
            get { return 60.0; }
        }
        public override int RequiredMana
        {
            get { return 20; }
        }

        public SinewyEtudeSong(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            //get songbook instrument
            Spellbook book = Spellbook.Find(Caster, -1, SpellbookType.Song);
            if (book == null)
            {
                return;
            }
            m_Book = (SongBook)book;
            if (
                m_Book.Instrument == null
                || !(Caster.InRange(m_Book.Instrument.GetWorldLocation(), 1))
            )
            {
                Caster.SendMessage(
                    "Your instrument is missing! You can select another from your song book."
                );
                return;
            }

            bool sings = false;

            if (CheckSequence())
            {
                sings = true;

                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (Caster.CanBeBeneficial(m, false, true) && !(m is Golem))
                        targets.Add(m);
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    int amount = Server.Misc.MyServerSettings.PlayerLevelMod(
                        (int)(MusicSkill(Caster) / 16),
                        Caster
                    );
                    string str = "str";

                    double duration = (double)(MusicSkill(Caster) * 2);

                    StatMod mod = new StatMod(
                        StatType.Str,
                        str,
                        +amount,
                        TimeSpan.FromSeconds(duration)
                    );

                    m.AddStatMod(mod);

                    m.FixedParticles(0x375A, 10, 15, 5017, 0x224, 3, EffectLayer.Waist);

                    string args = String.Format("{0}", amount);
                    BuffInfo.RemoveBuff(m, BuffIcon.SinewyEtude);
                    BuffInfo.AddBuff(
                        m,
                        new BuffInfo(
                            BuffIcon.SinewyEtude,
                            1063587,
                            1063588,
                            TimeSpan.FromSeconds(duration),
                            m,
                            args.ToString(),
                            true
                        )
                    );
                }
            }

            BardFunctions.UseBardInstrument(m_Book.Instrument, sings, Caster);
            FinishSequence();
        }
    }
}
