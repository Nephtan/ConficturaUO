using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
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
    // Preparation or bounded stationary combat on the actual unsaved copied character.
    public static class ArachnidArchmagePreparation
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "../evidence"));
        private static PlayerMobile Actor;
        private static Timer Poll;
        private static DateTime Deadline = DateTime.UtcNow.AddMinutes(5);
        private static string State = "preparing", Failure = String.Empty;
        private static string Mode = "preparation";
        private static ArachnidOutbreakController Controller;
        private static ArachnidOutbreakSite Site;
        private static ArachnidOutbreakPlacement Placement;
        private static DateTime Started, Ended, LastOwnedDamage;
        private static int EnemyDeaths, PlayerDeaths, Incoming, Restored, Outgoing, MovementRequests;
        private static bool BossKilled;
        private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly Dictionary<int, bool> OriginalGroundGold = new Dictionary<int, bool>();

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard = Path.Combine(Evidence, "native-private-root.txt");
            if (!File.Exists(guard) || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)
                || ArachnidOutbreakController.DeliveryReady)
                throw new InvalidOperationException("Archmage preparation requires the disabled-production disposable root.");
            AutoSave.SavesEnabled = false;
            string mode = Path.Combine(Evidence, "archmage-mode.txt");
            if (File.Exists(mode)) Mode = File.ReadAllText(mode).Trim();
            if (Mode != "preparation" && Mode != "stationary" && Mode != "route") throw new Exception("Unsupported archmage case.");
            EventSink.HealthChanged += OnHealth;
            EventSink.MobileDeath += OnDeath;
            EventSink.Movement += OnMovement;
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
                Point3D staging = new Point3D(2490, 2060, Map.Sosaria.GetAverageZ(2490, 2060));
                if (Mode != "preparation")
                {
                    List<WildernessEncounterReserve> reserves = new List<WildernessEncounterReserve>();
                    foreach (WildernessEncounterReserve reserve in WildernessEncounterReserves.Proposals)
                        reserves.Add(new WildernessEncounterReserve(reserve.Id, reserve.Center.X, reserve.Center.Y, reserve.Id == "central-woodland"));
                    typeof(WildernessEncounterReserves).GetField("m_Proposals", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, reserves.AsReadOnly());
                    Site = new ArachnidOutbreakSite("central-woodland", 2514, 2088, true);
                    typeof(ArachnidOutbreakSites).GetField("m_Candidates", BindingFlags.Static | BindingFlags.NonPublic)
                        .SetValue(null, Array.AsReadOnly(new ArachnidOutbreakSite[] { Site }));
                    string reason;
                    if (!Site.Validate(new SosariaOutbreakTerrain(), out Placement, out reason)) throw new Exception(reason);
                    staging = Placement.Brood(0); staging.Y -= 2;
                    staging.Z = Map.Sosaria.GetAverageZ(staging.X, staging.Y);
                }
                if (!Map.Sosaria.CanSpawnMobile(staging)) throw new Exception("Blocked initial staging.");
                Actor.MoveToWorld(staging, Map.Sosaria);
                // Spell.Cast explicitly rejects blessed players. Keep actual
                // Player vulnerability during native maintenance.
                State = "ready";
            }
            catch (Exception error) { Failure = error.ToString(); State = "failed"; }
            Poll = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromMilliseconds(250), Tick);
        }

        private static bool Running { get { return State.StartsWith("running-"); } }
        private static bool Owns(Mobile mobile)
        {
            IArachnidOutbreakOwned owned = mobile as IArachnidOutbreakOwned;
            return owned != null && owned.Controller == Controller && Controller != null;
        }
        private static void OnHealth(HealthChangedEventArgs e)
        {
            if (!Running) return;
            int change = e.NewHits - e.OldHits;
            if (e.Mobile == Actor) { if (change < 0) Incoming -= change; else Restored += change; }
            else if (change < 0 && Owns(e.Mobile)) { Outgoing -= change; LastOwnedDamage = DateTime.UtcNow; }
        }
        private static void OnDeath(Mobile mobile)
        {
            if (!Running) return;
            if (mobile == Actor) ++PlayerDeaths;
            if (Owns(mobile)) { ++EnemyDeaths; if (mobile is ArachnidOutbreakChampion) BossKilled = true; }
        }
        private static void OnMovement(MovementEventArgs e)
        { if (Running && e.Mobile == Actor) ++MovementRequests; }
        private static void Begin()
        {
            if (Mode == "preparation" || State != "prepared" || Actor.NetState == null || Actor.Blessed || Actor.AccessLevel != AccessLevel.Player)
                throw new Exception("Combat requires the frozen prepared original Player.");
            if (!File.Exists(Path.Combine(Evidence, "archmage-combat-freeze.private.json"))) throw new Exception("Missing combat freeze.");
            foreach (Item item in World.Items.Values)
                if (item is Gold && item.Parent == null && item.Map == Map.Sosaria && Site.Bounds.Contains(item.Location))
                    OriginalGroundGold[item.Serial.Value] = true;
            Controller = new ArachnidOutbreakController();
            ArachnidOutbreakState state = (ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State", Private).GetValue(Controller);
            state.SetEnabled(true);
            Started = LastOwnedDamage = DateTime.UtcNow; Deadline = Started.AddMinutes(Mode == "stationary" ? 6 : 46); State = "running-" + Mode;
            typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(Controller, new object[] { Site, Placement });
            typeof(ArachnidOutbreakController).GetMethod("StartTimer", Private).Invoke(Controller, null);
        }

        private static void Tick()
        {
            try
            {
                if (File.Exists(Path.Combine(Evidence, "archmage-client-error.txt"))) State = "client-failed";
                if (State == "ready" && File.Exists(Path.Combine(Evidence, "archmage-macro-prepared.txt")))
                { Manifest("archmage-prepared.private.xml"); State = "prepared"; }
                string command = Path.Combine(Evidence, "archmage-command.txt");
                if (File.Exists(command))
                {
                    string text = File.ReadAllText(command).Trim(); File.Delete(command);
                    if (text == "start-" + Mode && Mode != "preparation") Begin();
                    else if (text == "stop") State = "stopped";
                    else throw new Exception("Unsupported archmage command.");
                }
                if (Running)
                {
                    if (!Actor.Alive) State = "dead";
                    else if (Controller.Phase == ArachnidOutbreakPhase.Cooldown)
                        State = BossKilled && Controller.Status == "Completed; cooldown active." ? "completed" : "aborted";
                    else if (Mode == "stationary" && (DateTime.UtcNow - Started).TotalSeconds >= 300) State = "stationary-window-ended";
                    else if (Mode == "stationary" && Controller.ClearedBroods != 0 && (DateTime.UtcNow - LastOwnedDamage).TotalSeconds >= 120) State = "stationary-idle";
                    else if ((DateTime.UtcNow - Started).TotalSeconds >= 2700) State = "timeout";
                }
                if (Started != DateTime.MinValue && !Running && Ended == DateTime.MinValue)
                { Ended = DateTime.UtcNow; Manifest("archmage-final.private.xml"); }
                if (DateTime.UtcNow > Deadline && State == "ready")
                { Failure = "Native preparation deadline."; State = "failed"; }
                Status();
                if (File.Exists(Path.Combine(Evidence, "native-stop.txt")) || DateTime.UtcNow > Deadline)
                {
                    if (Controller != null) Controller.Delete();
                    Poll.Stop(); AutoSave.SavesEnabled = false;
                    File.WriteAllText(Path.Combine(Evidence, "native-stopped.txt"), "Archmage fixture stopped without saving; only its owned encounter cleaned.");
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
                if (item is ResearchBag || item is AncientSpellbook)
                    foreach (FieldInfo field in item.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                        if (field.FieldType == typeof(int))
                        { writer.WriteStartElement("research"); Attr(writer, "field", field.Name); Attr(writer, "value", field.GetValue(item)); writer.WriteEndElement(); }
                writer.WriteEndElement();
            }
        }

        private static void Status()
        {
            string path = Path.Combine(Evidence, "archmage-state.xml");
            using (XmlWriter writer = XmlWriter.Create(path + ".tmp", new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("archmage"); Attr(writer, "state", State); Attr(writer, "failure", Failure);
                Attr(writer, "online", Actor != null && Actor.NetState != null); Attr(writer, "encounter", Started != DateTime.MinValue);
                Attr(writer, "mode", Mode); Attr(writer, "blessed", Actor != null && Actor.Blessed);
                Attr(writer, "seconds", Started == DateTime.MinValue ? 0 : ((Ended == DateTime.MinValue ? DateTime.UtcNow : Ended) - Started).TotalSeconds);
                Attr(writer, "ownedDeaths", EnemyDeaths); Attr(writer, "playerDeaths", PlayerDeaths); Attr(writer, "bossKilled", BossKilled);
                Attr(writer, "incoming", Incoming); Attr(writer, "outgoing", Outgoing); Attr(writer, "restored", Restored); Attr(writer, "movementRequests", MovementRequests);
                Attr(writer, "clearedBroods", Controller == null ? 0 : Controller.ClearedBroods);
                Attr(writer, "phase", Controller == null ? String.Empty : Controller.Phase.ToString());
                if (Ended != DateTime.MinValue && Controller != null)
                {
                    int glands = 0, skulls = 0, artifacts = 0, gold = 0;
                    foreach (Item item in World.Items.Values)
                    {
                        ArachnidOutbreakClaim claim = item as ArachnidOutbreakClaim;
                        if (claim != null && claim.Owner == Actor && claim.AttemptId == Controller.AttemptId)
                        {
                            foreach (Item reward in claim.Items)
                                if (reward is BroodVenomGland) glands += reward.Amount;
                            if (claim.Kind == ArachnidOutbreakClaimKind.Skull) ++skulls;
                            if (claim.Kind == ArachnidOutbreakClaimKind.Artifact) ++artifacts;
                        }
                        if (item is Gold && item.Parent == null && item.Map == Map.Sosaria && Site.Bounds.Contains(item.Location)
                            && !OriginalGroundGold.ContainsKey(item.Serial.Value)) gold += item.Amount;
                    }
                    Attr(writer, "pendingGlands", glands); Attr(writer, "pendingSkulls", skulls); Attr(writer, "pendingArtifacts", artifacts);
                    Attr(writer, "newGroundGold", gold);
                }
                if (Placement != null)
                    for (int stage = 0; stage < 4; ++stage)
                    {
                        Point3D point = stage == 3 ? Placement.Boss : Placement.Brood(stage);
                        writer.WriteStartElement("anchor"); Attr(writer, "stage", stage);
                        Attr(writer, "x", point.X); Attr(writer, "y", point.Y); Attr(writer, "z", point.Z); writer.WriteEndElement();
                    }
                if (Controller != null)
                    foreach (Mobile mobile in (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures", Private).GetValue(Controller))
                        if (mobile != null && !mobile.Deleted && mobile.Alive)
                        { writer.WriteStartElement("enemy"); Attr(writer, "serial", mobile.Serial.Value); Attr(writer, "type", mobile.GetType().Name); writer.WriteEndElement(); }
                writer.WriteEndElement();
            }
            if (File.Exists(path)) File.Replace(path + ".tmp", path, null); else File.Move(path + ".tmp", path);
        }
    }
}
