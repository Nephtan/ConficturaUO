using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Server.Commands;

namespace Server.Custom.Confictura.Integrations.Discord
{
    public static class TownCrierDiscord
    {
        private const string ConfigRelativePath = "Data/System/CFG/town-crier-discord.local.cfg";
        private const int DefaultMinimumIntervalMinutes = 5;
        private const int DefaultExplorationRepeatMinutes = 15;
        private const int MaximumMinimumIntervalMinutes = 1440;
        private const int MaximumExplorationRepeatMinutes = 1440;
        private const int MaximumPendingEvents = 5;
        private const int MaximumMessageLength = 2000;
        private const int MaximumRetryCount = 3;
        private static readonly TimeSpan MurdererRepeatInterval = TimeSpan.FromHours(24.0);

        private static readonly object m_SyncRoot = new object();
        private static readonly List<TownCrierEvent> m_PendingEvents = new List<TownCrierEvent>();
        private static readonly Dictionary<string, DateTime> m_MurdererDedupe =
            new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, DateTime> m_ExplorationDedupe =
            new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, bool> m_InFlightDedupeKeys =
            new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        private static bool m_Enabled;
        private static bool m_Configured;
        private static bool m_SessionDisabled;
        private static bool m_SendInProgress;
        private static bool m_HasObservedWantedRoster;
        private static bool m_LastObservedWantedRosterHadEntries;
        private static string m_WebhookUrl;
        private static string m_ConfigurationMessage = "Not initialized.";
        private static string m_LastFailure;
        private static DateTime m_LastFailureUtc = DateTime.MinValue;
        private static DateTime m_LastSuccessUtc = DateTime.MinValue;
        private static DateTime m_NextSendUtc = DateTime.MinValue;
        private static TimeSpan m_MinimumInterval = TimeSpan.FromMinutes(
            DefaultMinimumIntervalMinutes
        );
        private static TimeSpan m_ExplorationRepeatInterval = TimeSpan.FromMinutes(
            DefaultExplorationRepeatMinutes
        );
        private static Server.Timer m_PendingTimer;
        private static int m_ConfigGeneration;
        private static int m_PendingOmittedCount;
        private static long m_AcceptedCount;
        private static long m_CoalescedCount;
        private static long m_SuppressedCount;
        private static long m_OverflowDroppedCount;
        private static long m_SuccessfulPostCount;
        private static long m_RetryAttemptCount;
        private static long m_ExhaustedCount;
        private static long m_PermanentDisableCount;

        public static void Initialize()
        {
            CommandSystem.Register(
                "TownCrierDiscord",
                AccessLevel.Administrator,
                new CommandEventHandler(TownCrierDiscord_OnCommand)
            );

            string message;
            ReloadConfiguration(out message);

            Console.WriteLine("[Town Crier Discord] {0}", message);
        }

        public static void QueueEvent(string category, string eventText)
        {
            try
            {
                string label = GetCategoryLabel(category);

                if (label == null)
                    return;

                string normalizedText = NormalizeEventText(eventText);

                if (normalizedText.Length == 0)
                    return;

                TownCrierEvent entry = new TownCrierEvent(
                    label,
                    normalizedText,
                    String.Equals(category, "Logging Murderers", StringComparison.OrdinalIgnoreCase)
                        ? TownCrierEventKind.WantedRoster
                        : String.Equals(
                            category,
                            "Logging Journies",
                            StringComparison.OrdinalIgnoreCase
                        )
                            ? TownCrierEventKind.Exploration
                            : TownCrierEventKind.Normal,
                    normalizedText,
                    DateTime.UtcNow
                );

                lock (m_SyncRoot)
                {
                    if (!CanSendLocked())
                        return;

                    DateTime now = DateTime.UtcNow;

                    if (entry.IsWantedRoster && IsDuplicateWantedRosterLocked(entry, now))
                    {
                        ++m_SuppressedCount;
                        return;
                    }

                    if (entry.IsExploration && IsDuplicateExplorationLocked(entry, now))
                    {
                        ++m_SuppressedCount;
                        return;
                    }

                    ++m_AcceptedCount;
                    QueueEventLocked(entry, now);
                }
            }
            catch
            {
                // Discord must never interrupt the shard event that produced the news.
            }
        }

