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
    public class FindCitiesCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "FindCities",
                AccessLevel.GameMaster,
                new CommandEventHandler(FindCities_OnCommand)
            );
        }

        [Usage("FindCities")]
        [Description("Locates all cities on all facets.")]
        private static void FindCities_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new FindCitiesGump(0, null, null));
        }
    }
}
