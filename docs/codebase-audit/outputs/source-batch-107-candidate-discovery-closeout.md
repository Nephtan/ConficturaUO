# SOURCE-BATCH-107 Candidate Discovery Closeout

## Summary

SOURCE-BATCH-107+ required fresh candidate discovery after the SOURCE-BATCH-104 tailoring cloth queue was exhausted. Discovery selected four narrow blacksmithing resource guard candidates with zero POST-BATCH-Y gate hits and zero active overlay rows.

## Recommended Target

- Recommended: `SB107-CAND-001` / `SOURCE-BATCH-107` / CaddelliteOre Guard Repair.
- File: `Data/Scripts/Items/Trades/Resources/Blacksmithing/CaddelliteOre.cs`.
- Behavior: add stale/null/mobile/source-ore/backpack/region guards to `OnDoubleClick`.
- Rationale: single non-gated resource file; guard-only changes preserve the Dwarven Forge conversion rule, skill threshold, messages, sound/animation, source deletion, serialization, namespace/type/file layout, and project/config/data behavior.

## Next Clean Candidates

- `SB107-CAND-002` / `SOURCE-BATCH-108` / RareMetals Guard Repair.
- `SB107-CAND-003` / `SOURCE-BATCH-109` / HardScales Guard Repair.
- `SB107-CAND-004` / `SOURCE-BATCH-110` / HardCrystals Guard Repair.

## Exclusions

- `Data/Scripts/Items/Trades/Resources/Blacksmithing/Ore.cs` was excluded because the active backlog overlay scan found one `SafeNoChange` save-compatibility row. This runner requires zero active overlay rows before source edits.
- `Data/Scripts/Items/Trades/Lumberjack/Log.cs` was excluded because the active backlog overlay scan found one `SafeNoChange` save-compatibility row. This runner requires zero active overlay rows before source edits.
- Fishing and broader harvest/reward candidates were deferred because the clean blacksmithing resource candidates are narrower and avoid reward/economy policy ambiguity.
- Staff/access, command policy, balance/economy, region/map policy changes, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain outside the non-gated runner.

## Verification

- `source-change-executive-decision-intake.csv` confirms `EXEC-0002` permits sequential non-gated repairs with one commit per batch.
- `source-batch-controller-roadmap-status.csv` and `source-batch-intake-register.csv` confirmed `SOURCE-BATCH-107+` was `CandidateDiscoveryRequired`.
- POST-BATCH-Y gate scans returned `0` matches for `CaddelliteOre.cs`, `RareMetals.cs`, `HardScales.cs`, and `HardCrystals.cs`.
- Active overlay scans returned `0` matches for `CaddelliteOre.cs`, `RareMetals.cs`, `HardScales.cs`, and `HardCrystals.cs`.
- Candidate CSV imports successfully with `Import-Csv`.

## Result

Discovery is complete. SOURCE-BATCH-107 may implement `SB107-CAND-001` if preflight still confirms zero gate hits and zero active overlay rows.
