using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Server;
using Server.Commands;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Misc
{
    public class AutoSave : Timer
    {
        private static TimeSpan m_Delay = TimeSpan.FromMinutes(
            MyServerSettings.ServerSaveMinutes()
        );
        private static TimeSpan m_Warning = TimeSpan.Zero;

        public static void Initialize()
        {
            new AutoSave().Start();
            CommandSystem.Register(
                "SetSaves",
                AccessLevel.Administrator,
                new CommandEventHandler(SetSaves_OnCommand)
            );
        }

        private static bool m_SavesEnabled = true;

        public static bool SavesEnabled
        {
            get { return m_SavesEnabled; }
            set { m_SavesEnabled = value; }
        }

        [Usage("SetSaves <true | false>")]
        [Description("Enables or disables automatic shard saving.")]
        public static void SetSaves_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                m_SavesEnabled = e.GetBoolean(0);
                e.Mobile.SendMessage(
                    "Saves have been {0}.",
                    m_SavesEnabled ? "enabled" : "disabled"
                );
            }
            else
            {
                e.Mobile.SendMessage("Format: SetSaves <true | false>");
            }
        }

        public AutoSave()
            : base(m_Delay - m_Warning, m_Delay)
        {
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            if (!m_SavesEnabled || AutoRestart.Restarting)
                return;

            if (m_Warning == TimeSpan.Zero)
            {
                // CLEAN UP ANY GRAVE DISTURBED MONSTERS OR TREASURE MAP MONSTERS
                ArrayList monsters = new ArrayList();
                foreach (Mobile m in World.Mobiles.Values)
                {
                    if (m is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)m;
                        if ((bc.ControlSlots == 666) && (m.Combatant == null))
                            monsters.Add(bc);
                    }
                }
                for (int i = 0; i < monsters.Count; ++i)
                {
                    Mobile ridof = (Mobile)monsters[i];
                    Effects.SendLocationParticles(
                        EffectItem.Create(ridof.Location, ridof.Map, EffectItem.DefaultDuration),
                        0x3728,
                        8,
                        20,
                        5042
                    );
                    Effects.PlaySound(ridof, ridof.Map, 0x201);
                    ridof.Delete();
                }

                Save(true);
            }
            else
            {
                int s = (int)m_Warning.TotalSeconds;
                int m = s / 60;
                s %= 60;

                if (m > 0 && s > 0)
                    World.Broadcast(
                        0x35,
                        true,
                        "The world will save in {0} minute{1} and {2} second{3}.",
                        m,
                        m != 1 ? "s" : "",
                        s,
                        s != 1 ? "s" : ""
                    );
                else if (m > 0)
                    World.Broadcast(
                        0x35,
                        true,
                        "The world will save in {0} minute{1}.",
                        m,
                        m != 1 ? "s" : ""
                    );
                else
                    World.Broadcast(
                        0x35,
                        true,
                        "The world will save in {0} second{1}.",
                        s,
                        s != 1 ? "s" : ""
                    );

                Timer.DelayCall(m_Warning, new TimerCallback(Save));
            }
        }

        public static void Save()
        {
            AutoSave.Save(false);
        }

        public static void Save(bool permitBackgroundWrite)
        {
            if (AutoRestart.Restarting)
                return;

            Stopwatch totalWatch = Stopwatch.StartNew();

            Core.WriteDiagnostic(
                "AutoSave start (backgroundWrite={0}, {1})",
                permitBackgroundWrite,
                NetState.ConnectionSummary
            );

            Stopwatch waitWatch = Stopwatch.StartNew();
            World.WaitForWriteCompletion();
            waitWatch.Stop();

            Core.WriteDiagnostic(
                "AutoSave phase: wait for previous disk write took {0:F2}s",
                waitWatch.Elapsed.TotalSeconds
            );

            Stopwatch backupWatch = Stopwatch.StartNew();
            try
            {
                Core.WriteDiagnostic("AutoSave backup start");
                BackupReport report = Backup();
                backupWatch.Stop();

                Core.WriteDiagnostic(
                    "AutoSave backup complete in {0:F2}s ({1})",
                    backupWatch.Elapsed.TotalSeconds,
                    report
                );
            }
            catch (Exception e)
            {
                backupWatch.Stop();
                Core.WriteDiagnostic(
                    "WARNING: Automatic backup FAILED after {0:F2}s: {1}",
                    backupWatch.Elapsed.TotalSeconds,
                    e
                );
                Console.WriteLine("WARNING: Automatic backup FAILED: {0}", e);
            }

            Stopwatch saveWatch = Stopwatch.StartNew();
            bool worldSaveCompleted = false;

            try
            {
                World.Save(true, permitBackgroundWrite);
                worldSaveCompleted = true;
            }
            finally
            {
                saveWatch.Stop();
                totalWatch.Stop();

                Core.WriteDiagnostic(
                    "AutoSave end in {0:F2}s (worldSaveCompleted={1}, worldSaveElapsed={2:F2}s, {3})",
                    totalWatch.Elapsed.TotalSeconds,
                    worldSaveCompleted,
                    saveWatch.Elapsed.TotalSeconds,
                    NetState.ConnectionSummary
                );
            }
        }

        private static string[] m_Backups = new string[]
        {
            "Sixth Backup",
            "Fifth Backup",
            "Fourth Backup",
            "Third Backup",
            "Second Backup",
            "Most Recent"
        };

        private class BackupReport
        {
            public bool SavesMoved;
            public int RotatedBackups;
            public int DeletedBackups;
            public int InfoFiles;
            public int ArticleFiles;
            public int CustomFiles;

            public override string ToString()
            {
                return String.Format(
                    "savesMoved={0}, rotatedBackups={1}, deletedBackups={2}, infoFiles={3}, articleFiles={4}, customFiles={5}",
                    SavesMoved,
                    RotatedBackups,
                    DeletedBackups,
                    InfoFiles,
                    ArticleFiles,
                    CustomFiles
                );
            }
        }

        private static BackupReport Backup()
        {
            BackupReport report = new BackupReport();

            if (m_Backups.Length == 0)
                return report;

            string root = Path.Combine(Core.BaseDirectory, "Backups/Automatic");

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            string[] existing = Directory.GetDirectories(root);

            for (int i = 0; i < m_Backups.Length; ++i)
            {
                DirectoryInfo dir = Match(existing, m_Backups[i]);

                if (dir == null)
                    continue;

                if (i > 0)
                {
                    string timeStamp = FindTimeStamp(dir.Name);

                    if (timeStamp != null)
                    {
                        try
                        {
                            dir.MoveTo(FormatDirectory(root, m_Backups[i - 1], timeStamp));
                            ++report.RotatedBackups;
                        }
                        catch { }
                    }
                }
                else
                {
                    try
                    {
                        dir.Delete(true);
                        ++report.DeletedBackups;
                    }
                    catch { }
                }
            }

            string saves = Path.Combine(Core.BaseDirectory, "Saves");

            if (Directory.Exists(saves))
            {
                string time = GetTimeStamp();
                string target = FormatDirectory(root, m_Backups[m_Backups.Length - 1], time);

                Directory.Move(saves, target);
                report.SavesMoved = true;
                report.InfoFiles = InfoBackup(target);
                report.ArticleFiles = ArticleBackup(target);
                report.CustomFiles = CustomBackup(target);
            }

            return report;
        }

        public static int InfoBackup(string targetPath)
        {
            return CopyBackupFiles("Info", Path.Combine(targetPath, "Info"));
        }

        public static int ArticleBackup(string targetPath)
        {
            return CopyBackupFiles("Info/Articles", Path.Combine(targetPath, "Info/Articles"));
        }

        public static int CustomBackup(string targetPath)
        {
            return CopyBackupFiles("Info/Custom", Path.Combine(targetPath, "Info/Custom"));
        }

        private static int CopyBackupFiles(string sourcePath, string targetPath)
        {
            string fileName = "";
            string destFile = "";
            int copied = 0;

            System.IO.Directory.CreateDirectory(targetPath);

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);
                foreach (string s in files)
                {
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                    ++copied;
                }
            }

            return copied;
        }

        private static DirectoryInfo Match(string[] paths, string match)
        {
            for (int i = 0; i < paths.Length; ++i)
            {
                DirectoryInfo info = new DirectoryInfo(paths[i]);

                if (info.Name.StartsWith(match))
                    return info;
            }

            return null;
        }

        private static string FormatDirectory(string root, string name, string timeStamp)
        {
            return Path.Combine(root, String.Format("{0} ({1})", name, timeStamp));
        }

        private static string FindTimeStamp(string input)
        {
            int start = input.IndexOf('(');

            if (start >= 0)
            {
                int end = input.IndexOf(')', ++start);

                if (end >= start)
                    return input.Substring(start, end - start);
            }

            return null;
        }

        private static string GetTimeStamp()
        {
            DateTime now = DateTime.Now;

            return String.Format(
                "{0}-{1}-{2} {3}-{4:D2}-{5:D2}",
                now.Day,
                now.Month,
                now.Year,
                now.Hour,
                now.Minute,
                now.Second
            );
        }
    }
}
