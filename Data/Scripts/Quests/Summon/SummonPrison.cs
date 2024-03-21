// Necessary namespaces for the functionality of SummonPrison and related classes.
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

// Define the namespace and class within the Server.Items namespace.
namespace Server.Items
{
    // Defines a SummonPrison item class, extending the base Item class.
    public class SummonPrison : Item
    {
        // Field to store the owner of the SummonPrison.
        public Mobile owner;

        // Property to get/set the owner with GameMaster access level. Represents the owner of the SummonPrison.
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        // Fields for keys and their corresponding properties. These keys are part of the mechanism to unlock the prison.
        public string KeyA;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_KeyA
        {
            get { return KeyA; }
            set { KeyA = value; }
        }

        public string KeyB;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_KeyB
        {
            get { return KeyB; }
            set { KeyB = value; }
        }

        public string KeyC;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_KeyC
        {
            get { return KeyC; }
            set { KeyC = value; }
        }

        // Fields for reagents including their names and quantities, required to initiate the summoning or opening of the prison.
        public string ReagentNameA;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_ReagentNameA
        {
            get { return ReagentNameA; }
            set { ReagentNameA = value; }
        }

        public int ReagentQtyA;

        [CommandProperty(AccessLevel.GameMaster)]
        public int p_ReagentQtyA
        {
            get { return ReagentQtyA; }
            set { ReagentQtyA = value; }
        }

        public string ReagentNameB;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_ReagentNameB
        {
            get { return ReagentNameB; }
            set { ReagentNameB = value; }
        }

        public int ReagentQtyB;

        [CommandProperty(AccessLevel.GameMaster)]
        public int p_ReagentQtyB
        {
            get { return ReagentQtyB; }
            set { ReagentQtyB = value; }
        }

        // Fields and properties for defining the prisoner, including their name, appearance, and serial number.
        public string Prisoner;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_Prisoner
        {
            get { return Prisoner; }
            set { Prisoner = value; }
        }

        public string PrisonerBase;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_PrisonerBase
        {
            get { return PrisonerBase; }
            set { PrisonerBase = value; }
        }

        public int PrisonerBody;

        [CommandProperty(AccessLevel.GameMaster)]
        public int p_PrisonerBody
        {
            get { return PrisonerBody; }
            set { PrisonerBody = value; }
        }

        public int PrisonerHue;

        [CommandProperty(AccessLevel.GameMaster)]
        public int p_PrisonerHue
        {
            get { return PrisonerHue; }
            set { PrisonerHue = value; }
        }

        public Mobile PrisonerSerial;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile p_PrisonerSerial
        {
            get { return PrisonerSerial; }
            set { PrisonerSerial = value; }
        }

        // Fields and properties for customization and metadata, such as cloth color and whether the full name is used.
        public int PrisonerFullNameUsed;

        [CommandProperty(AccessLevel.GameMaster)]
        public int p_PrisonerFullNameUsed
        {
            get { return PrisonerFullNameUsed; }
            set { PrisonerFullNameUsed = value; }
        }

        public int PrisonerClothColorUsed;

        [CommandProperty(AccessLevel.GameMaster)]
        public int p_PrisonerClothColorUsed
        {
            get { return PrisonerClothColorUsed; }
            set { PrisonerClothColorUsed = value; }
        }

        // Fields and properties for defining the jailor and the dungeon context of the prison.
        public string Jailor;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_Jailor
        {
            get { return Jailor; }
            set { Jailor = value; }
        }

        public string Dungeon;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_Dungeon
        {
            get { return Dungeon; }
            set { Dungeon = value; }
        }

        // Fields and properties for defining the reward, including its ID, hue, and name.
        public int RewardID;

        [CommandProperty(AccessLevel.GameMaster)]
        public int p_RewardID
        {
            get { return RewardID; }
            set { RewardID = value; }
        }

        public int RewardHue;

        [CommandProperty(AccessLevel.GameMaster)]
        public int p_RewardHue
        {
            get { return RewardHue; }
            set { RewardHue = value; }
        }

        public string RewardName;

        [CommandProperty(AccessLevel.GameMaster)]
        public string p_RewardName
        {
            get { return RewardName; }
            set { RewardName = value; }
        }

        // Mark the constructor as constructable, allowing it to be created in-game.
        [Constructable]
        public SummonPrison()
            : base(0x4FD6) // Calls the base class constructor with the item ID for the magical prison.
        {
            Weight = 2.0; // Initial weight of the prison.
            Name = "Magical Prison"; // Name of the item.
            Light = LightType.Circle150; // Sets the light emitted by the prison.

            // Ensure the prison's weight is set correctly and initialize its properties.
            if (Weight > 1.0)
            {
                Weight = 1.0; // Adjusts the weight to a standard value.
                Hue = Utility.RandomMinMax(2501, 2644); // Randomizes the color hue of the prison.

                int most = 60; // The upper limit for random choices.
                int choice = 0; // Variable to store the current choice.

                string KeepTrack = "_"; // Initializes a string to keep track of choices made.

                // Randomly selects a number for KeyA, ensuring it hasn't been used before.
                choice = Utility.RandomMinMax(1, most);
                KeepTrack += choice.ToString() + "_";
                while (UsedNumberCheck(KeepTrack, choice))
                {
                    choice = Utility.RandomMinMax(1, most);
                }
                KeepTrack += choice.ToString() + "_";
                KeyA = GetItemNeeded(choice, 3);

                // Repeats the process for KeyB.
                while (UsedNumberCheck(KeepTrack, choice))
                {
                    choice = Utility.RandomMinMax(1, most);
                }
                KeepTrack += choice.ToString() + "_";
                KeyB = GetItemNeeded(choice, 3);

                // And again for KeyC.
                while (UsedNumberCheck(KeepTrack, choice))
                {
                    choice = Utility.RandomMinMax(1, most);
                }
                KeepTrack += choice.ToString() + "_";
                KeyC = GetItemNeeded(choice, 3);

                // Randomly determines the reagents needed for the prison.
                ReagentNameA = GetReagentNeeded();
                ReagentNameB = GetGemsNeeded();

                // Randomizes the quantity of each reagent required.
                ReagentQtyA = Utility.RandomMinMax(10, 70);
                ReagentQtyB = Utility.RandomMinMax(5, 20);

                // Searches the world for potential dungeon locations for the prison.
                ArrayList targets = new ArrayList();
                foreach (Item target in World.Items.Values)
                    if (
                        target is SearchBase
                        && Region.Find(target.Location, target.Map) is DungeonRegion
                    )
                    {
                        targets.Add(target);
                    }

                // Randomly selects one of the discovered dungeons to associate with this prison.
                int aCount = Utility.RandomMinMax(1, targets.Count);
                for (int i = 0; i < targets.Count; ++i)
                {
                    if (i + 1 == aCount)
                    {
                        Item finding = (Item)targets[i];
                        Dungeon = Server.Misc.Worlds.GetRegionName(finding.Map, finding.Location);
                        break; // Exits the loop once the dungeon is assigned.
                    }
                }

                // Randomly generates a name for the jailor and selects a random champion.
                Jailor = RandomThings.GetRandomName() + " " + GetRandomChampion();
                if (Utility.RandomMinMax(1, 4) == 1) // There's a chance to use a different naming scheme.
                {
                    Jailor = RandomThings.GetRandomSociety();
                }

                GetPrisoner(this); // Initializes the prisoner with the specified properties.
            }
        }

        // Overrides the AddNameProperties method to include custom name properties for the SummonPrison item.
        public override void AddNameProperties(ObjectPropertyList list)
        {
            // Formats the prisoner's name to title case for display.
            string sPrisoner =
                System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(
                    Prisoner.ToLower() // Ensures the prisoner's name is in lower case before converting to title case.
                );

            base.AddNameProperties(list); // Calls the base method to add default name properties.

            // Adds a custom property indicating who is imprisoned within the magical prison.
            list.Add(1070722, sPrisoner + " Lies Within"); // The prisoner's name is dynamically inserted into the message.

            // Adds another custom property indicating who imprisoned the creature.
            list.Add(1049644, "Imprisoned By " + Jailor); // The jailor's name is dynamically inserted into the message.
        }

