# Mobile Balance Outputs

This directory stores CSV exports normalized from `source/MobileBalanceTemplate.staff-approved-2026-07-28.xlsx` and checked against `workbooks/MobileBalanceTemplate.xlsx`.

Use these files for Codex implementation planning and validation. They are easier to parse than workbook free text and preserve normalized values, policy flags, and source-backed references.

## Output Catalog

| File | Purpose |
| --- | --- |
| `mobilechanges.csv` | Enhanced mobile change rows. |
| `newlootitems.csv` | Enhanced item rows with canonical class/base names and policy status. |
| `lootassignments.csv` | Enhanced drop assignment rows with explicit drop-rule status. |
| `mobileskillchanges.csv` | One row per normalized mobile skill change. |
| `itemskillmods.csv` | One row per normalized item skill modifier. |
| `itembonuses.csv` | One row per normalized item bonus/property. |
| `ref-skills.csv` | Source-generated skill names. |
| `ref-itemclasses.csv` | Source-generated item classes, including all 23 implemented balance classes. |
| `ref-mobileclasses.csv` | Source-generated mobile candidate classes and display names. |
| `ref-bonusnames.csv` | Canonical item bonus names and implementation surfaces. |
| `ref-droprules.csv` | Allowed drop-rule semantics. |
| `review-issues.csv` | Current policy and implementation findings extracted from the workbook. |

## Current Counts

| File | Data rows |
| --- | ---: |
| `mobilechanges.csv` | 35 |
| `newlootitems.csv` | 39 |
| `lootassignments.csv` | 39 |
| `mobileskillchanges.csv` | 64 |
| `itemskillmods.csv` | 73 |
| `itembonuses.csv` | 110 |
| `review-issues.csv` | 0 |

There are no current review issues. The CSV retains its header so consumers can import it without special handling. All 35 mobile rows and all 39 item rows have `ImplementationStatus=Implemented`.

## Reading Tips

Use PowerShell from the repository root:

```powershell
Import-Csv docs/mobile-balance-adjustments/outputs/review-issues.csv |
    Sort-Object Severity,IssueId |
    Format-Table -AutoSize
```

```powershell
Import-Csv docs/mobile-balance-adjustments/outputs/itemskillmods.csv |
    Where-Object { $_.ModifierKind -eq 'CustomSignedEquipSkillMod' } |
    Select-Object ItemId,CanonicalClassName,CanonicalSkillName,Value,ImplementationSurface |
    Format-Table -AutoSize
```

Do not edit generated CSVs by hand. Update the source workbook or generator, then regenerate and validate.

Final regeneration command:

```powershell
python docs/mobile-balance-adjustments/tools/New-MobileBalanceWorkbook.py --implementation-complete --csv-only
```
