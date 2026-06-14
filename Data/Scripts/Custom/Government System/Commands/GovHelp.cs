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
    public class GovHelpCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "GovHelp",
                AccessLevel.Player,
                new CommandEventHandler(GovHelp_OnCommand)
            );
        }

        [Usage("GovHelp")]
        [Description("Displaies Help Menu For FS Government System")]
        private static void GovHelp_OnCommand(CommandEventArgs e)
        {
            if (e == null || e.Mobile == null || e.Mobile.Deleted)
                return;

            e.Mobile.SendGump(new GovHelpGump());
        }
    }
}
