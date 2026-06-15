# Quest Framework

## Summary

The quest framework is a compiled set of RunUO `Item`, `Mobile`, `Teleporter`, and `Gump` scripts under `Data/Scripts/Quests/`, with the visible player log opened by the `[quests` command. It is not a single `BaseQuest` inheritance tree in active quest code. Quest progress is stored mostly on `PlayerMobile` string fields through `Server.Misc.PlayerSettings`, while many active quests are represented by owner-bound world `Item` instances such as `ThiefNote`, `CourierMail`, `SearchPage`, `SummonPrison`, `FrankenJournal`, `RuneBox`, `VortexCube`, `ObeliskTip`, `MuseumBook`, and `QuestTome`.

The obsolete `QuestSystem`/`BaseQuest` classes found in `Data/Scripts/System/Obsolete/Obsolete.cs` were not part of this active `Data/Scripts/Quests/` trace.

## Player Entry Points

| Entry point | Access | Script | Behavior |
| --- | --- | --- | --- |
| `[quests` | `AccessLevel.Player` | `Quests.cs` | Registers in `Initialize()`, closes any open `QuestsGump`, and sends a new `QuestsGump(from)`. |
| Quickbar quest button | Player UI | `QuickBar.cs` | Also closes and sends `QuestsGump`. |
| Help gump quest page | Player UI | `HelpGump.cs` | Embeds `HelpGump.MyQuests(from)` in the help interface. |

`QuestsGump` is a read-only server-side `Gump`. It plays sound `0x4A`, draws art `9585`, uses `PlayerSettings.GetGumpHue(from)` for the hue, and renders `Server.Engines.Help.HelpGump.MyQuests(from)` inside a scrollable HTML region.

## Quest Log Rendering

`HelpGump.MyQuests(Mobile from)` builds the text shown in `QuestsGump`.

The log includes:

| Source | How it is detected | Output type |
| --- | --- | --- |
| Standard contract | `PlayerSettings.GetQuestState(from, "StandardQuest")` | Current adventurer/bounty/item contract status. |
| Assassin contract | `PlayerSettings.GetQuestState(from, "AssassinQuest")` | Current assassination target or return-for-payment state. |
| Fishing contract | `PlayerSettings.GetQuestState(from, "FishingQuest")` | Current sea bounty/item contract status. |
| Other owned quest Items | Full scan of `World.Items.Values` | Owner-bound quest status for notes, pages, prisons, journals, rune boxes, codex cubes, obelisks, museum books, and quest tomes. |
| Backpack quest Items | `from.Backpack.FindItemByType(...)` checks | Serpent, Shadowlord, Gem of Immortality, and Staff of Ultimate Power hints. |
| Character key flags | `PlayerSettings.GetKeys(...)` | Discovery and completion achievements. |
| Bards Tale flags | `PlayerSettings.GetBardsTaleQuest(...)` | Skara Brae/Bards Tale progress achievements. |
| Discovered worlds | `PlayerSettings.GetDiscovered(...)` | World-discovery achievements. |

## Persistent Player State

Quest state is serialized directly on `PlayerMobile` in the version 29 block. These fields are written and read positionally with the rest of `PlayerMobile` data, so changing order or meaning is world-save sensitive.

| Field | Stored data |
| --- | --- |
| `CharacterKeys` | `#`-delimited key/achievement flags handled by `GetKeys` and `SetKeys`. |
| `CharacterDiscovered` | `#`-delimited discovered-world flags handled by `GetDiscovered` and `SetDiscovered`. |
| `StandardQuest` | Active standard contract or last-completion timestamp. |
| `FishingQuest` | Active fishing contract or last-completion timestamp. |
| `AssassinQuest` | Active assassin contract or last-completion timestamp. |
| `MessageQuest` | Generic message quest slot. |
| `BardsTaleQuest` | `#`-delimited Bards Tale story flags. |
| `ThiefQuest` | Active thief quest state. |
| `KilledSpecialMonsters` | `#`-delimited special-kill flags. |
| `EpicQuestName` / `EpicQuestNumber` | Epic quest metadata fields. |

### Contract String Format

Standard, Fishing, and Assassin contracts use a shared `#`-delimited payload assembled by their helper classes.

