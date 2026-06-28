# SOURCE-BATCH-078 CrystallineJar Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-078`
- Candidate: `SB078-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-jar and null-target guards to `CrystallineJar.OnDoubleClick(Mobile from)`, `ScoopTarget.OnTarget(Mobile from, object targeted)`, and `ThrowTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Potions / Bottles / CrystallineJar`
- Source file: `Data/Scripts/Items/Potions/Bottles/CrystallineJar.cs`

## Fence Result

- POST-BATCH-Y gate hits for `CrystallineJar.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Bottles/CrystallineJar.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source jar is deleted.
- Treat missing backpacks or source jars outside the backpack as the existing backpack-use failure.
- Treat deleted scoop targets as the existing invalid-substance failure.
- In `ThrowTarget.OnTarget`, validate `targeted as IPoint3D` before constructing a `Point3D`.

## Must Stay Unchanged

- Backpack message `1060640`.
- Empty flask scoop prompt and full flask throw prompt.
- `MonsterSplatter.TooMuchSplatter` behavior.
- `ThrowTarget` reuse check.
- `MonsterSplatter` and `HolyWater` scoop behavior.
- Throw range, distance, line-of-sight, casting, paralyzed, blessed, and frozen checks.
- `MonsterSplatter.AddSplatter` behavior.
- Reset-to-empty `Name`, `Hue`, and `Weight` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-078 CrystallineJar Guard Repair

Implement SB078-CAND-001 from source-batch-078-candidate-discovery.csv. Add stale/null/mobile/backpack/source-jar guards and fix the null-target ordering in Data/Scripts/Items/Potions/Bottles/CrystallineJar.cs while preserving scoop/throw behavior, messages, splatter behavior, reset behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard CrystallineJar interactions.
```
