using System;
using Server.Custom.Confictura;
using Server.Mobiles;

namespace Server.Items
{
    public sealed class CivicMarketVendorSlot : Item
    {
        private CivicSign m_Sign;
        private string m_VendorType;
        private BaseVendor m_Vendor;
        private Timer m_CheckTimer;

        public CivicSign Sign { get { return m_Sign; } }
        public string VendorType { get { return m_VendorType; } }
        public BaseVendor Vendor { get { return m_Vendor; } }

        public CivicMarketVendorSlot(CivicSign sign, Type type, Point3D at, Map map)
            : this(sign, type, at, map, null)
        {
        }

        private CivicMarketVendorSlot(CivicSign sign, Type type, Point3D at, Map map, BaseVendor vendor)
            : base(0x1F13)
        {
            m_Sign = sign;
            m_VendorType = type.FullName;
            m_Vendor = vendor;
            Movable = false;
            Visible = false;
            Name = "a civic market vendor marker";
            sign.toDelete.Add(this);
            MoveToWorld(at, map);
            if (vendor != null)
            {
                vendor.Home = at;
                vendor.RangeHome = 5;
                vendor.Frozen = true;
                vendor.Direction = Direction.South;
            }
            CheckVendor();
        }

        internal static void Adopt(CivicSign sign, PremiumSpawner source, BaseVendor vendor)
        {
            source.ReleaseCivicVendor(vendor);
            new CivicMarketVendorSlot(sign, vendor.GetType(), source.Location, source.Map, vendor);
            sign.toDelete.Remove(source);
            source.Delete();
        }

        public CivicMarketVendorSlot(Serial serial)
            : base(serial)
        {
        }

        private void CheckVendor()
        {
            if (Deleted || Map == null || Map == Map.Internal || m_Sign == null || m_Sign.Deleted
                || m_Sign.toDelete == null || !m_Sign.toDelete.Contains(this))
                return;

            if (m_Vendor == null || m_Vendor.Deleted || !m_Vendor.Alive)
            {
                Type type = Array.Find(CityLandLord.types, candidate => candidate.FullName == m_VendorType);
                if (type == null)
                    return;

                BaseVendor vendor = (BaseVendor)Activator.CreateInstance(type);
                vendor.Home = Location;
                vendor.RangeHome = 5;
                vendor.OnBeforeSpawn(Location, Map);
                vendor.MoveToWorld(Location, Map);
                Server.Misc.MorphingTime.SetGender(vendor);
                vendor.OnAfterSpawn();
                vendor.Frozen = true;
                vendor.Direction = Direction.South;
                m_Vendor = vendor;
            }

            TimeSpan interval = GovernmentTestingMode.Adjust(TimeSpan.FromSeconds(Utility.RandomMinMax(60, 120)));
            m_CheckTimer = Timer.DelayCall(interval, CheckVendor);
        }

        public override void OnAfterDelete()
        {
            if (m_CheckTimer != null)
                m_CheckTimer.Stop();

            if (m_Vendor != null && !m_Vendor.Deleted)
                m_Vendor.Delete();

            base.OnAfterDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Sign);
            writer.Write(m_VendorType);
            writer.Write(m_Vendor);
        }

        private void RestoreVendor()
        {
            if (m_Vendor != null && !m_Vendor.Deleted)
            {
                m_Vendor.Frozen = true;
                m_Vendor.Direction = Direction.South;
            }

            CheckVendor();
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Sign = reader.ReadItem() as CivicSign;
            m_VendorType = reader.ReadString();
            m_Vendor = reader.ReadMobile() as BaseVendor;
            m_CheckTimer = Timer.DelayCall(TimeSpan.Zero, RestoreVendor);
        }
    }
}
