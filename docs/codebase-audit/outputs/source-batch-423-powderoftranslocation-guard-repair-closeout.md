# SOURCE-BATCH-423 PowderOfTranslocation Guard Repair Closeout

## Result

`SOURCE-BATCH-423` implemented `SB423-CAND-001`, a non-gated guard repair for PowderOfTranslocation interactions.

## Source Change

- File: `Data/Scripts/Items/Special/Solen Items/PowderOfTranslocation.cs`
- `PowderOfTranslocation.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source powder is deleted before range checks or target assignment.
- `InternalTarget.OnTarget(Mobile from, object targeted)` now returns immediately when `from` is null, `from` is deleted, `m_Powder` is null, or the source powder is deleted before range checks, TranslocationItem checks, localized messages, charge/recharge mutations, or powder deletion.

## Preserved Behavior

- Range requirement, target assignment, `TranslocationItem` interface contract, charge cap check, max recharge check, partial recharge delta math, full powder consumption `Delete` behavior, localized messages `1054137` through `1054140`, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB423-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick and OnTarget guards are present; target assignment, range message, charge/recharge rules, success/no-effect messages, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Special/Solen Items/PowderOfTranslocation.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Special/Solen Items/PowderOfTranslocation.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-423` source commit: `790920b1`. `SOURCE-BATCH-424+` should run fresh candidate discovery after `SOURCE-BATCH-423`.
