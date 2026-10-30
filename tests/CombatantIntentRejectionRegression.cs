using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Mobiles;
using Server.Misc;
using Server.Custom.Confictura;

internal static class CombatantIntentRejectionRegression
{
    private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception("FAILED: " + message);
        Console.WriteLine("PASS " + message);
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Mobiles", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Item>());
            MapDefinitions.Configure();
            RaceDefinitions.Configure();
            TurnBasedCombatManager manager = (TurnBasedCombatManager)Activator.CreateInstance(typeof(TurnBasedCombatManager), true);
            manager.Configuration.LogEnabled = false;
            typeof(TurnBasedCombatManager).GetField("m_RuntimeEnabled", Private).SetValue(manager, true);
            Mobile outsider = new Mobile();
            outsider.Body = 0x190; outsider.RawStr = 100; outsider.Hits = 100;
            outsider.MoveToWorld(new Point3D(3000, 2500, 0), Map.Sosaria);
            PlayerMobile target = new PlayerMobile();
            target.Body = 0x190; target.RawStr = 100; target.Hits = 100;
            target.MoveToWorld(new Point3D(2514, 2088, 0), Map.Sosaria);
            TurnCombatGroup group = new TurnCombatGroup();
            group.ID = 1;
            TurnParticipant participant = new TurnParticipant();
            participant.Mobile = target; participant.Group = group; participant.ActionPoints = 20;
            group.Participants.Add(participant); group.Current = participant;
            ((Dictionary<Mobile, TurnParticipant>)typeof(TurnBasedCombatManager).GetField("m_Participants", Private).GetValue(manager)).Add(target, participant);
            ((List<TurnCombatGroup>)typeof(TurnBasedCombatManager).GetField("m_Groups", Private).GetValue(manager)).Add(group);
            TurnCombatantChangeRequest request = new TurnCombatantChangeRequest(outsider, null, target);
            bool failed = false;
            TurnCombatantChangeMode mode = TurnCombatantChangeMode.Native;
            try { mode = manager.DecideCombatantChange(request); }
            catch (NullReferenceException) { failed = true; }
            if (args.Length > 1 && args[1] == "--expect-fault")
            {
                Check(failed, "old actual-manager outsider rejection reproduces null dereference");
                return 0;
            }
            Check(!failed && mode == TurnCombatantChangeMode.Reject, "nonparticipant outside join range is rejected without throwing");
            Check(TurnBasedCombatBridge.Register(manager), "real core bridge registers actual manager");
            Check(TurnBasedCombatBridge.DecideCombatantChange(request) == TurnCombatantChangeMode.Reject
                && !TurnBasedCombatBridge.IsFaulted && manager.Enabled, "rejection leaves bridge and manager healthy");
            Check(manager.GroupCount == 1 && group.Current == participant && participant.ActionPoints == 20
                && manager.GetParticipant(outsider) == null, "rejection preserves existing group/AP and does not enroll outsider");
            Check(manager.DecideCombatantChange(new TurnCombatantChangeRequest(null, null, target)) == TurnCombatantChangeMode.Reject,
                "missing actor remains rejected");
            Check(manager.DecideCombatantChange(new TurnCombatantChangeRequest(target, outsider, null)) == TurnCombatantChangeMode.Native,
                "clearing a combatant retains native behavior");
            typeof(TurnBasedCombatManager).GetField("m_RuntimeEnabled", Private).SetValue(manager, false);
            Check(manager.DecideCombatantChange(request) == TurnCombatantChangeMode.Native, "disabled real-time path remains native");
            Console.WriteLine("PASS actual-manager combatant rejection; synthetic roster, no world saves/listeners or native gameplay claim");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
