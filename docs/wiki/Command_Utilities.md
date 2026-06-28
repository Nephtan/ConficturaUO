# Command Utilities

## Overview
The Command Utilities system is the shard's command framework plus the command scripts under `Data/Scripts/System/Commands`. It includes the core `CommandSystem` parser in `Data/System/Source/Commands.cs`, direct command handlers in `Handlers.cs`, the generic target-command framework in `Commands/` and `Implementors/`, administrative build/export tools, and player convenience commands.

Commands normally use the `[` prefix. `CommandHandlers.Initialize()` explicitly sets `CommandSystem.Prefix = "["`, then registers the core direct commands.

## Command Pipeline
| Stage | Compiled behavior |
| --- | --- |
| Command detection | `CommandSystem.Handle(Mobile from, string text, MessageType type)` treats text as a command when it starts with `CommandSystem.Prefix` or arrives as `MessageType.Command`. |
| Prefix stripping | Regular speech commands have the prefix removed before parsing. `MessageType.Command` text is not stripped. |
| Argument parsing | The command name is the text before the first space. Remaining text is split by `CommandSystem.Split`, which preserves quoted phrases and supports escaped quotes inside quoted text. |
| Registry lookup | Commands are stored in a case-insensitive `Dictionary<string, CommandEntry>`. Later registrations overwrite earlier entries with the same command string. |
| Access gate | The caller's `AccessLevel` must be greater than or equal to the registered `CommandEntry.AccessLevel`. Low-access invalid commands can be ignored silently through `BadCommandIgnoreLevel`. |
| Handler dispatch | The registered `CommandEventHandler` receives `CommandEventArgs`, then `EventSink.InvokeCommand(e)` fires for logging and observers. |

## Command Logging
`CommandLogging.Initialize()` hooks `EventSink.Command` and creates `Data/Logs/Commands`. Every command event is written to the daily command log and to a per-access-level, per-account/name log when logging is enabled.

The generic command executor temporarily disables `CommandLogging.Enabled` for list commands affecting more than 20 objects, then re-enables it after `ExecuteList` returns. Some destructive commands also log explicit action details before deleting, moving, banning, adding, or spawning objects.

## Generic Command Framework
Generic commands are `BaseCommand` subclasses registered by `TargetCommands.Initialize()`. Each command declares:

| Field | Meaning |
| --- | --- |
| `Commands` | One or more command words and aliases. |
| `AccessLevel` | Minimum account access required to use the command. |
| `Supports` | Which command implementors can invoke it. |
| `ObjectTypes` | Whether it accepts `Item`, `Mobile`, both, or all target types. |
| `Usage` / `Description` | Help text surfaced by the command documentation tools. |
| `ListOptimized` | Whether the command prefers a whole `ArrayList` over one object at a time. |

`BaseCommand.IsAccessible(from, obj)` protects higher-access Mobiles and their carried Items from lower-access staff. Administrators bypass this check.

## Generic Implementors
| Implementor command | Access | Target source | Condition support | Notes |
| --- | --- | --- | --- | --- |
| `[Single <command>]` | Counselor | One targeted object | No | `SingleCommandImplementor` also registers each supported generic command directly, so `[Set`, `[Get`, `[Kill`, etc. redirect through the single-target path. |
| `[Serial <serial> <command>]` | Counselor | `World.FindItem` or `World.FindMobile` by serial | No | Requires the serial argument before the generic command name. |
| `[Self <command>]` | Counselor | The commanding `Mobile` | No | Rejects item-only commands by returning no target object. |
| `[Multi <command>]`, `[m <command>]` | Counselor | Repeating target cursor | No | Runs the command and immediately retargets. |
| `[Area <command> [extensions]]`, `[Group <command> [extensions]]` | GameMaster | Targeted bounding box | Yes | Uses `BoundingBoxPicker`, map bounds queries, and optional extensions. |
| `[Global <command> [extensions]]` | Administrator | All `World.Items` and/or `World.Mobiles` | Yes | Filters global world dictionaries by object type and extensions. |
| `[Online <command> [extensions]]` | GameMaster | `NetState.Instances` Mobiles | Yes | Only supports Mobile commands. |
| `[Region <command> [extensions]]` | GameMaster | Mobiles from the caller's current `Region` | Yes | Rejects item-only use. |
| `[Contained <command> [extensions]]` | GameMaster | Child Items in a targeted `Container` | Parsed through extensions | Uses `Container.FindItemsByType(typeof(Item), true)`. |

