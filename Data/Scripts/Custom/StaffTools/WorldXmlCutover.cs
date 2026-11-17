using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Server.Commands;
using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Mobiles;
using Server.Multis;

namespace Server.Custom.Confictura
{
    public static class WorldXmlCutover
    {
        private static readonly string Manifest = Path.Combine("Data", "System", "CFG", "WorldXmlCutover.tsv");
        private static readonly string LogDirectory = Path.Combine("Data", "Logs", "WorldXmlCutover");
        private static Serial m_PreviewOwner;
        private static string m_PreviewCode;

        private sealed class Definition
        {
            public string Guid;
            public string Facet;
            public Point3D Point;
            public bool Running;
            public string Recipe;
            public string[] Types;
        }

        private sealed class Plan
        {
            public readonly List<XmlSpawner> Sources = new List<XmlSpawner>();
            public readonly Dictionary<IEntity, string> Children = new Dictionary<IEntity, string>();
            public readonly Dictionary<IEntity, string> Nested = new Dictionary<IEntity, string>();
            public readonly List<XmlAttachment> Attachments = new List<XmlAttachment>();
            public readonly List<XmlSpawner> Retained = new List<XmlSpawner>();
            public readonly List<string> Rows = new List<string>();
        }

        public static void Initialize()
        {
            CommandSystem.Register("WorldXmlCutover", AccessLevel.Owner, OnCommand);
        }