        public static void QueueWantedRoster(IList<string> notices)
        {
            try
            {
                List<string> normalizedNotices = new List<string>();
                Dictionary<string, bool> uniqueNotices = new Dictionary<string, bool>(
                    StringComparer.OrdinalIgnoreCase
                );

                if (notices != null)
                {
                    for (int i = 0; i < notices.Count; ++i)
                    {
                        string normalizedText = NormalizeEventText(notices[i]);

                        if (
                            normalizedText.Length > 0
                            && !uniqueNotices.ContainsKey(normalizedText)
                        )
                        {
                            uniqueNotices[normalizedText] = true;
                            normalizedNotices.Add(normalizedText);
                        }
                    }
                }

                normalizedNotices.Sort(StringComparer.OrdinalIgnoreCase);

                lock (m_SyncRoot)
                {
                    bool hasEntries = normalizedNotices.Count > 0;

                    if (!m_HasObservedWantedRoster)
                    {
                        m_HasObservedWantedRoster = true;
                        m_LastObservedWantedRosterHadEntries = hasEntries;

                        if (!hasEntries)
                            return;
                    }
                    else
                    {
                        bool postAllClear = m_LastObservedWantedRosterHadEntries && !hasEntries;
                        m_LastObservedWantedRosterHadEntries = hasEntries;

                        if (!hasEntries && !postAllClear)
                            return;
                    }

                    if (!CanSendLocked())
                        return;

                    string fingerprint = hasEntries
                        ? String.Join("\n", normalizedNotices.ToArray())
                        : "<empty>";
                    string eventText;

                    if (!hasEntries)
                    {
                        eventText = "No one is currently wanted for murder.";
                    }
                    else if (normalizedNotices.Count == 1)
                    {
                        eventText = normalizedNotices[0];
                    }
                    else
                    {
                        eventText =
                            "The wanted register lists "
                            + normalizedNotices.Count.ToString(CultureInfo.InvariantCulture)
                            + " outlaws: "
                            + String.Join(" ", normalizedNotices.ToArray());
                    }

                    DateTime now = DateTime.UtcNow;
                    TownCrierEvent entry = new TownCrierEvent(
                        "Wanted Murderers",
                        eventText,
                        TownCrierEventKind.WantedRoster,
                        fingerprint,
                        now
                    );

                    if (IsDuplicateWantedRosterLocked(entry, now))
                    {
                        ++m_SuppressedCount;
                        return;
                    }

                    ++m_AcceptedCount;
                    QueueEventLocked(entry, now);
                }
            }
            catch
            {
                // Discord must never interrupt the murderer register rebuild.
            }
        }

        private static void QueueEventLocked(TownCrierEvent entry, DateTime now)
        {
            if (!m_SendInProgress && now >= m_NextSendUtc && m_PendingEvents.Count == 0)
            {
                List<TownCrierEvent> immediate = new List<TownCrierEvent>();
                immediate.Add(entry);
                StartSendLocked(immediate, 0, now);
                return;
            }

            AddPendingEventLocked(entry);
            SchedulePendingTimerLocked(now);
        }

        private static void AddPendingEventLocked(TownCrierEvent entry)
        {
            for (int i = 0; i < m_PendingEvents.Count; ++i)
            {
                if (
                    String.Equals(
                        m_PendingEvents[i].DedupeKey,
                        entry.DedupeKey,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    m_PendingEvents[i].IncrementOccurrence();
                    ++m_CoalescedCount;
                    return;
                }
            }

            m_PendingEvents.Add(entry);

            while (m_PendingEvents.Count > MaximumPendingEvents)
            {
                int removeIndex = 0;

                for (int i = 1; i < m_PendingEvents.Count; ++i)
                {
                    TownCrierEvent candidate = m_PendingEvents[i];
                    TownCrierEvent selected = m_PendingEvents[removeIndex];

                    if (
                        candidate.Priority < selected.Priority
                        || (
                            candidate.Priority == selected.Priority
                            && candidate.CreatedUtc < selected.CreatedUtc
                        )
                    )
                    {
                        removeIndex = i;
                    }
                }

                m_PendingEvents.RemoveAt(removeIndex);
                ++m_PendingOmittedCount;
                ++m_OverflowDroppedCount;
            }
        }

        private static bool IsDuplicateWantedRosterLocked(TownCrierEvent entry, DateTime now)
        {
            DateTime cutoff = now - MurdererRepeatInterval;
            PruneDedupeLocked(m_MurdererDedupe, cutoff);

            DateTime lastSeen;

            if (
                m_MurdererDedupe.TryGetValue(entry.DedupeKey, out lastSeen)
                && lastSeen >= cutoff
            )
            {
                return true;
            }

            return IsOutstandingLocked(entry.DedupeKey);
        }

        private static bool IsDuplicateExplorationLocked(TownCrierEvent entry, DateTime now)
        {
            if (m_ExplorationRepeatInterval <= TimeSpan.Zero)
                return false;

            DateTime cutoff = now - m_ExplorationRepeatInterval;
            PruneDedupeLocked(m_ExplorationDedupe, cutoff);

            DateTime lastSeen;

            if (
                m_ExplorationDedupe.TryGetValue(entry.DedupeKey, out lastSeen)
                && lastSeen >= cutoff
            )
            {
                return true;
            }

            return m_InFlightDedupeKeys.ContainsKey(entry.DedupeKey);
        }

        private static bool IsOutstandingLocked(string dedupeKey)
        {
            if (m_InFlightDedupeKeys.ContainsKey(dedupeKey))
                return true;

            for (int i = 0; i < m_PendingEvents.Count; ++i)
            {
                if (
                    String.Equals(
                        m_PendingEvents[i].DedupeKey,
                        dedupeKey,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    return true;
                }
            }

            return false;
        }

        private static void PruneDedupeLocked(
            Dictionary<string, DateTime> dedupe,
            DateTime cutoff
        )
        {
            List<string> expiredKeys = null;

            foreach (KeyValuePair<string, DateTime> pair in dedupe)
            {
                if (pair.Value < cutoff)
                {
                    if (expiredKeys == null)
                        expiredKeys = new List<string>();

                    expiredKeys.Add(pair.Key);
                }
            }

            if (expiredKeys == null)
                return;

            for (int i = 0; i < expiredKeys.Count; ++i)
                dedupe.Remove(expiredKeys[i]);
        }

        private static void SchedulePendingTimerLocked(DateTime now)
        {
            if (m_PendingEvents.Count == 0 || m_SendInProgress || !CanSendLocked())
                return;

            TimeSpan delay = m_NextSendUtc - now;

            if (delay < TimeSpan.Zero)
                delay = TimeSpan.Zero;

            if (m_PendingTimer != null)
                m_PendingTimer.Stop();

            m_PendingTimer = Server.Timer.DelayCall(
                delay,
                new Server.TimerCallback(PendingTimer_OnTick)
            );
        }

        private static void PendingTimer_OnTick()
        {
            lock (m_SyncRoot)
            {
                m_PendingTimer = null;

                if (!CanSendLocked())
                {
                    m_PendingEvents.Clear();
                    m_PendingOmittedCount = 0;
                    return;
                }

                if (m_SendInProgress || m_PendingEvents.Count == 0)
                    return;

                DateTime now = DateTime.UtcNow;

                if (now < m_NextSendUtc)
                {
                    SchedulePendingTimerLocked(now);
                    return;
                }

                List<TownCrierEvent> batch = new List<TownCrierEvent>(m_PendingEvents);
                int omittedCount = m_PendingOmittedCount;
                m_PendingEvents.Clear();
                m_PendingOmittedCount = 0;
                StartSendLocked(batch, omittedCount, now);
            }
        }

        private static void StartSendLocked(
            List<TownCrierEvent> events,
            int omittedCount,
            DateTime now
        )
        {
            if (events == null || events.Count == 0 || !CanSendLocked())
                return;

            events.Sort(new Comparison<TownCrierEvent>(CompareEventsByCreatedUtc));

            string content = BuildMessage(events, omittedCount);
            string payload = BuildPayload(content);
            SendWorkItem work = new SendWorkItem(
                m_WebhookUrl,
                payload,
                events,
                omittedCount,
                m_ConfigGeneration,
                0
            );

            m_SendInProgress = true;
            m_NextSendUtc = now + m_MinimumInterval;
            m_InFlightDedupeKeys.Clear();

            for (int i = 0; i < events.Count; ++i)
            {
                if (events[i].IsWantedRoster || events[i].IsExploration)
                    m_InFlightDedupeKeys[events[i].DedupeKey] = true;
            }

            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(SendCallback), work))
            {
                m_SendInProgress = false;
                m_InFlightDedupeKeys.Clear();
                ++m_ExhaustedCount;
                RecordFailureLocked("The server thread pool rejected the Discord request.");
                SchedulePendingTimerLocked(now);
            }
        }

