using Server.Custom.Confictura.Stabling;
using System;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class StableStone : Item
    {
        [Constructable]
        public StableStone()
            : base(0x14E7)
        {
            Name = "hitching post";
            Weight = 20.0;
        }

        public StableStone(Serial serial)
            : base(serial) { }

        private class StableEntry : ContextMenuEntry
        {
            private StableStone m_Trainer;
            private Mobile m_From;

            public StableEntry(StableStone trainer, Mobile from)
                : base(6126, 2)
            {
                m_Trainer = trainer;
                m_From = from;
            }

            public override void OnClick()
            {
                m_Trainer.BeginStable(m_From);
            }
        }

        public class ClaimingGumpEntry : ContextMenuEntry
        {
            private StableStone m_Trainer;
            private Mobile m_From;

            public ClaimingGumpEntry(StableStone trainer, Mobile from)
                : base(6165, 3)
            {
                m_Trainer = trainer;
                m_From = from;
            }

            public override void OnClick()
            {
                if (!(m_From is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_From;
                {
                    m_Trainer.BeginClaimList(m_From);
                }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from.Alive)
            {
                list.Add(new StableEntry(this, from));
                list.Add(new ClaimingGumpEntry(this, from));

                if (from.Stabled.Count > 0)
                    list.Add(new ClaimAllEntry(this, from));
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.ItemID == 0x14E7)
            {
                this.ItemID = 0x14E8;
            }
            else
            {
                this.ItemID = 0x14E7;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Stable Your Pets At Your Home");
            list.Add(1049644, "For Grandmasters In Camping");
        }

        private class StableTarget : Target
        {
            private StableStone m_Trainer;

            public StableTarget(StableStone trainer)
                : base(12, false, TargetFlags.None)
            {
                m_Trainer = trainer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature)
                    m_Trainer.EndStable(from, (BaseCreature)targeted);
                else
                    from.SendMessage("You can't stable that!");
            }
        }

        public void BeginStable(Mobile from)
        {
            if (Deleted || from == null || !from.CheckAlive())
                return;
            if (Movable || from.Skills[SkillName.Camping].Base < 100)
            {
                from.SendMessage("Lock this post down in your home and reach grandmaster Camping to use it.");
                return;
            }
            from.SendMessage(StableService.Terms);
            from.SendMessage("You are using {0} of {1} normal stable slots.", from.Stabled.Count, AnimalTrainer.GetMaxStabled(from));
            from.Target = new StableTarget(this);
        }

        public void EndStable(Mobile from, BaseCreature pet)
        {
            if (Deleted || from == null || !from.CheckAlive() || from.Map != Map || !from.InRange(this, 14))
                return;
            if (Movable || from.Skills[SkillName.Camping].Base < 100)
                return;
            StableService.StableManually(from, pet, AnimalTrainer.GetMaxStabled(from), null);
        }

        private class ClaimListGump : Gump
        {
            private StableStone m_Trainer;
            private Mobile m_From;
            private ArrayList m_List;

            public ClaimListGump(StableStone trainer, Mobile from, ArrayList list)
                : base(50, 50)
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
                    m_Trainer.EndClaimList(m_From, m_List[index] as BaseCreature);
            }
        }

        public void BeginClaimList(Mobile from)
        {
            if (Deleted || !from.CheckAlive())
                return;

            ArrayList list = new ArrayList();

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

            if (this.Movable)
                from.SendMessage("This must be locked down in a house to use!");
            else if (from.Skills[SkillName.Camping].Base < 100)
                from.SendMessage("Only grandmasters in camping may stable pets at home!");
            else if (list.Count > 0)
                from.SendGump(new ClaimListGump(this, from, list));
            else
                from.SendMessage("But I have no animals stabled with me at the moment!");
        }

        public void EndClaimList(Mobile from, BaseCreature pet)
        {
            if (Deleted || from == null || pet == null || pet.Deleted || !from.CheckAlive()
                || from.Map != Map || !from.InRange(this, 14) || !from.Stabled.Contains(pet))
                return;
            StableService.TryReturn(from, pet, true);
        }

        public override bool HandlesOnSpeech
        {
            get { return true; }
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if ((!e.Handled && e.HasKeyword(0x0008)) && (e.Mobile.InRange(this, 2)))
            {
                e.Handled = true;
                BeginStable(e.Mobile);
            }
            else if ((!e.Handled && e.HasKeyword(0x0009)) && (e.Mobile.InRange(this, 2)))
            {
                e.Handled = true;

                if (!Insensitive.Equals(e.Speech, "Claim"))
                    BeginClaimList(e.Mobile);
                else
                    BeginClaimList(e.Mobile);
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        private class ClaimAllEntry : ContextMenuEntry
        {
            private StableStone m_Trainer;
            private Mobile m_From;

            public ClaimAllEntry(StableStone trainer, Mobile from)
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
