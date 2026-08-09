# SOURCE-BATCH-154 Candidate Discovery Closeout

## Summary

Fresh discovery after `SOURCE-BATCH-153` found nine clean Mystic scroll guard candidates. Each candidate is one file, has 0 POST-BATCH-Y gate hits, has 0 active overlay rows, and shares the same stale/null mobile/source-scroll guard gap in `OnDoubleClick(Mobile from)` and `OnDragLift(Mobile from)`.

## Recommended Next Target

- Candidate: `SB154-CAND-001`
- Batch: `SOURCE-BATCH-154`
- Target: `AstralProjectionScroll`
- File: `Data/Scripts/Magic/Mystic/Scrolls/AstralProjectionScroll.cs`

## Candidate Set

- `SB154-CAND-001` / `SOURCE-BATCH-154` / `AstralProjectionScroll`
- `SB154-CAND-002` / `SOURCE-BATCH-155` / `AstralTravelScroll`
- `SB154-CAND-003` / `SOURCE-BATCH-156` / `CreateRobeScroll`
- `SB154-CAND-004` / `SOURCE-BATCH-157` / `GentleTouchScroll`
- `SB154-CAND-005` / `SOURCE-BATCH-158` / `LeapScroll`
- `SB154-CAND-006` / `SOURCE-BATCH-159` / `PsionicBlastScroll`
- `SB154-CAND-007` / `SOURCE-BATCH-160` / `PsychicWallScroll`
- `SB154-CAND-008` / `SOURCE-BATCH-161` / `PurityOfBodyScroll`
- `SB154-CAND-009` / `SOURCE-BATCH-162` / `QuiveringPalmScroll`

## Discovery Evidence

- Discovery source: `Data/Scripts/Magic/Mystic/Scrolls/*.cs`
- Excluded completed candidate: `WindRunnerScroll`
- Required shape: `OnDoubleClick(Mobile from)` and `OnDragLift(Mobile from)` present; existing stale guard absent.
- POST-BATCH-Y gate hits: 0 for every candidate file.
- Active overlay rows: 0 for every candidate file.

## Guard Boundary

Allowed future edits are local stale/null mobile/source-scroll guards only. Preserve owner serialization, valid non-owner crumble/delete behavior, valid owner tome message, valid-state `OnDragLift` `true` return, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
