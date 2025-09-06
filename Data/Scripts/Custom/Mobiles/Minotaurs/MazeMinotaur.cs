using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for the Castle Basement spawn representing a minotaur captain.
    ///     Generated from the XML spawner definition for 'minotaurcaptain' named 'Maze Minotaur'.
    /// </summary>
    [CorpseName("a minotaur corpse")]
    public class MazeMinotaur : MinotaurCaptain
    {
        [Constructable]
        public MazeMinotaur()
            : base()
        {
            // Identity
            Name = "Maze Minotaur";
            Title = null; // No title in definition

            // Stats from XML definition
            SetHits(1000, 1000);
            SetDamage(180, 210);
        }

        public MazeMinotaur(Serial serial)
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
