using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Invasions
{
    public sealed class InvasionObjectiveItem : Item, IInvasionOwned
    {
        private int m_Session;
        private InvasionCityId m_City;
        private InvasionFaction m_Faction;
        private InvasionSide m_Side;
        private InvasionRole m_Role;
        private int m_ObjectiveIndex;

        public int InvasionSession { get { return m_Session; } }
        public InvasionCityId InvasionCity { get { return m_City; } }
        public InvasionFaction InvasionFaction { get { return m_Faction; } }
        public InvasionSide InvasionSide { get { return m_Side; } }
        public InvasionRole InvasionRole { get { return m_Role; } }
        public int InvasionObjectiveIndex { get { return m_ObjectiveIndex; } }

        [Constructable]
        public InvasionObjectiveItem()
            : this(0, InvasionCityId.Britain, InvasionFaction.None, InvasionSide.None, InvasionRole.RitualFocus, -1)
        {
        }

        public InvasionObjectiveItem(
            int session,
            InvasionCityId city,
            InvasionFaction faction,
            InvasionSide side,
            InvasionRole role,
            int objectiveIndex
        )
            : base(GetItemID(role))
        {
            m_Session = session;
            m_City = city;
            m_Faction = faction;
            m_Side = side;
            m_Role = role;
            m_ObjectiveIndex = objectiveIndex;
            Movable = false;
            Light = LightType.Circle300;
            Hue = GetHue(faction, side);
            Name = GetName(role);
        }

        public InvasionObjectiveItem(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || from.Deleted || !from.Alive)
                return;

            if (!from.InRange(GetWorldLocation(), 3) || from.Map != Map)
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            InvasionService.InteractWithObjective(from, this);
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            string status = InvasionService.GetObjectiveStatus(this);

            if (!String.IsNullOrEmpty(status))
                LabelTo(from, status);
        }

        private static int GetItemID(InvasionRole role)
        {
            switch (role)
            {
                case InvasionRole.RitualFocus:
                    return 0x1F2B;
                case InvasionRole.CampStandard:
                    return 0x15AE;
                case InvasionRole.CivicWard:
                    return 0x1F18;
                case InvasionRole.SupplyAnchor:
                    return 0x1B72;
                default:
                    return 0x1F14;
            }
        }

        private static string GetName(InvasionRole role)
        {
            switch (role)
            {
                case InvasionRole.RitualFocus:
                    return "a corruption ritual focus";
                case InvasionRole.CampStandard:
                    return "an invasion camp standard";
                case InvasionRole.CivicWard:
                    return "a civic ward";
                case InvasionRole.SupplyAnchor:
                    return "an occupation supply anchor";
                default:
                    return "an invasion objective";
            }
        }

        private static int GetHue(InvasionFaction faction, InvasionSide side)
        {
            if (side == InvasionSide.Defender)
                return 0x59;

            switch (faction)
            {
                case InvasionFaction.ClockworkDominion:
                    return 0x972;
                case InvasionFaction.BloodCourt:
                    return 0x21;
                case InvasionFaction.AbyssalLegion:
                    return 0x455;
                default:
                    return 0x497;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Session);
            writer.Write((int)m_City);
            writer.Write((int)m_Faction);
            writer.Write((int)m_Side);
            writer.Write((int)m_Role);
            writer.Write(m_ObjectiveIndex);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Session = reader.ReadInt();
            m_City = (InvasionCityId)reader.ReadInt();
            m_Faction = (InvasionFaction)reader.ReadInt();
            m_Side = (InvasionSide)reader.ReadInt();
            m_Role = (InvasionRole)reader.ReadInt();
            m_ObjectiveIndex = reader.ReadInt();
        }
    }

    public class InvasionCreature : BaseCreature, IInvasionOwned
    {
        private int m_Session;
        private InvasionCityId m_City;
        private InvasionFaction m_Faction;
        private InvasionSide m_Side;
        private InvasionRole m_Role;
        private int m_ObjectiveIndex;
        private int m_ParticipantScale;
        private DateTime m_NextFactionAbilityUtc;

        public int InvasionSession { get { return m_Session; } }
        public InvasionCityId InvasionCity { get { return m_City; } }
        public InvasionFaction InvasionFaction { get { return m_Faction; } }
        public InvasionSide InvasionSide { get { return m_Side; } }
        public InvasionRole InvasionRole { get { return m_Role; } }
        public int InvasionObjectiveIndex { get { return m_ObjectiveIndex; } }
        public int ParticipantScale { get { return m_ParticipantScale; } }

        [Constructable]
        public InvasionCreature()
            : this(0, InvasionCityId.Britain, InvasionFaction.ClockworkDominion, InvasionSide.Invader, InvasionRole.RankMelee, -1, 1)
        {
        }

        public InvasionCreature(
            int session,
            InvasionCityId city,
            InvasionFaction faction,
            InvasionSide side,
            InvasionRole role,
            int objectiveIndex,
            int participantScale
        )
            : base(GetAI(role), FightMode.Closest, 16, 1, 0.2, 0.4)
        {
            m_Session = session;
            m_City = city;
            m_Faction = faction;
            m_Side = side;
            m_Role = role;
            m_ObjectiveIndex = objectiveIndex;
            m_ParticipantScale = Math.Max(1, Math.Min(8, participantScale));
            Team = side == InvasionSide.Invader ? 70 : 71;
            ConfigureCreature();
        }

        public InvasionCreature(Serial serial)
            : base(serial)
        {
        }

        private static AIType GetAI(InvasionRole role)
        {
            if (role == InvasionRole.RankCaster || role == InvasionRole.Commander || role == InvasionRole.CivicWarden)
                return AIType.AI_Mage;

            if (role == InvasionRole.RankRanged)
                return AIType.AI_Archer;

            return AIType.AI_Melee;
        }

        private void ConfigureCreature()
        {
            if (m_Side == InvasionSide.Defender)
                Body = Utility.RandomBool() ? 0x190 : 0x191;
            else if (m_Faction == InvasionFaction.ClockworkDominion)
                Body = Utility.RandomList(752, 358);
            else if (m_Faction == InvasionFaction.AbyssalLegion)
                Body = Utility.RandomList(9, 10, 40);
            else
                Body = Utility.RandomBool() ? 0x190 : 0x191;

            Hue = GetFactionHue();
            SpeechHue = Utility.RandomTalkHue();
            Name = GetCreatureName();
            Title = GetCreatureTitle();

            int scale = Math.Max(1, m_ParticipantScale);

            switch (m_Role)
            {
                case InvasionRole.Lieutenant:
                case InvasionRole.AnchorCaptain:
                    SetStr(350 + (scale * 20), 400 + (scale * 20));
                    SetDex(110, 140);
                    SetInt(140, 190);
                    SetHits(650 + (scale * 80), 750 + (scale * 80));
                    SetDamage(14, 24);
                    Fame = 12000;
                    Karma = m_Side == InvasionSide.Invader ? -12000 : 12000;
                    break;
                case InvasionRole.Commander:
                case InvasionRole.CivicWarden:
                    SetStr(700 + (scale * 50), 800 + (scale * 50));
                    SetDex(140, 180);
                    SetInt(400, 520);
                    SetHits(2200 + (scale * 450), 2500 + (scale * 450));
                    SetDamage(20, 32);
                    Fame = 24000;
                    Karma = m_Side == InvasionSide.Invader ? -24000 : 24000;
                    break;
                default:
                    SetStr(220 + (scale * 10), 280 + (scale * 10));
                    SetDex(90, 130);
                    SetInt(90, 150);
                    SetHits(260 + (scale * 35), 340 + (scale * 35));
                    SetDamage(10, 18);
                    Fame = 5000;
                    Karma = m_Side == InvasionSide.Invader ? -5000 : 5000;
                    break;
            }

            SetResistance(ResistanceType.Physical, 35, 55);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 30, 50);

            if (m_Faction == InvasionFaction.ClockworkDominion && m_Side == InvasionSide.Invader)
            {
                SetResistance(ResistanceType.Physical, 55, 70);
                SetResistance(ResistanceType.Fire, 45, 60);
                SetResistance(ResistanceType.Poison, 90, 100);
            }
            else if (m_Faction == InvasionFaction.BloodCourt && m_Side == InvasionSide.Invader)
            {
                SetDex(130, 180);
                ActiveSpeed = 0.15;
            }
            else if (m_Faction == InvasionFaction.AbyssalLegion && m_Side == InvasionSide.Invader)
            {
                SetResistance(ResistanceType.Fire, 70, 85);
                SetResistance(ResistanceType.Poison, 55, 70);
            }
            SetSkill(SkillName.MagicResist, 80.0, 110.0);
            SetSkill(SkillName.Tactics, 80.0, 110.0);
            SetSkill(SkillName.FistFighting, 80.0, 110.0);
            SetSkill(SkillName.Swords, 80.0, 110.0);
            SetSkill(SkillName.Magery, 75.0, 110.0);
            SetSkill(SkillName.Psychology, 75.0, 110.0);

            if (m_Role == InvasionRole.RankRanged)
            {
                AddItem(new Bow());
                PackItem(new Arrow(Utility.RandomMinMax(40, 80)));
            }
            else if (m_Role != InvasionRole.RankCaster && m_Role != InvasionRole.Commander && m_Role != InvasionRole.CivicWarden)
            {
                AddItem(new Longsword());
            }
        }

        private int GetFactionHue()
        {
            if (m_Side == InvasionSide.Defender)
                return 0x83EA;

            switch (m_Faction)
            {
                case InvasionFaction.ClockworkDominion:
                    return 0x972;
                case InvasionFaction.BloodCourt:
                    return 0x847E;
                case InvasionFaction.AbyssalLegion:
                    return 0x83E9;
                default:
                    return 0;
            }
        }

        private string GetCreatureName()
        {
            if (m_Role == InvasionRole.CivicWarden)
                return InvasionCities.Get(m_City).Name + " civic warden";

            string faction;

            switch (m_Faction)
            {
                case InvasionFaction.ClockworkDominion:
                    faction = "clockwork";
                    break;
                case InvasionFaction.BloodCourt:
                    faction = "blood court";
                    break;
                case InvasionFaction.AbyssalLegion:
                    faction = "abyssal";
                    break;
                default:
                    faction = "invasion";
                    break;
            }

            switch (m_Role)
            {
                case InvasionRole.Lieutenant:
                    return faction + " lieutenant";
                case InvasionRole.Commander:
                    return faction + " commander";
                case InvasionRole.AnchorCaptain:
                    return faction + " supply captain";
                case InvasionRole.OccupationPatrol:
                    return faction + " occupier";
                case InvasionRole.RankRanged:
                    if (m_Faction == InvasionFaction.ClockworkDominion)
                        return "clockwork siege engine";
                    if (m_Faction == InvasionFaction.BloodCourt)
                        return "blood court skirmisher";
                    return "abyssal flame hurler";
                case InvasionRole.RankCaster:
                    if (m_Faction == InvasionFaction.ClockworkDominion)
                        return "clockwork repair artificer";
                    if (m_Faction == InvasionFaction.BloodCourt)
                        return "blood court hexer";
                    return "abyssal summoner";
                default:
                    return faction + " soldier";
            }
        }

        private string GetCreatureTitle()
        {
            if (m_Side == InvasionSide.Defender)
                return "the defender";

            return "the invader";
        }

        public override bool IsEnemy(Mobile mobile)
        {
            bool decision;

            if (InvasionService.TryResolveEnemy(this, mobile, out decision))
                return decision;

            return base.IsEnemy(mobile);
        }

        public void RaiseBossScale(int participantScale)
        {
            int scale = Math.Max(1, Math.Min(8, participantScale));

            if ((m_Role != InvasionRole.Commander && m_Role != InvasionRole.CivicWarden) || scale <= m_ParticipantScale)
                return;

            int increase = scale - m_ParticipantScale;
            m_ParticipantScale = scale;
            RawStr += 50 * increase;
            HitsMaxSeed += 450 * increase;
            Hits = Math.Min(HitsMax, Hits + (450 * increase));
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender == null || !defender.Alive || !InvasionService.CanHarm(this, defender))
                return;

            if (m_Side == InvasionSide.Invader && m_Faction == InvasionFaction.BloodCourt && Utility.RandomDouble() < 0.35)
            {
                int drained = Utility.RandomMinMax(8, 18);
                defender.Damage(drained, this);
                Hits = Math.Min(HitsMax, Hits + drained);
                defender.SendMessage(0x22, "The Blood Court steals a measure of your life.");
            }
            else if (m_Side == InvasionSide.Invader && m_Faction == InvasionFaction.AbyssalLegion && Utility.RandomDouble() < 0.25)
            {
                defender.ApplyPoison(this, Poison.Regular);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow < m_NextFactionAbilityUtc || !InvasionService.IsActiveOwned(this))
                return;

            if (m_Faction == InvasionFaction.ClockworkDominion && m_Role == InvasionRole.RankCaster)
            {
                RepairNearbyConstruct();
                m_NextFactionAbilityUtc = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
            else if (m_Faction == InvasionFaction.BloodCourt && m_Role == InvasionRole.RankCaster)
            {
                Mobile target = Combatant as Mobile;

                if (target != null && target.Alive && InRange(target, 10) && InvasionService.CanHarm(this, target))
                {
                    DoHarmful(target);
                    target.Stam = Math.Max(0, target.Stam - Utility.RandomMinMax(12, 24));
                    target.Mana = Math.Max(0, target.Mana - Utility.RandomMinMax(8, 18));
                    target.SendMessage(0x22, "A blood curse saps your strength.");
                }

                m_NextFactionAbilityUtc = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(7, 11));
            }
            else if (m_Faction == InvasionFaction.AbyssalLegion && m_Role == InvasionRole.RankCaster)
            {
                Mobile target = Combatant as Mobile;

                if (target != null && target.Alive && InRange(target, 12) && InvasionService.CanHarm(this, target))
                {
                    DoHarmful(target);
                    MovingParticles(target, 0x36D4, 5, 0, false, false, 0x489, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                    target.Damage(Utility.RandomMinMax(14, 26), this);
                    target.ApplyPoison(this, Poison.Regular);
                    InvasionService.TrySummonAbyssalSupport(this);
                }

                m_NextFactionAbilityUtc = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        private void RepairNearbyConstruct()
        {
            IPooledEnumerable eable = GetMobilesInRange(6);

            foreach (Mobile mobile in eable)
            {
                IInvasionOwned owned = mobile as IInvasionOwned;

                if (mobile == this
                    || mobile.Deleted
                    || !mobile.Alive
                    || owned == null
                    || owned.InvasionSession != m_Session
                    || owned.InvasionCity != m_City
                    || owned.InvasionSide != m_Side
                    || mobile.Hits >= mobile.HitsMax)
                {
                    continue;
                }

                mobile.Hits = Math.Min(mobile.HitsMax, mobile.Hits + Utility.RandomMinMax(18, 32));
                mobile.FixedEffect(0x376A, 9, 16);
                break;
            }

            eable.Free();
        }

        public override void OnDeath(Container corpse)
        {
            base.OnDeath(corpse);
            InvasionService.NotifyCreatureDeath(this);
        }

        public override void GenerateLoot()
        {
            if (m_Spawning)
                return;

            switch (m_Role)
            {
                case InvasionRole.Lieutenant:
                case InvasionRole.AnchorCaptain:
                    PackGold(1000, 2500);
                    PackMagicItems(2, 4, 0.75, 0.75);
                    break;
                case InvasionRole.Commander:
                case InvasionRole.CivicWarden:
                    PackGold(8000, 15000);
                    PackMagicItems(4, 5, 1.0, 1.0);
                    PackMagicItems(4, 5, 1.0, 1.0);

                    if (Utility.RandomDouble() < 0.10)
                        PackMagicItems(4, 5, 1.0, 1.0);
                    break;
                default:
                    PackGold(150, 400);
                    PackMagicItems(1, 3, 0.25, 0.20);
                    break;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Session);
            writer.Write((int)m_City);
            writer.Write((int)m_Faction);
            writer.Write((int)m_Side);
            writer.Write((int)m_Role);
            writer.Write(m_ObjectiveIndex);
            writer.Write(m_ParticipantScale);
            writer.Write(m_NextFactionAbilityUtc);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Session = reader.ReadInt();
            m_City = (InvasionCityId)reader.ReadInt();
            m_Faction = (InvasionFaction)reader.ReadInt();
            m_Side = (InvasionSide)reader.ReadInt();
            m_Role = (InvasionRole)reader.ReadInt();
            m_ObjectiveIndex = reader.ReadInt();
            m_ParticipantScale = reader.ReadInt();
            m_NextFactionAbilityUtc = reader.ReadDateTime();
        }
    }

    public sealed class InvasionBanker : Banker, IInvasionOwned
    {
        private int m_Session;
        private InvasionCityId m_City;
        private InvasionFaction m_Faction;

        public int InvasionSession { get { return m_Session; } }
        public InvasionCityId InvasionCity { get { return m_City; } }
        public InvasionFaction InvasionFaction { get { return m_Faction; } }
        public InvasionSide InvasionSide { get { return InvasionSide.Invader; } }
        public InvasionRole InvasionRole { get { return InvasionRole.Banker; } }
        public int InvasionObjectiveIndex { get { return -1; } }

        [Constructable]
        public InvasionBanker()
            : this(0, InvasionCityId.Britain, InvasionFaction.ClockworkDominion)
        {
        }

        public InvasionBanker(int session, InvasionCityId city, InvasionFaction faction)
        {
            m_Session = session;
            m_City = city;
            m_Faction = faction;
            Name = "occupation banker";
            Title = "the faction paymaster";
        }

        public InvasionBanker(Serial serial)
            : base(serial)
        {
        }

        public override bool CheckVendorAccess(Mobile from)
        {
            return InvasionService.CanUseOccupationService(from, m_City, m_Session);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!CheckVendorAccess(e.Mobile))
            {
                if (e.Mobile.InRange(Location, 12))
                {
                    Say("This treasury serves the occupation alone.");
                    e.Handled = true;
                }

                return;
            }

            InvasionService.EndGrace(e.Mobile);
            base.OnSpeech(e);
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (CheckVendorAccess(from))
                list.Add(new OpenBankEntry(from, this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (CheckVendorAccess(from))
                base.GetContextMenuEntries(from, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Session);
            writer.Write((int)m_City);
            writer.Write((int)m_Faction);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Session = reader.ReadInt();
            m_City = (InvasionCityId)reader.ReadInt();
            m_Faction = (InvasionFaction)reader.ReadInt();
        }
    }

    public sealed class InvasionHealer : Healer, IInvasionOwned
    {
        private int m_Session;
        private InvasionCityId m_City;
        private InvasionFaction m_Faction;

        public int InvasionSession { get { return m_Session; } }
        public InvasionCityId InvasionCity { get { return m_City; } }
        public InvasionFaction InvasionFaction { get { return m_Faction; } }
        public InvasionSide InvasionSide { get { return InvasionSide.Invader; } }
        public InvasionRole InvasionRole { get { return InvasionRole.Healer; } }
        public int InvasionObjectiveIndex { get { return -1; } }

        [Constructable]
        public InvasionHealer()
            : this(0, InvasionCityId.Britain, InvasionFaction.ClockworkDominion)
        {
        }

        public InvasionHealer(int session, InvasionCityId city, InvasionFaction faction)
        {
            m_Session = session;
            m_City = city;
            m_Faction = faction;
            Name = "occupation chirurgeon";
            Title = "the faction healer";
            Karma = -10000;
        }

        public InvasionHealer(Serial serial)
            : base(serial)
        {
        }

        public override bool CheckVendorAccess(Mobile from)
        {
            return InvasionService.CanUseOccupationService(from, m_City, m_Session);
        }

        public override bool CheckResurrect(Mobile mobile)
        {
            return CheckVendorAccess(mobile);
        }

        public override void OnMovement(Mobile mobile, Point3D oldLocation)
        {
            if (CheckVendorAccess(mobile))
                base.OnMovement(mobile, oldLocation);
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (CheckVendorAccess(from))
                base.AddCustomContextEntries(from, list);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (CheckVendorAccess(from))
                base.GetContextMenuEntries(from, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Session);
            writer.Write((int)m_City);
            writer.Write((int)m_Faction);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Session = reader.ReadInt();
            m_City = (InvasionCityId)reader.ReadInt();
            m_Faction = (InvasionFaction)reader.ReadInt();
        }
    }

    public sealed class InvasionOccupationVendor : Provisioner, IInvasionOwned
    {
        private int m_Session;
        private InvasionCityId m_City;
        private InvasionFaction m_Faction;
        private InvasionRole m_Role;

        public int InvasionSession { get { return m_Session; } }
        public InvasionCityId InvasionCity { get { return m_City; } }
        public InvasionFaction InvasionFaction { get { return m_Faction; } }
        public InvasionSide InvasionSide { get { return InvasionSide.Invader; } }
        public InvasionRole InvasionRole { get { return m_Role; } }
        public int InvasionObjectiveIndex { get { return -1; } }

        [Constructable]
        public InvasionOccupationVendor()
            : this(0, InvasionCityId.Britain, InvasionFaction.ClockworkDominion, InvasionRole.Provisioner)
        {
        }

        public InvasionOccupationVendor(int session, InvasionCityId city, InvasionFaction faction, InvasionRole role)
        {
            m_Session = session;
            m_City = city;
            m_Faction = faction;
            m_Role = role;
            Name = GetRoleName(role);
            Title = "of the occupation";
            Karma = -10000;
        }

        public InvasionOccupationVendor(Serial serial)
            : base(serial)
        {
        }

        public override bool CheckVendorAccess(Mobile from)
        {
            bool allowed = InvasionService.CanUseOccupationService(from, m_City, m_Session);

            if (allowed)
                InvasionService.EndGrace(from);

            return allowed;
        }

        private static string GetRoleName(InvasionRole role)
        {
            switch (role)
            {
                case InvasionRole.Fence:
                    return "occupation fence";
                case InvasionRole.Quartermaster:
                    return "ritual quartermaster";
                default:
                    return "occupation provisioner";
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Session);
            writer.Write((int)m_City);
            writer.Write((int)m_Faction);
            writer.Write((int)m_Role);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Session = reader.ReadInt();
            m_City = (InvasionCityId)reader.ReadInt();
            m_Faction = (InvasionFaction)reader.ReadInt();
            m_Role = (InvasionRole)reader.ReadInt();
        }
    }
}
