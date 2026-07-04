# SOURCE-BATCH-480 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-480+` was pending fresh candidate discovery after `SOURCE-BATCH-479`. Discovery selected one zero-gate, zero-overlay display guard candidate for implementation.

## Recommended Candidate

`SB480-CAND-001` / `SOURCE-BATCH-480` / `RunicSewingKit Label Guard Repair`

- File: `Data/Scripts/Items/Trades/Tools/RunicSewingKit.cs`
- System: `Items:Trades / Tools / RunicSewingKit`
- Behavior: add stale/null mobile and deleted source item guards to `RunicSewingKit.OnSingleClick(Mobile from)` before sending the localized runic sewing kit label.
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`

## Skipped Candidate Families

- `BaseSuit.cs` and clone backpack access paths: access-policy sensitive behavior.
- Homestead/cooking/crops/brewing/juicing/winecrafting files: crafting/economy and homestead policy-adjacent.
- Government, invasion, and staff-gift files: gated policy surfaces.
- Merchant/vendor books and item purchase files: economy/reward behavior.
- Quest, region, and major quest files: region/map and quest progression behavior.
- Animal cage and pack animal files: pet/follower/control behavior.
- Broad runic crafting behavior in `BaseRunicTool.cs`: crafting/economy and attribute-generation behavior.

## Result

`SOURCE-BATCH-480` should implement `SB480-CAND-001` only if pre-commit verification confirms the source diff stays guard-only and does not alter valid label text, runic resource behavior, uses, crafting behavior, or serialization.
