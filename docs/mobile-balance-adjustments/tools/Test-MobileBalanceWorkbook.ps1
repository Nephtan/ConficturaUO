param(
    [string]$WorkbookPath = "docs/mobile-balance-adjustments/workbooks/MobileBalanceTemplate.xlsx",
    [string]$OutputDirectory = "docs/mobile-balance-adjustments/outputs"
)

$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.IO.Compression.FileSystem

function Read-ZipXmlText {
    param(
        [System.IO.Compression.ZipArchive]$Zip,
        [string]$Name
    )

    $entry = $Zip.GetEntry($Name)
    if ($entry -eq $null) {
        throw "Missing XLSX entry: $Name"
    }

    $stream = $entry.Open()
    try {
        $reader = [System.IO.StreamReader]::new($stream)
        try {
            return $reader.ReadToEnd()
        }
        finally {
            $reader.Dispose()
        }
    }
    finally {
        $stream.Dispose()
    }
}

function ConvertTo-XlsxCellText {
    param(
        [System.Xml.XmlElement]$Cell,
        [object[]]$SharedStrings
    )

    $type = [string]$Cell.GetAttribute("t")
    $valueNode = $Cell.SelectSingleNode("*[local-name()='v']")

    if ($type -eq "s") {
        if ($valueNode -eq $null -or $valueNode.InnerText -eq "") {
            return ""
        }

        return [string]$SharedStrings[[int]$valueNode.InnerText]
    }

    if ($type -eq "inlineStr") {
        $pieces = @()
        foreach ($textNode in $Cell.SelectNodes(".//*[local-name()='t']")) {
            $pieces += $textNode.InnerText
        }

        return [string]($pieces -join "")
    }

    if ($valueNode -ne $null) {
        return [string]$valueNode.InnerText
    }

    return ""
}

function Get-ColumnName {
    param([string]$CellRef)

    return ([regex]::Match($CellRef, "^[A-Z]+")).Value
}

function Read-XlsxWorkbook {
    param([string]$Path)

    $resolvedPath = (Resolve-Path -LiteralPath $Path).Path
    $zip = [System.IO.Compression.ZipFile]::OpenRead($resolvedPath)
    try {
        $sharedStrings = @()
        if ($zip.GetEntry("xl/sharedStrings.xml") -ne $null) {
            [xml]$sharedDoc = Read-ZipXmlText -Zip $zip -Name "xl/sharedStrings.xml"
            foreach ($si in $sharedDoc.SelectNodes("//*[local-name()='si']")) {
                $pieces = @()
                foreach ($textNode in $si.SelectNodes(".//*[local-name()='t']")) {
                    $pieces += $textNode.InnerText
                }

                $sharedStrings += ($pieces -join "")
            }
        }

        [xml]$workbookDoc = Read-ZipXmlText -Zip $zip -Name "xl/workbook.xml"
        [xml]$relsDoc = Read-ZipXmlText -Zip $zip -Name "xl/_rels/workbook.xml.rels"
        $relsById = @{}
        foreach ($rel in $relsDoc.Relationships.Relationship) {
            $relsById[[string]$rel.Id] = [string]$rel.Target
        }

        $result = [ordered]@{}
        $validations = [ordered]@{}

        foreach ($sheetNode in $workbookDoc.SelectNodes("//*[local-name()='sheet']")) {
            $sheetName = [string]$sheetNode.name
            $relId = $sheetNode.GetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships")
            $target = $relsById[$relId]
            $entryName = if ($target.StartsWith("/")) { $target.TrimStart("/") } else { "xl/" + $target }
            [xml]$sheetDoc = Read-ZipXmlText -Zip $zip -Name $entryName

            $rows = @()
            foreach ($row in $sheetDoc.SelectNodes("//*[local-name()='sheetData']/*[local-name()='row']")) {
                $cells = [ordered]@{}
                foreach ($cell in $row.SelectNodes("*[local-name()='c']")) {
                    $cells[(Get-ColumnName -CellRef ([string]$cell.r))] = ConvertTo-XlsxCellText -Cell $cell -SharedStrings $sharedStrings
                }

                $rows += [pscustomobject]@{
                    Row = [int]$row.r
                    Cells = $cells
                }
            }

            $header = $rows | Where-Object { $_.Row -eq 4 } | Select-Object -First 1
            $headers = [ordered]@{}
            if ($header -ne $null) {
                foreach ($key in $header.Cells.Keys) {
                    if ($header.Cells[$key] -ne "") {
                        $headers[$key] = $header.Cells[$key]
                    }
                }
            }

            $objects = @()
            foreach ($row in $rows | Where-Object { $_.Row -gt 4 }) {
                $hasData = $false
                $object = [ordered]@{}
                foreach ($col in $headers.Keys) {
                    $value = if ($row.Cells.Contains($col)) { [string]$row.Cells[$col] } else { "" }
                    if ($value -ne "") {
                        $hasData = $true
                    }

                    $object[$headers[$col]] = $value
                }

                if ($hasData) {
                    $objects += [pscustomobject]$object
                }
            }

            $result[$sheetName] = $objects
            $validations[$sheetName] = @($sheetDoc.SelectNodes("//*[local-name()='dataValidation']")).Count
        }

        return [pscustomobject]@{
            Sheets = $result
            ValidationCounts = $validations
        }
    }
    finally {
        $zip.Dispose()
    }
}

