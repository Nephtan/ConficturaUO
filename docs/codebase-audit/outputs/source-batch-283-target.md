# SOURCE-BATCH-283 SpellScroll Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-283`
- Candidate: `SB283-CAND-001`
- System: `Magic:Magery / SpellScroll`
- Source file: `Data/Scripts/Magic/Magery/Scrolls/SpellScroll.cs`
- Behavior: add stale/null/mobile/source-item/list guards to `SpellScroll.GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)` and `OnDoubleClick(Mobile from)` before context-menu population, design-context checks, backpack checks, spell construction, or casting.

## Allowed Source Change

- In `GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)`, return immediately when `from == null || from.Deleted || Deleted || list == null`.
- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Context menu base dispatch and AddToSpellbook eligibility for valid mobiles.
- `Multis.DesignContext.Check(from)` customization guard.
- Backpack-use requirement and localized message `1042001`.
- `SpellRegistry.NewSpell(m_SpellID, from, this)` and `spell.Cast()` behavior.
- Disabled-spell localized message `502345`.
- `ICommodity` behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-283 SpellScroll Guard Repair`
