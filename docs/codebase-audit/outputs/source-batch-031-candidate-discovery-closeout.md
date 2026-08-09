# SOURCE-BATCH-031 Candidate Discovery Closeout

Reviewed at: 2026-06-16T20:23:00-05:00

## Summary

`SOURCE-BATCH-031` ran a discovery-only pass after `SOURCE-BATCH-030` exhausted `source-batch-028-candidate-discovery.csv`.

The pass identified six clean, non-gated, zero-overlay D&D dice guard candidates:

1. `SB031-CAND-001` / `SOURCE-BATCH-031` / Dice4 Guard Repair
2. `SB031-CAND-002` / `SOURCE-BATCH-032` / Dice6 Guard Repair
3. `SB031-CAND-003` / `SOURCE-BATCH-033` / Dice8 Guard Repair
4. `SB031-CAND-004` / `SOURCE-BATCH-034` / Dice10 Guard Repair
5. `SB031-CAND-005` / `SOURCE-BATCH-035` / Dice12 Guard Repair
6. `SB031-CAND-006` / `SOURCE-BATCH-036` / Dice20 Guard Repair

`SB031-CAND-001` is the recommended next implementation target.

## Discovery Scope

- Preferred stale/null/mobile guard repairs in simple item interaction flows.
- Filtered candidates against `post-batch-y-source-change-gate-register.csv`.
- Filtered candidates against `post-audit-active-backlog-status.csv`.
- Excluded active-overlay, staff/access, command policy, economy/reward, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces.

## Gate And Overlay Evidence

| Candidate | File | POST-BATCH-Y gate hits | Active overlay rows |
| --- | --- | ---: | ---: |
| `SB031-CAND-001` | `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs` | 0 | 0 |
| `SB031-CAND-002` | `Data/Scripts/Items/Misc/Games/DandD/Dice6.cs` | 0 | 0 |
| `SB031-CAND-003` | `Data/Scripts/Items/Misc/Games/DandD/Dice8.cs` | 0 | 0 |
| `SB031-CAND-004` | `Data/Scripts/Items/Misc/Games/DandD/Dice10.cs` | 0 | 0 |
| `SB031-CAND-005` | `Data/Scripts/Items/Misc/Games/DandD/Dice12.cs` | 0 | 0 |
| `SB031-CAND-006` | `Data/Scripts/Items/Misc/Games/DandD/Dice20.cs` | 0 | 0 |

## Exclusions

- `Data/Scripts/Items/Misc/Games/Dices.cs` has zero POST-BATCH-Y gate hits but active overlay rows still match the file, so it remains excluded from the automated queue.
- Additional policy-adjacent or overlay-conflicting candidates from the broader scan remain excluded until a later discovery proves they are zero-gate, zero-overlay, narrow guard-only work.

## Verification

- `source-change-executive-decision-intake.csv` allows sequential runners only for non-gated rows with a commit after each batch.
- `source-batch-controller-roadmap-status.csv` showed `SOURCE-BATCH-031+` as `PendingConcreteSourceTarget` with `CandidateDiscoveryRequired`.
- Candidate source files were inspected directly.
- Selected candidate files have zero POST-BATCH-Y gate hits and zero active overlay rows.
- No source/project/XML/config/data behavior files changed in this discovery batch.
- `git diff --check` passed with expected LF-to-CRLF warnings only.

## Result

- Durable discovery output exists at `source-batch-031-candidate-discovery.csv`.
- `SOURCE-BATCH-031 Dice4 Guard Repair` is the next concrete non-gated target.
- No source code changed in this discovery batch.
