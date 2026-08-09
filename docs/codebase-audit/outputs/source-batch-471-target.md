# SOURCE-BATCH-471 Dyes Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-471`
- Candidate: `SB471-CAND-001`
- Behavior: add stale/null mobile, deleted source dyes, stale target dye tub, and stale callback dye tub guards to `Dyes` interaction paths.
- System: `Items:Trades / Tailor Items / Dyes`
- File: `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- `Dyes` item identity
- target range
- localized prompt `500856`
- invalid-target localized message `500857`
- `DyeTub.Redyable` behavior
- `BlackDyeTub` rejection localized message `1010092`
- may-not-redye message
- `HuePicker` behavior
- `CustomHuePickerGump` behavior
- `DyedHue` assignment for valid dye tubs
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-471 Dyes Guard Repair`
