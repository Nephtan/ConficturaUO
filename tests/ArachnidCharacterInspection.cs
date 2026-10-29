using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    public static class ArachnidCharacterInspection
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "../evidence"));

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard = Path.Combine(Evidence, "native-private-root.txt");
            if (!File.Exists(guard) || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)
                || ArachnidOutbreakController.DeliveryReady)
                throw new InvalidOperationException("Inspection requires the guarded disabled-production copy.");
            AutoSave.SavesEnabled = false;
            Timer.DelayCall(TimeSpan.FromSeconds(1), Inspect);
        }

        private static void Attr(XmlWriter writer, string key, object value)
        {
            writer.WriteAttributeString(key, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
        }
        private static void Inspect()
        {
            try
            {
                List<PlayerMobile> found = new List<PlayerMobile>();
                foreach (Mobile mobile in World.Mobiles.Values)
                    if (mobile is PlayerMobile && String.Equals(mobile.Name, "Zanthus", StringComparison.OrdinalIgnoreCase))
                        found.Add((PlayerMobile)mobile);
                using (XmlWriter writer = XmlWriter.Create(Path.Combine(Evidence, "character-inspection.private.xml"), new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartElement("inspection"); Attr(writer, "target", "Zanthus"); Attr(writer, "matches", found.Count);
                    Attr(writer, "scope", "Matching-serializer offline copied-world observation; no character mutations or gameplay test.");
                    foreach (PlayerMobile player in found)
                    {
                        writer.WriteStartElement("character"); Attr(writer, "serial", player.Serial.Value); Attr(writer, "access", player.AccessLevel);
                        Attr(writer, "alive", player.Alive); Attr(writer, "blessed", player.Blessed); Attr(writer, "race", player.RaceID);
                        Attr(writer, "rawStr", player.RawStr); Attr(writer, "rawDex", player.RawDex); Attr(writer, "rawInt", player.RawInt);
                        Attr(writer, "str", player.Str); Attr(writer, "dex", player.Dex); Attr(writer, "int", player.Int);
                        Attr(writer, "hitsMax", player.HitsMax); Attr(writer, "stamMax", player.StamMax); Attr(writer, "manaMax", player.ManaMax);
                        Attr(writer, "skillCap", player.Skills.Cap); Attr(writer, "baseSkillTotal", player.Skills.Total);
                        Attr(writer, "skillOrigin", player.SkillStart); Attr(writer, "skillBoost", player.SkillBoost); Attr(writer, "skillEther", player.SkillEther);
                        for (int i = 0; i < player.Skills.Length; ++i)
                            if (player.Skills[i].Base > 0 || player.Skills[i].Value > 0)
                            {
                                Skill skill = player.Skills[i]; writer.WriteStartElement("skill"); Attr(writer, "name", skill.SkillName);
                                Attr(writer, "base", skill.Base); Attr(writer, "effective", skill.Value); Attr(writer, "cap", skill.Cap); writer.WriteEndElement();
                            }
                        foreach (Item item in player.Items)
                        {
                            if (item.Layer == Layer.Backpack || item.Layer == Layer.Bank) continue;
                            writer.WriteStartElement("equipment"); Attr(writer, "type", item.GetType().FullName); Attr(writer, "name", item.Name); Attr(writer, "layer", item.Layer);
                            foreach (string group in new string[] { "Attributes", "ArmorAttributes", "WeaponAttributes" })
                            {
                                PropertyInfo property = item.GetType().GetProperty(group);
                                object attributes = property == null ? null : property.GetValue(item, null);
                                if (attributes == null) continue;
                                foreach (PropertyInfo attribute in attributes.GetType().GetProperties())
                                    if (attribute.PropertyType == typeof(int) && attribute.GetIndexParameters().Length == 0)
                                    {
                                        int value = (int)attribute.GetValue(attributes, null);
                                        if (value != 0) { writer.WriteStartElement("attribute"); Attr(writer, "group", group); Attr(writer, "name", attribute.Name); Attr(writer, "value", value); writer.WriteEndElement(); }
                                    }
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
            }
            catch (Exception error) { File.WriteAllText(Path.Combine(Evidence, "character-inspection-error.private.txt"), error.ToString()); }
            AutoSave.SavesEnabled = false;
            File.WriteAllText(Path.Combine(Evidence, "native-stopped.txt"), "Offline inspection finished without saving; no client or character mutations.");
        }
    }
}
