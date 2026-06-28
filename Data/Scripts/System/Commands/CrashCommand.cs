using System;
using Server;
using Server.Accounting;

namespace Server.Commands
{
    // Staff-only diagnostic command used to validate crash handling and auto-restart behavior.
    public static class CrashCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Crash", AccessLevel.Administrator, Crash_OnCommand);
        }

        [Usage("Crash")]
        [Description("Intentionally crashes the server process to test crash recovery and auto-restart.")]
        private static void Crash_OnCommand(CommandEventArgs e)
        {
            Mobile mobile = e.Mobile;
            string mobileName = mobile != null ? mobile.Name : "Unknown";
            string accountName = "Unknown";

            Account account = mobile != null ? mobile.Account as Account : null;
            if (account != null)
                accountName = account.Username;

            Console.WriteLine(
                "Crash: Diagnostic crash requested by {0} (Account: {1}).",
                mobileName,
                accountName
            );

            throw new InvalidOperationException("Intentional crash triggered by [Crash] command.");
        }
    }
}
