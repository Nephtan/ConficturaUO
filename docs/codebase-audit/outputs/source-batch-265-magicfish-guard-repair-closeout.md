# SOURCE-BATCH-265 MagicFish Guard Repair Closeout

## Summary

`SOURCE-BATCH-265` implemented `SB265-CAND-001` in `Data/Scripts/Items/Trades/Resources/Fishing/MagicFish.cs`.

`BaseMagicFish.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source fish before backpack checking, race checks, stat buff application, hunger/healing/poison cure, effects, sounds, messaging, or deleting the fish. Missing backpacks use the existing pack-use failure path.

## Preserved Behavior

- Backpack requirement and localized failure `1042001`.
- BloodDrinker/BrainEater rejection message.
- `Apply` and stat buff behavior.
- `PeculiarFish` stamina behavior.
- Hunger increment, human animation, tasting-based healing, and poison cure.
- Fixed effect, sound `0x1E7`, and localized swallow message `501774`.
- Source fish `Delete()` semantics.
- Subclass hue repair behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the new stale/null/mobile/source-item/backpack guard and preserved pack failure, race rejection, stat buffs, hunger/healing/poison-cure behavior, effects, sounds, messages, `Delete()`, subclass hue repairs, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
