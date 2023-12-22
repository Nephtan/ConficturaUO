using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Items.Crops
{
    public enum TreeType
    {
        AlmondTree,
        AppleTree,
        ApricotTree,
        AvocadoTree,
        BananaTree,
        BlackberryTree,
        BlackRaspberryTree,
        BlueberryTree,
        CherryTree,
        CocoaTree,
        CoconutTree,
        CranberryTree,
        DateTree,
        GrapefruitTree,
        LemonTree,
        LimeTree,
        MangoTree,
        OrangeTree,
        PeachTree,
        PearTree,
        PineappleTree,
        PistacioTree,
        PlumTree,
        PomegranateTree,
        RedRaspberryTree
    }

    public class TreeHelper
    {
        public static bool CanPickMounted
        {
            get { return true; }
        } // If true Player can pick fruit while mounted
        public static bool TreeOrdinance
        {
            get { return false; }
        } // Criminal to Chop fruit trees in town.

        public static TimeSpan SaplingTime = TimeSpan.FromMinutes(5); // Time spent as a Sapling
        public static TimeSpan StumpTime = TimeSpan.FromMinutes(30); // Time spent as a Stump --changed from-- TimeSpan.FromHours (1)

        public class ChopAction : Timer
        {
            private Mobile m_chopper;
            private int cnt;

            public ChopAction(Mobile from)
                : base(TimeSpan.FromMilliseconds(900), TimeSpan.FromMilliseconds(900))
            {
                Priority = TimerPriority.TenMS;
                m_chopper = from;
                from.CantWalk = true;
                cnt = 1;
            }

            protected override void OnTick()
            {
                switch (cnt++)
                {
                    case 1:
                    case 3:
                    case 5:
                    {
                        m_chopper.Animate(13, 7, 1, true, false, 0); // Chop
                        break;
                    }
                    case 2:
                    case 4:
                    {
                        Effects.PlaySound(m_chopper.Location, m_chopper.Map, 0x13E);
                        break;
                    }
                    case 6:
                    {
                        Effects.PlaySound(m_chopper.Location, m_chopper.Map, 0x13E);
                        m_chopper.CantWalk = false;
                        this.Stop();
                        break;
                    }
                }
            }
        }

        public class TreeTimer : Timer
        {
            private Item i_sapling;
            private Type t_crop;

            public TreeTimer(Item sapling, Type croptype, TimeSpan delay)
                : base(delay)
            {
                Priority = TimerPriority.OneMinute;

                i_sapling = sapling;
                t_crop = croptype;
            }

            protected override void OnTick()
            {
                if ((i_sapling != null) && (!i_sapling.Deleted))
                {
                    object[] args = { i_sapling.Location, i_sapling.Map };
                    Item newitem = Activator.CreateInstance(t_crop, args) as Item;

                    i_sapling.Delete();
                }
            }
        }

        public class GrowTimer : Timer
        {
            private Item i_stump;
            private Type t_tree;
            private DateTime d_timerstart;
            private Item i_newtree;

            public GrowTimer(Item stump, Type treetype, TimeSpan delay)
                : base(delay)
            {
                Priority = TimerPriority.OneMinute;

                i_stump = stump;
                t_tree = treetype;

                d_timerstart = DateTime.Now;
            }

            protected override void OnTick()
            {
                Point3D pnt = i_stump.Location;
                Map map = i_stump.Map;

                if (t_tree == typeof(AlmondTree))
                    i_newtree = new AlmondSapling();
                else if (t_tree == typeof(ApricotTree))
                    i_newtree = new ApricotSapling();
                else if (t_tree == typeof(AvocadoTree))
                    i_newtree = new AvocadoSapling();
                else if (t_tree == typeof(BananaTree))
                    i_newtree = new BananaSapling();
                else if (t_tree == typeof(BlackberryTree))
                    i_newtree = new BlackberrySapling();
                else if (t_tree == typeof(BlackRaspberryTree))
                    i_newtree = new BlackRaspberrySapling();
                else if (t_tree == typeof(BlueberryTree))
                    i_newtree = new BlueberrySapling();
                else if (t_tree == typeof(CherryTree))
                    i_newtree = new CherrySapling();
                else if (t_tree == typeof(CocoaTree))
                    i_newtree = new CocoaSapling();
                else if (t_tree == typeof(CoconutTree))
                    i_newtree = new CoconutSapling();
                else if (t_tree == typeof(CranberryTree))
                    i_newtree = new CranberrySapling();
                else if (t_tree == typeof(DateTree))
                    i_newtree = new DateSapling();
                else if (t_tree == typeof(GrapefruitTree))
                    i_newtree = new GrapefruitSapling();
                else if (t_tree == typeof(LemonTree))
                    i_newtree = new LemonSapling();
                else if (t_tree == typeof(LimeTree))
                    i_newtree = new LimeSapling();
                else if (t_tree == typeof(MangoTree))
                    i_newtree = new MangoSapling();
                else if (t_tree == typeof(OrangeTree))
                    i_newtree = new OrangeSapling();
                else if (t_tree == typeof(PeachTree))
                    i_newtree = new PeachSapling();
                else if (t_tree == typeof(PearTree))
                    i_newtree = new PearSapling();
                else if (t_tree == typeof(PineappleTree))
                    i_newtree = new PineappleSapling();
                else if (t_tree == typeof(PistacioTree))
                    i_newtree = new PistacioSapling();
                else if (t_tree == typeof(PlumTree))
                    i_newtree = new PlumSapling();
                else if (t_tree == typeof(PomegranateTree))
                    i_newtree = new PomegranateSapling();
                else if (t_tree == typeof(RedRaspberryTree))
                    i_newtree = new RedRaspberrySapling();
                else
                    i_newtree = new AppleSapling();

                i_stump.Delete();
                i_newtree.MoveToWorld(pnt, map);
            }
        }
    }

    public class BaseTree : Item, IChopable
    {
        public BaseTree(int itemID)
            : base(itemID) { }

        public BaseTree(Serial serial)
            : base(serial) { }

        public virtual void OnChop(Mobile from) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TreeTrunk : Item, IChopable
    {
        private Item i_leaves;

        public Item Leaves
        {
            get { return i_leaves; }
        }

        public TreeTrunk(int itemID, Item TreeLeaves)
            : base(itemID)
        {
            Movable = false;
            i_leaves = TreeLeaves;
        }

        public TreeTrunk(Serial serial)
            : base(serial) { }

        public override void OnAfterDelete()
        {
            if ((i_leaves != null) && (!i_leaves.Deleted))
                i_leaves.Delete();

            base.OnAfterDelete();
        }

        public void OnChop(Mobile from)
        {
            int testID = ((Item)i_leaves).ItemID;
            int testHue = ((Item)i_leaves).Hue;

            switch (testID)
            {
                case 0xD96:
                case 0xD9A:
                {
                    AppleTree thistree = i_leaves as AppleTree;
                    if (thistree != null)
                        thistree.Chop(from);
                    break;
                }
                case 0xDAA:
                {
                    if (i_leaves.Hue == 0)
                    {
                        PearTree thistree = i_leaves as PearTree;
                        if (thistree != null)
                            thistree.Chop(from);
                    }
                    if (i_leaves.Hue == 0x21A)
                    {
                        AvocadoTree thistree = i_leaves as AvocadoTree;
                        if (thistree != null)
                            thistree.Chop(from);
                    }
                    break;
                }
                case 0xD9E:
                {
                    PeachTree thistree = i_leaves as PeachTree;
                    if (thistree != null)
                        thistree.Chop(from);
                    break;
                }
                case 0xCE5:
                {
                    OrangeTree thistree = i_leaves as OrangeTree;
                    if (thistree != null)
                        thistree.Chop(from);
                    break;
                }
                case 0xC9E:
                {
                    if (i_leaves.Hue == 0x592)
                    {
                        BlackberryTree thistree = i_leaves as BlackberryTree;
                        if (thistree != null)
                            thistree.Chop(from);
                    }
                    //{
                    //other bushes using same itemid go here
                    //BlackberryTree thistree = i_leaves as BlackberryTree;
                    //if (thistree != null)
                    //    thistree.Chop(from);
                    //}
                    break;
                }
                case 0x2471: //note using orential cherry
                case 0x2472:
                case 0x2473:
                case 0x2474:
                case 0x2475:
                {
                    CherryTree thistree = i_leaves as CherryTree;
                    if (thistree != null)
                        thistree.Chop(from);
                    break;
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write((Item)i_leaves);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            Item item = reader.ReadItem();
            if (item != null)
                i_leaves = item;
        }
    }

    public class FruitTreeStump : Item
    {
        private Type t_treeType;
        private int e_tree;
        public Timer thisTimer;
        public DateTime treeTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public String Sapling
        {
            get { return treeTime.ToString("T"); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public String Type
        {
            get
            {
                switch (e_tree)
                {
                    case (int)TreeType.AlmondTree:
                        return "Almond Tree";
                    case (int)TreeType.AppleTree:
                        return "Apple Tree";
                    case (int)TreeType.ApricotTree:
                        return "Apricot Tree";
                    case (int)TreeType.AvocadoTree:
                        return "Avocado Tree";
                    case (int)TreeType.BananaTree:
                        return "Banana Palm";
                    case (int)TreeType.BlackberryTree:
                        return "Blackberry Bush";
                    case (int)TreeType.BlackRaspberryTree:
                        return "Black Raspberry Bush";
                    case (int)TreeType.BlueberryTree:
                        return "Blueberry Bush";
                    case (int)TreeType.CherryTree:
                        return "Cherry Tree";
                    case (int)TreeType.CocoaTree:
                        return "Cocoa Tree";
                    case (int)TreeType.CoconutTree:
                        return "Coconut Palm";
                    case (int)TreeType.CranberryTree:
                        return "Cranberry Bush";
                    case (int)TreeType.DateTree:
                        return "Date Palm";
                    case (int)TreeType.GrapefruitTree:
                        return "Grapefruit Tree";
                    case (int)TreeType.LemonTree:
                        return "Lemon Tree";
                    case (int)TreeType.LimeTree:
                        return "Lime Tree";
                    case (int)TreeType.MangoTree:
                        return "Mango Tree";
                    case (int)TreeType.OrangeTree:
                        return "Orange Tree";
                    case (int)TreeType.PeachTree:
                        return "Peach Tree";
                    case (int)TreeType.PearTree:
                        return "Pear Tree";
                    case (int)TreeType.PineappleTree:
                        return "Pineapple Palm";
                    case (int)TreeType.PistacioTree:
                        return "Pistacio Tree";
                    case (int)TreeType.PlumTree:
                        return "Plum Tree";
                    case (int)TreeType.PomegranateTree:
                        return "Pomegranate Tree";
                    case (int)TreeType.RedRaspberryTree:
                        return "Red Raspberry Bush";
                    default:
                        return "No Listing in Treehelper.cs";
                }
            }
        }

        [Constructable]
        public FruitTreeStump(Type FruitTree)
            : base(0xDAC)
        {
            Movable = false;
            Hue = 0x74E;
            Name = "fruit tree stump";

            t_treeType = FruitTree;

            if (FruitTree == typeof(AlmondTree))
                e_tree = (int)TreeType.AlmondTree;
            else if (FruitTree == typeof(ApricotTree))
                e_tree = (int)TreeType.ApricotTree;
            else if (FruitTree == typeof(AvocadoTree))
                e_tree = (int)TreeType.AvocadoTree;
            else if (FruitTree == typeof(BananaTree))
                e_tree = (int)TreeType.BananaTree;
            else if (FruitTree == typeof(BlackberryTree))
                e_tree = (int)TreeType.BlackberryTree;
            else if (FruitTree == typeof(BlackRaspberryTree))
                e_tree = (int)TreeType.BlackRaspberryTree;
            else if (FruitTree == typeof(BlueberryTree))
                e_tree = (int)TreeType.BlueberryTree;
            else if (FruitTree == typeof(CherryTree))
                e_tree = (int)TreeType.CherryTree;
            else if (FruitTree == typeof(CocoaTree))
                e_tree = (int)TreeType.CocoaTree;
            else if (FruitTree == typeof(CoconutTree))
                e_tree = (int)TreeType.CoconutTree;
            else if (FruitTree == typeof(CranberryTree))
                e_tree = (int)TreeType.CranberryTree;
            else if (FruitTree == typeof(DateTree))
                e_tree = (int)TreeType.DateTree;
            else if (FruitTree == typeof(GrapefruitTree))
                e_tree = (int)TreeType.GrapefruitTree;
            else if (FruitTree == typeof(LemonTree))
                e_tree = (int)TreeType.LemonTree;
            else if (FruitTree == typeof(LimeTree))
                e_tree = (int)TreeType.LimeTree;
            else if (FruitTree == typeof(MangoTree))
                e_tree = (int)TreeType.MangoTree;
            else if (FruitTree == typeof(OrangeTree))
                e_tree = (int)TreeType.OrangeTree;
            else if (FruitTree == typeof(PeachTree))
                e_tree = (int)TreeType.PeachTree;
            else if (FruitTree == typeof(PearTree))
                e_tree = (int)TreeType.PearTree;
            else if (FruitTree == typeof(PineappleTree))
                e_tree = (int)TreeType.PineappleTree;
            else if (FruitTree == typeof(PistacioTree))
                e_tree = (int)TreeType.PistacioTree;
            else if (FruitTree == typeof(PlumTree))
                e_tree = (int)TreeType.PlumTree;
            else if (FruitTree == typeof(PomegranateTree))
                e_tree = (int)TreeType.PomegranateTree;
            else if (FruitTree == typeof(RedRaspberryTree))
                e_tree = (int)TreeType.RedRaspberryTree;
            else
                e_tree = (int)TreeType.AppleTree;

            init(this);
        }

        public static void init(FruitTreeStump plant)
        {
            TimeSpan delay = TreeHelper.StumpTime;
            plant.treeTime = DateTime.Now + delay;

            plant.thisTimer = new TreeHelper.GrowTimer(plant, plant.t_treeType, delay);
            plant.thisTimer.Start();
        }

        public FruitTreeStump(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write(e_tree);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            int e_tree = reader.ReadInt();
            switch (e_tree)
            {
                case (int)TreeType.AlmondTree:
                    t_treeType = typeof(AlmondTree);
                    break;
                case (int)TreeType.AppleTree:
                    t_treeType = typeof(AppleTree);
                    break;
                case (int)TreeType.ApricotTree:
                    t_treeType = typeof(ApricotTree);
                    break;
                case (int)TreeType.AvocadoTree:
                    t_treeType = typeof(AvocadoTree);
                    break;
                case (int)TreeType.BananaTree:
                    t_treeType = typeof(BananaTree);
                    break;
                case (int)TreeType.BlackberryTree:
                    t_treeType = typeof(BlackberryTree);
                    break;
                case (int)TreeType.BlackRaspberryTree:
                    t_treeType = typeof(BlackRaspberryTree);
                    break;
                case (int)TreeType.BlueberryTree:
                    t_treeType = typeof(BlueberryTree);
                    break;
                case (int)TreeType.CherryTree:
                    t_treeType = typeof(CherryTree);
                    break;
                case (int)TreeType.CocoaTree:
                    t_treeType = typeof(CocoaTree);
                    break;
                case (int)TreeType.CoconutTree:
                    t_treeType = typeof(CoconutTree);
                    break;
                case (int)TreeType.CranberryTree:
                    t_treeType = typeof(CranberryTree);
                    break;
                case (int)TreeType.DateTree:
                    t_treeType = typeof(DateTree);
                    break;
                case (int)TreeType.GrapefruitTree:
                    t_treeType = typeof(GrapefruitTree);
                    break;
                case (int)TreeType.LemonTree:
                    t_treeType = typeof(LemonTree);
                    break;
                case (int)TreeType.LimeTree:
                    t_treeType = typeof(LimeTree);
                    break;
                case (int)TreeType.MangoTree:
                    t_treeType = typeof(MangoTree);
                    break;
                case (int)TreeType.OrangeTree:
                    t_treeType = typeof(OrangeTree);
                    break;
                case (int)TreeType.PeachTree:
                    t_treeType = typeof(PeachTree);
                    break;
                case (int)TreeType.PearTree:
                    t_treeType = typeof(PearTree);
                    break;
                case (int)TreeType.PineappleTree:
                    t_treeType = typeof(PineappleTree);
                    break;
                case (int)TreeType.PistacioTree:
                    t_treeType = typeof(PistacioTree);
                    break;
                case (int)TreeType.PlumTree:
                    t_treeType = typeof(PlumTree);
                    break;
                case (int)TreeType.PomegranateTree:
                    t_treeType = typeof(PomegranateTree);
                    break;
                case (int)TreeType.RedRaspberryTree:
                    t_treeType = typeof(RedRaspberryTree);
                    break;
            }
            init(this);
        }
    }
}
