# SOURCE-BATCH-014 Target: MagicHammer Guard Repair

Reviewed at: 2026-06-16T18:43:10.0827741-05:00

## Target

- Batch: `SOURCE-BATCH-014`
- Candidate: `SB012-CAND-003`
- Behavior: add stale/null/backpack guards to `MagicHammer.OnDoubleClick(Mobile from)` and `WearTarget.OnTarget(Mobile from, object targeted)` before mobile, source hammer, target ownership, artifact policy, or transform state is evaluated.
- System: `Items:Magical / MagicHammer`
- File: `Data/Scripts/Items/Magical/MagicHammer.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/MagicHammer.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/MagicHammer.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- Backpack-use message `1060640`.
- Target range.
- Ownership rule.
- `MyServerSettings.AlterArtifact` policy.
- Every item ID/name transform.
- Weapon replacement/deletion behavior.
- Sounds `0x541` and `0x233`.
- Success/failure messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-014 MagicHammer Guard Repair

Implement SB012-CAND-003 from docs/codebase-audit/outputs/source-batch-012-candidate-discovery.csv.

Add stale/null/backpack guards to Data/Scripts/Items/Magical/MagicHammer.cs:
- Return immediately when Mobile from is null or deleted.
- Treat deleted MagicHammer state, missing backpacks, or the hammer outside the backpack as the existing localized pack failure 1060640.
- Treat null/deleted target item state as the existing cannot-use-this-hammer failure.
- Preserve target range, ownership rule, artifact policy, transform matrix, weapon replacement/delete behavior, sounds, messages, serialization, and layout.

Verify POST-BATCH-Y gate hits=0, active overlay rows=0, targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, and generated root artifact restoration before staging.
```
