using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Unique wailing banshee haunting Mad Duke's Manor.
    ///     Hard codes the XMLSpawner definition for 'Ghost of Lady Everdear'.
    /// </summary>
    public class GhostOfLadyEverdear : WailingBanshee
    {
        [Constructable]
        public GhostOfLadyEverdear()
            : base()
        {
            // Identity
            Name = "Ghost of Lady Everdear";
            Title = null; // No title specified in the XML definition

            // Survivability
            HitsMaxSeed = 300;
            SetHits(300, 300);

            // Damage profile
            SetDamage(50, 100);

            // Maxed resistances granted by the XML definition
            SetResistance(ResistanceType.Physical, 100, 100);
            SetResistance(ResistanceType.Fire, 100, 100);
            SetResistance(ResistanceType.Cold, 100, 100);
            SetResistance(ResistanceType.Poison, 100, 100);

            // Unique loot – sealed message scroll from the Mad Duke's wife
            SimpleNote note = new SimpleNote
            {
                Name = "Message Scroll (it is sealed with the Mad Duke's signet)",
                TitleString = "The last account of a Lady",
                NoteString = "The thing that was once my husband is now sealed away in the tower. Speak his name at the door to end his suffering."
            };
            PackItem(note);
        }

        public GhostOfLadyEverdear(Serial serial)
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