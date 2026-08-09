# SOURCE-BATCH-264 DartBoard Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-264`
- Candidate: `SB264-CAND-001`
- System: `Items:Construction / DartBoard`
- Source file: `Data/Scripts/Items/Construction/Addons/DartBoard.cs`
- Behavior: add stale/null/mobile/source-component guards to `DartBoard.OnDoubleClick(Mobile from)` and `DartBoard.Throw(Mobile from)` before direction, range, LOS, weapon, animation, effect, sound, scoring, or reach-message paths.

## Allowed Source Change

- In `DartBoard.OnDoubleClick(Mobile from)` and `DartBoard.Throw(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- `NeedsWall` and `WallPosition` behavior.
- Direction selection.
- Range 4 and LOS rules.
- East/south valid throw directions.
- Knife requirement message.
- Animation choices.
- Moving effect and sound `0x238`.
- Random scoring thresholds and messages.
- Addon/deed behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file save-compat rows remain `IntentionalLegacy`; they do not approve serializer edits and do not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-264 DartBoard Guard Repair`