        private static void SendCallback(object state)
        {
            SendWorkItem work = state as SendWorkItem;

            if (work == null)
                return;

            SendResult result = SendOnce(work);
            Server.Timer.DelayCall(
                TimeSpan.Zero,
                new Server.TimerStateCallback(CompleteSend),
                result
            );
        }

        private static SendResult SendOnce(SendWorkItem work)
        {
            HttpWebRequest request = null;

            try
            {
                byte[] body = Encoding.UTF8.GetBytes(work.Payload);
                string separator = work.WebhookUrl.IndexOf('?') >= 0 ? "&" : "?";

                request = (HttpWebRequest)WebRequest.Create(
                    work.WebhookUrl + separator + "wait=true"
                );
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json";
                request.AllowAutoRedirect = false;
                request.KeepAlive = false;
                request.Timeout = 15000;
                request.ReadWriteTimeout = 15000;
                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(body, 0, body.Length);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    int statusCode = (int)response.StatusCode;

                    if (statusCode >= 200 && statusCode <= 299)
                        return SendResult.Success(work, statusCode);

                    return ClassifyHttpFailure(work, response);
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = ex.Response as HttpWebResponse;

                if (response != null)
                {
                    using (response)
                        return ClassifyHttpFailure(work, response);
                }

                return SendResult.Retryable(
                    work,
                    0,
                    TimeSpan.Zero,
                    "Network failure: " + ex.Status.ToString() + "."
                );
            }
            catch
            {
                return SendResult.Retryable(
                    work,
                    0,
                    TimeSpan.Zero,
                    "Unexpected failure while sending the Discord request."
                );
            }
        }

        private static SendResult ClassifyHttpFailure(
            SendWorkItem work,
            HttpWebResponse response
        )
        {
            int statusCode = (int)response.StatusCode;

            if (statusCode == 429)
            {
                TimeSpan retryAfter = ReadRetryAfter(response);
                return SendResult.Retryable(
                    work,
                    statusCode,
                    retryAfter,
                    "Discord rate limited the webhook request."
                );
            }

            if (statusCode == 401 || statusCode == 403 || statusCode == 404)
            {
                return SendResult.Permanent(
                    work,
                    statusCode,
                    "Discord rejected or could not find the configured webhook."
                );
            }

            if (statusCode >= 500 && statusCode <= 599)
            {
                return SendResult.Retryable(
                    work,
                    statusCode,
                    TimeSpan.Zero,
                    "Discord returned a temporary server error."
                );
            }

            return SendResult.Failure(
                work,
                statusCode,
                "Discord rejected the webhook payload."
            );
        }

        private static TimeSpan ReadRetryAfter(HttpWebResponse response)
        {
            string value = response.Headers["Retry-After"];
            double seconds;

            if (
                value != null
                && Double.TryParse(
                    value,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out seconds
                )
                && seconds > 0.0
            )
            {
                return TimeSpan.FromMilliseconds(Math.Ceiling(seconds * 1000.0));
            }

            DateTime retryUtc;

            if (
                value != null
                && DateTime.TryParse(
                    value,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out retryUtc
                )
                && retryUtc > DateTime.UtcNow
            )
            {
                return retryUtc - DateTime.UtcNow;
            }

            return TimeSpan.FromSeconds(5.0);
        }

