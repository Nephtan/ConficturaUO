using System;
using Server.Custom.Confictura;
using Server.Mobiles;

namespace Server.Items
{
    public sealed class CivicBarkeeperSlot : Item
    {
        private Barkeeper m_Barkeeper;
        private Timer m_CheckTimer;

        public Barkeeper Barkeeper
        {
            get { return m_Barkeeper; }
        }

        public CivicBarkeeperSlot()
            : base(0x1F13)
        {
            Movable = false;
            Visible = false;
            Name = "a civic tavern's barkeeper marker";
        }

        public CivicBarkeeperSlot(Serial serial)
            : base(serial)
        {
        }

        public void Place(Point3D location, Map map)
        {
            MoveToWorld(location, map);
            CheckBarkeeper();
        }

        private void CheckBarkeeper()
        {
            if (Deleted || Map == null || Map == Map.Internal)
                return;

            if (m_Barkeeper == null || m_Barkeeper.Deleted)
            {
                Barkeeper barkeeper = new Barkeeper();
                barkeeper.Home = Location;
                barkeeper.RangeHome = 3;
                barkeeper.MoveToWorld(Location, Map);
                m_Barkeeper = barkeeper;
            }

            TimeSpan interval = GovernmentTestingMode.Adjust(TimeSpan.FromSeconds(Utility.RandomMinMax(60, 120)));
            m_CheckTimer = Timer.DelayCall(interval, CheckBarkeeper);
        }

        public override void OnAfterDelete()
        {
            if (m_CheckTimer != null)
                m_CheckTimer.Stop();

            if (m_Barkeeper != null && !m_Barkeeper.Deleted)
                m_Barkeeper.Delete();

            base.OnAfterDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Barkeeper);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Barkeeper = reader.ReadMobile() as Barkeeper;
            m_CheckTimer = Timer.DelayCall(TimeSpan.Zero, CheckBarkeeper);
        }
    }
}
