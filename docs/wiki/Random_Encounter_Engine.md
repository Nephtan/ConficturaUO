# Random Encounter Engine

This page documents the random encounter engine as the compiled C# currently behaves. The live encounter tables come from `Data/Scripts/Custom/PvE/RandomEncounters/RandomEncounters.xml`; the wiki is not the source of truth for per-region spawn lists.

## Key Scripts
- `Data/Scripts/Custom/PvE/RandomEncounters/EncounterEngine.cs`
- `Data/Scripts/Custom/PvE/RandomEncounters/Timers.cs`
- `Data/Scripts/Custom/PvE/RandomEncounters/SpawnFinder.cs`
- `Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs`
- `Data/Scripts/Custom/PvE/RandomEncounters/Records.cs`
- `Data/Scripts/Custom/PvE/RandomEncounters/Commands.cs`
- `Data/Scripts/Custom/PvE/RandomEncounters/RandomEncounters.xml`
- `Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs`

## Runtime Summary
- The engine loads `Data/Scripts/Custom/PvE/RandomEncounters/RandomEncounters.xml` on initialization.
- A reinitialize timer checks that file every 15 seconds and reloads the system after the file write time changes.
- Encounter timers are always created for `DungeonRegion`, `CaveRegion`, `BardDungeonRegion`, `OutDoorRegion`, and `VillageRegion`, even if the XML only defines some of those region types.
- Spawned mobiles and items receive a `RandomEncountersCreated` XML attachment so the cleanup timer can track them later.

## Current Live Root Settings
These values are read from the current checked-in `RandomEncounters.xml`:

| Setting | Current value | Effect |
| --- | --- | --- |
| `picker` | `all` | Every eligible player in the timer bucket gets an encounter roll. |
| `language` | `en-US` | Used for numeric parsing. |
| `skiphidden` | `true` | Hidden players are skipped. |
| `delay` | `120` | First tick delay, in seconds, for every encounter timer. |
| `interval` | `150:300:300` | Dungeon, outdoor, village timer intervals in seconds. `DungeonRegion`, `CaveRegion`, and `BardDungeonRegion` all use the first value. |
| `cleanup` | `300` | Cleanup sweep delay and repeat interval, in seconds. |
| `cleanupGrace` | `1` | Nearby players can block only one cleanup attempt before deletion is allowed. |
| `debug` | `true` | Enables verbose console logging. |
| `debugEffect` | `false` | Disables debug-only spawn placement particles. |

## Commands
All commands require `Administrator`.

| Command | Actual behavior |
| --- | --- |
| `[rand init]` | Rebuilds the region hash from XML and restarts encounter timers. |
| `[rand stop]` | Stops the encounter timers and cleanup timer. |
| `[rand now <RegionType>]` | Immediately runs one encounter pass for the supplied timer type string. |
| `[rand import]` | Runs the legacy import helper for premium spawn data. |

## Player Eligibility
- Only connected `PlayerMobile` instances are considered.
- Staff are skipped because access level must be exactly `Player`.
- Dead players are skipped.
- Hidden players are skipped only when `skiphidden="true"`.
- A player only enters the candidate set for the timer whose runtime region category matches `Helpers.RegionCategory(player.Region)`.
- `Helpers.RegionCategory` collapses regions into `VillageRegion`, `SafeRegion`, `CaveRegion`, `BardDungeonRegion`, `DungeonRegion`, or `OutDoorRegion`.

## Encounter Lookup And Roll Order
For each chosen player, the engine computes:
- Map name from `player.Map.Name`
- Region name from `player.Region.Name`, falling back to `default` when blank
- Time bucket from `Helpers.DetermineTimeForMobile`
- Land bucket from `Helpers.DetermineLandTypeForMobile`

Lookup order is:
1. Exact region name + exact land type + exact time
2. Exact region name + exact land type + `AnyTime`
3. Exact region name + `AnyLand` + `AnyTime`
4. `default` region + exact land type + exact time
5. `default` region + exact land type + `AnyTime`
6. `default` region + `AnyLand` + `AnyTime`

Encounter selection then works like this:
- The engine draws `Utility.RandomDouble()`.
- Encounter sets are ordered by numeric `p` ascending.
- The first set whose `p` is greater than or equal to the draw is the normal encounter bucket.
- If that bucket contains multiple encounters with the same `p`, one is chosen uniformly from that bucket.
- `p="*"` creates an inclusive bucket that is processed separately after the normal roll; every encounter in that bucket can also fire if its level gate passes.

