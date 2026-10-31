using System;
using System.Collections.Generic;
using System.Reflection;
using Server;

public static class SectorActivityLeaseRegression
{
    private static int Checks;
    private static List<string> Order=new List<string>();
    private static void Check(bool value,string message){++Checks;if(!value)throw new Exception(message);}
    private sealed class Observer : Mobile
    {
        public int Activated,Deactivated;
        public override void OnSectorActivate(){++Activated;Order.Add("mobile");base.OnSectorActivate();}
        public override void OnSectorDeactivate(){++Deactivated;base.OnSectorDeactivate();}
    }
    public static int Main()
    {
        try
        {
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            Map.Maps[0x7F]=new Map(0x7F,0x7F,0,16,16,0,"internal fixture",MapRules.Internal);
            Map map=new Map(240,240,0,256,256,0,"lease fixture",MapRules.SosariaRules);Map.Maps[240]=map;
            Sector sector=map.GetSector(64,64);Observer observer=new Observer();sector.OnEnter(observer);
            Action<Sector> activating=delegate(Sector current){if(current==sector){Check(!current.Active,"pre-activation hook precedes active state");Order.Add("hook");}};
            Sector.Activating+=activating;
            IDisposable a=sector.AcquireActivityLease(),b=sector.AcquireActivityLease();
            Check(sector.Active && sector.ActivityLeaseCount==2 && observer.Activated==1,"leases are counted and activation is idempotent");
            Check(Order.Count==2 && Order[0]=="hook" && Order[1]=="mobile","admission hook runs before mobile activation");
            sector.Deactivate();Check(sector.Active && observer.Deactivated==0,"ordinary sector sleep cannot freeze leased combat");
            a.Dispose();a.Dispose();Check(sector.ActivityLeaseCount==1 && sector.Active,"duplicate release cannot consume another lease");
            b.Dispose();Check(sector.ActivityLeaseCount==0 && !sector.Active && observer.Deactivated==1,"last lease releases an otherwise unused sector");
            Mobile player=new Mobile();player.Player=true;
            int entries=0;Action<Sector,Mobile> entering=delegate(Sector current,Mobile mobile)
            {
                if(current!=sector)return;
                ++entries;Check(current.Mobiles.Contains(mobile),"entry hook can inspect the arriving actor");
                if(mobile==player)Check(!current.Active,"player admission precedes initial sector activation");
            };
            Sector.MobileEntering+=entering;sector.OnEnter(player);
            IDisposable c=sector.AcquireActivityLease();c.Dispose();
            Check(sector.Active,"lease release preserves a player-relevant sector");
            Mobile second=new Mobile();sector.OnEnter(second);Check(entries==2,"entry hook also runs in an already active sector");
            sector.OnLeave(second);second.Delete();Sector.MobileEntering-=entering;
            sector.OnLeave(player);Check(!sector.Active,"ordinary player departure still permits sleep");
            IDisposable nested=null;Action<Sector> reentrant=delegate(Sector current){if(current==sector && nested==null)nested=current.AcquireActivityLease();};
            Sector.Activating+=reentrant;sector.Activate();Check(nested!=null && sector.ActivityLeaseCount==1,"activation hook can safely lease without recursive activation");
            nested.Dispose();Sector.Activating-=reentrant;Sector.Activating-=activating;
            bool rejected=false;try{Map.Internal.GetSector(0,0).AcquireActivityLease();}catch(InvalidOperationException){rejected=true;}
            Check(rejected,"internal sectors cannot be leased");
            sector.OnLeave(observer);observer.Delete();player.Delete();
            Console.WriteLine("PASS sector leases: "+Checks+" core method checks; no saves, scripts or listener.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
