using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for the Underground Temple spawn representing a minotaur captain facing south.
    ///     Generated from the XML spawner definition for 'minotaurcaptain' named 'Maze Minotaur'.
    /// </summary>
    [CorpseName("a minotaur corpse")]
    public class MazeMinotaurSouth : MinotaurCaptain
    {
        [Constructable]
        public MazeMinotaurSouth()
            : base()
        {
            // Identity values taken from the XML definition
            Name = "Maze Minotaur";
            Title = null; // No title specified in the spawn definition
            Direction = Direction.South;

            // Health adjustments from the XML definition
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Configured melee damage range
            SetDamage(180, 210);
        }

        public MazeMinotaurSouth(Serial serial)
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