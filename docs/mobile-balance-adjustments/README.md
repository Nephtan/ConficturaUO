# Mobile Balance Adjustments

This directory is the durable working area for staff-proposed mobile balance, loot, and custom item changes. It exists so staff can fill out structured workbook data and Codex can later implement the changes without relying on chat history or Discord screenshots.

The workflow mirrors the practical parts of `docs/codebase-audit`: keep source evidence, keep machine-readable outputs, record policy blockers, and validate before code changes.

## Read This First

| Need | File |
| --- | --- |
| Staff-editable workbook | [workbooks/MobileBalanceTemplate.xlsx](workbooks/MobileBalanceTemplate.xlsx) |
| Exact staff submission snapshot | [source/MobileBalanceTemplate.staff-filled-2026-07-15.xlsx](source/MobileBalanceTemplate.staff-filled-2026-07-15.xlsx) |
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

The canonical workbook was generated from the staff-filled snapshot on July 15, 2026.

| Area | Count |
| --- | ---: |
| Mobile change rows | 33 |
| Loot item rows | 39 |
| Loot assignment rows | 39 |
| Normalized mobile skill rows | 58 |
| Normalized item skill modifier rows | 72 |
| Normalized item bonus rows | 110 |

Important current blockers are recorded in [REVIEW_FINDINGS.md](REVIEW_FINDINGS.md):

- 39 loot rows need a staff decision because `Guaranteed=Yes` is paired with `ChancePercent` below 100.
- 8 owner-bound items need an owner binding policy.
- Signed item skill modifiers require custom equip/unequip skill-mod handling instead of stock `SkillBonuses.SetValues`.
- The requested `Pestilence` mace name conflicts with an existing obsolete `Pestilence` source class.

## Staff Workflow

1. Open [workbooks/MobileBalanceTemplate.xlsx](workbooks/MobileBalanceTemplate.xlsx).
2. Use the first three sheets for broad editing: `MobileChanges`, `NewLootItems`, and `LootAssignments`.
3. Use dropdowns where available. The `Ref_*` sheets are source-backed references for class names, skills, bonuses, and drop rules.
4. Do not edit `source/`. If staff sends a new workbook, save it as a new dated source snapshot.
5. If a row is marked `NeedsDecision`, resolve the policy question in the workbook notes before asking Codex to implement code changes.

## Codex Workflow

1. Read [AGENTS.md](AGENTS.md), [DATA_DICTIONARY.md](DATA_DICTIONARY.md), and [REVIEW_FINDINGS.md](REVIEW_FINDINGS.md).
2. Use CSVs in [outputs/](outputs/) for implementation planning instead of parsing staff free text.
3. Run the validator:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceWorkbook.ps1
```

4. Treat `NeedsDecision` rows as blockers for source implementation unless the user explicitly resolves the policy.
5. When later changing `Data/Scripts`, follow the root `AGENTS.md` build, runtime script compile, serialization, and commit rules.

## Regenerating The Workbook

The generated workbook and CSVs can be rebuilt from the immutable source snapshot:

```powershell
python docs/mobile-balance-adjustments/tools/New-MobileBalanceWorkbook.py
powershell -NoProfile -ExecutionPolicy Bypass -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceWorkbook.ps1
```

The generator uses only the Python standard library. It reads the source workbook as an XLSX package, scans source files for skills/classes, normalizes rows, writes CSVs, and writes a fresh workbook with dropdown validations.
