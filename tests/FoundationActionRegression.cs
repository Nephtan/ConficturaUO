using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Server;
using Server.Engines.BulkOrders;
using Server.Engines.XmlSpawner2;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;

internal static class FoundationActionRegression
{
    private const BindingFlags Private = BindingFlags.NonPublic | BindingFlags.Instance;
    private static int checks;
    private static bool historical;
    private static List<System.Net.Sockets.Socket> sockets = new List<System.Net.Sockets.Socket>();
    private sealed class Capture : NetState
    {
        public List<string> Messages;
        public Capture() : base(null, null) { }
        public override void Send(Packet packet)
        {
            int length; byte[] data = packet.Compile(false, out length);
            if (data[0] == 0xAE && length >= 50) Messages.Add(Encoding.BigEndianUnicode.GetString(data, 48, length - 50).TrimEnd('\0'));
            packet.OnSend();
        }
    }
    public sealed class Reward : Item
    {
        public static int Created;
        public static Action OnCreate;
        public Reward() : base(0xF3F) { ++Created; if (OnCreate != null) OnCreate(); }
    }
    public sealed class BrokenReward
    {
        public BrokenReward() { throw new InvalidOperationException("deliberate factory failure before returning a reward"); }
    }
    private sealed class Pack : Backpack
    {
        public bool RejectHold, RejectDrop, ThrowDrop, DiscardDrop, DeleteDrop;
        public override bool CheckHold(Mobile from, Item item, bool message, bool items, int plusItems, int plusWeight)
        { return !RejectHold && base.CheckHold(from, item, message, items, plusItems, plusWeight); }
        public override bool TryDropItem(Mobile from, Item item, bool message)
        {
            if (ThrowDrop) throw new InvalidOperationException("deliberate drop failure");
            if (DiscardDrop) return true;
            if (DeleteDrop) { item.Delete(); return true; }
            return !RejectDrop && base.TryDropItem(from, item, message);
        }
    }
    private static void Check(bool value, string text)
    { if (!value) throw new Exception("FAILED: " + text); ++checks; }
    private static PlayerMobile Player()
    {
        PlayerMobile p = new PlayerMobile(); p.Name = "action fixture"; p.Body = 400; p.Player = true; p.RawStr = 100;
        p.AddItem(new Pack()); p.Map = Map.Sosaria; p.Location = new Point3D(80, 80, 0);
        Capture state = (Capture)FormatterServices.GetUninitializedObject(typeof(Capture)); state.Messages = new List<string>();
        System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        sockets.Add(socket); // Unbound capture identity only; never connected or listening.
        typeof(NetState).GetField("m_Socket", Private).SetValue(state, socket);
        typeof(NetState).GetField("m_WorldSendLock", Private).SetValue(state, new object());
        typeof(NetState).GetField("m_SendQueue", Private).SetValue(state, new SendQueue());
        foreach (FieldInfo f in typeof(NetState).GetFields(Private))
            if (f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(List<>)) f.SetValue(state, Activator.CreateInstance(f.FieldType));
        typeof(NetState).GetField("m_Mobile", Private).SetValue(state, p);
        typeof(Mobile).GetField("m_NetState", Private).SetValue(p, state);
        return p;
    }
    private static RelayInfo Reply(int button) { return new RelayInfo(button, new int[0], new TextRelay[0]); }
    private static Gump RewardMenu(PlayerMobile p, bool quest) { return quest ? (Gump)new QuestRewardGump(p, 0) : new PointsRewardGump(p, 0); }
    private static int Credits(PlayerMobile p, bool quest) { return quest ? XmlQuestPoints.GetCredits(p) : XmlPoints.GetCredits(p); }
    private static void Rewards()
    {
        foreach (bool quest in new bool[] { true, false })
        {
            ArrayList list = quest ? XmlQuestPointsRewards.RewardsList : XmlPointsRewards.RewardsList;
            list.Clear(); PlayerMobile p = Player();
            if (quest) XmlAttach.AttachTo(p, new XmlQuestPoints { Points = 0, Credits = 100 });
            else XmlAttach.AttachTo(p, new XmlPoints { Points = 0, Credits = 100 });
            object entry = quest ? (object)new XmlQuestPointsRewards(50, typeof(Reward), "fixture", 10, 0, null) : new XmlPointsRewards(50, typeof(Reward), "fixture", 10, 0, null);
            list.Add(entry); int created = Reward.Created;
            RewardMenu(p, quest).OnResponse(p.NetState, Reply(1000));
            Console.WriteLine("Reward observation: quest=" + quest + " created=" + Reward.Created + " before=" + created + " credits=" + Credits(p, quest) + " messages=" + String.Join(";", ((Capture)p.NetState).Messages.ToArray()));
            Check(Reward.Created == created + (historical ? 1 : 0) && Credits(p, quest) == (historical ? 90 : 100), "forged insufficient-points reward " + quest);
            if (historical) continue;
            if (quest) ((XmlQuestPoints)XmlAttach.FindAttachment(p, typeof(XmlQuestPoints))).Points = 100;
            else ((XmlPoints)XmlAttach.FindAttachment(p, typeof(XmlPoints))).Points = 100;
            Gump valid = RewardMenu(p, quest); valid.OnResponse(p.NetState, Reply(1000)); valid.OnResponse(p.NetState, Reply(1000));
            Check(Reward.Created == created + 1 && Credits(p, quest) == 90, "reward once " + quest);
            Gump replaced = RewardMenu(p, quest); list[0] = quest ? (object)new XmlQuestPointsRewards(0, typeof(Reward), "replacement", 1, 0, null) : new XmlPointsRewards(0, typeof(Reward), "replacement", 1, 0, null);
            replaced.OnResponse(p.NetState, Reply(1000));
            Check(Reward.Created == created + 1 && Credits(p, quest) == 90, "replaced offer rejected " + quest);
            list[0] = entry;
            foreach (int cost in new int[] { -1, 200 })
            {
                if (quest) ((XmlQuestPointsRewards)entry).Cost = cost; else ((XmlPointsRewards)entry).Cost = cost;
                RewardMenu(p, quest).OnResponse(p.NetState, Reply(1000));
                Check(Reward.Created == created + 1 && Credits(p, quest) == 90, "invalid/unaffordable cost " + quest);
            }
            if (quest) { ((XmlQuestPointsRewards)entry).Cost = 10; ((XmlQuestPointsRewards)entry).RewardType = typeof(BrokenReward); }
            else { ((XmlPointsRewards)entry).Cost = 10; ((XmlPointsRewards)entry).RewardType = typeof(BrokenReward); }
            int world = World.Items.Count; RewardMenu(p, quest).OnResponse(p.NetState, Reply(1000));
            Check(Credits(p, quest) == 90 && World.Items.Count == world, "factory refusal has no debit/object " + quest);
        }
    }
    private static CityManagementStone City(PlayerMobile mayor)
    {
        CityManagementStone city = new CityManagementStone(); city.Mayor = mayor; city.CityTreasury = 10000;
        city.CityName = "fixture city";
        foreach (FieldInfo field in typeof(CityManagementStone).GetFields(Private))
            if (field.FieldType == typeof(ArrayList) && field.GetValue(city) == null) field.SetValue(city, new ArrayList());
        city.Map = Map.Sosaria; city.Citizens = new ArrayList(); city.Vendors = new ArrayList(); city.Citizens.Add(mayor); mayor.City = city;
        return city;
    }
    private static void Prompts()
    {
        foreach (bool taxes in new bool[] { false, true })
        {
            PlayerMobile p = Player(); CityManagementStone city = City(p);
            Prompt prompt = taxes ? (Prompt)new CityTaxesIncomePrompt(city, p) : new CityTreasuryWithdrawPrompt(city, p);
            city.Mayor = Player(); prompt.OnResponse(p, "10");
            Check(taxes ? city.IncomeTax == (historical ? 10 : 0) : city.CityTreasury == (historical ? 9990 : 10000), "stale mayor authority " + taxes);
            if (historical) continue;
            p.AccessLevel = AccessLevel.GameMaster;
            prompt = taxes ? (Prompt)new CityTaxesIncomePrompt(city, p) : new CityTreasuryWithdrawPrompt(city, p);
            prompt.OnResponse(p, "10");
            Check(taxes ? city.IncomeTax == 10 : city.CityTreasury == 9990, "legitimate staff " + taxes);
        }
    }
    private static void Corpses()
    {
        PlayerMobile p = Player(); p.BankBox.DropItem(new Gold(100));
        CityManagementStone city = City(p); city.CorpseFee = 10;
        CityResurrectionStone stone = new CityResurrectionStone(); stone.Map = p.Map; stone.Location = p.Location; stone.Sign = new CivicSign { Stone = city };
        Gump menu = historical ? (Gump)Activator.CreateInstance(typeof(CityCorpseFeeGump), stone) : (Gump)Activator.CreateInstance(typeof(CityCorpseFeeGump), stone, p);
        menu.OnResponse(p.NetState, Reply(1));
        Check(Banker.GetBalance(p) == (historical ? 90 : 100) && city.CityTreasury == 10000 && stone.Ghosts == null, "missing corpse never charged or quota used");
    }
    private static void Bods()
    {
        PlayerMobile p = Player(); p.Backpack.DropItem(new Gold(100)); ((Pack)p.Backpack).RejectHold = true;
        PlayerVendor vendor = new PlayerVendor(Player(), null); BulkOrderBook book = new BulkOrderBook(); vendor.Backpack.DropItem(book);
        typeof(PlayerVendor).GetMethod("SetVendorItem", Private, null, new Type[] { typeof(Item), typeof(int), typeof(string) }, null).Invoke(vendor, new object[] { book, -1, "fixture" });
        SmallSmithBOD bod = new SmallSmithBOD(0, 10, typeof(Dagger), 0, 0, false, BulkMaterialType.None);
        BOBSmallEntry entry = new BOBSmallEntry(bod); entry.Price = 10; book.Entries.Add(entry); bod.Delete();
        int before = World.Items.Count; int vendorGold = vendor.HoldGold;
        new BODBuyGump(p, book, entry, 0, 10).OnResponse(p.NetState, Reply(2));
        Check(World.Items.Count == before + (historical ? 1 : 0), "full-pack BOD lifetime");
        Check(p.Backpack.GetAmount(typeof(Gold)) == 100 && vendor.HoldGold == vendorGold && book.Entries.Contains(entry), "full-pack BOD resources unchanged");
    }

