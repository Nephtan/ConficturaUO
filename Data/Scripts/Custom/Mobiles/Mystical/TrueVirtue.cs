using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Archangel champion stationed within the Underground Temple.
    ///     Hard codes the XMLSpawner definition for 'True Virtue'.
    /// </summary>
    public class TrueVirtue : Archangel
    {
        [Constructable]
        public TrueVirtue()
            : base()
        {
            // Identity
            Name = "True Virtue";
            Title = null; // No title specified in the XML definition
            Direction = Direction.South;

            // Health
            HitsMaxSeed = 7000;
            SetHits(7000, 7000);

            // Damage
            SetDamage(240, 290);

            // Combat behaviour
            FightMode = FightMode.Closest;
        }

        public TrueVirtue(Serial serial)
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