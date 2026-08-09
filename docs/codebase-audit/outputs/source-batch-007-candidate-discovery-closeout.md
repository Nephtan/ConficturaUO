# SOURCE-BATCH-007 Candidate Discovery Closeout

Reviewed at: 2026-06-16T12:36:40.8277251-05:00

## Summary

`EXEC-0001` in `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv` directs non-gated source work to start with a candidate discovery list. This batch created `docs/codebase-audit/outputs/source-batch-007-candidate-discovery.csv` with five narrow zero-gate, zero-overlay source candidates and recommends `SB007-CAND-001` as the next implementation target.

No source, project, XML/config/data, serializer implementation, or reorganization files changed.

## Recommended Next Target

Recommended candidate: `SB007-CAND-001` / `SOURCE-BATCH-007 UnusualDyes Target Guard Repair`.

Target boundary:

- Behavior: add stale/null/backpack guards to `UnusualDyes.OnDoubleClick` and `DyeTarget.OnTarget`.
- System: `Items:Misc / Dyes / UnusualDyes`.
- Expected file: `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs`.
- Gate evidence: POST-BATCH-Y gate hits = 0.
- Active overlay evidence: active overlay rows = 0.
- Reason: this is a single consumable item interaction with the same stale source-item/mobile/target shape as the completed guard batches, with no policy, serializer, config, project, or reorganization gate hit.

Must stay unchanged:

- `DyeColor` persistence.
- randomized jar names/hues.
- `DyeTub.Redyable` behavior.
- `BlackDyeTub` rejection.
- `MagicPigment` hue assignment.
- `RevealingAction`.
- sound `0x23E`.
- empty `Jar` return.
- dye `Consume()` semantics.
- messages.
- serialization layout/versioning.
- namespace, type, file layout, project files, XML/config/data files, staff/access behavior, balance/economy behavior, and region/map behavior.

## Candidate List

| Candidate | Recommendation | System | File | POST-BATCH-Y hits | Active overlay rows |
| --- | --- | --- | --- | ---: | ---: |
| `SB007-CAND-001` | Recommended next target | Items:Misc / Dyes / UnusualDyes | `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs` | 0 | 0 |
| `SB007-CAND-002` | Strong follow-up | Items:Magical / VelocityDeed | `Data/Scripts/Items/Magical/VelocityDeed.cs` | 0 | 0 |
| `SB007-CAND-003` | Good follow-up | Items:Magical / WeaponRenamingTool | `Data/Scripts/Items/Magical/WeaponRenamingTool.cs` | 0 | 0 |
| `SB007-CAND-004` | Small safe follow-up | Items:Misc / Scales | `Data/Scripts/Items/Misc/Scales.cs` | 0 | 0 |
| `SB007-CAND-005` | Lower priority due to large transform surface | Items:Magical / MagicScissors | `Data/Scripts/Items/Magical/MagicScissors.cs` | 0 | 0 |

`Origami` and `KeyRing` remain excluded from this recommended path. Both still have zero POST-BATCH-Y gate hits, but `Origami.cs` has 7 active save-compat overlay rows and `KeyRing.cs` has 1 active save-compat overlay row. They should not be preferred over the clean zero-overlay candidates unless a later implementation goal explicitly proves serializer methods and saved state are untouched.

## Verification

- `git status --short` was clean before discovery edits.
- Applicable `AGENTS.md` files were re-read for repository root and `docs/codebase-audit/`; inspected source paths have no deeper applicable `AGENTS.md`.
- `EXEC-0001` was confirmed as `Ask for candidate discovery list`.
- `EXEC-0002` was confirmed as sequential runner only for non-gated rows.
- `SOURCE-BATCH-007+` was confirmed as `PendingConcreteSourceTarget`.
- POST-BATCH-Y gate matches were checked for every candidate file.
- Active overlay matches were checked for every candidate file.
- Candidate CSV imports with `Import-Csv`.
- Every candidate row has nonblank behavior, system, expected source files, gate result, risk, verification, and unchanged-behavior fields.
- The recommended candidate has 0 POST-BATCH-Y gate hits and 0 active overlay rows.
- No source/project/XML/config/data files changed.
- `git diff --check` passed.

## Outputs

- `docs/codebase-audit/outputs/source-batch-007-candidate-discovery.csv`
- `docs/codebase-audit/outputs/source-batch-007-candidate-discovery-closeout.md`
