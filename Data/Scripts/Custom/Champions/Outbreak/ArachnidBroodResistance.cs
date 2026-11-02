using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public partial class ArachnidOutbreakController
    {
        private sealed class NestResistance
        {
            public int Patch, Wave, Remaining;
            public DateTime Due, Cooldown;
            public readonly List<DateTime> Kills = new List<DateTime>();
            public readonly Dictionary<Mobile,DateTime> Participants = new Dictionary<Mobile,DateTime>();
        }
        private readonly Dictionary<int,NestResistance> m_NestResistance = new Dictionary<int,NestResistance>();
        private readonly Dictionary<Mobile,Dictionary<Mobile,DateTime>> m_BroodAttackers = new Dictionary<Mobile,Dictionary<Mobile,DateTime>>();
        public int BroodWarnings { get; private set; }
        public int BroodResponseBirths { get; private set; }

        private void RecordInfestationAttacker(Mobile enemy, Mobile principal)
        {
            ArachnidInfestationCreature brood = enemy as ArachnidInfestationCreature;
            if (m_Infestation == null || brood == null || brood.EggSac || brood is ArachnidBroodReinforcement
                || principal == null || principal.Deleted || !principal.Player || principal.AccessLevel != AccessLevel.Player) return;
            Dictionary<Mobile,DateTime> attackers;
            if (!m_BroodAttackers.TryGetValue(enemy,out attackers))
            { attackers = new Dictionary<Mobile,DateTime>(); m_BroodAttackers.Add(enemy,attackers); }
            attackers[principal] = DateTime.UtcNow;
        }

        private void CancelNestResponse(int id) { m_NestResistance.Remove(id); }

        private void RecordInfestationKill(ArachnidInfestationCreature brood, ArachnidInfestationPatch patch, DateTime now)
        {
            Dictionary<Mobile,DateTime> attackers;
            bool attributed = m_BroodAttackers.TryGetValue(brood,out attackers);
            m_BroodAttackers.Remove(brood);
            if (!attributed || brood.EggSac || brood is ArachnidBroodReinforcement || patch.Retired || m_Infestation.Tier(patch) != 0) return;
            ArachnidInfestationPatch nest = null;
            int nearest = 97;
            foreach (NestResistance active in m_NestResistance.Values)
            {
                ArachnidInfestationPatch candidate = m_Infestation.Find(active.Patch);
                if (candidate == null || candidate.Retired || candidate.ResponseBirths >= 9 || m_Infestation.Tier(candidate) != 0) continue;
                int distance = Math.Max(Math.Abs(candidate.Center.X-patch.Center.X),Math.Abs(candidate.Center.Y-patch.Center.Y));
                if (distance < nearest) { nearest = distance; nest = candidate; }
            }
            bool shared = nest != null;
            // The cell index bounds local work independently of total territorial extent.
            for (int x = patch.Center.X-96; !shared && x <= patch.Center.X+96; x += 12)
                for (int y = patch.Center.Y-96; y <= patch.Center.Y+96; y += 12)
                {
                    ArachnidInfestationPatch candidate = m_Infestation.Cell(x,y);
                    if (candidate == null || candidate.Retired || !candidate.Established || candidate.ResponseBirths >= 9
                        || m_Infestation.Tier(candidate) != 0 || !ArachnidTerritoryPolicy.HasEggSac(candidate.Center)) continue;
                    int distance = Math.Max(Math.Abs(candidate.Center.X-patch.Center.X),Math.Abs(candidate.Center.Y-patch.Center.Y));
                    if (distance < nearest) { nearest = distance; nest = candidate; }
                }
            if (nest == null) return;
            NestResistance response;
            if (!m_NestResistance.TryGetValue(nest.Id,out response))
            { response = new NestResistance(); response.Patch = nest.Id; m_NestResistance.Add(nest.Id,response); }
            bool meaningful = false;
            foreach (KeyValuePair<Mobile,DateTime> attacker in attackers)
                if (!attacker.Key.Deleted && attacker.Key.Alive && now-attacker.Value <= TimeSpan.FromSeconds(120))
                { response.Participants[attacker.Key] = now; meaningful = true; }
            if (!meaningful) return;
            response.Kills.Add(now);
            response.Kills.RemoveAll(delegate(DateTime when) { return now-when > TimeSpan.FromSeconds(120); });
            int wave = nest.ResponseBirths < 2 ? 1 : nest.ResponseBirths < 5 ? 2 : 3;
            int threshold = wave == 1 ? 3 : wave == 2 ? 6 : 10;
            if (response.Remaining != 0 || now < response.Cooldown || response.Kills.Count < threshold) return;
            response.Wave = wave; response.Remaining = wave+1; response.Due = now.AddSeconds(LocalWarningSeconds); ++BroodWarnings;
            foreach (Mobile participant in response.Participants.Keys)
                if (!participant.Deleted && participant.Alive && participant.Map == Map.Sosaria && participant.InRange(nest.Center,96))
                    participant.SendMessage(0x35,"The nearby egg sacs stir. Stronger brood will defend this nest in {0} seconds. Withdraw to avoid them.",LocalWarningSeconds);
        }

        private void ServiceNestResponses(DateTime now)
        {
            foreach (NestResistance response in new List<NestResistance>(m_NestResistance.Values))
            {
                ArachnidInfestationPatch patch = m_Infestation.Find(response.Patch);
                if (patch == null || patch.Retired || patch.ResponseBirths >= 9 || m_Infestation.Tier(patch) != 0)
                { CancelNestResponse(response.Patch); continue; }
                response.Kills.RemoveAll(delegate(DateTime when) { return now-when > TimeSpan.FromSeconds(120); });
                List<Mobile> targets = new List<Mobile>();
                foreach (KeyValuePair<Mobile,DateTime> participant in response.Participants)
                    if (!participant.Key.Deleted && participant.Key.Alive && participant.Key.Map == Map.Sosaria
                        && participant.Key.InRange(patch.Center,48) && now-participant.Value <= TimeSpan.FromSeconds(120)) targets.Add(participant.Key);
                if (response.Remaining == 0)
                {
                    if (response.Kills.Count == 0 && now >= response.Cooldown) CancelNestResponse(response.Patch);
                    continue;
                }
                if (now < response.Due) continue;
                if (targets.Count == 0) { response.Remaining = 0; response.Cooldown = now.AddSeconds(60); continue; }
                Point3D point;
                if (!FindInfestationPoint(patch.Center,18,out point)) continue;
                bool clear = true;
                IPooledEnumerable nearby = Map.Sosaria.GetMobilesInRange(point,10);
                try { foreach (Mobile mobile in nearby) if (mobile.Player || mobile is BaseCreature && (((BaseCreature)mobile).Controlled || ((BaseCreature)mobile).Summoned)) { clear = false; break; } }
                finally { nearby.Free(); }
                if (!clear || !SpreadingEncounterScheduler.AdmitStructuralActor()) continue;
                ArachnidBroodReinforcement defender = new ArachnidBroodReinforcement(this,patch.Id,response.Wave,targets);
                defender.Home = patch.Center; defender.RangeHome = 18; defender.LastRelevant = now;
                m_Creatures.Add(defender); defender.MoveToWorld(point,Map.Sosaria); SpreadingEncounterScheduler.Track(defender);
                if (defender.AIObject != null) defender.AIObject.Activate();
                ++patch.ResponseBirths; ++BroodResponseBirths; --response.Remaining;
                if (response.Remaining == 0) response.Cooldown = now.AddSeconds(60);
            }
        }
    }

    public class ArachnidBroodReinforcement : ArachnidInfestationCreature
    {
        private readonly List<Mobile> m_Principals = new List<Mobile>();
        public ArachnidBroodReinforcement(ArachnidOutbreakController owner, int patch, int tier, List<Mobile> principals)
            : base(owner,patch,tier,false,false)
        { m_Principals.AddRange(principals); Name = "an arachnid nest defender"; }

        private bool Participant(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive || target.Map != Map || !target.InRange(Home,48)) return false;
            BaseCreature creature = target as BaseCreature;
            if (!target.Player && (creature == null || !creature.Controlled && !creature.Summoned
                && creature.ControlMaster == null && creature.SummonMaster == null)) return true;
            Mobile principal = target.Player ? target : target.GetDamageMaster(this);
            return principal != null && m_Principals.Contains(principal);
        }
        public override bool IsEnemy(Mobile target) { return Participant(target) && base.IsEnemy(target); }
        public override bool CanBeHarmful(Mobile target, bool message, bool ignoreOurBlessedness)
        { return Participant(target) && base.CanBeHarmful(target,message,ignoreOurBlessedness); }
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            Mobile principal = from == null ? null : from.Player ? from : from.GetDamageMaster(this);
            if (principal != null && principal.Player && !m_Principals.Contains(principal)) m_Principals.Add(principal);
            base.OnDamage(amount,from,willKill);
        }
        public override void OnThink()
        {
            if (Combatant != null && !Participant(Combatant)) { Combatant = null; FocusMob = null; }
            if (FocusMob != null && !Participant(FocusMob)) FocusMob = null;
            base.OnThink();
        }
        public override void GenerateLoot() { }
        public ArachnidBroodReinforcement(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); writer.Write(0); writer.Write(m_Principals.Count);
            foreach (Mobile principal in m_Principals) writer.Write(principal);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); int version = reader.ReadInt(); int count = reader.ReadInt();
            if (version != 0 || count < 0 || count > 10000) throw new InvalidOperationException("Invalid brood reinforcement participants.");
            for (int i = 0; i < count; ++i) { Mobile principal = reader.ReadMobile(); if (principal != null) m_Principals.Add(principal); }
        }
    }
}
