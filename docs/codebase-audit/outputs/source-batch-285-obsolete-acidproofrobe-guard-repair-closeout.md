# SOURCE-BATCH-285 Obsolete AcidProofRobe Guard Repair Closeout

## Summary

`SOURCE-BATCH-285` implemented `SB285-CAND-001` in `Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AcidProofRobe.cs`.

Legacy `AcidProofRobe.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted robes before cooldown math, parent checks, bottle consumption, acid creation, or messages, and missing backpacks now use the existing empty-bottle failure message.

## Preserved Behavior

- Legacy runtime-visible file path and `AcidProofRobe` type identity.
- Worn-robe requirement.
- 180-minute cooldown and wait message.
- Empty-bottle failure message.
- `Bottle` consumption, `BottleOfAcid` creation, sound `0x240`, success message, and `TimeUsed` update.
- Name properties.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file overlay evidence: `RB-00865=FalsePositive`; `RB-05756/RB-06080=IntentionalLegacy`; all remain nonblocking because serialization, type identity, and layout are untouched.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-item/backpack guards and preserved worn-robe requirement, cooldown math, wait message, empty-bottle failure, `Bottle` consumption, `BottleOfAcid` creation, sound, success message, `TimeUsed` update, name properties, legacy type identity, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows; resolved `FalsePositive` and `IntentionalLegacy` overlay rows remain nonblocking.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AcidProofRobe.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
