# SOURCE-BATCH-028 Closeout: DyeTub Guard Repair

Reviewed at: 2026-06-16T20:09:30-05:00

## Summary

`SOURCE-BATCH-028` implemented `SB028-CAND-001`, a non-gated `DyeTub` guard repair in `Data/Scripts/Items/Misc/Dyes/DyeTub.cs`.

The batch adds stale/null safety before mobile, source tub, target item, backpack, range, or hue mutation state is evaluated. It preserves existing dye eligibility, house/furniture rules, messages, persistence, layout, and gated systems.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles or deleted tub state before range checks and target assignment.
- `InternalTarget.OnTarget(Mobile from, object targeted)` now returns for null/deleted mobiles or null/deleted source tub state before target processing.
- `InternalTarget.OnTarget(Mobile from, object targeted)` now treats deleted target items as the existing `FailMessage` invalid-target outcome before reading target state.
- The furniture branch now checks for a non-null backpack before calling `item.IsChildOf(from.Backpack)`, while preserving the existing locked-down/co-owner house path.

## Preserved Behavior

- DyeTub serialization layout/versioning.
- `TargetMessage` and `FailMessage` behavior.
- Target range.
- `IDyable.Dye` behavior.
- `DyeTub.Redyable` behavior.
- Furniture locked-down/co-owner behavior.
- `Runebook` and `RecallRune` hue handling.
- `ILevelable` and `IGiftable` hue handling.
- Hollow book, ribbon tree, orb, addon deed, monster statuette, armor, and weapon eligibility.
- Hue assignment.
- Sound `0x23E`.
- Localized messages.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/DyeTub.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/DyeTub.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source tub, deleted-target, and backpack guards.
- Targeted source scan confirmed `TargetMessage`, `FailMessage`, `IDyable`, `Redyable`, `FurnitureAttribute.Check`, `BaseHouse.FindHouseAt`, `Runebook`, `RecallRune`, `ILevelable`, `IGiftable`, `MonsterStatuette`, `BaseArmor`, `BaseWeapon`, `PlaySound(0x23E)`, `writer.Write`, and `reader.ReadInt` markers remain present.
- POST-BATCH-Y gate scan returned `0`.
- Active overlay scan returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only the expected line-ending warning was reported by Git for the edited source file.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-028` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-029+` remains pending the next concrete non-gated source target from `source-batch-028-candidate-discovery.csv`.
