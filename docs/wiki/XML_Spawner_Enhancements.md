# XML Spawner Enhancements

## Overview
The XML Spawner system extends RunUO's standard spawners with XML-driven definitions, smart spawning, and attachment support. Spawners can be saved to disk, imported from map files, and edited in-game through gumps and commands. This toolset is primarily for shard staff to control dynamic world content.

## Commands
- `[XmlAdd]` — open a gump to create spawners with default settings.
- `[XmlEdit]` — edit XmlDialog entries attached to an object.
- `[XmlFind <objecttype> [range]]` — search the world for items or mobiles matching criteria.
- `[XmlHome [go|gump|send]]` — target a spawner to move, open its gump, or teleport to its home.
- `[XmlLoad <file> [prefix]]` / `[XmlLoadHere <file>]` — load spawner definitions from the `Spawns` directory. Saving uses `[XmlSave]`, `[XmlSaveAll]`, or `[XmlSaveOld]`.
- `[XmlImportSpawners <file>]`, `[XmlImportMSF <file>]`, `[XmlImportMap <dir>]` — import map-based spawners for conversion to XML.
- `[GetAtt <target>]`, `[AvailAtt]`, and the `AddAtt`/`DelAtt` targeting commands manage attachments on items or mobiles.

## Configuration
- Spawn files save to the `Spawns` directory; config files load from `SpawnerConfigs`.
- Disk operations require `Administrator` access level (`DiskAccessLevel`).
- Default spawner values include 5–10 minute delays, relative home ranges, and one spawn count by default.

## Example
```
[XmlAdd
```
Use the gump to set `SpawnRange` and `HomeRange`, then place the spawner. Save all active spawners with:
```
[XmlSaveAll MySpawnFile
```
Later, reload them using:
```
[XmlLoad MySpawnFile
```

## Audience
Staff