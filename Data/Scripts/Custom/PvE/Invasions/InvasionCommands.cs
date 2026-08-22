using Server.Commands;

namespace Server.Custom.Confictura.Invasions
{
    public static class InvasionCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("Invasion", AccessLevel.Administrator, Invasion_OnCommand);
            CommandSystem.Register("InvasionInfo", AccessLevel.Player, InvasionInfo_OnCommand);
        }

        [Usage("Invasion")]
        [Description("Opens the Organic City Invasions administration console.")]
        private static void Invasion_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || e.Mobile.Deleted || e.Mobile.AccessLevel < AccessLevel.Administrator)
                return;

            e.Mobile.SendGump(new Server.Custom.Confictura.Gumps.Invasions.InvasionAdminGump(e.Mobile, false, false));
        }

        [Usage("InvasionInfo")]
        [Description("Shows the current Organic City Invasions status.")]
        private static void InvasionInfo_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || e.Mobile.Deleted)
                return;

            e.Mobile.SendGump(new Server.Custom.Confictura.Gumps.Invasions.InvasionInfoGump(e.Mobile));
        }
    }
}
