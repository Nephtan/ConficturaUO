using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Mutated adventurer encountered in the Dark Dragon Temple.
    ///     Hard codes the XMLSpawner definition for 'Cursed Adventurer'.
    /// </summary>
    [CorpseName("a mutant corpse")]
    public class CursedAdventurer : Mutant
    {
        [Constructable]
        public CursedAdventurer()
            : base()
        {
            // Identity
            Name = "Cursed Adventurer";
            Title = null; // No title specified
            Hue = 0;

            // Health
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Damage
            SetDamage(200, 220);

            // Physical resistance
            SetResistance(ResistanceType.Physical, 90, 90);
        }

        public CursedAdventurer(Serial serial)
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
