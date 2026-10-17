using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Targeting;

namespace Server.Spells.Research
{
    public class ResearchEnchant : ResearchSpell
    {
        public override int spellIndex
        {
            get { return 35; }
        }
        public int CirclePower = 5;
        public static int spellID = 35;
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(3.0); }
        }
        public override double RequiredSkill
        {
            get
            {
                return (double)(Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 8)));
            }
        }
        public override int RequiredMana
        {
            get { return Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 7)); }
        }

        private static SpellInfo m_Info = new SpellInfo(
            Server.Misc.Research.SpellInformation(spellID, 2),
            Server.Misc.Research.CapsCast(Server.Misc.Research.SpellInformation(spellID, 4)),
            266,
            9040,
            Reagent.PixieSkull,
            Reagent.FairyEgg,
            Reagent.DragonTooth,
            Reagent.MoonCrystal
        );

        public ResearchEnchant(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public static void Initialize()
        {
            EventSink.Logout -= OnLogout;
            EventSink.Logout += OnLogout;
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this, spellID);
        }

        private static void OnLogout(LogoutEventArgs e)
        {
            if (e == null)
                return;

            Mobile mobile = e.Mobile;

            if (mobile == null || mobile.Deleted)
                return;

            ResearchEnchant.EndEffects(mobile);
        }

        public void Target(object o, int spellID)
        {
            if (o is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)o;

                if (Caster.Backpack.FindItemByType(typeof(ResearchEnchantStone)) != null)
                {
                    DoFizzle();
                }
                else if (!Caster.CanSee(weapon))
                {
                    Caster.SendLocalizedMessage(500237); // Target can not be seen.
                }
                else if (!Caster.CanBeginAction(typeof(ResearchEnchant)))
                {
                    Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                }
                else if (!weapon.IsChildOf(Caster.Backpack))
                {
                    Caster.SendMessage("The weapon must be in your pack to enchant.");
                }
                else if (CheckSequence())
                {
                    ResearchEnchantStone orb = new ResearchEnchantStone();
                    Caster.AddToBackpack(orb);
                    Server.Misc.Research.ConsumeScroll(
                        Caster,
                        true,
                        spellID,
                        alwaysConsume,
                        Scroll
                    );

                    string name = weapon.Name;
                    if (weapon.Name != null && weapon.Name != "")
                    {
                        name = weapon.Name;
                    }
                    if (name == null)
                    {
                        name = MorphingItem.AddSpacesToSentence((weapon.GetType()).Name);
                    }

                    orb.EnchantOwner = Caster;
                    orb.EnchantSerial = weapon.Serial;
                    orb.EnchantName = name;
                    orb.EnchantDmg = weapon.Attributes.WeaponDamage;
                    orb.EnchantHue = weapon.Hue;

                    weapon.Name = "" + name + " [enchanted]";
                    weapon.Hue = 0x489;
                    weapon.Attributes.WeaponDamage += 100;

                    Caster.PlaySound(0x1F7);
                    Caster.FixedParticles(
                        0x3039,
                        9,
                        32,
                        5008,
                        Server.Misc.PlayerSettings.GetMySpellHue(true, Caster, 0x85E),
                        0,
                        EffectLayer.Waist
                    );

                    int val = (int)(DamagingSkill(Caster) / 2);

                    BuffInfo.RemoveBuff(Caster, BuffIcon.EnchantWeapon);
                    BuffInfo.AddBuff(
                        Caster,
                        new BuffInfo(
                            BuffIcon.EnchantWeapon,
                            1063636,
                            TimeSpan.FromMinutes(val),
                            Caster
                        )
                    );

                    if (val > 120)
                        val = 120;

                    new InternalTimer(orb, TimeSpan.FromMinutes(val)).Start();
                }
            }
            else
            {
                Caster.SendMessage("You can only enchant weapons with this spell.");
            }
            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ResearchEnchant m_Owner;
            private int m_SpellID;

            public InternalTarget(ResearchEnchant owner, int spellID)
                : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
                m_SpellID = spellID;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is BaseWeapon)
                {
                    m_Owner.Target((BaseWeapon)o, m_SpellID);
                }
                else
                {
                    from.SendMessage("That cannot be enchanted.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class InternalTimer : Timer
        {
            private ResearchEnchantStone m_Orb;
            private DateTime m_Expire;

            public InternalTimer(ResearchEnchantStone orb, TimeSpan duration)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.1))
            {
                m_Orb = orb;
                m_Expire = DateTime.Now + duration;
            }

            protected override void OnTick()
            {
                if (m_Orb == null || m_Orb.Deleted)
                {
                    Stop();
                    return;
                }

                if (DateTime.Now >= m_Expire)
                {
                    EndEffect(m_Orb);
                    Stop();
                }
            }
        }

        public static void EndEffects(Mobile m)
        {
            List<ResearchEnchantStone> targets = ResearchEnchantStone.FindByOwner(m);
            for (int i = 0; i < targets.Count; ++i)
            {
                EndEffect(targets[i]);
            }
        }

        internal static void EndEffect(ResearchEnchantStone orb)
        {
            if (orb == null || orb.Deleted)
                return;

            BaseWeapon weapon = World.FindItem((Serial)orb.EnchantSerial) as BaseWeapon;
            if (weapon != null && !weapon.Deleted && weapon.Name != null && weapon.Name.Contains("[enchanted]"))
            {
                weapon.Hue = orb.EnchantHue;
                weapon.Name = orb.EnchantName;
                weapon.Attributes.WeaponDamage = orb.EnchantDmg;
            }

            Mobile owner = orb.EnchantOwner;
            orb.Delete();

            if (owner != null && !owner.Deleted)
            {
                owner.PlaySound(0x1F8);
                BuffInfo.RemoveBuff(owner, BuffIcon.EnchantWeapon);
            }
        }
    }
}

