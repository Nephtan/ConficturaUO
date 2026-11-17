using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Regions;
using Server.Custom.Confictura;

namespace Server.Mobiles
{
    public class CityLandLord : BaseLandLord
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get { return m_SBInfos; }
        }
        private CityManagementStone m_Stone;
        private Rectangle3D[] m_Area;
        public CityMarketRegion m_Region;
        private CivicSign m_Sign;
        private CivicMarketKeeperSlot m_KeeperSlot;
        private List<PremiumSpawner> m_vendorSpawners;

        public List<PremiumSpawner> VendorSpawners
        {
            get { return m_vendorSpawners; }
            set { m_vendorSpawners = value; }
        }

        public CivicSign Sign
        {
            get { return m_Sign; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public CityManagementStone Stone
        {
            get { return m_Stone; }
        }

        public Rectangle3D[] Box
        {
            get { return m_Area; }
        }

        public static Type[] types = new Type[] //Types of random vendors that can be created.
        {
            typeof(Weaver),
            typeof(Tailor),
            typeof(Blacksmith),
            typeof(Butcher),
            typeof(Tanner),
            typeof(Armorer),
            typeof(AnimalTrainer),
            typeof(Carpenter),
            typeof(Mage),
            typeof(Tinker),
            typeof(Fisherman),
            typeof(Alchemist),
            typeof(Herbalist),
            typeof(InnKeeper),
            typeof(Veterinarian),
            typeof(Shipwright),
            typeof(Miner),
            typeof(Miller),
            typeof(Farmer),
            typeof(Bard)
        };

        [Constructable]
        public CityLandLord()
            : base("Marketkeeper")
        {
            Frozen = true;
            m_vendorSpawners = new List<PremiumSpawner>();
        }

        [Constructable]
        public CityLandLord(
            CityManagementStone stone,
            Rectangle3D[] area,
            CivicSign sign,
            Point3D p,
            Map map
        )
            : this()
        {
            Initialize(stone, area, sign, p, map);
        }

        public void Initialize(
            CityManagementStone stone,
            Rectangle3D[] area,
            CivicSign sign,
            Point3D p,
            Map map
        )
        {
            Point3D loc = new Point3D(p.X - 4, p.Y, p.Z);
            MoveToWorld(loc, map);
            Direction = Direction.South;
            m_Stone = stone;
            m_Area = area;
            m_Sign = sign;
            if (sign.toDelete == null)
                sign.toDelete = new ArrayList();

            UpdateMarketRegion();
            CreateRandomVendors(loc, map);
        }

        public void UpdateMarketRegion()
        {
            if (m_Region != null && m_Region.Registered)
                return;

            m_Region = new CityMarketRegion(m_Stone, this, this.Map, m_Area, m_Sign);
            m_Region.Register();
        }

        internal void BindMarketKeeper(
            CivicMarketKeeperSlot slot, CivicSign sign, Rectangle3D[] area, CityMarketRegion region)
        {
            m_KeeperSlot = slot;
            m_Sign = sign;
            m_Stone = sign.Stone;
            m_Area = area;
            m_Region = region;
            Home = slot.Location;
            RangeHome = 5;
            Frozen = true;
            Direction = Direction.South;
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCityLandLord());
        }

        public override void SayPriceTo(Mobile m)
        {
            PlayerMobile pm = (PlayerMobile)m;
            if (pm.City != m_Stone)
                SayTo(
                    m,
                    String.Format("To rent a spot in this town's market pay me {0} gold.", RentCost)
                );
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pm = (PlayerMobile)from;
            if (pm.City != null && pm == pm.City.Mayor && pm.City == m_Stone)
            {
                from.SendGump(new CityMarketGump(this));
            }
            else
                SayPriceTo(from);
        }

        public override bool OnGoldGiven(Mobile from, Gold dropped)
        {
            PlayerMobile pm = (PlayerMobile)from;
            if (m_Stone == pm.City)
            {
                from.SendMessage(
                    "You are a member of the city and do not need to rent a spot to place your vendor!"
                );
                return false;
            }
            else if (dropped.Amount == RentCost)
            {
                CityMallToken token = new CityMallToken(
                    this,
                    m_Sign,
                    from,
                    this.Duration,
                    RentCost
                );
                from.AddToBackpack(token);
                SayTo(
                    from,
                    "You have purchased a spot for your vendor.  Stand in the place you wish to place them and doubleclick the token."
                );
                this.Stone.CityTreasury += dropped.Amount;
                return true;
            }
            else
            {
                SayTo(from, "That's not the right amount!");
                SayPriceTo(from);
                return false;
            }
        }

        public override void OnDelete()
        {
            if (m_KeeperSlot != null && m_KeeperSlot.KeeperDeleted(this))
            {
                base.OnDelete();
                return;
            }

            if (m_Region != null)
                m_Region.Unregister();

            DeleteVendorSlots(false);

            if (m_vendorSpawners.Count > 0)
            {
                foreach (PremiumSpawner sp in m_vendorSpawners)
                {
                    sp.Delete();
                }
                m_vendorSpawners.Clear();
            }

            if (this.Stone != null)
            {
                this.Stone.CheckVendors(false);
            }
        }

        private void DeleteVendorSlots(bool removeFromSign)
        {
            if (m_Sign == null || m_Sign.toDelete == null)
                return;

            List<CivicMarketVendorSlot> owned = new List<CivicMarketVendorSlot>();
            foreach (Item item in m_Sign.toDelete)
            {
                CivicMarketVendorSlot slot = item as CivicMarketVendorSlot;
                if (slot != null && slot.Sign == m_Sign)
                    owned.Add(slot);
            }

            foreach (CivicMarketVendorSlot slot in owned)
            {
                slot.Delete();
                if (removeFromSign)
                    m_Sign.toDelete.Remove(slot);
            }
        }

        public void CreateRandomVendors(Point3D p, Map map)
        {
            if (Deleted || m_Sign == null || m_Sign.Deleted || m_Sign.toDelete == null
                || map == null || map == Map.Internal)
                return;

            DeleteVendorSlots(true);
            foreach (PremiumSpawner spawner in m_vendorSpawners)
            {
                spawner.Delete();
                m_Sign.toDelete.Remove(spawner);
            }
            m_vendorSpawners.Clear();

            int first = Utility.Random(types.Length);
            int second = Utility.Random(types.Length - 1);
            if (second >= first)
                ++second;

            new CivicMarketVendorSlot(m_Sign, types[first], new Point3D(p.X + 1, p.Y, p.Z), map);
            new CivicMarketVendorSlot(m_Sign, types[second], new Point3D(p.X + 2, p.Y, p.Z), map);
        }

        public CityLandLord(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.WriteItemList<PremiumSpawner>(m_vendorSpawners, true);
            writer.Write(m_Sign);
            writer.Write(m_Stone);
            Server.Items.CityManagementStone.WriteRect3DArray(writer, m_Area);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            ArrayList list = reader.ReadItemList();
            m_vendorSpawners = new List<PremiumSpawner>(list.Count);
            foreach (Item item in list)
            {
                PremiumSpawner sp = item as PremiumSpawner;
                if (sp != null)
                {
                    m_vendorSpawners.Add(sp);
                }
            }
            m_Sign = (CivicSign)reader.ReadItem();
            m_Stone = (CityManagementStone)reader.ReadItem();
            m_Area = Server.Items.CityManagementStone.ReadRect3DArray(reader);

            Frozen = true;
            // Spawner item data is read after mobile data during World.Load.
            Timer.DelayCall(TimeSpan.Zero, RestoreMarket);
        }

        private void RestoreMarket()
        {
            if (Deleted || m_Stone == null || m_Stone.Deleted || m_Sign == null || m_Sign.Deleted
                || m_Area == null || m_Area.Length == 0 || Map == null || Map == Map.Internal)
                return;

            UpdateMarketRegion();
            foreach (PremiumSpawner spawner in m_vendorSpawners)
            {
                if (spawner == null || spawner.Deleted)
                    continue;

                Dictionary<Mobile, bool> vendors = new Dictionary<Mobile, bool>();
                spawner.CollectLivingChildren(vendors);
                if (vendors.Count == 0 && spawner.Running)
                {
                    spawner.Respawn();
                    spawner.CollectLivingChildren(vendors);
                }

                foreach (Mobile vendor in vendors.Keys)
                {
                    vendor.Frozen = true;
                    vendor.Direction = Direction.South;
                }

                if (m_Sign.toDelete != null && !m_Sign.toDelete.Contains(spawner))
                    m_Sign.toDelete.Add(spawner);
            }
        }
    }
}
