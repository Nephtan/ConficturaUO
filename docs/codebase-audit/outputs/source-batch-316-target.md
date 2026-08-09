# SOURCE-BATCH-316 DDRelicCoins Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-316`
- Candidate: `SB316-CAND-001`
- Behavior: add stale/null/mobile/source-item/backpack guards to the relic coin flip path.
- System: `Items:Relics / DDRelicCoins`
- File: `Data/Scripts/Items/Relics/DDRelicCoins.cs`

## Allowed Source Change

Add guard-only checks to `DDRelicCoins.OnDoubleClick(Mobile from)`.

The guards may return early for null/deleted mobiles or a deleted source relic before dereferencing the mobile or the relic item. Missing backpacks should use the existing backpack failure path and messages.

## Must Stay Unchanged

- Identification guidance message
- Backpack-use failure message
- `RelicFlipID1`/`RelicFlipID2` item-id toggle behavior
- `RelicGoldValue` and flip-id persistence
- Constructor/name randomization
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-316 DDRelicCoins Guard Repair
```
