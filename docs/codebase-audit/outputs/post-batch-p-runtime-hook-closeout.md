# POST-BATCH-P Runtime Hook Source Review Closeout

Generated: 2026-06-14T15:28:15.5263451-05:00

## Scope

POST-BATCH-P processed all 103 `ReadyForSourceBatch` rows from `docs/codebase-audit/outputs/post-batch-n-source-readiness-queue.csv` where `ImplementationLane=POST-BATCH-N-RUNTIME-HOOK-SOURCE-BATCH`.

Source edits were limited to confirmed runtime-hook guard defects found during direct source review. The batch preserved command names, public APIs, namespaces, type names, file locations, project files, XML/config, serialization layout, and existing player/staff workflow semantics.

## Disposition Summary

FalsePositive=8; Fixed=80; SafeNoChange=15

## Review Class Summary

AmbientMovementGuardFixed=10; EmptyRuntimeHookFalsePositive=3; ExistingEventGuardSafeNoChange=1; ExistingMovementGuardSafeNoChange=1; FrameworkCallbackReviewedNoChange=3; LegacyHookGuardFixed=2; MovementHookGuardFixed=31; NoSideEffectCallbackFalsePositive=2; PriorGuardOrExistingGuardSafeNoChange=12; RegionHookGuardFixed=4; SendGumpCallbackGuardFixed=17; ServerBootstrapReviewedNoChange=1; SpeechHookGuardFixed=8; SpellCallbackGuardFixed=1; StalePlayerStateGuardFixed=4; TargetCallbackGuardFixed=3

## Fixed Source Files

- `Data/Scripts/Custom/Government System/Commands/AdminAdd.cs`
- `Data/Scripts/Custom/Government System/Commands/FindCities.cs`
- `Data/Scripts/Custom/Government System/Commands/GovHelp.cs`
- `Data/Scripts/Custom/Government System/Items/Stones/CityBankStone.cs`
- `Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Morgan/Morgan2.cs`
- `Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Dracana's Winecrafting [2.0]/Commands/AddGrapevines.cs`
- `Data/Scripts/Custom/XMLSpawner/XmlItems/SimpleTileTrap.cs`
- `Data/Scripts/Items/Books/BulletinBoards/RulesBoard.cs`
- `Data/Scripts/Items/Books/BulletinBoards/StealingBoard.cs`
- `Data/Scripts/Items/Containers/HiddenChest.cs`
- `Data/Scripts/Items/Doors/KeywordDoor.cs`
- `Data/Scripts/Items/Gifts/Holiday/Halloween/EerieGhost.cs`
- `Data/Scripts/Items/Houses/Monopoly/Items/TownHouse.cs`
- `Data/Scripts/Items/Houses/Remodeling/ContextMenuEntries.cs`
- `Data/Scripts/Items/Magical/Gifts/Gifts.cs`
- `Data/Scripts/Items/Magical/God/LevelInfoEntry.cs`
- `Data/Scripts/Items/Misc/Games/LiarsDice/DiceChannel.cs`
- `Data/Scripts/Items/Misc/LifeFountain.cs`
- `Data/Scripts/Items/Misc/MagicForges.cs`
- `Data/Scripts/Items/Misc/MorphItem.cs`
- `Data/Scripts/Items/Special/Evil Home Decor Collection/HauntedMirror.cs`
- `Data/Scripts/Items/Traps/FlameSpurtTrap.cs`
- `Data/Scripts/Items/Traps/HiddenTrap.cs`
- `Data/Scripts/Magic/Elementalism/ElementalEffect.cs`
- `Data/Scripts/Magic/Elementalism/ElementalShrine.cs`
- `Data/Scripts/Magic/Magery/Magery 7th/Polymorph.cs`
- `Data/Scripts/Magic/Research/Spells/Theurgy/ResearchDivination.cs`
- `Data/Scripts/Mobiles/Civilized/TimeLord.cs`
- `Data/Scripts/Mobiles/Humanoids/Elves/ElfRogue.cs`
- `Data/Scripts/Mobiles/Humanoids/Humans/Rogue.cs`
- `Data/Scripts/Mobiles/Humanoids/Orcs/OrkRogue.cs`
- `Data/Scripts/Mobiles/Insects/Spiders/PhaseSpider.cs`
- `Data/Scripts/Mobiles/Insects/Spiders/ShadowRecluse.cs`
- `Data/Scripts/Mobiles/Mystical/Pixie.cs`
- `Data/Scripts/Mobiles/Mystical/Sprite.cs`
- `Data/Scripts/Quests/Frankenstein/PowerCoil.cs`
- `Data/Scripts/Quests/Search/SearchBoard.cs`
- `Data/Scripts/Quests/TriggerTile.cs`
- `Data/Scripts/System/Misc/Environment.cs`
- `Data/Scripts/System/Misc/InhumanSpeech.cs`
- `Data/Scripts/System/Obsolete/Obsolete.cs`
- `Data/Scripts/System/Regions/HouseRegion.cs`
- `Data/Scripts/System/Regions/PirateRegion.cs`
- `Data/Scripts/System/Skills/Weapon Abilities/SpecialAttacksDisplay.cs`

## No-Change Rows

- EmptyRuntimeHookFalsePositive: 3
- ExistingEventGuardSafeNoChange: 1
- ExistingMovementGuardSafeNoChange: 1
- FrameworkCallbackReviewedNoChange: 3
- NoSideEffectCallbackFalsePositive: 2
- PriorGuardOrExistingGuardSafeNoChange: 12
- ServerBootstrapReviewedNoChange: 1

No-change rows were retained when the scoped callback was empty, framework/base-only, already guarded by current source, or already covered by the prior POST-BATCH-O gump guard batch. `XmlSpawner2.OnMovement` remains safe through `ValidPlayerTrig`; a same-file speech argument guard was added during direct runtime-hook review.

## Same-File Companion Guard

- `Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs`: the queued `OnMovement` row is `SafeNoChange` because `ValidPlayerTrig` already rejects invalid movers; `OnSpeech` received a null speech-args/string guard while reviewing the same runtime-hook surface.

## Queue Reconciliation

- Review CSV rows: 103
- Active overlay `POST-BATCH-P` rows: 103
- Remaining active `POST-BATCH-N-RUNTIME-HOOK-SOURCE-BATCH` rows: 0
- Active `QueuedSourceFollowUp` rows: 0
- Active overlay `POST-BATCH-N` rows after replacement: 561
- Active overlay `POST-BATCH-O` rows preserved: 235

## Verification

- Targeted hook/static scans covered edited speech, movement, region, command/send-gump, target, spell, timer/status, and framework no-change paths.
- `git diff --check` passed with expected LF-to-CRLF working-copy warnings only.
- `msbuild` was not on PATH; direct Visual Studio MSBuild was used. The first sandboxed Visual Studio MSBuild run hit user-profile SDK access denial, and the final escalated `Data/System/Source/Server.csproj` `Debug|x86` build passed with 0 warnings and 0 errors.
- The first `.\ConficturaServer.exe -compileonly -nocache` run exposed a `KeywordDoor.InternalTimer` constructor mismatch introduced during this batch; the source was corrected.
- Final `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`

## Outputs

- `docs/codebase-audit/outputs/post-batch-p-runtime-hook-source-review.csv`
- `docs/codebase-audit/outputs/post-batch-p-runtime-hook-closeout.md`
- `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
