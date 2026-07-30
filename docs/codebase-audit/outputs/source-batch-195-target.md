# SOURCE-BATCH-195 ECrystalAltar Guard Repair

## Target

- Candidate: `SB195-CAND-001`
- Behavior: add stale/null/mobile/source-component guard to `ECrystalAltarComponent.OnDoubleClick(Mobile from)`.
- System: `Items:Gifts / CrystalDeeds / ECrystalAltar`
- File: `Data/Scripts/Items/Gifts/CrystalDeeds/ECrystalAltar.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately when `from == null`, `from.Deleted`, or the addon component is deleted before reading `from.InRange(...)`.

## Must Stay Unchanged

- Out-of-range localized overhead message `1019045`.
- Range distance `2`.
- Valid in-range no-op behavior.
- Component item IDs and addon layout.
- Deed item ID, hue, weight, blessed loot type, and label number.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted guard/range/message scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
