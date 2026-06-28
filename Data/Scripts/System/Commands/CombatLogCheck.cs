using System;
using System.Collections;
using Server.Commands;
using Server.Commands.Generic;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    class CombatLogCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "combatlog",
                AccessLevel.Player,
                new CommandEventHandler(ShowCombatLog)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("combatlog")]
        [Description("Shows combat log to the player.")]
        public static void ShowCombatLog(CommandEventArgs e)
        {
            Mobile m = e.Mobile;
            PlayerMobile pm = (PlayerMobile)m;

            //pm.SendMessage(pm.CombatLog[0]);

            foreach (string logEntry in pm.CombatLog)
            {
                if (string.IsNullOrEmpty(logEntry))
                {
                    // If the log entry is empty, break out of the loop
                    break;
                }
                else
                {
                    // Send the message if the log entry is not empty
                    pm.SendMessage(logEntry);
                }
            }

        }
    }
}
