# SOURCE-BATCH-419 DartBoards Guard Repair Closeout

## Result

`SOURCE-BATCH-419` implemented `SB419-CAND-001`, a non-gated guard repair for Halloween dart board interactions.

## Source Change

- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/MongbatDartBoard.cs`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/DaemonDartBoard.cs`
- `MongbatDartBoard.OnDoubleClick(Mobile from)` and `DaemonDartBoard.OnDoubleClick(Mobile from)` now return immediately when `from` is null, `from` is deleted, or the source dart board is deleted.
- `MongbatDartBoard.Throw(Mobile from)` and `DaemonDartBoard.Throw(Mobile from)` now return immediately for the same stale/null mobile or deleted source-board state before reading the held weapon or applying scoring behavior.
- `OnMongbatReset`, `OnMongbatHit`, `OnMongbatNick`, `OnDaemonReset`, `OnDaemonHit`, and `OnDaemonNick` now return safely if the source board has been deleted before delayed item ID reset or sound callbacks run.

## Preserved Behavior

- Scoring thresholds and localized score messages, `BaseKnife`-only eligibility, range/line-of-sight and facing checks, animation choice, moving effect behavior, sounds, timer delays, item ID hit/reset behavior, addon/deed definitions, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB419-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick, Throw, delayed reset, delayed hit, and delayed nick guards are present in both dart board files; knife eligibility, range/line-of-sight checks, scoring messages, timers, and sound behavior remain present.
- Exact-file POST-BATCH-Y scan: passed; both dart board files have `0` gate hits.
- Exact-file active overlay scan: passed; both dart board files have `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer policy expansion, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild after rerunning outside the sandbox read restriction.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-419` source commit: `c8a06efe`. `SOURCE-BATCH-420+` should run fresh candidate discovery after `SOURCE-BATCH-419`.
