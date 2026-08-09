# SOURCE-BATCH-358 SerpentSpawners Guard Repair Closeout

## Result

`SOURCE-BATCH-358` implemented `SB358-CAND-001`, a non-gated guard repair for `SerpentSpawners`.

## Source Change

- File: `Data/Scripts/Quests/Serpents/SerpentSpawners.cs`
- Added early returns when `from == null || from.Deleted || Deleted`.
- Added guarded local backpack reads and existing blue/red no-serpent messages for missing or deleted backpacks.

## Preserved Behavior

Item metadata, item hues/names/weights, `BlackrockSerpentOrder` and `BlackrockSerpentChaos` lookup types, `SerpentOfOrder` and `SerpentOfChaos` spawn classes, spawn location/map, sound `0x217`, source item `Delete` behavior, blue/red no-serpent messages, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Serpents/SerpentSpawners.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Serpents/SerpentSpawners.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed with one recommended row and no missing required fields.
- Targeted source scan: passed for two stale/null mobile/source-item guards, two guarded backpack reads, two missing/deleted backpack failure branches, both blue/red no-serpent messages, both serpent item lookup types, both serpent spawn classes, spawn location/map, sound `0x217`, source item `Delete`, and serialization method presence.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed with no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed with no command, event hook, packet handler, timer, gump policy, region, serializer, namespace, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with `0` warnings and `0` errors.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: completed with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`.

## Commit

`SOURCE-BATCH-358` committed as `b4e4a92b` with message `fix: guard SerpentSpawners interactions`. `SOURCE-BATCH-359+` should run fresh candidate discovery next.
