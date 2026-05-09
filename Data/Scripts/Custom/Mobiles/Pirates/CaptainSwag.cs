using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom pirate captain from the Booty Bay spawn.
    ///     Hard codes the XMLSpawner definition for 'Captain Swag'.
    /// </summary>
    [CorpseName("a pirate corpse")]
    public class CaptainSwag : PirateCaptain
    {
        [Constructable]
        public CaptainSwag()
            : base()
        {
            // Basic identity
            Name = "Captain Swag";
            Title = null; // No title per XML definition

            // Health and damage
            SetHits(900, 900);
            SetDamage(100, 150);

            // Unique loot item – message from Blackbart Roberts
            SimpleNote note = new SimpleNote
            {
                Name = "Message from Blackbart Roberts",
                NoteString = "Arg, matey! I be taking me ship and crew across the worlds on the Jolly Roger! At any time I could be on any world, pillaging the seas!",
                TitleString = "Location of the Jolly Roger"
            };
            PackItem(note);
        }

        public CaptainSwag(Serial serial)
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
