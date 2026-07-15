# Mobile Balance Run Log

This log records commands and workflow actions for the mobile balance workbook effort.

## 2026-07-15 - Documentation And Workbook Upgrade

Working directory: `D:\ConficturaUO`

### Baseline

Command:

```powershell
git status --short
```

Result:

- Found untracked staff-filled workbook at `MobileBalanceTemplate.xlsx`.

### Source Snapshot Move

Command:

```powershell
New-Item -ItemType Directory -Force -Path 'docs/mobile-balance-adjustments/source','docs/mobile-balance-adjustments/workbooks','docs/mobile-balance-adjustments/outputs','docs/mobile-balance-adjustments/tools'
Move-Item -LiteralPath 'MobileBalanceTemplate.xlsx' -Destination 'docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-filled-2026-07-15.xlsx' -Force
```

Result:

- Moved the staff-filled workbook into `docs/mobile-balance-adjustments/source/` as the immutable source snapshot.

### Workbook Generation

Command:

```powershell
python docs/mobile-balance-adjustments/tools/New-MobileBalanceWorkbook.py
```

Result:

- Wrote `docs/mobile-balance-adjustments/workbooks/MobileBalanceTemplate.xlsx`.
- Wrote CSV outputs under `docs/mobile-balance-adjustments/outputs/`.
- Row counts:
  - `MobileChanges`: 33
  - `NewLootItems`: 39
  - `LootAssignments`: 39
  - `MobileSkillChanges`: 58
  - `ItemSkillMods`: 72
  - `ItemBonuses`: 110

Notes:

- The preferred spreadsheet artifact-tool loader was not exposed in this session, and no connected Excel document session was available.
- The generator uses only the Python standard library and writes the XLSX package directly.

### Workbook Validation

Command:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceWorkbook.ps1
```

Result:

- PASS.
- Workbook rows:
  - `MobileChanges`: 33
  - `NewLootItems`: 39
  - `LootAssignments`: 39
  - `MobileSkillChanges`: 58
  - `ItemSkillMods`: 72
  - `ItemBonuses`: 110
- Flagged policy rows:
  - `DropSemanticsStatus=NeedsDecision`: 39
  - `OwnerBoundPolicyStatus=NeedsOwnerBindingPolicy`: 8
