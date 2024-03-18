/*************************************************************
 * File: CloneOfflinePlayerCharacters.cs
 * Version: 1.0
 * Author: Nephtan
 * Shard: Confictura - Legend & Adventure
 * Description: A class for cloning player characters on logout
 * Special Thanks: Felladrin and Quick_silver for their ideas, methods, and logic included within.
 *************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;

namespace Server.Commands
{
    public static class CheckClonesCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "CheckClones",
                AccessLevel.Administrator,
                new CommandEventHandler(CheckClones_OnCommand)
            );
        }

        [Usage("CheckClones")]
        [Description("Creates clones of all offline players who don't currently have one.")]
        public static void CheckClones_OnCommand(CommandEventArgs e)
        {
            Confictura.Custom.CloneOfflinePlayerCharacters.CheckFirstRun();
            e.Mobile.SendMessage("Clone check initiated.");
        }
    }
}

namespace Confictura.Custom
{
    #region Cloning Players and Their Things
    public static class CloneOfflinePlayerCharacters
    {
        public static void Initialize()
        {
            EventSink.Logout += OnLogout;
            EventSink.Login += OnLogin;
            CheckFirstRun();
        }

        public static bool IsValidRegion(PlayerMobile playerMobile)
        {
            return !(
                playerMobile.Region.IsPartOf(typeof(StartRegion))
                || playerMobile.Region.IsPartOf(typeof(PublicRegion))
                || playerMobile.Region.IsPartOf(typeof(CrashRegion))
                || playerMobile.Region.IsPartOf(typeof(PrisonArea))
                || playerMobile.Region.IsPartOf(typeof(SafeRegion))
            );
        }

        static void OnLogout(LogoutEventArgs e)
        {
            PlayerMobile playerMobile = e.Mobile as PlayerMobile;
            if (
                !(e.Mobile is CharacterClone)
                && playerMobile != null
                && playerMobile.Alive
                && playerMobile.AccessLevel == AccessLevel.Player
                && IsValidRegion(playerMobile)
            )
            {
                var characterClone = CloneThings.CreateClone(e.Mobile);
                CloneThings.CloneMobileProperties(e.Mobile, characterClone);
                CloneThings.CloneMobileItems(e.Mobile, characterClone);
                CloneThings.CloneMobileBackpack(e.Mobile, characterClone);
                CloneThings.CloneMobileMount(e.Mobile, characterClone);
            }
        }

        static void OnLogin(LoginEventArgs e)
        {
            DeleteClonesOf(e.Mobile);
        }

        public static void CreateCloneOf(Mobile m)
        {
            PlayerMobile playerMobile = m as PlayerMobile;
            if (
                !(m is CharacterClone)
                && playerMobile != null
                && playerMobile.Alive
                && playerMobile.AccessLevel == AccessLevel.Player
                //&& playerMobile.RaceID == 0
                && IsValidRegion(playerMobile)
            )
            {
                var characterClone = CloneThings.CreateClone(m);
                CloneThings.CloneMobileProperties(m, characterClone);
                CloneThings.CloneMobileItems(m, characterClone);
                CloneThings.CloneMobileBackpack(m, characterClone);
                CloneThings.CloneMobileMount(m, characterClone);
            }
        }

        public static void DeleteClonesOf(Mobile m)
        {
            foreach (var mobile in new List<Mobile>(World.Mobiles.Values))
            {
                if (mobile is CharacterClone && ((CharacterClone)mobile).Original == m)
                {
                    IMount mount = mobile.Mount;

                    if (mount != null)
                    {
                        // Dismount the clone before deleting it
                        mount.Rider = null;

                        // Delete the mount if it's an EtherealMount or a BaseMount
                        if (mount is EtherealMount)
                        {
                            EtherealMount etherealMount = (EtherealMount)mount;
                            etherealMount.Delete();
                        }
                        else if (mount is BaseMount)
                        {
                            BaseMount baseMount = (BaseMount)mount;
                            baseMount.Delete();
                        }
                    }

                    mobile.Delete();
                }
            }
        }

        public static void CheckFirstRun()
        {
            // Initialize counters for total clones and processed clones.
            int totalClones = 0;
            int processedClones = 0;

            // Create a dictionary to store existing clones and their associated original players.
            Dictionary<PlayerMobile, CharacterClone> existingClones =
                new Dictionary<PlayerMobile, CharacterClone>();

            // Identify existing clones and store them in the dictionary.
            foreach (var mobile in new List<Mobile>(World.Mobiles.Values))
            {
                CharacterClone clone = mobile as CharacterClone;
                if (clone != null)
                {
                    PlayerMobile originalPlayer = clone.Original as PlayerMobile;
                    if (originalPlayer != null)
                    {
                        existingClones[originalPlayer] = clone;
                    }

                    if (clone.RaceID != 0)
                    {
                        clone.HueMod = 0;
                    }
                }
            }

            // Identify all valid PlayerMobiles that are alive, have Player access level, and don't have an existing clone.
            foreach (var mobile in new List<Mobile>(World.Mobiles.Values))
            {
                PlayerMobile playerMobile = mobile as PlayerMobile;
                if (
                    playerMobile != null
                    && playerMobile.Alive
                    && playerMobile.AccessLevel == AccessLevel.Player
                    && !existingClones.ContainsKey(playerMobile)
                )
                {
                    // Store the original map and location for later restoration.
                    Map originalMap = playerMobile.Map;
                    Point3D originalLocation = playerMobile.Location;

                    // Temporarily move the player to their logout map and location.
                    playerMobile.Map = playerMobile.LogoutMap;
                    playerMobile.Location = playerMobile.LogoutLocation;

                    // Adjust the player's X-coordinate (this is to trigger the race item body transformation for players playing as races other than human).
                    playerMobile.X += 1;
                    playerMobile.X -= 1;

                    if (IsValidRegion(playerMobile))
                    {
                        // Increment the total clones counter.
                        totalClones++;
                    }
                }
            }

            // Create clones for each identified PlayerMobile.
            Console.Write("Cloning Offline Players... "); // Display the initial message.
            foreach (var mobile in new List<Mobile>(World.Mobiles.Values))
            {
                PlayerMobile playerMobile = mobile as PlayerMobile;
                if (
                    playerMobile != null
                    && playerMobile.Alive
                    && playerMobile.AccessLevel == AccessLevel.Player
                    && !existingClones.ContainsKey(playerMobile)
                )
                {
                    CreateCloneOf(playerMobile);

                    // Increment the processed clones counter and display progress in the console.
                    if (IsValidRegion(playerMobile))
                    {
                        processedClones++;
                    }
                    Console.CursorLeft = 27; // Adjust cursor position to after the initial message.
                    Console.Write(String.Format("{0}/{1}", processedClones, totalClones));
                }
            }

            // Move to the next line in the console after displaying progress.
            Console.WriteLine();

            // Delete any CharacterClones that are in invalid regions.
            foreach (var mobile in new List<Mobile>(World.Mobiles.Values))
            {
                if (
                    mobile is CharacterClone
                    && (
                        mobile.Region.IsPartOf(typeof(StartRegion))
                        || mobile.Region.IsPartOf(typeof(PublicRegion))
                        || mobile.Region.IsPartOf(typeof(CrashRegion))
                        || mobile.Region.IsPartOf(typeof(PrisonArea))
                        || mobile.Region.IsPartOf(typeof(SafeRegion))
                    )
                )
                {
                    mobile.Delete();
                }
            }

            // Restore the original location and map of each PlayerMobile.
            foreach (var mobile in new List<Mobile>(World.Mobiles.Values))
            {
                PlayerMobile playerMobile = mobile as PlayerMobile;
                if (
                    playerMobile != null
                    && playerMobile.Alive
                    && playerMobile.AccessLevel == AccessLevel.Player
                )
                {
                    playerMobile.Location = playerMobile.LogoutLocation;
                    playerMobile.Map = Map.Internal;
                }
            }
        }
    }

    public static class CloneThings
    {
        public static BaseCreature CreateClone(Mobile mobile)
        {
            BaseCreature m = new CharacterClone(mobile);
            return m;
        }

        public static void CloneMobileProperties(Mobile source, Mobile target)
        {
            // Clone basic
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

            // Clone max health, mana, and stamina
            target.Hits = source.HitsMax;
            target.Mana = source.ManaMax;
            target.Stam = source.StamMax;

            // Clone followers
            target.FollowersMax = source.FollowersMax;
            target.Followers = source.Followers;

            // Clone appearance
            target.HairItemID = source.HairItemID;
            target.FacialHairItemID = source.FacialHairItemID;
            target.HairHue = source.HairHue;
            target.FacialHairHue = source.FacialHairHue;

            // Clone skills
            for (int i = 0; i < source.Skills.Length; i++)
                target.Skills[i].Base = source.Skills[i].Base;
        }

        public static void CloneMobileItems(Mobile source, Mobile target)
        {
            if (source.Items == null)
                return;

            List<Item> items = new List<Item>(source.Items);
            List<Item> newItems = new List<Item>();

            foreach (Item item in items)
            {
                if (item == null || item.Parent != source || item == source.Backpack)
                    continue;

                Item newItem = CloneItem(item);
                if (newItem == null)
                    continue;

                newItems.Add(newItem);
            }

            if (newItems.Count > 0)
                target.AddItem(newItems);
        }

        public static void AddItem(this Mobile mobile, IEnumerable<Item> items)
        {
            foreach (Item item in items)
                mobile.AddItem(item);
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
                Item clonedItem = CloneItem(item);
                if (clonedItem != null)
                {
                    targetContainer.AddItem(clonedItem);
                    Server.Items.Container sourceNestedContainer = item as Server.Items.Container;
                    Server.Items.Container targetNestedContainer =
                        clonedItem as Server.Items.Container;
                    if (sourceNestedContainer != null && targetNestedContainer != null)
                    {
                        CloneContainerContents(sourceNestedContainer, targetNestedContainer);
                    }
                }
            }
        }

        public static Item CloneItem(Item item)
        {
            Type itemType = item.GetType();
            if (itemType == null)
                return null;

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
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
            }

            return null;
        }

        public static void CloneMobileMount(Mobile source, Mobile target)
        {
            if (!source.Mounted)
                return;

            var baseMount = source.Mount as BaseMount;
            var etherealMount = source.Mount as EtherealMount;

            if (baseMount != null)
            {
                var clonedBaseMount = new MountClone(baseMount);
                clonedBaseMount.Rider = target;
            }
            else if (etherealMount != null)
            {
                var clonedEtherealMount = new EtherealMountClone(etherealMount);
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
                        // These properties must not be copied during the dupe, they get set implicitely by placing
                        // items properly using "DropItem()" etc. .
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
                catch { }

                // BaseArmor, BaseClothing, BaseJewel, BaseWeapon: copy nested classes
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

            foreach (PropertyInfo prop in props)
            {
                try
                {
                    if (prop.CanRead && prop.CanWrite)
                    {
                        prop.SetValue(dest, prop.GetValue(src, null), null);
                    }
                }
                catch { }
            }
        }
    }
    #endregion

    #region Character Clone Mobile
    public class CharacterClone : BaseCreature
    {
        private int m_Pay = 1;
        private bool m_IsHired;
        private int m_HoldGold = 8;
        private Timer m_PayTimer;
        private static Hashtable m_HireTable = new Hashtable();
        private bool m_IsTraining = false; // Flag to determine if the player is currently training

        public int Pay
        {
            get { return m_Pay; }
            set { m_Pay = value; }
        }

        public int HoldGold
        {
            get { return m_HoldGold; }
            set { m_HoldGold = value; }
        }

        public static Hashtable HireTable
        {
            get { return m_HireTable; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Original { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsHired
        {
            get { return m_IsHired; }
            set { m_IsHired = value; }
        }

        public CharacterClone(Mobile original)
            : base(
                AIType.AI_Melee,
                FightMode.Aggressor,
                18,
                CalculateRangeFight(original),
                0.2,
                0.3
            )
        {
            Original = original;
            foreach (var property in (typeof(Mobile)).GetProperties())
            {
                if (property.CanRead && property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(Original, null), null);
                }
            }

            for (int i = 0, l = Original.Skills.Length; i < l; ++i)
            {
                Skills[i].Base = Original.Skills[i].Base;
            }
            Payday(this);

            // Set maximum hit points, stamina, and mana
            SetHits(Str * 2);
            SetStam(Dex * 2);
            SetMana(Int * 2);

            CalculateBaseDamage();

            Hits = HitsMax;
            Stam = StamMax;
            Mana = ManaMax;

            ControlSlots = 4;

            Player = false;

            if (Map == Map.Internal)
            {
                Map = LogoutMap;
            }

            //if (Kills >= 1)
            //{
            //    FightMode = FightMode.Aggressor;
            //}
            //else
            //{
            //    FightMode = FightMode.Evil;
            //}
        }

        protected override BaseAI ForcedAI
        {
            get { return new OmniAI(this); }
        }

        public override int GetMaxResistance(ResistanceType type)
        {
            return 70;
        }

        private void CalculateBaseDamage()
        {
            double tactics = Original.Skills[SkillName.Tactics].Value;
            double anatomy = Original.Skills[SkillName.Anatomy].Value;
            double lumberjacking = Original.Skills[SkillName.Lumberjacking].Value;
            double strength = Original.Str;

            double tacticsBonus = tactics >= 100 ? (tactics / 1.6) + 6.25 : tactics / 1.6;
            double anatomyBonus = anatomy >= 100 ? (anatomy / 2) + 5 : anatomy / 2;
            double lumberjackingBonus =
                lumberjacking >= 100 ? (lumberjacking / 5) + 10 : lumberjacking / 5;
            double strengthBonus = strength >= 100 ? (strength * 0.3) + 5 : strength * 0.3;

            double totalDamageBonus =
                tacticsBonus + anatomyBonus + lumberjackingBonus + strengthBonus;

            Item weapon = FindItemOnLayer(Layer.OneHanded) ?? FindItemOnLayer(Layer.TwoHanded);
            int minBaseDamage = 5;
            int maxBaseDamage = 10;

            if (weapon is BaseWeapon)
            {
                BaseWeapon baseWeapon = weapon as BaseWeapon;
                minBaseDamage = baseWeapon.MinDamage;
                maxBaseDamage = baseWeapon.MaxDamage;
            }

            int minFinalDamage = (int)(minBaseDamage + (minBaseDamage * totalDamageBonus / 100));
            int maxFinalDamage = (int)(maxBaseDamage + (maxBaseDamage * totalDamageBonus / 100));

            SetDamage(minFinalDamage, maxFinalDamage);
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.DisplayPaperdollTo(from);
            base.OnDoubleClick(from);
        }

        public bool TryTrainSkill(Mobile from, Item item)
        {
            if (item is Gold && CheckTeachingMatch(from))
            {
                if (Teach(m_Teaching, from, item.Amount, true))
                {
                    item.Delete();
                    return true;
                }
            }
            return false;
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            // First, check if the player is trying to train a skill
            if (TryTrainSkill(from, item))
            {
                return true;
            }

            if (m_Pay <= 0) // Check if the pay is above 0
            {
                SayTo(from, "I am not skilled enough to be hired.");
                return false;
            }

            if (m_Pay != 0)
            {
                // Is the creature already hired
                if (Controlled == false)
                {
                    // Is the item the payment in gold
                    if (item is Gold)
                    {
                        // Is the payment in gold sufficient
                        if (item.Amount >= m_Pay)
                        {
                            // Check if this mobile already has a hire
                            CharacterClone hire = (CharacterClone)m_HireTable[from];

                            if (hire != null && !hire.Deleted && hire.GetOwner() == from)
                            {
                                SayTo(from, 500896); // I see you already have an escort.
                                return false;
                            }

                            // Try to add the hireling as a follower
                            if (AddHire(from) == true)
                            {
                                SayTo(
                                    from,
                                    string.Format(
                                        "I thank thee for paying me. I will work for thee for {0} hours.",
                                        (int)item.Amount / m_Pay
                                    )
                                );
                                m_HireTable[from] = this;
                                m_HoldGold += item.Amount;
                                m_PayTimer = new PayTimer(this);
                                m_PayTimer.Start();
                                return true;
                            }
                            else
                                return false;
                        }
                        else
                        {
                            this.SayHireCost();
                        }
                    }
                    else
                    {
                        SayTo(from, 1043268); // Tis crass of me, but I want gold
                    }
                }
                else
                {
                    Say(1042495); // I have already been hired.
                }
            }
            else
            {
                SayTo(from, 500200); // I have no need for that.
            }

            return base.OnDragDrop(from, item);
        }

        private static int CalculateRangeFight(Mobile original)
        {
            int maxRangeFight = 1;
            Item weapon =
                original.FindItemOnLayer(Layer.OneHanded)
                ?? original.FindItemOnLayer(Layer.TwoHanded);

            double meleeSkillSum =
                original.Skills[SkillName.Swords].Value
                + original.Skills[SkillName.Bludgeoning].Value
                + original.Skills[SkillName.Fencing].Value
                + original.Skills[SkillName.Bushido].Value
                + original.Skills[SkillName.Ninjitsu].Value
                + original.Skills[SkillName.Knightship].Value;

            double marksmanshipSkill = original.Skills[SkillName.Marksmanship].Value;
            double magerySkill = original.Skills[SkillName.Magery].Value;

            if (
                marksmanshipSkill >= meleeSkillSum
                && marksmanshipSkill >= magerySkill
                && weapon is BaseRanged
            )
            {
                maxRangeFight = 10;
            }
            else if (
                meleeSkillSum >= marksmanshipSkill
                && meleeSkillSum >= magerySkill
                && weapon is BaseMeleeWeapon
            )
            {
                maxRangeFight = 1;
            }
            else if (magerySkill > 0.0)
            {
                maxRangeFight = 9;
            }

            return maxRangeFight;
        }

        public override void OnThink()
        {
            base.OnThink();

            int newRangeFight = CalculateRangeFight(Original);
            if (newRangeFight != RangeFight)
            {
                RangeFight = newRangeFight;
            }

            CalculateBaseDamage();
        }

        public override bool OnBeforeDeath()
        {
            if (this.Mount != null)
            {
                IMount mount = this.Mount;

                if (mount is EtherealMountClone)
                {
                    EtherealMountClone etherealMount = (EtherealMountClone)mount;
                    etherealMount.Delete();
                }
                else if (mount is MountClone)
                {
                    MountClone baseMount = (MountClone)mount;
                    baseMount.Delete();
                }
            }

            return base.OnBeforeDeath();
        }

        public override void OnDeath(Server.Items.Container c)
        {
            base.OnDeath(c);
            if (c != null)
            {
                List<Item> items = new List<Item>(c.Items);
                foreach (Item item in items)
                {
                    SetItemsMovableFalse(item);
                }
            }
        }

        private void SetItemsMovableFalse(Item item)
        {
            Server.Items.Container container = item as Server.Items.Container;
            if (container != null)
            {
                List<Item> subItems = new List<Item>(container.Items);
                foreach (Item subItem in subItems)
                {
                    SetItemsMovableFalse(subItem);
                }
            }
            item.Movable = false;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            if (Original != null)
            {
                Original.GetProperties(list);
            }
            else
            {
                base.GetProperties(list);
            }
        }

        public virtual bool AddHire(Mobile m)
        {
            Mobile owner = GetOwner();
            if (owner != null)
            {
                m.SendLocalizedMessage(1043283, owner.Name);
                return false;
            }

            if (SetControlMaster(m))
            {
                IsHired = true; // Set the IsHired property
                return true;
            }
            return false;
        }

        public virtual bool Payday(CharacterClone m)
        {
            SkillName[] skills = new SkillName[]
            {
                SkillName.Anatomy,
                SkillName.Tactics,
                SkillName.Bludgeoning,
                SkillName.Swords,
                SkillName.Fencing,
                SkillName.Marksmanship,
                SkillName.MagicResist,
                SkillName.Healing,
                SkillName.Magery,
                SkillName.Parry,
                SkillName.Bushido,
                SkillName.Knightship,
                SkillName.Necromancy,
                SkillName.Ninjitsu,
                SkillName.Spiritualism,
                SkillName.Psychology,
                SkillName.Stealth,
                SkillName.Hiding
            };

            m_Pay = 0;

            foreach (SkillName skill in skills)
            {
                m_Pay += (int)m.Skills[skill].Value;
            }

            m_Pay *= 4;

            return true;
        }

        internal void SayHireCost()
        {
            if (m_Pay > 0)
            {
                Say(
                    string.Format(
                        "I am available for hire for {0} gold coins per hour. If thou dost give me gold, I will work for thee.",
                        m_Pay
                    )
                );
            }
            else
            {
                Say("I am not skilled enough to be hired.");
            }
        }

        public virtual Mobile GetOwner()
        {
            if (!Controlled)
                return null;
            Mobile Owner = ControlMaster;
            m_IsHired = true;
            if (Owner == null)
                return null;
            if (Owner.Deleted || Owner.Map != this.Map || !Owner.InRange(Location, 30))
            {
                Say(1005653); // I seem to have lost my master.
                if (Owner != null) // Add this null check
                {
                    CharacterClone.HireTable.Remove(Owner);
                }
                SetControlMaster(null);
                return null;
            }

            return Owner;
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            Mobile Owner = GetOwner();

            if (Owner == null)
            {
                base.GetContextMenuEntries(from, list);
                list.Add(new HireEntry(from, this));
            }
            else
                base.GetContextMenuEntries(from, list);
        }

        public override bool CanRegenHits
        {
            get { return true; }
        }

        public override bool CanTeach
        {
            get { return true; }
        }

        public CharacterClone(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(Original);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            Original = reader.ReadMobile();

            if (Original == null)
            {
                Delete();
            }
        }
    }

    internal class PayTimer : Timer
    {
        private CharacterClone m_Hire;

        public PayTimer(CharacterClone vend)
            : base(TimeSpan.FromMinutes(60.0), TimeSpan.FromMinutes(60.0))
        {
            m_Hire = vend;
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            if (m_Hire != null && !m_Hire.Deleted)
            {
                int m_Pay = m_Hire.Pay;
                if (m_Hire.HoldGold <= m_Pay)
                {
                    Mobile owner = m_Hire.GetOwner();
                    if (owner != null) // Add this null check
                    {
                        m_Hire.Say(
                            "Ah, my purse grows light, and I must depart. Replenish it, and I shall return. Farewell!"
                        );
                        m_Hire.SetControlMaster(null);
                        m_Hire.IsHired = false;
                        CharacterClone.HireTable.Remove(owner);
                    }
                }
                else
                {
                    m_Hire.HoldGold -= m_Pay;
                }
            }
        }
    }

    public class HireEntry : ContextMenuEntry
    {
        private Mobile m_Mobile;
        private CharacterClone m_Hire;

        public HireEntry(Mobile from, CharacterClone hire)
            : base(6120, 3)
        {
            m_Hire = hire;
            m_Mobile = from;
        }

        public override void OnClick()
        {
            m_Hire.Payday(m_Hire);
            m_Hire.SayHireCost();
        }
    }
    #endregion

    #region Other Cloned Stuff
    public class BackpackClone : Backpack
    {
        public BackpackClone()
            : base() { }

        public override bool IsAccessibleTo(Mobile m)
        {
            if (m is PlayerMobile && (m.AccessLevel < AccessLevel.Owner))
            {
                m.SendMessage("You cannot access this clone's backpack.");
                return false;
            }

            return base.IsAccessibleTo(m);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && (from.AccessLevel < AccessLevel.Owner))
            {
                from.SendMessage("You cannot access this clone's backpack.");
                base.OnDoubleClick(from);
            }
        }

        public BackpackClone(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class MountClone : BaseMount
    {
        public MountClone(BaseMount original)
            : base(
                original.Name,
                original.BodyValue,
                original.ItemID,
                original.AI,
                original.FightMode,
                original.RangePerception,
                original.RangeFight,
                original.ActiveSpeed,
                original.PassiveSpeed
            )
        {
            foreach (var property in (typeof(BaseMount)).GetProperties())
            {
                if (property.CanRead && property.CanWrite && property.Name != "Rider")
                {
                    property.SetValue(this, property.GetValue(original, null), null);
                }
            }
        }

        public MountClone(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class EtherealMountClone : EtherealMount
    {
        public EtherealMountClone(EtherealMount original)
            : base(original.RegularID, original.MountedID)
        {
            foreach (var property in (typeof(EtherealMount)).GetProperties())
            {
                if (property.CanRead && property.CanWrite && property.Name != "Rider")
                {
                    property.SetValue(this, property.GetValue(original, null), null);
                }
            }
        }

        public EtherealMountClone(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
    #endregion
}
