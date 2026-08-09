# Obsolete Script Collection

## Scope

This page documents the legacy compatibility scripts under `Data/Scripts/System/Obsolete/`.
These files are still compiled by `Data/Scripts/Scripts.csproj`, so the collection is not an inert archive.

The collection is a maintenance reference for developers and shard operators. It is not a player guide and should not be treated as a single gameplay system.

## Compiled Files

| File | Role |
| --- | --- |
| `GardenTool.cs` | Legacy herbalism harvest tool that uses the obsolete herbalism harvest system and schedules itself for deletion after loading from a save. |
| `Herbalism.cs` | Cooking-based plant harvest system with static tile lists for leaves, flowers, lilies, mushrooms, cactus, and grass. |
| `HerbalistCauldron.cs` | Legacy herbal potion cauldron with recipe memory, water/reagent state, and potion mixing targets. |
| `MixingCauldron.cs` | Legacy necrotic alchemy cauldron with ingredient slots, recipe memory, filled jar output, and the `MixingSpoon` item. |
| `Obsolete.cs` | Large aggregated source file containing old compatibility items, books, mobiles, faction code, ethics code, chat code, old quest framework pieces, MyRunUO support, virtues, Treasures of Tokuno rewards, and other imported systems. |
| `Pegasus.cs` | Standalone legacy pegasus mobile. |
| `PlantHerbalism.cs` | Temporary herbalism resource items that restore item art from their names, then schedule deletion after deserialization. |
| `Sphinx.cs` | Standalone legacy sphinx mobile with breath and petrifying melee effects. |
| `SurgeonsKnife.cs` | Legacy forensic corpse-harvesting tool that schedules itself for deletion after loading from a save. |

## Obsolete.cs Inventory

`Obsolete.cs` is an aggregation of many older scripts. Its namespaces include `Server.Items`, `Server.Mobiles`, `Server.Factions`, `Server.Ethics`, `Server.Engines.Quests`, `Server.Engines.Chat`, `Server.Engines.MyRunUO`, `Server.Engines.VeteranRewards`, and shared `Server` utility namespaces.

Important families inside the file include:

| Family | Examples |
| --- | --- |
| Compatibility items and books | `TribalPaint`, `TribalBerry`, `CharacterDatabase`, `SavageBook`, `WelcomeBookWanted`, `WelcomeBookElf`, `DoorTimeLord` |
| Legacy mobiles | Dragon, wyrm, bear, beetle, familiar, and other creature variants, plus standalone old creature definitions. |
| Factions | Faction base classes, towns, guards, vendors, traps, elections, town stones, sigils, finance and sheriff gumps, and faction commands. |
| Ethics and virtues | Hero/evil ethics powers, ethic persistence, virtue gumps, and virtue helpers. |
| Quest framework | `BaseQuester`, quest regions, objectives, conversations, quest serializer, and old quest gumps. |
| Chat and MyRunUO | Packet-based chat handlers, chat request hook, MyRunUO character/status update timers, and public/private character commands. |
| Treasures of Tokuno | Lesser and greater artifact reward tables, turn-in/redeem gumps, and persistence item. |

## Active Hooks

Although the folder is named obsolete, several classes still register startup behavior when compiled:

| Area | Hook or command surface |
| --- | --- |
| Legacy chat | Packet handlers for chat window open and chat actions, plus a separate chat request event hook. |
| Ethics | Speech event hook for ethic command phrases. |
| Factions | Login/logout hooks, recurring timers, and administrator commands such as `FactionElection`, `FactionCommander`, `FactionItemReset`, `FactionReset`, and `FactionTownReset`. |
| Faction generation | `GenerateFactions` administrator command. |
| MyRunUO | Recurring character/status update timers plus `UpdateMyRunUO`, `PublicChar`, `PrivateChar`, and `UpdateWebStatus`. |
| Towns | `GrantTownSilver` administrator command. |
| Treasures of Tokuno | Internal persistence item creation. |
| Herbalism | Startup sorting of the obsolete herbalism tile lists. |

## Save Compatibility Notes

Several classes in this collection exist primarily to consume old serialized world data safely.
Examples include `TribalPaint` and `TribalBerry`, which delete themselves during deserialization, and the obsolete herbalism tools/resources, which schedule delayed deletion after loading.

`CharacterDatabase` remains more sensitive because it serializes many character state fields, including quest, toolbar, title, poison, spell hue, and display settings. Any cleanup of that type should preserve read order until all live saves have been migrated.

## Maintenance Guidance

Do not remove or rename these compiled types as a bulk cleanup without a save-compatibility pass.
Work through the collection by subsystem, confirm whether a maintained replacement exists, and keep legacy read paths until old saves can no longer reference them.

For player-facing documentation, prefer the maintained replacement pages. This page exists to explain why the obsolete folder still matters during audit, cleanup, and world-save maintenance.

## Known Issues

