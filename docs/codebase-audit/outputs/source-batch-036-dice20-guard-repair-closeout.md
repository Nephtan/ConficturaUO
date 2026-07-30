# SOURCE-BATCH-036 Dice20 Guard Repair Closeout

## Summary

SOURCE-BATCH-036 implemented the guard-only Dice20 repair in `Data/Scripts/Items/Misc/Games/DandD/Dice20.cs`.

`OnDoubleClick` and `OnTelekinesis` now return before range checks, telekinesis effects, sound, or `Roll(from)` when the interacting mobile is null/deleted or the dice item is deleted.

## Scope Preserved

- ItemID `0x301A`
- name and weight
- `AddNameProperties` labels
- range `2`
- telekinesis effects and sound `0x1F5`
- roll overhead message
- `Utility.Random(1, 20)`
- sound `0x34`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Games/DandD/Dice20.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Misc/Games/DandD/Dice20.cs`: 0
- Gated approval crossed: no

## Verification

- Targeted source scan confirmed guards in `OnDoubleClick` and `OnTelekinesis`.
- Behavior preservation scan confirmed `twenty sided`, `Dungeons & Dragons`, telekinesis effects, sound `0x1F5`, `Utility.Random(1, 20)`, sound `0x34`, serializer version write, and serializer version read remain present.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-036 is complete. The `source-batch-031-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-037+` remains pending candidate discovery for the next clean non-gated target.
