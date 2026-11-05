using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class BedrolledOut : Item
    {
        private Timer m_Timer;
        private DateTime m_Created;
        private Mobile m_Owner;

        public BedrolledOut(Mobile owner)
            : base(0x0A55)
        {
            ItemID = Utility.RandomMinMax(0x0A55, 0x0A56);
            m_Owner = owner;
            Movable = false;
            m_Created = DateTime.Now;
            m_Timer = Timer.DelayCall(
                TimeSpan.FromSeconds(1.0),
                TimeSpan.FromSeconds(1.0),
                new TimerCallback(OnTick)
            );
        }

        public BedrolledOut(Serial serial)
            : base(serial) { }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return m_Owner; }
            set
            {
                m_Owner = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime Created
        {
            get { return m_Created; }
        }

        public static bool IsSafeCamp(Mobile from)
        {
            return from != null && !from.Deleted && from.Alive
                && from.Map != null && from.Map != Map.Internal
                && !from.Criminal && from.Combatant == null
                && from.Aggressors.Count == 0 && from.Aggressed.Count == 0
                && Kindling.CampAllowed(from) && !Server.Misc.Worlds.IsOnBoat(from)
                && !Kindling.EnemiesNearby(from);
        }

        public static bool CanLogout(Mobile from)
        {
            if (!IsSafeCamp(from))
                return false;

            // Recheck the live bed at disconnect, rather than leaving a sticky logout flag.
            IPooledEnumerable beds = from.GetItemsInRange(3);
            try
            {
                foreach (Item item in beds)
                {
                    BedrolledOut bed = item as BedrolledOut;
                    if (bed != null && !bed.Deleted && bed.Parent == null
                        && bed.Owner == from && bed.Map == from.Map
                        && Math.Abs(bed.Z - from.Z) <= 16
                        && DateTime.Now < bed.Created + TimeSpan.FromSeconds(100.0)
                        && from.InLOS(bed))
                        return true;
                }
            }
            finally
            {
                beds.Free();
            }
            return false;
        }

        private void OnTick()
        {
            DateTime now = DateTime.Now;
            TimeSpan age = now - this.Created;

            if (age >= TimeSpan.FromSeconds(100.0))
                RollUp();

            if (this.Deleted)
                return;

            List<Mobile> toRest = new List<Mobile>();

            IPooledEnumerable eable = GetMobilesInRange(3);

            try
            {
                foreach (Mobile m in eable)
                {
                    if (m is PlayerMobile && m == m_Owner && !Server.Items.Kindling.EnemiesNearby(m))
                        toRest.Add(m);
                }
            }
            finally
            {
                eable.Free();
            }

            for (int i = 0; i < toRest.Count; i++)
                Rest(toRest[i]);
        }

        public void RollUp()
        {
            if (Deleted)
                return;

            if (m_Owner != null)
            {
                Container cont = m_Owner.Backpack;
                if (cont != null)
                    m_Owner.AddToBackpack(new Bedroll());
            }

            this.Delete();
        }

        public void Rest(Mobile m)
        {
            if (m.Hunger > 4 && m.Thirst > 4)
            {
                if (m.Stam < m.StamMax)
                {
                    int stam = Server.Misc.MyServerSettings.PlayerLevelMod(2, m);
                    if (stam < 1)
                        stam = 1;

                    m.Stam = m.Stam + stam;
                }
                if (m.Hits < m.HitsMax)
                {
                    int hits = Server.Misc.MyServerSettings.PlayerLevelMod(2, m);
                    if (hits < 1)
                        hits = 1;

                    m.Hits = m.Hits + hits;
                }
                m.CheckSkill(SkillName.Camping, 0, 125);
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Owner == from)
                RollUp();
        }

        public override void OnAfterDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Mobile)m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile();
            RollUp();
        }
    }
}
