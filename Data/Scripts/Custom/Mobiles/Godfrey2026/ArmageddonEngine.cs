using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class ArmageddonEngine : SecurityDroid
    {
        [Constructable]
        public ArmageddonEngine()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Armageddon Engine", null, 7000, 150, 240);
            Godfrey2026Support.SetResistance(this, ResistanceType.Physical);
            Godfrey2026Support.AddMortalStrike(this);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 1);
        }

        public ArmageddonEngine(Serial serial)
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
