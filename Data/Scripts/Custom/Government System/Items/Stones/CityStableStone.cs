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
    public class CityStableStone : Item
    {
        private CivicSign m_Sign;

        public CivicSign Sign
        {
            get { return m_Sign; }
            set { m_Sign = value; }
        }

        [Constructable]
        public CityStableStone()
            : base(0xED4)
        {
            Name = "a city stable stone";
            Movable = false;
        }

        public CityStableStone(Serial serial)
            : base(serial) { }

        private class StableEntry : ContextMenuEntry
        {
            private CityStableStone m_Trainer;
            private Mobile m_From;

            public StableEntry(CityStableStone trainer, Mobile from)
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

        private class ClaimAllEntry : ContextMenuEntry
        {
            private CityStableStone m_Trainer;
            private Mobile m_From;

            public ClaimAllEntry(CityStableStone trainer, Mobile from)
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

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive)
            {
                list.Add(new StableEntry(this, from));

                if (from.Stabled.Count > 0)
                    list.Add(new ClaimAllEntry(this, from));
            }
        }

        public static int GetMaxStabled(Mobile from)
        {
            double taming = from.Skills[SkillName.Taming].Value;
            double anlore = from.Skills[SkillName.Druidism].Value;
            double vetern = from.Skills[SkillName.Veterinary].Value;
            double sklsum = taming + anlore + vetern;

            int max;

            if (sklsum >= 240.0)
                max = 5;
            else if (sklsum >= 200.0)
                max = 4;
            else if (sklsum >= 160.0)
                max = 3;
            else
                max = 2;

            if (taming >= 100.0)
                max += (int)((taming - 90.0) / 10);

            if (anlore >= 100.0)
                max += (int)((anlore - 90.0) / 10);

            if (vetern >= 100.0)
                max += (int)((vetern - 90.0) / 10);

            return max;
        }

        private class StableTarget : Target
        {
            private CityStableStone m_Trainer;

            public StableTarget(CityStableStone trainer)
                : base(12, false, TargetFlags.None)
            {
                m_Trainer = trainer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature)
                    m_Trainer.EndStable(from, (BaseCreature)targeted);
                else if (targeted == from)
                    m_Trainer.Say(502672);
                else
                    m_Trainer.Say(1048053);
            }
        }

        public void BeginStable(Mobile from)
        {
            if (Deleted || from == null || !from.CheckAlive())
                return;
            from.SendMessage(StableService.Terms);
            from.Target = new StableTarget(this);
        }

        public void EndStable(Mobile from, BaseCreature pet)
        {
            if (Deleted || from == null || !from.CheckAlive() || from.Map != Map || !from.InRange(this, 14))
                return;
            StableService.StableManually(from, pet, GetMaxStabled(from), m_Sign == null ? null : m_Sign.Stone);
        }

        private class ClaimListGump : Gump
        {
            private CityStableStone m_Trainer;
            private Mobile m_From;
            private ArrayList m_List;

            public ClaimListGump(CityStableStone trainer, Mobile from, ArrayList list)
                : base(50, 50)
            {
                m_Trainer = trainer;
                m_From = from;
                m_List = list;

                from.CloseGump(typeof(ClaimListGump));

                AddPage(0);

                AddBackground(0, 0, 325, 50 + (list.Count * 20), 9250);
                AddAlphaRegion(5, 5, 315, 40 + (list.Count * 20));

                AddHtml(
                    15,
                    15,
                    275,
                    20,
                    "<BASEFONT COLOR=#FFFFFF>Select a pet to retrieve from the stables:</BASEFONT>",
                    false,
                    false
                );

                for (int i = 0; i < list.Count; ++i)
                {
                    BaseCreature pet = list[i] as BaseCreature;

                    if (pet == null || pet.Deleted)
                        continue;

                    AddButton(15, 39 + (i * 20), 10006, 10006, i + 1, GumpButtonType.Reply, 0);
                    AddHtml(
                        32,
                        35 + (i * 20),
                        275,
                        18,
                        String.Format("<BASEFONT COLOR=#C0C0EE>{0}</BASEFONT>", pet.Name),
                        false,
                        false
                    );
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
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

            if (list.Count > 0)
                from.SendGump(new ClaimListGump(this, from, list));
            else
                Say(502671); // But I have no animals stabled with me at the moment!
        }

        public void EndClaimList(Mobile from, BaseCreature pet)
        {
            if (Deleted || from == null || pet == null || pet.Deleted || !from.CheckAlive()
                || from.Map != Map || !from.InRange(this, 14) || !from.Stabled.Contains(pet))
                return;
            StableService.TryReturn(from, pet, true);
        }

        public void Claim(Mobile from)
        {
            if (Deleted || from == null || from.Map != Map || !from.InRange(this, 14))
                return;
            StableService.Claim(from, null);
        }

        public override bool HandlesOnSpeech
        {
            get { return true; }
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.HasKeyword(0x0008))
            {
                e.Handled = true;
                BeginStable(e.Mobile);
            }
            else if (!e.Handled && e.HasKeyword(0x0009))
            {
                e.Handled = true;

                if (!Insensitive.Equals(e.Speech, "claim"))
                    BeginClaimList(e.Mobile);
                else
                    Claim(e.Mobile);
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public void Say(int number)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, number);
        }

        public void Say(string args)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, args);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Sign);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                {
                    m_Sign = (CivicSign)reader.ReadItem();
                    break;
                }
            }
        }
    }
}
