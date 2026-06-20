using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.CSharp;
using Server;

namespace CCWM.Tools.LiveStateExporter
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                ExportOptions options = ExportOptions.Parse(args);
                string repoRoot = ResolveRepositoryRoot();
                string savesPath = Path.GetFullPath(Path.Combine(repoRoot, options.SavesPath));

                if (!Directory.Exists(savesPath))
                {
                    Console.Error.WriteLine("Saves directory does not exist: {0}", savesPath);
                    return 2;
                }

                if (!String.Equals(Path.GetFileName(savesPath), "Saves", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine("World.Load uses fixed relative Saves paths. The saves directory must be named Saves.");
                    return 2;
                }

                Directory.SetCurrentDirectory(Path.GetDirectoryName(savesPath));
                Directory.CreateDirectory(Path.GetFullPath(Path.Combine(repoRoot, options.OutputPath)));

                BootstrapServerTypes(repoRoot);
                World.Load();

                ExportSnapshot(repoRoot, options);
                return 0;
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine(ex.Message);
                PrintUsage();
                return 2;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }
        }

        private static void PrintUsage()
        {
            Console.Error.WriteLine("Usage: CCWMLiveStateExporter --saves Saves --out docs/CCWM/live-state --scan Map,x,y,z,range --run N");
        }

        private static string ResolveRepositoryRoot()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            while (!String.IsNullOrEmpty(directory))
            {
                if (Directory.Exists(Path.Combine(directory, "Data")) && Directory.Exists(Path.Combine(directory, "docs", "CCWM")))
                {
                    return directory;
                }

                DirectoryInfo parent = Directory.GetParent(directory);
                if (parent == null)
                {
                    break;
                }

                directory = parent.FullName;
            }

            return Directory.GetCurrentDirectory();
        }

        private static void BootstrapServerTypes(string repoRoot)
        {
            RegisterAssemblyResolver(repoRoot);
            Core.Assembly = typeof(Core).Assembly;
            SetCoreField("m_BaseDirectory", repoRoot);
            SetCoreField("m_ExePath", typeof(Core).Assembly.Location);
            SetCoreField("m_Process", Process.GetCurrentProcess());
            SetCoreField("m_Thread", Thread.CurrentThread);
            SetCoreField("m_MultiConOut", new MultiTextWriter(Console.Out));
            ScriptCompiler.Assemblies = new Assembly[] { CompileScripts(repoRoot) };
            try
            {
                ScriptCompiler.Invoke("Configure");
            }
            catch (ReflectionTypeLoadException ex)
            {
                for (int index = 0; index < ex.LoaderExceptions.Length; index++)
                {
                    if (ex.LoaderExceptions[index] != null)
                    {
                        Console.Error.WriteLine(ex.LoaderExceptions[index].Message);
                    }
                }

                throw;
            }
        }

        private static void RegisterAssemblyResolver(string repoRoot)
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                AssemblyName name = new AssemblyName(args.Name);
                string fileName = name.Name + ".dll";
                string[] candidates = new string[]
                {
                    Path.Combine(repoRoot, fileName),
                    Path.Combine(repoRoot, "Data", fileName),
                    Path.Combine(repoRoot, "Data", "System", "Source", "bin", "Release", fileName),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)
                };

                for (int index = 0; index < candidates.Length; index++)
                {
                    if (File.Exists(candidates[index]))
                    {
                        return Assembly.LoadFrom(candidates[index]);
                    }
                }

                return null;
            };
        }

        private static Assembly CompileScripts(string repoRoot)
        {
            string scriptsRoot = Path.Combine(repoRoot, "Data", "Scripts");
            if (!Directory.Exists(scriptsRoot))
            {
                throw new DirectoryNotFoundException(scriptsRoot);
            }

            string[] discoveredScriptFiles = Directory.GetFiles(scriptsRoot, "*.cs", SearchOption.AllDirectories);
            List<string> filteredScriptFiles = new List<string>();
            for (int index = 0; index < discoveredScriptFiles.Length; index++)
            {
                string scriptFile = discoveredScriptFiles[index];
                string relative = scriptFile.Substring(scriptsRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                if (IsGeneratedBuildPath(relative))
                {
                    continue;
                }

                filteredScriptFiles.Add(scriptFile);
            }

            string[] scriptFiles = filteredScriptFiles.ToArray();
            if (scriptFiles.Length == 0)
            {
                throw new InvalidOperationException("No Data/Scripts C# files were found.");
            }

            Array.Sort(scriptFiles, StringComparer.OrdinalIgnoreCase);

            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;
                parameters.IncludeDebugInformation = false;
                parameters.CompilerOptions = ScriptCompiler.GetDefines();

                foreach (string reference in ResolveScriptReferences(repoRoot))
                {
                    parameters.ReferencedAssemblies.Add(reference);
                }

                CompilerResults results = provider.CompileAssemblyFromFile(parameters, scriptFiles);
                if (results.Errors.HasErrors)
                {
                    ScriptCompiler.Display(results);
                    throw new InvalidOperationException("Data/Scripts compile failed; see compiler output above.");
                }

                return results.CompiledAssembly;
            }
        }

        private static bool IsGeneratedBuildPath(string relativePath)
        {
            string normalized = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return normalized.StartsWith("bin" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                || normalized.StartsWith("obj" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                || normalized.IndexOf(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) >= 0
                || normalized.IndexOf(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static IEnumerable<string> ResolveScriptReferences(string repoRoot)
        {
            List<string> references = new List<string>();
            references.Add(typeof(Core).Assembly.Location);
            references.Add("System.dll");
            references.Add("System.Core.dll");
            references.Add("System.Data.dll");
            references.Add("System.Drawing.dll");
            references.Add("System.Web.dll");
            references.Add("System.Windows.Forms.dll");
            references.Add("System.Xml.dll");
            references.Add("System.Runtime.Remoting.dll");
            references.Add("Microsoft.CSharp.dll");

            string cfgPath = Path.Combine(repoRoot, "Data", "System", "CFG", "Assemblies.cfg");
            if (File.Exists(cfgPath))
            {
                foreach (string rawLine in File.ReadAllLines(cfgPath))
                {
                    string line = rawLine.Trim();
                    if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    string resolved = ResolveReference(repoRoot, line);
                    if (!references.Contains(resolved))
                    {
                        references.Add(resolved);
                    }
                }
            }

            return references;
        }

        private static string ResolveReference(string repoRoot, string reference)
        {
            if (Path.IsPathRooted(reference))
            {
                return reference;
            }

            string repoPath = Path.Combine(repoRoot, reference);
            if (File.Exists(repoPath))
            {
                return repoPath;
            }

            return reference;
        }

        private static void SetCoreField(string fieldName, object value)
        {
            FieldInfo field = typeof(Core).GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new MissingFieldException(typeof(Core).FullName, fieldName);
            }

            field.SetValue(null, value);
        }

        private static void ExportSnapshot(string repoRoot, ExportOptions options)
        {
            string outputPath = Path.GetFullPath(Path.Combine(repoRoot, options.OutputPath));
            string spawnersPath = Path.Combine(outputPath, "spawners.jsonl");
            string mobilesPath = Path.Combine(outputPath, "mobiles.jsonl");
            string itemsPath = Path.Combine(outputPath, "items.jsonl");

            ExportMobiles(mobilesPath);
            int itemCount = ExportItems(itemsPath);
            int spawnerCount = ExportSpawners(spawnersPath);

            string scanRelativePath = null;
            if (options.Scan != null)
            {
                scanRelativePath = ExportScan(outputPath, options);
            }

            WriteManifest(outputPath, spawnerCount, World.Mobiles.Count, itemCount, scanRelativePath, options);
        }

        private static void ExportMobiles(string path)
        {
            List<Mobile> mobiles = new List<Mobile>(World.Mobiles.Values);
            mobiles.Sort(delegate (Mobile left, Mobile right) { return left.Serial.Value.CompareTo(right.Serial.Value); });

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (Mobile mobile in mobiles)
                {
                    if (mobile == null || mobile.Deleted || mobile.Map == null || mobile.Map == Map.Internal)
                    {
                        continue;
                    }

                    JsonObject json = new JsonObject();
                    json.Add("serial", mobile.Serial.Value);
                    json.Add("type", mobile.GetType().FullName);
                    json.Add("name", mobile.Name);
                    json.Add("map", mobile.Map.Name);
                    json.Add("x", mobile.X);
                    json.Add("y", mobile.Y);
                    json.Add("z", mobile.Z);
                    json.Add("body", mobile.Body.BodyID);
                    json.Add("alive", mobile.Alive);
                    json.Add("player", mobile.Player);
                    json.Add("controlled", ReadBoolean(mobile, "Controlled"));
                    json.WriteTo(writer);
                    writer.WriteLine();
                }
            }
        }

        private static int ExportItems(string path)
        {
            List<Item> items = new List<Item>(World.Items.Values);
            items.Sort(delegate (Item left, Item right) { return left.Serial.Value.CompareTo(right.Serial.Value); });

            int count = 0;
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (Item item in items)
                {
                    if (!IsExportedWorldItem(item))
                    {
                        continue;
                    }

                    JsonObject json = ItemJson(item);
                    json.WriteTo(writer);
                    writer.WriteLine();
                    count++;
                }
            }

            return count;
        }

        private static int ExportSpawners(string path)
        {
            List<Item> items = new List<Item>(World.Items.Values);
            items.Sort(delegate (Item left, Item right) { return left.Serial.Value.CompareTo(right.Serial.Value); });

            int count = 0;
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (Item item in items)
                {
                    if (item == null || item.Deleted || item.Map == null || item.Map == Map.Internal)
                    {
                        continue;
                    }

                    if (!IsSpawner(item))
                    {
                        continue;
                    }

                    JsonObject json = ItemJson(item);
                    json.Add("running", ReadBoolean(item, "Running"));
                    json.Add("home_range", ReadInt(item, "HomeRange"));
                    json.Add("walking_range", ReadInt(item, "WalkingRange"));
                    json.Add("spawn_names", ReadStringList(item, "CreaturesName"));
                    json.Add("sub_spawner_a", ReadStringList(item, "SubSpawnerA"));
                    json.Add("sub_spawner_b", ReadStringList(item, "SubSpawnerB"));
                    json.Add("sub_spawner_c", ReadStringList(item, "SubSpawnerC"));
                    json.Add("sub_spawner_d", ReadStringList(item, "SubSpawnerD"));
                    json.Add("sub_spawner_e", ReadStringList(item, "SubSpawnerE"));
                    json.WriteTo(writer);
                    writer.WriteLine();
                    count++;
                }
            }

            return count;
        }

        private static bool IsExportedWorldItem(Item item)
        {
            if (item == null || item.Deleted || item.Map == null || item.Map == Map.Internal || item.Parent != null)
            {
                return false;
            }

            if (IsSpawner(item))
            {
                return true;
            }

            string typeName = item.GetType().FullName;
            if (typeName.IndexOf("Door", StringComparison.OrdinalIgnoreCase) >= 0
                || typeName.IndexOf("Sign", StringComparison.OrdinalIgnoreCase) >= 0
                || typeName.IndexOf("Board", StringComparison.OrdinalIgnoreCase) >= 0
                || typeName.IndexOf("Chest", StringComparison.OrdinalIgnoreCase) >= 0
                || typeName.IndexOf("Coffer", StringComparison.OrdinalIgnoreCase) >= 0
                || typeName.IndexOf("Trapdoor", StringComparison.OrdinalIgnoreCase) >= 0
                || typeName.IndexOf("Gate", StringComparison.OrdinalIgnoreCase) >= 0
                || typeName.IndexOf("Moongate", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            return item.Movable || item.Name != null;
        }

        private static bool IsSpawner(Item item)
        {
            string typeName = item.GetType().FullName;
            return String.Equals(typeName, "Server.Mobiles.PremiumSpawner", StringComparison.Ordinal)
                || typeName.IndexOf("XmlSpawner", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static JsonObject ItemJson(Item item)
        {
            JsonObject json = new JsonObject();
            json.Add("serial", item.Serial.Value);
            json.Add("type", item.GetType().FullName);
            json.Add("name", item.Name);
            json.Add("map", item.Map.Name);
            json.Add("x", item.X);
            json.Add("y", item.Y);
            json.Add("z", item.Z);
            json.Add("item_id", item.ItemID);
            json.Add("amount", item.Amount);
            json.Add("movable", item.Movable);
            return json;
        }

        private static string ExportScan(string outputPath, ExportOptions options)
        {
            ScanOptions scan = options.Scan;
            Map map = Map.Parse(scan.MapName);
            if (map == null)
            {
                throw new ArgumentException("Unknown scan map: " + scan.MapName);
            }

            string scansPath = Path.Combine(outputPath, "scans");
            Directory.CreateDirectory(scansPath);

            string fileName = String.Format(
                CultureInfo.InvariantCulture,
                "run-{0}-{1}-{2}.yaml",
                options.RunNumber,
                scan.X,
                scan.Y
            );
            string path = Path.Combine(scansPath, fileName);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine("scan:");
                writer.WriteLine("  run: {0}", options.RunNumber);
                writer.WriteLine("  center: {{ map: \"{0}\", x: {1}, y: {2}, z: {3} }}", map.Name, scan.X, scan.Y, scan.Z);
                writer.WriteLine("  range_tiles: {0}", scan.Range);
                writer.WriteLine("  range_shape: \"rectangle\"");
                writer.WriteLine("  range_width: {0}", (scan.Range * 2) + 1);
                writer.WriteLine("  range_height: {0}", (scan.Range * 2) + 1);
                writer.WriteLine("  mobiles:");

                IPooledEnumerable mobiles = map.GetMobilesInRange(new Point3D(scan.X, scan.Y, scan.Z), scan.Range);
                foreach (Mobile mobile in mobiles)
                {
                    writer.WriteLine("    - {{ serial: {0}, type: \"{1}\", name: \"{2}\", x: {3}, y: {4}, z: {5} }}",
                        mobile.Serial.Value,
                        EscapeYaml(mobile.GetType().FullName),
                        EscapeYaml(mobile.Name),
                        mobile.X,
                        mobile.Y,
                        mobile.Z);
                }
                mobiles.Free();

                writer.WriteLine("  items:");
                IPooledEnumerable items = map.GetItemsInRange(new Point3D(scan.X, scan.Y, scan.Z), scan.Range);
                foreach (Item item in items)
                {
                    if (!IsExportedWorldItem(item))
                    {
                        continue;
                    }

                    writer.WriteLine("    - {{ serial: {0}, type: \"{1}\", name: \"{2}\", x: {3}, y: {4}, z: {5} }}",
                        item.Serial.Value,
                        EscapeYaml(item.GetType().FullName),
                        EscapeYaml(item.Name),
                        item.X,
                        item.Y,
                        item.Z);
                }
                items.Free();
            }

            return "docs/CCWM/live-state/scans/" + fileName;
        }

        private static void WriteManifest(string outputPath, int spawnerCount, int mobileCount, int itemCount, string scanRelativePath, ExportOptions options)
        {
            string manifestPath = Path.Combine(outputPath, "manifest.yaml");
            using (StreamWriter writer = new StreamWriter(manifestPath, false))
            {
                writer.WriteLine("ccwm_live_state_snapshot:");
                writer.WriteLine("  generated_utc: \"{0:O}\"", DateTime.UtcNow);
                writer.WriteLine("  exporter: \"CCWM.Tools.LiveStateExporter.CCWMLiveStateExporter\"");
                writer.WriteLine("  source: \"root Saves/ loaded through World.Load()\"");
                writer.WriteLine("  saves_read_only: true");
                writer.WriteLine("  automation_policy: \"Reuse this committed snapshot until manually refreshed; do not re-export every run.\"");
                writer.WriteLine("  files:");
                writer.WriteLine("    spawners: \"docs/CCWM/live-state/spawners.jsonl\"");
                writer.WriteLine("    mobiles: \"docs/CCWM/live-state/mobiles.jsonl\"");
                writer.WriteLine("    items: \"docs/CCWM/live-state/items.jsonl\"");
                if (scanRelativePath != null)
                {
                    writer.WriteLine("    latest_scan: \"{0}\"", scanRelativePath);
                }
                writer.WriteLine("  counts:");
                writer.WriteLine("    spawners: {0}", spawnerCount);
                writer.WriteLine("    mobiles: {0}", mobileCount);
                writer.WriteLine("    interactable_world_items: {0}", itemCount);
                writer.WriteLine("    world_mobiles_loaded: {0}", World.Mobiles.Count);
                writer.WriteLine("    world_items_loaded: {0}", World.Items.Count);
                if (options.Scan != null)
                {
                    writer.WriteLine("  latest_scan:");
                    writer.WriteLine("    center: {{ map: \"{0}\", x: {1}, y: {2}, z: {3} }}", options.Scan.MapName, options.Scan.X, options.Scan.Y, options.Scan.Z);
                    writer.WriteLine("    range_tiles: {0}", options.Scan.Range);
                    writer.WriteLine("    range_shape: \"rectangle\"");
                    writer.WriteLine("    range_width: {0}", (options.Scan.Range * 2) + 1);
                    writer.WriteLine("    range_height: {0}", (options.Scan.Range * 2) + 1);
                    writer.WriteLine("    source_semantics: \"Map.GetMobilesInRange(Point3D) uses Rectangle2D(x-range,y-range,(range*2)+1,(range*2)+1).\"");
                }
                writer.WriteLine("  maps:");
                for (int index = 0; index < Map.AllMaps.Count; index++)
                {
                    Map map = Map.AllMaps[index];
                    if (map == null || map == Map.Internal)
                    {
                        continue;
                    }

                    writer.WriteLine("    - name: \"{0}\"", map.Name);
                    writer.WriteLine("      map_id: {0}", map.MapID);
                    writer.WriteLine("      map_index: {0}", map.MapIndex);
                    writer.WriteLine("      file_index: {0}", ReadPrivateInt(map, "m_FileIndex"));
                }
            }
        }

        private static bool? ReadBoolean(object source, string propertyName)
        {
            object value = ReadProperty(source, propertyName);
            if (value is bool)
            {
                return (bool)value;
            }

            return null;
        }

        private static int? ReadInt(object source, string propertyName)
        {
            object value = ReadProperty(source, propertyName);
            if (value is int)
            {
                return (int)value;
            }

            return null;
        }

        private static List<string> ReadStringList(object source, string propertyName)
        {
            object value = ReadProperty(source, propertyName);
            IEnumerable enumerable = value as IEnumerable;
            List<string> result = new List<string>();

            if (enumerable == null || value is string)
            {
                return result;
            }

            foreach (object entry in enumerable)
            {
                if (entry != null)
                {
                    result.Add(entry.ToString());
                }
            }

            result.Sort(StringComparer.Ordinal);
            return result;
        }

        private static object ReadProperty(object source, string propertyName)
        {
            PropertyInfo property = source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property == null || !property.CanRead)
            {
                return null;
            }

            try
            {
                return property.GetValue(source, null);
            }
            catch
            {
                return null;
            }
        }

        private static int ReadPrivateInt(object source, string fieldName)
        {
            FieldInfo field = source.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                return 0;
            }

            object value = field.GetValue(source);
            if (value is int)
            {
                return (int)value;
            }

            return 0;
        }

        private static string EscapeYaml(string value)
        {
            if (value == null)
            {
                return "";
            }

            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }

    internal sealed class ExportOptions
    {
        public string SavesPath;
        public string OutputPath;
        public int RunNumber;
        public ScanOptions Scan;

        public static ExportOptions Parse(string[] args)
        {
            ExportOptions options = new ExportOptions();
            options.SavesPath = "Saves";
            options.OutputPath = Path.Combine("docs", "CCWM", "live-state");

            for (int index = 0; index < args.Length; index++)
            {
                string arg = args[index];
                if (arg == "--saves")
                {
                    options.SavesPath = RequireValue(args, ref index, arg);
                }
                else if (arg == "--out")
                {
                    options.OutputPath = RequireValue(args, ref index, arg);
                }
                else if (arg == "--run")
                {
                    options.RunNumber = Int32.Parse(RequireValue(args, ref index, arg), CultureInfo.InvariantCulture);
                }
                else if (arg == "--scan")
                {
                    options.Scan = ScanOptions.Parse(RequireValue(args, ref index, arg));
                }
                else if (arg == "--help" || arg == "-h")
                {
                    throw new ArgumentException("Help requested.");
                }
                else
                {
                    throw new ArgumentException("Unknown argument: " + arg);
                }
            }

            if (options.RunNumber <= 0)
            {
                throw new ArgumentException("--run must be supplied with a positive run number.");
            }

            return options;
        }

        private static string RequireValue(string[] args, ref int index, string name)
        {
            if (index + 1 >= args.Length)
            {
                throw new ArgumentException(name + " requires a value.");
            }

            index++;
            return args[index];
        }
    }

    internal sealed class ScanOptions
    {
        public string MapName;
        public int X;
        public int Y;
        public int Z;
        public int Range;

        public static ScanOptions Parse(string value)
        {
            string[] parts = value.Split(',');
            if (parts.Length != 5)
            {
                throw new ArgumentException("--scan must be Map,x,y,z,range.");
            }

            ScanOptions options = new ScanOptions();
            options.MapName = parts[0];
            options.X = Int32.Parse(parts[1], CultureInfo.InvariantCulture);
            options.Y = Int32.Parse(parts[2], CultureInfo.InvariantCulture);
            options.Z = Int32.Parse(parts[3], CultureInfo.InvariantCulture);
            options.Range = Int32.Parse(parts[4], CultureInfo.InvariantCulture);

            if (options.Range < 0)
            {
                throw new ArgumentException("--scan range must be non-negative.");
            }

            return options;
        }
    }

    internal sealed class JsonObject
    {
        private readonly List<KeyValuePair<string, object>> _values = new List<KeyValuePair<string, object>>();

        public void Add(string key, object value)
        {
            _values.Add(new KeyValuePair<string, object>(key, value));
        }

        public void WriteTo(TextWriter writer)
        {
            writer.Write("{");

            for (int index = 0; index < _values.Count; index++)
            {
                if (index > 0)
                {
                    writer.Write(",");
                }

                writer.Write("\"");
                writer.Write(Escape(_values[index].Key));
                writer.Write("\":");
                WriteValue(writer, _values[index].Value);
            }

            writer.Write("}");
        }

        private static void WriteValue(TextWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write("null");
            }
            else if (value is string)
            {
                writer.Write("\"");
                writer.Write(Escape((string)value));
                writer.Write("\"");
            }
            else if (value is bool)
            {
                writer.Write(((bool)value) ? "true" : "false");
            }
            else if (value is IEnumerable<string>)
            {
                writer.Write("[");
                bool first = true;
                foreach (string entry in (IEnumerable<string>)value)
                {
                    if (!first)
                    {
                        writer.Write(",");
                    }

                    first = false;
                    writer.Write("\"");
                    writer.Write(Escape(entry));
                    writer.Write("\"");
                }
                writer.Write("]");
            }
            else if (value is IFormattable)
            {
                writer.Write(((IFormattable)value).ToString(null, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.Write("\"");
                writer.Write(Escape(value.ToString()));
                writer.Write("\"");
            }
        }

        private static string Escape(string value)
        {
            if (value == null)
            {
                return "";
            }

            return value
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }
    }
}
