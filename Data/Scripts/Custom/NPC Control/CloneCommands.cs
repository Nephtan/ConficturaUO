using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Custom.Confictura;
using Server.Custom.Confictura.CloneOfflinePlayerCharacters;

namespace Server.Commands
{
    public class CloneCommands
    {
        public static AccessLevel accessLevel = AccessLevel.Counselor;

        public static void Initialize()
        {
            CommandSystem.Register("Clone", accessLevel, new CommandEventHandler(Clone_OnCommand));
            CommandSystem.Register(
                "Control",
                accessLevel,
                new CommandEventHandler(Control_OnCommand)
            );
        }

        [Usage("Clone")]
        [Description("Assume the form of another Player or Creature.")]
        public static void Clone_OnCommand(CommandEventArgs e)
        {
            if (e == null || e.Mobile == null)
                return;

            if (GetControlItem(e.Mobile) != null)
            {
                e.Mobile.SendMessage("Return to your original body before cloning.");
                return;
            }

            e.Mobile.Target = new CloneTarget();
        }

        [Usage("Control")]
        [Description("Lets you control an NPC.")]
        private static void Control_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null)
                return;

            if (e.Arguments != null && e.Arguments.Length > 0)
            {
                from.SendMessage("Usage: [Control");
                from.SendMessage("The NoStats, NoSkills, and NoItems options are no longer supported.");
                return;
            }

            if (GetCloneItem(from) != null)
            {
                from.SendMessage("Return to your original form before controlling an NPC.");
                return;
            }

