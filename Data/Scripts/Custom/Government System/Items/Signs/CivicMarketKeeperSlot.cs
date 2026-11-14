using System;
using System.Collections.Generic;
using Server.Custom.Confictura;
using Server.Mobiles;
using Server.Regions;

namespace Server.Items
{
    internal sealed class CivicMarketKeeperSlot : Item
    {
        private CivicSign m_Sign;
        private Rectangle3D[] m_Area;
        private CityLandLord m_Keeper;
        private CityMarketRegion m_Region;
        private int m_Rent;
        private MallDuration m_Duration;
        private DateTime m_RespawnAt;
        private Timer m_Timer;

        internal CityLandLord Keeper { get { return m_Keeper; } }
        internal CityMarketRegion MarketRegion { get { return m_Region; } }

        private CivicMarketKeeperSlot(CivicSign sign, Rectangle3D[] area, Point3D at, Map map)
            : base(0x1F13)
        {
            m_Sign = sign;
            m_Area = area;
            Name = "a civic marketkeeper marker";
            Visible = false;
            Movable = false;
            sign.toDelete.Add(this);
            MoveToWorld(at, map);
        }

        public CivicMarketKeeperSlot(Serial serial)
            : base(serial)
        {
        }

        internal static void Create(CivicSign sign, Rectangle3D[] area, Point3D at, Map map)
        {
            CivicMarketKeeperSlot slot = new CivicMarketKeeperSlot(sign, area, at, map);
            slot.SpawnKeeper();
            slot.m_Keeper.CreateRandomVendors(at, map);
        }

        // This is an exact adoption path for a reviewed legacy market. Never search by proximity.
        internal static CivicMarketKeeperSlot Adopt(CityLandLord keeper, PremiumSpawner spawner)
        {
            if (keeper == null || keeper.Deleted || !keeper.Alive || keeper.Spawner != null
                || spawner == null || spawner.Deleted || keeper.Map == null || keeper.Map == Map.Internal
                || keeper.Map != spawner.Map || keeper.Location != spawner.Location)
                return null;

            CivicSign sign = keeper.Sign;
            CityManagementStone city = keeper.Stone;
            if (city == null || city.Deleted || !city.HasMarket || city.toDelete == null
                || sign == null || sign.Deleted || sign.Type != CivicSignType.Market
                || sign.Stone != city || !city.toDelete.Contains(sign) || sign.LandlordRemove != keeper
                || sign.toDelete == null || !sign.toDelete.Contains(spawner) || sign.Map != keeper.Map
                || keeper.Location != new Point3D(sign.X, sign.Y + 1, sign.Z - 7)
                || keeper.Box == null || keeper.Box.Length == 0)
                return null;

            List<PremiumSpawner> sources = keeper.VendorSpawners;
            if (sources == null || (sources.Count != 0 && sources.Count != 2)
                || spawner.GetSingleCivicVendor() != keeper)
                return null;

            int directSlots = 0;
            int legacySlots = 0;
            foreach (Item item in sign.toDelete)
            {
                if (item == null || item.Deleted)
                    continue;
                if (item is CivicMarketKeeperSlot)
                    return null;
                PremiumSpawner legacy = item as PremiumSpawner;
                if (legacy != null && legacy != spawner)
                {
                    if (!sources.Contains(legacy))
                        return null;
                    ++legacySlots;
                }
                CivicMarketVendorSlot direct = item as CivicMarketVendorSlot;
                if (direct != null)
                {
                    if (direct.Sign != sign)
                        return null;
                    ++directSlots;
                }
            }
            if (legacySlots != sources.Count || directSlots + legacySlots != 2)
                return null;

            // Verify the complete market before detaching any existing vendor.
            List<BaseVendor> vendors = new List<BaseVendor>();
            for (int i = 0; i < sources.Count; ++i)
            {
                PremiumSpawner source = sources[i];
                if (source == null || !sign.toDelete.Contains(source) || source.Map != keeper.Map
                    || source.Location != new Point3D(keeper.X + i + 1, keeper.Y, keeper.Z))
                    return null;
                BaseVendor vendor = source.GetSingleCivicVendor();
                if (vendor == null || Array.IndexOf(CityLandLord.types, vendor.GetType()) < 0)
                    return null;
                vendors.Add(vendor);
            }

            spawner.ReleaseCivicVendor(keeper);
            CivicMarketKeeperSlot slot = new CivicMarketKeeperSlot(sign, keeper.Box, keeper.Location, keeper.Map);
            slot.Attach(keeper);
            for (int i = 0; i < sources.Count; ++i)
                CivicMarketVendorSlot.Adopt(sign, sources[i], vendors[i]);
            sources.Clear();
            if (city.Vendors != null)
            {
                foreach (Mobile vendor in city.Vendors)
                {
                    CityRentedVendor rented = vendor as CityRentedVendor;
                    if (rented != null)
                        rented.BindLegacyMarket(keeper, sign);
                }
            }

            sign.toDelete.Remove(spawner);
            spawner.Delete();
            return slot;
        }

