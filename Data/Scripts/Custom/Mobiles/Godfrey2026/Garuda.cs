using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Garuda : HarpyElder
    {
        [Constructable]
        public Garuda()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Garuda", null, 1000, 120, 150);
            Godfrey2026Support.SetResistance(this, ResistanceType.Fire);
            Godfrey2026Support.AddMortalStrike(this);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 1);
        }

        public Garuda(Serial serial)
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
