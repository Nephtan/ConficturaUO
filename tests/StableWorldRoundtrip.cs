using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Custom.Confictura.Stabling;

// Private copied-world serializer verification. Never run in the repository root.
internal static class StableWorldRoundtrip
{
    private static void Require(bool value, string message)
    {
        if (!value) throw new Exception(message);
    }

    public static int Main(string[] args)
    {
        try
        {
            string root = Path.GetFullPath(Environment.CurrentDirectory);
            Require(root.Contains("stabling-verification") && root.Contains("private-world"), "Disposable copied world required");
            Core.Assembly = typeof(Core).Assembly;
            Core.Expansion = Expansion.SA;
            Core.DataDirectories.Add(args[0]);
            ScriptCompiler.Assemblies = new Assembly[] { typeof(PlayerMobile).Assembly };
            MapDefinitions.Configure();
            RaceDefinitions.Configure();
            PoisonImpl.Configure();
            Server.Movement.MovementImpl.Configure();
            World.Load();
            if (args[1] == "write")
            {
                int adopted = 0;
                foreach (Mobile mobile in new List<Mobile>(World.Mobiles.Values))
                {
                    PlayerMobile player = mobile as PlayerMobile;
                    if (player == null) continue;
                    // All existing player records are version 37 in the copied input.
                    Require(player.StableAccounts.Count == 0, "Expected pre-billing copied save");
                    StableService.Reconcile(player);
                    foreach (StableAccount account in player.StableAccounts)
                    {
                        Require(account.Outstanding == 0 && account.NextBillUtc > DateTime.UtcNow.AddDays(6), "Legacy adoption must grant grace");
                        ++adopted;
                    }
                }
                PlayerMobile owner = new PlayerMobile();
                owner.Player = true;
                owner.Name = "private stable roundtrip fixture";
                owner.Body = 400;
                owner.RawStr = 100;
                owner.Hits = 100;
                owner.MoveToWorld(new Point3D(2824, 1005, 0), Map.Sosaria);
                PackHorse pet = new PackHorse();
                pet.MoveToWorld(owner.Location, owner.Map);
                Require(pet.SetControlMaster(owner), "Fixture master assignment");
                Bag bag = new Bag();
                Gold cargo = new Gold(321);
                bag.DropItem(cargo);
                pet.Backpack.DropItem(bag);
                owner.BedrollLogout = true;
                owner.OnDisconnected();
                StableAccount bill = StableService.FindAccount(owner, pet);
                Require(bill != null && owner.AutoStabled.Contains(pet), "Fixture instant shelter");
                bill.Accrue(bill.NextBillUtc.AddDays(14));
                owner.Internalize();
                File.WriteAllLines("fixture.private.txt", new string[] {
                    owner.Serial.Value.ToString(), pet.Serial.Value.ToString(), cargo.Serial.Value.ToString(),
                    bill.WeeklyRate.ToString(), bill.Outstanding.ToString(), bill.NextBillUtc.Ticks.ToString(), adopted.ToString()
                });
                World.Save(false, false);
                Console.WriteLine("PASS: adopted {0} pre-billing stable records; saved version-38 world with pending loaded pet and unpaid billing.", adopted);
            }
            else
            {
                string[] saved = File.ReadAllLines("fixture.private.txt");
                PlayerMobile owner = World.FindMobile((Serial)Int32.Parse(saved[0])) as PlayerMobile;
                BaseCreature pet = World.FindMobile((Serial)Int32.Parse(saved[1])) as BaseCreature;
                Item cargo = World.FindItem((Serial)Int32.Parse(saved[2]));
                Require(owner != null && pet != null && cargo != null, "Fixture identities reload");
                StableAccount bill = StableService.FindAccount(owner, pet);
                Require(bill != null && bill.WeeklyRate == Int32.Parse(saved[3]) && bill.Outstanding == Int64.Parse(saved[4])
                    && bill.NextBillUtc.Ticks == Int64.Parse(saved[5]), "Billing positional roundtrip");
                Require(pet.IsStabled && pet.Map == Map.Internal && pet.ControlMaster == null
                    && owner.Stabled.Contains(pet) && owner.AutoStabled.Contains(pet), "Pending stable ownership roundtrip");
                Require(cargo.Amount == 321 && cargo.IsChildOf(pet.Backpack), "Cargo hierarchy and amount roundtrip");
                long debt = bill.Outstanding;
                StableService.Reconcile(owner);
                bill.Accrue(DateTime.UtcNow);
                Require(bill.Outstanding == debt, "Reload cannot duplicate already accrued billing");
                Console.WriteLine("PASS: independent world reload preserved pet identity, owner stable/pending lists, nested cargo, weekly rate, due date and unpaid balance.");
            }
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