| Position | Meaning |
| --- | --- |
| 1 | Target type name, or blank for item-search quests. |
| 2 | Target title, if any. |
| 3 | Display target name or generated quest item name. |
| 4 | Region text. |
| 5 | Completion flag, normally `0` until replaced with `1`. |
| 6 | Gold reward. |
| 7 | World name. |
| 8 | Category such as `Monster`, `Item`, or `Innocent`. |
| 9 | Generated story/status sentence. |

`PlayerSettings.GetQuestState()` treats a quest as active when the selected quest string splits into more than three `#` segments. Completion functions call `QuestTimeAllowed()` to replace the quest payload with `DateTime.Now.ToString()`, and board entries compare the elapsed minutes against `MyServerSettings.GetTimeBetweenQuests()`.

### Character Key Slots

`CharacterKeys` has 13 recognized slots.

| Slot | Key string |
| --- | --- |
| 1 | `UndermountainKey` |
| 2 | `BlackKnightKey` |
| 3 | `RangerOutpost` |
| 4 | `VordoKey` |
| 5 | `SkullGate` |
| 6 | `SerpentPillars` |
| 7 | `Antiques` |
| 8 | `Museums` |
| 9 | `Runes` |
| 10 | `Virtue` |
| 11 | `Corrupt` |
| 12 | `Gygax` |
| 13 | `DragonRiding` |

### Bards Tale Slots

`BardsTaleQuest` has 15 recognized slots.

| Slot | Flag string |
| --- | --- |
| 1 | `BardsTaleMadGodName` |
| 2 | `BardsTaleCatacombKey` |
| 3 | `BardsTaleEbonyKey` |
| 4 | `BardsTaleKylearanKey` |
| 5 | `BardsTaleHarkynKey` |
| 6 | `BardsTaleDragonKey` |
| 7 | `BardsTaleSpectreEye` |
| 8 | `BardsTaleCrystalSword` |
| 9 | `BardsTaleSilverSquare` |
| 10 | `BardsTaleBedroomKey` |
| 11 | `BardsTaleSilverTriangle` |
| 12 | `BardsTaleCrystalGolem` |
| 13 | `BardsTaleSilverCircle` |
| 14 | `BardsTaleMangarKey` |
| 15 | `BardsTaleWin` |

When `BardsTaleWin` is set, `SetBardsTaleQuest()` replaces the whole string with only the final win slot set to `1`.

### Discovered World Slots

`CharacterDiscovered` has 9 recognized slots.

| Slot | World string |
| --- | --- |
| 1 | `the Land of Lodoria` |
| 2 | `the Land of Sosaria` |
| 3 | `the Island of Umber Veil` |
| 4 | `the Land of Ambrosia` |
| 5 | `the Serpent Island` |
| 6 | `the Isles of Dread` |
| 7 | `the Savaged Empire` |
| 8 | `the Bottle World of Kuldar` |
| 9 | `the Underworld` |

### Special Kill Slots

`KilledSpecialMonsters` has 22 recognized slots.

| Slot | Kill string |
| --- | --- |
| 1 | `Arachnar` |
| 2 | `BlackGateDemon` |
| 3 | `BloodDemigod` |
| 4 | `Xurtzar` |
| 5 | `CaddelliteDragon` |
| 6 | `DragonKing` |
| 7 | `Vulcrum` |
| 8 | `OrkDemigod` |
| 9 | `Mangar` |
| 10 | `Astaroth` |
| 11 | `Faulinei` |
| 12 | `Nosfentor` |
| 13 | `Tarjan` |
| 14 | `Dracula` |
| 15 | `LichKing` |
| 16 | `Surtaz` |
| 17 | `TitanLithos` |
| 18 | `TitanPyros` |
| 19 | `TitanHydros` |
| 20 | `TitanStatos` |
| 21 | `Jormungandr` |
| 22 | `Exodus` |

## Dynamic Contract Boards

### StandardQuestBoard

`StandardQuestBoard` offers context menu entries for accepting and completing standard contracts, and accepts gold drag/drop to pay the failure penalty.

