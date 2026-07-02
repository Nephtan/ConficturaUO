# SOURCE-BATCH-399 Sextant Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-399`
- Candidate: `SB399-CAND-001`
- System: `Items:Trades / Fishing / Misc / Sextant`
- File: `Data/Scripts/Items/Trades/Fishing/Misc/Sextant.cs`
- Behavior: add stale/null mobile and deleted source-sextant guard coverage around `Sextant.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Sextant and MagicSextant inherited use behavior.
- Coordinate formatting.
- World and region message decisions.
- Magic sextant Underworld behavior.
- Existing messages.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
