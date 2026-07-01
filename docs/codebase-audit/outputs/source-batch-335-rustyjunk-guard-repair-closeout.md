# SOURCE-BATCH-335 RustyJunk Guard Repair Closeout

## Summary

`SOURCE-BATCH-335` implemented `SB335-CAND-001`, a non-gated guard repair for `RustyJunk`.

## Source Change

- File: `Data/Scripts/Items/Trades/Fishing/RustyJunk.cs`
- `RustyJunk.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles before backpack checks or target assignment.
- `OnDoubleClick` now uses the existing backpack-use failure message for deleted source rusty items, missing backpacks, or source rusty items outside the mobile backpack.
- `InternalTarget.OnTarget(Mobile from, object targeted)` now returns safely for null/deleted mobiles and uses the existing backpack-use failure message for null/deleted/stale source rusty items, missing backpacks, or source rusty items outside the mobile backpack before forge checks, mining skill reads, ingot conversion, sound behavior, or source rusty item deletion.

## Preserved Behavior

- Generated rusty item names, item IDs, hues, weights, and scrap iron property text.
- Backpack-use message: `This must be in your backpack to use.`
- Target prompt: `Select the forge to smelt this item.`
- Target range `2`.
- `DefBlacksmithy.IsForge(targeted)` requirement.
- `difficulty=50.0`, `minSkill=25.0`, and `maxSkill=75.0`.
- Mining skill threshold message and `CheckTargetSkill` behavior.
- `IronIngot(1)` creation, `ingot.Amount = weight`, and `AddToBackpack` behavior.
- Sound `0x208`.
- Success and failure messages.
- Source rusty item `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Fishing/RustyJunk.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Trades/Fishing/RustyJunk.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms mobile/source/backpack guards and preserved prompt, forge requirement, mining skill check, `CheckTargetSkill`, ingot conversion, sound, source deletion, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-335` is ready to commit as `fix: guard RustyJunk interactions`. `SOURCE-BATCH-336+` should run fresh candidate discovery before any further source edits.
