# Region System

## Overview
The Region System is the shard's XML-backed map area registry plus the runtime hooks that attach rules to those areas. The core engine class is `Server.Region`; Confictura replaces the default XML region type with `Server.Regions.BaseRegion` during `Configure()`, then `Region.Load()` reads `Data/System/XML/Regions.xml` before world items and mobiles are loaded.

Regions are not persisted as world-save objects. Static regions are rebuilt from XML on each boot. Regional spawn runtime state is persisted separately through the internal `SpawnPersistence` `Item`.

## Source Files
| File | Type | Purpose |
| --- | --- | --- |
| `Data/System/Source/Region.cs` | Core engine class | Stores registered regions, loads `Regions.xml`, resolves `Region.Find`, handles movement permission and enter/exit dispatch. |
| `Data/System/Source/Sector.cs` | Core sector registry | Stores sorted `RegionRect` entries per sector and updates mobiles when region rectangles enter or leave a sector. |
| `Data/System/Source/Map.cs` | Core map registry | Stores region names in a case-insensitive per-map dictionary used by region spawn commands. |
| `Data/Scripts/System/Regions/BaseRegion.cs` | Default XML region subclass | Adds shard rules, player entry/exit maintenance, regional spawning, rune names, logout-delay overrides, and XML spawn parsing. |
| `Data/Scripts/System/Regions/GuardedRegion.cs` | Guarded region subclass | Adds guard commands, guard speech handling, criminal candidate timers, and guard spawning. |
| `Data/Scripts/System/Regions/HouseRegion.cs` | Dynamic region subclass | Builds a dynamic region from a `BaseHouse`, enforces house access, speech commands, secure access, decay, and logout rules. |
| `Data/Scripts/System/Regions/RegionMusic.cs` | Music helper | Picks and sends random `MusicName` tracks based on region type, region name, and player playlist settings. |
| `Data/Scripts/System/Regions/Spawning/SpawnEntry.cs` | Region spawner | Owns one regional spawn entry, its timer, spawned object list, admin commands, and serialized runtime state. |
| `Data/Scripts/System/Regions/Spawning/SpawnDefinition.cs` | Spawn definition parser | Converts XML spawn nodes into mobile, item, treasure chest, or weighted group definitions. |
| `Data/Scripts/System/Regions/Spawning/SpawnPersistence.cs` | Internal persistence item | Saves and reloads `SpawnEntry` runtime state. |
| `Data/System/XML/Regions.xml` | Region data | Declares the static regions, rectangles, priorities, music, logout options, and region spawns. |

## Boot Flow
| Stage | Compiled behavior |
| --- | --- |
| Script compile | `ScriptCompiler.Compile(...)` completes before any region XML load. |
| `Configure` pass | `ScriptCompiler.Invoke("Configure")` runs. `BaseRegion.Configure()` sets `Region.DefaultRegionType = typeof(BaseRegion)`. |
| Region XML load | `Region.Load()` reads `Data/System/XML/Regions.xml`, finds the `ServerRegions` root, iterates `<Facet name="...">`, parses each map, and recursively loads `<region>` nodes. |
| Region construction | Each `<region>` defaults to `BaseRegion` unless the `type` attribute resolves to a subclass of `Region`. Construction uses the `(XmlElement xml, Map map, Region parent)` constructor. |
| Registration | `Region.Register()` calls `OnRegister()`, attaches to parent children, adds to the global `Region.Regions` list, stores the region in `Map.Regions` by name, and inserts each rectangle into every covered `Sector`. |
| World load | `World.Load()` runs after regions are registered, so world items and mobiles can resolve their regions. |
| Initialize pass | `ScriptCompiler.Invoke("Initialize")` registers region commands and creates the `SpawnPersistence` item if missing. |

## Region Resolution
`Region.Find(Point3D p, Map map)` returns `Map.Internal.DefaultRegion` for a null map. Otherwise it gets the point's `Sector`, scans the sector's sorted `RegionRect` list, and returns the first rectangle that contains the point. If no rectangle matches, it returns `map.DefaultRegion`.

`Sector.OnEnter(Region, Rectangle3D)` inserts a `RegionRect`, sorts the list, and updates mobiles already in that sector. Sorting delegates to `Region.CompareTo`:

