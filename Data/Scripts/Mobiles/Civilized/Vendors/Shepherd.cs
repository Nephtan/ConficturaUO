using Server.Custom.Confictura.Stabling;
using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    public class Shepherd : BaseVendor
    {
        private DateTime m_NextTalk;
        public DateTime NextTalk
        {
            get { return m_NextTalk; }
            set { m_NextTalk = value; }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            Region reg = Region.Find(m.Location, m.Map);

            if (m is PlayerMobile && reg.IsPartOf("the Dungeon Room"))
            {
                if (DateTime.Now >= m_NextTalk && InRange(m, 20) && InLOS(m))
                {
                    switch (Utility.Random(45))
                    {
                        case 0:
                            Say("Another ale over here!");
                            break;
                        case 1:
                            PlaySound(Female ? 781 : 1052);
                            Say("*blows nose*");
                            break;
                        case 2:
                            PlaySound(Female ? 786 : 1057);
                            Say("*cough*");
                            break;
                        case 3:
                            PlaySound(Female ? 782 : 1053);
                            Say("*burp*");
                            break;
                        case 4:
                            PlaySound(Female ? 784 : 1055);
                            Say("*clears throat*");
                            break;
                        case 5:
                            PlaySound(Female ? 785 : 1056);
                            Say("*cough*");
                            break;
                        case 6:
                            PlaySound(Female ? 792 : 1064);
                            Say("*farts*");
                            break;
                        case 7:
                            PlaySound(Female ? 0x31B : 0x42B);
                            Say("*groans*");
                            break;
                        case 8:
                            PlaySound(Female ? 0x338 : 0x44A);
                            Say("*growls*");
                            break;
                        case 9:
                            PlaySound(Female ? 798 : 1070);
                            Say("*hiccup*");
                            break;
                        case 10:
                            PlaySound(Female ? 816 : 1090);
                            Say("*sigh*");
                            break;
                        case 11:
                            PlaySound(Female ? 817 : 1091);
                            Say("Ahh-choo!");
                            break;
                        case 12:
                            PlaySound(Female ? 818 : 1092);
                            Say("*sniff*");
                            break;
                        case 13:
                            PlaySound(Female ? 819 : 1093);
                            Say("*snore*");
                            break;
                        case 14:
                            PlaySound(Female ? 820 : 1094);
                            Say("*spits*");
                            break;
                        case 15:
                            PlaySound(Female ? 822 : 1096);
                            Say("*yawns*");
                            break;
                    }
                    ;
                    m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds(30));
                }
            }
        }

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get { return m_SBInfos; }
        }

        public override NpcGuild NpcGuild
        {
            get { return NpcGuild.DruidsGuild; }
        }

        [Constructable]
        public Shepherd()
            : base("the shepherd")
        {
            SetSkill(SkillName.Druidism, 64.0, 100.0);
            SetSkill(SkillName.Taming, 90.0, 100.0);
            SetSkill(SkillName.Veterinary, 65.0, 88.0);
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHumanAnimalTrainer());
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new Server.Items.FloppyHat(Utility.RandomNeutralHue()));
            AddItem(new Server.Items.ShepherdsCrook());
        }

        private class StableEntry : ContextMenuEntry
        {
            private Shepherd m_Trainer;
            private Mobile m_From;

            public StableEntry(Shepherd trainer, Mobile from)
                : base(6126, 12)
            {
                m_Trainer = trainer;
                m_From = from;
            }

            public override void OnClick()
            {
                m_Trainer.BeginStable(m_From);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new ClaimingGumpEntry(from, this));
            list.Add(new SpeechGumpEntry(from, this));
            list.Add(new RidingGumpEntry(from, this));
        }

        public class SpeechGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public SpeechGumpEntry(Mobile from, Mobile giver)
                : base(6146, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    if (!mobile.HasGump(typeof(SpeechGump)))
                    {
                        Server.Misc.IntelligentAction.SayHey(m_Giver);
                        mobile.SendGump(
                            new SpeechGump(
                                mobile,
                                "Animal Companions",
                                SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Pets")
                            )
                        );
                    }
                }
            }
        }

        public class RidingGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public RidingGumpEntry(Mobile from, Mobile giver)
                : base(6098, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    if (!mobile.HasGump(typeof(Server.Mobiles.Veterinarian.RidingGump)))
                    {
                        mobile.SendGump(new Server.Mobiles.Veterinarian.RidingGump(mobile));
                    }
                }
            }
        }

        public class ClaimingGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Shepherd m_Giver;

            public ClaimingGumpEntry(Mobile from, Shepherd giver)
                : base(6165, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    m_Giver.BeginClaimList(m_Mobile);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        private class ClaimListGump : Gump
        {
            private Shepherd m_Trainer;
            private Mobile m_From;
            private List<BaseCreature> m_List;

            public ClaimListGump(Shepherd trainer, Mobile from, List<BaseCreature> list)
                : base(25, 25)
            {
                from.SendSound(0x0EB);
                string color = "#bfad7d";

                m_Trainer = trainer;
                m_From = from;
                m_List = list;

                from.CloseGump(typeof(ClaimListGump));

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                AddPage(0);
                AddImage(0, 0, 9590, Server.Misc.PlayerSettings.GetGumpHue(from));
                AddHtml(
                    12,
                    12,
                    376,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">PETS IN THE STABLE</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(410, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);

                int y = 15;

                for (int i = 0; i < list.Count; ++i)
                {
                    BaseCreature pet = list[i] as BaseCreature;

                    if (pet == null || pet.Deleted)
                        continue;

                    y = y + 30;

                    AddButton(13, y, 4005, 4005, (i + 1), GumpButtonType.Reply, 0);
                    AddHtml(
                        50,
                        y,
                        349,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">"
                            + pet.Name
                            + " | "
                            + CheckBondingStatus(pet)
                            + "</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                }
            }

            public string CheckBondingStatus(BaseCreature pet)
            {
                string BondInfo;
                if (pet.IsBonded == true)
                {
                    BondInfo = "Bonded";
                }
                else if (pet.BondingBegin == DateTime.MinValue)
                {
                    BondInfo = "not bonding";
                }
                else
                {
                    DateTime today = DateTime.Now;
                    DateTime willbebonded = pet.BondingBegin + pet.BondingDelay;
                    TimeSpan daystobond = willbebonded - today;
                    if (daystobond.TotalDays > 0)
                    {
                        BondInfo = string.Format(
                            "{0} days, {1} hours and {2} minutes until it bonds",
                            daystobond.Days,
                            daystobond.Hours,
                            daystobond.Minutes
                        );
                    }
                    else
                    {
                        BondInfo = "Ready to bond!";
                    }
                }
                return BondInfo;
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                m_From.SendSound(0x0F2);
                int index = info.ButtonID - 1;

                if (index >= 0 && index < m_List.Count)
                    m_Trainer.EndClaimList(m_From, m_List[index]);
            }
        }

        private class ClaimAllEntry : ContextMenuEntry
        {
            private Shepherd m_Trainer;
            private Mobile m_From;

            public ClaimAllEntry(Shepherd trainer, Mobile from)
                : base(6127, 12)
            {
                m_Trainer = trainer;
                m_From = from;
            }

            public override void OnClick()
            {
                m_Trainer.Claim(m_From);
            }
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive)
            {
                list.Add(new StableEntry(this, from));

                if (from.Stabled.Count > 0)
                    list.Add(new ClaimAllEntry(this, from));
            }

            base.AddCustomContextEntries(from, list);
        }

        private class StableTarget : Target
        {
            private Shepherd m_Trainer;

            public StableTarget(Shepherd trainer)
                : base(12, false, TargetFlags.None)
            {
                m_Trainer = trainer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature)
                    m_Trainer.EndStable(from, (BaseCreature)targeted);
                else if (targeted == from)
                    m_Trainer.SayTo(from, 502672); // HA HA HA! Sorry, I am not an inn.
                else
                    m_Trainer.SayTo(from, 1048053); // You can't stable that!
            }
        }

        private void CloseClaimList(Mobile from)
        {
            from.CloseGump(typeof(ClaimListGump));
        }

        public void BeginClaimList(Mobile from)
        {
            if (Deleted || !from.CheckAlive())
                return;

            List<BaseCreature> list = new List<BaseCreature>();

            for (int i = 0; i < from.Stabled.Count; ++i)
            {
                BaseCreature pet = from.Stabled[i] as BaseCreature;

                if (pet == null || pet.Deleted)
                {
                    if (pet != null)
                        pet.IsStabled = false;
                    from.Stabled.RemoveAt(i);
                    --i;
                    continue;
                }

                StableService.ShowAccount(from, pet);
                list.Add(pet);
            }

            if (list.Count > 0)
                from.SendGump(new ClaimListGump(this, from, list));
            else
                SayTo(from, 502671); // But I have no animals stabled with me at the moment!
        }

        public void EndClaimList(Mobile from, BaseCreature pet)
        {
            if (Deleted || from == null || pet == null || pet.Deleted || !from.CheckAlive()
                || from.Map != Map || !from.InRange(this, 14) || !from.Stabled.Contains(pet))
                return;
            StableService.TryReturn(from, pet, true);
        }

        public void BeginStable(Mobile from)
        {
            if (Deleted || from == null || !from.CheckAlive())
                return;
            from.SendMessage(StableService.Terms);
            from.SendMessage("You are using {0} of {1} normal stable slots.", from.Stabled.Count, AnimalTrainer.GetMaxStabled(from));
            from.Target = new StableTarget(this);
        }

        public void EndStable(Mobile from, BaseCreature pet)
        {
            if (Deleted || from == null || !from.CheckAlive() || from.Map != Map || !from.InRange(this, 14))
                return;
            StableService.StableManually(from, pet, AnimalTrainer.GetMaxStabled(from), null);
        }

        public void Claim(Mobile from)
        {
            Claim(from, null);
        }

        public void Claim(Mobile from, string petName)
        {
            if (Deleted || from == null || from.Map != Map || !from.InRange(this, 14))
                return;
            StableService.Claim(from, petName);
        }

        public bool CanClaim(Mobile from, BaseCreature pet)
        {
            return ((from.Followers + pet.ControlSlots) <= from.FollowersMax);
        }



        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.HasKeyword(0x0008)) // *stable*
            {
                e.Handled = true;

                CloseClaimList(e.Mobile);
                BeginStable(e.Mobile);
            }
            else if (!e.Handled && e.HasKeyword(0x0009)) // *claim*
            {
                e.Handled = true;

                CloseClaimList(e.Mobile);

                int index = e.Speech.IndexOf(' ');

                if (index != -1)
                    Claim(e.Mobile, e.Speech.Substring(index).Trim());
                else
                    Claim(e.Mobile);
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public Shepherd(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
