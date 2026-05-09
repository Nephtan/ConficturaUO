using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Custom.Confictura;
using Server.Custom.Confictura.CloneOfflinePlayerCharacters;
using Server.Items;
using Server.Mobiles;

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
            if (e == null || e.Mobile == null || e.Mobile.Deleted)
                return;

            Mobile mobile = e.Mobile;

            if (CloneCommands.GetCloneItem(mobile) != null || CloneCommands.GetControlItem(mobile) != null)
            {
                mobile.SendMessage("Return to your original body before using CloneMe.");
                return;
            }

            BaseCreature clone = null;

            try
            {
                clone = CloneThings.CreateClone(mobile);

                if (clone == null)
                {
                    mobile.SendMessage("A clone could not be created.");
                    return;
                }

                CloneThings.CloneMobileProperties(mobile, clone);
                int skippedItems;
                CloneMobileInventory(mobile, clone, out skippedItems);
                CloneMobileMount(mobile, clone, ref skippedItems);

                if (skippedItems > 0)
                {
                    mobile.SendMessage(
                        "{0} item{1} could not be copied into the CloneMe body.",
                        skippedItems,
                        skippedItems == 1 ? "" : "s"
                    );
                }
            }
            catch (Exception ex)
            {
                if (clone != null && !clone.Deleted)
                    clone.Delete();

                Console.WriteLine("CloneMe failed for {0}: {1}", mobile, ex);
                mobile.SendMessage("CloneMe failed while copying your body.");
                return;
            }

            clone.Hidden = false;
            mobile.Hidden = true;
        }

        private static void CloneMobileInventory(Mobile source, Mobile target, out int skippedItems)
        {
            skippedItems = 0;

            if (source == null || target == null)
                return;

            List<Item> wornItems = new List<Item>(source.Items);

            foreach (Item item in wornItems)
            {
                if (
                    item == null
                    || item.Deleted
                    || item.Parent != source
                    || item == source.Backpack
                    || item is IMount
                )
                {
                    continue;
                }

                Item clone = CloneItemForCloneMe(item, ref skippedItems);

                if (clone != null)
                    target.AddItem(clone);
            }

            Container pack = source.Backpack as Container;

            if (pack != null && !pack.Deleted)
            {
                BackpackClone clonedBackpack = new BackpackClone();
                target.AddItem(clonedBackpack);
                CloneContainerContents(pack, clonedBackpack, ref skippedItems);
            }
        }

        private static void CloneContainerContents(Container source, Container target, ref int skippedItems)
        {
            List<Item> items = new List<Item>(source.Items);

            foreach (Item item in items)
            {
                Item clone = CloneItemForCloneMe(item, ref skippedItems);

                if (clone != null)
                    target.DropItem(clone);
            }
        }

        private static Item CloneItemForCloneMe(Item item, ref int skippedItems)
        {
            if (item == null || item.Deleted)
                return null;

            if (IsSystemCloneItem(item))
            {
                skippedItems++;
                return null;
            }

            Item clone = CloneThings.CloneItem(item);

            if (clone == null)
            {
                skippedItems++;
                Console.WriteLine(
                    "CloneMe skipped unsupported item {0} ({1}).",
                    item,
                    item.GetType().FullName
                );
                return null;
            }

            Container sourceContainer = item as Container;
            Container targetContainer = clone as Container;

            if (sourceContainer != null && targetContainer != null)
                CloneContainerContents(sourceContainer, targetContainer, ref skippedItems);

            return clone;
        }

        private static void CloneMobileMount(Mobile source, Mobile target, ref int skippedItems)
        {
            if (source == null || !source.Mounted || source.Mount == null)
                return;

            if (source.Mount is MountClone || source.Mount is EtherealMountClone)
            {
                skippedItems++;
                return;
            }

            CloneThings.CloneMobileMount(source, target);
        }

        private static bool IsSystemCloneItem(Item item)
        {
            return item is CloneItem
                || item is ControlItem
                || item is BackpackClone
                || item is EtherealMountClone;
        }
    }
}
