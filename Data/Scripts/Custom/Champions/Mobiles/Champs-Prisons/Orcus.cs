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
    [CorpseName("corpse of Orcus")]
    public class Orcus : DaemonTemplate
    {
        [Constructable]
        public Orcus()
            : base()
        {
            Name = "Orcus the Daemon Prince of Undead";
            Title = ""; // Set the title.
            Body = 427;
            Hue = 0xA93;
            NameHue = 0x22;
            EmoteHue = 505;
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();

            SummonPrison.SetDifficultyForMonster(this);

            SummonPrison.DressUpMonsters(this, "Orcus the Daemon Prince of Undead");

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

        public Orcus(Serial serial)
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
