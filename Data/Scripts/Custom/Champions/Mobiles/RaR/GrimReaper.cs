using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Grim Reaper")]
    public class GrimReaper : BoneKnight
    {
        [Constructable]
        public GrimReaper()
            : base()
        {
            Name = "The Grim Reaper";
            Body = 0x190;
            Hue = 0x47E;
            NameHue = 0x22;
            EmoteHue = 505;
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();

            SummonPrison.SetDifficultyForMonster(this);

            SummonPrison.DressUpMonsters(this, "The Grim Reaper");

            // Visual and sound effects to signal the summoning.
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            this.PlaySound(0x1FE);
        }

        public GrimReaper(Serial serial)
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
