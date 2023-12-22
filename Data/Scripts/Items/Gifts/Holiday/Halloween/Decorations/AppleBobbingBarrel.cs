using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class AppleBobbingBarrel : Item
    {
        // Constructor for the item
        [Constructable]
        public AppleBobbingBarrel()
            : base(0x154D)
        {
            Weight = 100;
            Name = "An Apple Bobbing Barrel";
        }

        // Deserialization constructor
        public AppleBobbingBarrel(Serial serial)
            : base(serial)
        {
            Weight = 100;
            Name = "An Apple Bobbing Barrel";
        }

        // Called when the item is double-clicked
        public override void OnDoubleClick(Mobile from)
        {
            // Check if the player is mounted
            if (from.Mounted)
            {
                from.SendMessage("You cannot bob for apples while mounted.");
                return;
            }

            // Send message and start the animation
            from.SendMessage(
                "You dunk your head in the water trying frantically to sink your teeth into an apple!"
            );
            from.CantWalk = true;
            from.Direction = from.GetDirectionTo(this);
            from.Animate(32, 5, 1, true, true, 0);
            from.PlaySound(37);

            // Set a timer to stop the animation and determine if the player gets an apple after 6 seconds
            Timer.DelayCall(TimeSpan.FromSeconds(6), new TimerStateCallback(StopBobbing), from);
        }

        // Callback for the timer that stops the animation and determines if the player gets an apple
        private void StopBobbing(object state)
        {
            Mobile from = state as Mobile;

            // Make sure the player is still valid
            if (from == null)
                return;

            // Stop the animation and play a splash sound
            from.CantWalk = false;
            from.Animate(32, 5, 1, false, false, 0);
            from.PlaySound(37);

            // Determine if the player gets an apple
            double AppleChance = Utility.RandomDouble();

            if (AppleChance <= .30)
            {
                from.AddToBackpack(new Apple(1));
                from.SendMessage(
                    "You bite into an apple and pull your soaking wet head out of the water!"
                );
                from.PublicOverheadMessage(
                    MessageType.Regular,
                    0xFE,
                    false,
                    "*"
                        + from.Name
                        + " victoriously pulls an apple from the barrel using only their teeth!*"
                );
            }
            else
            {
                from.SendMessage("You fail to bite into any of the apples in the barrel...");
                from.PublicOverheadMessage(
                    MessageType.Regular,
                    0xFE,
                    false,
                    "*" + from.Name + " is soaking wet without an apple to show for it...*"
                );
            }
        }

        // Serialization function
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        // Deserialization function
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Weight = 100;
            Name = "An Apple Bobbing Barrel";
        }
    }
}
