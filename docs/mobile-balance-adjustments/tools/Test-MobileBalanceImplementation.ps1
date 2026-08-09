# Compares normalized mobile-balance CSV data with the implemented C# source.
param(
    [string]$OutputDirectory = "docs/mobile-balance-adjustments/outputs",
    [string]$CatalogPath = "Data/Scripts/Custom/PvE/MobileBalance/MobileBalanceCatalog.cs",
    [string]$SignedControllerPath = "Data/Scripts/Custom/PvE/MobileBalance/SignedEquipSkillModController.cs"
)

$ErrorActionPreference = "Stop"
$script:Errors = @()

function Add-Error {
    param([string]$Message)

    $script:Errors += $Message
}

function Read-RequiredCsv {
    param([string]$Name)

    $path = Join-Path $OutputDirectory $Name
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Missing normalized CSV: $path"
    }

    return @(Import-Csv -LiteralPath $path)
}

function ConvertTo-Number {
    param([string]$Value)

    $number = 0.0
    if (-not [double]::TryParse(
        $Value,
        [System.Globalization.NumberStyles]::Float,
        [System.Globalization.CultureInfo]::InvariantCulture,
        [ref]$number
    )) {
        throw "Not a number: '$Value'"
    }

    return $number
}

function Get-ClassBody {
    param(
        [string]$Source,
        [string]$ClassName,
        [string]$BaseClass
    )

    $pattern = "(?ms)^\s{4}public class " + [regex]::Escape($ClassName) +
        "\s*:\s*" + [regex]::Escape($BaseClass) +
        "\s*\{(?<Body>.*?)(?=^\s{4}public class |^\})"
    $match = [regex]::Match($Source, $pattern)

    if (-not $match.Success) {
        Add-Error "Missing item class declaration: $ClassName : $BaseClass."
        return ""
    }

    return $match.Groups["Body"].Value
}

$mobileChanges = Read-RequiredCsv "mobilechanges.csv"
$mobileSkills = Read-RequiredCsv "mobileskillchanges.csv"
$items = Read-RequiredCsv "newlootitems.csv"
$assignments = Read-RequiredCsv "lootassignments.csv"
$itemSkills = Read-RequiredCsv "itemskillmods.csv"
$itemBonuses = Read-RequiredCsv "itembonuses.csv"

if ($mobileChanges.Count -ne 35) { Add-Error "Expected 35 mobile profiles; found $($mobileChanges.Count)." }
if ($mobileSkills.Count -ne 64) { Add-Error "Expected 64 mobile skill rows; found $($mobileSkills.Count)." }
if ($items.Count -ne 39) { Add-Error "Expected 39 item rows; found $($items.Count)." }
if ($assignments.Count -ne 39) { Add-Error "Expected 39 loot rows; found $($assignments.Count)." }
if ($itemSkills.Count -ne 73) { Add-Error "Expected 73 item skill rows; found $($itemSkills.Count)." }
if ($itemBonuses.Count -ne 110) { Add-Error "Expected 110 item bonus rows; found $($itemBonuses.Count)." }

$catalog = Get-Content -LiteralPath $CatalogPath -Raw
$signedController = Get-Content -LiteralPath $SignedControllerPath -Raw
$mobileBalanceSource = (
    Get-ChildItem -LiteralPath (Split-Path -Parent $CatalogPath) -File -Filter "*.cs" |
        ForEach-Object { Get-Content -LiteralPath $_.FullName -Raw }
) -join "`n"
if (
    $mobileBalanceSource -match
        "public\s+static\s+\w+\s+(?:Configure|Initialize)\s*\((?!\s*\))"
) {
    Add-Error "Mobile balance source declares a parameterized public Configure/Initialize runtime hook."
}
$profileSection = [regex]::Match(
    $catalog,
    "(?ms)public static void ApplyProfile\(BaseCreature creature\)(?<Body>.*?)public static void DropLoot"
).Groups["Body"].Value
$dropSection = [regex]::Match(
    $catalog,
    "(?ms)public static void DropLoot\(BaseCreature creature, Container corpse\)(?<Body>.*?)private static void Apply"
).Groups["Body"].Value
$factorySection = [regex]::Match(
    $catalog,
    "(?ms)internal static class MobileBalanceItemFactory(?<Body>.*)"
).Groups["Body"].Value

