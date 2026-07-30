# SOURCE-BATCH-332 EmbalmingFluid Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-332`
- Candidate: `SB332-CAND-001`
- Behavior: add stale/null/mobile/source-fluid/backpack/deleted-target guards to the embalming fluid interaction path.
- System: `Quests:Frankenstein / EmbalmingFluid`
- File: `Data/Scripts/Quests/Frankenstein/EmbalmingFluid.cs`

## Allowed Source Change

Add guard-only checks to `EmbalmingFluid.OnDoubleClick(Mobile from)` and `FluidTarget.OnTarget(Mobile from, object targeted)`.

The guards may return early for null/deleted mobiles, deleted source fluids, null source fluid state, missing backpacks, source fluids outside the backpack, or deleted target items before dereferencing those values. Missing backpacks or stale source-fluid state should use the existing backpack failure message; deleted target porter items should use the existing item-in-pack failure path.

## Must Stay Unchanged

- Target range `1`
- Localized backpack message `1060640`
- Target prompt
- Fluid-in-pack failure message
- Invalid-target message
- `FrankenPorterItem` eligibility
- `+5` or `+1` charge increment rule
- Charge cap `100`
- Success and already-full messages
- `RevealingAction`
- Sound `0x23E`
- Empty `Bottle` return
- `InvalidateProperties`
- Source fluid `Consume` behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-332 EmbalmingFluid Guard Repair
```
