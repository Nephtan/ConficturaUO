# SOURCE-BATCH-009 Target: WeaponRenamingTool Guard Repair

Reviewed at: 2026-06-16T18:14:26.8607304-05:00

## Target

- Batch: `SOURCE-BATCH-009`
- Candidate: `SB007-CAND-003`
- Behavior: add stale/null guard coverage to `WeaponRenamingTool.OnDoubleClick`, `TargetWeapon.OnTarget`, `Find(Mobile from)`, and `InternalGump.OnResponse`.
- System: `Items:Magical / WeaponRenamingTool`
- Expected source file: `Data/Scripts/Items/Magical/WeaponRenamingTool.cs`

## Governance

- Executive decision source: `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv`
- Candidate discovery source: `docs/codebase-audit/outputs/source-batch-007-candidate-discovery.csv`
- Candidate recommendation: `SB007-CAND-003` is the next clean non-gated follow-up candidate after `SOURCE-BATCH-008`.
- POST-BATCH-Y gate hits for expected file: `0`
- Active overlay rows for expected file: `0`
- Fence result: this batch stays inside a non-gated source-safe guard repair and crosses no staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate.

## Allowed Change

- In `OnDoubleClick(Mobile from)`, return when the mobile or tool state is stale.
- In `Find(Mobile from)`, return null when the mobile is null or deleted before reading backpack state.
- In `TargetWeapon.OnTarget`, return when mobile or tool state is stale, and use the existing invalid-target message for null/deleted/non-weapon targets.
- In `InternalGump.OnResponse`, return when `NetState`, mobile, relay info, tool, or target weapon state is stale before reading or mutating rename state.

## Must Stay Unchanged

- `RewardSystem.CheckIsUsableBy` behavior.
- `BaseWeapon` target eligibility.
- `InternalGump` text entry flow.
- 64-character truncation.
- Blank rename removal.
- Localized messages.
- Tool `Delete()` behavior on nonblank rename.
- `IsRewardItem` serialization.
- Namespace/type/file layout.
- Project/config/data files.

## Goal Command

```text
/goal SOURCE-BATCH-009 WeaponRenamingTool Guard Repair
```
