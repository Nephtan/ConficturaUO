using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Regions;

namespace Server.Custom.Confictura.Stabling
{
    // Embedded in PlayerMobile version 38; this is not a separately saved world object.
    public sealed class StableAccount
    {
        public BaseCreature Pet { get; private set; }
        public int WeeklyRate { get; private set; }
        public DateTime NextBillUtc { get; private set; }
        public long Outstanding { get; private set; }
        public CityManagementStone City { get; private set; }

        public StableAccount(BaseCreature pet, DateTime now, CityManagementStone city)
        {
            Pet = pet;
            WeeklyRate = StableService.GetWeeklyRate(pet);
            NextBillUtc = now.AddDays(7);
            City = city;
        }

        public StableAccount(GenericReader reader)
        {
            int version = reader.ReadInt();
            if (version != 0)
                throw new InvalidOperationException("Unsupported stable account version.");

            Pet = reader.ReadMobile() as BaseCreature;
            WeeklyRate = reader.ReadInt();
            NextBillUtc = DateTime.SpecifyKind(reader.ReadDateTime(), DateTimeKind.Utc);
            Outstanding = reader.ReadLong();
            City = reader.ReadItem() as CityManagementStone;
            if (WeeklyRate < 30 || Outstanding < 0)
                throw new InvalidOperationException("Invalid stable account values.");
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);
            writer.Write(Pet);
            writer.Write(WeeklyRate);
            writer.Write(NextBillUtc);
            writer.Write(Outstanding);
            writer.Write(City);
        }

        public void Accrue(DateTime now)
        {
            if (now < NextBillUtc)
                return;

            long weeks = (now.Ticks - NextBillUtc.Ticks) / TimeSpan.TicksPerDay / 7 + 1;
            // Calculate both before committing either, so a failed check cannot double bill.
            long balance = checked(Outstanding + weeks * WeeklyRate);
            DateTime next = NextBillUtc.AddTicks(checked(weeks * 7 * TimeSpan.TicksPerDay));
            Outstanding = balance;
            NextBillUtc = next;
        }

