# SOURCE-BATCH-214 GemOfSeeing Guard Repair

## Target

- Candidate: `SB214-CAND-001`
- Behavior: add stale/null/mobile/source-item and stale target-callback guards to `GemOfSeeing`.
- System: `Items:Magical / Minor Artifacts / GemOfSeeing`
- File: `Data/Scripts/Items/Magical/Artifacts/Minor/GemOfSeeing.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or a deleted source gem before range checks, charge consumption, target assignment, or delete-on-empty behavior. Return immediately from the target callback when the callback mobile is null, deleted, or has no map.

## Must Stay Unchanged

- Range `3` use checks.
- Charge decrement timing.
- Localized messages `500819`, `502138`, `500814`, and `500817`.
- Target range `12`.
- Hidden mobile reveal rules.
- Hidden chest deletion behavior.
- No-hidden-result message.
- Empty-gem delete behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Candidate CSV import.
- Targeted `OnDoubleClick` and `OnTarget` guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
