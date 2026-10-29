using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Spells.Research;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    // No encounter: observe the original macro's native preparation on the actual copied character.
    public static class ArachnidArchmagePreparation
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "../evidence"));
        private static PlayerMobile Actor;
        private static Timer Poll;
        private static DateTime Deadline = DateTime.UtcNow.AddMinutes(5);
        private static string State = "preparing", Failure = String.Empty;

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard = Path.Combine(Evidence, "native-private-root.txt");
            if (!File.Exists(guard) || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)
                || ArachnidOutbreakController.DeliveryReady)
                throw new InvalidOperationException("Archmage preparation requires the disabled-production disposable root.");
            AutoSave.SavesEnabled = false;
            Timer.DelayCall(TimeSpan.FromSeconds(1), Prepare);
        }

        private static void Prepare()
        {
            try
            {
                foreach (Mobile mobile in World.Mobiles.Values)
                    if (mobile is PlayerMobile && String.Equals(mobile.Name, "Zanthus", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Actor != null) throw new Exception("Ambiguous copied character.");
                        Actor = (PlayerMobile)mobile;
                    }
                if (Actor == null || !Actor.Alive || Actor.Blessed || Actor.AccessLevel != AccessLevel.Player)
                    throw new Exception("Requires the actual living, unblessed Player Zanthus; no substitute or privilege normalization.");
                Manifest("archmage-original.private.xml");
                IAccount login = Accounts.GetAccount(File.ReadAllText(Path.Combine(Evidence, "account-name.private")));
                if (login == null) throw new Exception("Private login account unavailable.");
                IAccount original = Actor.Account;
                if (original != null)
                    for (int i = 0; i < original.Length; ++i)
                        if (original[i] == Actor) { original[i] = null; break; }
                login[0] = Actor;
                // Account routing and initial staging affect this unsaved process only.
                Actor.Combatant = null; Actor.Warmode = false;
                Actor.MoveToWorld(new Point3D(2490, 2060, Map.Sosaria.GetAverageZ(2490, 2060)), Map.Sosaria);
                // Spell.Cast explicitly rejects blessed players. Keep actual
                // Player vulnerability during native maintenance.
                State = "ready";
            }
            catch (Exception error) { Failure = error.ToString(); State = "failed"; }
            Poll = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromMilliseconds(250), Tick);
        }

        private static void Tick()
        {
            try
            {
                if (File.Exists(Path.Combine(Evidence, "archmage-client-error.txt"))) State = "client-failed";
                if (State == "ready" && File.Exists(Path.Combine(Evidence, "archmage-macro-prepared.txt")))
                { Manifest("archmage-prepared.private.xml"); State = "prepared"; }
                if (DateTime.UtcNow > Deadline && State == "ready")
                { Failure = "Native preparation deadline."; State = "failed"; }
                Status();
                if (File.Exists(Path.Combine(Evidence, "native-stop.txt")) || DateTime.UtcNow > Deadline)
                {
                    Poll.Stop(); AutoSave.SavesEnabled = false;
                    File.WriteAllText(Path.Combine(Evidence, "native-stopped.txt"), "Preparation fixture stopped without saving; no encounter.");
                }
            }
            catch (Exception error) { Failure = error.ToString(); State = "failed"; Status(); }
        }

        private static void Attr(XmlWriter writer, string name, object value)
        { writer.WriteAttributeString(name, Convert.ToString(value, CultureInfo.InvariantCulture)); }

        private static void Manifest(string name)
        {
            using (XmlWriter writer = XmlWriter.Create(Path.Combine(Evidence, name), new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("character"); Attr(writer, "serial", Actor.Serial.Value);
                Attr(writer, "access", Actor.AccessLevel); Attr(writer, "blessed", Actor.Blessed);
                Attr(writer, "rawStr", Actor.RawStr); Attr(writer, "rawDex", Actor.RawDex); Attr(writer, "rawInt", Actor.RawInt);
                Attr(writer, "str", Actor.Str); Attr(writer, "dex", Actor.Dex); Attr(writer, "int", Actor.Int);
                Attr(writer, "hits", Actor.Hits); Attr(writer, "hitsMax", Actor.HitsMax);
                Attr(writer, "stam", Actor.Stam); Attr(writer, "stamMax", Actor.StamMax);
                Attr(writer, "mana", Actor.Mana); Attr(writer, "manaMax", Actor.ManaMax);
                Attr(writer, "body", Actor.BodyValue); Attr(writer, "followers", Actor.Followers);
                Attr(writer, "ancientDamage", ResearchSpell.DamagingSkill(Actor)); Attr(writer, "ancientCasting", ResearchSpell.CastingSkill(Actor));
                Attr(writer, "x", Actor.X); Attr(writer, "y", Actor.Y); Attr(writer, "z", Actor.Z);
                for (int i = 0; i < Actor.Skills.Length; ++i)
                {
                    Skill skill = Actor.Skills[i];
                    if (skill.Base == 0 && skill.Value == 0) continue;
                    writer.WriteStartElement("skill"); Attr(writer, "name", skill.SkillName);
                    Attr(writer, "base", skill.Base); Attr(writer, "effective", skill.Value); Attr(writer, "cap", skill.Cap); writer.WriteEndElement();
                }
                foreach (Item item in Actor.Items)
                {
                    if (item.Layer == Layer.Backpack || item.Layer == Layer.Bank) continue;
                    writer.WriteStartElement("equipment"); Attr(writer, "serial", item.Serial.Value);
                    Attr(writer, "type", item.GetType().FullName); Attr(writer, "layer", item.Layer); writer.WriteEndElement();
                }
                if (Actor.Backpack != null) Inventory(writer, Actor.Backpack);
                writer.WriteEndElement();
            }
        }

        private static void Inventory(XmlWriter writer, Container container)
        {
            foreach (Item item in container.Items)
            {
                writer.WriteStartElement("item"); Attr(writer, "serial", item.Serial.Value); Attr(writer, "type", item.GetType().FullName);
                Attr(writer, "amount", item.Amount); Attr(writer, "graphic", item.ItemID); Attr(writer, "hue", item.Hue);
                Container child = item as Container;
                if (child != null) Inventory(writer, child);
                writer.WriteEndElement();
            }
        }

        private static void Status()
        {
            string path = Path.Combine(Evidence, "archmage-state.xml");
            using (XmlWriter writer = XmlWriter.Create(path + ".tmp", new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("archmage"); Attr(writer, "state", State); Attr(writer, "failure", Failure);
                Attr(writer, "online", Actor != null && Actor.NetState != null); Attr(writer, "encounter", false);
                writer.WriteEndElement();
            }
            if (File.Exists(path)) File.Replace(path + ".tmp", path, null); else File.Move(path + ".tmp", path);
        }
    }
}
