# SOURCE-BATCH-382 BardsTaleNote Guard Repair Closeout

## Result

`SOURCE-BATCH-382` implemented `SB382-CAND-001`, a non-gated guard repair for `BardsTaleNote`.

## Source Change

- File: `Data/Scripts/Items/Books/BardsTaleNote.cs`
- Added an early return in `OnDoubleClick` when `e == null || e.Deleted || Deleted`.
- Added `e.Backpack == null` to the existing backpack-use failure path before `IsChildOf(e.Backpack)`.

## Preserved Behavior

BardsTaleNote randomized item ID/name/hue/weight, clue message choices, `ScrollMessage` persistence, `ClueGump` text and layout, backpack-use failure message, gump close/send behavior, sound `0x249` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Books/BardsTaleNote.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Books/BardsTaleNote.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved note-reading behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-382` committed as `e35abb78`. `SOURCE-BATCH-383+` should run fresh candidate discovery after `SOURCE-BATCH-382`.
