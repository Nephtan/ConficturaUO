using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for the Castle Dio spawn representing a service droid variant named "Mana Golem".
    ///     Generated from the XML spawner definition for the ServiceDroid entry in DioXMLSpawns.md.
    /// </summary>
    [CorpseName("a mana golem corpse")]
    public class ManaGolem : ServiceDroid
    {
        [Constructable]
        public ManaGolem()
            : base()
        {
            // Identity
            Name = "Mana Golem";
            Title = null; // No title in definition

            // Health and damage
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);
            SetDamage(50, 100);

            // Behavior settings
            FightMode = FightMode.Evil;
            Direction = Direction.South;
        }

        public ManaGolem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }
}
