# SOURCE-BATCH-107 CaddelliteOre Guard Repair Closeout

## Summary

Implemented `SB107-CAND-001` from `source-batch-107-candidate-discovery.csv`.

`Data/Scripts/Items/Trades/Resources/Blacksmithing/CaddelliteOre.cs` now guards stale/null mobile, deleted source ore, missing backpack, and null region state before backpack, skill, region, conversion, animation, sound, or delete state is evaluated.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source ore.
- `OnDoubleClick(Mobile from)` now treats a missing backpack as the existing backpack-use failure.
- `OnDoubleClick(Mobile from)` now checks `from.Region != null` before evaluating the existing `the Great Dwarven Forge` rule.

## Preserved Behavior

- Backpack failure message remains `This must be in your backpack to use.`
- Master blacksmith requirement and failure message are unchanged.
- Valid Dwarven Forge conversion still plays sound `0x208`, runs animation `11, 5, 1, true, false, 0`, sends the success message, adds `new CaddelliteIngot(this.Amount)`, and deletes the source ore.
- Missing or nonmatching region state still follows the existing not-in-forge failure message.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Resources/Blacksmithing/CaddelliteOre.cs`: `0`.
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Blacksmithing/CaddelliteOre.cs`: `0`.

## Verification

- Candidate discovery CSV imported successfully with `Import-Csv`.
- Targeted source scan confirmed the new mobile/source/backpack/region guards.
- Targeted source scan confirmed valid-state conversion sound, animation, ingot creation, and source deletion remain present.
- Serializer diff scan showed no changes to `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read`.
- Forbidden-surface scan showed no command, event hook, gump, timer, packet handler, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-107 is complete and ready to commit. `SOURCE-BATCH-108+` remains pending the next concrete non-gated source target from `source-batch-107-candidate-discovery.csv`.
