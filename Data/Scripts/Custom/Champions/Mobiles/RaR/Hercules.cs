using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Misc;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of Hercules")]
    public class Hercules : ForestGiant
    {
        [Constructable]
        public Hercules()
            : base()
        {
            Name = "Hercules the Argonaut";
            Body = 0x190;
            Hue = 0x83F3;
            NameHue = 0x22;
            EmoteHue = 505;
        }

        public override void OnAfterSpawn()
        {
            Server.Misc.IntelligentAction.ChooseFighter(this, "");
            base.OnAfterSpawn();

            SummonPrison.SetDifficultyForMonster(this);

            SummonPrison.DressUpMonsters(this, "Hercules the Argonaut");

            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            PlaySound(0x1FE);
        }

        public Hercules(Serial serial)
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
