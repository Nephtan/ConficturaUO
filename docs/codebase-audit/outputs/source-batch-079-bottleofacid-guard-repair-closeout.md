# SOURCE-BATCH-079 BottleOfAcid Guard Repair Closeout

## Summary

`SOURCE-BATCH-079` implemented `SB078-CAND-002` from `source-batch-078-candidate-discovery.csv`.

`Data/Scripts/Items/Potions/Special/BottleOfAcid.cs` now guards null/deleted mobiles, deleted source acid, missing backpacks, out-of-backpack source acid, and deleted item targets before lock/trap, door, head, return-container, or acid consumption state is evaluated.

## Preservation

The batch preserved:

- Backpack message `1060640`.
- Target range `1` and `CheckLOS`.
- Lock/trap and dungeon-door eligibility.
- House door, BookBox, artifact, curse, and generic rejection behavior.
- Head skull conversion behavior.
- `Jar`/`Bottle` return behavior.
- Sound/reveal behavior.
- `m_Key.Consume()` behavior.
- Existing messages for valid and invalid live targets.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Special/BottleOfAcid.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/BottleOfAcid.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-078-candidate-discovery.csv` still imports successfully and lists `SB078-CAND-002`.
- Targeted source scan confirmed the new mobile/source-acid/backpack/deleted-target guards and preserved lock/trap/head evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-079` is closed. `SOURCE-BATCH-080+` remains pending the next concrete non-gated source target from `source-batch-078-candidate-discovery.csv`.
