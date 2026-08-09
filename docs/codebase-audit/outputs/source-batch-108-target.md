# SOURCE-BATCH-108 RareMetals Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-108`
- Candidate: `SB107-CAND-002`
- Behavior: add stale/null/mobile/source-stones/backpack guards to `RareMetals.OnDoubleClick`.
- System: `Items:Trades / Blacksmithing Resources / RareMetals`
- File: `Data/Scripts/Items/Trades/Resources/Blacksmithing/RareMetals.cs`

## Fence Result

- POST-BATCH-Y gate hits for `RareMetals.cs`: `0`
- Active overlay rows for `RareMetals.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Backpack localized message `1060640`.
- Forge requirement and failure message.
- Blacksmith skill success threshold `Value >= 50` and apprentice failure message.
- All existing `Name` to ingot mappings.
- `ingot.Amount = this.Amount`, `AddToBackpack`, sound `0x208`, success message, and source `Delete()`.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-108 RareMetals Guard Repair`
