# SOURCE-BATCH-149 SunkenBag Guard Repair Closeout

## Summary

Implemented `SB144-CAND-006` from `source-batch-144-candidate-discovery.csv`.

`Data/Scripts/Items/Containers/SunkenBag.cs` now guards stale/null mobile and deleted source container state before reading `from.Luck`, changing delayed-fill state, or filling the container.

## Source Changes

- `SunkenBag.OnDragLift(Mobile from)` now returns `false` for null/deleted mobiles or deleted source containers.

## Preserved Behavior

- Random naming remains unchanged.
- Valid-state delayed-fill behavior remains unchanged.
- `Movable` assignment remains unchanged.
- `FillMeUpLevel = (int)(Weight - 11)` math remains unchanged.
- Valid-state reset to `Weight = 2.0` remains unchanged.
- `GetPlayerInfo.LuckyPlayer(from.Luck)` bonus behavior remains unchanged.
- `ContainerFunctions.FillTheContainer(FillMeUpLevel, this, from)` remains unchanged.
- Valid-state return remains `true`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Containers/SunkenBag.cs`: `0`.
- Active overlay rows for `Data/Scripts/Items/Containers/SunkenBag.cs`: `0`.

## Verification

- Targeted source scan confirmed the new mobile/source guard, invalid-state `false` return, preserved delayed-fill math, preserved weight reset, preserved luck bonus, preserved fill call, and preserved valid-state `true` return.
- Serializer diff scan showed no changes to `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read`.
- Forbidden-surface scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-149 is complete and ready to commit. `SOURCE-BATCH-150+` remains pending the next concrete non-gated source target from `source-batch-144-candidate-discovery.csv`.