foreach ($mobile in $mobileChanges) {
    $className = $mobile.ResolvedClassName
    $profilePattern = "type == typeof\(" + [regex]::Escape($className) +
        "\)\)\s*\{\s*Apply\(creature,\s*(?<Min>-?\d+),\s*(?<Max>-?\d+),\s*new MobileBalanceSkill\[\]\s*\{(?<Skills>.*?)\}\s*\);"
    $profile = [regex]::Match($profileSection, $profilePattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)

    if (-not $profile.Success) {
        Add-Error "Missing catalog profile for $className."
        continue
    }

    if ($profile.Groups["Min"].Value -ne $mobile.DamageMin -or $profile.Groups["Max"].Value -ne $mobile.DamageMax) {
        Add-Error "$className damage differs from CSV. Source=$($profile.Groups['Min'].Value)-$($profile.Groups['Max'].Value) CSV=$($mobile.DamageMin)-$($mobile.DamageMax)."
    }

    $sourceSkills = @([regex]::Matches(
        $profile.Groups["Skills"].Value,
        "new MobileBalanceSkill\(SkillName\.(?<Name>\w+),\s*(?<Value>-?[\d.]+)\)"
    ))
    $csvSkills = @($mobileSkills | Where-Object { $_.ChangeId -eq $mobile.ChangeId })

    if ($sourceSkills.Count -ne $csvSkills.Count) {
        Add-Error "$className skill count differs. Source=$($sourceSkills.Count) CSV=$($csvSkills.Count)."
    }
    else {
        for ($i = 0; $i -lt $csvSkills.Count; ++$i) {
            $csvValue = if ($csvSkills[$i].SkillValue) { $csvSkills[$i].SkillValue } else { $csvSkills[$i].SkillMin }
            if (
                $sourceSkills[$i].Groups["Name"].Value -ne $csvSkills[$i].CanonicalSkillName -or
                (ConvertTo-Number $sourceSkills[$i].Groups["Value"].Value) -ne (ConvertTo-Number $csvValue)
            ) {
                Add-Error "$className skill row $i differs from CSV."
            }
        }
    }

    $mobilePath = $mobile.KnownFilePath -replace "/", "\"
    if (-not (Test-Path -LiteralPath $mobilePath)) {
        Add-Error "Missing target mobile source: $mobilePath."
        continue
    }

    $mobileSource = Get-Content -LiteralPath $mobilePath -Raw
    if (([regex]::Matches($mobileSource, "MobileBalanceCatalog\.ApplyProfile\(this\)")).Count -ne 2) {
        Add-Error "$className must apply the profile in its constructor and version-0 migration."
    }
    if ($mobileSource -notmatch "writer\.Write\((?:\(int\))?1\)") {
        Add-Error "$className serializer is not version 1."
    }
    if ($mobileSource -notmatch "if \(version < 1\)") {
        Add-Error "$className lacks the version-0 one-time profile migration."
    }

    $expectedLootRows = @($assignments | Where-Object { $_.ResolvedMobileClass -eq $className })
    $sourceHookCount = ([regex]::Matches($mobileSource, "MobileBalanceCatalog\.DropLoot\(this, c\)")).Count
    $expectedHookCount = if ($expectedLootRows.Count -gt 0) { 1 } else { 0 }
    if ($sourceHookCount -ne $expectedHookCount) {
        Add-Error "$className loot hook count differs. Source=$sourceHookCount Expected=$expectedHookCount."
    }

    $dropPattern = "type == typeof\(" + [regex]::Escape($className) + "\)\)\s*\{(?<Body>.*?)\n\s*\}"
    $dropBranch = [regex]::Match($dropSection, $dropPattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)

    if ($expectedLootRows.Count -eq 0) {
        if ($dropBranch.Success) {
            Add-Error "$className must not appear in the loot catalog."
        }
        continue
    }

    if (-not $dropBranch.Success) {
        Add-Error "Missing loot catalog branch for $className."
        continue
    }

    $sourceDrops = @([regex]::Matches(
        $dropBranch.Groups["Body"].Value,
        'TryDrop\(corpse,\s*"(?<ItemId>ITEM-\d{3})",\s*(?<Chance>[\d.]+),\s*(?<Min>\d+),\s*(?<Max>\d+)\)'
    ))
    if ($sourceDrops.Count -ne $expectedLootRows.Count) {
        Add-Error "$className drop count differs. Source=$($sourceDrops.Count) CSV=$($expectedLootRows.Count)."
    }
    else {
        for ($i = 0; $i -lt $expectedLootRows.Count; ++$i) {
            $drop = $sourceDrops[$i]
            $row = $expectedLootRows[$i]
            if (
                $drop.Groups["ItemId"].Value -ne $row.ItemId -or
                (ConvertTo-Number $drop.Groups["Chance"].Value) -ne (ConvertTo-Number $row.ChancePercent) -or
                $drop.Groups["Min"].Value -ne $row.QuantityMin -or
                $drop.Groups["Max"].Value -ne $row.QuantityMax
            ) {
                Add-Error "$className drop row $i differs from CSV."
            }
        }
    }
}

