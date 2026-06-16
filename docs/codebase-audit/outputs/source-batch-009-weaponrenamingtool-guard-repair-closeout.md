# SOURCE-BATCH-009 Closeout: WeaponRenamingTool Guard Repair

Reviewed at: 2026-06-16T18:14:26.8607304-05:00

## Summary

`SOURCE-BATCH-009` implemented `SB007-CAND-003`, a non-gated `WeaponRenamingTool` interaction guard repair in `Data/Scripts/Items/Magical/WeaponRenamingTool.cs`.

The batch adds stale/null safety before mobile, `NetState`, tool, target weapon, relay info, or backpack state are dereferenced. It does not change reward usability, rename flow, text policy, persistence, layout, or gated systems.

## Source Changes

- `WeaponRenamingTool.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null/deleted or the tool is deleted.
- `WeaponRenamingTool.Find(Mobile from)` now returns null when the mobile is null/deleted before reading backpack state.
- `TargetWeapon.OnTarget(Mobile from, object targeted)` now returns when the mobile is null/deleted or the tool is null/deleted.
- `TargetWeapon.OnTarget(Mobile from, object targeted)` now avoids opening the gump when the target is null, deleted, or not a `BaseWeapon`, preserving the existing invalid-target message.
- `InternalGump.OnResponse(NetState state, RelayInfo info)` now returns when `NetState`, mobile, relay info, tool, or target weapon state is stale.
- `InternalGump.OnResponse` now uses the guarded mobile local for existing rename result messages.

## Preserved Behavior

- `RewardSystem.CheckIsUsableBy` behavior.
- `BaseWeapon` target eligibility.
- `InternalGump` text entry flow.
- 64-character truncation.
- Blank rename removal.
- Localized success, removal, cancel, and invalid-target messages.
- Tool `Delete()` behavior on nonblank rename.
- `IsRewardItem` serialization.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/WeaponRenamingTool.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/WeaponRenamingTool.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, tool, target, `NetState`, `RelayInfo`, and backpack guards.
- Targeted source scan confirmed `RewardSystem.CheckIsUsableBy`, `BaseWeapon` target handling, 64-character truncation, blank rename removal, tool deletion, and `IsRewardItem` serialization remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Magical/WeaponRenamingTool.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Magical/WeaponRenamingTool.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named gump-response guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-009` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-010+` remains pending the next concrete non-gated source target.
