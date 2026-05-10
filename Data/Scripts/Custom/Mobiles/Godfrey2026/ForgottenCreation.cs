using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class ForgottenCreation : RustGolem
    {
        [Constructable]
        public ForgottenCreation()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Forgotten Creation", null, 800, 100, 150);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 1);
        }

        public ForgottenCreation(Serial serial)
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
