using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using Knives.TownHouses;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Targeting;

// Run only through scripts/Test-TownHouseRegions.ps1, outside the live script tree.
// Production types run against in-memory fixtures; no Configure/Initialize, saves,
// timer thread, listener, or connected client is started.
internal static class TownHouseRegionRegression
{
    private static Map s_Map;
    private static int s_Checks;

    private sealed class Actor : Mobile
    {
        public bool Dead;
        public override bool Alive { get { return !Dead && base.Alive; } }
    }

    private sealed class ClassicHouse : BaseHouse
    {
        public override Rectangle2D[] Area { get { return SmallOldHouse.AreaArray; } }
        public override Point3D BaseBanLocation { get { return Point3D.Zero; } }
        public override int GetAosMaxSecures() { return 10; }
        public override int GetAosMaxLockdowns() { return 1000; }

        public ClassicHouse(Mobile owner) : base(0x64, owner, 1000, 10)
        {
            SetSign(2, 4, 5);
        }
    }

    private static void Check(bool condition, string description)
    {
        if (!condition)
            throw new Exception("FAILED: " + description);
        ++s_Checks;
    }

    private static TownHouse CreateHouse(Actor owner, TownHouseSign sign, int x, int minZ, int maxZ)
    {
        sign.Blocks.Add(new Rectangle2D(x, 64, 12, 12));
        sign.MinZ = minZ;
        sign.MaxZ = maxZ;
        sign.BanLoc = new Point3D(x - 1, 64, minZ);
        sign.Map = s_Map;
        TownHouse house = new TownHouse(owner, sign, 1000, 10);
        sign.House = house;
        house.Location = new Point3D(x, 64, 0);
        house.Map = s_Map;
        house.Public = true;
        house.InitSectorDefinition();
        RUOVersion.UpdateRegion(sign);
        return house;
    }

    private static void Move(Actor actor, Point3D point)
    {
        actor.Target = null;
        actor.MoveToWorld(point, s_Map);
    }

    private static void Press(Actor actor, InteriorDecorator tool, int button)
    {
        Type gumpType = typeof(InteriorDecorator).GetNestedType("InternalGump", BindingFlags.NonPublic);
        Gump gump = (Gump)Activator.CreateInstance(gumpType, new object[] { tool });
        // OnResponse only needs the input envelope's Mobile; the actor has no NetState.
        NetState input = (NetState)FormatterServices.GetUninitializedObject(typeof(NetState));
        input.Mobile = actor;
        actor.Target = null;
        gump.OnResponse(input, new RelayInfo(button, new int[0], new TextRelay[0]));
    }

    private static void Speak(Actor actor, int keyword, string speech)
    {
        actor.Target = null;
        actor.DoSpeech(speech, new int[] { keyword }, MessageType.Regular, 0);
    }

    private static void CompleteTarget(Actor actor, Item item)
    {
        Target target = actor.Target;
        Check(target != null, "action has a target before completion");
        target.Invoke(actor, item);
    }