## Generic Extensions And Conditions
Extensions are parsed from the end of the argument list, optimized, sorted by `ExtensionInfo.Order`, and then applied to the target list.

| Extension | Syntax | Effect |
| --- | --- | --- |
| `Where` | `Where <type> <property> <operator> <value> ...` | Builds an `ObjectConditional` against a C# type and property chain. |
| `Distinct` | `Distinct <property> [property ...]` | Sorts a copy and keeps one object per comparer group. Requires a `Where` base type. |
| `Order` | `Order [by] <property> [asc|desc] ...` | Sorts the target list by compiled property comparers. Requires a `Where` base type. |
| `Limit` | `Limit <count>` | Removes entries after the given count. Negative counts throw. |

Conditional operators include equality (`=`, `==`, `is`, `!=`), relational comparisons (`>`, `<`, `>=`, `<=`), and string operators (`starts`, `ends`, `contains`, plus `~` case-insensitive variants). `not` or `!` inverts the next condition.

## Generic Command Catalog
| Command(s) | Access | Object types | Compiled effect |
| --- | --- | --- | --- |
| `Add` | GameMaster | All target points | Opens `CategorizedAddGump` with no arguments, or constructs an `Item`/`Mobile` by type name at the targeted point. Supports constructor parameters and `set <property> <value>` pairs. |
| `AddToPack`, `AddToCont` | GameMaster | Mobiles and Containers | Constructs an Item by type name and drops it into target backpacks or Containers. |
| `BringToPack` | GameMaster | Items | Places the targeted Item into the caller's backpack if it fits. |
| `Teleport`, `Tele` | Counselor | All target points | Moves the caller to the targeted point, Item top location, or Mobile location. |
| `Get` | Counselor | All | Reads one or more command properties from a targeted object. |
| `GetType` | Counselor | All | Reports the target object's C# type name. |
| `Set` | Counselor | Items and Mobiles | Sets one or more command properties by name and value. |
| `Increase`, `Inc` | Counselor | Items and Mobiles | Adds offsets to one or more command properties; rejects `BaseMulti`. |
| `Count` | GameMaster | All | Counts the list built by a complex implementor. |
| `Condition` | GameMaster | All | Tests whether a targeted object matches a parsed `ObjectConditional`. |
| `Delete`, `Remove` | GameMaster | Items and NPC Mobiles | Deletes target Items or non-player Mobiles; prompts before deleting multi-object lists. |
| `Kill` | GameMaster | Mobiles | Deals 10000 damage from the caller after `CanBeDamaged()` passes. |
| `Resurrect`, `Res` | GameMaster | Mobiles | Resurrects dead Mobiles and dead bonded pets; applies normal death-penalty recovery path for non-pets. |
| `Hide`, `Unhide` | Counselor | Mobiles | Plays visual effects and toggles `Mobile.Hidden`. |
| `Kick` | GameMaster | Mobiles | Disposes the target player's `NetState` when the caller outranks the target. |
| `Ban` | Administrator | Mobiles | Disposes the target `NetState`, sets the target `Account.Banned`, records an unspecified ban, and opens `BanDurationGump`. |
| `Firewall` | Administrator | Mobiles | Adds the online target's `NetState.Address` to `Firewall`. It does not kick or ban by itself. |
| `OpenBrowser`, `OB` | GameMaster | Player Mobiles | Sends a `WarningGump`; on confirmation, launches the target client's browser to the supplied URL. |
| `Tell` | Counselor | Mobiles | Sends the argument string as a system message to the target. |
| `PrivSound` | GameMaster | Mobiles | Sends a `PlaySound` packet at the target's location. |
| `Dismount` | GameMaster | Mobiles | Dismounts a mounted target. |
| `Restock` | GameMaster | NPC Mobiles | Calls vendor restock behavior on supported targets. |
| `RefreshHouse` | GameMaster | Items | Refreshes a targeted house sign's `BaseHouse` decay timer. |
| `Interface` | GameMaster | All | Opens a grid `Gump` for selected properties, with item/mobile sub-gumps and property access. |
| `Immortal`, `Invul`, `Mortal`, `NoInvul` | GameMaster | Mobiles | Aliased `Set` commands for the `Blessed` property. |
| `Squelch`, `Unsquelch` | GameMaster | Mobiles | Aliased `Set` commands for the `Squelched` property. |
| `ShaveHair`, `ShaveBeard` | GameMaster | Mobiles | Aliased `Set` commands setting hair or facial hair item IDs to `0`. |
| `FactionKick`, `FactionBan`, `FactionUnban` | GameMaster | Mobiles | Registered from the faction generic command class; modifies faction participation or faction ban status. |

