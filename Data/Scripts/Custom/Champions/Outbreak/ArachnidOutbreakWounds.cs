using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Custom.Confictura.Champions
{
    // Oldest injuries are restored first. Unrelated injury and every form of
    // restoration consume the same balance, preventing repeated healing credit.
    public sealed class ArachnidOutbreakWounds
    {
        private sealed class Injury
        {
            public int Stage;
            public int Amount;
            public Injury(int stage, int amount) { Stage = stage; Amount = amount; }
        }
        private readonly Dictionary<Mobile, List<Injury>> m_Injuries = new Dictionary<Mobile, List<Injury>>();

        public void Clear() { m_Injuries.Clear(); }
        public void Forget(Mobile patient) { m_Injuries.Remove(patient); }

        private static int Consume(List<Injury> injuries, int amount, int stage)
        {
            int restored = 0;
            for (int i = 0; i < injuries.Count && amount > 0; )
            {
                Injury injury = injuries[i];
                int used = Math.Min(amount, injury.Amount);
                if (injury.Stage == stage)
                    restored += used;
                injury.Amount -= used;
                amount -= used;
                if (injury.Amount == 0)
                    injuries.RemoveAt(i);
                else
                    ++i;
            }
            return restored;
        }

        public bool Observe(HealthChangedEventArgs change, int incomingStage, Mobile healer, ArachnidOutbreakLedger ledger)
        {
            Mobile patient = change.Mobile;
            if (patient == null || patient.Deleted || ledger == null)
                return false;
            if (!patient.Alive || patient.IsDeadBondedPet)
            {
                m_Injuries.Remove(patient);
                return false;
            }
            List<Injury> injuries;
            if (!m_Injuries.TryGetValue(patient, out injuries))
            {
                if (incomingStage < 0 || incomingStage > 3 || m_Injuries.Count >= 100000)
                    return false;
                injuries = new List<Injury>();
                m_Injuries.Add(patient, injuries);
            }
            int deficit = Math.Max(0, patient.HitsMax - change.OldHits);
            long total = 0;
            foreach (Injury injury in injuries)
                total += injury.Amount;
            // Reconcile changed maximum health and previously unobserved injury
            // conservatively, without awarding any points for the adjustment.
            if (total > deficit)
                Consume(injuries, (int)(total - deficit), -2);
            else if (total < deficit)
                injuries.Insert(0, new Injury(-1, deficit - (int)total));
            bool meaningful = false;
            if (change.NewHits < change.OldHits)
            {
                int stage = incomingStage >= 0 && incomingStage < 4 ? incomingStage : -1;
                int amount = change.OldHits - change.NewHits;
                if (injuries.Count > 0 && injuries[injuries.Count - 1].Stage == stage)
                    injuries[injuries.Count - 1].Amount += amount;
                else if (injuries.Count < 100000)
                    injuries.Add(new Injury(stage, amount));
                else
                    m_Injuries.Remove(patient); // Fail closed instead of writing an unloadable record.
            }
            else if (change.NewHits > change.OldHits)
            {
                int amount = change.NewHits - change.OldHits;
                int[] restored = new int[4];
                // Consume once, retaining the exact stage split for this heal.
                for (int i = 0; i < injuries.Count && amount > 0; )
                {
                    Injury injury = injuries[i];
                    int used = Math.Min(amount, injury.Amount);
                    if (injury.Stage >= 0 && injury.Stage < 4)
                        restored[injury.Stage] += used;
                    injury.Amount -= used;
                    amount -= used;
                    if (injury.Amount == 0) injuries.RemoveAt(i);
                    else ++i;
                }
                if (change.Kind == HealthChangeKind.Healing && healer != null && healer.Player && !healer.Deleted && healer != patient)
                    for (int stage = 0; stage < 4; ++stage)
                        if (restored[stage] > 0 && !ledger.IsSealed(stage))
                        {
                            ledger.CreditValidatedHealing(stage, healer, patient, restored[stage]);
                            meaningful = true;
                        }
            }
            if (injuries.Count == 0 || change.NewHits >= patient.HitsMax)
                m_Injuries.Remove(patient);
            return meaningful;
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);
            writer.Write(m_Injuries.Count);
            foreach (KeyValuePair<Mobile, List<Injury>> entry in m_Injuries)
            {
                writer.Write(entry.Key);
                writer.Write(entry.Value.Count);
                foreach (Injury injury in entry.Value)
                {
                    writer.Write(injury.Stage);
                    writer.Write(injury.Amount);
                }
            }
        }
        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            int count = reader.ReadInt();
            if (version != 0 || count < 0 || count > 100000)
                throw new InvalidDataException("Invalid outbreak wound data.");
            m_Injuries.Clear();
            for (int i = 0; i < count; ++i)
            {
                Mobile patient = reader.ReadMobile();
                int length = reader.ReadInt();
                if (length < 0 || length > 100000)
                    throw new InvalidDataException("Invalid outbreak injury count.");
                List<Injury> injuries = new List<Injury>();
                long total = 0;
                for (int j = 0; j < length; ++j)
                {
                    int stage = reader.ReadInt();
                    int amount = reader.ReadInt();
                    total += amount;
                    if (stage < -1 || stage > 3 || amount <= 0 || total > Int32.MaxValue)
                        throw new InvalidDataException("Invalid outbreak injury segment.");
                    injuries.Add(new Injury(stage, amount));
                }
                if (patient != null && injuries.Count > 0)
                    m_Injuries[patient] = injuries;
            }
        }
    }
}
