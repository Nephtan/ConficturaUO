using System;

namespace Server.Custom.Confictura
{
    /// <summary>
    /// Provides a centralized toggle and speed multiplier for accelerating
    /// time based mechanics within the Government System. When enabled,
    /// all registered timers should have their duration divided by the
    /// configured multiplier allowing rapid testing of long running features.
    /// </summary>
    public static class GovernmentTestingMode
    {
        /// <summary>
        /// True if the accelerated testing mode is active.
        /// </summary>
        public static bool Enabled { get; set; }

        /// <summary>
        /// Multiplier applied to timer durations when <see cref="Enabled"/> is true.
        /// A value of 2 will cause timers to elapse twice as fast. The default is 1.
        /// </summary>
        public static double Multiplier { get; set; } = 1.0;

        /// <summary>
        /// Adjusts a <see cref="TimeSpan"/> according to the current testing mode
        /// settings. When disabled or supplied with an invalid multiplier the
        /// original span is returned.
        /// </summary>
        public static TimeSpan Adjust(TimeSpan span)
        {
            if (!Enabled || Multiplier <= 0)
            {
                return span;
            }

            return TimeSpan.FromTicks((long)(span.Ticks / Multiplier));
        }

        /// <summary>
        /// Convenience method returning <c>DateTime.Now + Adjust(span)</c>.
        /// </summary>
        public static DateTime GetAdjustedTime(TimeSpan span)
        {
            return DateTime.Now + Adjust(span);
        }
    }
}