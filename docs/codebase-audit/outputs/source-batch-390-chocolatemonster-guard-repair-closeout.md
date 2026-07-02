# SOURCE-BATCH-390 ChocolateMonster Guard Repair Closeout

## Result

`SOURCE-BATCH-390` implemented `SB390-CAND-001`, a non-gated guard repair for `ChocolateMonster`.

## Source Change

- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/HalloweenBag.cs`
- Added an early return in `ChocolateMonster.OnDoubleClick` when `m == null || m.Deleted || Deleted`.

## Preserved Behavior

ChocolateMonster randomized candy names, item IDs, hues, weight, sound `Utility.Random(0x3A, 3)`, human unmounted animation, phrase choices, `PrivateOverheadMessage` behavior, `Stam`, `Hits`, `Mana`, `Thirst`, and `Hunger` assignments, source `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Gifts/Holiday/Halloween/HalloweenBag.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Gifts/Holiday/Halloween/HalloweenBag.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guard and preserved ChocolateMonster candy consumption behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-390` committed as `1f95e98b`. `SOURCE-BATCH-391+` should run fresh candidate discovery after `SOURCE-BATCH-390`.