        // Overrides the OnDragLift method to define custom behavior when the item is picked up.
        public override bool OnDragLift(Mobile from)
        {
            // Checks if the entity attempting to pick up the item is a player.
            if (from is PlayerMobile)
            {
                ArrayList targets = new ArrayList(); // Initializes a list to track other SummonPrison items.
                // Iterates through all items in the world to find other SummonPrison items not equal to this instance.
                foreach (Item item in World.Items.Values)
                    if (item is SummonPrison && item != this)
                    {
                        // If the found SummonPrison item belongs to the player attempting the drag, add it to the targets list.
                        if (((SummonPrison)item).owner == from)
                            targets.Add(item);
                    }

                // Deletes all SummonPrison items found in the targets list, ensuring a player cannot own multiple instances simultaneously.
                for (int i = 0; i < targets.Count; ++i)
                {
                    Item item = (Item)targets[i];
                    item.Delete();
                }

                // Formats the prisoner's name to title case for logging or messaging purposes.
                string sPrisoner =
                    System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(
                        Prisoner.ToLower()
                    );

                // If this SummonPrison item does not have an owner, logs the event of the player discovering the prisoner.
                if (this.owner == null)
                {
                    LoggingFunctions.LogGenericQuest(
                        from,
                        "has discovered " + sPrisoner + " locked in a Magical Prison"
                    );
                }

                // Sets the player who lifted the item as its new owner.
                this.owner = from;
            }

            // Always returns true, allowing the drag action to proceed.
            return true;
        }

        // Overrides the OnDoubleClick method to define custom behavior when the item is double-clicked.
        public override void OnDoubleClick(Mobile from)
        {
            // Checks if the player has the necessary items in their pack to summon the prisoner.
            if (
                TestMyPack(
                    from,
                    Dungeon,
                    KeyA,
                    KeyB,
                    KeyC,
                    ReagentQtyA,
                    ReagentNameA,
                    ReagentQtyB,
                    ReagentNameB,
                    false
                )
            )
            {
                // Finds the type of the prisoner to be summoned using the name stored in PrisonerBase.
                Type mobType = ScriptCompiler.FindTypeByName(PrisonerBase);
                // Creates an instance of the prisoner's type.
                Mobile mob = (Mobile)Activator.CreateInstance(mobType);
                BaseCreature monster = (BaseCreature)mob; // Casts the Mobile to a BaseCreature.

                // Adjusts the difficulty of the summoned monster.
                SetDifficultyForMonster(monster);

                Map map = from.Map; // Gets the map where the player is located.

                // Tries to find a suitable location near the player to spawn the creature.
                bool validLocation = false;
                Point3D loc = from.Location;
                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = from.X + Utility.Random(3) - 1; // Randomly adjusts X location.
                    int y = from.Y + Utility.Random(3) - 1; // Randomly adjusts Y location.
                    int z = map.GetAverageZ(x, y); // Gets the average Z location for the new coordinates.

                    // Checks if the location is suitable for spawning.
                    if (validLocation = map.CanFit(x, y, from.Z, 16, false, false))
                        loc = new Point3D(x, y, from.Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                // Overrides the spawn location if a PremiumSpawner is within range.
                foreach (Item i in from.GetItemsInRange(10))
                {
                    if (i is PremiumSpawner)
                    {
                        loc = i.Location;
                    }
                }

                // Sets the appearance of the monster.
                monster.NameHue = 0x22;
                monster.Hue = PrisonerHue;
                if (PrisonerBody > 0)
                {
                    monster.Body = PrisonerBody;
                }
                monster.Title = null; // Clears any title the monster might have.
                monster.Name = Prisoner; // Sets the name of the monster to the prisoner's name.

                // Optionally dresses the monster in specific gear.
                DressUpMonsters(monster, Prisoner);

                // Moves the monster to the world at the determined location and sets the player as its combatant.
                monster.MoveToWorld(loc, map);
                monster.Combatant = from;

                // Visual and sound effects to signal the summoning.
                Effects.SendLocationParticles(
                    EffectItem.Create(monster.Location, monster.Map, EffectItem.DefaultDuration),
                    0x3728,
                    10,
                    10,
                    2023
                );
                monster.PlaySound(0x1FE);

                monster.EmoteHue = 505; // Sets the emote color.
                monster.PackItem(this); // Adds the SummonPrison item to the monster's inventory.
                this.PrisonerSerial = monster; // Links the summoned monster to this prison item.
                this.LootType = LootType.Blessed; // Changes the loot type to Blessed.

                // Removes the necessary items from the player's pack if they are a regular player.
                if (from.AccessLevel == AccessLevel.Player)
                {
                    TestMyPack(
                        from,
                        Dungeon,
                        KeyA,
                        KeyB,
                        KeyC,
                        ReagentQtyA,
                        ReagentNameA,
                        ReagentQtyB,
                        ReagentNameB,
                        true
                    );
                }

                // Starts a timer related to the summoning process.
                SummonTimer thisTimer = new SummonTimer(this);
                thisTimer.Start();
            }
            else
            {
                // If the player lacks the necessary items, sends them a tutorial Gump.
                if (!from.HasGump(typeof(SummonTutorial)))
                {
                    from.SendGump(new SummonTutorial(from, this));
                }
            }
        }

        // Constructor for deserializing an instance of SummonPrison from the data store.
        public SummonPrison(Serial serial)
            : base(serial) { }

        // Serializes the state of the SummonPrison item to the data store.
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Calls the base class serialize method.
            writer.Write((int)1); // version number for future-proofing the serialization format.

            // Writes properties of the SummonPrison to the data store.
            writer.Write((Mobile)owner);
            writer.Write(KeyA);
            writer.Write(KeyB);
            writer.Write(KeyC);
            writer.Write(ReagentNameA);
            writer.Write(ReagentQtyA);
            writer.Write(ReagentNameB);
            writer.Write(ReagentQtyB);
            writer.Write(Prisoner);
            writer.Write(PrisonerHue);
            writer.Write(PrisonerBase);
            writer.Write(PrisonerBody);
            writer.Write(PrisonerFullNameUsed);
            writer.Write(PrisonerClothColorUsed);
            writer.Write((Mobile)PrisonerSerial);
            writer.Write(Jailor);
            writer.Write(Dungeon);
            writer.Write(RewardID);
            writer.Write(RewardHue);
            writer.Write(RewardName);
        }

        // Deserializes the state of the SummonPrison item from the data store.
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Calls the base class deserialize method.
            int version = reader.ReadInt(); // Reads the data format version.

            // Reads properties of the SummonPrison from the data store.
            owner = reader.ReadMobile();
            KeyA = reader.ReadString();
            KeyB = reader.ReadString();
            KeyC = reader.ReadString();
            ReagentNameA = reader.ReadString();
            ReagentQtyA = reader.ReadInt();
            ReagentNameB = reader.ReadString();
            ReagentQtyB = reader.ReadInt();
            Prisoner = reader.ReadString();
            PrisonerHue = reader.ReadInt();
            PrisonerBase = reader.ReadString();
            PrisonerBody = reader.ReadInt();
            PrisonerSerial = reader.ReadMobile();
            PrisonerFullNameUsed = reader.ReadInt();
            PrisonerClothColorUsed = reader.ReadInt();
            Jailor = reader.ReadString();
            Dungeon = reader.ReadString();
            RewardID = reader.ReadInt();
            RewardHue = reader.ReadInt();
            RewardName = reader.ReadString();

            // Additional logic based on the deserialized state.
            if (PrisonerSerial != null)
            {
                // If a prisoner is associated with this item, perform additional setup.
                LeaveThisPlace(this);
            }

            // Sets the item's ID to a specific value post-deserialization.
            ItemID = 0x4FD6;
        }

        // Method to check if a number has already been used, based on its presence in a string.
        public static bool UsedNumberCheck(string info, int numb)
        {
            // Constructs a search string with the number enclosed in underscores.
            // This helps in ensuring that only distinct numbers are matched, preventing partial matches.
            string number = "_" + numb.ToString() + "_";

            // Checks if the constructed string exists within the provided info string.
            if (info.Contains(number))
            {
                // If the number is found within the string, it indicates the number has been used.
                return true;
            }

            // If the number is not found, it indicates the number has not been used.
            return false;
        }

