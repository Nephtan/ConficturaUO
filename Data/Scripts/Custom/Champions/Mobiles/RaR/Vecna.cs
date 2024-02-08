using System;
using System.Collections;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of Vecna")]
    public class Vecna : LichLord
    {
        [Constructable]
        public Vecna()
            : base()
        {
            Name = "Vecna the Lich";
            Title = ""; // Set the title.
            Hue = 0xA03;
            NameHue = 0x22;
            EmoteHue = 505;
        }

        public override void OnAfterSpawn()
        {
            Server.Misc.IntelligentAction.BeforeMyBirth(this);
            SummonPrison.SetDifficultyForMonster(this);

            // Visual and sound effects to signal the summoning.
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            this.PlaySound(0x1FE);
            base.OnAfterSpawn();
        }

        public Vecna(Serial serial)
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
