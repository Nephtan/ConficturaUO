# SOURCE-BATCH-015 Target: BookofDead Guard Repair

Reviewed at: 2026-06-16T18:47:37.0216166-05:00

## Target

- Batch: `SOURCE-BATCH-015`
- Candidate: `SB012-CAND-004`
- Behavior: add stale/null/backpack guards to `BookofDead.OnDoubleClick(Mobile from)` before mobile, skills, follower state, resource consumption, or corpse creation state is evaluated.
- System: `Items:Misc / BookofDead`
- File: `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- Backpack localized message `1042001`.
- Necromancy threshold.
- Spiritualism scalar, siphon, and empowerment math.
- Follower requirement.
- Resource types and quantities.
- `ConsumeTotal` result messages.
- Corpse creation/control behavior.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-015 BookofDead Guard Repair

Implement SB012-CAND-004 from docs/codebase-audit/outputs/source-batch-012-candidate-discovery.csv.

Add stale/null/backpack guards to Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs:
- Return immediately when Mobile from is null or deleted.
- Treat deleted BookofDead state, missing backpacks, or the book outside the backpack as the existing localized pack failure 1042001.
- Preserve Necromancy/Spiritualism math, follower requirement, resource consumption, corpse creation/control behavior, serialization, and layout.

Verify POST-BATCH-Y gate hits=0, active overlay rows=0, targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, and generated root artifact restoration before staging.
```