    private static void RewardMatrix()
    {
        foreach (bool quest in new bool[] { true, false })
        foreach (string scenario in new string[] { "low", "lostpoints", "lostcredits", "negative", "changedcost", "changedtype", "replaced", "changedargs", "wrongactor", "deletedactor", "offpage", "invalidtype", "factory", "dropthrow", "factorychanges", "success", "reenter", "full", "missing", "stack", "mobile", "attachment" })
        {
            PlayerMobile p = Player(); Pack pack = (Pack)p.Backpack;
            XmlAttachment points = quest ? (XmlAttachment)new XmlQuestPoints { Points = 100, Credits = 100 } : new XmlPoints { Points = 100, Credits = 100 };
            XmlAttach.AttachTo(p, points);
            ArrayList list = quest ? XmlQuestPointsRewards.RewardsList : XmlPointsRewards.RewardsList; list.Clear();
            Type type = scenario == "factory" ? typeof(BrokenReward) : scenario == "invalidtype" ? null : scenario == "stack" || scenario == "changedargs" ? typeof(Gold) : scenario == "mobile" ? typeof(RidableLlama) : scenario == "attachment" ? typeof(XmlStr) : typeof(Reward);
            object[] arguments = type == typeof(Gold) ? new object[] { 10 } : type == typeof(XmlStr) ? new object[] { 5 } : null;
            int cost = scenario == "negative" ? -1 : 10;
            object entry = quest ? (object)new XmlQuestPointsRewards(50, type, "fixture", cost, 0, arguments) : new XmlPointsRewards(50, type, "fixture", cost, 0, arguments);
            list.Add(entry);
            if (scenario == "offpage") for (int i = 0; i < 9; ++i) list.Add(entry);
            if (scenario == "low") points.GetType().GetProperty("Points").SetValue(points, 0, null);
            Gump menu = RewardMenu(p, quest);
            if (scenario == "lostpoints") points.GetType().GetProperty("Points").SetValue(points, 0, null);
            if (scenario == "lostcredits") points.GetType().GetProperty("Credits").SetValue(points, 0, null);
            if (scenario == "changedcost") entry.GetType().GetField("Cost").SetValue(entry, 20);
            if (scenario == "changedtype") entry.GetType().GetField("RewardType").SetValue(entry, typeof(Gold));
            if (scenario == "replaced") list[0] = quest ? (object)new XmlQuestPointsRewards(50, type, "fixture", cost, 0, arguments) : new XmlPointsRewards(50, type, "fixture", cost, 0, arguments);
            if (scenario == "changedargs") arguments[0] = 20;
            if (scenario == "full") pack.RejectHold = true;
            if (scenario == "missing") pack.Delete();
            if (scenario == "dropthrow") pack.ThrowDrop = true;
            if (scenario == "stack") pack.DropItem(new Gold(5));
            PlayerMobile actor = scenario == "wrongactor" ? Player() : p;
            NetState response = actor.NetState;
            if (scenario == "deletedactor")
            {
                typeof(Mobile).GetField("m_NetState",Private).SetValue(p,null);
                p.Delete();
            }
            int credits = Credits(p, quest), items = World.Items.Count, mobiles = World.Mobiles.Count;
            if (scenario == "factorychanges") Reward.OnCreate = delegate { points.GetType().GetProperty("Credits").SetValue(points, 0, null); };
            if (scenario == "reenter") Reward.OnCreate = delegate { menu.OnResponse(p.NetState, Reply(1000)); };
            try { menu.OnResponse(response, Reply(scenario == "offpage" ? 1009 : 1000)); menu.OnResponse(response, Reply(1000)); }
            finally { Reward.OnCreate = null; }
            bool success = scenario == "success" || scenario == "reenter" || scenario == "full" || scenario == "missing" || scenario == "stack" || scenario == "mobile" || scenario == "attachment";
            Check(Credits(p, quest) == (scenario == "factorychanges" ? 0 : credits - (success ? 10 : 0)), "XML credit invariant " + quest + "/" + scenario);
            if (scenario == "mobile") Check(World.Mobiles.Count == mobiles + 1, "mobile reward once");
            else Check(World.Items.Count == items + (success && scenario != "stack" && scenario != "attachment" ? 1 : 0), "XML object invariant " + quest + "/" + scenario);
            if (scenario == "attachment") Check(XmlAttach.FindAttachment(p,typeof(XmlStr)) != null && p.Str == p.RawStr + 5, "valid attachment behavior");
            if (scenario == "stack") Check(pack.GetAmount(typeof(Gold)) == 15, "merged item counts as delivery");
            if (scenario == "full" || scenario == "missing")
            {
                int loose = 0; foreach (Item item in World.Items.Values) if (item is Reward && item.Parent == null && item.Map == p.Map && item.Location == p.Location) ++loose;
                Check(loose > 0, "preserved XML ground fallback");
            }
        }
    }
    private static Prompt CityPrompt(bool tax, CityManagementStone city, Mobile p)
    { return tax ? (Prompt)new CityTaxesIncomePrompt(city, p) : new CityTreasuryWithdrawPrompt(city, p); }
    private static void PromptMatrix()
    {
        foreach (bool tax in new bool[] { false, true })
        foreach (string scenario in new string[] { "lostmayor", "demoted", "deleted", "actor", "mayor", "staff", "cancel", "badnumber", "zero", "maximum", "overmaximum", "negative", "exactbalance", "check" })
        {
            PlayerMobile p = Player(); CityManagementStone city = City(p); city.CityTreasury = 2000000;
            CityPlayerVendor vendor = null;
            if (tax) { vendor = new CityPlayerVendor(p, city); city.Vendors.Add(vendor); }
            if (scenario == "demoted" || scenario == "staff") { city.Mayor = Player(); p.AccessLevel = AccessLevel.GameMaster; }
            Prompt prompt = CityPrompt(tax, city, p);
            if (scenario == "lostmayor") city.Mayor = Player();
            if (scenario == "demoted") p.AccessLevel = AccessLevel.Player;
            if (scenario == "deleted") city.Delete();
            PlayerMobile actor = scenario == "actor" ? Player() : p;
            string value = scenario == "zero" ? "0" : scenario == "maximum" ? (tax ? "100" : "1000000") : scenario == "overmaximum" ? (tax ? "101" : "1000001") : scenario == "negative" ? "-1" : scenario == "exactbalance" ? "2000000" : scenario == "check" ? "5000" : scenario == "badnumber" ? "not a number" : "10";
            bool valid = scenario == "mayor" || scenario == "staff" || scenario == "maximum" || (tax && scenario == "zero") || (!tax && scenario == "check");
            int beforeMoney = p.Backpack.GetAmount(typeof(Gold)), beforeTax = city.IncomeTax, beforeTreasury = city.CityTreasury;
            if (scenario == "cancel") prompt.OnCancel(actor);
            prompt.OnResponse(actor, value); prompt.OnResponse(actor, value);
            int amount = valid ? Int32.Parse(value) : 0;
            Check(tax ? city.IncomeTax == (valid ? amount : beforeTax) : city.CityTreasury == beforeTreasury - amount, "city final authority/bounds " + tax + "/" + scenario);
            if (tax) Check(vendor.TaxRate == (valid ? amount : 0), "vendor tax matches authorized change");
            else
            {
                Item[] bankChecks = p.Backpack.FindItemsByType(typeof(BankCheck));
                Check(amount >= 5000 ? bankChecks.Length == 1 && ((BankCheck)bankChecks[0]).Worth == amount : p.Backpack.GetAmount(typeof(Gold)) == beforeMoney + amount && bankChecks.Length == 0, "city exact payment once " + scenario);
            }
        }
    }
    private static void CorpseMatrix()
    {
        foreach (string scenario in new string[] { "missing", "deletedcorpse", "quota", "funds", "changedfee", "moved", "map", "stone", "sign", "replacedsign", "deletedcity", "city", "actor", "negative", "overflow", "newcorpse", "cancel", "success" })
        {
            PlayerMobile p = Player(); p.BankBox.DropItem(new Gold(scenario == "funds" ? 5 : 100)); CityManagementStone city = City(p); city.CorpseFee = scenario == "negative" ? -1 : 10;
            CityResurrectionStone stone = new CityResurrectionStone { Map = p.Map, Location = p.Location, Sign = new CivicSign { Stone = city } };
            Container corpse = new Backpack(); corpse.MoveToWorld(new Point3D(100,100,0), p.Map); p.Corpse = scenario == "missing" ? null : corpse;
            if (scenario == "quota") { stone.Ghosts = new Hashtable(); stone.Ghosts[p] = 3; }
            if (scenario == "overflow") city.CityTreasury = Int32.MaxValue;
            Gump menu = (Gump)Activator.CreateInstance(typeof(CityCorpseFeeGump), stone, p);
            if (scenario == "deletedcorpse") corpse.Delete();
            if (scenario == "changedfee") city.CorpseFee = 20;
            if (scenario == "moved") p.Location = new Point3D(120,120,0);
            if (scenario == "map") p.Map = Map.Lodor;
            if (scenario == "stone") stone.Delete();
            if (scenario == "sign") stone.Sign.Delete();
            if (scenario == "replacedsign") stone.Sign = new CivicSign { Stone = city };
            if (scenario == "deletedcity") city.Delete();
            if (scenario == "city") p.City = City(Player());
            if (scenario == "newcorpse") p.Corpse = new Backpack();
            PlayerMobile actor = scenario == "actor" ? Player() : p;
            int bank = Banker.GetBalance(p), treasury = city.CityTreasury;
            menu.OnResponse(actor.NetState, Reply(scenario == "cancel" ? 2 : 1)); menu.OnResponse(actor.NetState, Reply(1));
            bool success = scenario == "success";
            Check(Banker.GetBalance(p) == bank - (success ? 10 : 0) && city.CityTreasury == treasury + (success ? 10 : 0), "corpse exact resource invariant " + scenario);
            Check(success ? (int)stone.Ghosts[p] == 1 && corpse.Location == p.Location && corpse.Map == p.Map : scenario == "quota" ? (int)stone.Ghosts[p] == 3 : stone.Ghosts == null, "corpse quota/lifecycle invariant " + scenario);
            if (!success && scenario != "actor" && scenario != "cancel") Check(((Capture)p.NetState).Messages.Exists(delegate(string message) { return message.Contains("No money was taken"); }), "accurate corpse refusal message " + scenario);
        }
        PlayerMobile free = Player(); CityResurrectionStone freeStone = new CityResurrectionStone();
        Check(freeStone.RegCorpse(free) && freeStone.RegCorpse(free) && freeStone.RegCorpse(free) && !freeStone.RegCorpse(free) && (int)freeStone.Ghosts[free] == 3, "free-service RegCorpse wrapper unchanged");
        freeStone.ClearGhosts(); Check(freeStone.RegCorpse(free) && (int)freeStone.Ghosts[free] == 1, "weekly quota reset preserves wrapper");
        PlayerMobile paid = Player(); paid.BankBox.DropItem(new Gold(100)); CityManagementStone paidCity = City(paid); paidCity.CorpseFee = 10;
        CityResurrectionStone paidStone = new CityResurrectionStone { Map = paid.Map, Location = paid.Location, Sign = new CivicSign { Stone = paidCity } };
        paid.Corpse = new Backpack();
        for (int attempt = 0; attempt < 5; ++attempt)
        {
            if (attempt == 4) paidStone.ClearGhosts();
            Gump fresh = (Gump)Activator.CreateInstance(typeof(CityCorpseFeeGump), paidStone, paid);
            fresh.OnResponse(paid.NetState, Reply(1));
            int charges = attempt == 4 ? 4 : Math.Min(attempt + 1, 3);
            Check(Banker.GetBalance(paid) == 100 - charges * 10 && paidCity.CityTreasury == 10000 + charges * 10 && (int)paidStone.Ghosts[paid] == (attempt == 4 ? 1 : charges), "paid quota progression/reset " + attempt);
        }
    }
    private static void BodMatrix()
    {
        foreach (string scenario in new string[] { "full", "missing", "funds", "reject", "throw", "discard", "delete", "price", "negative", "overflow", "deletedbook", "deletedvendor", "nullfactory", "entry", "actor", "bank", "success" })
        {
            PlayerMobile p = Player(); Pack pack = (Pack)p.Backpack;
            if (scenario != "funds") { if (scenario == "bank") p.BankBox.DropItem(new Gold(100)); else pack.DropItem(new Gold(100)); }
            PlayerVendor vendor = new PlayerVendor(Player(), null); BulkOrderBook book = new BulkOrderBook(); vendor.Backpack.DropItem(book);
            typeof(PlayerVendor).GetMethod("SetVendorItem", Private, null, new Type[] { typeof(Item), typeof(int), typeof(string) }, null).Invoke(vendor, new object[] { book, -1, "fixture" });
            SmallSmithBOD bod = new SmallSmithBOD(0,10,typeof(Dagger),0,0,false,BulkMaterialType.None); BOBSmallEntry entry = new BOBSmallEntry(bod); entry.Price = scenario == "negative" ? -10 : 10; book.Entries.Add(entry); bod.Delete();
            BODBuyGump menu = new BODBuyGump(p,book,entry,0,entry.Price);
            if (scenario == "full") pack.RejectHold = true;
            if (scenario == "missing") pack.Delete();
            if (scenario == "reject") pack.RejectDrop = true;
            if (scenario == "throw") pack.ThrowDrop = true;
            if (scenario == "discard") pack.DiscardDrop = true;
            if (scenario == "delete") pack.DeleteDrop = true;
            if (scenario == "overflow") vendor.HoldGold = Int32.MaxValue;
            if (scenario == "deletedbook") book.Delete();
            if (scenario == "deletedvendor") vendor.Delete();
            if (scenario == "price") entry.Price = 20;
            if (scenario == "nullfactory") typeof(BOBSmallEntry).GetField("m_DeedType",Private).SetValue(entry,(BODType)99);
            if (scenario == "entry") book.Entries.Remove(entry);
            PlayerMobile actor = scenario == "actor" ? Player() : p;
            int count=World.Items.Count, vendorGold=vendor.HoldGold, money=(p.Backpack==null?0:p.Backpack.GetAmount(typeof(Gold)))+Banker.GetBalance(p);
            bool threw=false; try { menu.OnResponse(actor.NetState,Reply(2)); menu.OnResponse(actor.NetState,Reply(2)); } catch (InvalidOperationException) { threw=true; }
            bool success=scenario=="success" || scenario=="bank";
            int now=(p.Backpack==null?0:p.Backpack.GetAmount(typeof(Gold)))+Banker.GetBalance(p);
            Check(now==money-(success?10:0) && vendor.HoldGold==vendorGold+(success?10:0),"BOD money invariant "+scenario);
            Check(World.Items.Count==count+(success?1:0) && book.Entries.Contains(entry)==(!success && scenario!="entry"),"BOD ownership invariant "+scenario);
            Check(threw==(scenario=="throw"),"BOD exception remains explicit "+scenario);
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            historical = args.Length > 1 && args[1] == "historical";
            Core.Assembly = typeof(Core).Assembly; Core.Expansion = Expansion.SA; Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            typeof(World).GetField("m_Mobiles", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Item>());
            MapDefinitions.Configure(); RaceDefinitions.Configure();
            if (historical) { Rewards(); Prompts(); Corpses(); Bods(); }
            else { RewardMatrix(); PromptMatrix(); CorpseMatrix(); BodMatrix(); }
            Console.WriteLine((historical ? "REPRODUCED historical actions" : "PASS current actions") + ": " + checks + " assertions; no world/listener."); return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
        finally { foreach (System.Net.Sockets.Socket socket in sockets) socket.Close(); }
    }
}
