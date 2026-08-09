# SOURCE-BATCH-351 HighSeasRelic Guard Repair Closeout

## Summary

`SOURCE-BATCH-351` implemented `SB351-CAND-001`, a non-gated guard repair for `HighSeasRelic`.

## Source Change

- File: `Data/Scripts/Items/Trades/Fishing/HighSeasRelic.cs`
- `HighSeasRelic.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles before reading `from.Backpack`.
- Deleted source relics, missing backpacks, and relics outside the backpack now use the existing identification/backpack failure messages before the existing flip path.

## Preserved Behavior

- Generated relic names, item IDs, hue, weight, value, and origin behavior.
- `RelicGoldValue`, `RelicFlipID1`, `RelicFlipID2`, and `RelicOrigin`.
- Existing identification guidance message: `This can be identified to determine its value.`
- Existing backpack-use failure message: `This must be in your backpack to flip.`
- Existing `RelicFlipID1`/`RelicFlipID2` item-ID toggle behavior.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Fishing/HighSeasRelic.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Trades/Fishing/HighSeasRelic.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms the new guard and preserved constructor/generation, messages, flip behavior, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-351` was committed as `7eaca282` with `fix: guard HighSeasRelic interactions`. `SOURCE-BATCH-352+` should run fresh candidate discovery next.
