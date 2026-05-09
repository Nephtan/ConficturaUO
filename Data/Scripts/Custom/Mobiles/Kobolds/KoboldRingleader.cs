using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Commanding kobold warrior rallying troops for the Dark Dragon cult.
    ///     Hard codes the XMLSpawner definition for 'Kobold Ringleader'.
    /// </summary>
    public class KoboldRingleader : OrxWarrior
    {
        [Constructable]
        public KoboldRingleader()
            : base()
        {
            // Identity
            Name = "Kobold Ringleader";
            Title = null; // No title specified in the XML definition
            Direction = Direction.South;

            // Health values
            HitsMaxSeed = 500;
            SetHits(500, 500);

            // Damage output
            SetDamage(50, 100);

            // Orders from the necromancer for nearby kobolds
            SimpleNote note = new SimpleNote
            {
                Name = "Message from the Necromancer",
                TitleString = "Hail Dark Dragon!",
                NoteString = "Minions! Recruit the nearby kobolds to weaken Britain. Then we attack from the Mad Duke's Manor in the west."
            };
            PackItem(note);
        }

        public KoboldRingleader(Serial serial)
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