# SOURCE-BATCH-246 CreepyPortrait Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-246`
- Candidate: `SB246-CAND-001`
- Behavior: add stale/null/mobile/source-component guard to `CreepyPortraitComponent.OnDoubleClick(Mobile from)`.
- System: `Items:Special / Evil Home Decor / CreepyPortrait`
- Source file: `Data/Scripts/Items/Special/Evil Home Decor Collection/CreepyPortrait.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Special/Evil Home Decor Collection/CreepyPortrait.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Range 2 rule.
- Random sound range `0x565`-`0x566`.
- Unreachable localized overhead message `1019045`.
- `HandlesOnMovement` and `OnMovement` behavior.
- `Timer.DelayCall`, `Up`, and `Down` behavior.
- Flipable art IDs.
- Addon/deed behavior.
- Label number.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-246 CreepyPortrait Guard Repair

Implement SB246-CAND-001 from source-batch-246-candidate-discovery.csv. Add stale/null/mobile/source-component guard to Data/Scripts/Items/Special/Evil Home Decor Collection/CreepyPortrait.cs while preserving range, sound, reach message, movement/timer behavior, addon/deed behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard CreepyPortrait interactions.
```
