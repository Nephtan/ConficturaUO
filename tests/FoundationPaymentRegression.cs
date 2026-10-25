using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

// Explicit historical mode characterizes c747ecd8; it is never current acceptance.
internal static class FoundationPaymentRegression
{
    private static int checks;
    private static bool historical;
    private static void Check(bool value, string text)
    {
        if (!value) throw new Exception("FAILED: " + text);
        ++checks;
    }
    private sealed class Vendor : BaseVendor
    {
        private List<SBInfo> entries = new List<SBInfo>();
        public GenericBuyInfo Stock;
        public GenericBuyInfo Stock2;
        public IShopSellInfo Sell;
        protected override List<SBInfo> SBInfos { get { return entries; } }
        public Vendor() : base("fixture") { }
        public override void InitSBInfo() { }
        public override void InitOutfit() { }
        public override bool CheckVendorAccess(Mobile from) { return true; }
        public override IBuyItemInfo[] GetBuyInfo() { return Stock == null ? new IBuyItemInfo[0] : Stock2 == null ? new IBuyItemInfo[] { Stock } : new IBuyItemInfo[] { Stock, Stock2 }; }
        public override IShopSellInfo[] GetSellInfo() { return Sell == null ? new IShopSellInfo[0] : new IShopSellInfo[] { Sell }; }
    }
    private sealed class Resale : IShopSellInfo
    {
        public int Price;
        public string GetNameFor(Item i) { return "resale fixture"; }
        public int GetSellPriceFor(Item i, int barter) { return Price; }
        public int GetBuyPriceFor(Item i) { return Price; }
        public bool IsSellable(Item i) { return i is Arrow; }
        public bool IsResellable(Item i) { return true; }
        public Type[] Types { get { return new Type[] { typeof(Arrow) }; } }
    }
    private sealed class SlotStock : GenericBuyInfo
    {
        public SlotStock() : base(typeof(Arrow), 10, 10, 0xF3F, 0) { }
        public override int ControlSlots { get { return int.MaxValue; } }
    }
    private static PlayerMobile Player()
    {
        PlayerMobile p = new PlayerMobile();
        p.Body = 400; p.RawStr = 100; p.Player = true;
        p.AddItem(new Backpack()); p.Backpack.DropItem(new Gold(1000));
        return p;
    }
    private static void Deductions()
    {
        Gold direct = new Gold(10);
        direct.Consume(-2);
        Check(direct.Amount == (historical ? 12 : 10), "negative Item.Consume");
        if (!historical)
        {
            direct.Consume(int.MinValue); Check(direct.Amount == 10, "minimum integer deduction");
            direct.Consume(0); Check(direct.Amount == 10, "zero direct");
            direct.Consume(10); Check(direct.Deleted, "exact direct exhaustion");
        }
        for (int kind = 0; kind < 6; ++kind)
        {
            Backpack pack = new Backpack();
            Gold gold = new Gold(10); Arrow arrows = new Arrow(10);
            pack.DropItem(gold); pack.DropItem(arrows);
            int calls = 0;
            OnItemConsumed callback = delegate(Item item, int amount) { ++calls; };
            CheckItemGroup group = delegate(Item a, Item b) { ++calls; return 0; };
            bool success;
            switch (kind)
            {
                case 0: success = pack.ConsumeTotal(typeof(Gold), -2, true, callback); break;
                case 1: success = pack.ConsumeTotal(new Type[] { typeof(Arrow), typeof(Gold) }, new int[] { 3, -2 }, true, callback) == -1; break;
                case 2: success = pack.ConsumeTotal(new Type[][] { new Type[] { typeof(Arrow) }, new Type[] { typeof(Gold) } }, new int[] { 3, -2 }, true, callback) == -1; break;
                case 3: success = pack.ConsumeTotalGrouped(typeof(Gold), -2, true, callback, group); break;
                case 4: success = pack.ConsumeTotalGrouped(new Type[] { typeof(Arrow), typeof(Gold) }, new int[] { 3, -2 }, true, callback, group) == -1; break;
                default: success = pack.ConsumeTotalGrouped(new Type[][] { new Type[] { typeof(Arrow) }, new Type[] { typeof(Gold) } }, new int[] { 3, -2 }, true, callback, group) == -1; break;
            }
            Check(success == historical && gold.Amount == (historical ? 12 : 10), "negative container overload " + kind);
            Check(historical || (arrows.Amount == 10 && calls == 0), "whole request preflight " + kind);
            pack.Delete();
        }
        Backpack up = new Backpack(); Backpack nested = new Backpack(); up.DropItem(nested);
        Gold stack = new Gold(10); nested.DropItem(stack);
        Check(up.ConsumeUpTo(typeof(Gold), -2) == (historical ? -2 : 0) && stack.Amount == (historical ? 12 : 10), "negative nested ConsumeUpTo");
        PlayerMobile player = Player(); player.BankBox.DropItem(new Gold(100));
        Check(Banker.Withdraw(player, -1) == historical && Banker.GetBalance(player) == 100, "negative bank withdrawal");
        if (historical) return;
        Check(Banker.Withdraw(player, 0) && Banker.GetBalance(player) == 100, "zero bank");
        Check(Banker.Withdraw(player, 100) && Banker.GetBalance(player) == 0, "exact bank");
        player.BankBox.DropItem(new BankCheck(100));
        Check(!Banker.Withdraw(player, -1) && Banker.GetBalance(player) == 100, "negative check unchanged");
        Check(Banker.Withdraw(player, 30) && Banker.GetBalance(player) == 70, "partial check withdrawal");
        Backpack valid = new Backpack(); valid.DropItem(new Gold(3));
        Backpack child = new Backpack(); child.DropItem(new Gold(7)); valid.DropItem(child);
        int consumed = 0;
        Check(valid.ConsumeTotal(typeof(Gold), 0, true, delegate(Item i, int a) { consumed += a; }) && consumed == 0, "zero stocked container");
        Check(valid.ConsumeTotal(typeof(Gold), 10, true, delegate(Item i, int a) { consumed += a; }) && consumed == 10 && valid.GetAmount(typeof(Gold)) == 0, "multiple-stack exact deduction");
        Check(!new Backpack().ConsumeTotal(typeof(Gold), 0), "preserve zero empty container result");
        valid.DropItem(new Gold(10)); valid.DropItem(new Arrow(10));
        Check(valid.ConsumeTotalGrouped(new Type[] { typeof(Gold), typeof(Arrow) }, new int[] { 2, 4 }, true, null, delegate(Item a, Item b) { return 0; }) == -1, "normal grouped crafting request");
        Check(valid.GetAmount(typeof(Gold)) == 8 && valid.GetAmount(typeof(Arrow)) == 6, "normal crafting resources");
        Check(valid.ConsumeUpTo(typeof(Gold), 20) == 8, "positive up-to bounded exhaustion");
        for (int kind = 0; kind < 6; ++kind)
        {
            Backpack pack = new Backpack(); pack.DropItem(new Gold(10)); pack.DropItem(new Arrow(10));
            CheckItemGroup group = delegate(Item a, Item b) { return 0; };
            Type[] types = new Type[] { typeof(Gold), typeof(Arrow) };
            Type[][] alternatives = new Type[][] { new Type[] { typeof(Gold) }, new Type[] { typeof(Arrow) } };
            int[] amounts = new int[] { 2, 4 };
            bool success;
            switch (kind)
            {
                case 0: success = pack.ConsumeTotal(typeof(Gold), 2); break;
                case 1: success = pack.ConsumeTotal(types, amounts) == -1; break;
                case 2: success = pack.ConsumeTotal(alternatives, amounts) == -1; break;
                case 3: success = pack.ConsumeTotalGrouped(typeof(Gold), 2, true, null, group); break;
                case 4: success = pack.ConsumeTotalGrouped(types, amounts, true, null, group) == -1; break;
                default: success = pack.ConsumeTotalGrouped(alternatives, amounts, true, null, group) == -1; break;
            }
            Check(success && pack.GetAmount(typeof(Gold)) == 8 && pack.GetAmount(typeof(Arrow)) == ((kind == 0 || kind == 3) ? 10 : 6), "positive overload " + kind);
        }
    }
    private static void Purchase(int price, int amount, bool duplicate, bool accepted, int paid, int delivered)
    {
        PlayerMobile buyer = Player(); Vendor vendor = new Vendor();
        vendor.Stock = new GenericBuyInfo(typeof(Arrow), price, 10, 0xF3F, 0);
        Serial serial = vendor.Stock.GetDisplayEntity().Serial;
        List<BuyItemResponse> request = new List<BuyItemResponse>();
        request.Add(new BuyItemResponse(serial, amount));
        if (duplicate) request.Add(new BuyItemResponse(serial, amount));
        bool result = vendor.OnBuyItems(buyer, request);
        Check(result == accepted, "purchase result " + price + "/" + amount + "/" + duplicate);
        Check(buyer.Backpack.GetAmount(typeof(Gold)) == 1000 - paid, "purchase exact money");
        Check(buyer.Backpack.GetAmount(typeof(Arrow)) == delivered && vendor.Stock.Amount == 10 - delivered, "purchase exact delivery and stock");
        vendor.Delete(); buyer.Delete();
    }
    private static void Purchases()
    {
        Purchase(10, 8, true, historical, historical ? 160 : 0, historical ? 10 : 0);
        Purchase(int.MaxValue, 2, false, historical, historical ? -2 : 0, historical ? 2 : 0);
        if (historical) return;
        Purchase(-1, 2, false, false, 0, 0);
        Purchase(10, 3, false, true, 30, 3);
        Purchase(10, 20, false, true, 100, 10);
        Purchase(0, 3, false, true, 0, 3);
        Purchase(1000, 2, false, false, 0, 0);
        PlayerMobile buyer = Player(); Vendor vendor = new Vendor(); vendor.Stock = new SlotStock();
        Check(!vendor.OnBuyItems(buyer, new List<BuyItemResponse> { new BuyItemResponse(vendor.Stock.GetDisplayEntity().Serial, 2) }), "wide control slot product");
        Check(buyer.Backpack.GetAmount(typeof(Gold)) == 1000 && vendor.Stock.Amount == 10, "slot refusal unchanged resources");
        vendor.Stock = new GenericBuyInfo(typeof(Arrow), int.MaxValue, 10, 0xF3F, 0);
        vendor.Stock2 = new GenericBuyInfo(typeof(Bolt), int.MaxValue, 10, 0x1BFB, 0);
        List<BuyItemResponse> sum = new List<BuyItemResponse> { new BuyItemResponse(vendor.Stock.GetDisplayEntity().Serial, 1), new BuyItemResponse(vendor.Stock2.GetDisplayEntity().Serial, 1) };
        Check(!vendor.OnBuyItems(buyer, sum) && buyer.Backpack.GetAmount(typeof(Gold)) == 1000 && vendor.Stock.Amount == 10 && vendor.Stock2.Amount == 10, "aggregate overflow rejects whole quote");
        buyer.AccessLevel = AccessLevel.GameMaster;
        Check(!vendor.OnBuyItems(buyer, sum) && vendor.Stock.Amount == 10, "staff invalid quote rejected");
        Check(vendor.OnBuyItems(buyer, new List<BuyItemResponse> { sum[0] }) && vendor.Stock.Amount == 9 && buyer.Backpack.GetAmount(typeof(Gold)) == 1000, "staff valid int-max quote preserves free purchase");
        buyer = Player(); vendor = new Vendor();
        vendor.Stock = new GenericBuyInfo(typeof(Arrow), 2000, 10, 0xF3F, 0);
        buyer.BankBox.DropItem(new Gold(5000));
        BuyItemResponse bankLine = new BuyItemResponse(vendor.Stock.GetDisplayEntity().Serial, 2);
        Check(vendor.OnBuyItems(buyer, new List<BuyItemResponse> { bankLine }) && Banker.GetBalance(buyer) == 1000 && buyer.Backpack.GetAmount(typeof(Gold)) == 1000 && vendor.Stock.Amount == 8, "valid bank payment");
        vendor.Stock.Price = 10;
        Check(!vendor.OnBuyItems(buyer, new List<BuyItemResponse> { bankLine, new BuyItemResponse((Serial)0x40012345, -1) }) && vendor.Stock.Amount == 8 && buyer.Backpack.GetAmount(typeof(Gold)) == 1000, "negative request after valid entry rejects before debit");
        foreach (int price in new int[] { int.MaxValue, -1, 10 })
        foreach (bool duplicate in new bool[] { false, true })
        {
            buyer = Player(); vendor = new Vendor(); vendor.Sell = new Resale { Price = price };
            Arrow stock = new Arrow(10); vendor.BuyPack.DropItem(stock);
            List<BuyItemResponse> request = new List<BuyItemResponse> { new BuyItemResponse(stock.Serial, 2) };
            if (duplicate) request.Add(request[0]);
            bool accepted = price == 10 && !duplicate;
            Check(vendor.OnBuyItems(buyer, request) == accepted, "resale response");
            Check(buyer.Backpack.GetAmount(typeof(Gold)) == (accepted ? 980 : 1000) && buyer.Backpack.GetAmount(typeof(Arrow)) == (accepted ? 2 : 0) && stock.Amount == (accepted ? 8 : 10), "resale exact resources " + price + "/" + duplicate);
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            historical = args.Length > 1 && args[1] == "historical";
            Core.Assembly = typeof(Core).Assembly; Core.Expansion = Expansion.SA;
            Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            typeof(World).GetField("m_Mobiles", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new Dictionary<Serial, Item>());
            MapDefinitions.Configure();
            Deductions(); Purchases();
            Console.WriteLine((historical ? "REPRODUCED historical payments" : "PASS current payments") + ": " + checks + " assertions; no saves or listener.");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
