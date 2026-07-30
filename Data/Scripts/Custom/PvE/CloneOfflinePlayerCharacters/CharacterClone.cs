/*************************************************************
 * File: CharacterClone.cs
 * Description: Defines the mobile that represents an offline
 *              player's clone, including hireling behaviour.
 *************************************************************/
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Custom.Confictura.CloneOfflinePlayerCharacters
{
    /// <summary>
    /// Replica of a logged-out <see cref="PlayerMobile"/>. Handles combat,
    /// equipment replication, and temporary hireling functionality.
    /// </summary>
    public class CharacterClone : BaseCreature
    {
        private int m_Pay = 1;
        private bool m_IsHired;
        private int m_HoldGold = 8;
        private Timer m_PayTimer;
        private static readonly Hashtable m_HireTable = new Hashtable();

        public CharacterClone(Mobile original)
            : base(
                AIType.AI_Melee,
                FightMode.Aggressor,
                18,
                CalculateRangeFight(original),
                0.2,
                0.3
            )
        {
            Original = original;
            Player = false;
        }

        public CharacterClone(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Original { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsHired
        {
            get { return m_IsHired; }
            set { m_IsHired = value; }
        }

        public int Pay
        {
            get { return m_Pay; }
            set { m_Pay = value; }
        }

        public int HoldGold
        {
            get { return m_HoldGold; }
            set { m_HoldGold = value; }
        }

        public static Hashtable HireTable
        {
            get { return m_HireTable; }
        }

        public void FinalizeClone()
        {
            Payday(this);

            SetHits(Str * 2);
            SetStam(Dex * 2);
            SetMana(Int * 2);

            CalculateBaseDamage();

            Hits = HitsMax;
            Stam = StamMax;
            Mana = ManaMax;

            ControlSlots = 4;

            if (Map == Map.Internal)
            {
                Map = LogoutMap;
            }
        }

        protected override BaseAI ForcedAI
        {
            get { return new OmniAI(this); }
        }

        public override int GetMaxResistance(ResistanceType type)
        {
            return 70;
        }

        public override void OnDoubleClick(Mobile from)
        {
            DisplayPaperdollTo(from);
            base.OnDoubleClick(from);
        }

        public override void OnThink()
        {
            base.OnThink();

            int newRangeFight = CalculateRangeFight(Original);

            if (newRangeFight != RangeFight)
            {
                RangeFight = newRangeFight;
            }

            CalculateBaseDamage();
        }

        public override bool OnBeforeDeath()
        {
            if (Mount != null)
            {
                IMount mount = Mount;

                if (mount is EtherealMountClone)
                {
                    ((EtherealMountClone)mount).Delete();
                }
                else if (mount is MountClone)
                {
                    ((MountClone)mount).Delete();
                }
            }

            return base.OnBeforeDeath();
        }

        public override void OnDeath(Server.Items.Container container)
        {
            base.OnDeath(container);

            if (container != null)
            {
                List<Item> items = new List<Item>(container.Items);

                foreach (Item item in items)
                {
                    SetItemsMovableFalse(item);
                }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            if (Original != null)
            {
                Original.GetProperties(list);
            }
            else
            {
                base.GetProperties(list);
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            Mobile owner = GetOwner();

            if (owner == null)
            {
                base.GetContextMenuEntries(from, list);
                list.Add(new HireEntry(from, this));
            }
            else
            {
                base.GetContextMenuEntries(from, list);
            }
        }

        public override bool CanRegenHits
        {
            get { return true; }
        }

        public override bool CanTeach
        {
            get { return true; }
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (TryTrainSkill(from, item))
            {
                return true;
            }

            if (m_Pay <= 0)
            {
                SayTo(from, "I am not skilled enough to be hired.");
                return false;
            }

            if (m_Pay != 0)
            {
                if (!Controlled)
                {
                    if (item is Gold)
                    {
                        BankBox originalPlayerBank = Original != null ? Original.BankBox : null;

                        if (originalPlayerBank == null)
                        {
                            return false;
                        }

                        if (item.Amount >= m_Pay)
                        {
                            CharacterClone hire = (CharacterClone)m_HireTable[from];

                            if (hire != null && !hire.Deleted && hire.GetOwner() == from)
                            {
                                SayTo(from, 500896);
                                return false;
                            }

                            if (AddHire(from))
                            {
                                SayTo(
                                    from,
                                    string.Format(
                                        "I thank thee for paying me. I will work for thee for {0} hours.",
                                        (int)item.Amount / m_Pay
                                    )
                                );
                                m_HireTable[from] = this;
                                m_HoldGold += item.Amount;
                                originalPlayerBank.DropItem(item);
                                m_PayTimer = new PayTimer(this);
                                m_PayTimer.Start();
                                return true;
                            }

                            return false;
                        }

                        SayHireCost();
                    }
                    else
                    {
                        SayTo(from, 1043268);
                    }
                }
                else
                {
                    Say(1042495);
                }
            }
            else
            {
                SayTo(from, 500200);
            }

            return base.OnDragDrop(from, item);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(Original);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            Original = reader.ReadMobile();

            if (Original == null)
            {
                Delete();
            }
        }

        public virtual bool AddHire(Mobile mobile)
        {
            Mobile owner = GetOwner();

            if (owner != null)
            {
                mobile.SendLocalizedMessage(1043283, owner.Name);
                return false;
            }

            if (SetControlMaster(mobile))
            {
                IsHired = true;
                return true;
            }

            return false;
        }

        public virtual bool Payday(CharacterClone clone)
        {
            SkillName[] skills = new SkillName[]
            {
                SkillName.Anatomy,
                SkillName.Tactics,
                SkillName.Bludgeoning,
                SkillName.Swords,
                SkillName.Fencing,
                SkillName.Marksmanship,
                SkillName.MagicResist,
                SkillName.Healing,
                SkillName.Magery,
                SkillName.Parry,
                SkillName.Bushido,
                SkillName.Knightship,
                SkillName.Necromancy,
                SkillName.Ninjitsu,
                SkillName.Spiritualism,
                SkillName.Psychology,
                SkillName.Stealth,
                SkillName.Hiding
            };

            m_Pay = 0;

            foreach (SkillName skill in skills)
            {
                m_Pay += (int)clone.Skills[skill].Value;
            }

            m_Pay *= 4;

            return true;
        }

        internal void SayHireCost()
        {
            if (m_Pay > 0)
            {
                Say(
                    string.Format(
                        "I am available for hire for {0} gold coins per hour. If thou dost give me gold, I will work for thee.",
                        m_Pay
                    )
                );
            }
            else
            {
                Say("I am not skilled enough to be hired.");
            }
        }

        public virtual Mobile GetOwner()
        {
            if (!Controlled)
            {
                return null;
            }

            Mobile owner = ControlMaster;
            m_IsHired = true;

            if (owner == null)
            {
                return null;
            }

            if (owner.Deleted || owner.Map != Map || !owner.InRange(Location, 30))
            {
                Say(1005653);

                if (owner != null)
                {
                    HireTable.Remove(owner);
                }

                SetControlMaster(null);
                return null;
            }

            return owner;
        }

        public bool TryTrainSkill(Mobile from, Item item)
        {
            if (item is Gold && CheckTeachingMatch(from))
            {
                if (Teach(m_Teaching, from, item.Amount, true))
                {
                    item.Delete();
                    return true;
                }
            }

            return false;
        }

        private static int CalculateRangeFight(Mobile original)
        {
            int maxRangeFight = 1;
            Item weapon =
                original.FindItemOnLayer(Layer.OneHanded)
                ?? original.FindItemOnLayer(Layer.TwoHanded);

            double meleeSkillSum =
                original.Skills[SkillName.Swords].Value
                + original.Skills[SkillName.Bludgeoning].Value
                + original.Skills[SkillName.Fencing].Value
                + original.Skills[SkillName.Bushido].Value
                + original.Skills[SkillName.Ninjitsu].Value
                + original.Skills[SkillName.Knightship].Value;

            double marksmanshipSkill = original.Skills[SkillName.Marksmanship].Value;
            double magerySkill = original.Skills[SkillName.Magery].Value;

            if (
                marksmanshipSkill >= meleeSkillSum
                && marksmanshipSkill >= magerySkill
                && weapon is BaseRanged
            )
            {
                maxRangeFight = 10;
            }
            else if (
                meleeSkillSum >= marksmanshipSkill
                && meleeSkillSum >= magerySkill
                && weapon is BaseMeleeWeapon
            )
            {
                maxRangeFight = 1;
            }
            else if (magerySkill > 0.0)
            {
                maxRangeFight = 9;
            }

            return maxRangeFight;
        }

        private void CalculateBaseDamage()
        {
            double tactics = Original.Skills[SkillName.Tactics].Value;
            double anatomy = Original.Skills[SkillName.Anatomy].Value;
            double lumberjacking = Original.Skills[SkillName.Lumberjacking].Value;
            double strength = Original.Str;

            double tacticsBonus = tactics >= 100 ? (tactics / 1.6) + 6.25 : tactics / 1.6;
            double anatomyBonus = anatomy >= 100 ? (anatomy / 2) + 5 : anatomy / 2;
            double lumberjackingBonus =
                lumberjacking >= 100 ? (lumberjacking / 5) + 10 : lumberjacking / 5;
            double strengthBonus = strength >= 100 ? (strength * 0.3) + 5 : strength * 0.3;

            double totalDamageBonus =
                tacticsBonus + anatomyBonus + lumberjackingBonus + strengthBonus;

            Item weapon = FindItemOnLayer(Layer.OneHanded) ?? FindItemOnLayer(Layer.TwoHanded);
            int minBaseDamage = 5;
            int maxBaseDamage = 10;

            if (weapon is BaseWeapon)
            {
                BaseWeapon baseWeapon = weapon as BaseWeapon;
                minBaseDamage = baseWeapon.MinDamage;
                maxBaseDamage = baseWeapon.MaxDamage;
            }

            int minFinalDamage = (int)(minBaseDamage + (minBaseDamage * totalDamageBonus / 100));
            int maxFinalDamage = (int)(maxBaseDamage + (maxBaseDamage * totalDamageBonus / 100));

            SetDamage(minFinalDamage, maxFinalDamage);
        }

        private void SetItemsMovableFalse(Item item)
        {
            Server.Items.Container container = item as Server.Items.Container;

            if (container != null)
            {
                List<Item> subItems = new List<Item>(container.Items);

                foreach (Item subItem in subItems)
                {
                    SetItemsMovableFalse(subItem);
                }
            }

            item.Movable = false;
        }

        private sealed class PayTimer : Timer
        {
            private readonly CharacterClone m_Hire;

            public PayTimer(CharacterClone vend)
                : base(TimeSpan.FromMinutes(60.0), TimeSpan.FromMinutes(60.0))
            {
                m_Hire = vend;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                if (m_Hire != null && !m_Hire.Deleted)
                {
                    int pay = m_Hire.Pay;

                    if (m_Hire.HoldGold <= pay)
                    {
                        Mobile owner = m_Hire.GetOwner();

                        if (owner != null)
                        {
                            m_Hire.Say(
                                "Ah, my purse grows light, and I must depart. Replenish it, and I shall return. Farewell!"
                            );
                            m_Hire.SetControlMaster(null);
                            m_Hire.IsHired = false;
                            HireTable.Remove(owner);
                        }
                    }
                    else
                    {
                        m_Hire.HoldGold -= pay;
                    }
                }
            }
        }

        private sealed class HireEntry : ContextMenuEntry
        {
            private readonly CharacterClone m_Hire;

            public HireEntry(Mobile from, CharacterClone hire)
                : base(6120, 12)
            {
                m_Hire = hire;
            }

            public override void OnClick()
            {
                m_Hire.Payday(m_Hire);
                m_Hire.SayHireCost();
            }
        }
    }
}