        private static void CompleteSend(object state)
        {
            SendResult result = state as SendResult;

            if (result == null)
                return;

            lock (m_SyncRoot)
            {
                if (result.Work.ConfigGeneration != m_ConfigGeneration)
                {
                    m_SendInProgress = false;
                    m_InFlightDedupeKeys.Clear();
                    SchedulePendingTimerLocked(DateTime.UtcNow);
                    return;
                }

                if (result.WasSuccessful)
                {
                    DateTime now = DateTime.UtcNow;

                    m_SendInProgress = false;
                    m_LastSuccessUtc = now;
                    m_LastFailure = null;
                    m_LastFailureUtc = DateTime.MinValue;
                    ++m_SuccessfulPostCount;
                    RecordSuccessfulDedupeLocked(result.Work.Events, now);
                    m_InFlightDedupeKeys.Clear();

                    DateTime successCooldown = now + m_MinimumInterval;

                    if (m_NextSendUtc < successCooldown)
                        m_NextSendUtc = successCooldown;

                    SchedulePendingTimerLocked(now);
                    return;
                }

                if (result.IsPermanent)
                {
                    m_SendInProgress = false;
                    m_SessionDisabled = true;
                    m_PendingEvents.Clear();
                    m_PendingOmittedCount = 0;
                    m_InFlightDedupeKeys.Clear();
                    ++m_PermanentDisableCount;
                    StopPendingTimerLocked();
                    RecordFailureLocked(result.FailureMessage);
                    Console.WriteLine(
                        "[Town Crier Discord] Delivery disabled until reload after HTTP {0}.",
                        result.StatusCode
                    );
                    return;
                }

                if (result.IsRetryable && result.Work.RetryCount < MaximumRetryCount)
                {
                    TimeSpan delay = result.RetryAfter;

                    if (delay <= TimeSpan.Zero)
                        delay = GetTransientRetryDelay(result.Work.RetryCount);

                    SendWorkItem retry = result.Work.CreateRetry();
                    ++m_RetryAttemptCount;
                    RecordFailureLocked(
                        result.FailureMessage
                            + " Retry "
                            + retry.RetryCount.ToString(CultureInfo.InvariantCulture)
                            + " of "
                            + MaximumRetryCount.ToString(CultureInfo.InvariantCulture)
                            + " is scheduled."
                    );

                    Server.Timer.DelayCall(
                        delay,
                        new Server.TimerStateCallback(RetrySend),
                        retry
                    );
                    return;
                }

                m_SendInProgress = false;
                m_InFlightDedupeKeys.Clear();
                ++m_ExhaustedCount;
                RecordFailureLocked(result.FailureMessage);

                if (result.RetryAfter > TimeSpan.Zero)
                {
                    DateTime retryBoundary = DateTime.UtcNow + result.RetryAfter;

                    if (m_NextSendUtc < retryBoundary)
                        m_NextSendUtc = retryBoundary;
                }

                SchedulePendingTimerLocked(DateTime.UtcNow);
            }
        }

        private static void RecordSuccessfulDedupeLocked(
            IList<TownCrierEvent> events,
            DateTime now
        )
        {
            if (events == null)
                return;

            for (int i = 0; i < events.Count; ++i)
            {
                TownCrierEvent entry = events[i];

                if (entry.IsWantedRoster)
                    m_MurdererDedupe[entry.DedupeKey] = now;
                else if (entry.IsExploration && m_ExplorationRepeatInterval > TimeSpan.Zero)
                    m_ExplorationDedupe[entry.DedupeKey] = now;
            }
        }

        private static int CompareEventsByCreatedUtc(TownCrierEvent left, TownCrierEvent right)
        {
            return DateTime.Compare(left.CreatedUtc, right.CreatedUtc);
        }

        private static TimeSpan GetTransientRetryDelay(int retryCount)
        {
            switch (retryCount)
            {
                case 0:
                    return TimeSpan.FromSeconds(5.0);
                case 1:
                    return TimeSpan.FromSeconds(15.0);
                default:
                    return TimeSpan.FromSeconds(45.0);
            }
        }

        private static void RetrySend(object state)
        {
            SendWorkItem work = state as SendWorkItem;

            if (work == null)
                return;

            lock (m_SyncRoot)
            {
                if (
                    work.ConfigGeneration != m_ConfigGeneration
                    || !CanSendLocked()
                )
                {
                    m_SendInProgress = false;
                    SchedulePendingTimerLocked(DateTime.UtcNow);
                    return;
                }

                if (!ThreadPool.QueueUserWorkItem(new WaitCallback(SendCallback), work))
                {
                    m_SendInProgress = false;
                    m_InFlightDedupeKeys.Clear();
                    ++m_ExhaustedCount;
                    RecordFailureLocked("The server thread pool rejected a Discord retry.");
                    SchedulePendingTimerLocked(DateTime.UtcNow);
                }
            }
        }