if ($catalog -notmatch "(?s)Utility\.RandomDouble\(\).*?MobileBalanceItemFactory\.Create") {
    Add-Error "The item factory must be called only after a successful Utility.RandomDouble roll."
}
if ($catalog -notmatch 'TryDrop\(corpse,\s*"ITEM-032",\s*50,\s*1,\s*3\)') {
    Add-Error "PhoenixFeather does not use the approved 1-3 quantity range."
}

$newItemIds = @(
    "ITEM-001", "ITEM-002", "ITEM-003", "ITEM-004", "ITEM-005", "ITEM-006",
    "ITEM-007", "ITEM-008", "ITEM-009", "ITEM-010", "ITEM-011", "ITEM-012",
    "ITEM-013", "ITEM-014", "ITEM-015", "ITEM-016", "ITEM-017", "ITEM-018",
    "ITEM-019", "ITEM-020", "ITEM-021", "ITEM-022", "ITEM-036"
)
$itemSourcePaths = @(
    "Data/Scripts/Custom/PvE/MobileBalance/MobileBalanceItems.Armor.cs",
    "Data/Scripts/Custom/PvE/MobileBalance/MobileBalanceItems.TalismansAndTrophies.cs",
    "Data/Scripts/Custom/PvE/MobileBalance/MobileBalanceItems.Weapons.cs"
)
$itemSource = ($itemSourcePaths | ForEach-Object { Get-Content -LiteralPath $_ -Raw }) -join "`n"
$layerMap = @{
    "None" = "Invalid"
    "Talisman" = "Talisman"
    "OneHanded" = "OneHanded"
    "TwoHanded" = "TwoHanded"
    "Helm" = "Helm"
    "InnerTorso" = "InnerTorso"
    "OuterTorso" = "OuterTorso"
    "Gloves" = "Gloves"
}

foreach ($item in $items) {
    $casePattern = 'case "' + [regex]::Escape($item.ItemId) +
        '":(?<Body>.*?)break;'
    $case = [regex]::Match($factorySection, $casePattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)
    if (-not $case.Success) {
        Add-Error "Missing factory case for $($item.ItemId)."
        continue
    }

    $classMatch = [regex]::Match($case.Groups["Body"].Value, "item = new (?<Class>\w+)\(")
    if (-not $classMatch.Success -or $classMatch.Groups["Class"].Value -ne $item.CanonicalClassName) {
        Add-Error "$($item.ItemId) factory class differs from CSV."
    }

    $configure = [regex]::Match(
        $case.Groups["Body"].Value,
        'Configure\(item,\s*"(?<Name>[^"]*)",\s*(?<Graphic>0x[0-9A-Fa-f]+|\d+),\s*(?<Hue>-?\d+),\s*Layer\.(?<Layer>\w+),\s*(?<Label>null|"[^"]*"),\s*amount\)'
    )
    if (-not $configure.Success) {
        Add-Error "$($item.ItemId) lacks exact factory configuration."
        continue
    }

    $expectedGraphic = [Convert]::ToInt32(($item.ItemIDGraphic -replace "^0x", ""), 16)
    $actualGraphic = if ($configure.Groups["Graphic"].Value -like "0x*") {
        [Convert]::ToInt32($configure.Groups["Graphic"].Value.Substring(2), 16)
    }
    else {
        [int]$configure.Groups["Graphic"].Value
    }
    $actualLabel = $configure.Groups["Label"].Value
    if ($actualLabel -eq "null") { $actualLabel = "" } else { $actualLabel = $actualLabel.Trim('"') }

    if ($configure.Groups["Name"].Value -ne $item.DisplayName) {
        Add-Error "$($item.ItemId) display name differs from CSV."
    }
    if ($actualGraphic -ne $expectedGraphic -or $configure.Groups["Hue"].Value -ne $item.Hue) {
        Add-Error "$($item.ItemId) art or hue differs from CSV."
    }
    if ($configure.Groups["Layer"].Value -ne $layerMap[$item.Layer]) {
        Add-Error "$($item.ItemId) layer differs from CSV."
    }
    if ($actualLabel -ne $item.PropertyLabel) {
        Add-Error "$($item.ItemId) property label differs from CSV."
    }
    if ($item.LootType -ne "Regular") {
        Add-Error "$($item.ItemId) normalized loot type must be Regular after implementation."
    }

    if ($newItemIds -contains $item.ItemId) {
        $body = Get-ClassBody $itemSource $item.CanonicalClassName $item.CanonicalBaseItem
        if ($body -notmatch "\[Constructable\]") {
            Add-Error "$($item.CanonicalClassName) is not constructable."
        }
        if ($body -notmatch "writer\.Write\(\(int\)0\)") {
            Add-Error "$($item.CanonicalClassName) does not use version-0 serialization."
        }
    }
}

