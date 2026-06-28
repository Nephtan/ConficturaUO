# SOURCE-BATCH-218 HolidayBell Guard Repair Closeout

## Summary

`SOURCE-BATCH-218` implemented `SB217-CAND-002` in `Data/Scripts/Items/Special/Holiday/HolidayBell.cs`.

`HolidayBell.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before checking range or playing the configured bell sound.

## Preserved Behavior

- Range `2` behavior, localized too-far message `500446`, bell sound playback through `m_SoundID`, maker, hue, sound-id, blessed loot, default-name behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Special/Holiday/HolidayBell.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed; confirmed the new mobile/source guard and preserved range check, too-far message, sound playback, blessed loot, default name, and serialization methods.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache`: passed; runtime script compile completed successfully.
- `git diff --check`: passed with only expected CRLF working-copy warnings.
- Generated root build artifacts restoration: completed before staging.

## Result

Verified and ready for commit.
