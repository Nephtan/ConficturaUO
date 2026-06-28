# SOURCE-BATCH-081 ArrowsAndBolts Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-081`
- Candidate: `SB081-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-bundle guards to `ManyArrows100`, `ManyArrows1000`, `ManyBolts100`, and `ManyBolts1000` `OnDoubleClick(Mobile from)` paths.
- System: `Items:Explorers / ArrowsAndBolts`
- Source file: `Data/Scripts/Items/Explorers/ArrowsAndBolts.cs`

## Fence Result

- POST-BATCH-Y gate hits for `ArrowsAndBolts.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Explorers/ArrowsAndBolts.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source bundle is deleted.
- Treat missing backpacks or source bundles outside the backpack as the existing backpack-use failure.

## Must Stay Unchanged

- Backpack failure message.
- `Arrow(100)`, `Arrow(1000)`, `Bolt(100)`, and `Bolt(1000)` quantities.
- `AddToBackpack` behavior.
- `PrivateOverheadMessage` text, hue, and message type.
- Bundle `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-081 ArrowsAndBolts Guard Repair

Implement SB081-CAND-001 from source-batch-081-candidate-discovery.csv. Add stale/null/mobile/backpack/source-bundle guards to Data/Scripts/Items/Explorers/ArrowsAndBolts.cs while preserving bundle quantities, AddToBackpack behavior, overhead messages, Delete semantics, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard ArrowsAndBolts interactions.
```
