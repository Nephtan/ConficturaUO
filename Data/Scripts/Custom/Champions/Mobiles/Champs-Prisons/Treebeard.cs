using System;
using Server;
using Server.Engines.Plants;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("corpse of Treebeard")]
    public class Treebeard : WalkingReaper
    {
        [Constructable]
        public Treebeard()
            : base()
        {
            Name = "Treebeard the Ent";
            Body = 312;
            Hue = 0x83F;
            NameHue = 0x22;
            EmoteHue = 505;
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();

            SummonPrison.SetDifficultyForMonster(this);

            // Visual and sound effects to signal the summoning.
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3728,
                10,
                10,
                2023
            );
            this.PlaySound(0x1FE);
        }

        public Treebeard(Serial serial)
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
