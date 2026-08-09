param(
    [string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot),
    [string]$OutputDirectory = "docs/turn-based-combat"
)

$ErrorActionPreference = "Stop"

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)

function Get-CanonicalSourceHash {
    param(
        [AllowEmptyString()]
        [string]$Content
    )

    $normalized = $Content.Replace("`r`n", "`n").Replace("`r", "`n")
    $bytes = $utf8NoBom.GetBytes($normalized)
    $algorithm = [System.Security.Cryptography.SHA256]::Create()

    try {
        return ([System.BitConverter]::ToString($algorithm.ComputeHash($bytes))).Replace("-", "")
    }
    finally {
        $algorithm.Dispose()
    }
}

$scriptRoot = Join-Path $RepositoryRoot "Data/Scripts"
$outputRoot = Join-Path $RepositoryRoot $OutputDirectory
$csvPath = Join-Path $outputRoot "compatibility-register.csv"
$summaryPath = Join-Path $outputRoot "COMPATIBILITY_SUMMARY.md"
$auditInventoryPath = Join-Path $RepositoryRoot "docs/codebase-audit/outputs/cross-tree-runtime-inventory.csv"

if (-not (Test-Path -LiteralPath $scriptRoot -PathType Container)) {
    throw "Runtime script root was not found: $scriptRoot"
}

if (-not (Test-Path -LiteralPath $outputRoot -PathType Container)) {
    New-Item -ItemType Directory -Path $outputRoot | Out-Null
}

