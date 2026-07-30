# SOURCE-BATCH-022 Target: AllDyeTubsBookRune Guard Repair

Reviewed at: 2026-06-16T19:31:49.9389645-05:00

## Target

- Batch: `SOURCE-BATCH-022`
- Candidate: `SB022-CAND-001`
- Behavior: add stale/null/source-tub/target guards to `AllDyeTubsBookRune.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsBookRuneTarget.OnTarget` before mobile, tub, target item, or charge state is evaluated.
- System: `Items:Misc / Dyes / AllDyeTubsBookRune`
- File: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookRune.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookRune.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookRune.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `AllowPack` behavior.
- Target range.
- `Runebook` eligibility.
- Existing 100 gold cost behavior and ordering.
- Charged tub deletion/charge decrement behavior.
- Hue assignment.
- Sound `0x23F`.
- Messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
