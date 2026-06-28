# SOURCE-BATCH-028 Candidate Discovery Closeout

Reviewed at: 2026-06-16T20:04:51-05:00

## Summary

`SOURCE-BATCH-028` ran a discovery-only pass after `SOURCE-BATCH-027` exhausted `source-batch-025-candidate-discovery.csv`.

The pass identified three clean, non-gated, zero-overlay guard candidates:

1. `SB028-CAND-001` / `SOURCE-BATCH-028` / DyeTub Guard Repair
2. `SB028-CAND-002` / `SOURCE-BATCH-029` / Key Interaction Guard Repair
3. `SB028-CAND-003` / `SOURCE-BATCH-030` / PuzzleCube Guard Repair

`SB028-CAND-001` is the recommended next implementation target.

## Discovery Scope

- Preferred stale/null/mobile/backpack/source-item/target guard repairs.
- Filtered candidates against `post-batch-y-source-change-gate-register.csv`.
- Filtered candidates against `post-audit-active-backlog-status.csv`.
- Excluded staff/access, command policy, economy/reward, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces.

## Gate And Overlay Evidence

| Candidate | File | POST-BATCH-Y gate hits | Active overlay rows |
| --- | --- | ---: | ---: |
| `SB028-CAND-001` | `Data/Scripts/Items/Misc/Dyes/DyeTub.cs` | 0 | 0 |
| `SB028-CAND-002` | `Data/Scripts/Items/Misc/Key.cs` | 0 | 0 |
| `SB028-CAND-003` | `Data/Scripts/Items/Misc/Games/PuzzleCube.cs` | 0 | 0 |

## Exclusions

- `Data/Scripts/Items/Magical/Gifts/Weapons/GiftStave.cs`, `Data/Scripts/Items/Magical/Gifts/Weapons/GiftThrowingGloves.cs`, `Data/Scripts/Items/Magical/God/Weapons/LevelStave.cs`, and `Data/Scripts/Items/Magical/God/Weapons/LevelThrowingGloves.cs` had zero gate hits and zero active overlay rows, but were excluded from the automated queue because they are progression, reward, and artifact-policy adjacent.
- `Data/Scripts/Items/Magical/Moonstone.cs` had zero gate hits and zero active overlay rows, but was excluded because the interaction is travel/world adjacent under the preserved region/map policy.
- `Data/Scripts/Items/Misc/Bola.cs` had zero gate hits and zero active overlay rows, but was excluded because it is combat and PvP adjacent under the preserved balance/region policy.
- `Data/Scripts/Items/Misc/Dyes/HueStone.cs` had zero gate hits and zero active overlay rows, but was excluded from this runnable queue because its charge, gold, and broad hue-application paths are economy-adjacent and higher risk than the selected guard-only candidates.
- Active-overlay candidates such as `KeyRing.cs`, `Origami.cs`, `RuneOfVirtue.cs`, `BaseBoard.cs`, `PlayerVendorDeed.cs`, `LegendaryArtifactRename.cs`, `MagicCandle.cs`, `MagicTorch.cs`, `MagicLantern.cs`, and related obsolete artifact files remain excluded unless a later goal proves the relevant overlay rows are not crossed.

## Verification

- `source-change-executive-decision-intake.csv` was checked for `EXEC-0002`, which allows sequential runners only for non-gated rows with a commit after each batch.
- `source-batch-controller-roadmap-status.csv` showed `SOURCE-BATCH-028+` as `PendingConcreteSourceTarget` with `CandidateDiscoveryRequired`.
- Candidate source files were inspected directly.
- Selected candidate files have zero POST-BATCH-Y gate hits and zero active overlay rows.
- No source/project/XML/config/data behavior files changed in this discovery batch.
- `git diff --check` passed with expected LF-to-CRLF warnings only.

## Result

- Durable discovery output exists at `source-batch-028-candidate-discovery.csv`.
- `SOURCE-BATCH-028 DyeTub Guard Repair` is the next concrete non-gated target.
- No source code changed in this discovery batch.
