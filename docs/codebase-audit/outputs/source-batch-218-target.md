# SOURCE-BATCH-218 HolidayBell Guard Repair

## Target

- Candidate: `SB217-CAND-002`
- Behavior: add stale/null/mobile/source-item guard to `HolidayBell.OnDoubleClick(Mobile from)`.
- System: `Items:Special / Holiday / HolidayBell`
- File: `Data/Scripts/Items/Special/Holiday/HolidayBell.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source bells before checking range or playing the bell sound.

## Must Stay Unchanged

- Range `2` behavior.
- Localized too-far message `500446`.
- Bell sound playback using `m_SoundID`.
- Maker, hue, sound-id, blessed loot, and default-name behavior.
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
