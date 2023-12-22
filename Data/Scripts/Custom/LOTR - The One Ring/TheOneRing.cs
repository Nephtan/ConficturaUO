// LOTR - Ring & Ring Wraith Package
// X-SirSly-X

// This code defines a class for an item called "The One Ring" in a game called RunUO.
// The One Ring is a type of ring that can be equipped by a PlayerMobile.
// When the ring is equipped, it may cause the player to take damage and become hidden,
// and may also spawn a RingWraith mobile that attacks the player.
// The ring has a certain number of charges that are used up each time it is equipped.

using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
    public class TheOneRing : BaseRing
    {
        // The number of charges the ring has remaining
        private int i_charges;

        // Property to get and set the value of i_charges
        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return i_charges; }
            set
            {
                i_charges = value;
                InvalidateProperties();
            }
        }

        // Constructor for the TheOneRing class
        [Constructable]
        public TheOneRing()
            : base(0x108a)
        {
            // Set the name and weight of the ring
            Name = "The One Ring";
            Weight = 1;

            // Set the number of charges for the ring to a random value between 2 and 4
            Charges = Utility.RandomMinMax(2, 4);

            // Set the loot type of the ring to Cursed
            LootType = LootType.Cursed;
        }

        // Overridden method for when the ring is equipped
        public override bool OnEquip(Mobile from)
        {
            // Only continue if the mobile trying to equip the ring is a PlayerMobile
            if (from is PlayerMobile)
            {
                // If the ring has at least one charge remaining
                if (this.Charges >= 1)
                {
                    // Generate a random number between 0 and 2
                    switch (Utility.Random(3))
                    {
                        // If the random number is 0
                        case 0:
                            // Inflict a random amount of damage between 15 and 30 on the player
                            from.Hits -= Utility.RandomMinMax(15, 30);
                            // Send a message to the player indicating that they have taken damage
                            from.SendMessage("You feel pain surge throughout your body...");
                            break;
                    }

                    // Set the player's Hidden flag to true
                    from.Hidden = true;
                    // Set the player's AllowedStealthSteps property to a random number between 1 and 3
                    from.AllowedStealthSteps = Utility.RandomMinMax(10, 30);

                    // Generate a random number between 0 and 4
                    switch (Utility.Random(5))
                    {
                        // If the random number is 0
                        case 0:
                            // Create a new RingWraith mobile and set its Location and Map properties to those of the player
                            RingWraith mob = new RingWraith();
                            mob.MoveToWorld(from.Location, from.Map);
                            // Set the RingWraith's Combatant property to the player
                            mob.Combatant = from;
                            break;
                    }

                    // Set the player's Hidden flag to true, hiding them from other players and NPCs in the game
                    from.Hidden = true;

                    // Set the player's AllowedStealthSteps property to a random number between 1 and 3, determining the number of steps the player can take while hidden before being revealed
                    from.AllowedStealthSteps = Utility.RandomMinMax(10, 30);

                    // Generate a random number between 0 and 4
                    switch (Utility.Random(5))
                    {
                        // If the random number is 0
                        case 0:
                            // Create a new RingWraith mobile and set its Location and Map properties to those of the player
                            RingWraith mob = new RingWraith();
                            mob.MoveToWorld(from.Location, from.Map);

                            // Set the RingWraith's Combatant property to the player
                            mob.Combatant = from;
                            break;
                    }

                    // Decrement the number of charges remaining on the ring
                    this.Charges = this.Charges - 1;

                    // Return true to indicate that the ring was equipped successfully
                    return true;

                    // If the ring has no charges remaining
                    if (this.Charges < 1)
                    {
                        // Send a message to the player indicating that they do not feel strong enough to equip the ring
                        from.SendMessage("You do not feel strong enough to equip the ring...");

                        // Return false to indicate that the ring was not equipped
                        return false;
                    }
                }
            }
            return false;
        }

        // Constructor for the TheOneRing class
        public TheOneRing(Serial serial)
            : base(serial) // Call the base class' serialization constructor
        { }

        // Method for serializing the TheOneRing object
        public override void Serialize(GenericWriter writer)
        {
            // Call the base class' Serialize method
            base.Serialize(writer);

            // Write the current version number of the TheOneRing class to the stream
            writer.Write((int)0);
        }

        // Method for deserializing the TheOneRing object
        public override void Deserialize(GenericReader reader)
        {
            // Call the base class' Deserialize method
            base.Deserialize(reader);

            // Read the version number from the stream
            int version = reader.ReadInt();

            // Set the number of charges on the ring to a random number between 1 and 4
            Charges = Utility.RandomMinMax(1, 4);
        }
    }
}
