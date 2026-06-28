# System: XMLSpawner

## Classification

SharedService

## Summary

Shared staff/event infrastructure for spawners, attachments, quests, points, packet hooks, speech, movement, and persistence.

## Source Files

Matched source files: 133.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs | StartupWiring | CustomTimerSubclass;Initialize | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/ItemFlags.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Custom/XMLSpawner/PacketHandlerOverrides.cs | PacketNetwork | EventSink;Initialize;PacketHandlers.Register;Timer.DelayCall | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs | CommandSurface | CustomTimerSubclass;Initialize;OnMovement;OnSpeech;Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlSpawnerSkillCheck.cs | StaffTooling |  | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlTextEntryBook.cs | PacketNetwork | Initialize;PacketHandlers.Register | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Gumps/XmlSpawnerGumps.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/Wands/BaseWand.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/ClumsyWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/FeebleWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/FireballWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/GreaterHealWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/HarmWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/HealWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/IDWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/LightningWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/MagicArrowWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/ManaDrainWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/Wands/RandomWand.cs | StaffTooling |  | No |  |
| Data/Scripts/Custom/XMLSpawner/Wands/WandTarget.cs | StaffTooling |  | No |  |
| Data/Scripts/Custom/XMLSpawner/Wands/WeaknessWand.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs | CommandSurface | EventSink;Initialize;OnMovement;OnSpeech;WorldLoad;WorldSave | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttachment.cs | StartupWiring | CustomTimerSubclass;Initialize;OnMovement;OnSpeech;Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttach/Gumps/XmlGetAttachGump.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/TemporaryQuestObject.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlAddFame.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlAddKarma.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlAddTithing.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlAddVirtue.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlData.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDate.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDeathAction.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDex.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs | CommandSurface | CustomTimerSubclass;OnMovement;OnSpeech;Timer.DelayCall | Yes | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlEnemyMastery.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlFire.cs | Persistence | OnMovement | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlFreeze.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlHue.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlInt.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlLifeDrain.cs | Persistence | OnMovement | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlLightning.cs | Persistence | OnMovement | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlLocalVariable.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlMagicWord.cs | StartupWiring | OnSpeech;Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlManaDrain.cs | Persistence | OnMovement | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlMessage.cs | Persistence | OnMovement;OnSpeech | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlMinionStrike.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlMorph.cs | StartupWiring | CustomTimerSubclass;OnMovement;OnSpeech | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs | CommandSurface | CustomTimerSubclass;EventSink;Timer.DelayCall | Yes | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlSaveItem.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlSkill.cs | StartupWiring | OnSpeech;Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlSound.cs | Persistence | OnMovement;OnSpeech | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlStamDrain.cs | Persistence | OnMovement | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlStr.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlUse.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlValue.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlWeaponAbility.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlItems/QuestHolder.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlItems/QuestNote.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlItems/SimpleMap.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlItems/SimpleNote.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlItems/SimpleSwitches.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlItems/SimpleTileTrap.cs | Persistence | OnMovement | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlItems/SingleUseSwitch.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlItems/TimedSwitches.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlItems/XmlQuestMaker.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlItems/XmlSpawnerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlMobiles/TalkingBaseCreature.cs | StartupWiring | Initialize | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlMobiles/TalkingBaseVendor.cs | StartupWiring | Initialize | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlMobiles/TalkingDrake.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlMobiles/TalkingJeweler.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlMobiles/XmlQuestNPC.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/BaseChallengeGame.cs | StartupWiring | CustomTimerSubclass;Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGameRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeRegionStone.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/EnglishText.cs | StartupWiring | Initialize | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/LBSStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/PointsRewardStone.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/PortugueseText.cs | StartupWiring | Initialize | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/SpanishText.cs | StartupWiring | Initialize | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/topplayersboard.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/XmlPointsRewards.cs | StartupWiring | Initialize | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/CTF.cs | StartupWiring | OnMovement;Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/DeathBallGauntlet.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/DeathmatchGauntlet.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/KingOfTheHillGauntlet.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/LastManStandingGauntlet.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/TeamDeathballGauntlet.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/TeamDeathmatchGauntlet.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/TeamKotHGauntlet.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/TeamLMSGauntlet.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/CTFGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/DeathBallGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/DeathmatchGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/KingOfTheHillGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/LastManStandingGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/TeamDeathballGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/TeamDeathmatchGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/TeamKotHGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/Gumps/TeamLMSGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/Gumps/PointsRewardGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlPropsGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlSetCustomEnumGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlSetGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlSetListOptionGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlSetObjectGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlSetObjectTarget.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlSetPoint2DGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlSetPoint3DGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlPropsGumps/XmlSetTimeSpanGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/QuestLeadersBoard.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/QuestLeadersStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/QuestRewardStone.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuest.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestAttachment.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestBook.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestHolder.cs | StartupWiring | Initialize;Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestLeaders.cs | CommandSurface | CustomTimerSubclass;Initialize | Yes | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestPoints.cs | CommandSurface |  | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestPointsRewards.cs | StartupWiring | Initialize | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestToken.cs | StartupWiring | Initialize;Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/Gumps/QuestLogGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/Gumps/QuestRewardGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/Gumps/XmlPlayerQuestGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/Gumps/XmlQuestBookGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/Gumps/XmlQuestGumps.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/XMLSpawner/XmlQuest/Gumps/XmlQuestHolderGumps.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlUtils/WhatIsIt.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlUtils/WriteMulti.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlAdd.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlEdit.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlFind.cs | CommandSurface | Initialize;Timer.DelayCall | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlUtils/Gumps/XmlCategorizedAddGump.cs | StartupWiring | Timer.DelayCall | No | OnResponse;SendGump |
| Data/Scripts/Custom/XMLSpawner/XmlUtils/Gumps/XmlPartialCategorizedAddGump.cs | StartupWiring | Timer.DelayCall | No | OnResponse;SendGump |

