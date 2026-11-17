using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Knives.TownHouses;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Multis;
using Server.Regions;
using Server.Custom.Confictura.Stabling;

// Runs against the compiled production scripts, outside the runtime script tree.
internal static class StableRegression
{
    private const BindingFlags Hidden = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    private static int s_Checks;
    private static Map s_Map;

    private sealed class Actor : PlayerMobile
    {
        public bool Dead;
        public override bool Alive { get { return !Dead && base.Alive; } }
    }

    private sealed class BrokenHorse : PackHorse
    {
        public override void OnRegionChange(Region oldRegion, Region newRegion)
        {
            if (Map == Map.Internal)
                throw new InvalidOperationException("Injected internalization callback failure");
            base.OnRegionChange(oldRegion, newRegion);
        }
    }

    private sealed class ReturnHorse : PackHorse
    {
        public bool Fail;
        public override void OnRegionChange(Region oldRegion, Region newRegion)
        {
            if (Fail && Map != Map.Internal)
                throw new InvalidOperationException("Injected return callback failure");
            base.OnRegionChange(oldRegion, newRegion);
        }
    }

    private static void Check(bool value, string message)
    {
        if (!value)
            throw new Exception("FAILED: " + message);
        ++s_Checks;
    }

    private static Actor Player()
    {
        Actor owner = new Actor();
        owner.Player = true;
        owner.Body = 400;
        owner.Name = "stable regression";
        owner.RawStr = owner.RawDex = owner.RawInt = 100;
        owner.Hits = 100;
        owner.AddItem(new Backpack());
        owner.MoveToWorld(new Point3D(100, 100, 0), s_Map);
        return owner;
    }

    private static T Pet<T>(PlayerMobile owner) where T : BaseCreature, new()
    {
        T pet = new T();
        pet.MoveToWorld(owner.Location, owner.Map);
        Check(pet.SetControlMaster(owner), "fixture ownership assigned");
        return pet;
    }

    private static StableAccount Stable(PlayerMobile owner, BaseCreature pet)
    {
        Check(StableService.TryStable(owner, pet, false, true, null), "production stable intake");
        return StableService.FindAccount(owner, pet);
    }

    private static void Set(object obj, string property, object value)
    {
        obj.GetType().GetProperty(property).GetSetMethod(true).Invoke(obj, new object[] { value });
    }

