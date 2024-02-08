using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("corpse of Nosferatu")]
    public class Nosferatu : VampireLord
    {
        [Constructable]
        public Nosferatu()
            : base()
        {
            Name = "Nosferatu the Vampire";
            Body = 605;
            Hue = 0x47E;
            NameHue = 0x22;
            EmoteHue = 505;
        }

        public override void OnAfterSpawn()
        {
            Server.Misc.IntelligentAction.BeforeMyBirth(this);

            base.OnAfterSpawn();

            SummonPrison.SetDifficultyForMonster(this);

            SummonPrison.DressUpMonsters(this, "Nosferatu the Vampire");

            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            PlaySound(0x1FE);
        }

        public Nosferatu(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_TrueForm);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_TrueForm = reader.ReadBool();
                        break;
                    }
            }
        }
    }
}
