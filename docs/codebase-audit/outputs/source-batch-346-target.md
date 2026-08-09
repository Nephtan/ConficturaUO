# SOURCE-BATCH-346 CandleOfLove Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-346`
- Candidate: `SB346-CAND-001`
- System: `Quests:Shadowlords / CandleOfLove`
- Source file: `Data/Scripts/Quests/Shadowlords/CandleOfLove.cs`
- Behavior: add stale/null/mobile/source-item guards to `CandleOfLove.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted.
- Use the existing backpack-use failure message when the candle item is deleted, the mobile has no backpack, or the candle is not in the backpack.

## Must Stay Unchanged

- Item ID `0x1C14`, `Name = "Candle of Love"`, `Weight = 1.0`, and `Light = LightType.Circle150`.
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You feel the loving warmth of the flame.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-346 CandleOfLove Guard Repair`
