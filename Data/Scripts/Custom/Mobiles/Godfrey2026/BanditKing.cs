using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class BanditKing : Rogue
    {
        [Constructable]
        public BanditKing()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Bandit King", null, 900, 50, 100);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
            Godfrey2026Support.AddMortalStrike(this);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 1);
        }

        public BanditKing(Serial serial)
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
