# SOURCE-BATCH-106 UncutCloth Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-106`
- Candidate: `SB104-CAND-003`
- Behavior: add stale/null/mobile/source-uncut-cloth/dye-sender/scissors guards to `UncutCloth.Dye` and `UncutCloth.Scissor`.
- System: `Items:Trades / Tailor Resources / UncutCloth`
- File: `Data/Scripts/Items/Trades/Resources/Tailor/UncutCloth.cs`

## Fence Result

- POST-BATCH-Y gate hits for `UncutCloth.cs`: `0`
- Active overlay rows for `UncutCloth.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state dye behavior: `Hue = sender.DyedHue`.
- Scissor behavior: existing `CanSee` rule and `base.ScissorHelper(from, new Bandage(), 1)`.
- `OnSingleClick` labels and amount display.
- `Deserialize` `ItemID = 0x1765` and `Name = "folded cloth"` reset behavior.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-106 UncutCloth Guard Repair`
