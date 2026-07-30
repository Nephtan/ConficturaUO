# SOURCE-BATCH-275 MaterialLiquifier Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-275`
- Candidate: `SB275-CAND-001`
- System: `Items:Technology / MaterialLiquifier`
- Source file: `Data/Scripts/Items/Technology/MaterialLiquifier.cs`
- Behavior: add stale/null/mobile/item/backpack/gump guards to `MaterialLiquifier.OnDoubleClick`, `OnDragDrop`, `GetColor`, and `MaterialLiquifierGump.OnResponse` before backpack checks, dropped-item handling, bottle lookup, material dye creation, charge consumption, or gump sound response.

## Allowed Source Change

- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, use the existing backpack-use failure when the mobile has no backpack or the liquifier is no longer in the backpack.
- In `OnDragDrop(Mobile from, Item dropped)`, return `false` for null/deleted mobiles, deleted source liquifier, or null/deleted dropped items.
- In `OnDragDrop(Mobile from, Item dropped)`, treat a missing backpack like the existing no-bottle destruction path.
- In `GetColor(Item item, Mobile from)`, return `false` for null/deleted items, null/deleted mobiles, or missing backpacks.
- In `GetColor(Item item, Mobile from)`, return `false` if the bottle disappears before the successful dye creation path consumes it.
- In `MaterialLiquifierGump.OnResponse(NetState state, RelayInfo info)`, return immediately for null `NetState` or null/deleted mobiles.

## Must Stay Unchanged

- Backpack-use failure `1060640`, gump open sound `0x54D`, drag-drop sound `0x55B`, dye success sound `0x23E`, and `RevealingAction`.
- Destroyed-item messages, `SpaceDyes` rejection, material-name matching, and material color/name mapping.
- Bottle consumption, `SpaceDyes` vial creation, vial `Name`, `Hue`, and `vialHue` assignment.
- Charge consumption, out-of-charges message `1019073`, source liquifier delete behavior, and dropped item delete semantics.
- Gump layout/text/sound response.
- `ItemCharges` serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, material/reward policy, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit rows remain `Documented`, `FalsePositive`, and `ReviewedNoChange`; they do not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-275 MaterialLiquifier Guard Repair`
