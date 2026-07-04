# SOURCE-BATCH-483 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-483+` was pending fresh candidate discovery after `SOURCE-BATCH-482`. Discovery selected one zero-gate, zero-overlay display guard candidate for implementation.

## Recommended Candidate

`SB483-CAND-001` / `SOURCE-BATCH-483` / `BasePiece Label Guard Repair`

- File: `Data/Scripts/Items/Misc/Games/BasePiece.cs`
- System: `Items:Misc / Games / BasePiece`
- Behavior: add stale/null mobile and deleted source item guard coverage to `BasePiece.OnSingleClick(Mobile from)` before the final base label dispatch, while preserving existing orphan-board delete and reparent behavior.
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`

## Skipped Candidate Families

- Broad base item classes such as `BaseArmor.cs`, `BaseClothing.cs`, and `BaseRunicTool.cs`: wide shared behavior or crafting/economy surfaces.
- Boat, house, moongate, teleporter, runebook, and recall-rune files: travel/housing/policy surfaces.
- `WayPoint.cs`: command/GM waypoint workflow and map behavior.
- `BagOfSending.cs`: region/translocation and charge-use behavior.
- `Cloth.cs`: exact-file gate/overlay clean, but already has a committed source-batch row from `SOURCE-BATCH-104`.
- Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization candidates.

## Result

`SOURCE-BATCH-483` should implement `SB483-CAND-001` only if pre-commit verification confirms the source diff stays guard-only and does not alter board cleanup, drag/drop behavior, targeting behavior, derived piece identities, or serialization.