namespace Server.Items
{
    public class ResearchEnchantStone : Item
    {
        // Track only enchantment orbs; logout must not enumerate the world's items.
        private static List<ResearchEnchantStone> m_Stones = new List<ResearchEnchantStone>();

        internal static List<ResearchEnchantStone> FindByOwner(Mobile owner)
        {
            List<ResearchEnchantStone> matches = new List<ResearchEnchantStone>();
            for (int i = 0; i < m_Stones.Count; ++i)
            {
                ResearchEnchantStone orb = m_Stones[i];
                if (!orb.Deleted && (owner == null || orb.EnchantOwner == owner))
                    matches.Add(orb);
            }
            return matches;
        }

        [Constructable]
        public ResearchEnchantStone()
            : base(0x3199)
        {
            Weight = 1.0;
            Movable = false;
            Hue = 0x489;
            Name = "Enchantment Power Orb";
            LootType = LootType.Blessed;
            m_Stones.Add(this);
        }

        public override void OnAfterDelete()
        {
            m_Stones.Remove(this);
            base.OnAfterDelete();
        }

        public override bool DisplayLootType
        {
            get { return false; }
        }

        public ResearchEnchantStone(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.Write((Mobile)EnchantOwner);
            writer.Write(EnchantSerial);
            writer.Write(EnchantName);
            writer.Write(EnchantDmg);
            writer.Write(EnchantHue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            EnchantOwner = reader.ReadMobile();
            EnchantSerial = reader.ReadInt();
            EnchantName = reader.ReadString();
            EnchantDmg = reader.ReadInt();
            EnchantHue = reader.ReadInt();

            m_Stones.Add(this);
            RunTime thisTimer = new RunTime(this);
            thisTimer.Start();
        }

        public class RunTime : Timer
        {
            private ResearchEnchantStone m_Orb;

            public RunTime(Item task)
                : base(TimeSpan.FromSeconds(10.0))
            {
                m_Orb = task as ResearchEnchantStone;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                Server.Spells.Research.ResearchEnchant.EndEffect(m_Orb);
            }
        }

        public Mobile EnchantOwner;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Enchant_Owner
        {
            get { return EnchantOwner; }
            set { EnchantOwner = value; }
        }

        public int EnchantSerial;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Enchant_Serial
        {
            get { return EnchantSerial; }
            set { EnchantSerial = value; }
        }

        public string EnchantName;

        [CommandProperty(AccessLevel.GameMaster)]
        public string Enchant_Name
        {
            get { return EnchantName; }
            set { EnchantName = value; }
        }

        public int EnchantDmg;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Enchant_Dmg
        {
            get { return EnchantDmg; }
            set { EnchantDmg = value; }
        }

        public int EnchantHue;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Enchant_Hue
        {
            get { return EnchantHue; }
            set { EnchantHue = value; }
        }
    }
}