| Sort factor | Result |
| --- | --- |
| Dynamic flag | Dynamic regions sort before static regions. |
| Priority | Higher numeric `Priority` wins. |
| Child level | Deeper child regions win when priority ties. |

The stock default priority is `50`. `HouseRegion.HousePriority` is `Region.DefaultPriority + 1`, so dynamic house regions normally sort above nearby static regions.

## XML Schema
The current XML contains 7 facets and 419 top-level region nodes. No nested child-region nodes were found in the current file, but `Region.LoadRegions` supports nested `<region>` children.

| XML element or attribute | Parsed by | Behavior |
| --- | --- | --- |
| `<ServerRegions>` | `Region.Load()` | Required root element. Missing root logs a console message. |
| `<Facet name="...">` | `Region.Load()` | `name` is parsed with `Map.Parse`. `Map.Internal` is rejected. |
| `<region type="..." name="..." priority="...">` | `Region.LoadRegions()` / `Region` constructor | `type` resolves through `ScriptCompiler.FindTypeByName`; `name` becomes `Region.Name`; root regions can set `priority`. |
| `<rect x="..." y="..." width="..." height="...">` | `ReadRectangle3D` | Builds a rectangle from start plus width/height. Width/height are converted to the end coordinate by addition. |
| `<rect x1="..." y1="..." x2="..." y2="...">` | `ReadRectangle3D` | Builds a rectangle from explicit endpoints. |
| `<zrange min="..." max="...">` | `Region` constructor | Sets default Z bounds for all child rects in that region. Defaults are `Region.MinZ` and `Region.MaxZ`. |
| `zmin` / `zmax` on `<rect>` | `ReadRectangle3D` | Overrides the region-level Z bounds for that rectangle. |
| `<go x="..." y="..." z="...">` | `ReadPoint3D` | Sets `Region.GoLocation`. If omitted, the first rectangle center and map average Z are used. |
| `<music name="...">` | `ReadEnum<MusicName>` | Sets `Region.Music`; otherwise inherited `DefaultMusic` is used. |
| `<rune name="...">` | `BaseRegion` constructor | Sets `BaseRegion.RuneName`; `BaseRegion.GetRuneNameFor` walks parents for the first rune name. |
| `<logoutDelay active="...">` | `BaseRegion` constructor | `active="false"` sets `NoLogoutDelay = true`. Zero logout only applies when the mobile has no aggressors, has not aggressed, and is not criminal. |
| `<guards type="..." disabled="...">` | `GuardedRegion` constructor | Optional guard mobile type and initial disabled flag. Invalid non-mobile types fall back to `DefaultGuardType`. |
| `<spawning excludeFromParent="..." zLevel="...">` | `BaseRegion` constructor | Enables regional spawns, optionally blocks parent spawns, and chooses `Lowest`, `Highest`, or `Random` Z selection. |
| `<object id="..." type="..." amount="..." minSpawnTime="..." maxSpawnTime="...">` | `SpawnDefinition.GetSpawnDefinition` / `BaseRegion` | Creates a mobile or item spawn entry. Current XML uses 195 `<object>` entries. |
| `<home x="..." y="..." z="..." range="...">` | `BaseRegion` constructor | Optional spawn home point and random range for that spawn entry. |
| `<direction value="...">` | `BaseRegion` constructor | Optional direction assigned to spawned mobiles. |

## Current XML Inventory
| Region type | Count |
| --- | ---: |
| `DungeonRegion` | 127 |
| `CaveRegion` | 77 |
| `PirateRegion` | 37 |
| `OutDoorBadRegion` | 32 |
| `VillageRegion` | 32 |
| `PublicRegion` | 27 |
| `OutDoorRegion` | 20 |
| `SafeRegion` | 14 |
| `StartRegion` | 10 |
| `BardDungeonRegion` | 9 |
| `PrisonArea` | 7 |
| `ProtectedRegion` | 4 |
| `UnderHouseRegion` | 3 |
| `NecromancerRegion` | 3 |
| `MoonCore` | 3 |
| `GargoyleRegion` | 2 |
| `MazeRegion`, `LunaRegion`, `DungeonHomeRegion`, `DeadRegion`, `DawnRegion`, `CrashRegion`, `SavageRegion`, `SkyHomeDwelling`, `BargeDeadRegion`, `UmbraRegion`, `BardTownRegion`, `WantedRegion` | 1 each |

