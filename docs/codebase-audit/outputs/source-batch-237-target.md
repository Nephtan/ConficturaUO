# SOURCE-BATCH-237 Pillows Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-237`
- Candidate: `SB237-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `Pillows.OnDoubleClick(Mobile from)`.
- System: `Items:Decorations / Pillows`
- Source file: `Data/Scripts/Items/Decorations/Pillows.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Decorations/Pillows.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Flip ID fields.
- Random construction item IDs and hues.
- `ItemID` toggle behavior.
- Name and weight.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-237 Pillows Guard Repair

Implement SB237-CAND-001 from source-batch-237-candidate-discovery.csv. Add stale/null/mobile/source-item guard to Data/Scripts/Items/Decorations/Pillows.cs while preserving flip IDs, item-ID toggle behavior, construction randomization, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard Pillows interactions.
```
