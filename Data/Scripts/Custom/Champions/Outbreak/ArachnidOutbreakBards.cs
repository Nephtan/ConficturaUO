using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.SkillHandlers;

namespace Server.Custom.Confictura.Champions
{
    // Only elapsed, useful time is retained here. Earned points live in the saved
    // ledger; skill effects and their observation windows do not survive reload.
    internal sealed class ArachnidOutbreakBards
    {
        private sealed class Watch
        {
            public DateTime Clock;
            public object Domain;
            public bool Active;
        }

        private static readonly object m_RealtimeClock = new object();
        private static readonly object m_BridgeClock = new object();

        private readonly Dictionary<Mobile, Dictionary<Mobile, DateTime>> m_Combat = new Dictionary<Mobile, Dictionary<Mobile, DateTime>>();
        private readonly Dictionary<Mobile, Watch[]> m_Watches = new Dictionary<Mobile, Watch[]>();

        private static DateTime Clock(Mobile mobile)
        {
            return TurnBasedCombatBridge.GetTime(mobile);
        }

        private static object ClockDomain(Mobile mobile)
        {
            if (!TurnBasedCombatBridge.IsParticipant(mobile))
                return m_RealtimeClock;
            TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;
            if (manager == null || !manager.Enabled)
                return m_BridgeClock;
            TurnParticipant actor = manager.GetParticipant(mobile);
            return actor == null ? null : actor.Group;
        }

        public void RecordCombat(Mobile enemy, Mobile participant)
        {
            if (participant == null || participant.Deleted || !participant.Player)
                return;
            Dictionary<Mobile, DateTime> participants;
            if (!m_Combat.TryGetValue(enemy, out participants))
            {
                participants = new Dictionary<Mobile, DateTime>();
                m_Combat.Add(enemy, participants);
            }
            participants[participant] = Clock(enemy);
        }

        public void Clear()
        {
            m_Combat.Clear();
            m_Watches.Clear();
        }

        public bool Observe(List<Mobile> enemies, Func<Mobile, int> stageOf, ArachnidOutbreakLedger ledger, DateTime wallTime, bool credit)
        {
            Dictionary<Mobile, bool[]> active = new Dictionary<Mobile, bool[]>();
            foreach (Mobile enemy in enemies)
            {
                int stage = stageOf(enemy);
                BaseCreature creature = enemy as BaseCreature;
                if (stage < 0 || !enemy.Alive || creature == null || creature.Controlled || creature.Summoned)
                    continue;
                Add(active, Discordance.GetBard(enemy), enemy, stage);
                if (creature.BardPacified && creature.BardEndTime > wallTime)
                    Add(active, creature.PacifyingBard, enemy, stage);
                BaseCreature opponent = creature.BardTarget as BaseCreature;
                if (creature.BardProvoked && creature.BardEndTime > wallTime && creature.Combatant == opponent
                    && opponent != null && !opponent.Deleted && opponent.Alive && !opponent.Player && !opponent.Controlled && !opponent.Summoned)
                    Add(active, creature.BardMaster, enemy, stage);
            }

            foreach (Mobile bard in active.Keys)
                if (!m_Watches.ContainsKey(bard))
                {
                    Watch[] watches = new Watch[4];
                    for (int stage = 0; stage < 4; ++stage)
                        watches[stage] = new Watch { Clock = Clock(bard), Domain = ClockDomain(bard) };
                    m_Watches.Add(bard, watches);
                }

            bool useful = false;
            foreach (KeyValuePair<Mobile, Watch[]> entry in m_Watches)
            {
                bool[] stages;
                active.TryGetValue(entry.Key, out stages);
                DateTime now = Clock(entry.Key);
                object domain = ClockDomain(entry.Key);
                for (int stage = 0; stage < 4; ++stage)
                {
                    Watch watch = entry.Value[stage];
                    bool current = stages != null && stages[stage] && HelpsAnother(entry.Key, stage, enemies, stageOf);
                    double elapsed = (now - watch.Clock).TotalSeconds;
                    // Do not manufacture credit over a stopped observer or a clock
                    // transition. All effects/enemies share this bard/stage clock.
                    if (credit && watch.Active && current && domain != null && Object.ReferenceEquals(watch.Domain, domain)
                        && elapsed > 0.0 && elapsed <= 10.0 && !ledger.IsSealed(stage))
                    {
                        ledger.CreditValidatedSupport(stage, entry.Key, elapsed / 10.0);
                        useful = true;
                    }
                    watch.Clock = now;
                    watch.Domain = domain;
                    watch.Active = current;
                }
            }
            return useful;
        }

        private bool HelpsAnother(Mobile bard, int stage, List<Mobile> enemies, Func<Mobile, int> stageOf)
        {
            foreach (Mobile enemy in enemies)
            {
                Dictionary<Mobile, DateTime> participants;
                if (!enemy.Alive || stageOf(enemy) != stage || !m_Combat.TryGetValue(enemy, out participants))
                    continue;
                DateTime now = Clock(enemy);
                foreach (KeyValuePair<Mobile, DateTime> entry in participants)
                {
                    double elapsed = (now - entry.Value).TotalSeconds;
                    if (entry.Key != bard && !entry.Key.Deleted && entry.Key.Player && elapsed >= 0.0 && elapsed <= 15.0)
                        return true;
                }
            }
            return false;
        }

        private static void Add(Dictionary<Mobile, bool[]> active, Mobile bard, Mobile enemy, int stage)
        {
            if (bard == null || bard.Deleted || !bard.Player)
                return;
            bool turnBased = TurnBasedCombatBridge.IsParticipant(enemy);
            if (turnBased != TurnBasedCombatBridge.IsParticipant(bard))
                return;
            TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;
            if (turnBased && manager != null && manager.Enabled)
            {
                TurnParticipant enemyActor = manager.GetParticipant(enemy);
                TurnParticipant bardActor = manager.GetParticipant(bard);
                if (enemyActor == null || bardActor == null || enemyActor.Group == null || enemyActor.Group != bardActor.Group)
                    return;
            }
            bool[] stages;
            if (!active.TryGetValue(bard, out stages))
            {
                stages = new bool[4];
                active.Add(bard, stages);
            }
            stages[stage] = true;
        }
    }
}
