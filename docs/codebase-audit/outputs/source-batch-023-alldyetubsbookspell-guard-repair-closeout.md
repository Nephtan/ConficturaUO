# SOURCE-BATCH-023 Closeout: AllDyeTubsBookSpell Guard Repair

Reviewed at: 2026-06-16T19:35:56.9822424-05:00

## Summary

`SOURCE-BATCH-023` implemented `SB022-CAND-002`, a non-gated `AllDyeTubsBookSpell` guard repair in `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookSpell.cs`.

The batch adds stale/null safety before mobile, source tub, target item, or charge state is evaluated. It preserves world-use behavior and does not change the current 100 gold cost ordering, dye eligibility, charge behavior, persistence, layout, or gated systems.

## Source Changes

- `OnDoubleClick(Mobile from)`, `DoPack(Mobile from)`, and `DoOut(Mobile from)` now return for null/deleted mobiles or deleted tubs before dereferencing state.
- `OnDoubleClick(Mobile from)` now checks `from.Backpack != null` before testing whether the tub is in the backpack, preserving world-use through `DoOut`.
- `AllDyeTubsBookSpellTarget.OnTarget(Mobile from, object targeted)` now returns for null/deleted mobiles, null/deleted source tubs, and deleted target items.

## Preserved Behavior

- `AllowPack` behavior.
- Target range.
- `Spellbook` eligibility.
- Existing 100 gold cost behavior and ordering.
- Charged tub deletion/charge decrement behavior.
- Hue assignment.
- Sound `0x23F`.
- Messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookSpell.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookSpell.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source tub, and deleted-target guards.
- Targeted source scan confirmed `AllowPack`, target range, `Spellbook`, `ConsumeTotal(typeof(Gold), 100)`, hue assignment, charged deletion/decrement, sound `0x23F`, and serialization markers remain present.
- POST-BATCH-Y gate scan returned `0`.
- Active overlay scan returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-023` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-024+` remains pending the next concrete non-gated source target.
