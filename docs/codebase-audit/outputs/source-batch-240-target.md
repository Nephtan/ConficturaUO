# SOURCE-BATCH-240 MountedPixieGreen Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-240`
- Candidate: `SB240-CAND-001`
- Behavior: add stale/null/mobile/source-component guard to `MountedPixieGreenComponent.OnDoubleClick(Mobile from)`.
- System: `Items:Special / Evil Home Decor / MountedPixieGreen`
- Source file: `Data/Scripts/Items/Special/Evil Home Decor Collection/MountedPixieGreen.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Special/Evil Home Decor Collection/MountedPixieGreen.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Range 2 rule.
- Random sound range `0x554`-`0x557`.
- Unreachable localized overhead message `1019045`.
- Flipable art IDs.
- Addon/deed behavior.
- Label number.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-240 MountedPixieGreen Guard Repair

Implement SB240-CAND-001 from source-batch-240-candidate-discovery.csv. Add stale/null/mobile/source-component guard to Data/Scripts/Items/Special/Evil Home Decor Collection/MountedPixieGreen.cs while preserving range, sound, reach message, addon/deed behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard MountedPixieGreen interactions.
```
