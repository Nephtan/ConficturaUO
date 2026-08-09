# SOURCE-BATCH-034 Target: Dice10 Guard Repair

Reviewed at: 2026-06-16T20:34:00-05:00

## Target

- Batch: `SOURCE-BATCH-034`
- Candidate: `SB031-CAND-004`
- Behavior: add stale/null/mobile/deleted-item guards to `Dice10.OnDoubleClick` and `Dice10.OnTelekinesis` before range checks, particle effects, sound, or `Roll(from)` dereferences mobile state.
- System: `Items:Misc / Games / DandD Dice10`
- File: `Data/Scripts/Items/Misc/Games/DandD/Dice10.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Games/DandD/Dice10.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Games/DandD/Dice10.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- ItemID `0x301B`.
- Name and weight.
- `AddNameProperties` labels.
- Range `2`.
- Telekinesis effects and sound `0x1F5`.
- Roll overhead message.
- `Utility.Random(1, 10)`.
- Sound `0x34`.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