        // Method to check if a player has the required items in their inventory and optionally remove them.
        public static bool TestMyPack(
            Mobile from, // The player to check.
            string region, // The region name where the check is applicable.
            string key1, // Identifier for the first key item.
            string key2, // Identifier for the second key item.
            string key3, // Identifier for the third key item.
            int iReg, // Quantity of the required reagent.
            string sReg, // Identifier for the reagent.
            int iGem, // Quantity of the required gem.
            string sGem, // Identifier for the gem.
            bool remove // Whether to remove the items if found.
        )
        {
            bool pass = true; // Flag to track if all checks pass.

            // Check if the player is in the specified region.
            if (Server.Misc.Worlds.GetRegionName(from.Map, from.Location) != region)
            {
                pass = false; // Fails the check if the player is not in the specified region.
            }
            // Check for the presence of the first key item in the player's inventory.
            if (CheckForItem(from, key1, 0, remove) == false)
            {
                pass = false; // Fails the check if the first key item is not found.
            }
            // Repeat the check for the second key item.
            if (CheckForItem(from, key2, 0, remove) == false)
            {
                pass = false; // Fails the check if the second key item is not found.
            }
            // And again for the third key item.
            if (CheckForItem(from, key3, 0, remove) == false)
            {
                pass = false; // Fails the check if the third key item is not found.
            }
            // Check for the specified quantity of the reagent.
            if (CheckForItem(from, sReg, iReg, remove) == false)
            {
                pass = false; // Fails the check if the required quantity of the reagent is not found.
            }
            // Lastly, check for the specified quantity of the gem.
            if (CheckForItem(from, sGem, iGem, remove) == false)
            {
                pass = false; // Fails the check if the required quantity of the gem is not found.
            }

            return pass; // Returns true if all checks pass, otherwise false.
        }

