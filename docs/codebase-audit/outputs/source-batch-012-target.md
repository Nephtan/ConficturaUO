# SOURCE-BATCH-012 Target: BalancingDeed Guard Repair

Reviewed at: 2026-06-16T18:32:00.1198582-05:00

## Target

- Batch: `SOURCE-BATCH-012`
- Candidate: `SB012-CAND-001`
- Behavior: add stale/null/backpack guards to `BalancingDeed.OnDoubleClick(Mobile from)` and `BalancingTarget.OnTarget(Mobile from, object target)`.
- System: `Items:Magical / BalancingDeed`
- Expected source file: `Data/Scripts/Items/Magical/BalancingDeed.cs`

## Governance

- Executive decision source: `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv`
- Candidate discovery source: `docs/codebase-audit/outputs/source-batch-012-candidate-discovery.csv`
- Candidate recommendation: `SB012-CAND-001` is the recommended implementation target.
- POST-BATCH-Y gate hits for expected file: `0`
- Active overlay rows for expected file: `0`
- Fence result: this batch stays inside a non-gated source-safe guard repair and crosses no staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate.

## Allowed Change

- In `OnDoubleClick(Mobile from)`, return when mobile state is stale.
- In `OnDoubleClick(Mobile from)`, use existing backpack failure when the deed is deleted, the backpack is missing, or the deed is outside the backpack.
- In `BalancingTarget.OnTarget`, return when mobile state is stale.
- In `BalancingTarget.OnTarget`, use existing pack failure when the source deed is null, deleted, or no longer rooted to the mobile.
- In `BalancingTarget.OnTarget`, use existing invalid-target failure when the target is not a live `BaseRanged`.

## Must Stay Unchanged

- `BaseRanged`-only target eligibility.
- `Balanced` flag assignment.
- `RootParent` ownership rule.
- Existing success/failure messages.
- Deed `Delete()` semantics.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.

## Goal Command

```text
/goal SOURCE-BATCH-012 BalancingDeed Guard Repair
```
