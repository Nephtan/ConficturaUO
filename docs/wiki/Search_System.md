# System Name: Search System

## Overview

The Search System is a custom geometric traversal helper library in `Server.Searches`. It does not search world `Item` or `Mobile` instances by type, name, serial, or range. The only compiled player-facing command is `[search`, which displays particle effects along hard-coded traversal patterns for debugging or visualization.

The runtime API is also used by the Random Encounter Engine's `SpawnFinder` to probe nearby candidate spawn tiles around a player.

Code-Verified: 2026-05-07

## Script Inventory

| Script | Namespace | Role |
| --- | --- | --- |
| `Data/Scripts/Custom/Searches/Searches.cs` | `Server.Searches` | Static API facade and initialization for octant, line, circle, concentric circle, and spiral helpers. |
| `Data/Scripts/Custom/Searches/Types.cs` | `Server.Searches` | Defines `DistanceFunction`, `SearchDirection`, `ClockDirection`, `Compass`, and the `Tour` delegate. |
| `Data/Scripts/Custom/Searches/BaseEntry.cs` | `Server.Searches` | Stores `(x, y)` offsets and computes geometric or linear distance. |
| `Data/Scripts/Custom/Searches/Helpers.cs` | `Server.Searches` | Provides `Trig.Theta(int x, int y)`. |
| `Data/Scripts/Custom/Searches/LineSearch.cs` | `Server.Searches` | Implements Bresenham-style line traversal between two `Point2D` endpoints. |
| `Data/Scripts/Custom/Searches/CircleSearch.cs` | `Server.Searches` | Builds radius-indexed circle perimeter entries and traverses them by angle. |
| `Data/Scripts/Custom/Searches/OctantSearch.cs` | `Server.Searches` | Builds compass-octant buckets from a 24-tile matrix. |
| `Data/Scripts/Custom/Searches/SpiralSearch.cs` | `Server.Searches` | Uses a generated square-spiral offset table through range 24. |
| `Data/Scripts/Custom/Searches/Commands.cs` | `Server.Scripts.Commands` | Registers `[search` and runs visualization subcommands. |

`Data/Scripts/Scripts.csproj` explicitly compiles the eight C# scripts above. The Python generator scripts and `compile.bat` are included as non-compiled content only.

## Public API

The `Search` facade creates the helper instances from `Search.Initialize()`. The system constructs `OctantSearch(24)`, `CircleSearch(24)`, `SpiralSearch()`, and `LineSearch()`.

| Method | Parameters | Traversal behavior |
| --- | --- | --- |
| `Search.Octant(Map map, Direction direction, int start, SearchDirection searchDirection, Tour tour)` | UO `Direction`, start range, in/out direction | Maps UO direction to a `Compass` octant and delegates to `OctantSearch.SearchOctant`. |
| `Search.Line(Map map, Point2D begin, Point2D end, Tour tour)` | Absolute begin/end points | Delegates to `LineSearch.SearchLine`. Coordinates passed to `tour` are absolute map coordinates. |
| `Search.Circle(Map map, int radius, Tour tour)` | Radius | Counterclockwise circle traversal around `(0, 0)` offsets. |
| `Search.Circle(Map map, int radius, ClockDirection clockDirection, Tour tour)` | Radius and clock direction | Same circle, optionally reversed for clockwise traversal. |
| `Search.ConcentricCircles(Map map, int start, SearchDirection searchDirection, Tour tour)` | Start radius and in/out direction | Intended to traverse multiple radius rings, but currently exits after the first non-empty ring. |
| `Search.Spiral(Map map, int start, SearchDirection direction, bool randomStart, Tour tour)` | Start range, in/out direction, first-ring randomization | Traverses generated square-spiral offsets grouped by linear distance. |

`Tour` is declared as:

```csharp
public delegate bool Tour(Map map, int x, int y);
```

In the compiled implementations, `tour(...) == true` means "stop and return true." If all visited points return false, the search method returns false.

## Command Surface

| Command | Access | Usage attribute | Description attribute | Parameters | Behavior |
| --- | --- | --- | --- | --- | --- |
| `[search octant` | `Player` | None | None | None | Draws particles for the caller's facing octant, using offsets relative to `Mobile.Location`. |
| `[search line` | `Player` | None | None | None | Draws particles on a fixed diagonal line from `(X - 3, Y - 3)` to `(X + 3, Y + 3)`. |
| `[search circle [radius]` | `Player` | None | None | Optional integer radius | Draws particles around the caller at the requested radius, defaulting to `5`. |
| `[search spiral` | `Player` | None | None | None | Draws particles around the caller using the outward square spiral starting at range `1`. |

The command handler does nothing for missing or unknown first arguments. It also sends no usage text to the caller.

All four subcommands use `Effects.SendLocationParticles` with item ID `0x37CC`, speed `1`, duration `40`, render mode `3`, effect `9917`, and an `EffectItem` placed at the traced land tile Z. `octant`, `line`, and `circle` use hue `96`; `spiral` increments the hue with each visited point.

## Traversal Mechanics

### Entry distance

`Entry` stores `X`, `Y`, and `Distance`.

| Distance function | Formula |
| --- | --- |
| `Geometric` | `(int)Math.Sqrt(x * x + y * y)` |
| `Linear` | `Math.Max(Math.Abs(x), Math.Abs(y))` |

`OctantEntry` and `CircleEntry` use geometric distance. `SpiralEntry` uses linear distance, so each spiral ring is a square shell rather than a circular radius.

### Line search

`LineSearch.SearchLine` is a Bresenham-style integer line walker:

1. If begin and end are identical, it calls `tour(map, x1, y1)` once.
2. It computes `dX = Math.Abs(x2 - x1)` and `dY = Math.Abs(y2 - y1)`.
3. It chooses X or Y as the independent variable based on whether `dX >= dY`.
4. It visits each integer point from begin through end.
5. It returns true immediately when `tour` returns true; otherwise it returns false after the full line is exhausted.

Unlike the other visual commands, `Search.Line` expects absolute map coordinates. The command supplies absolute caller-relative endpoints before invoking it.

### Circle search

`CircleSearch` precomputes search sets for radii `0..24`.

| Detail | Compiled behavior |
| --- | --- |
| Matrix generation | Bresenham circle generation writes eight symmetric points per step into a radius-indexed matrix. |
| Angle ordering | Each `CircleEntry` stores `Trig.Theta(x, y)` and is inserted into a `SortedList<double, CircleEntry>`. |
| Default direction | `Search.Circle(map, radius, tour)` uses `ClockDirection.CounterClockwise`. |
| Clockwise direction | Copies the sorted values into a list, reverses the list, then visits entries. |
| Return behavior | Returns true on the first `tour == true`; otherwise returns false after that circle's perimeter is exhausted. |

The `radius` argument is used directly as `m_SearchSets[radius]`; there is no public bounds guard.

### Octant search

`OctantSearch` precomputes a 49-by-49 matrix for offsets `-24..24` and assigns each offset to one of eight compass octants by angle. It groups entries by geometric distance inside each octant.

`Search.Octant` maps RunUO `Direction` values to `Compass` in this order:

| RunUO direction | Compass bucket |
| --- | --- |
| `North` | `North` |
| `Right` | `NorthEast` |
| `East` | `East` |
| `Down` | `SouthEast` |
| `South` | `South` |
| `Left` | `SouthWest` |
| `West` | `West` |
| `Up` | `NorthWest` |

The current implementation only traverses the first non-empty distance bucket it reaches. It does not continue inward or outward after every point in that bucket returns false.

### Spiral search

`SpiralSearch` uses a generated `m_SpiralPattern` table and groups offsets by linear distance through `MAXRANGE = 24`.

| Parameter | Behavior |
| --- | --- |
| `start` | Initial square-shell range. |
| `direction` | `Outwards` increments range; `Inwards` decrements range. |
| `randomStart` | On the first pass only, picks `Utility.RandomMinMax(0, entries.Count)` as the starting index. |
| `tour` return | True stops the traversal and returns true. |

When `randomStart` is false, outward searches start at index `0`; inward searches start at `entries.Count - 1`.

## Random Encounter Integration

`Data/Scripts/Custom/PvE/RandomEncounters/SpawnFinder.cs` is the only traced consumer outside the search package.

| SpawnFinder method | Search API used | Purpose |
| --- | --- | --- |
| `FindAhead(...)` | `Search.Octant(pm.Map, dir, distance, SearchDirection.Inwards, tour)` | Attempts to find a valid spawn tile in the octant ahead of a running player. |
| `FindInwards(...)` | `Search.Spiral(..., distance, SearchDirection.Inwards, true, tour)` | Searches from the outer requested range toward the player. |
| `FindOutwards(...)` | `Search.Spiral(..., distance, SearchDirection.Outwards, true, tour)` | Searches from the requested range away from the player. |

`SpawnFinder` converts each returned offset into a current point using the player's current `Location`, then rejects tiles outside the source region type, inside `HouseRegion`, mismatched to water/land requirements, or failing `Map.CanFit` / `Map.CanSpawnMobile` checks.

## Persistence

The Search System defines no `Item`, `Mobile`, attachment, Gump, or persistent world object. There are no `Serialize(GenericWriter writer)` or `Deserialize(GenericReader reader)` overrides in `Data/Scripts/Custom/Searches`.

## Known Issues

* The audit note says "Search commands for items/mobiles," but no compiled search script performs item or mobile lookup. The package is a geometry traversal library plus particle visualization command.
* `SearchCommands` exposes `[search` to `AccessLevel.Player` without `[Usage]` or `[Description]` attributes and without feedback for missing or unknown subcommands.
* `Searches.cs` comments say the search terminates when the delegate returns false, but the compiled implementations return true when `tour(...)` returns true and continue while it returns false.
* `[search circle <radius>]` accepts any parsed integer. Values below `0` or above `24` can index outside `CircleSearch.m_SearchSets`; a non-numeric second argument silently becomes radius `0` because `int.TryParse` writes the failed out value over the default `5`.
* `Search.Octant(...)` and `OctantSearch.SearchOctant(...)` do not validate the supplied direction or helper initialization state. A direct caller with an out-of-range `Direction` value can index outside `m_CompassLookup`.
* `OctantSearch.SearchOctant(...)` returns false immediately after scanning the first non-empty distance bucket. `SearchDirection.Inwards` and `SearchDirection.Outwards` therefore do not sweep multiple octant ranges unless earlier distance buckets are empty.
* `CircleSearch.SearchConcentricCircles(...)` also returns false after the first non-empty radius ring, so the public `Search.ConcentricCircles(...)` API does not actually traverse concentric circles.
* `SpiralSearch.SearchSpiral(...)` uses `Utility.RandomMinMax(0, entries.Count)` for first-ring randomization. `RandomMinMax` is inclusive, so `entries.Count` can be selected and skip the first ring entirely. The random start also does not wrap around to visit entries before the chosen index.
* `Search.Initialize()` catches construction exceptions, logs a fatal message, but does not rethrow. If initialization fails, later API calls can dereference null helper fields while the server continues running.