        private static string BuildMessage(List<TownCrierEvent> events, int omittedCount)
        {
            StringBuilder message = new StringBuilder();
            message.Append("**Hear ye, hear ye!**");

            if (events.Count > 1)
                message.Append(" News from across the realm:");

            List<string> prefixes = new List<string>();
            List<string> eventTexts = new List<string>();
            List<string> suffixes = new List<string>();
            int fixedLength = message.Length;

            for (int i = 0; i < events.Count; ++i)
            {
                TownCrierEvent entry = events[i];
                string prefix = "\n- **" + entry.CategoryLabel + ":** ";
                string suffix = entry.OccurrenceCount > 1
                    ? " (reported "
                        + entry.OccurrenceCount.ToString(CultureInfo.InvariantCulture)
                        + " times)"
                    : String.Empty;

                prefixes.Add(prefix);
                eventTexts.Add(EscapeDiscordText(entry.EventText));
                suffixes.Add(suffix);
                fixedLength += prefix.Length + suffix.Length;
            }

            string omissionFooter = BuildOmissionFooter(omittedCount);
            fixedLength += omissionFooter.Length;
            int remainingTextLength = MaximumMessageLength - fixedLength;

            if (remainingTextLength < events.Count)
                remainingTextLength = events.Count;

            for (int i = 0; i < events.Count; ++i)
            {
                int remainingEvents = events.Count - i;
                int textBudget = remainingTextLength / remainingEvents;
                string eventText = TruncateEscapedText(eventTexts[i], textBudget);

                message.Append(prefixes[i]);
                message.Append(eventText);
                message.Append(suffixes[i]);
                remainingTextLength -= eventText.Length;
            }

            message.Append(omissionFooter);

            return message.ToString();
        }

        private static string BuildOmissionFooter(int omittedCount)
        {
            if (omittedCount <= 0)
                return String.Empty;

            return "\n- *"
                + omittedCount.ToString(CultureInfo.InvariantCulture)
                + (omittedCount == 1
                    ? " lower-priority report was omitted.*"
                    : " lower-priority reports were omitted.*");
        }

        private static string TruncateEscapedText(string value, int maximumLength)
        {
            if (String.IsNullOrEmpty(value) || maximumLength <= 0)
                return String.Empty;

            if (value.Length <= maximumLength)
                return value;

            if (maximumLength == 1)
                return "\u2026";

            int truncatedLength = maximumLength - 1;

            if (Char.IsHighSurrogate(value[truncatedLength - 1]))
                --truncatedLength;

            while (truncatedLength > 0 && value[truncatedLength - 1] == '\\')
                --truncatedLength;

            if (truncatedLength <= 0)
                return "\u2026";

            return value.Substring(0, truncatedLength) + "\u2026";
        }

        private static string BuildPayload(string content)
        {
            return "{\"content\":\""
                + EscapeJson(content)
                + "\",\"allowed_mentions\":{\"parse\":[]}}";
        }

        private static string EscapeDiscordText(string value)
        {
            StringBuilder escaped = new StringBuilder(value.Length);

            for (int i = 0; i < value.Length; ++i)
            {
                char c = value[i];

                if (c == '@')
                {
                    escaped.Append('@');
                    escaped.Append('\u200B');
                }
                else if ("\\*_~`>|[]()".IndexOf(c) >= 0)
                {
                    escaped.Append('\\');
                    escaped.Append(c);
                }
                else
                {
                    escaped.Append(c);
                }
            }

            return escaped.ToString();
        }

        private static string EscapeJson(string value)
        {
            StringBuilder escaped = new StringBuilder(value.Length + 16);

            for (int i = 0; i < value.Length; ++i)
            {
                char c = value[i];

                switch (c)
                {
                    case '\"':
                        escaped.Append("\\\"");
                        break;
                    case '\\':
                        escaped.Append("\\\\");
                        break;
                    case '\b':
                        escaped.Append("\\b");
                        break;
                    case '\f':
                        escaped.Append("\\f");
                        break;
                    case '\n':
                        escaped.Append("\\n");
                        break;
                    case '\r':
                        escaped.Append("\\r");
                        break;
                    case '\t':
                        escaped.Append("\\t");
                        break;
                    default:
                        if (c < 0x20)
                        {
                            escaped.Append("\\u");
                            escaped.Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            escaped.Append(c);
                        }
                        break;
                }
            }

            return escaped.ToString();
        }

        private static string NormalizeEventText(string eventText)
        {
            if (String.IsNullOrEmpty(eventText))
                return String.Empty;

            int metadataIndex = eventText.IndexOf('#');

            if (metadataIndex >= 0)
                eventText = eventText.Substring(0, metadataIndex);

            StringBuilder normalized = new StringBuilder(eventText.Length);
            bool previousWasWhitespace = false;

            for (int i = 0; i < eventText.Length; ++i)
            {
                char c = eventText[i];

                if (Char.IsWhiteSpace(c) || Char.IsControl(c))
                {
                    if (!previousWasWhitespace)
                    {
                        normalized.Append(' ');
                        previousWasWhitespace = true;
                    }
                }
                else
                {
                    normalized.Append(c);
                    previousWasWhitespace = false;
                }
            }

            return normalized.ToString().Trim();
        }

        private static string GetCategoryLabel(string category)
        {
            if (String.Equals(category, "Logging Quests", StringComparison.OrdinalIgnoreCase))
                return "Deeds";

            if (String.Equals(category, "Logging Journies", StringComparison.OrdinalIgnoreCase))
                return "Exploration";

            if (String.Equals(category, "Logging Battles", StringComparison.OrdinalIgnoreCase))
                return "Victories";

            if (String.Equals(category, "Logging Deaths", StringComparison.OrdinalIgnoreCase))
                return "Deaths";

            if (String.Equals(category, "Logging Murderers", StringComparison.OrdinalIgnoreCase))
                return "Wanted Murderers";

            if (String.Equals(category, "Logging Adventures", StringComparison.OrdinalIgnoreCase))
                return "Gossip";

            return null;
        }

