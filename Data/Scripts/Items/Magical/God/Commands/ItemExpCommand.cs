//================================================//
// Created by dracana				  			  //
// Desc: Command for leveling weapons             //
//================================================//
using System;
using System.Collections;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
    public class ixpCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "itemexp",
                AccessLevel.Player,
                new CommandEventHandler(ixp_OnCommand)
            );
            CommandSystem.Register(
                "ixp",
                AccessLevel.Player,
                new CommandEventHandler(ixp_OnCommand)
            );
        }

        [Usage("itemexp")]
        [Description("Item Experience and Points.")]
        public static void ixp_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Select an item to view experience");
            e.Mobile.Target = new InternalTarget(e.Mobile);
        }

        private class InternalTarget : Target
        {
            private Mobile m_From;

            public InternalTarget(Mobile from)
                : base(2, false, TargetFlags.None)
            {
                m_From = from;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item item = (Item)targeted;

                    if (item.Parent != from && item.Parent != from.Backpack)
                    {
                        from.SendMessage("The item must be in your pack or equiped!");
                        return;
                    }

                    if (item is ILevelable)
                    {
                        if (
                            item is BaseWeapon
                            || item is BaseArmor
                            || item is BaseJewel
                            || item is BaseClothing
                        )
                        {
                            from.SendGump(
                                new ItemExperienceGump(from, item, AttributeCategory.Melee)
                            );
                        }
                        else
                        {
                            from.SendMessage("That is not a valid levelable item");
                        }
                    }
                    else
                    {
                        from.SendMessage("That item is not levelable!");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid item!");
                }
            }
        }
    }
}
