# Phase 0 Baseline And Guardrails

Generated: 2026-06-05T16:27:58.9513512-05:00

Branch: `SAR`

Baseline HEAD: `86659bba First commit of the Systems Audit and Reorganization branch.`

Phase plan: `docs/codebase-audit-phases/phase-00-baseline-and-guardrails.md`

Root plan: `CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`

## Required Inputs

| Input | Status | Evidence |
| --- | --- | --- |
| Root `AGENTS.md` | Present and read | `Get-Content -Raw -LiteralPath AGENTS.md` |
| Nested `AGENTS.md` files | Located | `rg --files -g AGENTS.md` |
| `git status --short` | Captured | Returned no output before Phase 0 edits |
| Current branch and recent commits | Captured | `git branch --show-current`; `git log --oneline -5` |
| `ConficturaUO.sln` | Present and inspected | `Get-Content -LiteralPath ConficturaUO.sln -TotalCount 70` |
| `Data/System/Source/Server.csproj` | Present and inspected | `Get-Content -LiteralPath Data/System/Source/Server.csproj -TotalCount 55` |
| `Data/Scripts/Scripts.csproj` | Present and inspected | `Get-Content -LiteralPath Data/Scripts/Scripts.csproj -TotalCount 45` |
| Root audit plan | Present and read | `Get-Content -Raw -LiteralPath CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md` |
| Detailed phase files | Present and read | `Get-Content -Raw` for phase files `phase-00` through `phase-14` |

## Required Outputs

| Output | Status | Location |
| --- | --- | --- |
| Baseline status note | Complete | This file |
| Instruction scope register | Complete | This file |
| Build entry point register | Complete | This file |
| Initial risk boundary note | Complete | This file |
| Explicit no-source-reorganization decision | Complete | This file |

## Worktree Baseline

`git status --short` returned no output before Phase 0 edits. There were no pre-existing changes to classify.

| Path | Status | Owner | Action |
| --- | --- | --- | --- |
| None | Clean | Not applicable | Proceed with documentation-only Phase 0 outputs |

Current branch is `SAR`.

Recent commits:

| Commit | Subject |
| --- | --- |
| `86659bba` | First commit of the Systems Audit and Reorganization branch. |
| `b52f9c54` | docs: complete development history coverage |
| `721c8591` | docs: add development chronicle and preserve news archive |
| `bdd893f5` | Update .gitignore |
| `5b5936fe` | Update simulation state |

## Instruction Scope Register

Root `AGENTS.md` applies to the full repository unless a deeper `AGENTS.md` applies to a target path. No deeper instruction file applies to `docs/codebase-audit/`, so root instructions govern the Phase 0 audit artifacts.

| Instruction File | Applies To | Notes |
| --- | --- | --- |
| `AGENTS.md` | Repository root | Governs `docs/codebase-audit/` and this Phase 0 batch. |
| `DioXMLSpawn/AGENTS.md` | `DioXMLSpawn/` | Re-check before touching this subtree. |
| `docs/CCWM/AGENTS.md` | `docs/CCWM/` | Does not apply to `docs/codebase-audit/`. |
| `Data/Scripts/Custom/AIOverhaul/AGENTS.md` | `Data/Scripts/Custom/AIOverhaul/` | Re-check before touching AI Overhaul files. |
| Nested Gump `AGENTS.md` files | Listed by `rg --files -g AGENTS.md` | Re-check before modifying any matching Gump subtree. |

Nested instruction files found during Phase 0:

```text
AGENTS.md
DioXMLSpawn\AGENTS.md
docs\CCWM\AGENTS.md
Data\Scripts\Magic\Syth\Gumps\AGENTS.md
Data\Scripts\Items\Boats\Gumps\AGENTS.md
Data\Scripts\Items\Books\BulletinBoards\Gumps\AGENTS.md
Data\Scripts\Custom\XMLSpawner\XmlUtils\Gumps\AGENTS.md
Data\Scripts\Custom\XMLSpawner\XmlQuest\Gumps\AGENTS.md
Data\Scripts\Items\Houses\Remodeling\Gumps\AGENTS.md
Data\Scripts\Custom\XMLSpawner\XmlPropsGumps\AGENTS.md
Data\Scripts\Items\Gifts\Gumps\AGENTS.md
Data\Scripts\Items\Magical\God\Gumps\AGENTS.md
Data\Scripts\Magic\Research\Gumps\AGENTS.md
Data\Scripts\System\Gumps\AGENTS.md
Data\Scripts\Items\Houses\Monopoly\Gumps\AGENTS.md
Data\Scripts\Items\Houses\Monopoly\Gumps\Error Reporting\Gumps\AGENTS.md
Data\Scripts\Magic\Mystic\Gumps\AGENTS.md
Data\Scripts\System\Commands\Gumps\AGENTS.md
Data\Scripts\System\Chat\Gumps\AGENTS.md
Data\Scripts\Items\Magical\Gifts\Gumps\AGENTS.md
Data\Scripts\Items\Special\House Raffle\Gumps\AGENTS.md
Data\Scripts\Custom\XMLSpawner\XmlAttach\Gumps\AGENTS.md
Data\Scripts\Custom\XMLSpawner\Gumps\AGENTS.md
Data\Scripts\Magic\Jedi\Gumps\AGENTS.md
Data\Scripts\Magic\Holy Man\Gumps\AGENTS.md
Data\Scripts\Magic\Elementalism\Gumps\AGENTS.md
Data\Scripts\Quests\Museum\Gumps\AGENTS.md
Data\Scripts\Quests\Epic\Gumps\AGENTS.md
Data\Scripts\Magic\Death Knight\Gumps\AGENTS.md
Data\Scripts\Custom\PvPConsent\Gumps\AGENTS.md
Data\Scripts\Custom\Vhaerun's CRL Homestead System [2.0]\Vhaerun's CRL Crops\Gumps\AGENTS.md
Data\Scripts\Custom\AIOverhaul\AGENTS.md
Data\Scripts\Magic\Bard\Gumps\AGENTS.md
Data\Scripts\Custom\OzThothStaticGump\AGENTS.md
Data\Scripts\Custom\Government System\Gumps\AGENTS.md
Data\Scripts\Custom\Vhaerun's CRL Homestead System [2.0]\Vhaerun's CRL Cooking\Package Systems\HungerGump\AGENTS.md
Data\Scripts\Custom\Invasion System\Gump\AGENTS.md
Data\Scripts\Items\Misc\Games\LiarsDice\Gumps\AGENTS.md
Data\Scripts\Custom\Vhaerun's CRL Homestead System [2.0]\Dracana's Winecrafting [2.0]\Gumps\AGENTS.md
```

