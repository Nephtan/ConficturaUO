# SOURCE-BATCH-350 ShardOfCowardice Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-350`
- Candidate: `SB350-CAND-001`
- System: `Quests:Shadowlords / ShardOfCowardice`
- File: `Data/Scripts/Quests/Shadowlords/ShardOfCowardice.cs`

## Behavior

Add stale/null/mobile/source-item guards to `ShardOfCowardice.OnDoubleClick(Mobile from)` before reading `from.Backpack` or sending the existing shard messages.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- Item ID `0x3155`
- `Name = "Shard of Cowardice"`
- `Weight = 1.0`
- `Hue = 0x491`
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You feel the cowardice emanating from this shard.`
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Balance/economy tuning
- Region/map behavior
- Reorganization state

## Ready Goal Shape

`/goal SOURCE-BATCH-350 ShardOfCowardice Guard Repair`