        private bool IsOwned
        {
            get
            {
                return !Deleted && Map != null && Map != Map.Internal && m_Area != null
                    && m_Area.Length > 0 && m_Sign != null && !m_Sign.Deleted && m_Sign.Map == Map
                    && m_Sign.Type == CivicSignType.Market && m_Sign.toDelete != null
                    && m_Sign.toDelete.Contains(this) && m_Sign.Stone != null && !m_Sign.Stone.Deleted
                    && m_Sign.Stone.HasMarket && m_Sign.Stone.toDelete != null
                    && m_Sign.Stone.toDelete.Contains(m_Sign);
            }
        }

        private void Attach(CityLandLord keeper)
        {
            if (keeper.m_Region != null && keeper.m_Region.Registered)
                m_Region = keeper.m_Region;

            if (m_Region == null || !m_Region.Registered)
            {
                m_Region = new CityMarketRegion(m_Sign.Stone, keeper, Map, m_Area, m_Sign);
                m_Region.Register();
            }

            m_Keeper = keeper;
            m_Sign.LandlordRemove = keeper;
            m_Region.BindMarketKeeper(keeper);
            keeper.BindMarketKeeper(this, m_Sign, m_Area, m_Region);
        }

        internal bool KeeperDeleted(CityLandLord keeper)
        {
            if (!IsOwned || m_Keeper != keeper || m_Sign.LandlordRemove != keeper)
                return false;

            m_Rent = keeper.RentCost;
            m_Duration = keeper.Duration;
            m_Keeper = null;
            m_Sign.LandlordRemove = null;
            m_Region.BindMarketKeeper(null);
            m_RespawnAt = DateTime.UtcNow + GovernmentTestingMode.Adjust(
                TimeSpan.FromSeconds(Utility.RandomMinMax(60, 120)));
            ScheduleRespawn();
            return true;
        }

        private void ScheduleRespawn()
        {
            TimeSpan delay = m_RespawnAt - DateTime.UtcNow;
            m_Timer = Timer.DelayCall(delay > TimeSpan.Zero ? delay : TimeSpan.Zero, SpawnKeeper);
        }

        private void SpawnKeeper()
        {
            if (!IsOwned || m_Sign.LandlordRemove != null)
                return;

            CityLandLord keeper = new CityLandLord();
            if (m_RespawnAt != DateTime.MinValue)
            {
                keeper.RentCost = m_Rent;
                keeper.Duration = m_Duration;
            }

            keeper.Home = Location;
            keeper.RangeHome = 5;
            keeper.OnBeforeSpawn(Location, Map);
            keeper.MoveToWorld(Location, Map);
            Server.Misc.MorphingTime.SetGender(keeper);
            keeper.OnAfterSpawn();
            Attach(keeper);
            m_RespawnAt = DateTime.MinValue;
        }

        public override void OnAfterDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            if (m_Keeper != null && !m_Keeper.Deleted)
            {
                m_Keeper.Delete();
                if (m_Sign.LandlordRemove == m_Keeper)
                    m_Sign.LandlordRemove = null;
            }
            else if (m_Region != null)
            {
                m_Region.Unregister();
                if (m_Sign != null && m_Sign.Stone != null && !m_Sign.Stone.Deleted)
                    m_Sign.Stone.CheckVendors(false);
            }

            base.OnAfterDelete();
        }

        private void Restore()
        {
            if (!IsOwned)
                return;

            if (m_Keeper != null && !m_Keeper.Deleted && m_Sign.LandlordRemove == m_Keeper
                && m_Keeper.Sign == m_Sign && m_Keeper.Stone == m_Sign.Stone
                && m_Keeper.Map == Map && m_Keeper.Location == Location)
                Attach(m_Keeper);
            else if (m_Keeper == null && m_Sign.LandlordRemove == null && m_RespawnAt != DateTime.MinValue)
            {
                m_Region = new CityMarketRegion(m_Sign.Stone, null, Map, m_Area, m_Sign);
                m_Region.Register();
                ScheduleRespawn();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Sign);
            CityManagementStone.WriteRect3DArray(writer, m_Area);
            writer.Write(m_Keeper);
            writer.Write(m_Rent);
            writer.Write((int)m_Duration);
            writer.Write(m_RespawnAt);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Sign = reader.ReadItem() as CivicSign;
            m_Area = CityManagementStone.ReadRect3DArray(reader);
            m_Keeper = reader.ReadMobile() as CityLandLord;
            m_Rent = reader.ReadInt();
            m_Duration = (MallDuration)reader.ReadInt();
            m_RespawnAt = reader.ReadDateTime();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, Restore);
        }
    }
}
