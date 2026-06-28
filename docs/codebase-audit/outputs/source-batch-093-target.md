# SOURCE-BATCH-093 BloodDrink Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-093`
- Candidate: `SB093-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-drink guards to `BloodyDrink.OnDoubleClick(Mobile from)`.
- System: `Items:Food / BloodDrink`
- Source file: `Data/Scripts/Items/Food/BloodDrink.cs`

## Fence Result

- POST-BATCH-Y gate hits for `BloodDrink.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Food/BloodDrink.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source drink is deleted.
- Treat missing backpacks as the existing backpack-use failure using message `This must be in your backpack to drink.`

## Must Stay Unchanged

- `BaseRace.BloodDrinker` eligibility and rejection message.
- Hunger/thirst deltas and full-state clamp.
- Consumption, sound `0x2D6`, human animation, and `AwardKarma(from, -50, true)` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-093 BloodDrink Guard Repair

Implement SB093-CAND-001 from source-batch-093-candidate-discovery.csv. Add stale/null/mobile/backpack/source-drink guards to Data/Scripts/Items/Food/BloodDrink.cs while preserving BloodDrinker eligibility, hunger/thirst behavior, messages, consume, sound, animation, karma, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard BloodDrink interaction.
```
