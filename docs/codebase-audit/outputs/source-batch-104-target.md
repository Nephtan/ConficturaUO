# SOURCE-BATCH-104 Cloth Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-104`
- Candidate: `SB104-CAND-001`
- Behavior: add stale/null/mobile/source-cloth/dye-sender/scissors guards to `Cloth.Dye`, `Cloth.OnDoubleClick`, and `Cloth.Scissor`.
- System: `Items:Trades / Tailor Resources / Cloth`
- File: `Data/Scripts/Items/Trades/Resources/Tailor/Cloth.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Cloth.cs`: `0`
- Active overlay rows for `Cloth.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state dye behavior: `Hue = sender.DyedHue`.
- Fold behavior: `UncutCloth(Amount)`, hue copy, `from.AddToBackpack(cloth)`, fold message, and source cloth `Delete()`.
- Scissor behavior: existing `CanSee` rule and `base.ScissorHelper(from, new Bandage(), 1)`.
- `OnSingleClick` labels and amount display.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-104 Cloth Guard Repair`