## Level Gating
- Encounter `level` values are checked against `Helpers.CalculateLevelForMobile`.
- For players, that method now delegates to `CharacterLevelService.GetEncounterLevel`.
- The alias mapping is broader than the old wiki implied:
  - `Fighter` uses the highest result from martial, archer, assassin, ninja, samurai, knight, death knight, mystic monk, jedi, or syth lanes.
  - `Ranger` uses the highest result from ranger, druid, archer, or bard lanes.
  - `Mage` uses the highest result from arcane mage, elementalist, holy man, or researcher lanes.
  - `Necromancer` uses the highest result from necromancer, witch, or death knight lanes.
  - `Thief` uses the highest result from thief, assassin, ninja, or jester lanes.
  - `Overall` uses the player overall level.
- For non-player mobiles, the helper falls back to the legacy fame/karma/skills/stats formula.

## Spawn Placement And Aggro
- If the player is running, the engine first searches the octant ahead of the player.
- If that fails, it searches inward on a spiral around the player.
- Nested elements search outward from their parent object.
- Spawn checks reject tiles that cross into a different region type or a `HouseRegion`.
- Water encounters require a wet land tile and `Map.CanFit(...)`.
- Land encounters reject coastal wet statics and then probe static tile Z values, land tile Z, and player Z.
- `forceAttack="true"` only sets `Combatant` when the spawned `BaseCreature` treats the player as an enemy and is not using vendor AI.
- `scaleUp="true"` duplicates the top-level mobile up to two extra times based on `player Fighter level / spawned mobile Overall level`.

## XML Contract Actually Parsed
### Root attributes
- Parsed: `picker`, `language`, `skiphidden`, `delay`, `interval`, `cleanup`, `cleanupGrace`, `debug`, `debugEffect`, `RTFM`
- `RTFM="false"` aborts initialization on purpose.

### Region and encounter attributes
- `<Facet name="...">`
- `<Region type="..." name="...">`
- `<Encounter p="..." distance="min:max" landType="..." time="..." level="value[:LevelType]" scaleUp="...">`
- Deprecated `<Encounter water="true">` still works by forcing `landType="Water"`.

### Element attributes
- `<Mobile>` and `<Item>` parse `p`, `pick`, `n`, `id`, `forceAttack`, and `effect`
- `effect` may be `Smoke`, `Fire`, `Vortex`, `Swirl`, `Glow`, `Explosion`, or `None`
- `effect` also accepts a hue suffix such as `Glow:1153`
- `pick` is split on commas and one type is chosen per spawned element instance
- `n="min:max"` rolls how many sibling copies of that element are created

### Nesting rules
- `<Mobile>` can contain nested `<Mobile>`, `<Item>`, and `<Prop>`
- `<Item>` can contain nested `<Item>`, `<Mobile>`, and `<Prop>`, but the code explicitly rejects a nested `<Mobile>` under an `<Item>`
- A nested item is added to a parent mobile or container when possible; otherwise it is moved into the world

## Current XML Shape
- The checked-in XML defines facets for `Lodor`, `Sosaria`, `Underworld`, `SerpentIsland`, `SavagedEmpire`, and `Atlantis`.
- The live file is dungeon-focused. It defines `DungeonRegion`, `CaveRegion`, and a smaller number of `BardDungeonRegion` tables.
- The current XML does not use `<Prop>`, `p="*"`, `scaleUp="true"`, or deprecated `water="true"` entries.

## Cleanup Rules
- The cleanup timer starts after `cleanup` seconds and repeats every `cleanup` seconds.
- Objects younger than the cleanup window are ignored.
- Encounter items in the world can be deleted.
- Encounter items held by a mobile are detached from cleanup forever instead of being deleted later.
- Tamed encounter creatures are detached from cleanup forever.
- Untamed encounter creatures are only deleted when they have no `Combatant`.
- If a non-staff client is within 18 tiles, cleanup is delayed until the attachment counter exceeds `cleanupGrace`.

## Known C# Gaps
- `<Prop>` nodes are recognized during XML traversal, but `AddProp(...)` is a stub that only logs a message. Property mutation for encounter elements is not implemented.
- `DeleteTimer.MaybeRemove(...)` enumerates `GetClientsInRange(...)` without freeing the pooled enumerable, so cleanup sweeps leak pooled range iterators.

## Practical Editing Guidance
- Treat `RandomEncounters.xml` as the authoritative encounter table.
- Match `Map.Name`, region type, and region name strings exactly.
- Use `name="default"` only for the fallback table of a facet/region-type pair.
- Do not rely on `<Prop>` tags until the engine code is completed.
