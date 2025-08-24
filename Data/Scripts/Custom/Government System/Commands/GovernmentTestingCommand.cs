using System;
using Server.Commands;
using Server.Mobiles;

namespace Server.Custom.Confictura
{
    /// <summary>
    /// GM command for managing the Government Testing Mode. Allows staff to
    /// check status, enable the mode with a multiplier and disable it when
    /// finished.
    /// </summary>
    public static class GovernmentTestingCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "GovTesting",
                AccessLevel.GameMaster,
                new CommandEventHandler(OnCommand)
            );
        }

        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (e.Length == 0)
            {
                from.SendMessage(
                    "Government testing mode is {0} (multiplier {1}).",
                    GovernmentTestingMode.Enabled ? "enabled" : "disabled",
                    GovernmentTestingMode.Multiplier
                );

                return;
            }

            string arg = e.GetString(0).ToLower();

            if (arg == "on" || arg == "enable")
            {
                double mult = 1.0;

                if (e.Length > 1)
                {
                    mult = e.GetDouble(1);
                }

                if (mult <= 0)
                {
                    from.SendMessage("Multiplier must be greater than zero.");
                    return;
                }

                GovernmentTestingMode.Enabled = true;
                GovernmentTestingMode.Multiplier = mult;

                from.SendMessage(
                    "Government testing mode enabled. Speed multiplier set to {0}.",
                    mult
                );
            }
            else if (arg == "off" || arg == "disable")
            {
                GovernmentTestingMode.Enabled = false;
                GovernmentTestingMode.Multiplier = 1.0;

                from.SendMessage("Government testing mode disabled.");
            }
            else
            {
                from.SendMessage("Usage: [GovTesting [on <multiplier>|off]");
            }
        }
    }
}