        private static int GetCategoryPriority(string categoryLabel)
        {
            if (String.Equals(categoryLabel, "Test", StringComparison.OrdinalIgnoreCase))
                return 4;

            if (
                String.Equals(
                    categoryLabel,
                    "Wanted Murderers",
                    StringComparison.OrdinalIgnoreCase
                )
                || String.Equals(categoryLabel, "Deaths", StringComparison.OrdinalIgnoreCase)
                || String.Equals(categoryLabel, "Deeds", StringComparison.OrdinalIgnoreCase)
            )
            {
                return 3;
            }

            if (String.Equals(categoryLabel, "Victories", StringComparison.OrdinalIgnoreCase))
                return 2;

            if (String.Equals(categoryLabel, "Gossip", StringComparison.OrdinalIgnoreCase))
                return 1;

            return 0;
        }

        private static bool CanSendLocked()
        {
            return m_Enabled && m_Configured && !m_SessionDisabled;
        }

        private static void RecordFailureLocked(string message)
        {
            m_LastFailure = message;
            m_LastFailureUtc = DateTime.UtcNow;
        }

        private static void StopPendingTimerLocked()
        {
            if (m_PendingTimer != null)
            {
                m_PendingTimer.Stop();
                m_PendingTimer = null;
            }
        }

        private static bool ReloadConfiguration(out string message)
        {
            bool requestedEnabled = false;
            bool configured = false;
            string webhookUrl = null;
            int minimumIntervalMinutes = DefaultMinimumIntervalMinutes;
            int explorationRepeatMinutes = DefaultExplorationRepeatMinutes;
            string configurationMessage;
            string path = Path.Combine(Core.BaseDirectory, ConfigRelativePath);

            if (!File.Exists(path))
            {
                configurationMessage = "Disabled; local configuration file is not present.";
            }
            else
            {
                Dictionary<string, string> values;
                string parseError;

                if (!TryReadConfiguration(path, out values, out parseError))
                {
                    configurationMessage = "Disabled; " + parseError;
                }
                else
                {
                    string enabledValue;

                    if (!values.TryGetValue("Enabled", out enabledValue))
                    {
                        configurationMessage = "Disabled; Enabled is missing from the local configuration.";
                    }
                    else if (!Boolean.TryParse(enabledValue, out requestedEnabled))
                    {
                        configurationMessage = "Disabled; Enabled must be true or false.";
                    }
                    else if (!TryReadMinimumInterval(values, out minimumIntervalMinutes))
                    {
                        configurationMessage =
                            "Disabled; MinimumIntervalMinutes must be between 1 and 1440.";
                        requestedEnabled = false;
                    }
                    else if (
                        !TryReadExplorationRepeatInterval(values, out explorationRepeatMinutes)
                    )
                    {
                        configurationMessage =
                            "Disabled; ExplorationRepeatMinutes must be between 0 and 1440.";
                        requestedEnabled = false;
                    }
                    else
                    {
                        values.TryGetValue("WebhookUrl", out webhookUrl);
                        configured = IsValidWebhookUrl(webhookUrl);

                        if (!requestedEnabled)
                        {
                            configurationMessage = "Disabled by local configuration.";
                        }
                        else if (!configured)
                        {
                            configurationMessage =
                                "Disabled; WebhookUrl is not a valid Discord incoming webhook URL.";
                            requestedEnabled = false;
                        }
                        else
                        {
                            configurationMessage = "Enabled and ready.";
                        }
                    }
                }
            }

            lock (m_SyncRoot)
            {
                ++m_ConfigGeneration;
                m_Enabled = requestedEnabled;
                m_Configured = configured;
                m_SessionDisabled = false;
                m_WebhookUrl = configured ? webhookUrl.Trim() : null;
                m_MinimumInterval = TimeSpan.FromMinutes(minimumIntervalMinutes);
                m_ExplorationRepeatInterval = TimeSpan.FromMinutes(explorationRepeatMinutes);
                m_ConfigurationMessage = configurationMessage;
                m_LastFailure = null;
                m_LastFailureUtc = DateTime.MinValue;
                m_NextSendUtc = DateTime.MinValue;
                m_PendingEvents.Clear();
                m_PendingOmittedCount = 0;
                StopPendingTimerLocked();
            }

            message = configurationMessage;
            return requestedEnabled && configured;
        }

        private static bool TryReadConfiguration(
            string path,
            out Dictionary<string, string> values,
            out string error
        )
        {
            values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            error = null;

            try
            {
                string[] lines = File.ReadAllLines(path);

                for (int i = 0; i < lines.Length; ++i)
                {
                    string line = lines[i].Trim();

                    if (line.Length == 0 || line.StartsWith("#") || line.StartsWith(";"))
                        continue;

                    int separatorIndex = line.IndexOf('=');

                    if (separatorIndex <= 0)
                    {
                        error = "the local configuration contains an invalid line.";
                        return false;
                    }

                    string key = line.Substring(0, separatorIndex).Trim();
                    string value = line.Substring(separatorIndex + 1).Trim();

                    if (key.Length == 0)
                    {
                        error = "the local configuration contains an empty key.";
                        return false;
                    }

                    values[key] = value;
                }

                return true;
            }
            catch
            {
                error = "the local configuration could not be read.";
                return false;
            }
        }

        private static bool TryReadMinimumInterval(
            Dictionary<string, string> values,
            out int minimumIntervalMinutes
        )
        {
            minimumIntervalMinutes = DefaultMinimumIntervalMinutes;
            string value;

            if (!values.TryGetValue("MinimumIntervalMinutes", out value))
                return true;

            return Int32.TryParse(
                    value,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out minimumIntervalMinutes
                )
                && minimumIntervalMinutes >= 1
                && minimumIntervalMinutes <= MaximumMinimumIntervalMinutes;
        }

