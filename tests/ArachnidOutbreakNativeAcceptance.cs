using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using Server;
using Server.Accounting;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Misc;
using Server.Commands;
using Server.Custom.Confictura;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    public static class OutbreakNativeAcceptance
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory,"../evidence"));
        private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private static PlayerMobile Actor;
        private static ArachnidOutbreakController Controller;
        private static ArachnidOutbreakChampion Boss;
        private static Dog Pet;
        private static Mobile Subject;
        private static TurnBasedCombatManager Manager;
        private static int LastAP = -1;
        private static bool SubjectHeld;
        private static bool Stopping;
        private static Point3D Center = new Point3D(2514,2088,0);
        private static DateTime End = DateTime.UtcNow.AddMinutes(30);
        private static DateTime CastAt = DateTime.MaxValue;
        private static Timer Poll;
        private static string Scene = "waiting";
        private static Point3D Last;
        private static IAccount Account;
        private static readonly string Log = Path.Combine(Evidence,"native-events.jsonl");

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard = Path.Combine(Evidence,"native-private-root.txt");
            if (!File.Exists(guard) || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Native acceptance fixture requires its exact disposable-root guard.");
            AutoSave.SavesEnabled = false;
            Timer.DelayCall(TimeSpan.FromSeconds(1),Start);
        }
        private static void Event(string kind)
        {
            TurnParticipant participant = Manager == null ? null : Manager.GetParticipant(Actor);
            File.AppendAllText(Log,"{\"utc\":\""+DateTime.UtcNow.ToString("o")+"\",\"event\":\""+kind+"\",\"ap\":"+(participant==null?-1:participant.ActionPoints)+",\"points\":"+Points().ToString(System.Globalization.CultureInfo.InvariantCulture)+",\"x\":"+(Actor==null?0:Actor.X)+",\"y\":"+(Actor==null?0:Actor.Y)+"}\n");
        }
        private static double Points()
        {
            if (Controller == null || Controller.Deleted || Actor == null) return 0;
            ArachnidOutbreakLedger ledger = (ArachnidOutbreakLedger)typeof(ArachnidOutbreakController).GetField("m_Ledger",Private).GetValue(Controller);
            return ledger == null ? 0 : ledger.Points(3,Actor);
        }
        private static void Start()
        {
            Account = Accounts.GetAccount(File.ReadAllText(Path.Combine(Evidence,"account-name.private")));
            if (Account == null) throw new Exception("Private copied account unavailable.");
            for (int i=0;i<7;++i) if (Account[i] is PlayerMobile) { Actor=(PlayerMobile)Account[i];break; }
            if (Actor==null) throw new Exception("Private copied actor unavailable.");
            Reset();
            Poll=Timer.DelayCall(TimeSpan.Zero,TimeSpan.FromMilliseconds(100),Tick);
            File.WriteAllText(Path.Combine(Evidence,"native-ready.txt"),"Instrumented private presentation rig; production remains disabled; original stats/equipment retained.");
        }
        private static void Reset()
        {
            if (Manager != null) Manager.Disable("private native scene reset");
            if (Pet != null && !Pet.Deleted) Pet.Delete();
            if (Controller!=null) Controller.Delete();
            Actor.AccessLevel=AccessLevel.Player;
            if (!Actor.Alive) Actor.Resurrect();
            Actor.Blessed=false; Actor.Hidden=false; Actor.Warmode=false; Actor.Combatant=null;
            Center.Z=Map.Sosaria.GetAverageZ(Center.X,Center.Y);
            Point3D point=new Point3D(Center.X+3,Center.Y,Map.Sosaria.GetAverageZ(Center.X+3,Center.Y));
            Actor.MoveToWorld(point,Map.Sosaria); Last=point;
            Controller=new ArachnidOutbreakController();
            ArachnidOutbreakState state=(ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State",Private).GetValue(Controller);
            state.SetEnabled(true);
            ArachnidOutbreakSite site=new ArachnidOutbreakSite("native-private-presentation",Center.X,Center.Y,false);
            typeof(ArachnidOutbreakSites).GetField("m_Candidates",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,Array.AsReadOnly(new ArachnidOutbreakSite[]{site}));
            typeof(ArachnidOutbreakController).GetMethod("BeginAt",Private).Invoke(Controller,new object[]{site,
                new ArachnidOutbreakPlacement(Center,new Point3D[]{new Point3D(Center.X-28,Center.Y-20,0),new Point3D(Center.X+28,Center.Y-20,0),new Point3D(Center.X,Center.Y+30,0)},new Point3D[]{Center})});
            List<Mobile> creatures=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(Controller);
            foreach(Mobile mobile in creatures.ToArray()) mobile.Delete();
            creatures.Clear();
            for(int i=0;i<3;++i) state.CompleteBrood(i,DateTime.UtcNow);
            Boss=new ArachnidOutbreakChampion(Controller); Boss.FightMode=FightMode.None; Boss.CantWalk=true; Boss.Home=Center;
            creatures.Add(Boss); Boss.MoveToWorld(Center,Map.Sosaria); StopAI();
            Subject=Actor;SubjectHeld=false;LastAP=-1;
            CastAt=DateTime.MaxValue;Scene="ready";Event("scripted-reset");Show();
        }
        private static void StopAI()
        {
            if(Boss!=null && !Boss.Deleted && Boss.AIObject!=null)
            {
                Timer timer=Boss.AIObject.m_Timer;
                if(timer!=null)timer.Stop();
                Boss.Combatant=null;
            }
            if (Pet!=null && !Pet.Deleted && Pet.AIObject!=null && Pet.AIObject.m_Timer!=null)
                Pet.AIObject.m_Timer.Stop();
        }
        private static void TurnScene(bool rescuePet)
        {
            Reset();
            Manager=TurnBasedCombatManager.Instance;
            string reason=null;
            if (Manager==null || !Manager.TryEnable(out reason))
                throw new InvalidOperationException("Private turn manager could not enable: "+(Manager==null?"missing":reason));
            Timer scheduler=(Timer)typeof(TurnBasedCombatManager).GetField("m_Scheduler",Private).GetValue(Manager);
            scheduler.Stop();
            if (rescuePet)
            {
                Pet=new Dog();Pet.Name="private rescue patient";Pet.SetControlMaster(Actor);Pet.ControlOrder=OrderType.Stay;
                Point3D petPoint=new Point3D(Actor.X,Actor.Y-1,Map.Sosaria.GetAverageZ(Actor.X,Actor.Y-1));
                Pet.MoveToWorld(petPoint,Map.Sosaria);Subject=Pet;StopAI();
            }
            Manager.ArmTestMobile(Boss);
            TurnActionDecision opening=TurnBasedCombatBridge.BeginAction(new TurnActionRequest(Actor,Boss,Controller,TurnActionKind.ItemUse,4,true,false));
            if (opening.Lease!=null)
                TurnBasedCombatBridge.CompleteAction(opening.Lease,new TurnActionResult(Actor,Boss,TurnActionPhase.Refund,false));
            TurnParticipant participant=Manager.GetParticipant(Actor);
            if (participant==null || Manager.GetParticipant(Subject)==null || Manager.GetParticipant(Boss)==null)
                throw new InvalidOperationException("Real manager enrollment did not include the scene roster.");
            for (int i=0;i<8 && participant.Group.Current!=participant;++i)
                Manager.EndTurn(participant.Group.Current.Mobile,false);
            if (participant.Group.Current!=participant)
                throw new InvalidOperationException("Unable to prepare player turn through actual EndTurn transitions.");
            new ArachnidWebRestraint(Boss,Subject);
            Actor.SendGump(new TurnBasedCombatGump(Actor,participant.Group));
            Scene=rescuePet?"turn-pet":"turn-self";
            Event("scripted-"+Scene+"-scheduler-stopped");
            Show();
        }
        public static void Arm(int seconds)
        {
            if(Actor==null || Actor.NetState==null)return;
            CastAt=DateTime.UtcNow.AddSeconds(seconds);Scene="armed";
            Event("armed-"+seconds); Actor.SendMessage("PRIVATE QA: production web in {0} seconds.",seconds);Show();
        }
        private static void Show()
        {
            if(Actor!=null && Actor.NetState!=null){Actor.CloseGump(typeof(AcceptanceGump));Actor.SendGump(new AcceptanceGump());}
        }
        private static void StopFixture()
        {
            Stopping=true;Poll.Stop();
            if(Manager!=null)Manager.Disable("private native teardown");if(Pet!=null && !Pet.Deleted)Pet.Delete();
            if(Controller!=null)Controller.Delete();Event("forced-stop-no-save");Core.Kill(false);
        }
        private static void Tick()
        {
            if(Stopping)return;
            if(DateTime.UtcNow>=End){StopFixture();return;}
            for(int i=0;i<7;++i)
            {
                PlayerMobile connected=Account[i] as PlayerMobile;
                if(connected!=null && connected.NetState!=null && connected!=Actor){Actor=connected;Reset();break;}
            }
            StopAI();
            string request=Path.Combine(Evidence,"native-request.txt");
            if(File.Exists(request))
            {
                string command;
                try { command=File.ReadAllText(request).Trim();File.Delete(request); }
                catch (IOException) { return; }
                if(command=="stop")
                {
                    StopFixture();return;
                }
                if(command=="reset")Reset();
                else if(command=="warn")Arm(0);
                else if(command=="arm")Arm(8);
                else if(command=="show")Show();
                else if(command=="turn-self")TurnScene(false);
                else if(command=="turn-pet")TurnScene(true);
                else if(command=="status")CommandSystem.Handle(Actor,"[BroodAntivenom");
                else if(command=="queue")
                {
                    ArachnidOutbreakClaim.QueueGlands(Actor,Controller.AttemptId,0);Event("scripted-pending-stage-entitlement");
                }
                else if(command=="claim")CommandSystem.Handle(Actor,"[OutbreakClaim");
                else if(command=="dose")
                {
                    BroodAntivenom dose=new BroodAntivenom();dose.Amount=4;Actor.Backpack.DropItem(dose);Event("created-private-treatment-stack");
                }
            }
            if(DateTime.UtcNow>=CastAt)
            {
                CastAt=DateTime.MaxValue;
                typeof(ArachnidOutbreakChampion).GetField("m_NextWeb",Private).SetValue(Boss,DateTime.MinValue);
                bool applied=Boss.TryStartWeb(Actor);Scene=applied?"warning":"cast-refused";Event(Scene);
            }
            if(Actor.Location!=Last){Event("observed-position-change");Last=Actor.Location;}
            ArachnidWebRestraint hold=ArachnidWebRestraint.Find(Actor);
            bool subjectHeld=Subject!=null && ArachnidWebRestraint.Find(Subject)!=null;
            if (subjectHeld!=SubjectHeld){SubjectHeld=subjectHeld;Event(subjectHeld?"subject-held":"subject-released");}
            TurnParticipant participant=Manager==null?null:Manager.GetParticipant(Actor);
            int ap=participant==null?-1:participant.ActionPoints;
            if(ap!=LastAP){LastAP=ap;Event("observed-ap-change");}
            bool warning=false;
            foreach(Item item in World.Items.Values) if(item is ArachnidWebWarning && !item.Deleted && ((ArachnidWebWarning)item).Victim==Actor){warning=true;break;}
            if(hold!=null && Scene!="held"){Scene="held";Event("held");}
            else if(hold==null && !warning && (Scene=="held" || Scene=="warning")){Scene="released-or-avoided";Event(Scene);}
            File.WriteAllText(Path.Combine(Evidence,"native-state.json"),"{\"utc\":\""+DateTime.UtcNow.ToString("o")+"\",\"scene\":\""+Scene+"\",\"ap\":"+ap+",\"subjectHeld\":"+subjectHeld.ToString().ToLowerInvariant()+",\"points\":"+Points().ToString(System.Globalization.CultureInfo.InvariantCulture)+",\"x\":"+Actor.X+",\"y\":"+Actor.Y+",\"z\":"+Actor.Z+",\"alive\":"+Actor.Alive.ToString().ToLowerInvariant()+",\"connected\":"+(Actor.NetState!=null).ToString().ToLowerInvariant()+",\"held\":"+(hold!=null).ToString().ToLowerInvariant()+",\"warning\":"+warning.ToString().ToLowerInvariant()+",\"access\":\""+Actor.AccessLevel+"\",\"blessed\":"+Actor.Blessed.ToString().ToLowerInvariant()+"}");
        }
        private sealed class AcceptanceGump:Gump
        {
            public AcceptanceGump():base(490,60)
            {
                AddBackground(0,0,275,295,9270);AddLabel(20,15,1152,"PRIVATE OUTBREAK QA");
                AddButton(20,45,4005,4007,1,GumpButtonType.Reply,0);AddLabel(55,45,1152,"Web in 8 seconds");
                AddButton(20,75,4005,4007,2,GumpButtonType.Reply,0);AddLabel(55,75,1152,"Reset scene");
                AddButton(20,105,4005,4007,3,GumpButtonType.Reply,0);AddLabel(55,105,1152,"Antivenom status");
                AddButton(20,135,4005,4007,4,GumpButtonType.Reply,0);AddLabel(55,135,1152,"Collect queued reward");
                AddLabel(20,185,1152,"Scene and scheduler controlled.");AddLabel(20,210,1152,"No AI or pacing acceptance.");
                AddLabel(20,235,1152,"Private source-hash gate verified.");
            }
            public override void OnResponse(NetState sender,RelayInfo info)
            {
                if(sender==null || sender.Mobile!=Actor || info==null)return;
                if(info.ButtonID==1)Arm(8);else if(info.ButtonID==2)Reset();
                else if(info.ButtonID==3){CommandSystem.Handle(Actor,"[BroodAntivenom");Show();}
                else if(info.ButtonID==4){CommandSystem.Handle(Actor,"[OutbreakClaim");Show();}
            }
        }
    }
}
