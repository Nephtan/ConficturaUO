using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Knives.TownHouses
{
    public enum Intu
    {
        Neither,
        No,
        Yes
    }

    [Flipable(0xC0B, 0xC0C)]
    public class TownHouseSign : Item
    {
        private static ArrayList s_TownHouseSigns = new ArrayList();

        public static ArrayList AllSigns
        {
            get { return s_TownHouseSigns; }
        }

        private Point3D c_BanLoc,
            c_SignLoc;

        private int c_Locks,
            c_Secures,
            c_Price,
            c_MinZ,
            c_MaxZ,
            c_MinTotalSkill,
            c_MaxTotalSkill,
            c_ItemsPrice,
            c_RTOPayments;

        private bool c_YoungOnly,
            c_RecurRent,
            c_Relock,
            c_KeepItems,
            c_LeaveItems,
            c_RentToOwn,
            c_Free,
            c_ForcePrivate,
            c_ForcePublic,
            c_NoTrade,
            c_NoBanning;

        private string c_Skill;

        private double c_SkillReq;

        private ArrayList c_Blocks, c_DecoreItemInfos, c_PreviewItems;

        private TownHouse c_House;

        private Timer c_DemolishTimer, c_RentTimer, c_PreviewTimer;

        private DateTime c_DemolishTime, c_RentTime;

        private TimeSpan c_RentByTime, c_OriginalRentTime;

        private Intu c_Murderers;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D BanLoc
        {
            get { return c_BanLoc; }
            set
            {
                c_BanLoc = value;
                InvalidateProperties();
                if (Owned)
                {
                    c_House.Region.GoLocation = value;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D SignLoc
        {
            get { return c_SignLoc; }
            set
            {
                c_SignLoc = value;
                InvalidateProperties();

                if (Owned)
                {
                    c_House.Sign.Location = value;
                    c_House.Hanger.Location = value;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Locks
        {
            get { return c_Locks; }
            set
            {
                c_Locks = value;
                InvalidateProperties();
                if (Owned)
                {
                    c_House.MaxLockDowns = value;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Secures
        {
            get { return c_Secures; }
            set
            {
                c_Secures = value;
                InvalidateProperties();
                if (Owned)
                {
                    c_House.MaxSecures = value;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Price
        {
            get { return c_Price; }
            set
            {
                c_Price = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinZ
        {
            get { return c_MinZ; }
            set
            {
                if (value > c_MaxZ)
                {
                    c_MaxZ = value + 1;
                }

                c_MinZ = value;
                if (Owned)
                {
                    RUOVersion.UpdateRegion(this);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxZ
        {
            get { return c_MaxZ; }
            set
            {
                if (value < c_MinZ)
                {
                    value = c_MinZ;
                }

                c_MaxZ = value;
                if (Owned)
                {
                    RUOVersion.UpdateRegion(this);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinTotalSkill
        {
            get { return c_MinTotalSkill; }
            set
            {
                if (value > c_MaxTotalSkill)
                {
                    value = c_MaxTotalSkill;
                }

                c_MinTotalSkill = value;
                ValidateOwnership();
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxTotalSkill
        {
            get { return c_MaxTotalSkill; }
            set
            {
                if (value < c_MinTotalSkill)
                {
                    value = c_MinTotalSkill;
                }

                c_MaxTotalSkill = value;
                ValidateOwnership();
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool YoungOnly
        {
            get { return c_YoungOnly; }
            set
            {
                c_YoungOnly = value;

                if (c_YoungOnly)
                {
                    c_Murderers = Intu.Neither;
                }

                ValidateOwnership();
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RentByTime
        {
            get { return c_RentByTime; }
            set
            {
                c_RentByTime = value;
                c_OriginalRentTime = value;

                if (value == TimeSpan.Zero)
                {
                    ClearRentTimer();
                }
                else
                {
                    ClearRentTimer();
                    BeginRentTimer(value);
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RecurRent
        {
            get { return c_RecurRent; }
            set
            {
                c_RecurRent = value;

                if (!value)
                {
                    c_RentToOwn = false;
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool KeepItems
        {
            get { return c_KeepItems; }
            set
            {
                c_LeaveItems = false;
                c_KeepItems = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Free
        {
            get { return c_Free; }
            set
            {
                c_Free = value;
                c_Price = 1;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Intu Murderers
        {
            get { return c_Murderers; }
            set
            {
                c_Murderers = value;

                ValidateOwnership();
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ForcePrivate
        {
            get { return c_ForcePrivate; }
            set
            {
                c_ForcePrivate = value;

                if (value)
                {
                    c_ForcePublic = false;

                    if (c_House != null)
                    {
                        c_House.Public = false;
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ForcePublic
        {
            get { return c_ForcePublic; }
            set
            {
                c_ForcePublic = value;

                if (value)
                {
                    c_ForcePrivate = false;

                    if (c_House != null)
                    {
                        c_House.Public = true;
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoBanning
        {
            get { return c_NoBanning; }
            set
            {
                c_NoBanning = value;

                if (value && c_House != null)
                {
                    c_House.Bans.Clear();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Type Currency { get; set; }

        public ArrayList Blocks
        {
            get { return c_Blocks; }
            set { c_Blocks = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Skill
        {
            get { return c_Skill; }
            set
            {
                c_Skill = value;
                ValidateOwnership();
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double SkillReq
        {
            get { return c_SkillReq; }
            set
            {
                c_SkillReq = value;
                ValidateOwnership();
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool LeaveItems
        {
            get { return c_LeaveItems; }
            set
            {
                c_LeaveItems = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RentToOwn
        {
            get { return c_RentToOwn; }
            set
            {
                c_RentToOwn = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Relock
        {
            get { return c_Relock; }
            set { c_Relock = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoTrade
        {
            get { return c_NoTrade; }
            set { c_NoTrade = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ItemsPrice
        {
            get { return c_ItemsPrice; }
            set
            {
                c_ItemsPrice = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TownHouse House
        {
            get { return c_House; }
            set { c_House = value; }
        }
        public Timer DemolishTimer
        {
            get { return c_DemolishTimer; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DemolishTime
        {
            get { return c_DemolishTime; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Owned
        {
            get { return c_House != null && !c_House.Deleted; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Floors
        {
            get { return (c_MaxZ - c_MinZ) / 20 + 1; }
        }

        public bool BlocksReady
        {
            get { return Blocks.Count != 0; }
        }

        public bool FloorsReady
        {
            get { return (BlocksReady && MinZ != short.MinValue); }
        }

        public bool SignReady
        {
            get { return (FloorsReady && SignLoc != Point3D.Zero); }
        }

        public bool BanReady
        {
            get { return (SignReady && BanLoc != Point3D.Zero); }
        }

        public bool LocSecReady
        {
            get { return (BanReady && Locks != 0 && Secures != 0); }
        }

        public bool ItemsReady
        {
            get { return LocSecReady; }
        }

        public bool LengthReady
        {
            get { return ItemsReady; }
        }

        public bool PriceReady
        {
            get { return (LengthReady && Price != 0); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PriceType
        {
            get
            {
                if (c_RentByTime == TimeSpan.Zero)
                {
                    return "Sale";
                }

                if (c_RentByTime == TimeSpan.FromDays(1))
                {
                    return "Daily";
                }

                if (c_RentByTime == TimeSpan.FromDays(7))
                {
                    return "Weekly";
                }

                if (c_RentByTime == TimeSpan.FromDays(30))
                {
                    return "Monthly";
                }

                return "Sale";
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PriceTypeShort
        {
            get
            {
                if (c_RentByTime == TimeSpan.Zero)
                {
                    return "Sale";
                }

                if (c_RentByTime == TimeSpan.FromDays(1))
                {
                    return "Day";
                }

                if (c_RentByTime == TimeSpan.FromDays(7))
                {
                    return "Week";
                }

                if (c_RentByTime == TimeSpan.FromDays(30))
                {
                    return "Month";
                }

                return "Sale";
            }
        }

        [Constructable]
        public TownHouseSign()
            : base(0xC0B)
        {
            Name = "This building is for sale or rent!";
            Movable = false;

            Blocks = new ArrayList();

            c_BanLoc = Point3D.Zero;
            c_SignLoc = Point3D.Zero;
            c_Skill = "";
            c_DecoreItemInfos = new ArrayList();
            c_PreviewItems = new ArrayList();
            c_DemolishTime = DateTime.Now;
            c_RentTime = DateTime.Now;
            c_RentByTime = TimeSpan.Zero;
            c_RecurRent = true;

            c_MinZ = short.MinValue;
            c_MaxZ = short.MaxValue;

            Currency = typeof(Gold);

            s_TownHouseSigns.Add(this);
        }

        private void SearchForHouse()
        {
            foreach (TownHouse house in TownHouse.AllTownHouses)
            {
                if (house.ForSaleSign == this)
                {
                    c_House = house;
                }
            }
        }

        public void UpdateBlocks()
        {
            if (!Owned)
            {
                return;
            }

            if (c_Blocks.Count == 0)
            {
                UnconvertDoors();
            }

            RUOVersion.UpdateRegion(this);
            ConvertItems(false);
            c_House.InitSectorDefinition();
        }

        public void ShowAreaPreview(Mobile m)
        {
            ClearPreview();

            Point2D point = Point2D.Zero;
            ArrayList blocks = new ArrayList();

            foreach (Rectangle2D rect in c_Blocks)
            {
                for (int x = rect.Start.X; x < rect.End.X; ++x)
                {
                    for (int y = rect.Start.Y; y < rect.End.Y; ++y)
                    {
                        point = new Point2D(x, y);

                        if (!blocks.Contains(point))
                        {
                            blocks.Add(point);
                        }
                    }
                }
            }

            if (blocks.Count > 500)
            {
                m.SendMessage("Due to size of the area, skipping the preview.");
                return;
            }

            Item item = null;

            foreach (Point2D p in blocks)
            {
                int avgz = Map.GetAverageZ(p.X, p.Y);

                c_PreviewItems.Add(
                    new Item(0x1766)
                    {
                        Name = "Area Preview",
                        Movable = false,
                        Location = new Point3D(p.X, p.Y, (avgz <= m.Z ? m.Z + 2 : avgz + 2)),
                        Map = Map
                    }
                );
            }

            c_PreviewTimer = Timer.DelayCall(
                TimeSpan.FromSeconds(100),
                new TimerCallback(ClearPreview)
            );
        }

        public void ShowSignPreview()
        {
            ClearPreview();

            Item sign = new Item(0xBD2)
            {
                Name = "Sign Preview",
                Movable = false,
                Location = SignLoc,
                Map = Map
            };

            c_PreviewItems.Add(sign);

            sign = new Item(0xB98)
            {
                Name = "Sign Preview",
                Movable = false,
                Location = SignLoc,
                Map = Map
            };

            c_PreviewItems.Add(sign);

            c_PreviewTimer = Timer.DelayCall(
                TimeSpan.FromSeconds(100),
                new TimerCallback(ClearPreview)
            );
        }

        public void ShowBanPreview()
        {
            ClearPreview();

            Item ban = new Item(0x17EE)
            {
                Name = "Ban Loc Preview",
                Movable = false,
                Location = BanLoc,
                Map = Map
            };

            c_PreviewItems.Add(ban);

            c_PreviewTimer = Timer.DelayCall(
                TimeSpan.FromSeconds(100),
                new TimerCallback(ClearPreview)
            );
        }

        public void ShowFloorsPreview(Mobile m)
        {
            ClearPreview();

            Item item = new Item(0x7BD)
            {
                Name = "Bottom Floor Preview",
                Movable = false,
                Location = m.Location,
                Z = c_MinZ,
                Map = Map
            };

            c_PreviewItems.Add(item);

            item = new Item(0x7BD)
            {
                Name = "Top Floor Preview",
                Movable = false,
                Location = m.Location,
                Z = c_MaxZ,
                Map = Map
            };

            c_PreviewItems.Add(item);

            c_PreviewTimer = Timer.DelayCall(
                TimeSpan.FromSeconds(100),
                new TimerCallback(ClearPreview)
            );
        }

        public void ClearPreview()
        {
            foreach (Item item in new ArrayList(c_PreviewItems))
            {
                c_PreviewItems.Remove(item);
                item.Delete();
            }

            if (c_PreviewTimer != null)
            {
                c_PreviewTimer.Stop();
            }

            c_PreviewTimer = null;
        }

        public void Purchase(Mobile m)
        {
            Purchase(m, false);
        }

        public void Purchase(Mobile m, bool sellitems)
        {
            try
            {
                if (Owned)
                {
                    m.SendMessage("Someone already owns this house!");
                    return;
                }

                if (!PriceReady)
                {
                    m.SendMessage("The setup for this house is not yet complete.");
                    return;
                }

                int price = c_Price;

                if (sellitems)
                {
                    price += c_ItemsPrice;
                }

                if (c_Free)
                {
                    price = 0;
                }

                bool currency = Currency != null && Currency != typeof(Gold);

                if (currency)
                {
                    if (
                        m.AccessLevel < AccessLevel.Counselor
                        && !m.BankBox.ConsumeTotal(Currency, price)
                        && !m.Backpack.ConsumeTotal(Currency, price)
                    )
                    {
                        m.SendMessage("You cannot afford this house.");
                        return;
                    }
                }
                else if (m.AccessLevel == AccessLevel.Player && !Server.Mobiles.Banker.Withdraw(m, price))
                {
                    m.SendMessage("You cannot afford this house.");
                    return;
                }

                if (currency && m.AccessLevel < AccessLevel.Counselor)
                {
                    m.SendMessage(
                        "You pay {0} {1} to purchase this house.",
                        price.ToString("#,0"),
                        SpaceWords(Currency.Name)
                    );
                }
                else if (m.AccessLevel < AccessLevel.Counselor)
                {
                    m.SendLocalizedMessage(1060398, price.ToString("#,0")); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                }

                Visible = false;

                int minX = ((Rectangle2D)c_Blocks[0]).Start.X;
                int minY = ((Rectangle2D)c_Blocks[0]).Start.Y;
                int maxX = ((Rectangle2D)c_Blocks[0]).End.X;
                int maxY = ((Rectangle2D)c_Blocks[0]).End.Y;

                foreach (Rectangle2D rect in c_Blocks)
                {
                    if (rect.Start.X < minX)
                    {
                        minX = rect.Start.X;
                    }

                    if (rect.Start.Y < minY)
                    {
                        minY = rect.Start.Y;
                    }

                    if (rect.End.X > maxX)
                    {
                        maxX = rect.End.X;
                    }

                    if (rect.End.Y > maxY)
                    {
                        maxY = rect.End.Y;
                    }
                }

                c_House = new TownHouse(m, this, c_Locks, c_Secures);

                c_House.Components.Resize(maxX - minX, maxY - minY);
                c_House.Components.Add(
                    0x520,
                    c_House.Components.Width - 1,
                    c_House.Components.Height - 1,
                    -5
                );

                c_House.Location = new Point3D(minX, minY, Map.GetAverageZ(minX, minY));
                c_House.Map = Map;
                c_House.Region.GoLocation = c_BanLoc;
                c_House.Sign.Location = c_SignLoc;
                c_House.Hanger = new Item(0xB9C)
                {
                    Location = c_SignLoc,
                    Map = Map,
                    Movable = false
                };

                if (c_ForcePublic)
                {
                    c_House.Public = true;
                }

                c_House.Price = (RentByTime <= TimeSpan.Zero ? c_Price : 1);

                RUOVersion.UpdateRegion(this);

                c_House.Price = Math.Max(1, House.Price);

                if (c_RentByTime != TimeSpan.Zero)
                {
                    BeginRentTimer(c_RentByTime);
                }

                c_RTOPayments = 1;

                HideOtherSigns();

                c_DecoreItemInfos = new ArrayList();

                ConvertItems(sellitems);
            }
            catch (Exception e)
            {
                Errors.Report(
                    "An error occurred during home purchasing.  More information available on the console."
                );
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }

        private void HideOtherSigns()
        {
            foreach (Item item in c_House.Sign.GetItemsInRange(0))
            {
                if (!(item is HouseSign))
                {
                    if (
                        item.ItemID == 0xB95
                        || item.ItemID == 0xB96
                        || item.ItemID == 0xC43
                        || item.ItemID == 0xC44
                        || (item.ItemID > 0xBA3 && item.ItemID < 0xC0E)
                    )
                    {
                        item.Visible = false;
                    }
                }
            }
        }

        public virtual void ConvertItems(bool keep)
        {
            if (c_House == null)
            {
                return;
            }

            ArrayList items = new ArrayList();

            foreach (Rectangle2D rect in c_Blocks)
            {
                foreach (Item item in Map.GetItemsInBounds(rect))
                {
                    if (
                        c_House.Region.Contains(item.Location)
                        && item.RootParent == null
                        && !items.Contains(item)
                    )
                    {
                        items.Add(item);
                    }
                }
            }

            foreach (Item item in new ArrayList(items))
            {
                if (
                    item is HouseSign
                    || item is BaseMulti
                    || item is BaseAddon
                    || item is AddonComponent
                    || item == c_House.Hanger
                    || !item.Visible
                    || item.IsLockedDown
                    || item.IsSecure
                    || item.Movable
                    || c_PreviewItems.Contains(item)
                )
                {
                    continue;
                }

                if (item is BaseDoor)
                {
                    ConvertDoor((BaseDoor)item);
                }
                else if (!c_LeaveItems)
                {
                    c_DecoreItemInfos.Add(
                        new DecoreItemInfo(
                            item.GetType().ToString(),
                            item.Name,
                            item.ItemID,
                            item.Hue,
                            item.Location,
                            item.Map
                        )
                    );

                    if (!c_KeepItems || !keep)
                    {
                        item.Delete();
                    }
                    else
                    {
                        item.Movable = true;
                        c_House.LockDown(c_House.Owner, item, false);
                    }
                }
            }
        }

        protected void ConvertDoor(BaseDoor door)
        {
            if (!Owned)
            {
                return;
            }

            if (door is Server.Gumps.ISecurable)
            {
                door.Locked = false;
                c_House.Doors.Add(door);
                return;
            }

            door.Open = false;

            GenericHouseDoor newdoor = new GenericHouseDoor((DoorFacing)0, door.ClosedID, door.OpenedSound, door.ClosedSound)
            {
                Offset = door.Offset,
                ClosedID = door.ClosedID,
                OpenedID = door.OpenedID,
                Location = door.Location,
                Map = door.Map
            };

            door.Delete();

            foreach (Item inneritem in newdoor.GetItemsInRange(1))
            {
                if (inneritem is BaseDoor && inneritem != newdoor && inneritem.Z == newdoor.Z)
                {
                    ((BaseDoor)inneritem).Link = newdoor;
                    newdoor.Link = (BaseDoor)inneritem;
                }
            }

            c_House.Doors.Add(newdoor);
        }

        public virtual void UnconvertDoors()
        {
            if (c_House == null)
            {
                return;
            }

            BaseDoor newdoor = null;

            foreach (BaseDoor door in new ArrayList(c_House.Doors))
            {
                door.Open = false;

                if (c_Relock)
                {
                    door.Locked = true;
                }

                newdoor = new StrongWoodDoor((DoorFacing)0)
                {
                    ItemID = door.ItemID,
                    ClosedID = door.ClosedID,
                    OpenedID = door.OpenedID,
                    OpenedSound = door.OpenedSound,
                    ClosedSound = door.ClosedSound,
                    Offset = door.Offset,
                    Location = door.Location,
                    Map = door.Map
                };

                door.Delete();

                foreach (Item inneritem in newdoor.GetItemsInRange(1))
                {
                    if (inneritem is BaseDoor && inneritem != newdoor && inneritem.Z == newdoor.Z)
                    {
                        ((BaseDoor)inneritem).Link = newdoor;
                        newdoor.Link = (BaseDoor)inneritem;
                    }
                }

                c_House.Doors.Remove(door);
            }
        }

        public void RecreateItems()
        {
            foreach (DecoreItemInfo info in c_DecoreItemInfos)
            {
                Item item = null;

                if (info.TypeString.ToLower().IndexOf("static") != -1)
                {
                    item = new Static(info.ItemID);
                }
                else
                {
                    try
                    {
                        item =
                            Activator.CreateInstance(
                                ScriptCompiler.FindTypeByFullName(info.TypeString)
                            ) as Item;
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (item == null)
                {
                    continue;
                }

                item.ItemID = info.ItemID;
                item.Name = info.Name;
                item.Hue = info.Hue;
                item.Location = info.Location;
                item.Map = info.Map;
                item.Movable = false;
            }
        }

        private void TryCatch(Action a)
        {
            try
            {
                a();
            }
            catch { }
        }

        public virtual void ClearHouse()
        {
            TryCatch(UnconvertDoors);
            TryCatch(ClearDemolishTimer);
            TryCatch(ClearRentTimer);
            // TryCatch(PackUpItems);
            TryCatch(RecreateItems);

            c_House = null;
            Visible = true;

            if (c_RentToOwn)
            {
                c_RentByTime = c_OriginalRentTime;
            }
        }

        public virtual void ValidateOwnership()
        {
            if (!Owned)
            {
                return;
            }

            if (c_House.Owner == null)
            {
                c_House.Delete();
                return;
            }

            if (c_House.Owner.AccessLevel != AccessLevel.Player)
            {
                return;
            }

            if (!CanBuyHouse(c_House.Owner) && c_DemolishTimer == null)
            {
                BeginDemolishTimer();
            }
            else
            {
                ClearDemolishTimer();
            }
        }

        public int CalcVolume()
        {
            int floors = 1;

            if (c_MaxZ - c_MinZ < 100)
            {
                floors = 1 + Math.Abs((c_MaxZ - c_MinZ) / 20);
            }

            Point3D point = Point3D.Zero;
            ArrayList blocks = new ArrayList();

            foreach (Rectangle2D rect in c_Blocks)
            {
                for (int x = rect.Start.X; x < rect.End.X; ++x)
                {
                    for (int y = rect.Start.Y; y < rect.End.Y; ++y)
                    {
                        for (int z = 0; z < floors; z++)
                        {
                            point = new Point3D(x, y, z);

                            if (!blocks.Contains(point))
                            {
                                blocks.Add(point);
                            }
                        }
                    }
                }
            }

            return blocks.Count;
        }

        private void StartTimers()
        {
            if (c_DemolishTime > DateTime.Now)
            {
                BeginDemolishTimer(c_DemolishTime - DateTime.Now);
            }
            else if (c_RentByTime != TimeSpan.Zero)
            {
                BeginRentTimer(c_RentByTime);
            }
        }

        #region Demolish
        public void ClearDemolishTimer()
        {
            if (c_DemolishTimer == null)
            {
                return;
            }

            c_DemolishTimer.Stop();
            c_DemolishTimer = null;
            c_DemolishTime = DateTime.Now;

            if (!c_House.Deleted && Owned)
            {
                c_House.Owner.SendMessage("Demolition canceled.");
            }
        }

        public void CheckDemolishTimer()
        {
            if (c_DemolishTimer == null || !Owned)
            {
                return;
            }

            DemolishAlert();
        }

        protected void BeginDemolishTimer()
        {
            BeginDemolishTimer(TimeSpan.FromHours(24));
        }

        protected void BeginDemolishTimer(TimeSpan time)
        {
            if (!Owned)
            {
                return;
            }

            c_DemolishTime = DateTime.Now + time;
            c_DemolishTimer = Timer.DelayCall(time, new TimerCallback(PackUpHouse));

            DemolishAlert();
        }

        protected virtual void DemolishAlert()
        {
            c_House.Owner.SendMessage(
                "You no longer meet the requirements for your town house, which will be demolished automatically in {0}:{1}:{2}.",
                (c_DemolishTime - DateTime.Now).Hours,
                (c_DemolishTime - DateTime.Now).Minutes,
                (c_DemolishTime - DateTime.Now).Seconds
            );
        }

        protected void PackUpHouse()
        {
            if (!Owned || c_House.Deleted)
            {
                return;
            }

            // TryCatch(PackUpItems);

            c_House.Owner.BankBox.DropItem(new BankCheck(c_House.Price));

            try
            {
                c_House.Delete();
            }
            catch
            {
                Errors.Report("The infamous SVN bug has occured.");
            }
        }

        /* Added for fixing Rental Loop Crash bug */
        protected void DeleteTest()
        {
            if (!Owned || c_House.Deleted)
                return;

            // TryCatch(PackUpItems);
            c_House.Owner.BankBox.DropItem(new BankCheck(c_House.Price));
        }

        //protected void PackUpItems()
        //{
        //    if (c_House == null || c_House.Deleted || c_House.Map == null || c_House.Map == Map.Internal)
        //    {
        //        return;
        //    }

        //    Container bag = new Bag();
        //    bag.Name = "Town House Belongings";

        //    // Handling LockDowns with TryCatch
        //    ArrayList lockDowns = new ArrayList(c_House.LockDowns);

        //    foreach (Item item in lockDowns)
        //    {
        //        TryCatch(() =>
        //        {
        //            c_House.LockDowns.Remove(item);
        //            item.IsLockedDown = false;
        //            item.Movable = true;
        //            bag.DropItem(item);
        //        });
        //    }

        //    // Handling Secures with TryCatch
        //    ArrayList secures = new ArrayList(c_House.Secures);

        //    foreach (SecureInfo info in secures)
        //    {
        //        TryCatch(() =>
        //        {
        //            c_House.Secures.Remove(info);
        //            if (info.Item != null)
        //            {
        //                info.Item.IsLockedDown = false;
        //                info.Item.IsSecure = false;
        //                info.Item.Movable = true;
        //                info.Item.SetLastMoved();
        //                bag.DropItem(info.Item);
        //            }
        //        });
        //    }

        //    // Handling Blocks with TryCatch
        //    foreach (Rectangle2D rect in c_Blocks)
        //    {
        //        ArrayList itemsInBounds = new ArrayList();

        //        foreach (Item item in Map.GetItemsInBounds(rect))
        //        {
        //            itemsInBounds.Add(item);
        //        }

        //        foreach (Item item in itemsInBounds)
        //        {
        //            TryCatch(() =>
        //            {
        //                if (item is HouseSign
        //                    || item is BaseDoor
        //                    || item is BaseMulti
        //                    || item is BaseAddon
        //                    || item is AddonComponent
        //                    || !item.Visible
        //                    || item.IsLockedDown
        //                    || item.IsSecure
        //                    || !item.Movable
        //                    || item.Map != c_House.Map
        //                    || !c_House.Region.Contains(item.Location))
        //                {
        //                    return; // Using return instead of continue in TryCatch
        //                }

        //                bag.DropItem(item);
        //            });
        //        }
        //    }

        //    // Deleting the bag if empty, same as before
        //    if (bag.Items.Count == 0)
        //    {
        //        bag.Delete();
        //        return;
        //    }

        //    c_House.Owner.BankBox.DropItem(bag);
        //}
        #endregion

        #region Rent
        public void ClearRentTimer()
        {
            if (c_RentTimer != null)
            {
                c_RentTimer.Stop();
                c_RentTimer = null;
            }

            c_RentTime = DateTime.Now;
        }

        private void BeginRentTimer()
        {
            BeginRentTimer(TimeSpan.FromDays(1));
        }

        private void BeginRentTimer(TimeSpan time)
        {
            if (!Owned)
            {
                return;
            }

            c_RentTimer = Timer.DelayCall(time, new TimerCallback(RentDue));
            c_RentTime = DateTime.Now + time;
        }

        public void CheckRentTimer()
        {
            if (c_RentTimer == null || !Owned)
            {
                return;
            }

            c_House.Owner.SendMessage(
                "This rent cycle ends in {0} days, {1}:{2}:{3}.",
                (c_RentTime - DateTime.Now).Days,
                (c_RentTime - DateTime.Now).Hours,
                (c_RentTime - DateTime.Now).Minutes,
                (c_RentTime - DateTime.Now).Seconds
            );
        }

        private void RentDue()
        {
            if (!Owned || c_House.Owner == null)
            {
                return;
            }

            if (!c_RecurRent)
            {
                c_House.Owner.SendMessage(
                    "Your town house rental contract has expired, and the bank has once again taken possession."
                );
                PackUpHouse();
                return;
            }

            bool currency = Currency != null && Currency != typeof(Gold);

            if (
                currency
                && !c_Free
                && c_House.Owner.AccessLevel < AccessLevel.Counselor
                && !c_House.Owner.BankBox.ConsumeTotal(Currency, c_Price)
                && !c_House.Owner.Backpack.ConsumeTotal(Currency, c_Price)
            )
            {
                c_House.Owner.SendMessage(
                    "Since you can not afford the rent, the bank has reclaimed your town house."
                );
                PackUpHouse();
                return;
            }

            if (
                !currency
                && !c_Free
                && c_House.Owner.AccessLevel < AccessLevel.Counselor
                && !Server.Mobiles.Banker.Withdraw(c_House.Owner, c_Price)
            )
            {
                c_House.Owner.SendMessage(
                    "Since you can not afford the rent, the bank has reclaimed your town house."
                );
                PackUpHouse();
                return;
            }

            if (!c_Free && !currency)
            {
                c_House.Owner.SendMessage(
                    "The bank has withdrawn {0} gold rent for your town house.",
                    c_Price
                );
            }

            if (!c_Free && currency)
            {
                c_House.Owner.SendMessage(
                    "You pay your rent with {0} {1}.",
                    c_Price,
                    SpaceWords(Currency.Name)
                );
            }

            OnRentPaid();

            if (c_RentToOwn)
            {
                c_RTOPayments++;

                bool complete = false;

                if (c_RentByTime == TimeSpan.FromDays(1) && c_RTOPayments >= 60)
                {
                    complete = true;
                    c_House.Price = c_Price * 60;
                }

                if (c_RentByTime == TimeSpan.FromDays(7) && c_RTOPayments >= 9)
                {
                    complete = true;
                    c_House.Price = c_Price * 9;
                }

                if (c_RentByTime == TimeSpan.FromDays(30) && c_RTOPayments >= 2)
                {
                    complete = true;
                    c_House.Price = c_Price * 2;
                }

                if (complete)
                {
                    c_House.Owner.SendMessage("You now own your rental home.");
                    c_RentByTime = TimeSpan.Zero;
                    return;
                }
            }

            BeginRentTimer(c_RentByTime);
        }

        protected virtual void OnRentPaid() { }

        public void NextPriceType()
        {
            if (c_RentByTime == TimeSpan.Zero)
            {
                RentByTime = TimeSpan.FromDays(1);
            }
            else if (c_RentByTime == TimeSpan.FromDays(1))
            {
                RentByTime = TimeSpan.FromDays(7);
            }
            else if (c_RentByTime == TimeSpan.FromDays(7))
            {
                RentByTime = TimeSpan.FromDays(30);
            }
            else
            {
                RentByTime = TimeSpan.Zero;
            }
        }

        public void PrevPriceType()
        {
            if (c_RentByTime == TimeSpan.Zero)
            {
                RentByTime = TimeSpan.FromDays(30);
            }
            else if (c_RentByTime == TimeSpan.FromDays(30))
            {
                RentByTime = TimeSpan.FromDays(7);
            }
            else if (c_RentByTime == TimeSpan.FromDays(7))
            {
                RentByTime = TimeSpan.FromDays(1);
            }
            else
            {
                RentByTime = TimeSpan.Zero;
            }
        }
        #endregion

        public bool CanBuyHouse(Mobile m)
        {
            if (c_Skill != "")
            {
                try
                {
                    SkillName index = (SkillName)Enum.Parse(typeof(SkillName), c_Skill, true);

                    if (m.Skills[index].Value < c_SkillReq)
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            if (c_MinTotalSkill != 0 && m.SkillsTotal / 10 < c_MinTotalSkill)
            {
                return false;
            }

            if (c_MaxTotalSkill != 0 && m.SkillsTotal / 10 > c_MaxTotalSkill)
            {
                return false;
            }

            if (c_YoungOnly && m.Player && !((PlayerMobile)m).Young)
            {
                return false;
            }

            if (c_Murderers == Intu.Yes && m.Kills < 5)
            {
                return false;
            }

            if (c_Murderers == Intu.No && m.Kills >= 5)
            {
                return false;
            }

            return true;
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (m.AccessLevel != AccessLevel.Player)
            {
                m.SendGump(new TownHouseSetupGump(m, this));
                return;
            }

            if (!Visible)
            {
                return;
            }

            if (CanBuyHouse(m) && !BaseHouse.HasAccountHouse(m))
            {
                new TownHouseConfirmGump(m, this);
            }
            else
            {
                m.SendMessage("You cannot purchase this house.");
                return;
            }
        }

        public override void Delete()
        {
            if (c_House == null || c_House.Deleted)
            {
                base.Delete();
            }
            else
            {
                PublicOverheadMessage(
                    Server.Network.MessageType.Regular,
                    0x0,
                    true,
                    "You cannot delete this while the home is owned."
                );
            }

            if (this.Deleted)
            {
                s_TownHouseSigns.Remove(this);
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            string currencyName = Currency != null && Currency != typeof(Gold) ? SpaceWords(Currency.Name) : "Gold";
            string cost = String.Format("{0:n0}", c_Price);
            string cost_items = String.Format("{0:n0}", c_ItemsPrice);
            string locks = String.Format("{0:n0}", c_Locks);
            string secures = String.Format("{0:n0}", c_Secures);

            if (c_Free)
            {
                list.Add(1060658, "Price\tFree");
            }
            else if (c_RentByTime == TimeSpan.Zero)
            {
                list.Add(
                    1060658,
                    "Price\t{0} {1} {2}",
                    cost,
                    currencyName,
                    c_KeepItems ? " (+" + cost_items + " for the items)" : ""
                );
            }
            else if (c_RecurRent)
            {
                list.Add(
                    1060658,
                    "{0}\t{1} {2}\t{3}",
                    PriceType + (c_RentToOwn ? " Rent-to-Own" : " Recurring"),
                    cost,
                    currencyName,
                    c_KeepItems ? "(+" + cost_items + " for the items)" : ""
                );
            }
            else
            {
                list.Add(
                    1060658,
                    "One {0}\t{1} {2} {3}",
                    PriceTypeShort,
                    cost,
                    currencyName,
                    c_KeepItems ? "(+" + cost_items + " for the items)" : ""
                );
            }

            list.Add(1060659, "Lockdowns\t{0}", locks);
            list.Add(1060660, "Secures\t{0}", secures);

            if (c_SkillReq != 0.0)
            {
                list.Add(1060661, "Requires\t{0}", c_SkillReq + " in " + c_Skill);
            }

            if (c_MinTotalSkill != 0)
            {
                list.Add(1060662, "Requires more than\t{0} total skills", c_MinTotalSkill);
            }

            if (c_MaxTotalSkill != 0)
            {
                list.Add(1060663, "Requires less than\t{0} total skills", c_MaxTotalSkill);
            }

            if (c_YoungOnly)
            {
                list.Add(1063483, "Must be\tYoung");
            }
            else if (c_Murderers == Intu.Yes)
            {
                list.Add(1063483, "Must be\ta murderer");
            }
            else if (c_Murderers == Intu.No)
            {
                list.Add(1063483, "Must be\tinnocent");
            }
        }

        public TownHouseSign(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(14);

            // Version 14
            writer.Write(Currency != null ? Currency.FullName : null);

            // Version 13

            writer.Write(c_ForcePrivate);
            writer.Write(c_ForcePublic);
            writer.Write(c_NoTrade);

            // Version 12

            writer.Write(c_Free);

            // Version 11

            writer.Write((int)c_Murderers);

            // Version 10

            writer.Write(c_LeaveItems);

            // Version 9
            writer.Write(c_RentToOwn);
            writer.Write(c_OriginalRentTime);
            writer.Write(c_RTOPayments);

            // Version 7
            writer.WriteItemList(c_PreviewItems, true);

            // Version 6
            writer.Write(c_ItemsPrice);
            writer.Write(c_KeepItems);

            // Version 5
            writer.Write(c_DecoreItemInfos.Count);

            foreach (DecoreItemInfo info in c_DecoreItemInfos)
            {
                info.Save(writer);
            }

            writer.Write(c_Relock);

            // Version 4
            writer.Write(c_RecurRent);
            writer.Write(c_RentByTime);
            writer.Write(c_RentTime);
            writer.Write(c_DemolishTime);
            writer.Write(c_YoungOnly);
            writer.Write(c_MinTotalSkill);
            writer.Write(c_MaxTotalSkill);

            // Version 3
            writer.Write(c_MinZ);
            writer.Write(c_MaxZ);

            // Version 2
            writer.Write(c_House);

            // Version 1
            writer.Write(c_Price);
            writer.Write(c_Locks);
            writer.Write(c_Secures);
            writer.Write(c_BanLoc);
            writer.Write(c_SignLoc);
            writer.Write(c_Skill);
            writer.Write(c_SkillReq);
            writer.Write(c_Blocks.Count);

            foreach (Rectangle2D rect in c_Blocks)
            {
                writer.Write(rect);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 14)
                Currency = ScriptCompiler.FindTypeByFullName(reader.ReadString());

            if (Currency == null)
                Currency = typeof(Gold);

            if (version >= 13)
            {
                c_ForcePrivate = reader.ReadBool();
                c_ForcePublic = reader.ReadBool();
                c_NoTrade = reader.ReadBool();
            }

            if (version >= 12)
            {
                c_Free = reader.ReadBool();
            }

            if (version >= 11)
            {
                c_Murderers = (Intu)reader.ReadInt();
            }

            if (version >= 10)
            {
                c_LeaveItems = reader.ReadBool();
            }

            if (version >= 9)
            {
                c_RentToOwn = reader.ReadBool();
                c_OriginalRentTime = reader.ReadTimeSpan();
                c_RTOPayments = reader.ReadInt();
            }

            c_PreviewItems = new ArrayList();

            if (version >= 7)
            {
                c_PreviewItems = reader.ReadItemList();
            }

            if (version >= 6)
            {
                c_ItemsPrice = reader.ReadInt();
                c_KeepItems = reader.ReadBool();
            }

            c_DecoreItemInfos = new ArrayList();

            if (version >= 5)
            {
                int decorecount = reader.ReadInt();

                DecoreItemInfo info;

                for (int i = 0; i < decorecount; ++i)
                {
                    info = new DecoreItemInfo();
                    info.Load(reader);
                    c_DecoreItemInfos.Add(info);
                }

                c_Relock = reader.ReadBool();
            }

            if (version >= 4)
            {
                c_RecurRent = reader.ReadBool();
                c_RentByTime = reader.ReadTimeSpan();
                c_RentTime = reader.ReadDateTime();
                c_DemolishTime = reader.ReadDateTime();
                c_YoungOnly = reader.ReadBool();
                c_MinTotalSkill = reader.ReadInt();
                c_MaxTotalSkill = reader.ReadInt();
            }

            if (version >= 3)
            {
                c_MinZ = reader.ReadInt();
                c_MaxZ = reader.ReadInt();
            }

            if (version >= 2)
            {
                c_House = (TownHouse)reader.ReadItem();
            }

            c_Price = reader.ReadInt();
            c_Locks = reader.ReadInt();
            c_Secures = reader.ReadInt();
            c_BanLoc = reader.ReadPoint3D();
            c_SignLoc = reader.ReadPoint3D();
            c_Skill = reader.ReadString();
            c_SkillReq = reader.ReadDouble();

            int count = reader.ReadInt();

            c_Blocks = new ArrayList();

            for (int i = 0; i < count; ++i)
            {
                c_Blocks.Add(reader.ReadRect2D());
            }

            if (c_RentTime > DateTime.Now)
            {
                BeginRentTimer(c_RentTime - DateTime.Now);
            }

            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(StartTimers));

            ClearPreview();

            s_TownHouseSigns.Add(this);
        }

        private static readonly Regex _SpaceWordsRegex = new Regex(
            @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))"
        );

        private static string SpaceWords(string str, params char[] whiteSpaceAlias)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return str ?? String.Empty;
            }

            if (whiteSpaceAlias == null || whiteSpaceAlias.Length == 0)
            {
                whiteSpaceAlias = new[] { '_' };
            }

            // Replace specified characters with spaces
            foreach (char c in whiteSpaceAlias)
            {
                str = str.Replace(c, ' ');
            }

            // Apply regex to add space before each word character
            str = _SpaceWordsRegex.Replace(str, " $0");

            // Split by space, filter out empty entries, and rejoin with single spaces
            string[] parts = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            str = String.Join(" ", parts);

            return str;
        }
    }
}
