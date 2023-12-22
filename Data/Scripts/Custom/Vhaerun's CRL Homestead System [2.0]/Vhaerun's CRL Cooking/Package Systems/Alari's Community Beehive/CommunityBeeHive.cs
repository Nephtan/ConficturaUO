// community hive file version 3
// 4:23 PM 8/6/2005


/*

How to configure:

Scroll down to the first part of the script. There are several variables with
default values specified. They can be changed on the object for customizations
or changed in this script to apply settings to all new hives.


Leave m_honey and m_wax set to 0, unless you want newly-created hives to have
honey and wax already in them.

for the static min and max values, change those to your default preferred minutes to respawn.
(ex: 1 minute min/max, set min and max to 1; 7 minutes min and 11 minutes max, change min to
7 and max to 11)

Leave m_minseconds and m_maxseconds alone, modify min and max.

m_maxhoney and m_maxwax specify default honey and wax maximums, change the number to your preferred default.

m_stoptimer indicates whether the hive should stop "buzzing" even after it's full. (true = no buzzing, false = buzzing)


*/

// variables

// honey and wax ints, settable on object & serialized - current honey and wax counts
// min and max static ints, changable only in script - easy way to change min and max time. Specified in Minutes, converted to seconds.
// minseconds and maxseconds ints, settable on object and serialized - min seconds and max seconds to add new honey and wax
// maxhoney and maxwax ints, settable on object & serialized - max honey and wax counts
// stoptimer variable, settable on object & serialized - stops the timer when beehive is full. Restarts timer when changed.

// added item leakage protection


// based off of:

/********************************************************************************************
**Apiculuture By Crystal 2003 (Api_item.cs,Api_Production.cs)                              **
**le script comprend 1 ruche, 1 essaim d'abeille , 1 caquelot a cire, de la cire , du miel **
**un moules a bougies 									   **
**               http://invisionfree.com/forums/Hyel_dev/index.php                         **
********************************************************************************************/

using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Targeting;

namespace Server.Items
{
    public class CommunityBeeHive : AddonComponent
    {
        private int m_honey = 0,
            m_wax = 0; //  initial honey and wax.

        private static int min = 7,
            max = 11; // min and max minutes. * CHANGE THESE *

        // NOTE: MINUTES. Will be multiplied by 60.

        // Modify MinSeconds and MaxSeconds on the object. Modify min and max in the script.
        private int m_minseconds = min * 60,
            m_maxseconds = max * 60;

        private int m_maxhoney = 10,
            m_maxwax = 10; // adjust these as appropriate

        private bool m_stoptimer = false; // change to true to stop "Bzzzz"ing after the hive is full.

        private Timer m_timer; // holds the timer object. Don't change this.

