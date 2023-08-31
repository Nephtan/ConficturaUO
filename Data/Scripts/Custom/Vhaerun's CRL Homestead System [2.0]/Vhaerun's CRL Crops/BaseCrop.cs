// by Korbas
using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

/* Script to display different frames/stages of an item
* (almost like a slow animation).
*/


namespace Server.Items
{
    public abstract class BaseCrop : Item
    {
        public int version;
        private DateTime m_NextAgeCheck;
        private TimeSpan m_AgeDelay;
        private int m_MinAgeTime,
            m_MaxAgeTime;
        private int m_NumAges;
        private int m_CurrentAge = 0;
        private int[] IdList;
        private bool m_FullGrown = false;
        private bool m_Harvestable;
        private bool m_DeleteWhenDone;
        private int m_MinHarvest,
            m_MaxHarvest;
        private CropTimer m_CropTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextAgeCheck
        {
            get { return m_NextAgeCheck; }
            set { m_NextAgeCheck = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan AgeDelay
        {
            get { return m_AgeDelay; }
            set { m_AgeDelay = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinAgeTime
        {
            get { return m_MinAgeTime; }
            set { m_MinAgeTime = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxAgeTime
        {
            get { return m_MaxAgeTime; }
            set { m_MaxAgeTime = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int NumAges
        {
            get { return m_NumAges; }
            set { m_NumAges = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int CurrentAge
        {
            get { return m_CurrentAge; }
            set { m_CurrentAge = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FullGrown
        {
            get { return m_FullGrown; }
            set { m_FullGrown = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Harvestable
        {
            get { return m_Harvestable; }
            set { m_Harvestable = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinHarvest
        {
            get { return m_MinHarvest; }
            set { m_MinHarvest = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHarvest
        {
            get { return m_MaxHarvest; }
            set { m_MaxHarvest = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DeleteWhenDone
        {
            get { return m_DeleteWhenDone; }
            set { m_DeleteWhenDone = value; }
        }

        public int[] AllId
        {
            get { return IdList; }
            set { IdList = value; }
        }

        public override void OnAfterSpawn()
        {
            Server.Mobiles.PremiumSpawner.SpreadItems(this);
            base.OnAfterSpawn();
        }

        public BaseCrop(int ItemID, int MinDelay, int MaxDelay)
            : base(ItemID)
        {
            MinAgeTime = MinDelay;
            MaxAgeTime = MaxDelay;
            Movable = false;

            Start(this, TimeSpan.FromMinutes(Utility.RandomMinMax(MinDelay, MaxDelay)));
        }

        public BaseCrop(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)version); // version

            writer.Write((int)m_MinAgeTime);
            writer.Write((int)m_MaxAgeTime);
            writer.Write((TimeSpan)m_AgeDelay);
            writer.Write((int)m_NumAges);
            writer.Write((int)m_CurrentAge);
            writer.Write((int)m_MinHarvest);
            writer.Write((int)m_MaxHarvest);
            writer.Write((bool)m_Harvestable);
            writer.Write((bool)m_DeleteWhenDone);

            for (int i = 0; i < NumAges; i++)
                writer.Write(IdList[i]);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            version = reader.ReadInt();

            m_MinAgeTime = reader.ReadInt();
            m_MaxAgeTime = reader.ReadInt();
            m_AgeDelay = reader.ReadTimeSpan();
            m_NumAges = reader.ReadInt();
            m_CurrentAge = reader.ReadInt();
            m_MinHarvest = reader.ReadInt();
            m_MaxHarvest = reader.ReadInt();
            m_Harvestable = reader.ReadBool();
            m_DeleteWhenDone = reader.ReadBool();

            int[] IdList = new int[m_NumAges];

            for (int i = 0; i < m_NumAges; i++)
            {
                IdList[i] = reader.ReadInt();
            }

            AllId = IdList;

            Start(this, m_AgeDelay);
        }

        public virtual void Start(BaseCrop item, TimeSpan AgeDelay)
        {
            m_AgeDelay = AgeDelay;

            NextAgeCheck = DateTime.Now + AgeDelay;

            if (m_CropTimer == null)
                m_CropTimer = new CropTimer(this, AgeDelay);

            m_CropTimer.Start();
        }

        public void OnTick()
        {
            NextAgeCheck = DateTime.Now + AgeDelay;

            if (m_CropTimer != null)
                m_CropTimer.Stop();

            if (!FullGrown)
            {
                m_CropTimer = new CropTimer(this, m_AgeDelay);
                m_CropTimer.Start();
            }
            else if (!Harvestable)
            {
                if (DeleteWhenDone)
                    Delete();
                else // Start it over.
                {
                    CurrentAge = 0;
                    FullGrown = false;
                }
            }

            if (CurrentAge > NumAges - 1)
                FullGrown = true;
            else
                ItemID = IdList[CurrentAge];

            CurrentAge++;
        }

        public override void OnDoubleClick(Mobile from)
        {
            Item m_Crop;
            int i,
                HarvestCount = Utility.RandomMinMax(m_MinHarvest, m_MaxHarvest);

            if (!Harvestable)
                return;

            if (!CheckRange(from, 3, this, this.Location))
                return;

            if (FullGrown == true)
            {
                m_Crop = this.FinalItem(HarvestCount);

                if (m_Crop.Stackable)
                {
                    from.AddToBackpack(m_Crop);
                }
                else
                {
                    // should fix item leakage for non-stackable harvests
                    m_Crop.Delete();

                    for (i = 0; i < HarvestCount; i++)
                    {
                        m_Crop = this.FinalItem(1);
                        from.AddToBackpack(m_Crop);
                    }
                }

                if (DeleteWhenDone)
                {
                    this.Delete();
                }
                else
                {
                    CurrentAge = 0;
                    FullGrown = false;
                }
            }
            else
                from.SendMessage("The crop is too young to harvest.");
        }

        private class CropTimer : Timer
        {
            private BaseCrop m_Crop;

            public CropTimer(BaseCrop item, TimeSpan Delay)
                : base(Delay)
            {
                m_Crop = item;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                m_Crop.NextAgeCheck = DateTime.Now + m_Crop.AgeDelay;

                if (m_Crop.Deleted)
                    Stop();

                if (m_Crop == null)
                {
                    m_Crop.Delete();
                    Stop();
                }

                m_Crop.OnTick();
            }
        }

        public virtual bool CheckRange(Mobile from, int Range, BaseCrop m_Crop, Point3D loc)
        {
            bool inRange = (from.Map == m_Crop.Map && from.InRange(loc, Range));

            if (!inRange)
                from.SendLocalizedMessage(500446); // that is too far away.

            return inRange;
        }

        public virtual Item FinalItem(int Count)
        {
            return null;
        }
    }
} // EOF
