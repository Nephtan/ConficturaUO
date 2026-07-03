# SOURCE-BATCH-433 RecipeScroll Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-433`
- Candidate: `SB433-CAND-001`
- System: `Items:Trades / RecipeScroll`
- File: `Data/Scripts/Items/Trades/Misc/RecipeScroll.cs`

## Behavior

Add an early stale/null guard to `RecipeScroll.OnDoubleClick(Mobile from)` before existing range, recipe, skill, learn, message, and scroll deletion logic.

Allowed source change:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Range 2 rule, localized reach failure `1019045`, `Recipe` property lookup, `PlayerMobile` learning requirement, `HasRecipe` check, `CraftItem.GetSuccessChance` skill gate, localized messages `1073451`, `1044153`, and `1073427`, `AcquireRecipe` behavior, scroll `Delete()` semantics, `RecipeID` persistence, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Fence Result

Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Misc/RecipeScroll.cs`: `0`.

Exact-file active overlay rows for `Data/Scripts/Items/Trades/Misc/RecipeScroll.cs`: `0`.

No gated approval is crossed.
