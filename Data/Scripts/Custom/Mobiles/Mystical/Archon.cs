using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded wisp variant spawned as "Archon" within the Underground Temple Dio encounter.
    ///     Applies the XML-defined combat statistics for the Archon mobile.
    /// </summary>
    public class Archon : Wisp
    {
        [Constructable]
        public Archon()
            : base()
        {
            // Identity details from the spawn definition
            Name = "Archon";
            Title = null; // No title provided in the XML entry

            // Durability configuration
            HitsMaxSeed = 300;
            SetHits(300, 300);

            // Damage output configuration
            SetDamage(100, 250);

            // Behavior configuration
            FightMode = FightMode.Evil;
        }

        public Archon(Serial serial)
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