## Direct Staff And Admin Commands
| Command(s) | Access | Script | Compiled effect |
| --- | --- | --- | --- |
| `Go` | Counselor | `Handlers.cs` | Opens the go menu, moves to a map or region name, moves to an Item/Mobile serial, moves to `x y [z]`, or reverse-lookups sextant coordinates. |
| `Where` | Counselor | `Handlers.cs` | Reports current coordinates, map/facet, and region data. |
| `DropHolding` | Counselor | `Handlers.cs` | Drops the caller's held item. |
| `GetFollowers` | GameMaster | `Handlers.cs` | Targets a player and teleports that player's pets to the caller. |
| `ClearFacet` | Administrator | `Handlers.cs` | Collects root Items on the current map and non-player Mobiles on the current map, then prompts before deleting them. |
| `AutoPageNotify`, `APN` | Counselor | `Handlers.cs` | Toggles the caller's page-notification flag. |
| `Animate` | GameMaster | `Handlers.cs` | Plays a requested animation on the caller. |
| `Cast` | Counselor | `Handlers.cs` | Casts a spell by name through the spell registry lookup path. |
| `Stuck` | Counselor | `Handlers.cs` | Opens `StuckMenu` for the targeted Mobile. |
| `Help` | Player | `Handlers.cs` | Lists available commands for the caller's access level. |
| `Save` | Administrator | `Handlers.cs` | Saves the world synchronously. |
| `BackgroundSave`, `BGSave`, `SaveBG` | Administrator | `Handlers.cs` | Saves the world with background write enabled. |
| `Move` | GameMaster | `Handlers.cs` | Retargets an Item or Mobile to a new position. |
| `Client` | Counselor | `Handlers.cs` | Opens `ClientGump` for a targeted online player. |
| `SMsg`, `SM`, `S` | Counselor | `Handlers.cs` | Broadcasts a staff message to online staff. |
| `BCast`, `BC`, `B` | GameMaster | `Handlers.cs` | Broadcasts a message to everyone online. |
| `Bank` | GameMaster | `Handlers.cs` | Opens a targeted Mobile's bank box. |
| `Echo` | Counselor | `Handlers.cs` | Echoes text back as a system message. |
| `Sound` | GameMaster | `Handlers.cs` | Plays a sound locally or to all, depending on the optional boolean. |
| `ViewEquip` | GameMaster | `Handlers.cs` | Opens an item-list menu for a targeted Mobile's equipped Items. |
| `Light` | Counselor | `Handlers.cs` | Sets the caller's local light level. |
| `Stats` | Counselor | `Handlers.cs` | Shows server stats. |
| `ReplaceBankers` | Administrator | `Handlers.cs` | Finds non-`BaseCreature` `Banker` Mobiles and creates a `Spawner(1, 1, 5, 0, 4, "banker")` at their location if no banker spawner is on the same tile. |
| `SpeedBoost` | Counselor | `Handlers.cs` | Sends `SpeedControl.MountSpeed`, or disables speed control when passed `false`. |

