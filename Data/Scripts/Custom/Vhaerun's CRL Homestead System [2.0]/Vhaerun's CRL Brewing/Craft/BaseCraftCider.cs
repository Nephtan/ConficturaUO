using System;
using System.Collections;
using Server.Engines.Craft;
using Server.Network;

namespace Server.Items
{
    public abstract class BaseCraftCider : Item, ICraftable
    {
        private Mobile m_Poisoner;
        private Poison m_Poison;
        private int m_FillFactor;
        private Mobile m_Crafter;
        private CiderQuality m_Quality;
        private HopsVariety m_Variety;

        public virtual Item EmptyItem
        {
            get { return null; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Poisoner
        {
            get { return m_Poisoner; }
            set { m_Poisoner = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison Poison
        {
            get { return m_Poison; }
            set { m_Poison = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FillFactor
        {
            get { return m_FillFactor; }
            set { m_FillFactor = value; }
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
        public CiderQuality Quality
        {
            get { return m_Quality; }
            set
            {
                m_Quality = value;
                InvalidateProperties();
            }
        }

        public BaseCraftCider(Serial serial)
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
            Crafter = 0x00000001,
            Quality = 0x00000002,
            Variety = 0x00000004
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            //Version 2
            SaveFlag flags = SaveFlag.None;
            SetSaveFlag(ref flags, SaveFlag.Crafter, m_Crafter != null);
            SetSaveFlag(ref flags, SaveFlag.Quality, m_Quality != CiderQuality.Regular);
            SetSaveFlag(ref flags, SaveFlag.Variety, m_Variety != DefaultVariety);

            writer.WriteEncodedInt((int)flags);

            if (GetSaveFlag(flags, SaveFlag.Crafter))
                writer.Write((Mobile)m_Crafter);
            if (GetSaveFlag(flags, SaveFlag.Quality))
                writer.WriteEncodedInt((int)m_Quality);
            if (GetSaveFlag(flags, SaveFlag.Variety))
                writer.WriteEncodedInt((int)m_Variety);

            //Version 1
            writer.Write(m_Poisoner);

            Poison.Serialize(m_Poison, writer);
            writer.Write(m_FillFactor);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                {
                    SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

                    if (GetSaveFlag(flags, SaveFlag.Crafter))
                        m_Crafter = reader.ReadMobile();

                    if (GetSaveFlag(flags, SaveFlag.Quality))
                        m_Quality = (CiderQuality)reader.ReadEncodedInt();
                    else
                        m_Quality = CiderQuality.Regular;
                    if (m_Quality == CiderQuality.Low)
                        m_Quality = CiderQuality.Regular;

                    if (GetSaveFlag(flags, SaveFlag.Variety))
                        m_Variety = (HopsVariety)reader.ReadEncodedInt();
                    else
                        m_Variety = DefaultVariety;

                    if (m_Variety == HopsVariety.None)
                        m_Variety = DefaultVariety;

                    //break;
                    goto case 1;
                }
                case 1:
                {
                    m_Poisoner = reader.ReadMobile();

                    goto case 0;
                }
                case 0:
                {
                    m_Poison = Poison.Deserialize(reader);
                    m_FillFactor = reader.ReadInt();
                    /*
                    m_Crafter = reader.ReadMobile();
                    m_Quality = (CiderQuality)reader.ReadInt();

                    if ( version >= 2 )
                    {
                        m_Variety = (HopsVariety)reader.ReadInt();
                    }
                    else
                    {
                        HopsInfo info;

                        switch ( reader.ReadInt() )
                        {
                            default:
                            case 0: info = HopsInfo.BitterHops; break;
                            case 1: info = HopsInfo.SnowHops; break;
                            case 2: info = HopsInfo.ElvenHops; break;
                            case 3: info = HopsInfo.SweetHops; break;
                        }

                        m_Variety = BrewingResources.GetFromHopsInfo( info );
                    }
                    */
                    break;
                }
            }
        }

        public virtual HopsVariety DefaultVariety
        {
            get { return HopsVariety.BitterHops; }
        }

        public BaseCraftCider(int itemID)
            : base(itemID)
        {
            m_Quality = CiderQuality.Regular;
            m_Crafter = null;

            m_Variety = DefaultVariety;

            this.FillFactor = 4;
        }

        public void Drink(Mobile from)
        {
            if (Thirsty(from, m_FillFactor))
            {
                // Play a random drinking sound
                from.PlaySound(Utility.Random(0x30, 2));

                if (from.Body.IsHuman && !from.Mounted)
                    from.Animate(34, 5, 1, true, false, 0);

                if (m_Poison != null)
                    from.ApplyPoison(m_Poisoner, m_Poison);

                int bac = 5;
                from.BAC += bac;
                if (from.BAC > 60)
                    from.BAC = 60;

                BaseBeverage.CheckHeaveTimer(from);

                this.Consume();

                Item item = EmptyItem;

                if (item != null)
                    from.AddToBackpack(item);
            }
        }

        public static bool Thirsty(Mobile from, int fillFactor)
        {
            if (from.Thirst >= 20)
            {
                from.SendMessage("You are simply too full to drink any more!");
                return false;
            }

            int iThirst = from.Thirst + fillFactor;
            if (iThirst >= 20)
            {
                from.Thirst = 20;
                from.SendMessage("You manage to drink the beverage, but you are full!");
            }
            else
            {
                from.Thirst = iThirst;

                if (iThirst < 5)
                    from.SendMessage("You drink the beverage, but are still extremely thirsty.");
                else if (iThirst < 10)
                    from.SendMessage("You drink the beverage, and begin to feel more satiated.");
                else if (iThirst < 15)
                    from.SendMessage("After drinking the beverage, you feel much less thirsty.");
                else
                    from.SendMessage("You feel quite full after consuming the beverage.");
            }

            return true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            if (from.InRange(this.GetWorldLocation(), 1))
                Drink(from);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (this.Name == null)
            {
                if (m_Crafter != null)
                    list.Add(m_Crafter.Name + " Brewery");
                else
                    list.Add("Keg of Cider");
            }
            else
            {
                list.Add(this.Name);
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (m_Quality == CiderQuality.Exceptional)
            {
                list.Add("Reserve Dark Cider");
            }
            else
            {
                list.Add("Hard Cider");
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            if (this.Name == null)
            {
                if (m_Crafter != null)
                    this.LabelTo(from, "{0} Brewery", m_Crafter.Name);
                else
                    this.LabelTo(from, "Hard Cider");
            }
            else
            {
                this.LabelTo(from, "{0}", this.Name);
            }

            if (m_Quality == CiderQuality.Exceptional)
            {
                this.LabelTo(from, "Reserve Dark Cider");
            }
            else
            {
                this.LabelTo(from, "Hard Cider");
            }
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
            Quality = (CiderQuality)quality;

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

            //if ( context != null && context.DoNotColor )
            Hue = 0;

            return quality;
        }
        #endregion
    }
}