    private static void TestProtection()
    {
        Actor owner = Player();
        int capacity = AnimalTrainer.GetMaxStabled(owner);
        for (int i = 0; i < capacity; ++i)
            Stable(owner, Pet<PackHorse>(owner));
        PackHorse horse = Pet<PackHorse>(owner);
        Bag bag = new Bag();
        bag.DropItem(new Gold(200));
        horse.Backpack.DropItem(bag);
        owner.AutoStablePets();
        Check(!horse.IsStabled, "delayed logout still respects capacity and loaded-pack restrictions");
        owner.BedrollLogout = true;
        owner.OnDisconnected();
        Console.WriteLine("Protection fixture: delay={0}, stable={1}, pending={2}, count={3}/{4}, followers={5}, owner={6}, controlled={7}",
            owner.GetLogoutDelay(), horse.IsStabled, owner.AutoStabled.Contains(horse), owner.Stabled.Count,
            capacity, owner.AllFollowers.Count, horse.ControlMaster == owner, horse.Controlled);
        Check(horse.IsStabled && owner.AutoStabled.Contains(horse) && owner.Stabled.Count == capacity + 1,
            "instant disconnection protects loaded horse despite full stable before event dispatch");
        Check(bag.Parent == horse.Backpack && bag.FindItemByType(typeof(Gold)).Amount == 200,
            "nested cargo intact");
        DateTime due = StableService.FindAccount(owner, horse).NextBillUtc;
        owner.AutoStablePets();
        owner.OnDisconnected();
        Check(owner.Stabled.Count == capacity + 1 && StableService.FindAccount(owner, horse).NextBillUtc == due,
            "logout fallback and repeat disconnect do not duplicate or reset billing");
        owner.Dead = true;
        owner.ClaimAutoStabledPets();
        Check(owner.AutoStabled.Contains(horse) && horse.IsStabled, "ghost preserves pending return");
        owner.Dead = false;
        owner.Followers = owner.FollowersMax;
        owner.ClaimAutoStabledPets();
        Check(owner.AutoStabled.Contains(horse), "follower limit preserves pending return");
        owner.Followers = 0;
        owner.AutoStabled.Add(null);
        owner.ClaimAutoStabledPets();
        Check(!horse.IsStabled && horse.ControlMaster == owner && owner.AutoStabled.Count == 0,
            "later login returns horse and null cleanup is safe");
        Check(!StableService.StableManually(owner, horse, capacity, null), "manual capacity remains enforced");
        typeof(PlayerMobile).GetMethod("EventSink_Connected", Hidden).Invoke(null, new object[] { new ConnectedEventArgs(owner) });
        owner.AutoStablePets();
        Check(!horse.IsStabled && !owner.BedrollLogout, "reconnect resets captured protection and bedroll state");

        Actor summonOwner = Player();
        summonOwner.BedrollLogout = true;
        PackHorse summon = Pet<PackHorse>(summonOwner);
        summon.Summoned = true;
        summonOwner.AutoStablePets();
        Check(!summon.IsStabled && !summon.Deleted, "same-map summon retains existing exclusion");

        Actor first = Player();
        first.BedrollLogout = true;
        PackHorse valid = Pet<PackHorse>(first);
        Actor second = Player();
        PackHorse foreign = Pet<PackHorse>(second);
        first.AllFollowers.Add(foreign);
        BrokenHorse broken = Pet<BrokenHorse>(first);
        first.OnDisconnected();
        Check(valid.IsStabled && foreign.ControlMaster == second && !first.Stabled.Contains(foreign),
            "foreign follower rejected and another pet's callback cannot abort remaining protection");
        Check(first.Stabled.Contains(broken) && broken.IsStabled && broken.Map == Map.Internal,
            "partially internalized pet retains stable ownership record");

        Actor mixed = Player();
        mixed.BedrollLogout = true;
        PackHorse pending = Pet<PackHorse>(mixed);
        mixed.AutoStablePets();
        PackHorse active = Pet<PackHorse>(mixed);
        mixed.Internalize();
        typeof(PlayerMobile).GetMethod("CheckPets", Hidden).Invoke(null, null);
        Check(active.IsStabled && mixed.AutoStabled.Contains(pending), "startup handles mixed pending and active followers");

        XmlDocument xml = new XmlDocument();
        xml.LoadXml("<region name='stable public fixture'><rect x='200' y='200' width='10' height='10'/></region>");
        PublicRegion region = new PublicRegion(xml.DocumentElement, s_Map, null);
        region.Register();
        Actor publicOwner = Player();
        publicOwner.MoveToWorld(new Point3D(202, 202, 0), s_Map);
        Check(publicOwner.GetLogoutDelay() == TimeSpan.Zero, "production PublicRegion grants instant logout");
        PackHorse publicHorse = Pet<PackHorse>(publicOwner);
        publicOwner.OnDisconnected();
        Check(publicHorse.IsStabled, "public-area disconnect protects pet");
        region.Unregister();

        Actor houseOwner = Player();
        TownHouseSign sign = new TownHouseSign();
        sign.Blocks.Add(new Rectangle2D(300, 300, 12, 12));
        sign.MinZ = 0;
        sign.MaxZ = 30;
        sign.Map = s_Map;
        TownHouse house = new TownHouse(houseOwner, sign, 1000, 10);
        sign.House = house;
        house.Location = new Point3D(300, 300, 0);
        house.Map = s_Map;
        house.InitSectorDefinition();
        Knives.TownHouses.RUOVersion.UpdateRegion(sign);
        houseOwner.MoveToWorld(new Point3D(304, 304, 0), s_Map);
        Check(houseOwner.GetLogoutDelay() == TimeSpan.Zero, "production home eligibility grants instant logout");
        PackHorse homeHorse = Pet<PackHorse>(houseOwner);
        houseOwner.OnDisconnected();
        Check(homeHorse.IsStabled, "qualifying home disconnect protects pet");
        Actor visitor = Player();
        visitor.MoveToWorld(new Point3D(305, 304, 0), s_Map);
        Check(visitor.GetLogoutDelay() != TimeSpan.Zero, "unfriended home visitor does not acquire instant-logout protection");
        house.Delete();
    }

