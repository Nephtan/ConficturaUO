# SOURCE-BATCH-230 Crystals Bank Conversion Guard Repair

## Target

- Candidate: `SB230-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `Crystals.OnDoubleClick(Mobile from)`.
- System: `Items:Gems / Crystals`
- File: `Data/Scripts/Items/Gems/Crystals.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted crystal items before bank-box lookup, bank-box containment check, deletion, gold calculation, or message sends.

## Must Stay Unchanged

- Bank-box-only eligibility.
- Crystal `Delete()` behavior.
- `Amount * 5` gold calculation.
- `from.AddToBackpack(new Gold(nGold))`.
- Localized bank-box failure message `1047026`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted `OnDoubleClick` guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
