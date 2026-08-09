# SOURCE-BATCH-267 MagicTalisman Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-267`
- Candidate: `SB267-CAND-001`
- System: `Items:Magical / MagicTalisman`
- Source file: `Data/Scripts/Items/Magical/MagicTalisman.cs`
- Behavior: add a stale/null/mobile/source-item guard to `MagicTalisman.OnDoubleClick(Mobile from)` before sending the worn-slot message.

## Allowed Source Change

- In `MagicTalisman.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Randomized talisman names, item IDs, hues, and skill bonus assignments.
- `Resource`, `Layer.Talisman`, and `Weight` setup.
- Worn-slot message text: `Trinkets are worn in the upper right slot.`
- Constructor metadata, serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `SafeNoChange`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-267 MagicTalisman Guard Repair`