function Add-Error {
    param([string]$Message)

    $script:Errors += $Message
}

function Add-Warning {
    param([string]$Message)

    $script:Warnings += $Message
}

$script:Errors = @()
$script:Warnings = @()

$workbook = Read-XlsxWorkbook -Path $WorkbookPath
$sheets = $workbook.Sheets

$expectedSheets = @(
    "MobileChanges",
    "NewLootItems",
    "LootAssignments",
    "MobileSkillChanges",
    "ItemSkillMods",
    "ItemBonuses",
    "Ref_Skills",
    "Ref_ItemClasses",
    "Ref_MobileClasses",
    "Ref_BonusNames",
    "Ref_DropRules"
)

foreach ($sheetName in $expectedSheets) {
    if (-not $sheets.Contains($sheetName)) {
        Add-Error "Missing sheet: $sheetName"
    }
}

foreach ($sheetName in @("MobileChanges", "NewLootItems", "LootAssignments", "MobileSkillChanges", "ItemSkillMods", "ItemBonuses")) {
    if ($workbook.ValidationCounts[$sheetName] -le 0) {
        Add-Error "Expected dropdown/data validation rules on $sheetName."
    }
}

if ($script:Errors.Count -eq 0) {
    $items = @($sheets["NewLootItems"])
    $assignments = @($sheets["LootAssignments"])
    $mobileChanges = @($sheets["MobileChanges"])
    $mobileSkills = @($sheets["MobileSkillChanges"])
    $itemSkills = @($sheets["ItemSkillMods"])
    $itemBonuses = @($sheets["ItemBonuses"])
    $refSkills = @($sheets["Ref_Skills"])
    $refBonuses = @($sheets["Ref_BonusNames"])

    $itemIds = @{}
    foreach ($item in $items) {
        $itemIds[$item.ItemId] = $true
    }

    $assignmentIds = @{}
    foreach ($assignment in $assignments) {
        $assignmentIds[$assignment.AssignmentId] = $true
    }

    foreach ($assignment in $assignments) {
        if (-not $itemIds.ContainsKey($assignment.ItemId)) {
            Add-Error "LootAssignments references missing ItemId '$($assignment.ItemId)' on assignment '$($assignment.AssignmentId)'."
        }

        $chance = 0.0
        [void][double]::TryParse($assignment.ChancePercent, [ref]$chance)
        if ($assignment.Guaranteed -eq "Yes" -and $chance -lt 100 -and $assignment.DropSemanticsStatus -ne "NeedsDecision") {
            Add-Error "Assignment '$($assignment.AssignmentId)' has Guaranteed=Yes and ChancePercent < 100 but is not flagged NeedsDecision."
        }
    }

    foreach ($mobileChange in $mobileChanges) {
        if ($mobileChange.LootAssignmentIds -ne "" -and -not $assignmentIds.ContainsKey($mobileChange.LootAssignmentIds)) {
            Add-Error "MobileChanges row '$($mobileChange.ChangeId)' references missing LootAssignmentIds '$($mobileChange.LootAssignmentIds)'."
        }
    }

    $skillNames = @{}
    foreach ($skill in $refSkills) {
        $skillNames[$skill.SkillName] = $true
    }

    foreach ($skillRow in $mobileSkills + $itemSkills) {
        if ($skillRow.CanonicalSkillName -ne "" -and -not $skillNames.ContainsKey($skillRow.CanonicalSkillName)) {
            Add-Error "Unknown canonical skill '$($skillRow.CanonicalSkillName)' in normalized skill rows."
        }
    }

    $bonusNames = @{}
    foreach ($bonus in $refBonuses) {
        $bonusNames[$bonus.BonusName] = $true
    }

    foreach ($bonusRow in $itemBonuses) {
        if ($bonusRow.Status -eq "Ready" -and -not $bonusNames.ContainsKey($bonusRow.CanonicalBonusName)) {
            Add-Error "Ready item bonus '$($bonusRow.CanonicalBonusName)' is absent from Ref_BonusNames."
        }
    }

    foreach ($item in $items) {
        if ($item.OwnerBound -eq "Yes" -and $item.OwnerBoundPolicyStatus -eq "NotOwnerBound") {
            Add-Error "Item '$($item.ItemId)' is OwnerBound=Yes but not flagged for owner policy."
        }
    }

    foreach ($skillRow in $itemSkills) {
        $value = 0.0
        [void][double]::TryParse($skillRow.Value, [ref]$value)
        if ($value -lt 0 -and $skillRow.ModifierKind -ne "CustomSignedEquipSkillMod") {
            Add-Error "Negative skill modifier '$($skillRow.SkillModId)' is not marked CustomSignedEquipSkillMod."
        }
    }

    $csvExpectations = @{
        "mobilechanges.csv" = $mobileChanges.Count
        "newlootitems.csv" = $items.Count
        "lootassignments.csv" = $assignments.Count
        "mobileskillchanges.csv" = $mobileSkills.Count
        "itemskillmods.csv" = $itemSkills.Count
        "itembonuses.csv" = $itemBonuses.Count
    }

    foreach ($fileName in $csvExpectations.Keys) {
        $csvPath = Join-Path $OutputDirectory $fileName
        if (-not (Test-Path -LiteralPath $csvPath)) {
            Add-Error "Missing CSV output: $csvPath"
            continue
        }

        $csvRows = @(Import-Csv -LiteralPath $csvPath)
        if ($csvRows.Count -ne $csvExpectations[$fileName]) {
            Add-Error "CSV row count mismatch for $fileName. Workbook=$($csvExpectations[$fileName]) CSV=$($csvRows.Count)"
        }
    }

    $needsDropDecision = @($assignments | Where-Object { $_.DropSemanticsStatus -eq "NeedsDecision" }).Count
    if ($needsDropDecision -eq 0) {
        Add-Warning "No loot rows are flagged NeedsDecision. Confirm staff policy if this was intentional."
    }

    $ownerPolicyRows = @($items | Where-Object { $_.OwnerBoundPolicyStatus -eq "NeedsOwnerBindingPolicy" }).Count
    if ($ownerPolicyRows -eq 0) {
        Add-Warning "No owner-bound rows are flagged NeedsOwnerBindingPolicy. Confirm owner policy if this was intentional."
    }

    Write-Host "Workbook rows:"
    Write-Host "  MobileChanges: $($mobileChanges.Count)"
    Write-Host "  NewLootItems: $($items.Count)"
    Write-Host "  LootAssignments: $($assignments.Count)"
    Write-Host "  MobileSkillChanges: $($mobileSkills.Count)"
    Write-Host "  ItemSkillMods: $($itemSkills.Count)"
    Write-Host "  ItemBonuses: $($itemBonuses.Count)"
    Write-Host "Flagged policy rows:"
    Write-Host "  Drop semantics NeedsDecision: $needsDropDecision"
    Write-Host "  OwnerBound NeedsOwnerBindingPolicy: $ownerPolicyRows"
}

if ($script:Warnings.Count -gt 0) {
    Write-Host ""
    Write-Host "Warnings:"
    foreach ($warning in $script:Warnings) {
        Write-Host "  $warning"
    }
}

if ($script:Errors.Count -gt 0) {
    Write-Host ""
    Write-Host "Errors:"
    foreach ($errorMessage in $script:Errors) {
        Write-Host "  $errorMessage"
    }

    exit 1
}

Write-Host ""
Write-Host "PASS: mobile balance workbook validation succeeded."
