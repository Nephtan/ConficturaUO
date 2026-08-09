# SOURCE-BATCH-470 RejuvinationAnkh Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-470`
- Candidate: `SB470-CAND-001`
- Behavior: add stale/null mobile, deleted source component, and malformed delayed-callback state guards to `RejuvinationAddonComponent` interaction paths.
- System: `Items:Construction / Addons / RejuvinationAnkh`
- File: `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- `RejuvinationAddonComponent` item IDs
- `BeginAction` lock type
- `FixedEffect` `0x373A`
- random restore selection
- Hits/Mana/Stam restore behavior
- localized messages `500801`, `500802`, `500803`, and `500807`
- two-hour `DelayCall`
- `EndAction` behavior for valid mobiles
- `BaseRejuvinationAnkh` movement message
- addon components
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-470 RejuvinationAnkh Guard Repair`
