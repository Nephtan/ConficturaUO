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
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4293; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(newname, access, e.Handler);}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4293 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4482; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4482 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4487; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4487 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4492; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4492 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4497; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4497 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4502; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4502 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4507; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4507 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4512; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4512 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4518; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4518 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4523; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4523 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4530; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4530 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4535; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4535 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4541; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4541 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4546; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4546 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4551; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4551 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4556; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4556 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4561; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4561 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4566; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4566 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4571; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4571 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4576; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4576 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4581; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4581 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4586; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4586 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4591; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4591 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4596; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4596 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4601; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4601 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4606; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4606 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4611; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4611 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4618; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4618 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4623; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4623 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4628; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4628 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4636; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4636 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4641; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4641 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4646; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4646 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=194; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:194 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs; Line=204; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs:204 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttach/Gumps/XmlGetAttachGump.cs; Line=65; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttach/Gumps/XmlGetAttachGump.cs:65 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs; Line=2040; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs:2040 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs; Line=2045; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlDialog.cs:2045 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=772; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:772 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=777; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:777 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=782; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:782 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=787; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:787 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=792; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:792 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=797; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:797 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=802; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:802 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=807; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:807 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=812; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:812 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=817; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:817 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=822; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:822 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=827; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:827 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=832; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:832 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=837; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:837 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=842; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:842 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=847; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:847 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=852; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:852 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=857; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:857 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs; Line=862; LikelySystem=Custom:XMLSpawner; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs:862 |
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
| Command $(Escape-MarkdownCell @{Command=XmlSet; AccessLevel=GameMaster; Handler=XmlSetValue_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4617; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "XmlSet", AccessLevel.GameMaster, new CommandEventHandler( XmlSetValue_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=XmlSet; AccessLevel=GameMaster; Handler=XmlSetValue_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4617; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "XmlSet", AccessLevel.GameMaster, new CommandEventHandler( XmlSetValue_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4617 |
| Command $(Escape-MarkdownCell @{Command=TagList; AccessLevel=Administrator; Handler=ShowTagList_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4633; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "TagList", AccessLevel.Administrator, new CommandEventHandler( ShowTagList_OnCommand ) );}.Command) ($(Escape-MarkdownCell @{Command=TagList; AccessLevel=Administrator; Handler=ShowTagList_OnCommand; File=Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs; Line=4633; LikelySystem=Custom:XMLSpawner; RegistrationLine=//CommandSystem.Register( "TagList", AccessLevel.Administrator, new CommandEventHandler( ShowTagList_OnCommand ) );}.AccessLevel)) | Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4633 |
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

No matching wiki page was found by Phase 4 doc-name pattern matching.

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
