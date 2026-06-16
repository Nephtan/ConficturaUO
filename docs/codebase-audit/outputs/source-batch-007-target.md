# SOURCE-BATCH-007 Target: UnusualDyes Target Guard Repair

Reviewed at: 2026-06-16T14:53:11.7853163-05:00

## Target

- Batch: `SOURCE-BATCH-007`
- Candidate: `SB007-CAND-001`
- Behavior: add stale/null/backpack guards to `UnusualDyes.OnDoubleClick(Mobile from)` and `DyeTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Misc / Dyes / UnusualDyes`
- Expected source file: `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs`

## Governance

- Executive decision source: `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv`
- Candidate discovery source: `docs/codebase-audit/outputs/source-batch-007-candidate-discovery.csv`
- Candidate recommendation: `SB007-CAND-001` is the recommended implementation target.
- POST-BATCH-Y gate hits for expected file: `0`
- Active overlay rows for expected file: `0`
- Fence result: this batch stays inside a non-gated source-safe guard repair and crosses no staff/access, balance/economy, region/map, serializer migration, project/config/data, or reorganization gate.

## Allowed Change

- In `OnDoubleClick(Mobile from)`, return when `from == null || from.Deleted`.
- In `OnDoubleClick(Mobile from)`, use existing localized backpack-use failure `1060640` when the dye item is deleted, the backpack is missing, or the dye item is outside the backpack.
- In `DyeTarget.OnTarget`, return when `from == null || from.Deleted`.
- In `DyeTarget.OnTarget`, use existing localized backpack-use failure `1060640` when the source dye is null, deleted, outside the backpack, or the backpack is missing.
- Move the `m_Dye.DyeColor` read until after source dye/mobile/backpack guards.
- Treat null or deleted target dye tubs/pigments as the existing invalid-target path without mutation or consumption.

## Must Stay Unchanged

- `DyeColor` persistence.
- Randomized jar names and hues.
- `DyeTub.Redyable` behavior.
- `BlackDyeTub` rejection behavior.
- `MagicPigment` hue assignment behavior.
- `RevealingAction`.
- Sound `0x23E`.
- Empty `Jar` return.
- Dye `Consume()` semantics.
- Localized messages.
- Target eligibility except stale/deleted/null safety.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Goal Command

```text
/goal SOURCE-BATCH-007 UnusualDyes Target Guard Repair
```
