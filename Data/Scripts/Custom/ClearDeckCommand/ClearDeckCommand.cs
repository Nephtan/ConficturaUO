using System.Collections.Generic;
using System.ComponentModel;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Multis;

namespace Server.Commands
{
    public class ClearDeckCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "ClearDeck",
                AccessLevel.Player,
                new CommandEventHandler(ClearDeck_OnCommand)
            );
        }

        [Usage("ClearDeck")]
        [Description("Clears the deck of the boat the player is on.")]
        public static void ClearDeck_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            BaseBoat playerBoat = BaseBoat.FindBoatAt(from, from.Map);

            if (playerBoat == null)
            {
                from.SendMessage("You must be on a boat to use this command.");
                return;
            }

            List<Corpse> corpsesToDelete = new List<Corpse>();

            // Iterate through all items in range
            foreach (Item item in playerBoat.GetItemsInRange(18)) // Assuming 18 is the max range of a boat
            {
                if (item is Corpse)
                {
                    Corpse corpse = (Corpse)item;
                    // Check if the corpse is on the same boat as the player
                    BaseBoat corpseBoat = BaseBoat.FindBoatAt(corpse, corpse.Map);

                    if (corpseBoat == playerBoat)
                    {
                        if (
                            !(corpse.Owner is PlayerMobile)
                            || (corpse.Owner is PlayerMobile && corpse.Items.Count == 0)
                        )
                        {
                            corpsesToDelete.Add(corpse);
                        }
                    }
                }
            }

            // Delete the corpses
            foreach (Corpse corpse in corpsesToDelete)
            {
                corpse.Delete();
            }

            from.SendMessage("Deck cleared.");
        }
    }
}
