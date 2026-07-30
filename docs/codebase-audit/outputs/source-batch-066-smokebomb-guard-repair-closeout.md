# SOURCE-BATCH-066 SmokeBomb Guard Repair Closeout

## Summary

SOURCE-BATCH-066 implemented the SmokeBomb guard repair in `Data/Scripts/Items/Trades/Ninjitsu/SmokeBomb.cs`.

`OnDoubleClick` now guards null/deleted mobiles and deleted/out-of-backpack source smoke bombs before backpack, skill, timing, mana, hiding, effect, or consume state is evaluated.

## Preservation

Ninjitsu threshold `50.0`, `NextSkillTime` wait behavior, mana cost `10`, `SkillHandlers.Hiding.CombatOverride` flow, `UseSkill(SkillName.Hiding)`, particle and sound effects, `Consume()` semantics, localization IDs, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Ninjitsu/SmokeBomb.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Trades/Ninjitsu/SmokeBomb.cs`: 0
- Gate crossed: none

## Verification

- Targeted source scan confirmed guards and preserved Ninjitsu behavior.
- Serializer diff scan showed no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-066 is closed as a committed non-gated source batch. `SOURCE-BATCH-067+` remains pending the next concrete non-gated source target from `source-batch-064-candidate-discovery.csv`.