    private static void TestBilling()
    {
        Actor owner = Player();
        PackMule mule = Pet<PackMule>(owner);
        Bag bag = new Bag();
        bag.DropItem(new IronIngot(20));
        mule.Backpack.DropItem(bag);
        int rate = 30 + mule.Backpack.TotalWeight;
        StableAccount account = Stable(owner, mule);
        Check(account.WeeklyRate == rate && account.Outstanding == 0, "custom carrier rate includes nested cargo, excludes carrying pack");
        DateTime due = account.NextBillUtc;
        account.Accrue(due.AddTicks(-1));
        Check(account.Outstanding == 0, "no charge before seven days");
        account.Accrue(due);
        Check(account.Outstanding == rate && account.NextBillUtc == due.AddDays(7), "first boundary bills once");
        account.Accrue(due);
        Check(account.Outstanding == rate, "repeated tick cannot duplicate invoice");
        account.Accrue(due.AddDays(21));
        Check(account.Outstanding == rate * 4L, "elapsed real weeks accrue during downtime");
        owner.BankBox.DropItem(new Gold(rate));
        owner.Backpack.DropItem(new Gold(rate));
        Check(!account.Collect(owner, true) && owner.BankBox.GetAmount(typeof(Gold)) == rate
            && owner.Backpack.GetAmount(typeof(Gold)) == rate, "insufficient combined balance is untouched");
        owner.BankBox.DropItem(new BankCheck(rate * 2));
        Check(!account.Collect(owner, false), "recurring collection cannot spend backpack gold");
        Check(account.Collect(owner, true) && account.Outstanding == 0, "explicit collection combines bank, checks and backpack atomically");
        Check(account.Collect(owner, true), "paid collection is idempotent");
        account.Accrue(due.AddDays(28));
        owner.AutoStabled.Add(mule);
        Check(!StableService.TryReturn(owner, mule, false) && mule.Map == Map.Internal
            && mule.IsStabled && owner.AutoStabled.Contains(mule) && bag.Parent == mule.Backpack,
            "unpaid cargo stays safe and pending");
        Set(account, "Outstanding", 0L);
        Set(account, "NextBillUtc", DateTime.UtcNow.AddDays(7));
        Check(StableService.TryReturn(owner, mule, false) && StableService.FindAccount(owner, mule) == null,
            "retrieval ends billing episode");
        StableAccount again = Stable(owner, mule);
        Check(again.NextBillUtc > DateTime.UtcNow.AddDays(6), "new episode receives fresh grace");

        Actor cityOwner = Player();
        CityManagementStone city = new CityManagementStone();
        city.CityTreasury = 100;
        PackHorse cityPet = Pet<PackHorse>(cityOwner);
        Check(StableService.TryStable(cityOwner, cityPet, false, true, city), "city-origin intake");
        StableAccount cityBill = StableService.FindAccount(cityOwner, cityPet);
        cityBill.Accrue(cityBill.NextBillUtc);
        cityOwner.BankBox.DropItem(new BankCheck(100));
        Check(cityBill.Collect(cityOwner, false) && city.CityTreasury == 130, "weekly revenue credits originating city");
        cityBill.Accrue(cityBill.NextBillUtc);
        city.Delete();
        Check(cityBill.Collect(cityOwner, false), "deleted city does not prevent settlement");

        Actor timerOwner = Player();
        StableAccount timerBill = Stable(timerOwner, Pet<PackHorse>(timerOwner));
        Set(timerBill, "NextBillUtc", DateTime.UtcNow.AddDays(-1));
        timerOwner.BankBox.DropItem(new Gold(60));
        typeof(StableService).GetMethod("ProcessAll", Hidden).Invoke(null, null);
        Check(timerBill.Outstanding == 0 && timerOwner.BankBox.GetAmount(typeof(Gold)) == 30,
            "production five-minute billing callback accrues and collects bank funds");
        typeof(StableService).GetMethod("ProcessAll", Hidden).Invoke(null, null);
        Check(timerOwner.BankBox.GetAmount(typeof(Gold)) == 30, "repeated billing callback does not collect twice");
    }

