// by Alari
// modified watertrough script
using System;
using Server;

namespace Server.Items
{
    public class MushroomAddon : AddonComponent
    {
        private DateTime lastpicked = DateTime.Now;
        private int fullGraphic;
        private int pickedGraphic;
        private int m_yield;

        public Timer regrowTimer;

        private DateTime m_lastvisit;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastSowerVisit
        {
            get { return m_lastvisit; }
            set { m_lastvisit = value; }
        }

        public virtual TimeSpan SowerPickTime
        {
            get { return TimeSpan.FromDays(14); }
        }

        [CommandProperty(AccessLevel.GameMaster)] // debuging
        public bool Growing
        {
            get { return regrowTimer.Running; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Yield
        {
            get { return m_yield; }
            set { m_yield = value; }
        }

        public int FullGraphic
        {
            get { return fullGraphic; }
            set { fullGraphic = value; }
        }

        public int PickGraphic
        {
            get { return pickedGraphic; }
            set { pickedGraphic = value; }
        }

        public DateTime LastPick
        {
            get { return lastpicked; }
            set { lastpicked = value; }
        }

        public MushroomAddon()
            : base(Utility.RandomMinMax(3340, 3348))
        {
            Yield = 10;
        }

        public MushroomAddon(int itemid)
            : base(itemid)
        {
            Yield = 10;
        }

        public static void init(MushroomAddon plant, bool full)
        {
            plant.PickGraphic = (0xD18);
            plant.FullGraphic = Utility.RandomMinMax(3340, 3348);

            plant.LastPick = DateTime.Now;
            plant.regrowTimer = new CropTimer(plant);

            if (full)
            {
                plant.Yield = 1;
                ((Item)plant).ItemID = plant.FullGraphic;
            }
            else
            {
                plant.Yield = 0;
                ((Item)plant).ItemID = plant.PickGraphic;
                plant.regrowTimer.Start();
            }
        }

        public void UpRoot(Mobile from)
        {
            from.SendMessage(
                "The mushrooms wither. You must add some more mushroom seeds to the garden."
            );

            this.Hue = 50;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Mounted)
            {
                from.SendMessage("You cannot reach. It may be best to dismount.");
                return;
            }

            if (from.InRange(this.GetWorldLocation(), 2))
            {
                if (m_yield < 1)
                {
                    from.SendMessage("There is nothing here to harvest.");
                }
                else if (from.CheckTargetSkill(SkillName.Tasting, this, 0, 25))
                {
                    if (from.Skills[SkillName.Tasting].Base < 50)
                    {
                        from.AddToBackpack(new Mushrooms(this.ItemID));
                        from.SendMessage("You find some edible mushrooms!");
                    }
                    else if (from.Skills[SkillName.Tasting].Base >= 50)
                    {
                        //	switch (Utility.Random(3))
                        //	{
                        //	 	case 0:
                        from.AddToBackpack(new Mushrooms(this.ItemID));
                        from.SendMessage("You find some edible mushrooms!");
                        //		break;
                        //	 	case 1:
                        //		from.AddToBackpack( new DestroyingAngel( Utility.Random(4) ) );
                        //		from.SendMessage( "You find some special mushrooms!" );
                        //		break;
                        //		case 2:
                        //		from.AddToBackpack( new ExecutionersCap( Utility.Random(4) ) );
                        //		from.SendMessage( "You find some special mushrooms!" );
                        //		break;
                        //	}
                    }
                    m_yield -= 1;
                }
                else
                {
                    from.SendMessage("You fail to find any edible mushrooms.");
                }
            }
        }

        private class CropTimer : Timer
        {
            private MushroomAddon i_plant;

            public CropTimer(MushroomAddon plant)
                : base(TimeSpan.FromSeconds(600), TimeSpan.FromSeconds(15))
            {
                Priority = TimerPriority.OneSecond;
                i_plant = plant;
            }

            protected override void OnTick()
            {
                if ((i_plant != null) && (!i_plant.Deleted))
                {
                    int current = i_plant.Yield;

                    if (++current >= 10)
                    {
                        current = 10;
                        ((Item)i_plant).ItemID = i_plant.FullGraphic;
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

        public MushroomAddon(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
            writer.Write(m_lastvisit);
            writer.Write(m_yield);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    m_lastvisit = reader.ReadDateTime();
                    m_yield = reader.ReadInt();
                    break;
                }
                case 0:
                {
                    break;
                }
            }

            if (version == 0)
            {
                m_lastvisit = DateTime.Now;
                m_yield = 10;
            }

            init(this, true);
        }
    }

    public class MushroomGardenSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get { return new MushroomGardenSouthDeed(); }
        }

        [Constructable]
        public MushroomGardenSouthAddon()
        {
            AddonComponent one = new AddonComponent(2818);
            AddonComponent two = new AddonComponent(2816);

            one.Name = "mushroom garden";
            one.Hue = 637;

            two.Name = one.Name;
            two.Hue = one.Hue;

            AddComponent(one, 0, 0, 0);
            AddComponent(two, 1, 0, 0);

            AddComponent(new MushroomAddon(Utility.RandomMinMax(3340, 3348)), 0, 0, 4);
            AddComponent(new MushroomAddon(Utility.RandomMinMax(3340, 3348)), 1, 0, 4);
            AddComponent(new MushroomAddon(Utility.RandomMinMax(3340, 3348)), 0, 0, 4);
            AddComponent(new MushroomAddon(Utility.RandomMinMax(3340, 3348)), 1, 0, 4);
        }

        public MushroomGardenSouthAddon(Serial serial)
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

    public class MushroomGardenSouthDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get { return new MushroomGardenSouthAddon(); }
        }

        // public override int LabelNumber{ get{ return 1044350; } } // water trough (south)

        [Constructable]
        public MushroomGardenSouthDeed()
        {
            Name = "mushroom garden (south)";
        }

        public MushroomGardenSouthDeed(Serial serial)
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

    public class MushroomGardenEastAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get { return new MushroomGardenEastDeed(); }
        }

        [Constructable]
        public MushroomGardenEastAddon()
        {
            AddonComponent one = new AddonComponent(2824);
            AddonComponent two = new AddonComponent(2822);

            one.Name = "mushroom garden";
            one.Hue = 637;

            two.Name = one.Name;
            two.Hue = one.Hue;

            AddComponent(one, 0, 0, 0);
            AddComponent(two, 0, 1, 0);

            AddComponent(new MushroomAddon(Utility.RandomMinMax(3340, 3348)), 0, 0, 4);
            AddComponent(new MushroomAddon(Utility.RandomMinMax(3340, 3348)), 0, 1, 4);
            AddComponent(new MushroomAddon(Utility.RandomMinMax(3340, 3348)), 0, 0, 4);
            AddComponent(new MushroomAddon(Utility.RandomMinMax(3340, 3348)), 0, 1, 4);
        }

        public MushroomGardenEastAddon(Serial serial)
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

    public class MushroomGardenEastDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get { return new MushroomGardenEastAddon(); }
        }

        // public override int LabelNumber{ get{ return 1044349; } } // water trough (east)

        [Constructable]
        public MushroomGardenEastDeed()
        {
            Name = "mushroom garden (east)";
        }

        public MushroomGardenEastDeed(Serial serial)
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
}
