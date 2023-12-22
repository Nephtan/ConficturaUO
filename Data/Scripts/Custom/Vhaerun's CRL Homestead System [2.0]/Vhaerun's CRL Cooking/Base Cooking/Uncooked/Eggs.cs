using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    /*	public class Eggs : Eggs
        {
            [Constructable]
            public Eggs() : base( 0x9B5, 15 )
            {
                Weight = 0.5;
            }
    
            public Eggs( Serial serial ) : base( serial )
            {
            }
    
            public override void Serialize( GenericWriter writer )
            {
                base.Serialize( writer );
    
                writer.Write( (int) 0 ); // version
            }
    
            public override void Deserialize( GenericReader reader )
            {
                base.Deserialize( reader );
    
                int version = reader.ReadInt();
            }
    
            public override Food Cook()
            {
                return new FriedEggs();
            }
    
        }
    */
    public class Eggs : Item
    {
        private int m_MinSkill;
        private int m_MaxSkill;

        public string Desc;

        public Food CookedFood
        {
            get { return new FriedEggs(); }
        }

        //	public Food BurnedFood{ get{ return new BurnedFriedEggs(); } }

        public int MinSkill
        {
            get { return m_MinSkill; }
        }
        public int MaxSkill
        {
            get { return m_MaxSkill; }
        }

        [Constructable]
        public Eggs()
            : base(0x9B5)
        {
            Weight = 1.0;

            m_MinSkill = 0;
            m_MaxSkill = 15;
        }

        public Eggs(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_MinSkill);
            writer.Write((int)m_MaxSkill);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    m_MinSkill = reader.ReadInt();
                    m_MaxSkill = reader.ReadInt();
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
            private Eggs m_Item;

            public InternalTarget(Eggs item)
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

                        from.AddToBackpack(new Eggshells(m_Item.Hue)); // !

                        InternalTimer t = new InternalTimer(
                            from,
                            targeted as IPoint3D,
                            from.Map,
                            m_Item.MinSkill,
                            m_Item.MaxSkill,
                            m_Item.CookedFood
                        ); // , m_Item.BurnedFood );
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
                        //			if ( m_From.AddToBackpack( m_BurnedFood ) )
                        m_From.PlaySound(0x57);

                        m_From.SendLocalizedMessage(500686); // You burn the food to a crisp! It's ruined.
                    }
                }
            }
        }
    }
}
