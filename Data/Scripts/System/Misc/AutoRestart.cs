/****************************** AutoRestart.cs ********************************
 * Modified by Lokai for Free Ultima Online shards
 *   This completely custom version includes several commands to assist
 *   in managing your restart, whether automated or manual.
/***************************************************************************
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Misc
{
    public enum RestartType
    {
        Daily,
        Weekly
    }

    public class AutoRestart : Timer
    {
        public static void Initialize()
        {
            CommandSystem.Register("Restart", AccessLevel.Administrator, Restart_OnCommand);
            CommandSystem.Register(
                "AutoRestartOn",
                AccessLevel.Administrator,
                AutoRestartOn_OnCommand
            );
            CommandSystem.Register(
                "AutoRestartOff",
                AccessLevel.Administrator,
                AutoRestartOff_OnCommand
            );
            CommandSystem.Register(
                "AutoRestartWhen",
                AccessLevel.Administrator,
                AutoRestartWhen_OnCommand
            );
            CommandSystem.Register(
                "AutoRestartTime",
                AccessLevel.Administrator,
                AutoRestartTime_OnCommand
            );
            CommandSystem.Register(
                "AutoRestartColor",
                AccessLevel.Administrator,
                AutoRestartColor_OnCommand
            );
            CommandSystem.Register(
                "AutoRestartText",
                AccessLevel.Administrator,
                AutoRestartText_OnCommand
            );
            CommandSystem.Register(
                "AutoRestartSave",
                AccessLevel.Administrator,
                AutoRestartSave_OnCommand
            );
            CommandSystem.Register(
                "AutoRestartLoad",
                AccessLevel.Administrator,
                AutoRestartLoad_OnCommand
            );
            CommandSystem.Register(
                "AR-On",
                AccessLevel.Administrator,
                AutoRestartOn_OnCommand
            );
            CommandSystem.Register(
                "AR-Off",
                AccessLevel.Administrator,
                AutoRestartOff_OnCommand
            );
            CommandSystem.Register(
                "AR-When",
                AccessLevel.Administrator,
                AutoRestartWhen_OnCommand
            );
            CommandSystem.Register(
                "AR-Time",
                AccessLevel.Administrator,
                AutoRestartTime_OnCommand
            );
            CommandSystem.Register(
                "AR-Color",
                AccessLevel.Administrator,
                AutoRestartColor_OnCommand
            );
            CommandSystem.Register(
                "AR-Text",
                AccessLevel.Administrator,
                AutoRestartText_OnCommand
            );
            CommandSystem.Register("AR-Save",
                AccessLevel.Administrator,
                AutoRestartSave_OnCommand
            );
            CommandSystem.Register(
                "AR-Load",
                AccessLevel.Administrator,
                AutoRestartLoad_OnCommand
            );

            // -------------------- START HERE ----------------------
            // At what time interval(s) (in minutes) should the restart warning be displayed prior to restart?
            // (These numbers should go from Lowest to Highest for best results.)
            // -------------------- START HERE ----------------------
            WarningDelays = new List<double>()
            {
                1.0,
                2.0,
                5.0,
                10.0,
                15.0,
                20.0,
                25.0,
                30.0,
                45.0
            };

            ResetWarningDelayBools(true);
            new AutoRestart().Start();
        }

        private static RestartType RestartFrequency = RestartType.Weekly; // The server restarts daily or weekly on a particular day of the week.
        private static DayOfWeek RestartDay = DayOfWeek.Wednesday; // IF the server restarts weekly, the day of week is set here.
        private static TimeSpan RestartDelay = TimeSpan.Zero; // Here we set how long to delay the restart once the timer has finished.

        private static bool m_Enabled = true; // is auto-restarting enabled?
        public static bool Enabled
        {
            get { return m_Enabled; }
        }

        private static TimeSpan RestartTimeOfDay = TimeSpan.FromHours(9); // The time of day at which to restart (in Server time.)
        private static List<double> WarningDelays;
        private static string RestartMessage = "The server will be restarting for routine maintenance";
        private static bool m_Restarting;

        public static bool Restarting
        {
            get { return m_Restarting; }
        }

        private static DateTime m_RestartDateTime;
        private static int WarningColor = 0x22;

        private static List<bool> WarningDelaysNOTSent;

        private static void ResetWarningDelayBools(bool auto)
        {
            if (auto)
            {
                m_RestartDateTime = DateTime.Now.Date + RestartTimeOfDay;

                if (m_RestartDateTime < DateTime.Now)
                    m_RestartDateTime += TimeSpan.FromDays(1.0);

                if (RestartFrequency == RestartType.Weekly)
                {
                    while (m_RestartDateTime.DayOfWeek != RestartDay)
                    {
                        m_RestartDateTime += TimeSpan.FromDays(1.0);
                    }
                }
            }

            WarningDelaysNOTSent = new List<bool>();

            for (int i = 0; i < WarningDelays.Count; i++)
            {
                WarningDelaysNOTSent.Add(getNextWarningTime(WarningDelays[i]) > DateTime.Now);
            }
        }

        public static void LoadSettings(Mobile from, AutoRestarter ar)
        {
            try
            {
                RestartFrequency = ar.RestartFrequency;
                RestartDay = ar.RestartDay;
                RestartDelay = ar.RestartDelay;
                m_Enabled = ar.Enabled;
                RestartTimeOfDay = ar.RestartTimeOfDay;
                WarningDelays = new List<double>();

                foreach (double delay in ar.WarningDelays)
                {
                    WarningDelays.Add(delay);
                }

                RestartMessage = ar.RestartMessage;
                m_Restarting = ar.Restarting;
                m_RestartDateTime = ar.RestartDateTime;
                WarningColor = ar.WarningColor;
                ResetWarningDelayBools(true);
                from.SendMessage("Finished loading AutoRestart settings.");
            }
            catch (Exception ex)
            {
                from.SendMessage("Error loading AutoRestart settings: {0}", ex);
            }
        }

        public AutoRestart()
            : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
        {
            Priority = TimerPriority.FiveSeconds;
        }

        private static int getWarningColor(string color)
        {
            switch (color)
            {
                case "red":
                    return Utility.RandomRedHue();
                case "pink":
                    return Utility.RandomPinkHue();
                case "blue":
                    return Utility.RandomBlueHue();
                case "yellow":
                    return Utility.RandomYellowHue();
                case "green":
                    return Utility.RandomGreenHue();
                case "orange":
                    return Utility.RandomOrangeHue();
                case "brown":
                    return Utility.RandomBirdHue();
                default:
                    try
                    {
                        return int.Parse(color);
                    }
                    catch { } // Maybe they typed a number...
                    return Utility.RandomDyedHue();
            }
        }

        private static DateTime getNextWarningTime(double nextDelay)
        {
            return m_RestartDateTime - TimeSpan.FromMinutes(nextDelay);
        }

        private static void AutoRestartLoad_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target the AutoRestarter whose settings you would like to load.");
            e.Mobile.Target = new AutoRestartLoadTarget();
        }

        private class AutoRestartLoadTarget : Target
        {
            public AutoRestartLoadTarget()
                : base(0, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is AutoRestarter)
                {
                    AutoRestarter ar = (AutoRestarter)targeted;
                    AutoRestart.LoadSettings(from, ar);
                }
            }
        }

        private static void AutoRestartSave_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)e.Mobile;
                if (pm == null || pm.Deleted)
                    return;
                Server.Items.Container pack = pm.Backpack;
                if (pack == null || pack.Deleted)
                    return;
                pack.DropItem(
                    new AutoRestarter(
                        m_Enabled,
                        m_Restarting,
                        RestartFrequency,
                        RestartDay,
                        RestartMessage,
                        RestartTimeOfDay,
                        WarningColor,
                        RestartDelay,
                        WarningDelays
                    )
                );
                pm.SendMessage(
                    "Your settings have been saved to a new AutoRestarter in your pack."
                );
            }
        }

        [Usage("AutoRestartText")]
        [Description("Sets the text for AutoRestart warning messages.")]
        private static void AutoRestartText_OnCommand(CommandEventArgs e)
        {
            try
            {
                RestartMessage = e.ArgString;
                e.Mobile.SendAsciiMessage(
                    WarningColor,
                    "New Restart Message: {0}.",
                    RestartMessage
                );
            }
            catch
            {
                e.Mobile.SendMessage(
                    "Usage: AR-Text <string> ((  ex:  [AR-Text The Shard will be restarting for a major update  ))"
                );
            }
        }

        [Usage("AutoRestartColor")]
        [Description("Sets the color for AutoRestart warning messages.")]
        private static void AutoRestartColor_OnCommand(CommandEventArgs e)
        {
            try
            {
                WarningColor = getWarningColor(e.Arguments[0].ToLower());
                e.Mobile.SendAsciiMessage(WarningColor, "This is the new Warning Color.");
            }
            catch
            {
                e.Mobile.SendMessage("Usage: AR-Color <name of color> ((  ex:  [AR-Color Blue  ))");
            }
        }

        [Usage("AutoRestartTime")]
        [Description("Sets the time of day for the AutoRestart feature.")]
        private static void AutoRestartTime_OnCommand(CommandEventArgs e)
        {
            try
            {
                RestartTimeOfDay = TimeSpan.FromHours(double.Parse(e.Arguments[0]));
                ResetWarningDelayBools(true);
                e.Mobile.SendMessage(
                    "Restart time set to {0} {1}.",
                    m_RestartDateTime.ToShortTimeString(),
                    RestartFrequency == RestartType.Daily ? "Daily" : RestartDay.ToString()
                );
            }
            catch
            {
                e.Mobile.SendMessage(
                    "Usage: ARTime <int> ((int is the hour of the day to restart))"
                );
            }
        }

        [Usage("AutoRestartWhen")]
        [Description("Displays the time of day set for AutoRestart.")]
        private static void AutoRestartWhen_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage(
                "AutoRestart is {0}scheduled for {1} {2}.",
                Enabled ? "" : "DISABLED, but if enabled would be ",
                m_RestartDateTime.ToShortTimeString(),
                RestartFrequency == RestartType.Daily ? "Daily" : RestartDay.ToString()
            );
        }

        [Usage("AutoRestartOff")]
        [Description("Disables the AutoRestart feature.")]
        private static void AutoRestartOff_OnCommand(CommandEventArgs e)
        {
            m_Enabled = false;
            e.Mobile.SendMessage("AutoRestart is now DISABLED.");
        }

        [Usage("AutoRestartOn")]
        [Description("Enables the AutoRestart feature.")]
        private static void AutoRestartOn_OnCommand(CommandEventArgs e)
        {
            m_Enabled = true;
            e.Mobile.SendMessage("AutoRestart is now ENABLED.");
        }

        [Usage("Restart [x (integer)]")]
        [Description("Restarts the Server now [or in ~x~ minutes].")]
        public static void Restart_OnCommand(CommandEventArgs e)
        {
            double minutes = 0.0;
            try
            {
                minutes = double.Parse(e.Arguments[0]);
            }
            catch { }
            if (m_Restarting)
            {
                e.Mobile.SendMessage("The server is already restarting.");
            }
            else
            {
                e.Mobile.SendMessage(
                    "You have initiated a server restart{0}.",
                    minutes > 0 ? string.Format(" for {0} minutes from now.", (int)minutes) : ""
                );
                m_Enabled = true;
                m_RestartDateTime = DateTime.Now + TimeSpan.FromMinutes(minutes);
                ResetWarningDelayBools(false);
            }
        }

        protected override void OnTick()
        {
            if (m_Restarting || !Enabled)
                return;

            int loopCounter = 0;
            foreach (double delay in WarningDelays)
            {
                if (
                    WarningDelaysNOTSent[loopCounter] && DateTime.Now > getNextWarningTime(delay)
                )
                {
                    WarningDelaysNOTSent[loopCounter] = false;
                    Warning_Callback((int)delay);
                    return;
                }
                loopCounter++;
            }

            if (DateTime.Now < m_RestartDateTime)
                return;

            AutoSave.Save();

            m_Restarting = true;

            Timer.DelayCall(RestartDelay, new TimerCallback(Restart_Callback));
        }

        private void Warning_Callback(int time)
        {
            World.Broadcast(WarningColor, true, "{0} in {1} minutes.", RestartMessage, time);
        }

        private void Restart_Callback()
        {
            Core.Kill(true);
        }
    }

    [Flipable(0x1810, 0x1811)]
    public class AutoRestarter : Item
    {
        private bool m_Enabled;
        private bool m_Restarting;
        private RestartType m_RestartFrequency;
        private DayOfWeek m_RestartDay;
        private string m_RestartMessage;
        private DateTime m_RestartDateTime;
        private DateTime m_NextWarningTime;
        private int m_WarningColor;

        private TimeSpan m_RestartDelay;
        private TimeSpan m_RestartTimeOfDay;
        private List<double> m_WarningDelays;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Restarting
        {
            get { return m_Restarting; }
            set { m_Restarting = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public RestartType RestartFrequency
        {
            get { return m_RestartFrequency; }
            set { m_RestartFrequency = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DayOfWeek RestartDay
        {
            get { return m_RestartDay; }
            set { m_RestartDay = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string RestartMessage
        {
            get { return m_RestartMessage; }
            set { m_RestartMessage = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime RestartDateTime
        {
            get { return m_RestartDateTime; }
            set { m_RestartDateTime = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextWarningTime
        {
            get { return m_NextWarningTime; }
            set { m_NextWarningTime = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WarningColor
        {
            get { return m_WarningColor; }
            set { m_WarningColor = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RestartDelay
        {
            get { return m_RestartDelay; }
            set { m_RestartDelay = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RestartTimeOfDay
        {
            get { return m_RestartTimeOfDay; }
            set { m_RestartTimeOfDay = value; }
        }

        public List<double> WarningDelays
        {
            get { return m_WarningDelays; }
            set { m_WarningDelays = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string WarningDelaysList
        {
            get
            {
                string delays = "";
                foreach (double delay in m_WarningDelays)
                {
                    delays += delay + ",";
                }
                if (delays.Contains(","))
                    delays = delays.Remove(delays.Length - 1, 1);
                return delays;
            }
            set
            {
                try
                {
                    m_WarningDelays = new List<double>();
                    string[] values = value.Split(',');
                    foreach (string delay in values)
                    {
                        m_WarningDelays.Add(double.Parse(delay));
                    }
                }
                catch { }
            }
        }

        [Constructable]
        public AutoRestarter()
            : this(
                false,
                false,
                RestartType.Daily,
                DayOfWeek.Monday,
                "The server will be restarting for routine Maintenance",
                TimeSpan.FromHours(2.0),
                0x22,
                TimeSpan.Zero,
                new List<double>() { 1.0, 2.0, 5.0, 10.0 }
            )
        { }

        [Constructable]
        public AutoRestarter(
            bool enabled,
            bool restarting,
            RestartType frequency,
            DayOfWeek restartDay,
            string message,
            TimeSpan timeOfDay,
            int color,
            TimeSpan delay,
            List<double> delays
        )
            : base(0x1810)
        {
            Name = "Auto-Restart Control Item";
            m_Enabled = enabled;
            m_Restarting = restarting;
            m_RestartFrequency = frequency;
            m_RestartDay = restartDay;
            m_RestartMessage = message;
            m_RestartTimeOfDay = timeOfDay;
            m_WarningColor = color;
            m_RestartDelay = delay;
            m_WarningDelays = delays;
            m_RestartDateTime = DateTime.Now.Date + m_RestartTimeOfDay;
            TimeSpan lastDelay = TimeSpan.Zero;
            try
            {
                lastDelay = TimeSpan.FromHours(m_WarningDelays[m_WarningDelays.Count]);
            }
            catch { }
            m_NextWarningTime = m_RestartDateTime - lastDelay;
        }

        public AutoRestarter(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            // version 0
            writer.Write((bool)m_Enabled);
            writer.Write((bool)m_Restarting);
            writer.Write((int)m_RestartFrequency);
            writer.Write((int)m_RestartDay);
            writer.Write((string)m_RestartMessage);
            writer.Write((DateTime)m_RestartDateTime);
            writer.Write((DateTime)m_NextWarningTime);
            writer.Write((int)m_WarningColor);
            writer.Write((TimeSpan)m_RestartDelay);
            writer.Write((TimeSpan)m_RestartTimeOfDay);
            writer.Write((int)m_WarningDelays.Count);

            foreach (double delay in m_WarningDelays)
            {
                writer.Write((double)delay);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    m_Enabled = reader.ReadBool();
                    m_Restarting = reader.ReadBool();
                    m_RestartFrequency = (RestartType)reader.ReadInt();
                    m_RestartDay = (DayOfWeek)reader.ReadInt();
                    m_RestartMessage = reader.ReadString();
                    m_RestartDateTime = reader.ReadDateTime();
                    m_NextWarningTime = reader.ReadDateTime();
                    m_WarningColor = reader.ReadInt();
                    m_RestartDelay = reader.ReadTimeSpan();
                    m_RestartTimeOfDay = reader.ReadTimeSpan();
                    int count = reader.ReadInt();
                    m_WarningDelays = new List<double>();

                    for (int i = 0; i < count; i++)
                    {
                        m_WarningDelays.Add(reader.ReadDouble());
                    }

                    break;
            }
        }
    }
}