## BaseRegion Behavior
| Hook or property | Compiled behavior |
| --- | --- |
| `YoungProtected` | Defaults to `true`; `DungeonRegion` overrides it to `false`. |
| `GetLogoutDelay` | If `NoLogoutDelay` is set and the mobile is not in combat and not criminal, returns `TimeSpan.Zero`; otherwise delegates to `Region.GetLogoutDelay`. |
| `AllowHarmful` | Blocks direct PvE player attacks against PvP/Neutral players, blocks direct attacks against PvE players, blocks `Warriors` versus non-`Warriors` both directions, and blocks harmless citizen AI creatures from harming or being harmed. |
| `CanSpawn` | Walks the current region and parents. A region must allow spawning, and a matching `SpawnEntry.Definition.CanSpawn(...)` must be found unless a child excludes parent spawns. |
| `AcceptsSpawnsFrom` | Rejects parent-spawn inheritance when `ExcludeFromParentSpawns` is set and the source region is not this region. |
| `OnEnter` | For `PlayerMobile`: cleans buff icons, updates marching order, runs `RuneOfVirtue.MoralityCheck`, calls ghost helper for dead players with no quest arrow, forces profession `1` players to at least one kill, and runs `PlayerMobile.SkillVerification`. |
| `OnExit` | For `PlayerMobile`: cleans buff icons, calls `QuestTome.BossEscaped` when the player is not deleted and is not on `Map.Internal`, updates marching order, runs morality check, and performs dungeon-empty cleanup when the last player leaves a `DungeonRegion` or `BardDungeonRegion`. |
| Dungeon-empty cleanup | Unlocks locked `BaseDoor` items and moves unmanaged `BaseCreature` dwellers with `RangeHome < 10` back to `Home` when their current region name matches the exited region. |
| `RandomSpawnLocation` | Attempts 10 random points, validates map bounds, land/water/static/item surfaces, target region acceptance, blocking items, blocking statics, impassable land, and mobile collisions. |
| `ToString` | Returns `Name`, then `RuneName`, then type name. |