| Mechanic | Logic |
| --- | --- |
| New quest cooldown | `StandardQuestFunctions.QuestTimeNew()` parses the previous completion timestamp and compares elapsed minutes to `MyServerSettings.GetTimeBetweenQuests()`. |
| Target pool | Negative-karma `BaseCreature` targets in dungeon regions, below the caller's fame cap and no harder than `GetPlayerInfo.GetPlayerDifficulty(m)`. |
| World selection | Random world choice falls back to Sosaria unless the player has discovered the selected world. A non-Sosaria starter without Sosaria discovery is redirected to Savaged Empire or Lodoria. |
| Quest type | 50% `Monster` kill quest, 50% `Item` search quest. |
| Item find chance | `ChanceToFindQuestedItem()` returns `10`, checked as `10 >= Utility.RandomMinMax(1, 100)`. |
| Monster reward | `target.Fame / 5 * (QuestRewardModifier * 0.01)`. |
| Item reward | `(target.Fame / 3)` rounded down to the nearest 100, then multiplied by `(QuestRewardModifier * 0.01)`. |
| Completion reward | Adds `Gold(nPCFee)`, awards fame `nPCFee / 100`, and awards karma `+nPCFee / 100` unless `PlayerMobile.KarmaLocked` is true, in which case it awards negative karma. |

### FishingQuestBoard

`FishingQuestBoard` mirrors the standard board but uses sea and pirate targets.

| Mechanic | Logic |
| --- | --- |
| Pirate hunt roll | If `Utility.RandomMinMax(1, 8) == 1` and the input fame/fee is above `4000`, the board switches to pirate-hunt target selection. |
| Normal sea targets | Negative-karma creatures with `WhisperHue == 999`, in main regions, on world-specific maps. `SkipMe()` excludes several named pirate classes unless `EmoteHue == 123`. |
| Sea completion | `CheckTarget()` accepts item finds from `WhisperHue == 999` targets, or monster kills when the target is a sea target in the selected world. |
| Item find chance | Same 10% random check as standard contracts. |
| Normal monster reward | `target.Fame / 2` before storage. |
| Pirate captain reward | Starts from `(Fame / 10) * 3`, caps at `2000`, randomizes between 75% and 100% of that value, multiplies by 10, then applies `QuestRewardModifier * 0.01`. |
| Find-item reward | Randomizes from player fame clamped to 1000-10000, computes `((RandomMinMax(500, qFame) / 10) * 10) / 2`, then applies `QuestRewardModifier * 0.01`. |

### AssassinQuest and BoxOfAtonement

Assassin contracts are tracked in `AssassinQuest`.

| Mechanic | Logic |
| --- | --- |
| Monster target | Similar world/dungeon search to standard contracts, but always a kill target. Reward is `target.Fame / 5 * (QuestRewardModifier * 0.01)`. |
| Innocent target | `FindInnocentTarget()` searches unblessed `BaseVendor` mobiles in village regions and assigns category `Innocent` with base reward `1000 * (QuestRewardModifier * 0.01)`. |
| Completion | `CheckTarget()` marks the contract done when the killed target type and region match. |
| Payment | `PayAssassin()` pays the stored gold value when the done flag is set. |
| Failure atonement | `BoxOfAtonement.OnDragDrop()` accepts `Gold` only when `dropped.Amount == AssassinFunctions.QuestFailure(from)`; on exact payment it clears `AssassinQuest`, otherwise it returns the dropped item to the backpack. |

## Clue and Lore Items

### ScrollClue

`ScrollClue` is a generated parchment clue with command-editable fields for text, solved state, intelligence requirement, level, true/false state, quest type, target coordinates, and map.

| Mechanic | Logic |
| --- | --- |
| Appearance | ItemID `0x4CC6` or `0x4CC7`, name `a parchment`, weight `1.0`. |
| Language/encoding | Random language text builds `ScrollDescribe`. `ScrollIntelligence` is `Utility.RandomMinMax(2, 8) * 10`; `ScrollLevel` is `(ScrollIntelligence / 10) - 1`. |
| Code label | `ScrollSolved` is based on intelligence: `Diabolically`, `Ingeniously`, `Deviously`, `Cleverly`, `Adeptly`, `Expertly`, or `Plainly Coded`. |
| Truth flag | `ScrollTrue` randomly starts as `0` or `1`. False clues can consume themselves with failure messages when used at the coordinates. |
| Quest target | Generated as `grave`, `chest`, or sea/world lore depending on the constructor branch. Grave/chest branches store `ScrollX`, `ScrollY`, and `ScrollMap`. |
| Deciphering | On double click, a player can decipher when `Int >= ScrollIntelligence` or `Skills[Inscribe] >= Utility.RandomMinMax(30, 120)`. Success sets `ScrollIntelligence = 0` and `ScrollSolved = "Deciphered by <name>"`. |
| Dig/use radius | Grave and chest rewards require the player to stand within `ScrollX +/- 2`, `ScrollY +/- 2`, on `ScrollMap`. |
| Rewards | Valid grave clues create `BuriedBody(ScrollLevel * 2, ScrollCharacter, e)`. Valid chest clues create `BuriedChest(ScrollLevel * 2, ScrollCharacter, e)`. |
| Serialization | Version `0`, then text, solved label, intelligence, level, truth flag, description, quest type, character, X, Y, and map. On deserialize, invalid item art is normalized back to one of the parchment IDs and hue is reset to `0`. |

