using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Custom.Confictura;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    public static class OutbreakPresentation
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "../evidence"));
        private static readonly BindingFlags Static = BindingFlags.Static | BindingFlags.NonPublic;
        private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly Type Native = typeof(OutbreakNativeAcceptance);
        private static DateTime End;
        private static string CurrentSite = "waiting";
        private static int CurrentAnchor = -1;

        private sealed class ProposedTerrain : IArachnidOutbreakTerrain
        {
            private readonly SosariaOutbreakTerrain m_Terrain = new SosariaOutbreakTerrain();
            public bool HasPermanentHousingProhibition(Rectangle2D bounds) { return true; }
            public bool IsReserved(Rectangle2D bounds) { return m_Terrain.IsReserved(bounds); }
            public bool TryGetPoint(int x, int y, out Point3D point) { return m_Terrain.TryGetPoint(x, y, out point); }
        }

        private static object Field(string name) { return Native.GetField(name, Static).GetValue(null); }
        private static void Field(string name, object value) { Native.GetField(name, Static).SetValue(null, value); }
        private static void Stop() { Native.GetMethod("StopFixture", Static).Invoke(null, null); }

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            if (!File.Exists(Path.Combine(Evidence, "presentation-inputs.json"))
                || !File.Exists(Path.Combine(Evidence, "native-private-root.txt"))
                || !String.Equals(Path.GetFullPath(File.ReadAllText(Path.Combine(Evidence, "native-private-root.txt"))).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)
                || ArachnidOutbreakController.DeliveryReady)
                throw new InvalidOperationException("Presentation fixture requires its prepared private disabled-delivery root.");
            End = DateTime.UtcNow.AddMinutes(20);
            Timer.DelayCall(TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(250), Tick);
        }

        private static void Pose(int index, int anchor)
        {
            PlayerMobile actor = Field("Actor") as PlayerMobile;
            if (actor == null || actor.NetState == null || !actor.Alive || actor.Blessed || actor.AccessLevel != AccessLevel.Player)
                throw new InvalidOperationException("Presentation requires the connected unchanged living Player.");
            TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;
            manager.Disable("private presentation relocation");
            ArachnidOutbreakController old = Field("Controller") as ArachnidOutbreakController;
            if (old != null) old.Delete();
            WildernessEncounterReserve reserve = WildernessEncounterReserves.Proposals[index];
            // The base acceptance fixture replaces the catalog with its private scene.
            ArachnidOutbreakSite site = new ArachnidOutbreakSite(reserve.Id, reserve.Center.X, reserve.Center.Y, reserve.Reviewed);
            ArachnidOutbreakPlacement placement;
            string reason;
            IArachnidOutbreakTerrain validationTerrain = reserve.Reviewed ? (IArachnidOutbreakTerrain)new SosariaOutbreakTerrain() : new ProposedTerrain();
            if (!site.Validate(validationTerrain, out placement, out reason))
                throw new InvalidOperationException("Selected snapshot layout is invalid: " + reason);
            typeof(ArachnidOutbreakSites).GetField("m_Candidates", Static).SetValue(null, Array.AsReadOnly(new ArachnidOutbreakSite[] { site }));
            ArachnidOutbreakController controller = new ArachnidOutbreakController();
            ArachnidOutbreakState state = (ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State", Private).GetValue(controller);
            state.SetEnabled(true);
            typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(controller, new object[] { site, placement });
            List<Mobile> creatures = (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures", Private).GetValue(controller);
            foreach (Mobile mobile in creatures.ToArray()) mobile.Delete();
            creatures.Clear();
            for (int stage = 0; stage < 3; ++stage) state.CompleteBrood(stage, DateTime.UtcNow);
            typeof(ArachnidOutbreakController).GetField("m_BossCreated", Private).SetValue(controller, true);
            Point3D point = anchor < 0 ? placement.Boss : placement.Brood(anchor);
            Point3D camera = point;
            bool cameraFound = false;
            SosariaOutbreakTerrain terrain = new SosariaOutbreakTerrain();
            int[] dx = new int[] { 2, 0, -2, 0 };
            int[] dy = new int[] { 0, 2, 0, -2 };
            for (int candidate = 0; candidate < 4 && !cameraFound; ++candidate)
                cameraFound = terrain.TryGetPoint(point.X + dx[candidate], point.Y + dy[candidate], out camera);
            if (!cameraFound) throw new InvalidOperationException("No legal nearby presentation tile.");
            actor.Hidden = false;
            actor.Combatant = null;
            actor.Warmode = false;
            actor.MoveToWorld(camera, Map.Sosaria);
            ArachnidOutbreakChampion boss = new ArachnidOutbreakChampion(controller);
            boss.Home = point;
            boss.MoveToWorld(point, Map.Sosaria);
            creatures.Add(boss);
            if (boss.AIObject != null && boss.AIObject.m_Timer != null) boss.AIObject.m_Timer.Stop();
            Field("Controller", controller);
            Field("Boss", boss);
            Field("Subject", actor);
            Field("LiveTurns", false);
            Field("Manager", manager);
            string enableReason;
            if (!manager.TryEnable(out enableReason)) throw new InvalidOperationException(enableReason);
            ((Timer)typeof(TurnBasedCombatManager).GetField("m_Scheduler", Private).GetValue(manager)).Stop();
            manager.ArmTestMobile(boss);
            TurnActionDecision opening = TurnBasedCombatBridge.BeginAction(new TurnActionRequest(actor, boss, controller, TurnActionKind.ItemUse, 4, true, false));
            if (opening.Lease != null) TurnBasedCombatBridge.CompleteAction(opening.Lease, new TurnActionResult(actor, boss, TurnActionPhase.Refund, false));
            TurnParticipant participant = manager.GetParticipant(boss);
            if (participant == null) throw new InvalidOperationException("Presentation roster did not enroll.");
            for (int count = 0; count < 8 && participant.Group.Current != participant; ++count)
                manager.EndTurn(participant.Group.Current.Mobile, false);
            if (participant.Group.Current != participant || !boss.TryStartWeb(actor))
                throw new InvalidOperationException("Presentation warning was refused.");
            CurrentSite = reserve.Id;
            CurrentAnchor = anchor;
            File.AppendAllText(Path.Combine(Evidence, "presentation-events.txt"), DateTime.UtcNow.ToString("o") + " staged " + index + " " + anchor + " " + reserve.Id + "\n");
        }

        private static void Tick()
        {
            try
            {
                if (DateTime.UtcNow >= End) { Stop(); return; }
                if (Field("Actor") == null) return;
                string request = Path.Combine(Evidence, "presentation-command.txt");
                if (File.Exists(request))
                {
                    string command;
                    try { command = File.ReadAllText(request).Trim(); File.Delete(request); }
                    catch (IOException) { return; }
                    if (command == "stop") { Stop(); return; }
                    string[] fields = command.Split(' ');
                    int index, anchor;
                    if (fields.Length != 2 || !Int32.TryParse(fields[0], out index) || !Int32.TryParse(fields[1], out anchor)
                        || index < 0 || index >= 6 || anchor < -1 || anchor > 2)
                        throw new InvalidOperationException("Invalid presentation location request.");
                    Pose(index, anchor);
                }
                PlayerMobile actor = (PlayerMobile)Field("Actor");
                int warning = 0;
                IPooledEnumerable items = actor.GetItemsInRange(1);
                try
                {
                    foreach (Item item in items)
                    {
                        ArachnidWebWarning web = item as ArachnidWebWarning;
                        if (web != null && !web.Deleted && web.Victim == actor) warning = web.Serial.Value;
                    }
                }
                finally { items.Free(); }
                string path = Path.Combine(Evidence, "presentation-state.xml");
                using (XmlWriter writer = XmlWriter.Create(path + ".tmp", new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartElement("presentation");
                    writer.WriteAttributeString("site", CurrentSite);
                    writer.WriteAttributeString("anchor", CurrentAnchor.ToString());
                    writer.WriteAttributeString("warning", warning.ToString());
                    writer.WriteAttributeString("alive", actor.Alive.ToString());
                    writer.WriteAttributeString("connected", (actor.NetState != null).ToString());
                    writer.WriteAttributeString("scope", "Staged relocation; AI and turn scheduler stopped; rendering only");
                    writer.WriteEndElement();
                }
                if (File.Exists(path)) File.Replace(path + ".tmp", path, null); else File.Move(path + ".tmp", path);
            }
            catch (Exception error)
            {
                File.WriteAllText(Path.Combine(Evidence, "presentation-error.txt"), error.ToString());
                Stop();
            }
        }
    }
}
