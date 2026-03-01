using System;
using Server.Commands;
using Server.Mobiles;
using Server.Custom.Confictura.CloneOfflinePlayerCharacters;

namespace Server.Commands
{
    public class CloneMeCommand
    {
        public static AccessLevel accessLevel = AccessLevel.Counselor;

        public static void Initialize()
        {
            CommandSystem.Register(
                "CloneMe",
                accessLevel,
                new CommandEventHandler(CloneMe_OnCommand)
            );
        }

        [Usage("CloneMe")]
        [Description("Makes an exact duplicate of you at your present location and hides you")]
        public static void CloneMe_OnCommand(CommandEventArgs e)
        {
            Mobile mobile = e.Mobile;

            // Create and populate a clone using the advanced cloning utilities
            BaseCreature m = CloneThings.CreateClone(mobile);
            CloneThings.CloneMobileProperties(mobile, m);
            CloneThings.CloneMobileItems(mobile, m);
            CloneThings.CloneMobileBackpack(mobile, m);
            CloneThings.CloneMobileMount(mobile, m);

            // Reveal the clone and hide the original player
            m.Hidden = false;
            mobile.Hidden = true;
        }
    }
}
