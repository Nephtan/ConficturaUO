using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class GreatOldOne : DemonOfTheSea
    {
        [Constructable]
        public GreatOldOne()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Great Old One", null, 1800, 140, 170);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
            Godfrey2026Support.SetResistance(this, ResistanceType.Energy);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
            Godfrey2026Support.AddMortalStrike(this);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 1);
        }

        public GreatOldOne(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
