using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Spells.Research;
using Server.Targeting;

// Exercise production handlers and cleanup without loading saves or starting listeners/timer threads.
internal static class ResearchLogoutRegression
{
    private static int s_Checks;
    private static int s_OtherLogouts;
    private const BindingFlags PrivateStatic = BindingFlags.NonPublic | BindingFlags.Static;

    // Eligibility/resource checks are outside this fixture; targeting and cleanup use production code.
    private sealed class Enchant : ResearchEnchant
    {
        public Enchant(Mobile caster) : base(caster, null) { }
        public override bool CheckSequence() { return true; }
    }

    private sealed class Sneak : ResearchSneak
    {
        private bool m_Success;
        public Sneak(Mobile caster, bool success) : base(caster, null) { m_Success = success; }
        public override bool CheckSequence() { return m_Success; }
    }

    private static void Check(bool condition, string description)
    {
        if (!condition)
            throw new Exception("FAILED: " + description);
        ++s_Checks;
    }

    private static PlayerMobile Player()
    {
        PlayerMobile player = new PlayerMobile();
        player.Name = "research logout fixture";
        player.Player = true;
        player.Body = 400;
        player.RawStr = player.RawDex = player.RawInt = 100;
        player.Hits = 100;
        player.Skills.Magery.Base = 100;
        player.SkillStart = 10000;
        player.AddItem(new Backpack());
        player.MoveToWorld(new Point3D(80, 80, 0), Map.Sosaria);
        return player;
    }

    private static BaseWeapon Weapon(Mobile owner)
    {
        BaseWeapon weapon = new Longsword();
        weapon.Name = "fixture sword";
        weapon.Hue = 33;
        weapon.Attributes.WeaponDamage = 17;
        owner.AddToBackpack(weapon);
        return weapon;
    }

    private static ResearchEnchantStone EnchantWeapon(Mobile owner, BaseWeapon weapon)
    {
        int originalDamage = weapon.Attributes.WeaponDamage;
        Enchant spell = new Enchant(owner);
        spell.OnCast();
        spell.Target(weapon, 35);
        Target.Cancel(owner);
        ResearchEnchantStone orb = owner.Backpack.FindItemByType(typeof(ResearchEnchantStone)) as ResearchEnchantStone;
        Check(orb != null && weapon.Attributes.WeaponDamage == originalDamage + 100, "production target applies enchantment");
        return orb;
    }

    private static void Restored(BaseWeapon weapon, ResearchEnchantStone orb)
    {
        Check(orb.Deleted, "tracking orb is deleted");
        Check(weapon.Name == orb.EnchantName && weapon.Hue == orb.EnchantHue && weapon.Attributes.WeaponDamage == orb.EnchantDmg,
            "each weapon's original name, hue and damage are restored");
    }