    private static void TestEntrypoints()
    {
        Actor owner = Player();
        PackHorse horse = Pet<PackHorse>(owner);
        horse.Backpack.DropItem(new Gold(5));
        AnimalTrainer trainer = new AnimalTrainer();
        trainer.MoveToWorld(owner.Location, s_Map);
        trainer.EndStable(owner, horse);
        Check(horse.IsStabled, "trainer accepts loaded pet without upfront gold");
        StableAccount bill = StableService.FindAccount(owner, horse);
        Set(bill, "NextBillUtc", DateTime.UtcNow.AddDays(-1));
        trainer.EndClaimList(owner, horse);
        Check(horse.IsStabled, "trainer claim cannot bypass unpaid bill");
        Shepherd shepherd = new Shepherd();
        shepherd.MoveToWorld(owner.Location, s_Map);
        shepherd.Claim(owner);
        Check(horse.IsStabled, "shepherd claim cannot bypass unpaid bill");
        StableStone post = new StableStone();
        post.MoveToWorld(owner.Location, s_Map);
        post.Claim(owner);
        Check(horse.IsStabled, "post claim cannot bypass unpaid bill");
        CityStableStone cityPost = new CityStableStone();
        cityPost.MoveToWorld(owner.Location, s_Map);
        cityPost.Claim(owner);
        Check(horse.IsStabled, "city claim cannot bypass unpaid bill");
        BallOfSummoning ball = new BallOfSummoning();
        ball.Pet = horse;
        ball.Charges = 10;
        ball.SummonPet(owner);
        Check(horse.IsStabled && ball.Charges == 10, "summoning cannot bypass debt or spend failed charge");
        owner.BankBox.DropItem(new Gold(100));
        ball.SummonPet(owner);
        Check(!horse.IsStabled && ball.Charges == 9, "paid summoning returns pet and spends one charge");

        PackHorse shepherdPet = Pet<PackHorse>(owner);
        shepherdPet.Backpack.DropItem(new Gold(1));
        shepherd.EndStable(owner, shepherdPet);
        Check(shepherdPet.IsStabled, "shepherd intake accepts loaded pet without upfront gold");
        Check(StableService.TryReturn(owner, shepherdPet, true), "shepherd intake shares grace and return service");
        owner.Skills.Camping.Base = 100;
        post.Movable = false;
        PackHorse postPet = Pet<PackHorse>(owner);
        postPet.Backpack.DropItem(new Gold(1));
        post.EndStable(owner, postPet);
        Check(postPet.IsStabled, "eligible locked post accepts loaded pet without upfront gold");
        Check(StableService.TryReturn(owner, postPet, true), "post intake shares grace and return service");
        PackHorse civicPet = Pet<PackHorse>(owner);
        civicPet.Backpack.DropItem(new Gold(1));
        cityPost.EndStable(owner, civicPet);
        Check(civicPet.IsStabled, "city intake accepts loaded pet without upfront gold");
        Check(StableService.TryReturn(owner, civicPet, true), "city intake shares grace and return service");

        Actor rider = Player();
        Horse mount = Pet<Horse>(rider);
        mount.Rider = rider;
        AnimalTrainer.DismountPlayer(rider);
        Check(mount.IsStabled && mount.Language == "mount" && StableService.FindAccount(rider, mount) != null,
            "no-mount shelter starts weekly grace");
        StableAccount mountBill = StableService.FindAccount(rider, mount);
        Set(mountBill, "NextBillUtc", DateTime.UtcNow.AddDays(-1));
        AnimalTrainer.GetLastMounted(rider);
        Check(mount.IsStabled && mount.Language == "mount", "unpaid no-mount retrieval preserves retry marker");
        rider.BankBox.DropItem(new Gold(30));
        AnimalTrainer.GetLastMounted(rider);
        Check(!mount.IsStabled && mount.Rider == rider, "paid no-mount retrieval restores rider");

        Actor failedOwner = Player();
        ReturnHorse returning = Pet<ReturnHorse>(failedOwner);
        StableAccount failedBill = Stable(failedOwner, returning);
        failedOwner.AutoStabled.Add(returning);
        Set(failedBill, "NextBillUtc", DateTime.UtcNow.AddDays(-1));
        failedOwner.BankBox.DropItem(new Gold(60));
        returning.Fail = true;
        Check(!StableService.TryReturn(failedOwner, returning, false) && returning.IsStabled
            && failedOwner.AutoStabled.Contains(returning) && failedBill.Outstanding == 0
            && failedOwner.BankBox.GetAmount(typeof(Gold)) == 30,
            "failed movement keeps paid invoice and pending pet safely internalized");
        returning.Fail = false;
        Check(StableService.TryReturn(failedOwner, returning, false)
            && failedOwner.BankBox.GetAmount(typeof(Gold)) == 30, "retry after callback failure cannot double charge");
    }