        [CommandProperty(AccessLevel.GameMaster)]
        public int Honey // hive honey
        {
            get { return m_honey; }
            set
            {

                if (value < 0) // sanity check. Adjust values as needed.
                    m_honey = 0;
                else
                    m_honey = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Wax // hive wax
        {
            get { return m_wax; }
            set
            {

                if (value < 0) // sanity check. Adjust values as needed.
                    m_wax = 0;
                else
                    m_wax = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinSeconds // min time to spawn
        {
            get { return m_minseconds; }
            set
            {
                if (value < 60) // sanity check. Hive will "Bzzzzz" like hell otherwise.
                    m_minseconds = 60;
                else
                    m_minseconds = value;

                if (m_timer == null)
                {
                    m_timer = new CommunityBeeHiveTimer(this);
                    m_timer.Start();
                }
                else if (m_timer != null)
                {
                    m_timer.Stop();
                    m_timer = new CommunityBeeHiveTimer(this);
                    m_timer.Start();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxSeconds // max time to spawn
        {
            get { return m_maxseconds; }
            set
            {
                if (value < 60) // sanity check. Adjust values as needed.
                    m_maxseconds = 60;
                else
                    m_maxseconds = value;

                if (m_timer == null)
                {
                    m_timer = new CommunityBeeHiveTimer(this);
                    m_timer.Start();
                }
                else if (m_timer != null)
                {
                    m_timer.Stop();
                    m_timer = new CommunityBeeHiveTimer(this);
                    m_timer.Start();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHoney // max honey
        {
            get { return m_maxhoney; }
            set
            {
                if (value < 0) // sanity check. Adjust values as needed.
                    m_maxhoney = 0;
                else
                    m_maxhoney = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxWax // max wax
        {
            get { return m_maxwax; }
            set
            {
                if (value < 0) // sanity check. Adjust values as needed.
                    m_maxwax = 0;
                else
                    m_maxwax = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool StopTimer // stop timer after hive is full
        {
            get { return m_stoptimer; }
            set
            {
                m_stoptimer = value;

                if (m_timer == null)
                {
                    m_timer = new CommunityBeeHiveTimer(this);
                    m_timer.Start();
                }
                else if (m_timer != null)
                {
                    m_timer.Stop();
                    m_timer = new CommunityBeeHiveTimer(this);
                    m_timer.Start();
                }
            }
        }

        [Constructable]
        public CommunityBeeHive()
            : base(0x91a)
        {
            Name = "A Community BeeHive";

            m_timer = new CommunityBeeHiveTimer(this);
            m_timer.Start();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this.GetWorldLocation(), 1))
            {
                from.LocalOverheadMessage(MessageType.Regular, 906, 1019045); // I can't reach that.
            }
            else
            {
                if (m_wax == 0 && m_honey == 0)
                    from.SendMessage("The hive is empty.");
                else
                {
                    bool givehoney = false,
                        givewax = false;

                    if (m_wax > 0)
                    {
                        Beeswax wax = new Beeswax(m_wax);

                        if (!from.AddToBackpack(wax))
                        {
                            wax.Delete();
                        }
                        else
                            givewax = true;
                    }

                    if (m_honey > 0)
                    {
                        JarHoney honey = new JarHoney(m_honey);

                        if (!from.AddToBackpack(honey))
                        {
                            honey.Delete();
                        }
                        else
                            givehoney = true;
                    }

                    if (givehoney && givewax)
                    {
                        from.SendMessage(
                            String.Format("You gather {0} honey and {1} wax.", m_honey, m_wax)
                        );
                        m_wax = 0;
                        m_honey = 0;
                    }
                    else if (givehoney)
                    {
                        from.SendMessage(String.Format("You gather {0} honey.", m_honey));
                        m_honey = 0;
                    }
                    else if (givewax)
                    {
                        from.SendMessage(String.Format("You gather {0} wax.", m_wax));
                        m_wax = 0;
                    }
                    else
                        from.SendMessage("You do not manage to gather anything from the hive.");
                }

                if (m_timer == null)
                {
                    m_timer = new CommunityBeeHiveTimer(this);
                    m_timer.Start();
                }
                else if (m_timer != null && !m_timer.Running)
                {
                    m_timer.Stop();
                    m_timer = new CommunityBeeHiveTimer(this);
                    m_timer.Start();
                }
            }
        }

        public CommunityBeeHive(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)3); // version

            // version 3
            writer.Write((bool)m_stoptimer);

            // version 2
            writer.Write((int)m_maxwax);
            writer.Write((int)m_maxhoney);

            // version 1
            writer.Write(m_minseconds);
            writer.Write(m_maxseconds);

            // version 0
            writer.Write((int)m_wax);
            writer.Write((int)m_honey);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 3:
                {
                    m_stoptimer = reader.ReadBool();

                    goto case 2;
                }
                case 2:
                {
                    m_maxwax = reader.ReadInt();
                    m_maxhoney = reader.ReadInt();

                    goto case 1;
                }
                case 1:
                {
                    m_minseconds = reader.ReadInt();
                    m_maxseconds = reader.ReadInt();

                    goto case 0;
                }
                case 0:
                {
                    m_wax = reader.ReadInt();
                    m_honey = reader.ReadInt();

                    break;
                }
            }

            m_timer = new CommunityBeeHiveTimer(this);
            m_timer.Start();
        }
    }

    public class CommunityBeeHiveAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get { return new CommunityBeeHiveDeed(); }
        }

        [Constructable]
        public CommunityBeeHiveAddon()
        {
            AddComponent(new CommunityBeeHive(), 0, 0, 0);
            AddComponent(new AddonComponent(2331), 0, 0, 0); // bees
        }

        public CommunityBeeHiveAddon(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CommunityBeeHiveDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get { return new CommunityBeeHiveAddon(); }
        }

        [Constructable]
        public CommunityBeeHiveDeed()
        {
            Name = "a community beehive deed";
        }

        public CommunityBeeHiveDeed(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CommunityBeeHiveTimer : Timer
    {
        private CommunityBeeHive m_Beehive;

        public CommunityBeeHiveTimer(CommunityBeeHive hive)
            : base(
                TimeSpan.FromSeconds(Utility.Random(hive.MinSeconds, hive.MaxSeconds)),
                TimeSpan.FromSeconds(Utility.Random(hive.MinSeconds, hive.MaxSeconds))
            )
        {
            m_Beehive = hive;

            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            // stop timer if max wax and honey are full and stoptimer == true
            if (
                m_Beehive.StopTimer
                && m_Beehive.Honey >= m_Beehive.MaxHoney
                && m_Beehive.Wax >= m_Beehive.MaxWax
            )
                Stop();

            m_Beehive.PublicOverheadMessage(
                MessageType.Emote,
                0x35,
                false,
                string.Format("Bzzzzzz")
            );

            if (m_Beehive.Wax < m_Beehive.MaxWax && Utility.RandomBool())
                m_Beehive.Wax++;

            if (m_Beehive.Honey < m_Beehive.MaxHoney && Utility.RandomBool())
                m_Beehive.Honey++;
        }
    }
}
