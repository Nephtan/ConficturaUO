# SOURCE-BATCH-357 ObeliskOnCorpse Guard Repair Closeout

## Result

`SOURCE-BATCH-357` implemented `SB357-CAND-001`, a non-gated guard repair for `ObeliskOnCorpse`.

## Source Change

- File: `Data/Scripts/Quests/Pagan/ObeliskOnCorpse.cs`
- Added an early return when `from == null || from.Deleted || Deleted`.

## Preserved Behavior

Item metadata, `OnDragLift` delegation, `PlayerMobile` eligibility, Titan `StatCap` rejection behavior, duplicate `ObeliskTip` return behavior, `SetupObelisk` default fields and ownership assignment, `AddToBackpack` behavior, messages, sound `0x3D`, `LoggingFunctions.LogGeneric` call, source item `Delete` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Pagan/ObeliskOnCorpse.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Pagan/ObeliskOnCorpse.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed with one recommended row and no missing required fields.
- Targeted source scan: passed for stale/null mobile/source-item guard, `OnDragLift` delegation, `PlayerMobile` eligibility, Titan rejection, existing `ObeliskTip` scan, duplicate tip return, `SetupObelisk`, ownership/default field assignment, `AddToBackpack` calls, messages, sound `0x3D`, `LoggingFunctions.LogGeneric`, source item `Delete`, and serialization method presence.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed with no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed with no command, event hook, packet handler, timer, gump policy, region, serializer, namespace, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with `0` warnings and `0` errors.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: completed with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`.

## Commit

`SOURCE-BATCH-357` is ready to commit as `fix: guard ObeliskOnCorpse interactions`. `SOURCE-BATCH-358+` should run fresh candidate discovery after the commit.
