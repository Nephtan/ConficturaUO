using System;
using Server;
using Server.Network;
using Server.Targeting;

namespace Server.Engines.BulkOrders
{
    public class LargeBODTarget : Target
    {
        private LargeBOD m_Deed;

        public LargeBODTarget(LargeBOD deed)
            : base(18, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (from == null || from.Deleted || m_Deed == null || m_Deed.Deleted)
                return;

            if (from.Backpack == null || !m_Deed.IsChildOf(from.Backpack))
                return;

            m_Deed.EndCombine(from, targeted);
        }
    }
}
