# SOURCE-BATCH-075 DwarvenForge Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-075`
- Candidate: `SB075-CAND-001`
- Behavior: add stale/null/mobile/source-forge guards to `DwarvenForge.OnDoubleClick(Mobile from)`.
- System: `Items:Trades / Blacksmith Items / DwarvenForge`
- Source file: `Data/Scripts/Items/Trades/Blacksmith Items/DwarvenForge.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DwarvenForge.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Blacksmith Items/DwarvenForge.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source forge is deleted.
- Avoid dereferencing `from.Backpack` when the mobile has no backpack while preserving existing non-backpack behavior.

## Must Stay Unchanged

- Secure-in-home failure message.
- Range failure message.
- Movable-item rejection.
- `ItemID` toggle between `0x544A` and `0x544B`.
- `LightType.Empty` / `LightType.Circle225` behavior.
- Sound `0x208`.
- `OnDragLift` reset behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-075 DwarvenForge Guard Repair

Implement SB075-CAND-001 from source-batch-075-candidate-discovery.csv. Add stale/null/mobile/source-forge guards to Data/Scripts/Items/Trades/Blacksmith Items/DwarvenForge.cs while preserving forge toggle behavior, messages, sound, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard DwarvenForge interactions.
```
