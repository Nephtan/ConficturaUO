# XML Spawner Enhancements

## Overview
The `XMLSpawner` package is a large staff-facing content toolkit built around the `XmlSpawner` item class in [`Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs`](../../Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs). It is not just a spawn-file importer. The package includes:

- `XmlSpawner`: a custom spawner item with trigger logic, smart spawning, import/export, and a dedicated gump.
- `BaseXmlSpawner`: shared reflection, property-setting, keyword-tag, and parsing helpers used by the spawner and related tools.
- `XmlAttach` and many `XmlAttachment` subclasses: attachment storage and trigger hooks for items and mobiles.
- `XmlEdit`: an editor for `XmlDialog` attachments only.
- `XmlFind`: a search gump for finding world objects, spawners, spawn entries, and attachments.
- `XmlQuestHolder`, `XmlQuestNPC`, `XmlQuestBook`, and related classes: a built-in quest subsystem shipped inside the same package.
- `PacketHandlerOverrides`: optional packet-handler replacement for XML text-entry books and attachment-aware item use.

## Core Spawner Behavior
`XmlSpawner` is an `Item` that stores its own spawn list, timers, trigger state, and persistence. The current serialized version is `31`. It supports more than simple timed spawning, including:

- Timed spawn delays, refractory windows, spawn durations, and despawn timers.
- Proximity, speech, skill, item, mobile, and player-property triggers.
- Sequential spawning, subgroup progression, per-entry timing, and smart-spawning activation.
- Relative or fixed home range, waypoint/region targeting, and external trigger mode.
- Spawn-file import/export with per-spawner GUIDs so normal loads can replace matching spawners while `XmlNewLoad*` creates new GUIDs instead.

The in-game `XmlSpawnerGump` exposes run state, respawn, bring-home, goto, props, reset, sort, subgroup data, and per-entry controls. `XmlAdd` creates `XmlSpawner` items and can place them directly into a targeted container or into the world at the targeted location.

## Commands And Access
The package registers more commands than the original wiki listed.

### GameMaster-level
- `[XmlAdd [-defaults]]`: opens the add gump and can reset the caller's saved defaults before opening it.
- `[XmlEdit]`: targets an object and edits its `XmlDialog` attachment; if no dialog exists, it offers to attach one first.
- `[XmlFind [objecttype] [range]]`: opens the search tool.
- `[XmlHome [go][gump][send]]`: targets a spawned item/mobile, resolves its owning `XmlSpawner`, then prints the spawner coordinates and optionally teleports the caller, opens the spawner gump, or sends the object back home.
- `[XmlGet <property>]`: reads a public property from a targeted object.
- `AddAtt`, `DelAtt`, `[GetAtt]`, and `[AvailAtt]`: attachment management tools.
- `XmlSpawnerRespawn`, `XmlSpawnerRespawnAll`, `SmartStat`, and `XmlGo`.

### Administrator / Disk-access level
`XmlSpawner.DiskAccessLevel` defaults to `Administrator`.

- `XmlLoad`, `XmlLoadHere`, `XmlNewLoad`, `XmlNewLoadHere`, `XmlUnLoad`.
- `XmlSave`, `XmlSaveAll`, `XmlSaveOld`.
- `XmlImportSpawners`, `XmlImportMSF`, `XmlImportMap`.
- `XmlSpawnerShowAll`, `XmlSpawnerHideAll`, `XmlSpawnerWipe`, `XmlSpawnerWipeAll`.
- `XmlDefaults`, `XmlSet`, and `OptimalSmartSpawning`.

## Files And Directories
- Spawn save/load commands look in the `Spawns` directory first. If it does not exist, they fall back to the server root.
- `XmlSpawner.ConfigFile` plus setting `LoadConfig = true` loads a runtime XML config from `SpawnerConfigs`.
- `XmlAdd` preset save/load does **not** use `SpawnerConfigs`. Its per-staffer `.defs` files are written to and read from `SpawnerDefs` when that folder exists, otherwise the server root.
- `XmlImportSpawners` reads Sno exporter files from `Saves/Spawners/<filename>`.
- `XmlImportMSF` reads the path provided on the command line directly.

## Attachments And Packet Hooks
`XmlAttach` maintains global attachment tables for items, mobiles, and attachment serials. It hooks `WorldLoad`, `WorldSave`, speech, and movement events so attachments can persist and react to world activity.

`PacketHandlerOverrides.Initialize()` activates two packet overrides by default:

- Book content changes (`0x66`) are redirected to `XmlTextEntryBook.ContentChange`.
- Use requests (`0x06`) are redirected to `XmlAttach.UseReq`, which preserves normal use behavior and adds attachment `OnUse` handling.

The quest-button override exists in code but is commented out, so the package does **not** currently replace the paperdoll quest button handler.

## Quest Subsystem
This package also includes quest helpers:

- `XmlQuestNPC` derives from `TalkingBaseCreature`, generates a random male/female outfit, can hear ghosts, and serializes at version `0`.
- `XmlQuestHolder` is an abstract `Container` implementing `IXmlQuest`. It stores quest objectives, descriptions, states, reward item/attachment metadata, repeatability, journal entries, party-sharing settings, and config-file loading. Its current serialized version is `6`.

These quest classes are part of the XMLSpawner package, but they are separate from the core `XmlSpawner` item workflow.

## Code-Verified Notes
- `XmlEdit` edits `XmlDialog` attachments only. It is not a generic attachment editor.
- `XmlLoad` replaces existing spawners when GUIDs match; `XmlNewLoad` and `XmlNewLoadHere` always create new GUIDs instead.
- `XmlLoadHere` and `XmlNewLoadHere` can relocate loaded spawners relative to the caller, but only within the optional `-maxrange` limit.
- `XmlSaveOld` writes the legacy `Objects` column format; the normal save path writes the newer `Objects2` format.
- `BaseXmlSpawner.Initialize()` protects `Mobile.AccessLevel` and `Item.StaffLevel` from being set through spawner property operations.

## Known Code Issues
- `XmlFind.SearchCriteria` assigns `Dosearchtok = doter` instead of `Dosearchter = doter`, so the Ter Mur map filter is wired incorrectly.
- The quest-button override is implemented but intentionally disabled in `PacketHandlerOverrides`, so paperdoll quest-button integration is not active.

## Audience
Staff

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0133.
- Backlog rows: RB-06801.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=35; Gump=1; Initialize=1; Movement=2; Speech=1; Timer=8.
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L2251 Gump SendGump access=Internal
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L3943 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4031 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4092 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4119 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4193 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4302 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4431 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4491 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4496 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4501 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:L4506 Command CommandSystem.Register access=Unknown
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 2.
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:Server.Mobiles.SpawnObject version=Unknown serialize=L15662 deserialize=L alignment=CountMismatch:Writes=20;Reads=0
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:Server.Mobiles.XmlSpawner version=31 serialize=L14792 deserialize=L14928 alignment=CountMismatch:Writes=74;Reads=120

### Project And Runtime Coverage

- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs=Keep
- Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
