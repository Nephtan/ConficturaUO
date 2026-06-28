# SOURCE-BATCH-104 Candidate Discovery Closeout

## Summary

SOURCE-BATCH-104+ required fresh candidate discovery after the SOURCE-BATCH-099 tailoring resource queue was exhausted. Discovery selected three narrow Tailor resource guard candidates with zero POST-BATCH-Y gate hits and zero active overlay rows.

## Recommended Target

- Recommended: `SB104-CAND-001` / `SOURCE-BATCH-104` / Cloth Guard Repair.
- File: `Data/Scripts/Items/Trades/Resources/Tailor/Cloth.cs`.
- Behavior: add stale/null/mobile/source-cloth/dye-sender/scissors guards to `Dye`, `OnDoubleClick`, and `Scissor`.
- Rationale: single non-gated resource file; guard-only changes preserve existing folding, dyeing, scissoring, messaging, consumption/delete, serialization, namespace/type/file layout, and project/config/data behavior.

## Next Clean Candidates

- `SB104-CAND-002` / `SOURCE-BATCH-105` / BoltOfCloth Guard Repair.
- `SB104-CAND-003` / `SOURCE-BATCH-106` / UncutCloth Guard Repair.

## Exclusions

- `Data/Scripts/Items/Trades/Resources/Tailor/Hides.cs` was excluded because the active backlog overlay scan found one `SafeNoChange` row. This runner requires zero active overlay rows before source edits.
- Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain outside the non-gated runner.

## Verification

- `source-change-executive-decision-intake.csv` confirms `EXEC-0002` permits sequential non-gated repairs with one commit per batch.
- `source-batch-controller-roadmap-status.csv` and `source-batch-intake-register.csv` confirmed `SOURCE-BATCH-104+` was `CandidateDiscoveryRequired`.
- POST-BATCH-Y gate scans returned `0` matches for `Cloth.cs`, `BoltOfCloth.cs`, and `UncutCloth.cs`.
- Active overlay scans returned `0` matches for `Cloth.cs`, `BoltOfCloth.cs`, and `UncutCloth.cs`.
- Candidate CSV imports successfully with `Import-Csv`.

## Result

Discovery is complete. SOURCE-BATCH-104 may implement `SB104-CAND-001` if preflight still confirms zero gate hits and zero active overlay rows.
