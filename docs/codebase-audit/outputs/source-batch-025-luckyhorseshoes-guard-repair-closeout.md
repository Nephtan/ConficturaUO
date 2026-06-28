# SOURCE-BATCH-025 Closeout: LuckyHorseShoes Guard Repair

Reviewed at: 2026-06-16T20:06:00-05:00

## Summary

`SOURCE-BATCH-025` implemented `SB025-CAND-001`, a non-gated `LuckyHorseShoes` guard repair in `Data/Scripts/Items/Magical/LuckyHorseShoes.cs`.

The batch adds stale/null safety before mobile, source deed, backpack, target item, luck mutation, or deed deletion state is evaluated. It preserves existing target eligibility, luck math, ownership checks, messages, persistence, layout, and gated systems.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles before dereferencing state.
- `OnDoubleClick(Mobile from)` now treats deleted deed state, missing backpacks, or deeds outside the backpack as the existing localized pack failure.
- `LuckTarget.OnTarget(Mobile from, object target)` now returns for null/deleted mobiles before dereferencing state.
- `LuckTarget.OnTarget(Mobile from, object target)` now treats null/deleted/out-of-backpack source deed state as the existing localized pack failure.
- `LuckTarget.OnTarget(Mobile from, object target)` now treats deleted target item state as the existing invalid-target outcome before reading or mutating item attributes.

## Preserved Behavior

- `BaseWeapon`, `BaseClothing`, `BaseJewel`, `BaseArmor`, and `Spellbook` eligibility.
- RootParent ownership rule for target items.
- `+100` Luck increment.
- Luck cap at `1000`.
- Existing messages, including pack and invalid-target messages.
- Target range.
- Deed `Delete()` semantics.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/LuckyHorseShoes.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/LuckyHorseShoes.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source deed, backpack, and deleted-target guards.
- Targeted source scan confirmed `BaseWeapon`, `BaseClothing`, `BaseJewel`, `BaseArmor`, `Spellbook`, `Attributes.Luck`, cap `1000`, `m_Deed.Delete()`, `writer.Write`, and `reader.ReadInt` markers remain present.
- POST-BATCH-Y gate scan returned `0`.
- Active overlay scan returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-025` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-026+` remains pending the next concrete non-gated target from `source-batch-025-candidate-discovery.csv`.
