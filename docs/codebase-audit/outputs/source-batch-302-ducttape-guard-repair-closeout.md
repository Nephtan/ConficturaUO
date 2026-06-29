# SOURCE-BATCH-302 DuctTape Guard Repair Closeout

## Summary

`SOURCE-BATCH-302` implemented `SB302-CAND-001` in `Data/Scripts/Items/Technology/DuctTape.cs`.

`DuctTape.OnDoubleClick(Mobile m)` now returns safely for null/deleted mobiles or deleted tape before range checks or target assignment. `ConsumeCharge(DuctTape tape, Mobile from)` now returns safely for null/deleted tape or mobiles before consumption, revealing, or sound. `RepairTarget.OnTarget(Mobile from, object targeted)` now returns safely for stale mobile/tape state and treats deleted target armor/weapons or missing backpacks through the existing backpack-repair failure message before repair mutation.

## Preserved Behavior

- One-tile source range check.
- Prompt and failure/success messages.
- `BaseArmor` and `BaseWeapon` eligibility.
- Target item backpack requirement.
- Full-repair check.
- `MaxHitPoints` decrement and `HitPoints` assignment repair behavior.
- Sound `0x3E4`.
- `RevealingAction`.
- Duct tape `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-tape/target-item/backpack guards and preserved range check, prompt, full-repair messages, success messages, repair math, `Consume()` behavior, sound, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, economy/reward, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Technology/DuctTape.cs` as a tracked source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