        private static bool TryReadExplorationRepeatInterval(
            Dictionary<string, string> values,
            out int explorationRepeatMinutes
        )
        {
            explorationRepeatMinutes = DefaultExplorationRepeatMinutes;
            string value;

            if (!values.TryGetValue("ExplorationRepeatMinutes", out value))
                return true;

            return Int32.TryParse(
                    value,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out explorationRepeatMinutes
                )
                && explorationRepeatMinutes >= 0
                && explorationRepeatMinutes <= MaximumExplorationRepeatMinutes;
        }

        private static bool IsValidWebhookUrl(string value)
        {
            if (String.IsNullOrEmpty(value))
                return false;

            Uri uri;

            if (!Uri.TryCreate(value.Trim(), UriKind.Absolute, out uri))
                return false;

            if (!String.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!String.Equals(uri.Host, "discord.com", StringComparison.OrdinalIgnoreCase))
                return false;

            if (uri.UserInfo.Length != 0 || uri.Fragment.Length != 0)
                return false;

            const string requiredPrefix = "/api/webhooks/";

            if (!uri.AbsolutePath.StartsWith(requiredPrefix, StringComparison.OrdinalIgnoreCase))
                return false;

            string remainder = uri.AbsolutePath.Substring(requiredPrefix.Length);
            string[] segments = remainder.Split('/');

            return segments.Length == 2 && segments[0].Length > 0 && segments[1].Length > 0;
        }

        [Usage("TownCrierDiscord <status|test|reload>")]
        [Description("Displays, tests, or reloads the Town Crier Discord webhook bridge.")]
        private static void TownCrierDiscord_OnCommand(CommandEventArgs e)
        {
            if (e == null || e.Mobile == null || e.Mobile.Deleted)
                return;

            Mobile from = e.Mobile;

            if (e.Length != 1)
            {
                from.SendMessage("Usage: [TownCrierDiscord <status|test|reload>");
                return;
            }

            string action = e.GetString(0).ToLowerInvariant();

            switch (action)
            {
                case "status":
                    SendStatus(from);
                    break;
                case "test":
                    QueueTest(from);
                    break;
                case "reload":
                {
                    string message;
                    ReloadConfiguration(out message);
                    from.SendMessage("Town Crier Discord: {0}", message);
                    break;
                }
                default:
                    from.SendMessage("Usage: [TownCrierDiscord <status|test|reload>");
                    break;
            }
        }

        private static void SendStatus(Mobile from)
        {
            lock (m_SyncRoot)
            {
                from.SendMessage("Town Crier Discord: {0}", m_ConfigurationMessage);
                from.SendMessage(
                    "Webhook configured: {0}; session disabled: {1}; send active: {2}; pending: {3}; pending omissions: {4}.",
                    m_Configured ? "yes" : "no",
                    m_SessionDisabled ? "yes" : "no",
                    m_SendInProgress ? "yes" : "no",
                    m_PendingEvents.Count,
                    m_PendingOmittedCount
                );
                from.SendMessage(
                    "Minimum interval: {0} minute(s); exploration repeat window: {1} minute(s).",
                    m_MinimumInterval.TotalMinutes.ToString(
                        "0.##",
                        CultureInfo.InvariantCulture
                    ),
                    m_ExplorationRepeatInterval.TotalMinutes.ToString(
                        "0.##",
                        CultureInfo.InvariantCulture
                    )
                );
                from.SendMessage(
                    "Session events: accepted {0}; coalesced {1}; suppressed {2}; overflow-dropped {3}.",
                    m_AcceptedCount,
                    m_CoalescedCount,
                    m_SuppressedCount,
                    m_OverflowDroppedCount
                );
                from.SendMessage(
                    "Session delivery: successful {0}; retries {1}; exhausted {2}; permanent disables {3}.",
                    m_SuccessfulPostCount,
                    m_RetryAttemptCount,
                    m_ExhaustedCount,
                    m_PermanentDisableCount
                );

                if (m_NextSendUtc > DateTime.UtcNow)
                    from.SendMessage("Next eligible post: {0:u}.", m_NextSendUtc);

                if (m_LastSuccessUtc != DateTime.MinValue)
                    from.SendMessage("Last successful post: {0:u}.", m_LastSuccessUtc);

                if (m_LastFailure != null)
                {
                    from.SendMessage(
                        "Last failure at {0:u}: {1}",
                        m_LastFailureUtc,
                        m_LastFailure
                    );
                }
            }
        }

        private static void QueueTest(Mobile from)
        {
            lock (m_SyncRoot)
            {
                if (!CanSendLocked())
                {
                    from.SendMessage(
                        "Town Crier Discord is not ready. Use [TownCrierDiscord status for details."
                    );
                    return;
                }

                DateTime now = DateTime.UtcNow;
                TownCrierEvent entry = new TownCrierEvent(
                    "Test",
                    "The Town Crier Discord bridge is working.",
                    TownCrierEventKind.Normal,
                    "The Town Crier Discord bridge is working.",
                    now
                );

                ++m_AcceptedCount;
                QueueEventLocked(entry, now);
                from.SendMessage(
                    "Town Crier Discord test queued. It may wait for the current cooldown."
                );
            }
        }

        private enum TownCrierEventKind
        {
            Normal,
            Exploration,
            WantedRoster,
        }

        private sealed class TownCrierEvent
        {
            private readonly string m_CategoryLabel;
            private readonly string m_EventText;
            private readonly TownCrierEventKind m_Kind;
            private readonly DateTime m_CreatedUtc;
            private readonly string m_DedupeKey;
            private readonly int m_Priority;
            private int m_OccurrenceCount;

