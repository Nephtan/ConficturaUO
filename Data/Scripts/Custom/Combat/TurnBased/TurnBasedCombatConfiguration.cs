using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Server.Mobiles;

namespace Server.Custom.Confictura
{
    public sealed class TurnActionRule
    {
        public string Key;
        public string RuntimeType;
        public TurnActionKind Kind;
        public bool Allowed;
        public string APPolicy;
        public int APValue;
        public string CommitPolicy;
        public string TargetPolicy;
        public string FailureMessage;
    }

    public sealed class TurnEffectRule
    {
        public string Key;
        public string RuntimeType;
        public TurnMutationKind Kind;
        public string ClockPolicy;
        public string TickPhase;
        public string Adapter;
    }

    public sealed class TurnAIRule
    {
        public string RuntimeType;
        public string Strategy;
        public string PulsePolicy;
        public string SpecialHandling;
        public string TestID;
    }

    public sealed class TurnBasedCombatConfiguration
    {
        private readonly Dictionary<TurnActionKind, List<TurnActionRule>> m_ActionRules;
        private readonly Dictionary<TurnMutationKind, List<TurnEffectRule>> m_EffectRules;
        private readonly List<TurnAIRule> m_AIRules;

        public bool Enabled;
        public bool RequireCompatibilityGate;
        public int ActionPoints;
        public double ActorSeconds;
        public int PlayerTimeoutSeconds;
        public int DisconnectGraceSeconds;
        public int EscapeRange;
        public int MaxAIDecisionsPerTurn;
        public int SchedulerSliceMilliseconds;
        public int HudRefreshMilliseconds;
        public bool LogEnabled;
        public int CatalogVersion;

        public int EffectRuleCount
        {
            get
            {
                int count = 0;

                foreach (List<TurnEffectRule> rules in m_EffectRules.Values)
                    count += rules.Count;

                return count;
            }
        }

        public int AIRuleCount
        {
            get { return m_AIRules.Count; }
        }

        public TurnBasedCombatConfiguration()
        {
            m_ActionRules = new Dictionary<TurnActionKind, List<TurnActionRule>>();
            m_EffectRules = new Dictionary<TurnMutationKind, List<TurnEffectRule>>();
            m_AIRules = new List<TurnAIRule>();
            Enabled = false;
            RequireCompatibilityGate = true;
            ActionPoints = 20;
            ActorSeconds = 5.0;
            PlayerTimeoutSeconds = 30;
            DisconnectGraceSeconds = 300;
            EscapeRange = 18;
            MaxAIDecisionsPerTurn = 80;
            SchedulerSliceMilliseconds = 25;
            HudRefreshMilliseconds = 1000;
            LogEnabled = true;
            CatalogVersion = 1;
        }

        public static string DataDirectory
        {
            get { return Path.Combine(Core.BaseDirectory, "Data", "TurnBasedCombat"); }
        }

        public static TurnBasedCombatConfiguration Load()
        {
            TurnBasedCombatConfiguration config = new TurnBasedCombatConfiguration();
            config.LoadSettings(Path.Combine(DataDirectory, "TurnBasedCombat.cfg"));
            config.LoadActions(Path.Combine(DataDirectory, "TurnActions.csv"));
            config.LoadEffects(Path.Combine(DataDirectory, "TurnEffects.csv"));
            config.LoadAI(Path.Combine(DataDirectory, "TurnAI.csv"));
            config.Validate();
            return config;
        }

        public TurnEffectRule ResolveEffect(TurnMutationKind kind, object context)
        {
            List<TurnEffectRule> rules;

            if (!m_EffectRules.TryGetValue(kind, out rules))
                return null;

            string runtimeType = context == null ? null : context.GetType().FullName;

            for (int i = 0; i < rules.Count; ++i)
            {
                if (!String.IsNullOrEmpty(runtimeType) && rules[i].RuntimeType == runtimeType)
                    return rules[i];
            }

            for (int i = 0; i < rules.Count; ++i)
            {
                if (rules[i].RuntimeType == "*")
                    return rules[i];
            }

            return null;
        }

        public TurnAIRule ResolveAI(BaseCreature creature)
        {
            if (creature == null || creature.AIObject == null)
                return null;

            Type mobileType = creature.GetType();
            Type aiType = creature.AIObject.GetType();

            for (int i = 0; i < m_AIRules.Count; ++i)
            {
                if (
                    m_AIRules[i].RuntimeType == mobileType.FullName
                    || m_AIRules[i].RuntimeType == aiType.FullName
                )
                    return m_AIRules[i];
            }

            Type baseType = mobileType.BaseType;

            while (baseType != null)
            {
                for (int i = 0; i < m_AIRules.Count; ++i)
                {
                    if (m_AIRules[i].RuntimeType == baseType.FullName)
                        return m_AIRules[i];
                }

                baseType = baseType.BaseType;
            }

            return null;
        }

