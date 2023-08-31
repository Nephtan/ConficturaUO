using System;
using Server;
using Server.Items;
using Server.Gumps;

namespace Server.Mobiles
{
    [CorpseName("a magic dragon corpse")]
    public class GoddessOfProtection : BaseCreature
    {
        private static bool m_Talked; // flag to prevent spam

        string[] kfcsay = new string[]
        {
            "Hail!",
            "Would thou wish to choose thy path? PvP, PvE, or wish to remain neutral?",
            "Hail traveler. Would thee wish to choose thy path as PvP, PvE, or wish to remain neutral?",
            "Welcome, I am Thuvia, the Goddess of Protection. I can guide thee in thy choice of PvP, PvE, or thou may choose to remain neutral.",
            "If thou choose PvP, thou can engage in combat with any player, except those who chose PvE. Thou can perform and receive beneficial actions with any player.",
            "If thou choose PvE, thou cannot engage in combat with any player. Thou can perform beneficial actions on other PvE and neutral players but not PvP players. Thou can receive beneficial actions from any player.",
            "If thou choose to remain neutral, thou can attack PvP and other neutral players, but not PvE players. Thou can perform and receive beneficial actions with any player.",
            "To choose thy path, say 'PvE' for Player versus Environment, 'PvP' for Player versus Player, or remain silent to stay neutral."
        };
        public Timer m_SpeechTimer;

        [Constructable]
        public GoddessOfProtection()
            : base(AIType.AI_Melee, FightMode.None, 20, 1, 0.9, 0.9)
        {
            Body = 0x191;
            BaseSoundID = 0;
            Name = ("Thuvia");
            Title = ("the Goddess Of Protection");
            Female = true;
            Hue = 33770;
            SpeechHue = 16;
            EmoteHue = 17;
            Direction = Direction.South;
            Blessed = true;
            CantWalk = true;

            SetStr(225, 250);
            SetDex(120, 125);
            SetInt(800, 935);
            SetHits(500, 550);

            SetSkill(SkillName.Bludgeoning, 115.0, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.5, 120.0);
            SetSkill(SkillName.Healing, 110.0, 120.0);
            SetSkill(SkillName.Anatomy, 110.0, 120.0);
            SetSkill(SkillName.Focus, 116.0, 120.0);
            SetSkill(SkillName.Magery, 119.0, 125.0);

            VirtualArmor = 14;

            Container pack = new Backpack();
            pack.Movable = false;
            AddItem(pack);

            FancyDress dress = new FancyDress();
            dress.Hue = 1151;
            dress.LootType = LootType.Blessed;
            dress.Movable = false;
            AddItem(dress);

            Sandals sandals = new Sandals();
            sandals.Hue = 1151;
            sandals.LootType = LootType.Blessed;
            sandals.Movable = false;
            AddItem(sandals);

            Item hair = new Item(0x203C);
            hair.Hue = 1151;
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);
        }

        public GoddessOfProtection(Serial serial)
            : base(serial) { }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 3))
                return true;

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.Mobile.InRange(this, 6))
            {
                Mobile from = e.Mobile;
                CheckSpeech(from, e.Speech.ToLower());
            }
        }

        public virtual void CheckSpeech(Mobile from, string speech)
        {
            PlayerMobile pk = from as PlayerMobile;
            if (
                speech == "hail goddess"
                || speech == "hail"
                || speech == "pvp"
                || speech == "pve"
                || speech == "pk"
                || speech == "nonpk"
            )
            {
                this.Say(
                    "Greetings, {0}. Does thou wish to choose between PvE and PvP? If so, say 'choose'.",
                    from.Name
                );
            }
            else if (speech == "help")
            {
                this.Say(
                    "I am here to help thee choose between PvE and PvP modes. In PvE, thou shall fight against monsters, whilst in PvP, thou can fight against other players. To make a choice, say 'choose'."
                );
            }
            else if (speech == "status")
            {
                if (pk.NONPK == NONPK.NONPK)
                {
                    pk.SendMessage(33, "Thou art currently in PvE mode.");
                }
                else if (pk.NONPK == NONPK.PK)
                {
                    pk.SendMessage(33, "Thou art currently in PvP mode.");
                }
                else
                {
                    pk.SendMessage(
                        33,
                        "Thou hast not yet chosen a mode. Say 'choose' to make a choice."
                    );
                }
            }
            else if (speech == "choose")
            {
                pk.SendGump(new PKNONPK());
            }
        }

        private class SpeechTimer : Timer
        {
            private GoddessOfProtection gp;

            private string spch1,
                spch2,
                spch3,
                spch4,
                spch5;

            private int cnt;

            public SpeechTimer(
                GoddessOfProtection owner,
                string speech1,
                string speech2,
                string speech3,
                string speech4,
                string speech5
            )
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(2.0))
            {
                gp = owner;
                spch1 = speech1;
                spch2 = speech2;
                spch3 = speech3;
                spch4 = speech4;
                spch5 = speech5;
            }

            protected override void OnTick()
            {
                cnt++;

                switch (cnt)
                {
                    case 1:
                    {
                        gp.Say(spch1);

                        if (spch2 == null)
                        {
                            Stop();
                            return;
                        }
                        break;
                    }
                    case 5:
                    {
                        gp.Say(spch2);

                        if (spch3 == null)
                        {
                            Stop();
                            return;
                        }
                        break;
                    }
                    case 9:
                    {
                        gp.Say(spch3);

                        if (spch4 == null)
                        {
                            Stop();
                            return;
                        }
                        break;
                    }
                    case 13:
                    {
                        gp.Say(spch4);
                        if (spch5 == null)
                        {
                            Stop();
                            return;
                        }
                        break;
                    }
                    case 17:
                    {
                        gp.Say(spch5);
                        Stop();
                        break;
                    }
                }
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m_Talked == false)
            {
                if (m is PlayerMobile && m.InRange(this, 8))
                {
                    m_Talked = true;
                    SayRandom(kfcsay, this);
                    this.Move(GetDirectionTo(m.Location));
                    m.SendMessage(
                        "Hail, {0}. Would thou wish to choose thy path? PvP, PvE, or wish to remain neutral?",
                        m.Name
                    );
                    m.SendMessage(
                        "Say 'help' for more information, or 'choose' to select your path."
                    );
                    SpamTimer t = new SpamTimer();
                    t.Start();
                }
            }
        }

        private class SpamTimer : Timer
        {
            public SpamTimer()
                : base(TimeSpan.FromSeconds(8))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Talked = false;
            }
        }

        private static void SayRandom(string[] say, Mobile m)
        {
            m.Say(say[Utility.Random(say.Length)]);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
