using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom.Confictura.Champions
{
    public interface IArachnidOutbreakOwned
    {
        ArachnidOutbreakController Controller { get; }
        string AttemptId { get; }
    }

    public class ArachnidOutbreakSpider : DreadSpider, IArachnidOutbreakOwned
    {
        public const int GuardianHealth = 3000;
        public const int DefenderMaximumHealth = 132;
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        private int m_Stage;
        private bool m_Guardian;
        private int m_Reinforcements;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public int Stage { get { return m_Stage; } }
        public bool Guardian { get { return m_Guardian; } }

        public ArachnidOutbreakSpider(ArachnidOutbreakController controller, int stage, bool guardian)
        {
            m_Controller = controller;
            m_AttemptId = controller.AttemptId;
            m_Stage = stage;
            m_Guardian = guardian;
            Tamable = false;
            if (guardian)
            {
                Name = "an arachnid brood guardian";
                SetHits(GuardianHealth);
            }
        }


        public override void OnThink()
        {
            if (m_Controller == null || m_Controller.Deleted)
            {
                Delete();
                return;
            }
            if (!m_Controller.KeepCreatureInBounds(this))
                return;
            base.OnThink();
            if (m_Guardian)
            {
                if ((m_Reinforcements & 8) == 0)
                {
                    m_Reinforcements |= 8;
                    m_Controller.SpawnDefenders(this, m_Stage, 4);
                }
                for (int i = 0; i < 3; ++i)
                {
                    int bit = 1 << i;
                    if ((m_Reinforcements & bit) == 0 && (long)Hits * 100 <= (long)HitsMax * (75 - i * 25))
                    {
                        m_Reinforcements |= bit;
                        m_Controller.SpawnDefenders(this, m_Stage, 2);
                    }
                }
            }
        }

        public override void OnDeath(Container corpse)
        {
            base.OnDeath(corpse);
            if (m_Controller != null)
                m_Controller.CreatureDied(this, corpse);
        }

        public ArachnidOutbreakSpider(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Controller);
            writer.Write(m_AttemptId);
            writer.Write(m_Stage);
            writer.Write(m_Guardian);
            writer.Write(m_Reinforcements);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString();
            m_Stage = reader.ReadInt();
            m_Guardian = reader.ReadBool();
            m_Reinforcements = reader.ReadInt();
        }
    }

    public class ArachnidOutbreakChampion : Mephitis, IArachnidOutbreakOwned
    {
        public const int ChampionHealth = 9000;
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        private int m_Reinforcements;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public override bool AwardLegacyChampionRewards { get { return false; } }
        public virtual Item RollVictoryArtifact() { return GetArtifact(); }

        public ArachnidOutbreakChampion(ArachnidOutbreakController controller)
        {
            m_Controller = controller;
            m_AttemptId = controller.AttemptId;
            SetHits(ChampionHealth);
            Tamable = false;
        }
        public override void OnThink()
        {
            if (m_Controller == null || m_Controller.Deleted)
            {
                Delete();
                return;
            }
            if (!m_Controller.KeepCreatureInBounds(this))
                return;
            base.OnThink();
            for (int i = 0; i < 3; ++i)
            {
                int bit = 1 << i;
                if ((m_Reinforcements & bit) == 0 && (long)Hits * 100 <= (long)HitsMax * (75 - i * 25))
                {
                    m_Reinforcements |= bit;
                    m_Controller.SpawnDefenders(this, 3, 2);
                }
            }
        }
        public override void OnDeath(Container corpse)
        {
            base.OnDeath(corpse);
            if (m_Controller != null)
                m_Controller.CreatureDied(this, corpse);
        }
        public ArachnidOutbreakChampion(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Controller);
            writer.Write(m_AttemptId);
            writer.Write(m_Reinforcements);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString();
            m_Reinforcements = reader.ReadInt();
        }
    }

    public class ArachnidBroodSign : Item, IArachnidOutbreakOwned
    {
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public ArachnidBroodSign(ArachnidOutbreakController controller) : base(0x10D5)
        {
            m_Controller = controller;
            m_AttemptId = controller.AttemptId;
            Movable = false;
            Name = "a nest of thick arachnid webs";
        }
        public ArachnidBroodSign(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Controller);
            writer.Write(m_AttemptId);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString();
        }
    }
}
