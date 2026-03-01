using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded Kuthulu variant for the Lair of Exodus spawn.
    ///     Generated from the XML spawner definition for "Final Exodus" in DioXMLSpawns.md.
    /// </summary>
    [CorpseName("a kuthulu corpse")]
    public class FinalExodus : Kuthulu
    {
        [Constructable]
        public FinalExodus()
            : base()
        {
            // Identity
            Name = "Final Exodus";
            Title = null; // No title specified in the XML definition
            Hue = 0;
            BodyValue = 451;
            BaseSoundID = 357;
            Direction = Direction.South;

            // Health and damage profile
            HitsMaxSeed = 9000;
            SetHits(9000, 9000);
            SetDamage(280, 300);

            // Resistances
            SetResistance(ResistanceType.Physical, 100, 100);
            SetResistance(ResistanceType.Fire, 100, 100);
            SetResistance(ResistanceType.Cold, 100, 100);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Energy, 70, 70);

            // Defensive bonuses
            MeleeDamageAbsorb = 30;

            // Loot additions defined by the XML spawner
            PackItem(new LargeBagofHolding());
            PackItem(new DarkCoreExodus());
            PackItem(new XormiteIngot(50));

            JarsOfWaxMetal metalWax = new JarsOfWaxMetal
            {
                Amount = 10
            };
            PackItem(metalWax);

            PackItem(new RobotSheetMetal(90));
        }

        public FinalExodus(Serial serial)
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