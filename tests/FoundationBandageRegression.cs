using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using Server;
using Server.Custom.Confictura;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Targeting;

internal static class FoundationBandageRegression
{
    private const BindingFlags Instance = BindingFlags.Instance | BindingFlags.NonPublic;
    private const BindingFlags Static = BindingFlags.Static | BindingFlags.NonPublic;
    private static int checks, skills;
    private static Handler handler = new Handler();
    private static List<System.Net.Sockets.Socket> sockets = new List<System.Net.Sockets.Socket>();
    private sealed class Patient : Mobile
    {
        public int Heals;
        public Action<Mobile> HealCallback;
        public override void OnHeal(ref int amount, Mobile from) { ++Heals; if (HealCallback!=null) HealCallback(from); }
        public override bool CanBeBeneficial(Mobile target, bool message, bool allowDead) { return true; }
    }
    private sealed class Capture : NetState
    {
        public int Packets;
        public Capture() : base(null, null) { }
        public override void Send(Packet packet) { ++Packets; packet.OnSend(); }
    }
    private sealed class Handler : ITurnBasedCombatHandler
    {
        public int Clears, Starts;
        public BandageContext Active;
        public TurnBasedCombatManager Manager;
        public bool Enabled { get { return true; } }
        public bool IsParticipant(Mobile m) { return false; }
        public TurnActionDecision BeginAction(TurnActionRequest r) { return TurnActionDecision.Allow(); }
        public TurnActionLease GetPendingActionLease(Mobile m) { return null; }
        public void CompleteAction(TurnActionLease l, TurnActionResult r) { }
        public bool AuthorizeMutation(TurnMutationRequest r) { return true; }
        public TurnCombatantChangeMode DecideCombatantChange(TurnCombatantChangeRequest r) { return TurnCombatantChangeMode.Native; }
        public DateTime GetActorTime(Mobile m, DateTime t) { return t; }
        public TimeSpan GetActorInterval(Mobile m) { return TimeSpan.FromSeconds(5); }
        public void TargetFinished(Mobile m, Target t, bool invoked) { }
        public void EffectScheduled(Mobile m, TurnMutationKind k, TimeSpan t) { }
        public void EffectTimerChanged(Mobile m, TurnMutationKind k, Timer t)
        {
            if (k != TurnMutationKind.Healing) return;
            if (t == null) { ++Clears; Active = null; }
            else { ++Starts; Active = BandageContext.GetContext(m); }
            if (Manager != null) Manager.EffectTimerChanged(m, k, t);
        }
        public void MobileRelocated(Mobile m, Point3D p, Map map, bool forced) { }
        public void MobileDeleted(Mobile m) { }
        public void EmergencyDisable(string reason, Exception e) { throw new Exception(reason, e); }
    }
    private static void Check(bool value, string label)
    { if (!value) throw new Exception("FAILED: " + label); ++checks; }
    private static Patient Mobile()
    {
        Patient m = new Patient(); m.Name = "bandage fixture"; m.Player = true; m.Body = 400;
        m.RawStr = 100; m.RawDex = 100; m.Hits = 10; m.Hunger = 20;
        m.Skills[SkillName.Healing].Base = 120; m.Skills[SkillName.Anatomy].Base = 120;
        m.Map = Map.Sosaria; m.Location = new Point3D(80, 80, 0);
        Capture state = (Capture)FormatterServices.GetUninitializedObject(typeof(Capture));
        System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        sockets.Add(socket); // Identity only: never bound, connected or listening.
        typeof(NetState).GetField("m_Socket", Instance).SetValue(state, socket);
        typeof(NetState).GetField("m_WorldSendLock", Instance).SetValue(state, new object());
        typeof(NetState).GetField("m_SendQueue", Instance).SetValue(state, new SendQueue());
        foreach (FieldInfo field in typeof(NetState).GetFields(Instance))
            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>)) field.SetValue(state, Activator.CreateInstance(field.FieldType));
        typeof(NetState).GetField("m_Mobile", Instance).SetValue(state, m);
        typeof(Server.Mobile).GetField("m_NetState", Instance).SetValue(m, state);
        return m;
    }
    private static int Packets(Patient m) { return ((Capture)m.NetState).Packets; }
    private static void Queue(Timer timer)
    {
        // Reproduce TimerThread's one-shot queue-before-Stop order, then use real Timer.Slice.
        typeof(Timer).GetField("m_Queued", Instance).SetValue(timer, true);
        ((Queue<Timer>)typeof(Timer).GetField("m_Queue", Static).GetValue(null)).Enqueue(timer);
        timer.Stop();
    }
    private static BandageContext Begin(Patient h, Patient p)
    {
        BandageContext c = BandageContext.BeginHeal(h, p);
        Check(c != null && BandageContext.GetContext(h) == c, "BeginHeal registered"); return c;
    }
    private static void Historical()
    {
        Patient h = Mobile(), p = Mobile(); BandageContext a = Begin(h,p); Timer old = a.Timer; Queue(old);
        BandageContext b = Begin(h,p); int clear = handler.Clears; Timer.Slice();
        Check(p.Heals == 1 && BandageContext.GetContext(h) == null && handler.Clears == clear+1, "historical cancelled callback heals and clears replacement"); b.StopHeal();
        h=Mobile(); p=Mobile(); a=Begin(h,p); old=a.Timer; Queue(old); a.SuspendForTurnBased(); a.ResumeFromTurnBased(); Timer replacement=a.Timer; Timer.Slice();
        Check(p.Heals==1 && a.Timer==null && !replacement.Running, "historical pre-suspend callback completes resumed context");
        h=Mobile(); p=Mobile(); a=Begin(h,p); a.EndHeal(); p.Hits=10; a.EndHeal();
        Check(p.Heals==2, "historical repeated completion heals twice");
        h=Mobile(); p=Mobile(); a=Begin(h,p); b=Begin(h,p); clear=handler.Clears; a.StopHeal();
        Check(BandageContext.GetContext(h)==null && handler.Clears==clear+1, "historical stale cancellation clears newer effect"); b.StopHeal();
    }
    private static void Current()
    {
        Patient h=Mobile(), p=Mobile(); BandageContext a=Begin(h,p); Timer old=a.Timer; Queue(old);
        BandageContext b=Begin(h,p); int clear=handler.Clears, count=Packets(h)+Packets(p), skill=skills;
        Timer.Slice(); a.StopHeal(); a.EndHeal();
        Check(p.Heals==0 && p.Hits==10 && skills==skill, "stale callback cannot heal or train");
        Check(Packets(h)+Packets(p)==count, "stale callback emits no messages or effects");
        Check(BandageContext.GetContext(h)==b && handler.Active==b && handler.Clears==clear, "stale callback preserves newer table and effect");
        Timer valid=b.Timer; Queue(valid); Check(!valid.Running, "normal one-shot already stopped before dispatch"); Timer.Slice();
        Check(p.Heals==1 && skills==skill+2 && p.Hits>10, "normal one-shot completes despite Running false");
        Check(b.Timer==null && BandageContext.GetContext(h)==null && handler.Clears==clear+1, "normal completion releases ownership once");
        count=Packets(h)+Packets(p); p.Hits=10; Queue(valid); Timer.Slice(); b.EndHeal(); b.StopHeal();
        Check(p.Heals==1 && p.Hits==10 && skills==skill+2 && handler.Clears==clear+1 && Packets(h)+Packets(p)==count, "completed callback and public completion idempotent");

        h=Mobile(); p=Mobile(); a=Begin(h,p); old=a.Timer; Queue(old); TimeSpan remaining=a.SuspendForTurnBased();
        Check(remaining>TimeSpan.Zero && a.Timer==old && !old.Running, "suspension preserves timer subtype");
        Check(a.SuspendForTurnBased()==remaining, "repeated suspension does not reset countdown");
        Timer.Slice(); a.EndHeal(); Check(p.Heals==0 && BandageContext.GetContext(h)==a, "queued/manual real-time completion inert while suspended");
        Check(!a.AdvanceTurnBased(TimeSpan.FromTicks(1)), "partial turn does not complete");
        Check(a.SuspendForTurnBased()==remaining-TimeSpan.FromTicks(1), "repeated suspension retains advanced remaining");
        Queue(old); a.ResumeFromTurnBased(); Timer resumed=a.Timer; a.ResumeFromTurnBased();
        Check(resumed!=old && a.Timer==resumed && resumed.Running, "resume creates one replacement timer"); Timer.Slice();
        Check(p.Heals==0 && a.Timer==resumed, "queued pre-resume timer cannot complete replacement");
        Queue(resumed); Timer.Slice(); Check(p.Heals==1, "resumed timer completes exactly once");

        h=Mobile(); p=Mobile(); a=Begin(h,p); old=a.Timer; Queue(old); remaining=a.SuspendForTurnBased();
        Check(a.AdvanceTurnBased(remaining), "turn advancement completes due bandage"); clear=handler.Clears;
        Timer.Slice(); Check(!a.AdvanceTurnBased(remaining) && p.Heals==1 && handler.Clears==clear, "queued callback after turn completion is inert");
        p.Hits=10; b=Begin(h,p); a.ResumeFromTurnBased(); a.SuspendForTurnBased(); a.AdvanceTurnBased(TimeSpan.MaxValue); a.StopHeal();
        Check(BandageContext.GetContext(h)==b && handler.Active==b && p.Heals==1, "stale turn operations preserve newer context"); b.StopHeal();

        h=Mobile(); p=Mobile(); a=Begin(h,p); old=a.Timer; Queue(old); a.StopHeal(); clear=handler.Clears; Timer.Slice();
        Check(p.Heals==0 && a.Timer==null && handler.Clears==clear, "cancel without replacement never completes");
        foreach (bool deleteHealer in new bool[]{false,true})
        {
            h=Mobile(); p=Mobile(); a=Begin(h,p); old=a.Timer; Queue(old); (deleteHealer?h:p).Delete(); skill=skills; Timer.Slice();
            Check(p.Heals==0 && skills==skill && a.Timer==null && BandageContext.GetContext(h)==null, "deleted participant releases without heal or skill");
        }
        Check(BandageContext.BeginHeal(null,Mobile())==null && BandageContext.BeginHeal(Mobile(),null)==null && BandageContext.GetContext(null)==null, "null entry guards");
        h=Mobile(); h.Delete(); Check(BandageContext.BeginHeal(h,Mobile())==null && BandageContext.BeginHeal(Mobile(),h)==null, "deleted entry guards");

        h=Mobile(); clear=handler.Clears; int starts=handler.Starts; a=new BandageContext(h,h,TimeSpan.FromSeconds(5));
        Check(BandageContext.GetContext(h)==null && handler.Starts==starts && a.Timer.Delay==TimeSpan.FromSeconds(5), "OmniAI direct constructor retains public invisibility and timing");
        old=a.Timer; Queue(old); Timer.Slice(); Check(h.Heals==1 && handler.Clears==clear, "direct constructor still heals without registered effect clear");
        h.Hits=10; a=new BandageContext(h,h,TimeSpan.FromSeconds(5)); old=a.Timer; Queue(old); b=Begin(h,h); Timer.Slice();
        Check(h.Heals==1 && BandageContext.GetContext(h)==b, "registered heal supersedes direct context"); b.EndHeal(); h.Hits=10; Queue(old); Timer.Slice(); Check(h.Heals==2 && h.Hits==10, "superseded direct context remains inert after newer completion");
        h=Mobile(); a=Begin(h,h); old=a.Timer; Queue(old); b=new BandageContext(h,h,TimeSpan.FromSeconds(5)); Timer.Slice();
        Check(h.Heals==0 && BandageContext.GetContext(h)==null && b.Timer!=null, "direct context supersedes registered owner safely"); Queue(b.Timer); Timer.Slice(); Check(h.Heals==1, "new direct owner completes");
        h=Mobile(); a=new BandageContext(h,h,TimeSpan.FromSeconds(5)); old=a.Timer; Queue(old); b=new BandageContext(h,h,TimeSpan.FromSeconds(6)); Timer.Slice();
        Check(h.Heals==0 && b.Timer!=null, "direct replacement suppresses old callback"); Queue(b.Timer); Timer.Slice(); Check(h.Heals==1, "replacement direct callback heals once");

        foreach (int dex in new int[]{20,100,210})
        {
            h=Mobile(); p=Mobile(); h.RawDex=dex; a=Begin(h,p); double seconds=dex<204?3.2-Math.Sin((double)h.Dex/130)*2.5:0.7;
            Check(a.Timer.Delay==TimeSpan.FromMilliseconds(seconds*1000), "other-heal timing unchanged"); a.StopHeal();
            a=Begin(h,h); Check(a.Timer.Delay==TimeSpan.FromMilliseconds((5.0+0.5*((120.0-h.Dex)/10))*1000), "self-heal timing unchanged"); a.StopHeal();
        }
        Integration();
        RealManagerTurns();
    }
    private static void Integration()
    {
        TurnBasedCombatManager manager=(TurnBasedCombatManager)FormatterServices.GetUninitializedObject(typeof(TurnBasedCombatManager));
        TurnBasedCombatConfiguration config=new TurnBasedCombatConfiguration(); config.LogEnabled=false;
        Dictionary<TurnMutationKind,List<TurnEffectRule>> rules=(Dictionary<TurnMutationKind,List<TurnEffectRule>>)typeof(TurnBasedCombatConfiguration).GetField("m_EffectRules",Instance).GetValue(config);
        rules.Add(TurnMutationKind.Healing,new List<TurnEffectRule>{new TurnEffectRule{RuntimeType="Server.Items.BandageContext+InternalTimer",Kind=TurnMutationKind.Healing,ClockPolicy="ActorClock",TickPhase="EndTurn"}});
        typeof(TurnBasedCombatManager).GetField("m_Configuration",Instance).SetValue(manager,config);
        Dictionary<Server.Mobile,TurnParticipant> participants=new Dictionary<Server.Mobile,TurnParticipant>();
        typeof(TurnBasedCombatManager).GetField("m_Participants",Instance).SetValue(manager,participants);
        Patient h=Mobile(),p=Mobile(); TurnParticipant participant=new TurnParticipant{Mobile=h,Effects=new TurnEffectState()}; participants.Add(h,participant); handler.Manager=manager;
        BandageContext a=Begin(h,p); Timer old=a.Timer; Queue(old); BandageContext b=Begin(h,p); int clear=handler.Clears;
        Check(participant.Effects.BandageContext==b && !b.Timer.Running && participant.Effects.BandageRemaining>TimeSpan.Zero, "real manager captures suspended typed context");
        Timer.Slice(); a.StopHeal(); Check(participant.Effects.BandageContext==b && handler.Clears==clear && p.Heals==0, "stale timer preserves real manager effect");
        MethodInfo enabled=typeof(TurnBasedCombatManager).GetMethod("IsEffectEnabled",Instance);
        Check((bool)enabled.Invoke(manager,new object[]{participant,TurnMutationKind.Healing,b.Timer,"EndTurn"}), "manager resolves retained suspended timer type");
        TimeSpan remaining=participant.Effects.BandageRemaining;
        Check(b.AdvanceTurnBased(remaining), "manager-captured context advances");
        Check(p.Heals==1 && participant.Effects.BandageContext==null && participant.Effects.BandageRemaining==TimeSpan.Zero, "owner completion clears real manager effect"); handler.Manager=null;
    }
    private static void RealManagerTurns()
    {
        foreach (bool self in new bool[]{false,true})
        {
            Patient h=Mobile(), p=self?h:Mobile(), opponent=self?Mobile():p, outsider=Mobile();
            BandageContext bandage=Begin(h,p); Timer old=bandage.Timer; Queue(old);
            TurnBasedCombatManager manager=(TurnBasedCombatManager)FormatterServices.GetUninitializedObject(typeof(TurnBasedCombatManager));
            TurnBasedCombatConfiguration config=new TurnBasedCombatConfiguration(); config.LogEnabled=false; config.ActorSeconds=10;
            Dictionary<TurnMutationKind,List<TurnEffectRule>> rules=(Dictionary<TurnMutationKind,List<TurnEffectRule>>)typeof(TurnBasedCombatConfiguration).GetField("m_EffectRules",Instance).GetValue(config);
            rules.Add(TurnMutationKind.Healing,new List<TurnEffectRule>{new TurnEffectRule{RuntimeType="Server.Items.BandageContext+InternalTimer",Kind=TurnMutationKind.Healing,ClockPolicy="ActorClock",TickPhase="StartOfTurn"}});
            TurnCombatGroup group=new TurnCombatGroup(); group.ID=1; group.Round=1;
            group.Edges.Add(new TurnHostilityEdge{Source=h,Target=opponent});
            TurnParticipant actor=new TurnParticipant{Mobile=h,Group=group,Effects=new TurnEffectState(),HasTakenFirstTurn=true};
            TurnParticipant other=new TurnParticipant{Mobile=opponent,Group=group,Effects=new TurnEffectState(),HasTakenFirstTurn=true};
            group.Participants.Add(actor); group.Participants.Add(other); group.Current=actor;
            Dictionary<Server.Mobile,TurnParticipant> participants=new Dictionary<Server.Mobile,TurnParticipant>(); participants.Add(h,actor); participants.Add(opponent,other);
            typeof(TurnBasedCombatManager).GetField("m_Configuration",Instance).SetValue(manager,config);
            typeof(TurnBasedCombatManager).GetField("m_Participants",Instance).SetValue(manager,participants);
            typeof(TurnBasedCombatManager).GetField("m_Groups",Instance).SetValue(manager,new List<TurnCombatGroup>{group});
            typeof(TurnBasedCombatManager).GetField("m_RuntimeEnabled",Instance).SetValue(manager,true);
            FieldInfo instance=typeof(TurnBasedCombatManager).GetField("m_Instance",Static);
            object previousManager=instance.GetValue(null); instance.SetValue(null,manager);
            TurnBasedCombatBridge.Unregister(handler);
            try
            {
                Check(TurnBasedCombatBridge.Register(manager), "register real manager for actor-clock integration");
                manager.EffectTimerChanged(h,TurnMutationKind.Healing,bandage.Timer);
                Check(actor.Pending==null && actor.Effects.BandageContext==bandage && !old.Running, "captured bandage has no pending action");
                Check(TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(h,h,TurnMutationKind.Effect,0,0))==null, "ordinary self effect cannot bypass pending-action guard");
                Check(TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(h,p,TurnMutationKind.Healing,10,20))==null, "ordinary healing cannot bypass pending-action guard");
                Check(TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(h,outsider,TurnMutationKind.Effect,0,0))==null, "ordinary outsider effect cannot bypass pending-action guard");
                MethodInfo effects=typeof(TurnBasedCombatManager).GetMethod("ProcessActorEffects",Instance);
                TimeSpan remaining=actor.Effects.BandageRemaining;
                TurnParticipant stale=new TurnParticipant{Mobile=h,Group=group,Effects=actor.Effects};
                effects.Invoke(manager,new object[]{stale,TimeSpan.FromSeconds(10),"StartOfTurn"});
                Check(actor.Effects.BandageRemaining==remaining && p.Heals==0, "stale participant cannot process owned effects");
                int skill=skills, heals=p.Heals;
                Check(manager.EndTurn(h,true) && group.Current==other, "real EndTurn advances to opponent");
                effects.Invoke(manager,new object[]{actor,TimeSpan.FromSeconds(10),"StartOfTurn"});
                Check(actor.Effects.BandageRemaining==remaining && p.Heals==heals, "out-of-turn actor cannot process effects");
                Check(p.Heals==heals, "bandage waits for healer StartOfTurn");
                Check(manager.EndTurn(opponent,true) && group.Current==actor && group.Round==2, "real EndTurn returns to healer next round");
                Check(p.Heals==heals+1 && p.Hits>10 && skills==skill+2, "real manager StartOfTurn completes self or other healing without pending action");
                Check(actor.Effects.BandageContext==null && actor.Effects.BandageRemaining==TimeSpan.Zero && BandageContext.GetContext(h)==null, "real manager completion clears owned effect");
                int count=Packets(h)+Packets(opponent); Timer.Slice(); bandage.EndHeal();
                Check(p.Heals==heals+1 && skills==skill+2 && Packets(h)+Packets(opponent)==count, "queued old callback after real actor completion stays inert");
                Check(TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(h,h,TurnMutationKind.Effect,0,0))==null, "actor effect authorization does not escape scope");
                Check(TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(h,p,TurnMutationKind.Healing,p.Hits,p.Hits+1))==null, "patient authorization does not escape scope");

                TurnBasedCombatBridge.Unregister(manager); p.Hits=10; h.Hits=10;
                BandageContext completing=BandageContext.BeginHeal(h,p), replacement=null;
                Check(TurnBasedCombatBridge.Register(manager), "restore real manager after independent callback fixture setup");
                manager.EffectTimerChanged(h,TurnMutationKind.Healing,completing.Timer);
                p.HealCallback=delegate(Server.Mobile from){p.HealCallback=null;replacement=BandageContext.BeginHeal(h,h);};
                Check(manager.EndTurn(h,true) && manager.EndTurn(opponent,true), "advance real turns for completion callback replacement");
                Check(replacement!=null && BandageContext.GetContext(h)==replacement && actor.Effects.BandageContext==replacement && actor.Effects.BandageRemaining>TimeSpan.Zero, "old completion preserves callback-installed replacement effect");
                replacement.StopHeal();
                Check(TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(h,outsider,TurnMutationKind.Healing,10,20))==null, "replacement callback cannot leak outsider authorization");

                TurnBasedCombatBridge.Unregister(manager); p.Hits=10;
                BandageContext throwing=BandageContext.BeginHeal(h,p);
                Check(TurnBasedCombatBridge.Register(manager), "restore real manager after exception fixture setup");
                manager.EffectTimerChanged(h,TurnMutationKind.Healing,throwing.Timer);
                p.HealCallback=delegate(Server.Mobile from){p.HealCallback=null;throw new InvalidOperationException("owned heal callback probe");};
                bool threw=false;
                try{manager.EndTurn(h,true);manager.EndTurn(opponent,true);}catch(InvalidOperationException e){threw=e.Message=="owned heal callback probe";}
                Check(threw, "completion exception propagated from owned callback");
                Check(TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(h,h,TurnMutationKind.Effect,0,0))==null && TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(h,p,TurnMutationKind.Healing,10,20))==null, "self and patient scopes close on completion exception");
                Check(!TurnBasedCombatBridge.IsFaulted, "real manager bridge remains healthy");
            }
            finally
            {
                TurnBasedCombatBridge.Unregister(manager); TurnBasedCombatBridge.Register(handler); bandage.StopHeal(); instance.SetValue(null,previousManager);
            }
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly; Core.Expansion=Expansion.SA; Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            typeof(World).GetField("m_Mobiles",Static).SetValue(null,new Dictionary<Serial,Server.Mobile>());
            typeof(World).GetField("m_Items",Static).SetValue(null,new Dictionary<Serial,Item>());
            MapDefinitions.Configure(); RaceDefinitions.Configure();
            Server.Mobile.SkillCheckLocationHandler=delegate(Server.Mobile m,SkillName s,double min,double max){++skills;return true;};
            Check(TurnBasedCombatBridge.Register(handler), "isolated bridge registration");
            bool historical=args.Length>1 && args[1]=="historical"; if(historical) Historical(); else Current();
            Check(!TurnBasedCombatBridge.IsFaulted,"bridge remains healthy");
            Console.WriteLine((historical?"REPRODUCED historical bandages":"PASS current bandages")+": "+checks+" assertions; real Timer.Slice, no world/listener/timer thread."); return 0;
        }
        catch(Exception e){Console.Error.WriteLine(e);return 1;}
        finally{TurnBasedCombatBridge.Unregister(handler); foreach(System.Net.Sockets.Socket socket in sockets)socket.Close();}
    }
}