        public TurnActionRule ResolveAction(TurnActionRequest request)
        {
            List<TurnActionRule> rules;

            if (!m_ActionRules.TryGetValue(request.Kind, out rules))
                return null;

            string runtimeType = null;

            if (request.Context != null)
                runtimeType = request.Context.GetType().FullName;

            for (int i = 0; i < rules.Count; ++i)
            {
                TurnActionRule rule = rules[i];

                if (!String.IsNullOrEmpty(runtimeType) && rule.RuntimeType == runtimeType)
                    return rule;
            }

            for (int i = 0; i < rules.Count; ++i)
            {
                if (rules[i].RuntimeType == "*")
                    return rules[i];
            }

            return null;
        }

        public TurnActionRule FindActionRule(TurnActionKind kind, string runtimeType)
        {
            List<TurnActionRule> rules;

            if (!m_ActionRules.TryGetValue(kind, out rules))
                return null;

            for (int i = 0; i < rules.Count; ++i)
            {
                if (rules[i].RuntimeType == runtimeType)
                    return rules[i];
            }

            return null;
        }

        public bool CheckCompatibilityGate(out string reason)
        {
            reason = null;

            if (!RequireCompatibilityGate)
                return true;

            string path = Path.Combine(
                Core.BaseDirectory,
                "docs",
                "turn-based-combat",
                "compatibility-register.csv"
            );

            if (!File.Exists(path))
            {
                reason = "The compatibility register is missing. Run Generate-TurnBasedCombatCompatibility.ps1.";
                return false;
            }

            Dictionary<string, string> expectedHashes = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase
            );

            using (StreamReader reader = new StreamReader(path))
            {
                string headerLine = reader.ReadLine();

                if (headerLine == null)
                {
                    reason = "The compatibility register is empty.";
                    return false;
                }

                List<string> header = ParseCsvLine(headerLine);
                int dispositionIndex = header.IndexOf("Disposition");
                int fileIndex = header.IndexOf("File");
                int sourceHashIndex = header.IndexOf("SourceHash");

                if (dispositionIndex < 0 || fileIndex < 0 || sourceHashIndex < 0)
                {
                    reason = "The compatibility register is missing File, SourceHash, or Disposition.";
                    return false;
                }

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    List<string> fields = ParseCsvLine(line);

                    if (dispositionIndex < fields.Count && fields[dispositionIndex] == "Unknown")
                    {
                        reason = "The compatibility register still contains Unknown dispositions.";
                        return false;
                    }

                    if (fileIndex >= fields.Count || sourceHashIndex >= fields.Count)
                    {
                        reason = "The compatibility register contains a malformed row.";
                        return false;
                    }

                    string registeredFile = fields[fileIndex].Replace('\\', '/');
                    string registeredHash = fields[sourceHashIndex];
                    string priorHash;

                    if (
                        expectedHashes.TryGetValue(registeredFile, out priorHash)
                        && !String.Equals(priorHash, registeredHash, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        reason = "The compatibility register contains conflicting hashes for " + registeredFile;
                        return false;
                    }

                    expectedHashes[registeredFile] = registeredHash;
                }
            }

            string scriptRoot = Path.Combine(Core.BaseDirectory, "Data", "Scripts");
            string[] runtimeFiles = Directory.GetFiles(scriptRoot, "*.cs", SearchOption.AllDirectories);
            int currentCount = 0;

