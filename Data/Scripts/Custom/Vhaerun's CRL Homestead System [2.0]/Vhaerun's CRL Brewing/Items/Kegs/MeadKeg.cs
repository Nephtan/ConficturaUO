using System;
using System.Collections;
using Server;
using Server.Engines.Craft;
using Server.Network;

namespace Server.Items
{
    public class MeadKeg : Item, ICraftable
    {
        public static readonly TimeSpan CheckDelay = TimeSpan.FromDays(7.0);

        private int m_Held;
        private Mobile m_Crafter;
        private MeadQuality m_Quality;
        private HopsVariety m_Variety;
        private DateTime m_Start;
        private double m_BottleDuration;
        private bool m_AllowBottling;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Held
        {
            get { return m_Held; }
            set
            {
                if (m_Held != value)
                {
                    this.Weight += (value - m_Held) * 0.8;

                    m_Held = value;
                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public HopsVariety Variety
        {
            get { return m_Variety; }
            set
            {
                if (m_Variety != value)
                {
                    m_Variety = value;

                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get { return m_Crafter; }
            set
            {
                m_Crafter = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public MeadQuality Quality
        {
            get { return m_Quality; }
            set
            {
                m_Quality = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowBottling
        {
            get
            {
                if (!m_AllowBottling)
                    m_AllowBottling = (
                        0
                        >= TimeSpan.Compare(
                            TimeSpan.FromDays(m_BottleDuration),
                            DateTime.Now.Subtract(m_Start)
                        )
                    );
                return m_AllowBottling;
            }
            set { m_AllowBottling = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double BottleDuration
        {
            get { return m_BottleDuration; }
            set { m_BottleDuration = value; }
        }

        public MeadKeg(Serial serial)
            : base(serial) { }

        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SaveFlag
        {
            None = 0x00000000,
            Held = 0x00000001,
            Crafter = 0x00000002,
            Quality = 0x00000004,
            Variety = 0x00000008
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version
            //version 2
            writer.Write((DateTime)m_Start);
            writer.Write((double)m_BottleDuration);
            writer.Write((bool)m_AllowBottling);

            //version 1
            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.Held, m_Held != 0);
            SetSaveFlag(ref flags, SaveFlag.Crafter, m_Crafter != null);
            SetSaveFlag(ref flags, SaveFlag.Quality, m_Quality != MeadQuality.Regular);
            SetSaveFlag(ref flags, SaveFlag.Variety, m_Variety != DefaultVariety);

            writer.WriteEncodedInt((int)flags);

            if (GetSaveFlag(flags, SaveFlag.Held))
                writer.Write((int)m_Held);
            if (GetSaveFlag(flags, SaveFlag.Crafter))
                writer.Write((Mobile)m_Crafter);
            if (GetSaveFlag(flags, SaveFlag.Quality))
                writer.WriteEncodedInt((int)m_Quality);
            if (GetSaveFlag(flags, SaveFlag.Variety))
                writer.WriteEncodedInt((int)m_Variety);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                {
                    m_Start = reader.ReadDateTime();
                    m_BottleDuration = reader.ReadDouble();
                    m_AllowBottling = reader.ReadBool();

                    goto case 1;
                }
                case 1:
                {
                    SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

                    if (GetSaveFlag(flags, SaveFlag.Held))
                        m_Held = reader.ReadInt();

                    if (GetSaveFlag(flags, SaveFlag.Crafter))
                        m_Crafter = reader.ReadMobile();

                    if (GetSaveFlag(flags, SaveFlag.Quality))
                        m_Quality = (MeadQuality)reader.ReadEncodedInt();
                    else
                        m_Quality = MeadQuality.Regular;

                    if (m_Quality == MeadQuality.Low)
                        m_Quality = MeadQuality.Regular;

                    if (GetSaveFlag(flags, SaveFlag.Variety))
                        m_Variety = (HopsVariety)reader.ReadEncodedInt();
                    else
                        m_Variety = DefaultVariety;

                    if (m_Variety == HopsVariety.None)
                        m_Variety = DefaultVariety;

                    break;
                }
            }
        }

        public virtual HopsVariety DefaultVariety
        {
            get { return HopsVariety.BitterHops; }
        }

        [Constructable]
        public MeadKeg()
            : base(0x1940)
        {
            this.Weight = 1.0;

            m_Held = 75;
            m_Quality = MeadQuality.Regular;
            m_Crafter = null;
            m_Variety = DefaultVariety;
            m_BottleDuration = 7.0;
            m_AllowBottling = false;
            m_Start = DateTime.Now;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (this.Name == null)
            {
                if (m_Crafter != null)
                    list.Add(m_Crafter.Name + " Brewery");
                else
                    list.Add("Cheap Mead");
            }
            else
            {
                list.Add(this.Name);
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            string meadType;
            meadType = BrewingResources.GetName(m_Variety);

            if (m_Quality == MeadQuality.Exceptional)
            {
                list.Add(1060847, "Black Label\t{0}", meadType);
            }
            else
            {
                list.Add(1060847, "\t{0}", meadType);
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            int number;

            if (m_Held <= 0)
                number = 502246; // The keg is empty.
            else if (m_Held < 5)
                number = 502248; // The keg is nearly empty.
            else if (m_Held < 10)
                number = 502249; // The keg is not very full.
            else if (m_Held < 18)
                number = 502250; // The keg is about one quarter full.
            else if (m_Held < 25)
                number = 502251; // The keg is about one third full.
            else if (m_Held < 32)
                number = 502252; // The keg is almost half full.
            else if (m_Held < 38)
                number = 502254; // The keg is approximately half full.
            else if (m_Held < 45)
                number = 502253; // The keg is more than half full.
            else if (m_Held < 56)
                number = 502255; // The keg is about three quarters full.
            else if (m_Held < 64)
                number = 502256; // The keg is very full.
            else if (m_Held < 75)
                number = 502257; // The liquid is almost to the top of the keg.
            else
                number = 502258; // The keg is completely full.

            list.Add(number);
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            int number;
            string meadType;

            if (this.Name == null)
            {
                if (m_Crafter != null)
                    this.LabelTo(from, "{0} Brewery", m_Crafter.Name);
                else
                    this.LabelTo(from, "Cheap Mead");
            }
            else
            {
                this.LabelTo(from, "{0}", this.Name);
            }

            meadType = BrewingResources.GetName(m_Variety);

            if (m_Quality == MeadQuality.Exceptional)
            {
                this.LabelTo(from, "Black Label {0}", meadType);
            }
            else
            {
                this.LabelTo(from, "{0}", meadType);
            }

            if (m_Held <= 0)
                number = 502246; // The keg is empty.
            else if (m_Held < 5)
                number = 502248; // The keg is nearly empty.
            else if (m_Held < 10)
                number = 502249; // The keg is not very full.
            else if (m_Held < 18)
                number = 502250; // The keg is about one quarter full.
            else if (m_Held < 25)
                number = 502251; // The keg is about one third full.
            else if (m_Held < 32)
                number = 502252; // The keg is almost half full.
            else if (m_Held < 38)
                number = 502254; // The keg is approximately half full.
            else if (m_Held < 45)
                number = 502253; // The keg is more than half full.
            else if (m_Held < 56)
                number = 502255; // The keg is about three quarters full.
            else if (m_Held < 64)
                number = 502256; // The keg is very full.
            else if (m_Held < 75)
                number = 502257; // The liquid is almost to the top of the keg.
            else
                number = 502258; // The keg is completely full.

            this.LabelTo(from, number);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (AllowBottling)
            {
                if (from.InRange(GetWorldLocation(), 2))
                {
                    if (m_Held > 0)
                    {
                        Container pack = from.Backpack;

                        if (pack != null && pack.ConsumeTotal(typeof(EmptyMeadBottle), 1))
                        {
                            from.SendLocalizedMessage(502242); // You pour some of the keg's contents into an empty bottle...

                            BaseCraftMead mead = FillBottle();
                            mead.Crafter = m_Crafter;
                            mead.Quality = m_Quality;
                            mead.Variety = m_Variety;
                            if (this.Name != null)
                                mead.Name = this.Name;

                            if (pack.TryDropItem(from, mead, false))
                            {
                                from.SendLocalizedMessage(502243); // ...and place it into your backpack.
                                from.PlaySound(0x240);

                                if (--Held == 0)
                                {
                                    this.Delete();

                                    if (GiveKeg(from))
                                    {
                                        from.SendMessage(
                                            "The Keg is empty and you clean it for reuse"
                                        );
                                    }
                                    else
                                    {
                                        from.SendMessage(
                                            "The Keg is now empty and cannot be reused."
                                        );
                                    }
                                }
                            }
                            else
                            {
                                from.SendLocalizedMessage(502244); // ...but there is no room for the bottle in your backpack.
                                mead.Delete();
                            }
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(502246); // The keg is empty.
                    }
                }
                else
                {
                    from.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                }
            }
            else
            {
                from.SendMessage(
                    "This keg is not ready to bottle yet, the fermentation process is not yet complete."
                );
            }
        }

        public bool GiveKeg(Mobile m)
        {
            Container pack = m.Backpack;

            Keg keg = new Keg();

            if (pack == null || !pack.TryDropItem(m, keg, false))
            {
                keg.Delete();
                return false;
            }

            return true;
        }

        public BaseCraftMead FillBottle()
        {
            switch (m_Variety)
            {
                default:
                    return new BottleOfMead();
            }
        }

        public static void Initialize()
        {
            TileData.ItemTable[0x1940].Height = 4;
        }

        #region ICraftable Members
        public int OnCraft(
            int quality,
            bool makersMark,
            Mobile from,
            CraftSystem craftSystem,
            Type typeRes,
            BaseTool tool,
            CraftItem craftItem,
            int resHue
        )
        {
            Held = 75;

            Quality = (MeadQuality)quality;

            if (makersMark)
                Crafter = from;

            Item[] items = from.Backpack.FindItemsByType(typeof(BreweryLabelMaker));

            if (items.Length != 0)
            {
                foreach (BreweryLabelMaker lm in items)
                {
                    if (lm.BreweryName != null)
                    {
                        this.Name = lm.BreweryName;
                        break;
                    }
                }
            }

            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            Variety = BrewingResources.GetFromType(resourceType);

            CraftContext context = craftSystem.GetContext(from);

            Hue = 0;

            BottleDuration = 7.0;
            AllowBottling = false;
            m_Start = DateTime.Now;

            return quality;
        }
        #endregion
    }
}