        // Method to check for the presence of a specific item (or items) in a player's inventory and optionally remove them.
        public static bool CheckForItem(Mobile from, string item, int qty, bool remove)
        {
            bool pass = false; // Initially assumes the check will fail.
            int carry = 0; // Counter for the amount of the specified item found.

            // If a quantity greater than 0 is specified, proceed with checking standard and custom items.
            if (qty > 0)
            {
                // Aggregate all matching items from the player's backpack.
                foreach (Item i in from.Backpack.Items)
                {
                    // Series of if-else statements to match the item by its name and type, then accumulate its quantity.
                    // Each condition checks for a specific item type that corresponds to the provided string identifier.
                    if (item == "black pearls" && i is BlackPearl)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "blood mosses" && i is Bloodmoss)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "cloves of garlic" && i is Garlic)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "ginsengs" && i is Ginseng)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "mandrake roots" && i is MandrakeRoot)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "nightshades" && i is Nightshade)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "spider silks" && i is SpidersSilk)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "sulfurous ashes" && i is SulfurousAsh)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "grave dusts" && i is GraveDust)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "nox crystals" && i is NoxCrystal)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "vials of daemon blood" && i is DaemonBlood)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "bat wings" && i is BatWing)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "pig irons" && i is PigIron)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "eye of toad" && i is EyeOfToad)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "fairy egg" && i is FairyEgg)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "gargoyle ear" && i is GargoyleEar)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "beetle shell" && i is BeetleShell)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "moon crystal" && i is MoonCrystal)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "pixie skull" && i is PixieSkull)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "red lotus" && i is RedLotus)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "sea salt" && i is SeaSalt)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "silver widow" && i is SilverWidow)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "swamp berries" && i is SwampBerries)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "brimstone" && i is Brimstone)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "butterfly wings" && i is ButterflyWings)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "bitter root" && i is BitterRoot)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "black sand" && i is BlackSand)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "blood rose" && i is BloodRose)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "dried toad" && i is DriedToad)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "maggot" && i is Maggot)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "mummy wrap" && i is MummyWrap)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "violet fungus" && i is VioletFungus)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "werewolf claw" && i is WerewolfClaw)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "wolfsbane" && i is Wolfsbane)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "star sapphires" && i is StarSapphire)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "emeralds" && i is Emerald)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "sapphires" && i is Sapphire)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "rubies" && i is Ruby)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "citrines" && i is Citrine)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "amethysts" && i is Amethyst)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "tourmalines" && i is Tourmaline)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "ambers" && i is Amber)
                    {
                        carry = carry + i.Amount;
                    }
                    else if (item == "diamonds" && i is Diamond)
                    {
                        carry = carry + i.Amount;
                    }
                }

                // If the accumulated quantity meets or exceeds the required amount, the check passes.
                if (carry >= qty)
                {
                    pass = true;
                    Container pack = from.Backpack; // Reference to the player's backpack.
                    // If removal is requested and the item matches a specific condition, consume the required quantity from the backpack.
                    if (remove == true && item == "black pearls")
                    {
                        pack.ConsumeTotal(typeof(BlackPearl), qty);
                    }
                    else if (remove == true && item == "blood mosses")
                    {
                        pack.ConsumeTotal(typeof(Bloodmoss), qty);
                    }
                    else if (remove == true && item == "cloves of garlic")
                    {
                        pack.ConsumeTotal(typeof(Garlic), qty);
                    }
                    else if (remove == true && item == "ginsengs")
                    {
                        pack.ConsumeTotal(typeof(Ginseng), qty);
                    }
                    else if (remove == true && item == "mandrake roots")
                    {
                        pack.ConsumeTotal(typeof(MandrakeRoot), qty);
                    }
                    else if (remove == true && item == "nightshades")
                    {
                        pack.ConsumeTotal(typeof(Nightshade), qty);
                    }
                    else if (remove == true && item == "spider silks")
                    {
                        pack.ConsumeTotal(typeof(SpidersSilk), qty);
                    }
                    else if (remove == true && item == "sulfurous ashes")
                    {
                        pack.ConsumeTotal(typeof(SulfurousAsh), qty);
                    }
                    else if (remove == true && item == "grave dusts")
                    {
                        pack.ConsumeTotal(typeof(GraveDust), qty);
                    }
                    else if (remove == true && item == "nox crystals")
                    {
                        pack.ConsumeTotal(typeof(NoxCrystal), qty);
                    }
                    else if (remove == true && item == "vials of daemon blood")
                    {
                        pack.ConsumeTotal(typeof(DaemonBlood), qty);
                    }
                    else if (remove == true && item == "bat wings")
                    {
                        pack.ConsumeTotal(typeof(BatWing), qty);
                    }
                    else if (remove == true && item == "pig irons")
                    {
                        pack.ConsumeTotal(typeof(PigIron), qty);
                    }
                    else if (remove == true && item == "eye of toad")
                    {
                        pack.ConsumeTotal(typeof(EyeOfToad), qty);
                    }
                    else if (remove == true && item == "fairy egg")
                    {
                        pack.ConsumeTotal(typeof(FairyEgg), qty);
                    }
                    else if (remove == true && item == "gargoyle ear")
                    {
                        pack.ConsumeTotal(typeof(GargoyleEar), qty);
                    }
                    else if (remove == true && item == "beetle shell")
                    {
                        pack.ConsumeTotal(typeof(BeetleShell), qty);
                    }
                    else if (remove == true && item == "moon crystal")
                    {
                        pack.ConsumeTotal(typeof(MoonCrystal), qty);
                    }
                    else if (remove == true && item == "pixie skull")
                    {
                        pack.ConsumeTotal(typeof(PixieSkull), qty);
                    }
                    else if (remove == true && item == "red lotus")
                    {
                        pack.ConsumeTotal(typeof(RedLotus), qty);
                    }
                    else if (remove == true && item == "sea salt")
                    {
                        pack.ConsumeTotal(typeof(SeaSalt), qty);
                    }
                    else if (remove == true && item == "silver widow")
                    {
                        pack.ConsumeTotal(typeof(SilverWidow), qty);
                    }
                    else if (remove == true && item == "swamp berries")
                    {
                        pack.ConsumeTotal(typeof(SwampBerries), qty);
                    }
                    else if (remove == true && item == "brimstone")
                    {
                        pack.ConsumeTotal(typeof(Brimstone), qty);
                    }
                    else if (remove == true && item == "butterfly wings")
                    {
                        pack.ConsumeTotal(typeof(ButterflyWings), qty);
                    }
                    else if (remove == true && item == "bitter root")
                    {
                        pack.ConsumeTotal(typeof(BitterRoot), qty);
                    }
                    else if (remove == true && item == "black sand")
                    {
                        pack.ConsumeTotal(typeof(BlackSand), qty);
                    }
                    else if (remove == true && item == "blood rose")
                    {
                        pack.ConsumeTotal(typeof(BloodRose), qty);
                    }
                    else if (remove == true && item == "dried toad")
                    {
                        pack.ConsumeTotal(typeof(DriedToad), qty);
                    }
                    else if (remove == true && item == "maggot")
                    {
                        pack.ConsumeTotal(typeof(Maggot), qty);
                    }
                    else if (remove == true && item == "mummy wrap")
                    {
                        pack.ConsumeTotal(typeof(MummyWrap), qty);
                    }
                    else if (remove == true && item == "violet fungus")
                    {
                        pack.ConsumeTotal(typeof(VioletFungus), qty);
                    }
                    else if (remove == true && item == "werewolf claw")
                    {
                        pack.ConsumeTotal(typeof(WerewolfClaw), qty);
                    }
                    else if (remove == true && item == "wolfsbane")
                    {
                        pack.ConsumeTotal(typeof(Wolfsbane), qty);
                    }
                    else if (remove == true && item == "star sapphires")
                    {
                        pack.ConsumeTotal(typeof(StarSapphire), qty);
                    }
                    else if (remove == true && item == "emeralds")
                    {
                        pack.ConsumeTotal(typeof(Emerald), qty);
                    }
                    else if (remove == true && item == "sapphires")
                    {
                        pack.ConsumeTotal(typeof(Sapphire), qty);
                    }
                    else if (remove == true && item == "rubies")
                    {
                        pack.ConsumeTotal(typeof(Ruby), qty);
                    }
                    else if (remove == true && item == "citrines")
                    {
                        pack.ConsumeTotal(typeof(Citrine), qty);
                    }
                    else if (remove == true && item == "amethysts")
                    {
                        pack.ConsumeTotal(typeof(Amethyst), qty);
                    }
                    else if (remove == true && item == "tourmalines")
                    {
                        pack.ConsumeTotal(typeof(Tourmaline), qty);
                    }
                    else if (remove == true && item == "ambers")
                    {
                        pack.ConsumeTotal(typeof(Amber), qty);
                    }
                    else if (remove == true && item == "diamonds")
                    {
                        pack.ConsumeTotal(typeof(Diamond), qty);
                    }
                }
            }
            else
            {
                // For items without a specified quantity (qty <= 0), perform a different kind of check.
                ArrayList targets = new ArrayList();
                foreach (Item i in from.Backpack.Items)
                {
                    // Check for custom or unique items by their name.
                    if (i is SummonItems && i.Name == item)
                    {
                        pass = true;
                        targets.Add(i); // Collect items for potential removal.
                    }
                }
                // If removal is requested, delete the collected items from the inventory.
                for (int r = 0; r < targets.Count; ++r)
                {
                    Item rid = (Item)targets[r];
                    if (remove == true)
                    {
                        Server.Items.QuestSouvenir.GiveReward(from, rid.Name, rid.Hue, rid.ItemID); // Optionally handle rewards or further processing.
                        rid.Delete(); // Remove the item from the inventory.
                    }
                }
            }

            return pass; // Return the result of the check.
        }

        // Method to randomly select a reagent from a predefined list.
        public static string GetReagentNeeded()
        {
            string sReagent = ""; // Initializes the variable to hold the selected reagent.

            // Randomly selects a number between 1 and 34, each corresponding to a specific reagent.
            switch (Utility.RandomMinMax(1, 34))
            {
                case 1:
                    sReagent = "black pearls";
                    break;
                case 2:
                    sReagent = "blood mosses";
                    break;
                case 3:
                    sReagent = "cloves of garlic";
                    break;
                case 4:
                    sReagent = "ginsengs";
                    break;
                case 5:
                    sReagent = "mandrake roots";
                    break;
                case 6:
                    sReagent = "nightshades";
                    break;
                case 7:
                    sReagent = "spider silks";
                    break;
                case 8:
                    sReagent = "sulfurous ashes";
                    break;
                case 9:
                    sReagent = "grave dusts";
                    break;
                case 10:
                    sReagent = "nox crystals";
                    break;
                case 11:
                    sReagent = "vials of daemon blood";
                    break;
                case 12:
                    sReagent = "bat wings";
                    break;
                case 13:
                    sReagent = "pig irons";
                    break;
                case 14:
                    sReagent = "eye of toad";
                    break;
                case 15:
                    sReagent = "fairy egg";
                    break;
                case 16:
                    sReagent = "gargoyle ear";
                    break;
                case 17:
                    sReagent = "beetle shell";
                    break;
                case 18:
                    sReagent = "moon crystal";
                    break;
                case 19:
                    sReagent = "pixie skull";
                    break;
                case 20:
                    sReagent = "red lotus";
                    break;
                case 21:
                    sReagent = "sea salt";
                    break;
                case 22:
                    sReagent = "silver widow";
                    break;
                case 23:
                    sReagent = "swamp berries";
                    break;
                case 24:
                    sReagent = "brimstone";
                    break;
                case 25:
                    sReagent = "butterfly wings";
                    break;
                case 26:
                    sReagent = "bitter root";
                    break;
                case 27:
                    sReagent = "black sand";
                    break;
                case 28:
                    sReagent = "blood rose";
                    break;
                case 29:
                    sReagent = "dried toad";
                    break;
                case 30:
                    sReagent = "maggot";
                    break;
                case 31:
                    sReagent = "mummy wrap";
                    break;
                case 32:
                    sReagent = "violet fungus";
                    break;
                case 33:
                    sReagent = "werewolf claw";
                    break;
                case 34:
                    sReagent = "wolfsbane";
                    break;
            }

            return sReagent; // Returns the name of the randomly selected reagent.
        }

        // Method to randomly select a gem type from a predefined list.
        public static string GetGemsNeeded()
        {
            string sGem = ""; // Initializes the variable to hold the selected gem.

            // Randomly selects a number between 1 and 9, with each number corresponding to a specific gem type.
            switch (Utility.RandomMinMax(1, 9))
            {
                case 1:
                    sGem = "star sapphires";
                    break;
                case 2:
                    sGem = "emeralds";
                    break;
                case 3:
                    sGem = "sapphires";
                    break;
                case 4:
                    sGem = "rubies";
                    break;
                case 5:
                    sGem = "citrines";
                    break;
                case 6:
                    sGem = "amethysts";
                    break;
                case 7:
                    sGem = "tourmalines";
                    break;
                case 8:
                    sGem = "ambers";
                    break;
                case 9:
                    sGem = "diamonds";
                    break;
                // Cases for other gems are omitted for brevity but follow the same assignment logic.
            }

            return sGem; // Returns the name of the randomly selected gem.
        }

        // Method to randomly select a title for a champion from a predefined list.
        public static string GetRandomChampion()
        {
            // Array of possible titles for champions.
            string[] vTitle = new string[]
            {
                // List includes a diverse range of titles, from traditional fantasy roles to specific ranks or occupations.
                "Adventurer",
                "Bandit",
                "Barbarian",
                "Bard",
                "Baron",
                "Baroness",
                "Cavalier",
                "Cleric",
                "Conjurer",
                "Defender",
                "Diviner",
                "Enchanter",
                "Enchantress",
                "Explorer",
                "Fighter",
                "Gladiator",
                "Heretic",
                "Hunter",
                "Illusionist",
                "Invoker",
                "King",
                "Knight",
                "Lady",
                "Lord",
                "Mage",
                "Magician",
                "Mercenary",
                "Minstrel",
                "Monk",
                "Mystic",
                "Necromancer",
                "Outlaw",
                "Paladin",
                "Priest",
                "Priestess",
                "Prince",
                "Princess",
                "Prophet",
                "Queen",
                "Ranger",
                "Rogue",
                "Sage",
                "Scout",
                "Seeker",
                "Seer",
                "Shaman",
                "Slayer",
                "Sorcerer",
                "Sorcereress",
                "Summoner",
                "Templar",
                "Thief",
                "Traveler",
                "Warlock",
                "Warrior",
                "Witch",
                "Wizard"
            };
            // Randomly selects a title from the array and prefixes it with "the".
            string sTitle = "the " + vTitle[Utility.RandomMinMax(0, (vTitle.Length - 1))];

            return sTitle; // Returns the randomly selected title.
        }

        // Method to assign a randomly selected prisoner and their attributes to a SummonPrison item.
        public static void GetPrisoner(SummonPrison item)
        {
            // Randomly picks a prisoner from a predefined list of 25 possibilities.
            int pick = Utility.RandomMinMax(1, 25);

            // Depending on the random pick, sets various attributes of the prisoner including name, base type, body, hue, and reward details.
            if (pick == 1)
            {
                // Sets prisoner details for "Tiamat the Lord of Dragons".
                item.Prisoner = "Tiamat the Lord of Dragons";
                item.PrisonerBase = "AncientWyrm";
                item.PrisonerBody = 105;
                item.PrisonerHue = 0xA54;
                // Reward details specific to Tiamat.
                item.RewardID = Utility.RandomList(0x2234, 0x2235);
                item.RewardHue = 0xA54;
                item.RewardName = "Head of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 2)
            {
                item.Prisoner = "Bahamut the Platinum Dragon";
                item.PrisonerBase = "AncientWyrm";
                item.PrisonerBody = 105;
                item.PrisonerHue = 0x430;
                item.RewardID = Utility.RandomList(0x2234, 0x2235);
                item.RewardHue = 0x430;
                item.RewardName = "Head of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 3)
            {
                item.Prisoner = "Balar of the Evil Eye";
                item.PrisonerBase = "ShamanicCyclops";
                item.PrisonerBody = 0;
                item.PrisonerHue = 0xB96;
                item.RewardID = 0x2F60;
                item.RewardHue = 0;
                item.RewardName = "Eye of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 4)
            {
                item.Prisoner = "Vecna the Lich";
                item.PrisonerBase = "LichLord";
                item.PrisonerBody = 0;
                item.PrisonerHue = 0xA03;
                item.RewardID = 0x5721;
                item.RewardHue = 0xA03;
                item.RewardName = "Hand of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 5)
            {
                item.Prisoner = "Orcus the Daemon Prince of Undead";
                item.PrisonerBase = "DaemonTemplate";
                item.PrisonerBody = 427;
                item.PrisonerHue = 0xA93;
                item.RewardID = Utility.RandomList(0x369C, 0x364D);
                item.RewardHue = 0;
                item.RewardName = "Wand of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 6)
            {
                item.Prisoner = "Kronos the Ancient Titan";
                item.PrisonerBase = "ElderTitan";
                item.PrisonerBody = 0;
                item.PrisonerHue = 0xB8E;
                item.RewardID = Utility.RandomList(0x35ED, 0x35EF);
                item.RewardHue = 0xB8E;
                item.RewardName = "Throne of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 7)
            {
                item.Prisoner = "Volturnus the Ruler of the Waves";
                item.PrisonerBase = "WaterElemental";
                item.PrisonerBody = 0;
                item.PrisonerHue = 0xA14;
                item.RewardID = Utility.RandomList(0x438E, 0x437E);
                item.RewardHue = 0;
                item.RewardName = "Shrine of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 8)
            {
                item.Prisoner = "Adimarchus the Demon Prince of Madness";
                item.PrisonerBase = "DaemonTemplate";
                item.PrisonerBody = 427;
                item.PrisonerHue = 0xA9E;
                item.RewardID = 0x2104;
                item.RewardHue = 0xA9E;
                item.RewardName = "Statue of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 9)
            {
                item.Prisoner = "Lord Verminaard";
                item.PrisonerBase = "Berserker";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = Utility.RandomSkinColor();
                item.RewardID = 0x2645;
                item.RewardHue = 0x515;
                item.RewardName = "Helm of " + item.Prisoner;
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 1;
            }
            else if (pick == 10)
            {
                item.Prisoner = "Nosferatu the Vampire";
                item.PrisonerBase = "VampireLord";
                item.PrisonerBody = 605;
                item.PrisonerHue = 0x47E;
                item.RewardID = 0x2631;
                item.RewardHue = 0x497;
                item.RewardName = "Spirit of " + item.Prisoner;
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 11)
            {
                item.Prisoner = "Merlin the Wizard";
                item.PrisonerBase = "EvilMageLord";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = Utility.RandomSkinColor();
                item.RewardID = 0x1718;
                item.RewardHue = 0x674;
                item.RewardName = "Hat of " + item.Prisoner;
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 12)
            {
                item.Prisoner = "Saruman the White";
                item.PrisonerBase = "EvilMageLord";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = Utility.RandomSkinColor();
                item.RewardID = Utility.RandomList(0xE89, 0xE8A);
                item.RewardHue = 0x47E;
                item.RewardName = "Staff of " + item.Prisoner;
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 13)
            {
                item.Prisoner = "Elric of Melnibone";
                item.PrisonerBase = "ElfBerserker";
                item.PrisonerBody = 605;
                item.PrisonerHue = 0x430;
                item.RewardID = Utility.RandomList(0x4023, 0x4024);
                item.RewardHue = 0x9C5;
                item.RewardName = item.Prisoner + "'s Ruby Throne";
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 14)
            {
                item.Prisoner = "Conan the Cimmerian";
                item.PrisonerBase = "Berserker";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = 0x419;
                item.RewardID = Utility.RandomList(0x48B0, 0x48B1);
                item.RewardHue = 0;
                item.RewardName = "Battle Axe of " + item.Prisoner;
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 15)
            {
                item.Prisoner = "The Grim Reaper";
                item.PrisonerBase = "BoneKnight";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = 0x47E;
                item.RewardID = Utility.RandomList(0x48C4, 0x48C5);
                item.RewardHue = 0xB92;
                item.RewardName = item.Prisoner + "'s Scythe";
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 1;
            }
            else if (pick == 16)
            {
                item.Prisoner = "Talos the Protector Europa";
                item.PrisonerBase = "CaddelliteGolem";
                item.PrisonerBody = 189;
                item.PrisonerHue = 0x972;
                item.RewardID = Utility.RandomList(0x156C, 0x156D);
                item.RewardHue = 0x972;
                item.RewardName = "Shield of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 17)
            {
                item.Prisoner = "Red Sonja";
                item.PrisonerBase = "Berserker";
                item.PrisonerBody = 0x191;
                item.PrisonerHue = Utility.RandomSkinColor();
                item.RewardID = Utility.RandomList(0x90C, 0x4073);
                item.RewardHue = 0x9C4;
                item.RewardName = item.Prisoner + "'s Sword";
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 1;
            }
            else if (pick == 18)
            {
                item.Prisoner = "Treebeard the Ent";
                item.PrisonerBase = "WalkingReaper";
                item.PrisonerBody = 312;
                item.PrisonerHue = 0x83F;
                item.RewardID = Utility.RandomList(0xE57, 0xE59);
                item.RewardHue = 0x83F;
                item.RewardName = "Stump of " + item.Prisoner;
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 19)
            {
                item.Prisoner = "Zeus of Mount Olympus";
                item.PrisonerBase = "ElderTitan";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = 0x8420;
                item.RewardID = 0x4518;
                item.RewardHue = 0x9C2;
                item.RewardName = "Stone from Mount Olympus";
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 20)
            {
                item.Prisoner = "Thor of Asgard";
                item.PrisonerBase = "Berserker";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = Utility.RandomSkinColor();
                item.RewardID = 0x25C2;
                item.RewardHue = 0x849;
                item.RewardName = "The Serpent of Midgard";
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 21)
            {
                item.Prisoner = "Hercules the Argonaut";
                item.PrisonerBase = "ForestGiant";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = 0x83F3;
                item.RewardID = Utility.RandomList(0x11F7, 0x11FA);
                item.RewardHue = 0x9D3;
                item.RewardName = "Golden Fleece";
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 22)
            {
                item.Prisoner = "King Arthur";
                item.PrisonerBase = "Berserker";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = 0x83F3;
                item.RewardID = Utility.RandomList(0x15CA, 0x15CB);
                item.RewardHue = 0x9C4;
                item.RewardName = "Royal Banner of Camelot";
                item.PrisonerClothColorUsed = 1;
                item.PrisonerFullNameUsed = 1;
            }
            else if (pick == 23)
            {
                item.Prisoner = "Brona the Warlock Lord";
                item.PrisonerBase = "EvilMageLord";
                item.PrisonerBody = 0x190;
                item.PrisonerHue = 0x9E0;
                item.RewardID = Utility.RandomList(0x90C, 0x4073);
                item.RewardHue = 0x496;
                item.RewardName = "Sword of Shannara";
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 24)
            {
                item.Prisoner = "Demogorgon";
                item.PrisonerBase = "DaemonTemplate";
                item.PrisonerBody = 662;
                item.PrisonerHue = 0;
                item.RewardID = Utility.RandomList(0x115F);
                item.RewardHue = 0;
                item.RewardName = "Heart of Demogorgon";
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
            else if (pick == 25)
            {
                item.Prisoner = "Lolth the Drow Goddess";
                item.PrisonerBase = "Succubus";
                item.PrisonerBody = 769;
                item.PrisonerHue = 0;
                item.RewardID = Utility.RandomList(0x39A0);
                item.RewardHue = 0xB60;
                item.RewardName = "Altar of Lolth";
                item.PrisonerClothColorUsed = 0;
                item.PrisonerFullNameUsed = 0;
            }
        }

        // Defines a method to adjust the difficulty level of a monster (BaseCreature).
        public static void SetDifficultyForMonster(BaseCreature bc)
        {
            // Calculate the highest stat (strength, intelligence, or dexterity) of the creature.
            int HighestStat = Math.Max(Math.Max(bc.RawStr, bc.RawInt), bc.RawDex);

            // Determine the amount to increase each stat by, ensuring total stats are at least 1200.
            int BumpUp = 1200 - HighestStat;

            // If the highest stat is less than 1200, increase all stats to meet this threshold.
            if (BumpUp > 0)
            {
                // Increase strength, intelligence, and dexterity by the calculated amount.
                bc.RawStr += BumpUp;
                bc.RawInt += BumpUp;
                bc.RawDex += BumpUp;

                // Adjust the maximum health, mana, and stamina to the new values.
                bc.HitsMaxSeed = (int)(bc.RawStr * 1.5);
                bc.ManaMaxSeed = (int)(bc.RawInt * 1.5);
                bc.StamMaxSeed = (int)(bc.RawDex * 1.5);

                // Set current health, mana, and stamina to their maximum values.
                bc.Hits = bc.HitsMax;
                bc.Mana = bc.ManaMax;
                bc.Stam = bc.StamMax;
            }

            // Instantiate a Random object outside the following loop.
            Random rnd = new Random();

            // Iterate through all skills of the creature.
            for (int i = 0; i < bc.Skills.Length; i++)
            {
                Skill skill = (Skill)bc.Skills[i];

                // If the skill is already developed (base > 0), set it to a random number between 105 and 125.
                if (skill.Base > 0.0)
                    skill.Base = rnd.Next(105, 125);
                // Check if the skill base is 0.0, then increase these skills by a random number from 35 to 85.
                else if (skill.Base == 0.0)
                {
                    skill.Base = rnd.Next(35, 86); // Use the instantiated Random object for number generation.
                }
            }

            // Add additional hit points to the creature to increase its survivability.
            Server.Mobiles.BaseCreature.AdditionalHitPoints(bc, 4);

            // Set the creature's minimum and maximum damage output.
            bc.DamageMin = 22;
            bc.DamageMax = 35;

            // Reset fame and karma to 0.
            bc.Fame = 0;
            bc.Karma = 0;
        }

        // Method to customize the appearance and equipment of monsters based on specific characters or roles.
        public static void DressUpMonsters(Mobile m, string who)
        {
            // Removes existing clothes and items from the monster.
            Server.Misc.MorphingTime.RemoveMyClothes(m);

            // Customizes the monster based on the specified character or role.
            if (who == "Lord Verminaard")
            {
                // Adds specific items to the monster to match the appearance and theme of Lord Verminaard.
                Item helm = new OrcHelm();
                helm.Name = "great helm";
                helm.ItemID = 0x2645;
                m.AddItem(helm);
                m.AddItem(new PlateArms());
                m.AddItem(new PlateGorget());
                m.AddItem(new PlateLegs());
                m.AddItem(new PlateChest());
                m.AddItem(new PlateGloves());
                m.AddItem(new BronzeShield());
                m.AddItem(new WarMace());

                MorphingTime.ColorMyClothes(m, 0x515, 0);

                m.AddItem(new Cloak(0x650));

                m.Female = false;
            }
            else if (who == "Nosferatu the Vampire")
            {
                Item robe = new Robe();
                robe.Name = "vampire robe";
                robe.Hue = 0x497;
                robe.ItemID = 0x2799;
                m.AddItem(robe);
                Item boot = new Boots();
                boot.Hue = 0x497;
                m.AddItem(boot);

                m.Female = false;
            }
            else if (who == "Merlin the Wizard")
            {
                Item robe = new Robe();
                robe.Name = "Merlin's robe";
                robe.Hue = 0x674;
                m.AddItem(robe);
                Item hat = new WizardsHat();
                hat.Name = "Merlin's hat";
                hat.Hue = 0x674;
                m.AddItem(hat);
                Item boot = new Boots();
                boot.Hue = 0x674;
                m.AddItem(boot);

                m.FacialHairItemID = 0x204B;
                m.HairItemID = 0x203C;
                m.FacialHairHue = 0x430;
                m.HairHue = 0x430;

                m.Female = false;
            }
            else if (who == "Saruman the White")
            {
                Item robe = new Robe();
                robe.Name = "Saruman's robe";
                robe.Hue = 0x47E;
                m.AddItem(robe);
                Item staff = new QuarterStaff();
                staff.Name = "Saruman's staff";
                staff.Hue = 0x47E;
                m.AddItem(staff);
                Item boot = new Boots();
                boot.Hue = 0x47E;
                m.AddItem(boot);

                m.FacialHairItemID = 0x204C;
                m.HairItemID = 0x203C;
                m.FacialHairHue = 0x47E;
                m.HairHue = 0x47E;

                m.Female = false;
            }
            else if (who == "Elric of Melnibone")
            {
                PlateChest bk_chest = new PlateChest();
                m.AddItem(bk_chest);
                PlateArms bk_arms = new PlateArms();
                m.AddItem(bk_arms);
                PlateLegs bk_legs = new PlateLegs();
                m.AddItem(bk_legs);
                PlateGorget bk_gorget = new PlateGorget();
                m.AddItem(bk_gorget);
                PlateGloves bk_gloves = new PlateGloves();
                m.AddItem(bk_gloves);
                Longsword bk_sword = new Longsword();
                m.AddItem(bk_sword);
                bk_sword.Attributes.SpellChanneling = 1;

                MorphingTime.ColorMyClothes(m, 0x497, 0);

                ((BaseCreature)m).SetSkill(SkillName.Swords, 125);
                m.FacialHairItemID = 0;
                m.HairItemID = 0x2FCD;
                m.FacialHairHue = 0;
                m.HairHue = 0x47E;

                m.Female = false;
            }
            else if (who == "Conan the Cimmerian")
            {
                Item loin = new LoinCloth();
                loin.Hue = 0x972;
                m.AddItem(loin);
                Item axe = new DoubleAxe();
                m.AddItem(axe);
                Item boot = new Boots();
                boot.Hue = 0x972;
                m.AddItem(boot);

                m.FacialHairItemID = 0;
                m.HairItemID = 0x203C;
                m.FacialHairHue = 0;
                m.HairHue = 0x497;

                m.Female = false;
            }
            else if (who == "The Grim Reaper")
            {
                Item robe = new Robe();
                robe.Hue = 0x497;
                m.AddItem(robe);
                Item scythe = new Scythe();
                scythe.Hue = 0xB92;
                m.AddItem(scythe);
                Item boot = new Boots();
                boot.Hue = 0x497;
                m.AddItem(boot);

                Item helm = new WornHumanDeco();
                helm.Name = "skull";
                helm.ItemID = 0x1451;
                helm.Hue = 0x47E;
                helm.Layer = Layer.Helm;
                m.AddItem(helm);

                Item hands = new WornHumanDeco();
                hands.Name = "bony fingers";
                hands.ItemID = 0x1450;
                hands.Hue = 0x47E;
                hands.Layer = Layer.Gloves;
                m.AddItem(hands);

                Item feet = new WornHumanDeco();
                feet.Name = "bony feet";
                feet.ItemID = 0x170D;
                feet.Hue = 0x47E;
                feet.Layer = Layer.Shoes;
                m.AddItem(feet);

                ((BaseCreature)m).SetSkill(SkillName.Swords, 125);

                m.Female = false;
            }
            else if (who == "Red Sonja")
            {
                Item skirt = new LeatherSkirt();
                skirt.Hue = 0x9C4;
                m.AddItem(skirt);
                Item shirt = new StuddedBustierArms();
                shirt.Hue = 0x9C4;
                m.AddItem(shirt);
                Item sword = new VikingSword();
                sword.Hue = 0x9C4;
                m.AddItem(sword);
                Item boot = new Boots();
                boot.Hue = 0x9C4;
                m.AddItem(boot);

                m.FacialHairItemID = 0;
                m.HairItemID = 0x203C;
                m.FacialHairHue = 0;
                m.HairHue = 0x846;

                m.Female = true;
            }
            else if (who == "Zeus of Mount Olympus")
            {
                Item skirt = new Skirt();
                skirt.Hue = 0x9C2;
                m.AddItem(skirt);
                Item shirt = new BodySash();
                shirt.Hue = 0x9C2;
                m.AddItem(shirt);
                Item staff = new QuarterStaff();
                staff.Hue = 0x9C2;
                ((BaseWeapon)staff).WeaponAttributes.HitLightning = 50;
                m.AddItem(staff);

                m.FacialHairItemID = 0x204B;
                m.HairItemID = 0x203C;
                m.FacialHairHue = 0x9C4;
                m.HairHue = 0x9C4;
                m.BaseSoundID = 0;

                m.Female = false;
            }
            else if (who == "Thor of Asgard")
            {
                m.AddItem(new PlateArms());
                m.AddItem(new PlateGorget());
                m.AddItem(new PlateLegs());
                m.AddItem(new PlateChest());
                m.AddItem(new PlateGloves());

                Item hammer = new WarHammer();
                ((BaseWeapon)hammer).WeaponAttributes.HitLightning = 50;
                m.AddItem(hammer);

                MorphingTime.ColorMyClothes(m, 0x430, 0);

                m.AddItem(new Cloak(0x534));

                m.FacialHairItemID = 0x204B;
                m.HairItemID = 0x203C;
                m.FacialHairHue = 0x8A5;
                m.HairHue = 0x8A5;

                m.Female = false;
            }
            else if (who == "Hercules the Argonaut")
            {
                Item loin = new LoinCloth();
                loin.Hue = 0x9D3;
                m.AddItem(loin);
                Item boot = new Sandals();
                boot.Hue = 0x9D3;
                m.AddItem(boot);

                m.FacialHairItemID = 0x204B;
                m.HairItemID = 0x203D;
                m.FacialHairHue = 0x497;
                m.HairHue = 0x497;

                m.BaseSoundID = 0;

                m.Female = false;
            }
            else if (who == "King Arthur")
            {
                Item cloth2 = new MagicJewelryCirclet();
                cloth2.Name = "crown";
                cloth2.Hue = 0;
                m.AddItem(cloth2);

                m.AddItem(new PlateArms());
                m.AddItem(new PlateGorget());
                m.AddItem(new PlateLegs());
                m.AddItem(new PlateChest());
                m.AddItem(new PlateGloves());
                m.AddItem(new VikingSword());
                m.AddItem(new OrderShield());

                MorphingTime.ColorMyClothes(m, 0x9C4, 0);

                m.AddItem(new Cloak(0x602));

                m.FacialHairItemID = 0x204B;
                m.HairItemID = 0x203C;
                m.FacialHairHue = 0x8FD;
                m.HairHue = 0x8FD;

                m.Female = false;
            }
            else if (who == "Warlock Lord of Shannara")
            {
                Item robe = new Robe();
                robe.Hue = 0x840;
                robe.ItemID = 0x2687;
                m.AddItem(robe);
                Item boots = new Boots();
                boots.Hue = 0x840;
                m.AddItem(boots);
                Item staff = new QuarterStaff();
                staff.Hue = 0x840;
                m.AddItem(staff);

                m.FacialHairItemID = 0;
                m.HairItemID = 0;
                m.BaseSoundID = 0x47D;

                m.Female = false;
            }
            else if (who == "Orcus the Daemon Prince of Undead")
            {
                m.BaseSoundID = 357;
            }
            else if (who == "Adimarchus the Demon Prince of Madness")
            {
                m.BaseSoundID = 357;
            }
            else if (who == "Demogorgon")
            {
                m.BaseSoundID = 357;
            }

            Server.Misc.MorphingTime.SetGender(m);
        }

        // Method to determine the required item's location, guardian, and name based on a random selection for game quests or objectives.
        public static string GetItemNeeded(int item, int part)
        {
            string sWhere = ""; // Location of the item.
            string sWho = ""; // Guardian or character associated with the item.
            string sWhat = ""; // Name of the item.

            // Random selection determines the specific quest item details.
            switch (item)
            {
                // Each case sets the location, guardian, and item name for a specific quest or objective.
                case 1:
                    sWhere = "Stonegate Castle";
                    sWho = "the wyrm of draconic ash";
                    sWhat = "heart of ash";
                    break;
                case 2:
                    sWhere = "the Vault of the Black Knight";
                    sWho = "a mystical wax golem";
                    sWhat = "mystical wax";
                    break;
                case 3:
                    sWhere = "the Crypts of Dracula";
                    sWho = "the son of Dracula";
                    sWhat = "vampire teeth";
                    break;
                case 4:
                    sWhere = "the Lodoria Catacombs";
                    sWho = "the corpse of the ancient king";
                    sWhat = "face of the ancient king";
                    break;
                case 5:
                    sWhere = "Dungeon Deceit";
                    sWho = "Talosh the wizard of fear";
                    sWhat = "wand of Talosh";
                    break;
                case 6:
                    sWhere = "Dungeon Despise";
                    sWho = "Urg the troll warlord";
                    sWhat = "head of Urg";
                    break;
                case 7:
                    sWhere = "Dungeon Destard";
                    sWho = "Dramulox of the shadows";
                    sWhat = "flame of Dramulox";
                    break;
                case 8:
                    sWhere = "the City of Embers";
                    sWho = "Vorgol the baron of flame";
                    sWhat = "crown of Vorgol";
                    break;
                case 9:
                    sWhere = "Dungeon Hythloth";
                    sWho = "Saramon the slayer of souls";
                    sWhat = "claw of Saramon";
                    break;
                case 10:
                    sWhere = "the Ice Fiend Lair";
                    sWho = "the fiend of the frozen hells";
                    sWhat = "horn of the frozen hells";
                    break;
                case 11:
                    sWhere = "Dungeon Shame";
                    sWho = "a salt water elemental";
                    sWhat = "elemental salt";
                    break;
                case 12:
                    sWhere = "Terathan Keep";
                    sWho = "the dragon of blight";
                    sWhat = "eye of plagues";
                    break;
                case 13:
                    sWhere = "the Halls of Undermountain";
                    sWho = "a tangle weed";
                    sWhat = "hair of the earth";
                    break;
                case 14:
                    sWhere = "the Volcanic Cave";
                    sWho = "Turlox the warlord of the sun";
                    sWhat = "skull of Turlox";
                    break;
                case 15:
                    sWhere = "the Mausoleum";
                    sWho = "Mezlo of the green death";
                    sWhat = "tattered robe of Mezlo";
                    break;
                case 16:
                    sWhere = "the Tower of Brass";
                    sWho = "the daemon of the dark forest";
                    sWhat = "blood of the forest";
                    break;
                case 17:
                    sWhere = "Vordo's Dungeon";
                    sWho = "a magma flow";
                    sWhat = "cinders of life";
                    break;
                case 18:
                    sWhere = "the Dragon's Maw";
                    sWho = "the crystal dragon";
                    sWhat = "crystal scales";
                    break;
                case 19:
                    sWhere = "the Ancient Pyramid";
                    sWho = "the pharaoh of suffering";
                    sWhat = "chest of suffering";
                    break;
                case 20:
                    sWhere = "Dungeon Exodus";
                    sWho = "the torturer from below";
                    sWhat = "whip from below";
                    break;
                case 21:
                    sWhere = "the Caverns of Poseidon";
                    sWho = "the naga from the deep";
                    sWhat = "scale of the sea";
                    break;
                case 22:
                    sWhere = "Dungeon Clues";
                    sWho = "Marxas the titan of war";
                    sWhat = "braclet of war";
                    break;
                case 23:
                    sWhere = "Dardin's Pit";
                    sWho = "the ancient reaper";
                    sWhat = "stump of the ancients";
                    break;
                case 24:
                    sWhere = "Dungeon Doom";
                    sWho = "a dark blood elemental";
                    sWhat = "dark blood";
                    break;
                case 25:
                    sWhere = "the Fires of Hell";
                    sWho = "a firescale drake";
                    sWhat = "firescale tooth";
                    break;
                case 26:
                    sWhere = "the Mines of Morinia";
                    sWho = "Xthizx the antaur king";
                    sWhat = "ichor of Xthizx";
                    break;
                case 27:
                    sWhere = "the Perinian Depths";
                    sWho = "the vampire queen";
                    sWhat = "heart of a vampire queen";
                    break;
                case 28:
                    sWhere = "the Dungeon of Time Awaits";
                    sWho = "the daemon of ages";
                    sWhat = "hourglass of ages";
                    break;
                case 29:
                    sWhere = "the Ancient Prison";
                    sWho = "Saramak the forgotten prisoner";
                    sWhat = "shackles of Saramak";
                    break;
                case 30:
                    sWhere = "the Cave of Fire";
                    sWho = "the dragon of embers";
                    sWhat = "mouth of embers";
                    break;
                case 31:
                    sWhere = "the Cave of Souls";
                    sWho = "the corpse of the shadegloom thief";
                    sWhat = "cowl of shadegloom";
                    break;
                case 32:
                    sWhere = "Dungeon Ankh";
                    sWho = "the dutchess of virtue";
                    sWhat = "wedding dress of virtue";
                    break;
                case 33:
                    sWhere = "Dungeon Bane";
                    sWho = "a swamp elemental";
                    sWhat = "lilly pad of the bog";
                    break;
                case 34:
                    sWhere = "Dungeon Hate";
                    sWho = "the immortal one";
                    sWhat = "immortal bones";
                    break;
                case 35:
                    sWhere = "Dungeon Scorn";
                    sWho = "Sylpha the princess of scorn";
                    sWhat = "staff of scorn";
                    break;
                case 36:
                    sWhere = "Dungeon Torment";
                    sWho = "Hertana of vile allurement";
                    sWhat = "mind of allurement";
                    break;
                case 37:
                    sWhere = "Dungeon Vile";
                    sWho = "the wanderer of mystics";
                    sWhat = "mask of the ghost";
                    break;
                case 38:
                    sWhere = "Dungeon Wicked";
                    sWho = "an insect swarm";
                    sWhat = "dead venom flies";
                    break;
                case 39:
                    sWhere = "Dungeon Wrath";
                    sWho = "a reaping willow";
                    sWhat = "branch of the reaper";
                    break;
                case 40:
                    sWhere = "the Flooded Temple";
                    sWho = "a deep sea squid";
                    sWhat = "ink of the deep";
                    break;
                case 41:
                    sWhere = "the Gargoyle Crypts";
                    sWho = "a spirit of a gargoyle priest";
                    sWhat = "amulet of the stygian abyss";
                    break;
                case 42:
                    sWhere = "the Serpent Sanctum";
                    sWho = "Siluphtis the guardian of the sanctum";
                    sWhat = "skin of the guardian";
                    break;
                case 43:
                    sWhere = "the Tomb of the Fallen Wizard";
                    sWho = "the fallen wizard";
                    sWhat = "orb of the fallen wizard";
                    break;
                case 44:
                    sWhere = "the Blood Temple";
                    sWho = "a bloody mist";
                    sWhat = "bleeding crystal";
                    break;
                case 45:
                    sWhere = "the Dungeon of the Mad Archmage";
                    sWho = "the mad archmage";
                    sWhat = "jade idol of Nesfatiti";
                    break;
                case 46:
                    sWhere = "the Tombs";
                    sWho = "the seeker of the words";
                    sWhat = "scroll of Abraxus";
                    break;
                case 47:
                    sWhere = "the Dungeon of the Lich King";
                    sWho = "Permaxumus the ruler of the dark circle";
                    sWhat = "sphere of the dark circle";
                    break;
                case 48:
                    sWhere = "the Forgotten Halls";
                    sWho = "Ulmarek the lich";
                    sWhat = "urn of Ulmarek's ashes";
                    break;
                case 49:
                    sWhere = "the Ice Queen Fortress";
                    sWho = "a greater ice elemental";
                    sWhat = "crystal of everfrost";
                    break;
                case 50:
                    sWhere = "the Halls of Ogrimar";
                    sWho = "the war wizard";
                    sWhat = "tablet of the wizard wars";
                    break;
                case 51:
                    sWhere = "Dungeon Rock";
                    sWho = "the gargoyle of night";
                    sWhat = "stone of the night gargoyle";
                    break;
                case 52:
                    sWhere = "the Scurvy Reef";
                    sWho = "the defiler of the sea";
                    sWhat = "pearl of Neptune";
                    break;
                case 53:
                    sWhere = "the Undersea Castle";
                    sWho = "the coral dragon";
                    sWhat = "Black Beard's brandy";
                    break;
                case 54:
                    sWhere = "the Tomb of Kazibal";
                    sWho = "Tutamak";
                    sWhat = "lamp of the desert";
                    break;
                case 55:
                    sWhere = "the Azure Castle";
                    sWho = "the soul of azure";
                    sWhat = "azure dust";
                    break;
                case 56:
                    sWhere = "the Catacombs of Azerok";
                    sWho = "Azerok of the Deathly Veil";
                    sWhat = "skull of Azerok";
                    break;
                case 57:
                    sWhere = "Dungeon Covetous";
                    sWho = "a harpy hen";
                    sWhat = "egg of the harpy hen";
                    break;
                case 58:
                    sWhere = "the Glacial Scar";
                    sWho = "Murgor the frost giant chief";
                    sWhat = "bone of the frost giant";
                    break;
                case 59:
                    sWhere = "the Temple of Osirus";
                    sWho = "a silver drake";
                    sWhat = "mind of silver";
                    break;
                case 60:
                    sWhere = "the Sanctum of Saltmarsh";
                    sWho = "a silver drake";
                    sWhat = "scale of Scarthis";
                    break;
            }

            // Based on the requested part of the information, returns either location, guardian, or item name.
            if (part == 1)
            {
                return sWhere; // Returns the location if part 1 is requested.
            }
            else if (part == 2)
            {
                return sWho; // Returns the guardian if part 2 is requested.
            }

            // Default return is the item name if part is not 1 or 2.
            return sWhat;
        }

        // Method to remove a summoned prisoner (non-player character) from the game world, typically after their purpose has been fulfilled.
        public static void LeaveThisPlace(SummonPrison item)
        {
            // Retrieves the Mobile object representing the prisoner associated with the given SummonPrison item.
            Mobile lord = item.PrisonerSerial;

            // Checks if the prisoner is not a player character.
            if (!(lord is PlayerMobile))
            {
                // If the prisoner is an NPC, apply a visual effect to indicate their departure.
                Server.Misc.IntelligentAction.BurnAway(lord);
                // Deletes the NPC from the world, effectively removing them from the game.
                lord.Delete();
            }
            // This mechanism ensures that summoned entities are properly cleaned up, maintaining game balance and performance.
        }
    }

    // Defines a Timer subclass specifically for managing the lifetime of a summoned entity or item in the game.
    public class SummonTimer : Timer
    {
        private Item i_item; // Holds a reference to the item associated with the timer.

        // Constructor for the timer, setting it to expire after 60 minutes and initializing with the associated item.
        public SummonTimer(Item item)
            : base(TimeSpan.FromMinutes(60.0)) // Sets the duration of the timer to 60 minutes.
        {
            Priority = TimerPriority.OneMinute; // Sets the timer's check interval to one minute for efficiency.
            i_item = item; // Stores the item passed to the constructor for later use.
        }

        // Override of the OnTick method, called when the timer expires.
        protected override void OnTick()
        {
            // Checks if the item still exists and has not been deleted.
            if ((i_item != null) && (!i_item.Deleted))
            {
                // Calls the LeaveThisPlace method on the SummonPrison item, triggering any cleanup or removal logic.
                SummonPrison.LeaveThisPlace((SummonPrison)i_item);
            }
            // This method ensures that summoned entities or items are automatically cleaned up after their intended lifetime, maintaining game balance and world integrity.
        }
    }
}
