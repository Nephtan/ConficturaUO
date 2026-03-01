using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a dead body")]
    public class corpse : BaseCreature
    {
        private bool m_Stunning;
        private readonly double m_DamageRedirectRatio;
        private readonly double m_EmpowerBonus;

        public override bool IsScaredOfScaryThings
        {
            get { return false; }
        }
        public override bool IsScaryToPets
        {
            get { return true; }
        }

        public override bool IsBondable
        {
            get { return false; }
        }

        public override FoodType FavoriteFood
        {
            get { return FoodType.None; }
        }

        [Constructable]
        public corpse()
            : this(false, 1.0, 0.75, 0.0) { }

        [Constructable]
        public corpse(bool summoned, double scalar)
            : this(summoned, scalar, 0.75, 0.0) { }

        [Constructable]
        public corpse(bool summoned, double scalar, double damageRedirectRatio, double empowerBonus)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a summoned corpse";
            Body = 154;

            if (summoned)
                Hue = 2707;

            m_DamageRedirectRatio = Math.Max(0.0, Math.Min(1.0, damageRedirectRatio));
            m_EmpowerBonus = Math.Max(0.0, empowerBonus);

            SetStr((int)(251 * scalar), (int)(350 * scalar));
            SetDex((int)(76 * scalar), (int)(100 * scalar));
            SetInt((int)(101 * scalar), (int)(150 * scalar));

            SetHits((int)(151 * scalar), (int)(210 * scalar));

            SetDamage((int)(13 * scalar), (int)(24 * scalar));

            // Empowered corpses now deliver a mix of physical and necrotic (poison) damage.
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, (int)(35 * scalar), (int)(55 * scalar));

            if (summoned)
                SetResistance(ResistanceType.Fire, (int)(50 * scalar), (int)(60 * scalar));
            else
                SetResistance(ResistanceType.Fire, (int)(100 * scalar));

            SetResistance(ResistanceType.Cold, (int)(10 * scalar), (int)(30 * scalar));
            SetResistance(ResistanceType.Poison, (int)(10 * scalar), (int)(25 * scalar));
            SetResistance(ResistanceType.Energy, (int)(30 * scalar), (int)(40 * scalar));

            SetSkill(SkillName.MagicResist, (150.1 * scalar), (190.0 * scalar));
            SetSkill(SkillName.Tactics, (70.1 * scalar), (110.0 * scalar));
            SetSkill(SkillName.FistFighting, (70.1 * scalar), (110.0 * scalar));

            if (summoned)
            {
                Fame = 10;
                Karma = 10;
            }
            else
            {
                Fame = 3500;
                Karma = -3500;
            }

            if (!summoned)
            {
                PackItem(new IronIngot(Utility.RandomMinMax(13, 21)));

                if (0.1 > Utility.RandomDouble())
                    PackItem(new PowerCrystal());

                if (0.15 > Utility.RandomDouble())
                    PackItem(new ClockworkAssembly());

                if (0.2 > Utility.RandomDouble())
                    PackItem(new ArcaneGem());

                if (0.25 > Utility.RandomDouble())
                    PackItem(new Gears());
            }

            ControlSlots = 2;
        }

        public override bool DeleteOnRelease
        {
            get { return true; }
        }

        public override int GetAngerSound()
        {
            return 0x4E3;
        }

        public override int GetIdleSound()
        {
            if (!Controlled)
                return 0x4E2;
            return base.GetIdleSound();
        }

        public override int GetDeathSound()
        {
            if (!Controlled)
                return 0x4E0;
            return base.GetDeathSound();
        }

        public override int GetAttackSound()
        {
            return 0x4E1;
        }

        public override int GetHurtSound()
        {
            if (Controlled)
                return 0x4E9;
            return base.GetHurtSound();
        }

        public override bool AutoDispel
        {
            get { return !Controlled; }
        }
        public override bool BleedImmune
        {
            get { return true; }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (!m_Stunning && 0.3 > Utility.RandomDouble())
            {
                m_Stunning = true;

                defender.Animate(21, 6, 1, true, false, 0);
                this.PlaySound(0xEE);
                defender.LocalOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    "You have been stunned by a colossal blow!"
                );

                BaseWeapon weapon = this.Weapon as BaseWeapon;
                if (weapon != null)
                    weapon.OnHit(this, defender);

                if (defender.Alive)
                {
                    defender.Frozen = true;
                    Timer.DelayCall(
                        TimeSpan.FromSeconds(5.0),
                        new TimerStateCallback(Recover_Callback),
                        defender
                    );
                }
            }

            if (Controlled)
            {
                Mobile master = ControlMaster;

                if (master != null)
                {
                    double chance = 0.15 + Math.Min(0.25, m_EmpowerBonus);

                    if (Utility.RandomDouble() <= chance)
                    {
                        int siphon = Utility.RandomMinMax(10, 16) + (int)Math.Round(m_EmpowerBonus * 20);
                        int manaGain = Math.Max(1, (int)Math.Round(siphon * 0.6));
                        int selfHeal = Math.Max(1, (int)Math.Round(siphon * 0.4));

                        SpellHelper.Heal(siphon, master, this);

                        master.Mana += manaGain;

                        if (master.Mana > master.ManaMax)
                            master.Mana = master.ManaMax;

                        int newHits = Hits + selfHeal;

                        if (newHits > HitsMax)
                            newHits = HitsMax;

                        Hits = newHits;

                        defender.FixedParticles(0x374A, 10, 15, 5032, 0x455, 3, EffectLayer.Head);
                        master.FixedParticles(0x375A, 1, 15, 5037, 0x455, 3, EffectLayer.Waist);
                        FixedParticles(0x376A, 1, 15, 5037, 0x455, 3, EffectLayer.Waist);

                        master.SendMessage(0x59, "Your corpse siphons vitality from its foe.");
                    }
                }
            }
        }

        private void Recover_Callback(object state)
        {
            Mobile defender = state as Mobile;

            if (defender != null)
            {
                defender.Frozen = false;
                defender.Combatant = null;
                defender.LocalOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    "You recover your senses."
                );
            }

            m_Stunning = false;
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (Controlled || Summoned)
            {
                Mobile master = (this.ControlMaster);

                if (master == null)
                    master = this.SummonMaster;

                if (
                    master != null
                    && master.Player
                    && master.Map == this.Map
                    && master.InRange(Location, 20)
                )
                {
                    int siphon = (int)Math.Ceiling(amount * m_DamageRedirectRatio);

                    if (siphon > 0)
                    {
                        if (master.Mana >= siphon)
                        {
                            master.Mana -= siphon;
                        }
                        else
                        {
                            siphon -= master.Mana;
                            master.Mana = 0;
                            master.Damage(siphon);
                        }
                    }
                }
            }

            base.OnDamage(amount, from, willKill);
        }

        public override bool BardImmune
        {
            get { return !Core.AOS || Controlled; }
        }
        public override Poison PoisonImmune
        {
            get { return Poison.Deadly; }
        }

        public corpse(Serial serial)
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
}