    private static Item Trash(BaseHouse house)
    {
        return (Item)typeof(BaseHouse).GetField("m_Trash", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(house);
    }

    private static void TestBoundaries()
    {
        Actor owner = new Actor();
        TownHouseSign sign = new TownHouseSign();
        TownHouse house = CreateHouse(owner, sign, 64, 50, 90);
        sign.Blocks = new ArrayList(new Rectangle2D[] {
            new Rectangle2D(64, 64, 10, 4),
            new Rectangle2D(64, 68, 4, 6),
            new Rectangle2D(82, 64, 4, 4)
        });
        RUOVersion.UpdateRegion(sign);
        Check(house.Z < sign.MinZ, "fixture reproduces the sky-house ground/floor mismatch");
        Check(house.Region.Area.Length == 3, "each configured block contributes a region rectangle");
        Check(house.Region.Contains(new Point3D(64, 64, 50)), "minimum X/Y/Z are included");
        Check(house.Region.Contains(new Point3D(73, 67, 89)), "last interior tile and Z are included");
        Check(!house.Region.Contains(new Point3D(74, 65, 60)), "maximum X is excluded");
        Check(!house.Region.Contains(new Point3D(65, 74, 60)), "maximum Y is excluded");
        Check(!house.Region.Contains(new Point3D(65, 65, 90)), "maximum Z is excluded");
        Check(!house.Region.Contains(new Point3D(65, 65, 49)), "below-floor Z is excluded");
        Check(!house.Region.Contains(new Point3D(70, 70, 60)), "L-shaped footprint keeps its missing corner");
        Check(!house.Region.Contains(new Point3D(79, 65, 60)), "disconnected blocks keep their gap");
        Check(house.Region.Contains(new Point3D(83, 65, 60)), "disconnected block remains included");
        Check(house.Region.GoLocation == sign.BanLoc, "configured ban location is retained");
        Check(house.Region.Priority == HouseRegion.HousePriority, "ordinary townhouse priority is unchanged");

        Region previous = house.Region;
        sign.MinZ = 60;
        Check(!previous.Registered, "floor edit unregisters the previous region");
        Check(!house.Region.Contains(new Point3D(65, 65, 59)), "minimum-floor edit takes effect");
        sign.MaxZ = 80;
        Check(!house.Region.Contains(new Point3D(65, 65, 80)), "maximum-floor edit takes effect");
        sign.Blocks.Add(new Rectangle2D(90, 64, 2, 2));
        sign.UpdateBlocks();
        Check(Region.Find(new Point3D(90, 64, 60), s_Map) == house.Region, "block edit registers the new footprint");

        previous = house.Region;
        typeof(TownHouse).GetMethod("FinishInit", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(house, null);
        Check(!previous.Registered && house.Region.Registered, "delayed initialization rebuilds the region");
        Check(house.Region.Contains(new Point3D(90, 64, 60)), "delayed initialization retains configured bounds");
        previous = house.Region;
        typeof(General).GetMethod("OnStarted", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
        Check(!previous.Registered && Region.Find(new Point3D(90, 64, 60), s_Map) == house.Region,
            "server-started callback rebuilds and registers the configured region");

        TownHouse unlinked = new TownHouse((Serial)0x40001000);
        Check(new HouseRegion(unlinked).Area.Length == 0, "unlinked deserialization constructor is safe");
        sign.Blocks.Clear();
        sign.Blocks.Add(new Rectangle2D(64, 64, 0, 4));
        sign.Blocks.Add(new Rectangle2D(64, 64, 4, 0));
        RUOVersion.UpdateRegion(sign);
        Check(house.Region.Area.Length == 0, "zero-sized blocks do not create a region");
        sign.Blocks = null;
        RUOVersion.UpdateRegion(sign);
        Check(house.Region.Area.Length == 0, "missing block data is safe");
        sign.Blocks = new ArrayList();
        sign.Blocks.Add(new Rectangle2D(64, 64, 4, 4));
        sign.MaxZ = sign.MinZ;
        Check(house.Region.Area.Length == 0, "empty vertical interval is safe");
        RUOVersion.UpdateRegion(null);
        RUOVersion.UpdateRegion(new TownHouseSign());
        Check(true, "refresh accepts absent or unowned signs without throwing");
        Console.WriteLine("PASS: configured boundaries, edits, and startup callbacks");
    }

    private static void TestActions(BaseHouse house, Actor owner, Point3D inside)
    {
        Move(owner, inside);
        Check(owner.Region == house.Region, "speaker resolves to the expected house region");
        Check(InteriorDecorator.InHouse(owner), "decorator recognizes the owner's house");
        InteriorDecorator tool = new InteriorDecorator();
        Item item = new Item(0xEED);
        item.MoveToWorld(inside, s_Map);
        Press(owner, tool, 8);
        Check(owner.Target is LockdownTarget, "Lock button creates a lockdown cursor");
        CompleteTarget(owner, item);
        Check(item.IsLockedDown && !item.Movable, "Lock button successfully locks the item");
        Press(owner, tool, 10);
        CompleteTarget(owner, item);
        Check(!item.IsLockedDown && item.Movable, "Release button successfully releases the item");
        Container container = new Container(0xE7D);
        container.MoveToWorld(inside, s_Map);
        Press(owner, tool, 9);
        Check(owner.Target is SecureTarget, "Secure button creates a secure cursor");
        CompleteTarget(owner, container);
        Check(container.IsSecure && !container.Movable, "Secure button successfully secures the container");
        Speak(owner, 0x24, "I wish to release this");
        CompleteTarget(owner, container);
        Check(!container.IsSecure && container.Movable, "spoken Release releases a secure container");
        Speak(owner, 0x23, "I wish to lock this down");
        CompleteTarget(owner, item);
        Check(item.IsLockedDown, "spoken Lock successfully locks the item");
        Speak(owner, 0x24, "I wish to release this");
        CompleteTarget(owner, item);
        Check(!item.IsLockedDown, "spoken Release successfully releases the item");
        Speak(owner, 0x25, "I wish to secure this");
        CompleteTarget(owner, container);
        Check(container.IsSecure, "spoken Secure successfully secures the container");

        Actor coOwner = new Actor();
        house.CoOwners.Add(coOwner);
        Move(coOwner, inside);
        Press(coOwner, tool, 8);
        Check(coOwner.Target is LockdownTarget, "co-owner may lock items");
        Press(coOwner, tool, 10);
        Check(coOwner.Target is LockdownTarget, "co-owner may release items");
        Press(coOwner, tool, 9);
        Check(coOwner.Target == null, "co-owner cannot initiate owner-only securing");
        Press(coOwner, tool, 12);
        Item trash = Trash(house);
        Check(trash is TrashBarrel && trash.Location == inside, "Trash places a barrel at the co-owner's feet");
        Check(coOwner.Target == null, "Trash does not create a target");
        Speak(owner, 0x28, "trash barrel");
        Check(Trash(house) == trash, "spoken Trash preserves the existing barrel");

        Actor stranger = new Actor();
        Move(stranger, inside);
        Check(!InteriorDecorator.InHouse(stranger), "decorator rejects a stranger");
        int[] buttons = new int[] { 8, 9, 10, 12 };
        int[] keywords = new int[] { 0x23, 0x25, 0x24, 0x28 };
        for (int i = 0; i < buttons.Length; ++i)
        {
            Press(stranger, tool, buttons[i]);
            Check(stranger.Target == null, "stale/forged button response does not grant stranger access");
            Speak(stranger, keywords[i], "house command");
            Check(stranger.Target == null, "spoken command does not grant stranger access");
            owner.Dead = true;
            Press(owner, tool, buttons[i]);
            Check(owner.Target == null, "dead owner cannot start a house action");
            Speak(owner, keywords[i], "house command");
            Check(owner.Target == null, "dead owner cannot start a spoken house action");
            owner.Dead = false;
        }
        Check(Trash(house) == trash, "rejected commands do not replace the trash barrel");
        Move(owner, new Point3D(450, 450, 0));
        Press(owner, tool, 8);
        Check(owner.Target == null, "stale gump outside the house does not start a lockdown");
        Console.WriteLine("PASS: buttons, speech, mutations, Trash, and permissions for " + house.GetType().Name);
    }

    private static void TestRentalsAndClassicHouse()
    {
        Actor landlord = new Actor();
        Actor tenant = new Actor();
        TownHouse parent = CreateHouse(landlord, new TownHouseSign(), 240, 40, 100);
        RentalContract contract = new RentalContract();
        TownHouse rental = CreateHouse(tenant, contract, 240, 60, 80);
        Check(rental.Region.Priority == parent.Region.Priority + 1, "rental priority is immediately above parent");
        Check(Region.Find(new Point3D(243, 67, 65), s_Map) == rental.Region, "rental wins inside its vertical interval");
        Check(Region.Find(new Point3D(243, 67, 59), s_Map) == parent.Region, "parent wins below the rental");
        Check(Region.Find(new Point3D(243, 67, 80), s_Map) == parent.Region, "parent wins at the rental's exclusive top");
        RUOVersion.UpdateRegion(parent.ForSaleSign);
        Check(Region.Find(new Point3D(243, 67, 65), s_Map) == rental.Region, "parent rebuild does not steal rental precedence");
        Move(landlord, new Point3D(243, 67, 65));
        Speak(landlord, 0x23, "I wish to lock this down");
        Check(landlord.Target == null, "landlord receives no tenant lockdown privileges");
        TestActions(rental, tenant, new Point3D(243, 67, 65));
        Console.WriteLine("PASS: vertically bounded rental precedence");

        Actor owner = new Actor();
        ClassicHouse classic = new ClassicHouse(owner);
        classic.Location = new Point3D(340, 100, 0);
        classic.Map = s_Map;
        classic.Public = true;
        Check(classic.Region.Priority == HouseRegion.HousePriority, "classic-house priority is unchanged");
        MultiComponentList components = classic.Components;
        Point3D interior = Point3D.Zero;
        for (int x = -1; x <= components.Width; ++x)
        {
            for (int y = -1; y <= components.Height; ++y)
            {
                Point3D p = new Point3D(classic.X + components.Min.X + x, classic.Y + components.Min.Y + y, classic.Z);
                bool inside = classic.IsInside(p, 16);
                Check(classic.Region.Contains(p) == inside, "classic region retains component-derived boundaries");
                if (inside && interior == Point3D.Zero)
                    interior = new Point3D(p.X, p.Y, classic.Z + 7); // SmallOldHouse floor elevation.
            }
        }
        Check(interior != Point3D.Zero, "classic fixture has an interior");
        TestActions(classic, owner, interior);
    }

    private static void TestTrashDoorRestriction()
    {
        Actor owner = new Actor();
        TownHouse house = CreateHouse(owner, new TownHouseSign(), 160, 0, 30);
        Point3D point = new Point3D(164, 68, 0);
        BaseDoor door = new StrongWoodDoor(DoorFacing.WestCW);
        door.MoveToWorld(point, s_Map);
        house.Doors.Add(door);
        Move(owner, point);
        Press(owner, new InteriorDecorator(), 12);
        Check(Trash(house) == null && owner.Target == null, "Trash is refused near a door without creating a cursor");
        Move(owner, new Point3D(169, 73, 0));
        Speak(owner, 0x28, "trash barrel");
        Check(Trash(house) != null && Trash(house).Location == owner.Location, "spoken Trash places a barrel away from doors");
        Console.WriteLine("PASS: ground-level townhouse and trash placement restrictions");
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.Expansion = Expansion.SA;
            MultiComponentList.PostHSFormat = true;
            Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            Item.LockedDownFlag = 1;
            Item.SecureFlag = 2;
            // World.Load normally creates these registries; initialize empty ones instead.
            typeof(World).GetField("m_Mobiles", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, new Dictionary<Serial, Item>());
            Map.Maps[0x7F] = new Map(0x7F, 0x7F, 0x7F, 16, 16, 0, "Internal", MapRules.Internal);
            s_Map = new Map(0, 0, 0, 512, 512, 0, "TownHouseFixtures", MapRules.LodorRules);
            Map.Maps[0] = s_Map;
            TestBoundaries();
            Actor skyOwner = new Actor();
            TownHouse sky = CreateHouse(skyOwner, new TownHouseSign(), 112, 50, 90);
            TestActions(sky, skyOwner, new Point3D(116, 68, 55));
            Actor groundOwner = new Actor();
            TownHouse ground = CreateHouse(groundOwner, new TownHouseSign(), 192, 0, 40);
            TestActions(ground, groundOwner, new Point3D(196, 68, 0));
            TestTrashDoorRestriction();
            TestRentalsAndClassicHouse();
            Console.WriteLine("PASS: " + s_Checks + " assertions; no saves loaded or listeners started.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
