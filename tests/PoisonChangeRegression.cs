using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Targeting;

internal static class PoisonChangeRegression
{
    private static int s_Checks,s_Preventions;
    private static bool s_Prevent;
    private static readonly List<PoisonChangedEventArgs> s_Changes=new List<PoisonChangedEventArgs>();
    private static void Check(bool condition,string message){if(!condition)throw new Exception("FAILED: "+message);++s_Checks;}
    private sealed class Patient:Mobile
    {
        public bool Immune;
        public bool PermitCure=true;
        public override bool CheckPoisonImmunity(Mobile from,Poison poison){return Immune || base.CheckPoisonImmunity(from,poison);}
        public override bool CheckCure(Mobile from){return PermitCure;}
    }
    internal sealed class Denier:ITurnBasedCombatHandler
    {
        public TurnMutationKind Denied=TurnMutationKind.Poison;
        public bool Enabled{get{return true;}}
        public bool IsParticipant(Mobile mobile){return false;}
        public TurnActionDecision BeginAction(TurnActionRequest request){return TurnActionDecision.Allow();}
        public TurnActionLease GetPendingActionLease(Mobile mobile){return null;}
        public void CompleteAction(TurnActionLease lease,TurnActionResult result){}
        public bool AuthorizeMutation(TurnMutationRequest request){return request.Kind!=Denied;}
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
    private static void Prevention(PoisonPreventionEventArgs e){++s_Preventions;if(s_Prevent)e.Prevent();}
    private static void Changed(PoisonChangedEventArgs e){s_Changes.Add(e);}
    private static void Run()
    {
        Patient patient=new Patient();Mobile source=new Mobile();Mobile healer=new Mobile();Denier denier=new Denier();
        try
        {
            EventSink.PoisonPrevention+=Prevention;EventSink.PoisonChanged+=Changed;
            patient.Immune=true;s_Prevent=true;
            Check(patient.ApplyPoison(source,Poison.Deadly)==ApplyPoisonResult.Immune && s_Preventions==0 && s_Changes.Count==0,"existing immunity precedes prevention");
            patient.Immune=false;
            Check(patient.ApplyPoison(source,Poison.Deadly)==ApplyPoisonResult.Immune && !patient.Poisoned && s_Preventions==1 && s_Changes.Count==0,"prevention skips actual poison and change notification");
            s_Prevent=false;
            Check(patient.ApplyPoison(source,Poison.Deadly)==ApplyPoisonResult.Poisoned,"ordinary application preserved");
            PoisonChangedEventArgs change=s_Changes[0];
            Check(change.Mobile==patient && change.Source==source && change.OldPoison==null && change.NewPoison==Poison.Deadly && change.Kind==PoisonChangeKind.Application,"committed poison reports source and final severity");
            int calls=s_Preventions;s_Changes.Clear();
            Check(patient.ApplyPoison(source,Poison.Regular)==ApplyPoisonResult.HigherPoisonActive && s_Preventions==calls && s_Changes.Count==0,"already active stronger poison consumes no prevention");
            patient.PermitCure=false;Check(!patient.CurePoison(healer) && patient.Poisoned && s_Changes.Count==0,"failed cure changes nothing");patient.PermitCure=true;
            Check(TurnBasedCombatBridge.Register(denier),"private poison mutation denier registered");
            Check(!patient.CurePoison(healer) && patient.Poisoned && s_Changes.Count==0,"turn denied cure cannot remove poison");
            Check(patient.ApplyPoison(healer,null)==ApplyPoisonResult.Blocked && patient.Poisoned,"null-poison cure reports a blocked mutation accurately");
            patient.Poison=null;Check(patient.Poisoned && s_Changes.Count==0,"direct poison setter still obeys turn mutation guard");
            TurnBasedCombatBridge.Unregister(denier);
            Check(patient.CurePoison(healer) && !patient.Poisoned && s_Changes.Count==1,"successful cure observed once");change=s_Changes[0];
            Check(change.Kind==PoisonChangeKind.Cure && change.Source==healer && change.OldPoison==Poison.Deadly && change.NewPoison==null,"cure carries actual old condition and curing actor");
            s_Changes.Clear();patient.Poison=Poison.Regular;
            Check(s_Changes.Count==1 && s_Changes[0].Kind==PoisonChangeKind.Unattributed && s_Changes[0].Source==null,"direct assignment cannot inherit previous poisoner or healer");
            patient.Poison=null;s_Changes.Clear();calls=s_Preventions;
            TurnBasedCombatBridge.Register(denier);
            Check(patient.ApplyPoison(source,Poison.Lethal)==ApplyPoisonResult.Blocked && !patient.Poisoned && s_Preventions==calls && s_Changes.Count==0,"turn denied application cannot consume a prevention");
            TurnBasedCombatBridge.Unregister(denier);
            patient.ApplyPoison(source,Poison.Lethal);patient.CurePoison(healer);
            Check(!patient.Poisoned,"denied operation leaves no stale mutation scope");
            EventSink.Reset();s_Changes.Clear();patient.Poison=Poison.Lesser;
            Check(s_Changes.Count==0,"reset removes poison subscriptions");
        }
        finally
        {
            TurnBasedCombatBridge.Unregister(denier);EventSink.PoisonPrevention-=Prevention;EventSink.PoisonChanged-=Changed;
            patient.Delete();source.Delete();healer.Delete();
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();PoisonImpl.Configure();Run();
            Console.WriteLine("PASS poison changes: "+s_Checks+" actual mutation assertions; no world saves or listeners.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