if ($newItemIds.Count -ne 23) {
    Add-Error "Internal validator error: expected 23 new item IDs."
}
if ($catalog -notmatch "item\.LootType = LootType\.Regular" -and $signedController -notmatch "item\.LootType = LootType\.Regular") {
    Add-Error "Shared item initialization does not force LootType.Regular."
}

$customSkillRows = @($itemSkills | Where-Object { $_.ModifierKind -eq "CustomSignedEquipSkillMod" })
$stockSkillRows = @($itemSkills | Where-Object { $_.ModifierKind -eq "StockPositiveSkillBonus" })
if ($customSkillRows.Count -ne 70) { Add-Error "Expected 70 custom signed skill rows; found $($customSkillRows.Count)." }
if ($stockSkillRows.Count -ne 3) { Add-Error "Expected 3 stock positive skill rows; found $($stockSkillRows.Count)." }

foreach ($group in @($customSkillRows | Group-Object CanonicalClassName)) {
    $pattern = "item is (?:Server\.)?Custom\.Confictura\.Items\." + [regex]::Escape($group.Name) +
        "\)\s*\{\s*return new SignedSkillModDefinition\[\]\s*\{(?<Rows>.*?)\};"
    $block = [regex]::Match($signedController, $pattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)
    if (-not $block.Success) {
        Add-Error "Missing signed skill definition block for $($group.Name)."
        continue
    }

    $sourceRows = @([regex]::Matches(
        $block.Groups["Rows"].Value,
        "new SignedSkillModDefinition\(SkillName\.(?<Name>\w+),\s*(?<Value>-?[\d.]+)\)"
    ))
    if ($sourceRows.Count -ne $group.Count) {
        Add-Error "$($group.Name) signed skill count differs. Source=$($sourceRows.Count) CSV=$($group.Count)."
    }
    else {
        for ($i = 0; $i -lt $group.Count; ++$i) {
            if (
                $sourceRows[$i].Groups["Name"].Value -ne $group.Group[$i].CanonicalSkillName -or
                (ConvertTo-Number $sourceRows[$i].Groups["Value"].Value) -ne (ConvertTo-Number $group.Group[$i].Value)
            ) {
                Add-Error "$($group.Name) signed skill row $i differs from CSV."
            }
        }
    }

    $item = @($items | Where-Object { $_.CanonicalClassName -eq $group.Name })[0]
    $body = Get-ClassBody $itemSource $item.CanonicalClassName $item.CanonicalBaseItem
    foreach ($requiredCall in @(
        "SignedEquipSkillModController.Apply",
        "SignedEquipSkillModController.Remove",
        "SignedEquipSkillModController.Rehydrate",
        "SignedEquipSkillModController.AddProperties"
    )) {
        if ($body -notmatch [regex]::Escape($requiredCall)) {
            Add-Error "$($group.Name) lacks lifecycle call $requiredCall."
        }
    }
}

