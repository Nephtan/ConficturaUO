# Invasion System

## Overview
The Invasion System orchestrates large-scale city assaults across multiple maps. Staff can trigger events that spawn coordinated waves of invaders following preset paths toward target cities.

## Commands
- `[invasion]` — opens the City Invasion gump. Requires `Administrator` access.

## Starting an Invasion
1. Run `[invasion]` and choose the destination city.
2. The gump teleports you to the city and presents **Start** and **Stop** options.
3. Selecting **Start an Invasion** calls `CreateEntry` for each defined route. `CreateEntry` places a `Spawner` and optional `WayPoint` chain, setting spawn counts, delays, team ID, and creature type.

## Stopping and Cleanup
- Use the **Stop an Invasion** button in the city gump to remove active spawners and waypoints.
- Invasion stones placed in each city also provide cleanup helpers. For example, the Britain stone deletes all spawners and waypoints named `BritInvasionSosaria` or `BritInvasionLodor` when its stop methods are invoked.
- Double‑clicking an active stone informs nearby players that the city is under attack.

## Customization
- City scripts reside in `Data/Scripts/Custom/Invasion System/`. For each city, edit the corresponding `StartStop*.cs` file to adjust waypoints, spawn amounts, or creature types used in `CreateEntry` calls.
- Invasion stones can be customized or duplicated for new locations by altering the cleanup names and message text.

## Audience
Staff