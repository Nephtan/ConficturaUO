# Champion Spawns

## Overview
Champion spawns create escalating waves of monsters that culminate in a champion boss. Each spawn type defines its own creature lists and unique rewards. Progress is represented by skull candles around the altar.

## Player Usage
- Fight through the waves to advance the altar. When the champion appears, defeat it to finish the event.
- Eligible participants may receive power scrolls or Scrolls of Transcendence. Champions can also drop rare artifacts and a Champion Skull.

## Staff Configuration
- Create a spawn with `[add ChampionSpawn]`; the idol, altar, and platform are placed automatically.
- Set the `Type` property or enable `RandomizeType` to rotate between spawn themes.
- Adjust `SpawnArea` and `ConfinedRoaming` to control where monsters wander.
- Use `Start` to activate the spawn or `Stop` to halt it. Spawns reset after completion.

## Example
1. `[add ChampionSpawn]` and set `Type` to `VerminHorde`.
2. After clearing waves, Barracoon appears and is slain, rewarding power scrolls and artifacts.

## Audience
Players & Staff