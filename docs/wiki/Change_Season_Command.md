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
