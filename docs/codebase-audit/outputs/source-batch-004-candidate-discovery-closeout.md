# SOURCE-BATCH-004 Candidate Discovery Closeout

Reviewed at: 2026-06-16T10:10:56.0324952-05:00

## Summary

`EXEC-0001` in `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv` directs the next action to produce a candidate discovery list for non-gated source work. This batch created `docs/codebase-audit/outputs/source-batch-004-candidate-discovery.csv` with five narrow `SOURCE-BATCH-004` candidates and recommends `SB004-CAND-001` as the next source target.

No source, project, XML/config/data, serializer implementation, or reorganization files changed.

## Recommended Next Target

Recommended candidate: `SB004-CAND-001` / `SOURCE-BATCH-004 ArcaneGem Interaction Guard Repair`.

Target boundary:

- Behavior: add stale/null/backpack guards to `ArcaneGem.OnDoubleClick` and `ArcaneGem.OnTarget`.
- System: `Items:Misc / ArcaneGem`.
- Expected file: `Data/Scripts/Items/Misc/ArcaneGem.cs`.
- Gate evidence: POST-BATCH-Y gate hits = 0.
- Active overlay evidence: active overlay rows = 0.
- Reason: the file has the same narrow item-interaction safety shape as the completed OilCloth and Firebomb batches, with no policy, serializer, config, project, or reorganization gate hit.

Must stay unchanged:

- Arcane charge math.
- `DefaultArcaneHue`.
- Tailoring thresholds.
- item eligibility and resource restrictions.
- blessed-item behavior.
- messages.
- gem amount/deletion semantics.
- serialization layout/versioning.
- namespace, type, file layout, project files, XML/config/data files, staff/access behavior, balance/economy behavior, and region/map behavior.

## Candidate List

| Candidate | Recommendation | System | File | POST-BATCH-Y hits | Active overlay rows |
| --- | --- | --- | --- | ---: | ---: |
| `SB004-CAND-001` | Recommended next target | Items:Misc / ArcaneGem | `Data/Scripts/Items/Misc/ArcaneGem.cs` | 0 | 0 |
| `SB004-CAND-002` | Strong follow-up | Items:Misc / PowerCrystal | `Data/Scripts/Items/Misc/PowerCrystal.cs` | 0 | 0 |
| `SB004-CAND-003` | Good follow-up | Items:Misc / ClockworkAssembly | `Data/Scripts/Items/Misc/ClockworkAssembly.cs` | 0 | 0 |
| `SB004-CAND-004` | Lower priority due to save overlay rows | Items:Misc / OrigamiPaper | `Data/Scripts/Items/Misc/Origami.cs` | 0 | 7 |
| `SB004-CAND-005` | Lower priority due to save overlay row | Items:Misc / KeyRing | `Data/Scripts/Items/Misc/KeyRing.cs` | 0 | 1 |

The lower-priority rows are still possible guard-only candidates, but they should not be preferred for the immediate next source batch because they share files with active `IntentionalLegacy` save-compatibility overlay rows. Any later implementation must prove serializer methods were untouched.

## Verification

- `git status --short` was clean before discovery edits.
- Applicable `AGENTS.md` files were re-read for repository root and `docs/codebase-audit/`; inspected source paths have no deeper applicable `AGENTS.md`.
- `EXEC-0001` was confirmed as `Ask for candidate discovery list`.
- `SOURCE-BATCH-004+` was confirmed as `PendingConcreteSourceTarget`.
- POST-BATCH-Y gate matches were checked for every candidate file.
- Active overlay matches were checked for every candidate file.
- Candidate CSV imports with `Import-Csv`.
- Every candidate row has nonblank behavior, system, expected source files, gate result, risk, verification, and unchanged-behavior fields.
- No recommended candidate crosses POST-BATCH-Y gated areas.
- No source/project/XML/config/data files changed.
- `git diff --check` passed.

## Outputs

- `docs/codebase-audit/outputs/source-batch-004-candidate-discovery.csv`
- `docs/codebase-audit/outputs/source-batch-004-candidate-discovery-closeout.md`
