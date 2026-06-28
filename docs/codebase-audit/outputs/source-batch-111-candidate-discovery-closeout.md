# SOURCE-BATCH-111 Candidate Discovery Closeout

## Summary

SOURCE-BATCH-111+ required fresh candidate discovery after the SOURCE-BATCH-107 blacksmithing resource queue was exhausted. Discovery selected eight narrow trap/junk item guard candidates with zero POST-BATCH-Y gate hits and zero active overlay rows.

## Recommended Target

- Recommended: `SB111-CAND-001` / `SOURCE-BATCH-111` / BrokenGear Guard Repair.
- File: `Data/Scripts/Items/Traps/BrokenGear.cs`.
- Behavior: add stale/null/mobile/source-item guards to `OnDoubleClick`.
- Rationale: single non-gated trap/junk item file; guard-only changes preserve the existing no-effect message, constructor state, serialization, namespace/type/file layout, and project/config/data behavior.

## Next Clean Candidates

- `SB111-CAND-002` / `SOURCE-BATCH-112` / CurseItem Guard Repair.
- `SB111-CAND-003` / `SOURCE-BATCH-113` / TaintedBandage Guard Repair.
- `SB111-CAND-004` / `SOURCE-BATCH-114` / WeedItem Guard Repair.
- `SB111-CAND-005` / `SOURCE-BATCH-115` / SlimeItem Guard Repair.
- `SB111-CAND-006` / `SOURCE-BATCH-116` / SewageItem Guard Repair.
- `SB111-CAND-007` / `SOURCE-BATCH-117` / RottedReagents Guard Repair.
- `SB111-CAND-008` / `SOURCE-BATCH-118` / RuinedGems Guard Repair.

## Exclusions

- Boats, housing/addons, tools, weapons, fishing, food, and technology candidates remain deferred from this discovery because the trap/junk item cluster is smaller and has less policy or gameplay ambiguity.
- Staff/access, command policy, balance/economy, region/map policy changes, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain outside the non-gated runner.

## Verification

- `source-change-executive-decision-intake.csv` confirms `EXEC-0002` permits sequential non-gated repairs with one commit per batch.
- `source-batch-controller-roadmap-status.csv` and `source-batch-intake-register.csv` confirmed `SOURCE-BATCH-111+` was `CandidateDiscoveryRequired`.
- POST-BATCH-Y gate scans returned `0` matches for all eight selected trap/junk item files.
- Active overlay scans returned `0` matches for all eight selected trap/junk item files.
- Candidate CSV imports successfully with `Import-Csv`.

## Result

Discovery is complete. SOURCE-BATCH-111 may implement `SB111-CAND-001` if preflight still confirms zero gate hits and zero active overlay rows.
