# Change Season Command

## Overview
`[ChangeSeason]` is an administrator-only command that changes the `Season` value on the map where the caller is standing. After updating the map, it pushes a season refresh to every connected mobile currently on that same map.

## Supported Values
The command accepts exactly one argument:

- `Spring` -> season `0`
- `Summer` -> season `1`
- `Autumn` or `Fall` -> season `2`
- `Winter` -> season `3`
- `Desolation` -> season `4`

Any other input, or no input, causes the command to stop and send its usage message.

## Usage
```text
[ChangeSeason Winter]
```

Access level: `Administrator`

## Code-Verified Behavior
1. `ChangeSeason.Initialize()` registers the `ChangeSeason` command at `AccessLevel.Administrator`.
2. `ChangeSeason_OnCommand` requires exactly one argument.
3. The argument is lowercased and mapped to an integer season ID.
4. The caller's current `Map.Season` field is set to that integer.
5. The script iterates `NetState.Instances` and only updates clients whose `Mobile` is on the same `Map`.
6. Each matching client receives `SeasonChange.Instantiate(ns.Mobile.GetSeason(), true)`, then `SendEverything()` is called to refresh world state.
7. The caller receives a confirmation message in the form `{MapName} season changed to {Input}.`

## Notes
- The refresh packet uses `Mobile.GetSeason()`, which in the base `Mobile` implementation returns `m_Map.Season` when the mobile is on a map.
- The command updates only the caller's current map. It does not change other maps.
- The built-in usage message in the script is missing a closing `]`, but the accepted command syntax above reflects the intended input.

## Audience
Staff

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0082.
- Backlog rows: RB-06673.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/ChangeSeason/ChangeSeason.cs (CurrentFile)
- Data/System/Source/Network/Packets.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=1; Initialize=1.
- Data/Scripts/Custom/ChangeSeason/ChangeSeason.cs:L10 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/ChangeSeason/ChangeSeason.cs:L12 Command CommandSystem.Register access=Unknown

### Serialization Evidence

- No serialized classes matched the reviewed source set in serialization-register.csv.

### Project And Runtime Coverage

- Data/Scripts/Custom/ChangeSeason/ChangeSeason.cs=Keep
- Data/Scripts/Custom/ChangeSeason/ChangeSeason.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
