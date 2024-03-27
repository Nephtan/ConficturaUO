// Check PackUpHouse() for that crash on item delete.  It causes a crash in RemoveMulti (Core)

using System;
using System.Collections;
using Server;
using Server.Multis;

namespace Knives.TownHouses
{
    public class General
    {
        public static string Version
        {
            get { return "3.01"; }
        }

        // This setting determines the suggested gold value for a single square of a home
        //  which then derives price, lockdowns and secures.
        public static int SuggestionFactor
        {
            get { return 600; }
        }

        // This setting determines if players need License in order to rent out their property
        public static bool RequireRenterLicense
        {
            get { return false; }
        }

        public static void Configure()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnSave);
            EventSink.ServerStarted += new ServerStartedEventHandler(OnStarted);

            EventSink.Login += new LoginEventHandler(OnLogin);
            EventSink.Speech += new SpeechEventHandler(HandleSpeech);
        }

        private static void OnStarted()
        {
            int townHouseCount = TownHouse.AllTownHouses.Count;

            while (--townHouseCount >= 0)
            {
                if (townHouseCount >= TownHouse.AllTownHouses.Count)
                {
                    continue;
                }

                TownHouse house = (TownHouse)TownHouse.AllTownHouses[townHouseCount];  // Line 50

                house.InitSectorDefinition();

                RUOVersion.UpdateRegion(house.ForSaleSign);
            }
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            int signCount = TownHouseSign.AllSigns.Count;

            while (--signCount >= 0)
            {
                if (signCount >= TownHouseSign.AllSigns.Count)
                {
                    continue;
                }

                TownHouseSign sign = (TownHouseSign)TownHouseSign.AllSigns[signCount];  // Line 69

                sign.ValidateOwnership();
            }
        }

        private static void OnLogin(LoginEventArgs e)
        {
            ArrayList houses = new ArrayList(BaseHouse.GetHouses(e.Mobile));

            if (houses == null)
            {
                return;
            }

            foreach (BaseHouse house in houses)
            {
                if (house is TownHouse)
                {
                    ((TownHouse)house).ForSaleSign.CheckDemolishTimer();
                }
            }
        }

        private static void HandleSpeech(SpeechEventArgs e)
        {
            ArrayList houses = new ArrayList(BaseHouse.GetHouses(e.Mobile));

            if (houses == null)
            {
                return;
            }

            foreach (BaseHouse house in houses)
            {
                try
                {
                    if (!RUOVersion.RegionContains(house.Region, e.Mobile))
                    {
                        continue;
                    }

                    if (house is TownHouse)
                    {
                        house.OnSpeech(e);
                    }

                    if (house.Owner == e.Mobile && e.Speech.ToLower() == "create rental contract" && CanRent(e.Mobile, house, true))
                    {
                        e.Mobile.AddToBackpack(new RentalContract());
                        e.Mobile.SendMessage("A rental contract has been placed in your bag.");
                    }
                    else if (house.Owner == e.Mobile && e.Speech.ToLower() == "check storage")
                    {
                        int count = 0;

                        e.Mobile.SendMessage(
                            "You have {0} lockdowns and {1} secures available.",
                            RemainingSecures(house),
                            RemainingLocks(house)
                        );

                        if ((count = AllRentalLocks(house)) != 0)
                        {
                            e.Mobile.SendMessage(
                                "Current rentals are using {0} of your lockdowns.",
                                count
                            );
                        }

                        if ((count = AllRentalSecures(house)) != 0)
                        {
                            e.Mobile.SendMessage(
                                "Current rentals are using {0} of your secures.",
                                count
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception or log it
                    Console.WriteLine("Error handling speech for house: " + ex.Message);
                }
            }
        }

        private static bool CanRent(Mobile m, BaseHouse house, bool say)
        {
            if (house is TownHouse && ((TownHouse)house).ForSaleSign.PriceType != "Sale")
            {
                if (say)
                {
                    m.SendMessage("You must own your property to rent it.");
                }

                return false;
            }

            if (RequireRenterLicense)
            {
                RentalLicense lic = m.Backpack.FindItemByType(typeof(RentalLicense)) as RentalLicense;

                if (lic != null && lic.Owner == null)
                {
                    lic.Owner = m;
                }

                if (lic == null || lic.Owner != m)
                {
                    if (say)
                    {
                        m.SendMessage("You must have a renter's license to rent your property.");
                    }

                    return false;
                }
            }

            if (EntireHouseContracted(house))
            {
                if (say)
                {
                    m.SendMessage("This entire house already has a rental contract.");
                }

                return false;
            }

            if (RemainingSecures(house) < 0 || RemainingLocks(house) < 0)
            {
                if (say)
                {
                    m.SendMessage("You don't have the storage available to rent property.");
                }

                return false;
            }

            return true;
        }

        #region Rental Info
        public static bool EntireHouseContracted(BaseHouse house)
        {
            foreach (Item item in TownHouseSign.AllSigns)
            {
                if (item is RentalContract && house == ((RentalContract)item).ParentHouse)
                {
                    if (((RentalContract)item).EntireHouse)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool HasContract(BaseHouse house)
        {
            foreach (Item item in TownHouseSign.AllSigns)
            {
                if (item is RentalContract && house == ((RentalContract)item).ParentHouse)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasOtherContract(BaseHouse house, RentalContract contract)
        {
            foreach (Item item in TownHouseSign.AllSigns)
            {
                if (
                    item is RentalContract
                    && item != contract
                    && house == ((RentalContract)item).ParentHouse
                )
                {
                    return true;
                }
            }

            return false;
        }

        public static int RemainingSecures(BaseHouse house)
        {
            if (house == null)
            {
                return 0;
            }

            int total;

            int a,
                b,
                c,
                d;

            total = house.GetAosMaxSecures() - house.GetAosCurSecures(out a, out b, out c, out d);

            return total - AllRentalSecures(house);
        }

        public static int RemainingLocks(BaseHouse house)
        {
            if (house == null)
            {
                return 0;
            }

            int total;

            total = house.GetAosMaxLockdowns() - house.GetAosCurLockdowns();

            return total - AllRentalLocks(house);
        }

        public static int AllRentalSecures(BaseHouse house)
        {
            int count = 0;

            foreach (TownHouseSign sign in TownHouseSign.AllSigns)
            {
                if (sign is RentalContract && ((RentalContract)sign).ParentHouse == house)
                {
                    count += sign.Secures;
                }
            }

            return count;
        }

        public static int AllRentalLocks(BaseHouse house)
        {
            int count = 0;

            foreach (TownHouseSign sign in TownHouseSign.AllSigns)
            {
                if (sign is RentalContract && ((RentalContract)sign).ParentHouse == house)
                {
                    count += sign.Locks;
                }
            }

            return count;
        }
        #endregion
    }
}
