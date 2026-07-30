using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Misc;
using Server.Targeting;

namespace Server.Custom.Confictura
{
    public class CharacterLevelCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "CharLevel",
                AccessLevel.GameMaster,
                new CommandEventHandler(CharLevel_OnCommand)
            );

            CommandSystem.Register(
                "CharLevelTarget",
                AccessLevel.GameMaster,
                new CommandEventHandler(CharLevelTarget_OnCommand)
            );
        }

        [Usage("CharLevel")]
        [Description("Shows the canonical character level diagnostics for yourself.")]
        public static void CharLevel_OnCommand(CommandEventArgs e)
        {
            SendDiagnostics(e.Mobile, e.Mobile);
        }

        [Usage("CharLevelTarget")]
        [Description("Shows canonical character level diagnostics for a targeted mobile.")]
        public static void CharLevelTarget_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target a mobile to inspect character level diagnostics.");
            e.Mobile.Target = new CharacterLevelTarget();
        }

        private static void SendDiagnostics(Mobile from, Mobile target)
        {
            if (from == null)
                return;

            if (target == null)
            {
                from.SendMessage("That is not a mobile.");
                return;
            }

            CharacterLevelDiagnostics diagnostics = CharacterLevelService.GetDiagnostics(target);

            from.SendMessage(
                "Level diagnostics for {0}: overall {1}; best {2} {3}.",
                target.Name,
                diagnostics.OverallLevel,
                diagnostics.BestArchetype,
                diagnostics.BestArchetypeLevel
            );

            from.SendMessage(
                "Caps: skills {0}, stats {1}; start {2}, boost {3}, ether {4}; race {5}, profession {6}, guild {7}.",
                diagnostics.SkillCap,
                diagnostics.StatCap,
                diagnostics.SkillStart,
                diagnostics.SkillBoost,
                diagnostics.SkillEther,
                diagnostics.RaceID,
                diagnostics.Profession,
                diagnostics.NpcGuild
            );

            from.SendMessage(
                "Progress: skills {0:0.00%}, stats {1:0.00%}, reputation {2:0.00%}.",
                diagnostics.SkillProgress,
                diagnostics.StatProgress,
                diagnostics.ReputationProgress
            );

            foreach (KeyValuePair<LevelType, int> entry in diagnostics.EncounterAliasLevels)
                from.SendMessage("Encounter {0}: {1}", entry.Key, entry.Value);

            from.SendMessage("Archetypes above level 10:");

            foreach (KeyValuePair<CharacterArchetype, int> entry in diagnostics.ArchetypeLevels)
            {
                if (entry.Key != CharacterArchetype.Overall && entry.Value > 10)
                    from.SendMessage("{0}: {1}", entry.Key, entry.Value);
            }
        }

        private class CharacterLevelTarget : Target
        {
            public CharacterLevelTarget()
                : base(12, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                SendDiagnostics(from, targeted as Mobile);
            }
        }
    }
}
