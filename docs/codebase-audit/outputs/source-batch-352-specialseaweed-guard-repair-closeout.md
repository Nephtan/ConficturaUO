# SOURCE-BATCH-352 SpecialSeaweed Guard Repair Closeout

## Result

`SOURCE-BATCH-352` implemented `SB352-CAND-001`, a non-gated guard repair for `SpecialSeaweed`.

## Source Change

- File: `Data/Scripts/Items/Trades/Fishing/SpecialSeaweed.cs`
- Added an early return when `from == null || from.Deleted || Deleted`.
- Added a missing-backpack guard that sends the existing empty-bottle message before returning.
- Preserved the existing behavior that does not require the seaweed item itself to be in the backpack.

## Preserved Behavior

Randomized seaweed names, hues, `SkillNeeded` values, `Stackable`, `Amount`, `Seafaring` skill check range, empty-bottle message, `PlaySound(0x240)`, every potion `AddToBackpack` mapping, success/failure messages, source `Consume()` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Fishing/SpecialSeaweed.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Trades/Fishing/SpecialSeaweed.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed.
- Exact-file POST-BATCH-Y scan: passed during preflight.
- Exact-file active overlay scan: passed during preflight.
- Serializer diff scan: passed with no serialization changes.
- Forbidden-surface diff scan: passed with no gated surface changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with only repository LF-to-CRLF working-tree warnings.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-352` was committed as `d0db52d7` with `fix: guard SpecialSeaweed interactions`. `SOURCE-BATCH-353+` should run fresh candidate discovery next.
