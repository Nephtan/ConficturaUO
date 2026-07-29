# Validates the canonical row-4 workbook and all normalized CSV exports.
param(
    [string]$WorkbookPath = "docs/mobile-balance-adjustments/workbooks/MobileBalanceTemplate.xlsx",
    [string]$OutputDirectory = "docs/mobile-balance-adjustments/outputs"
)

$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.IO.Compression.FileSystem
Add-Type -AssemblyName System.IO.Compression

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
    $fileStream = [System.IO.FileStream]::new(
        $resolvedPath,
        [System.IO.FileMode]::Open,
        [System.IO.FileAccess]::Read,
        [System.IO.FileShare]::ReadWrite -bor [System.IO.FileShare]::Delete
    )
    $memoryStream = [System.IO.MemoryStream]::new()
    try {
        $fileStream.CopyTo($memoryStream)
    }
    finally {
        $fileStream.Dispose()
    }
    $memoryStream.Position = 0
    $zip = [System.IO.Compression.ZipArchive]::new(
        $memoryStream,
        [System.IO.Compression.ZipArchiveMode]::Read,
        $false
    )
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
        $definedNames = [ordered]@{}
        foreach ($definedName in $workbookDoc.SelectNodes("//*[local-name()='definedName']")) {
            $definedNames[[string]$definedName.name] = [string]$definedName.InnerText
        }

        $result = [ordered]@{}
        $headersBySheet = [ordered]@{}
        $rowsBySheet = [ordered]@{}
        $validations = [ordered]@{}
        $validationRules = [ordered]@{}

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
            $headersBySheet[$sheetName] = @($headers.Values)
            $rowsBySheet[$sheetName] = $rows
            $rules = @()
            foreach ($validation in $sheetDoc.SelectNodes("//*[local-name()='dataValidation']")) {
                $formulaNode = $validation.SelectSingleNode("*[local-name()='formula1']")
                $rules += [pscustomobject]@{
                    SqRef = [string]$validation.sqref
                    Type = [string]$validation.type
                    Formula = if ($formulaNode -eq $null) { "" } else { [string]$formulaNode.InnerText }
                }
            }
            $validations[$sheetName] = $rules.Count
            $validationRules[$sheetName] = $rules
        }

        return [pscustomobject]@{
            Sheets = $result
            Headers = $headersBySheet
            Rows = $rowsBySheet
            ValidationCounts = $validations
            ValidationRules = $validationRules
            DefinedNames = $definedNames
        }
    }
    finally {
        $zip.Dispose()
        $memoryStream.Dispose()
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

function Get-CellValue {
    param(
        [object]$Workbook,
        [string]$SheetName,
        [int]$Row,
        [string]$Column
    )

    $rowObject = $Workbook.Rows[$SheetName] | Where-Object { $_.Row -eq $Row } | Select-Object -First 1
    if ($rowObject -eq $null -or -not $rowObject.Cells.Contains($Column)) {
        return ""
    }

    return [string]$rowObject.Cells[$Column]
}

function Get-Pairs {
    param([string]$Text)

    $pairs = @()
    foreach ($token in $Text.Split(";")) {
        $trimmed = $token.Trim()
        if ($trimmed -eq "" -or -not $trimmed.Contains("=")) {
            continue
        }

        $parts = $trimmed.Split("=", 2)
        $pairs += [pscustomobject]@{
            Name = $parts[0].Trim()
            Value = $parts[1].Trim()
        }
    }

    return $pairs
}

function Normalize-Key {
    param([string]$Value)

    return ([regex]::Replace($Value.ToLowerInvariant(), "[^a-z0-9]", ""))
}

function Test-Validation {
    param(
        [object]$Workbook,
        [string]$SheetName,
        [string]$SqRef,
        [string]$ExpectedFormula = ""
    )

    $rule = @($Workbook.ValidationRules[$SheetName] | Where-Object { @($_.SqRef.Split(" ")) -contains $SqRef })
    if ($rule.Count -eq 0) {
        Add-Error "Missing data validation on $SheetName!$SqRef."
        return
    }

    if ($ExpectedFormula -ne "" -and $rule[0].Formula -ne $ExpectedFormula) {
        Add-Error "Validation formula mismatch on $SheetName!$SqRef. Expected '$ExpectedFormula'; actual '$($rule[0].Formula)'."
    }
}

function Test-CsvParity {
    param(
        [object]$Workbook,
        [string]$SheetName,
        [string]$FileName,
        [string[]]$Headers
    )

    $csvPath = Join-Path $OutputDirectory $FileName
    if (-not (Test-Path -LiteralPath $csvPath)) {
        Add-Error "Missing CSV output: $csvPath"
        return
    }

    $workbookRows = @($Workbook.Sheets[$SheetName])
    $csvRows = @(Import-Csv -LiteralPath $csvPath)
    if ($workbookRows.Count -ne $csvRows.Count) {
        Add-Error "CSV row count mismatch for $FileName. Workbook=$($workbookRows.Count) CSV=$($csvRows.Count)"
        return
    }

    for ($index = 0; $index -lt $workbookRows.Count; $index++) {
        foreach ($header in $Headers) {
            $workbookValue = [string]$workbookRows[$index].$header
            $csvValue = [string]$csvRows[$index].$header
            if ($workbookValue -ne $csvValue) {
                Add-Error "CSV content mismatch for $FileName row $($index + 2) column $header. Workbook='$workbookValue' CSV='$csvValue'."
                return
            }
        }
    }
}

$script:Errors = @()
$script:Warnings = @()

$workbook = Read-XlsxWorkbook -Path $WorkbookPath
$sheets = $workbook.Sheets

$expectedHeaders = [ordered]@{
    "MobileChanges" = @("ChangeId", "MobileClassOrName", "ResolvedClassName", "KnownFilePath", "Scope", "DamageMin", "DamageMax", "SkillChangesRaw", "LootAssignmentIds", "Priority", "ReviewStatus", "ImplementationStatus", "Notes", "CodexNotes")
    "NewLootItems" = @("ItemId", "ClassNameRaw", "CanonicalClassName", "BaseItemRaw", "CanonicalBaseItem", "ExistingClassStatus", "DisplayName", "ItemIDGraphic", "Hue", "Layer", "LootType", "SkillBonusesRaw", "AttributesRaw", "OwnerBound", "OwnerBoundPolicyStatus", "PropertyLabel", "ImplementationStatus", "Notes", "CodexNotes")
    "LootAssignments" = @("AssignmentId", "MobileClassOrName", "ResolvedMobileClass", "ItemId", "CanonicalItemClass", "DropTiming", "ChancePercent", "QuantityMin", "QuantityMax", "Guaranteed", "DropRule", "DropSemanticsStatus", "Notes", "CodexNotes")
    "MobileSkillChanges" = @("SkillChangeId", "ChangeId", "MobileClassOrName", "ResolvedMobileClass", "SkillNameRaw", "CanonicalSkillName", "SkillMin", "SkillMax", "SkillValue", "Operation", "Status", "Notes")
    "ItemSkillMods" = @("SkillModId", "ItemId", "CanonicalClassName", "SkillNameRaw", "CanonicalSkillName", "Value", "ModifierKind", "ImplementationSurface", "Status", "Notes")
    "ItemBonuses" = @("BonusId", "ItemId", "CanonicalClassName", "SourceColumn", "BonusNameRaw", "CanonicalBonusName", "BonusGroup", "Value", "ImplementationSurface", "Status", "Notes")
    "Ref_Skills" = @("SkillId", "SkillName", "SourcePath", "Status", "Notes")
    "Ref_ItemClasses" = @("ClassName", "BaseClass", "SourcePath", "Kind", "Status", "Notes")
    "Ref_MobileClasses" = @("ClassName", "DisplayName", "SourcePath", "Status", "Notes")
    "Ref_BonusNames" = @("BonusName", "BonusGroup", "ImplementationSurface", "SupportsNegative", "Status", "Notes")
    "Ref_DropRules" = @("DropRule", "Meaning", "Status")
}

$expectedCounts = [ordered]@{
    "MobileChanges" = 35
    "NewLootItems" = 39
    "LootAssignments" = 39
    "MobileSkillChanges" = 64
    "ItemSkillMods" = 73
    "ItemBonuses" = 110
    "Ref_Skills" = 58
    "Ref_ItemClasses" = 6643
    "Ref_MobileClasses" = 1599
    "Ref_BonusNames" = 32
    "Ref_DropRules" = 5
}

foreach ($sheetName in $expectedHeaders.Keys) {
    if (-not $sheets.Contains($sheetName)) {
        Add-Error "Missing sheet: $sheetName"
        continue
    }

    $actualHeaders = @($workbook.Headers[$sheetName])
    if (($actualHeaders -join "|") -ne ($expectedHeaders[$sheetName] -join "|")) {
        Add-Error "Row-4 headers differ on $sheetName."
    }

    $actualCount = @($sheets[$sheetName]).Count
    if ($actualCount -ne $expectedCounts[$sheetName]) {
        Add-Error "$sheetName row count mismatch. Expected=$($expectedCounts[$sheetName]) Actual=$actualCount."
    }
}

if (-not $sheets.Contains("Reference")) {
    Add-Error "Missing sheet: Reference"
}
else {
    $referenceChecks = @(
        @("A", 1, "Mobile Balance Template Reference"),
        @("A", 4, "How to Fill"),
        @("D", 4, "Common Skill Aliases"),
        @("A", 14, "Valid SkillName Values"),
        @("D", 14, "Common Item Attribute Names")
    )
    foreach ($check in $referenceChecks) {
        $actual = Get-CellValue -Workbook $workbook -SheetName "Reference" -Row $check[1] -Column $check[0]
        if ($actual -ne $check[2]) {
            Add-Error "Reference!$($check[0])$($check[1]) mismatch. Expected '$($check[2])'; actual '$actual'."
        }
    }
}

foreach ($sheetName in @("MobileChanges", "NewLootItems", "LootAssignments", "MobileSkillChanges", "ItemSkillMods", "ItemBonuses")) {
    if ($workbook.ValidationCounts[$sheetName] -le 0) {
        Add-Error "Expected dropdown/data validation rules on $sheetName."
    }
}

$referenceValidationFormulas = @{
    "MobileChanges|B5:B1000" = "MobileDisplayList"
    "MobileChanges|C5:C1000" = "MobileClassList"
    "NewLootItems|C5:C1000" = "ItemClassList"
    "NewLootItems|E5:E1000" = "ItemClassList"
    "LootAssignments|C5:C1000" = "MobileClassList"
    "LootAssignments|E5:E1000" = "ItemClassList"
    "LootAssignments|K5:K1000" = "DropRuleList"
    "MobileSkillChanges|D5:D1000" = "MobileClassList"
    "MobileSkillChanges|F5:F1000" = "SkillNameList"
    "ItemSkillMods|C5:C1000" = "ItemClassList"
    "ItemSkillMods|E5:E1000" = "SkillNameList"
    "ItemBonuses|C5:C1000" = "ItemClassList"
    "ItemBonuses|F5:F1000" = "BonusNameList"
    "ItemBonuses|G5:G1000" = "BonusGroupList"
}
foreach ($key in $referenceValidationFormulas.Keys) {
    $parts = $key.Split("|", 2)
    Test-Validation -Workbook $workbook -SheetName $parts[0] -SqRef $parts[1] -ExpectedFormula $referenceValidationFormulas[$key]
}

$expectedDefinedNames = @{
    "SkillNameList" = "Ref_Skills!`$B`$5:`$B`$62"
    "ItemClassList" = "Ref_ItemClasses!`$A`$5:`$A`$6647"
    "MobileClassList" = "Ref_MobileClasses!`$A`$5:`$A`$1603"
    "MobileDisplayList" = "Ref_MobileClasses!`$B`$5:`$B`$1603"
    "BonusNameList" = "Ref_BonusNames!`$A`$5:`$A`$36"
    "BonusGroupList" = "Ref_BonusNames!`$B`$5:`$B`$36"
    "DropRuleList" = "Ref_DropRules!`$A`$5:`$A`$9"
}
foreach ($name in $expectedDefinedNames.Keys) {
    if (-not $workbook.DefinedNames.Contains($name)) {
        Add-Error "Missing workbook defined name: $name"
    }
    elseif ($workbook.DefinedNames[$name] -ne $expectedDefinedNames[$name]) {
        Add-Error "Defined name '$name' mismatch. Expected '$($expectedDefinedNames[$name])'; actual '$($workbook.DefinedNames[$name])'."
    }
}

foreach ($target in @(
    @("MobileChanges", "E5:E1000"), @("MobileChanges", "J5:J1000"), @("MobileChanges", "K5:K1000"), @("MobileChanges", "L5:L1000"),
    @("NewLootItems", "F5:F1000"), @("NewLootItems", "J5:J1000"), @("NewLootItems", "K5:K1000"), @("NewLootItems", "N5:N1000"), @("NewLootItems", "O5:O1000"), @("NewLootItems", "Q5:Q1000"),
    @("LootAssignments", "F5:F1000"), @("LootAssignments", "J5:J1000"), @("LootAssignments", "L5:L1000"),
    @("MobileSkillChanges", "J5:J1000"), @("MobileSkillChanges", "K5:K1000"),
    @("ItemSkillMods", "G5:G1000"), @("ItemSkillMods", "I5:I1000"),
    @("ItemBonuses", "J5:J1000")
)) {
    Test-Validation -Workbook $workbook -SheetName $target[0] -SqRef $target[1]
}

if ($script:Errors.Count -eq 0) {
    $items = @($sheets["NewLootItems"])
    $assignments = @($sheets["LootAssignments"])
    $mobileChanges = @($sheets["MobileChanges"])
    $mobileSkills = @($sheets["MobileSkillChanges"])
    $itemSkills = @($sheets["ItemSkillMods"])
    $itemBonuses = @($sheets["ItemBonuses"])
    $refSkills = @($sheets["Ref_Skills"])
    $refItems = @($sheets["Ref_ItemClasses"])
    $refBonuses = @($sheets["Ref_BonusNames"])

    $repoRoot = (Resolve-Path -LiteralPath ".").Path
    foreach ($mobileChange in $mobileChanges) {
        if ($mobileChange.KnownFilePath -eq "") {
            Add-Error "MobileChanges row '$($mobileChange.ChangeId)' has no KnownFilePath."
            continue
        }

        $sourcePath = Join-Path $repoRoot $mobileChange.KnownFilePath
        if (-not (Test-Path -LiteralPath $sourcePath -PathType Leaf)) {
            Add-Error "MobileChanges row '$($mobileChange.ChangeId)' source path does not exist: $($mobileChange.KnownFilePath)."
        }
    }

    $itemIds = @{}
    $itemsById = @{}
    foreach ($item in $items) {
        $itemIds[$item.ItemId] = $true
        $itemsById[$item.ItemId] = $item
    }

    $assignmentIds = @{}
    foreach ($assignment in $assignments) {
        $assignmentIds[$assignment.AssignmentId] = $true
    }

    foreach ($assignment in $assignments) {
        if (-not $itemIds.ContainsKey($assignment.ItemId)) {
            Add-Error "LootAssignments references missing ItemId '$($assignment.ItemId)' on assignment '$($assignment.AssignmentId)'."
            continue
        }

        if ($assignment.CanonicalItemClass -ne $itemsById[$assignment.ItemId].CanonicalClassName) {
            Add-Error "Loot assignment '$($assignment.AssignmentId)' canonical item class is out of sync."
        }

        $chance = 0.0
        [void][double]::TryParse($assignment.ChancePercent, [ref]$chance)
        if ($assignment.Guaranteed -eq "Yes" -and $chance -lt 100 -and $assignment.DropSemanticsStatus -ne "NeedsDecision") {
            Add-Error "Assignment '$($assignment.AssignmentId)' has Guaranteed=Yes and ChancePercent < 100 but is not flagged NeedsDecision."
        }
        if ($assignment.Guaranteed -eq "No" -and ($assignment.DropRule -ne "ChancePercentOnCorpse" -or $assignment.DropSemanticsStatus -ne "Ready")) {
            Add-Error "Assignment '$($assignment.AssignmentId)' does not encode the approved independent corpse-roll policy."
        }
    }

    if (@($assignments | Where-Object { $_.Guaranteed -ne "No" }).Count -ne 0) {
        Add-Error "All 39 approved loot assignments must have Guaranteed=No."
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

    $skillByKey = @{}
    foreach ($skill in $refSkills) {
        $skillByKey[(Normalize-Key $skill.SkillName)] = $skill.SkillName
    }
    $skillAliases = @{
        "magicresist" = "MagicResist"
        "parrying" = "Parry"
        "parry" = "Parry"
        "blacksmithing" = "Blacksmith"
        "blacksmithy" = "Blacksmith"
        "merchantile" = "Mercantile"
        "mercantile" = "Mercantile"
        "aantomy" = "Anatomy"
    }
    $expectedMobileSkills = @{}
    foreach ($mobileChange in $mobileChanges) {
        foreach ($pair in Get-Pairs $mobileChange.SkillChangesRaw) {
            $key = Normalize-Key $pair.Name
            $canonical = if ($skillAliases.ContainsKey($key)) { $skillAliases[$key] } elseif ($skillByKey.ContainsKey($key)) { $skillByKey[$key] } else { $pair.Name }
            $expectedMobileSkills["$($mobileChange.ChangeId)|$canonical|$($pair.Value)"] = $true
        }
    }
    if ($expectedMobileSkills.Count -ne $mobileSkills.Count) {
        Add-Error "Raw mobile skill count does not match MobileSkillChanges. Raw=$($expectedMobileSkills.Count) Child=$($mobileSkills.Count)."
    }

    foreach ($skillRow in $mobileSkills + $itemSkills) {
        if ($skillRow.CanonicalSkillName -ne "" -and -not $skillNames.ContainsKey($skillRow.CanonicalSkillName)) {
            Add-Error "Unknown canonical skill '$($skillRow.CanonicalSkillName)' in normalized skill rows."
        }
    }

    foreach ($skillRow in $mobileSkills) {
        $value = if ($skillRow.SkillValue -ne "") { $skillRow.SkillValue } else { "$($skillRow.SkillMin)-$($skillRow.SkillMax)" }
        if (-not $expectedMobileSkills.ContainsKey("$($skillRow.ChangeId)|$($skillRow.CanonicalSkillName)|$value")) {
            Add-Error "MobileSkillChanges row '$($skillRow.SkillChangeId)' does not match raw mobile skill data."
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
        if ($item.OwnerBound -eq "No" -and $item.OwnerBoundPolicyStatus -ne "NotOwnerBound") {
            Add-Error "Item '$($item.ItemId)' is not owner-bound but has policy status '$($item.OwnerBoundPolicyStatus)'."
        }
    }

    if (@($items | Where-Object { $_.OwnerBound -ne "No" }).Count -ne 0) {
        Add-Error "All 39 approved loot items must have OwnerBound=No."
    }

    foreach ($skillRow in $itemSkills) {
        if (-not $itemsById.ContainsKey($skillRow.ItemId)) {
            Add-Error "Item skill '$($skillRow.SkillModId)' references missing ItemId '$($skillRow.ItemId)'."
            continue
        }
        if ($skillRow.CanonicalClassName -ne $itemsById[$skillRow.ItemId].CanonicalClassName) {
            Add-Error "Item skill '$($skillRow.SkillModId)' canonical class is out of sync."
        }

        $value = 0.0
        [void][double]::TryParse($skillRow.Value, [ref]$value)
        if ($value -lt 0 -and ($skillRow.ModifierKind -ne "CustomSignedEquipSkillMod" -or $skillRow.ImplementationSurface -ne "custom equip/unequip SkillMod")) {
            Add-Error "Negative skill modifier '$($skillRow.SkillModId)' is not marked CustomSignedEquipSkillMod."
        }
    }

    foreach ($bonusRow in $itemBonuses) {
        if (-not $itemsById.ContainsKey($bonusRow.ItemId)) {
            Add-Error "Item bonus '$($bonusRow.BonusId)' references missing ItemId '$($bonusRow.ItemId)'."
            continue
        }
        if ($bonusRow.CanonicalClassName -ne $itemsById[$bonusRow.ItemId].CanonicalClassName) {
            Add-Error "Item bonus '$($bonusRow.BonusId)' canonical class is out of sync."
        }
        if ($bonusRow.BonusGroup -eq "" -or $bonusRow.ImplementationSurface -eq "") {
            Add-Error "Item bonus '$($bonusRow.BonusId)' lacks an implementation group or surface."
        }
    }

    $expectedItemSkillCount = 0
    $expectedItemBonusCount = 0
    foreach ($item in $items) {
        foreach ($pair in Get-Pairs $item.SkillBonusesRaw) {
            $key = Normalize-Key $pair.Name
            if ($skillAliases.ContainsKey($key) -or $skillByKey.ContainsKey($key)) {
                $expectedItemSkillCount++
            }
            else {
                $expectedItemBonusCount++
            }
        }
        $expectedItemBonusCount += @(Get-Pairs $item.AttributesRaw).Count
    }
    if ($expectedItemSkillCount -ne $itemSkills.Count) {
        Add-Error "Raw item skill count does not match ItemSkillMods. Raw=$expectedItemSkillCount Child=$($itemSkills.Count)."
    }
    if ($expectedItemBonusCount -ne $itemBonuses.Count) {
        Add-Error "Raw item bonus count does not match ItemBonuses. Raw=$expectedItemBonusCount Child=$($itemBonuses.Count)."
    }

    $negativeSkillRows = @($itemSkills | Where-Object { [double]$_.Value -lt 0 })
    $negativeSkillItems = @($negativeSkillRows | Select-Object -ExpandProperty ItemId -Unique)
    if ($negativeSkillRows.Count -ne 36 -or $negativeSkillItems.Count -ne 19) {
        Add-Error "Expected 36 negative skill modifiers across 19 items. Rows=$($negativeSkillRows.Count) Items=$($negativeSkillItems.Count)."
    }

    $dreadItem = @($items | Where-Object { $_.ItemId -eq "ITEM-014" })
    if ($dreadItem.Count -ne 1 -or $dreadItem[0].CanonicalClassName -ne "DreadMace" -or $dreadItem[0].DisplayName -ne "The Dread Mace") {
        Add-Error "ITEM-014 is not consistently defined as DreadMace / The Dread Mace."
    }
    if (@($itemSkills | Where-Object { $_.ItemId -eq "ITEM-014" -and $_.CanonicalSkillName -eq "Knightship" -and $_.Value -eq "-10" }).Count -ne 1) {
        Add-Error "DreadMace is missing the Knightship=-10 signed skill modifier."
    }
    if (@($refItems | Where-Object { $_.ClassName -eq "DreadMace" -and $_.Kind -eq "ProposedItemClass" -and $_.Status -eq "Proposed" }).Count -ne 1) {
        Add-Error "Ref_ItemClasses does not contain DreadMace as one proposed class."
    }
    if (@($itemSkills | Where-Object { $_.CanonicalClassName -eq "Pestilence" }).Count -ne 0 -or @($itemBonuses | Where-Object { $_.CanonicalClassName -eq "Pestilence" }).Count -ne 0 -or @($assignments | Where-Object { $_.CanonicalItemClass -eq "Pestilence" }).Count -ne 0) {
        Add-Error "Stale Pestilence references remain in normalized sheets."
    }

    $weight = @($itemBonuses | Where-Object { $_.ItemId -eq "ITEM-010" -and $_.CanonicalBonusName -eq "Weight" })
    if ($weight.Count -ne 1 -or $weight[0].Value -ne "500") {
        Add-Error "Weightoftheworld must normalize Weight=500."
    }

    foreach ($changeId in @("MC-034", "MC-035")) {
        $row = @($mobileChanges | Where-Object { $_.ChangeId -eq $changeId })
        if ($row.Count -ne 1 -or $row[0].Priority -ne "" -or $row[0].ReviewStatus -ne "Ready" -or $row[0].ImplementationStatus -ne "Pending") {
            Add-Error "$changeId must remain priority-unresolved with Ready/Pending statuses."
        }
    }

    $csvFiles = @{
        "MobileChanges" = "mobilechanges.csv"
        "NewLootItems" = "newlootitems.csv"
        "LootAssignments" = "lootassignments.csv"
        "MobileSkillChanges" = "mobileskillchanges.csv"
        "ItemSkillMods" = "itemskillmods.csv"
        "ItemBonuses" = "itembonuses.csv"
        "Ref_Skills" = "ref-skills.csv"
        "Ref_ItemClasses" = "ref-itemclasses.csv"
        "Ref_MobileClasses" = "ref-mobileclasses.csv"
        "Ref_BonusNames" = "ref-bonusnames.csv"
        "Ref_DropRules" = "ref-droprules.csv"
    }
    foreach ($sheetName in $csvFiles.Keys) {
        Test-CsvParity -Workbook $workbook -SheetName $sheetName -FileName $csvFiles[$sheetName] -Headers $expectedHeaders[$sheetName]
    }

    $issuesPath = Join-Path $OutputDirectory "review-issues.csv"
    if (-not (Test-Path -LiteralPath $issuesPath)) {
        Add-Error "Missing CSV output: $issuesPath"
    }
    else {
        $issues = @(Import-Csv -LiteralPath $issuesPath)
        if ($issues.Count -ne 3) {
            Add-Error "review-issues.csv must contain one signed-skill constraint and two priority decisions. Actual=$($issues.Count)."
        }
        if (@($issues | Where-Object { $_.Status -eq "ImplementationConstraint" }).Count -ne 1) {
            Add-Error "review-issues.csv is missing the signed-skill implementation constraint."
        }
        foreach ($changeId in @("MC-034", "MC-035")) {
            if (@($issues | Where-Object { $_.RowKey -eq $changeId -and $_.Status -eq "NeedsDecision" }).Count -ne 1) {
                Add-Error "review-issues.csv is missing the priority decision for $changeId."
            }
        }
    }

    Write-Host "Workbook rows:"
    Write-Host "  MobileChanges: $($mobileChanges.Count)"
    Write-Host "  NewLootItems: $($items.Count)"
    Write-Host "  LootAssignments: $($assignments.Count)"
    Write-Host "  MobileSkillChanges: $($mobileSkills.Count)"
    Write-Host "  ItemSkillMods: $($itemSkills.Count)"
    Write-Host "  ItemBonuses: $($itemBonuses.Count)"
    Write-Host "Resolved policy:"
    Write-Host "  Loot assignments: 39 independent corpse percentage rolls"
    Write-Host "  Owner binding: 39 items not owner-bound"
    Write-Host "Open decisions:"
    Write-Host "  Signed skills: 36 negative modifiers across 19 items"
    Write-Host "  Priorities: MC-034 and MC-035"
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
