using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Engines.XmlSpawner2;
using Server.Custom.Confictura;
using Server.Misc;
using Server.Items;

internal static class PacketInputThrottleRegression
{
    private static int s_Checks;
    private const BindingFlags Private = BindingFlags.NonPublic | BindingFlags.Instance;
    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception("FAILED: " + message);
        ++s_Checks;
    }
    private sealed class Probe : Mobile
    {
        public int Uses, Refusals;
        public override void Use(Item item) { ++Uses; }
        public override void Use(Mobile mobile) { ++Uses; }
        public override void OnPaperdollRequest() { ++Uses; }
        public override void SendActionMessage() { ++Refusals; }
    }
    private sealed class Clock : ITurnBasedCombatHandler
    {
        public Probe Actor;
        public bool Participant;
        public DateTime Paused = DateTime.Now.AddHours(-2);
        public bool Enabled { get { return true; } }
        public bool IsParticipant(Mobile mobile) { return Participant && mobile == Actor; }
        public TurnActionDecision BeginAction(TurnActionRequest request) { return TurnActionDecision.Allow(); }
        public TurnActionLease GetPendingActionLease(Mobile mobile) { return null; }
        public void CompleteAction(TurnActionLease lease, TurnActionResult result) { }
        public bool AuthorizeMutation(TurnMutationRequest request) { return true; }
        public TurnCombatantChangeMode DecideCombatantChange(TurnCombatantChangeRequest request) { return TurnCombatantChangeMode.Native; }
        public DateTime GetActorTime(Mobile mobile, DateTime wall) { return IsParticipant(mobile) ? Paused : wall; }
        public TimeSpan GetActorInterval(Mobile mobile) { return TimeSpan.FromSeconds(5); }
        public void TargetFinished(Mobile mobile, Target target, bool invoked) { }
        public void EffectScheduled(Mobile mobile, TurnMutationKind kind, TimeSpan duration) { }
        public void EffectTimerChanged(Mobile mobile, TurnMutationKind kind, Timer timer) { }
        public void MobileRelocated(Mobile mobile, Point3D location, Map map, bool forced) { }
        public void MobileDeleted(Mobile mobile) { }
        public void EmergencyDisable(string reason, Exception error) { throw new Exception(reason, error); }
    }
    private static PacketReader Reader(int serial)
    {
        return new PacketReader(new byte[] { 0x06, (byte)(serial >> 24), (byte)(serial >> 16), (byte)(serial >> 8), (byte)serial }, 5, true);
    }
    private static void Route(bool xml, NetState state, int serial)
    {
        if (xml) XmlAttach.UseReq(state, Reader(serial));
        else PacketHandlers.UseReq(state, Reader(serial));
    }
    private static void Packets()
    {
        foreach (bool xml in new bool[] { false, true })
        foreach (bool participant in new bool[] { false, true })
        foreach (int targetKind in new int[] { 0, 1, 2 })
        {
            Probe actor = new Probe();
            Item item = new Item(1);
            Mobile target = new Mobile();
            NetState state = (NetState)FormatterServices.GetUninitializedObject(typeof(NetState));
            typeof(NetState).GetField("m_Mobile", Private).SetValue(state, actor);
            Clock clock = new Clock { Actor = actor, Participant = participant };
            Check(TurnBasedCombatBridge.Register(clock), "isolated paused-clock handler registered");
            try
            {
                int serial = targetKind == 0 ? item.Serial.Value : targetKind == 1 ? target.Serial.Value : (actor.Serial.Value | Int32.MinValue);
                DateTime before = DateTime.Now;
                Route(xml, state, serial);
                Check(actor.Uses == 1, "first input dispatched; xml=" + xml + "; participant=" + participant + "; target=" + targetKind);
                Check(actor.NextActionTime >= before.AddMilliseconds(490) && actor.NextActionTime <= DateTime.Now.AddMilliseconds(510), "input delay uses wall time rather than paused actor time");
                Route(xml, state, serial);
                Check(actor.Uses == 1 && actor.Refusals == 1, "immediate input flood remains throttled");
                actor.NextActionTime = DateTime.Now.AddMilliseconds(-1);
                Route(xml, state, serial);
                Check(actor.Uses == 2 && actor.Refusals == 1, "expired wall input delay permits another request without advancing actor clock");
                actor.AccessLevel = AccessLevel.Counselor;
                actor.NextActionTime = DateTime.Now.AddHours(1);
                Route(xml, state, serial);
                Check(actor.Uses == 3, "existing staff bypass retained");
            }
            finally
            {
                TurnBasedCombatBridge.Unregister(clock);
                actor.Delete(); item.Delete(); target.Delete();
            }
        }
    }
    private static void LeaveBattle()
    {
        Probe actor = new Probe();
        TurnParticipant participant = new TurnParticipant { Mobile = actor, LogicalTime = DateTime.Now.AddHours(-2) };
        actor.NextActionTime = DateTime.Now.AddSeconds(10);
        actor.NextSkillTime = participant.LogicalTime.AddSeconds(3);
        actor.NextSpellTime = participant.LogicalTime.AddSeconds(4);
        actor.NextCombatTime = participant.LogicalTime.AddSeconds(5);
        DateTime inputDeadline = actor.NextActionTime;
        TurnBasedCombatManager manager = (TurnBasedCombatManager)FormatterServices.GetUninitializedObject(typeof(TurnBasedCombatManager));
        typeof(TurnBasedCombatManager).GetMethod("RebaseCooldowns", Private).Invoke(manager, new object[] { participant });
        Check(actor.NextActionTime == inputDeadline, "leaving combat cannot translate a wall input deadline by paused logical time");
        Check(Math.Abs((actor.NextSkillTime-DateTime.Now).TotalSeconds-3) < 0.5, "skill cooldown still rebases from actor time");
        Check(Math.Abs((actor.NextSpellTime-DateTime.Now).TotalSeconds-4) < 0.5, "spell cooldown still rebases from actor time");
        Check(Math.Abs((actor.NextCombatTime-DateTime.Now).TotalSeconds-5) < 0.5, "combat cooldown still rebases from actor time");
        actor.Delete();
    }
    private static void LiftInput()
    {
        Probe actor=new Probe();actor.Body=0x190;actor.RawStr=100;
        actor.MoveToWorld(new Point3D(2517,2088,0),Map.Sosaria);
        Backpack pack=new Backpack();actor.AddItem(pack);
        Item item=new Item(1);pack.DropItem(item);
        Clock clock=new Clock {Actor=actor,Participant=true};
        Check(TurnBasedCombatBridge.Register(clock), "paused actor clock registered for actual lift");
        try
        {
            bool rejected;LRReason reason;
            actor.NextActionTime=DateTime.Now.AddSeconds(1);
            actor.Lift(item,1,out rejected,out reason);
            Check(rejected && actor.Refusals==1 && actor.Holding==null, "actual lift retains future wall input delay");
            actor.NextActionTime=DateTime.Now.AddMilliseconds(-1);
            DateTime before=DateTime.Now;
            actor.Lift(item,1,out rejected,out reason);
            Check(!rejected && actor.Holding==item, "expired wall delay permits actual lift with paused actor clock; reason="+reason);
            Check(actor.NextActionTime>=before.AddMilliseconds(490) && actor.NextActionTime<=DateTime.Now.AddMilliseconds(510), "actual lift stamps wall input delay");
        }
        finally {TurnBasedCombatBridge.Unregister(clock);actor.Holding=null;item.Delete();actor.Delete();}
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();
            Packets(); LeaveBattle(); LiftInput();
            Console.WriteLine("PASS packet input throttle: " + s_Checks + " production packet/rebase assertions; paused bridge clock and unconnected state; no saves/listeners.");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
