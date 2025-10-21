/*************************************************************
 * File: CloneThings.cs
 * Description: Helper routines that duplicate mobiles, items,
 *              and container hierarchies for clone creation.
 *************************************************************/
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Server.Custom.Confictura.CloneOfflinePlayerCharacters
{
    /// <summary>
    /// Provides utility methods shared by the clone manager and clone
    /// mobile for copying appearance, inventory, and equipment.
    /// </summary>
    public static class CloneThings
    {
        public static CharacterClone CreateClone(Mobile mobile)
        {
            return new CharacterClone(mobile);
        }

        public static void CloneMobileProperties(Mobile source, Mobile target)
        {
            target.Location = source.Location;
            target.Map = source.Map;
            target.Direction = source.Direction;
            target.Hidden = false;

            target.Dex = source.Dex;
            target.Int = source.Int;
            target.Str = source.Str;
            target.Fame = source.Fame;
            target.Karma = source.Karma;
            target.NameHue = source.NameHue;
            target.SpeechHue = source.SpeechHue;
            target.Criminal = source.Criminal;
            target.Name = source.Name;
            target.Title = source.Title;
            target.Female = source.Female;
            target.Body = source.Body;
            target.BodyValue = source.BodyValue;
            target.Hue = source.Hue;

            int sourceHitsMax = Math.Max(1, source.HitsMax);
            int sourceStamMax = Math.Max(1, source.StamMax);
            int sourceManaMax = Math.Max(0, source.ManaMax);

            target.Hits = Math.Min(Math.Max(0, source.Hits), sourceHitsMax);
            target.Stam = Math.Min(Math.Max(0, source.Stam), sourceStamMax);
            target.Mana = Math.Min(Math.Max(0, source.Mana), sourceManaMax);
            target.Hunger = source.Hunger;
            target.Thirst = source.Thirst;

            target.FollowersMax = source.FollowersMax;
            target.Followers = source.Followers;

            target.HairItemID = source.HairItemID;
            target.FacialHairItemID = source.FacialHairItemID;
            target.HairHue = source.HairHue;
            target.FacialHairHue = source.FacialHairHue;

            for (int i = 0; i < source.Skills.Length; i++)
            {
                target.Skills[i].Base = source.Skills[i].Base;
            }

            CharacterClone clone = target as CharacterClone;

            if (clone != null)
            {
                clone.FinalizeClone();
            }
        }

        public static void CloneMobileItems(Mobile source, Mobile target)
        {
            if (source.Items == null)
            {
                return;
            }

            List<Item> items = new List<Item>(source.Items);
            List<Item> newItems = new List<Item>();

            foreach (Item item in items)
            {
                if (item == null || item.Parent != source || item == source.Backpack)
                {
                    continue;
                }

                if (!item.Stackable && item.Amount > 1)
                {
                    for (int i = 0; i < item.Amount; i++)
                    {
                        Item newItem = CloneItem(item);

                        if (newItem == null)
                        {
                            continue;
                        }

                        newItem.Amount = 1;
                        newItems.Add(newItem);
                    }
                }
                else
                {
                    Item newItem = CloneItem(item);

                    if (newItem == null)
                    {
                        continue;
                    }

                    newItems.Add(newItem);
                }
            }

            if (newItems.Count > 0)
            {
                target.AddItem(newItems);
            }
        }

        public static void AddItem(this Mobile mobile, IEnumerable<Item> items)
        {
            foreach (Item item in items)
            {
                mobile.AddItem(item);
            }
        }

        public static void CloneMobileBackpack(Mobile source, Mobile target)
        {
            Backpack originalBackpack = source.Backpack as Backpack;

            if (originalBackpack != null)
            {
                BackpackClone clonedBackpack = new BackpackClone();
                target.AddItem(clonedBackpack);
                CloneContainerContents(originalBackpack, clonedBackpack);
            }
        }

        public static void CloneContainerContents(
            Server.Items.Container sourceContainer,
            Server.Items.Container targetContainer
        )
        {
            Item[] itemsCopy = sourceContainer.Items.ToArray();

            foreach (Item item in itemsCopy)
            {
                if (!item.Stackable && item.Amount > 1)
                {
                    for (int i = 0; i < item.Amount; i++)
                    {
                        Item clonedItem = CloneItem(item);

                        if (clonedItem == null)
                        {
                            continue;
                        }

                        clonedItem.Amount = 1;
                        targetContainer.AddItem(clonedItem);

                        Server.Items.Container sourceNestedContainer = item as Server.Items.Container;
                        Server.Items.Container targetNestedContainer = clonedItem as Server.Items.Container;

                        if (sourceNestedContainer != null && targetNestedContainer != null)
                        {
                            CloneContainerContents(sourceNestedContainer, targetNestedContainer);
                        }
                    }
                }
                else
                {
                    Item clonedItem = CloneItem(item);

                    if (clonedItem != null)
                    {
                        targetContainer.AddItem(clonedItem);

                        Server.Items.Container sourceNestedContainer = item as Server.Items.Container;
                        Server.Items.Container targetNestedContainer = clonedItem as Server.Items.Container;

                        if (sourceNestedContainer != null && targetNestedContainer != null)
                        {
                            CloneContainerContents(sourceNestedContainer, targetNestedContainer);
                        }
                    }
                }
            }
        }

        public static Item CloneItem(Item item)
        {
            Type itemType = item.GetType();

            if (itemType == null)
            {
                return null;
            }

            try
            {
                if (item is StaffFiveParts)
                {
                    StaffFiveParts sourceStaff = (StaffFiveParts)item;

                    StaffFiveParts newStaff = (StaffFiveParts)
                        Activator.CreateInstance(
                            itemType,
                            sourceStaff.Staff_Owner,
                            sourceStaff.Staff_Magic
                        );

                    CopyProperties(newStaff, sourceStaff);
                    item.OnAfterDuped(newStaff);
                    newStaff.Parent = null;

                    return newStaff;
                }
                else
                {
                    ConstructorInfo constructor = itemType.GetConstructor(Type.EmptyTypes);

                    if (constructor != null)
                    {
                        object instance = constructor.Invoke(null);

                        if (instance != null && instance is Item)
                        {
                            Item newItem = (Item)instance;
                            CopyProperties(newItem, item);
                            item.OnAfterDuped(newItem);
                            newItem.Parent = null;

                            return newItem;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Intentionally swallow exceptions so a bad item does not
                // prevent other equipment from being cloned.
            }

            return null;
        }

        public static void CloneMobileMount(Mobile source, Mobile target)
        {
            if (!source.Mounted)
            {
                return;
            }

            BaseMount baseMount = source.Mount as BaseMount;
            EtherealMount etherealMount = source.Mount as EtherealMount;

            if (baseMount != null)
            {
                MountClone clonedBaseMount = new MountClone(baseMount);
                clonedBaseMount.Rider = target;
            }
            else if (etherealMount != null)
            {
                EtherealMountClone clonedEtherealMount = new EtherealMountClone(etherealMount);
                clonedEtherealMount.Rider = target;
            }
        }

        public static void CopyProperties(Item dest, Item src)
        {
            PropertyInfo[] props = src.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                try
                {
                    if (prop.CanRead && prop.CanWrite)
                    {
                        if (
                            prop.Name != "Parent"
                            && prop.Name != "TotalWeight"
                            && prop.Name != "TotalItems"
                            && prop.Name != "TotalGold"
                        )
                        {
                            prop.SetValue(dest, prop.GetValue(src, null), null);
                        }
                    }
                }
                catch
                {
                }

                if (src is BaseWeapon)
                {
                    object srcObject = ((BaseWeapon)src).Attributes;
                    object destObject = ((BaseWeapon)dest).Attributes;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseWeapon)src).SkillBonuses;
                    destObject = ((BaseWeapon)dest).SkillBonuses;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseWeapon)src).WeaponAttributes;
                    destObject = ((BaseWeapon)dest).WeaponAttributes;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseWeapon)src).AosElementDamages;
                    destObject = ((BaseWeapon)dest).AosElementDamages;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }
                }
                else if (src is BaseArmor)
                {
                    object srcObject = ((BaseArmor)src).Attributes;
                    object destObject = ((BaseArmor)dest).Attributes;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseArmor)src).SkillBonuses;
                    destObject = ((BaseArmor)dest).SkillBonuses;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseArmor)src).ArmorAttributes;
                    destObject = ((BaseArmor)dest).ArmorAttributes;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }
                }
                else if (src is BaseJewel)
                {
                    object srcObject = ((BaseJewel)src).Attributes;
                    object destObject = ((BaseJewel)dest).Attributes;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseJewel)src).SkillBonuses;
                    destObject = ((BaseJewel)dest).SkillBonuses;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseJewel)src).Resistances;
                    destObject = ((BaseJewel)dest).Resistances;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }
                }
                else if (src is BaseClothing)
                {
                    object srcObject = ((BaseClothing)src).Attributes;
                    object destObject = ((BaseClothing)dest).Attributes;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseClothing)src).SkillBonuses;
                    destObject = ((BaseClothing)dest).SkillBonuses;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseClothing)src).ClothingAttributes;
                    destObject = ((BaseClothing)dest).ClothingAttributes;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }

                    srcObject = ((BaseClothing)src).Resistances;
                    destObject = ((BaseClothing)dest).Resistances;

                    if (srcObject != null && destObject != null)
                    {
                        CopyProperties(destObject, srcObject);
                    }
                }
            }
        }

        public static void CopyProperties(object dest, object src)
        {
            PropertyInfo[] props = src.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                try
                {
                    if (prop.CanRead && prop.CanWrite)
                    {
                        prop.SetValue(dest, prop.GetValue(src, null), null);
                    }
                }
                catch
                {
                }
            }
        }
    }
}