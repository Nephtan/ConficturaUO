using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;

//using Server.Health;  //<------Added for Health System

namespace Server.Mobiles
{
    public abstract class BaseSpecialCreature : BaseCreature
    {
        //BEGIN MOBILE FEATURES
        private bool e_AntiMarksmanship = false;
        private bool e_AntiEscape = false;
        private bool e_FireAreaAttack = false;
        private bool e_WaterAreaAttack = false;
        private bool e_AirAreaAttack = false;
        private bool e_RobotRevealer = false;
        private bool e_HumanRevealer = false;
        private bool e_Bomber = false;
        private bool e_MassPeace = false;
        private bool e_MassProvoke = false;
        private bool e_DrainStam = false;
        private bool e_DrainMana = false;
        private bool e_DrainHits = false;
        private bool e_TakesDrugs = false;
        private bool e_Displacer = false;

        //END MOBILE FEATURES
        ////////////////////////////////////////////////////////
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_AntiMarksmanship
        {
            get { return e_AntiMarksmanship; }
            set { e_AntiMarksmanship = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_AntiEscape
        {
            get { return e_AntiEscape; }
            set { e_AntiEscape = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_FireAreaAttack
        {
            get { return e_FireAreaAttack; }
            set { e_FireAreaAttack = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_WaterAreaAttack
        {
            get { return e_WaterAreaAttack; }
            set { e_WaterAreaAttack = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_AirAreaAttack
        {
            get { return e_AirAreaAttack; }
            set { e_AirAreaAttack = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_RobotRevealer
        {
            get { return e_RobotRevealer; }
            set { e_RobotRevealer = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_HumanRevealer
        {
            get { return e_HumanRevealer; }
            set { e_HumanRevealer = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_Bomber
        {
            get { return e_Bomber; }
            set { e_Bomber = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_MassPeace
        {
            get { return e_MassPeace; }
            set { e_MassPeace = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_MassProvoke
        {
            get { return e_MassProvoke; }
            set { e_MassProvoke = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_DrainStam
        {
            get { return e_DrainStam; }
            set { e_DrainStam = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_DrainMana
        {
            get { return e_DrainMana; }
            set { e_DrainMana = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_DrainHits
        {
            get { return e_DrainHits; }
            set { e_DrainHits = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_TakesDrugs
        {
            get { return e_TakesDrugs; }
            set { e_TakesDrugs = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool MF_Displacer
        {
            get { return e_Displacer; }
            set { e_Displacer = value; }
        }

        ////////////////////////////////////////////////////////
        public virtual bool DoesTeleporting
        {
            get { return false; }
        }
        public virtual double TeleportingChance
        {
            get { return 0.625; }
        }

        public virtual bool DoesNoxStriking
        {
            get { return false; }
        }
        public virtual double NoxStrikingChance
        {
            get { return 0.250; }
        }

        public virtual bool DoesLifeDraining
        {
            get { return false; }
        }
        public virtual double LifeDrainingChance
        {
            get
            {
                if (Hits < (HitsMax / Utility.RandomMinMax(1, 3)))
                    return 0.125;

                return 0.0;
            }
        }

        public virtual bool DoesTripleBolting
        {
            get { return false; }
        }
        public virtual double TripleBoltingChance
        {
            get { return 0.250; }
        }

        public virtual bool DoesMultiFirebreathing
        {
            get { return false; }
        }
        public virtual double MultiFirebreathingChance
        {
            get { return 1.000; }
        }
        public virtual int BreathDamagePercent
        {
            get { return 100; }
        }
        public virtual int BreathMaxTargets
        {
            get { return 5; }
        }
        public virtual int BreathMaxRange
        {
            get { return 5; }
        }
        public override bool HasBreath
        {
            get { return DoesMultiFirebreathing; }
        }

        public virtual bool DoesEarthquaking
        {
            get { return false; }
        }
        public virtual double EarthquakingChance
        {
            get { return 0.50; }
        }

        public virtual bool DoesSummoning
        {
            get { return false; }
        }
        public virtual double SummoningChance
        {
            get { return 0.150; }
        }
        public virtual double SummoningLowChance
        {
            get { return 0.050; }
        }
        public virtual Type SummoningType
        {
            get { return null; }
        }
        public virtual TimeSpan SummoningDuration
        {
            get { return TimeSpan.FromMinutes(2.0); }
        }
        public virtual TimeSpan SummoningDelay
        {
            get { return TimeSpan.FromMinutes(2.0); }
        }
        public virtual int SummoningSound
        {
            get { return -1; }
        }
        public virtual int SummoningMin
        {
            get { return 1; }
        }
        public virtual int SummoningMax
        {
            get { return 1; }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            Mobile target = attacker;

            if (target is BaseCreature && ((BaseCreature)target).Controlled)
                target = ((BaseCreature)target).ControlMaster;

            if (target == null)
                target = attacker;

            if (DoesNoxStriking && NoxStrikingChance >= Utility.RandomDouble())
                NoxStrike(target);

            if (DoesLifeDraining && LifeDrainingChance >= Utility.RandomDouble())
                DrainLife();

            if (DoesTripleBolting && TripleBoltingChance >= Utility.RandomDouble())
                TripleBolt(target);

            if (DoesSummoning && SummoningChance >= Utility.RandomDouble())
                SummonMinions(target);

            if (MF_AntiMarksmanship)
            {
                if (0.5 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.DoAntiMarksmanship(this, attacker);
            }
            if (MF_MassProvoke)
            {
                if (0.7 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.DoMassProvoke(this, attacker);
            }
            if (MF_FireAreaAttack)
            {
                if (0.6 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.DoFireAreaAttack(this, attacker);
            }
            if (MF_WaterAreaAttack)
            {
                if (0.6 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.DoWaterAreaAttack(this, attacker);
            }
            if (MF_AirAreaAttack)
            {
                if (0.6 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.DoAirAreaAttack(this, attacker);
            }
            if (MF_Displacer)
            {
                if (0.8 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.DoDisplace(this, attacker);
            }
        }

        /* public override void OnGaveMeleeAttack( Mobile defender )
             {
             base.OnGaveMeleeAttack( defender );
 
             if( DoesLifeDraining && LifeDrainingChance >= Utility.RandomDouble() )
                 DrainLife();
 
             if( DoesEarthquaking && EarthquakingChance >= Utility.RandomDouble() )
                 Earthquake();
 
             }*/
        // start Special Attack Prototype by: enoch144
        // Bleed Attack 2% chance

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DoesLifeDraining && LifeDrainingChance >= Utility.RandomDouble())
                DrainLife();

            if (DoesEarthquaking && EarthquakingChance >= Utility.RandomDouble())
                Earthquake();
            //////Below here added
            double chanceofspecialmove = .02;
            double random = Utility.RandomDouble();
            if (chanceofspecialmove > random)
            {
                this.SendLocalizedMessage(1060159); // Your target is bleeding!
                defender.SendLocalizedMessage(1060160); // You are bleeding!

                defender.PlaySound(0x133);
                defender.FixedParticles(0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist);

                BeginBleed(defender, this);
                //<------Added for Health System
                // WoundType wnd = WoundType.Cut;
                //Condition wud = InfectionUtility.GetCondition(defender);
                // wud.Wounds[(int)wnd] = true;
                //<------Added for Health System
            }

            if (MF_MassPeace)
            {
                if (0.5 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.DoMassPeace(this, defender);
            }
            if (MF_FireAreaAttack)
            {
                if (0.5 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.DoFireAreaAttack(this, defender);
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);
            if (MF_RobotRevealer)
            {
                Server.Mobiles.MobileFeatures.DoRobotReveal(this);
            }
            if (MF_HumanRevealer)
            {
                Server.Mobiles.MobileFeatures.DoHumanReveal(this);
            }
            if (MF_DrainHits)
            {
                Server.Mobiles.MobileFeatures.DoHitsDrainAttack(this);
            }
            if (MF_DrainStam)
            {
                Server.Mobiles.MobileFeatures.DoStamDrainAttack(this);
            }
            if (MF_DrainMana)
            {
                Server.Mobiles.MobileFeatures.DoManaDrainAttack(this);
            }
            if (MF_TakesDrugs)
            {
                if (0.5 >= Utility.RandomDouble())
                    Server.Mobiles.MobileFeatures.ActStoned(this);
            }
        }

        private static Hashtable m_Table = new Hashtable();
        private static Hashtable m_BleedTable = new Hashtable();

        public static bool IsBleeding(Mobile m)
        {
            return m_BleedTable.Contains(m);
        }

        public static void BeginBleed(Mobile m, Mobile from)
        {
            Timer t = (Timer)m_BleedTable[m];

            if (t != null)
                t.Stop();

            t = new InternalBleedTimer(from, m);
            m_BleedTable[m] = t;

            t.Start();
        }

        public static void DoBleed(Mobile m, Mobile from, int level)
        {
            if (m.Alive)
            {
                int damage = Utility.RandomMinMax(level, level * 2);

                if (!m.Player)
                    damage *= 2;

                m.PlaySound(0x133);
                m.Damage(damage, from);

                Blood blood = new Blood();

                blood.ItemID = Utility.Random(0x122A, 5);

                blood.MoveToWorld(m.Location, m.Map);
            }
            else
            {
                EndBleed(m, false);
                //<------Added for Health System
                //WoundType nwd = WoundType.Cut;
                //Condition uwd = InfectionUtility.GetCondition(m);
                //uwd.Wounds[(int)nwd] = true;
                //<------Added for Health System
            }
        }

        public static void EndBleed(Mobile m, bool message)
        {
            Timer t = (Timer)m_BleedTable[m];

            if (t == null)
                return;

            t.Stop();
            m_Table.Remove(m);

            m.SendLocalizedMessage(1060167); // The bleeding wounds have healed, you are no longer bleeding!
        }

        private class InternalBleedTimer : Timer
        {
            private Mobile m_From;
            private Mobile m_Mobile;
            private int m_Count;

            public InternalBleedTimer(Mobile from, Mobile m)
                : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_From = from;
                m_Mobile = m;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                DoBleed(m_Mobile, m_From, 5 - m_Count);

                if (++m_Count == 5)
                    EndBleed(m_Mobile, true);
            }
        }

        // end Special Attack Prototype
        public override void OnDamagedBySpell(Mobile attacker)
        {
            base.OnDamagedBySpell(attacker);

            Mobile target = attacker;

            if (target is BaseCreature && ((BaseCreature)target).Controlled)
                target = ((BaseCreature)target).ControlMaster;

            if (target == null)
                target = attacker;

            if (DoesTripleBolting && TripleBoltingChance >= Utility.RandomDouble())
                TripleBolt(target);

            if (DoesSummoning && SummoningChance >= Utility.RandomDouble())
                SummonMinions(target);
        }

        #region Summoning

        private DateTime m_NextSummonTime = DateTime.Now;

        public virtual int SummonMinions(Mobile victim)
        {
            int minions = 0;

            if (Map == null || Map == Map.Internal || Map != victim.Map || SummoningType == null)
                return minions;

            if (m_NextSummonTime >= DateTime.Now && Utility.RandomDouble() > SummoningLowChance)
                return minions;

            #region Cantidad de Summons
            int min = SummoningMin;
            int max = SummoningMax;

            if (min > max)
            {
                int aux = min;
                max = min;
                min = max;
            }

            int amount = min;

            if (min != max)
                amount = Utility.RandomMinMax(min, max);

            if (amount < 1)
                amount = 1;
            #endregion

            for (int m = 0; m < amount; m++)
            {
                BaseCreature minion;

                try
                {
                    minion = (BaseCreature)Activator.CreateInstance(SummoningType);
                }
                catch
                {
                    continue;
                }

                int offset = Utility.Random(8) * 2;
                Point3D selectedOffset = victim.Location;

                for (int i = 0; i < m_Offsets.Length; i += 2)
                {
                    int x = X + m_Offsets[(offset + i) % m_Offsets.Length];
                    int y = Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

                    if (Map.CanSpawnMobile(x, y, Z))
                    {
                        selectedOffset = new Point3D(x, y, Z);
                        break;
                    }
                    else
                    {
                        int z = Map.GetAverageZ(x, y);

                        if (Map.CanSpawnMobile(x, y, z))
                        {
                            selectedOffset = new Point3D(x, y, z);
                            break;
                        }
                    }
                }

                BaseCreature.Summon(
                    minion,
                    false,
                    this,
                    selectedOffset,
                    SummoningSound,
                    SummoningDuration
                );
                minion.Combatant = victim;
                minions++;
            }

            /* NOTA:
             *
             * Cada 6 minions esperamos el doble de tiempo que el pautado,
             * esto logra que no se sumonee 28 summons y se tarde lo mismo
             * que cuando se summonean 3 de ellos. Eh lo.
             */

            m_NextSummonTime =
                DateTime.Now
                + TimeSpan.FromSeconds(SummoningDelay.TotalSeconds * (0.8 + (minions * 0.2)));

            return minions;
        }

        #endregion

        #region Earthquake

        public virtual void Earthquake()
        {
            if (Map == null)
                return;

            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in this.GetMobilesInRange(6))
            {
                if (m == this || !CanBeHarmful(m) || m.AccessLevel >= AccessLevel.Counselor)
                    continue;

                if (
                    m is BaseCreature
                    && (
                        ((BaseCreature)m).Controlled
                        || ((BaseCreature)m).Summoned
                        || ((BaseCreature)m).Team != this.Team
                    )
                )
                    targets.Add(m);
                else if (m.Player)
                    targets.Add(m);
            }

            PlaySound(0x2F3);

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = targets[i];

                double damage = m.Hits * 0.6;

                if (damage < 10.0)
                    damage = 10.0;
                else if (damage > 75.0)
                    damage = 75.0;

                DoHarmful(m);

                AOS.Damage(m, this, (int)damage, 100, 0, 0, 0, 0);

                if (m.Alive && m.Body.IsHuman && !m.Mounted)
                    m.Animate(20, 7, 1, true, false, 0); // take hit
            }
        }

        #endregion

        #region Multi Breath

        public override void BreathStart(Mobile target)
        {
            base.BreathStart(target); //tiramos un firebreath al objetivo original.

            if (!DoesMultiFirebreathing || Utility.RandomDouble() > MultiFirebreathingChance)
                return;

            List<Mobile> posibleTgts = new List<Mobile>();

            foreach (Mobile m in target.Map.GetMobilesInRange(target.Location, BreathMaxRange))
                if (
                    m != null
                    && !m.Deleted
                    && m != target
                    && m.Alive
                    && !m.IsDeadBondedPet
                    && (m.AccessLevel < AccessLevel.Counselor || CanSee(m))
                    && CanBeHarmful(m)
                    && (m.Player || (m is BaseCreature && ((BaseCreature)m).Controlled))
                )
                    posibleTgts.Add(m);

            int maxTgts = BreathMaxTargets - 1; //BreathMaxTargets - 1 + el primer firebreath que va al target original.
            int mt = 0;
            int maxAtt = 3;
            int at = 0;

            for (int i = 0; i < posibleTgts.Count && mt++ < maxTgts && at++ < maxAtt; i++)
            {
                int x = Utility.Random(posibleTgts.Count);
                Mobile t = posibleTgts[x];

                if (t == null || !CanBeHarmful(t))
                    return;

                BreathStallMovement();
                BreathPlayAngerSound();
                BreathPlayAngerAnimation();

                Direction = GetDirectionTo(t);

                Timer.DelayCall(
                    TimeSpan.FromSeconds(BreathEffectDelay),
                    new TimerStateCallback(BreathEffect_Callback),
                    t
                );

                posibleTgts.RemoveAt(x);
                at = 0;
            }
        }

        public override int BreathComputeDamage() //Que haga un toke menos de da�o,
        { //si va a meter 5 firebreaths....
            int fromBase = base.BreathComputeDamage(); //Adem�s, lo usa la Hydra que tiene 1.5Ks de hits, y el Abscess con sus 7Ks.

            if (DoesMultiFirebreathing)
                return (int)(fromBase * (BreathDamagePercent / 10));

            return fromBase;
        }

        #endregion

        #region Triple Energy Bolts

        private int Bolts = 0;

        public virtual void TripleBolt(Mobile to)
        {
            Bolts = 0;
            Timer.DelayCall(
                TimeSpan.FromSeconds(Utility.Random(3)),
                new TimerStateCallback(Bolt_Callback),
                to
            );
            Timer.DelayCall(
                TimeSpan.FromSeconds(Utility.Random(3)),
                new TimerStateCallback(Bolt_Callback),
                to
            );
            Timer.DelayCall(
                TimeSpan.FromSeconds(Utility.Random(3)),
                new TimerStateCallback(Bolt_Callback),
                to
            );
        }

        public virtual void Bolt_Callback(object state)
        {
            Mobile to = state as Mobile;

            if (to == null)
                return;

            DoHarmful(to);

            to.BoltEffect(0);

            int damage = Utility.RandomMinMax(23, 29);

            AOS.Damage(to, this, damage, false, 0, 0, 0, 0, 100);

            if (++Bolts == 3 && damage > 0)
                to.SendMessage("You get shocked and dazed!");
        }

        #endregion

        #region Teleport

        private Timer m_Timer;

        public static int[] m_Offsets = new int[]
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
            1
        };

        private class TeleportTimer : Timer
        {
            private BaseSpecialCreature m_Owner;

            public TeleportTimer(BaseSpecialCreature owner)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                if (m_Owner.Deleted)
                {
                    Stop();
                    return;
                }

                if (Utility.RandomDouble() > m_Owner.TeleportingChance)
                    return;

                Map map = m_Owner.Map;

                if (map == null)
                    return;

                List<Mobile> toTeleport = new List<Mobile>();

                foreach (Mobile m in m_Owner.Region.GetMobiles())
                    if (
                        m != m_Owner
                        && m.Player
                        && m_Owner.CanBeHarmful(m)
                        && m_Owner.CanSee(m)
                        && m.AccessLevel < AccessLevel.Counselor
                    )
                        toTeleport.Add(m);

                if (toTeleport.Count > 0)
                {
                    int offset = Utility.Random(8) * 2;

                    Point3D to = m_Owner.Location;

                    for (int i = 0; i < m_Offsets.Length; i += 2)
                    {
                        int x =
                            m_Owner.X
                            + BaseSpecialCreature.m_Offsets[
                                (offset + i) % BaseSpecialCreature.m_Offsets.Length
                            ];
                        int y =
                            m_Owner.Y
                            + BaseSpecialCreature.m_Offsets[
                                (offset + i + 1) % BaseSpecialCreature.m_Offsets.Length
                            ];

                        if (map.CanSpawnMobile(x, y, m_Owner.Z))
                        {
                            to = new Point3D(x, y, m_Owner.Z);
                            break;
                        }
                        else
                        {
                            int z = map.GetAverageZ(x, y);

                            if (map.CanSpawnMobile(x, y, z))
                            {
                                to = new Point3D(x, y, z);
                                break;
                            }
                        }
                    }

                    Mobile m = toTeleport[Utility.Random(toTeleport.Count)];

                    Point3D from = m.Location;

                    m.Location = to;

                    SpellHelper.Turn(m_Owner, m);
                    SpellHelper.Turn(m, m_Owner);

                    m.ProcessDelta();

                    Effects.SendLocationParticles(
                        EffectItem.Create(from, m.Map, EffectItem.DefaultDuration),
                        0x3728,
                        10,
                        10,
                        2023
                    );
                    Effects.SendLocationParticles(
                        EffectItem.Create(to, m.Map, EffectItem.DefaultDuration),
                        0x3728,
                        10,
                        10,
                        5023
                    );

                    m.PlaySound(0x1FE);

                    m_Owner.Combatant = m;
                }
            }
        }

        #endregion

        #region NoxStrike

        public virtual void NoxStrike(Mobile attacker)
        {
            /* Counterattack with Hit Poison Area
             * 20-25 damage, unresistable
             * Lethal poison, 100% of the time
             * Particle effect: Type: "2" From: "0x4061A107" To: "0x0" ItemId: "0x36BD" ItemIdName: "explosion" FromLocation: "(296 615, 17)" ToLocation: "(296 615, 17)" Speed: "1" Duration: "10" FixedDirection: "True" Explode: "False" Hue: "0xA6" RenderMode: "0x0" Effect: "0x1F78" ExplodeEffect: "0x1" ExplodeSound: "0x0" Serial: "0x4061A107" Layer: "255" Unknown: "0x0"
             * Doesn't work on provoked monsters
             */

            if (attacker is BaseCreature && ((BaseCreature)attacker).BardProvoked)
                return;

            Mobile target = null;

            if (attacker is BaseCreature)
            {
                Mobile m = ((BaseCreature)attacker).GetMaster();

                if (m != null)
                    target = m;
            }

            if (target == null || !target.InRange(this, 18))
                target = attacker;

            this.Animate(10, 4, 1, true, false, 0);

            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in target.GetMobilesInRange(8))
            {
                if (m == this || !CanBeHarmful(m) || m.AccessLevel >= AccessLevel.Counselor)
                    continue;

                if (
                    m is BaseCreature
                    && (
                        ((BaseCreature)m).Controlled
                        || ((BaseCreature)m).Summoned
                        || ((BaseCreature)m).Team != this.Team
                    )
                )
                    targets.Add(m);
                else if (m.Player && m.Alive)
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = targets[i];

                DoHarmful(m);

                AOS.Damage(m, this, Utility.RandomMinMax(20, 25), true, 0, 0, 0, 100, 0);

                m.FixedParticles(0x36BD, 1, 10, 0x1F78, 0xA6, 0, (EffectLayer)255);
                m.ApplyPoison(this, Poison.Lethal);
            }
        }

        #endregion

        #region DrainLife

        public virtual void DrainLife()
        {
            List<Mobile> list = new List<Mobile>();

            foreach (Mobile m in Region.GetMobiles())
            {
                if (
                    m == this
                    || !CanBeHarmful(m)
                    || !CanSee(m)
                    || m.AccessLevel >= AccessLevel.Counselor
                )
                    continue;

                if (
                    m is BaseCreature
                    && (
                        ((BaseCreature)m).Controlled
                        || ((BaseCreature)m).Summoned
                        || ((BaseCreature)m).Team != Team
                    )
                )
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                DoHarmful(m);

                m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
                m.PlaySound(0x231);

                m.SendMessage("You feel the life drain out of you!");

                int toDrain = Utility.RandomMinMax(10, 40);

                Hits += toDrain;
                m.Damage(toDrain, this);
            }
        }

        #endregion

        public BaseSpecialCreature(
            AIType ai,
            FightMode mode,
            int iRangePerception,
            int iRangeFight,
            double dActiveSpeed,
            double dPassiveSpeed
        )
            : base(ai, mode, iRangePerception, iRangeFight, dActiveSpeed, dPassiveSpeed)
        {
            if (DoesTeleporting)
            {
                m_Timer = new TeleportTimer(this);
                m_Timer.Start();
            }
        }

        public BaseSpecialCreature(Serial serial)
            : base(serial)
        {
            if (DoesTeleporting)
            {
                m_Timer = new TeleportTimer(this);
                m_Timer.Start();
            }
        }

        public virtual void OnActionCombat()
        {
            if (MF_Bomber)
            {
                BaseCreature mobile = this;
                Mobile player = this.Combatant;
                Server.Mobiles.MobileFeatures.DoBomber(mobile, player);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // Version 17
            writer.Write((bool)e_AntiEscape);
            writer.Write((bool)e_AirAreaAttack);
            writer.Write((bool)e_AntiMarksmanship);
            writer.Write((bool)e_Bomber);
            writer.Write((bool)e_Displacer);
            writer.Write((bool)e_DrainHits);
            writer.Write((bool)e_DrainMana);
            writer.Write((bool)e_DrainStam);
            writer.Write((bool)e_FireAreaAttack);
            writer.Write((bool)e_WaterAreaAttack);
            writer.Write((bool)e_RobotRevealer);
            writer.Write((bool)e_HumanRevealer);
            writer.Write((bool)e_TakesDrugs);
            writer.Write((bool)e_MassProvoke);
            writer.Write((bool)e_MassPeace);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            e_AntiEscape = reader.ReadBool();
            e_AirAreaAttack = reader.ReadBool();
            e_AntiMarksmanship = reader.ReadBool();
            e_Bomber = reader.ReadBool();
            e_Displacer = reader.ReadBool();
            e_DrainHits = reader.ReadBool();
            e_DrainMana = reader.ReadBool();
            e_DrainStam = reader.ReadBool();
            e_FireAreaAttack = reader.ReadBool();
            e_WaterAreaAttack = reader.ReadBool();
            e_RobotRevealer = reader.ReadBool();
            e_HumanRevealer = reader.ReadBool();
            e_TakesDrugs = reader.ReadBool();
            e_MassProvoke = reader.ReadBool();
            e_MassPeace = reader.ReadBool();
        }
    }
}
