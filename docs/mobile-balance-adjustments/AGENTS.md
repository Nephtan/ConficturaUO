# Agent Instructions For Mobile Balance Adjustments

This file applies to `docs/mobile-balance-adjustments/` and its descendants.

## Purpose

This directory stores staff-authored balance data, normalized Codex-readable outputs, and review findings for mobile damage, skill, loot, and custom item changes.

## Source Snapshot Rules

- Treat files under `source/` as immutable evidence.
- Do not edit or overwrite a source snapshot. Add a new dated source file if staff provides a revised workbook.
- The canonical workbook in `workbooks/` and CSVs in `outputs/` may be regenerated from a source snapshot.

## Data Semantics

- Negative item skill modifiers are not stock `AosSkillBonuses`. Preserve them as custom signed equip/unequip skill-mod requirements.
- Do not silently resolve `Guaranteed=Yes` plus `ChancePercent < 100`. Flag it as `NeedsDecision` until staff chooses the intended drop rule.
- Do not silently resolve `OwnerBound=Yes`. Flag it as `NeedsOwnerBindingPolicy` until staff chooses when ownership binds.
- Keep raw staff text in `*Raw` columns. Put normalized values in canonical columns and child tables.
- Use repository-relative source paths in docs and CSVs.

## Validation

For workbook-only or documentation-only changes, run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceWorkbook.ps1
git diff --check
```

Confirm no source files under `Data/Scripts` changed unintentionally.

For later source-code implementation, follow the root `AGENTS.md` rules for runtime script compile, serialization, verification, staging, and commits.

## Documentation Updates

- Update `RUN_LOG.md` whenever tools are run or workbook outputs are regenerated.
- Update `REVIEW_FINDINGS.md` when blockers are resolved, reclassified, or newly found.
- Update `DATA_DICTIONARY.md` when sheet schemas or allowed values change.
- Preserve the distinction between staff source data, normalized workbook data, generated CSVs, and actual source-code implementation.
