# SOURCE-BATCH-110 HardCrystals Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-110`
- Candidate: `SB107-CAND-004`
- Behavior: add stale/null/mobile/source-crystals/backpack guards to `HardCrystals.OnDoubleClick`.
- System: `Items:Trades / Blacksmithing Resources / HardCrystals`
- File: `Data/Scripts/Items/Trades/Resources/Blacksmithing/HardCrystals.cs`

## Fence Result

- POST-BATCH-Y gate hits for `HardCrystals.cs`: `0`
- Active overlay rows for `HardCrystals.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Backpack localized message `1060640`.
- Forge requirement and failure message.
- Blacksmith skill success threshold `Value >= 50` and apprentice failure message.
- All existing `Name` to ingot mappings.
- `ingot.Amount = this.Amount`, `AddToBackpack`, sound `0x208`, existing success message text, and source `Delete()`.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-110 HardCrystals Guard Repair`
