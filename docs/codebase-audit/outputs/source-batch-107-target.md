# SOURCE-BATCH-107 CaddelliteOre Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-107`
- Candidate: `SB107-CAND-001`
- Behavior: add stale/null/mobile/source-ore/backpack/region guards to `CaddelliteOre.OnDoubleClick`.
- System: `Items:Trades / Blacksmithing Resources / CaddelliteOre`
- File: `Data/Scripts/Items/Trades/Resources/Blacksmithing/CaddelliteOre.cs`

## Fence Result

- POST-BATCH-Y gate hits for `CaddelliteOre.cs`: `0`
- Active overlay rows for `CaddelliteOre.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Backpack failure message: `This must be in your backpack to use.`
- Master blacksmith requirement and failure message.
- Existing Dwarven Forge region requirement and not-in-forge failure message.
- Success behavior: sound `0x208`, animation `11, 5, 1, true, false, 0`, success message, `new CaddelliteIngot(this.Amount)`, `AddToBackpack`, and source ore `Delete()`.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-107 CaddelliteOre Guard Repair`
