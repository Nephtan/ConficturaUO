# SOURCE-BATCH-270 PlasmaTorch Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-270`
- Candidate: `SB270-CAND-001`
- System: `Items:Technology / PlasmaTorch`
- Source file: `Data/Scripts/Items/Technology/PlasmaTorch.cs`
- Behavior: add stale/null/mobile/source-tool guards to `PlasmaTorch.OnDoubleClick(Mobile from)` and `UnlockTarget.OnTarget(Mobile from, object targeted)` before backpack checks, target assignment, lock/trap/door checks, messages, sound, revealing action, or source torch consumption.

## Allowed Source Change

- In `PlasmaTorch.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `UnlockTarget.OnTarget(Mobile from, object targeted)`, return immediately when `from == null || from.Deleted`.
- In `UnlockTarget.OnTarget(Mobile from, object targeted)`, use the existing backpack-use failure `1060640` and return when the source torch is null or deleted.

## Must Stay Unchanged

- Target range `1`, `CheckLOS = true`, and target prompt text.
- Backpack-use failure message `1060640`.
- BaseHouseDoor, BookBox, UnidentifiedArtifact, UnidentifiedItem, CurseItem, BaseDoor, and ILockable handling.
- Dungeon-door unlock behavior and TreasureMapChest behavior.
- Lock/trap mutation rules, lock level adjustment, picker assignment, and trap clearing.
- Messages, sound `0x227`, `RevealingAction`, and source torch `Consume()` semantics.
- Constructor metadata, serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-270 PlasmaTorch Guard Repair`
