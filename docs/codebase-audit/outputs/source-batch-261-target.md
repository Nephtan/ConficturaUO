# SOURCE-BATCH-261 FirstAidKit Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-261`
- Candidate: `SB261-CAND-001`
- System: `Items:Technology / FirstAidKit`
- Source file: `Data/Scripts/Items/Technology/FirstAidKit.cs`
- Behavior: add stale/null/mobile/source-item/backpack guard to `FirstAidKit.OnDoubleClick(Mobile from)` before backpack containment checks, reward item creation, `AddToBackpack` calls, overhead messaging, or deleting the kit.

## Allowed Source Change

- In `FirstAidKit.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- Treat a missing backpack as the existing backpack-use failure by sending `This must be in your backpack to use.` and returning.

## Must Stay Unchanged

- Backpack-use failure message.
- Random bandage and potion reward types.
- Reward probability checks and amount ranges.
- `BasePotion.MakePillBottle` calls.
- `AddToBackpack` reward destination.
- Private overhead success message.
- Source kit `Delete()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file documentation row `RB-06785` remains `Documented`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-261 FirstAidKit Guard Repair`
