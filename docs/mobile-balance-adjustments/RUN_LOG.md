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

## 2026-07-28 - Approved Implementation

Working directory: `D:\ConficturaUO`

### Approved Source Snapshot

The current staff-approved workbook was preserved without editing:

`docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-approved-2026-07-28.xlsx`

Snapshot facts:

- Size: 420,895 bytes.
- SHA-256: `BFD0435E02EA468B789EC5F60A83311D55020FA2EFA0F75B4B7EC178CD5E3BEA`.
- The July 15 and earlier July 28 snapshots remain unchanged.
- The only changes from the repaired submission were `Priority=High` for `MC-034` and `MC-035`.

Commit:

`0a7a8960 docs: approve mobile balance workbook`

### Source Implementation

Implemented and committed:

- 35 exact mobile profiles and 64 mobile skill changes.
- Version-1 serialization and version-0 profile migration in all 35 target mobiles.
- 33 corpse-drop hooks and no hooks for `FireIllusion` or `Lovecraftian`.
- 39 independent percentage rolls, including repeated rows and `PhoenixFeather` quantity 1-3.
- 23 new public item classes and 16 configured existing-item drops.
- 70 custom signed equip skill modifiers and three stock positive bonuses.
- 110 item bonuses through native item, AOS, weapon, armor, resistance, damage, and weight APIs.
- Legal census behavior for “Legendary Registry of Heroes.”

Commits:

- `5bba73b2 feat: implement mobile balance adjustments`
- `5333af58 fix: harden mobile balance runtime startup`

The runtime smoke found that public parameterized methods named `Initialize` or `Configure` collide with RunUO's zero-argument reflection hooks. The shared item metadata helper was renamed `ApplyMetadata`, and the implementation validator now rejects future parameterized public methods with either reserved name.

The documented Release x86 server command also exposed a pre-existing `Server.csproj` configuration defect: unsafe framework code was enabled only for Debug. `AllowUnsafeBlocks=true` was added to `Release|x86`.

### Workbook Completion

The connected Excel session was updated and saved through the Excel host API:

- `MC-034` and `MC-035` retain approved `High` priority.
- All 35 mobile rows and 39 item rows are `ImplementationStatus=Implemented`.
- All normalized item loot types are `Regular`.
- The 23 new item classes are `ExistingSourceClass` and `SourceVerified`.
- `Ref_ItemClasses` records those classes as source classes.
- Implementation-status dropdowns include `Implemented`.
- `Reference` keeps its fixed row-14 sections and lists the four additional skill aliases at `D48:E52`.

Final CSV command:

```powershell
python docs/mobile-balance-adjustments/tools/New-MobileBalanceWorkbook.py --implementation-complete --csv-only
```

Result:

- `MobileChanges`: 35
- `NewLootItems`: 39
- `LootAssignments`: 39
- `MobileSkillChanges`: 64
- `ItemSkillMods`: 73
- `ItemBonuses`: 110
- `review-issues.csv`: zero data rows

The workbook package contains 12 worksheet parts, zero formula cells, and zero spreadsheet error cells. The connected Excel session confirmed the final used ranges and rendered the changed `MobileChanges` and `NewLootItems` tables without lost formatting or unreadable data.

A full implementation-complete workbook was also generated to an ignored `output/` path and passed `Test-MobileBalanceWorkbook.ps1`. This caught and corrected generator-only drift in the fixed `Reference` layout and defined-name syntax before the final commit.

### Validators

Commands:

```powershell
pwsh -NoProfile -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceWorkbook.ps1
pwsh -NoProfile -File docs/mobile-balance-adjustments/tools/Test-MobileBalanceImplementation.ps1
```

Result:

- Workbook validation passed.
- Implementation parity passed for 35 profiles, 64 mobile skills, 39 loot assignments, 23 new item classes, 70 custom modifiers, three stock modifiers, 110 item bonuses, 35 migrations, and 33 loot hooks.
- All five new source files are included in `Data/Scripts/Scripts.csproj`.

### Build Verification

Commands:

```powershell
msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86
msbuild Data/System/Source/Server.csproj /p:Configuration=Release /p:Platform=x86
msbuild ConficturaUO.sln /p:Configuration=Release /p:Platform="Any CPU"
```

Result:

- Debug x86 server build passed.
- Release x86 server build passed after the Release unsafe-code setting was corrected.
- Release solution and `Scripts.csproj` project-hygiene build passed.
- Existing repository warnings remain, including the known `Scripts.csproj` MSIL-to-x86 reference warning; there were no errors.

### Isolated Runtime Verification

Disposable test shards were created under ignored `output/` paths with empty save directories. Production `Saves` was not read or modified.

Command:

```powershell
.\ConficturaServer.exe -service -nocache
```

Results:

- Forced runtime script compile completed and the isolated server reached `Console ready`.
- A temporary, uncommitted runtime verifier instantiated all 35 mobile profiles and all 39 factory items.
- It verified all 19 signed-mod items through equip, repeated apply, remove, and re-equip.
- It verified the three stock positive skill bonuses.
- It saved and reloaded all 19 signed-mod items, then verified delayed rehydration, removal, and re-equip after deserialization.
- A separate version-0 `FireIllusion` fixture was saved with stale damage and skills. Current source reloaded it as damage 75-120 with `Searching`, `Tactics`, `MagicResist`, `Magery`, and `Psychology` all at 125.
- Each isolated server process was terminated after its verification milestone.

### Deployment Constraint

Back up the world save before deployment. Once any of the 23 new item types enters a saved world, rollback must retain compatible type stubs or restore the pre-deployment save.
