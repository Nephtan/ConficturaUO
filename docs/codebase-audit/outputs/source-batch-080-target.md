# SOURCE-BATCH-080 RepairDeed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-080`
- Candidate: `SB078-CAND-003`
- Behavior: add stale/null/mobile/backpack/source-deed guards to `RepairDeed.OnDoubleClick(Mobile from)` and `Check(Mobile from)`.
- System: `Items:Trades / Misc / RepairDeed`
- Source file: `Data/Scripts/Items/Trades/Misc/RepairDeed.cs`

## Fence Result

- POST-BATCH-Y gate hits for `RepairDeed.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Misc/RepairDeed.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source deed is deleted.
- Treat missing backpacks or source deeds outside the backpack as the existing backpack-use failure.

## Must Stay Unchanged

- Backpack message `1047012`.
- `Repair.Do` call.
- `VerifyRegion` behavior.
- Nearby craft-station messages.
- `RepairSkillInfo` table and skill-system selection.
- Crafter/name property behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-080 RepairDeed Guard Repair

Implement SB078-CAND-003 from source-batch-078-candidate-discovery.csv. Add stale/null/mobile/backpack/source-deed guards to Data/Scripts/Items/Trades/Misc/RepairDeed.cs while preserving Repair.Do, VerifyRegion, nearby craft-station messages, RepairSkillInfo, crafter/name properties, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard RepairDeed interactions.
```