## Region Class Behavior
| Region class | Compiled behavior |
| --- | --- |
| `BardDungeonRegion` | Blocks housing, sets cave light, logs player enter/exit, and calls region music. |
| `BardTownRegion` | Blocks housing and prevents harmful actions when both sides are players, player-like NPCs/vendors, or player-owned/summoned/provoked creatures. |
| `BargeDeadRegion` | Blocks housing, sets dungeon light, and logs player enter/exit. |
| `CaveRegion` | Blocks housing, sets cave light, logs player enter/exit, and calls region music. |
| `CrashRegion` | Blocks housing, has zero logout delay, blocks player-vs-player harm, blocks spell casts, tells players they are near a crashed shuttle, and calls `PlayerSettings.SetSpaceMan` for non-GM players whose `SkillStart != 40000`. |
| `DawnRegion` | Blocks housing. Players with Elementalism, Magery, or Necromancy at least `80.0` are logged; others and their pets are teleported to `(3696,523,5)` on `Map.Sosaria`. |
| `DeadRegion` | Blocks housing, sets dungeon light, logs player enter/exit, and calls region music. |
| `DungeonHomeRegion` | Blocks housing, sets jail light, logs as `a Dungeon Dwelling`, and calls region music. |
| `DungeonRegion` | Blocks housing, disables young protection, optionally parses an `<entrance>`, sets dungeon light except for named outdoor-style dungeon regions, logs player enter/exit, and calls region music. |
| `GargoyleRegion` | Blocks housing, sets cave light only in `the Burning Mines`, logs player enter/exit, and calls region music. |
| `GuardedRegion` | Registers guard commands, blocks housing, blocks town spell casts unless `s.OnCastInTown(this)` succeeds, tracks 15-second guard-call candidates, responds to `*guards*` speech, and creates or reuses `TownGuards`. |
| `HouseRegion` | Dynamic per-house region. Enforces house access, ban/combat restrictions, customization restrictions, secure/container access, lockdown labels, house speech keywords, house decay exemptions, and zero logout for friends inside the house unless recent player combat heat exists. |
| `Jail` | Plain `BaseRegion` subclass; no additional compiled behavior. |
| `LunaRegion` | Blocks harmful actions against targets in `HouseRegion`. Applies the same `80.0` Elementalism/Magery/Necromancy moon access gate as `DawnRegion`. |
| `MazeRegion` | Blocks housing and logs player enter/exit. |
| `MoonCore` | Blocks housing and all spell casts, sets cave light, enforces moon access skill gates, and in `the Core of the Moon` deals 10000 damage to alive unblessed player-access mobiles. |
| `NecromancerRegion` | Blocks harmful actions against targets in `HouseRegion`, allows housing only for `GetPlayerInfo.EvilPlayer(from)`, sets dungeon light, logs player enter/exit, and calls region music. |
| `NoHousingRegion` | Reads `<smartNoHousing active="...">` into `SmartChecking`; `AllowHousing` returns that flag. |
| `OutDoorBadRegion` | Blocks housing, sets dungeon light on `Map.Underworld`, logs player enter/exit, and calls region music. |
| `OutDoorRegion` | Blocks housing except in `the Ranger Outpost` for players with Camping or Tracking at least `90`. Entering the Ranger Outpost sets the `RangerOutpost` player setting key. |
| `PirateRegion` | Logs player enter, calls region music, and teleports specific pirate/undead crew creatures back to their `Home` with effects when they exit. |
| `PrisonArea` | Blocks housing, has zero logout delay, sets dungeon light, blocks harmful actions and all spell casts, and opens `PrisonGump` for players on entry. |
| `ProtectedRegion` | Blocks housing, harmful actions, and all spell casts. Certain names set cave or dungeon light. Exiting `the Chamber of the Codex` resets `CodexWisdom` lens state when selected skills changed. |
| `PublicRegion` | Blocks housing, has zero logout delay, sets night light except for `the Lyceum`, `the Druid's Glade`, and `the Port`, allows harmful actions only between `Warriors`, and allows spell casts only when `Worlds.IsAllowedSpell(m, s)` and the region is not `the Chasm`. |
| `SafeRegion` | Blocks housing, blocks harmful actions except `Warriors` versus `Warriors`, restricts spells unless the spell is allowed or the region is in a hard-coded dock/lighthouse/fort allowlist, and deletes uncontrolled non-vendor/non-citizen water creatures entering several dock regions. |
| `SavageRegion` | Blocks housing, has zero logout delay, sets night light, blocks player-vs-player harm and all spell casts, and calls `PlayerSettings.SetSavage` for non-GM players whose `SkillStart != 11000`. |
| `SeaSpawnRegion` | Plain `BaseRegion` subclass; no additional compiled behavior. |
| `SkyHomeDwelling` | Blocks housing and logs enter/exit as `a Dwelling in the Sky`. |
| `StartRegion` | Blocks housing, zero logout delay, name-based light levels, blocks player-vs-player harm, blocks all spell casts, opens start/welcome gumps on entry, and opens MOTD on exit. |
| `TownRegion` | Guarded-region subclass that forces day light. No `TownRegion` entries are present in the current XML. |
| `UmbraRegion` | Blocks housing, sets night light, logs player enter/exit, and calls region music. |
| `UnderHouseRegion` | Allows housing, blocks harmful actions against targets in `HouseRegion`, sets night light, and logs all enter/exit calls. |
| `VillageRegion` | Allows housing only in `the Village of Barako`, allows all harmful actions, applies special light rules for Underworld/Grey Archeological Dig, logs player enter/exit, and calls region music. |
| `WantedRegion` | Blocks housing, has zero logout delay, sets cave light, blocks player-vs-player harm and all spell casts, displays jail-cell messages, and calls `PlayerSettings.SetWanted` for non-GM players. |

