using System;
using System.IO;
using Server;
using Server.Accounting;
using System.Text.RegularExpressions;

namespace Server.Commands
{
    public static class CombatLogging
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
            string baseDirectory = "Data/Logs/Combat";
            if (!Directory.Exists(baseDirectory))
                Directory.CreateDirectory(baseDirectory);

            string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
            string logFilePath = Path.Combine(baseDirectory, logFileName);

            m_Output = new StreamWriter(logFilePath, true);
            m_Output.AutoFlush = true;

            // Check if the file is new and write headers
            FileInfo fileInfo = new FileInfo(logFilePath);
            if (fileInfo.Length == 0)
            {
                m_Output.WriteLine("Timestamp,IP,Account,Character,Damage,Attacker,LocationX,LocationY,LocationZ,Map,Region");
            }
        }

        public static void LogDamageTaken(Mobile from, int amount, Mobile source, string location, string map, string region)
        {
            if (!m_Enabled || m_Output == null)
                return;

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string ip = (from.NetState != null) ? from.NetState.ToString() : "Disconnected";
            string accountName = from.Account.Username;
            string username = from.Name;
            string attackerName = ExtractAttackerName(source);
            string[] coords = FormatLocation(location).Split(',');
            string formattedLocation = string.Join(",", coords); // Ensure it's CSV-compatible

            string csvLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                timestamp, EscapeCsv(ip), EscapeCsv(accountName), EscapeCsv(username), amount, EscapeCsv(attackerName),
                coords[0], coords[1], coords[2], EscapeCsv(map), EscapeCsv(region));

            m_Output.WriteLine(csvLine);
            WritePlayerSpecificLog(from, csvLine);
        }

        private static void WritePlayerSpecificLog(Mobile from, string csvLine)
        {
            if (from.Account == null)
                return; // If the account is not associated with the mobile, exit

            // Sanitize account name to remove invalid characters from the path
            string sanitizedAccountName = SanitizeFileName(from.Account.Username);

            string combatDirectory = "Data/Logs/Combat"; // Base combat directory
            string playerDirectory = Path.Combine(combatDirectory, "Player"); // Player directory under combat

            if (!Directory.Exists(playerDirectory))
                Directory.CreateDirectory(playerDirectory);

            string logFilePath = Path.Combine(playerDirectory, string.Format("{0}.csv", sanitizedAccountName));

            using (StreamWriter playerLog = new StreamWriter(logFilePath, true))
            {
                FileInfo fileInfo = new FileInfo(logFilePath);
                if (fileInfo.Length == 0)
                {
                    playerLog.WriteLine("Timestamp,IP,Account,Character,Damage,Attacker,LocationX,LocationY,LocationZ,Map,Region");
                }
                playerLog.WriteLine(csvLine);
            }
        }

        // SanitizeFileName method to remove invalid characters from the player's name
        private static string SanitizeFileName(string fileName)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            string sanitizedFileName = string.Join("_", fileName.Split(invalidChars.ToCharArray()));
            return sanitizedFileName;
        }

        private static string EscapeCsv(string data)
        {
            return "\"" + data.Replace("\"", "\"\"") + "\"";
        }

        private static string FormatLocation(string location)
        {
            return Regex.Replace(location, "[^0-9,]", "");
        }

        private static string ExtractAttackerName(Mobile mobile)
        {
            string result = mobile.ToString();
            int quotePosition = result.IndexOf('\"');
            if (quotePosition != -1)
            {
                result = result.Substring(quotePosition + 1);
                quotePosition = result.IndexOf('\"');
                if (quotePosition != -1)
                {
                    result = result.Substring(0, quotePosition);
                }
            }
            return result;
        }
    }
}
