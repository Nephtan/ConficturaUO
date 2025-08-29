using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Powerful boss version of the Ancient Lich used in the Dark Dragon Temple encounter.
    ///     This mobile hard codes the XMLSpawner definition for 'The Necromancer'.
    /// </summary>
    [CorpseName("the necromancer's corpse")]
    public class TheNecromancer : AncientLich
    {
        [Constructable]
        public TheNecromancer()
            : base()
        {
            // Basic identity
            Name = "The Necromancer";
            Title = null; // No title per XML definition

            // Massive health pool
            SetHits(5000, 5000);

            // High melee damage output
            SetDamage(200, 250);

            // Maximize resistances
            SetResistance(ResistanceType.Physical, 70, 70);
            SetResistance(ResistanceType.Fire, 100, 100);
            SetResistance(ResistanceType.Cold, 100, 100);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Energy, 100, 100);

            VirtualArmor = 70;

            // Unique loot item – a simple note styled as a book
            SimpleNote note = new SimpleNote
            {
                Name = "Forbidden Book",
                TitleString = "The Black Pyramid",
                NoteString = "In the center of the deepest lake, speak \"Necropotence\" within the darkest temple.",
                ItemID = 17087
            };
            PackItem(note);
        }

        public TheNecromancer(Serial serial)
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