    private static void Tick(Timer timer)
    {
        timer.GetType().GetMethod("OnTick", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(timer, null);
    }

    private static Timer Expiry(ResearchEnchantStone orb)
    {
        Type timerType = typeof(ResearchEnchant).GetNestedType("InternalTimer", BindingFlags.NonPublic);
        return (Timer)Activator.CreateInstance(timerType, new object[] { orb, TimeSpan.Zero });
    }

    private static void OtherLogout(LogoutEventArgs e) { ++s_OtherLogouts; }

    private static void TestRegistration()
    {
        EventSink.Reset();
        EventSink.Logout += OtherLogout;
        ResearchEnchant.Initialize();
        ResearchSneak.Initialize();
        ResearchEnchant.Initialize();
        ResearchSneak.Initialize();
        PlayerMobile caster = Player();
        for (int i = 0; i < 25; ++i)
        {
            new Enchant(caster).OnCast();
            Target.Cancel(caster);
            new Sneak(caster, true).OnCast();
            Target.Cancel(caster);
            new Sneak(caster, false).OnCast();
        }
        Delegate handlers = (Delegate)typeof(EventSink).GetField("Logout", PrivateStatic).GetValue(null);
        Delegate[] invocationList = handlers.GetInvocationList();
        Check(invocationList.Length == 3, "repeat initialization and successful/cancelled/failed casts retain only two spell handlers");
        foreach (Delegate handler in invocationList)
            Check(handler.Target == null, "logout handlers do not retain spell instances");
        EventSink.InvokeLogout(null);
        EventSink.InvokeLogout(new LogoutEventArgs(null));
        PlayerMobile deleted = Player();
        deleted.Delete();
        EventSink.InvokeLogout(new LogoutEventArgs(deleted));
        Check(s_OtherLogouts == 3, "initialization preserves unrelated logout subscribers");
        Console.WriteLine("PASS: one static handler per spell, repeated casts/cancellation, invalid events and unrelated subscribers");
    }

    private static void TestEnchantCleanup()
    {
        PlayerMobile owner = Player();
        PlayerMobile other = Player();
        BaseWeapon weapon = Weapon(owner);
        ResearchEnchantStone orb = EnchantWeapon(owner, weapon);
        EventSink.InvokeLogout(new LogoutEventArgs(other));
        Check(!orb.Deleted && weapon.Attributes.WeaponDamage == 117, "unrelated logout preserves enchantment");
        orb.MoveToWorld(owner.Location, owner.Map);
        other.AddToBackpack(weapon);
        Timer oldTimer = Expiry(orb);
        EventSink.InvokeLogout(new LogoutEventArgs(owner));
        Restored(weapon, orb);
        EventSink.InvokeLogout(new LogoutEventArgs(owner));
        Restored(weapon, orb);

        BaseWeapon replacement = Weapon(owner);
        ResearchEnchantStone replacementOrb = EnchantWeapon(owner, replacement);
        Tick(oldTimer);
        Check(!replacementOrb.Deleted && replacement.Attributes.WeaponDamage == 117,
            "old expiry cannot remove an enchantment cast after logout");
        Tick(Expiry(replacementOrb));
        Restored(replacement, replacementOrb);

        BaseWeapon missing = Weapon(owner);
        ResearchEnchantStone missingOrb = EnchantWeapon(owner, missing);
        missing.Delete();
        EventSink.InvokeLogout(new LogoutEventArgs(owner));
        Check(missingOrb.Deleted, "deleted weapon cleanup is safe");
        BaseWeapon unnamed = Weapon(owner);
        ResearchEnchantStone unnamedOrb = EnchantWeapon(owner, unnamed);
        unnamed.Name = null;
        EventSink.InvokeLogout(new LogoutEventArgs(owner));
        Check(unnamedOrb.Deleted && unnamed.Name == null, "null weapon name does not throw or overwrite an unmarked item");
        Console.WriteLine("PASS: owner-only cleanup, moved orbs/weapons, repeat cleanup, expiry, stale timers and missing/unnamed weapons");
    }

    private static ResearchEnchantStone Reload(ResearchEnchantStone orb)
    {
        Mobile owner = orb.EnchantOwner;
        int weaponSerial = orb.EnchantSerial;
        string originalName = orb.EnchantName;
        int originalHue = orb.EnchantHue;
        int originalDamage = orb.EnchantDmg;
        MemoryStream stream = new MemoryStream();
        BinaryFileWriter writer = new BinaryFileWriter(stream, true);
        orb.Serialize(writer);
        writer.Write(123456789);
        writer.Flush();
        byte[] bytes = stream.ToArray();
        Serial serial = orb.Serial;
        orb.Delete();
        ResearchEnchantStone loaded = new ResearchEnchantStone(serial);
        World.AddItem(loaded);
        BinaryFileReader reader = new BinaryFileReader(new BinaryReader(new MemoryStream(bytes)));
        loaded.Deserialize(reader);
        Check(reader.ReadInt() == 123456789, "deserializer consumes exactly the existing save record");
        reader.Close();
        writer.Close();
        Check(loaded.EnchantOwner == owner && loaded.EnchantSerial == weaponSerial
            && loaded.EnchantName == originalName && loaded.EnchantHue == originalHue && loaded.EnchantDmg == originalDamage,
            "all five persisted enchantment fields survive reload");
        return loaded;
    }

    private static void TestLoadedOrbs()
    {
        PlayerMobile first = Player();
        PlayerMobile second = Player();
        BaseWeapon firstWeapon = Weapon(first);
        BaseWeapon secondWeapon = Weapon(second);
        secondWeapon.Name = "second fixture sword";
        secondWeapon.Hue = 44;
        secondWeapon.Attributes.WeaponDamage = 29;
        ResearchEnchantStone firstOrb = Reload(EnchantWeapon(first, firstWeapon));
        ResearchEnchantStone secondOrb = Reload(EnchantWeapon(second, secondWeapon));
        Tick(new ResearchEnchantStone.RunTime(firstOrb));
        Restored(firstWeapon, firstOrb);
        Check(!secondOrb.Deleted && secondWeapon.Attributes.WeaponDamage == 129,
            "one loaded orb timer cannot delete another player's tracking data");
        EventSink.InvokeLogout(new LogoutEventArgs(second));
        Restored(secondWeapon, secondOrb);
        Tick(new ResearchEnchantStone.RunTime(secondOrb));
        Restored(secondWeapon, secondOrb);

        firstOrb = EnchantWeapon(first, firstWeapon);
        secondOrb = EnchantWeapon(second, secondWeapon);
        ResearchEnchant.EndEffects(null);
        Restored(firstWeapon, firstOrb);
        Restored(secondWeapon, secondOrb);
        ResearchEnchant.EndEffects(null);
        Check(((System.Collections.IList)typeof(ResearchEnchantStone).GetField("m_Stones", PrivateStatic).GetValue(null)).Count == 0,
            "deletion unregisters all orbs and all-effects cleanup is idempotent");
        Console.WriteLine("PASS: saved-orb roundtrip, registration after load, independent load timers and complete all-effects cleanup");
    }

    private static void TestSneakCleanup()
    {
        PlayerMobile caster = Player();
        PlayerMobile recipient = Player();
        PlayerMobile hidden = Player();
        hidden.Hidden = true;
        EventSink.InvokeLogout(new LogoutEventArgs(hidden));
        Check(hidden.Hidden, "logout does not reveal a player without Sneak");
        new Sneak(caster, true).Target(recipient);
        Check(ResearchSneak.HasEffect(recipient) && recipient.Hidden, "production Sneak targets another player");
        Check(!recipient.CanBeginAction(typeof(ResearchSneak)), "active Sneak action is held");
        EventSink.InvokeLogout(new LogoutEventArgs(caster));
        Check(ResearchSneak.HasEffect(recipient) && recipient.Hidden, "caster logout preserves recipient's Sneak");
        EventSink.InvokeLogout(new LogoutEventArgs(recipient));
        Check(!ResearchSneak.HasEffect(recipient) && !recipient.Hidden, "recipient logout removes Sneak and reveals them");
        Check(recipient.Skills.Hiding.Value == recipient.Skills.Hiding.Base
            && recipient.Skills.Stealth.Value == recipient.Skills.Stealth.Base
            && recipient.CanBeginAction(typeof(ResearchSneak)), "logout removes both skill modifiers and releases action");
        recipient.Hidden = true;
        EventSink.InvokeLogout(new LogoutEventArgs(recipient));
        Check(recipient.Hidden, "later logout without Sneak preserves ordinary hiding");
        Console.WriteLine("PASS: Sneak recipient cleanup and preservation of unrelated hidden players");
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.Expansion = Expansion.SA;
            Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            typeof(World).GetField("m_Mobiles", PrivateStatic).SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", PrivateStatic).SetValue(null, new Dictionary<Serial, Item>());
            MapDefinitions.Configure();
            TestRegistration();
            TestEnchantCleanup();
            TestLoadedOrbs();
            TestSneakCleanup();
            Console.WriteLine("PASS: " + s_Checks + " assertions; no world saves loaded or listeners started.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
