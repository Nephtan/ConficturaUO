using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;

// Run through scripts/Test-WyrmDamage.ps1, outside the live script tree.
// Only in-memory entities and serialized buffers are used; no world save,
// script initialization, timer thread, listener, or client session is started.
internal static class WyrmDamageRegression
{
    private const int Sentinel = 0x13572468;
    private static int s_Checks;

    private sealed class Actor : Mobile
    {
        public Actor() { }
        public Actor(Serial serial) : base(serial) { }
    }

    private static void Check(bool condition, string description)
    {
        if (!condition)
            throw new Exception("FAILED: " + description);
        ++s_Checks;
    }

    private static byte[] Save(Mobile mobile)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFileWriter writer = new BinaryFileWriter(stream, true);
            mobile.Serialize(writer);
            writer.Write(Sentinel);
            writer.Flush();
            return stream.ToArray();
        }
    }

    private static byte[] SaveWyrm(Wyrm wyrm, int version)
    {
        byte[] data = Save(wyrm);
        // Wyrm v0 and v1 both contain only a trailing Int32 after BaseCreature.
        // Changing that marker recreates v0 without mocking either base serializer.
        int versionOffset = data.Length - sizeof(int) * 2;
        Check(BitConverter.ToInt32(data, versionOffset) == 1, "Wyrm writes version 1");
        Buffer.BlockCopy(BitConverter.GetBytes(version), 0, data, versionOffset, sizeof(int));
        return data;
    }

    private static void Load(Mobile mobile, byte[] data)
    {
        using (MemoryStream stream = new MemoryStream(data))
        {
            BinaryFileReader reader = new BinaryFileReader(new BinaryReader(stream));
            mobile.Deserialize(reader);
            Check(reader.Position == data.Length - sizeof(int), "deserializer consumes exactly its record");
            Check(reader.ReadInt() == Sentinel, "following record sentinel remains aligned");
            Check(reader.Position == data.Length, "record has no unread trailing bytes");
        }
    }

    private static void LoadOwner(Actor owner, byte[] data)
    {
        Load(owner, data);
        // Stable membership is serialized by Mobile on the owner. PlayerMobile's
        // Deserialize restores this flag from that list, before or after pet loading.
        foreach (Mobile mobile in owner.Stabled)
        {
            BaseCreature pet = mobile as BaseCreature;
            if (pet != null)
                pet.IsStabled = true;
        }
    }

    private static Wyrm RoundTrip(Wyrm source, Actor owner, int version, bool ownerFirst, out Actor loadedOwner)
    {
        byte[] petData = SaveWyrm(source, version);
        byte[] ownerData = owner == null ? null : Save(owner);
        Wyrm loaded = new Wyrm(source.Serial);
        World.AddMobile(loaded);
        loadedOwner = null;
        if (owner != null)
        {
            loadedOwner = new Actor(owner.Serial);
            World.AddMobile(loadedOwner);
            if (ownerFirst)
                LoadOwner(loadedOwner, ownerData);
        }

        Load(loaded, petData);
        if (owner != null && !ownerFirst)
            LoadOwner(loadedOwner, ownerData);
        return loaded;
    }

    private static int[] Profile(Wyrm wyrm)
    {
        return new int[] { wyrm.PhysicalDamage, wyrm.FireDamage, wyrm.ColdDamage,
            wyrm.PoisonDamage, wyrm.EnergyDamage, wyrm.ChaosDamage, wyrm.DirectDamage };
    }

    private static void SetProfile(Wyrm wyrm, int[] profile)
    {
        wyrm.PhysicalDamage = profile[0];
        wyrm.FireDamage = profile[1];
        wyrm.ColdDamage = profile[2];
        wyrm.PoisonDamage = profile[3];
        wyrm.EnergyDamage = profile[4];
        wyrm.ChaosDamage = profile[5];
        wyrm.DirectDamage = profile[6];
    }

    private static void CheckProfile(Wyrm wyrm, int[] expected, string description)
    {
        int[] actual = Profile(wyrm);
        for (int i = 0; i < expected.Length; ++i)
            Check(actual[i] == expected[i], description + ": channel " + i);
    }

    private static void CheckState(Wyrm source, Wyrm loaded, Actor owner)
    {
        Check(source.Serial == loaded.Serial, "serial is preserved");
        Check(source.Name == loaded.Name && source.Title == loaded.Title, "name and title are preserved");
        Check(source.Body == loaded.Body && source.Hue == loaded.Hue, "appearance is preserved");
        Check(source.Map == loaded.Map && source.Location == loaded.Location, "location is preserved");
        Check(source.RawStr == loaded.RawStr && source.RawDex == loaded.RawDex && source.RawInt == loaded.RawInt,
            "raw stats are preserved");
        Check(source.HitsMax == loaded.HitsMax && source.Hits == loaded.Hits, "hit points are preserved");
        Check(source.Stam == loaded.Stam && source.Mana == loaded.Mana, "stamina and mana are preserved");
        Check(source.DamageMin == loaded.DamageMin && source.DamageMax == loaded.DamageMax,
            "base damage range is preserved");
        Check(source.PhysicalResistanceSeed == loaded.PhysicalResistanceSeed && source.FireResistSeed == loaded.FireResistSeed
            && source.ColdResistSeed == loaded.ColdResistSeed && source.PoisonResistSeed == loaded.PoisonResistSeed
            && source.EnergyResistSeed == loaded.EnergyResistSeed, "resistance seeds are preserved");
        Check(source.Skills.Length == loaded.Skills.Length, "skill list length is preserved");
        for (int i = 0; i < source.Skills.Length; ++i)
        {
            Skill before = source.Skills[i];
            Skill after = loaded.Skills[i];
            Check(before.BaseFixedPoint == after.BaseFixedPoint && before.CapFixedPoint == after.CapFixedPoint
                && before.Lock == after.Lock, "skill " + i + " is preserved");
        }
        Check(source.Controlled == loaded.Controlled && source.ControlOrder == loaded.ControlOrder,
            "control state is preserved");
        Check(loaded.ControlMaster == (source.Controlled ? owner : null), "control master resolves correctly");
        Check(source.IsBonded == loaded.IsBonded && source.IsDeadPet == loaded.IsDeadPet, "bonding state is preserved");
        Check(source.IsStabled == loaded.IsStabled, "stabling state is preserved");
        Check(source.Tamable == loaded.Tamable && source.MinTameSkill == loaded.MinTameSkill
            && source.ControlSlots == loaded.ControlSlots && source.Loyalty == loaded.Loyalty,
            "taming requirements and loyalty are preserved");
        Check(source.Owners.Count == loaded.Owners.Count, "owner history length is preserved");
        if (owner != null)
        {
            Check(loaded.Owners.Count == 1 && loaded.Owners[0] == owner, "owner history resolves correctly");
            Check(owner.Stabled.Contains(loaded) == source.IsStabled, "owner stable membership resolves correctly");
        }
    }

    private static Wyrm CreateSavedPet(string name)
    {
        Wyrm wyrm = new Wyrm();
        wyrm.Name = "Beithir " + name;
        wyrm.Title = "the fixture";
        wyrm.Hue = 1150;
        wyrm.SetStr(974);
        wyrm.SetDex(142);
        wyrm.SetInt(544);
        wyrm.SetHits(492);
        wyrm.Hits = 451;
        wyrm.Stam = 132;
        wyrm.Mana = 503;
        wyrm.SetSkill(SkillName.Magery, 81.7);
        wyrm.Skills.Magery.SetLockNoRelay(SkillLock.Locked);
        wyrm.PhysicalDamage = 65;
        return wyrm;
    }

    private static void TestSavedState(string name, bool owned, bool bonded, bool stabled, bool ownerFirst)
    {
        Wyrm source = CreateSavedPet(name);
        Actor owner = owned ? new Actor() : null;
        source.IsBonded = bonded;
        if (owned)
        {
            source.Owners.Add(owner);
            if (stabled)
            {
                source.IsStabled = true;
                owner.Stabled.Add(source);
            }
            else
            {
                source.Controlled = true;
                source.ControlMaster = owner;
                source.ControlOrder = OrderType.Follow;
            }
        }

        Actor loadedOwner;
        Wyrm loaded = RoundTrip(source, owner, 0, ownerFirst, out loadedOwner);
        int[] corrected = new int[] { 75, 25, 0, 0, 0, 0, 0 };
        CheckProfile(loaded, corrected, name + " legacy repair");
        CheckState(source, loaded, loadedOwner);

        Actor reloadedOwner;
        Wyrm reloaded = RoundTrip(loaded, loadedOwner, 1, !ownerFirst, out reloadedOwner);
        CheckProfile(reloaded, corrected, name + " repaired save reload");
        CheckState(loaded, reloaded, reloadedOwner);
        Console.WriteLine("PASS: " + name + " migration, state preservation, and reload");
    }

    private static void TestProfiles()
    {
        Wyrm spawned = new Wyrm();
        CheckProfile(spawned, new int[] { 75, 25, 0, 0, 0, 0, 0 }, "new spawn");
        int total = 0;
        foreach (int channel in Profile(spawned))
            total += channel;
        Check(total == 100, "new spawn damage totals 100 percent");

        int[][] profiles = new int[][] {
            new int[] { 75, 25, 0, 0, 0, 0, 0 },
            new int[] { 65, 35, 0, 0, 0, 0, 0 },
            new int[] { 60, 25, 0, 0, 0, 0, 0 },
            new int[] { 65, 25, 10, 0, 0, 0, 0 },
            new int[] { 65, 25, 0, 10, 0, 0, 0 },
            new int[] { 65, 25, 0, 0, 10, 0, 0 },
            new int[] { 100, 0, 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0, 0, 0 }
        };
        foreach (int[] profile in profiles)
        {
            Wyrm source = new Wyrm();
            SetProfile(source, profile);
            for (int version = 0; version <= 1; ++version)
            {
                Actor unusedOwner;
                Wyrm loaded = RoundTrip(source, null, version, false, out unusedOwner);
                CheckProfile(loaded, profile, "custom profile version " + version);
            }
        }

        Wyrm newer = CreateSavedPet("version 1 customization");
        Actor unused;
        Wyrm newerLoaded = RoundTrip(newer, null, 1, false, out unused);
        CheckProfile(newerLoaded, Profile(newer), "version 1 does not repeat the migration");

        // BaseCreature currently does not serialize ChaosDamage or DirectDamage.
        // Set each on the receiving instance to exercise the migration guard itself.
        for (int channel = 5; channel <= 6; ++channel)
        {
            Wyrm source = CreateSavedPet("extra channel");
            byte[] data = SaveWyrm(source, 0);
            Wyrm loaded = new Wyrm(source.Serial);
            if (channel == 5)
                loaded.ChaosDamage = 10;
            else
                loaded.DirectDamage = 10;
            Load(loaded, data);
            int[] expected = new int[] { 65, 25, 0, 0, 0, 0, 0 };
            expected[channel] = 10;
            CheckProfile(loaded, expected, "additional channel prevents migration");
        }
        Console.WriteLine("PASS: spawn defaults, custom profiles, version gate, and extra channel guards");
    }

    private static int DealDamage(Wyrm attacker, Mobile target)
    {
        int phys, fire, cold, pois, nrgy, chaos, direct;
        Fists fists = new Fists();
        fists.GetDamageTypes(attacker, out phys, out fire, out cold, out pois, out nrgy, out chaos, out direct);
        return AOS.Damage(target, attacker, 20, false, phys, fire, cold, pois, nrgy, chaos, direct, false, false, false);
    }

    private static void TestCombat()
    {
        Actor target = new Actor();
        target.Body = 400;
        target.RawStr = 100;
        target.Hits = 100;
        Check(target.PhysicalResistance == 0 && target.FireResistance == 0 && target.ColdResistance == 0
            && target.PoisonResistance == 0 && target.EnergyResistance == 0, "combat target has zero resistances");
        Wyrm legacy = CreateSavedPet("combat");
        Actor unused;
        Wyrm repaired = RoundTrip(legacy, null, 0, false, out unused);
        int before = target.Hits;
        Check(DealDamage(legacy, target) == 18, "legacy profile returns 18 damage from 20 input");
        Check(before - target.Hits == 18, "legacy profile actually removes 18 hits");
        before = target.Hits;
        Check(DealDamage(repaired, target) == 20, "repaired profile returns 20 damage from 20 input");
        Check(before - target.Hits == 20, "repaired profile actually removes 20 hits");
        Console.WriteLine("PASS: actual Fists/AOS damage changes from 18 to 20 against zero resistances");
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.Expansion = Expansion.SA;
            Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            typeof(World).GetField("m_Mobiles", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, new Dictionary<Serial, Item>());
            Map.Maps[0x7F] = new Map(0x7F, 0x7F, 0x7F, 16, 16, 0, "Internal", MapRules.Internal);
            TestProfiles();
            TestSavedState("wild", false, false, false, false);
            TestSavedState("controlled", true, false, false, true);
            TestSavedState("bonded", true, true, false, false);
            TestSavedState("stabled owner loads first", true, true, true, true);
            TestSavedState("stabled pet loads first", true, true, true, false);
            TestCombat();
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
