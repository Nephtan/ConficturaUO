# SOURCE-BATCH-487 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-487+` was pending fresh candidate discovery after `SOURCE-BATCH-486`. Discovery selected one zero-gate, zero-overlay cooking interaction and delayed-callback guard candidate for implementation.

## Recommended Candidate

`SB487-CAND-001` / `SOURCE-BATCH-487` / `CookableFood Guard Repair`

- File: `Data/Scripts/Items/Food/CookableFood.cs`
- System: `Items:Food / CookableFood`
- Behavior: add stale/null mobile, deleted source food, and delayed-timer guards to `CookableFood.OnDoubleClick(Mobile from)`, `InternalTarget.OnTarget(Mobile from, object targeted)`, and `InternalTimer.OnTick()`.
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`

## Skipped Candidate Families

- `BasementDoor.cs`, tents, moonstones, moongates, teleporters, runebooks, and bag-of-sending files: travel, housing, map, or region-policy surfaces.
- `BarkeepContract.cs` and `CommodityDeed.cs`: vendor/housing or resource-redemption workflows.
- `DisguiseKit.cs`: appearance, gump, timer, and player-state behavior is a wider workflow.
- `BasePotion.cs`, bandages, fishing nets, archery buttes, and transmutation spells: broad consumable, healing, reward, skill-gain, or progression surfaces better handled by separate target selection.
- Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization candidates.

## Result

`SOURCE-BATCH-487` should implement `SB487-CAND-001` only if pre-commit verification confirms the source diff stays guard-only and does not alter cooking recipes, heat-source eligibility, ingredient consumption, skill checks, timer delay, cooked-food output, or serialization.
