using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Skinwalker : Jackalwitch
    {
        [Constructable]
        public Skinwalker()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Skinwalker", null, 800, 180, 200);
            Godfrey2026Support.SetResistance(this, ResistanceType.Energy);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
            Godfrey2026Support.AddMortalStrike(this);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 1);
        }

        public Skinwalker(Serial serial)
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
