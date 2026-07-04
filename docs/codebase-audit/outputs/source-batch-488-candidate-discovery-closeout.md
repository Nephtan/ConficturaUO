# SOURCE-BATCH-488 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-488+` was pending fresh candidate discovery after `SOURCE-BATCH-487`. Discovery selected one zero-gate, zero-overlay WheatSheaf target guard candidate for implementation.

## Recommended Candidate

`SB488-CAND-001` / `SOURCE-BATCH-488` / `WheatSheaf Guard Repair`

- File: `Data/Scripts/Items/Food/Cooking.cs`
- System: `Items:Food / Cooking / WheatSheaf`
- Behavior: add stale/null mobile, deleted source wheat sheaf, and deleted target item guards to `WheatSheaf.OnDoubleClick(Mobile from)` and `WheatSheaf.OnTarget(Mobile from, object obj)`.
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`

## Skipped Candidate Families

- Magic spell target callbacks: mostly combat, travel, buff/debuff, summon, or progression surfaces, so they require separate grouping and risk review before sweeping.
- `SmallTent.cs`, basement doors, moonstones, moongates, teleporters, runebooks, and bag-of-sending files: travel, housing, map, or region-policy surfaces.
- `BarkeepContract.cs`, `CommodityDeed.cs`, `DisguiseKit.cs`, `BasePotion.cs`, bandages, fishing nets, archery buttes, and transmutation spells: vendor/housing, economy, player-state, consumable, healing, reward, skill-gain, or progression workflows better handled by separate target selection.
- Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization candidates.

## Result

`SOURCE-BATCH-488` should implement `SB488-CAND-001` only if pre-commit verification confirms the source diff stays guard-only and does not alter flour mill eligibility, flour quantity math, wheat consumption, or serialization.