## Guard Commands
| Command | Access | Usage | Behavior |
| --- | --- | --- | --- |
| `CheckGuarded` | GameMaster | `[CheckGuarded` | Reports whether the caller is in a `GuardedRegion`, and whether guards are enabled there. |
| `SetGuarded` | Administrator | `[SetGuarded <true|false>` | Sets `Disabled` to the inverse of the supplied boolean. `true` enables guards, `false` disables them. |
| `ToggleGuarded` | Administrator | `[ToggleGuarded` | Flips `Disabled` for the caller's current `GuardedRegion`. |

Guard candidates are alive, unblessed player-access mobiles that are either criminal or red when `AllowReds` is false. A candidate timer lasts 15 seconds and sends localized messages when guards can or can no longer be called. Speech with keyword `0x0007` (`*guards*`) searches 14 tiles around the speech location and summons or redirects one guard against the first matching candidate.

## Regional Spawn System
The current XML declares 195 regional spawn entries, all as `<object>` nodes. The parsed type mix is:

| Spawn type | Entries | Configured amount |
| --- | ---: | ---: |
| `HiddenChest` | 96 | 2041 |
| `HiddenTrap` | 93 | 4045 |
| `DungeonChest` | 4 | 50 |
| `RavendarkStorm` | 2 | 115 |
| **Total** | **195** | **6251** |

Every current XML spawn entry uses `minSpawnTime="PT1S"` and `maxSpawnTime="PT1S"`.

### Spawn Timer Math
| Step | Formula or behavior |
| --- | --- |
| Timer interval | `Utility.RandomMinMax((int)MinSpawnTime.TotalSeconds, (int)MaxSpawnTime.TotalSeconds)` seconds. |
| Spawn batch size | `Math.Max((Max - SpawnedObjects.Count) / 3, 1)`. Integer division is used. |
| Completion check | `SpawnedObjects.Count >= Max`. |
| Timer state | A timer exists only while `Running == true` and the entry is incomplete. |
| Respawn | Deletes current uncontrolled spawn objects, spawns until complete or `i == Max`, sets `Running = true`, then schedules if needed. |
| Delete spawned objects | Deletes current uncontrolled spawn objects, clears the list, sets `Running = false`, and stops the timer. |

### Spawn Location Selection
| Step | Behavior |
| --- | --- |
| Rectangle normalization | Overlapping region rectangles are split into non-overlapping rectangles. Each final rectangle has weight `Width * Height`. |
| Non-home spawn | Picks a weighted random point from the normalized rectangles. |
| Home spawn | Picks `x` and `y` from `home +/- range`, then uses the first area rectangle containing that point for Z bounds. |
| Attempts | Tries up to 10 candidate points. Failure returns `Point3D.Zero`, and no object is spawned. |
| Surface collection | Adds valid land, static, and immovable item Z surfaces matching the spawn definition's land/water requirements. |
| Z choice | Uses `SpawnZLevel.Lowest`, `Highest`, or `Random` against the collected Z candidates. |
| Region acceptance | Rejects the point if `Region.Find(point, map).AcceptsSpawnsFrom(this)` is false. |
| Collision checks | Rejects blocking world items, statics, impassable land, and mobiles whose occupied Z overlaps `z..z + spawnHeight`. |

### Spawn Definitions
| XML node | SpawnDefinition | Behavior |
| --- | --- | --- |
| `<object type="MobileType">` | `SpawnMobile` | Instantiates the mobile type, infers land/water support once from a temporary instance, assigns `Home` and `RangeHome` for `BaseCreature`, optionally assigns direction, then calls spawn hooks and moves it to the world. |
| `<object type="ItemType">` | `SpawnItem` | Instantiates the item type, infers item height once from a temporary instance, then calls spawn hooks and moves it to the world. |
| `<treasureChest itemID="..." level="...">` | `SpawnTreasureChest` | Creates `BaseTreasureChest(itemID, level)` and uses the item table height for `itemID`. Not used by the current XML. |
| `<group name="...">` | `SpawnGroup` | Resolves a registered weighted group and delegates spawning to one weighted element. No group declarations are present in `Regions.xml`; groups would need to be registered in C# before XML load. |