### SomeRandomNote

`SomeRandomNote` is generated lore text, not a tracked quest objective. It randomizes name, art, hue/text theme, and `ScrollMessage`, then displays a nested `ClueGump` on double-click. It serializes only version `0`, `ScrollMessage`, and `ScrollTrue`.

### NoticeClue

`NoticeClue` is an invisible, immovable movement trigger. When a `PlayerMobile` comes within 5 tiles and the 30-second local cooldown has elapsed, it displays this item's `Name` overhead and, for specific hard-coded X/Y coordinates, sends a fixed `ClueGump` hint.

## Quest Gates and Teleporters

| Script | Type | Mechanics |
| --- | --- | --- |
| `QuestTeleporter` | `Item` | Double-click and dead double-click call `DoQuestTeleporter()`. If `TeleporterOpen == 1` or `TeleporterQuest == "blank"`, it teleports the player and pets immediately. Otherwise it checks `PlayerSettings.GetKeys(m, TeleporterQuest)` or `GetBardsTaleQuest(m, TeleporterQuest)`, opens for 2 minutes, moves pets and player to `TeleporterPointDest`/`TeleporterMapDest`, plays `TeleporterSound`, and sends configured overhead text. |
| `QuestTransporter` | `Teleporter` | `OnMoveOver()` blocks `PlayerMobile` movement when `Required == "yes"` and `PlayerSettings.GetKeys(m, TeleportName)` is false. It sends `MessageString` through `UnicodeMessage`, throttled by `BeginAction(this)` for 5 seconds. Otherwise it calls `StartTeleport(m)`. |
| `TriggerTile` | `Item` | Movement trigger that sets `PlayerSettings.SetBardsTaleQuest(m, this.Name, true)` for `PlayerMobile` movers. |
| Quest chests/books | `Item` | Bards Tale chests and fixed quest books use `OnDoubleClick` range checks, set `PlayerSettings` flags, send overhead messages, sounds, and optional `ClueGump` hints. |

`QuestTeleporter` serializes version `0` plus `TeleporterOpen`, `TeleporterSound`, `TeleporterItem`, `TeleporterMessage`, `TeleporterFail`, `TeleporterQuest`, `TeleporterLock`, `TeleporterLockMsg`, destination point, and destination map. Deserialization closes the teleporter by forcing `TeleporterOpen = 0`.

`QuestTransporter` serializes version `0` plus `TeleportName`, `Required`, and `MessageString`.

## Major Quest Tome

`QuestTake` is a pickup journal that either returns an existing owner-bound `QuestTome` to the player's backpack or creates one through `SetupBook(from)`.

`SetupBook()`:

| Step | Logic |
| --- | --- |
| Owner | Sets `QuestTomeOwner = from`. |
| Appearance | Copies `ItemID`, `Hue`, and `Name` from the `QuestTake`. |
| NPC endpoints | Calls `NPCGood(from, tome)` and `NPCEvil(from, tome)`. |
| Relic goals | Generates `GoalItem1`, `GoalItem2`, and `GoalItem3` from either `QuestCharacters.QuestItems(false)`, `RandomHerb()`, or `RandomMagic()`. |
| Final goal | Generates `GoalItem4` from `QuestCharacters.QuestItems(false)`. |
| Villain | Calls `MakeVillain(tome)` and writes good/evil story text around the generated villain and four goal items. |

`QuestTome`:

