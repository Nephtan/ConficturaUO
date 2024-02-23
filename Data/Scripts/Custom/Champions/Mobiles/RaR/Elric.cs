using System;
using System.Collections;
using Server.ContextMenus;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Elric")]
    public class Elric : ElfBerserker
    {
        [Constructable]
        public Elric()
            : base()
        {
            Name = "Elric of Melnibone";
            Body = 605;
            Hue = 0x430;
            NameHue = 0x22;
            EmoteHue = 505;
        }

        public override void OnAfterSpawn()
        {
            Server.Misc.IntelligentAction.ChooseFighter(this, "drow ");
            base.OnAfterSpawn();

            SummonPrison.SetDifficultyForMonster(this);

            SummonPrison.DressUpMonsters(this, "Elric of Melnibone");

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728,
                10,
                10,
                2023
            );
            PlaySound(0x1FE);
        }

        public Elric(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
