using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
    public class Quests
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "quests",
                AccessLevel.Player,
                new CommandEventHandler(MyQuests_OnCommand)
            );
        }

        [Usage("quests")]
        [Description("Opens Quest Gump.")]
        private static void MyQuests_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(QuestsGump));
            from.SendGump(new QuestsGump(from));
        }
    }
}
