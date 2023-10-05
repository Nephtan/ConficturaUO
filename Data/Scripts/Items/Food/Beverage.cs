using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Engines.Plants;
using Server.Engines.Quests;
using Server.ContextMenus;

namespace Server.Items
{
    public enum BeverageType
    {
        Ale,
        Cider,
        Liquor,
        Milk,
        Wine,
        Water,
        Juice,
        Mead
    }

    public interface IHasQuantity
    {
        int Quantity { get; set; }
    }

    public interface IWaterSource : IHasQuantity { }

    // TODO: Flipable attributes

    [TypeAlias("Server.Items.BottleAle", "Server.Items.BottleLiquor", "Server.Items.BottleWine")]
    public class BeverageBottle : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get { return 1042959; }
        } // a bottle of Ale
        public override int MaxQuantity
        {
            get { return 5; }
        }
        public override bool Fillable
        {
            get { return false; }
        }

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                switch (Content)
                {
                    case BeverageType.Ale:
                        return 0x99F;
                    case BeverageType.Cider:
                        return 0x99F;
                    case BeverageType.Liquor:
                        return 0x99B;
                    case BeverageType.Milk:
                        return 0x99B;
                    case BeverageType.Wine:
                        return 0x9C7;
                    case BeverageType.Water:
                        return 0x99B;
                    case BeverageType.Juice:
                        return 0x99B;
                    case BeverageType.Mead:
                        return 0x9C7;
                }
            }

            return 0;
        }

        [Constructable]
        public BeverageBottle(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public BeverageBottle(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("BottleAle"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Ale;
                        }
                        else if (CheckType("BottleLiquor"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Liquor;
                        }
                        else if (CheckType("BottleWine"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Wine;
                        }
                        else
                        {
                            throw new Exception(World.LoadingType);
                        }

                        break;
                    }
            }
        }
    }

    public class Jug : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get { return 1042965; }
        } // a jug of Ale
        public override int MaxQuantity
        {
            get { return 10; }
        }
        public override bool Fillable
        {
            get { return false; }
        }

        public override int ComputeItemID()
        {
            if (!IsEmpty)
                return 0x9C8;

            return 0;
        }

        [Constructable]
        public Jug(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public Jug(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CeramicMug : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get { return 1042982; }
        } // a ceramic mug of Ale
        public override int MaxQuantity
        {
            get { return 1; }
        }

        public override int ComputeItemID()
        {
            if (ItemID >= 0x995 && ItemID <= 0x999)
                return ItemID;
            else if (ItemID == 0x9CA)
                return ItemID;

            return 0x995;
        }

        [Constructable]
        public CeramicMug()
        {
            Weight = 1.0;
        }

        [Constructable]
        public CeramicMug(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public CeramicMug(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class PewterMug : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get { return 1042994; }
        } // a pewter mug with Ale
        public override int MaxQuantity
        {
            get { return 1; }
        }

        public override int ComputeItemID()
        {
            if (ItemID >= 0xFFF && ItemID <= 0x1002)
                return ItemID;

            return 0xFFF;
        }

        [Constructable]
        public PewterMug()
        {
            Weight = 1.0;
        }

        [Constructable]
        public PewterMug(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public PewterMug(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Goblet : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get { return 1043000; }
        } // a goblet of Ale
        public override int MaxQuantity
        {
            get { return 1; }
        }

        public override int ComputeItemID()
        {
            if (ItemID == 0x99A || ItemID == 0x9B3 || ItemID == 0x9BF || ItemID == 0x9CB)
                return ItemID;

            return 0x99A;
        }

        [Constructable]
        public Goblet()
        {
            Weight = 1.0;
        }

        [Constructable]
        public Goblet(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public Goblet(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [TypeAlias(
        "Server.Items.MugAle",
        "Server.Items.GlassCider",
        "Server.Items.GlassLiquor",
        "Server.Items.GlassMilk",
        "Server.Items.GlassWine",
        "Server.Items.GlassWater",
        "Server.Items.GlassJuice",
        "Server.Items.GlassMead"
    )]
    public class GlassMug : BaseBeverage
    {
        public override int EmptyLabelNumber
        {
            get { return 1022456; }
        } // mug
        public override int BaseLabelNumber
        {
            get { return 1042976; }
        } // a mug of Ale
        public override int MaxQuantity
        {
            get { return 5; }
        }

        public override int ComputeItemID()
        {
            if (IsEmpty)
                return (ItemID >= 0x1F81 && ItemID <= 0x1F84 ? ItemID : 0x1F81);

            switch (Content)
            {
                case BeverageType.Ale:
                    return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.Cider:
                    return (ItemID >= 0x1F7D && ItemID <= 0x1F80 ? ItemID : 0x1F7D);
                case BeverageType.Liquor:
                    return (ItemID >= 0x1F85 && ItemID <= 0x1F88 ? ItemID : 0x1F85);
                case BeverageType.Milk:
                    return (ItemID >= 0x1F89 && ItemID <= 0x1F8C ? ItemID : 0x1F89);
                case BeverageType.Wine:
                    return (ItemID >= 0x1F8D && ItemID <= 0x1F90 ? ItemID : 0x1F8D);
                case BeverageType.Water:
                    return (ItemID >= 0x1F91 && ItemID <= 0x1F94 ? ItemID : 0x1F91);
                case BeverageType.Juice:
                    return (ItemID >= 0x1F91 && ItemID <= 0x1F94 ? ItemID : 0x1F91);
                case BeverageType.Mead:
                    return (ItemID >= 0x1F8D && ItemID <= 0x1F90 ? ItemID : 0x1F8D);
            }

            return 0;
        }

        [Constructable]
        public GlassMug()
        {
            Weight = 1.0;
        }

        [Constructable]
        public GlassMug(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public GlassMug(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("MugAle"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Ale;
                        }
                        else if (CheckType("GlassCider"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Cider;
                        }
                        else if (CheckType("GlassLiquor"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Liquor;
                        }
                        else if (CheckType("GlassMilk"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Milk;
                        }
                        else if (CheckType("GlassWine"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Wine;
                        }
                        else if (CheckType("GlassWater"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Water;
                        }
                        else if (CheckType("GlassJuice"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Juice;
                        }
                        else if (CheckType("GlassMead"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Mead;
                        }
                        else
                        {
                            throw new Exception(World.LoadingType);
                        }

                        break;
                    }
            }
        }
    }

    [TypeAlias(
        "Server.Items.PitcherAle",
        "Server.Items.PitcherCider",
        "Server.Items.PitcherLiquor",
        "Server.Items.PitcherMilk",
        "Server.Items.PitcherWine",
        "Server.Items.PitcherWater",
        "Server.Items.PitcherJuice",
        "Server.Items.PitcherMead",
        "Server.Items.GlassPitcher"
    )]
    public class Pitcher : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get { return 1048128; }
        } // a Pitcher of Ale
        public override int MaxQuantity
        {
            get { return 5; }
        }

        public override int ComputeItemID()
        {
            if (IsEmpty)
            {
                if (ItemID == 0x9A7 || ItemID == 0xFF7)
                    return ItemID;

                return 0xFF6;
            }

            switch (Content)
            {
                case BeverageType.Ale:
                    {
                        if (ItemID == 0x1F96)
                            return ItemID;

                        return 0x1F95;
                    }
                case BeverageType.Cider:
                    {
                        if (ItemID == 0x1F98)
                            return ItemID;

                        return 0x1F97;
                    }
                case BeverageType.Liquor:
                    {
                        if (ItemID == 0x1F9A)
                            return ItemID;

                        return 0x1F99;
                    }
                case BeverageType.Milk:
                    {
                        if (ItemID == 0x9AD)
                            return ItemID;

                        return 0x9F0;
                    }
                case BeverageType.Wine:
                    {
                        if (ItemID == 0x1F9C)
                            return ItemID;

                        return 0x1F9B;
                    }
                case BeverageType.Water:
                    {
                        if (ItemID == 0xFF8 || ItemID == 0xFF9 || ItemID == 0x1F9E)
                            return ItemID;

                        return 0x1F9D;
                    }
                case BeverageType.Juice:
                    {
                        if (ItemID == 0x1F9C)
                            return ItemID;

                        return 0x1F9B;
                    }
                case BeverageType.Mead:
                    {
                        if (ItemID == 0x1F9C)
                            return ItemID;

                        return 0x1F97;
                    }
            }

            return 0;
        }

        [Constructable]
        public Pitcher()
        {
            Weight = 2.0;
        }

        [Constructable]
        public Pitcher(BeverageType type)
            : base(type)
        {
            Weight = 2.0;
        }

        public Pitcher(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            if (CheckType("PitcherWater") || CheckType("GlassPitcher"))
                base.InternalDeserialize(reader, false);
            else
                base.InternalDeserialize(reader, true);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("PitcherAle"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Ale;
                        }
                        else if (CheckType("PitcherCider"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Cider;
                        }
                        else if (CheckType("PitcherLiquor"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Liquor;
                        }
                        else if (CheckType("PitcherMilk"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Milk;
                        }
                        else if (CheckType("PitcherWine"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Wine;
                        }
                        else if (CheckType("PitcherWater"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Water;
                        }
                        else if (CheckType("GlassPitcher"))
                        {
                            Quantity = 0;
                            Content = BeverageType.Water;
                        }
                        else if (CheckType("PitcherJuice"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Juice;
                        }
                        else if (CheckType("PitcherMead"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Mead;
                        }
                        else
                        {
                            throw new Exception(World.LoadingType);
                        }

                        break;
                    }
            }
        }
    }

    public abstract class BaseBeverage : Item, IHasQuantity
    {
        private BeverageType m_Content;
        private int m_Quantity;
        private Mobile m_Poisoner;
        private Poison m_Poison;

        public override int LabelNumber
        {
            get
            {
                int num = BaseLabelNumber;

                if (IsEmpty || num == 0)
                    return EmptyLabelNumber;

                return BaseLabelNumber + (int)m_Content;
            }
        }

        public virtual bool ShowQuantity
        {
            get { return (MaxQuantity > 1); }
        }
        public virtual bool Fillable
        {
            get { return true; }
        }
        public virtual bool Pourable
        {
            get { return true; }
        }

        public virtual int EmptyLabelNumber
        {
            get { return base.LabelNumber; }
        }
        public virtual int BaseLabelNumber
        {
            get { return 0; }
        }

        public abstract int MaxQuantity { get; }

        public abstract int ComputeItemID();

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsEmpty
        {
            get { return (m_Quantity <= 0); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ContainsAlchohol
        {
            get
            {
                return (
                    !IsEmpty
                    && m_Content != BeverageType.Milk
                    && m_Content != BeverageType.Water
                    && m_Content == BeverageType.Liquor
                );
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsFull
        {
            get { return (m_Quantity >= MaxQuantity); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison Poison
        {
            get { return m_Poison; }
            set { m_Poison = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Poisoner
        {
            get { return m_Poisoner; }
            set { m_Poisoner = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public BeverageType Content
        {
            get { return m_Content; }
            set
            {
                m_Content = value;

                InvalidateProperties();

                int itemID = ComputeItemID();

                if (itemID > 0)
                    ItemID = itemID;
                else
                    Delete();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quantity
        {
            get { return m_Quantity; }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > MaxQuantity)
                    value = MaxQuantity;

                m_Quantity = value;

                InvalidateProperties();

                int itemID = ComputeItemID();

                if (itemID > 0)
                    ItemID = itemID;
                else
                    Delete();
            }
        }

        public virtual int GetQuantityDescription()
        {
            int perc = (m_Quantity * 100) / MaxQuantity;

            if (perc <= 0)
                return 1042975; // It's empty.
            else if (perc <= 33)
                return 1042974; // It's nearly empty.
            else if (perc <= 66)
                return 1042973; // It's half full.
            else
                return 1042972; // It's full.
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (ShowQuantity)
                list.Add(GetQuantityDescription());
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (ShowQuantity)
                LabelTo(from, GetQuantityDescription());
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (!IsEmpty)
            {
                if (Content == BeverageType.Ale)
                {
                    list.Add(1070722, "ale");
                }
                else if (Content == BeverageType.Cider)
                {
                    list.Add(1070722, "cider");
                }
                else if (Content == BeverageType.Liquor)
                {
                    list.Add(1070722, "liquor");
                }
                else if (Content == BeverageType.Milk)
                {
                    list.Add(1070722, "milk");
                }
                else if (Content == BeverageType.Wine)
                {
                    list.Add(1070722, "wine");
                }
                else if (Content == BeverageType.Water)
                {
                    list.Add(1070722, "water");
                }
            }
        }

        public virtual bool ValidateUse(Mobile from, bool message)
        {
            if (Deleted)
                return false;

            if (!Movable && !Fillable)
            {
                Multis.BaseHouse house = Multis.BaseHouse.FindHouseAt(this);

                if (house == null || !house.IsLockedDown(this))
                {
                    if (message)
                        from.SendLocalizedMessage(502946, "", 0x59); // That belongs to someone else.

                    return false;
                }
            }

            if (from.Map != Map || !from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                if (message)
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.

                return false;
            }

            return true;
        }

        public virtual void Fill_OnTarget(Mobile from, object targ)
        {
            // Ensure the vessel is empty, fillable, and the use is valid before proceeding
            if (!IsEmpty || !Fillable || !ValidateUse(from, false))
                return;  // Exit method if conditions are not met

            // Determine the type of target to decide the filling action
            if (targ is StaticTarget)
            {
                int id = ((StaticTarget)targ).ItemID;  // Extract item ID from static target

                // Check if the static target is a valid water source
                if (DrinkingFunctions.CheckWaterTarget(id))
                {
                    Content = BeverageType.Water;  // Set beverage type to water
                    Quantity = MaxQuantity;  // Fill to maximum quantity
                    from.SendLocalizedMessage(1010089);  // Notify player: You fill the container with water.
                    from.PlaySound(0x240);  // Play a water pouring sound
                }
            }

            // Check if target is an item and a valid water source
            else if (targ is Item && DrinkingFunctions.CheckWaterTarget(((Item)targ).ItemID))
            {
                int id = ((Item)targ).ItemID;  // Extract item ID from item target

                // Check if the item is a valid water source
                if (DrinkingFunctions.CheckWaterTarget(id))
                {
                    Content = BeverageType.Water;  // Set beverage type to water
                    Quantity = MaxQuantity;  // Fill to maximum quantity
                    from.SendLocalizedMessage(1010089);  // Notify player: You fill the container with water.
                    from.PlaySound(0x240);  // Play a water pouring sound
                }
            }

            // Check if the target is a BaseBeverage
            else if (targ is BaseBeverage)
            {
                BaseBeverage bev = (BaseBeverage)targ;  // Cast target object to BaseBeverage type

                // Check if the target beverage is empty or not validated for use
                if (bev.IsEmpty || !bev.ValidateUse(from, true))
                    return;  // Exit method if conditions are not met

                // Transfer content, poison, and poisoner information from target beverage to this beverage
                this.Content = bev.Content;
                this.Poison = bev.Poison;
                this.Poisoner = bev.Poisoner;

                // Determine the quantity to transfer based on available quantity in target beverage and max quantity of this beverage
                if (bev.Quantity > this.MaxQuantity)
                {
                    this.Quantity = this.MaxQuantity;  // Fill to maximum quantity
                    bev.Quantity -= this.MaxQuantity;  // Deduct filled quantity from target beverage
                }
                else
                {
                    this.Quantity += bev.Quantity;  // Add available quantity to this beverage
                    bev.Quantity = 0;  // Set target beverage quantity to zero
                }
            }

            // Check if target is an item representing a water source or an addon component representing a water source
            else if (targ is Item)
            {
                Item item = (Item)targ;  // Cast target to Item
                IWaterSource src;  // Interface for water source

                src = (item as IWaterSource);  // Try to cast item to IWaterSource interface

                // Check if item is an addon component and try to cast the addon to IWaterSource
                if (src == null && item is AddonComponent)
                    src = (((AddonComponent)item).Addon as IWaterSource);

                // Check if cast was unsuccessful or water source is empty
                if (src == null || src.Quantity <= 0)
                    return;  // Exit method if conditions are not met

                // Check if player is on the same map, within range, and has line of sight to the item
                if (
                    from.Map != item.Map
                    || !from.InRange(item.GetWorldLocation(), 2)
                    || !from.InLOS(item)
                )
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045);  // Notify player: I can't reach that.
                    return;  // Exit method if conditions are not met
                }

                this.Content = BeverageType.Water;  // Set beverage type to water
                this.Poison = null;  // Clear any poison values
                this.Poisoner = null;  // Clear poisoner values

                // Determine the quantity to transfer based on available quantity in source and max quantity of vessel
                if (src.Quantity > this.MaxQuantity)
                {
                    this.Quantity = this.MaxQuantity;  // Fill to maximum quantity
                    src.Quantity -= this.MaxQuantity;  // Deduct filled quantity from source
                }
                else
                {
                    this.Quantity += src.Quantity;  // Add available quantity to vessel
                    src.Quantity = 0;  // Set source quantity to zero
                }

                from.SendLocalizedMessage(1010089);  // Notify player: You fill the container with water.
            }

            // Check if target is a cow and if cow allows milking
            else if (targ is Cow)
            {
                Cow cow = (Cow)targ;  // Cast target to Cow

                // Try to milk the cow
                if (cow.TryMilk(from))
                {
                    Content = BeverageType.Milk;  // Set beverage type to milk
                    Quantity = MaxQuantity;  // Fill to maximum quantity
                    from.SendLocalizedMessage(1080197);  // Notify player: You fill the container with milk.
                }
            }
        }

        private static int[] m_SwampTiles = new int[]
        {
            0x9C4,
            0x9EB,
            0x3D65,
            0x3D65,
            0x3DC0,
            0x3DD9,
            0x3DDB,
            0x3DDC,
            0x3DDE,
            0x3EF0,
            0x3FF6,
            0x3FF6,
            0x3FFC,
            0x3FFE,
        };

        #region Effects of achohol

        private static Hashtable m_Table = new Hashtable();

        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            CheckHeaveTimer(e.Mobile);
        }

        public static void CheckHeaveTimer(Mobile from)
        {
            if (from.BAC > 0 && from.Map != Map.Internal && !from.Deleted)
            {
                Timer t = (Timer)m_Table[from];

                if (t == null)
                {
                    if (from.BAC > 60)
                        from.BAC = 60;

                    t = new HeaveTimer(from);
                    t.Start();

                    m_Table[from] = t;
                }
            }
            else
            {
                Timer t = (Timer)m_Table[from];

                if (t != null)
                {
                    t.Stop();
                    m_Table.Remove(from);

                    from.SendLocalizedMessage(500850); // You feel sober.
                }
            }
        }

        private class HeaveTimer : Timer
        {
            private Mobile m_Drunk;

            public HeaveTimer(Mobile drunk)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                m_Drunk = drunk;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Drunk.Deleted || m_Drunk.Map == Map.Internal)
                {
                    Stop();
                    m_Table.Remove(m_Drunk);
                }
                else if (m_Drunk.Alive)
                {
                    if (m_Drunk.BAC > 60)
                        m_Drunk.BAC = 60;

                    // chance to get sober
                    if (10 > Utility.Random(100))
                        --m_Drunk.BAC;

                    // lose some stats
                    m_Drunk.Stam -= 1;
                    m_Drunk.Mana -= 1;

                    if (Utility.Random(1, 4) == 1)
                    {
                        if (!m_Drunk.Mounted)
                        {
                            // turn in a random direction
                            m_Drunk.Direction = (Direction)Utility.Random(8);

                            // heave
                            m_Drunk.Animate(32, 5, 1, true, false, 0);
                        }

                        // *hic*
                        m_Drunk.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, 500849);
                    }

                    if (m_Drunk.BAC <= 0)
                    {
                        Stop();
                        m_Table.Remove(m_Drunk);

                        m_Drunk.SendLocalizedMessage(500850); // You feel sober.
                    }
                }
            }
        }

        #endregion

        public virtual void Pour_OnTarget(Mobile from, object targ)
        {
            if (IsEmpty || !Pourable || !ValidateUse(from, false))
                return;

            if (targ is BaseBeverage)
            {
                BaseBeverage bev = (BaseBeverage)targ;

                if (!bev.ValidateUse(from, true))
                    return;

                if (bev.IsFull && bev.Content == this.Content)
                {
                    from.SendLocalizedMessage(500848); // Couldn't pour it there.  It was already full.
                }
                else if (!bev.IsEmpty)
                {
                    from.SendLocalizedMessage(500846); // Can't pour it there.
                }
                else
                {
                    bev.Content = this.Content;
                    bev.Poison = this.Poison;
                    bev.Poisoner = this.Poisoner;

                    if (this.Quantity > bev.MaxQuantity)
                    {
                        bev.Quantity = bev.MaxQuantity;
                        this.Quantity -= bev.MaxQuantity;
                    }
                    else
                    {
                        bev.Quantity += this.Quantity;
                        this.Quantity = 0;
                    }

                    from.PlaySound(0x4E);
                }
            }
            else if (from == targ)
            {
                if (from.Thirst >= 20)
                {
                    from.SendMessage("You are simply too full to drink any more!");
                    return;
                }

                int fillFactor = 4;

                int iThirst = from.Thirst + fillFactor;

                if (from.Stam < from.StamMax)
                    from.Stam += Utility.Random(6, 3) + fillFactor / 5;

                if (iThirst >= 20)
                {
                    from.Thirst = 20;
                    from.SendMessage("You manage to drink the beverage, but you are full!");
                }
                else
                {
                    from.Thirst = iThirst;

                    if (iThirst < 5)
                        from.SendMessage(
                            "You drink the beverage, but are still extremely thirsty."
                        );
                    else if (iThirst < 10)
                        from.SendMessage(
                            "You drink the beverage, and begin to feel more satiated."
                        );
                    else if (iThirst < 15)
                        from.SendMessage(
                            "After drinking the beverage, you feel much less thirsty."
                        );
                    else
                        from.SendMessage("You feel quite full after consuming the beverage.");
                }

                if (ContainsAlchohol)
                {
                    int bac = 0;

                    switch (this.Content)
                    {
                        case BeverageType.Ale:
                            bac = 1;
                            break;
                        case BeverageType.Wine:
                            bac = 2;
                            break;
                        case BeverageType.Cider:
                            bac = 3;
                            break;
                        case BeverageType.Liquor:
                            bac = 4;
                            break;
                    }

                    from.BAC += bac;

                    if (from.BAC > 60)
                        from.BAC = 60;

                    CheckHeaveTimer(from);
                }

                from.PlaySound(Utility.RandomList(0x30, 0x2D6));

                if (m_Poison != null)
                    from.ApplyPoison(m_Poisoner, m_Poison);

                --Quantity;
            }
            else if (targ is PlantItem)
            {
                ((PlantItem)targ).Pour(from, this);
            }
            else
            {
                from.SendLocalizedMessage(500846); // Can't pour it there.
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsEmpty)
            {
                if (!Fillable || !ValidateUse(from, true))
                    return;

                from.BeginTarget(-1, true, TargetFlags.None, new TargetCallback(Fill_OnTarget));
                SendLocalizedMessageTo(from, 500837); // Fill from what?
            }
            else if (Pourable && ValidateUse(from, true))
            {
                from.BeginTarget(-1, true, TargetFlags.None, new TargetCallback(Pour_OnTarget));
                from.SendLocalizedMessage(1010086); // What do you want to use this on?
            }
        }

        public class PourMenu : ContextMenuEntry
        {
            private BaseBeverage i_Beverage;
            private Mobile m_From;

            public PourMenu(Mobile from, BaseBeverage drink)
                : base(6250, 1)
            {
                m_From = from;
                i_Beverage = drink;
            }

            public override void OnClick()
            {
                if (i_Beverage.IsChildOf(m_From.Backpack))
                {
                    m_From.BeginTarget(
                        -1,
                        true,
                        TargetFlags.None,
                        new TargetCallback(i_Beverage.Pour_OnTarget)
                    );
                    m_From.SendLocalizedMessage(1010086); // What do you want to use this on?
                }
                else
                {
                    m_From.SendMessage("This must be in your backpack to use.");
                }
            }
        }

        public class FillMenu : ContextMenuEntry
        {
            private BaseBeverage i_Beverage;
            private Mobile m_From;

            public FillMenu(Mobile from, BaseBeverage drink)
                : base(6255, 1)
            {
                m_From = from;
                i_Beverage = drink;
            }

            public override void OnClick()
            {
                if (i_Beverage.IsChildOf(m_From.Backpack))
                {
                    m_From.BeginTarget(
                        -1,
                        true,
                        TargetFlags.None,
                        new TargetCallback(i_Beverage.Fill_OnTarget)
                    );
                    m_From.SendLocalizedMessage(500837); // Fill from what?
                }
                else
                {
                    m_From.SendMessage("This must be in your backpack to use.");
                }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive && IsEmpty)
            {
                if (!Fillable || !ValidateUse(from, true)) { }
                else
                    list.Add(new FillMenu(from, this));
            }
            else if (from.Alive && Pourable && ValidateUse(from, true))
                list.Add(new PourMenu(from, this));
        }

        public static bool ConsumeTotal(Container pack, BeverageType content, int quantity)
        {
            return ConsumeTotal(pack, typeof(BaseBeverage), content, quantity);
        }

        public static bool ConsumeTotal(
            Container pack,
            Type itemType,
            BeverageType content,
            int quantity
        )
        {
            Item[] items = pack.FindItemsByType(itemType);

            // First pass, compute total
            int total = 0;

            for (int i = 0; i < items.Length; ++i)
            {
                BaseBeverage bev = items[i] as BaseBeverage;

                if (bev != null && bev.Content == content && !bev.IsEmpty)
                    total += bev.Quantity;
            }

            if (total >= quantity)
            {
                // We've enough, so consume it

                int need = quantity;

                for (int i = 0; i < items.Length; ++i)
                {
                    BaseBeverage bev = items[i] as BaseBeverage;

                    if (bev == null || bev.Content != content || bev.IsEmpty)
                        continue;

                    int theirQuantity = bev.Quantity;

                    if (theirQuantity < need)
                    {
                        bev.Quantity = 0;
                        need -= theirQuantity;
                    }
                    else
                    {
                        bev.Quantity -= need;
                        return true;
                    }
                }
            }

            return false;
        }

        public BaseBeverage()
        {
            ItemID = ComputeItemID();
        }

        public BaseBeverage(BeverageType type)
        {
            m_Content = type;
            m_Quantity = MaxQuantity;
            ItemID = ComputeItemID();
        }

        public BaseBeverage(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((Mobile)m_Poisoner);

            Poison.Serialize(m_Poison, writer);
            writer.Write((int)m_Content);
            writer.Write((int)m_Quantity);
        }

        protected bool CheckType(string name)
        {
            return (World.LoadingType == String.Format("Server.Items.{0}", name));
        }

        public override void Deserialize(GenericReader reader)
        {
            InternalDeserialize(reader, true);
        }

        protected void InternalDeserialize(GenericReader reader, bool read)
        {
            base.Deserialize(reader);

            if (!read)
                return;

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Poisoner = reader.ReadMobile();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Poison = Poison.Deserialize(reader);
                        m_Content = (BeverageType)reader.ReadInt();
                        m_Quantity = reader.ReadInt();
                        break;
                    }
            }
        }
    }
}