| Issue | Impact |
| --- | --- |
| The `System/Obsolete` directory is still compiled even though the audit row labels it obsolete. | Cleanup work can affect runtime behavior and world loading, so removal is a C# migration task rather than a documentation-only deletion. |
| `Obsolete.cs` mixes compatibility loaders, active startup hooks, command registrations, old feature implementations, and imported systems in one file. | It is hard to reason about which parts are dead code, save shims, or still reachable at runtime. |
| Several obsolete item classes intentionally delete themselves during or shortly after deserialization. | Renaming or removing those classes before old saves are consumed can prevent the intended cleanup path from running. |
| `CharacterDatabase` still stores many legacy character fields in positional RunUO serialization order. | Field cleanup needs versioned migration work; ad hoc edits can shift save data and corrupt later reads. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0048.
- Backlog rows: RB-06734.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/System/Obsolete/ (CurrentDirectory)
- Data/Scripts/Scripts.csproj (CurrentFile)

### Runtime Evidence

- Hook summary: Command=11; Event=7; Gump=93; Initialize=19; Movement=5; Packet=2; Region=5; Speech=2; Timer=36; WorldSave=1.
- Data/Scripts/System/Obsolete/GardenTool.cs:L45 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/System/Obsolete/Herbalism.cs:L474 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Obsolete/HerbalistCauldron.cs:L570 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/System/Obsolete/HerbalistCauldron.cs:L1393 Gump SendGump access=Internal
- Data/Scripts/System/Obsolete/MixingCauldron.cs:L589 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/System/Obsolete/MixingCauldron.cs:L1525 Gump SendGump access=Internal
- Data/Scripts/System/Obsolete/MixingCauldron.cs:L3287 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/System/Obsolete/Obsolete.cs:L1657 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/System/Obsolete/Obsolete.cs:L1770 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/System/Obsolete/Obsolete.cs:L1772 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/System/Obsolete/Obsolete.cs:L2071 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/System/Obsolete/Obsolete.cs:L2346 Movement OnMovement access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 184.
- Data/Scripts/System/Obsolete/GardenTool.cs:Server.Items.GardenTool version=0 serialize=L35 deserialize=L41 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/System/Obsolete/HerbalistCauldron.cs:Server.Items.HerbalistCauldron version=0 serialize=L510 deserialize=L540 alignment=CountMatchNeedsTypeReview:UnknownWrites=24
- Data/Scripts/System/Obsolete/MixingCauldron.cs:Server.Items.MixingCauldron version=0 serialize=L527 deserialize=L558 alignment=CountMatchNeedsTypeReview:UnknownWrites=25
- Data/Scripts/System/Obsolete/MixingCauldron.cs:Server.Items.MixingSpoon version=0 serialize=L3268 deserialize=L3275 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/System/Obsolete/Obsolete.cs:Server.Items.AmethystWyrm version=0 serialize=L1333 deserialize=L1339 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/System/Obsolete/Obsolete.cs:Server.Items.AncientFarmersKasa version=2 serialize=L20521 deserialize=L20528 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/System/Obsolete/Obsolete.cs:Server.Items.AncientNightmare version=0 serialize=L1537 deserialize=L1543 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/System/Obsolete/Obsolete.cs:Server.Items.AncientSamuraiDo version=0 serialize=L20585 deserialize=L20592 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/System/Obsolete/Obsolete.cs:Server.Items.AncientUrn version=0 serialize=L21401 deserialize=L21409 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/System/Obsolete/Obsolete.cs:Server.Items.ArmsOfTacticalExcellence version=0 serialize=L20640 deserialize=L20647 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/System/Obsolete/Obsolete.cs:Server.Items.AssassinShroud version=0 serialize=L2906 deserialize=L2912 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/System/Obsolete/Obsolete.cs:Server.Items.AxeBeak version=0 serialize=L36255 deserialize=L36261 alignment=AlignedByCountAndKnownTypes
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/System/Obsolete/GardenTool.cs=Keep
- Data/Scripts/System/Obsolete/GardenTool.cs=Keep
- Data/Scripts/System/Obsolete/Herbalism.cs=Keep
- Data/Scripts/System/Obsolete/Herbalism.cs=Keep
- Data/Scripts/System/Obsolete/HerbalistCauldron.cs=Keep
- Data/Scripts/System/Obsolete/HerbalistCauldron.cs=Keep
- Data/Scripts/System/Obsolete/MixingCauldron.cs=Keep
- Data/Scripts/System/Obsolete/MixingCauldron.cs=Keep
- Data/Scripts/System/Obsolete/Obsolete.cs=Keep
- Data/Scripts/System/Obsolete/Obsolete.cs=Keep
- Data/Scripts/System/Obsolete/Pegasus.cs=Keep
- Data/Scripts/System/Obsolete/Pegasus.cs=Keep
- Data/Scripts/System/Obsolete/PlantHerbalism.cs=Keep
- Data/Scripts/System/Obsolete/PlantHerbalism.cs=Keep
- Data/Scripts/System/Obsolete/Sphinx.cs=Keep
- Data/Scripts/System/Obsolete/Sphinx.cs=Keep
- Data/Scripts/System/Obsolete/SurgeonsKnife.cs=Keep
- Data/Scripts/System/Obsolete/SurgeonsKnife.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
