# SOURCE-BATCH-433 RecipeScroll Guard Repair Closeout

## Result

`SOURCE-BATCH-433` implemented `SB433-CAND-001`, a non-gated guard repair for RecipeScroll double-click interaction.

## Source Change

- File: `Data/Scripts/Items/Trades/Misc/RecipeScroll.cs`
- `RecipeScroll.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source recipe scroll is deleted before reading range, recipe state, skill state, or recipe learning/deletion behavior.

## Preserved Behavior

Range 2 rule, localized reach failure `1019045`, `Recipe` property lookup, `PlayerMobile` learning requirement, `HasRecipe` check, `CraftItem.GetSuccessChance` skill gate, localized messages `1073451`, `1044153`, and `1073427`, `AcquireRecipe` behavior, scroll `Delete()` semantics, `RecipeID` persistence, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one recommended `SB433-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick guard is present; range check, reach message, recipe lookup, skill gate, recipe acquisition, scroll deletion, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Trades/Misc/RecipeScroll.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Trades/Misc/RecipeScroll.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-433` source commit: pending. `SOURCE-BATCH-434+` should run fresh candidate discovery after `SOURCE-BATCH-433`.