        [Usage("WorldXmlCutover preview | apply <preview code>")]
        [Description("Previews or retires the reviewed public legacy XML spawners for the world rebuild.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null || from.Deleted || from.AccessLevel < AccessLevel.Owner)
                return;

            bool preview = e.Length == 1 && e.GetString(0).Equals("preview", StringComparison.OrdinalIgnoreCase);
            bool apply = e.Length == 2 && e.GetString(0).Equals("apply", StringComparison.OrdinalIgnoreCase);
            if (!preview && !apply)
            {
                from.SendMessage("Use [WorldXmlCutover preview or [WorldXmlCutover apply <preview code>.");
                return;
            }

            bool deleting = false;
            try
            {
                Require(!World.Loading && !World.Saving, "Wait for world loading or saving to finish.");
                if (preview)
                    m_PreviewCode = null;

                Plan plan = Inspect();
                if (plan.Sources.Count == 0)
                {
                    m_PreviewCode = null;
                    from.SendMessage("World XML cutover: all reviewed legacy sources are absent. {0} other XML controllers present.", plan.Retained.Count);
                    return;
                }

                plan.Rows.Sort(StringComparer.Ordinal);
                string code = Hash(String.Join("\n", plan.Rows));
                if (apply)
                {
                    Require(m_PreviewOwner == from.Serial && m_PreviewCode == code
                        && code.Equals(e.GetString(1), StringComparison.OrdinalIgnoreCase),
                        "The preview code or world state changed. Run preview again and review the new receipt.");
                }

                string receipt = WriteReceipt(plan, code, from, apply ? "apply" : "preview");
                if (preview)
                {
                    m_PreviewOwner = from.Serial;
                    m_PreviewCode = code;
                    from.SendMessage("XML preview: {0} sources, {1} direct children, {2} nested children, {3} attachments, {4} other XML controllers.",
                        plan.Sources.Count, plan.Children.Count, plan.Nested.Count, plan.Attachments.Count, plan.Retained.Count);
                    from.SendMessage("Private receipt: {0}", receipt);
                    from.SendMessage("Preview code: {0}", code);
                    return;
                }

                m_PreviewCode = null;
                deleting = true;
                foreach (XmlSpawner source in plan.Sources)
                    source.Delete();

                Require(plan.Sources.TrueForAll(source => source.Deleted), "A reviewed source survived retirement.");
                foreach (IEntity child in plan.Children.Keys)
                    Require(IsDeleted(child), "A direct child survived retirement: " + child.Serial);
                foreach (IEntity child in plan.Nested.Keys)
                    Require(IsDeleted(child), "A nested child survived retirement: " + child.Serial);
                // XML attachments can outlive a deleted mobile until the next load.
                foreach (XmlAttachment attachment in plan.Attachments)
                    if (!attachment.Deleted)
                        attachment.Delete();
                Require(plan.Attachments.TrueForAll(attachment => attachment.Deleted), "A reviewed attachment survived retirement.");
                Require(plan.Retained.TrueForAll(source => !source.Deleted), "An unrelated XML source disappeared.");

                File.AppendAllText(receipt, "verified\n");
                from.SendMessage("XML cutover verified: {0} sources retired and {1} other XML controllers preserved. Receipt: {2}",
                    plan.Sources.Count, plan.Retained.Count, receipt);
            }
            catch (Exception ex)
            {
                string result = deleting ? "Cutover interrupted after deletion began. Keep the shard closed and restore the checkpoint."
                    : "Cutover stopped before deletion.";
                Console.WriteLine("WorldXmlCutover: {0} {1}", result, ex);
                from.SendMessage("{0} {1}", result, ex.Message);
            }
        }

        private static Plan Inspect()
        {
            Dictionary<string, Definition> definitions = ReadManifest();
            Dictionary<string, XmlSpawner> found = new Dictionary<string, XmlSpawner>(StringComparer.OrdinalIgnoreCase);
            Plan plan = new Plan();
            foreach (Item item in World.Items.Values)
            {
                XmlSpawner source = item as XmlSpawner;
                if (source == null || source.Deleted)
                    continue;

                if (source.UniqueId != null && definitions.ContainsKey(source.UniqueId))
                {
                    Require(!found.ContainsKey(source.UniqueId), "Duplicate legacy GUID: " + source.UniqueId);
                    found.Add(source.UniqueId, source);
                }
                else
                {
                    plan.Retained.Add(source);
                    plan.Rows.Add(Row("retained", source.UniqueId, source.Serial.Value, source.Map, source.Location,
                        source.Parent is IEntity ? ((IEntity)source.Parent).Serial.Value : 0, source.Running,
                        Hash(source.GetSerializedObjectList2())));
                }
            }

            if (found.Count == 0)
                return plan;

            Require(found.Count == definitions.Count, "Only " + found.Count + " of " + definitions.Count + " legacy sources are present. Reconcile the partial cohort.");
            foreach (KeyValuePair<string, XmlSpawner> entry in found)
                InspectSource(entry.Value, definitions[entry.Key], plan);

            foreach (IEntity child in plan.Nested.Keys)
                Require(!plan.Children.ContainsKey(child), "A child appears in both ownership levels: " + child.Serial);

            foreach (XmlSpawner source in plan.Retained)
                foreach (XmlSpawner.SpawnObject slot in source.SpawnObjects)
                    foreach (object value in slot.SpawnedObjects)
                    {
                        IEntity child = value as IEntity;
                        Require(child == null || (!plan.Children.ContainsKey(child) && !plan.Nested.ContainsKey(child)
                            && !(child is XmlSpawner && plan.Sources.Contains((XmlSpawner)child))),
                            "A retained XML controller tracks a cutover object: " + source.UniqueId);
                    }

            return plan;
        }

        private static void InspectSource(XmlSpawner source, Definition expected, Plan plan)
        {
            string guid = expected.Guid;
            Require(source.GetType() == typeof(XmlSpawner) && source.Map != null && source.Map != Map.Internal
                && source.Map.Name == expected.Facet && source.Location == expected.Point && source.Running == expected.Running
                && IsPublicRoot(source) && AttachmentCount(source) == 0 && !source.ShowBounds
                && source.Spawner == null
                && (source.m_TextEntryBook == null || source.m_TextEntryBook.Count == 0)
                && (source.m_KeywordTagList == null || source.m_KeywordTagList.Count == 0)
                && Hash(source.GetSerializedObjectList2()) == expected.Recipe, "Legacy source differs from the manifest: " + guid);

            plan.Sources.Add(source);
            plan.Rows.Add(Row("source", guid, source.Serial.Value, source.Map, source.Location, source.Running, expected.Recipe,
                String.Join("|", expected.Types)));
            XmlSpawner.SpawnObject[] slots = source.SpawnObjects;
            for (int index = 0; index < slots.Length; index++)
            {
                XmlSpawner.SpawnObject slot = slots[index];
                if (slot.SpawnedObjects.Count == 0)
                    continue;

                // The verified recipe and the manifest must both authorize the runtime type.
                Type recipeType = SpawnerType.GetType(BaseXmlSpawner.ParseObjectType(slot.TypeName.TrimStart('*')));
                foreach (object value in slot.SpawnedObjects)
                {
                    IEntity child = value as IEntity;
                    string at = guid + " (slot " + index + ")";
                    Require(child != null, "Invalid tracked entry at " + at + ": " + (value == null ? "null" : value.GetType().Name));
                    Type actualType = child.GetType();
                    string identity = actualType.Name + " " + child.Serial;
                    Require(actualType == recipeType && Array.IndexOf(expected.Types, actualType.FullName) >= 0,
                        "Unapproved child at " + at + ": " + identity + ". Recipe type: " + (recipeType == null ? "unresolved" : recipeType.Name) + ".");
                    Require(!IsDeleted(child) && child.Map == source.Map && !plan.Children.ContainsKey(child),
                        "Child is deleted, on another facet, or tracked twice at " + at + ": " + identity + ".");

                    Item item = child as Item;
                    if (item != null)
                        InspectItem(item, source, plan);
                    else
                    {
                        BaseCreature creature = child as BaseCreature;
                        Require(creature != null && creature.Spawner == source && IsPublicMobile(creature),
                            "Protected mobile child at " + guid + ": " + child.Serial);
                        InspectDialogue(creature, guid, plan);
                    }

                    plan.Children.Add(child, guid);
                    RecordEntity(plan, "child", guid, child, index);
                }
            }
        }

        private static void InspectItem(Item item, XmlSpawner source, Plan plan)
        {
            string guid = source.UniqueId;
            Require(IsPublicRoot(item) && AttachmentCount(item) == 0 && item.Spawner == source,
                "Protected item child at " + guid + ": " + item.Serial);
            if (item is DDJewels)
                Require(item.Amount == 1000 && item.ItemID == 0xEF0 && item.Hue == 0 && item.Name == "jewels",
                    "The Atlantis treasure stack differs from the replacement recipe at " + guid + ": " + item.Serial);
            if (item is HoardPiles)
                Require(item.Hue == 583 && item.Name == "Atlantian Treasure",
                    "The Atlantis hoard differs from the replacement recipe at " + guid + ": " + item.Serial);

            MonsterNest nest = item as MonsterNest;
            if (nest != null)
            {
                Require(nest.Hits == nest.HitsMax, "A nest is damaged at " + guid + ": " + nest.Serial);
                InspectNested(nest, nest.m_Entity, guid, plan);
                if (nest.m_Spawn != null)
                    foreach (Mobile defender in nest.m_Spawn)
                        InspectNested(nest, defender, guid, plan);
            }

            BaseAddon addon = item as BaseAddon;
            if (addon != null)
                foreach (AddonComponent component in addon.Components)
                {
                    Require(component != null && component.Addon == addon, "A component has an unexpected owner at " + guid);
                    InspectNested(addon, component, guid, plan);
                }
        }

        private static void InspectNested(Item owner, IEntity child, string guid, Plan plan)
        {
            Require(child != null && !IsDeleted(child) && child.Map == owner.Map && !plan.Nested.ContainsKey(child)
                && AttachmentCount(child) == 0, "Unexpected nested child at " + guid + ": " + owner.Serial);
            BaseCreature creature = child as BaseCreature;
            Item item = child as Item;
            Require((creature != null && IsPublicMobile(creature) && creature.Spawner == null)
                || (item != null && IsPublicRoot(item) && item.Spawner == null),
                "Protected nested child at " + guid + ": " + child.Serial);
            plan.Nested.Add(child, guid);
            RecordEntity(plan, "nested", guid, child, owner.Serial.Value);
        }

        private static void InspectDialogue(BaseCreature creature, string guid, Plan plan)
        {
            ArrayList attachments = XmlAttach.FindAttachments(creature);
            if (attachments == null)
                return;
            foreach (XmlAttachment attachment in attachments)
            {
                if (attachment.Deleted)
                    continue;
                XmlDialog dialog = attachment as XmlDialog;
                Require(dialog != null && creature is XmlQuestNPC && attachment.AttachedTo == creature
                    && (attachment.OwnedBy == null || attachment.OwnedBy == creature) && !plan.Attachments.Contains(attachment),
                    "Unexpected mobile attachment at " + guid + ": " + creature.Serial);
                plan.Attachments.Add(attachment);
                List<string> entries = new List<string>();
                if (dialog.SpeechEntries != null)
                    foreach (XmlDialog.SpeechEntry entry in dialog.SpeechEntries)
                        entries.Add(Row(entry.EntryNumber, entry.ID, Encode(entry.Text), Encode(entry.Keywords), Encode(entry.Action),
                            Encode(entry.Condition), Encode(entry.DependsOn), Encode(entry.Gump), entry.Pause, entry.PrePause,
                            entry.LockConversation, entry.AllowNPCTrigger, entry.SpeechStyle, entry.SpeechHue, entry.IgnoreCarried));
                plan.Rows.Add(Row("dialogue", guid, creature.Serial.Value, attachment.Serial, Encode(dialog.Name), Hash(String.Join("\n", entries))));
            }
        }

        private static bool IsPublicRoot(Item item)
        {
            return item.Parent == null && !item.IsLockedDown && !item.IsSecure && item.Items.Count == 0
                && item.Weight != -3 && !ItemFlags.GetTaken(item) && BaseHouse.FindHouseAt(item) == null;
        }

        private static bool IsPublicMobile(BaseCreature creature)
        {
            return !creature.Player && creature.Alive && !creature.Controlled && !creature.IsStabled
                && !creature.Summoned && creature.ControlMaster == null && creature.SummonMaster == null
                && (creature.Owners == null || creature.Owners.Count == 0)
                && creature.Combatant == null && BaseHouse.FindHouseAt(creature) == null;
        }

        private static bool IsDeleted(IEntity entity)
        {
            Item item = entity as Item;
            Mobile mobile = entity as Mobile;
            return item != null ? item.Deleted : mobile == null || mobile.Deleted;
        }

        private static int AttachmentCount(object entity)
        {
            ArrayList attachments = XmlAttach.FindAttachments(entity);
            return attachments == null ? 0 : attachments.Count;
        }

        private static void RecordEntity(Plan plan, string kind, string guid, IEntity child, int owner)
        {
            Item item = child as Item;
            Mobile mobile = child as Mobile;
            MonsterNest nest = child as MonsterNest;
            string state = item != null ? Row(item.Amount, item.ItemID, item.Hue, item.Weight, Encode(item.Name), item.Light,
                item.Movable, item.Visible, nest == null ? 0 : nest.Hits)
                : Row(mobile.Hits, mobile.HitsMax, mobile.Body, mobile.Hue, Encode(mobile.Name));
            // Idle creatures can move between staff commands. Inspect their current facet, house and combat status again on apply.
            object position = item != null ? (object)item.Location : child.Map;
            plan.Rows.Add(Row(kind, guid, child.Serial.Value, child.GetType().FullName, position, owner, state));
        }

        private static Dictionary<string, Definition> ReadManifest()
        {
            Dictionary<string, Definition> definitions = new Dictionary<string, Definition>(StringComparer.OrdinalIgnoreCase);
            foreach (string line in File.ReadAllLines(Manifest))
            {
                if (line.Length == 0 || line[0] == '#')
                    continue;
                string[] row = line.Split('\t');
                Guid id;
                Require(row.Length == 8 && Guid.TryParseExact(row[0], "D", out id) && row[6].Length == 64,
                    "Invalid XML cutover manifest row.");
                Definition definition = new Definition();
                definition.Guid = row[0];
                definition.Facet = row[1];
                definition.Point = new Point3D(Int32.Parse(row[2], CultureInfo.InvariantCulture),
                    Int32.Parse(row[3], CultureInfo.InvariantCulture), Int32.Parse(row[4], CultureInfo.InvariantCulture));
                definition.Running = Boolean.Parse(row[5]);
                definition.Recipe = row[6];
                definition.Types = row[7] == "-" ? new string[0] : row[7].Split('|');
                definitions.Add(definition.Guid, definition);
            }
            Require(definitions.Count == 199, "Expected the 199 reviewed public legacy definitions.");
            return definitions;
        }

        private static string WriteReceipt(Plan plan, string code, Mobile owner, string action)
        {
            Directory.CreateDirectory(LogDirectory);
            string path = Path.Combine(LogDirectory, DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffffffZ", CultureInfo.InvariantCulture)
                + "-" + action + ".tsv");
            using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.CreateNew, FileAccess.Write)))
            {
                writer.WriteLine(Row(action, code, owner.Serial.Value, plan.Sources.Count, plan.Children.Count, plan.Nested.Count,
                    plan.Attachments.Count, plan.Retained.Count));
                foreach (string row in plan.Rows)
                    writer.WriteLine(row);
            }
            return path;
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }

        private static string Row(params object[] fields)
        {
            return String.Join("\t", Array.ConvertAll(fields, field => Convert.ToString(field, CultureInfo.InvariantCulture)));
        }

        private static string Encode(string value)
        {
            return value == null ? "-" : Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        private static string Hash(string value)
        {
            using (SHA256 hash = SHA256.Create())
                return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", "");
        }
    }
}
