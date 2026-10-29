using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura;
using Server.Custom.Confictura.Champions;
using Server.Spells;
using Server.Spells.Chivalry;

internal static class ArachnidOutbreakWebRegression
{
    private static int s_Checks;
    private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static readonly BindingFlags Static = BindingFlags.Static | BindingFlags.NonPublic;
    private static void Check(bool condition, string message) { if (!condition) throw new Exception("FAILED: " + message); ++s_Checks; }
    private static object Get(object obj, string name) { return obj.GetType().GetField(name, Private).GetValue(obj); }
    private static void Set(object obj, string name, object value) { obj.GetType().GetField(name, Private).SetValue(obj, value); }
    private static void PrepareCatalog(string source)
    {
        string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        Check(String.Equals(Path.GetFullPath(Core.BaseDirectory).TrimEnd('\\'), Path.GetFullPath(root).TrimEnd('\\'), StringComparison.OrdinalIgnoreCase), "catalog writes confined to this disposable executable directory");
        string target = Path.Combine(root, "Data/TurnBasedCombat"); Directory.CreateDirectory(target);
        foreach (string name in new string[] { "TurnBasedCombat.cfg", "TurnActions.csv", "TurnEffects.csv", "TurnAI.csv" })
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine(source, name)); string destination = Path.Combine(target, name);
            if (!File.Exists(destination)) File.WriteAllBytes(destination, bytes);
            using (SHA256 hash = SHA256.Create())
            {
                string expected = BitConverter.ToString(hash.ComputeHash(bytes));
                Check(expected == BitConverter.ToString(hash.ComputeHash(File.ReadAllBytes(destination))), "private catalog matches checked-in " + name);
                Console.WriteLine("Catalog " + name + " SHA256=" + expected.Replace("-", ""));
            }
        }
    }
    private sealed class Encounter : IDisposable
    {
        public ArachnidOutbreakController Controller;
        public ArachnidOutbreakChampion Boss;
        public PlayerMobile Victim, Helper;
        public ArachnidOutbreakLedger Ledger;
        public TurnBasedCombatManager Manager;
        public TurnCombatGroup Group;
        public TurnParticipant BossActor, VictimActor, HelperActor;
        private object m_PreviousManager;
        private object m_PreviousSites;
        public Encounter()
        {
            Victim = Player(2515); Helper = Player(2516);
            Controller = new ArachnidOutbreakController();
            ArachnidOutbreakState state = (ArachnidOutbreakState)Get(Controller, "m_State"); state.SetEnabled(true);
            Point3D point = new Point3D(2514, 2088, 0);
            ArachnidOutbreakSite site = new ArachnidOutbreakSite("web-component", 2514, 2088, false);
            FieldInfo sites = typeof(ArachnidOutbreakSites).GetField("m_Candidates", Static); m_PreviousSites = sites.GetValue(null);
            sites.SetValue(null, Array.AsReadOnly(new ArachnidOutbreakSite[] { site }));
            typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(Controller, new object[] { site,
                new ArachnidOutbreakPlacement(point, new Point3D[] { new Point3D(2486,2068,0), new Point3D(2542,2068,0), new Point3D(2514,2118,0) }, new Point3D[] { point }) });
            for (int stage = 0; stage < 3; ++stage) state.CompleteBrood(stage, DateTime.UtcNow);
            Ledger = (ArachnidOutbreakLedger)Get(Controller, "m_Ledger");
            Boss = new ArachnidOutbreakChampion(Controller); Boss.Home = point; Boss.MoveToWorld(point, Map.Sosaria);
            ((List<Mobile>)Get(Controller, "m_Creatures")).Add(Boss);
            Boss.Combatant = Victim;
        }
        private static PlayerMobile Player(int x)
        {
            PlayerMobile player = new PlayerMobile(); player.Player = true; player.Body = 0x190;
            player.RawStr = 100; player.RawDex = 100; player.RawInt = 100; player.Hits = player.HitsMax; player.Stam = player.StamMax; player.Mana = player.ManaMax;
            player.AddItem(new Backpack()); player.MoveToWorld(new Point3D(x,2088,0), Map.Sosaria); return player;
        }
        public void Battle()
        {
            Manager = (TurnBasedCombatManager)Activator.CreateInstance(typeof(TurnBasedCombatManager), true);
            Manager.Configuration.LogEnabled = false;
            FieldInfo instance = typeof(TurnBasedCombatManager).GetField("m_Instance", Static); m_PreviousManager = instance.GetValue(null); instance.SetValue(null, Manager);
            Group = new TurnCombatGroup(); Group.ID = 1; Group.Round = 1;
            BossActor = Participant(Boss); VictimActor = Participant(Victim); HelperActor = Participant(Helper);
            BossActor.HasTakenFirstTurn = true; BossActor.TurnsStarted = 1; Group.Current = BossActor;
            Group.Edges.Add(new TurnHostilityEdge { Source = Boss, Target = Victim });
            Group.Edges.Add(new TurnHostilityEdge { Source = Boss, Target = Helper });
            ((List<TurnCombatGroup>)Get(Manager, "m_Groups")).Add(Group);
            Set(Manager, "m_RuntimeEnabled", true);
            Check(TurnBasedCombatBridge.Register(Manager), "real manager registered with private catalog and synthetic battle roster");
        }
        private TurnParticipant Participant(Mobile mobile)
        {
            TurnParticipant actor = new TurnParticipant { Mobile = mobile, Group = Group, ActionPoints = 20, LogicalTime = DateTime.Now,
                ActionTime = DateTime.Now, Effects = (TurnEffectState)typeof(TurnBasedCombatManager).GetMethod("CaptureEffects",Private).Invoke(Manager,new object[]{mobile}), EligibleRound = 1 };
            Group.Participants.Add(actor); ((Dictionary<Mobile, TurnParticipant>)Get(Manager, "m_Participants")).Add(mobile, actor); return actor;
        }
        public ArachnidWebWarning Warn()
        {
            Check(TryWarn(Victim), "boss starts an authorized web warning");
            return (ArachnidWebWarning)Get(Boss, "m_Web");
        }
        public bool TryWarn(Mobile target) { return (bool)typeof(ArachnidOutbreakChampion).GetMethod("TryStartWeb",Private).Invoke(Boss,new object[]{target}); }
        public bool Resolve(ArachnidWebWarning warning, bool bossPulse)
        {
            return (bool)typeof(ArachnidWebWarning).GetMethod("TryResolve", Private).Invoke(warning, new object[] { bossPulse });
        }
        public void Due(ArachnidWebWarning warning) { Set(warning, "m_Due", DateTime.UtcNow.AddMilliseconds(-1)); }
        public void Pay(Mobile actor, int cost, bool refund)
        {
            TurnActionDecision decision = TurnBasedCombatBridge.BeginAction(new TurnActionRequest(actor, null, this, TurnActionKind.ItemUse, cost, false, false));
            Check(decision.Allowed && decision.Lease != null, "real manager reserves test action");
            TurnBasedCombatBridge.CompleteAction(decision.Lease, new TurnActionResult(actor, null, refund ? TurnActionPhase.Refund : TurnActionPhase.Commit, !refund));
        }
        public void Act(Mobile actor, Mobile target, TurnActionKind kind, int cost, Action apply)
        {
            TurnActionDecision decision = TurnBasedCombatBridge.BeginAction(new TurnActionRequest(actor, target, this, kind, cost, kind == TurnActionKind.Attack, kind != TurnActionKind.Attack));
            Check(decision.Allowed && decision.Lease != null, "actual effect receives a legal paid action; " + decision.Message);
            using (IDisposable scope = TurnBasedCombatBridge.BeginActionScope(decision.Lease)) apply();
            TurnBasedCombatBridge.CompleteAction(decision.Lease, new TurnActionResult(actor,target,TurnActionPhase.Commit,true));
        }
        public void Dispose()
        {
            if (Manager != null)
            {
                Manager.Disable("isolated web fixture"); TurnBasedCombatBridge.Unregister(Manager);
                typeof(TurnBasedCombatManager).GetField("m_Instance", Static).SetValue(null, m_PreviousManager);
            }
            Controller.Delete(); Victim.Delete(); Helper.Delete();
            typeof(ArachnidOutbreakSites).GetField("m_Candidates", Static).SetValue(null, m_PreviousSites);
        }
    }
    private static Direction Step(Mobile mobile)
    {
        for (int i = 0; i < 8; ++i)
        {
            int z; Direction direction = (Direction)i;
            if (mobile.CheckMovement(direction, out z)) { mobile.Direction = direction; return direction; }
        }
        throw new Exception("No legal step in component terrain.");
    }
    private static void Realtime()
    {
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn();
            Check(warning.Visible && warning.Map == e.Victim.Map && warning.Location == e.Victim.Location && warning.Hue == 0x21, "visible ground warning marks original target tile");
            Check((TileData.ItemTable[warning.ItemID].Flags & TileFlag.Impassable) == 0, "web artwork does not impose geometric collision");
            Check(!e.Resolve(warning, false) && ArachnidWebRestraint.Find(e.Victim) == null, "warning cannot impact before three-second deadline");
            Point3D original = e.Victim.Location; Check(e.Victim.Move(Step(e.Victim)) && e.Victim.Location != original, "actual movement leaves warning tile");
            e.Due(warning); Check(!e.Resolve(warning, false) && warning.Deleted, "moving out avoids impact");
        }
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn(); e.Due(warning); Check(e.Resolve(warning, false), "due real-time warning applies restraint");
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim); Check(hold != null && !e.Victim.Frozen && !e.Victim.Paralyzed, "restraint uses neither freeze nor paralysis");
            Point3D original = e.Victim.Location; Check(!e.Victim.Move(Step(e.Victim)) && e.Victim.Location == original, "real movement event blocks walking");
            int hp = e.Boss.Hits; e.Boss.Damage(1,e.Victim); Check(e.Boss.Hits < hp, "held attacker can deal actual damage");
            e.Victim.Hits = 50; e.Victim.Heal(1,e.Victim,false); Check(e.Victim.Hits == 51, "held patient can receive actual healing");
            e.Victim.TithingPoints = 100;
            CloseWoundsSpell spell = new CloseWoundsSpell(e.Victim,null);
            Console.WriteLine("Cast gates: mix="+Spell.CantMixSpell(e.Victim,spell)+"; playercheck="+e.Victim.CheckSpellCast(spell)+"; spellcheck="+spell.CheckCast()+"; regioncheck="+e.Victim.Region.OnBeginSpellCast(e.Victim,spell)+"; elementalism="+e.Victim.Skills[SkillName.Elementalism].Value+"; magery="+e.Victim.Skills[SkillName.Magery].Value+"; peaced="+e.Victim.PeacedUntil.ToString("O"));
            bool cast = spell.Cast();
            Console.WriteLine("Held casting: accepted="+cast+"; mana="+e.Victim.Mana+"; max="+e.Victim.ManaMax+"; blessed="+e.Victim.Blessed+"; state="+spell.State+"; region="+e.Victim.Region.Name+"; frozen="+e.Victim.Frozen+"; paralyzed="+e.Victim.Paralyzed+"; next="+e.Victim.NextSpellTime.ToString("O"));
            Check(cast && e.Victim.Spell == spell, "held player can begin a real healing spell");
            spell.Disturb(DisturbType.NewCast,false,false);
            DateTime until = (DateTime)Get(hold,"m_Until"); Set(e.Boss,"m_NextWeb",DateTime.MinValue);
            Check(!(bool)typeof(ArachnidOutbreakChampion).GetMethod("TryStartWeb",Private).Invoke(e.Boss,new object[]{e.Victim})
                && (DateTime)Get(hold,"m_Until") == until, "repeat web cannot stack or extend active restraint");
            string reason; Check(hold.TryFree(e.Helper, out reason), "nearby helper tears actual web");
            Check(hold.Deleted && ArachnidWebRestraint.Find(e.Victim) == null && e.Ledger.Points(3,e.Helper) == 2, "one successful rescue grants two boss points");
            Check(!hold.TryFree(e.Helper, out reason) && e.Ledger.Points(3,e.Helper) == 2, "repeated rescue of resolved web grants nothing");
            Check(e.Victim.Move(Step(e.Victim)), "movement restored after rescue");
        }
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn(); e.Due(warning); e.Resolve(warning, false);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim); string reason;
            Check(hold.TryFree(e.Victim, out reason) && e.Ledger.Points(3,e.Victim) == 0, "self escape grants no support credit");
        }
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn(); e.Due(warning); e.Resolve(warning, false);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim);
            Set(hold,"m_Until",DateTime.UtcNow.AddMilliseconds(-1));
            Check(ArachnidWebRestraint.Find(e.Victim) == null && hold.Deleted, "four-second real-time expiry releases without assistance");
        }
    }
    private static void TurnBased()
    {
        using (Encounter e = new Encounter())
        {
            e.Victim.Hits = 50;
            e.Battle(); ArachnidWebWarning warning = e.Warn(); Check(e.BossActor.ActionPoints == 16, "warning spends four boss AP"); e.Due(warning);
            Check(!e.Resolve(warning,true), "elapsed wall time cannot replace reaction turn");
            Check(e.Manager.EndTurn(e.Boss,true) && e.Group.Current == e.VictimActor && e.VictimActor.TurnsStarted == 1, "real turn transition gives victim legal reaction opportunity");
            Check(!e.Resolve(warning,true), "impact waits until reaction turn finishes");
            e.Manager.EndTurn(e.Victim,true); e.Manager.EndTurn(e.Helper,true);
            Check(e.Group.Current == e.BossActor && !e.Resolve(warning,false), "wall callback cannot impact even during boss turn");
            Check(e.Resolve(warning,true) && e.BossActor.ActionPoints == 16, "authorized next boss pulse impacts for four AP");
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim); Check(hold != null, "turn-based victim held");
            string reason; Check(!hold.TryFree(e.Helper,out reason), "out-of-turn helper cannot free victim");
            e.Manager.EndTurn(e.Boss,true); Check(ArachnidWebRestraint.Find(e.Victim) == hold, "restraint survives until victim can act rather than expiring at turn start");
            Check(!e.Victim.Move(Step(e.Victim)), "turn-based walking is blocked");
            DateTime baseline = e.Manager.GetActionTime(e.Victim,DateTime.Now); int ap = e.VictimActor.ActionPoints;
            e.Pay(e.Victim,4,true); Check(e.VictimActor.ActionPoints == ap && e.Manager.GetActionTime(e.Victim,DateTime.Now) == baseline, "refunded action advances neither AP spending nor effect time");
            int beforeAttack = e.VictimActor.ActionPoints;
            TurnActionDecision attack = TurnBasedCombatBridge.BeginAction(new TurnActionRequest(e.Victim,e.Boss,e.Victim.Weapon,TurnActionKind.Attack,12,true,false));
            int paidAttack = beforeAttack - e.VictimActor.ActionPoints;
            Check(attack.Handled && paidAttack > 0 && ArachnidWebRestraint.Find(e.Victim) == hold, "native weapon attack executes through manager while held");
            e.Pay(e.Victim,12-paidAttack,false);
            Check(ArachnidWebRestraint.Find(e.Victim) == hold, "three committed action-seconds retain the web");
            e.Act(e.Victim,e.Victim,TurnActionKind.ItemUse,4,delegate
            {
                // Establish synthetic missing health after the normal start-turn
                // regeneration pass; this is not reward-provenance evidence.
                e.Victim.Hits = 50; e.Victim.Heal(1,e.Victim,false);
            });
            Check(e.Victim.Hits == 51 && ArachnidWebRestraint.Find(e.Victim) == null && hold.Deleted, "actual paid healing works and four action-seconds release in same turn; after="+e.Victim.Hits);
            Check(e.VictimActor.ActionPoints == 4 && e.Victim.Move(Step(e.Victim)), "remaining AP can move after natural release");
        }
        using (Encounter e = new Encounter())
        {
            e.Battle(); ArachnidWebWarning warning = e.Warn(); e.Due(warning);
            e.Manager.EndTurn(e.Boss,true); e.Manager.EndTurn(e.Victim,true); e.Manager.EndTurn(e.Helper,true); e.Resolve(warning,true);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim);
            e.Manager.EndTurn(e.Boss,true); int ap = e.VictimActor.ActionPoints;
            e.Victim.Use(hold);
            Check(hold.Deleted && e.VictimActor.ActionPoints == ap - 4 && e.Ledger.Points(3,e.Victim) == 0, "native Mobile.Use self escape spends four AP without support points");
        }
        using (Encounter e = new Encounter())
        {
            e.Battle(); ArachnidWebWarning warning = e.Warn(); e.Due(warning);
            e.Manager.EndTurn(e.Boss,true); e.Manager.EndTurn(e.Victim,true); e.Manager.EndTurn(e.Helper,true); e.Resolve(warning,true);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim);
            e.Manager.EndTurn(e.Boss,true); e.Manager.EndTurn(e.Victim,true);
            Check(ArachnidWebRestraint.Find(e.Victim) == null && hold.Deleted, "ending victim turn expires a short restraint without adding another turn");
        }
        using (Encounter e = new Encounter())
        {
            e.Battle(); e.Group.Participants.Clear(); e.Group.Participants.Add(e.BossActor); e.Group.Participants.Add(e.HelperActor); e.Group.Participants.Add(e.VictimActor);
            ArachnidWebWarning warning = e.Warn(); e.Due(warning);
            e.Manager.EndTurn(e.Boss,true); e.Manager.EndTurn(e.Helper,true); e.Manager.EndTurn(e.Victim,true); e.Resolve(warning,true);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim); e.Manager.EndTurn(e.Boss,true);
            int ap = e.HelperActor.ActionPoints; e.Helper.Use(hold);
            Check(hold.Deleted && e.HelperActor.ActionPoints == ap - 4 && e.Ledger.Points(3,e.Helper) == 2, "actual ally ItemUse rescue costs four AP and credits once");
        }
    }
    private static void Safety()
    {
        using (Encounter e = new Encounter())
        {
            e.Victim.Hidden = true; Check(!e.TryWarn(e.Victim), "hidden target is not revealed by a stale combatant web"); e.Victim.Hidden = false;
            e.Boss.Pacify(e.Helper,DateTime.Now.AddSeconds(30)); Check(!e.TryWarn(e.Victim), "pacified boss cannot fire an ability during AI think"); e.Boss.Unpacify();
            e.Boss.Paralyzed = true; Check(!e.TryWarn(e.Victim), "paralyzed boss cannot cast web"); e.Boss.Paralyzed = false;
            e.Boss.Frozen = true; Check(!e.TryWarn(e.Victim), "frozen boss cannot cast web"); e.Boss.Frozen = false;
            e.Boss.BardProvoked = true; Check(!e.TryWarn(e.Victim), "provoked boss cannot manufacture web rewards"); e.Boss.BardProvoked = false;
            e.Victim.AccessLevel = AccessLevel.GameMaster; Check(!e.TryWarn(e.Victim), "staff target excluded"); e.Victim.AccessLevel = AccessLevel.Player;
            e.Victim.MoveToWorld(new Point3D(2600,2088,0),Map.Sosaria); Check(!e.TryWarn(e.Victim), "out-of-range and out-of-footprint target excluded");
        }
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn(); e.Due(warning); e.Resolve(warning,false);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim); string reason;
            e.Helper.MoveToWorld(new Point3D(2518,2088,0),Map.Sosaria); Check(!hold.TryFree(e.Helper,out reason), "rescue range beyond two rejected");
            e.Helper.MoveToWorld(new Point3D(2517,2088,0),Map.Sosaria); Static wall = new Static(0x80); wall.MoveToWorld(new Point3D(2516,2088,0),Map.Sosaria);
            try { Check(!e.Helper.InLOS(e.Victim) && !hold.TryFree(e.Helper,out reason), "real wall blocks rescue line of sight"); }
            finally { wall.Delete(); }
            e.Helper.MoveToWorld(new Point3D(2516,2088,0),Map.Sosaria);
            e.Helper.AccessLevel = AccessLevel.GameMaster; Check(!hold.TryFree(e.Helper,out reason), "staff cannot earn rescue points"); e.Helper.AccessLevel = AccessLevel.Player;
            Check(e.Ledger.Points(3,e.Helper) == 0 && ArachnidWebRestraint.Find(e.Victim) == hold, "failed rescues preserve effect and pay nothing");
            e.Victim.MoveToWorld(new Point3D(2515,2089,0),Map.Sosaria); Check(ArachnidWebRestraint.Find(e.Victim) == null && hold.Deleted, "legal forced relocation releases movement restraint");
            Check(e.Ledger.Points(3,e.Helper) == 0, "relocation is not a paid web rescue");
        }
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn(); e.Controller.Stop(); e.Due(warning);
            Check(warning.Deleted && !e.Resolve(warning,false) && ArachnidWebRestraint.Find(e.Victim) == null, "stop cancels pending impact and owned warning");
        }
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn(); e.Due(warning); e.Resolve(warning,false);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim);
            e.Victim.Kill(); Check(hold.Deleted && ArachnidWebRestraint.Find(e.Victim) == null, "real death releases restraint without escape");
        }
        using (Encounter e = new Encounter())
        {
            DreadSpider pet = new DreadSpider(); pet.Controlled = true; pet.ControlMaster = e.Victim; pet.MoveToWorld(e.Victim.Location,e.Victim.Map);
            try
            {
                Check(e.TryWarn(pet), "controlled participant pet can be targeted");
                ArachnidWebWarning warning = (ArachnidWebWarning)Get(e.Boss,"m_Web"); e.Due(warning); Check(e.Resolve(warning,false), "pet restraint applies");
                ArachnidWebRestraint hold = ArachnidWebRestraint.Find(pet); string reason;
                Check(hold.TryFree(e.Victim,out reason) && e.Ledger.Points(3,e.Victim) == 2, "owner can rescue participating pet and earn support");
            }
            finally { pet.Delete(); }
        }
    }
    private static void ModeChanges()
    {
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn(); e.Battle(); e.Due(warning);
            Check(!e.Resolve(warning,true) && warning.Deleted, "real-time warning is cancelled when the target enters turn-based combat");
            Check(!e.TryWarn(e.Victim), "new battle clock cannot immediately reset an existing web cooldown");
            Check(ArachnidWebRestraint.Find(e.Victim) == null, "mode transition creates no unannounced restraint");
        }
        using (Encounter e = new Encounter())
        {
            ArachnidWebWarning warning = e.Warn(); e.Due(warning); e.Resolve(warning,false);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim); e.Battle();
            Check(ArachnidWebRestraint.Find(e.Victim) == null && hold.Deleted, "existing restraint releases rather than importing a wall clock into battle");
        }
        using (Encounter e = new Encounter())
        {
            e.Battle();
            TurnActionDecision pending = TurnBasedCombatBridge.BeginAction(new TurnActionRequest(e.Boss,null,e,TurnActionKind.ItemUse,4,false,false));
            Check(pending.Allowed && pending.Lease != null && !e.TryWarn(e.Victim), "a pending boss action cannot acquire a free nested web attack");
            TurnBasedCombatBridge.CompleteAction(pending.Lease,new TurnActionResult(e.Boss,null,TurnActionPhase.Refund,false));
            ArachnidWebWarning warning = e.Warn(); e.Due(warning);
            e.Manager.EndTurn(e.Boss,true); e.Manager.EndTurn(e.Victim,true); e.Manager.EndTurn(e.Helper,true); e.Resolve(warning,true);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim);
            e.Manager.Disable("test battle exit");
            Check(ArachnidWebRestraint.Find(e.Victim) == null && hold.Deleted, "battle dissolution releases restraint without stale action clocks");
        }
        using (Encounter e = new Encounter())
        {
            e.Battle(); ArachnidWebWarning warning = e.Warn(); e.Due(warning);
            e.Manager.EndTurn(e.Boss,true); e.Manager.EndTurn(e.Victim,true); e.Manager.EndTurn(e.Helper,true); e.Resolve(warning,true);
            ArachnidWebRestraint hold = ArachnidWebRestraint.Find(e.Victim);
            TurnCombatGroup other = new TurnCombatGroup(); other.ID = 2; other.Round = 2; other.Current = e.HelperActor;
            e.HelperActor.ActionPoints = 20; e.HelperActor.ActionSecondsThisTurn = 0.0;
            e.Group.Participants.Remove(e.HelperActor); e.HelperActor.Group = other; other.Participants.Add(e.HelperActor);
            ((List<TurnCombatGroup>)Get(e.Manager,"m_Groups")).Add(other);
            e.Helper.Use(hold);
            Console.WriteLine("Cross-battle rescue: deleted="+hold.Deleted+"; points="+e.Ledger.Points(3,e.Helper)+"; victimGroup="+e.VictimActor.Group.ID+"; helperGroup="+e.HelperActor.Group.ID+"; held="+(ArachnidWebRestraint.Find(e.Victim)==hold));
            Check(ArachnidWebRestraint.Find(e.Victim) == null && hold.Deleted && e.Ledger.Points(3,e.Helper) == 0,
                "classified rescue merges battles and cancels the old web clock without paying an invalidated rescue");
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly; Core.Expansion=Expansion.SA; Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",Static).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",Static).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure(); RaceDefinitions.Configure(); Fists.Initialize(); Server.Movement.MovementImpl.Configure(); PrepareCatalog(args[1]); ArachnidOutbreakController.Initialize(); ArachnidWebRestraint.Initialize();
            Realtime(); TurnBased(); Safety(); ModeChanges();
            Console.WriteLine("PASS outbreak webs: "+s_Checks+" production-method assertions; real manager action/turn paths; synthetic roster; no existing saves/listeners."); return 0;
        }
        catch(Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