        public bool Collect(PlayerMobile owner, bool useBackpack)
        {
            if (Outstanding == 0)
                return true;

            if (City != null && !City.Deleted && Outstanding > (long)Int32.MaxValue - City.CityTreasury)
                return false;

            List<Item> funds = new List<Item>();
            Container bank = owner.FindBankNoCreate();
            if (bank != null)
            {
                funds.AddRange(bank.FindItemsByType(typeof(Gold)));
                funds.AddRange(bank.FindItemsByType(typeof(BankCheck)));
            }
            if (useBackpack && owner.Backpack != null)
                funds.AddRange(owner.Backpack.FindItemsByType(typeof(Gold)));

            long available = 0;
            foreach (Item item in funds)
            {
                BankCheck check = item as BankCheck;
                available += check == null ? item.Amount : Math.Max(0, check.Worth);
            }
            if (available < Outstanding)
                return false;

            long remaining = Outstanding;
            foreach (Item item in funds)
            {
                if (remaining == 0)
                    break;

                BankCheck check = item as BankCheck;
                int value = check == null ? item.Amount : Math.Max(0, check.Worth);
                int take = (int)Math.Min(remaining, value);
                if (take == value)
                    item.Delete();
                else if (check != null)
                    check.Worth -= take;
                else
                    item.Amount -= take;
                remaining -= take;
            }

            if (City != null && !City.Deleted)
                City.CityTreasury = (int)(City.CityTreasury + Outstanding);
            Outstanding = 0;
            return true;
        }
    }

    public static class StableService
    {
        public const string Terms = "The first seven days are free. After that, each pet costs 30 gold per real week, plus its pack cargo weight. Unpaid pets and cargo stay safe until the bill is paid.";

        public static void Initialize()
        {
            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(ProcessAll));
            Timer.DelayCall(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5), new TimerCallback(ProcessAll));
        }

        public static int GetWeeklyRate(BaseCreature pet)
        {
            Container pack = pet.Backpack;
            int cargo = pack is StrongBackpack ? Math.Max(0, pack.TotalWeight) : 0;
            return cargo > Int32.MaxValue - 30 ? Int32.MaxValue : 30 + cargo;
        }

        public static bool IsInstantLogout(PlayerMobile owner)
        {
            if (owner.Map != Map.Internal)
                return owner.GetLogoutDelay() == TimeSpan.Zero;
            if (owner.LogoutMap == null || owner.LogoutMap == Map.Internal)
                return false;

            // Legacy saves have no captured bedroll flag; recover region/home protection at their saved location.
            BaseHouse house = BaseHouse.FindHouseAt(owner.LogoutLocation, owner.LogoutMap, 16);
            if (house != null && house.IsFriend(owner) && house.IsInside(owner.LogoutLocation, 16))
                return true;
            return Region.Find(owner.LogoutLocation, owner.LogoutMap).GetLogoutDelay(owner) == TimeSpan.Zero;
        }

        public static StableAccount FindAccount(PlayerMobile owner, BaseCreature pet)
        {
            foreach (StableAccount account in owner.StableAccounts)
                if (account.Pet == pet)
                    return account;
            return null;
        }

        private static StableAccount EnsureAccount(PlayerMobile owner, BaseCreature pet, CityManagementStone city)
        {
            StableAccount account = FindAccount(owner, pet);
            if (account == null)
            {
                account = new StableAccount(pet, DateTime.UtcNow, city);
                owner.StableAccounts.Add(account);
            }
            return account;
        }

        public static void Reconcile(PlayerMobile owner)
        {
            Dictionary<Mobile, bool> seen = new Dictionary<Mobile, bool>();
            for (int i = owner.Stabled.Count - 1; i >= 0; --i)
            {
                BaseCreature pet = owner.Stabled[i] as BaseCreature;
                if (pet == null || pet.Deleted || seen.ContainsKey(pet)
                    || (pet.ControlMaster != null && pet.ControlMaster != owner))
                {
                    owner.Stabled.RemoveAt(i);
                    continue;
                }
                seen.Add(pet, true);
                pet.IsStabled = true;
                EnsureAccount(owner, pet, null);
            }
            for (int i = owner.StableAccounts.Count - 1; i >= 0; --i)
                if (owner.StableAccounts[i].Pet == null || !owner.Stabled.Contains(owner.StableAccounts[i].Pet)
                    || FindAccount(owner, owner.StableAccounts[i].Pet) != owner.StableAccounts[i])
                    owner.StableAccounts.RemoveAt(i);
            for (int i = owner.AutoStabled.Count - 1; i >= 0; --i)
                if (!owner.Stabled.Contains(owner.AutoStabled[i]))
                    owner.AutoStabled.RemoveAt(i);
        }

        private static void ProcessAll()
        {
            foreach (Mobile mobile in new List<Mobile>(World.Mobiles.Values))
            {
                PlayerMobile owner = mobile as PlayerMobile;
                if (owner == null || owner.Deleted)
                    continue;
                try
                {
                    Reconcile(owner);
                    foreach (StableAccount account in owner.StableAccounts)
                    {
                        try
                        {
                            account.Accrue(DateTime.UtcNow);
                            account.Collect(owner, false);
                        }
                        catch (Exception exception)
                        {
                            ReportFailure(owner, account.Pet, exception);
                        }
                    }
                }
                catch (Exception exception)
                {
                    ReportFailure(owner, null, exception);
                }
            }
        }

        public static void ShowAccount(Mobile from, BaseCreature pet)
        {
            PlayerMobile owner = from as PlayerMobile;
            if (owner == null || pet == null || pet.Deleted || !owner.Stabled.Contains(pet))
                return;
            StableAccount account = EnsureAccount(owner, pet, null);
            account.Accrue(DateTime.UtcNow);
            from.SendMessage("{0}: {1} gold/week; next bill {2:u}; outstanding {3} gold.",
                pet.Name, account.WeeklyRate, account.NextBillUtc, account.Outstanding);
        }

        public static bool TryStable(Mobile from, BaseCreature pet, bool automatic, bool protect, CityManagementStone city)
        {
            PlayerMobile owner = from as PlayerMobile;
            if (owner == null || owner.Deleted || pet == null || pet.Deleted || pet.Summoned)
                return false;
            if (owner.Stabled.Contains(pet))
                return pet.Map == Map.Internal && pet.IsStabled;
            if (!pet.Controlled || pet.ControlMaster != owner || (pet is IMount && ((IMount)pet).Rider != null))
                return false;
            if (!protect && owner.Stabled.Count >= AnimalTrainer.GetMaxStabled(owner))
                return false;
            if (automatic && !protect && pet.Backpack is StrongBackpack && pet.Backpack.Items.Count > 0)
                return false;

            try
            {
                pet.ControlTarget = null;
                pet.ControlOrder = OrderType.Stay;
                pet.Internalize();
                // Register ownership before callbacks clear ControlMaster, so partial failures remain recoverable.
                owner.Stabled.Add(pet);
                pet.IsStabled = true;
                EnsureAccount(owner, pet, city);
                pet.SetControlMaster(null);
                pet.SummonMaster = null;
                // RemoveFollowers also removes AutoStabled entries; append only after clearing the master.
                if (automatic && !owner.AutoStabled.Contains(pet))
                    owner.AutoStabled.Add(pet);
                pet.Loyalty = BaseCreature.MaxLoyalty;
                pet.Language = null;
                if (!automatic)
                    ShowAccount(owner, pet);
                return true;
            }
            catch (Exception exception)
            {
                if (!pet.Deleted && pet.Map == Map.Internal)
                {
                    if (!owner.Stabled.Contains(pet))
                        owner.Stabled.Add(pet);
                    pet.IsStabled = true;
                    EnsureAccount(owner, pet, city);
                    if (automatic && !owner.AutoStabled.Contains(pet))
                        owner.AutoStabled.Add(pet);
                }
                ReportFailure(owner, pet, exception);
                return false;
            }
        }

        public static bool StableManually(Mobile from, BaseCreature pet, int capacity, CityManagementStone city)
        {
            PlayerMobile owner = from as PlayerMobile;
            if (owner == null || !owner.CheckAlive() || pet == null || pet.Deleted)
                return false;
            Reconcile(owner);
            if (pet.Body.IsHuman || pet.IsDeadPet || pet.Summoned || !pet.Controlled || pet.ControlMaster != owner)
            {
                owner.SendMessage("You cannot stable that creature.");
                return false;
            }
            if (pet.Map != owner.Map || !owner.InRange(pet, 14)
                || (pet is IMount && ((IMount)pet).Rider != null)
                || (pet.Combatant != null && pet.Map == pet.Combatant.Map && pet.InRange(pet.Combatant, 12)))
            {
                owner.SendMessage("Your pet must be nearby, dismounted, and out of combat.");
                return false;
            }
            if (owner.Stabled.Count >= capacity)
            {
                owner.SendLocalizedMessage(1042565);
                return false;
            }
            return TryStable(owner, pet, false, true, city);
        }

        public static bool TryReturn(Mobile from, BaseCreature pet, bool explicitClaim)
        {
            PlayerMobile owner = from as PlayerMobile;
            if (owner == null || owner.Deleted || !owner.Alive || owner.Map == null || owner.Map == Map.Internal
                || pet == null || pet.Deleted || !owner.Stabled.Contains(pet) || !pet.IsStabled
                || (pet.ControlMaster != null && pet.ControlMaster != owner))
                return false;

            if (owner.Followers + (pet.ControlMaster == owner ? 0 : pet.ControlSlots) > owner.FollowersMax)
            {
                owner.SendLocalizedMessage(1049612, pet.Name);
                return false;
            }

            bool pending = owner.AutoStabled.Contains(pet);
            try
            {
                StableAccount account = EnsureAccount(owner, pet, null);
                account.Accrue(DateTime.UtcNow);
                if (!account.Collect(owner, explicitClaim))
                {
                    owner.SendMessage("{0} and its cargo remain safe in the stables. Pay {1} gold to retrieve it.", pet.Name, account.Outstanding);
                    return false;
                }
                if (pet.ControlMaster != owner && !pet.SetControlMaster(owner))
                    return false;
                pet.ControlTarget = owner;
                pet.ControlOrder = OrderType.Follow;
                pet.MoveToWorld(owner.Location, owner.Map);
                pet.IsStabled = false;
                pet.Loyalty = BaseCreature.MaxLoyalty;
                pet.Language = null;
                while (owner.Stabled.Remove(pet)) { }
                while (owner.AutoStabled.Remove(pet)) { }
                owner.StableAccounts.Remove(account);
                return true;
            }
            catch (Exception exception)
            {
                // A paid bill stays paid if a later movement callback fails.
                try
                {
                    pet.Internalize();
                    pet.IsStabled = true;
                    pet.SetControlMaster(null);
                    if (pending && !owner.AutoStabled.Contains(pet))
                        owner.AutoStabled.Add(pet);
                }
                catch (Exception recoveryException)
                {
                    ReportFailure(owner, pet, recoveryException);
                }
                ReportFailure(owner, pet, exception);
                return false;
            }
        }

        public static void Claim(Mobile from, string name)
        {
            PlayerMobile owner = from as PlayerMobile;
            if (owner == null || !owner.CheckAlive())
                return;
            Reconcile(owner);
            if (owner.Stabled.Count == 0)
            {
                owner.SendMessage("You have no pets in the stables.");
                return;
            }
            foreach (Mobile mobile in new List<Mobile>(owner.Stabled))
            {
                BaseCreature pet = mobile as BaseCreature;
                if (pet != null && (name == null || Insensitive.Equals(pet.Name, name)))
                    TryReturn(owner, pet, true);
            }
        }

        public static void ReportFailure(PlayerMobile owner, BaseCreature pet, Exception exception)
        {
            Console.WriteLine("Stable operation failed: owner={0}, pet={1}, exception={2}",
                owner.Serial, pet == null ? Serial.MinusOne : pet.Serial, exception);
        }
    }
}
