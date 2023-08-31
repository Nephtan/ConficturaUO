using System;
using System.Collections;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Items
{
    public abstract class BaseCraftJuice : Item, ICraftable
    {
        private Mobile m_Poisoner;
        private Poison m_Poison;
        private int m_FillFactor;
        private Mobile m_Crafter;
        private JuiceQuality m_Quality;
        private FruitsVariety m_Variety;

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
        public FruitsVariety Variety
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
        public JuiceQuality Quality
        {
            get { return m_Quality; }
            set
            {
                m_Quality = value;
                InvalidateProperties();
            }
        }

        public BaseCraftJuice(Serial serial)
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
            SetSaveFlag(ref flags, SaveFlag.Quality, m_Quality != JuiceQuality.Regular);
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
                        m_Quality = (JuiceQuality)reader.ReadEncodedInt();
                    else
                        m_Quality = JuiceQuality.Regular;
                    if (m_Quality == JuiceQuality.Low)
                        m_Quality = JuiceQuality.Regular;

                    if (GetSaveFlag(flags, SaveFlag.Variety))
                        m_Variety = (FruitsVariety)reader.ReadEncodedInt();
                    else
                        m_Variety = DefaultVariety;

                    if (m_Variety == FruitsVariety.None)
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
                    m_Quality = (JuiceQuality)reader.ReadInt();

                    if ( version >= 2 )
                    {
                        m_Variety = (FruitsVariety)reader.ReadInt();
                    }
                    else
                    {
                        FruitsInfo info;

                        switch ( reader.ReadInt() )
                        {
                            default:
                            case 0: info = FruitsInfo.Apple; break;
                            case 1: info = FruitsInfo.Banana; break;
                            case 2: info = FruitsInfo.Dates; break;
                            case 3: info = FruitsInfo.Grapes; break;
                            case 4: info = FruitsInfo.Lemon; break;
                            case 5: info = FruitsInfo.Lime; break;
                            case 6: info = FruitsInfo.Orange; break;
                            case 7: info = FruitsInfo.Peach; break;
                            case 8: info = FruitsInfo.Pear; break;
                            case 9: info = FruitsInfo.Pumpkin; break;
                            case 10: info = FruitsInfo.Tomato; break;
                            case 11: info = FruitsInfo.Watermelon; break;
                            case 12: info = FruitsInfo.Apricot; break;
                            case 13: info = FruitsInfo.Blackberries; break;
                            case 14: info = FruitsInfo.Blueberries; break;
                            case 15: info = FruitsInfo.Cherries; break;
                            case 16: info = FruitsInfo.Cranberries; break;
                            case 17: info = FruitsInfo.Grapefruit; break;
                            case 18: info = FruitsInfo.Kiwi; break;
                            case 19: info = FruitsInfo.Mango; break;
                            case 20: info = FruitsInfo.Pineappe; break;
                            case 21: info = FruitsInfo.Pomegranate; break;
                            case 22: info = FruitsInfo.Strawberry; break;
                        }

                        m_Variety = BrewingResources.GetFromFruitsInfo( info );
                    }
                    */
                    break;
                }
            }
        }

        public virtual FruitsVariety DefaultVariety
        {
            get { return FruitsVariety.Apple; }
        }

        public BaseCraftJuice(int itemID)
            : base(itemID)
        {
            m_Quality = JuiceQuality.Regular;
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

                this.Consume();

                Item item = EmptyItem;

                if (item != null)
                    from.AddToBackpack(item);
            }
        }

        static public bool Thirsty(Mobile from, int fillFactor)
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
            string JuiceType;
            JuiceType = JuicingResources.GetName(m_Variety);

            if (this.Name == null)
            {
                if (m_Crafter != null)
                    list.Add(m_Crafter.Name + " Farms");
                else
                    list.Add("Bottle of Juice");
            }
            else
            {
                list.Add(this.Name + " Farms");
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            string JuiceType;
            JuiceType = JuicingResources.GetName(m_Variety);

            if (m_Quality == JuiceQuality.Exceptional)
            {
                list.Add(1060847, "Fresh\t{0} Juice", JuiceType);
            }
            else
            {
                list.Add(1060847, "\t{0} Juice", JuiceType);
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            string JuiceType;

            if (this.Name == null)
            {
                if (m_Crafter != null)
                    this.LabelTo(from, "{0} Farms", m_Crafter.Name);
                else
                    this.LabelTo(from, "Bottle of Juice");
            }
            else
            {
                this.LabelTo(from, "{0} Farms", this.Name);
            }

            JuiceType = JuicingResources.GetName(m_Variety);

            if (m_Quality == JuiceQuality.Exceptional)
            {
                this.LabelTo(from, "Fresh {0} Juice", JuiceType);
            }
            else
            {
                this.LabelTo(from, "{0} Juice", JuiceType);
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
            Quality = (JuiceQuality)quality;

            if (makersMark)
                Crafter = from;

            Item[] items = from.Backpack.FindItemsByType(typeof(FarmLabelMaker));

            if (items.Length != 0)
            {
                foreach (FarmLabelMaker lm in items)
                {
                    if (lm.FarmName != null)
                    {
                        this.Name = lm.FarmName;
                        break;
                    }
                }
            }

            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            Variety = JuicingResources.GetFromType(resourceType);

            CraftContext context = craftSystem.GetContext(from);

            //if ( context != null && context.DoNotColor )
            Hue = 0;

            return quality;
        }
        #endregion
    }
}