## Build, World, And Export Commands
| Command(s) | Access | Script | Compiled effect |
| --- | --- | --- | --- |
| `Tile` | GameMaster | `Add.cs` | Targets a bounding box and fills every tile with a constructed Item or Mobile. |
| `TileRXYZ` | GameMaster | `Add.cs` | Tiles a relative `x y width height z` rectangle from the caller's current position. |
| `TileXYZ` | GameMaster | `Add.cs` | Tiles an absolute `x y width height z` rectangle. |
| `TileZ` | GameMaster | `Add.cs` | Targets a bounding box but forces the supplied Z. |
| `AddTrap` | GameMaster | `AddTrap.cs` | Opens `AddTrapGump`, whose buttons add fire column, flame spurt, giant spike, gas, mushroom, wall spike, wall saw, floor saw, and stone-face trap Items. |
| `AddDoor` | GameMaster | `Gumps/Adddoorgump.cs` | Opens a door-addition Gump. |
| `Dupe` | GameMaster | `Dupe.cs` | Duplicates a targeted Item into the caller's backpack or world location using a zero-argument constructor and writable property copying. |
| `DupeInBag` | GameMaster | `Dupe.cs` | Duplicates a targeted Item into its current Container or parent's backpack. |
| `AddToBank` | Administrator | `AddToBank.cs` | Opens `AddToBankGump`, then distributes duplicated Items by account, by character, or by selected staff access levels. |
| `Wipe`, `WipeItems`, `WipeNPCs`, `WipeMultis` | GameMaster | `Wipe.cs` | Targets a bounding box and deletes matching Items, non-player Mobiles, or `BaseMulti` objects. `Wipe` excludes multis and deletes Items plus NPC Mobiles. |
| `Freeze`, `FreezeMap`, `FreezeWorld` | Administrator | `Statics.cs` | Converts dynamic Items into static map data for a targeted area, current map, or all maps after warning confirmation. |
| `Unfreeze`, `UnfreezeMap`, `UnfreezeWorld` | Administrator | `Statics.cs` | Converts static map entries into immovable dynamic Items for a targeted area, current map, or all maps after warning confirmation. |
| `StaticExport`, `StaEx` | Administrator | `StaticExport.cs` | Exports static data to a file, optionally named by argument. |
| `SpecialExport`, `SpcEx` | Administrator | `SpecialExport.cs` | Exports special map data to a file, optionally named by argument. |
| `ExportWSC` | Administrator | `ExportWSC.cs` | Exports world state in WSC format. |
| `AddonGen` | Administrator | `AddonGenerator.cs` | Opens an addon script generator Gump or starts with name/namespace arguments; writes generated addon/deed C# under `TheBox` unless a custom output directory is configured. |
| `MultiGen` | Administrator | `MultiMaker.cs` | Opens a multi/addon generator Gump using similar state defaults to `AddonGen`. |
| `RecordItems` | Counselor | `RecordItems.cs` | Records targeted Item data. |
| `BodyValues` | Counselor | `BodyValues.cs` | Generates body-value information. |
| `SignGen` | Administrator | `SignParser.cs` | Generates signs from parsed data. |
| `RebuildCategorization` | Administrator | `GenCategorization.cs` | Rebuilds add-menu categorization data. |
| `SpawnerCatalog` | Counselor | `SpawnerCatalog.cs` | Builds or displays spawner catalog information. |

## Diagnostics And Server-Control Commands
| Command(s) | Access | Script | Compiled effect |
| --- | --- | --- | --- |
| `Props [serial]` | Counselor | `Properties.cs` | Opens the property Gump for a target or supplied serial. |
| `Skills` | Counselor | `SkillsMenu.cs` | Opens the staff skills menu. |
| `SetSkill <name> <value>` | GameMaster | `Skills.cs` | Targets a Mobile and sets one named skill. |
| `GetSkill <name>` | GameMaster | `Skills.cs` | Targets a Mobile and reports one named skill. |
| `SetAllSkills <name> <value>` | GameMaster | `Skills.cs` | Sets one named skill for all applicable targets. |
| `Vis`, `VisList`, `VisClear` | Counselor | `VisibilityList.cs` | Maintains a counselor visibility list, with login hook support. |
| `TargetLog`, `PointLog`, `AreaLog`, `FaceLog` | Counselor | `TargetLog.cs`, `PointLog.cs`, `AreaLog.cs`, `FaceLog.cs` | Writes targeted, point, area, or facing data to logs. |
| `Batch` | Counselor | `Batch.cs` | Opens `BatchGump` to queue and run repeated command batches. |
| `Admin` | Administrator | `Gumps/AdminGump.cs` | Opens the main admin Gump. |
| `Scan` | Counselor | `Gumps/ScanGump.cs` | Opens a scan Gump. |
| `HelpInfo [<command>]` | Player | `HelpInfo.cs` | Shows command help info for the command list or a specific command. |
| `helpadmin` | Administrator | `HelpAdmin.cs` | Opens administrative help. |
| `DocGen` | Administrator | `Docs.cs` | Generates command documentation data from registered command metadata. |
| `Crash` | Administrator | `CrashCommand.cs` | Intentionally crashes the server process. |
| `logchests` | Administrator | `LogTreasureChests.cs` | Logs treasure chest information. |
| `DumpTimers` | Administrator | `Profiling.cs` | Dumps timer diagnostics. |
| `CountObjects` | Administrator | `Profiling.cs` | Counts world objects by type. |
| `ProfileWorld` | Administrator | `Profiling.cs` | Profiles world state. |
| `TraceInternal`, `TraceExpanded` | Administrator | `Profiling.cs` | Writes internal or expanded trace data. |
| `WriteProfiles` | Administrator | `Profiling.cs` | Writes collected profile data. |
| `SetProfiles [true | false]` | Administrator | `Profiling.cs` | Enables or disables profiling collection. |

