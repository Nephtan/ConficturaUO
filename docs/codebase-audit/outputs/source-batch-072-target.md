# SOURCE-BATCH-072 MonsterStatueDeed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-072`
- Candidate: `SB071-CAND-002`
- Behavior: add stale/null/mobile/backpack/source-deed guards to `MonsterStatueDeed.OnDoubleClick(Mobile from)`.
- System: `Items:Decorations / MonsterStatueDeed`
- Source file: `Data/Scripts/Items/Decorations/MonsterStatueDeed.cs`

## Fence Result

- POST-BATCH-Y gate hits for `MonsterStatueDeed.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Decorations/MonsterStatueDeed.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source deed is deleted.
- Treat a missing backpack as the existing backpack-use failure before calling `IsChildOf(from.Backpack)`.

## Must Stay Unchanged

- Backpack message `1042001`.
- Existing `Utility.RandomMinMax` calls and all generated `MonsterStatuetteType` values.
- `AddToBackpack` behavior and deed `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-072 MonsterStatueDeed Guard Repair

Implement SB071-CAND-002 from source-batch-071-candidate-discovery.csv. Add stale/null/mobile/backpack/source-deed guards to Data/Scripts/Items/Decorations/MonsterStatueDeed.cs while preserving statuette generation, messages, deletion, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard MonsterStatueDeed interactions.
```
