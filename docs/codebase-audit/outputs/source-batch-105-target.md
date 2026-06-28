# SOURCE-BATCH-105 BoltOfCloth Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-105`
- Candidate: `SB104-CAND-002`
- Behavior: add stale/null/mobile/source-bolt/dye-sender/scissors guards to `BoltOfCloth.Dye` and `BoltOfCloth.Scissor`.
- System: `Items:Trades / Tailor Resources / BoltOfCloth`
- File: `Data/Scripts/Items/Trades/Resources/Tailor/BoltOfCloth.cs`

## Fence Result

- POST-BATCH-Y gate hits for `BoltOfCloth.cs`: `0`
- Active overlay rows for `BoltOfCloth.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state dye behavior: `Hue = sender.DyedHue`.
- Scissor behavior: existing `CanSee` rule and `base.ScissorHelper(from, new Cloth(), 50)`.
- `OnSingleClick` `Amount * 50` label behavior.
- `Deserialize` `ItemID = 0xF95` reset behavior.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-105 BoltOfCloth Guard Repair`
