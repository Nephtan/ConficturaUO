# SOURCE-BATCH-429 ForgetfulGem Guard Repair Closeout

## Result

`SOURCE-BATCH-429` implemented `SB429-CAND-001`, a non-gated guard repair for ForgetfulGem crystal interaction.

## Source Change

- File: `Data/Scripts/Magic/Base/ForgetfulGem.cs`
- `ForgetfulGem.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source crystal is deleted before reading magic skills or opening the crystal gump.
- `CrystalGump.OnResponse(NetState state, RelayInfo info)` now returns immediately when response state/info is null or the response mobile is null/deleted before playing sounds, navigating gump pages, or resetting a magic skill.

## Preserved Behavior

Magery/Necromancy/Elementalism skill checks, `CrystalGump` text/page navigation, sound IDs `0x5C9`, `0x4A`, and `0x65C`, skill `BaseFixedPoint` reset to `0`, location effect `0x3039`, no-skill warm-touch message, stationary crystal item setup, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB429-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick and OnResponse guards are present; CrystalGump page sends, skill BaseFixedPoint reset, sound IDs, location effect, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Magic/Base/ForgetfulGem.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Magic/Base/ForgetfulGem.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-429` source commit: `d33ddc40` (`fix: guard ForgetfulGem interactions`). `SOURCE-BATCH-430+` should run fresh candidate discovery after `SOURCE-BATCH-429`.
