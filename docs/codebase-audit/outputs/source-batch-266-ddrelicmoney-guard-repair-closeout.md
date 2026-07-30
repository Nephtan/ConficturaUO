# SOURCE-BATCH-266 DDRelicMoney Guard Repair Closeout

## Summary

`SOURCE-BATCH-266` implemented `SB266-CAND-001` in `Data/Scripts/Items/Relics/DDRelicMoney.cs`.

The six DDRelicMoney currency `OnDoubleClick(Mobile from)` paths now return immediately for null/deleted mobiles or deleted source items before bank-box lookup, conversion, `AddToBackpack`, messages, or deleting the source item.

## Preserved Behavior

- Bank-box-only eligibility and localized failure `1047026`.
- `DDCopper` exchange rate `10` and change return behavior.
- `DDSilver` exchange rate `5` and change return behavior.
- `DDJewels` conversion to `Amount * 2` gold.
- `DDXormite` conversion to `Amount * 3` gold.
- `DDGemstones` conversion to `Amount * 2` gold.
- `DDGoldNuggets` conversion to `Amount` gold.
- `AddToBackpack` reward destination.
- Source item `Delete()` semantics.
- Constructor metadata, serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file documentation row remains `Documented`; this batch did not change documentation policy or broad relic/economy behavior.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed six stale/null/mobile/source-item guards and preserved bank lookups, localized failure message, exchange-rate markers, change returns, gold creation, delete calls, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `Documented` row remains nonblocking because source behavior and documentation policy were untouched.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
