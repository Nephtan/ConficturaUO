# SOURCE-BATCH-248 Dices Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-248`
- Candidate: `SB248-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `Dices.OnDoubleClick(Mobile from)`.
- System: `Items:Misc / Games / Dices`
- Source file: `Data/Scripts/Items/Misc/Games/Dices.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Games/Dices.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Range 2 rule.
- `Roll` output.
- `OnTelekinesis` behavior.
- Sound `0x34`.
- Hue repair `0x982`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-248 Dices Guard Repair

Implement SB248-CAND-001 from source-batch-248-candidate-discovery.csv. Add stale/null/mobile/source-item guard to Data/Scripts/Items/Misc/Games/Dices.cs while preserving range, Roll output, OnTelekinesis, sound, hue repair, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard Dices interactions.
```
