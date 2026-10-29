using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.SkillHandlers;
using Server.Targeting;
using Server.Custom.Confictura;
using Server.Custom.Confictura.Champions;

internal static class ArachnidOutbreakBardRegression
{
    private static int s_Checks;
    private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAILED: " + message);
        ++s_Checks;
    }
    private static void Near(double actual, double expected, string message)
    {
        Check(Math.Abs(actual - expected) < 1e-7, message + "; actual=" + actual + "; expected=" + expected);
    }

    // Deterministic bridge clock, not a real battle/AP acceptance result.
    private sealed class ClockHandler : ITurnBasedCombatHandler
    {
        public DateTime Now = DateTime.Now;
        public HashSet<Mobile> Actors = new HashSet<Mobile>();
        public bool Enabled { get { return true; } }
        public bool IsParticipant(Mobile mobile) { return Actors.Contains(mobile); }
        public TurnActionDecision BeginAction(TurnActionRequest request) { return TurnActionDecision.Allow(); }
        public TurnActionLease GetPendingActionLease(Mobile mobile) { return null; }
        public void CompleteAction(TurnActionLease lease, TurnActionResult result) { }
        public bool AuthorizeMutation(TurnMutationRequest request) { return true; }
        public TurnCombatantChangeMode DecideCombatantChange(TurnCombatantChangeRequest request) { return TurnCombatantChangeMode.Native; }
        public DateTime GetActorTime(Mobile mobile, DateTime wall) { return Now; }
        public TimeSpan GetActorInterval(Mobile mobile) { return TimeSpan.FromSeconds(5); }
        public void TargetFinished(Mobile mobile, Target target, bool invoked) { }
        public void EffectScheduled(Mobile mobile, TurnMutationKind kind, TimeSpan duration) { }
        public void EffectTimerChanged(Mobile mobile, TurnMutationKind kind, Timer timer) { }
        public void MobileRelocated(Mobile mobile, Point3D oldLocation, Map oldMap, bool forced) { }
        public void MobileDeleted(Mobile mobile) { }
        public void EmergencyDisable(string reason, Exception error) { throw new Exception(reason, error); }
    }

    private sealed class Encounter : IDisposable
    {
        public ArachnidOutbreakController Controller;
        public ArachnidOutbreakLedger Ledger;
        public List<Mobile> Foes;
        public PlayerMobile Bard, Tank, Other;
        public ArachnidOutbreakSpider Subject, Combat;
        public ClockHandler Clock = new ClockHandler();
        public Lute Instrument;

        public Encounter()
        {
            Controller = new ArachnidOutbreakController();
            ArachnidOutbreakState state = (ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State", Private).GetValue(Controller);
            state.SetEnabled(true);
            Point3D point = new Point3D(2514, 2088, 0);
            ArachnidOutbreakPlacement placement = new ArachnidOutbreakPlacement(point, new Point3D[] { point, point, point }, new Point3D[] { point });
            typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(Controller,
                new object[] { new ArachnidOutbreakSite("bard-component", 2514, 2088, false), placement });
            Foes = (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures", Private).GetValue(Controller);
            Ledger = (ArachnidOutbreakLedger)typeof(ArachnidOutbreakController).GetField("m_Ledger", Private).GetValue(Controller);
            Subject = (ArachnidOutbreakSpider)Foes[0];
            // Extra owned health is solely a component target; this is not a
            // legal roster, balance run, or site-approval fixture.
            Combat = Add(0);
            Bard = Player(2514); Tank = Player(2515); Other = Player(2516);
            Instrument = new Lute(); Bard.Backpack.AddItem(Instrument);
            foreach (SkillName skill in new SkillName[] { SkillName.Musicianship, SkillName.Peacemaking, SkillName.Discordance })
            {
                Bard.Skills[skill].Cap = 125;
                Bard.Skills[skill].Base = 125;
                Bard.Skills[skill].SetLockNoRelay(SkillLock.Locked);
            }
            Clock.Actors.Add(Bard); Clock.Actors.Add(Tank); Clock.Actors.Add(Other);
            foreach (Mobile enemy in Foes) Clock.Actors.Add(enemy);
            Check(TurnBasedCombatBridge.Register(Clock), "controlled actor clock registered");
        }
        private static PlayerMobile Player(int x)
        {
            PlayerMobile player = new PlayerMobile(); player.Player = true; player.Body = 0x190; player.RawStr = 100;
            player.Hits = player.HitsMax; player.AddItem(new Backpack()); player.MoveToWorld(new Point3D(x, 2088, 0), Map.Sosaria);
            return player;
        }
        public ArachnidOutbreakSpider Add(int stage)
        {
            ArachnidOutbreakSpider enemy = new ArachnidOutbreakSpider(Controller, stage, true);
            Foes.Add(enemy); enemy.MoveToWorld(new Point3D(2514, 2088, 0), Map.Sosaria);
            Clock.Actors.Add(enemy); return enemy;
        }
        public void Observe()
        {
            typeof(ArachnidOutbreakController).GetMethod("ObserveBards", Private).Invoke(Controller, new object[] { DateTime.UtcNow, true });
        }
        public void Advance(double seconds) { Clock.Now = Clock.Now.AddSeconds(seconds); }
        public void Fight() { Combat.Damage(1, Tank); }
        public void Peace(Mobile target, bool expected = true)
        {
            BaseCreature creature = target as BaseCreature;
            bool applied = false; int attempts = 0;
            do
            {
                Peacemaking.OnPickedInstrument(Bard, Instrument);
                if (Bard.Target == null) throw new Exception("Real peace cursor not created.");
                Bard.Target.Invoke(Bard, target); ++attempts;
                applied = creature != null && creature.BardPacified && creature.PacifyingBard == Bard;
            } while (expected && !applied && attempts < 20);
            Console.WriteLine("Peace setup: attempts=" + attempts + "; skill=" + Bard.Skills[SkillName.Peacemaking].Value
                + "; music=" + Bard.Skills[SkillName.Musicianship].Value + "; applied=" + applied);
            Check(applied == expected, "real peace outcome established within bounded actual skill rolls");
        }
        public void Discord(Mobile target, bool expected = true)
        {
            bool applied = false; int attempts = 0;
            do
            {
                Discordance.OnPickedInstrument(Bard, Instrument);
                if (Bard.Target == null) throw new Exception("Real discordance cursor not created.");
                Bard.Target.Invoke(Bard, target); ++attempts;
                applied = Discordance.GetBard(target) == Bard;
            } while (expected && !applied && attempts < 20);
            Console.WriteLine("Discord setup: attempts=" + attempts + "; skill=" + Bard.Skills[SkillName.Discordance].Value + "; applied=" + applied);
            Check(applied == expected, "real discordance outcome established within bounded actual skill rolls");
        }
        public void Dispose()
        {
            TurnBasedCombatBridge.Unregister(Clock);
            Controller.Delete(); Bard.Delete(); Tank.Delete(); Other.Delete();
            // Skill timers are stopped explicitly; no main timer thread runs.
            Hashtable table = (Hashtable)typeof(Discordance).GetField("m_Table", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            foreach (object info in table.Values)
            {
                Timer timer = (Timer)info.GetType().GetField("m_Timer").GetValue(info);
                if (timer != null) timer.Stop();
            }
            table.Clear();
        }
    }

    private static void SkillAndUnion()
    {
        using (Encounter e = new Encounter())
        {
            e.Fight(); e.Peace(e.Subject);
            Check(e.Subject.BardPacified && e.Subject.PacifyingBard == e.Bard, "actual successful peace retains caster");
            e.Discord(e.Subject);
            Check(Discordance.GetBard(e.Subject) == e.Bard, "actual successful discordance retains caster");
            ArachnidOutbreakSpider second = e.Add(0); e.Peace(second);
            e.Advance(5); e.Observe(); Near(e.Ledger.Points(0, e.Bard), .5, "multiple enemies and simultaneous effects share five seconds");
            for (int i = 0; i < 20; ++i) e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2));
            Near(e.Ledger.Points(0, e.Bard), .5, "refreshes without elapsed time add nothing");
            for (int i = 0; i < 20; ++i) e.Observe();
            Near(e.Ledger.Points(0, e.Bard), .5, "paused actor time adds nothing across wall callbacks");
            e.Advance(5); e.Fight(); Near(e.Ledger.Points(0, e.Bard), 1, "actual ongoing fight advances useful time once");
            for (int i = 0; i < 18; ++i) { e.Advance(5); e.Fight(); }
            Near(e.Ledger.Points(0, e.Bard), 10, "one hundred useful actor seconds qualify independently of bard damage");
            Check(e.Ledger.Qualified(0).Contains(e.Bard) && e.Ledger.Points(1, e.Bard) == 0, "qualification is stage specific");
            e.Ledger.Seal(0); e.Advance(5); e.Observe(); Near(e.Ledger.Points(0, e.Bard), 10, "sealed stage rejects late effects");
        }
    }

    private static void Boundaries()
    {
        using (Encounter e = new Encounter())
        {
            e.Peace(e.Subject);
            for (int i = 0; i < 6; ++i) { e.Advance(5); e.Observe(); }
            Near(e.Ledger.Points(0, e.Bard), 0, "calming idle enemies never manufactures an engagement");
            DreadSpider unrelated = new DreadSpider(); unrelated.Damage(1, e.Tank); unrelated.Delete();
            e.Advance(5); e.Observe(); Near(e.Ledger.Points(0, e.Bard), 0, "unrelated combat does not start event credit");
            e.Fight(); e.Advance(3); e.Subject.Unpacify();
            Near(e.Ledger.Points(0, e.Bard), .3, "early break flushes only three effective seconds");
            e.Advance(2); e.Observe(); Near(e.Ledger.Points(0, e.Bard), .3, "ended peace has no lingering contribution");
            Check(e.Subject.PacifyingBard == null, "ended peace clears transient caster attribution");
            e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2));
            e.Advance(5); e.Subject.Pacify(e.Other, DateTime.Now.AddMinutes(2));
            Near(e.Ledger.Points(0, e.Bard), .8, "replacement preserves old caster's elapsed interval");
            e.Advance(5); e.Fight(); Near(e.Ledger.Points(0, e.Other), .5, "replacement caster starts at replacement time");
            e.Subject.Unpacify(); e.Subject.Controlled = true; e.Subject.ControlMaster = e.Tank;
            e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2)); e.Advance(5); e.Fight();
            Near(e.Ledger.Points(0, e.Bard), .8, "controlled enemy cannot manufacture support credit");
            e.Subject.Controlled = false; e.Subject.Summoned = true; e.Subject.SummonMaster = e.Tank;
            e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2)); e.Advance(5); e.Fight();
            Near(e.Ledger.Points(0, e.Bard), .8, "summoned enemy cannot manufacture support credit");
        }
        using (Encounter e = new Encounter())
        {
            e.Fight(); e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2));
            e.Advance(60); e.Observe(); Near(e.Ledger.Points(0, e.Bard), 0, "stopped observer and stale fight grant no retroactive time");
            e.Fight(); e.Advance(5); e.Observe(); Near(e.Ledger.Points(0, e.Bard), .5, "new progress restarts observation without replay");
            e.Advance(-30); e.Observe(); Near(e.Ledger.Points(0, e.Bard), .5, "clock rewind grants no time");
            e.Fight(); e.Clock.Actors.Remove(e.Bard); e.Advance(5); e.Observe();
            Near(e.Ledger.Points(0, e.Bard), .5, "wall observer outside a turn-based battle cannot earn battle time");
            e.Clock.Actors.Add(e.Bard); e.Observe(); e.Advance(5); e.Fight();
            Near(e.Ledger.Points(0, e.Bard), 1, "joining resumes from new baseline");
            e.Clock.Actors.Remove(e.Subject); e.Observe(); e.Advance(5); e.Fight();
            Near(e.Ledger.Points(0, e.Bard), 1, "bard's battle time cannot accelerate a real-time enemy's effect");
            e.Clock.Actors.Add(e.Subject); e.Observe();
            e.Subject.BardEndTime = DateTime.Now.AddSeconds(-1); e.Advance(5); e.Fight();
            Near(e.Ledger.Points(0, e.Bard), 1, "expired effect earns no further time");
        }
    }

    private static void ProvocationAndFailure()
    {
        using (Encounter e = new Encounter())
        {
            e.Fight(); DreadSpider opponent = new DreadSpider(); opponent.MoveToWorld(e.Subject.Location, e.Subject.Map);
            try
            {
                e.Subject.Provoke(e.Bard, opponent, true); e.Advance(5); e.Observe();
                Near(e.Ledger.Points(0, e.Bard), .5, "useful provocation against a wild opponent earns control time");
                e.Subject.BardProvoked = false; e.Advance(5); e.Fight(); Near(e.Ledger.Points(0, e.Bard), .5, "ended provocation adds nothing");
                e.Subject.Provoke(e.Bard, e.Tank, false); e.Advance(5); e.Fight();
                Near(e.Ledger.Points(0, e.Bard), .5, "failed provocation and player targets add nothing");
            }
            finally { opponent.Delete(); }
        }
        using (Encounter e = new Encounter())
        {
            e.Bard.Skills[SkillName.Musicianship].Base = 0;
            e.Fight(); e.Peace(e.Subject, false); e.Discord(e.Subject, false); e.Advance(5); e.Observe();
            Check(!e.Subject.BardPacified && Discordance.GetBard(e.Subject) == null, "failed real skills create no bard effects");
            Near(e.Ledger.Points(0, e.Bard), 0, "failed real skills grant no support points");
        }
    }

    private static void IncomingCombat()
    {
        using (Encounter e = new Encounter())
        {
            e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2));
            e.Tank.Damage(1, e.Combat); e.Advance(5); e.Observe();
            Near(e.Ledger.Points(0, e.Bard), .5, "actual event harm establishes engagement without outgoing player damage");
            e.Advance(20); e.Observe();
            Check(e.Tank.ApplyPoison(e.Combat, Poison.Deadly) == ApplyPoisonResult.Poisoned, "actual event poison applies to participant");
            e.Advance(5); e.Observe();
            Near(e.Ledger.Points(0, e.Bard), 1, "actual event poison reestablishes useful control window");
            e.Controller.Stop(); e.Advance(5); e.Observe();
            Near(e.Ledger.Points(0, e.Bard), 1, "cleanup stops unearned observation without changing earned ledger points");
        }
    }

    private static void SoloAndPet()
    {
        using (Encounter e = new Encounter())
        {
            e.Combat.Damage(1, e.Bard); double damage = e.Ledger.Points(0, e.Bard);
            e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2)); e.Advance(5); e.Observe();
            Near(e.Ledger.Points(0, e.Bard), damage, "solo damage cannot count again as helping another participant");
            DreadSpider pet = new DreadSpider(); pet.Controlled = true; pet.ControlMaster = e.Tank;
            pet.MoveToWorld(e.Tank.Location, e.Tank.Map);
            try
            {
                pet.Damage(1, e.Combat); e.Advance(5); e.Observe();
                Near(e.Ledger.Points(0, e.Bard), damage + .5, "helping another participant's harmed pet earns support time");
            }
            finally { pet.Delete(); }
        }
        using (Encounter e = new Encounter())
        {
            BroodAntivenomEffect.Initialize();
            BroodAntivenom treatment = new BroodAntivenom(); e.Tank.Backpack.AddItem(treatment);
            string reason; Check(treatment.TryApply(e.Tank, e.Tank, out reason), "participant can prepare their own poison protection");
            int count; BroodAntivenomEffect effect = BroodAntivenomEffect.Find(e.Tank, out count);
            try
            {
                e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2));
                Check(e.Tank.ApplyPoison(e.Combat, Poison.Deadly) == ApplyPoisonResult.Immune, "actual self-prepared protection consumes event poison");
                e.Advance(5); e.Observe();
                Near(e.Ledger.Points(0, e.Bard), .5, "prevented event harm still makes another participant's control useful");
                Near(e.Ledger.Points(0, e.Tank), 0, "self protection grants no direct support points");
            }
            finally { if (effect != null) effect.Delete(); }
        }
    }

    private static void BattleIdentity()
    {
        using (Encounter e = new Encounter())
        {
            e.Fight(); e.Subject.Pacify(e.Bard, DateTime.Now.AddMinutes(2));
            Type type = typeof(TurnBasedCombatManager);
            FieldInfo instance = type.GetField("m_Instance", BindingFlags.Static | BindingFlags.NonPublic);
            object previous = instance.GetValue(null);
            // Exercise the real read-only participant/group lookup with synthetic
            // records. This neither initializes the manager nor proves AP flow.
            TurnBasedCombatManager manager = (TurnBasedCombatManager)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
            Dictionary<Mobile, TurnParticipant> participants = new Dictionary<Mobile, TurnParticipant>();
            TurnCombatGroup first = new TurnCombatGroup(); TurnCombatGroup second = new TurnCombatGroup();
            TurnParticipant bard = new TurnParticipant { Mobile = e.Bard, Group = second };
            participants.Add(e.Bard, bard);
            TurnParticipant enemy = new TurnParticipant { Mobile = e.Subject, Group = first };
            participants.Add(e.Subject, enemy);
            type.GetField("m_Participants", Private).SetValue(manager, participants);
            type.GetField("m_RuntimeEnabled", Private).SetValue(manager, true);
            instance.SetValue(null, manager);
            try
            {
                e.Observe(); e.Advance(5); e.Fight();
                Near(e.Ledger.Points(0, e.Bard), 0, "a different battle cannot advance this encounter's support time");
                bard.Group = first; e.Observe(); e.Advance(5); e.Fight();
                Near(e.Ledger.Points(0, e.Bard), .5, "same-battle lookup permits useful actor time");
                bard.Group = second; enemy.Group = second; e.Advance(5); e.Observe();
                Near(e.Ledger.Points(0, e.Bard), .5, "replacement battle resets even a small positive clock interval");
                participants.Remove(e.Subject); e.Observe(); e.Advance(5); e.Fight();
                Near(e.Ledger.Points(0, e.Bard), .5, "missing battle identity fails closed without retroactive credit");
            }
            finally { instance.SetValue(null, previous); }
        }
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly; Core.Expansion = Expansion.SA; Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Item>());
            typeof(World).GetField("m_Mobiles", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Mobile>());
            MapDefinitions.Configure(); RaceDefinitions.Configure(); PoisonImpl.Configure(); SkillCheck.Initialize(); ArachnidOutbreakController.Initialize();
            SkillAndUnion(); Boundaries(); ProvocationAndFailure(); IncomingCombat(); SoloAndPet(); BattleIdentity();
            Console.WriteLine("PASS outbreak bards: " + s_Checks + " production-method assertions; controlled actor clock; no saves or listeners."); return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
