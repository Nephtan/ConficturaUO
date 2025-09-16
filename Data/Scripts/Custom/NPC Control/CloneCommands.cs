using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Confictura.Custom;
using Server.Custom.Confictura;

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
            e.Mobile.Target = new CloneTarget();
        }

        [Usage("Control [target]")]
        [Description("Let's you control an NPC")]
        private static void Control_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            e.GetInt32(0);

            if (from != null)
            {
                from.SendMessage("Choose the target to control...");
                from.Target = new ControlTarget(e.Arguments);
            }
        }

        private class CloneTarget : Target
        {
            bool real = true;
            bool location = false;

            public CloneTarget()
                : base(-1, false, TargetFlags.None) { }

            public void SimulateTarget(Mobile from, object targeted, bool loc)
            {
                real = false;
                location = loc;
                OnTarget(from, targeted);
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile targ = (Mobile)targeted;

                    if (from != targ && (!real || from.AccessLevel > targ.AccessLevel))
                    {
                        CloneItem cloneItem = null;

                        if (real)
                        {
                            CloneItem existing = GetCloneItem(from);

                            if (existing != null)
                            {
                                from.SendMessage("You are already cloned. Use the clone item to revert.");
                                return;
                            }

                            Mobile playerClone = DupeMobile(from);
                            new CloneTarget().SimulateTarget(playerClone, from, false);

                            Container pack = from.Backpack as Container;
                            if (pack == null)
                            {
                                pack = new Backpack();
                                from.AddItem(pack);
                            }

                            // Defer dropping the clone item until after the backpack is cleared
                            cloneItem = new CloneItem(from, playerClone);
                        }

                        CommandLogging.WriteLine(
                            from,
                            "{0} {1} is cloning {2}",
                            from.AccessLevel,
                            CommandLogging.Format(from),
                            CommandLogging.Format(targ)
                        );

                        // Clone mobile stats, skills, and appearance using advanced logic
                        CloneThings.CloneMobileProperties(targ, from);

                        if (location)
                        {
                            from.Location = targ.Location;
                            from.Direction = targ.Direction;
                            from.Map = targ.Map;
                        }

                        // Adjust body mod for non-player targets
                        if (!targ.Player)
                            from.BodyMod = targ.Body;
                        else
                            from.BodyMod = 0;

                        // Remove existing worn items
                        List<Item> wornItems = new List<Item>(from.Items);
                        foreach (Item item in wornItems)
                        {
                            if (item != null && item.Parent == from && item != from.Backpack)
                                item.Delete();
                        }

                        // Clear current backpack contents
                        Container fromPack = from.Backpack as Container;
                        if (fromPack == null)
                        {
                            fromPack = new Backpack();
                            from.AddItem(fromPack);
                        }

                        foreach (Item item in fromPack.Items.ToArray())
                            item.Delete();

                        // Clone target's worn items
                        CloneThings.CloneMobileItems(targ, from);

                        // Clone target's backpack contents recursively
                        Container targPack = targ.Backpack as Container;
                        if (targPack != null)
                            CloneThings.CloneContainerContents(targPack, fromPack);

                        // Drop the clone item after cloning to ensure it isn't deleted
                        if (cloneItem != null)
                            fromPack.DropItem(cloneItem);

                        // Replace mount with target's mount, if any
                        if (from.Mount != null)
                        {
                            IMount oldMount = from.Mount;
                            oldMount.Rider = null;
                            if (oldMount is EtherealMount)
                                ((EtherealMount)oldMount).Delete();
                            else if (oldMount is BaseMount)
                                ((BaseMount)oldMount).Delete();
                        }

                        CloneThings.CloneMobileMount(targ, from);
                        if (!real)
                            CopyProps(from, targ, true, true, location);

                        UpdateStaffDisguise(from, targ);
                    }
                }
            }
        }

        private static void UpdateStaffDisguise(Mobile controller, Mobile template)
        {
            if (controller is PlayerMobile player)
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

        private static void CopyProperties(Item dest, Item src)
        {
            PropertyInfo[] props = src.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    if (props[i].CanRead && props[i].CanWrite)
                    {
                        // These properties must not be copied during the dupe, they get set implicitely by placing
                        // items properly using "DropItem()" etc. .
                        switch (props[i].Name)
                        {
                            case "Parent":
                            case "TotalWeight":
                            case "TotalItems":
                            case "TotalGold":
                                break;
                            default:
                                props[i].SetValue(dest, props[i].GetValue(src, null), null);
                                break;
                        }
                        // end exceptions
                    }
                }
                catch { }

                // BaseArmor, BaseClothing, BaseJewel, BaseWeapon: copy nested classes
                // ToDo: If someone knows something about dynamic casting these 4 blocks
                //       could be integrated into one...
                if (src is BaseWeapon)
                {
                    object src_obj = ((BaseWeapon)src).Attributes;
                    object dest_obj = ((BaseWeapon)dest).Attributes;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseWeapon)src).SkillBonuses;
                    dest_obj = ((BaseWeapon)dest).SkillBonuses;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseWeapon)src).WeaponAttributes;
                    dest_obj = ((BaseWeapon)dest).WeaponAttributes;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseWeapon)src).AosElementDamages;
                    dest_obj = ((BaseWeapon)dest).AosElementDamages;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);
                }
                else if (src is BaseArmor)
                {
                    object src_obj = ((BaseArmor)src).Attributes;
                    object dest_obj = ((BaseArmor)dest).Attributes;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseArmor)src).SkillBonuses;
                    dest_obj = ((BaseArmor)dest).SkillBonuses;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseArmor)src).ArmorAttributes;
                    dest_obj = ((BaseArmor)dest).ArmorAttributes;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);
                }
                else if (src is BaseJewel)
                {
                    object src_obj = ((BaseJewel)src).Attributes;
                    object dest_obj = ((BaseJewel)dest).Attributes;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseJewel)src).SkillBonuses;
                    dest_obj = ((BaseJewel)dest).SkillBonuses;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseJewel)src).Resistances;
                    dest_obj = ((BaseJewel)dest).Resistances;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);
                }
                else if (src is BaseClothing)
                {
                    object src_obj = ((BaseClothing)src).Attributes;
                    object dest_obj = ((BaseClothing)dest).Attributes;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseClothing)src).SkillBonuses;
                    dest_obj = ((BaseClothing)dest).SkillBonuses;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseClothing)src).ClothingAttributes;
                    dest_obj = ((BaseClothing)dest).ClothingAttributes;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);

                    src_obj = ((BaseClothing)src).Resistances;
                    dest_obj = ((BaseClothing)dest).Resistances;

                    if (src_obj != null && dest_obj != null)
                        CopyProperties(dest_obj, src_obj);
                }
                // end copying nested classes
            }
        }

        //Duplicates props between two items of same type
        private static void CopyProperties(object dest, object src)
        {
            PropertyInfo[] props = src.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    if (props[i].CanRead && props[i].CanWrite)
                    {
                        props[i].SetValue(dest, props[i].GetValue(src, null), null);
                    }
                }
                catch { }
            }
        }

        private class ControlTarget : Target
        {
            string[] m_parameter;

            public ControlTarget(params string[] parameter)
                : base(-1, true, TargetFlags.None)
            {
                m_parameter = parameter;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                bool stats = true;
                bool skills = true;
                bool items = true;

                for (int i = 0; i < m_parameter.Length; i++)
                    if (string.Compare(m_parameter[i], "NoStats", true) == 0)
                        stats = false;

                for (int i = 0; i < m_parameter.Length; i++)
                    if (string.Compare(m_parameter[i], "NoSkills", true) == 0)
                        skills = false;

                for (int i = 0; i < m_parameter.Length; i++)
                    if (string.Compare(m_parameter[i], "NoItems", true) == 0)
                        items = false;

                DoControl(from, targeted, stats, skills, items);
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

        public static void DoControl(
            Mobile from,
            object targeted,
            bool stats,
            bool skills,
            bool items
        )
        {
            Mobile target;

            if (from is PlayerMobile && targeted is Mobile)
            {
                // Prevent controlling player mobiles entirely
                if (targeted is PlayerMobile)
                {
                    from.SendMessage("You can't control players.");
                    return;
                }

                target = (Mobile)targeted;
                ControlItem controlItem = GetControlItem(from);

                if (controlItem == null)
                {
                    from.SendMessage("Stats: {0} Skills: {1} Items: {2}", stats, skills, items);
                    StartControl(from, target, stats, skills, items);
                }
                else
                {
                    from.SendMessage(
                        "Stats: {0} Skills: {1} Items: {2}",
                        controlItem.Stats,
                        controlItem.Skills,
                        controlItem.Items
                    );
                    ChangeControl(
                        target,
                        controlItem,
                        controlItem.Stats,
                        controlItem.Skills,
                        controlItem.Items
                    );
                }
            }
            else if (from is PlayerMobile && targeted is ControlItem)
                ((Item)targeted).Delete();
        }

        private static void StartControl(
            Mobile from,
            Mobile target,
            bool stats,
            bool skills,
            bool items
        )
        {
            from.SendMessage("You possess the mortal body of {0}, {1}", target.Name, target.Title);
            // "You leave your Body and control {0}, {1}"

            // Remember the controller's original position and direction.
            Map originalMap = from.Map;
            Point3D originalLocation = from.Location;
            Direction originalDirection = from.Direction;

            // Clone Player
            PlayerMobile playerClone = (PlayerMobile)DupeMobile(from);
            new CloneTarget().SimulateTarget(playerClone, from, false);

            // Create ControlItem that links owner, clone, and NPC
            ControlItem controlItem = new ControlItem(
                from,
                playerClone,
                target,
                stats,
                skills,
                items
            )
            {
                OriginalMap = originalMap,
                OriginalLocation = originalLocation,
                OriginalDirection = originalDirection
            };

            controlItem.OriginalHidden = target.Hidden;
            controlItem.OriginalCantWalk = target.CantWalk;
            controlItem.OriginalFrozen = target.Frozen;
            controlItem.OriginalParalyzed = target.Paralyzed;

            // Clone the target onto the controller before dropping the control item.
            // Dropping beforehand would delete the item when the backpack is cleared.
            new CloneTarget().SimulateTarget(from, target, true);
            from.Hidden = target.Hidden;

            EnsureControllerNeeds(from);

            // Ensure the backpack exists after cloning and drop the control item.
            Container pack = from.Backpack as Container;
            if (pack == null)
            {
                pack = new Backpack();
                from.AddItem(pack);
            }
            pack.DropItem(controlItem);

            target.Internalize();
            playerClone.Internalize();
        }

        private static void ChangeControl(
            Mobile target,
            ControlItem controlItem,
            bool stats,
            bool skills,
            bool items
        )
        {
            Mobile from = controlItem.Owner;
            PlayerMobile oldPlayer = controlItem.Player;
            Mobile oldNPC = controlItem.NPC;

            if (oldNPC != null)
            {
                //Restore NPC
                if (!oldNPC.Deleted)
                {
                    //Always apply properties while reverting?
                    //Yes, because stats change
                    //Props from -> oldNPC
                    new CloneTarget().SimulateTarget(oldNPC, from, true);
                    RestoreNpcState(controlItem, oldNPC);
                }
                else
                {
                    from.SendMessage("Your original body has been destroyed.");
                    oldNPC.Delete();
                }
            }

            //Possess a new mobel or release control.
            if (target != oldPlayer && target != null && !target.Deleted) //Possess new mobile
            {
                from.SendMessage("You possess {0}, {1}", target.Name, target.Title);
                //"You Control  {0}, {1}"

                //Update ControlItem
                controlItem.NPC = target;
                controlItem.Stats = stats;
                controlItem.Skills = skills;
                controlItem.Items = items;
                controlItem.OriginalHidden = target.Hidden;
                controlItem.OriginalCantWalk = target.CantWalk;
                controlItem.OriginalFrozen = target.Frozen;
                controlItem.OriginalParalyzed = target.Paralyzed;
                new CloneTarget().SimulateTarget(from, target, true);
                from.Hidden = target.Hidden;

                EnsureControllerNeeds(from);

                target.Internalize();
            }
            else if (target == oldPlayer && !target.Deleted)
            {
                controlItem.Delete();
            }
        }

        public static void EndControl(ControlItem controlItem, bool stats, bool skills, bool items)
        {
            Mobile from = controlItem.Owner;
            PlayerMobile oldPlayer = controlItem.Player;
            Mobile oldNPC = controlItem.NPC;

            if (from == null)
                return;

            from.SendMessage("You are in your original Body.");
            //"You are in your original Body"

            //NPC wiederherstellen
            if (oldNPC != null && !oldNPC.Deleted)
            {
                new CloneTarget().SimulateTarget(oldNPC, from, true);
                RestoreNpcState(controlItem, oldNPC);
            }
            else
            {
                from.SendMessage("The original NPC was deleted. Maybe because a manual respawn.");
                //"The original NPC was deleted. Maybe because a manual respawn"
                if (oldNPC != null)
                    oldNPC.Delete();
            }

            if (oldPlayer != null && !oldPlayer.Deleted)
            {
                //Spieler Wiederherstellen (100%)
                //Props: oldPlayer -> player
                //CopyProps(from, oldPlayer, true, true);
                new CloneTarget().SimulateTarget(from, oldPlayer, false);
                // Restore the original position and direction after cloning.
                from.MoveToWorld(controlItem.OriginalLocation, controlItem.OriginalMap);
                from.Direction = controlItem.OriginalDirection;
                //Equip: oldPlayer -> player
                //MoveEquip(oldPlayer, from, true);

                oldPlayer.Delete();
            }

            UpdateStaffDisguise(from, null);
        }

        public static void EndClone(CloneItem cloneItem)
        {
            Mobile from = cloneItem.Owner;
            Mobile oldPlayer = cloneItem.Player;

            if (from == null)
                return;

            from.SendMessage("You return to your original form.");

            if (oldPlayer != null && !oldPlayer.Deleted)
            {
                new CloneTarget().SimulateTarget(from, oldPlayer, false);
                oldPlayer.Delete();
            }

            UpdateStaffDisguise(from, null);
        }

        //Return true if the base.OnBeforeDeath should be executed and false if not.
        public static bool UncontrolDeath(Mobile from)
        {
            if (from.AccessLevel < accessLevel)
                return true;

            ControlItem controlItem = GetControlItem(from);

            if (controlItem != null)
            {
                //Backup NPC
                Mobile NPC = (Mobile)controlItem.NPC;

                //Release GM
                controlItem.Delete();
                from.Hits = from.HitsMax;
                from.Stam = from.StamMax;
                from.Mana = from.StamMax;

                //Kill NPC as normal
                NPC.Kill();

                return false; //GM stirbt nicht ;)
            }

            return true;
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
                //CopyProperties( o, mobile, t, "Parent", "NetState" );

                //CopyProps(newMobile, srcMobile, true, true);

                //CopyProps didn't copy the AccessLevel, but we need it for some items the GM is wearing.
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

        /*copy the poropertys from one Mobile to another*/
        private static void CopyProps(
            Mobile target,
            Mobile from,
            bool stats,
            bool skills,
            bool location
        )
        {
            try
            {
                if (from.Map == Map.Internal)
                    from.MoveToWorld(target.Location, target.Map);

                if (stats)
                    CopyMobileProps(
                        target,
                        from,
                        location,
                        "Parent",
                        "NetState",
                        "Player",
                        "AccessLevel"
                    );
                else
                    CopyMobileProps(
                        target,
                        from,
                        location,
                        "Parent",
                        "NetState",
                        "Player",
                        "AccessLevel",
                        "RawStr",
                        "Str",
                        "RawDex",
                        "Dex",
                        "RawInt",
                        "Int",
                        "Hits",
                        "Mana",
                        "Stam"
                    );

                if (skills)
                    //Console.WriteLine("Copy {2} Skills from {0} to {1}", from, target, target.Skills.Length);
                    for (int i = 0; i < target.Skills.Length; ++i)
                    {
                        //Console.WriteLine("Skill {0} old Value = {1} new Value = {2}", i, target.Skills[i].Base, from.Skills[i].Base);
                        target.Skills[i].Base = from.Skills[i].Base;
                    }
            }
            catch
            {
                Console.WriteLine(
                    "Error in Control.cs -> CopyProps(Mobile from, Mobile target, bool stats, bool skills)"
                );
                return;
            }
        }

        private static void CopyMobileProps(
            Mobile dest,
            Mobile src,
            bool location,
            params string[] oProps
        )
        {
            //Type type = src.GetType(); didn't work correct
            List<String> omitProps = new List<String>(oProps);
            if (!location)
                omitProps.AddRange(new String[] { "Direction", "Location", "Map" });
            Type type = typeof(Mobile);

            PropertyInfo[] props = type.GetProperties();

            bool omit = false;
            //Console.WriteLine("----- COPPY PROPS ------");
            //Console.WriteLine("From: {0} to {1}", src.Name, dest.Name);
            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    for (int j = 0; j < omitProps.Count; j++)
                    {
                        if (string.Compare(omitProps[j], props[i].Name, true) == 0)
                        {
                            omit = true;
                            //Console.WriteLine("Skip Value {0} @ {1} = {2}", props[i].Name, dest.Name, props[i].GetValue( src, null ));
                            break;
                        }
                    }

                    if (props[i].CanRead && props[i].CanWrite && !omit)
                    {
                        //Set on target
                        //Console.WriteLine("SetValue {0} @ {1} = {2}", props[i].Name, dest.Name, props[i].GetValue( src, null ));
                        //dest.SendMessage("SetValue {0}", props[i].Name);
                        props[i].SetValue(dest, props[i].GetValue(src, null), null);
                        //Console.WriteLine("-> {0}", props[i].GetValue( dest, null ));
                    }

                    omit = false; //Keep copying
                }
                catch
                {
                    Console.WriteLine("Can't copy property: Control.cs");
                }
            }
        }

        private static bool CompareType(object o, Type type)
        {
            if (o.GetType() == type || o.GetType().IsSubclassOf(type))
                return true;
            else
                return false;
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
                Delete();

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
                if (m_Player is PlayerMobile)
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
                Delete();

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
