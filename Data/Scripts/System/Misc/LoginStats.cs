using System;
using Server.Network;
using Server.Gumps; // UNIQUE NAMING SYSTEM
using Server.Mobiles;

namespace Server.Misc
{
    public class LoginStats
    {
        public static void Initialize()
        {
            // Register our event handler
            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            int userCount = NetState.Instances.Count;
            int itemCount = World.Items.Count;
            int mobileCount = World.Mobiles.Count;

            Mobile m = args.Mobile;

            if (m.AccessLevel >= AccessLevel.GameMaster)
                m.SendMessage("You can type '[helpadmin' to learn the commands for this server.");
            else
                m.SendMessage(
                    "You can use the 'Help' button on your paperdoll for more information."
                );

            PlayerMobile pm = (PlayerMobile)m;

            if (pm.OwesBackTaxes == true)
            {
                if (pm.City != null)
                {
                    if (Banker.Withdraw(m, pm.BackTaxesAmount))
                    {
                        m.SendMessage(
                            "You have paid your back taxes in full from the money in your bank account."
                        );
                        pm.City.CityTreasury += pm.BackTaxesAmount;
                        pm.OwesBackTaxes = false;
                        pm.BackTaxesAmount = 0;
                    }
                    else
                    {
                        int balance = Banker.GetBalance(m);

                        if (Banker.Withdraw(m, balance))
                        {
                            pm.City.CityTreasury += balance;
                            pm.BackTaxesAmount -= 0;
                            m.SendMessage(
                                "You have made a payment on your back taxes of {0} you now owe {1} in back taxes.",
                                balance,
                                pm.BackTaxesAmount
                            );
                        }
                    }
                }
                else
                {
                    pm.OwesBackTaxes = false;
                    pm.BackTaxesAmount = 0;
                }
            }

            //Unique Naming System//
            #region CheckName
            if (
                (
                    m.Name == CharacterCreation.GENERIC_NAME
                    || !CharacterCreation.CheckDupe(m, m.Name)
                )
                && m.AccessLevel < AccessLevel.GameMaster
            )
            {
                m.CantWalk = true;
                m.SendGump(new NameChangeGump(m));
            }
            #endregion
            //Unique Naming System//
        }
    }
}
