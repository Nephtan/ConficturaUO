# SOURCE-BATCH-019 Target: AllDyeTubsArmor Guard Repair

Reviewed at: 2026-06-16T19:13:19.7195952-05:00

## Target

- Batch: `SOURCE-BATCH-019`
- Candidate: `SB017-CAND-003`
- Behavior: add stale/null/source-tub/target guards to `AllDyeTubsArmor.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsArmorTarget.OnTarget` before mobile, tub, target item, or charge state is evaluated.
- System: `Items:Misc / Dyes / AllDyeTubsArmor`
- File: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsArmor.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsArmor.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsArmor.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `AllowPack` behavior.
- Target range.
- Armor/shield eligibility.
- Existing 100 gold cost behavior and ordering.
- Charged tub deletion/charge decrement behavior.
- Hue assignment.
- Sound `0x23F`.
- Messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
