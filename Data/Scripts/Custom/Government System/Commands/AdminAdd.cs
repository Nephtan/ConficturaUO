using System;
using System.Collections;
using Server;
using Server.Commands.Generic;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class AdminAddCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AdminAdd",
                AccessLevel.GameMaster,
                new CommandEventHandler(AdminAdd_OnCommand)
            );
        }

        [Usage("AdminAdd")]
        [Description("Calls AdminAddGump.")]
        private static void AdminAdd_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new AdminAddGump(0, e.Mobile, new ArrayList(), new ArrayList()));
        }
    }
}
