using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Regions;

namespace Server.Gumps
{
    public class CityCorpseFeeGump : Gump
    {
        private CityResurrectionStone m_Stone;
        private Mobile m_From;
        private CivicSign m_Sign;
        private CityManagementStone m_City;
        private Item m_Corpse;
        private int m_Fee;
        private bool m_Responded;

        public CityCorpseFeeGump(CityResurrectionStone stone, Mobile from)
            : base(50, 50)
        {
            m_Stone = stone;
            m_From = from;
            m_Sign = stone.Sign;
            m_City = m_Sign.Stone;
            m_Corpse = from.Corpse;
            m_Fee = m_City.CorpseFee;
            from.CloseGump(typeof(CityCorpseFeeGump));

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(17, 12, 300, 139, 5120);
            AddItem(7, 15, 2);
            AddItem(28, 15, 3);
            AddHtml(
                76,
                18,
                234,
                66,
                @"<BASEFONT COLOR=WHITE><CENTER>This city has a fee to retrive your corpse.<BR>Woul you like to get your corpse?</CENTER></BASEFONT>",
                (bool)false,
                (bool)false
            );
            AddLabel(29, 92, 1149, @"Cost: " + m_Fee.ToString());
            AddButton(82, 115, 247, 248, 1, GumpButtonType.Reply, 0);
            AddButton(190, 115, 241, 242, 2, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (m_Responded || state == null || info == null)
                return;

            m_Responded = true;
            PlayerMobile from = state.Mobile as PlayerMobile;
            if (info.ButtonID != 1 || from == null || from != m_From || from.Deleted)
                return;

            if (m_Stone == null || m_Stone.Deleted || m_Sign == null || m_Sign.Deleted
                || m_City == null || m_City.Deleted || m_Stone.Sign != m_Sign
                || m_Sign.Stone != m_City || from.City != m_City)
            {
                from.SendMessage("This corpse retrieval offer is no longer available. No money was taken.");
                return;
            }
            if (from.Map == null || from.Map == Map.Internal || from.Map != m_Stone.Map
                || !from.InRange(m_Stone.GetWorldLocation(), 2))
            {
                from.SendMessage("You are not close enough to use this. No money was taken.");
                return;
            }
            if (m_Fee < 0 || m_Fee != m_City.CorpseFee || (long)m_City.CityTreasury + m_Fee > Int32.MaxValue)
            {
                from.SendMessage("This fee is no longer available. Please request a new offer. No money was taken.");
                return;
            }
            Item corpse = from.Corpse;
            if (corpse == null || corpse.Deleted || corpse != m_Corpse)
            {
                from.SendMessage("Your corpse could not be located for this offer. No money was taken.");
                return;
            }
            if (!m_Stone.CanRegCorpse(from))
            {
                from.SendMessage("You may only do this 3 times a week. No money was taken.");
                return;
            }
            if (!Banker.Withdraw(from, m_Fee))
            {
                from.SendMessage("You do not have enough money in your bank account. No money was taken.");
                return;
            }

            // Eligibility and payment precede the existing quota commit on the game thread.
            m_Stone.RegCorpse(from);
            m_City.CityTreasury += m_Fee;
            corpse.MoveToWorld(from.Location, from.Map);
            from.SendMessage("Your corpse has been found.");
            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                0x3728, 10, 30, 5052);
            Effects.PlaySound(from.Location, from.Map, 0x201);
        }
    }
}
