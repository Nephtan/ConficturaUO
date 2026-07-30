# SOURCE-BATCH-011 Target: MagicScissors Guard Repair

Reviewed at: 2026-06-16T18:22:39.1548451-05:00

## Target

- Batch: `SOURCE-BATCH-011`
- Candidate: `SB007-CAND-005`
- Behavior: add stale/null/backpack guards to `MagicScissors.OnDoubleClick(Mobile from)` and `WearTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Magical / MagicScissors`
- Expected source file: `Data/Scripts/Items/Magical/MagicScissors.cs`

## Governance

- Executive decision source: `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv`
- Candidate discovery source: `docs/codebase-audit/outputs/source-batch-007-candidate-discovery.csv`
- Candidate recommendation: `SB007-CAND-005` is the final clean non-gated follow-up candidate from the current candidate list.
- POST-BATCH-Y gate hits for expected file: `0`
- Active overlay rows for expected file: `0`
- Fence result: this batch stays inside a non-gated source-safe guard repair and crosses no staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate.

## Allowed Change

- In `OnDoubleClick(Mobile from)`, return when the mobile state is stale.
- In `OnDoubleClick(Mobile from)`, use the existing backpack-use failure when the scissors are deleted, the backpack is missing, or the scissors are outside the backpack.
- In `WearTarget.OnTarget`, return when the mobile state is stale.
- In `WearTarget.OnTarget`, use the existing backpack-use failure when the source scissors are null, deleted, outside the backpack, or the backpack is missing.
- In `WearTarget.OnTarget`, use the existing failure path when the target is null, deleted, or not an item.

## Must Stay Unchanged

- Backpack-use message.
- Target range.
- Ownership rule.
- `MyServerSettings.AlterArtifact` policy.
- Every item ID/layer/name transform.
- Sound `0x248`.
- Success/failure messages.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.

## Goal Command

```text
/goal SOURCE-BATCH-011 MagicScissors Guard Repair
```
