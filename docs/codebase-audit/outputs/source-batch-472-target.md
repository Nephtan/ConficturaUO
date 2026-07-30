# SOURCE-BATCH-472 Spellbook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-472`
- Candidate: `SB472-CAND-001`
- Behavior: add stale/null mobile and deleted source spellbook guards to `Spellbook.OnDoubleClick(Mobile from)`.
- System: `Magic:Magery / Spellbook`
- File: `Data/Scripts/Magic/Magery/Spellbook.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- `Spellbook` item identity
- `SpellbookType` enum behavior
- content bitmask and book count behavior
- parent/backpack open eligibility
- `DisplayTo` packet behavior
- localized message `500207`
- `OpenSpellbookRequest` and `CastSpellRequest` event behavior
- `AllSpells` command access and behavior
- crafting/slayer attributes
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-472 Spellbook Guard Repair`
