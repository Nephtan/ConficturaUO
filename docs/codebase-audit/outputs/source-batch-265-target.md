# SOURCE-BATCH-265 MagicFish Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-265`
- Candidate: `SB265-CAND-001`
- System: `Items:Trades / Fishing / MagicFish`
- Source file: `Data/Scripts/Items/Trades/Resources/Fishing/MagicFish.cs`
- Behavior: add stale/null/mobile/source-item/backpack guard to `BaseMagicFish.OnDoubleClick(Mobile from)` before backpack checking, race checks, stat buff application, hunger/healing/poison cure, effects, sounds, messaging, or deleting the fish.

## Allowed Source Change

- In `BaseMagicFish.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- Treat a missing backpack as the existing pack-use failure using localized message `1042001`.

## Must Stay Unchanged

- Backpack requirement and localized failure `1042001`.
- BloodDrinker/BrainEater rejection message.
- `Apply` and stat buff behavior.
- `PeculiarFish` stamina behavior.
- Hunger increment, human animation, tasting-based healing, and poison cure.
- Fixed effect, sound `0x1E7`, and localized swallow message `501774`.
- Source fish `Delete()` semantics.
- Subclass hue repair behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-265 MagicFish Guard Repair`
