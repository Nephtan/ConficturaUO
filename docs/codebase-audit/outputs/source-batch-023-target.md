# SOURCE-BATCH-023 Target: AllDyeTubsBookSpell Guard Repair

Reviewed at: 2026-06-16T19:35:56.9822424-05:00

## Target

- Batch: `SOURCE-BATCH-023`
- Candidate: `SB022-CAND-002`
- Behavior: add stale/null/source-tub/target guards to `AllDyeTubsBookSpell.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsBookSpellTarget.OnTarget` before mobile, tub, target item, or charge state is evaluated.
- System: `Items:Misc / Dyes / AllDyeTubsBookSpell`
- File: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookSpell.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookSpell.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookSpell.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `AllowPack` behavior.
- Target range.
- `Spellbook` eligibility.
- Existing 100 gold cost behavior and ordering.
- Charged tub deletion/charge decrement behavior.
- Hue assignment.
- Sound `0x23F`.
- Messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
