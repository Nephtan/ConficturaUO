using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class SweetDough : Item
    {
        public override int LabelNumber
        {
            get { return 1041340; }
        } // sweet dough
        private int m_MinSkill;
        private int m_MaxSkill;
        private Food m_CookedFood;

        //	private Food m_BurnedFood;

        public int MinSkill
        {
            get { return m_MinSkill; }
        }
        public int MaxSkill
        {
            get { return m_MaxSkill; }
        }
        public Food CookedFood
        {
            get { return m_CookedFood; }
        }

        //	public Food BurnedFood{ get{ return m_BurnedFood; } }


        [Constructable]
        public SweetDough()
            : base(0x103d)
        {
            Weight = 1.0;
            Hue = 51;
            Weight = 1.0;
            m_MinSkill = 5;
            m_MaxSkill = 20;
            m_CookedFood = new Muffins(3);
            //		m_BurnedFood = new BurnedMuffins( 3 );
        }

        public SweetDough(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SweetDough m_Item;

            public InternalTarget(SweetDough item)
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

                if (targeted is BowlFlour)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a cake mix");
                    if (m_Item.Parent == null)
                        new CakeMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new CakeMix());
                    m_Item.Consume();
                    ((BowlFlour)targeted).Use(from);
                }
                /*else if ( targeted is SackFlourOpen )
                {
                    if(!((Item)targeted).Movable) return;
                    from.SendMessage("You made a cake mix");
                    if( m_Item.Parent == null )
                        new CakeMix().MoveToWorld( m_Item.Location, m_Item.Map );
                    else
                        from.AddToBackpack( new CakeMix() );
                    m_Item.Consume();
                    ((SackFlourOpen)targeted).Use( from );
                }*/
                else if (targeted is JarHoney)
                {
                    if (!((Item)targeted).Movable)
                        return;
                    from.SendMessage("You made a cookie mix");
                    if (m_Item.Parent == null)
                        new CookieMix().MoveToWorld(m_Item.Location, m_Item.Map);
                    else
                        from.AddToBackpack(new CookieMix());
                    m_Item.Consume();
                    ((JarHoney)targeted).Consume();
                }
                else if (IsHeatSource(targeted))
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
                            m_Item.CookedFood /*, m_Item.BurnedFood*/
                        );
                        t.Start();
                    }
                    else
                    {
                        from.SendLocalizedMessage(500119); // You must wait to perform another action
                    }
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

                //		private Food m_BurnedFood;

                public InternalTimer(
                    Mobile from,
                    IPoint3D p,
                    Map map,
                    int min,
                    int max,
                    Food cookedFood /*, Food burnedFood*/
                )
                    : base(TimeSpan.FromSeconds(1.0))
                {
                    m_From = from;
                    m_Point = p;
                    m_Map = map;
                    Min = min;
                    Max = max;
                    m_CookedFood = cookedFood;
                    //	m_BurnedFood = burnedFood;
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
