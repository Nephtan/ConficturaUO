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

## 2026-07-28 - Staff Revision Repair

Working directory: `D:\ConficturaUO`

### Baseline

Command:

```powershell
git status --short
```

Result:

- The canonical workbook was the only pre-existing modified tracked file.
- No files under `Data/Scripts` were modified.

### Revised Source Snapshot

The open workbook was copied with shared-read access before repairs:

`docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-revised-2026-07-28.xlsx`

Snapshot facts:

- Size: 417,244 bytes.
- SHA-256: `97EF06A523D6F0228E3388B5F300AF0AD348F0B93CC50957F7045205A277F771`.
- The July 15 source snapshot remains unchanged.

### Live Workbook Repair

The connected Excel session was used to preserve the workbook's row-4 layout and repair generated data:

- Marked `MC-034` and `MC-035` as `ReviewStatus=Ready` and `ImplementationStatus=Pending`; their priorities remain blank.
- Added five `FireIllusion` and one `Lovecraftian` mobile skill rows.
- Marked all 39 loot rows `Guaranteed=No`, `DropRule=ChancePercentOnCorpse`, and `DropSemanticsStatus=Ready`.
- Replaced the last stale `Pestilence` assignment with `DreadMace`.
- Added `DreadMace.Knightship=-10` and rebuilt 73 sequential item skill rows.
- Updated `Weightoftheworld.Weight` from 300 to 500 in `ItemBonuses`.
- Synchronized all `DreadMace` bonus rows and added one proposed `Ref_ItemClasses` row.
- Replaced drifting dropdown endpoints with absolute source-backed list ranges.

### CSV Regeneration

Command:

```powershell
python docs/mobile-balance-adjustments/tools/New-MobileBalanceWorkbook.py --csv-only
```

Result:

- `MobileChanges`: 35
- `NewLootItems`: 39
- `LootAssignments`: 39
- `MobileSkillChanges`: 64
- `ItemSkillMods`: 73
- `ItemBonuses`: 110
- `review-issues.csv`: one signed-skill implementation constraint and two priority decisions.

### Validator Upgrade

`Test-MobileBalanceWorkbook.ps1` now checks:

- exact row-4 headers and the special `Reference` layout;
- exact normalized and reference row counts;
- absolute dropdown formulas;
- repository-relative mobile source paths;
- raw-to-normalized skill and bonus counts;
- item/assignment/child-table canonical class consistency;
- approved loot and owner-bound policy values;
- `DreadMace`, `Knightship=-10`, and `Weight=500`;
- all workbook-to-CSV row and cell values;
- the three expected review issues.

### Workbook Validation

Command:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceWorkbook.ps1
```

Result:

- PASS.
- `MobileChanges`: 35 rows.
- `NewLootItems`: 39 rows.
- `LootAssignments`: 39 rows.
- `MobileSkillChanges`: 64 rows.
- `ItemSkillMods`: 73 rows.
- `ItemBonuses`: 110 rows.
- Confirmed 39 independent corpse percentage rolls.
- Confirmed all 39 items are not owner-bound.
- Confirmed the only open decisions are signed skill handling and priorities for `MC-034` and `MC-035`.

### Package And Visual Inspection

The saved XLSX package was scanned directly:

- Formula cells: 0.
- Spreadsheet error tokens: 0.
- Revised source snapshot SHA-256 still matches `97EF06A523D6F0228E3388B5F300AF0AD348F0B93CC50957F7045205A277F771`.

The connected Excel session rendered and inspected all workbook sheets:

- `Reference`
- `MobileChanges`
- `NewLootItems`
- `LootAssignments`
- `MobileSkillChanges`
- `ItemSkillMods`
- `ItemBonuses`
- `Ref_Skills`
- `Ref_ItemClasses`
- `Ref_MobileClasses`
- `Ref_BonusNames`
- `Ref_DropRules`

Result:

- Row-4 headers, instruction bands, data ranges, and reference layouts are readable.
- No broken sheets, lost formatting, clipped table headers, or visibly invalid ranges were found.
- `DreadMace` appears in alphabetical order in `Ref_ItemClasses`.

### Repository Checks

Commands:

```powershell
git diff --check
git diff --name-only -- Data/Scripts
```

Result:

- `git diff --check` passed.
- No files under `Data/Scripts` changed.
