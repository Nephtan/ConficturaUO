# SOURCE-BATCH-351 HighSeasRelic Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-351`
- Candidate: `SB351-CAND-001`
- System: `Items:Trades:Fishing / HighSeasRelic`
- File: `Data/Scripts/Items/Trades/Fishing/HighSeasRelic.cs`

## Behavior

Add stale/null/mobile/source-item guards to `HighSeasRelic.OnDoubleClick(Mobile from)` before reading `from.Backpack` or flipping the relic `ItemID`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- Generated relic names, item IDs, hue, weight, value, and origin behavior
- `RelicGoldValue`, `RelicFlipID1`, `RelicFlipID2`, and `RelicOrigin`
- Existing identification guidance message: `This can be identified to determine its value.`
- Existing backpack-use failure message: `This must be in your backpack to flip.`
- Existing `RelicFlipID1`/`RelicFlipID2` item-ID toggle behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Balance/economy tuning
- Region/map behavior
- Reorganization state

## Ready Goal Shape

`/goal SOURCE-BATCH-351 HighSeasRelic Guard Repair`
