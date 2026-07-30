# SOURCE-BATCH-262 VenomSack Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-262`
- Candidate: `SB262-CAND-001`
- System: `Items:Potions / VenomSack`
- Source file: `Data/Scripts/Items/Potions/Standard/Poison Potions/VenomSack.cs`
- Behavior: add stale/null/mobile/source-item/backpack guard to `VenomSack.OnDoubleClick(Mobile from)` before venom extraction, bottle consumption, poison application, messaging, or consuming the sack.

## Allowed Source Change

- In `VenomSack.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- Treat a missing backpack at bottle consumption as the existing empty-bottle failure using `You need an empty bottle to drain the venom from the sack.`

## Must Stay Unchanged

- Venom-name-to-skill mapping.
- `CheckSkill` calls and thresholds.
- Empty bottle consumption rule.
- Poison potion output mapping.
- Sounds `0x240` and `0x62D`.
- `Poison!` speech/message behavior.
- Poison self-application types.
- Source sack `Consume()` semantics.
- `AddNameProperties` text.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-262 VenomSack Guard Repair`
