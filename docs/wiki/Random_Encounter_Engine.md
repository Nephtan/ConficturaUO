# Random Encounter Engine

## Overview
The Random Encounter Engine spawns dynamic mobile or item encounters near players based on online population, region type, and probability settings defined in an XML configuration.

## Commands
- `[rand init]` — load the XML configuration and start encounter timers.
- `[rand stop]` — halt all encounter timers and cleanup.
- `[rand now <RegionType>]` — immediately check a timer type such as `DungeonRegion`, `OutDoorRegion`, or `VillageRegion`.
- `[rand import]` — build a configuration template from `Data/Import/Premiums` map files.

All `rand` subcommands require `Administrator` access.

## Configuration
The engine reads `Data/Scripts/Custom/RandomEncounters/RandomEncounters.xml`. Key attributes on the `<RandomEncounters>` root tag:

- `picker` (`sqrt` or `all`) – how many players are considered each interval.
- `language` – culture used to parse numeric values.
- `skiphidden` – whether hidden players are excluded.
- `delay` – seconds to wait before the first encounter checks.
- `interval` – colon‑separated timer values for dungeon, wilderness, and village regions.
- `cleanup` – seconds before spawned groups are removed.
- `cleanupGrace` – number of failed cleanup attempts allowed.
- `debug` / `debugEffect` – optional logging and visual markers.

### Facets and Regions
Inside the root element, define `<Facet name="…">` elements containing `<Region>` entries. Regions specify a `type` such as `OutDoorRegion`, `DungeonRegion`, or `VillageRegion`, and a `name`. If no specific region matches, a region named `default` is used.

### Encounters
Within each region, `<Encounter>` elements describe possible spawns. Useful attributes include:

- `p` – probability of the encounter; `*` forces the encounter in addition to any other.
- `distance` – spawn distance from the player, optionally `min:max`.
- `landType` – `Water`, `OnRoad`, `OffRoad`, or `AnyLand`.
- `time` – `Day`, `Night`, `Twilight`, or `AnyTime`.
- `level` – minimum character level; `level:Class` sets a class requirement (`Fighter`, `Ranger`, `Mage`, `Necromancer`, `Thief`, or `Overall`).
- `scaleUp` – `true` scales weaker encounters toward the player's strength.

### Mobiles and Items
`<Mobile>` and `<Item>` tags define actual spawns and may be nested. Attributes:

- `p` – chance the element appears.
- `pick` – comma‑separated list to randomly choose from.
- `n` – quantity or `min:max` range.
- `effect` – optional visual effect (`Smoke`, `Fire`, `Vortex`, `Swirl`, `Glow`, `Explosion`).
- `forceAttack` – for mobiles, `true` forces immediate aggression.

Nested mobiles spawn near their parent mobile; nested items are placed inside their parent container.

## Example
```
<RandomEncounters interval="600:600:600" cleanup="300">
  <Facet name="Lodor">
    <Region type="OutDoorRegion" name="default">
      <Encounter p="0.01" distance="7" landType="AnyLand" time="AnyTime">
        <Mobile pick="Mongbat" n="1"/>
      </Encounter>
    </Region>
  </Facet>
</RandomEncounters>
```
This configuration checks every ten minutes and has a 1% chance to spawn a single Mongbat near a random outdoor player in Lodor.

## Audience
Staff