            for (int i = 0; i < runtimeFiles.Length; ++i)
            {
                string fullPath = runtimeFiles[i];
                string normalizedPath = fullPath.Replace('\\', '/');

                if (normalizedPath.IndexOf("/bin/", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                if (normalizedPath.IndexOf("/obj/", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                ++currentCount;
                string relative = fullPath.Substring(Core.BaseDirectory.Length)
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .Replace('\\', '/');
                string expectedHash;

                if (!expectedHashes.TryGetValue(relative, out expectedHash))
                {
                    reason = "Compatibility drift: unregistered runtime script " + relative;
                    return false;
                }

                string currentHash = ComputeFileHash(fullPath);

                if (!String.Equals(currentHash, expectedHash, StringComparison.OrdinalIgnoreCase))
                {
                    reason = "Compatibility drift: runtime script changed after generation: " + relative;
                    return false;
                }
            }

            if (currentCount != expectedHashes.Count)
            {
                reason = "Compatibility drift: the register contains scripts that are no longer present.";
                return false;
            }

            return true;
        }

        private static string ComputeFileHash(string path)
        {
            string content = File.ReadAllText(path);
            string normalized = content.Replace("\r\n", "\n").Replace("\r", "\n");
            byte[] source = new UTF8Encoding(false).GetBytes(normalized);

            using (SHA256 algorithm = SHA256.Create())
            {
                byte[] hash = algorithm.ComputeHash(source);
                StringBuilder builder = new StringBuilder(hash.Length * 2);

                for (int i = 0; i < hash.Length; ++i)
                    builder.Append(hash[i].ToString("X2", CultureInfo.InvariantCulture));

                return builder.ToString();
            }
        }

        private void LoadSettings(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Turn-based combat configuration was not found.", path);

            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i].Trim();

                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                int separator = line.IndexOf('=');

                if (separator <= 0)
                    throw new FormatException("Invalid turn-based combat setting at line " + (i + 1));

                string key = line.Substring(0, separator).Trim();
                string value = line.Substring(separator + 1).Trim();

                switch (key)
                {
                    case "Enabled": Enabled = ParseBoolean(key, value); break;
                    case "RequireCompatibilityGate": RequireCompatibilityGate = ParseBoolean(key, value); break;
                    case "ActionPoints": ActionPoints = ParseInteger(key, value); break;
                    case "ActorSeconds": ActorSeconds = ParseDouble(key, value); break;
                    case "PlayerTimeoutSeconds": PlayerTimeoutSeconds = ParseInteger(key, value); break;
                    case "DisconnectGraceSeconds": DisconnectGraceSeconds = ParseInteger(key, value); break;
                    case "EscapeRange": EscapeRange = ParseInteger(key, value); break;
                    case "MaxAIDecisionsPerTurn": MaxAIDecisionsPerTurn = ParseInteger(key, value); break;
                    case "SchedulerSliceMilliseconds": SchedulerSliceMilliseconds = ParseInteger(key, value); break;
                    case "HudRefreshMilliseconds": HudRefreshMilliseconds = ParseInteger(key, value); break;
                    case "LogEnabled": LogEnabled = ParseBoolean(key, value); break;
                    default: throw new FormatException("Unknown turn-based combat setting: " + key);
                }
            }
        }

        private void LoadActions(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Turn action catalog was not found.", path);

            using (StreamReader reader = new StreamReader(path))
            {
                string headerLine = reader.ReadLine();

                if (headerLine == null)
                    throw new FormatException("Turn action catalog is empty.");

                List<string> header = ParseCsvLine(headerLine);
                Dictionary<string, int> columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < header.Count; ++i)
                    columns[header[i]] = i;

                string line;
                int lineNumber = 1;

                while ((line = reader.ReadLine()) != null)
                {
                    ++lineNumber;

                    if (String.IsNullOrWhiteSpace(line))
                        continue;

                    List<string> fields = ParseCsvLine(line);
                    TurnActionRule rule = new TurnActionRule();
                    rule.Key = GetField(fields, columns, "Key", lineNumber);
                    rule.RuntimeType = GetField(fields, columns, "RuntimeType", lineNumber);
                    rule.Kind = (TurnActionKind)Enum.Parse(
                        typeof(TurnActionKind),
                        GetField(fields, columns, "ActionKind", lineNumber),
                        true
                    );
                    rule.Allowed = ParseBoolean("Allowed", GetField(fields, columns, "Allowed", lineNumber));
                    rule.APPolicy = GetField(fields, columns, "APPolicy", lineNumber);
                    rule.APValue = ParseInteger("APValue", GetField(fields, columns, "APValue", lineNumber));
                    rule.CommitPolicy = GetField(fields, columns, "CommitPolicy", lineNumber);
                    rule.TargetPolicy = GetField(fields, columns, "TargetPolicy", lineNumber);
                    rule.FailureMessage = GetField(fields, columns, "FailureMessage", lineNumber);

                    List<TurnActionRule> rules;

                    if (!m_ActionRules.TryGetValue(rule.Kind, out rules))
                    {
                        rules = new List<TurnActionRule>();
                        m_ActionRules[rule.Kind] = rules;
                    }

                    rules.Add(rule);
                }
            }
        }

