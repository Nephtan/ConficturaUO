# Agent Instructions for Premium Spawner Maps

This directory stores the `.map` files consumed by the PremiumSpawner engine. Each file defines a collection of PremiumSpawner entries that can be loaded, saved, or removed in-game with the `[spawngen ...]` command suite. Use this guide when you need to inspect, extend, or create map files.

## Directory Scope

* Applies to every `.map` file in `Data/Spawns/`.
* Maps are grouped by theme (e.g., `towns.map`, `pirates.map`). Keep related spawns together when adding new entries.

## File Structure Overview

Each map is plain text and is processed sequentially by the PremiumSpawner engine.

```
## Optional comment describing the section or region
*|List1|List2|List3|List4|List5|List6|X|Y|Z|Facet|MinDelay|MaxDelay|WalkRange|HomeRange|SpawnID|Count1|Count2|Count3|Count4|Count5|Count6
```

* Lines that start with `##` are human-readable comments. They are ignored by the loader and are perfect for region names, revision notes, and explanations.
* Spawner lines always start with `*|` and contain **21 pipe-separated data segments** after the leading asterisk. Each line defines a PremiumSpawner that can host up to six "fake" sub-spawners.

### Creature Lists (Slots 1–6)

* The first six segments (`List1` … `List6`) describe each fake spawner.
* Separate multiple creature types in the same slot with `:` to build a weighted random list. Repeating a creature increases its relative weight (e.g., `Spectre:Spectre:Wraith`).
* Leave a slot blank (`| |`) when you do not need that fake spawner.

### Location & Facet

* `X|Y|Z` – world coordinates for the spawner origin. Capture them in-game with `[where]` or `[get location]`.
* `Facet` – numeric map selector:
  * `0` Felucca **and** Trammel
  * `1` Felucca only
  * `2` Trammel only
  * `3` Ilshenar
  * `4` Malas
  * `5` Tokuno

### Timing & Ranges

* `MinDelay` / `MaxDelay` – respawn window. Defaults to minutes, but you may append `s` or `h` for seconds or hours (e.g., `10s`).
* `WalkRange` – how far spawned mobiles may roam.
* `HomeRange` – radius around the spawner where mobiles initially appear. Never exceed `WalkRange`.

### Spawn Identity & Counts

* `SpawnID` groups related spawners. Use a unique ID per map or encounter so you can unload them with `[spawngen unload <ID>]`.
* `Count1` … `Count6` – number of creatures to spawn for each fake spawner slot. Counts only apply when the corresponding creature list is non-empty.

## Override Directives

You can preface blocks of spawners with override commands. Each override affects all subsequent lines until another override of the same type appears.

* `overridemap <facet>` – force all following spawners to load on the given facet value.
* `overrideid <id>` – force all following spawners to use the specified SpawnID.
* `overridemintime <value>` / `overridemaxtime <value>` – replace individual delay values. Accepts the same suffixes (`s`, `m`, `h`).

Include overrides before the relevant comment header when you want a whole section (e.g., an entire dungeon) to share the same facet, timing, or ID.

## Editing Guidelines

* Maintain explanatory `##` headers for every region or encounter. When adding spawners, document the area name and purpose.
* Verify coordinates and elevations in-game to avoid unreachable placements (e.g., in water or inside static geometry).
* Keep `HomeRange ≤ WalkRange`. Values that violate this rule are ignored in-game.
* When building weighted random lists, avoid mixing hostile mobs with benign creatures or items in the same slot. Leaving unkillable entries alive will prevent the spawner from repopulating hostile mobs.
* Use overrides to assign consistent SpawnIDs instead of editing every line manually. This simplifies unloading and maintenance.
* When saving spawns in-game (`[spawngen save ...]`), rename the generated file before running another save command; new saves overwrite existing filenames.

## Testing & Maintenance

Because the PremiumSpawner system runs inside the game server, you cannot execute automated tests here. After editing a map file:

1. Load it on a development shard with `[spawngen <file>.map]`.
2. Verify spawn placement, counts, and timing in-game.
3. Use `[spawngen unload <SpawnID>]` to clean up if adjustments are needed.

Document any novel conventions or pitfalls in this AGENTS file so future contributors can keep map behavior consistent.