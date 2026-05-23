using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Nero : Berserker
    {
        [Constructable]
        public Nero()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Nero", "the Dark", 1800, 150, 200);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
            IsParagon = true;
            Godfrey2026Support.AddMortalStrike(this);
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();
            Godfrey2026Support.ApplyIdentity(this, "Nero", "the Dark");
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 1);
        }

        public Nero(Serial serial)
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
            Godfrey2026Support.ApplyIdentity(this, "Nero", "the Dark");
        }
    }
}