    private static byte[] Serialize(PlayerMobile owner)
    {
        MemoryStream stream = new MemoryStream();
        BinaryFileWriter writer = new BinaryFileWriter(stream, true);
        owner.Serialize(writer);
        writer.Write(123456789);
        writer.Flush();
        return stream.ToArray();
    }

    private static void TestPersistence()
    {
        Actor owner = Player();
        PackHorse horse = Pet<PackHorse>(owner);
        StableAccount original = Stable(owner, horse);
        original.Accrue(original.NextBillUtc.AddDays(14));
        byte[] bytes = Serialize(owner);
        PlayerMobile loaded = new PlayerMobile((Serial)0x00EF0001);
        BinaryFileReader reader = new BinaryFileReader(new BinaryReader(new MemoryStream(bytes)));
        loaded.Deserialize(reader);
        Check(reader.ReadInt() == 123456789, "version 38 consumes exact positional payload");
        StableAccount restored = StableService.FindAccount(loaded, horse);
        Check(restored != null && restored.Outstanding == original.Outstanding && restored.WeeklyRate == original.WeeklyRate
            && restored.NextBillUtc == original.NextBillUtc, "billing fields survive production serializer roundtrip");

        MemoryStream prefixStream = new MemoryStream();
        BinaryFileWriter prefixWriter = new BinaryFileWriter(prefixStream, true);
        prefixWriter.Write((int)38);
        prefixWriter.Write((bool)typeof(PlayerMobile).GetField("m_ProtectedPetLogout", Hidden).GetValue(owner));
        prefixWriter.Write(owner.StableAccounts.Count);
        foreach (StableAccount account in owner.StableAccounts) account.Serialize(prefixWriter);
        prefixWriter.Flush();
        byte[] prefix = prefixStream.ToArray();
        int start = -1;
        int matches = 0;
        for (int i = 0; i <= bytes.Length - prefix.Length; ++i)
        {
            int j = 0;
            while (j < prefix.Length && bytes[i + j] == prefix[j]) ++j;
            if (j == prefix.Length) { start = i; ++matches; }
        }
        Check(matches == 1, "fixture locates unique version-38 prefix without assuming Mobile offsets");
        int end = start + prefix.Length;
        MemoryStream legacy = new MemoryStream();
        legacy.Write(bytes, 0, start);
        legacy.Write(BitConverter.GetBytes(37), 0, 4);
        legacy.Write(bytes, end, bytes.Length - end);
        PlayerMobile old = new PlayerMobile((Serial)0x00EF0002);
        reader = new BinaryFileReader(new BinaryReader(new MemoryStream(legacy.ToArray())));
        old.Deserialize(reader);
        Check(reader.ReadInt() == 123456789 && old.StableAccounts.Count == 0, "unchanged version 37 payload loads without billing data");
        StableService.Reconcile(old);
        Check(StableService.FindAccount(old, horse).Outstanding == 0
            && StableService.FindAccount(old, horse).NextBillUtc > DateTime.UtcNow.AddDays(6), "legacy stabled pets receive grace without retroactive debt");
    }

