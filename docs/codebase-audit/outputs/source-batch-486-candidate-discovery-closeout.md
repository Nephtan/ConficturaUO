# SOURCE-BATCH-486 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-486+` was pending fresh candidate discovery after `SOURCE-BATCH-485`. Discovery selected one zero-gate, zero-overlay potion-cauldron interaction guard candidate for implementation.

## Recommended Candidate

`SB486-CAND-001` / `SOURCE-BATCH-486` / `BrewCauldron Guard Repair`

- File: `Data/Scripts/Items/Potions/Special/BrewCauldron.cs`
- System: `Items:Potions / Special / BrewCauldron`
- Behavior: add stale/null mobile and deleted source cauldron guards to `BrewCauldron.OnDoubleClick(Mobile from)` before range checks, backpack reads, bottle consumption, potion creation, use-count mutation, or empty-cauldron visual updates run.
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`

## Skipped Candidate Families

- `CommodityDeed.cs`: commodity redemption is an economy/resource-surface workflow, so it should be a separate target if selected.
- `DisguiseKit.cs`: appearance, gump, timer, and player-state behavior is a wider workflow than this cycle.
- `BasePotion.cs`, bandages, fishing nets, and transmutation spells: broad consumable, healing, reward, or progression surfaces better handled by separate target selection.
- Boat, house, moongate, teleporter, runebook, recall-rune, waypoint, tent, moonstone, and bag-of-sending files: travel, housing, map, or region-policy surfaces.
- Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization candidates.

## Result

`SOURCE-BATCH-486` should implement `SB486-CAND-001` only if pre-commit verification confirms the source diff stays guard-only and does not alter potion output, bottle consumption, use counts, decay timers, or serialization.
