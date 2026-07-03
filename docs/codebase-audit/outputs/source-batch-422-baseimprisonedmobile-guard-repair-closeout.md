# SOURCE-BATCH-422 BaseImprisonedMobile Guard Repair Closeout

## Result

`SOURCE-BATCH-422` implemented `SB422-CAND-001`, a non-gated guard repair for BaseImprisonedMobile interactions.

## Source Change

- File: `Data/Scripts/Items/Special/BaseImprisonedMobile.cs`
- `BaseImprisonedMobile.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source imprisoned-mobile item is deleted before reading `from.Backpack`, sending `ConfirmBreakCrystalGump`, or sending the backpack-use failure message.

## Preserved Behavior

- Backpack requirement, `ConfirmBreakCrystalGump` creation/send, localized backpack-use message `1042001`, `Summon` abstract property, `Release` virtual hook, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB422-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick mobile/source guard is present; confirmation gump send, backpack failure message, Release hook, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Special/BaseImprisonedMobile.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Special/BaseImprisonedMobile.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-422` source commit: pending. `SOURCE-BATCH-423+` should run fresh candidate discovery after `SOURCE-BATCH-422`.
