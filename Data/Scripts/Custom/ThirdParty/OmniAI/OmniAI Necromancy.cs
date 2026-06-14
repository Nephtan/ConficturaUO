// Created by Peoharen
using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.SkillHandlers;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Spells.Chivalry;
using Server.Spells.DeathKnight;
using Server.Spells.Eighth;
using Server.Spells.Elementalism;
using Server.Spells.Fifth;
using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells.Herbalist;
using Server.Spells.HolyMan;
using Server.Spells.Jedi;
using Server.Spells.Jester;
using Server.Spells.Magical;
using Server.Spells.Mystic;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Research;
using Server.Spells.Second;
using Server.Spells.Seventh;
using Server.Spells.Shinobi;
using Server.Spells.Sixth;
using Server.Spells.Song;
using Server.Spells.Syth;
using Server.Spells.Third;
using Server.Spells.Undead;
using Server.Targeting;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public DateTime m_NextShiftTime;

        public virtual bool m_CanShapeShift
        {
            get
            {
                return (
                    m_Mobile is BaseVendor /*|| m_Mobile is BaseEscortable*/
                );
            }
        }

        public void NecromancerPower()
        {
            Spell spell = null;

            spell = GetNecromancerSpell();

            if (spell != null)
                spell.Cast();

            return;
        }

        public Spell GetNecromancerSpell()
        {
            Spell spell = null;

            switch (Utility.Random(12))
            {
                case 0:
                case 1: // Create an army for our ourselves
                {
                    int whichone = Utility.RandomMinMax(1, 2);

                    if (whichone == 2 && m_Mobile.Skills[SkillName.Necromancy].Value > 50.0)
                    {
                        if (m_Mobile.Debug)
                            m_Mobile.Say(1109, "Undead: Casting Animate Dead");

                        spell = new AnimateDeadSpell(m_Mobile, null);
                    }
                    else if (m_Mobile.Followers == 0 || m_Mobile.Followers == 3)
                    {
                        if (m_Mobile.Debug)
                            m_Mobile.Say(1109, "Undead: Summoning Familiar ");

                        CreateNecromancerFamiliar();
                    }
                    else
                        goto default;

                    break;
                }
                case 2:
                case 3: // Curse them
                {
                    if (m_Mobile.Debug)
                        m_Mobile.Say(1109, "Cusing Them");

                    spell = GetNecromancerCurseSpell();
                    break;
                }
                case 4:
                case 5: // Reverse combat
                {
                    if (m_Mobile.Debug)
                        m_Mobile.Say(1109, "Casting Blood Oath");

                    if (m_Mobile.Skills[SkillName.Necromancy].Value > 30.0)
                        spell = new BloodOathSpell(m_Mobile, null);

                    break;
                }
                case 6: // Shapeshift
                {
                    if (m_CanShapeShift)
                    {
                        if (m_Mobile.Debug)
                            m_Mobile.Say(1109, "Shapechange ");

                        spell = GetNecromancerShapeshiftSpell();
                    }
                    else
                        goto case 3; // Curse them

                    break;
                }

                default: // Damage them
                {
                    if (m_Mobile.Debug)
                        m_Mobile.Say(1109, "Random damage spell");

                    spell = GetNecromancerDamageSpell();
                    break;
                }
            }

            return spell;
        }

        public Spell GetNecromancerDamageSpell()
        {
            int maxCircle = (int)(
                (m_Mobile.Skills[SkillName.Necromancy].Value + 20.0) / (100.0 / 7.0)
            );

            if (maxCircle < 2)
                maxCircle = 2;

            switch (Utility.Random(maxCircle + 1))
            {
                case 0:
                case 1:
                case 2:
                    return CheckForCurseWeapon();
                case 3:
                case 4:
                    return new PoisonStrikeSpell(m_Mobile, null);
                case 5:
                case 6:
                    return new WitherSpell(m_Mobile, null);
                case 7:
                    return new StrangleSpell(m_Mobile, null);
                default:
                    return CheckForVengefulSpiritSpell();
            }
        }

        public Spell GetNecromancerCurseSpell()
        {
            int whichone = Utility.RandomMinMax(1, 4);

            if (whichone == 4 && m_Mobile.Skills[SkillName.Necromancy].Value >= 75.0)
                return new StrangleSpell(m_Mobile, null);
            else if (whichone == 3 && m_Mobile.Skills[SkillName.Necromancy].Value >= 40.0)
                return new MindRotSpell(m_Mobile, null);
            else if (whichone >= 2 && m_Mobile.Skills[SkillName.Necromancy].Value >= 30.0)
                return new EvilOmenSpell(m_Mobile, null);
            else
                return new CorpseSkinSpell(m_Mobile, null);
        }

        public Spell GetNecromancerShapeshiftSpell()
        {
            if (DateTime.Now < m_NextShiftTime)
                return GetNecromancerDamageSpell();

            m_NextShiftTime = DateTime.Now + TimeSpan.FromSeconds(130);

            if (m_Mobile.Skills[SkillName.Necromancy].Value > 110.0)
                return new VampiricEmbraceSpell(m_Mobile, null);
            else if (m_Mobile.Skills[SkillName.Necromancy].Value > 80.0)
                return new LichFormSpell(m_Mobile, null);
            else if (m_Mobile.Skills[SkillName.Necromancy].Value > 50.0)
                return new HorrificBeastSpell(m_Mobile, null);
            else if (m_Mobile.Skills[SkillName.Necromancy].Value > 30.0)
                return new WraithFormSpell(m_Mobile, null);
            else
                return null;
        }

        public Spell CheckForCurseWeapon()
        {
            if (m_Mobile.Skills[SkillName.Necromancy].Value > 5.0)
            {
                BaseWeapon weapon = m_Mobile.FindItemOnLayer(Layer.OneHanded) as BaseWeapon;

                if (weapon == null)
                    weapon = m_Mobile.FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;

                if (weapon != null)
                {
                    if (!(weapon.Cursed))
                        return new CurseWeaponSpell(m_Mobile, null);
                }
            }

            return new PainSpikeSpell(m_Mobile, null);
        }

        public Spell CheckForVengefulSpiritSpell()
        {
            if (m_Mobile.Followers >= 3)
                return new PoisonStrikeSpell(m_Mobile, null);
            else
                return new VengefulSpiritSpell(m_Mobile, null);
        }

        public void CreateNecromancerFamiliar()
        {
            int whichone = Utility.RandomMinMax(1, 5);
            BaseCreature mob = null;

            if (whichone == 5 && m_Mobile.Skills[SkillName.Necromancy].Value >= 100.0)
                mob = new VampireBatFamiliar();
            else if (whichone >= 4 && m_Mobile.Skills[SkillName.Necromancy].Value >= 80.0)
                mob = new DeathAdder();
            else if (whichone >= 3 && m_Mobile.Skills[SkillName.Necromancy].Value >= 60.0)
                mob = new DarkWolfFamiliar();
            else if (whichone >= 2 && m_Mobile.Skills[SkillName.Necromancy].Value >= 50.0)
                mob = new ShadowWispFamiliar();
            else if (m_Mobile.Skills[SkillName.Necromancy].Value >= 30.0)
                mob = new HordeMinionFamiliar();

            if (mob != null)
            {
                BaseCreature.Summon(mob, m_Mobile, m_Mobile.Location, -1, TimeSpan.FromDays(1.0));
                mob.FixedParticles(0x3728, 1, 10, 9910, EffectLayer.Head);
                mob.PlaySound(mob.GetIdleSound());
            }

            return;
        }
    }
}
