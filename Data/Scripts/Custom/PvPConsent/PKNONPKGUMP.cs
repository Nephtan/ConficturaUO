using System;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class PKNONPK : Gump
    {
        public PKNONPK()
            : base(0, 0)
        {
            this.Closable = false;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            AddPage(0);
            this.AddBackground(0, 0, 400, 300, 9200);
            this.AddAlphaRegion(5, 5, 389, 289);
            AddLabel(100, 20, 62, @"Confictura Consensual PvP System");
            AddLabel(75, 40, 62, @"Welcome. You may choose to flag yourself");
            AddLabel(90, 60, 62, @"for PvP or for PvE only. Or you may");
            AddLabel(98, 80, 62, @"close this window to remain neutral.");
            AddLabel(55, 120, 37, @"Mark me for PvP");
            AddButton(97, 145, 2151, 2153, 1, GumpButtonType.Reply, 0);
            AddLabel(235, 120, 37, @"Mark me for PvE");
            AddButton(277, 145, 2151, 2153, 2, GumpButtonType.Reply, 0);
            AddImage(90, 180, 5);
            AddImage(270, 180, 6);
            AddLabel(160, 232, 682, @"Remain Neutral");
            AddButton(187, 254, 2151, 2153, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            PlayerMobile pk = from as PlayerMobile;
            switch (info.ButtonID)
            {
                case 0:
                {
                    pk.NONPK = NONPK.Null;
                    pk.Title = null;
                    pk.SendMessage(62, "You have chosen neutrality");
                    pk.CloseGump(typeof(PKNONPK));
                    pk.SendMessage(
                        62,
                        "Thou hast chosen to remain neutral. Thou canst attack PvP and other neutral players, but not PvE players."
                    );
                    break;
                }
                case 1:
                {
                    pk.NONPK = NONPK.PK;
                    pk.Title = "[PvP]";
                    pk.SendMessage(62, "You have chosen [PvP]");
                    pk.CloseGump(typeof(PKNONPK));
                    pk.SendMessage(
                        62,
                        "Thou hast chosen the path of PvP. Remember, thou canst attack any player, except PvE players, and perform beneficial actions on any player."
                    );
                    break;
                }
                case 2:
                {
                    pk.NONPK = NONPK.NONPK;
                    pk.Title = "[PvE]";
                    pk.SendMessage(62, "You have chosen [PvE]");
                    pk.CloseGump(typeof(PKNONPK));
                    pk.SendMessage(
                        62,
                        "Thou hast chosen the path of PvE. Remember, thou canst not attack any player. Thou canst perform beneficial actions on other PvE and neutral players, but not PvP players."
                    );
                    break;
                }
            }
        }
    }
}

namespace Server.Commands
{
    public class ChoosePvPCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "ChoosePvP",
                AccessLevel.Player,
                new CommandEventHandler(ChoosePvP_OnCommand)
            );
        }

        private static void ChoosePvP_OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
            if (pm != null)
            {
                if (pm.NONPK == NONPK.Null)
                {
                    pm.SendGump(new PKNONPK());
                }
                else
                {
                    pm.SendMessage(62, "You have already chosen a path");
                }
            }
        }
    }
}
