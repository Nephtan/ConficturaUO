# POST-BATCH-O Gump Guard Source Repair Closeout

Generated: 2026-06-14T14:40:06.0684849-05:00

## Scope

- Input: `docs/codebase-audit/outputs/post-batch-n-source-readiness-queue.csv`
- Filter: `ReadinessStatus=ReadyForSourceBatch`; `ImplementationLane=POST-BATCH-N-GUMP-GUARD-SOURCE-BATCH`
- Scoped rows reviewed: 235
- Review output: `docs/codebase-audit/outputs/post-batch-o-gump-guard-source-review.csv`
- Active overlay: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

## Disposition Summary

- FalsePositive=7; Fixed=84; ReviewedNoChange=4; SafeNoChange=140

## Review Class Summary

- SendGumpOpenSiteReviewed=140; GuardedCallbackFixed=84; BaseOrNoOpCallback=7; ReviewedNoSourceEdit=4

## Source Repair Summary

POST-BATCH-O fixed confirmed response-time guard defects in gump, prompt, hue-picker, property-list, book, token, guild, help, Liars Dice, polymorph, honor, prison, house security, item-level, gift-level, and enhancement callback paths. Edits were limited to pre-mutation validation and index/text guards. Public APIs, command names, namespaces, type names, file locations, serialization layout, XML/config, project files, and gump button IDs were preserved.

Fixed source files:
- `Data/Scripts/Custom/Government System/Prompts/CitizenTitleChangePrompt.cs`
- `Data/Scripts/Custom/Government System/Prompts/CityTreasuryDepoistPrompt.cs`
- `Data/Scripts/Custom/Government System/Prompts/CityURLPrompt.cs`
- `Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Cooking/Items/FoodDyes.cs`
- `Data/Scripts/Items/Books/BulletinBoards/Gumps/BoardGump.cs`
- `Data/Scripts/Items/Books/LearnGranite.cs`
- `Data/Scripts/Items/Books/LearnLeather.cs`
- `Data/Scripts/Items/Books/LearnMetal.cs`
- `Data/Scripts/Items/Books/LearnMisc.cs`
- `Data/Scripts/Items/Books/LearnReagents.cs`
- `Data/Scripts/Items/Books/LearnScales.cs`
- `Data/Scripts/Items/Books/LearnStealing.cs`
- `Data/Scripts/Items/Books/LearnTailor.cs`
- `Data/Scripts/Items/Books/LearnWood.cs`
- `Data/Scripts/Items/Books/TitleChangeDeed.cs`
- `Data/Scripts/Items/Decorations/HouseSign.cs`
- `Data/Scripts/Items/Gifts/Gumps/Crystal Token Gump.cs`
- `Data/Scripts/Items/Gifts/Gumps/NinthAnniversaryCoinGump.cs`
- `Data/Scripts/Items/Gifts/Gumps/Shadow Token Gump.cs`
- `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave1.cs`
- `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave2.cs`
- `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave3.cs`
- `Data/Scripts/Items/Houses/Remodeling/Gumps/LawnSecurityGump.cs`
- `Data/Scripts/Items/Houses/Remodeling/Gumps/ShantySecurityGump.cs`
- `Data/Scripts/Items/Magical/Gifts/Gumps/GiftGump.cs`
- `Data/Scripts/Items/Magical/God/Gumps/ItemExperienceGump.cs`
- `Data/Scripts/Items/Magical/God/LegendsBook.cs`
- `Data/Scripts/Items/Misc/Games/DandD/DungeonMastersGuide.cs`
- `Data/Scripts/Items/Misc/Games/LiarsDice/Gumps/CallBluffGump.cs`
- `Data/Scripts/Items/Misc/Games/LiarsDice/Gumps/ExitDiceGump.cs`
- `Data/Scripts/Items/Misc/Games/LiarsDice/Gumps/GameDiceGump.cs`
- `Data/Scripts/Items/Misc/Games/LiarsDice/Gumps/NewDiceGameGump.cs`
- `Data/Scripts/Items/Misc/Games/LiarsDice/Gumps/StatusDiceGump.cs`
- `Data/Scripts/Magic/Druidism/BookDruidBrewing.cs`
- `Data/Scripts/Magic/Witch/BookWitchBrewing.cs`
- `Data/Scripts/System/Commands/Player/Basics.cs`
- `Data/Scripts/System/Commands/Player/CreatureHelp.cs`
- `Data/Scripts/System/Commands/Player/FameKarma.cs`
- `Data/Scripts/System/Commands/Player/Gumps/SkillsGump.cs`
- `Data/Scripts/System/Commands/Player/ItemProps.cs`
- `Data/Scripts/System/Commands/Player/Quests.cs`
- `Data/Scripts/System/Commands/Player/Wanted.cs`
- `Data/Scripts/System/Gumps/BaseConfirmGump.cs`
- `Data/Scripts/System/Gumps/BaseImageTileButtonsGump.cs`
- `Data/Scripts/System/Gumps/ClueGump.cs`
- `Data/Scripts/System/Gumps/Guilds/GuildCandidatesGump.cs`
- `Data/Scripts/System/Gumps/Guilds/GuildCharterPrompt.cs`
- `Data/Scripts/System/Gumps/Guilds/GuildDeclareWarPrompt.cs`
- `Data/Scripts/System/Gumps/Guilds/GuildRosterGump.cs`
- `Data/Scripts/System/Gumps/Guilds/GuildWarAdminGump.cs`
- `Data/Scripts/System/Gumps/Guilds/GuildWarGump.cs`
- `Data/Scripts/System/Gumps/Guilds/GuildWebsitePrompt.cs`
- `Data/Scripts/System/Gumps/HonorSelf.cs`
- `Data/Scripts/System/Gumps/PolymorphGump.cs`
- `Data/Scripts/System/Gumps/PrisonGump.cs`
- `Data/Scripts/System/Gumps/Properties/SetListOptionGump.cs`
- `Data/Scripts/System/Help/Gumps/PageResponseGump.cs`
- `Data/Scripts/System/Help/Gumps/SpeechLogGump.cs`
- `Data/Scripts/System/Help/PagePrompt.cs`
- `Data/Scripts/System/Help/ServerSettings.cs`
- `Data/Scripts/Trades/Guild/Gumps/EnhancementGump.cs`

## Verification

Passed.

## Remaining Queue

All 235 POST-BATCH-N gump guard source-batch rows are dispositioned in the POST-BATCH-O review CSV. Plain `SendGump` open-site rows were reviewed as `SafeNoChange`; interface/base/no-op callback rows were reviewed as `FalsePositive` or `ReviewedNoChange`; edited callback rows were marked `Fixed`.


Verification details:

- Targeted invariants: review CSV rows=235; active overlay POST-BATCH-O rows=235; unreviewed lane rows=0.
- Targeted guard scan: queued OnResponse scan reports only Data/System/Source/Menus/IMenu.cs, an interface signature intentionally classified as FalsePositive.
- Rename prompt scan: 25 guarded rename prompt callbacks across HouseSign.cs and Halloween grave reward prompts.
- git diff --check: passed with expected LF-to-CRLF warnings only.
- msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86: initial sandboxed attempt failed on user-profile SDK access; escalated rerun passed with 0 warnings and 0 errors.
- .\ConficturaServer.exe -compileonly -nocache: passed; output included Scripts: Compile-only verification completed successfully..
