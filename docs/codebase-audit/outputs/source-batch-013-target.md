# SOURCE-BATCH-013 Target: HydraTooth Guard Repair

Reviewed at: 2026-06-16T18:37:56.3208151-05:00

## Target

- Batch: `SOURCE-BATCH-013`
- Candidate: `SB012-CAND-002`
- Behavior: add stale/null/backpack guards to `HydraTooth.OnDoubleClick(Mobile from)` before mobile, design-context, backpack, spell construction, or cast state is evaluated.
- System: `Items:Magical / HydraTooth`
- File: `Data/Scripts/Items/Magical/HydraTooth.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/HydraTooth.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/HydraTooth.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `Multis.DesignContext.Check(from)` behavior.
- Backpack localized message `1042001`.
- `SummonSkeletonSpell(from, this)` construction.
- `spell.Cast()` behavior.
- Item id, name, amount, and stacking behavior.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-013 HydraTooth Guard Repair

Implement SB012-CAND-002 from docs/codebase-audit/outputs/source-batch-012-candidate-discovery.csv.

Add stale/null/backpack guards to Data/Scripts/Items/Magical/HydraTooth.cs:
- Return immediately when Mobile from is null or deleted.
- Treat deleted HydraTooth state, missing backpacks, or the tooth outside the backpack as the existing localized pack failure 1042001.
- Preserve DesignContext.Check, SummonSkeletonSpell construction, Cast behavior, item identity, serialization, and layout.

Verify POST-BATCH-Y gate hits=0, active overlay rows=0, targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, and generated root artifact restoration before staging.
```
