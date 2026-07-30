# SOURCE-BATCH-241 MountedPixieOrange Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-241`
- Candidate: `SB241-CAND-001`
- Behavior: add stale/null/mobile/source-component guard to `MountedPixieOrangeComponent.OnDoubleClick(Mobile from)`.
- System: `Items:Special / Evil Home Decor / MountedPixieOrange`
- Source file: `Data/Scripts/Items/Special/Evil Home Decor Collection/MountedPixieOrange.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Special/Evil Home Decor Collection/MountedPixieOrange.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Range 2 rule.
- Random sound range `0x558`-`0x55B`.
- Unreachable localized overhead message `1019045`.
- Flipable art IDs.
- Addon/deed behavior.
- Label number.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-241 MountedPixieOrange Guard Repair

Implement SB241-CAND-001 from source-batch-241-candidate-discovery.csv. Add stale/null/mobile/source-component guard to Data/Scripts/Items/Special/Evil Home Decor Collection/MountedPixieOrange.cs while preserving range, sound, reach message, addon/deed behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard MountedPixieOrange interactions.
```
