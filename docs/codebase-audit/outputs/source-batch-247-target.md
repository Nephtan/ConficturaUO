# SOURCE-BATCH-247 IndecipherableMap Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-247`
- Candidate: `SB247-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `IndecipherableMap.OnDoubleClick(Mobile from)`.
- System: `Items:Trades / Maps / IndecipherableMap`
- Source file: `Data/Scripts/Items/Trades/Maps/IndecipherableMap.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Maps/IndecipherableMap.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Localized message `1070801`.
- Label number `1070799`.
- Random hue selection.
- `MapItem` inheritance.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-247 IndecipherableMap Guard Repair

Implement SB247-CAND-001 from source-batch-247-candidate-discovery.csv. Add stale/null/mobile/source-item guard to Data/Scripts/Items/Trades/Maps/IndecipherableMap.cs while preserving message, label, hue randomization, MapItem inheritance, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard IndecipherableMap interactions.
```