            public string CategoryLabel
            {
                get { return m_CategoryLabel; }
            }

            public string EventText
            {
                get { return m_EventText; }
            }

            public bool IsWantedRoster
            {
                get { return m_Kind == TownCrierEventKind.WantedRoster; }
            }

            public bool IsExploration
            {
                get { return m_Kind == TownCrierEventKind.Exploration; }
            }

            public DateTime CreatedUtc
            {
                get { return m_CreatedUtc; }
            }

            public string DedupeKey
            {
                get { return m_DedupeKey; }
            }

            public int Priority
            {
                get { return m_Priority; }
            }

            public int OccurrenceCount
            {
                get { return m_OccurrenceCount; }
            }

            public TownCrierEvent(
                string categoryLabel,
                string eventText,
                TownCrierEventKind kind,
                string fingerprint,
                DateTime createdUtc
            )
            {
                m_CategoryLabel = categoryLabel;
                m_EventText = eventText;
                m_Kind = kind;
                m_CreatedUtc = createdUtc;
                m_DedupeKey = categoryLabel + "\n" + fingerprint;
                m_Priority = GetCategoryPriority(categoryLabel);
                m_OccurrenceCount = 1;
            }

            public void IncrementOccurrence()
            {
                if (m_OccurrenceCount < Int32.MaxValue)
                    ++m_OccurrenceCount;
            }
        }

        private sealed class SendWorkItem
        {
            private readonly string m_WebhookUrl;
            private readonly string m_Payload;
            private readonly List<TownCrierEvent> m_Events;
            private readonly int m_OmittedCount;
            private readonly int m_ConfigGeneration;
            private readonly int m_RetryCount;

            public string WebhookUrl
            {
                get { return m_WebhookUrl; }
            }

            public string Payload
            {
                get { return m_Payload; }
            }

            public int ConfigGeneration
            {
                get { return m_ConfigGeneration; }
            }

            public IList<TownCrierEvent> Events
            {
                get { return m_Events; }
            }

            public int OmittedCount
            {
                get { return m_OmittedCount; }
            }

            public int RetryCount
            {
                get { return m_RetryCount; }
            }

            public SendWorkItem(
                string webhookUrl,
                string payload,
                IList<TownCrierEvent> events,
                int omittedCount,
                int configGeneration,
                int retryCount
            )
            {
                m_WebhookUrl = webhookUrl;
                m_Payload = payload;
                m_Events = events == null
                    ? new List<TownCrierEvent>()
                    : new List<TownCrierEvent>(events);
                m_OmittedCount = omittedCount;
                m_ConfigGeneration = configGeneration;
                m_RetryCount = retryCount;
            }

            public SendWorkItem CreateRetry()
            {
                return new SendWorkItem(
                    m_WebhookUrl,
                    m_Payload,
                    m_Events,
                    m_OmittedCount,
                    m_ConfigGeneration,
                    m_RetryCount + 1
                );
            }
        }

        private sealed class SendResult
        {
            private readonly SendWorkItem m_Work;
            private readonly bool m_WasSuccessful;
            private readonly bool m_IsRetryable;
            private readonly bool m_IsPermanent;
            private readonly int m_StatusCode;
            private readonly TimeSpan m_RetryAfter;
            private readonly string m_FailureMessage;

            public SendWorkItem Work
            {
                get { return m_Work; }
            }

            public bool WasSuccessful
            {
                get { return m_WasSuccessful; }
            }

            public bool IsRetryable
            {
                get { return m_IsRetryable; }
            }

            public bool IsPermanent
            {
                get { return m_IsPermanent; }
            }

            public int StatusCode
            {
                get { return m_StatusCode; }
            }

            public TimeSpan RetryAfter
            {
                get { return m_RetryAfter; }
            }

            public string FailureMessage
            {
                get { return m_FailureMessage; }
            }

            private SendResult(
                SendWorkItem work,
                bool wasSuccessful,
                bool isRetryable,
                bool isPermanent,
                int statusCode,
                TimeSpan retryAfter,
                string failureMessage
            )
            {
                m_Work = work;
                m_WasSuccessful = wasSuccessful;
                m_IsRetryable = isRetryable;
                m_IsPermanent = isPermanent;
                m_StatusCode = statusCode;
                m_RetryAfter = retryAfter;
                m_FailureMessage = failureMessage;
            }

            public static SendResult Success(SendWorkItem work, int statusCode)
            {
                return new SendResult(
                    work,
                    true,
                    false,
                    false,
                    statusCode,
                    TimeSpan.Zero,
                    null
                );
            }

            public static SendResult Retryable(
                SendWorkItem work,
                int statusCode,
                TimeSpan retryAfter,
                string failureMessage
            )
            {
                return new SendResult(
                    work,
                    false,
                    true,
                    false,
                    statusCode,
                    retryAfter,
                    failureMessage
                );
            }

            public static SendResult Permanent(
                SendWorkItem work,
                int statusCode,
                string failureMessage
            )
            {
                return new SendResult(
                    work,
                    false,
                    false,
                    true,
                    statusCode,
                    TimeSpan.Zero,
                    failureMessage
                );
            }

            public static SendResult Failure(
                SendWorkItem work,
                int statusCode,
                string failureMessage
            )
            {
                return new SendResult(
                    work,
                    false,
                    false,
                    false,
                    statusCode,
                    TimeSpan.Zero,
                    failureMessage
                );
            }
        }
    }
}
