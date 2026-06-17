# SOURCE-BATCH-037 EverlastingBottle Guard Repair Closeout

## Summary

SOURCE-BATCH-037 implemented the guard-only EverlastingBottle repair in `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingBottle.cs`.

`OnDoubleClick` now returns before assigning thirst, sending the refill message, or playing sound when the interacting mobile is null/deleted or the bottle item is deleted.

## Scope Preserved

- ItemID `0x2827`
- Hue `0x849`
- Name `Everlasting Bottle`
- `Artefact` label
- `from.Thirst = 20`
- drink refill message
- `Utility.RandomList(0x30, 0x2D6)`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingBottle.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingBottle.cs`: 0
- Gated approval crossed: no

## Verification

- Targeted source scan confirmed the new `OnDoubleClick` guard.
- Behavior preservation scan confirmed ItemID `0x2827`, Hue `0x849`, Name `Everlasting Bottle`, `Artefact` label, `from.Thirst = 20`, the refill message, `Utility.RandomList(0x30, 0x2D6)`, serializer version write, and serializer version read remain present.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-037 is complete. `SOURCE-BATCH-038+` remains pending the next clean target from `source-batch-037-candidate-discovery.csv`: EverlastingLoaf Guard Repair.