            from.SendMessage("Choose the target to control...");
            from.Target = new ControlTarget();
        }

        private class CloneTarget : Target
        {
            public CloneTarget()
                : base(-1, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                Mobile target = targeted as Mobile;

                if (target == null)
                    return;

                DoClone(from, target);
            }
        }

        private static void UpdateStaffDisguise(Mobile controller, Mobile template)
        {
            PlayerMobile player = controller as PlayerMobile;

            if (player != null)
            {
                if (template == null || player.NetState != null)
                    player.SetStaffDisguise(template);
            }
        }

        // Keeps staff-controlled bodies fed and hydrated so the command does not
        // immediately apply hunger or thirst penalties copied from NPCs.
        private static void EnsureControllerNeeds(Mobile controller)
        {
            if (controller == null)
            {
                return;
            }

            if (controller.AccessLevel >= accessLevel)
            {
                controller.Hunger = 20;
                controller.Thirst = 20;
            }
        }

        // Restores the original mobility/visibility flags for an NPC when control ends.
        private static void RestoreNpcState(ControlItem controlItem, Mobile npc)
        {
            if (controlItem == null || npc == null || npc.Deleted)
            {
                return;
            }

            npc.Hidden = controlItem.OriginalHidden;
            npc.CantWalk = controlItem.OriginalCantWalk;
            npc.Frozen = controlItem.OriginalFrozen;
            npc.Paralyzed = controlItem.OriginalParalyzed;

            if (!controlItem.OriginalHidden && npc.Hidden)
            {
                npc.RevealingAction();
            }

            if (npc.Map != null && npc.Map != Map.Internal)
            {
                npc.MoveToWorld(npc.Location, npc.Map);
            }
        }

        private class ControlTarget : Target
        {
            public ControlTarget()
                : base(-1, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                DoControl(from, targeted);
            }
        }

        /*Find the Clone item of the Mobile from*/
        public static CloneItem GetCloneItem(Mobile from)
        {
            Item result = SearchItemInCont(typeof(CloneItem), from.Backpack);

            if (result != null && result is CloneItem)
                return (CloneItem)result;
            else
                return null;
        }

        /*Find the Control item of the Mobile from*/
        public static ControlItem GetControlItem(Mobile from)
        {
            Item result = SearchItemInCont(typeof(ControlItem), from.Backpack);

            if (result != null && result is ControlItem)
                return (ControlItem)result;
            else
                return null;
        }

        private static Item SearchItemInCont(Type targetType, Container cont)
        {
            Item item;

            if (cont != null && !cont.Deleted)
            {
                for (int i = 0; i < cont.Items.Count; i++)
                {
                    item = (Item)cont.Items[i];
                    // recursively search containers
                    if (item != null && !item.Deleted)
                    {
                        if (item.GetType() == targetType)
                            return item;
                        else if (item is Container)
                            item = SearchItemInCont(targetType, (Container)item);

                        if (item != null && item.GetType() == targetType)
                            return item;
                    }
                }
            }

            return null;
        }

        private static void DoClone(Mobile from, Mobile target)
        {
            if (!ValidateCloneTarget(from, target))
                return;

            if (GetControlItem(from) != null)
            {
                from.SendMessage("Return to your original body before cloning.");
                return;
            }

            if (GetCloneItem(from) != null)
            {
                from.SendMessage("You are already cloned. Use the clone item to revert.");
                return;
            }

            if (!CanCopyMount(target, from, "clone"))
                return;

            List<Item> wornCopies;
            List<Item> packCopies;

            if (!BuildClonedInventory(target, from, "clone", out wornCopies, out packCopies))
                return;

            Mobile playerClone = DupeMobile(from);

            if (playerClone == null)
            {
                DeleteItems(wornCopies);
                DeleteItems(packCopies);
                from.SendMessage("Your current body could not be backed up.");
                return;
            }

            CopyMobileState(from, playerClone);
            ClearMobileInventory(playerClone, true);

            if (!MoveMobileInventory(from, playerClone, true))
            {
                DeleteItems(wornCopies);
                DeleteItems(packCopies);
                playerClone.Delete();
                from.SendMessage("Your current inventory could not be backed up.");
                return;
            }

            CommandLogging.WriteLine(
                from,
                "{0} {1} is cloning {2}",
                from.AccessLevel,
                CommandLogging.Format(from),
                CommandLogging.Format(target)
            );

            ClearMobileInventory(from, true);
            CopyMobileState(target, from);
            AddClonedInventory(from, wornCopies, packCopies);
            CloneMountTo(target, from, from, "clone");

            Container pack = EnsureBackpack(from);
            pack.DropItem(new CloneItem(from, playerClone));
            playerClone.Internalize();
            UpdateStaffDisguise(from, target);
        }

        public static void DoControl(Mobile from, object targeted)
        {
            if (!(from is PlayerMobile))
                return;

            ControlItem targetedControlItem = targeted as ControlItem;

            if (targetedControlItem != null)
            {
                if (targetedControlItem.Owner == from)
                    targetedControlItem.Delete();

                return;
            }

            Mobile target = targeted as Mobile;

            if (target == null)
                return;

            if (GetCloneItem(from) != null)
            {
                from.SendMessage("Return to your original form before controlling an NPC.");
                return;
            }

            if (!ValidateControlTarget(from, target))
                return;

            ControlItem controlItem = GetControlItem(from);

            if (controlItem == null)
                StartControl(from, target);
            else
                ChangeControl(target, controlItem);
        }

        private static void StartControl(Mobile from, Mobile target)
        {
            PlayerMobile playerClone = DupeMobile(from) as PlayerMobile;

            if (playerClone == null)
            {
                from.SendMessage("Your current body could not be backed up.");
                return;
            }

            Map originalMap = from.Map;
            Point3D originalLocation = from.Location;
            Direction originalDirection = from.Direction;
            bool originalHidden = target.Hidden;
            bool originalCantWalk = target.CantWalk;
            bool originalFrozen = target.Frozen;
            bool originalParalyzed = target.Paralyzed;

            CopyMobileState(from, playerClone);
            ClearMobileInventory(playerClone, true);

            if (!MoveMobileInventory(from, playerClone, true))
            {
                playerClone.Delete();
                from.SendMessage("Your current inventory could not be backed up.");
                return;
            }

            CopyMobileState(target, from);
            from.Hidden = originalHidden;

            if (!MoveMobileInventory(target, from, true))
            {
                MoveMobileInventory(from, target, true);
                CopyMobileState(playerClone, from);
                MoveMobileInventory(playerClone, from, true);
                playerClone.Delete();
                from.SendMessage("The target inventory could not be moved safely.");
                return;
            }

            EnsureControllerNeeds(from);

            ControlItem controlItem = new ControlItem(from, playerClone, target)
            {
                OriginalMap = originalMap,
                OriginalLocation = originalLocation,
                OriginalDirection = originalDirection,
                OriginalHidden = originalHidden,
                OriginalCantWalk = originalCantWalk,
                OriginalFrozen = originalFrozen,
                OriginalParalyzed = originalParalyzed
            };

            EnsureBackpack(from).DropItem(controlItem);
            target.Internalize();
            playerClone.Internalize();
            UpdateStaffDisguise(from, target);

            from.SendMessage("You possess the mortal body of {0}, {1}", target.Name, target.Title);
        }

        private static void ChangeControl(Mobile target, ControlItem controlItem)
        {
            Mobile from = controlItem.Owner;
            Mobile oldNPC = controlItem.NPC;

            if (from == null || from.Deleted)
                return;

            if (target == controlItem.Player)
            {
                controlItem.Delete();
                return;
            }

            if (!ValidateControlTarget(from, target))
                return;

            RestoreControlledNpc(controlItem, from, oldNPC);

            bool originalHidden = target.Hidden;
            bool originalCantWalk = target.CantWalk;
            bool originalFrozen = target.Frozen;
            bool originalParalyzed = target.Paralyzed;

            CopyMobileState(target, from);
            from.Hidden = originalHidden;

            if (!MoveMobileInventory(target, from, true))
            {
                from.SendMessage("The target inventory could not be moved safely.");
                return;
            }

            controlItem.NPC = target;
            controlItem.Stats = true;
            controlItem.Skills = true;
            controlItem.Items = true;
            controlItem.OriginalHidden = originalHidden;
            controlItem.OriginalCantWalk = originalCantWalk;
            controlItem.OriginalFrozen = originalFrozen;
            controlItem.OriginalParalyzed = originalParalyzed;

            EnsureControllerNeeds(from);
            target.Internalize();
            UpdateStaffDisguise(from, target);

            from.SendMessage("You possess {0}, {1}", target.Name, target.Title);
        }

        public static void EndControl(ControlItem controlItem, bool stats, bool skills, bool items)
        {
            if (controlItem == null)
                return;

            Mobile from = controlItem.Owner;
            PlayerMobile oldPlayer = controlItem.Player;
            Mobile oldNPC = controlItem.NPC;

            if (from == null || from.Deleted)
            {
                RestoreOrDeleteNpc(controlItem, oldNPC);
                return;
            }

            RestoreControlledNpc(controlItem, from, oldNPC);

            if (oldPlayer != null && !oldPlayer.Deleted)
            {
                RestorePlayerFromBackup(from, oldPlayer, controlItem.OriginalMap, controlItem.OriginalLocation, controlItem.OriginalDirection);
                oldPlayer.Delete();
            }
            else
            {
                from.SendMessage("Your original body backup was missing. Staff disguise was cleared, but manual review is required.");
            }

            UpdateStaffDisguise(from, null);
            from.SendMessage("You are in your original body.");
        }

        public static void EndClone(CloneItem cloneItem)
        {
            if (cloneItem == null)
                return;

            Mobile from = cloneItem.Owner;
            Mobile oldPlayer = cloneItem.Player;

            if (from == null || from.Deleted)
                return;

            if (oldPlayer != null && !oldPlayer.Deleted)
            {
                Map currentMap = from.Map;
                Point3D currentLocation = from.Location;
                Direction currentDirection = from.Direction;

                CopyMobileState(oldPlayer, from);
                ClearMobileInventory(from, true);
                MoveMobileInventory(oldPlayer, from, true);

                if (currentMap != null && currentMap != Map.Internal)
                    from.MoveToWorld(currentLocation, currentMap);

                from.Direction = currentDirection;
                oldPlayer.Delete();
            }
            else
            {
                from.SendMessage("Your original body backup was missing. Staff disguise was cleared, but manual review is required.");
            }

            UpdateStaffDisguise(from, null);
            from.SendMessage("You return to your original form.");
        }

        // Return true if base.OnBeforeDeath should execute, false if control cleanup handled it.
        public static bool UncontrolDeath(Mobile from)
        {
            if (from == null || from.AccessLevel < accessLevel)
                return true;

            ControlItem controlItem = GetControlItem(from);

            if (controlItem == null)
                return true;

            Mobile npc = controlItem.NPC;

            controlItem.Delete();

            from.Hits = from.HitsMax;
            from.Stam = from.StamMax;
            from.Mana = from.ManaMax;

            if (npc != null && !npc.Deleted)
                npc.Kill();
            else
                from.SendMessage("The controlled NPC was already missing during death cleanup.");

            return false;
        }

        private static bool ValidateCloneTarget(Mobile from, Mobile target)
        {
            if (!ValidateSharedTarget(from, target, "clone"))
                return false;

            if (from == target)
            {
                from.SendMessage("You cannot clone yourself.");
                return false;
            }

            if (from.AccessLevel <= target.AccessLevel)
            {
                from.SendMessage("You can only clone targets below your access level.");
                return false;
            }

            if (!CanCopyMount(target, from, "clone"))
                return false;

            return true;
        }

        private static bool ValidateControlTarget(Mobile from, Mobile target)
        {
            if (!ValidateSharedTarget(from, target, "control"))
                return false;

            if (target is PlayerMobile)
            {
                from.SendMessage("You can't control players.");
                return false;
            }

            if (from == target)
            {
                from.SendMessage("You cannot control yourself.");
                return false;
            }

            return true;
        }

        private static bool ValidateSharedTarget(Mobile from, Mobile target, string action)
        {
            if (from == null || from.Deleted || target == null)
                return false;

            if (target.Deleted || target.Map == null || target.Map == Map.Internal)
            {
                from.SendMessage("That target cannot be used for {0}.", action);
                return false;
            }

            if (target is CharacterClone || target is MountClone)
            {
                from.SendMessage("Clone-system mobiles cannot be used for {0}.", action);
                return false;
            }

            BaseMount mount = target as BaseMount;

            if (mount != null && mount.Rider != null)
            {
                from.SendMessage("You cannot {0} a ridden mount.", action);
                return false;
            }

            return true;
        }

        private static bool CanCopyMount(Mobile source, Mobile actor, string action)
        {
            if (source == null || !source.Mounted || source.Mount == null)
                return true;

            if (source.Mount is BaseMount || source.Mount is EtherealMount)
                return true;

            if (actor != null)
                actor.SendMessage(
                    "The {0} target has an unsupported mount type: {1}.",
                    action,
                    source.Mount.GetType().Name
                );

            return false;
        }

        private static void CopyMobileState(Mobile source, Mobile target)
        {
            if (source == null || target == null)
                return;

            CloneThings.CloneMobileProperties(source, target);

            if (source is PlayerMobile || source.Player)
                target.BodyMod = 0;
            else
                target.BodyMod = source.Body;
        }

        private static void RestoreControlledNpc(ControlItem controlItem, Mobile from, Mobile npc)
        {
            if (npc != null && !npc.Deleted)
            {
                CopyMobileState(from, npc);
                MoveMobileInventory(from, npc, true);
                RestoreNpcState(controlItem, npc);
            }
            else
            {
                if (from != null && !from.Deleted)
                {
                    from.SendMessage("The original NPC was deleted or missing.");
                    ClearMobileInventory(from, true);
                }

                RestoreOrDeleteNpc(controlItem, npc);
            }
        }

        private static void RestoreOrDeleteNpc(ControlItem controlItem, Mobile npc)
        {
            if (npc == null || npc.Deleted)
                return;

            RestoreNpcState(controlItem, npc);
        }

        private static void RestorePlayerFromBackup(
            Mobile from,
            Mobile oldPlayer,
            Map originalMap,
            Point3D originalLocation,
            Direction originalDirection
        )
        {
            CopyMobileState(oldPlayer, from);
            ClearMobileInventory(from, true);
            MoveMobileInventory(oldPlayer, from, true);

            if (originalMap != null && originalMap != Map.Internal)
                from.MoveToWorld(originalLocation, originalMap);

            from.Direction = originalDirection;
        }

        private static Container EnsureBackpack(Mobile mobile)
        {
            Container pack = mobile.Backpack as Container;

            if (pack == null || pack.Deleted)
            {
                pack = new Backpack();
                mobile.AddItem(pack);
            }

            return pack;
        }

        private static bool MoveMobileInventory(Mobile source, Mobile target, bool skipControlArtifacts)
        {
            if (source == null || target == null || source == target)
                return true;

            try
            {
                ClearMobileInventory(target, skipControlArtifacts);

                List<Item> sourceItems = new List<Item>(source.Items);

                foreach (Item item in sourceItems)
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

                    if (skipControlArtifacts && IsControlArtifact(item))
                        continue;

                    target.AddItem(item);
                }

                Container sourcePack = source.Backpack as Container;

                if (sourcePack != null && !sourcePack.Deleted)
                {
                    Container targetPack = EnsureBackpack(target);
                    List<Item> packItems = new List<Item>(sourcePack.Items);

                    foreach (Item item in packItems)
                    {
                        if (item == null || item.Deleted)
                            continue;

                        if (skipControlArtifacts && IsControlArtifact(item))
                            continue;

                        targetPack.DropItem(item);
                    }
                }

                return MoveMount(source, target);
            }
            catch (Exception ex)
            {
                Console.WriteLine("NPC Control inventory move failed: {0}", ex);
                return false;
            }
        }

        private static void ClearMobileInventory(Mobile mobile, bool skipControlArtifacts)
        {
            if (mobile == null || mobile.Deleted)
                return;

            List<Item> items = new List<Item>(mobile.Items);

            foreach (Item item in items)
            {
                if (
                    item == null
                    || item.Deleted
                    || item.Parent != mobile
                    || item == mobile.Backpack
                    || item is IMount
                )
                {
                    continue;
                }

                if (skipControlArtifacts && IsControlArtifact(item))
                    continue;

                item.Delete();
            }

            Container pack = mobile.Backpack as Container;

            if (pack != null && !pack.Deleted)
            {
                List<Item> packItems = new List<Item>(pack.Items);

                foreach (Item item in packItems)
                {
                    if (item == null || item.Deleted)
                        continue;

                    if (skipControlArtifacts && IsControlArtifact(item))
                        continue;

                    item.Delete();
                }
            }

            DeleteMount(mobile);
        }

        private static bool MoveMount(Mobile source, Mobile target)
        {
            IMount mount = source.Mount;

            if (mount == null)
                return true;

            try
            {
                mount.Rider = null;
                mount.Rider = target;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("NPC Control mount move failed: {0}", ex);

                try
                {
                    mount.Rider = source;
                }
                catch
                {
                }

                return false;
            }
        }

        private static void DeleteMount(Mobile mobile)
        {
            if (mobile == null || mobile.Mount == null)
                return;

            IMount mount = mobile.Mount;
            mount.Rider = null;

            Item mountItem = mount as Item;
            if (mountItem != null && !mountItem.Deleted)
            {
                mountItem.Delete();
                return;
            }

            Mobile mountMobile = mount as Mobile;
            if (mountMobile != null && !mountMobile.Deleted)
                mountMobile.Delete();
        }

        private static bool BuildClonedInventory(
            Mobile source,
            Mobile actor,
            string action,
            out List<Item> wornCopies,
            out List<Item> packCopies
        )
        {
            wornCopies = new List<Item>();
            packCopies = new List<Item>();

            try
            {
                List<Item> sourceItems = new List<Item>(source.Items);

                foreach (Item item in sourceItems)
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

                    Item clone;
                    if (!TryCloneItemTree(item, actor, action, out clone))
                    {
                        DeleteItems(wornCopies);
                        DeleteItems(packCopies);
                        return false;
                    }

                    if (clone != null)
                        wornCopies.Add(clone);
                }

                Container pack = source.Backpack as Container;

                if (pack != null && !pack.Deleted)
                {
                    List<Item> packItems = new List<Item>(pack.Items);

                    foreach (Item item in packItems)
                    {
                        if (item == null || item.Deleted)
                            continue;

                        Item clone;
                        if (!TryCloneItemTree(item, actor, action, out clone))
                        {
                            DeleteItems(wornCopies);
                            DeleteItems(packCopies);
                            return false;
                        }

                        if (clone != null)
                            packCopies.Add(clone);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("NPC Control inventory clone failed: {0}", ex);
                DeleteItems(wornCopies);
                DeleteItems(packCopies);
                return false;
            }
        }

        private static bool TryCloneItemTree(Item item, Mobile actor, string action, out Item clone)
        {
            clone = null;

            if (item == null || item.Deleted)
                return true;

            if (IsControlArtifact(item))
                return true;

            clone = CloneThings.CloneItem(item);

            if (clone == null)
            {
                ReportCloneFailure(actor, action, item);
                return false;
            }

            Container sourceContainer = item as Container;
            Container targetContainer = clone as Container;

            if (sourceContainer != null && targetContainer != null)
            {
                List<Item> items = new List<Item>(sourceContainer.Items);

                foreach (Item child in items)
                {
                    Item childClone;

                    if (!TryCloneItemTree(child, actor, action, out childClone))
                    {
                        clone.Delete();
                        clone = null;
                        return false;
                    }

                    if (childClone != null)
                        targetContainer.DropItem(childClone);
                }
            }

            return true;
        }

        private static void AddClonedInventory(
            Mobile target,
            List<Item> wornCopies,
            List<Item> packCopies
        )
        {
            foreach (Item item in wornCopies)
            {
                if (item != null && !item.Deleted)
                    target.AddItem(item);
            }

            if (packCopies.Count > 0)
            {
                Container pack = EnsureBackpack(target);

                foreach (Item item in packCopies)
                {
                    if (item != null && !item.Deleted)
                        pack.DropItem(item);
                }
            }
        }

        private static void CloneMountTo(Mobile source, Mobile target, Mobile actor, string action)
        {
            if (source == null || !source.Mounted)
                return;

            try
            {
                CloneThings.CloneMobileMount(source, target);
            }
            catch (Exception ex)
            {
                Console.WriteLine("NPC Control mount clone failed: {0}", ex);

                if (actor != null)
                    actor.SendMessage("The {0} target's mount could not be cloned.", action);
            }
        }

        private static void DeleteItems(List<Item> items)
        {
            if (items == null)
                return;

            foreach (Item item in items)
            {
                if (item != null && !item.Deleted)
                    item.Delete();
            }
        }

        private static bool IsControlArtifact(Item item)
        {
            return item is CloneItem
                || item is ControlItem
                || item is BackpackClone
                || item is EtherealMountClone;
        }

        private static void ReportCloneFailure(Mobile actor, string action, Item item)
        {
            string itemName = item.Name;

            if (itemName == null || itemName.Length == 0)
                itemName = item.GetType().Name;

            Console.WriteLine(
                "NPC Control {0} aborted; unable to clone {1} ({2}).",
                action,
                itemName,
                item.GetType().FullName
            );

            if (actor != null)
            {
                actor.SendMessage(
                    "Cannot safely {0} because {1} ({2}) could not be copied.",
                    action,
                    itemName,
                    item.GetType().Name
                );
            }
        }

        //With items for DupeCommand?
        public static Mobile DupeMobile(object mobile)
        {
            Type t = mobile.GetType();
            object o = Construct(t);

            if (o == null)
            {
                Console.WriteLine(
                    "Unable to dupe {0}. Mobile must have a 0 parameter constructor.",
                    t.Name
                );
                return null;
            }

            if (o is Mobile)
            {
                Mobile newMobile = (Mobile)o;
                Mobile srcMobile = (Mobile)mobile;
                newMobile.AccessLevel = srcMobile.AccessLevel;

                newMobile.Player = false;
                newMobile.UpdateTotals();
                return newMobile;
            }

            return null;
        }

        private static object Construct(Type type, params object[] constructParams)
        {
            bool constructed = false;
            object toReturn = null;
            ConstructorInfo[] info = type.GetConstructors();

            foreach (ConstructorInfo c in info)
            {
                if (constructed)
                    break;
                ParameterInfo[] paramInfo = c.GetParameters();

                if (paramInfo.Length == constructParams.Length)
                {
                    try
                    {
                        object o = c.Invoke(constructParams);

                        if (o != null)
                        {
                            constructed = true;
                            toReturn = o;
                        }
                    }
                    catch
                    {
                        toReturn = null;
                    }
                }
            }
            return toReturn;
        }

    }
}