    private static void TestBedrollLogout()
    {
        Actor owner = Player();
        Point3D camp = new Point3D(2540, 2110, s_Map.GetAverageZ(2540, 2110));
        owner.MoveToWorld(camp, s_Map);
        owner.Skills.Camping.Cap = 125;
        owner.Skills.Camping.Base = 125;
        SkillCheck.Initialize();
        Check(owner.GetLogoutDelay() != TimeSpan.Zero, "ordinary wilderness logout is delayed");
        Bedroll roll = new Bedroll();
        roll.MoveToWorld(camp, s_Map);
        roll.OnDoubleClick(owner);
        BedrolledOut bed = null;
        foreach (Item item in World.Items.Values)
            if (item is BedrolledOut && ((BedrolledOut)item).Owner == owner && !item.Deleted)
                bed = (BedrolledOut)item;
        Check(roll.Deleted && bed != null, "real bedroll double-click successfully creates camp");
        Check(!owner.BedrollLogout && owner.GetLogoutDelay() == TimeSpan.Zero,
            "real live bed grants instant logout without fixture flag");
        Bedroll spare = new Bedroll();
        owner.AddToBackpack(spare);
        owner.Bedroll = DateTime.MinValue;
        spare.OnDoubleClick(owner);
        Check(!spare.Deleted, "backpack bedroll detects already placed bed at player location");
        Actor other = Player();
        other.MoveToWorld(camp, s_Map);
        Check(!BedrolledOut.CanLogout(other), "another player cannot use the owner's camp");
        owner.MoveToWorld(new Point3D(camp.X + 4, camp.Y, camp.Z), s_Map);
        Check(owner.GetLogoutDelay() != TimeSpan.Zero, "walking beyond three paces revokes instant logout");
        owner.MoveToWorld(camp, s_Map);
        Check(BedrolledOut.CanLogout(owner), "returning to an active safe camp restores eligibility");
        owner.Criminal = true;
        Check(!BedrolledOut.CanLogout(owner), "criminal cannot escape through bedroll logout");
        owner.Criminal = false;
        owner.Combatant = other;
        Check(!BedrolledOut.CanLogout(owner), "active combat revokes instant logout synchronously");
        owner.Combatant = null;
        AggressorInfo aggression = AggressorInfo.Create(other, owner, false);
        owner.Aggressors.Add(aggression);
        Check(!BedrolledOut.CanLogout(owner), "recent incoming aggression blocks logout after combatant clears");
        owner.Aggressors.Clear();
        owner.Aggressed.Add(aggression);
        Check(!BedrolledOut.CanLogout(owner), "recent outgoing aggression blocks logout");
        owner.Aggressed.Clear();
        aggression.Free();
        owner.Dead = true;
        Check(!BedrolledOut.CanLogout(owner), "ghost cannot use a camp");
        owner.Dead = false;
        bed.Owner = other;
        Check(!BedrolledOut.CanLogout(owner), "ownership changes revoke eligibility");
        bed.Owner = owner;
        owner.Internalize();
        Check(!BedrolledOut.CanLogout(owner), "internalized player cannot use a world camp");
        owner.MoveToWorld(camp, s_Map);
        owner.AddToBackpack(bed);
        Check(!BedrolledOut.CanLogout(owner), "contained bed grants no logout protection");
        bed.MoveToWorld(camp, s_Map);
        typeof(BedrolledOut).GetField("m_Created", Hidden).SetValue(bed, DateTime.Now.AddSeconds(-99));
        Check(BedrolledOut.CanLogout(owner), "camp remains eligible before the 100 second boundary");
        typeof(BedrolledOut).GetField("m_Created", Hidden).SetValue(bed, DateTime.Now.AddSeconds(-100));
        Check(owner.GetLogoutDelay() != TimeSpan.Zero, "expired camp blocks logout before timer cleanup");
        bed.RollUp();
        int count = owner.Backpack.FindItemsByType(typeof(Bedroll)).Length;
        bed.RollUp();
        Check(owner.Backpack.FindItemsByType(typeof(Bedroll)).Length == count,
            "repeated roll-up returns only one item");
        Check(!BedrolledOut.CanLogout(owner), "rolled-up bed immediately revokes protection");
        bed = new BedrolledOut(owner);
        bed.MoveToWorld(camp, s_Map);
        Orc enemy = new Orc();
        enemy.MoveToWorld(new Point3D(camp.X + 2, camp.Y, camp.Z), s_Map);
        Check(!BedrolledOut.CanLogout(owner), "nearby hostile creature blocks safe camp");
        enemy.Delete();
        bed.MoveToWorld(new Point3D(camp.X, camp.Y, camp.Z + 100), s_Map);
        Check(!BedrolledOut.CanLogout(owner), "inaccessible bed on another elevation grants no protection");
        bed.MoveToWorld(camp, s_Map);
        int capacity = AnimalTrainer.GetMaxStabled(owner);
        for (int i = 0; i < capacity; ++i)
            Stable(owner, Pet<PackHorse>(owner));
        PackHorse first = Pet<PackHorse>(owner);
        PackHorse second = Pet<PackHorse>(owner);
        Bag cargo = new Bag();
        cargo.DropItem(new Gold(137));
        first.Backpack.DropItem(cargo);
        second.Backpack.DropItem(new IronIngot(20));
        owner.OnDisconnected();
        Check(!owner.BedrollLogout && first.IsStabled && second.IsStabled
            && first.Map == Map.Internal && second.Map == Map.Internal
            && owner.AutoStabled.Contains(first) && owner.AutoStabled.Contains(second)
            && owner.Stabled.Count == capacity + 2,
            "real camp disconnect protects multiple loaded followers beyond stable capacity");
        owner.ClaimAutoStabledPets();
        Check(first.ControlMaster == owner && second.ControlMaster == owner
            && cargo.Parent == first.Backpack && cargo.FindItemByType(typeof(Gold)).Amount == 137
            && second.Backpack.FindItemByType(typeof(IronIngot)).Amount == 20,
            "real camp followers return with their cargo intact");
        bed.Delete();
        Check(!BedrolledOut.CanLogout(owner), "deleted bed grants no protection");
        other.Delete();
        owner.Delete();
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.Expansion = Expansion.SA;
            Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            ScriptCompiler.Assemblies = new Assembly[] { typeof(PlayerMobile).Assembly };
            MapDefinitions.Configure();
            RaceDefinitions.Configure();
            PoisonImpl.Configure();
            typeof(World).GetField("m_Mobiles", Hidden).SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", Hidden).SetValue(null, new Dictionary<Serial, Item>());
            s_Map = Map.Sosaria;
            MultiComponentList.PostHSFormat = true;
            TestBedrollLogout();
            TestProtection();
            TestBilling();
            TestEntrypoints();
            TestPersistence();
            Console.WriteLine("PASS: {0} production component assertions; no saves loaded, timer thread or listeners started.", s_Checks);
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
