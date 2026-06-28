# SOURCE-BATCH-033 Target: Dice8 Guard Repair

Reviewed at: 2026-06-16T20:31:00-05:00

## Target

- Batch: `SOURCE-BATCH-033`
- Candidate: `SB031-CAND-003`
- Behavior: add stale/null/mobile/deleted-item guards to `Dice8.OnDoubleClick` and `Dice8.OnTelekinesis` before range checks, particle effects, sound, or `Roll(from)` dereferences mobile state.
- System: `Items:Misc / Games / DandD Dice8`
- File: `Data/Scripts/Items/Misc/Games/DandD/Dice8.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Games/DandD/Dice8.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Games/DandD/Dice8.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- ItemID `0x3019`.
- Name and weight.
- `AddNameProperties` labels.
- Range `2`.
- Telekinesis effects and sound `0x1F5`.
- Roll overhead message.
- `Utility.Random(1, 8)`.
- Sound `0x34`.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
