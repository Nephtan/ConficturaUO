param(
    [string]$RepoRoot,
    [string]$OutputDir
)

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($RepoRoot))
{
    $RepoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..\..'))
}
else
{
    $RepoRoot = [System.IO.Path]::GetFullPath($RepoRoot)
}

if ([string]::IsNullOrWhiteSpace($OutputDir))
{
    $OutputDir = Join-Path $RepoRoot 'docs\codebase-audit\outputs'
}

$RepoRoot = $RepoRoot.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar)
$OutputDir = [System.IO.Path]::GetFullPath($OutputDir)

function Export-AuditCsv
{
    param(
        [object[]]$Rows,
        [string]$FileName
    )

    $path = Join-Path $OutputDir $FileName
    $Rows | Export-Csv -LiteralPath $path -NoTypeInformation -Encoding UTF8
    return $path
}

function Convert-ToRepoPath
{
    param([string]$Path)

    $fullPath = [System.IO.Path]::GetFullPath($Path)

    if ($fullPath.StartsWith($RepoRoot, [System.StringComparison]::OrdinalIgnoreCase))
    {
        return ($fullPath.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/')
    }

    return ($Path -replace '\\', '/')
}

function Escape-MarkdownCell
{
    param([string]$Value)

    if ($null -eq $Value)
    {
        return ''
    }

    return (($Value -replace '\|', '\|') -replace "`r?`n", ' ')
}

function Join-Unique
{
    param([object[]]$Values)

    return (($Values | Where-Object { -not [string]::IsNullOrWhiteSpace([string]$_) } | Sort-Object -Unique) -join ';')
}

function Trim-Text
{
    param(
        [string]$Value,
        [int]$MaxLength = 160
    )

    if ($null -eq $Value)
    {
        return ''
    }

    $collapsed = (($Value -replace '\s+', ' ') -replace "`r?`n", ' ').Trim()

    if ($collapsed.Length -le $MaxLength)
    {
        return $collapsed
    }

    return $collapsed.Substring(0, $MaxLength - 3) + '...'
}

function Remove-CodeNoise
{
    param([string]$Line)

    if ($null -eq $Line)
    {
        return ''
    }

    $withoutStrings = $Line -replace '@"[^"]*"', '""'
    $withoutStrings = $withoutStrings -replace '"(?:\\.|[^"\\])*"', '""'
    return ($withoutStrings -replace '//.*$', '')
}

function Get-ScopeEnd
{
    param(
        [string[]]$Lines,
        [int]$StartIndex
    )

    $depth = 0
    $seenOpeningBrace = $false

    for ($i = $StartIndex; $i -lt $Lines.Count; $i++)
    {
        $line = Remove-CodeNoise $Lines[$i]

        foreach ($ch in $line.ToCharArray())
        {
            if ($ch -eq '{')
            {
                $depth++
                $seenOpeningBrace = $true
            }
            elseif ($ch -eq '}')
            {
                $depth--

                if ($seenOpeningBrace -and $depth -le 0)
                {
                    return $i
                }
            }
        }
    }

    return ($Lines.Count - 1)
}

function Find-BraceStart
{
    param(
        [string[]]$Lines,
        [int]$StartIndex,
        [int]$EndIndex
    )

    for ($i = $StartIndex; $i -le $EndIndex -and $i -lt $Lines.Count; $i++)
    {
        if ((Remove-CodeNoise $Lines[$i]).Contains('{'))
        {
            return $i
        }
    }

    return -1
}

function Get-Namespace
{
    param([string[]]$Lines)

    foreach ($line in $Lines)
    {
        if ($line -match '^\s*namespace\s+([A-Za-z_][A-Za-z0-9_.]*)')
        {
            return $Matches[1]
        }
    }

    return ''
}

function Get-ClassScopes
{
    param([string[]]$Lines)

    $scopes = New-Object System.Collections.Generic.List[object]
    $classPattern = '^\s*(?:\[[^\]]+\]\s*)*(?:(?:public|private|protected|internal|abstract|sealed|partial|static|unsafe|new)\s+)*(?:class|struct)\s+([A-Za-z_][A-Za-z0-9_]*)(?:\s*:\s*([^{\r\n]+))?'

    for ($i = 0; $i -lt $Lines.Count; $i++)
    {
        $line = $Lines[$i]

        if ($line -match $classPattern)
        {
            $name = $Matches[1]
            $baseList = ''
            $baseType = ''

            if ($Matches.Count -gt 2)
            {
                $baseList = Trim-Text $Matches[2] 200
                $baseType = (($baseList -split ',')[0]).Trim()
            }

            $braceStart = Find-BraceStart -Lines $Lines -StartIndex $i -EndIndex ([Math]::Min($i + 5, $Lines.Count - 1))

            if ($braceStart -lt 0)
            {
                continue
            }

            $end = Get-ScopeEnd -Lines $Lines -StartIndex $braceStart

            $scopes.Add([pscustomobject]@{
                Name = $name
                BaseList = $baseList
                BaseType = $baseType
                StartIndex = $i
                EndIndex = $end
            }) | Out-Null
        }
    }

    return @($scopes.ToArray())
}

function Get-ContainingClass
{
    param(
        [object[]]$ClassScopes,
        [int]$LineIndex
    )

    $matches = @($ClassScopes | Where-Object { $_.StartIndex -le $LineIndex -and $_.EndIndex -ge $LineIndex } | Sort-Object @{ Expression = { $_.EndIndex - $_.StartIndex } })

    if ($matches.Count -eq 0)
    {
        return $null
    }

    return $matches[0]
}

function Get-MethodScope
{
    param(
        [string[]]$Lines,
        [int]$LineIndex
    )

    $braceStart = Find-BraceStart -Lines $Lines -StartIndex $LineIndex -EndIndex ([Math]::Min($LineIndex + 8, $Lines.Count - 1))

    if ($braceStart -lt 0)
    {
        return [pscustomobject]@{
            StartIndex = $LineIndex
            EndIndex = $LineIndex
            CompleteScope = $false
        }
    }

    return [pscustomobject]@{
        StartIndex = $LineIndex
        EndIndex = (Get-ScopeEnd -Lines $Lines -StartIndex $braceStart)
        CompleteScope = $true
    }
}

function Get-InferredWriteType
{
    param(
        [string]$Method,
        [string]$Argument
    )

    if ($Method -ne 'Write')
    {
        return ($Method -replace '^Write', '')
    }

    if ($Argument -match '^\s*\(([^)]+)\)')
    {
        return $Matches[1].Trim()
    }

    if ($Argument -match '^\s*(true|false)\s*$')
    {
        return 'bool'
    }

    if ($Argument -match '^\s*".*"\s*$')
    {
        return 'string'
    }

    if ($Argument -match '^\s*-?[0-9]+\s*$')
    {
        return 'int'
    }

    return 'Unknown'
}

function Get-InferredReadType
{
    param([string]$Method)

    if ($Method -match '^Read(.+)$')
    {
        return $Matches[1]
    }

    return 'Unknown'
}

function Get-Operations
{
    param(
        [string[]]$Lines,
        [object]$Scope,
        [string]$Kind
    )

    $operations = New-Object System.Collections.Generic.List[object]

    if ($null -eq $Scope)
    {
        return @($operations.ToArray())
    }

    for ($i = $Scope.StartIndex; $i -le $Scope.EndIndex -and $i -lt $Lines.Count; $i++)
    {
        $codeLine = Remove-CodeNoise $Lines[$i]

        if ($Kind -eq 'Write' -and $codeLine -match 'writer\.(Write[A-Za-z0-9_]*)\s*\((.*)\)\s*;?')
        {
            $method = $Matches[1]
            $argument = Trim-Text $Matches[2] 160

            $operations.Add([pscustomobject]@{
                LineNumber = $i + 1
                Method = $method
                Argument = $argument
                InferredType = Get-InferredWriteType -Method $method -Argument $argument
                Raw = Trim-Text $Lines[$i] 200
            }) | Out-Null
        }
        elseif ($Kind -eq 'Read' -and $codeLine -match 'reader\.(Read[A-Za-z0-9_]*)\s*\(([^)]*)\)\s*;?')
        {
            $method = $Matches[1]
            $argument = Trim-Text $Matches[2] 160

            $operations.Add([pscustomobject]@{
                LineNumber = $i + 1
                Method = $method
                Argument = $argument
                InferredType = Get-InferredReadType $method
                Raw = Trim-Text $Lines[$i] 200
            }) | Out-Null
        }
    }

    return @($operations.ToArray())
}

function Get-FirstLineInScope
{
    param(
        [string[]]$Lines,
        [object]$Scope,
        [string]$Pattern
    )

    if ($null -eq $Scope)
    {
        return ''
    }

    for ($i = $Scope.StartIndex; $i -le $Scope.EndIndex -and $i -lt $Lines.Count; $i++)
    {
        if ((Remove-CodeNoise $Lines[$i]) -match $Pattern)
        {
            return [string]($i + 1)
        }
    }

    return ''
}

function Format-Operations
{
    param([object[]]$Operations)

    $items = foreach ($op in $Operations)
    {
        'L{0}:{1}[{2}]:{3}' -f $op.LineNumber, $op.Method, $op.InferredType, (Trim-Text $op.Argument 80)
    }

    return ($items -join '; ')
}

function Get-CurrentVersion
{
    param(
        [object[]]$WriteOps,
        [string]$BaseSerializeLine
    )

    $baseLine = 0

    if (-not [string]::IsNullOrWhiteSpace($BaseSerializeLine))
    {
        $baseLine = [int]$BaseSerializeLine
    }

    foreach ($op in $WriteOps)
    {
        if ($op.LineNumber -lt $baseLine)
        {
            continue
        }

        if ($op.Method -eq 'Write' -and $op.Argument -match '^\s*(?:\(int\)\s*)?([0-9]+)\s*$')
        {
            return [pscustomobject]@{
                Version = $Matches[1]
                Line = [string]$op.LineNumber
            }
        }
    }

    return [pscustomobject]@{
        Version = 'Unknown'
        Line = ''
    }
}

function Get-VersionReadLine
{
    param(
        [string[]]$Lines,
        [object]$DeserializeScope
    )

    if ($null -eq $DeserializeScope)
    {
        return ''
    }

    for ($i = $DeserializeScope.StartIndex; $i -le $DeserializeScope.EndIndex -and $i -lt $Lines.Count; $i++)
    {
        if ((Remove-CodeNoise $Lines[$i]) -match '\bversion\s*=\s*reader\.ReadInt\s*\(')
        {
            return [string]($i + 1)
        }
    }

    return ''
}

function Get-VersionHandling
{
    param(
        [string[]]$Lines,
        [object]$DeserializeScope,
        [string]$CurrentVersion,
        [string]$VersionWriteLine,
        [string]$VersionReadLine
    )

    if ([string]::IsNullOrWhiteSpace($VersionWriteLine) -and [string]::IsNullOrWhiteSpace($VersionReadLine))
    {
        return 'NoVersionFound'
    }

    if ([string]::IsNullOrWhiteSpace($VersionWriteLine))
    {
        return 'ReadVersionOnly'
    }

    if ([string]::IsNullOrWhiteSpace($VersionReadLine))
    {
        return 'WriteVersionOnly'
    }

    $body = ''

    if ($null -ne $DeserializeScope)
    {
        $body = (($Lines[$DeserializeScope.StartIndex..$DeserializeScope.EndIndex]) -join "`n")
    }

    if ($body -match 'switch\s*\(\s*version\s*\)' -and $body -match 'goto\s+case')
    {
        return 'SwitchGotoCase'
    }

    if ($body -match 'switch\s*\(\s*version\s*\)')
    {
        return 'Switch'
    }

    if ($body -match 'if\s*\(\s*version\s*[<>=!]')
    {
        return 'IfVersionGates'
    }

    if ($CurrentVersion -ne 'Unknown' -and [int]$CurrentVersion -gt 0)
    {
        return 'SuspiciousOrder'
    }

    return 'SingleVersionOnly'
}

function Get-OlderVersions
{
    param(
        [string[]]$Lines,
        [object]$DeserializeScope
    )

    if ($null -eq $DeserializeScope)
    {
        return ''
    }

    $body = (($Lines[$DeserializeScope.StartIndex..$DeserializeScope.EndIndex]) -join "`n")
    $versions = New-Object System.Collections.Generic.List[string]

    foreach ($match in [regex]::Matches($body, 'case\s+([0-9]+)\s*:'))
    {
        $value = $match.Groups[1].Value

        if (-not $versions.Contains($value))
        {
            $versions.Add($value) | Out-Null
        }
    }

    foreach ($match in [regex]::Matches($body, 'version\s*(?:<=|>=|==|!=|<|>)\s*([0-9]+)'))
    {
        $value = $match.Groups[1].Value

        if (-not $versions.Contains($value))
        {
            $versions.Add($value) | Out-Null
        }
    }

    return (@($versions) | Sort-Object { [int]$_ }) -join ';'
}

function Get-DeletedFields
{
    param(
        [string[]]$Lines,
        [object]$DeserializeScope
    )

    if ($null -eq $DeserializeScope)
    {
        return ''
    }

    $deletedFieldMatches = New-Object System.Collections.Generic.List[string]

    for ($i = $DeserializeScope.StartIndex; $i -le $DeserializeScope.EndIndex -and $i -lt $Lines.Count; $i++)
    {
        $line = $Lines[$i]

        if ($line -match '(removed|discard|legacy|obsolete|unused|old version|backward compat|compatibility)')
        {
            $lineNumber = $i + 1
            $entry = 'L{0}:{1}' -f $lineNumber, (Trim-Text $line 120)
            $deletedFieldMatches.Add($entry) | Out-Null
        }
        elseif ((Remove-CodeNoise $line) -match '^\s*reader\.Read[A-Za-z0-9_]*\s*\(')
        {
            $lineNumber = $i + 1
            $entry = 'L{0}:discard-read:{1}' -f $lineNumber, (Trim-Text $line 120)
            $deletedFieldMatches.Add($entry) | Out-Null
        }
    }

    return ($deletedFieldMatches -join '; ')
}

function Get-FieldAlignment
{
    param(
        [object[]]$WriteOps,
        [object[]]$ReadOps,
        [string]$VersionWriteLine,
        [string]$VersionReadLine
    )

    $dataWrites = @($WriteOps | Where-Object { [string]$_.LineNumber -ne $VersionWriteLine })
    $dataReads = @($ReadOps | Where-Object { [string]$_.LineNumber -ne $VersionReadLine })

    if ($dataWrites.Count -ne $dataReads.Count)
    {
        return "CountMismatch:Writes=$($dataWrites.Count);Reads=$($dataReads.Count)"
    }

    $unknownCount = 0
    $mismatchRows = New-Object System.Collections.Generic.List[string]

    for ($i = 0; $i -lt $dataWrites.Count; $i++)
    {
        $writeType = [string]$dataWrites[$i].InferredType
        $readType = [string]$dataReads[$i].InferredType

        if ($writeType -eq 'Unknown' -or $readType -eq 'Unknown')
        {
            $unknownCount++
            continue
        }

        if ($writeType -ne $readType)
        {
            if (($writeType -eq 'int' -and $readType -eq 'Int') -or ($writeType -eq 'bool' -and $readType -eq 'Bool') -or ($writeType -eq 'string' -and $readType -eq 'String'))
            {
                continue
            }

            $mismatchRows.Add(('#{0}:Write={1}/Read={2}' -f ($i + 1), $writeType, $readType)) | Out-Null
        }
    }

    if ($mismatchRows.Count -gt 0)
    {
        return 'TypeMismatch:' + ($mismatchRows -join ';')
    }

    if ($unknownCount -gt 0)
    {
        return "CountMatchNeedsTypeReview:UnknownWrites=$unknownCount"
    }

    return 'AlignedByCountAndKnownTypes'
}

function Get-Kind
{
    param(
        [string]$TypeName,
        [string]$BaseType,
        [string]$File
    )

    $combined = "$TypeName $BaseType $File"

    switch -Regex ($combined)
    {
        'PlayerMobile|BaseCreature|Mobile' { return 'Mobile' }
        'AddonDeed|Deed' { return 'Deed' }
        'BaseAddon|Addon' { return 'Addon' }
        'Spawner|Spawn' { return 'Spawner' }
        'Controller|System' { return 'Controller' }
        'BaseContainer|Container|BaseWeapon|BaseArmor|Item' { return 'Item' }
        'Gump' { return 'GumpSupport' }
        default { return 'Other' }
    }
}

function Get-MoveRisk
{
    param(
        [string]$File,
        [string]$Namespace,
        [string]$TypeName,
        [string]$CardSystems,
        [bool]$IncludedInProject
    )

    if (-not $IncludedInProject)
    {
        return [pscustomobject]@{
            Risk = 'UnknownUntilBuildTruthRepaired'
            Reason = 'The serialized source is not currently confirmed as compiled by Scripts.csproj.'
        }
    }

    if ([string]::IsNullOrWhiteSpace($Namespace) -or [string]::IsNullOrWhiteSpace($TypeName))
    {
        return [pscustomobject]@{
            Risk = 'UnknownUntilSaveTest'
            Reason = 'Namespace or type name could not be extracted reliably.'
        }
    }

    if ($File -match '^Data/System/Source/' -or $TypeName -eq 'PlayerMobile')
    {
        return [pscustomobject]@{
            Risk = 'DoNotMove'
            Reason = 'Core serialized type; moving or renaming needs an explicit migration and save-load test.'
        }
    }

    if ($CardSystems -match 'XMLSpawner|Homestead|Government|PlayerMobile|Housing|Regions|Spell Framework|Vendor Core')
    {
        return [pscustomobject]@{
            Risk = 'UnknownUntilSaveTest'
            Reason = 'High-risk or imported serialized system; file-only move may be safe, but namespace/type rename is dangerous and save testing is required.'
        }
    }

    return [pscustomobject]@{
        Risk = 'NamespaceOrTypeRenameDanger'
        Reason = 'World saves bind serialized namespace and type name; do not rename without a migration plan.'
    }
}

function New-BacklogRow
{
    param(
        [int]$Index,
        [object]$Row,
        [string]$Category,
        [string]$Risk,
        [string]$Recommendation,
        [string]$Verification
    )

    return [pscustomobject]@{
        Id = ('SERIAL-{0:D4}' -f $Index)
        Priority = $Risk
        Status = 'Open'
        System = $Row.System
        Category = $Category
        File = $Row.File
        Class = $Row.Class
        Evidence = $Row.Evidence
        Risk = $Risk
        Recommendation = $Recommendation
        Verification = $Verification
        Notes = 'Generated by Phase 6 marker-based serializer review; confirm against source before code changes.'
    }
}

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

$serializationMarkers = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-serialization-marker-inventory.csv'))
$runtimeInventory = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'cross-tree-runtime-inventory.csv'))
$projectTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'project-truth-register.csv'))
$systemOwnerMap = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'system-owner-map.csv'))

