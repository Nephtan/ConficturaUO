# SOURCE-BATCH-020 Target: AllDyeTubsWeapon Guard Repair

Reviewed at: 2026-06-16T19:19:55.8367442-05:00

## Target

- Batch: `SOURCE-BATCH-020`
- Candidate: `SB017-CAND-004`
- Behavior: add stale/null/source-tub/target guards to `AllDyeTubsWeapon.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsWeaponTarget.OnTarget` before mobile, tub, target item, or charge state is evaluated.
- System: `Items:Misc / Dyes / AllDyeTubsWeapon`
- File: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsWeapon.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsWeapon.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsWeapon.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `AllowPack` behavior.
- Target range.
- Weapon eligibility.
- Existing 100 gold cost behavior and ordering.
- Charged tub deletion/charge decrement behavior.
- Hue assignment.
- Sound `0x23F`.
- Messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