| Mechanic | Logic |
| --- | --- |
| Ownership | Must be in the caller's backpack. If another Mobile uses it, the book attempts to return to the stored owner by scanning all accounts; otherwise it deletes itself. |
| Rumors | `TellRumor(player, citizen)` can set a rumor when the citizen has `Fame == 0`, the player has an owned `QuestTome`, and `QuestTomeCitizen == ""`. `SetRumor()` picks a quest type and target world/dungeon. |
| Early goals | `FoundItem(player, type, chest)` advances `QuestTomeGoals` from 0 through 2 if the type, dungeon, owner, and region match. The success roll is `Utility.RandomMinMax(1, 3) != 1`, so two out of three attempts succeed. |
| Final boss | After three goals, using the tome in the recorded dungeon with a non-empty citizen field creates the villain type with `Activator.CreateInstance`, applies `SummonPrison.SetDifficultyForMonster`, moves it next to the player, sets generated name/title/body/hue, and starts combat. |
| Final reward marker | `FoundItem()` with a matching `MajorItemOnCorpse` and `QuestTomeGoals >= 3` calls `ApproachObsidian.TitanRiches(player)`, announces `GoalItem4`, and increments goals. |
| Boss cleanup | `BossEscaped(from, region)` deletes mobiles whose `Name` and `Title` match the tome villain when the owner is in the matching region and `QuestTomeGoals > 2`. |

`QuestTome` serializes version `0`, owner Mobile, good/evil story text, good/evil locations/worlds/NPC names, current citizen, goal count, dungeon, land, type, four goal item names, villain category/type/name/title/body/hue.

## Quest Package Map

| Area | Main scripts | Notes |
| --- | --- | --- |
| Bards Tale/fixed keys | `QuestChests.cs`, `NoticeClue.cs`, `TriggerTile.cs`, `Bards Tale/MangarsRewards.cs` | Progress flags live in `BardsTaleQuest` and `CharacterKeys`. |
| Standard contracts | `StandardQuestBoard.cs`, `StandardQuestFunctions.cs` | Bounty/item contracts stored in `StandardQuest`. |
| Fishing contracts | `FishingQuestBoard.cs`, `FishingQuestFunctions.cs` | Sea bounty/item contracts stored in `FishingQuest`. |
| Assassin contracts | `AssassinFunctions.cs`, `AssassinBox.cs` | Kill contracts and atonement box stored in `AssassinQuest`. |
| Thief quests | `ThiefNote.cs`, `StealBase*.cs`, `Coffer.cs`, `HayCrate.cs`, `HollowStump.cs` | Owned notes and steal containers participate in `OtherQuests()`. |
| Search quests | `SearchBoard.cs`, `SearchBook.cs`, `SearchPage.cs`, `SearchBase.cs` | Search pages are owner-bound quest Items. |
| Museum | `Museum.cs`, `MuseumBook.cs`, `Museum/Gumps/MuseumBookGump.cs` | Tracks antique collection progress through `MuseumBook`. |
| Epic courier | `Epic/CourierMail.cs`, `Epic/Gumps/EpicGump.cs` | Owned mail items appear in `OtherQuests()`. |
| Summon prison | `SummonPrison.cs`, `SummonTutorial.cs`, `SummonItems.cs`, `SummonChest.cs`, `SummonCarriers.cs` | Owned prison items appear in `OtherQuests()`. |
| Frankenstein | `FrankenJournal.cs`, body-part items, porter/fighter scripts | Journal counts collected body parts in `OtherQuests()`. |
| Golems/robots | `GolemManual.cs`, `RobotSchematics.cs`, component items | Component assembly quests with serialized manuals/schematics. |
| Codex | `VortexCube.cs`, `CodexWisdom.cs`, `DoorCodex.cs`, `CubeOnCorpse.cs`, `ApproachVoid.cs` | `VortexCube` progress appears in `OtherQuests()`. |
| Pagan | `ObeliskTip.cs`, `PaganBase*.cs`, `PaganArtifact.cs`, `ObsidianGate.cs`, `ApproachObsidian.cs` | `ObeliskTip` progress appears in `OtherQuests()`. |
| Serpents | `BaneBase*.cs`, `BlackrockSerpents.cs`, `SerpentSpawners.cs`, serpent artifact items | Backpack artifacts add Serpent quest hints. |
| Shadowlords | `FlamesBase*.cs`, virtue artifacts, `GemImmortality.cs`, `BalinorTeleporter.cs` | Backpack artifacts add Shadowlord/Gem hints. |
| Runes | `RunesBase*.cs`, `RuneBox.cs` | Rune ownership/progress appears in `OtherQuests()`. |
| Hoard | `HoardSpawner.cs`, `HoardPile.cs` | Proximity-based hoard spawner and loot pile. |
| Magic Pools | `MagicPool.cs` | Pool interactions with broad random effects. |
| Underworld | `UnderworldTeleporter.cs`, `RuneStoneGate.cs`, `SkullOfBaron Almric.cs` | Underworld travel and obstruction/key interactions. |
| Major journal | `Major/QuestTake.cs`, `Major/QuestTome.cs` | Generated four-stage journal quest. |

