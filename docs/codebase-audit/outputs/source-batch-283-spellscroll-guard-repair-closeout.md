# SOURCE-BATCH-283 SpellScroll Guard Repair Closeout

## Summary

`SOURCE-BATCH-283` implemented `SB283-CAND-001` in `Data/Scripts/Magic/Magery/Scrolls/SpellScroll.cs`.

`SpellScroll.GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)` now returns immediately for null/deleted mobiles, deleted scrolls, or null context-menu lists before base context-menu dispatch or AddToSpellbook entry checks. `SpellScroll.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted scrolls before design-context checks, backpack checks, spell construction, or casting.

## Preserved Behavior

- Context menu base dispatch and AddToSpellbook eligibility for valid mobiles.
- `Multis.DesignContext.Check(from)` customization guard.
- Backpack-use requirement and localized message `1042001`.
- `SpellRegistry.NewSpell(m_SpellID, from, this)` and `spell.Cast()` behavior.
- Disabled-spell localized message `502345`.
- `ICommodity` behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-item/list guards and preserved context menu base dispatch, AddToSpellbook entry, design-context check, backpack-use message, `SpellRegistry.NewSpell`, `spell.Cast`, disabled-spell message, `ICommodity` members, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Magic/Magery/Scrolls/SpellScroll.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
