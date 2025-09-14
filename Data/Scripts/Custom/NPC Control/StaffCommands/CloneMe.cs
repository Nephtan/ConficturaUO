using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

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

            BaseCreature m = CreateClone(mobile);
            CloneMobileProperties(mobile, m);
            CloneMobileItems(mobile, m);
            CloneMobileBackpack(mobile, m);
        }

        private static BaseCreature CreateClone(Mobile mobile)
        {
            BaseCreature m = new BaseCreature(
                AIType.AI_Melee,
                FightMode.Aggressor,
                10,
                1,
                0.2,
                0.4
            );

            m.Female = Utility.RandomBool();

            if (m.Female)
            {
                m.Body = 0x1A6;
                m.Name = NameList.RandomName("female");
            }
            else
            {
                m.Body = 0x1A4;
                m.Name = NameList.RandomName("male");
            }

            m.Hidden = true;
            m.Map = mobile.Map;
            m.Location = mobile.Location;
            m.Direction = mobile.Direction;

            return m;
        }

        public static void CloneMobileProperties(Mobile source, Mobile target)
        {
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
            target.Hue = source.Hue;
            target.Hits = source.HitsMax;
            target.Mana = source.ManaMax;
            target.Stam = source.StamMax;
            target.HairItemID = source.HairItemID;
            target.FacialHairItemID = source.FacialHairItemID;
            target.HairHue = source.HairHue;
            target.FacialHairHue = source.FacialHairHue;

            for (int i = 0; i < source.Skills.Length; i++)
                target.Skills[i].Base = source.Skills[i].Base;
        }

        public static void CloneMobileItems(Mobile source, Mobile target)
        {
            ArrayList items = new ArrayList(source.Items);

            for (int i = 0; i < items.Count; i++)
            {
                Item item = (Item)items[i];

                if (item != null && item.Parent == source && item != source.Backpack)
                {
                    Item newItem = CloneItem(item);
                    if (newItem != null)
                    {
                        target.AddItem(newItem);
                    }
                }
            }
        }

        public static void CloneMobileBackpack(Mobile source, Mobile target)
        {
            Backpack originalBackpack = source.Backpack as Backpack;
            if (originalBackpack != null)
            {
                Backpack clonedBackpack = new Backpack();
                target.AddItem(clonedBackpack);

                foreach (Item backpackItem in originalBackpack.Items)
                {
                    Item newItem = CloneItem(backpackItem);
                    if (newItem != null)
                    {
                        clonedBackpack.AddItem(newItem);
                    }
                }
            }
        }

        public static Item CloneItem(Item item)
        {
            Type itemType = item.GetType();
            ConstructorInfo constructor = itemType.GetConstructor(Type.EmptyTypes);

            if (constructor != null)
            {
                try
                {
                    object o = constructor.Invoke(null);
                    if (o != null && o is Item)
                    {
                        Item newItem = (Item)o;
                        CopyProperties(newItem, item);
                        item.OnAfterDuped(newItem);
                        newItem.Parent = null;
                        return newItem;
                    }
                }
                catch { }
            }

            return null;
        }

        public static void CopyProperties(Item dest, Item src)
        {
            PropertyInfo[] props = src.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    if (props[i].CanRead && props[i].CanWrite)
                    {
                        // These properties must not be copied during the dupe, they get set implicitely by placing items properly using "DropItem()" etc.
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
        public static void CopyProperties(object dest, object src)
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
    }
}
