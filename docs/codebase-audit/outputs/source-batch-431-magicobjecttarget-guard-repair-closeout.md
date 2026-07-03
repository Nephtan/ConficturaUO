# SOURCE-BATCH-431 MagicObjectTarget Guard Repair Closeout

## Result

`SOURCE-BATCH-431` implemented `SB431-CAND-001`, a non-gated guard repair for MagicObjectTarget dispatch.

## Source Change

- File: `Data/Scripts/Magic/Misc/MagicObjectTarget.cs`
- `MagicObjectTarget.OnTarget(Mobile from, object targeted)` now returns immediately when `from` is null, `from` is deleted, the stored source magic object is null, or the stored source magic object is deleted before dispatching to `BaseMagicObject.DoMagicObjectTarget`.

## Preserved Behavior

Target range `6`, harmful flag `false`, `TargetFlags.None`, `BaseMagicObject.DoMagicObjectTarget` dispatch, targeted object pass-through, `BaseMagicObject.OnMagicObjectUse` target assignment, `BaseMagicObject.DoMagicObjectTarget` rules, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB431-CAND-001` row.
- Targeted source scan: passed; OnTarget guard is present; Target constructor settings, BaseMagicObject.DoMagicObjectTarget dispatch, BaseMagicObject.OnMagicObjectUse target assignment, and BaseMagicObject.DoMagicObjectTarget rules remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Magic/Misc/MagicObjectTarget.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Magic/Misc/MagicObjectTarget.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-431` source commit: `7dc34d16` (`fix: guard MagicObjectTarget interactions`). `SOURCE-BATCH-432+` should run fresh candidate discovery after `SOURCE-BATCH-431`.
