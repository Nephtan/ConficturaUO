using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Targeting;

internal static class HealthChangeRegression
{
    private static int s_Checks;
    private static readonly List<HealthChangedEventArgs> s_Events = new List<HealthChangedEventArgs>();
    private static void Check(bool condition,string message)
    {
        if(!condition)throw new Exception("FAILED: "+message);
        ++s_Checks;
    }
    private sealed class Patient : Mobile
    {
        public bool Cancel;
        public bool Throw;
        public bool SelfWrite;
        public bool RewriteOnce;
        public Mobile Other;
        public Mobile NestedHealer;
        public override void OnHeal(ref int amount,Mobile from)
        {
            if(Other!=null)Other.Hits-=1;
            if(SelfWrite)Hits+=1;
            if(Throw)throw new InvalidOperationException("fixture heal callback");
            if(Cancel)amount=0;
        }
        public override void OnHitsChange(int oldValue)
        {
            if(RewriteOnce){RewriteOnce=false;Hits+=1;}
            base.OnHitsChange(oldValue);
        }
        public override void OnDamage(int amount,Mobile from,bool willKill)
        {
            if(NestedHealer!=null)Heal(5,NestedHealer,false);
            base.OnDamage(amount,from,willKill);
        }
    }
    private sealed class Denier : ITurnBasedCombatHandler
    {
        public bool Enabled {get{return true;}}
        public bool IsParticipant(Mobile mobile){return false;}
        public TurnActionDecision BeginAction(TurnActionRequest request){return TurnActionDecision.Allow();}
        public TurnActionLease GetPendingActionLease(Mobile mobile){return null;}
        public void CompleteAction(TurnActionLease lease,TurnActionResult result){}
        public bool AuthorizeMutation(TurnMutationRequest request){return request.Kind!=TurnMutationKind.Healing;}
        public TurnCombatantChangeMode DecideCombatantChange(TurnCombatantChangeRequest request){return TurnCombatantChangeMode.Native;}
        public DateTime GetActorTime(Mobile mobile,DateTime time){return time;}
        public TimeSpan GetActorInterval(Mobile mobile){return TimeSpan.FromSeconds(1);}
        public void TargetFinished(Mobile mobile,Target target,bool invoked){}
        public void EffectScheduled(Mobile mobile,TurnMutationKind kind,TimeSpan duration){}
        public void EffectTimerChanged(Mobile mobile,TurnMutationKind kind,Timer timer){}
        public void MobileRelocated(Mobile mobile,Point3D oldLocation,Map oldMap,bool forced){}
        public void MobileDeleted(Mobile mobile){}
        public void EmergencyDisable(string reason,Exception exception){throw new Exception(reason,exception);}
    }
    private static void Capture(HealthChangedEventArgs e){s_Events.Add(e);}
    private static void BadObserver(HealthChangedEventArgs e){throw new InvalidOperationException("expected observer-isolation fixture");}
    private static void Prepare(Mobile mobile,int hits)
    {
        mobile.RawStr=100;mobile.Body=0x190;mobile.Hits=hits;s_Events.Clear();
    }
    private static void Run()
    {
        Patient patient=new Patient();Mobile healer=new Mobile();Mobile attacker=new Mobile();Mobile other=new Mobile();
        Denier denier=new Denier();
        try
        {
            Prepare(patient,90);EventSink.HealthChanged+=Capture;
            patient.Heal(100,healer,false);
            Check(s_Events.Count==1 && s_Events[0].Mobile==patient && s_Events[0].Source==healer
                && s_Events[0].Kind==HealthChangeKind.Healing && s_Events[0].OldHits==90 && s_Events[0].NewHits==100,"heal reports actual clamped restoration and source");
            s_Events.Clear();patient.Heal(100,healer,false);
            Check(s_Events.Count==0,"overheal produces no committed health change");
            patient.Hits=80;
            Check(s_Events.Count==1 && s_Events[0].Kind==HealthChangeKind.Unattributed && s_Events[0].Source==null,"direct health write has no stale healer");
            s_Events.Clear();patient.Cancel=true;patient.Heal(10,healer,false);
            Check(s_Events.Count==0 && patient.Hits==80,"canceled callback earns no restoration");patient.Cancel=false;
            patient.SelfWrite=true;patient.Cancel=true;patient.Heal(10,healer,false);
            Check(s_Events.Count==1 && s_Events[0].Kind==HealthChangeKind.Unattributed && s_Events[0].Source==null,"direct callback write is not attributed to canceled heal");
            patient.SelfWrite=false;patient.Cancel=false;patient.Hits=80;s_Events.Clear();
            Check(TurnBasedCombatBridge.Register(denier),"private mutation denier registered");
            patient.Heal(10,healer,false);
            Check(s_Events.Count==0 && patient.Hits==80,"turn-denied heal produces no notification");
            TurnBasedCombatBridge.Unregister(denier);
            patient.Throw=true;bool threw=false;
            try{patient.Heal(10,healer,false);}catch(InvalidOperationException){threw=true;}
            Check(threw && s_Events.Count==0,"failed heal callback has no committed restoration");patient.Throw=false;
            patient.Hits=75;
            Check(s_Events.Count==1 && s_Events[0].Kind==HealthChangeKind.Unattributed,"exception unwinds attribution scope");
            Prepare(other,50);patient.Other=other;patient.Heal(10,healer,false);
            Check(s_Events.Count==2 && s_Events[0].Mobile==other && s_Events[0].Kind==HealthChangeKind.Unattributed
                && s_Events[0].Source==null && s_Events[1].Kind==HealthChangeKind.Healing,"context does not attribute another target's direct write");patient.Other=null;
            s_Events.Clear();patient.RewriteOnce=true;patient.Heal(1,healer,false);
            Check(s_Events.Count==2 && s_Events[0].Kind==HealthChangeKind.Healing && s_Events[1].Kind==HealthChangeKind.Unattributed,
                "same-target health observer callback cannot inherit operation source");
            s_Events.Clear();patient.NestedHealer=healer;patient.Damage(10,attacker);
            Check(s_Events.Count==2 && s_Events[0].Kind==HealthChangeKind.Healing && s_Events[0].Source==healer
                && s_Events[1].Kind==HealthChangeKind.Damage && s_Events[1].Source==attacker,"nested heal restores outer damage source and kind");patient.NestedHealer=null;
            s_Events.Clear();patient.Blessed=true;patient.Damage(10,attacker);
            Check(s_Events.Count==0,"invulnerability rejects damage without notification");patient.Blessed=false;
            Prepare(patient,25);EventSink.HealthChanged-=Capture;EventSink.HealthChanged+=BadObserver;EventSink.HealthChanged+=Capture;
            patient.Damage(5000,attacker);
            Check(s_Events.Count==1 && s_Events[0].OldHits==25 && s_Events[0].NewHits==0,"overkill reports actual loss despite failing prior observer");
            Check(!patient.Alive,"failing observer cannot interrupt death resolution");
            EventSink.HealthChanged-=BadObserver;EventSink.Reset();s_Events.Clear();other.Hits-=1;
            Check(s_Events.Count==0,"EventSink reset removes new subscriptions");
        }
        finally
        {
            TurnBasedCombatBridge.Unregister(denier);EventSink.HealthChanged-=BadObserver;EventSink.HealthChanged-=Capture;
            patient.Delete();healer.Delete();attacker.Delete();other.Delete();
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();Corpse.Initialize();Run();
            Console.WriteLine("PASS health changes: "+s_Checks+" actual resolved health assertions; no world saves or listeners.");return 0;
        }
        catch(Exception exception){Console.Error.WriteLine(exception);return 1;}
    }
}
