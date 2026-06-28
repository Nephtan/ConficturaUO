using System;
using System.Collections;
using Server;
using Server.Factions;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public class Taming
    {
        private static Hashtable m_BeingTamed = new Hashtable();

        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Taming].Callback = new SkillUseCallback(OnUse);
        }

        private static bool m_DisableMessage;

        public static bool DisableMessage
        {
            get { return m_DisableMessage; }
            set { m_DisableMessage = value; }
        }

        public static TimeSpan OnUse(Mobile m)
        {
            TamingLoopController.CancelForNewTarget(m);

            m.RevealingAction();

            m.Target = new InternalTarget();
            m.RevealingAction();

            if (!m_DisableMessage)
                m.SendLocalizedMessage(502789); // Tame which animal?

            return TimeSpan.FromHours(6.0);
        }

        internal static bool StartTamingFromLoop(Mobile from, BaseCreature creature)
        {
            if (!CanUseTamingSkill(from))
                return false;

            if (!CanTargetTamingCreature(from, creature))
                return false;

            bool started = TryStartTaming(from, creature);

            if (started)
                from.NextSkillTime = DateTime.Now + TimeSpan.FromHours(6.0);

            return started;
        }

        private static bool CanUseTamingSkill(Mobile from)
        {
            if (from == null)
                return false;

            if (!from.CheckAlive())
                return false;

            if (!from.Region.OnSkillUse(from, (int)SkillName.Taming))
                return false;

            if (!from.AllowSkillUse(SkillName.Taming))
                return false;

            if (from.NextSkillTime > DateTime.Now || from.Spell != null)
            {
                from.SendSkillMessage();
                return false;
            }

            from.DisruptiveAction();
            return true;
        }

        private static bool CanTargetTamingCreature(Mobile from, BaseCreature creature)
        {
            if (from == null || creature == null || creature.Deleted)
                return false;

            if (
                creature.Map == null
                || creature.Map != from.Map
                || !from.InRange(creature.Location, 2)
            )
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return false;
            }

            if (!from.CanSee(creature) || !from.InLOS(creature))
            {
                from.SendLocalizedMessage(500237); // Target can not be seen.
                return false;
            }

            return true;
        }

        private static bool TryStartTaming(Mobile from, object targeted)
        {
            if (from == null)
                return false;

            from.RevealingAction();

            if (targeted is Mobile)
            {
                if (targeted is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)targeted;

                    if (!creature.Tamable)
                    {
                        creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1049655,
                            from.NetState
                        ); // That creature cannot be tamed.
                    }
                    else if (creature.Controlled)
                    {
                        creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            502804,
                            from.NetState
                        ); // That animal looks tame already.
                    }
                    else if (from.Female && !creature.AllowFemaleTamer)
                    {
                        creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1049653,
                            from.NetState
                        ); // That creature can only be tamed by males.
                    }
                    else if (!from.Female && !creature.AllowMaleTamer)
                    {
                        creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1049652,
                            from.NetState
                        ); // That creature can only be tamed by females.
                    }
                    else if (from.Followers + creature.ControlSlots > from.FollowersMax)
                    {
                        from.SendLocalizedMessage(1049611); // You have too many followers to tame that creature.
                    }
                    else if (
                        creature.Owners.Count >= BaseCreature.MaxOwners
                        && !creature.Owners.Contains(from)
                    )
                    {
                        creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1005615,
                            from.NetState
                        ); // This animal has had too many owners and is too upset for you to tame.
                    }
                    else if (MustBeSubdued(creature))
                    {
                        creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1054025,
                            from.NetState
                        ); // You must subdue this creature before you can tame it!
                    }
                    else if (
                        CheckMastery(from, creature)
                        || from.Skills[SkillName.Taming].Value >= creature.MinTameSkill
                    )
                    {
                        FactionWarHorse warHorse = creature as FactionWarHorse;

                        if (warHorse != null)
                        {
                            Faction faction = Faction.Find(from);

                            if (faction == null || faction != warHorse.Faction)
                            {
                                creature.PrivateOverheadMessage(
                                    MessageType.Regular,
                                    0x3B2,
                                    1042590,
                                    from.NetState
                                ); // You cannot tame this creature.
                                return false;
                            }
                        }

                        if (m_BeingTamed.Contains(targeted))
                        {
                            creature.PrivateOverheadMessage(
                                MessageType.Regular,
                                0x3B2,
                                502802,
                                from.NetState
                            ); // Someone else is already taming this.
                        }
                        else if (creature.CanAngerOnTame && 0.95 >= Utility.RandomDouble())
                        {
                            creature.PrivateOverheadMessage(
                                MessageType.Regular,
                                0x3B2,
                                502805,
                                from.NetState
                            ); // You seem to anger the beast!
                            creature.PlaySound(creature.GetAngerSound());
                            creature.Direction = creature.GetDirectionTo(from);

                            if (creature.BardPacified && Utility.RandomDouble() > .24)
                            {
                                Timer.DelayCall(
                                    TimeSpan.FromSeconds(2.0),
                                    new TimerStateCallback(ResetPacify),
                                    creature
                                );
                            }
                            else
                            {
                                creature.BardEndTime = DateTime.Now;
                            }

                            creature.BardPacified = false;

                            creature.Move(creature.Direction);

                            if (from is PlayerMobile)
                                creature.Combatant = from;
                        }
                        else
                        {
                            m_BeingTamed[targeted] = from;

                            from.LocalOverheadMessage(MessageType.Emote, 0x59, 1010597); // You start to tame the creature.
                            from.NonlocalOverheadMessage(MessageType.Emote, 0x59, 1010598); // *begins taming a creature.*

                            new InternalTarget.InternalTimer(
                                from,
                                creature,
                                Utility.Random(3, 2)
                            ).Start();

                            return true;
                        }
                    }
                    else
                    {
                        creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            502806,
                            from.NetState
                        ); // You have no chance of taming this creature.
                    }
                }
                else
                {
                    ((Mobile)targeted).PrivateOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        502469,
                        from.NetState
                    ); // That being cannot be tamed.
                }
            }
            else
            {
                from.SendLocalizedMessage(502801); // You can't tame that!
            }

            return false;
        }

        private static void ResetPacify(object obj)
        {
            if (obj is BaseCreature)
            {
                ((BaseCreature)obj).BardPacified = true;
            }
        }

        public static bool CheckMastery(Mobile tamer, BaseCreature creature)
        {
            BaseCreature familiar = (BaseCreature)
                Spells.Necromancy.SummonFamiliarSpell.Table[tamer];

            if (familiar != null && !familiar.Deleted && familiar is DarkWolfFamiliar)
            {
                if (
                    creature is Worg
                    || creature is WolfDire
                    || creature is DireWolf
                    || creature is GreyWolf
                    || creature is TimberWolf
                    || creature is WhiteWolf
                    || creature is MysticalFox
                )
                    return true;
            }

            return false;
        }

        public static bool MustBeSubdued(BaseCreature bc)
        {
            if (bc.Owners.Count > 0)
            {
                return false;
            } //Checks to see if the animal has been tamed before
            return bc.SubdueBeforeTame && (bc.Hits > (bc.HitsMax / 10));
        }

        public static void ScaleStats(BaseCreature bc, double scalar)
        {
            if (bc.RawStr > 0)
                bc.RawStr = (int)Math.Max(1, bc.RawStr * scalar);

            if (bc.RawDex > 0)
                bc.RawDex = (int)Math.Max(1, bc.RawDex * scalar);

            if (bc.RawInt > 0)
                bc.RawInt = (int)Math.Max(1, bc.RawInt * scalar);

            if (bc.HitsMaxSeed > 0)
            {
                bc.HitsMaxSeed = (int)Math.Max(1, bc.HitsMaxSeed * scalar);
                bc.Hits = bc.Hits;
            }

            if (bc.StamMaxSeed > 0)
            {
                bc.StamMaxSeed = (int)Math.Max(1, bc.StamMaxSeed * scalar);
                bc.Stam = bc.Stam;
            }
        }

        public static void ScaleSkills(BaseCreature bc, double scalar)
        {
            ScaleSkills(bc, scalar, scalar);
        }

        public static void ScaleSkills(BaseCreature bc, double scalar, double capScalar)
        {
            for (int i = 0; i < bc.Skills.Length; ++i)
            {
                bc.Skills[i].Base *= scalar;

                bc.Skills[i].Cap = Math.Max(100.0, bc.Skills[i].Cap * capScalar);

                if (bc.Skills[i].Base > bc.Skills[i].Cap)
                {
                    bc.Skills[i].Cap = bc.Skills[i].Base;
                }
            }
        }

        private class InternalTarget : Target
        {
            private bool m_SetSkillTime = true;

            public InternalTarget()
                : base(2, false, TargetFlags.None) { }

            protected override void OnTargetFinish(Mobile from)
            {
                if (m_SetSkillTime)
                    from.NextSkillTime = DateTime.Now;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (TryStartTaming(from, targeted))
                {
                    m_SetSkillTime = false;

                    BaseCreature creature = targeted as BaseCreature;

                    if (creature != null)
                        TamingLoopController.BeginLoop(from, creature);
                }
            }

            internal class InternalTimer : Timer
            {
                private Mobile m_Tamer;
                private BaseCreature m_Creature;
                private int m_MaxCount;
                private int m_Count;
                private bool m_Paralyzed;
                private DateTime m_StartTime;

                public InternalTimer(Mobile tamer, BaseCreature creature, int count)
                    : base(TimeSpan.FromSeconds(3.0), TimeSpan.FromSeconds(3.0), count)
                {
                    m_Tamer = tamer;
                    m_Creature = creature;
                    m_MaxCount = count;
                    m_Paralyzed = creature.Paralyzed;
                    m_StartTime = DateTime.Now;
                    Priority = TimerPriority.TwoFiftyMS;
                }

                protected override void OnTick()
                {
                    m_Count++;

                    DamageEntry de = m_Creature.FindMostRecentDamageEntry(false);
                    bool alreadyOwned = m_Creature.Owners.Contains(m_Tamer);

                    if (!m_Tamer.InRange(m_Creature, 6))
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            502795,
                            m_Tamer.NetState
                        ); // You are too far away to continue taming.
                        TamingLoopController.StopLoop(m_Tamer);
                        Stop();
                    }
                    else if (!m_Tamer.CheckAlive())
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            502796,
                            m_Tamer.NetState
                        ); // You are dead, and cannot continue taming.
                        TamingLoopController.StopLoop(m_Tamer);
                        Stop();
                    }
                    else if (
                        !m_Tamer.CanSee(m_Creature)
                        || !m_Tamer.InLOS(m_Creature)
                        || !CanPath()
                    )
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1049654,
                            m_Tamer.NetState
                        ); // You do not have a clear path to the animal you are taming, and must cease your attempt.
                        TamingLoopController.StopLoop(m_Tamer);
                        Stop();
                    }
                    else if (!m_Creature.Tamable)
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1049655,
                            m_Tamer.NetState
                        ); // That creature cannot be tamed.
                        TamingLoopController.StopLoop(m_Tamer);
                        Stop();
                    }
                    else if (m_Creature.Controlled)
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            502804,
                            m_Tamer.NetState
                        ); // That animal looks tame already.
                        TamingLoopController.StopLoop(m_Tamer);
                        Stop();
                    }
                    else if (
                        m_Creature.Owners.Count >= BaseCreature.MaxOwners
                        && !m_Creature.Owners.Contains(m_Tamer)
                    )
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1005615,
                            m_Tamer.NetState
                        ); // This animal has had too many owners and is too upset for you to tame.
                        TamingLoopController.StopLoop(m_Tamer);
                        Stop();
                    }
                    else if (MustBeSubdued(m_Creature))
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1054025,
                            m_Tamer.NetState
                        ); // You must subdue this creature before you can tame it!
                        TamingLoopController.StopLoop(m_Tamer);
                        Stop();
                    }
                    else if (de != null && de.LastDamage > m_StartTime)
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Creature.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            502794,
                            m_Tamer.NetState
                        ); // The animal is too angry to continue taming.
                        TamingLoopController.StopLoop(m_Tamer);
                        Stop();
                    }
                    else if (m_Count < m_MaxCount)
                    {
                        m_Tamer.RevealingAction();

                        switch (Utility.Random(5))
                        {
                            case 0:
                                m_Tamer.PublicOverheadMessage(
                                    MessageType.Regular,
                                    0x3B2,
                                    false,
                                    "Easy...easy..."
                                );
                                break;
                            case 1:
                                m_Tamer.PublicOverheadMessage(
                                    MessageType.Regular,
                                    0x3B2,
                                    false,
                                    "Don't be afraid..."
                                );
                                break;
                            case 2:
                                m_Tamer.PublicOverheadMessage(
                                    MessageType.Regular,
                                    0x3B2,
                                    false,
                                    "I won't hurt you..."
                                );
                                break;
                            case 3:
                                m_Tamer.PublicOverheadMessage(
                                    MessageType.Regular,
                                    0x3B2,
                                    false,
                                    "See? Nothing to be afraid of..."
                                );
                                break;
                            case 4:
                                m_Tamer.PublicOverheadMessage(
                                    MessageType.Regular,
                                    0x3B2,
                                    false,
                                    "Nice and easy..."
                                );
                                break;
                        }

                        if (!alreadyOwned) // Passively check druidism for gain
                            m_Tamer.CheckTargetSkill(SkillName.Druidism, m_Creature, 0.0, 125.0);

                        if (m_Creature.Paralyzed)
                            m_Paralyzed = true;
                    }
                    else
                    {
                        m_Tamer.RevealingAction();
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_BeingTamed.Remove(m_Creature);

                        if (m_Creature.Paralyzed)
                            m_Paralyzed = true;

                        if (!alreadyOwned) // Passively check druidism for gain
                            m_Tamer.CheckTargetSkill(SkillName.Druidism, m_Creature, 0.0, 125.0);

                        double minSkill = m_Creature.MinTameSkill + (m_Creature.Owners.Count * 6.0);

                        if (minSkill > -24.9 && CheckMastery(m_Tamer, m_Creature))
                            minSkill = -24.9; // 50% at 0.0?

                        minSkill += 24.9;

                        double mod = m_Tamer.Skills[SkillName.Druidism].Value / 5;
                        bool tamed = false;

                        if (
                            CheckMastery(m_Tamer, m_Creature)
                            || alreadyOwned
                            || m_Tamer.CheckTargetSkill(
                                SkillName.Taming,
                                m_Creature,
                                (minSkill - 25.0 - mod),
                                (minSkill + 25.0)
                            )
                        )
                        {
                            tamed = true;

                            if (m_Creature.Owners.Count == 0) // First tame
                            {
                                if (m_Paralyzed)
                                    ScaleSkills(m_Creature, 0.86); // 86% of original skills if they were paralyzed during the taming
                                else
                                    ScaleSkills(m_Creature, 0.90); // 90% of original skills

                                if (m_Creature.StatLossAfterTame)
                                    ScaleStats(m_Creature, 0.50);
                            }

                            if (alreadyOwned)
                            {
                                m_Tamer.SendLocalizedMessage(502797); // That wasn't even challenging.
                            }
                            else
                            {
                                m_Creature.PrivateOverheadMessage(
                                    MessageType.Regular,
                                    0x3B2,
                                    502799,
                                    m_Tamer.NetState
                                ); // It seems to accept you as master.
                                m_Creature.Owners.Add(m_Tamer);
                            }

                            m_Creature.SetControlMaster(m_Tamer);
                            m_Creature.Fame = 0;
                            m_Creature.Karma = 0;
                            m_Creature.RangeHome = -1;
                            m_Creature.Home = new Point3D(0, 0, 0);
                            m_Creature.FightMode = FightMode.Aggressor;
                            m_Creature.IsBonded = false;

                            m_Creature.CanHearGhosts = false;
                            m_Creature.CantWalk = false;
                            m_Creature.Hidden = false;

                            PremiumSpawner.ActivateSpawner(m_Creature);
                        }
                        else
                        {
                            m_Creature.PrivateOverheadMessage(
                                MessageType.Regular,
                                0x3B2,
                                502798,
                                m_Tamer.NetState
                            ); // You fail to tame the creature.
                        }

                        TamingLoopController.OnTamingFinished(m_Tamer, m_Creature, !tamed);
                    }
                }

                private bool CanPath()
                {
                    IPoint3D p = m_Tamer as IPoint3D;

                    if (p == null)
                        return false;

                    if (m_Creature.InRange(new Point3D(p), 2))
                        return true;

                    MovementPath path = new MovementPath(m_Creature, new Point3D(p));
                    return path.Success;
                }
            }
        }
    }
}
