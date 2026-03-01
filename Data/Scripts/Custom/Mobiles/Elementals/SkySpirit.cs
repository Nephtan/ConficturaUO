using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Air elemental spirit from the Elemental Chaos spawn.
    ///     Hard codes the XMLSpawner definition for 'Sky Spirit'.
    /// </summary>
    [CorpseName("a spirit corpse")]
    public class SkySpirit : ElementalSpiritAir
    {
        [Constructable]
        public SkySpirit()
            : base()
        {
            // Identity
            Name = "Sky Spirit";
            Title = null; // No title specified in the XML definition

            // Health
            HitsMaxSeed = 600;
            SetHits(600, 600);

            // Damage range
            SetDamage(90, 190);

            // Resistances
            SetResistance(ResistanceType.Physical, 100, 100);
            SetResistance(ResistanceType.Cold, 100, 100);
            SetResistance(ResistanceType.Fire, 100, 100);
            SetResistance(ResistanceType.Poison, 1, 1);
        }

        public SkySpirit(Serial serial)
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