foreach ($group in @($stockSkillRows | Group-Object CanonicalClassName)) {
    $item = @($items | Where-Object { $_.CanonicalClassName -eq $group.Name })[0]
    $body = Get-ClassBody $itemSource $item.CanonicalClassName $item.CanonicalBaseItem
    foreach ($row in $group.Group) {
        $pattern = "SkillBonuses\.SetValues\(\d+,\s*SkillName\." +
            [regex]::Escape($row.CanonicalSkillName) + ",\s*" +
            [regex]::Escape($row.Value) + "(?:\.0)?\)"
        if ($body -notmatch $pattern) {
            Add-Error "$($group.Name) stock skill $($row.CanonicalSkillName)=$($row.Value) is missing."
        }
    }
}

foreach ($bonus in $itemBonuses) {
    $item = @($items | Where-Object { $_.ItemId -eq $bonus.ItemId })[0]
    $body = Get-ClassBody $itemSource $item.CanonicalClassName $item.CanonicalBaseItem
    $surface = switch ($bonus.BonusGroup) {
        "AosAttributes" { "Attributes.$($bonus.CanonicalBonusName)" }
        "AosWeaponAttributes" { "WeaponAttributes.$($bonus.CanonicalBonusName)" }
        "AosArmorAttributes" { "ArmorAttributes.$($bonus.CanonicalBonusName)" }
        default { $bonus.CanonicalBonusName }
    }
    $pattern = [regex]::Escape($surface) + "\s*=\s*" +
        [regex]::Escape($bonus.Value) + "(?:\.0)?\s*;"
    if ($body -notmatch $pattern) {
        Add-Error "$($item.CanonicalClassName) bonus $surface=$($bonus.Value) is missing."
    }
}

if ($signedController -notmatch "mod\.ObeyCap = false") {
    Add-Error "Signed skill mods must set ObeyCap=false."
}
if ($signedController -notmatch "Remove\(item\);\s*[\r\n]+\s*Mobile mobile") {
    Add-Error "Signed skill application is not idempotent."
}
if ($signedController -notmatch "new RehydrateTimer\(item\)\.Start\(\)") {
    Add-Error "Signed skill mods do not rehydrate after deserialization."
}
if ($itemSource -notmatch "ClearRandomTalismanProperties\(this\)") {
    Add-Error "New talismans do not clear inherited randomized properties."
}

$namingSource = Get-Content -LiteralPath "Data/Scripts/System/Misc/Naming.cs" -Raw
if ($namingSource -notmatch 'Name == "Census Records" \|\| Name == "Legendary Registry of Heroes"') {
    Add-Error "Legendary Registry of Heroes is not recognized as a legal CensusRecords interface."
}

$projectSource = Get-Content -LiteralPath "Data/Scripts/Scripts.csproj" -Raw
foreach ($path in @(
    "Custom\PvE\MobileBalance\MobileBalanceCatalog.cs",
    "Custom\PvE\MobileBalance\MobileBalanceItems.Armor.cs",
    "Custom\PvE\MobileBalance\MobileBalanceItems.TalismansAndTrophies.cs",
    "Custom\PvE\MobileBalance\MobileBalanceItems.Weapons.cs",
    "Custom\PvE\MobileBalance\SignedEquipSkillModController.cs"
)) {
    if ($projectSource -notmatch [regex]::Escape($path)) {
        Add-Error "Scripts.csproj is missing $path."
    }
}

Write-Host "Implementation parity:"
Write-Host "  Mobile profiles: $($mobileChanges.Count)"
Write-Host "  Mobile skills: $($mobileSkills.Count)"
Write-Host "  Loot assignments: $($assignments.Count)"
Write-Host "  New item classes: $($newItemIds.Count)"
Write-Host "  Custom signed skill mods: $($customSkillRows.Count)"
Write-Host "  Stock positive skill mods: $($stockSkillRows.Count)"
Write-Host "  Item bonuses: $($itemBonuses.Count)"

if ($script:Errors.Count -gt 0) {
    Write-Host ""
    Write-Host "Errors:"
    foreach ($message in $script:Errors) {
        Write-Host "  $message"
    }

    exit 1
}

Write-Host ""
Write-Host "PASS: mobile balance implementation matches normalized data."
