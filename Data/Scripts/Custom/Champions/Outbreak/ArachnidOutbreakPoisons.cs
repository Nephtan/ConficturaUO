using System;
using System.Collections.Generic;

namespace Server.Custom.Confictura.Champions
{
    public sealed class ArachnidOutbreakPoisons
    {
        private sealed class Condition
        {
            public int Stage, Level;
            public Condition(int stage, int level) { Stage = stage; Level = level; }
        }
        private readonly Dictionary<Mobile, Condition> m_Conditions = new Dictionary<Mobile, Condition>();
        public void Clear() { m_Conditions.Clear(); }
        public void Forget(Mobile patient) { m_Conditions.Remove(patient); }
        public bool Observe(PoisonChangedEventArgs change, int incomingStage, Mobile healer, ArachnidOutbreakLedger ledger)
        {
            Condition previous;
            m_Conditions.TryGetValue(change.Mobile, out previous);
            m_Conditions.Remove(change.Mobile);
            if (change.Kind == PoisonChangeKind.Application && change.NewPoison != null && incomingStage >= 0 && incomingStage < 4)
            {
                if (m_Conditions.Count < 100000)
                    m_Conditions[change.Mobile] = new Condition(incomingStage, change.NewPoison.Level);
            }
            else if (change.Kind == PoisonChangeKind.Cure && change.OldPoison != null && change.NewPoison == null && previous != null
                && previous.Level == change.OldPoison.Level && !ledger.IsSealed(previous.Stage)
                && healer != null && !healer.Deleted && healer.Player && healer != change.Mobile)
            {
                ledger.CreditValidatedSupport(previous.Stage, healer, 2.0);
                return true;
            }
            return false;
        }
    }
}
