using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class TreasureGoblin : Goblin
    {
        [Constructable]
        public TreasureGoblin()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Treasure Goblin", null, 900, 80, 100);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
            Godfrey2026Support.SetResistance(this, ResistanceType.Fire);
            Godfrey2026Support.SetResistance(this, ResistanceType.Energy);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 4);
        }

        public TreasureGoblin(Serial serial)
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
