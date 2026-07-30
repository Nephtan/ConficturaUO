# SOURCE-BATCH-262 VenomSack Guard Repair Closeout

## Summary

`SOURCE-BATCH-262` implemented `SB262-CAND-001` in `Data/Scripts/Items/Potions/Standard/Poison Potions/VenomSack.cs`.

`VenomSack.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source sacks before venom extraction, bottle consumption, poison application, messaging, or consuming the sack. Missing backpacks use the existing empty-bottle failure path at bottle consumption.

## Preserved Behavior

- Venom-name-to-skill mapping.
- `CheckSkill` calls and thresholds.
- Empty bottle consumption rule.
- Poison potion output mapping.
- Sounds `0x240` and `0x62D`.
- `Poison!` speech/message behavior.
- Poison self-application types.
- Source sack `Consume()` semantics.
- `AddNameProperties` text.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the new stale/null/mobile/source-item/backpack guard and preserved skill checks, empty bottle message, potion mapping, sounds, poison speech/message, poison application, `Consume()`, `AddNameProperties`, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
