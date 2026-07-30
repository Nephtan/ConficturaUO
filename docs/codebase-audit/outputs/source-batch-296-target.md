# SOURCE-BATCH-296 DragonBardingDeed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-296`
- Candidate: `SB296-CAND-001`
- System: `Items:Deeds / DragonBardingDeed`
- Source file: `Data/Scripts/Items/Deeds/DragonBardingDeed.cs`
- Behavior: add stale/null/mobile/source-deed/backpack/target-pet guards to `DragonBardingDeed.OnDoubleClick(Mobile from)` and `DragonBardingDeed.OnTarget(Mobile from, object obj)` before backpack checks, targeting, ownership checks, barding assignment, or deed deletion.

## Allowed Source Changes

- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, treat `from.Backpack == null` as the existing backpack-use failure with localized message `1042001`.
- In `OnTarget(Mobile from, object obj)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnTarget(Mobile from, object obj)`, treat a deleted targeted swamp dragon as the existing invalid-target path with localized message `1053025`.
- In `OnTarget(Mobile from, object obj)`, treat `from.Backpack == null` as the existing backpack-use failure with localized message `1060640`.

## Must Stay Unchanged

- Target prompt `1053024`.
- Backpack messages `1042001` and `1060640`.
- Invalid target message `1053025`.
- Ownership message `1053026`.
- Success message `1053027`.
- Swamp dragon eligibility and ownership rules.
- `BardingExceptional`, `BardingCrafter`, `BardingHP`, `BardingResource`, `HasBarding`, and `Hue` assignments.
- Deed `Delete` behavior.
- Craft metadata.
- Serialization layout/versioning, including current version 1 write/read order and version 0 legacy read.
- Constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact overlay: `RB-00643` is `IntentionalLegacy/Reviewed`; no serializer source changes are allowed in this batch.

## Ready Goal Shape

`/goal SOURCE-BATCH-296 DragonBardingDeed Guard Repair`
