# SOURCE-BATCH-021 Target: AllDyeTubsFurniture Guard Repair

Reviewed at: 2026-06-16T19:24:10.9554142-05:00

## Target

- Batch: `SOURCE-BATCH-021`
- Candidate: `SB017-CAND-005`
- Behavior: add stale/null/source-tub/target guards to `AllDyeTubsFurniture.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsFurnitureTarget.OnTarget` before mobile, tub, target item, or charge state is evaluated.
- System: `Items:Misc / Dyes / AllDyeTubsFurniture`
- File: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsFurniture.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsFurniture.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsFurniture.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `AllowPack` behavior.
- Target range.
- Furniture eligibility through `FurnitureAttribute.Check(item)`.
- Existing 100 gold cost behavior and ordering.
- Charged tub deletion/charge decrement behavior.
- Hue assignment.
- Sound `0x23F`.
- Messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
