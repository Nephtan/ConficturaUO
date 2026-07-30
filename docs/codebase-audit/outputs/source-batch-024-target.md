# SOURCE-BATCH-024 Target: AllDyeTubsMountEthereal Guard Repair

Reviewed at: 2026-06-16T19:40:06.2516530-05:00

## Target

- Batch: `SOURCE-BATCH-024`
- Candidate: `SB022-CAND-003`
- Behavior: add stale/null/source-tub/target guards to `AllDyeTubsMountEthereal.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsMountEtherealTarget.OnTarget` before mobile, tub, target item, or charge state is evaluated.
- System: `Items:Misc / Dyes / AllDyeTubsMountEthereal`
- File: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsMountEthereal.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsMountEthereal.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsMountEthereal.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `AllowPack` behavior.
- Target range.
- `EtherealMount` eligibility.
- Existing 100 gold cost behavior and ordering.
- Charged tub deletion/charge decrement behavior.
- Hue assignment.
- Sound `0x23F`.
- Messages, including `Select an ethereal to dye`.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
