using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Accounting;
using Server.Engines.PartySystem;
using Server.Items;
using Server.Mobiles;
using Server.Custom.Confictura;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    // Choreographed native role probes, not a resource, AI or pacing reference.
    public static class OutbreakCooperative
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "../evidence"));
        private static readonly BindingFlags Static = BindingFlags.Static | BindingFlags.NonPublic;
        private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly Type Native = typeof(OutbreakNativeAcceptance);
        private static PlayerMobile Patient, Helper;
        private static ArachnidOutbreakSpider BardEnemy;
        private static BroodAntivenom Treatment;
        private static Lute Instrument;
        private static string Phase = "waiting";
        private static DateTime Deadline, End;
        private static double Before;
        private static int InitialHits, Sequence, InitialCharges;
        private static bool HeldObserved;
        private static object Field(string name) { return Native.GetField(name, Static).GetValue(null); }
        private static void Stop()
        {
            if (Helper != null && !Helper.Deleted) Helper.Delete();
            Native.GetMethod("StopFixture", Static).Invoke(null, null);
        }
        private static ArachnidOutbreakController Controller { get { return (ArachnidOutbreakController)Field("Controller"); } }
        private static ArachnidOutbreakChampion Boss { get { return (ArachnidOutbreakChampion)Field("Boss"); } }
        private static ArachnidOutbreakLedger Ledger { get { return (ArachnidOutbreakLedger)typeof(ArachnidOutbreakController).GetField("m_Ledger", Private).GetValue(Controller); } }

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard = Path.Combine(Evidence, "native-private-root.txt");
            if (!File.Exists(guard) || !File.Exists(Path.Combine(Evidence, "cooperative-inputs.json"))
                || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)
                || ArachnidOutbreakController.DeliveryReady || File.Exists(Path.Combine(Evidence, "native-actor.txt")))
                throw new InvalidOperationException("Cooperative fixture requires its guarded synthetic-player copy.");
            End = DateTime.UtcNow.AddMinutes(20);
            Timer.DelayCall(TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(100), Tick);
        }

        private static void Setup()
        {
            Patient = (PlayerMobile)Field("Actor");
            Patient.RawStr = 100; Patient.RawDex = 100; Patient.RawInt = 100;
            Patient.Hits = Patient.HitsMax; Patient.Mana = Patient.ManaMax;
            Patient.Skills[SkillName.Magery].Base = 100;
            string[] login = File.ReadAllLines(Path.Combine(Evidence, "cooperative-account.private"));
            if (login.Length != 2 || Accounts.GetAccount(login[0]) != null) throw new InvalidOperationException("Fresh private helper account required.");
            Account account = new Account(login[0], login[1]);
            Helper = new PlayerMobile(); Helper.Player = true; Helper.Name = "private outbreak support"; Helper.Body = 0x190;
            Helper.AccessLevel = AccessLevel.Player; Helper.Blessed = false;
            Helper.RawStr = 100; Helper.RawDex = 100; Helper.RawInt = 100;
            Helper.AddItem(new Backpack()); account[0] = Helper;
            foreach (SkillName skill in new SkillName[] { SkillName.Magery, SkillName.Meditation, SkillName.Musicianship, SkillName.Peacemaking })
            { Helper.Skills[skill].Cap = 125; Helper.Skills[skill].Base = 125; }
            Helper.Hits = Helper.HitsMax; Helper.Mana = Helper.ManaMax;
            foreach (PlayerMobile mobile in new PlayerMobile[] { Patient, Helper })
            {
                mobile.Backpack.DropItem(new Spellbook(UInt64.MaxValue));
                foreach (Item reagent in new Item[] { new Garlic(100), new Ginseng(100), new MandrakeRoot(100), new SpidersSilk(100), new SulfurousAsh(100) })
                    mobile.Backpack.DropItem(reagent);
            }
            Instrument = new Lute(); Helper.Backpack.DropItem(Instrument);
            Treatment = new BroodAntivenom(); Treatment.Amount = 2; Helper.Backpack.DropItem(Treatment);
            Point3D point = Patient.Location; point.Y += 1; point.Z = Map.Sosaria.GetAverageZ(point.X, point.Y);
            if (!Map.Sosaria.CanFit(point, 16, false, false)) throw new InvalidOperationException("Helper placement blocked.");
            Helper.MoveToWorld(point, Map.Sosaria);
            Party party = new Party(Patient); Patient.Party = party; party.Add(Helper);
            File.WriteAllText(Path.Combine(Evidence, "cooperative-ready.txt"), "Synthetic trained role actors; two ordinary clients required; staged finite supplies and party; no actual Zanthus grants.");
        }

        private static int Charges()
        {
            int count;
            BroodAntivenomEffect effect = BroodAntivenomEffect.Find(Patient, out count);
            return effect == null ? 0 : effect.Charges;
        }

        private static void Begin(string name)
        {
            if (Phase != "waiting" && Phase != "complete") throw new InvalidOperationException("Previous role case is not complete.");
            if (Patient.NetState == null || Helper.NetState == null || !Patient.Player || !Helper.Player || !Patient.Alive || !Helper.Alive
                || TurnBasedCombatManager.Instance.Enabled) throw new InvalidOperationException("Two living ordinary clients and disabled TBC required.");
            if (name != "heal" && name != "cure" && name != "rescue" && name != "treat" && name != "prevent" && name != "bard")
                throw new InvalidOperationException("Unsupported role case.");
            Native.GetMethod("Reset", Static).Invoke(null, null);
            BardEnemy = null;
            Phase = name; ++Sequence; Deadline = DateTime.UtcNow.AddSeconds(name == "bard" ? 90 : 35);
            Helper.Mana = Helper.ManaMax; Patient.Mana = Patient.ManaMax; // Independent controlled cases; never measured supplies.
            Before = Ledger.Points(3, Helper); InitialHits = Patient.Hits; InitialCharges = Charges(); HeldObserved = false;
            if (name == "heal")
            {
                Boss.DoHarmful(Patient); Patient.Damage(Math.Max(1, Patient.Hits / 2), Boss);
                InitialHits = Patient.Hits;
            }
            else if (name == "cure")
            {
                if (Patient.ApplyPoison(Boss, Poison.Deadly) != ApplyPoisonResult.Poisoned) throw new InvalidOperationException("Patient did not receive actual poison.");
            }
            else if (name == "rescue")
            {
                if (!Boss.TryStartWeb(Patient)) throw new InvalidOperationException("Production web refused.");
            }
            else if (name == "prevent")
            {
                if (InitialCharges != 3) throw new InvalidOperationException("Native party treatment did not establish three charges.");
                Patient.ApplyPoison(Boss, Poison.Deadly);
            }
            else if (name == "bard")
            {
                BardEnemy = new ArachnidOutbreakSpider(Controller, 3, false); BardEnemy.Home = Boss.Location;
                Point3D point = Boss.Location; point.X += 1; point.Z = Map.Sosaria.GetAverageZ(point.X, point.Y);
                BardEnemy.MoveToWorld(point, Map.Sosaria);
                ((List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures", Private).GetValue(Controller)).Add(BardEnemy);
            }
            Patient.Combatant = null; Patient.Warmode = false;
        }

        private static void State(string path, bool passed)
        {
            using (XmlWriter writer = XmlWriter.Create(path + ".tmp", new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("cooperative");
                writer.WriteAttributeString("phase", Phase); writer.WriteAttributeString("sequence", Sequence.ToString());
                writer.WriteAttributeString("passed", passed.ToString());
                writer.WriteAttributeString("patient", Patient.Serial.Value.ToString()); writer.WriteAttributeString("helper", Helper.Serial.Value.ToString());
                writer.WriteAttributeString("patientConnected", (Patient.NetState != null).ToString()); writer.WriteAttributeString("helperConnected", (Helper.NetState != null).ToString());
                writer.WriteAttributeString("patientAlive", Patient.Alive.ToString()); writer.WriteAttributeString("helperAlive", Helper.Alive.ToString());
                writer.WriteAttributeString("patientPlayer", Patient.Player.ToString()); writer.WriteAttributeString("helperPlayer", Helper.Player.ToString());
                writer.WriteAttributeString("hits", Patient.Hits.ToString()); writer.WriteAttributeString("initialHits", InitialHits.ToString());
                writer.WriteAttributeString("poisoned", Patient.Poisoned.ToString()); writer.WriteAttributeString("charges", Charges().ToString());
                writer.WriteAttributeString("pointsBefore", Before.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteAttributeString("points", Ledger.Points(3, Helper).ToString(System.Globalization.CultureInfo.InvariantCulture));
                ArachnidWebRestraint hold = ArachnidWebRestraint.Find(Patient);
                writer.WriteAttributeString("web", (hold == null ? 0 : hold.Serial.Value).ToString());
                writer.WriteAttributeString("heldObserved", HeldObserved.ToString());
                writer.WriteAttributeString("treatment", Treatment.Serial.Value.ToString()); writer.WriteAttributeString("instrument", Instrument.Serial.Value.ToString());
                writer.WriteAttributeString("enemy", (BardEnemy == null ? 0 : BardEnemy.Serial.Value).ToString());
                writer.WriteAttributeString("enemyHits", (BardEnemy == null ? 0 : BardEnemy.Hits).ToString());
                writer.WriteAttributeString("enemyHitsMax", (BardEnemy == null ? 0 : BardEnemy.HitsMax).ToString());
                writer.WriteAttributeString("pacifiedByHelper", (BardEnemy != null && BardEnemy.BardPacified && BardEnemy.PacifyingBard == Helper).ToString());
                writer.WriteAttributeString("helperTarget", Helper.Target == null ? "none" : Helper.Target.GetType().FullName);
                writer.WriteAttributeString("helperSpell", Helper.Spell == null ? "none" : Helper.Spell.GetType().Name);
                writer.WriteAttributeString("helperSkillDelay", (Helper.NextSkillTime - TurnBasedCombatBridge.GetTime(Helper)).TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteAttributeString("scope", "Synthetic connected native roles; staged owned injury/poison/party and stopped AI; no pacing or actual-character capability claim");
                writer.WriteEndElement();
            }
            if (File.Exists(path)) File.Replace(path + ".tmp", path, null); else File.Move(path + ".tmp", path);
        }

        private static void Tick()
        {
            try
            {
                if (DateTime.UtcNow >= End) { Stop(); return; }
                if (Field("Actor") == null) return;
                if (Patient == null) Setup();
                if (BardEnemy != null && BardEnemy.AIObject != null) BardEnemy.AIObject.m_Timer.Stop();
                string command = Path.Combine(Evidence, "cooperative-command.txt");
                if (File.Exists(command))
                {
                    string name;
                    try { name = File.ReadAllText(command).Trim(); File.Delete(command); }
                    catch (IOException) { return; }
                    if (name == "stop") { Stop(); return; }
                    Begin(name);
                }
                bool passed = false;
                double delta = Ledger.Points(3, Helper) - Before;
                if (Phase == "heal") passed = delta > 0 && Patient.Hits > InitialHits;
                else if (Phase == "cure") passed = delta >= 1.99 && !Patient.Poisoned;
                else if (Phase == "rescue")
                {
                    if (ArachnidWebRestraint.Find(Patient) != null) HeldObserved = true;
                    passed = HeldObserved && delta >= 1.99 && ArachnidWebRestraint.Find(Patient) == null;
                }
                else if (Phase == "treat") passed = InitialCharges == 0 && Charges() == 3;
                else if (Phase == "prevent") passed = InitialCharges == 3 && Charges() == 2 && !Patient.Poisoned && delta >= 1.99;
                else if (Phase == "bard")
                {
                    // The guarded scene does not start the production director;
                    // sample its real observer on the normal wall clock instead.
                    typeof(ArachnidOutbreakController).GetMethod("ObserveBards", Private).Invoke(Controller, new object[] { DateTime.UtcNow, true });
                    passed = Ledger.Points(3, Helper) - Before >= 0.5 && BardEnemy.BardPacified && BardEnemy.PacifyingBard == Helper;
                }
                if (Phase != "waiting" && Phase != "complete" && (DateTime.UtcNow > Deadline || !Patient.Alive || !Helper.Alive))
                    throw new InvalidOperationException("Role case failed/deadline: " + Phase);
                State(Path.Combine(Evidence, "cooperative-state.xml"), passed);
                if (passed)
                {
                    State(Path.Combine(Evidence, "cooperative-result-" + Sequence + ".xml"), true);
                    Phase = "complete";
                }
            }
            catch (Exception error)
            {
                File.WriteAllText(Path.Combine(Evidence, "cooperative-error.txt"), error.ToString());
                Stop();
            }
        }
    }
}