## Spawn Commands
| Command | Access | Usage | Behavior |
| --- | --- | --- | --- |
| `RespawnAllRegions` | Administrator | `[RespawnAllRegions` | Respawns every `SpawnEntry` and sets all entries running. |
| `RespawnRegion` | GameMaster | `[RespawnRegion [<region name>]]` | Respawns the caller's current region, or a named region from the caller's map dictionary, and sets its entries running. |
| `DelAllRegionSpawns` | Administrator | `[DelAllRegionSpawns` | Deletes all spawned objects for every entry and stops all entries. |
| `DelRegionSpawns` | GameMaster | `[DelRegionSpawns [<region name>]]` | Deletes spawned objects for the current or named region and stops its entries. |
| `StartAllRegionSpawns` | Administrator | `[StartAllRegionSpawns` | Starts all regional spawn timers. Existing spawned objects remain. |
| `StartRegionSpawns` | GameMaster | `[StartRegionSpawns [<region name>]]` | Starts spawn timers for the current or named region. |
| `StopAllRegionSpawns` | Administrator | `[StopAllRegionSpawns` | Stops all regional spawn timers without deleting existing spawned objects. |
| `StopRegionSpawns` | GameMaster | `[StopRegionSpawns [<region name>]]` | Stops timers for the current or named region without deleting existing spawned objects. |

Named region commands call `from.Map.Regions.TryGetValue(name, out reg)`. Duplicate region names on one map log a warning during registration and keep the first registered name mapping.

## Serialization and Persistence
Static region definitions are not serialized. They are reconstructed from XML on each boot.

`SpawnPersistence` is a hidden internal `Item` with `ItemID = 1`, `Movable = false`, and default name `Region spawn persistence - Internal`. `SpawnEntry.Initialize()` calls `SpawnPersistence.EnsureExistence()` so the item exists before saves.

`SpawnPersistence.Serialize` writes encoded version `0`, then writes each entry ID and delegates to `SpawnEntry.Serialize`.

| Order | Serialized value | Reader |
| --- | --- | --- |
| 1 | Encoded version `0` | `ReadEncodedInt()` |
| 2 | Spawn entry count | `ReadInt()` |
| 3 | Entry ID | `ReadInt()` |
| 4 | Spawned object count | `ReadInt()` |
| 5 | Spawned object serials | `ReadInt()` repeated |
| 6 | `Running` flag | `ReadBool()` |
| 7 | Timer active flag | `ReadBool()` |
| 8 | `NextSpawn` delta time, only when timer active | `ReadDeltaTime()` |

On load, if an entry ID no longer exists in the current XML, `SpawnEntry.Remove(reader, version)` consumes the serialized fields and queues any found entities for deletion during `SpawnEntry.Initialize()`. If an entry still exists, serials are resolved through `World.FindEntity(serial)` and relinked with `spawn.Spawner = this`.

## Known Issues
* `BaseRegion.InitRectangles()` uses `int ez = rect.End.X` when splitting overlapping rectangles. That value is then used as the Z end for newly generated rectangles, so regional spawn Z bounds can be corrupted whenever overlapping area rectangles are normalized.
* `GuardedRegion.CheckGuardCandidate()` iterates `m.GetMobilesInRange(8)` directly and never calls `Free()` on the returned `IPooledEnumerable`. Other guard range scans in the same file correctly free their pooled enumerables.
* `BaseRegion.AllowHarmful()` wraps the PvE/PvP branch in `from is PlayerMobile && target is PlayerMobile`, but the branch then checks whether `from` is a `BaseCreature`. That creature-attacker code is unreachable in this region-layer hook, so controlled, summoned, or provoked creature attacks are not enforced by that intended block.
* `BaseRegion.OnExit()` unlocks every locked `BaseDoor` in `World.Items.Values` when the last player leaves any `DungeonRegion` or `BardDungeonRegion`. The door unlock is not scoped to the exited region.
* `GuardedRegion.MakeGuard()` silently swallows exceptions from guard construction. Bad guard constructors or runtime construction failures produce no diagnostic beyond guard absence.
* `StartRegion.OnEnter()` and `StartRegion.OnExit()` do not call `base.OnEnter` or `base.OnExit`, so those transitions bypass common `BaseRegion` player maintenance such as buff cleanup, morality checks, ghost helper checks, and skill verification.
