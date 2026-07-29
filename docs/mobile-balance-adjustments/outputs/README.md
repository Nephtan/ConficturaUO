# Mobile Balance Outputs

This directory stores CSV exports normalized from `source/MobileBalanceTemplate.staff-revised-2026-07-28.xlsx` and checked against `workbooks/MobileBalanceTemplate.xlsx`.

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
| `ref-itemclasses.csv` | Source-generated item candidate classes plus proposed classes. |
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
| `review-issues.csv` | 3 |

The three review issues are the signed-skill implementation choice and the blank priorities for `MC-034` and `MC-035`. Loot semantics, owner binding, and the `DreadMace` class name are resolved.

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
