# SOURCE-BATCH-152 BaseMagicStaff Guard Repair

Status: SkippedOverlayConflict

## Target

- Candidate: `SB144-CAND-009`
- System: `Items:Wands / BaseMagicStaff`
- Expected file: `Data/Scripts/Items/Wands/BaseMagicStaff.cs`
- Proposed behavior: add stale/null/mobile/source-staff guards to `BaseMagicStaff.OnDragLift(Mobile from)` before checking player type and sending the existing wand-use message.

## Fresh Preflight

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 4
- Active overlay row IDs: `RB-06714`, `RB-06715`, `RB-06731`, `RB-06782`
- Overlay evidence source: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
- Source inspection: `BaseMagicStaff.OnDragLift(Mobile from)` checks `from is PlayerMobile`, sends the existing wand-use message, and returns `true`.

## Decision

Skip this candidate before source edits. The sequential runner requires zero POST-BATCH-Y gate hits and zero active overlay rows before editing a candidate file. `BaseMagicStaff.cs` passed the gate scan but failed the active overlay scan.

## Must Stay Unchanged

- Staff charges
- Spell effects
- Equip policy
- Existing wand-use message
- Valid-state `true` return behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files

## Next Step

Advance to `SOURCE-BATCH-153+` / `SB144-CAND-010` / `WindRunnerScroll` after fresh zero-gate, zero-overlay preflight.
