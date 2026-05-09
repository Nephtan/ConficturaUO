using System;
using Server;
using Server.Items;

namespace Server.Engines.CannedEvil
{
    public class IdolOfTheChampion : Item
    {
        private ChampionSpawn m_Spawn;

        public ChampionSpawn Spawn
        {
            get { return m_Spawn; }
        }

        public override string DefaultName
        {
            get { return "Idol of the Champion"; }
        }

        public IdolOfTheChampion(ChampionSpawn spawn)
            : base(0x1F18)
        {
            m_Spawn = spawn;
            Movable = false;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Spawn != null)
                m_Spawn.Delete();
        }

        public IdolOfTheChampion(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Spawn);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                {
                    m_Spawn = reader.ReadItem() as ChampionSpawn;

                    if (m_Spawn == null)
                        Delete();

                    break;
                }
            }
        }
    }
}
