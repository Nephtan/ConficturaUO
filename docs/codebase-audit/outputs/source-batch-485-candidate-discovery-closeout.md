# SOURCE-BATCH-485 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-485+` was pending fresh candidate discovery after `SOURCE-BATCH-484`. Discovery selected one zero-gate, zero-overlay crafting-tool interaction guard candidate for implementation.

## Recommended Candidate

`SB485-CAND-001` / `SOURCE-BATCH-485` / `BaseTool Guard Repair`

- File: `Data/Scripts/Items/Trades/Tools/BaseTool.cs`
- System: `Items:Trades / Tools / BaseTool`
- Behavior: add stale/null mobile and deleted source tool guards to `BaseTool.OnSingleClick(Mobile from)`, `BaseTool.OnDoubleClick(Mobile from)`, and `BaseTool.OnDoubleClickRedirected(Mobile from, object o)` before durability labels, captcha dispatch, craft-gump dispatch, or callback backpack checks run.
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`

## Skipped Candidate Families

- `SmallWaxPot.cs`: exact-file active overlay rows remain in the audit backlog, so it is not clean for this cycle.
- Boat, house, moongate, teleporter, runebook, recall-rune, waypoint, tent, moonstone, and bag-of-sending files: travel, housing, map, or region-policy surfaces.
- Food, potions, bandages, fishing nets, commodity deeds, barkeep contracts, and transmutation spells: economy, reward, combat, vendor, or progression surfaces better handled by separate target selection.
- Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization candidates.

## Result

`SOURCE-BATCH-485` should implement `SB485-CAND-001` only if pre-commit verification confirms the source diff stays guard-only and does not alter craft eligibility, captcha policy, valid craft-gump dispatch, tool use counts, or serialization.
