// Created by Peoharen
using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Engines.PartySystem;
using Server.Guilds;
using Server.Items;
using Server.Network;
using Server.Misc;
using Server.Mobiles;
using Server.Regions;
using Server.SkillHandlers;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Spells.Chivalry;
using Server.Spells.DeathKnight;
using Server.Spells.Elementalism;
using Server.Spells.Herbalist;
using Server.Spells.HolyMan;
using Server.Spells.Jedi;
using Server.Spells.Syth;
using Server.Spells.Jester;
using Server.Spells.Magical;
using Server.Spells.Mystic;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Research;
using Server.Spells.Shinobi;
using Server.Spells.Song;
using Server.Spells.Undead;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Eighth;
using Server.Targeting;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        private DateTime m_NextCastTime;
        private DateTime m_NextHealTime;
        private DateTime m_NextFieldCheck;
        private DateTime m_NextCheckArmed;

        #region canuse
        public virtual bool m_IsSmart
        {
            get { return true; }
        }

        public virtual bool m_CanUseBard
        {
            get { return (m_Mobile.Skills[SkillName.Musicianship].Base > 10.0); }
        }

        public virtual bool m_CanUseBushido
        {
            get { return (m_Mobile.Skills[SkillName.Bushido].Base > 10.0); }
        }

        public virtual bool m_CanUseKnightship
        {
            get { return (m_Mobile.Skills[SkillName.Knightship].Base > 10.0); }
        }

        public virtual bool m_CanUseMagery
        {
            get { return (m_Mobile.Skills[SkillName.Magery].Base > 10.0); }
        }

        public virtual bool m_CanUseNecromancy
        {
            get { return (m_Mobile.Skills[SkillName.Necromancy].Base > 10.0); }
        }

        public virtual bool m_CanUseNinjitsu
        {
            get { return (m_Mobile.Skills[SkillName.Ninjitsu].Base > 10.0); }
        }

        public virtual bool m_SwapWeapons
        {
            get { return m_CanUseBushido || m_CanUseNinjitsu; }
        }

        public virtual bool m_Melees
        {
            get
            {
                if (m_Mobile.Weapon is BaseRanged)
                    return false;
                else if (!m_CanUseMagery && !m_CanUseNecromancy)
                    return true;
                if (m_CanUseKnightship || m_CanUseBushido || m_CanUseNinjitsu)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        public OmniAI(BaseCreature m)
            : base(m) { }

        #region getoutofmyway
        public override bool Think()
        {
            if (m_Mobile.Deleted)
                return false;

            if (DateTime.Now > m_NextFieldCheck)
            {
                CheckForFieldSpells();
                m_NextFieldCheck = DateTime.Now + TimeSpan.FromSeconds(2);
            }

            if (DateTime.Now > m_NextCheckArmed)
            {
                CheckArmed(m_SwapWeapons);
                m_NextCheckArmed = DateTime.Now + TimeSpan.FromSeconds(3);
            }

            if (ProcessTarget())
                return true;
            else
                return base.Think();
        }

        public override bool DoActionWander()
        {
            if (m_Mobile.Debug)
                m_Mobile.DebugSay("I have no combatant");

            if (!m_Mobile.Hidden && !m_Mobile.Poisoned && m_CanUseNinjitsu)
                m_Mobile.UseSkill(SkillName.Hiding);

            if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I have detected {0}, attacking", m_Mobile.FocusMob.Name);

                m_Mobile.Combatant = m_Mobile.FocusMob;
                Action = ActionType.Combat;
            }
            else if (
                m_Mobile.Mana < m_Mobile.ManaMax
                && m_Mobile.Skills[SkillName.Meditation].Value > 60.0
            )
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I am going to meditate");

                m_Mobile.UseSkill(SkillName.Meditation);
            }
            else
            {
                base.DoActionWander();
                TryToHeal();
            }

            return true;
        }
        #endregion

        public override bool DoActionCombat()
        {
            Mobile combatant = m_Mobile.Combatant;

            if (
                combatant == null
                || combatant.Deleted
                || combatant.Map != m_Mobile.Map
                || !combatant.Alive
                || combatant.IsDeadBondedPet
            )
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("My combatant is gone, so my guard is up");

                Action = ActionType.Guard;
                return true;
            }

            if (!m_Mobile.InRange(combatant, m_Mobile.RangePerception))
            {
                // They are somewhat far away, can we find something else?
                if (
                    AcquireFocusMob(
                        m_Mobile.RangePerception,
                        m_Mobile.FightMode,
                        false,
                        false,
                        true
                    )
                )
                {
                    m_Mobile.Combatant = m_Mobile.FocusMob;
                    m_Mobile.FocusMob = null;
                }
                else if (!m_Mobile.InRange(combatant, m_Mobile.RangePerception * 3))
                {
                    m_Mobile.Combatant = null;
                }

                combatant = m_Mobile.Combatant;

                if (combatant == null)
                {
                    if (m_Mobile.Debug)
                        m_Mobile.DebugSay("My combatant has fled, so I am on guard");

                    Action = ActionType.Guard;
                    return true;
                }
            }

            if (MoveTo(combatant, true, m_Mobile.RangeFight))
            {
                m_Mobile.Direction = m_Mobile.GetDirectionTo(combatant);
            }
            else if (
                AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true)
            )
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay(
                        "My move is blocked, so I am going to attack {0}",
                        m_Mobile.FocusMob.Name
                    );

                m_Mobile.Combatant = m_Mobile.FocusMob;
                Action = ActionType.Combat;

                return true;
            }
            else if (m_Mobile.GetDistanceToSqrt(combatant) > m_Mobile.RangePerception + 1)
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I cannot find {0}, so my guard is up", combatant.Name);

                Action = ActionType.Guard;

                return true;
            }
            else
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I should be closer to {0}", combatant.Name);
            }

            if (!m_Mobile.Controlled && !m_Mobile.Summoned && !m_Mobile.IsParagon)
            {
                if (m_Mobile.Hits < m_Mobile.HitsMax * 20 / 100)
                {
                    // We are low on health, should we flee?
                    bool flee = false;

                    if (m_Mobile.Hits < combatant.Hits)
                    {
                        // We are more hurt than them
                        int diff = combatant.Hits - m_Mobile.Hits;

                        flee = (Utility.Random(0, 100) < (10 + diff)); // (10 + diff)% chance to flee
                    }
                    else
                    {
                        flee = Utility.Random(0, 100) < 10; // 10% chance to flee
                    }

                    if (flee)
                    {
                        if (m_Mobile.Debug)
                            m_Mobile.DebugSay("I am going to flee from {0}", combatant.Name);

                        Action = ActionType.Flee;
                    }
                }
            }

            if (TryToHeal())
                return true;
            else if (
                m_Mobile.Spell == null
                && DateTime.Now > m_NextCastTime
                && m_Mobile.InRange(combatant, 12)
            )
            {
                m_NextCastTime = DateTime.Now + TimeSpan.FromSeconds(1);

                List<int> skill = new List<int>();

                if (m_CanUseBard)
                    skill.Add(0);
                if (m_CanUseBushido)
                    skill.Add(1);
                if (m_CanUseKnightship)
                    skill.Add(2);
                if (m_CanUseMagery)
                    skill.Add(3);
                if (m_CanUseNecromancy)
                    skill.Add(4);
                if (m_CanUseNinjitsu)
                    skill.Add(5);
                //if ( m_CanUseSpellweaving )
                //	skill.Add( 6 );

                if (skill.Count == 0)
                    return true;

                int whichone = skill[0];

                if (skill.Count > 1)
                    whichone = skill[Utility.Random(skill.Count)];

                switch (whichone)
                {
                    // case 0: BardPower(); break;
                    case 1:
                        BushidoPower();
                        break;
                    case 2:
                        KnightshipPower();
                        break;
                    case 3:
                        MageryPower();
                        break;
                    case 4:
                        NecromancerPower();
                        break;
                    case 5:
                        NinjitsuPower();
                        break;
                    //case 6: SpellweavingPower(); break;
                }
            }

            return true;
        }

        public override bool DoActionGuard()
        {
            if (!m_Mobile.Hidden && !m_Mobile.Poisoned && m_CanUseNinjitsu)
                m_Mobile.UseSkill(SkillName.Hiding);

            if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I have detected {0}, attacking", m_Mobile.FocusMob.Name);

                m_Mobile.Combatant = m_Mobile.FocusMob;
                Action = ActionType.Combat;
            }
            else
            {
                base.DoActionGuard();
            }

            return true;
        }

        public override bool DoActionFlee()
        {
            if (m_Mobile.Hits > (m_Mobile.HitsMax / 2))
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I am stronger now, so I will continue fighting");

                Action = ActionType.Combat;
            }
            else if (
                m_CanUseNinjitsu
                && m_Mobile.Combatant != null
                && m_SmokeBombs > 0
                && m_Mobile.Mana >= 10
                && m_Mobile.Hidden == false
            )
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I am using a smoke bomb. ");

                --m_SmokeBombs;

                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I have {0} smoke bombs left.", m_SmokeBombs.ToString());

                m_Mobile.Mana -= 10;
                m_Mobile.Hidden = true;
                Server.SkillHandlers.Stealth.OnUse(m_Mobile);
                m_Mobile.FixedParticles(0x3709, 1, 30, 9904, 1108, 6, EffectLayer.RightFoot);
                m_Mobile.PlaySound(0x22F);
            }

            m_Mobile.FocusMob = m_Mobile.Combatant;
            base.DoActionFlee();

            return true;
        }

        public override bool MoveTo(Mobile m, bool run, int range)
        {
            if (m_Mobile.Hidden && !m_Mobile.Poisoned && m_CanUseNinjitsu)
            {
                Server.SkillHandlers.Stealth.OnUse(m_Mobile);

                if (base.MoveTo(m, false, range) == false)
                {
                    if (m_Mobile.Hidden && m_Mobile.AllowedStealthSteps >= 1 && m_CanUseNinjitsu)
                    {
                        Spell spell = new Shadowjump(m_Mobile, null);
                        spell.Cast();
                    }

                    return false;
                }
                else
                    return true;
            }
            else if (!m_Melees && m != null)
            {
                if (m_Mobile.InRange(m, 2) && CheckMove())
                {
                    Direction d = Direction.North;

                    switch (m_Mobile.GetDirectionTo(m))
                    {
                        case Direction.North:
                            d = Direction.South;
                            break;
                        case Direction.South:
                            d = Direction.North;
                            break;
                        case Direction.East:
                            d = Direction.West;
                            break;
                        case Direction.West:
                            d = Direction.East;
                            break;
                        case Direction.Up:
                            d = Direction.Down;
                            break;
                        case Direction.Down:
                            d = Direction.Up;
                            break;
                        case Direction.Right:
                            d = Direction.Left;
                            break;
                        case Direction.Left:
                            d = Direction.Right;
                            break;
                    }

                    return DoMove(d, run);
                    // base.DoActionFlee();
                }
                else if (m_Mobile.InRange(m, 4))
                    return true;
            }

            return base.MoveTo(m, run, range);
        }

        public override bool WalkMobileRange(
            Mobile m,
            int iSteps,
            bool bRun,
            int iWantDistMin,
            int iWantDistMax
        )
        {
            if (m_Mobile.Hidden && !m_Mobile.Poisoned && m_CanUseNinjitsu)
            {
                Server.SkillHandlers.Stealth.OnUse(m_Mobile);

                return base.WalkMobileRange(m, iSteps, false, iWantDistMin, iWantDistMax);
            }
            else
                return base.WalkMobileRange(m, iSteps, bRun, iWantDistMin, iWantDistMax);
        }

        public override void WalkRandom(int iChanceToNotMove, int iChanceToDir, int iSteps)
        {
            if (m_Mobile.Hidden && m_CanUseNinjitsu)
                Server.SkillHandlers.Stealth.OnUse(m_Mobile);

            base.WalkRandom(iChanceToNotMove, iChanceToDir, iSteps);
        }

        private static int[] m_RandomLocations = new int[]
        {
            -1,
            -1,
            -1,
            0,
            -1,
            1,
            0,
            -1,
            0,
            1,
            1,
            -1,
            1,
            0,
            1,
            1,
            -2,
            -2,
            -2,
            -1,
            -2,
            0,
            -2,
            1,
            -2,
            2,
            -1,
            -2,
            -1,
            2,
            0,
            -2,
            0,
            2,
            1,
            -2,
            1,
            2,
            2,
            -2,
            2,
            -1,
            2,
            0,
            2,
            1,
            2,
            2
        };

        private bool ProcessTarget()
        {
            Target targ = m_Mobile.Target;

            if (targ == null)
                return false;

            Mobile toTarget;

            toTarget = m_Mobile.Combatant;

            //if ( toTarget != null )
            //RunTo( toTarget );

            if (targ is DispelSpell.InternalTarget && !(m_Mobile.AutoDispel))
            {
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in m_Mobile.GetMobilesInRange(12))
                {
                    if (m is BaseCreature)
                    {
                        if (((BaseCreature)m).IsDispellable && CanTarget(m_Mobile, m))
                            targets.Add(m);
                    }
                }

                if (targets.Count >= 0)
                {
                    int whichone = Utility.RandomMinMax(0, targets.Count);

                    if (targets[whichone] != null)
                        targ.Invoke(m_Mobile, targets[whichone]);
                }
            }
            else if (targ is TeleportSpell.InternalTarget || targ is Shadowjump.InternalTarget)
            {
                if (targ is Shadowjump.InternalTarget && !m_Mobile.Hidden)
                    return false;

                Map map = m_Mobile.Map;

                if (map == null)
                {
                    targ.Cancel(m_Mobile, TargetCancelType.Canceled);
                    return true;
                }

                int px,
                    py;
                bool teleportAway = (m_Mobile.Hits < (m_Mobile.Hits / 10));

                if (teleportAway)
                {
                    int rx = m_Mobile.X - toTarget.X;
                    int ry = m_Mobile.Y - toTarget.Y;
                    double d = m_Mobile.GetDistanceToSqrt(toTarget);

                    px = toTarget.X + (int)(rx * (10 / d));
                    py = toTarget.Y + (int)(ry * (10 / d));
                }
                else
                {
                    if (toTarget != null) // Add this null check
                    {
                        px = toTarget.X;
                        py = toTarget.Y;
                    }
                    else
                    {
                        // Handle the case when toTarget is null, for example, set px and py to default values or cancel the target
                        targ.Cancel(m_Mobile, TargetCancelType.Canceled);
                        return true;
                    }
                }

                for (int i = 0; i < m_RandomLocations.Length; i += 2)
                {
                    int x = m_RandomLocations[i],
                        y = m_RandomLocations[i + 1];

                    Point3D p = new Point3D(px + x, py + y, 0);

                    LandTarget lt = new LandTarget(p, map);

                    if (
                        (targ.Range == -1 || m_Mobile.InRange(p, targ.Range))
                        && m_Mobile.InLOS(lt)
                        && map.CanSpawnMobile(px + x, py + y, lt.Z)
                        && !SpellHelper.CheckMulti(p, map)
                    )
                    {
                        targ.Invoke(m_Mobile, lt);
                        return true;
                    }
                }

                int teleRange = targ.Range;

                if (teleRange < 0)
                    teleRange = 12;

                for (int i = 0; i < 10; ++i)
                {
                    Point3D randomPoint = new Point3D(
                        m_Mobile.X - teleRange + Utility.Random(teleRange * 2 + 1),
                        m_Mobile.Y - teleRange + Utility.Random(teleRange * 2 + 1),
                        0
                    );

                    LandTarget lt = new LandTarget(randomPoint, map);

                    if (
                        m_Mobile.InLOS(lt)
                        && map.CanSpawnMobile(lt.X, lt.Y, lt.Z)
                        && !SpellHelper.CheckMulti(randomPoint, map)
                    )
                    {
                        targ.Invoke(m_Mobile, new LandTarget(randomPoint, map));
                        return true;
                    }
                }
            }
            else if (targ is AnimateDeadSpell.InternalTarget)
            {
                Type type = null;

                List<Item> itemtargets = new List<Item>();

                foreach (Item itemstofind in m_Mobile.GetItemsInRange(5))
                {
                    if (itemstofind is Corpse)
                    {
                        itemtargets.Add(itemstofind);
                    }
                }

                for (int i = 0; i < itemtargets.Count; ++i)
                {
                    Corpse items = (Corpse)itemtargets[i];

                    if (items.Owner != null)
                        type = items.Owner.GetType();

                    if (
                        items.ItemID != 0x2006
                        || items.Channeled
                        || type == typeof(PlayerMobile)
                        || type == null
                        || (items.Owner != null && items.Owner.Fame < 100)
                        || (
                            (items.Owner != null)
                            && (items.Owner is BaseCreature)
                            && (
                                ((BaseCreature)items.Owner).Summoned
                                || ((BaseCreature)items.Owner).IsBonded
                            )
                        )
                    )
                        continue;
                    else
                    {
                        targ.Invoke(m_Mobile, items);
                        break;
                    }
                }

                if (targ != null)
                    targ.Cancel(m_Mobile, TargetCancelType.Canceled);
            }
            else if ((targ.Flags & TargetFlags.Harmful) != 0 && toTarget != null)
            {
                if (
                    (targ.Range == -1 || m_Mobile.InRange(toTarget, targ.Range))
                    && m_Mobile.CanSee(toTarget)
                    && m_Mobile.InLOS(toTarget)
                )
                {
                    targ.Invoke(m_Mobile, toTarget);
                }
            }
            else if ((targ.Flags & TargetFlags.Beneficial) != 0)
            {
                targ.Invoke(m_Mobile, m_Mobile);
            }
            else
            {
                targ.Cancel(m_Mobile, TargetCancelType.Canceled);
            }

            return true;
        }

        #region Targeting
        public static Mobile FindRandomTarget(Mobile from)
        {
            return FindRandomTarget(from, true);
        }

        public static Mobile FindRandomTarget(Mobile from, bool allowcombatant)
        {
            List<Mobile> list = new List<Mobile>();

            foreach (Mobile m in from.GetMobilesInRange(12))
            {
                if (m != null && m != from)
                    if (CanTarget(from, m) && from.InLOS(m))
                    {
                        if (allowcombatant && m == from.Combatant)
                            continue;
                        else
                            list.Add(m);
                    }
            }

            if (list.Count == 0)
                return null;
            if (list.Count == 1)
                return list[0];

            return list[Utility.Random(list.Count)];
        }

        public static bool CanTarget(Mobile from, Mobile to)
        {
            return CanTarget(from, to, true, false, false);
        }

        public static bool CanTarget(Mobile from, Mobile to, bool harm)
        {
            return CanTarget(from, to, harm, false, false);
        }

        public static bool CanTarget(
            Mobile from,
            Mobile to,
            bool harm,
            bool checkguildparty,
            bool allownull
        )
        {
            if (to == null)
                return false;
            else if (from == null)
                return allownull;
            else if (from == to && !harm)
                return true;
            else if ((harm && to.Blessed) || (to.AccessLevel != AccessLevel.Player && to.Hidden))
                return false;

            if (checkguildparty)
            {
                //Guilds
                Guild fromguild = GetGuild(from);
                Guild toguild = GetGuild(to);

                if (fromguild != null && toguild != null)
                    if (fromguild == toguild || fromguild.IsAlly(toguild))
                        return !harm;

                //Parties
                Party p = GetParty(from);

                if (p != null && p.Contains(to))
                    return !harm;
            }

            //Default
            if (harm)
                return (IsGoodGuy(from) && !(IsGoodGuy(to)))
                    | (!(IsGoodGuy(from)) && IsGoodGuy(to));
            else
                return (IsGoodGuy(from) && IsGoodGuy(to))
                    | (!(IsGoodGuy(from)) && !(IsGoodGuy(to)));
        }

        public static bool IsGoodGuy(Mobile m)
        {
            if (m.Criminal)
                return false;

            if (m.Player && m.Kills < 5)
                return true;

            if (m is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)m;

                if (bc.Controlled || bc.Summoned)
                {
                    if (bc.ControlMaster != null)
                        return IsGoodGuy(bc.ControlMaster);
                    else if (bc.SummonMaster != null)
                        return IsGoodGuy(bc.SummonMaster);
                }
            }

            return false;
        }

        public static Guild GetGuild(Mobile m)
        {
            Guild guild = m.Guild as Guild;

            if (guild == null && m is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)m;
                m = bc.ControlMaster;

                if (m != null)
                    guild = m.Guild as Guild;

                m = bc.SummonMaster;

                if (m != null && guild == null)
                    guild = m.Guild as Guild;
            }

            return guild;
        }

        public static Party GetParty(Mobile m)
        {
            Party party = Party.Get(m);

            if (party == null && m is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)m;
                m = bc.ControlMaster;

                if (m != null)
                    party = Party.Get(m);

                m = bc.SummonMaster;

                if (m != null && party == null)
                    party = Party.Get(m);
            }

            return party;
        }
        #endregion
    }
}
