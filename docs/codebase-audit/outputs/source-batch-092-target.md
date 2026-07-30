# SOURCE-BATCH-092 HueStone Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-092`
- Candidate: `SB092-CAND-001`
- Behavior: add stale/null/mobile/source-stone/deleted-target guards to `HueStone.OnDoubleClick(Mobile from)` and `WHueTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Misc / Dyes / HueStone`
- Source file: `Data/Scripts/Items/Misc/Dyes/HueStone.cs`

## Fence Result

- POST-BATCH-Y gate hits for `HueStone.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/HueStone.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source stone is deleted.
- Return immediately from target handling when the cached source stone is null/deleted.
- Treat null/deleted item targets as the existing invalid-target outcome without mutating, charging, or decrementing anything.

## Must Stay Unchanged

- `NCharges` semantics.
- `500 Gold` charge cost.
- Gold delete/amount reduction behavior.
- Hue cycle and Illusionist Stone names.
- Item-in-pack rule.
- Charge increment/decrement behavior.
- `RevealingAction`, sounds `0x1FA` and `0x2E6`, messages, target range, and target assignment behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-092 HueStone Guard Repair

Implement SB092-CAND-001 from source-batch-092-candidate-discovery.csv. Add stale/null/mobile/source-stone/deleted-target guards to Data/Scripts/Items/Misc/Dyes/HueStone.cs while preserving charge cost, gold behavior, hue cycle, item-in-pack rule, charge decrement, messages, sounds, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard HueStone interactions.
```
