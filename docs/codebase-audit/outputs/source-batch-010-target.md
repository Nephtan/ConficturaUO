# SOURCE-BATCH-010 Target: Scales Guard Repair

Reviewed at: 2026-06-16T18:18:35.5080479-05:00

## Target

- Batch: `SOURCE-BATCH-010`
- Candidate: `SB007-CAND-004`
- Behavior: add stale/null guards to `Scales.OnDoubleClick(Mobile from)` and `InternalTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Misc / Scales`
- Expected source file: `Data/Scripts/Items/Misc/Scales.cs`

## Governance

- Executive decision source: `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv`
- Candidate discovery source: `docs/codebase-audit/outputs/source-batch-007-candidate-discovery.csv`
- Candidate recommendation: `SB007-CAND-004` is the next clean non-gated follow-up candidate after `SOURCE-BATCH-009`.
- POST-BATCH-Y gate hits for expected file: `0`
- Active overlay rows for expected file: `0`
- Fence result: this batch stays inside a non-gated source-safe guard repair and crosses no staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate.

## Allowed Change

- In `OnDoubleClick(Mobile from)`, return when the mobile or scales item state is stale.
- In `InternalTarget.OnTarget`, return when the mobile or source scales state is stale.
- In `InternalTarget.OnTarget`, use the existing cannot-weigh failure for null or deleted target item state.

## Must Stay Unchanged

- Target range.
- Self-weigh rejection.
- `RootParent` awkward-location rule.
- Movable-object rule.
- Weight formatting.
- Messages.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.

## Goal Command

```text
/goal SOURCE-BATCH-010 Scales Guard Repair
```