## Build Entry Point Register

`where.exe msbuild` did not find MSBuild on `PATH`. Visual Studio discovery found MSBuild at:

```text
C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe
```

No build was run in Phase 0 because this batch only changes audit documentation.

| Project | Role | Framework | Platform | Output | Notes |
| --- | --- | --- | --- | --- | --- |
| `ConficturaUO.sln` | Maintained solution entry point | Visual Studio solution format 12.00 / VS 17 | Debug/Release, Any CPU/x86 mappings | Builds `Server` and `Scripts` except `Debug|x86` omits script build | `Scripts` depends on `Server`. |
| `Data/System/Source/Server.csproj` | Core executable | .NET Framework v4.8 | x86 | `ConficturaServer` | Debug output path is repository root; Release output path is `Data/System/Source/bin/Release/`. |
| `Data/Scripts/Scripts.csproj` | Script library and explicit script compile list | .NET Framework v4.8 | AnyCPU | `ClassLibrary` | Debug output path is `Data/Scripts/`; Release output path is `Data/Scripts/bin/Release/`; includes project reference to `..\System\Source\Server.csproj`. |

Expected verification commands:

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Release /p:Platform="Any CPU"
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Release /p:Platform=x86
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU
```

## Source Root Boundary

Later audit phases must include these roots and not treat `Data/Scripts/Custom` as the entire shard:

| Root | Phase 0 Boundary Note |
| --- | --- |
| `Data/Scripts/Custom` | Custom systems, imported packages, staff tools, and gameplay layers. |
| `Data/Scripts/Items` | Serialization-heavy item content and framework-facing items. |
| `Data/Scripts/Magic` | Spell framework, spell schools, gumps, and registry risks. |
| `Data/Scripts/Mobiles` | Serialization-heavy mobile content and AI behavior. |
| `Data/Scripts/Quests` | Quest entry points, gumps, rewards, and persistence surfaces. |
| `Data/Scripts/System` | Startup, commands, global hooks, packet handlers, and framework wiring. |
| `Data/Scripts/Trades` | Crafting, economy, harvest, and pooled enumerable risks. |
| `Data/System/Source` | Core server framework and maintained server executable. |
| `docs` | Audit plans, wiki pages, source traces, and truth tables. |
| XML/config/data files read or written by scripts | Data-driven runtime behavior must be traced with source usage. |

## Risk Boundary

The following areas require extra proof before edits, moves, or reorganization:

| Area | Risk | Required Proof Before Change |
| --- | --- | --- |
| Serialized `Item` and `Mobile` types | World save compatibility | Serialization register, write/read order review, version policy, and build. |
| `PlayerMobile` fields | Cross-system state coupling | Dependency graph and focused source review. |
| Global event hooks | Process-wide runtime behavior | Runtime hook map and owner/guard review. |
| Packet handlers | Network entry points | Packet register, access/state/malformed input review, and build. |
| `Scripts.csproj` compile entries | Build truth and silent exclusions | Project truth parser and narrow scripts build. |
| XMLSpawner attachments | Shared service persistence and runtime hooks | System card, runtime hook map, serialization review, XML/config review. |
| Spell registry and magic initialization | High-ID spell family registration risk | Registry inventory and dependency graph. |
| Region and Notoriety policy | Combat and travel policy conflicts | Hook/override map and system conflict matrix. |
| Staff event tools | High-impact staff-only side effects | Command access review and runtime hook map. |
| Legacy compatibility files | Old save or command behavior | Legacy risk track and source-traced backlog item before removal. |
| Gump response paths | Stale targets, bounds, and access checks | Gump response register and guard review. |
| Pooled enumerable ownership | Resource leaks and cleanup hazards | Range-scan inventory and ownership review. |

## No Reorganization Decision

Phase 0 performs no source reorganization. No source files, project files, serialized types, namespaces, fields, hooks, or runtime behavior were moved or changed in this phase.

## Exit Criteria Review

| Question | Answer |
| --- | --- |
| What exact repository state did the audit start from? | Branch `SAR`, HEAD `86659bba`, clean worktree before Phase 0 edits. |
| Which instructions govern edited paths? | Root `AGENTS.md` governs `docs/codebase-audit/`; no deeper instruction file applies there. |
| Which projects and build commands matter? | `ConficturaUO.sln`, `Server.csproj`, and `Scripts.csproj`; MSBuild path and narrow/full commands recorded above. |
| Which roots are in scope? | Cross-tree source roots, `Data/System/Source`, `docs`, and script-used XML/config/data files recorded above. |
| Which systems require extra caution before edits? | Save surfaces, project truth, XMLSpawner, spell registry, PvP Consent, Government, Homestead, Items, Mobiles, System hooks, gumps, packet handlers, and pooled enumerables. |

Phase 0 status: `Complete`, pending focused documentation commit.