## Data Files

*.xml;Data/objects.xml;Data/xmlspawner.cfg

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/ItemFlags.cs; Line=17; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/ItemFlags.cs:17 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/ItemFlags.cs; Line=22; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/ItemFlags.cs:22 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4302; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(newname, access, e.Handler);}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4302 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4491; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4491 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4496; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4496 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4501; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4501 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4506; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4506 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4511; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4511 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4516; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4516 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4521; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4521 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4527; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4527 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4532; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4532 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4539; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4539 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4544; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4544 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4550; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4550 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4555; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4555 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4560; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4560 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4565; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4565 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4570; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4570 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4575; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4575 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4580; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4580 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4585; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4585 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4590; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4590 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4595; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4595 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4600; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4600 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4605; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4605 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4610; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4610 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4615; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4615 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4620; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4620 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4627; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4627 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4632; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4632 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4637; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4637 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4645; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4645 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4650; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4650 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4655; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4655 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=194; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:194 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=204; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:204 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/Gumps/XmlGetAttachGump.cs; Line=65; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttach/Gumps/XmlGetAttachGump.cs:65 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs; Line=2040; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs:2040 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs; Line=2045; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs:2045 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=784; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:784 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=789; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:789 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=794; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:794 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=799; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:799 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=804; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:804 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=809; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:809 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=814; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:814 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=819; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:819 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=824; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:824 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=829; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:829 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=834; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:834 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=839; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:839 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=844; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:844 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=849; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:849 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=854; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:854 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=859; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:859 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=864; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:864 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=869; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:869 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=874; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:874 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestLeaders.cs; Line=160; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestLeaders.cs:160 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestLeaders.cs; Line=165; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestLeaders.cs:165 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestPoints.cs; Line=180; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestPoints.cs:180 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestPoints.cs; Line=186; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlQuest/XmlQuestPoints.cs:186 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlUtils/WhatIsIt.cs; Line=14; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlUtils/WhatIsIt.cs:14 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlUtils/WriteMulti.cs; Line=36; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlUtils/WriteMulti.cs:36 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlAdd.cs; Line=761; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlAdd.cs:761 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlEdit.cs; Line=52; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlEdit.cs:52 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlFind.cs; Line=245; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlUtils/XmlFind.cs:245 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=XmlSet; AccessLevel=GameMaster; Handler=XmlSetValue_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4626; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "XmlSet", AccessLevel.GameMaster, new CommandEventHandler( XmlSetValue_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=XmlSet; AccessLevel=GameMaster; Handler=XmlSetValue_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4626; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "XmlSet", AccessLevel.GameMaster, new CommandEventHandler( XmlSetValue_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4626 |
| Command $(Escape-MarkdownCell @{Command=TagList; AccessLevel=Administrator; Handler=ShowTagList_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4642; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "TagList", AccessLevel.Administrator, new CommandEventHandler( ShowTagList_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=TagList; AccessLevel=Administrator; Handler=ShowTagList_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4642; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "TagList", AccessLevel.Administrator, new CommandEventHandler( ShowTagList_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4642 |
| Command $(Escape-MarkdownCell @{Command=ItemAtt; AccessLevel=GameMaster; Handler=ListItemAttachments_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=192; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "ItemAtt", AccessLevel.GameMaster, new CommandEventHandler( ListItemAttachments_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=ItemAtt; AccessLevel=GameMaster; Handler=ListItemAttachments_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=192; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "ItemAtt", AccessLevel.GameMaster, new CommandEventHandler( ListItemAttachments_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:192 |
| Command $(Escape-MarkdownCell @{Command=MobAtt; AccessLevel=GameMaster; Handler=ListMobileAttachments_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=193; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "MobAtt", AccessLevel.GameMaster, new CommandEventHandler( ListMobileAttachments_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=MobAtt; AccessLevel=GameMaster; Handler=ListMobileAttachments_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=193; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "MobAtt", AccessLevel.GameMaster, new CommandEventHandler( ListMobileAttachments_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:193 |
| Command $(Escape-MarkdownCell @{Command=DelAtt; AccessLevel=GameMaster; Handler=DeleteAttachments_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=199; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "DelAtt", AccessLevel.GameMaster, new CommandEventHandler( DeleteAttachments_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=DelAtt; AccessLevel=GameMaster; Handler=DeleteAttachments_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=199; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "DelAtt", AccessLevel.GameMaster, new CommandEventHandler( DeleteAttachments_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:199 |
| Command $(Escape-MarkdownCell @{Command=TrigAtt; AccessLevel=GameMaster; Handler=ActivateAttachments_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=200; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "TrigAtt", AccessLevel.GameMaster, new CommandEventHandler( ActivateAttachments_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=TrigAtt; AccessLevel=GameMaster; Handler=ActivateAttachments_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=200; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "TrigAtt", AccessLevel.GameMaster, new CommandEventHandler( ActivateAttachments_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:200 |
| Command $(Escape-MarkdownCell @{Command=AddAtt; AccessLevel=GameMaster; Handler=AddAttachment_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=201; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "AddAtt", AccessLevel.GameMaster, new CommandEventHandler( AddAttachment_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=AddAtt; AccessLevel=GameMaster; Handler=AddAttachment_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=201; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "AddAtt", AccessLevel.GameMaster, new CommandEventHandler( AddAttachment_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:201 |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;EventSink;Timer.DelayCall;CustomTimerSubclass;Initialize;CustomTimerSubclass;Initialize;OnMovement;OnSpeech;Timer.DelayCall;CustomTimerSubclass;OnMovement;OnSpeech;CustomTimerSubclass;OnMovement;OnSpeech;Timer.DelayCall;CustomTimerSubclass;Timer.DelayCall;EventSink;Initialize;OnMovement;OnSpeech;WorldLoad;WorldSave;EventSink;Initialize;PacketHandlers.Register;Timer.DelayCall;Initialize;Initialize;PacketHandlers.Register;Initialize;Timer.DelayCall;OnMovement;OnMovement;OnSpeech;OnMovement;Timer.DelayCall;OnSpeech;Timer.DelayCall;RegionOverride;Timer.DelayCall

## Serialized State

Serialized marker files: 84. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Attachments; quests; packet handlers; speech/movement hooks; staff commands

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/codebase-audit/outputs/system-cards/xmlspawner.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
