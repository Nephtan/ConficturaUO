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
            if (e == null || e.Mobile == null || e.Mobile.Deleted)
                return;

            Mobile from = e.Mobile;

            from.SendGump(new AdminAddGump(0, from, new ArrayList(), new ArrayList()));
        }
    }
}