namespace Server.Custom.Confictura
{
    /// <summary>
    /// Item used to restore a mobile after using the Clone command.
    /// </summary>
    public class CloneItem : Item
    {
        private Mobile m_Owner;
        private Mobile m_Player;
        private bool m_Restoring;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return m_Owner; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Player
        {
            get { return m_Player; }
        }

        public CloneItem(Mobile owner, Mobile player)
            : base(0x2106)
        {
            m_Owner = owner;
            m_Player = player;

            Name = "Clone Item";
            LootType = LootType.Blessed;
        }

        public CloneItem(Serial serial)
            : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == m_Owner)
            {
                Delete();
                return;
            }

            base.OnDoubleClick(from);
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (RootParent != m_Owner)
                Delete();
        }

        public override bool DropToWorld(Mobile from, Point3D p)
        {
            Delete();

            return false;
        }

        public override void OnDelete()
        {
            if (m_Restoring)
            {
                base.OnDelete();
                return;
            }

            m_Restoring = true;

            // Remove the item from its container before processing the end of clone
            // to avoid recursive deletion when the player's backpack is cleared.
            Container container = Parent as Container;
            if (container != null)
            {
                container.RemoveItem(this);
            }

            CloneCommands.EndClone(this);

            base.OnDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((Mobile)m_Owner);
            writer.Write((Mobile)m_Player);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_Player = reader.ReadMobile();
        }
    }
}

