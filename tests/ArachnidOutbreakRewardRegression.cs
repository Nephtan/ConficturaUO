using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

internal static class ArachnidOutbreakRewardRegression
{
    private static int s_Checks;
    private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAILED: " + message);
        ++s_Checks;
    }
    private static Mobile Player()
    {
        Mobile player = new Mobile();
        player.Player = true;
        player.RawStr = 100;
        player.Body = 0x190;
        player.Hits = player.HitsMax;
        player.AddItem(new Backpack());
        return player;
    }
    private static ArachnidOutbreakLedger Ledger(ArachnidOutbreakController controller)
    {
        return (ArachnidOutbreakLedger)typeof(ArachnidOutbreakController).GetField("m_Ledger", Private).GetValue(controller);
    }
    private static ArachnidOutbreakController Begin()
    {
        ArachnidOutbreakController controller = new ArachnidOutbreakController();
        ArachnidOutbreakState state = (ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State", Private).GetValue(controller);
        state.SetEnabled(true);
        Point3D boss = new Point3D(1000,1000,0);
        ArachnidOutbreakPlacement placement = new ArachnidOutbreakPlacement(boss,
            new Point3D[] { new Point3D(972,980,0), new Point3D(1028,980,0), new Point3D(1000,1030,0) }, new Point3D[] { boss });
        typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(controller,
            new object[] { new ArachnidOutbreakSite("rewards-fixture",1000,1000,false), placement });
        return controller;
    }
    private static List<ArachnidOutbreakClaim> Claims(Mobile player)
    {
        List<ArachnidOutbreakClaim> result = new List<ArachnidOutbreakClaim>();
        foreach (Item item in World.Items.Values)
        {
            ArachnidOutbreakClaim claim = item as ArachnidOutbreakClaim;
            if (claim != null && !claim.Deleted && claim.Owner == player) result.Add(claim);
        }
        return result;
    }
    private static void Scoring()
    {
        Mobile damage = Player();
        Mobile support = Player();
        Mobile patient = Player();
        Mobile npc = new Mobile();
        try
        {
            ArachnidOutbreakLedger ledger = new ArachnidOutbreakLedger("fixture",4320,9792);
            Check(!ledger.CreditDamage(-1,damage,1000) && !ledger.CreditDamage(4,damage,1000)
                && !ledger.CreditDamage(0,damage,0) && !ledger.CreditDamage(0,npc,1000), "invalid events cannot contribute");
            ledger.CreditDamage(0,damage,431);
            Check(ledger.Qualified(0).Count == 0, "substantial but insufficient damage does not qualify");
            ledger.CreditDamage(0,damage,1);
            Check(ledger.Qualified(0).Contains(damage) && ledger.Qualified(1).Count == 0, "ten percent budget qualifies only its own stage");
            Check(!ledger.CreditValidatedSupport(0,support,Double.NaN)
                && !ledger.CreditValidatedSupport(0,support,Double.PositiveInfinity)
                && !ledger.CreditValidatedSupport(0,support,-2), "nonfinite or negative support rejected");
            for (int i=0;i<5;++i) ledger.CreditValidatedSupport(0,support,2);
            Check(ledger.Qualified(0).Contains(support), "independent support qualification without damage");
            Check(!ledger.CreditValidatedHealing(1,support,support,100), "self healing excluded");
            ledger.CreditValidatedHealing(1,support,patient,99);
            Check(ledger.Qualified(1).Count == 0, "healing threshold uses patient maximum health");
            ledger.CreditValidatedHealing(1,support,patient,1);
            Check(ledger.Qualified(1).Contains(support), "effective encounter healing accumulates");
            Check(ledger.Seal(0) && !ledger.Seal(0) && !ledger.CreditDamage(0,patient,4320), "completed stage seals once and rejects later contribution");
            damage.Body=0x192;
            Check(ledger.Qualified(0).Contains(damage) && damage.NetState == null, "dead offline characters retain earned qualification");
            MemoryStream bytes = new MemoryStream();
            BinaryFileWriter writer = new BinaryFileWriter(bytes,true);
            ledger.Serialize(writer); writer.Write(87321); writer.Close();
            BinaryFileReader reader = new BinaryFileReader(new BinaryReader(new MemoryStream(bytes.ToArray())));
            ArachnidOutbreakLedger restored = new ArachnidOutbreakLedger(); restored.Deserialize(reader);
            Check(reader.ReadInt()==87321 && restored.AttemptId==ledger.AttemptId, "ledger positional serializer and identity");
            Check(restored.IsSealed(0) && restored.Qualified(0).Count==2 && restored.Qualified(1).Contains(support), "stage scores and seal persist");
            reader.Close();
            ledger.CreditDamage(3,patient,Int32.MaxValue);
            Check(ledger.Points(3,patient)==10 && ledger.Qualified(3).Contains(patient), "large single hit qualifies without arithmetic overflow");
        }
        finally { damage.Delete();support.Delete();patient.Delete();npc.Delete(); }
    }
    private static void Delivery()
    {
        Mobile player=Player();
        Mobile stranger=Player();
        ArachnidOutbreakController controller=Begin();
        ArachnidOutbreakClaim claim=null;
        try
        {
            List<Mobile> creatures=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(controller);
            ArachnidOutbreakSpider guardian=(ArachnidOutbreakSpider)creatures[0];
            guardian.Damage(432,player);
            Check(Ledger(controller).Qualified(0).Contains(player), "actual damage hook qualifies participant");
            guardian.Kill();
            Check(Claims(player).Count==1 && Ledger(controller).IsSealed(0), "actual guardian death queues and seals stage");
            claim=Claims(player)[0];
            Item gland=claim.Items[0];
            guardian.OnDeath(null);
            Check(Claims(player).Count==1, "duplicate death callback cannot queue again");
            Check(!claim.TryClaim(stranger) && gland.Parent==claim, "another character cannot collect");
            player.Body=0x192;
            Check(!claim.TryClaim(player) && gland.Parent==claim, "dead claim stays intact");
            player.Body=0x190;
            player.Backpack.MaxItems=1;
            player.Backpack.AddItem(new Item(1));
            Check(!claim.TryClaim(player) && gland.Parent==claim && !gland.Deleted, "full backpack retains exact original item");
            controller.Stop(); controller.Delete();
            Check(!claim.Deleted && gland.Parent==claim, "stopping and deleting encounter preserve independent earned item");
            player.Backpack.Items[0].Delete();
            Check(claim.TryClaim(player) && claim.Deleted && gland.Parent==player.Backpack, "available space transfers existing item");
            Check(!claim.TryClaim(player) && player.Backpack.GetAmount(typeof(BroodVenomGland))==1, "repeated collection cannot mint another gland");
            guardian.OnDeath(null);
            Check(Claims(player).Count==0, "paid stage cannot recreate a consumed claim");
            player.Backpack.MaxItems=125;
            ArachnidOutbreakClaim boss=ArachnidOutbreakClaim.QueueGlands(player,"another-attempt",3);
            Check(Object.ReferenceEquals(boss,ArachnidOutbreakClaim.QueueGlands(player,"another-attempt",3)), "pending entitlement enqueue is idempotent");
            Check(boss.TryClaim(player) && player.Backpack.GetAmount(typeof(BroodVenomGland))==4, "boss awards three and merges stack without loss or duplication");
        }
        finally
        {
            foreach(ArachnidOutbreakClaim pending in Claims(player))pending.Delete();
            controller.Delete(); player.Delete(); stranger.Delete();
        }
    }
    private static void Attribution()
    {
        ArachnidOutbreakController controller=Begin();
        Mobile owner=Player();Mobile bard=Player();Mobile replacement=Player();
        DreadSpider pet=new DreadSpider();DreadSpider provoked=new DreadSpider();DreadSpider wild=new DreadSpider();
        try
        {
            List<Mobile> creatures=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(controller);
            pet.Controlled=true;pet.ControlMaster=owner;
            creatures[0].Damage(432,pet);
            Check(Ledger(controller).Qualified(0).Contains(owner), "actual controlled damage credits current owner");
            pet.ControlMaster=replacement;
            creatures[1].Damage(432,pet);
            Check(Ledger(controller).Qualified(1).Contains(replacement) && !Ledger(controller).Qualified(1).Contains(owner), "pet transfer does not credit former owner");
            provoked.BardProvoked=true;provoked.BardMaster=bard;provoked.BardTarget=creatures[1];
            creatures[1].Damage(432,provoked);
            Check(Ledger(controller).Qualified(1).Contains(bard), "actual provocation damage credits bard against designated target");
            creatures[2].Damage(432,provoked);
            creatures[2].Damage(432,wild);
            Check(Ledger(controller).Qualified(2).Count==0, "wrong provocation target and unattributed wild combat earn nothing");
            creatures[2].Delete();
            Check(Claims(owner).Count==0 && Claims(bard).Count==0 && Claims(replacement).Count==0, "administrative deletion produces no stage payout");
        }
        finally {controller.Delete();pet.Delete();provoked.Delete();wild.Delete();owner.Delete();bard.Delete();replacement.Delete();}
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();Corpse.Initialize();
            Scoring(); Delivery(); Attribution();
            Console.WriteLine("PASS outbreak rewards: "+s_Checks+" component assertions; no world saves or listeners.");return 0;
        }
        catch(Exception exception){Console.Error.WriteLine(exception);return 1;}
    }
}
