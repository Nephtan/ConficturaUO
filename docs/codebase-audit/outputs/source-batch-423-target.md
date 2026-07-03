# SOURCE-BATCH-423 PowderOfTranslocation Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-423`
- Candidate: `SB423-CAND-001`
- System: `Items:Special / Solen Items / PowderOfTranslocation`
- File: `Data/Scripts/Items/Special/Solen Items/PowderOfTranslocation.cs`
- Behavior: add stale/null mobile and missing/deleted source powder guards to target assignment and target handling.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Range requirement.
- Target assignment.
- `TranslocationItem` interface contract.
- Charge cap check.
- Max recharge check.
- Partial recharge delta math.
- Full powder consumption `Delete` behavior.
- Localized messages `1054137` through `1054140`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
