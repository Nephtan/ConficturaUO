# SOURCE-BATCH-292 ShipwreckedItem Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-292`
- Candidate: `SB292-CAND-001`
- System: `Items:Trades / Fishing / ShipwreckedItem`
- Source file: `Data/Scripts/Items/Trades/Fishing/Misc/ShipwreckedItem.cs`
- Behavior: add stale/null/mobile/source-item/dye-tub guards to `ShipwreckedItem.OnSingleClick(Mobile from)` and `ShipwreckedItem.Dye(Mobile from, DyeTub sender)` before label sends, hue assignment, or failure-message sends.

## Allowed Source Changes

- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `Dye(Mobile from, DyeTub sender)`, return `false` when `from == null || from.Deleted || sender == null || sender.Deleted || Deleted`.

## Must Stay Unchanged

- `LabelTo` clilocs and shipwreck label format.
- `AddNameProperties` clilocs and `ShipName` display.
- `ShipName` persistence.
- Eligible armor ItemID dye range.
- Successful dye behavior: `Hue = sender.DyedHue` and `return true`.
- Ineligible-item failure message: `from.SendLocalizedMessage(sender.FailMessage)` and `return false`.
- `IShipwreckedItem` behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-292 ShipwreckedItem Guard Repair`
