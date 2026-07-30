# SOURCE-BATCH-067 EggBomb Guard Repair Closeout

## Summary

SOURCE-BATCH-067 implemented the EggBomb guard repair in `Data/Scripts/Items/Trades/Ninjitsu/EggBomb.cs`.

`OnDoubleClick` now guards null/deleted mobiles and deleted/out-of-backpack source egg bombs before backpack, skill, timing, mana, hiding, effect, or consume state is evaluated.

## Preservation

Ninjitsu threshold `50.0`, `NextSkillTime` wait behavior, mana cost `10`, `SkillHandlers.Hiding.CombatOverride` flow, `UseSkill(SkillName.Hiding)`, particle and sound effects, `Consume()` semantics, localization IDs, `ItemID == 0x2809` deserialize compatibility, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Ninjitsu/EggBomb.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Trades/Ninjitsu/EggBomb.cs`: 0
- Gate crossed: none

## Verification

- Targeted source scan confirmed guards and preserved Ninjitsu behavior.
- Serializer diff scan showed no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- `Data/System/Source/Server.csproj` Debug/x86 build was unavailable: the required sandbox escalation was rejected because the Codex session hit its usage limit.
- Runtime compile-only verification was unavailable with the restored tracked server executable: `.\ConficturaServer.exe -compileonly -nocache` was interpreted as `-nocache`, hit an existing `obj/Debug/.NETFramework,Version=v4.8.AssemblyAttributes.cs` duplicate `TargetFrameworkAttribute` script compile error, and failed at the interactive retry prompt.

## Result

SOURCE-BATCH-067 is closed as a committed non-gated source batch with the build/runtime verification limitation recorded. The `source-batch-064-candidate-discovery.csv` queue is exhausted, and `SOURCE-BATCH-068+` requires a new candidate discovery pass once build verification is available again.
