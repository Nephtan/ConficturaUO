using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class RatRentSpawner : PremiumSpawner
    {
        private const string RequiredQuest = "The Rat's Rent,questholder";
        private const int TriggerRange = 8;
        private const int TriggerSound = 500;

        private bool m_TriggerPending;
        private bool m_QuestSpawn;

        [Constructable]
        public RatRentSpawner()
            : base("SlipperyPete localspawn")
        {
            Name = "02-RatsRent";
            HomeRange = 5;
            WalkingRange = 5;
            Running = false;
        }

        public RatRentSpawner(Serial serial)
            : base(serial)
        {
        }

        public override bool HandlesOnMovement
        {
            get { return Running; }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (!Running || Deleted || Parent != null || m == null || m.Deleted || m.Map != Map
                || !m.Player || m.AccessLevel != AccessLevel.Player || !m.Alive
                || !Utility.InRange(m.Location, Location, TriggerRange) || m_TriggerPending)
                return;

            Defrag();

            if (IsFull || !BaseXmlSpawner.CheckForCarried(m, RequiredQuest))
                return;

            m_TriggerPending = true;
            Timer.DelayCall(TimeSpan.FromSeconds(1), Trigger, m);
        }

        private void Trigger(Mobile player)
        {
            m_TriggerPending = false;

            if (!Running || Deleted || Parent != null || player.Deleted || player.Map != Map
                || !player.Alive || !BaseXmlSpawner.CheckForCarried(player, RequiredQuest))
                return;

            Defrag();

            if (IsFull)
                return;

            player.PlaySound(TriggerSound);
            m_QuestSpawn = true;

            try
            {
                Spawn();
            }
            finally
            {
                m_QuestSpawn = false;
            }
        }

        protected override IEntity CreateSpawnedObject(int index)
        {
            // Pete belongs to the quest trigger even when the ordinary spawn timer runs.
            return m_QuestSpawn ? base.CreateSpawnedObject(index) : null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SlipperyPete : Brigand
    {
        [Constructable]
        public SlipperyPete()
        {
        }

        public SlipperyPete(Serial serial)
            : base(serial)
        {
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();

            Name = "Slippery Pete";
            Title = "the deadbeat";
            RawStr = 150;
            RawDex = 100;
            RawInt = 25;
            Hits = 150;
            DamageMin = 10;
            DamageMax = 20;

            LeatherChest chest = new LeatherChest();
            if (!EquipItem(chest))
                PackItem(chest);

            GoldRing tooth = new GoldRing();
            tooth.Name = "Pete's Brass Tooth";
            PackItem(tooth);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
