# SOURCE-BATCH-249 PearlSkull Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-249`
- Candidate: `SB249-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `PearlSkull.OnDoubleClick(Mobile from)`.
- System: `Items:Trades / Fishing / PearlSkull`
- Source file: `Data/Scripts/Items/Trades/Fishing/PearlSkull.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Fishing/PearlSkull.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Backpack requirement.
- Failure message.
- `MysticalPearl` creation.
- Success message.
- Skull `Delete` semantics.
- Random ItemID/name construction.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-249 PearlSkull Guard Repair

Implement SB249-CAND-001 from source-batch-249-candidate-discovery.csv. Add stale/null/mobile/source-item guard to Data/Scripts/Items/Trades/Fishing/PearlSkull.cs while preserving backpack requirement, messages, MysticalPearl creation, deletion, construction randomization, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard PearlSkull interactions.
```
