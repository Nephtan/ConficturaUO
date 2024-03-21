using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class CakeMix : Item
    {
        public override int LabelNumber
        {
            get { return 1041002; }
        } // cake mix

        private string m_Desc;

        private int m_MinSkill;
        private int m_MaxSkill;

        public string Desc
        {
            get { return m_Desc; }
            set { m_Desc = value; }
        }

        public Food CookedFood
        {
            get { return new Cake(this.Hue); }
        }

        //	public Food BurnedFood{ get{ return new BurnedCake(); } }

        public int MinSkill
        {
            get { return m_MinSkill; }
        }
        public int MaxSkill
        {
            get { return m_MaxSkill; }
        }

        [Constructable]
        public CakeMix()
            : this("", 0) { }

        [Constructable]
        public CakeMix(string desc)
            : this(desc, 0) { }

        [Constructable]
        public CakeMix(int Color)
            : this("", Color) { }

        [Constructable]
        public CakeMix(string desc, int Color)
            : base(0x103F)
        {
            Weight = 1.0;
            if (Color != 0)
                Hue = Color;

            if (desc != "" && desc != null)
            {
                Desc = desc;
                Name = Desc + " cake mix";
            }

            m_MinSkill = 0;
            m_MaxSkill = 100;

            //	// m_CookedFood = new Cake( Desc, Hue );
            //	m_CookedFood = new Cake( this.Hue );

            // m_BurnedFood = new BurnedCake();
        }

        public CakeMix(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            writer.Write((int)m_MinSkill);
            writer.Write((int)m_MaxSkill);

            writer.Write((string)m_Desc);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                {
                    m_MinSkill = reader.ReadInt();
                    m_MaxSkill = reader.ReadInt();
                    goto case 1;
                }
                case 1:
                {
                    m_Desc = reader.ReadString();
                    break;
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            from.Target = new InternalTarget(this);
        }

        public class InternalTarget : Target
        {
            private CakeMix m_Item;

            public InternalTarget(CakeMix item)
                : base(1, false, TargetFlags.None)
            {
                m_Item = item;
            }

            public static bool IsHeatSource(object targeted)
            {
                int itemID;

                if (targeted is Item)
                    itemID = ((Item)targeted).ItemID & 0x3FFF;
                else if (targeted is StaticTarget)
                    itemID = ((StaticTarget)targeted).ItemID & 0x3FFF;
                else
                    return false;

                if (itemID >= 0xDE3 && itemID <= 0xDE9)
                    return true; // Campfire
                else if (itemID >= 0x461 && itemID <= 0x48E)
                    return true; // Sandstone oven/fireplace
                else if (itemID >= 0x92B && itemID <= 0x96C)
                    return true; // Stone oven/fireplace
                else if (itemID == 0xFAC)
                    return true; // Firepit
                else if (itemID >= 0x398C && itemID <= 0x399F)
                    return true; // Fire field
                else if (itemID == 0xFB1)
                    return true; // Forge
                else if (itemID >= 0x197A && itemID <= 0x19A9)
                    return true; // Large Forge
                else if (itemID >= 0x184A && itemID <= 0x184C)
                    return true;
                else if (itemID >= 0x184E && itemID <= 0x1850)
                    return true;

                return false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Item.Deleted)
                    return;

                if (IsHeatSource(targeted))
                {
                    if (from.BeginAction(typeof(Item)))
                    {
                        from.PlaySound(0x225);

                        m_Item.Consume();

                        InternalTimer t = new InternalTimer(
                            from,
                            targeted as IPoint3D,
                            from.Map,
                            m_Item.MinSkill,
                            m_Item.MaxSkill,
                            m_Item.CookedFood
                        );
                        t.Start();
                    }
                    else
                    {
                        from.SendLocalizedMessage(500119); // You must wait to perform another action
                    }
                }
                else if (targeted is Banana || targeted is Bananas)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a banana cake mix");
                    if (m_Item.Parent == null)
                        new BananaCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new BananaCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Cantaloupe)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a cantaloupe cake mix");
                    if (m_Item.Parent == null)
                        new CantaloupeCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new CantaloupeCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Carrot)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a carrot cake mix");
                    if (m_Item.Parent == null)
                        new CarrotCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new CarrotCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Coconut || targeted is SplitCoconut)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a coconut cake mix");
                    if (m_Item.Parent == null)
                        new CoconutCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new CoconutCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Grapes)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a grape cake mix");
                    if (m_Item.Parent == null)
                        new GrapeCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new GrapeCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is HoneydewMelon)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a honeydew melon cake mix");
                    if (m_Item.Parent == null)
                        new HoneydewMelonCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new HoneydewMelonCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Lemon)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a lemon cake mix");
                    if (m_Item.Parent == null)
                        new LemonCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new LemonCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Lime)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a key lime cake mix");
                    if (m_Item.Parent == null)
                        new KeyLimeCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new KeyLimeCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Peach)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a peach cake mix");
                    if (m_Item.Parent == null)
                        new PeachCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new PeachCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Pumpkin)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a pumpkin cake mix");
                    if (m_Item.Parent == null)
                        new PumpkinCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new PumpkinCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
                else if (targeted is Watermelon)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a watermelon cake mix");
                    if (m_Item.Parent == null)
                        new WatermelonCakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new WatermelonCakeMix());
                    m_Item.Consume();
                    ((Item)targeted).Consume();
                }
            }

            private class InternalTimer : Timer
            {
                private Mobile m_From;
                private IPoint3D m_Point;
                private Map m_Map;
                private int Min;
                private int Max;
                private Food m_CookedFood;

                //	private Food m_BurnedFood;

                public InternalTimer(
                    Mobile from,
                    IPoint3D p,
                    Map map,
                    int min,
                    int max,
                    Food cookedFood
                )
                    : base(TimeSpan.FromSeconds(1.0))
                {
                    m_From = from;
                    m_Point = p;
                    m_Map = map;
                    Min = min;
                    Max = max;
                    m_CookedFood = cookedFood;
                    //		m_BurnedFood = burnedFood;
                }

                protected override void OnTick()
                {
                    m_From.EndAction(typeof(Item));

                    if (
                        m_From.Map != m_Map
                        || (m_Point != null && m_From.GetDistanceToSqrt(m_Point) > 3)
                    )
                    {
                        m_From.SendLocalizedMessage(500686); // You burn the food to a crisp! It's ruined.
                        return;
                    }

                    if (m_From.CheckSkill(SkillName.Cooking, Min, Max))
                    {
                        if (m_From.AddToBackpack(m_CookedFood))
                            m_From.PlaySound(0x57);
                    }
                    else
                    {
                        //		if ( m_From.AddToBackpack( m_BurnedFood ) )
                        m_From.PlaySound(0x57);

                        m_From.SendLocalizedMessage(500686); // You burn the food to a crisp! It's ruined.
                    }
                }
            }
        }
    }
}
