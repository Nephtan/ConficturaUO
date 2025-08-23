using System;
using System.IO;
using Server;
using Server.Accounting;
using Server.Multis;
using Server.Mobiles;

namespace Server.Items
{
    public static class RunebookLogging
    {
        private static StreamWriter m_Output;
        private static bool m_Enabled = true;

        public static bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        public static void Initialize()
        {
            try
            {
                string directory = Path.Combine(Core.BaseDirectory, "Logs");

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                m_Output = new StreamWriter(Path.Combine(directory, "Runebook.log"), true);
                m_Output.AutoFlush = true;
            }
            catch
            {
            }
        }

        public static void Log(Mobile from, Runebook book, string action, RunebookEntry entry)
        {
            if (!m_Enabled || m_Output == null || from == null || book == null || entry == null)
                return;

            try
            {
                Account acct = from.Account as Account;
                string account = (acct != null ? acct.Username : "no account");

                Point3D playerLoc = from.Location;
                Map playerMap = from.Map;

                Point3D bookLoc = book.GetWorldLocation();
                Map bookMap = book.Map;

                BaseHouse house = BaseHouse.FindHouseAt(book);
                string houseInfo = "none";

                if (house != null)
                {
                    string regionName = (house.Region != null ? house.Region.Name : "unknown");
                    string ownerName = (house.Owner != null ? house.Owner.Name : "unknown");
                    houseInfo = String.Format("{0} owned by {1}", regionName, ownerName);
                }

                m_Output.WriteLine(
                    "{0}\t{1}\tPlayer:{2} ('{3}') @ {4} ({5})\tBook:0x{6:X} \"{7}\" @ {8} ({9})\tSecure:{10}\tRune:\"{11}\" {12} ({13})\tHouse:{14}",
                    DateTime.Now,
                    action,
                    from.Name,
                    account,
                    playerLoc,
                    playerMap,
                    book.Serial.Value,
                    book.Name ?? "(no name)",
                    bookLoc,
                    bookMap,
                    book.Level,
                    entry.Description ?? "(no description)",
                    entry.Location,
                    entry.Map,
                    houseInfo
                );
            }
            catch
            {
            }
        }
    }
}
