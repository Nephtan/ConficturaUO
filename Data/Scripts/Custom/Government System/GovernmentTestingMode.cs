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
        private static double _multiplier = 1.0;

        /// <summary>
        /// Gets or sets the testing speed multiplier. A backing field is used
        /// instead of an auto-property initializer for compatibility with the
        /// older C# compiler used by RunUO.
        /// </summary>
        public static double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }

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