$runtimeByPath = @{}
foreach ($row in $runtimeInventory)
{
    $runtimeByPath[$row.Path] = $row
}

$projectByPath = @{}
foreach ($row in $projectTruth)
{
    if (-not $projectByPath.ContainsKey($row.Path))
    {
        $projectByPath[$row.Path] = New-Object System.Collections.Generic.List[object]
    }

    $projectByPath[$row.Path].Add($row) | Out-Null
}

$cardSystemsByPath = @{}
foreach ($row in $systemOwnerMap)
{
    if (-not $cardSystemsByPath.ContainsKey($row.File))
    {
        $cardSystemsByPath[$row.File] = New-Object System.Collections.Generic.List[string]
    }

    if (-not $cardSystemsByPath[$row.File].Contains($row.System))
    {
        $cardSystemsByPath[$row.File].Add($row.System) | Out-Null
    }
}

$registerRows = New-Object System.Collections.Generic.List[object]

foreach ($marker in $serializationMarkers)
{
    $repoPath = $marker.File
    $literalPath = Join-Path $RepoRoot ($repoPath -replace '/', '\')

    if (-not (Test-Path -LiteralPath $literalPath -PathType Leaf))
    {
        continue
    }

    $lines = [string[]](Get-Content -LiteralPath $literalPath)
    $namespace = Get-Namespace $lines
    $classScopes = @(Get-ClassScopes $lines)
    $classStates = @{}
    $serializePattern = '\bSerialize\s*\(\s*GenericWriter\s+writer\s*\)'
    $deserializePattern = '\bDeserialize\s*\(\s*GenericReader\s+reader\s*\)'

    for ($i = 0; $i -lt $lines.Count; $i++)
    {
        $line = Remove-CodeNoise $lines[$i]

        if ($line -match $serializePattern -or $line -match $deserializePattern)
        {
            $class = Get-ContainingClass -ClassScopes $classScopes -LineIndex $i
            $key = if ($null -ne $class) { "$($class.StartIndex):$($class.Name)" } else { "file:$repoPath" }

            if (-not $classStates.ContainsKey($key))
            {
                $classStates[$key] = [pscustomobject]@{
                    ClassScope = $class
                    SerializeScope = $null
                    DeserializeScope = $null
                    SerialConstructorLine = ''
                }
            }

            if ($line -match $serializePattern)
            {
                $classStates[$key].SerializeScope = Get-MethodScope -Lines $lines -LineIndex $i
            }
            elseif ($line -match $deserializePattern)
            {
                $classStates[$key].DeserializeScope = Get-MethodScope -Lines $lines -LineIndex $i
            }
        }
    }

    foreach ($class in $classScopes)
    {
        $ctorPattern = ('\b{0}\s*\(\s*Serial\s+[A-Za-z_][A-Za-z0-9_]*\s*\)' -f [regex]::Escape($class.Name))
        $ctorLine = ''

        for ($i = $class.StartIndex; $i -le $class.EndIndex -and $i -lt $lines.Count; $i++)
        {
            if ((Remove-CodeNoise $lines[$i]) -match $ctorPattern)
            {
                $ctorLine = [string]($i + 1)
                break
            }
        }

        if (-not [string]::IsNullOrWhiteSpace($ctorLine))
        {
            $key = "$($class.StartIndex):$($class.Name)"

            if (-not $classStates.ContainsKey($key))
            {
                $classStates[$key] = [pscustomobject]@{
                    ClassScope = $class
                    SerializeScope = $null
                    DeserializeScope = $null
                    SerialConstructorLine = ''
                }
            }

            $classStates[$key].SerialConstructorLine = $ctorLine
        }
    }

    if ($classStates.Count -eq 0)
    {
        $classStates['file'] = [pscustomobject]@{
            ClassScope = $null
            SerializeScope = $null
            DeserializeScope = $null
            SerialConstructorLine = ''
        }
    }

    foreach ($state in $classStates.Values)
    {
        $classScope = $state.ClassScope
        $typeName = if ($null -ne $classScope) { $classScope.Name } else { 'Unknown' }
        $baseType = if ($null -ne $classScope) { $classScope.BaseType } else { '' }
        $fullClassName = if (-not [string]::IsNullOrWhiteSpace($namespace) -and $typeName -ne 'Unknown') { "$namespace.$typeName" } else { $typeName }
        $serializeScope = $state.SerializeScope
        $deserializeScope = $state.DeserializeScope
        $writeOps = @(Get-Operations -Lines $lines -Scope $serializeScope -Kind 'Write')
        $readOps = @(Get-Operations -Lines $lines -Scope $deserializeScope -Kind 'Read')
        $baseSerializeLine = Get-FirstLineInScope -Lines $lines -Scope $serializeScope -Pattern 'base\.Serialize\s*\(\s*writer\s*\)'
        $baseDeserializeLine = Get-FirstLineInScope -Lines $lines -Scope $deserializeScope -Pattern 'base\.Deserialize\s*\(\s*reader\s*\)'
        $versionInfo = Get-CurrentVersion -WriteOps $writeOps -BaseSerializeLine $baseSerializeLine
        $versionReadLine = Get-VersionReadLine -Lines $lines -DeserializeScope $deserializeScope
        $versionHandling = Get-VersionHandling -Lines $lines -DeserializeScope $deserializeScope -CurrentVersion $versionInfo.Version -VersionWriteLine $versionInfo.Line -VersionReadLine $versionReadLine
        $olderVersions = Get-OlderVersions -Lines $lines -DeserializeScope $deserializeScope
        $deletedFields = Get-DeletedFields -Lines $lines -DeserializeScope $deserializeScope
        $fieldAlignment = Get-FieldAlignment -WriteOps $writeOps -ReadOps $readOps -VersionWriteLine $versionInfo.Line -VersionReadLine $versionReadLine
        $runtimeRow = if ($runtimeByPath.ContainsKey($repoPath)) { $runtimeByPath[$repoPath] } else { $null }
        $system = if ($null -ne $runtimeRow) { $runtimeRow.System } else { $marker.LikelySystem }
        $cardSystems = if ($cardSystemsByPath.ContainsKey($repoPath)) { ($cardSystemsByPath[$repoPath] -join ';') } else { '' }
        $projectRows = if ($projectByPath.ContainsKey($repoPath)) { @($projectByPath[$repoPath].ToArray()) } else { @() }
        $included = @($projectRows | Where-Object { $_.IncludedInScriptsProject -eq 'True' }).Count -gt 0
        $missingCompile = @($projectRows | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count -gt 0
        $unincluded = @($projectRows | Where-Object { $_.UnincludedSource -eq 'True' }).Count -gt 0
        $kind = Get-Kind -TypeName $typeName -BaseType $baseType -File $repoPath
        $moveRisk = Get-MoveRisk -File $repoPath -Namespace $namespace -TypeName $typeName -CardSystems $cardSystems -IncludedInProject $included
        $hasSerialize = $null -ne $serializeScope
        $hasDeserialize = $null -ne $deserializeScope
        $hasSerialConstructor = -not [string]::IsNullOrWhiteSpace($state.SerialConstructorLine)
        $baseCallStatus = 'BaseCallsFirst'

        if ($hasSerialize -and [string]::IsNullOrWhiteSpace($baseSerializeLine))
        {
            $baseCallStatus = 'MissingBaseSerialize'
        }
        elseif ($hasDeserialize -and [string]::IsNullOrWhiteSpace($baseDeserializeLine))
        {
            $baseCallStatus = 'MissingBaseDeserialize'
        }
        elseif ($writeOps.Count -gt 0 -and -not [string]::IsNullOrWhiteSpace($baseSerializeLine) -and [int]$baseSerializeLine -gt $writeOps[0].LineNumber)
        {
            $baseCallStatus = 'BaseSerializeAfterWrite'
        }
        elseif ($readOps.Count -gt 0 -and -not [string]::IsNullOrWhiteSpace($baseDeserializeLine) -and [int]$baseDeserializeLine -gt $readOps[0].LineNumber)
        {
            $baseCallStatus = 'BaseDeserializeAfterRead'
        }

        $commentNeeded = 'No'
        $commentReason = ''
        $reviewReasons = New-Object System.Collections.Generic.List[string]

        if (-not $hasSerialize -or -not $hasDeserialize)
        {
            $reviewReasons.Add('Serializer pair is missing one side.') | Out-Null
        }

        if (-not $hasSerialConstructor)
        {
            $reviewReasons.Add('Serial constructor not detected.') | Out-Null
        }

        if ($baseCallStatus -ne 'BaseCallsFirst')
        {
            $reviewReasons.Add($baseCallStatus) | Out-Null
        }

        if ($versionHandling -in @('NoVersionFound', 'WriteVersionOnly', 'ReadVersionOnly', 'SuspiciousOrder'))
        {
            $reviewReasons.Add($versionHandling) | Out-Null
        }

        if ($fieldAlignment -match 'Mismatch')
        {
            $reviewReasons.Add($fieldAlignment) | Out-Null
        }

        if (-not $included -or $missingCompile -or $unincluded)
        {
            $reviewReasons.Add('Project truth mismatch affects serialized source.') | Out-Null
        }

        if ($moveRisk.Risk -in @('DoNotMove', 'UnknownUntilSaveTest', 'UnknownUntilBuildTruthRepaired'))
        {
            $reviewReasons.Add($moveRisk.Risk) | Out-Null
        }

        if ($deletedFields.Length -gt 0 -or $versionHandling -in @('SwitchGotoCase', 'Switch', 'IfVersionGates') -or $fieldAlignment -match 'NeedsTypeReview|Mismatch' -or $moveRisk.Risk -ne 'NamespaceOrTypeRenameDanger')
        {
            $commentNeeded = 'Yes'
            $commentReason = 'Fragile serialization order, version gates, discarded reads, or move/rename risk should be reviewed for a Phase 11 source comment.'
        }

        $reviewStatus = if ($reviewReasons.Count -gt 0) { 'NeedsSourceReview' } else { 'MachineMapped' }

        $evidence = @(
            if ($hasSerialize) { 'Serialize:L{0}' -f ($serializeScope.StartIndex + 1) }
            if ($hasDeserialize) { 'Deserialize:L{0}' -f ($deserializeScope.StartIndex + 1) }
            if ($hasSerialConstructor) { 'SerialCtor:L{0}' -f $state.SerialConstructorLine }
            if (-not [string]::IsNullOrWhiteSpace($versionInfo.Line)) { 'VersionWrite:L{0}' -f $versionInfo.Line }
            if (-not [string]::IsNullOrWhiteSpace($versionReadLine)) { 'VersionRead:L{0}' -f $versionReadLine }
        ) -join ';'

        $registerRows.Add([pscustomobject]@{
            System = $system
            CardSystems = $cardSystems
            File = $repoPath
            Namespace = $namespace
            Class = $fullClassName
            TypeName = $typeName
            BaseType = $baseType
            Kind = $kind
            IncludedInScriptsProject = [string]$included
            MissingCompileTarget = [string]$missingCompile
            UnincludedSource = [string]$unincluded
            HasSerialConstructor = [string]$hasSerialConstructor
            SerializeLine = if ($hasSerialize) { [string]($serializeScope.StartIndex + 1) } else { '' }
            DeserializeLine = if ($hasDeserialize) { [string]($deserializeScope.StartIndex + 1) } else { '' }
            BaseSerializeLine = $baseSerializeLine
            BaseDeserializeLine = $baseDeserializeLine
            BaseCallStatus = $baseCallStatus
            CurrentVersion = $versionInfo.Version
            VersionWriteLine = $versionInfo.Line
            VersionReadLine = $versionReadLine
            VersionHandling = $versionHandling
            OlderVersions = $olderVersions
            WriteCount = $writeOps.Count
            ReadCount = $readOps.Count
            Writes = Format-Operations $writeOps
            Reads = Format-Operations $readOps
            FieldAlignment = $fieldAlignment
            DeletedFields = $deletedFields
            MoveRenameRisk = $moveRisk.Risk
            MoveRenameRiskReason = $moveRisk.Reason
            CommentNeeded = $commentNeeded
            CommentReason = $commentReason
            ReviewStatus = $reviewStatus
            ReviewReasons = ($reviewReasons -join '; ')
            Evidence = $evidence
        }) | Out-Null
    }
}

$registerRows = @($registerRows | Sort-Object File, Class, SerializeLine, DeserializeLine)

$highRiskRows = @($registerRows | Where-Object {
    $_.ReviewStatus -ne 'MachineMapped' -or
    $_.CardSystems -ne '' -or
    $_.MoveRenameRisk -in @('DoNotMove', 'UnknownUntilSaveTest', 'UnknownUntilBuildTruthRepaired') -or
    $_.VersionHandling -in @('NoVersionFound', 'WriteVersionOnly', 'ReadVersionOnly', 'SuspiciousOrder') -or
    $_.FieldAlignment -match 'Mismatch'
})

$moveRiskRows = @($registerRows | Where-Object { $_.MoveRenameRisk -ne 'FileMoveOnlyLikelySafe' })

$commentTargetRows = @($registerRows | Where-Object { $_.CommentNeeded -eq 'Yes' } | ForEach-Object {
    [pscustomobject]@{
        File = $_.File
        Class = $_.Class
        Location = if (-not [string]::IsNullOrWhiteSpace($_.SerializeLine)) { 'Serialize/Deserialize' } else { 'Serialized type declaration' }
        CommentType = 'Serialization'
        Reason = $_.CommentReason
        DraftComment = '// Save format: keep Serialize and Deserialize field order, version gates, and discarded legacy reads aligned.'
        SourcePhase = 'Phase 6'
        Status = 'CandidateForPhase11'
        Evidence = $_.Evidence
    }
})

$backlogRows = New-Object System.Collections.Generic.List[object]
$backlogIndex = 1

foreach ($row in $registerRows)
{
    if ($row.HasSerialConstructor -ne 'True')
    {
        $backlogRows.Add((New-BacklogRow -Index $backlogIndex -Row $row -Category 'MissingSerialConstructor' -Risk 'High' -Recommendation 'Confirm whether the type is save-persistent; add the RunUO Serial constructor if it must load from world saves.' -Verification 'Re-run Phase 6 serialization register and narrow project build.')) | Out-Null
        $backlogIndex++
    }

    if ([string]::IsNullOrWhiteSpace($row.SerializeLine) -or [string]::IsNullOrWhiteSpace($row.DeserializeLine))
    {
        $backlogRows.Add((New-BacklogRow -Index $backlogIndex -Row $row -Category 'MissingSerializerPair' -Risk 'High' -Recommendation 'Pair Serialize and Deserialize or document why this marker is not a save-persistent type.' -Verification 'Re-run Phase 6 serialization register and narrow project build.')) | Out-Null
        $backlogIndex++
    }

    if ($row.BaseCallStatus -ne 'BaseCallsFirst')
    {
        $backlogRows.Add((New-BacklogRow -Index $backlogIndex -Row $row -Category 'BaseCallOrder' -Risk 'Critical' -Recommendation 'Review base Serialize/Deserialize order before any save-affecting edit.' -Verification 'Manual source review plus Phase 6 register rerun.')) | Out-Null
        $backlogIndex++
    }

    if ($row.VersionHandling -in @('NoVersionFound', 'WriteVersionOnly', 'ReadVersionOnly', 'SuspiciousOrder'))
    {
        $backlogRows.Add((New-BacklogRow -Index $backlogIndex -Row $row -Category 'VersionHandling' -Risk 'High' -Recommendation 'Confirm current save version write/read and add guarded older-version handling where needed.' -Verification 'Manual source review plus Phase 6 register rerun.')) | Out-Null
        $backlogIndex++
    }

    if ($row.FieldAlignment -match 'Mismatch')
    {
        $backlogRows.Add((New-BacklogRow -Index $backlogIndex -Row $row -Category 'WriteReadAlignment' -Risk 'Critical' -Recommendation 'Compare ordered writes and reads manually; preserve save order or add a migration plan before edits.' -Verification 'Manual source review plus Phase 6 register rerun.')) | Out-Null
        $backlogIndex++
    }

    if ($row.IncludedInScriptsProject -ne 'True')
    {
        $backlogRows.Add((New-BacklogRow -Index $backlogIndex -Row $row -Category 'SerializedSourceProjectTruth' -Risk 'Medium' -Recommendation 'Resolve project truth status before relying on this serialized source during reorganization.' -Verification 'Re-run project truth and Phase 6 serialization register.')) | Out-Null
        $backlogIndex++
    }
}

$serializationRegisterPath = Export-AuditCsv -Rows $registerRows -FileName 'serialization-register.csv'
$phaseRegisterPath = Export-AuditCsv -Rows $registerRows -FileName 'phase-06-serialization-register.csv'
$highRiskPath = Export-AuditCsv -Rows $highRiskRows -FileName 'phase-06-high-risk-serializer-list.csv'
$moveRiskPath = Export-AuditCsv -Rows $moveRiskRows -FileName 'phase-06-move-rename-risk-list.csv'
$commentTargetsPath = Export-AuditCsv -Rows $commentTargetRows -FileName 'phase-06-serializer-comment-target-list.csv'
$backlogRowsArray = @($backlogRows.ToArray())
$backlogPath = Export-AuditCsv -Rows $backlogRowsArray -FileName 'phase-06-save-compatibility-repair-backlog.csv'

$versionCounts = @($registerRows | Group-Object VersionHandling | Sort-Object Name)
$alignmentCounts = @($registerRows | Group-Object FieldAlignment | Sort-Object Count -Descending | Select-Object -First 12)
$moveRiskCounts = @($registerRows | Group-Object MoveRenameRisk | Sort-Object Name)

$summaryPath = Join-Path $OutputDir 'phase-06-summary.md'
$summaryLines = New-Object System.Collections.Generic.List[string]
$summaryLines.Add('# Phase 6 Serialization And Save Compatibility Summary') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add("Generated: $(Get-Date -Format o)") | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Required Inputs') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Input | Status |') | Out-Null
$summaryLines.Add('| --- | --- |') | Out-Null
$summaryLines.Add("| Serialization marker scans | Present: ``phase-01-serialization-marker-inventory.csv`` with $($serializationMarkers.Count) file rows |") | Out-Null
$summaryLines.Add("| CrossTreeRuntimeInventory | Present: ``cross-tree-runtime-inventory.csv`` with $($runtimeInventory.Count) rows |") | Out-Null
$summaryLines.Add("| System Cards | Present: ``system-owner-map.csv`` with $($systemOwnerMap.Count) owner rows |") | Out-Null
$summaryLines.Add("| Project Truth Register | Present: ``project-truth-register.csv`` with $($projectTruth.Count) rows |") | Out-Null
$summaryLines.Add("| Root serialization standards | Present in root ``AGENTS.md`` instructions |") | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Generated Outputs') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Output | Rows | Purpose |') | Out-Null
$summaryLines.Add('| --- | ---: | --- |') | Out-Null
$summaryLines.Add("| ``serialization-register.csv`` | $($registerRows.Count) | Canonical serializer map with class, save version, ordered writes/reads, version handling, field alignment, and move risk. |") | Out-Null
$summaryLines.Add("| ``phase-06-serialization-register.csv`` | $($registerRows.Count) | Phase-scoped copy of the serializer register. |") | Out-Null
$summaryLines.Add("| ``phase-06-high-risk-serializer-list.csv`` | $($highRiskRows.Count) | Rows requiring manual review because of high-risk systems, project truth, versioning, pairing, alignment, or move risk. |") | Out-Null
$summaryLines.Add("| ``phase-06-move-rename-risk-list.csv`` | $($moveRiskRows.Count) | Serialized types with namespace/type or file-move risk classification. |") | Out-Null
$summaryLines.Add("| ``phase-06-serializer-comment-target-list.csv`` | $($commentTargetRows.Count) | Candidate Phase 11 source-comment targets for fragile save behavior. |") | Out-Null
$summaryLines.Add("| ``phase-06-save-compatibility-repair-backlog.csv`` | $($backlogRowsArray.Count) | Concrete serializer follow-up items created from machine-detected risks. |") | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Version Handling Counts') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Version Handling | Count |') | Out-Null
$summaryLines.Add('| --- | ---: |') | Out-Null
foreach ($group in $versionCounts)
{
    $summaryLines.Add("| $(Escape-MarkdownCell $group.Name) | $($group.Count) |") | Out-Null
}
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Field Alignment Counts') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Field Alignment | Count |') | Out-Null
$summaryLines.Add('| --- | ---: |') | Out-Null
foreach ($group in $alignmentCounts)
{
    $summaryLines.Add("| $(Escape-MarkdownCell $group.Name) | $($group.Count) |") | Out-Null
}
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Move/Rename Risk Counts') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Move/Rename Risk | Count |') | Out-Null
$summaryLines.Add('| --- | ---: |') | Out-Null
foreach ($group in $moveRiskCounts)
{
    $summaryLines.Add("| $(Escape-MarkdownCell $group.Name) | $($group.Count) |") | Out-Null
}
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Exit Criteria') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('- Serialized marker files were deepened into per-class serializer rows with ordered write/read maps where method scopes could be extracted.') | Out-Null
$summaryLines.Add('- High-risk, asymmetric, unversioned, project-truth, and move/rename-sensitive rows are explicit in focused registers and backlog outputs.') | Out-Null
$summaryLines.Add('- Source comments are not added in Phase 6; candidate comments are written to the Phase 6 comment target list for Phase 11 review.') | Out-Null
$summaryLines.Add('- Reorganization remains blocked for serialized namespace/type renames unless a migration plan and save-load verification are approved.') | Out-Null

Set-Content -LiteralPath $summaryPath -Value $summaryLines -Encoding UTF8

[pscustomobject]@{
    RegisterRows = $registerRows.Count
    HighRiskRows = $highRiskRows.Count
    MoveRiskRows = $moveRiskRows.Count
    CommentTargetRows = $commentTargetRows.Count
    BacklogRows = $backlogRowsArray.Count
    SerializationRegister = Convert-ToRepoPath $serializationRegisterPath
    PhaseRegister = Convert-ToRepoPath $phaseRegisterPath
    HighRiskList = Convert-ToRepoPath $highRiskPath
    MoveRiskList = Convert-ToRepoPath $moveRiskPath
    CommentTargets = Convert-ToRepoPath $commentTargetsPath
    Backlog = Convert-ToRepoPath $backlogPath
    Summary = Convert-ToRepoPath $summaryPath
}
