# SOURCE-BATCH-260 WallTorch Guard Repair Closeout

## Summary

`SOURCE-BATCH-260` implemented `SB260-CAND-001` in `Data/Scripts/Items/Special/Heritage Items/WallTorch.cs`.

`WallTorchComponent.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source components before range checking, item-id toggling, sound playback, or reach-message paths.

## Preserved Behavior

- Range 2 reach rule.
- Localized reach failure `1019045`.
- Item-id toggle states `0x3D98`/`0x3D9B` and `0x3D94`/`0x3D97`.
- Sound `0x3BE`.
- `WallTorchAddon` and `WallTorchDeed` behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file save-compat rows `PBJ-0974`, `PBJ-0975`, and `PBJ-0976` remain `IntentionalLegacy`; this batch did not edit serialization.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the new stale/null/mobile/source-component guard and preserved range check, reach message, item-id toggles, sound, addon/deed classes, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `IntentionalLegacy` save-compat rows remain nonblocking because serialization was untouched.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