## Player Command Catalog
| Command(s) | Script | Compiled effect |
| --- | --- | --- |
| `afk [message]` | `Player/Afk.cs` | Toggles an `AFK` Timer. The timer wakes on logout, speech, death, or movement; otherwise it emotes the AFK message every 10 seconds and says `zZz` every 30 seconds. |
| `autoattack` | `Player/AutoAttack.cs` | Toggles `Mobile.NoAutoAttack`. |
| `sheathe` | `Player/AutoSheatheWeapon.cs` | Toggles `PlayerMobile.CharacterSheath`. `PlayerMobile` warmode changes call `AutoSheatheWeapon.From(this)` to move weapons between hand layers and backpack. |
| `bandself` | `Player/BandSelf.cs` | Finds a `Bandage` in the caller's backpack and calls `Bandage.BandSelfCommandCall`. Warns at 5 or fewer bandages. |
| `bandother` | `Player/BandSelf.cs` | Finds a `Bandage` in the caller's backpack and starts the target path through `Bandage.BandOtherCommandCall`. |
| `poisons` | `Player/ClassicPoisoning.cs` | Toggles `PlayerMobile.ClassicPoisoning` between classic weapon-hit poisoning and precision infectious-strike poisoning. |
| `corpse` | `Player/CorpseSearch.cs` | Searches up to 1000 tiles for the caller's non-empty corpse in the same land/region, spawns hidden `CorpseCritter` target Mobiles, and assigns a `QuestArrow` to the closest match. |
| `corpseclear` | `Player/CorpseSearch.cs` | Deletes the caller's corpses only when `Worlds.ItemOnBoat(body)` returns true. |
| `magicgate` | `Player/MoonSearch.cs` | Searches up to 1000 tiles for `GateMoon`, `moongates`, or `Moongate` Items in the same land/region, then assigns a `QuestArrow` to the closest hidden `MoonCritter`. |
| `loot` | `Player/Loot.cs` | Opens `LootChoices`, stores a `CharacterLoot` `#`-delimited option string, and can select one static autoloot Container. |
| `music` | `Player/MusicPlaylist.cs` | Opens the music playlist/player Gump and updates `PlayerMobile.MusicPlaylist`. |
| `musical` | `Player/MusicPlayer.cs` | Toggles `PlayerMobile.CharMusical` between `Forest` and `Dungeon`. |
| `quickbar` | `Player/QuickBar.cs` | Opens a resource/action quickbar Gump with counts from the backpack and equipped quiver. |
| `regbar`, `regclose` | `Player/RegBar.cs` | Opens or closes a reagent bar Gump configured through `PlayerSettings.ValReagentConfig`. |
| `wealth` | `Player/WealthBar.cs` | Opens a wealth tracking bar using `GetPlayerInfo.GetWealth`. |
| `skill` | `Player/Skills.cs` | Opens a long skill-description Gump. |
| `skilllist` | `Player/SkillListing.cs` | Opens a condensed skill watcher Gump based on skill locks and `PlayerMobile.SkillDisplay`. |
| `SkillName <name>` | `Player/SkillName.cs` | Sets `PlayerMobile.CharacterSkill` to a hard-coded ID for an exact lowercase skill name, or `0` for `clear`. |
| `spellhue [<number>]` | `Player/SpellHue.cs` | Sets `PlayerMobile.MagerySpellHue` to the supplied integer, or `0` with no argument. |
| `barbaric` | `Player/PlayBarbaric.cs` | Toggles `PlayerMobile.CharacterBarbaric`; female characters cycle into amazon fighter titles; enabling grants `BarbaricSatchel` and clears evil/oriental flags. |
| `evil` | `Player/PlayEvil.cs` | Toggles `PlayerMobile.CharacterEvil` and clears oriental/barbaric flags when enabled. |
| `oriental` | `Player/PlayOriental.cs` | Toggles `PlayerMobile.CharacterOriental` and clears evil/barbaric flags when enabled. |
| `private` | `Player/PrivateTime.cs` | Toggles `PlayerMobile.PublicMyRunUO` for town crier news visibility. |
| `emote`, `e` | `Player/Emote.cs` | Opens or uses the emote/sound system. Some emotes create temporary `Puke` Items. |
| `ancient`, `archclose*`, `mageclose*`, `elementclose*`, `necroclose*`, `deathclose*`, `holyclose*`, `knightclose*`, `bardclose*`, `monkclose*` | `Player/SpellBarsCommands.cs` | Opens or closes grouped spell toolbar Gumps. |
| `archtool*`, `magetool*`, `elementtool*`, `necrotool*`, `knighttool*`, `bardtool*`, `deathtool*`, `holytool*`, `monktool*` | `Player/SpellBarsDisplay.cs` | Displays configured magic toolbar Gumps. |
| `archspell*`, `magespell*`, `elementspell*`, `necrospell*`, `knightspell*`, `bardsong*`, `deathspell*`, `holyspell*`, `monkspell*` | `Player/SpellBarsManage.cs` | Opens configuration Gumps for magic toolbar slots. |

