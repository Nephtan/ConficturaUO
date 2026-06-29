# SOURCE-BATCH-295 EvilSkull Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-295`
- Candidate: `SB295-CAND-001`
- System: `Items:Potions / Special / EvilSkull`
- Source file: `Data/Scripts/Items/Potions/Special/EvilSkull.cs`
- Behavior: add stale/null/mobile/source-item/backpack guards to `EvilSkull.OnDoubleClick(Mobile from)` before backpack checks, mana restore, sound, karma award, or item deletion.

## Allowed Source Changes

- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- Treat a missing backpack as the existing backpack-use failure with message `This must be in your backpack to use.`

## Must Stay Unchanged

- Backpack-use message text.
- Mana restore to `from.ManaMax`.
- Sound `0x1FA`.
- Karma award call `Misc.Titles.AwardKarma(from, -100, true)`.
- Mana-restored and crumble-only messages.
- Item `Delete` semantics.
- Construction metadata.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-295 EvilSkull Guard Repair`
