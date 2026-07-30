# SOURCE-BATCH-014 Closeout: MagicHammer Guard Repair

Reviewed at: 2026-06-16T18:43:10.0827741-05:00

## Summary

`SOURCE-BATCH-014` implemented `SB012-CAND-003`, a non-gated `MagicHammer` interaction guard repair in `Data/Scripts/Items/Magical/MagicHammer.cs`.

The batch adds stale/null/backpack safety before mobile, source hammer, target ownership, artifact policy, and transform state are evaluated. It does not change item transforms, weapon replacement behavior, persistence, layout, or gated systems.

## Source Changes

- `MagicHammer.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null or deleted.
- `MagicHammer.OnDoubleClick(Mobile from)` now uses the existing pack failure when the hammer is deleted, the backpack is missing, or the hammer is outside the backpack.
- `WearTarget.OnTarget(Mobile from, object targeted)` now returns immediately when the mobile is null or deleted.
- `WearTarget.OnTarget(Mobile from, object targeted)` now uses the existing pack failure when the source hammer is null, deleted, or no longer in the backpack.
- `WearTarget.OnTarget(Mobile from, object targeted)` now treats deleted target item state as the existing cannot-use-this-hammer failure.

## Preserved Behavior

- Backpack-use message `1060640`.
- Target range.
- Ownership rule.
- `MyServerSettings.AlterArtifact` policy.
- Every item ID/name transform.
- Weapon replacement/deletion behavior.
- Sounds `0x541` and `0x233`.
- Success/failure messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/MagicHammer.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/MagicHammer.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source hammer, backpack, and deleted-target guards.
- Targeted source scan confirmed `iWear.RootParentEntity != from`, `MyServerSettings.AlterArtifact(iWear)`, sounds `0x541` and `0x233`, `weapon.Delete()`, and serialization markers remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Magical/MagicHammer.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Magical/MagicHammer.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-014` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-015+` remains pending the next concrete non-gated source target.
