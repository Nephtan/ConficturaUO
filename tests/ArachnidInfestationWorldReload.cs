using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

internal static class ArachnidInfestationWorldReload
{
    private static BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
    private static void Check(bool ok,string text){if(!ok)throw new Exception("FAILED: "+text);}
    private static object Get(object o,string name){return o.GetType().GetField(name,Private).GetValue(o);}
    private static void Bootstrap(string assets)
    {
        Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(assets);
        ScriptCompiler.Assemblies=new Assembly[]{typeof(ArachnidOutbreakController).Assembly};
        MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();World.Load();
    }
    public static int Main(string[] args)
    {
        try
        {
            Check(Path.GetFullPath(Environment.CurrentDirectory).StartsWith("D:\\ConficturaUOTestTools\\",StringComparison.OrdinalIgnoreCase),"disposable world only");
            Bootstrap(args[0]);
            if(args[1]=="save")
            {
                ArachnidOutbreakController c=new ArachnidOutbreakController();((ArachnidOutbreakState)Get(c,"m_State")).SetEnabled(true);
                Check((bool)typeof(ArachnidOutbreakController).GetMethod("BeginInfestation",Private).Invoke(c,new object[]{new Point3D(2514,2088,0),DateTime.UtcNow}),"create real new-format colony");
                ArachnidInfestationState s=(ArachnidInfestationState)Get(c,"m_Infestation");
                for(int i=0;i<16 && s.Patches.Count==1;++i)
                {
                    s.NextGrowth=DateTime.MinValue;typeof(ArachnidOutbreakController).GetMethod("Tick",Private).Invoke(c,new object[]{DateTime.UtcNow});
                }
                Mobile owner=new Mobile();owner.Player=true;owner.RawStr=100;owner.Body=0x190;owner.Hits=owner.HitsMax;owner.AddItem(new Backpack());
                ArachnidOutbreakClaim claim=ArachnidOutbreakClaim.QueueGlands(owner,c.AttemptId,0);
                File.WriteAllText("expected.tsv",String.Join("\t",new string[]{c.Serial.Value.ToString(),c.AttemptId,s.Births.ToString(),s.Patches.Count.ToString(),owner.Serial.Value.ToString(),claim.Serial.Value.ToString(),claim.Items[0].Serial.Value.ToString(),((List<Mobile>)Get(c,"m_Creatures")).Count.ToString(),s.Epoch.ToString()}));
                World.Save(false,false);World.WaitForWriteCompletion();Console.WriteLine("PASS saved new infestation world with existing identities and claim.");
            }
            else
            {
                string[] data=File.ReadAllText("expected.tsv").Split('\t');ArachnidOutbreakController c=World.FindItem((Serial)Int32.Parse(data[0])) as ArachnidOutbreakController;
                Check(c!=null && c.Infestation && c.AttemptId==data[1] && c.Phase==ArachnidOutbreakPhase.Boss,"new mode/controller identity restored");
                ArachnidInfestationState s=(ArachnidInfestationState)Get(c,"m_Infestation");
                Check(s.Births==Int32.Parse(data[2]) && s.Patches.Count==Int32.Parse(data[3]) && s.Epoch==Int32.Parse(data[8]),"patch identities, growth and birth budget restored");
                int queens=0;List<Mobile> creatures=(List<Mobile>)Get(c,"m_Creatures");
                foreach(Mobile mobile in creatures)
                {
                    IArachnidOutbreakOwned owned=mobile as IArachnidOutbreakOwned;
                    Check(owned!=null && owned.Controller==c && owned.AttemptId==c.AttemptId,"loaded back-reference belongs to this exact attempt");
                    if(mobile is ArachnidOutbreakChampion)++queens;
                }
                Check(queens==1 && creatures.Count==Int32.Parse(data[7]),"reload creates no duplicate queen or population");
                int births=s.Births,epoch=s.Epoch;s.Resume(DateTime.UtcNow.AddDays(2));
                Check(s.Births==births && s.Epoch==epoch && s.NextGrowth>DateTime.UtcNow,"restart has no catch-up births or maturation");
                Mobile owner=World.FindMobile((Serial)Int32.Parse(data[4]));ArachnidOutbreakClaim claim=World.FindItem((Serial)Int32.Parse(data[5])) as ArachnidOutbreakClaim;
                if(args[1]=="load")
                {
                    Check(claim!=null && claim.Owner==owner && claim.Items[0].Serial.Value==Int32.Parse(data[6]),"pending exact reward/owner survive reload");
                    Check(claim.TryClaim(owner) && !claim.TryClaim(owner),"loaded claim transfers once");World.Save(false,false);World.WaitForWriteCompletion();
                }
                else Check(claim==null && owner.Backpack.GetAmount(typeof(BroodVenomGland))==1,"paid entitlement does not return after another process reload");
                Console.WriteLine("PASS reloaded new infestation world: "+args[1]+"; no duplicate entities or rewards.");
            }
            return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