$rules = @(
    @{ Surface = "Clock.ParticipantCooldown"; Pattern = "(?s)(Next(Action|Skill|Spell|Combat)Time\s*(=|>=|<=|>|<)[^;]{0,240}DateTime\.Now|DateTime\.Now[^;]{0,240}(>=|<=|>|<|=)\s*[^;]{0,120}Next(Action|Skill|Spell|Combat)Time)"; Disposition = "Unknown"; Adapter = "TurnBasedCombatBridge.GetTime"; Catalog = "Clock:ParticipantCooldown"; Test = "TBC-CLOCK-COOLDOWN"; Rationale = "Direct participant cooldown and wall-clock mixing must be converted or explicitly reviewed before activation." },
    @{ Surface = "AI.OnThink"; Pattern = "\bOnThink\s*\("; Disposition = "ActorClockAdapter"; Adapter = "BaseAI.ProcessTurnBasedPulse"; Catalog = "AI:*"; Test = "TBC-AI-THINK"; Rationale = "Participant AI runs from its personal logical clock." },
    @{ Surface = "AI.ForcedAI"; Pattern = "\bForcedAI\b"; Disposition = "BlockedInCombat"; Adapter = "Compatibility fail-closed guard"; Catalog = "AI:ForcedAI"; Test = "TBC-AI-FORCED"; Rationale = "Forced shells require an explicit TurnAI.csv opt-in before acting in combat." },
    @{ Surface = "Timer.Subclass"; Pattern = ":\s*Timer\b"; Disposition = "WallClockUnaffected"; Adapter = "Mutation backstop and explicit effect adapters"; Catalog = "Effect:*"; Test = "TBC-TIMER-OWNER"; Rationale = "Timers remain wall-clock unless an owner-aware effect catalog entry overrides them." },
    @{ Surface = "Timer.DelayCall"; Pattern = "Timer\.DelayCall\s*\("; Disposition = "WallClockUnaffected"; Adapter = "Mutation backstop"; Catalog = "Effect:DelayCall"; Test = "TBC-TIMER-DELAY"; Rationale = "Delayed world work stays wall-clock; participant mutations still require an authorized scope." },
    @{ Surface = "Clock.DateTimeNow"; Pattern = "DateTime\.Now"; Disposition = "WallClockUnaffected"; Adapter = "Central actor-time seams"; Catalog = "Clock:*"; Test = "TBC-CLOCK-WALL"; Rationale = "Only central combat cooldown, skill, spell, effect, and AI seams use participant time." },
    @{ Surface = "Action.Spell"; Pattern = "\bclass\s+\w+\s*:\s*[^\r\n]*\bSpell\b"; Disposition = "NativeBridge"; Adapter = "Spell.Cast and sequence bridge"; Catalog = "Spell:*"; Test = "TBC-ACTION-SPELL"; Rationale = "Spell legality and resources stay native while AP and targeting use leases." },
    @{ Surface = "Action.ItemUse"; Pattern = "\bOnDoubleClick\s*\("; Disposition = "NativeBridge"; Adapter = "Mobile.Use bridge"; Catalog = "ItemUse:*"; Test = "TBC-ACTION-ITEM"; Rationale = "Item use is cataloged by runtime type while participants are active." },
    @{ Surface = "Action.Target"; Pattern = "\bOnTarget\s*\("; Disposition = "NativeBridge"; Adapter = "Target.Invoke completion bridge"; Catalog = "Target:*"; Test = "TBC-ACTION-TARGET"; Rationale = "Target completion commits or refunds the actor's pending lease." },
    @{ Surface = "Mutation.Damage"; Pattern = "\.Damage\s*\("; Disposition = "NativeBridge"; Adapter = "Mobile.Damage mutation scope"; Catalog = "Mutation:Damage"; Test = "TBC-MUTATION-DAMAGE"; Rationale = "Participant damage must originate from an authorized action or effect scope." },
    @{ Surface = "Mutation.Heal"; Pattern = "\.Heal\s*\("; Disposition = "NativeBridge"; Adapter = "Mobile.Heal mutation scope"; Catalog = "Mutation:Healing"; Test = "TBC-MUTATION-HEAL"; Rationale = "Participant healing must originate from an authorized action or effect scope." },
    @{ Surface = "Mutation.DirectState"; Pattern = "\.(Hits|Stam|Mana|Poison|Paralyzed|Frozen)\s*(\+\+|--|[+\-]?=)"; Disposition = "NativeBridge"; Adapter = "Mobile state mutation backstop"; Catalog = "Mutation:State"; Test = "TBC-MUTATION-DIRECT"; Rationale = "Direct participant state changes fail closed outside a mutation scope." },
    @{ Surface = "Legality.PvP"; Pattern = "CanBe(Harmful|Beneficial)|Notoriety|PvPConsent|ChallengeGame|Government"; Disposition = "NativeBridge"; Adapter = "Post-legality combat intent bridge"; Catalog = "Legality:*"; Test = "TBC-LEGALITY"; Rationale = "Existing legality remains authoritative and group creation occurs only after it passes." },
    @{ Surface = "Persistence.Serialization"; Pattern = "\b(Serialize|Deserialize)\s*\("; Disposition = "NotCombatRelevant"; Adapter = "No group serialization"; Catalog = "Persistence:Canonical"; Test = "TBC-SAVE-RESTART"; Rationale = "Canonical owner state remains unchanged; combat groups are process-local." }
)

$rows = New-Object System.Collections.Generic.List[object]
$auditByPath = @{}