        private void LoadEffects(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Turn effect catalog was not found.", path);

            using (StreamReader reader = new StreamReader(path))
            {
                string headerLine = reader.ReadLine();

                if (headerLine == null)
                    throw new FormatException("Turn effect catalog is empty.");

                List<string> header = ParseCsvLine(headerLine);
                Dictionary<string, int> columns = BuildColumns(header);
                string line;
                int lineNumber = 1;

                while ((line = reader.ReadLine()) != null)
                {
                    ++lineNumber;

                    if (String.IsNullOrWhiteSpace(line))
                        continue;

                    List<string> fields = ParseCsvLine(line);
                    TurnEffectRule rule = new TurnEffectRule();
                    rule.Key = GetField(fields, columns, "Key", lineNumber);
                    rule.RuntimeType = GetField(fields, columns, "RuntimeType", lineNumber);
                    rule.Kind = (TurnMutationKind)Enum.Parse(
                        typeof(TurnMutationKind),
                        GetField(fields, columns, "MutationKind", lineNumber),
                        true
                    );
                    rule.ClockPolicy = GetField(fields, columns, "ClockPolicy", lineNumber);
                    rule.TickPhase = GetField(fields, columns, "TickPhase", lineNumber);
                    rule.Adapter = GetField(fields, columns, "Adapter", lineNumber);

                    List<TurnEffectRule> rules;

                    if (!m_EffectRules.TryGetValue(rule.Kind, out rules))
                    {
                        rules = new List<TurnEffectRule>();
                        m_EffectRules[rule.Kind] = rules;
                    }

                    rules.Add(rule);
                }
            }
        }

        private void LoadAI(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Turn AI catalog was not found.", path);

            using (StreamReader reader = new StreamReader(path))
            {
                string headerLine = reader.ReadLine();

                if (headerLine == null)
                    throw new FormatException("Turn AI catalog is empty.");

                List<string> header = ParseCsvLine(headerLine);
                Dictionary<string, int> columns = BuildColumns(header);
                string line;
                int lineNumber = 1;

                while ((line = reader.ReadLine()) != null)
                {
                    ++lineNumber;

                    if (String.IsNullOrWhiteSpace(line))
                        continue;

                    List<string> fields = ParseCsvLine(line);
                    TurnAIRule rule = new TurnAIRule();
                    rule.RuntimeType = GetField(fields, columns, "RuntimeType", lineNumber);
                    rule.Strategy = GetField(fields, columns, "Strategy", lineNumber);
                    rule.PulsePolicy = GetField(fields, columns, "PulsePolicy", lineNumber);
                    rule.SpecialHandling = GetField(fields, columns, "SpecialHandling", lineNumber);
                    rule.TestID = GetField(fields, columns, "TestId", lineNumber);
                    m_AIRules.Add(rule);
                }
            }
        }

        private static Dictionary<string, int> BuildColumns(List<string> header)
        {
            Dictionary<string, int> columns = new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase
            );

            for (int i = 0; i < header.Count; ++i)
                columns[header[i]] = i;

            return columns;
        }

        private void Validate()
        {
            if (ActionPoints <= 0 || ActionPoints > 1000)
                throw new FormatException("ActionPoints must be between 1 and 1000.");

            if (ActorSeconds <= 0.0 || ActorSeconds > 60.0)
                throw new FormatException("ActorSeconds must be greater than 0 and at most 60.");

            if (PlayerTimeoutSeconds <= 0 || DisconnectGraceSeconds <= 0 || EscapeRange <= 0)
                throw new FormatException("Timeouts and EscapeRange must be positive.");

            if (MaxAIDecisionsPerTurn <= 0 || SchedulerSliceMilliseconds <= 0)
                throw new FormatException("AI and scheduler safety limits must be positive.");

            if (m_ActionRules.Count == 0)
                throw new FormatException("TurnActions.csv contains no action rules.");

            if (m_EffectRules.Count == 0)
                throw new FormatException("TurnEffects.csv contains no effect rules.");

            if (m_AIRules.Count == 0)
                throw new FormatException("TurnAI.csv contains no AI rules.");
        }

        public static List<string> ParseCsvLine(string line)
        {
            List<string> fields = new List<string>();
            System.Text.StringBuilder field = new System.Text.StringBuilder();
            bool quoted = false;

            for (int i = 0; i < line.Length; ++i)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (quoted && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        field.Append('"');
                        ++i;
                    }
                    else
                    {
                        quoted = !quoted;
                    }
                }
                else if (c == ',' && !quoted)
                {
                    fields.Add(field.ToString());
                    field.Length = 0;
                }
                else
                {
                    field.Append(c);
                }
            }

            fields.Add(field.ToString());
            return fields;
        }

        private static string GetField(
            List<string> fields,
            Dictionary<string, int> columns,
            string name,
            int lineNumber
        )
        {
            int index;

            if (!columns.TryGetValue(name, out index) || index >= fields.Count)
                throw new FormatException("Missing " + name + " at catalog line " + lineNumber);

            return fields[index].Trim();
        }

        private static bool ParseBoolean(string name, string value)
        {
            bool result;

            if (!Boolean.TryParse(value, out result))
                throw new FormatException(name + " must be true or false.");

            return result;
        }

        private static int ParseInteger(string name, string value)
        {
            int result;

            if (!Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                throw new FormatException(name + " must be an integer.");

            return result;
        }

        private static double ParseDouble(string name, string value)
        {
            double result;

            if (!Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                throw new FormatException(name + " must be a number.");

            return result;
        }
    }
}
