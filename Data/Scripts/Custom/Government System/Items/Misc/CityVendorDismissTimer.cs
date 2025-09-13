using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Custom.Confictura;

namespace Server.Misc
{
    public class CityVendorDismiss : Timer
    {
        private CityPlayerVendor m_vendor;
        private Mobile m_owner;
        private DateTime m_Expire;

        public CityVendorDismiss(CityPlayerVendor vend, DateTime expire)
            : base(TimeSpan.Zero, GovernmentTestingMode.Adjust(TimeSpan.FromSeconds(1.5)))
        {
            m_vendor = vend;
            m_Expire = expire;
            m_owner = vend.Owner;
        }

        public bool CheckVendorExist(Mobile vend)
        {
            return World.FindMobile(vend.Serial) != null;
        }

        protected override void OnTick()
        {
            if (DateTime.Now >= m_Expire)
            {
                if (m_vendor == null || !CheckVendorExist(m_vendor))
                {
                    Stop();
                }
                else
                {
                    Server.Items.Container pack = m_vendor.Backpack; // Fully qualify the reference
                    if (pack != null && pack.Items.Count > 0)
                    {
                        BankBox box = m_owner.BankBox;
                        List<Item> list = new List<Item>(pack.Items);
                        int number = list.Count; // Ensure we're using the list count
                        for (int i = 0; i < number; i++)
                        {
                            if (box.TryDropItem(m_owner, list[i], false))
                                continue;
                        }
                        m_vendor.Dismiss(m_owner);
                        Stop();
                    }
                    else
                    {
                        m_vendor.Dismiss(m_owner);
                        Stop();
                    }
                }
            }
        }
    }
}