if (Test-Path -LiteralPath $auditInventoryPath -PathType Leaf) {
    foreach ($entry in (Import-Csv -LiteralPath $auditInventoryPath)) {
        $auditByPath[$entry.Path.Replace('\', '/')] = $entry
    }
}

$files = Get-ChildItem -LiteralPath $scriptRoot -Recurse -File -Filter "*.cs" |
    Where-Object { $_.FullName -notmatch "[\\/](bin|obj)[\\/]" } |
    Sort-Object FullName

foreach ($file in $files) {
    $relative = $file.FullName.Substring($RepositoryRoot.Length).TrimStart('\', '/').Replace('\', '/')
    $ownerParts = $relative.Split('/')
    $auditEntry = $auditByPath[$relative]
    $owner = if ($null -ne $auditEntry -and -not [String]::IsNullOrWhiteSpace($auditEntry.System)) { $auditEntry.System } elseif ($ownerParts.Length -gt 2) { $ownerParts[2] } else { "Unknown" }
    $content = [System.IO.File]::ReadAllText($file.FullName)
    $sourceHash = Get-CanonicalSourceHash -Content $content
    $matched = $false

    foreach ($rule in $rules) {
        if ($content -match $rule.Pattern) {
            $matched = $true
            $rows.Add([pscustomobject]@{
                SystemOwner = $owner
                File = $relative
                SourceHash = $sourceHash
                Type = "RuntimeScript"
                Surface = $rule.Surface
                TimeSource = if ($rule.Surface -like "Timer.*" -or $rule.Surface -like "Clock.*") { "WallClock" } elseif ($rule.Disposition -eq "ActorClockAdapter") { "ActorClock" } else { "N/A" }
                Mutation = if ($rule.Surface -like "Mutation.*") { $rule.Surface.Substring(9) } else { "None" }
                Disposition = $rule.Disposition
                Adapter = $rule.Adapter
                CatalogKey = $rule.Catalog
                TestId = $rule.Test
                Evidence = "$relative regex:$($rule.Pattern)"
                Rationale = $rule.Rationale
            })
        }
    }

    if (-not $matched) {
        $rows.Add([pscustomobject]@{
            SystemOwner = $owner
            File = $relative
            SourceHash = $sourceHash
            Type = "RuntimeScript"
            Surface = "NoDetectedCombatSurface"
            TimeSource = "N/A"
            Mutation = "None"
            Disposition = "NotCombatRelevant"
            Adapter = "None"
            CatalogKey = "None"
            TestId = "TBC-INVENTORY-COVERAGE"
            Evidence = "$relative full-file scan"
            Rationale = "The reproducible surface scan found no turn-combat interception pattern."
        })
    }
}

$rows | Export-Csv -LiteralPath $csvPath -NoTypeInformation -Encoding UTF8

$dispositions = $rows | Group-Object Disposition | Sort-Object Name
$surfaces = $rows | Group-Object Surface | Sort-Object Name
$unknownCount = @($rows | Where-Object { $_.Disposition -eq "Unknown" }).Count

$summary = New-Object System.Collections.Generic.List[string]
$summary.Add("# Turn-Based Combat Compatibility Summary")
$summary.Add("")
$summary.Add("Generated by ``scripts/Generate-TurnBasedCombatCompatibility.ps1``.")
$summary.Add("")
$summary.Add("- Runtime scripts scanned: $($files.Count)")
$summary.Add("- Audit inventory entries loaded: $($auditByPath.Count)")
$summary.Add("- Compatibility rows: $($rows.Count)")
$summary.Add("- Unknown dispositions: $unknownCount")
$summary.Add("- Production gate: $(if ($unknownCount -eq 0) { 'PASS' } else { 'BLOCKED' })")
$summary.Add("")
$summary.Add("## Dispositions")
$summary.Add("")
foreach ($group in $dispositions) { $summary.Add("- $($group.Name): $($group.Count)") }
$summary.Add("")
$summary.Add("## Detected Surfaces")
$summary.Add("")
foreach ($group in $surfaces) { $summary.Add("- $($group.Name): $($group.Count)") }
$summary.Add("")
$summary.Add("Source hashes normalize CRLF, lone CR, and optional byte-order marks to UTF-8 text with LF line endings. Other whitespace and source changes still produce compatibility drift.")
$summary.Add("")
$summary.Add("Direct participant cooldown expressions may not mix ``NextActionTime``, ``NextSkillTime``, ``NextSpellTime``, or ``NextCombatTime`` with ``DateTime.Now``. Core native timers that explicitly skip turn-combat participants remain wall-clock allowlisted outside the runtime-script register.")
$summary.Add("")
$summary.Add("The register distinguishes live runtime script truth from ``Scripts.csproj`` IDE project hygiene. Regenerate it after any runtime-script change and review disposition drift before enabling combat.")

[System.IO.File]::WriteAllLines($summaryPath, $summary)

Write-Host "Scanned $($files.Count) runtime scripts."
Write-Host "Wrote $csvPath"
Write-Host "Wrote $summaryPath"
Write-Host "Unknown dispositions: $unknownCount"

if ($unknownCount -ne 0) {
    exit 2
}
