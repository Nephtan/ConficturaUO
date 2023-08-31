using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Items.Crops
{
    public class AvocadoTreeSeed : BaseCrop
    {
        // return true to allow planting on Dirt Item (ItemID 0x32C9)
        // See CropHelper.cs for other overriddable types
        public override bool CanGrowGarden
        {
            get { return true; }
        }

        [Constructable]
        public AvocadoTreeSeed()
            : this(1) { }

        [Constructable]
        public AvocadoTreeSeed(int amount)
            : base(0xF27)
        {
            Stackable = true;
            Weight = .5;
            Hue = 0x5E2;

            Movable = true;

            Amount = amount;
            Name = "Avocado Tree Seed";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Mounted && !CropHelper.CanWorkMounted)
            {
                from.SendMessage("You cannot plant a seed while mounted.");
                return;
            }

            Point3D m_pnt = from.Location;
            Map m_map = from.Map;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042010); //You must have the object in your backpack to use it.
                return;
            }
            else if (!CropHelper.CheckCanGrow(this, m_map, m_pnt.X, m_pnt.Y))
            {
                from.SendMessage("This seed will not grow here.");
                return;
            }

            //check for BaseCrop on this tile
            ArrayList cropshere = CropHelper.CheckCrop(m_pnt, m_map, 0);
            if (cropshere.Count > 0)
            {
                from.SendMessage("There is already a crop growing here.");
                return;
            }

            //check for over planting prohibt if 6 maybe 5 neighboring crops
            ArrayList cropsnear = CropHelper.CheckCrop(m_pnt, m_map, 1);
            if ((cropsnear.Count > 5) || ((cropsnear.Count == 5) && Utility.RandomBool()))
            {
                from.SendMessage("There are too many crops nearby.");
                return;
            }

            if (this.BumpZ)
                ++m_pnt.Z;

            if (!from.Mounted)
                from.Animate(32, 5, 1, true, false, 0); // Bow

            from.SendMessage("You plant the seed.");
            this.Consume();
            Item item = new AvocadoSapling();
            item.Location = m_pnt;
            item.Map = m_map;
        }

        public AvocadoTreeSeed(Serial serial)
            : base(serial) { }

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

    public class AvocadoSapling : BaseCrop
    {
        public Timer thisTimer;
        public DateTime treeTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public String FullGrown
        {
            get { return treeTime.ToString("T"); }
        }

        [Constructable]
        public AvocadoSapling()
            : base(Utility.RandomList(0xCE9, 0xCEA))
        {
            Movable = false;
            Name = "Avocado tree sapling";

            init(this);
        }

        public static void init(AvocadoSapling plant)
        {
            TimeSpan delay = TreeHelper.SaplingTime;
            plant.treeTime = DateTime.Now + delay;

            plant.thisTimer = new TreeHelper.TreeTimer(plant, typeof(AvocadoTree), delay);
            plant.thisTimer.Start();
        }

        public AvocadoSapling(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            init(this);
        }
    }

    public class AvocadoTree : BaseTree
    {
        public Item i_trunk;
        private Timer chopTimer;

        private const int max = 15;
        private DateTime lastpicked;
        private int m_yield;

        public Timer regrowTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Yield
        {
            get { return m_yield; }
            set { m_yield = value; }
        }

        public int Capacity
        {
            get { return max; }
        }
        public DateTime LastPick
        {
            get { return lastpicked; }
            set { lastpicked = value; }
        }

        [Constructable]
        public AvocadoTree(Point3D pnt, Map map)
            : base(0xDA9) // leaves
        {
            Movable = false;
            MoveToWorld(pnt, map);
            Name = "Avocado Tree";

            int trunkID = 0xDA8;

            i_trunk = new TreeTrunk(trunkID, this);
            i_trunk.MoveToWorld(pnt, map);

            init(this, false);
        }

        public static void init(AvocadoTree plant, bool full)
        {
            plant.LastPick = DateTime.Now;
            plant.regrowTimer = new FruitTimer(plant);

            if (full)
            {
                plant.Yield = plant.Capacity;
            }
            else
            {
                plant.Yield = 0;
                plant.regrowTimer.Start();
            }
        }

        public override void OnAfterDelete()
        {
            if ((i_trunk != null) && (!i_trunk.Deleted))
                i_trunk.Delete();

            base.OnAfterDelete();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Mounted && !TreeHelper.CanPickMounted)
            {
                from.SendMessage("You cannot pick fruit while mounted.");
                return;
            }

            if (DateTime.Now > lastpicked.AddSeconds(3)) // 3 seconds between picking
            {
                lastpicked = DateTime.Now;

                int lumberValue = (int)from.Skills[SkillName.Lumberjacking].Value / 20;
                if (from.Mounted)
                    ++lumberValue;

                if (lumberValue < 0) //Changed lumberValue == 0 to lv < 0
                {
                    from.SendMessage("You have no idea how to pick this fruit.");
                    return;
                }

                if (from.InRange(this.GetWorldLocation(), 2))
                {
                    if (m_yield < 1)
                    {
                        from.SendMessage("There is nothing here to harvest.");
                    }
                    else //check skill
                    {
                        from.Direction = from.GetDirectionTo(this);

                        from.Animate(from.Mounted ? 26 : 17, 7, 1, true, false, 0);

                        if (lumberValue < m_yield) //Changed lumberValue > to lv < 0
                            lumberValue = m_yield + 1;

                        int pick = Utility.Random(lumberValue);
                        if (pick == 0)
                        {
                            from.SendMessage("You do not manage to gather any fruit.");
                            return;
                        }

                        m_yield -= pick;
                        from.SendMessage("You pick {0} Avocado{1}!", pick, (pick == 1 ? "" : "s"));

                        //PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "{0}", m_yield ));

                        Avocado crop = new Avocado(pick);
                        from.AddToBackpack(crop);

                        if (!regrowTimer.Running)
                        {
                            regrowTimer.Start();
                        }
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500446); // That is too far away.
                }
            }
        }

        private class FruitTimer : Timer
        {
            private AvocadoTree i_plant;

            public FruitTimer(AvocadoTree plant)
                : base(TimeSpan.FromSeconds(900), TimeSpan.FromSeconds(30))
            {
                Priority = TimerPriority.OneSecond;
                i_plant = plant;
            }

            protected override void OnTick()
            {
                if ((i_plant != null) && (!i_plant.Deleted))
                {
                    int current = i_plant.Yield;

                    if (++current >= i_plant.Capacity)
                    {
                        current = i_plant.Capacity;
                        Stop();
                    }
                    else if (current <= 0)
                        current = 1;

                    i_plant.Yield = current;

                    //i_plant.PublicOverheadMessage( MessageType.Regular, 0x22, false, string.Format( "{0}", current ));
                }
                else
                    Stop();
            }
        }

        public void Chop(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 1))
            {
                if ((chopTimer == null) || (!chopTimer.Running))
                {
                    if ((TreeHelper.TreeOrdinance) && (from.AccessLevel == AccessLevel.Player))
                    {
                        if (from.Region is Regions.GuardedRegion)
                            from.CriminalAction(true);
                    }

                    chopTimer = new TreeHelper.ChopAction(from);

                    Point3D pnt = this.Location;
                    Map map = this.Map;

                    from.Direction = from.GetDirectionTo(this);
                    chopTimer.Start();

                    double lumberValue = from.Skills[SkillName.Lumberjacking].Value / 100;
                    if ((lumberValue > .5) && (Utility.RandomDouble() <= lumberValue))
                    {
                        Avocado fruit = new Avocado((int)Utility.Random(13) + m_yield);
                        from.AddToBackpack(fruit);

                        int cnt = Utility.Random((int)(lumberValue * 10) + 1);
                        Log logs = new Log(cnt); // Fruitwood Logs ??
                        from.AddToBackpack(logs);

                        FruitTreeStump i_stump = new FruitTreeStump(typeof(AvocadoTree));
                        Timer poof = new StumpTimer(this, i_stump, from);
                        poof.Start();
                    }
                    else
                        from.SendLocalizedMessage(500495); // You hack at the tree for a while, but fail to produce any useable wood.
                    //PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "{0}", lumberValue ));
                }
            }
            else
                from.SendLocalizedMessage(500446); // That is too far away.
        }

        private class StumpTimer : Timer
        {
            private AvocadoTree i_tree;
            private FruitTreeStump i_stump;
            private Mobile m_chopper;

            public StumpTimer(AvocadoTree Tree, FruitTreeStump Stump, Mobile from)
                : base(TimeSpan.FromMilliseconds(5500))
            {
                Priority = TimerPriority.TenMS;

                i_tree = Tree;
                i_stump = Stump;
                m_chopper = from;
            }

            protected override void OnTick()
            {
                i_stump.MoveToWorld(i_tree.Location, i_tree.Map);
                i_tree.Delete();
                m_chopper.SendMessage("You put some logs and fruit into your backpack");
            }
        }

        public override void OnChop(Mobile from)
        {
            if (!this.Deleted)
                Chop(from);
        }

        public AvocadoTree(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write((Item)i_trunk);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            Item item = reader.ReadItem();
            if (item != null)
                i_trunk = item;

            init(this, true);
        }
    }
}
