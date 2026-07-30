# SOURCE-BATCH-341 JokeBook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-341`
- Candidate: `SB341-CAND-001`
- System: `Quests:Jester / JokeBook`
- Source file: `Data/Scripts/Quests/Jester/JokeBook.cs`
- Behavior: add stale/null/mobile/source-item guard to `JokeBook.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted, or the joke book item is deleted.

## Must Stay Unchanged

- Item ID `0x1A98`, `Weight = 1.0`, generated owner-name prefix, and `Hue = 0xAFF`.
- `PlayerMobile`-only behavior.
- `Utility.RandomMinMax(0, 8)` joke selection.
- Sound `from.Female ? 801 : 1073` and all existing speech strings.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-341 JokeBook Guard Repair`
