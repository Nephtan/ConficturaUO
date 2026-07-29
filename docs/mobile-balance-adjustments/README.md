# Mobile Balance Adjustments

This directory is the durable working area for staff-proposed mobile balance, loot, and custom item changes. It exists so staff can fill out structured workbook data and Codex can later implement the changes without relying on chat history or Discord screenshots.

The workflow mirrors the practical parts of `docs/codebase-audit`: keep source evidence, keep machine-readable outputs, record policy blockers, and validate before code changes.

## Read This First

| Need | File |
| --- | --- |
| Staff-editable workbook | [workbooks/MobileBalanceTemplate.xlsx](workbooks/MobileBalanceTemplate.xlsx) |
| Current staff submission snapshot | [source/MobileBalanceTemplate.staff-revised-2026-07-28.xlsx](source/MobileBalanceTemplate.staff-revised-2026-07-28.xlsx) |
| Original staff submission snapshot | [source/MobileBalanceTemplate.staff-filled-2026-07-15.xlsx](source/MobileBalanceTemplate.staff-filled-2026-07-15.xlsx) |
| Sheet and column definitions | [DATA_DICTIONARY.md](DATA_DICTIONARY.md) |
| Current review findings and blockers | [REVIEW_FINDINGS.md](REVIEW_FINDINGS.md) |
| Codex rules for this area | [AGENTS.md](AGENTS.md) |
| Command and generation log | [RUN_LOG.md](RUN_LOG.md) |
| CSV output catalog | [outputs/README.md](outputs/README.md) |

## Directory Map

| Path | Purpose |
| --- | --- |
| `source/` | Immutable source submissions. Do not clean up or overwrite these files. |
| `workbooks/` | Canonical staff-editable workbook with dropdowns and normalized helper sheets. |
| `outputs/` | CSV exports for Codex review, implementation planning, and validation. |
| `tools/` | Reproducible generation and validation tools. |

## Current Workbook State

The canonical workbook was repaired from the staff-revised snapshot on July 28, 2026. Every structured sheet uses row 4 for headers and row 5 onward for data. `Reference` is the intentional exception: it is an instructional sheet with separate sections.

| Area | Count |
| --- | ---: |
| Mobile change rows | 35 |
| Loot item rows | 39 |
| Loot assignment rows | 39 |
| Normalized mobile skill rows | 64 |
| Normalized item skill modifier rows | 73 |
| Normalized item bonus rows | 110 |

Staff has resolved these earlier blockers:

- All 39 loot rows are independent `ChancePercentOnCorpse` rolls with `Guaranteed=No`.
- All 39 items have `OwnerBound=No`.
- The conflicting mace class is now `DreadMace`, displayed as `The Dread Mace`.
- Blank loot links for `FireIllusion` and `Lovecraftian` mean no loot change.

Two choices remain in [REVIEW_FINDINGS.md](REVIEW_FINDINGS.md):

- How to handle 36 negative skill modifiers across 19 items.
- Whether `MC-034 Fire Illusion` and `MC-035 Lovecraftian` are `High`, `Medium`, or `Low` priority.

## Staff Workflow

1. Open [workbooks/MobileBalanceTemplate.xlsx](workbooks/MobileBalanceTemplate.xlsx).
2. Use the first three sheets for broad editing: `MobileChanges`, `NewLootItems`, and `LootAssignments`. Their headers are on row 4.
3. Use dropdowns where available. The `Ref_*` sheets are source-backed references for class names, skills, bonuses, and drop rules.
4. Do not edit `source/`. If staff sends a new workbook, save it as a new dated source snapshot.
5. If a row is marked `NeedsDecision`, resolve the policy question in the workbook notes before asking Codex to implement code changes.

## Codex Workflow

1. Read [AGENTS.md](AGENTS.md), [DATA_DICTIONARY.md](DATA_DICTIONARY.md), and [REVIEW_FINDINGS.md](REVIEW_FINDINGS.md).
2. Use CSVs in [outputs/](outputs/) for implementation planning. The validator checks workbook-to-CSV content parity.
3. Run the validator:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceWorkbook.ps1
```

4. Treat `NeedsDecision` rows as blockers for source implementation unless the user explicitly resolves the policy.
5. When later changing `Data/Scripts`, follow the root `AGENTS.md` build, runtime script compile, serialization, and commit rules.

## Regenerating The Workbook

The CSVs can be regenerated from the current immutable source snapshot without touching an open workbook:

```powershell
python docs/mobile-balance-adjustments/tools/New-MobileBalanceWorkbook.py --csv-only
powershell -NoProfile -ExecutionPolicy Bypass -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceWorkbook.ps1
```

Run the generator without `--csv-only` only when deliberately rebuilding the canonical workbook. It reads the July 28 snapshot by default, accepts both original and enhanced column names, scans source files for current skills/classes, preserves raw staff text, and writes stable source-backed dropdown validations.