## Serialization Notes

Most quest `Item` classes write `base.Serialize(writer)`, then version `0`, then their fields in positional order. The files traced generally read the same sequence back in `Deserialize()`.

Admin-sensitive cases:

| Class | Notes |
| --- | --- |
| `QuestTeleporter` | Keeps unused-looking `TeleporterItem` and `TeleporterLock` in the save format; removing them without a version bump would corrupt later reads. |
| `QuestTransporter` | Uses a `switch (version)` with only case `0`; any future version must preserve all three current strings for older saves. |
| `ScrollClue` | Resets parchment art and hue on load after reading serialized data. |
| `QuestTome` | Persists owner Mobile and all generated story/villain state; final boss creation depends on `VillainType` resolving at runtime. |
| `PlayerMobile` quest fields | Quest strings are part of the large version 29 player serialization block. Their write/read order is fixed among many unrelated player settings. |

## Known Issues

* `Scripts.csproj` points at `Quests\Epic\EpicGump.cs` and `Quests\Museum\MuseumBookGump.cs`, but the discovered files are under `Quests\Epic\Gumps\EpicGump.cs` and `Quests\Museum\Gumps\MuseumBookGump.cs`. A project build that depends on the explicit include paths can miss or fail these gumps.
* `PlayerSettings.GetQuestState()`, `GetQuestInfo()`, `SetQuestInfo()`, `ClearQuestInfo()`, `MarkQuestInfo()`, `GetBardsTaleQuest()`, and `SetBardsTaleQuest()` cast `Mobile` to `PlayerMobile` without a null/type guard. Several quest surfaces accept a plain `Mobile` parameter, so non-player callers can throw instead of safely returning.
* `HelpGump.OtherQuests()` directly uses `from.Backpack.FindItemByType(...)` without checking `from.Backpack != null`. It also contains duplicate `else if (item is SearchPage)` and duplicate `else if (item is RuneBox)` branches, so the later branches are unreachable.
* `HelpGump.OtherQuests()` checks several key names that `PlayerSettings.GetKeys()` never recognizes, including `Virtues`, `Exodus`, `BlackGateDemon`, `Jormungandr`, `Dracula`, `Arachnar`, `Surtaz`, `Vordinax`, `Vulcrum`, and `Xurtzar`. Those quest-log achievement lines cannot become true through the traced `CharacterKeys` helper.
* Dynamic target selection calls `Utility.RandomMinMax(1, aCount)` after building target lists in Standard, Fishing, Assassin, Innocent Assassin, and Major Quest Tome flows. Some paths have only partial fallback comments and no final `aCount > 0` guard, so an empty target pool can fail at quest generation.
* `QuestTome.OnDoubleClick()` creates the final boss with `ScriptCompiler.FindTypeByName(VillainType)` and `Activator.CreateInstance(mobType)`, then casts to `BaseCreature` without checking for a null type, constructor failure, or non-`BaseCreature` type.
* `QuestTeleporter` exposes and serializes `TeleporterItem` and `TeleporterLock`, but the traced teleport logic does not use either field. Staff may assume these properties affect gate behavior when they do not.
* Several quest scripts iterate `GetMobilesInRange()` or `GetItemsInRange()` directly without freeing the pooled enumerable, including `HoardSpawner`, `SearchBase`, `SkullOfBaronAlmric`, `Coffer`, `HayCrate`, `HollowStump`, `BalinorTeleporter`, `SummonPrison`, and `FrankenJournal`.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0042; PBN-0049; PBN-0069; PBN-0127.
- Backlog rows: RB-06749; RB-06750; RB-06751; RB-06752.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Quests/ (CurrentDirectory)
- Data/Scripts/System/Obsolete/Obsolete.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=12; Event=7; Gump=159; Initialize=19; Movement=16; Packet=2; Region=5; Speech=3; Timer=49; WorldSave=1.
- Data/Scripts/Quests/Codex/CodexWisdom.cs:L424 Gump OnResponse access=Internal
- Data/Scripts/Quests/Codex/CodexWisdom.cs:L486 Gump SendGump access=Internal
- Data/Scripts/Quests/Codex/CodexWisdom.cs:L607 Gump OnResponse access=Internal
- Data/Scripts/Quests/Codex/CodexWisdom.cs:L794 Gump SendGump access=Internal
- Data/Scripts/Quests/Codex/CodexWisdom.cs:L799 Gump SendGump access=Internal
- Data/Scripts/Quests/Codex/CodexWisdom.cs:L804 Gump SendGump access=Internal
- Data/Scripts/Quests/Codex/VortexCube.cs:L336 Gump SendGump access=Internal
- Data/Scripts/Quests/Codex/VortexCube.cs:L847 Gump OnResponse access=Internal
- Data/Scripts/Quests/Epic/CourierMail.cs:L215 Gump SendGump access=Internal
- Data/Scripts/Quests/Epic/Gumps/EpicGump.cs:L604 Gump OnResponse access=Internal
- Data/Scripts/Quests/Fishing/FishingQuestBoard.cs:L40 Gump SendGump access=Internal
- Data/Scripts/Quests/Fishing/FishingQuestBoard.cs:L74 Gump SendGump access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 302.
- Data/Scripts/Quests/Assassin/AssassinBox.cs:Server.Items.BoxOfAtonement version=0 serialize=L59 deserialize=L65 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Quests/Assassin/PoisonFood.cs:Server.Items.PoisonFood version=0 serialize=L178 deserialize=L186 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Quests/Assassin/PoisonLiquid.cs:Server.Items.PoisonLiquid version=0 serialize=L179 deserialize=L187 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Quests/Bards Tale/MangarsRewards.cs:Server.Items.BardicFeatheredHat version=0 serialize=L151 deserialize=L157 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Quests/Bards Tale/MangarsRewards.cs:Server.Items.MangarsElementalistRobe version=0 serialize=L113 deserialize=L119 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Quests/Bards Tale/MangarsRewards.cs:Server.Items.MangarsNecroRobe version=0 serialize=L73 deserialize=L79 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Quests/Bards Tale/MangarsRewards.cs:Server.Items.MangarsRobe version=0 serialize=L33 deserialize=L39 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Quests/Codex/ApproachVoid.cs:Server.Items.ApproachVoid version=0 serialize=L90 deserialize=L96 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Quests/Codex/CodexWisdom.cs:Server.CodexWisdom version=1 serialize=L1065 deserialize=L1082 alignment=CountMatchNeedsTypeReview:UnknownWrites=6
- Data/Scripts/Quests/Codex/CubeOnCorpse.cs:Server.Items.CubeOnCorpse version=0 serialize=L180 deserialize=L186 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Quests/Codex/DoorCodex.cs:Server.Items.DoorCodex version=0 serialize=L142 deserialize=L148 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Quests/Codex/VortexCube.cs:Server.Items.VortexCube version=0 serialize=L343 deserialize=L374 alignment=CountMatchNeedsTypeReview:UnknownWrites=19
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Quests/Assassin/AssassinBox.cs=Keep
- Data/Scripts/Quests/Assassin/AssassinBox.cs=Keep
- Data/Scripts/Quests/Assassin/AssassinFunctions.cs=Keep
- Data/Scripts/Quests/Assassin/AssassinFunctions.cs=Keep
- Data/Scripts/Quests/Assassin/PoisonFood.cs=Keep
- Data/Scripts/Quests/Assassin/PoisonFood.cs=Keep
- Data/Scripts/Quests/Assassin/PoisonLiquid.cs=Keep
- Data/Scripts/Quests/Assassin/PoisonLiquid.cs=Keep
- Data/Scripts/Quests/Bards Tale/MangarsRewards.cs=Keep
- Data/Scripts/Quests/Bards Tale/MangarsRewards.cs=Keep
- Data/Scripts/Quests/Codex/ApproachVoid.cs=Keep
- Data/Scripts/Quests/Codex/ApproachVoid.cs=Keep
- Data/Scripts/Quests/Codex/CodexWisdom.cs=Keep
- Data/Scripts/Quests/Codex/CodexWisdom.cs=Keep
- Data/Scripts/Quests/Codex/CubeOnCorpse.cs=Keep
- Data/Scripts/Quests/Codex/CubeOnCorpse.cs=Keep
- Data/Scripts/Quests/Codex/DoorCodex.cs=Keep
- Data/Scripts/Quests/Codex/DoorCodex.cs=Keep
- Data/Scripts/Quests/Codex/VortexCube.cs=Keep
- Data/Scripts/Quests/Codex/VortexCube.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
