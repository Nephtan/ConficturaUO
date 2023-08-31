using System;
using Server;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public abstract class ContainerFood : Food
    {
        private Mobile m_Poisoner;
        private Poison m_Poison;
        private int m_FillFactor;

        private int m_Uses;

        public bool ShowUsesRemaining
        {
            get { return true; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Uses
        {
            get { return m_Uses; }
            set
            {
                m_Uses = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_Uses; }
            set
            {
                m_Uses = value;
                InvalidateProperties();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060584, m_Uses.ToString()); // uses remaining: ~1_val~
        }

        public virtual void DisplayDurabilityTo(Mobile m) // ???
        {
            LabelToAffix(m, 1017323, AffixType.Append, ": " + m_Uses.ToString()); // Durability
        }

        public override void OnSingleClick(Mobile from)
        {
            DisplayDurabilityTo(from);

            base.OnSingleClick(from);
        }

        public virtual int MinSkill
        {
            get { return 0; }
        }
        public virtual int MaxSkill
        {
            get { return 100; }
        }
        public virtual bool NeedSilverware
        {
            get { return false; }
        }
        public virtual string CookedMessage
        {
            get { return ""; }
        }
        public virtual Item FoodContainer
        {
            get { return new DirtyPlate(); }
        }

        public ContainerFood(int itemID)
            : base(itemID)
        {
            m_Uses = 1;
            Stackable = false;
            Weight = 1.0;
        }

        public ContainerFood(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_Uses);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    m_Uses = reader.ReadInt();
                    break;
                }
            }
        }

        /*
                public override bool Eat( Mobile from )
                {
                    if( CheckEatRequirement( from ) )
                        if( FillHunger( from, m_FillFactor, HitsBonus, ManaBonus ) )
                        {
                            Use( from );
        
                            if ( m_Poison != null )
                                from.ApplyPoison( m_Poisoner, m_Poison );
        
                            return true;
                        }
                    return false;
                }
        */

        // modified by alari for new food system
        public override bool Eat(Mobile from)
        {
            if (CheckEatRequirement(from))
            {
                // Fill the Mobile with FillFactor
                if (FillHunger(from, FillFactor, HitsBonus, ManaBonus)) // added HitsBonus, ManaBonus - alari
                {
                    // Play a random "eat" sound
                    from.PlaySound(Utility.Random(0x3A, 3));

                    if (from.Body.IsHuman && !from.Mounted)
                        from.Animate(34, 5, 1, true, false, 0);

                    if (m_Poison != null)
                        from.ApplyPoison(m_Poisoner, m_Poison);

                    Use(from);

                    return true;
                }
            }

            return false;
        }

        public void Use(Mobile from)
        {
            /*
            
            
            */

            Uses--;

            if (Uses <= 0)
            {
                // FoodContainer.MoveToWorld( new Point3D( this.X, this.Y, this.Z ), this.Map );
                from.AddToBackpack(FoodContainer);
                Consume();
            }

            // tracking item leakage
            //			if ( FoodContainer is DirtyPlate ) // && NeedSilverware )
            //			{
            //				if ( Uses == 2 )
            //					ItemID = 0x9D9;
            //				if ( Uses == 1 )
            //					ItemID = 0x9DA;
            //			}

            /*
                    if ( fc != null )
                        fc.Consume();
            
            
            */
        }

        public bool CheckEatRequirement(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("That must be in your backpack.");
                return false;
            }
            if (from.Mounted)
            {
                from.SendLocalizedMessage(1040016); // You cannot use this while mounted.
                return false;
            }
            if (NeedSilverware)
                if (!HaveSilverware(from))
                    return false;
            return true;
        }

        public bool HaveSilverware(Mobile from)
        {
            IPooledEnumerable inRange = from.Map.GetItemsInRange(from.Location, 1);

            foreach (Item item in inRange)
            {
                if (item is Silverware || item is Fork || item is Spoon || item is Knife)
                {
                    inRange.Free();
                    return true;
                }
            }
            inRange.Free();
            return false;
        }
    }
}
