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

            UpdateMarketRegion();
            CreateRandomVendors(loc, map);
        }

        public void UpdateMarketRegion()
        {
            m_Region = new CityMarketRegion(m_Stone, this, this.Map, m_Area, m_Sign);
            m_Region.Register();
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
            m_Region.Unregister();

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

        public void CreateRandomVendors(Point3D p, Map map)
        {
            if (m_vendorSpawners.Count > 0)
            {
                foreach (PremiumSpawner sp in m_vendorSpawners)
                    sp.Delete();

                m_vendorSpawners.Clear();
            }

            int index = Utility.RandomMinMax(0, types.Length - 1);
            int index2 = Utility.RandomMinMax(0, types.Length - 1);
            while (index == index2)
                index2 = Utility.RandomMinMax(0, types.Length - 1);

            Type type1 = types[index];
            Type type2 = types[index2];

            PremiumSpawner sp1 = new PremiumSpawner(type1.Name);
            sp1.Count = 1;
            sp1.Running = true;
            sp1.HomeRange = 5;
                sp1.MinDelay = GovernmentTestingMode.Adjust(TimeSpan.FromMinutes(1));
                sp1.MaxDelay = GovernmentTestingMode.Adjust(TimeSpan.FromMinutes(2));
            sp1.Visible = false;
            sp1.Movable = false;
            sp1.MoveToWorld(new Point3D(p.X + 1, p.Y, p.Z), map);
            sp1.Respawn();

            PremiumSpawner sp2 = new PremiumSpawner(type2.Name);
            sp2.Count = 1;
            sp2.Running = true;
            sp2.HomeRange = 5;
                sp2.MinDelay = GovernmentTestingMode.Adjust(TimeSpan.FromMinutes(1));
                sp2.MaxDelay = GovernmentTestingMode.Adjust(TimeSpan.FromMinutes(2));
            sp2.Visible = false;
            sp2.Movable = false;
            sp2.MoveToWorld(new Point3D(p.X + 2, p.Y, p.Z), map);
            sp2.Respawn();

            foreach (Mobile m in sp1.GetMobilesInRange(0))
            {
                m.Frozen = true;
                m.Direction = Direction.South;
            }

            foreach (Mobile m in sp2.GetMobilesInRange(0))
            {
                m.Frozen = true;
                m.Direction = Direction.South;
            }

            m_vendorSpawners.Add(sp1);
            m_vendorSpawners.Add(sp2);

            if (m_Sign != null && m_Sign.toDelete != null)
            {
                m_Sign.toDelete.Add(sp1);
                m_Sign.toDelete.Add(sp2);
            }
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

            foreach (PremiumSpawner sp in m_vendorSpawners)
            {
                sp.Respawn();
                foreach (Mobile m in sp.GetMobilesInRange(0))
                {
                    m.Frozen = true;
                    m.Direction = Direction.South;
                }

                if (m_Sign != null && m_Sign.toDelete != null && !m_Sign.toDelete.Contains(sp))
                    m_Sign.toDelete.Add(sp);
            }

            UpdateMarketRegion();
        }
    }
}
