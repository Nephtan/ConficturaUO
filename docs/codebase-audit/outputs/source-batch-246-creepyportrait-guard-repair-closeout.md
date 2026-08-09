# SOURCE-BATCH-246 CreepyPortrait Guard Repair Closeout

## Summary

`SOURCE-BATCH-246` created fresh candidate discovery and implemented `SB246-CAND-001` in `Data/Scripts/Items/Special/Evil Home Decor Collection/CreepyPortrait.cs`.

`CreepyPortraitComponent.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before range and sound state is evaluated.

## Preserved Behavior

- Range 2 rule, random sound range `0x565`-`0x566`, unreachable localized overhead message `1019045`, `HandlesOnMovement`/`OnMovement` behavior, `Timer.DelayCall` Up/Down behavior, flipable art IDs, addon/deed behavior, label number, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Special/Evil Home Decor Collection/CreepyPortrait.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed. Guard and preserved behavior markers were found.
- Movement/timer preservation scan: passed. `HandlesOnMovement`, `OnMovement`, `Timer.DelayCall`, `Up`, and `Down` remain present.
- Serializer diff scan: passed. No `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff lines were present.
- Forbidden-surface diff scan: passed. No command, event hook, gump, timer, movement, packet handler, region, startup, project, XML/config/data, or reorganization diff lines were present.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated root build artifacts restoration: completed.

## Result

Verified and ready for focused commit.
