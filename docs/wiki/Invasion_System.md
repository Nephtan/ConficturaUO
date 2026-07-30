# Invasion System

## Overview
The invasion system is an administrator-only gump workflow, not a single reusable engine. The `[invasion]` command opens `CityInvasion`, which lists invasion targets across four maps and teleports the caller to the chosen city while opening that city's `StartStop*` gump.

## Command Flow
- `[invasion]` is registered in `CityInvasion.Initialize()` for `AccessLevel.Administrator`.
- `CityInvasion` has four pages of destinations:
  - `Sosaria`: Britain, Minoc, Cove, Buccaneer's Den, Trinsic, Vesper, Nu'Jelm, Skara Brae, Magincia, Yew
  - `Lodor`: Britain, Minoc, Cove, Buccaneer's Den, Trinsic, Vesper, Nu'Jelm, Skara Brae, Magincia, Yew
  - `Underworld`: Mistas, Montor, Reg Volon, Savage Camp, Gargoyle City, Lakeshire/Mireg, Ancient Citadel, Terort Skitas
  - `SerpentIsland`: Luna, Umbra
- Each destination button sets the caller's `Map` and `Location`, plays sound `0x1FA`, fires location effect `14201`, sends a flavor message, and opens a city-specific `StartStop*` gump.

## How City Scripts Actually Work
- Every city has its own `StartStop*.cs` gump under one of these folders:
  - `Data/Scripts/Custom/Invasion System/Sosaria/`
  - `Data/Scripts/Custom/Invasion System/Lodor/`
  - `Data/Scripts/Custom/Invasion System/Underworld/`
  - `Data/Scripts/Custom/Invasion System/SerpentIsland/`
- Pressing `Start an Invasion` does not invoke a shared data-driven invasion engine.
- Most city scripts manually create `WayPoint` items, link them with `NextPoint`, create one or more `Spawner` items, assign `Spawner.WayPoint`, set a shared `Name`, move everything into the world at fixed coordinates, and call `Respawn()`.
- `StartStopBrittram` is the notable exception: it has a local `CreateEntry(...)` helper that builds a `Spawner` plus an optional waypoint loop from a `List<Point3D>`. That helper is local to that one gump and is not reused by the rest of the system.
- Start scripts finish by broadcasting that the chosen city is under invasion and returning the caller to `CityInvasion`.

## Cleanup Behavior
- Pressing `Stop an Invasion` instantiates the matching stone class and calls a `Stop*` method such as `StopBritSosaria()`.
- The stone classes do the real cleanup. Each `Stop*` method scans `World.Items` for placed instances of that stone type and then calls a matching `CleanUp*` method.
- Cleanup deletes every `Spawner` and `WayPoint` whose `Name` matches the invasion's shared token, such as `BritInvasionSosaria` or `BuccaneersDenInvasionLodor`.
- Double-clicking a placed invasion stone only sends a status message like `Britian is being invaded`.
- Because stop logic iterates existing world stones, cleanup depends on at least one corresponding invasion stone already being placed in the world.

## Representative Script Patterns
- `StartStopBrittram` builds twelve named entries for Britain on `Map.Sosaria`, including routed `Overseer` spawners plus stationary `IronDragon`, `RunicGolemInvader`, `MetalDaemon`, `MechGargoyle`, and `LordBlackThornBot` spawners.
- `StartStopBuccaneersDenfel` manually creates long waypoint chains and ten spawners for `Map.Lodor`, including `OrcBomber`, `Orc`, `OrcishLord`, `OrcCaptain`, `OrcBrute`, and `OrcCamp`.
- `StartStopLunaSerpentIsland` uses a much smaller four-spawner layout on `Map.SerpentIsland`.

## Verified Defects
- The `Lodor` Nu'Jelm button opens `StartStopNujelmtram`, so its start and stop actions operate on `NujelmInvasionSosaria` instead of the `Lodor` version.
- All `Underworld` start scripts currently place their spawners and waypoints on `Map.SerpentIsland`, even though `CityInvasion` teleports the administrator to `Map.Underworld`.
- `StartStopAncientCitadelilsh` names one spawner `AncientCitadelInvasionIlshendar` instead of `AncientCitadelInvasionUnderworld`, so the stone cleanup routine will not delete that spawner.

## Audience
Staff

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0032.
- Backlog rows: RB-06709.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/Invasion System/Sosaria/ (CurrentDirectory)
- Data/Scripts/Custom/Invasion System/Lodor/ (CurrentDirectory)
- Data/Scripts/Custom/Invasion System/Underworld/ (CurrentDirectory)
- Data/Scripts/Custom/Invasion System/SerpentIsland/ (CurrentDirectory)

### Runtime Evidence

- Hook summary: Gump=120.
- Data/Scripts/Custom/Invasion System/Lodor/StartStopBritFelucca.cs:L31 Gump OnResponse access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartStopBritFelucca.cs:L39 Gump SendGump access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartStopBritFelucca.cs:L689 Gump SendGump access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartStopBritFelucca.cs:L701 Gump SendGump access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartstopBuccaneersDenFelucca.cs:L31 Gump OnResponse access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartstopBuccaneersDenFelucca.cs:L39 Gump SendGump access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartstopBuccaneersDenFelucca.cs:L449 Gump SendGump access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartstopBuccaneersDenFelucca.cs:L461 Gump SendGump access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartstopCoveFelucca.cs:L31 Gump OnResponse access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartstopCoveFelucca.cs:L39 Gump SendGump access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartstopCoveFelucca.cs:L406 Gump SendGump access=Internal
- Data/Scripts/Custom/Invasion System/Lodor/StartstopCoveFelucca.cs:L418 Gump SendGump access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- No serialized classes matched the reviewed source set in serialization-register.csv.

### Project And Runtime Coverage

- Data/Scripts/Custom/Invasion System/Lodor/StartStopBritFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartStopBritFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopBuccaneersDenFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopBuccaneersDenFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopCoveFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopCoveFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopMaginciaFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopMaginciaFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopMinocFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopMinocFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopNujelmFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopNujelmFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopSkaraBraeFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopSkaraBraeFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopTrinsicFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopTrinsicFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopVesperFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopVesperFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopYewFelucca.cs=Keep
- Data/Scripts/Custom/Invasion System/Lodor/StartstopYewFelucca.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
