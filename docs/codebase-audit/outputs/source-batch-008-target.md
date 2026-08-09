# SOURCE-BATCH-008 Target: VelocityDeed Guard Repair

Reviewed at: 2026-06-16T18:08:44.8478282-05:00

## Target

- Batch: `SOURCE-BATCH-008`
- Candidate: `SB007-CAND-002`
- Behavior: add stale/null/backpack guards to `VelocityDeed.OnDoubleClick(Mobile from)` and `VelocityTargetx.OnTarget(Mobile from, object target)`.
- System: `Items:Magical / VelocityDeed`
- Expected source file: `Data/Scripts/Items/Magical/VelocityDeed.cs`

## Governance

- Executive decision source: `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv`
- Candidate discovery source: `docs/codebase-audit/outputs/source-batch-007-candidate-discovery.csv`
- Candidate recommendation: `SB007-CAND-002` is the next clean non-gated follow-up candidate after `SOURCE-BATCH-007`.
- POST-BATCH-Y gate hits for expected file: `0`
- Active overlay rows for expected file: `0`
- Fence result: this batch stays inside a non-gated source-safe guard repair and crosses no staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate.

## Allowed Change

- In `OnDoubleClick(Mobile from)`, return when `from == null || from.Deleted`.
- In `OnDoubleClick(Mobile from)`, use the existing backpack failure message when the deed is deleted, the backpack is missing, or the deed is outside the backpack.
- In `VelocityTargetx.OnTarget`, return when `from == null || from.Deleted`.
- In `VelocityTargetx.OnTarget`, use the existing cannot-add-velocity failure when the source deed is null, deleted, or no longer rooted to the mobile.
- In `VelocityTargetx.OnTarget`, use the existing invalid-target failure when the target is not a live `BaseRanged`.

## Must Stay Unchanged

- `BaseRanged`-only target eligibility.
- `+10` Velocity increment.
- Existing success and failure messages.
- Deed `RootParent` ownership rule.
- Deed `Delete()` semantics.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.

## Goal Command

```text
/goal SOURCE-BATCH-008 VelocityDeed Guard Repair
```
