using System;
using Server;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("corpse of Brona")]
    public class Brona : EvilMageLord
    {
        [Constructable]
        public Brona()
            : base()
        {
            Name = "Brona the Warlock Lord";
            Title = "";
            Body = 0x190;
            Hue = 0x9E0;
            NameHue = 0x22;
            EmoteHue = 505;
        }

        public override void OnAfterSpawn()
        {
            Server.Misc.IntelligentAction.BeforeMyBirth(this);
            base.OnAfterSpawn();

            SummonPrison.SetDifficultyForMonster(this);

            SummonPrison.DressUpMonsters(this, "Brona the Warlock Lord");

            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            PlaySound(0x1FE);
        }

        public Brona(Serial serial)
            : base(serial) { }

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
