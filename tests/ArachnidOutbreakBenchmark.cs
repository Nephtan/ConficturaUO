using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Spells.Chivalry;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    // Copied-world preparation and observation only. Combat is issued by the copied client.
    public static class ArachnidOutbreakBenchmark
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "../evidence"));
        private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private static PlayerMobile Actor;
        private static ArachnidOutbreakController Controller;
        private static ArachnidOutbreakPlacement Placement;
        private static ArachnidOutbreakSite Site;
        private static Timer Poll;
        private static DateTime Started;
        private static DateTime Ended;
        private static bool BossKilled;
        private static bool ManifestConnected;
        private static bool NavigationOnly;
        private static int NavigationStage = 1;
        private static readonly Point3D[][] Travel = new Point3D[3][];
        private static int EnemyDeaths;
        private static DateTime Deadline = DateTime.UtcNow.AddHours(2);
        private static string State = "preparing";
        private static string Failure = String.Empty;
        private static int IncomingDamage, OutgoingDamage, RestoredHits;
        private static readonly Dictionary<string, Item> Supplies = new Dictionary<string, Item>();
        private static readonly List<Item> Equipment = new List<Item>();
        private static int ProbeSequence;
        private static string ProbeName;
        private static string ProbeItem;
        private static BaseCreature ProbeTarget;
        private static readonly List<BaseCreature> ProbeExtras = new List<BaseCreature>();
        private static int ProbeMana, ProbeHits, ProbeStam, ProbeTithing, ProbeAmount, ProbeTargetHits;
        private static DateTime ProbeDeadline;
        private static bool ProbeResultWritten;

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard = Path.Combine(Evidence, "native-private-root.txt");
            if (!File.Exists(guard) || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Benchmark requires its exact disposable-root guard.");
            if (ArachnidOutbreakController.DeliveryReady)
                throw new InvalidOperationException("Production must remain disabled.");
            AutoSave.SavesEnabled = false;
            EventSink.HealthChanged += OnHealthChanged;
            EventSink.MobileDeath += OnMobileDeath;
            Timer.DelayCall(TimeSpan.FromSeconds(1), Prepare);
        }

        private static void Prepare()
        {
            try
            {
                IAccount account = Accounts.GetAccount(File.ReadAllText(Path.Combine(Evidence, "account-name.private")));
                if (account == null) throw new Exception("Copied account unavailable.");
                for (int i = 0; i < 7; ++i)
                    if (account[i] is PlayerMobile) { Actor = (PlayerMobile)account[i]; break; }
                if (Actor == null || Actor.RaceID != 0 || Actor.Mounted)
                    throw new Exception("Reference requires an ordinary unmounted human copied actor.");
                Actor.AccessLevel = AccessLevel.Player;
                if (!Actor.Alive) Actor.Resurrect();
                Actor.Blessed = true;
                Actor.Hidden = false;
                Actor.Warmode = false;
                Actor.Combatant = null;

                // Original copied inventory is retained in memory, never saved or destroyed.
                Bag retained = new Bag();
                foreach (Item item in new List<Item>(Actor.Backpack.Items)) retained.DropItem(item);
                foreach (Item item in new List<Item>(Actor.Items))
                    if (item.Layer != Layer.Backpack && item.Layer != Layer.Bank && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair)
                        retained.DropItem(item);

                Actor.SkillStart = 10000; Actor.SkillBoost = 0; Actor.SkillEther = 0;
                Actor.Skills.Cap = 10000; Actor.StatCap = 250;
                Actor.RawStr = 125; Actor.RawDex = 100; Actor.RawInt = 25;
                Actor.StrLock = Actor.DexLock = Actor.IntLock = StatLockType.Locked;
                Actor.Fame = 10000; Actor.Karma = 10000; Actor.TithingPoints = 10000;
                Actor.Hunger = Actor.Thirst = 20;
                for (int i = 0; i < Actor.Skills.Length; ++i)
                {
                    Actor.Skills[i].Base = 0;
                    Actor.Skills[i].Cap = 100;
                    Actor.Skills[i].SetLockNoRelay(SkillLock.Locked);
                }
                SetSkill(SkillName.Swords, 120, 125);
                SetSkill(SkillName.Tactics, 120, 120);
                SetSkill(SkillName.Anatomy, 120, 120);
                SetSkill(SkillName.MagicResist, 120, 120);
                SetSkill(SkillName.Healing, 100, 125);
                SetSkill(SkillName.Parry, 100, 125);
                SetSkill(SkillName.Knightship, 100, 100);
                SetSkill(SkillName.Focus, 100, 100);
                SetSkill(SkillName.Alchemy, 100, 100);

                Equip(new Artifact_BladeOfInsanity());
                Equip(new Artifact_AchillesShield());
                Equip(new Artifact_ConansHelm());
                Equip(new Artifact_TunicOfAegis());
                Equip(new Artifact_ArmsOfAegis());
                Equip(new Artifact_GorgetOfAegis());
                Equip(new Artifact_GlovesOfAegis());
                Equip(new Artifact_LeggingsOfAegis());
                Equip(new Artifact_RingOfHealth());
                Equip(new Artifact_BraceletOfHealth());
                Actor.AddToBackpack(new BookOfChivalry());
                Supply("bandage", new Bandage(), 500);
                Supply("cure", new GreaterCurePotion(), 100);
                Supply("heal", new GreaterHealPotion(), 75);
                Supply("petals", new OrangePetals(), 10);
                Actor.Hits = Actor.HitsMax; Actor.Stam = Actor.StamMax; Actor.Mana = Actor.ManaMax;

                // Only this disposable process reviews a reserve. Repository review flags stay false.
                List<WildernessEncounterReserve> reserves = new List<WildernessEncounterReserve>();
                foreach (WildernessEncounterReserve reserve in WildernessEncounterReserves.Proposals)
                    reserves.Add(new WildernessEncounterReserve(reserve.Id, reserve.Center.X, reserve.Center.Y, reserve.Id == "central-woodland"));
                typeof(WildernessEncounterReserves).GetField("m_Proposals", BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(null, reserves.AsReadOnly());
                Site = new ArachnidOutbreakSite("central-woodland", 2514, 2088, true);
                typeof(ArachnidOutbreakSites).GetField("m_Candidates", BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(null, Array.AsReadOnly(new ArachnidOutbreakSite[] { Site }));
                string reason;
                if (!Site.Validate(new SosariaOutbreakTerrain(), out Placement, out reason))
                    throw new Exception("Copied-world site rejected: " + reason);
                Point3D staging = Placement.Brood(0);
                staging.Y -= 2;
                staging.Z = Map.Sosaria.GetAverageZ(staging.X, staging.Y);
                Actor.MoveToWorld(staging, Map.Sosaria);
                BuildTravel();
                State = "ready";
                WriteManifest();
            }
            catch (Exception error) { Failure = error.ToString(); State = "failed"; }
            Poll = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromMilliseconds(250), Tick);
        }

        private static void SetSkill(SkillName name, double value, double cap)
        {
            Actor.Skills[name].Cap = cap;
            Actor.Skills[name].Base = value;
        }
        private static void BuildTravel()
        {
            Point3D[] mesh = (Point3D[])typeof(ArachnidOutbreakPlacement).GetField("m_Walkable", Private).GetValue(Placement);
            Dictionary<Point2D, Point3D> ground = new Dictionary<Point2D, Point3D>();
            foreach (Point3D point in mesh) ground[new Point2D(point.X, point.Y)] = point;
            int[] dx = new int[] { -1, 1, 0, 0 }, dy = new int[] { 0, 0, -1, 1 };
            for (int leg = 0; leg < 3; ++leg)
            {
                Point3D from = Placement.Brood(leg), to = leg == 2 ? Placement.Boss : Placement.Brood(leg + 1);
                Point2D start = new Point2D(from.X, from.Y), end = new Point2D(to.X, to.Y);
                Queue<Point2D> queue = new Queue<Point2D>();
                Dictionary<Point2D, Point2D> previous = new Dictionary<Point2D, Point2D>();
                previous[start] = start; queue.Enqueue(start);
                while (queue.Count > 0 && !previous.ContainsKey(end))
                {
                    Point2D current = queue.Dequeue();
                    for (int direction = 0; direction < 4; ++direction)
                    {
                        Point2D next = new Point2D(current.X + dx[direction], current.Y + dy[direction]);
                        if (!ground.ContainsKey(next) || previous.ContainsKey(next) || Math.Abs(ground[next].Z - ground[current].Z) > 2) continue;
                        previous[next] = current; queue.Enqueue(next);
                    }
                }
                if (!previous.ContainsKey(end)) throw new Exception("Disconnected benchmark travel leg.");
                List<Point3D> path = new List<Point3D>();
                Point2D cursor = end;
                while (cursor != start) { path.Add(ground[cursor]); cursor = previous[cursor]; }
                path.Add(ground[start]); path.Reverse();
                List<Point3D> waypoints = new List<Point3D>();
                for (int i = 0; i < path.Count; i += 6) waypoints.Add(path[i]);
                if (waypoints[waypoints.Count - 1] != to) waypoints.Add(to);
                Travel[leg] = waypoints.ToArray();
            }
        }
        private static void Equip(Item item)
        {
            if (Array.IndexOf(Loot.ArtyTypes, item.GetType()) < 0)
                throw new Exception("Equipment is absent from the obtainable named-artifact pool: " + item.GetType().FullName);
            if (!Actor.EquipItem(item)) throw new Exception("Equipment rejected: " + item.GetType().FullName);
            Equipment.Add(item);
        }
        private static void Supply(string key, Item item, int amount)
        {
            item.Amount = amount;
            Actor.AddToBackpack(item);
            Supplies.Add(key, item);
        }
        private static void Begin(bool antivenom)
        {
            if (State != "ready" || Actor.NetState == null) throw new Exception("Begin requires the ready connected copied Player.");
            if (antivenom) Supply("antivenom", new BroodAntivenom(), 12);
            Controller = new ArachnidOutbreakController();
            ArachnidOutbreakState state = (ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State", Private).GetValue(Controller);
            state.SetEnabled(true);
            Actor.Blessed = false;
            Started = DateTime.UtcNow;
            State = antivenom ? "running-antivenom" : "running-no-antivenom";
            typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(Controller, new object[] { Site, Placement });
            typeof(ArachnidOutbreakController).GetMethod("StartTimer", Private).Invoke(Controller, null);
        }
        private static List<Mobile> Creatures()
        {
            return Controller == null ? new List<Mobile>() : (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures", Private).GetValue(Controller);
        }
        private static void OnHealthChanged(HealthChangedEventArgs e)
        {
            if (ProbeName != null && State == "toolkit-" + ProbeName
                && (e.Mobile == Actor || e.Mobile == ProbeTarget || ProbeExtras.Contains(e.Mobile as BaseCreature)))
                File.AppendAllText(Path.Combine(Evidence, "toolkit-health.csv"), DateTime.UtcNow.ToString("o") + "," + ProbeSequence + "," + e.Mobile.Serial.Value + "," + (e.Source == null ? 0 : e.Source.Serial.Value) + "," + e.Kind + "," + e.OldHits + "," + e.NewHits + "\n");
            if (!State.StartsWith("running")) return;
            int change = e.NewHits - e.OldHits;
            if (e.Mobile == Actor)
            {
                if (change < 0) IncomingDamage -= change;
                else RestoredHits += change;
            }
            else if (change < 0 && Creatures().Contains(e.Mobile)) OutgoingDamage -= change;
        }
        private static void OnMobileDeath(Mobile mobile)
        {
            IArachnidOutbreakOwned owned = mobile as IArachnidOutbreakOwned;
            if (!State.StartsWith("running") || owned == null || owned.Controller != Controller) return;
            ++EnemyDeaths;
            if (mobile is ArachnidOutbreakChampion) BossKilled = true;
        }
        private static void ClearProbeTargets()
        {
            if (ProbeTarget != null && !ProbeTarget.Deleted) ProbeTarget.Delete();
            ProbeTarget = null;
            foreach (BaseCreature extra in ProbeExtras) if (!extra.Deleted) extra.Delete();
            ProbeExtras.Clear();
        }
        private static BaseCreature ProbeSpider(int dx, int dy)
        {
            BaseCreature spider = new Cow();
            spider.Name = "controlled toolkit target"; spider.Fame = 5000; spider.Karma = -5000; spider.Tamable = false;
            spider.SetHits(10000); spider.SetDamage(0); spider.FightMode = FightMode.None; spider.CantWalk = true;
            spider.SetResistance(ResistanceType.Physical, 50); spider.SetResistance(ResistanceType.Fire, 25);
            spider.SetResistance(ResistanceType.Energy, 25);
            spider.MoveToWorld(new Point3D(Actor.X + dx, Actor.Y + dy, Actor.Z), Actor.Map);
            if (spider.AIObject != null && spider.AIObject.m_Timer != null) spider.AIObject.m_Timer.Stop();
            return spider;
        }
        private static int Remaining(string key)
        {
            Item item;
            return Supplies.TryGetValue(key, out item) && !item.Deleted ? item.Amount : 0;
        }
        private static void BeginProbe(string name)
        {
            if (Started != DateTime.MinValue || Controller != null || Actor.NetState == null || !Actor.Alive)
                throw new Exception("Toolkit probes require the connected idle preparation scene, never a measured run.");
            string[] allowed = new string[] { "strength", "agility", "refresh", "petals", "cure", "cleanse", "bandage", "heal", "close-wounds", "consecrate", "fury", "enemy-one", "holy-light", "dispel", "remove-curse", "ability-1", "ability-2", "ability-3", "ability-4", "ability-5" };
            if (Array.IndexOf(allowed, name) < 0) throw new Exception("Unknown toolkit probe.");
            State = "preparing-probe";
            Failure = null;
            Actor.Combatant = null; Actor.Warmode = false; Actor.Blessed = false;
            Actor.Paralyzed = false; Actor.Frozen = false; Actor.CurePoison(Actor);
            WeaponAbility.ClearCurrentAbility(Actor);
            BandageContext bandage = BandageContext.GetContext(Actor);
            if (bandage != null) bandage.StopHeal();
            ClearProbeTargets();
            if (!Supplies.ContainsKey("strength"))
            {
                Supply("strength", new GreaterStrengthPotion(), 20);
                Supply("agility", new GreaterAgilityPotion(), 20);
                Supply("refresh", new TotalRefreshPotion(), 20);
            }
            if (name == "remove-curse")
            {
                Actor.RemoveStatMod("[Magic] Str Offset");
                Actor.AddStatMod(new StatMod(StatType.Str, "[Magic] Str Offset", -20, TimeSpan.FromMinutes(1)));
            }
            Actor.Hits = Actor.HitsMax; Actor.Stam = Actor.StamMax; Actor.Mana = Actor.ManaMax;
            if (name == "heal" || name == "close-wounds") Actor.Hits -= 100;
            if (name == "refresh" || name == "fury") Actor.Stam = 20;
            if (name == "cure" || name == "cleanse" || name == "bandage" || name.StartsWith("ability-") || name == "holy-light" || name == "dispel")
                ProbeTarget = ProbeSpider(0, 1);
            if (name == "ability-1")
            {
                ProbeExtras.Add(ProbeSpider(-1, 0)); ProbeExtras.Add(ProbeSpider(1, 0));
            }
            if (name == "cure" || name == "cleanse" || name == "bandage")
            {
                Actor.ApplyPoison(ProbeTarget, name == "cure" ? Poison.Lethal : Poison.Deadly);
                if (!Actor.Poisoned) throw new Exception("Probe poison setup did not apply.");
            }
            ++ProbeSequence; ProbeName = name; ProbeItem = Array.IndexOf(new string[] { "strength", "agility", "refresh", "petals", "cure", "bandage", "heal" }, name) >= 0 ? name : null;
            ProbeMana = Actor.Mana; ProbeHits = Actor.Hits; ProbeStam = Actor.Stam; ProbeTithing = Actor.TithingPoints;
            ProbeAmount = ProbeItem == null ? 0 : Remaining(ProbeItem); ProbeTargetHits = ProbeTarget == null ? 0 : ProbeTarget.Hits;
            ProbeDeadline = DateTime.UtcNow.AddSeconds(25); ProbeResultWritten = false;
            State = "toolkit-" + name;
        }
        private static bool ProbeSucceeded()
        {
            bool item = ProbeItem != null && Remaining(ProbeItem) < ProbeAmount;
            bool spell = Actor.TithingPoints < ProbeTithing && Actor.Mana < ProbeMana;
            if (ProbeName == "strength") return item && Actor.Str == 150;
            if (ProbeName == "agility") return item && Actor.Dex == 150;
            if (ProbeName == "refresh") return item && Actor.Stam == Actor.StamMax;
            if (ProbeName == "petals") return item;
            if (ProbeName == "cure" || ProbeName == "bandage") return item && !Actor.Poisoned;
            if (ProbeName == "cleanse") return spell && !Actor.Poisoned;
            if (ProbeName == "heal") return item && Actor.Hits > ProbeHits;
            if (ProbeName == "close-wounds") return spell && Actor.Hits > ProbeHits;
            if (ProbeName == "consecrate") return spell && ((BaseWeapon)Actor.Weapon).Consecrated;
            if (ProbeName == "fury") return spell && DivineFurySpell.UnderEffect(Actor) && Actor.Stam == Actor.StamMax;
            if (ProbeName == "enemy-one") return spell && (Actor.WaitingForEnemy || Actor.EnemyOfOneType != null);
            if (ProbeName == "holy-light") return spell && ProbeTarget.Hits < ProbeTargetHits;
            if (ProbeName == "dispel") return spell && ProbeTarget.CheckFlee();
            if (ProbeName == "remove-curse")
            {
                StatMod mod = Actor.GetStatMod("[Magic] Str Offset");
                return spell && (mod == null || mod.Offset >= 0);
            }
            return ProbeName.StartsWith("ability-") && Actor.Mana < ProbeMana && ProbeTarget.Hits < ProbeTargetHits
                && WeaponAbility.GetCurrentAbility(Actor) == null;
        }
        private static void Tick()
        {
            try
            {
                string request = Path.Combine(Evidence, "benchmark-command.txt");
                if (File.Exists(request))
                {
                    string command = File.ReadAllText(request).Trim(); File.Delete(request);
                    if (command == "start-no-antivenom") Begin(false);
                    else if (command == "start-antivenom") Begin(true);
                    else if (command == "start-navigation" && State == "ready" && Actor.NetState != null)
                    { NavigationOnly = true; State = "navigation"; Started = DateTime.UtcNow; }
                    else if (command.StartsWith("probe-")) BeginProbe(command.Substring(6));
                    else if (command == "stop") State = "stopped";
                    else throw new Exception("Unknown benchmark command.");
                }
                if (State.StartsWith("running"))
                {
                    if (!Actor.Alive) State = "dead";
                    else if (Controller.Phase == ArachnidOutbreakPhase.Cooldown) State = BossKilled && Controller.Status == "Completed; cooldown active." ? "completed" : "aborted";
                    else if (Controller.Phase == ArachnidOutbreakPhase.Disabled) State = "aborted";
                }
                if (State == "navigation")
                {
                    Point3D destination = NavigationStage == 3 ? Placement.Boss : Placement.Brood(NavigationStage);
                    if (Actor.InRange(destination, 1) && ++NavigationStage > 3) State = "navigation-completed";
                }
                if (ProbeName != null && State == "toolkit-" + ProbeName)
                {
                    if (ProbeSucceeded()) State = "toolkit-complete";
                    else if (DateTime.UtcNow > ProbeDeadline || !Actor.Alive || Actor.Hits < Actor.HitsMax / 5)
                    { State = "toolkit-failed"; Failure = "Timeout or unsafe health: " + ProbeName; Actor.Blessed = true; Actor.CurePoison(Actor); }
                    if (State == "toolkit-complete" || State == "toolkit-failed") { Actor.Combatant = null; Actor.Warmode = false; }
                }
                if (Actor != null && Actor.NetState != null && State == "ready" && !ManifestConnected)
                { WriteManifest(); ManifestConnected = true; }
                if (File.Exists(Path.Combine(Evidence, "benchmark-driver-failed.txt")) && (State.StartsWith("running") || State == "navigation"))
                    State = "driver-failed";
                if (Started != DateTime.MinValue && !State.StartsWith("running") && State != "navigation" && Ended == DateTime.MinValue) Ended = DateTime.UtcNow;
                WriteStatus();
                if (ProbeName != null && !ProbeResultWritten && (State == "toolkit-complete" || State == "toolkit-failed"))
                {
                    File.Copy(Path.Combine(Evidence, "benchmark-state.xml"), Path.Combine(Evidence, "toolkit-result-" + ProbeSequence + ".xml"));
                    ProbeResultWritten = true;
                }
                if (DateTime.UtcNow > Deadline || File.Exists(Path.Combine(Evidence, "native-stop.txt")))
                {
                    if (Controller != null) Controller.Delete();
                    ClearProbeTargets();
                    Poll.Stop(); AutoSave.SavesEnabled = false;
                    File.WriteAllText(Path.Combine(Evidence, "native-stopped.txt"), "Benchmark stopped without saving.");
                }
            }
            catch (Exception error) { Failure = error.ToString(); State = "failed"; WriteStatus(); }
        }

        private static XmlWriter Open(string name)
        {
            return XmlWriter.Create(Path.Combine(Evidence, name + ".tmp"), new XmlWriterSettings { Indent = true });
        }
        private static void Commit(string name)
        {
            string path = Path.Combine(Evidence, name);
            if (File.Exists(path)) File.Replace(path + ".tmp", path, null);
            else File.Move(path + ".tmp", path);
        }
        private static void Attr(XmlWriter writer, string name, object value)
        {
            writer.WriteAttributeString(name, Convert.ToString(value, CultureInfo.InvariantCulture));
        }
        private static void WriteManifest()
        {
            using (XmlWriter writer = Open("benchmark-reference.xml"))
            {
                writer.WriteStartElement("reference"); Attr(writer, "utc", DateTime.UtcNow.ToString("o"));
                Attr(writer, "serial", Actor.Serial.Value); Attr(writer, "access", Actor.AccessLevel);
                Attr(writer, "rawStr", Actor.RawStr); Attr(writer, "rawDex", Actor.RawDex); Attr(writer, "rawInt", Actor.RawInt);
                Attr(writer, "str", Actor.Str); Attr(writer, "dex", Actor.Dex); Attr(writer, "int", Actor.Int);
                Attr(writer, "hitsMax", Actor.HitsMax); Attr(writer, "stamMax", Actor.StamMax); Attr(writer, "manaMax", Actor.ManaMax);
                Attr(writer, "skillCap", Actor.Skills.Cap); Attr(writer, "baseSkillTotal", Actor.Skills.Total);
                Attr(writer, "physical", Actor.PhysicalResistance); Attr(writer, "fire", Actor.FireResistance);
                Attr(writer, "cold", Actor.ColdResistance); Attr(writer, "poison", Actor.PoisonResistance); Attr(writer, "energy", Actor.EnergyResistance);
                Attr(writer, "fame", Actor.Fame); Attr(writer, "karma", Actor.Karma); Attr(writer, "tithing", Actor.TithingPoints);
                Attr(writer, "totalWeight", Actor.TotalWeight); Attr(writer, "race", Actor.RaceID);
                Attr(writer, "coreAOS", Core.AOS); Attr(writer, "coreML", Core.ML);
                Attr(writer, "abilityBaseSkill", MyServerSettings.SpecialWeaponAbilSkill());
                Attr(writer, "enhancePotions", BasePotion.EnhancePotions(Actor));
                Attr(writer, "skillOrigin", Actor.SkillStart); Attr(writer, "skillBoost", Actor.SkillBoost); Attr(writer, "skillEther", Actor.SkillEther);
                BaseWeapon weapon = Actor.Weapon as BaseWeapon;
                if (weapon != null)
                    foreach (WeaponAbility ability in new WeaponAbility[] { weapon.PrimaryAbility, weapon.SecondaryAbility, weapon.ThirdAbility, weapon.FourthAbility, weapon.FifthAbility })
                    {
                        if (ability == null) continue;
                        writer.WriteStartElement("weaponAbility"); Attr(writer, "name", ability.GetType().Name);
                        Attr(writer, "baseMana", ability.BaseMana); Attr(writer, "currentManaCost", ability.CalculateMana(Actor));
                        Attr(writer, "requiredSkill", ability.GetRequiredSkill(Actor)); Attr(writer, "skillGate", ability.CheckSkills(Actor));
                        writer.WriteEndElement();
                    }
                foreach (PaladinSpell spell in new PaladinSpell[] { new CleanseByFireSpell(Actor, null), new CloseWoundsSpell(Actor, null), new ConsecrateWeaponSpell(Actor, null), new DispelEvilSpell(Actor, null), new DivineFurySpell(Actor, null), new EnemyOfOneSpell(Actor, null), new HolyLightSpell(Actor, null), new NobleSacrificeSpell(Actor, null), new RemoveCurseSpell(Actor, null), new SacredJourneySpell(Actor, null) })
                {
                    writer.WriteStartElement("spell"); Attr(writer, "name", spell.GetType().Name);
                    Attr(writer, "baseMana", spell.RequiredMana); Attr(writer, "currentManaCost", spell.ScaleMana(spell.RequiredMana));
                    Attr(writer, "requiredSkill", spell.RequiredSkill); Attr(writer, "tithing", spell.RequiredTithing); writer.WriteEndElement();
                }
                for (int i = 0; i < Actor.Skills.Length; ++i)
                    if (Actor.Skills[i].Base > 0)
                    {
                        Skill skill = Actor.Skills[i]; writer.WriteStartElement("skill");
                        Attr(writer, "name", skill.SkillName); Attr(writer, "base", skill.Base);
                        Attr(writer, "effective", skill.Value); Attr(writer, "cap", skill.Cap); Attr(writer, "lock", skill.Lock); writer.WriteEndElement();
                    }
                foreach (Item item in Equipment)
                {
                    writer.WriteStartElement("equipment"); Attr(writer, "type", item.GetType().FullName);
                    Attr(writer, "serial", item.Serial.Value); Attr(writer, "name", item.Name); Attr(writer, "layer", item.Layer);
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
                    foreach (string propertyName in new string[] { "HitPoints", "MaxHitPoints", "MinDamage", "MaxDamage", "Speed" })
                    {
                        PropertyInfo property = item.GetType().GetProperty(propertyName);
                        if (property != null) { writer.WriteStartElement("property"); Attr(writer, "name", propertyName); Attr(writer, "value", property.GetValue(item, null)); writer.WriteEndElement(); }
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            Commit("benchmark-reference.xml");
        }
        private static void WriteStatus()
        {
            using (XmlWriter writer = Open("benchmark-state.xml"))
            {
                writer.WriteStartElement("benchmark"); Attr(writer, "utc", DateTime.UtcNow.ToString("o")); Attr(writer, "state", State);
                Attr(writer, "seconds", Started == DateTime.MinValue ? 0 : ((Ended == DateTime.MinValue ? DateTime.UtcNow : Ended) - Started).TotalSeconds);
                Attr(writer, "failure", Failure); Attr(writer, "incomingDamage", IncomingDamage); Attr(writer, "outgoingDamage", OutgoingDamage); Attr(writer, "restoredHits", RestoredHits);
                Attr(writer, "enemyDeaths", EnemyDeaths); Attr(writer, "bossKilled", BossKilled);
                Attr(writer, "navigationOnly", NavigationOnly); Attr(writer, "navigationStage", NavigationStage);
                if (ProbeName != null)
                {
                    Attr(writer, "probe", ProbeName); Attr(writer, "probeSequence", ProbeSequence);
                    Attr(writer, "probeManaBefore", ProbeMana); Attr(writer, "probeHitsBefore", ProbeHits); Attr(writer, "probeStamBefore", ProbeStam);
                    Attr(writer, "probeTithingBefore", ProbeTithing); Attr(writer, "probeAmountBefore", ProbeAmount);
                    Attr(writer, "probeTarget", ProbeTarget == null ? 0 : ProbeTarget.Serial.Value);
                    Attr(writer, "probeTargetHitsBefore", ProbeTargetHits); Attr(writer, "probeTargetHits", ProbeTarget == null ? 0 : ProbeTarget.Hits);
                    Attr(writer, "probeScope", "Controlled native ability probe; harmless stopped targets; Player actor; between-case setup; not encounter acceptance");
                }
                if (Actor != null)
                {
                    Attr(writer, "actor", Actor.Serial.Value); Attr(writer, "online", Actor.NetState != null); Attr(writer, "alive", Actor.Alive);
                    Attr(writer, "x", Actor.X); Attr(writer, "y", Actor.Y); Attr(writer, "z", Actor.Z);
                    Attr(writer, "hits", Actor.Hits); Attr(writer, "hitsMax", Actor.HitsMax); Attr(writer, "mana", Actor.Mana); Attr(writer, "poisoned", Actor.Poisoned);
                    Attr(writer, "stam", Actor.Stam); Attr(writer, "stamMax", Actor.StamMax); Attr(writer, "manaMax", Actor.ManaMax);
                    WeaponAbility currentAbility = WeaponAbility.GetCurrentAbility(Actor);
                    Attr(writer, "currentAbility", currentAbility == null ? String.Empty : currentAbility.GetType().Name);
                    Attr(writer, "str", Actor.Str); Attr(writer, "dex", Actor.Dex); Attr(writer, "int", Actor.Int);
                    Attr(writer, "paralyzed", Actor.Paralyzed); Attr(writer, "frozen", Actor.Frozen);
                    Attr(writer, "blessed", Actor.Blessed); Attr(writer, "tithing", Actor.TithingPoints);
                    Attr(writer, "bandaging", BandageContext.GetContext(Actor) != null);
                    Attr(writer, "spellCooldownSeconds", Math.Max(0.0, (Actor.NextSpellTime - TurnBasedCombatBridge.GetTime(Actor)).TotalSeconds));
                    Attr(writer, "currentSpell", Actor.Spell == null ? String.Empty : Actor.Spell.GetType().Name);
                    Attr(writer, "targetPending", Actor.Target != null);
                    int effects;
                    BroodAntivenomEffect protection = BroodAntivenomEffect.Find(Actor, out effects);
                    Attr(writer, "antivenomCharges", protection != null && protection.Active(DateTime.UtcNow) ? protection.Charges : 0);
                    ArachnidWebRestraint restraint = ArachnidWebRestraint.Find(Actor);
                    Attr(writer, "restraint", restraint == null ? 0 : restraint.Serial.Value);
                    foreach (Mobile creature in Creatures())
                        if (creature is ArachnidOutbreakChampion)
                        {
                            ArachnidWebWarning warning = (ArachnidWebWarning)typeof(ArachnidOutbreakChampion).GetField("m_Web", Private).GetValue(creature);
                            if (warning != null && !warning.Deleted && warning.Victim == Actor)
                            {
                                Attr(writer, "warning", warning.Serial.Value); Attr(writer, "warningX", warning.X); Attr(writer, "warningY", warning.Y);
                            }
                        }
                }
                if (Controller != null) { Attr(writer, "phase", Controller.Phase); Attr(writer, "clearedBroods", Controller.ClearedBroods); Attr(writer, "controllerStatus", Controller.Status); }
                foreach (KeyValuePair<string, Item> supply in Supplies)
                {
                    writer.WriteStartElement("supply"); Attr(writer, "key", supply.Key); Attr(writer, "serial", supply.Value.Serial.Value);
                    Attr(writer, "remaining", supply.Value.Deleted ? 0 : supply.Value.Amount); writer.WriteEndElement();
                }
                if (Placement != null)
                {
                    for (int stage = 0; stage < 4; ++stage)
                    {
                        Point3D point = stage == 3 ? Placement.Boss : Placement.Brood(stage);
                        writer.WriteStartElement("anchor"); Attr(writer, "stage", stage); Attr(writer, "x", point.X); Attr(writer, "y", point.Y); Attr(writer, "z", point.Z); writer.WriteEndElement();
                    }
                    for (int leg = 0; leg < Travel.Length; ++leg)
                    {
                        writer.WriteStartElement("route"); Attr(writer, "to", leg + 1);
                        if (Travel[leg] != null) foreach (Point3D point in Travel[leg])
                        { writer.WriteStartElement("step"); Attr(writer, "x", point.X); Attr(writer, "y", point.Y); Attr(writer, "z", point.Z); writer.WriteEndElement(); }
                        writer.WriteEndElement();
                    }
                }
                foreach (Mobile creature in Creatures())
                    if (!creature.Deleted && creature.Alive)
                    {
                        writer.WriteStartElement("enemy"); Attr(writer, "serial", creature.Serial.Value); Attr(writer, "type", creature.GetType().Name);
                        Attr(writer, "name", creature.Name);
                        Attr(writer, "x", creature.X); Attr(writer, "y", creature.Y); Attr(writer, "z", creature.Z);
                        Attr(writer, "hits", creature.Hits); Attr(writer, "hitsMax", creature.HitsMax); writer.WriteEndElement();
                    }
                writer.WriteEndElement();
            }
            Commit("benchmark-state.xml");
        }
    }
}
