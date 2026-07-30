# SOURCE-BATCH-348 ShardOfHatred Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-348`
- Candidate: `SB348-CAND-001`
- System: `Quests:Shadowlords / ShardOfHatred`
- File: `Data/Scripts/Quests/Shadowlords/ShardOfHatred.cs`

## Behavior

Add stale/null/mobile/source-item guards to `ShardOfHatred.OnDoubleClick(Mobile from)` before reading `from.Backpack` or sending the existing shard messages.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- Item ID `0x3155`
- `Name = "Shard of Hatred"`
- `Weight = 1.0`
- `Hue = 0x48E`
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You feel the hate emanating from this shard.`
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Balance/economy tuning
- Region/map behavior
- Reorganization state

## Ready Goal Shape

`/goal SOURCE-BATCH-348 ShardOfHatred Guard Repair`