## Item And Mobile Persistence
Most command utilities are static handlers or `Gump` classes and do not serialize shard state themselves. The command folder does include several persistent or temporary world objects:

| Class | Type | Serialization behavior |
| --- | --- | --- |
| Generated addon/deed templates from `AddonGenerator` | `BaseAddon` / `BaseAddonDeed` | Generated code writes version `0` after `base.Serialize` and reads the version after `base.Deserialize`. |
| `CEOIdentifyAddon` | `BaseAddon` | Writes version `0`; deletes itself on deserialize when its map is null or internal. |
| `MultiIdentifyAddon` | `BaseAddon` | Writes version `0`; deletes itself on deserialize when its map is null or internal. |
| `WandOfColors` | `Item` | Writes and reads version `0`; no custom fields. |
| `Puke` | `Item` | Writes and reads version `0`; deletes itself on deserialize and also starts a 10-second deletion timer when constructed. |
| `CorpseCritter` | `BaseCreature` | Writes and reads version `0`; schedules deletion one second after deserialize. Constructed instances schedule deletion after 10 minutes. |
| `MoonCritter` | `BaseCreature` | Writes and reads version `0`; schedules deletion one second after deserialize. Constructed instances schedule deletion after 10 minutes. |

## Known Issues
| Issue | Evidence from trace |
| --- | --- |
| Generic conditional `or` / `||` parsing appears broken. `ObjectConditional.ParseDirect` only splits the current condition group when `conditions.Count > 1`, but `conditions` starts empty and is not populated until after parsing. In practice, `or` tokens do not create alternate condition groups. |
| Large-list generic commands can leave `CommandLogging.Enabled = false` if `command.ExecuteList(e, list)` throws before the re-enable block. `DeleteCommand.OnConfirmCallback` has the same no-`finally` pattern around both `CommandLogging.Enabled = false` and `NetState.Pause()`. |
| `Add.ParseValue` mishandles hexadecimal constructor parameters. It conditionally converts signed and unsigned numeric types, then unconditionally overwrites the result with `Convert.ToInt32(...)`, so wide or unsigned hex values can be truncated or rejected. |
| `AddToBank.GiveItemToAccounts` only reliably reaches account slot `0`, or slot `1` when slot `0` is null. The later account-slot checks are chained behind repeated `else if (acct[0] == null)`, so slots after the first failed branch are not reached. |
| `AddToBankGump` constructs any type returned by `ScriptCompiler.FindTypeByName` through `Activator.CreateInstance(type)` before verifying it is an `Item`. Non-Item default constructors can run and the constructed object is not cleaned up by this method. |
| `CorpseSearch` and `MoonSearch` iterate `from.GetItemsInRange(1000)` without freeing the pooled enumerable. Both commands also create hidden target `Mobile` instances as arrow anchors and leave valid but non-selected anchors to expire by timer instead of deleting them immediately. |
| `corpseclear` says it removes the caller's corpses in the land, but the deletion condition is `Worlds.ItemOnBoat(body)`, so it only deletes owned corpses that satisfy the boat check. |
| `ReplaceBankers_OnCommand` scans `m.GetItemsInRange(0)` without freeing the pooled enumerable. |
| Several player commands and Gump constructors assume `PlayerMobile` or a non-null backpack without local guards (`bandself`, `bandother`, `quickbar`, `regbar`, and multiple play-style toggles). These are player commands in normal use, but the handlers themselves are not defensive. |
