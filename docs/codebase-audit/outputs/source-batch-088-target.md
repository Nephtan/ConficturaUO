# SOURCE-BATCH-088 HairDyeBottle Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-088`
- Candidate: `SB087-CAND-002`
- Behavior: add stale/null/mobile/backpack/source-bottle guards to `HairDyeBottle.OnDoubleClick(Mobile from)`.
- System: `Items:Potions / Special / HairDyeBottle`
- Source file: `Data/Scripts/Items/Potions/Special/HairDyeBottle.cs`

## Fence Result

- POST-BATCH-Y gate hits for `HairDyeBottle.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/HairDyeBottle.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source bottle is deleted.
- Treat missing backpacks or source bottles outside the backpack as the existing backpack-use failure using message `This must be in your backpack to use.`

## Must Stay Unchanged

- Race restriction message.
- Backpack failure message.
- Neutral hue restore behavior.
- Dyed hue assignment behavior.
- Hair and facial hair hue behavior.
- Sound `0x5A4` and `this.Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-088 HairDyeBottle Guard Repair

Implement SB087-CAND-002 from source-batch-087-candidate-discovery.csv. Add stale/null/mobile/backpack/source-bottle guards to Data/Scripts/Items/Potions/Special/HairDyeBottle.cs while preserving race restrictions, hair hue behavior, messages, sound, Delete behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard HairDyeBottle interactions.
```
