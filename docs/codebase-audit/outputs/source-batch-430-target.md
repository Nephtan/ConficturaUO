# SOURCE-BATCH-430 SongBook Guard Repair

## Target

- Candidate: `SB430-CAND-001`
- System: `Magic:Bard / SongBook`
- File: `Data/Scripts/Magic/Bard/SongBook.cs`
- Behavior: add a stale/null mobile and deleted source-book guard to `SongBook.OnDoubleClick(Mobile from)` before existing range check and `SongBookGump` open behavior.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

`SpellbookType.Song`, `BookOffset` `351`, `BookCount` `16`, range requirement `1`, `SongBookGump` close/open behavior, `Instrument` field persistence, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

Targeted source scan, exact gate/overlay scans, serializer diff scan, forbidden-surface diff scan, `Data/System/Source/Server.csproj` Debug/x86 build, `.\ConficturaServer.exe -compileonly -nocache`, `git diff --check`, and generated root artifact restoration before staging.
