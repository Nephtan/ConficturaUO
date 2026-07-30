# SOURCE-BATCH-031 Target: Dice4 Guard Repair

Reviewed at: 2026-06-16T20:25:00-05:00

## Target

- Batch: `SOURCE-BATCH-031`
- Candidate: `SB031-CAND-001`
- Behavior: add stale/null/mobile/deleted-item guards to `Dice4.OnDoubleClick` and `Dice4.OnTelekinesis` before range checks, particle effects, sound, or `Roll(from)` dereferences mobile state.
- System: `Items:Misc / Games / DandD Dice4`
- File: `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- ItemID `0x301C`.
- Name and weight.
- `AddNameProperties` labels.
- Range `2`.
- Telekinesis effects and sound `0x1F5`.
- Roll overhead message.
- `Utility.Random(1, 4)`.
- Sound `0x34`.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
