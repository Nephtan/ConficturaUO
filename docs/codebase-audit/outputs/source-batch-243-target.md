# SOURCE-BATCH-243 DisturbingPortrait Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-243`
- Candidate: `SB243-CAND-001`
- Behavior: add stale/null/mobile/source-component guard to `DisturbingPortraitComponent.OnDoubleClick(Mobile from)`.
- System: `Items:Special / Evil Home Decor / DisturbingPortrait`
- Source file: `Data/Scripts/Items/Special/Evil Home Decor Collection/DisturbingPortrait.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Special/Evil Home Decor Collection/DisturbingPortrait.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Range 2 rule.
- Random sound range `0x567`-`0x568`.
- Unreachable localized overhead message `1019045`.
- `Timer.DelayCall` setup and `Change` callback behavior.
- Flipable art IDs.
- Addon/deed behavior.
- Label number.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-243 DisturbingPortrait Guard Repair

Implement SB243-CAND-001 from source-batch-243-candidate-discovery.csv. Add stale/null/mobile/source-component guard to Data/Scripts/Items/Special/Evil Home Decor Collection/DisturbingPortrait.cs while preserving range, sound, reach message, timer/change behavior, addon/deed behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard DisturbingPortrait interactions.
```
