# SOURCE-BATCH-421 Guillotine Guard Repair Closeout

## Result

`SOURCE-BATCH-421` implemented `SB421-CAND-001`, a non-gated guard repair for Guillotine interactions.

## Source Change

- File: `Data/Scripts/Items/Misc/Guillotine.cs`
- `Guillotine.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source guillotine is deleted.
- `Down1`, `Down2`, and `BackUp` now return safely if the source guillotine has been deleted before delayed animation/reset callbacks run.

## Preserved Behavior

- Range and line-of-sight check, localized reach failure message, `Visible`, `ItemID`, and `m_NextUse` eligibility, damage chance and `Utility.Dice(2, 10, 5)` damage amount, hurt sound, `Ouch!` public overhead message, blade sound `0x387`, blood creation, `Timer.DelayCall` timings, item ID down/reset behavior, deserialize reset behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB421-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick and delayed callback deleted-source guards are present; reach/LOS checks, damage behavior, sounds, timers, blood creation, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Misc/Guillotine.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Misc/Guillotine.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer policy expansion beyond deleted-source callback guards, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-421` source commit: pending. `SOURCE-BATCH-422+` should run fresh candidate discovery after `SOURCE-BATCH-421`.