namespace Server.Items
{
    public class ControlItem : Item
    {
        private Mobile m_Owner;
        private Mobile m_Player;
        private Mobile m_NPC;
        private bool m_Restoring;

        private bool m_Stats;
        private bool m_Skills;
        private bool m_Items;
        private bool m_OriginalHidden;
        private bool m_OriginalCantWalk;
        private bool m_OriginalFrozen;
        private bool m_OriginalParalyzed;

        private Map m_Map;
        private Point3D m_Location;
        private Direction m_Direction;

        [CommandProperty(AccessLevel.GameMaster)]
        public PlayerMobile Owner
        {
            get
            {
                if (m_Owner is PlayerMobile)
                    return (PlayerMobile)m_Owner;
                else
                    return null;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public PlayerMobile Player
        {
            get
            {
                if (m_Player is PlayerMobile)
                    return (PlayerMobile)m_Player;
                else
                    return null;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile NPC
        {
            get { return m_NPC; }
            set { m_NPC = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Stats
        {
            get { return m_Stats; }
            set { m_Stats = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Skills
        {
            get { return m_Skills; }
            set { m_Skills = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public new bool Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map OriginalMap
        {
            get { return m_Map; }
            set { m_Map = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D OriginalLocation
        {
            get { return m_Location; }
            set { m_Location = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Direction OriginalDirection
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool OriginalHidden
        {
            get { return m_OriginalHidden; }
            set { m_OriginalHidden = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool OriginalCantWalk
        {
            get { return m_OriginalCantWalk; }
            set { m_OriginalCantWalk = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool OriginalFrozen
        {
            get { return m_OriginalFrozen; }
            set { m_OriginalFrozen = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool OriginalParalyzed
        {
            get { return m_OriginalParalyzed; }
            set { m_OriginalParalyzed = value; }
        }

        public ControlItem(
            Mobile owner,
            Mobile player,
            Mobile npc,
            bool stats,
            bool skills,
            bool items
        )
            : base(0x2106)
        {
            m_Owner = owner;
            m_Player = player;
            m_NPC = npc;

            m_Stats = stats;
            m_Skills = skills;
            m_Items = items;

            Name = "Control Item";
            LootType = LootType.Blessed;
        }

        public ControlItem(Mobile owner, Mobile player, Mobile npc)
            : base(0x2106)
        {
            m_Owner = owner;
            m_Player = player;
            m_NPC = npc;

            m_Stats = true;
            m_Skills = true;
            m_Items = true;

            Name = "Control Item";
            LootType = LootType.Blessed;
        }

        public ControlItem(Serial serial)
            : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == m_Owner)
            {
                Delete();
                return;
            }

            base.OnDoubleClick(from);
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (RootParent != m_Owner)
                Delete();
        }

        public override bool DropToWorld(Mobile from, Point3D p)
        {
            Delete();

            return false;
            //return base.DropToWorld( from, p );
        }

        public override void OnDelete()
        {
            if (m_Restoring)
            {
                base.OnDelete();
                return;
            }

            m_Restoring = true;

            // Detach the control item from its container before ending control
            // to prevent recursive deletion when the owner's backpack is cleared.
            Container container = Parent as Container;
            if (container != null)
            {
                container.RemoveItem(this);
            }

            CloneCommands.EndControl(this, m_Stats, m_Skills, m_Items);

            base.OnDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)4); // version

            // Version 2
            writer.Write(m_Map);
            writer.Write(m_Location);
            writer.Write((byte)m_Direction);

            // Version 1
            writer.Write((bool)m_Stats);
            writer.Write((bool)m_Skills);
            writer.Write((bool)m_Items);

            // Version 0
            writer.Write((Mobile)m_Owner);
            writer.Write((Mobile)m_Player);
            writer.Write((Mobile)m_NPC);

            // Version 3
            writer.Write(m_OriginalHidden);
            writer.Write(m_OriginalCantWalk);
            writer.Write(m_OriginalFrozen);

            // Version 4
            writer.Write(m_OriginalParalyzed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 2)
            {
                m_Map = reader.ReadMap();
                m_Location = reader.ReadPoint3D();
                m_Direction = (Direction)reader.ReadByte();
            }

            if (version >= 1)
            {
                m_Stats = reader.ReadBool();
                m_Skills = reader.ReadBool();
                m_Items = reader.ReadBool();
            }

            if (version >= 0)
            {
                m_Owner = reader.ReadMobile();
                m_Player = reader.ReadMobile();
                m_NPC = reader.ReadMobile();
            }

            if (version >= 3)
            {
                m_OriginalHidden = reader.ReadBool();
                m_OriginalCantWalk = reader.ReadBool();
                m_OriginalFrozen = reader.ReadBool();
            }

            if (version >= 4)
            {
                m_OriginalParalyzed = reader.ReadBool();
            }
        }
    }
}
