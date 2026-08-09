# SOURCE-BATCH-324 Bola Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-324`
- Candidate: `SB324-CAND-001`
- Behavior: add stale/null/mobile/source-bola/backpack/deleted-target guards to the bola interaction path.
- System: `Items:Misc / Bola`
- File: `Data/Scripts/Items/Misc/Bola.cs`

## Allowed Source Change

Add guard-only checks to `Bola.OnDoubleClick(Mobile from)` and `BolaTarget.OnTarget(Mobile from, object obj)`.

The guards may return early for null/deleted mobiles or deleted source bolas before dereferencing the mobile or source bola. Missing backpacks should use the existing bola pack-use failure message. Deleted mobile targets should use the existing invalid-target message.

## Must Stay Unchanged

- Pack-use failure message `1040019`
- Cooldown message `1049624`
- Already-used message `1049631`
- Hand, mount, and animal-form restrictions
- Target invalid/no-reason messages
- Target range `8` and `TargetFlags.Harmful`
- `Core.AOS` behavior
- `BeginAction` cooldown flow
- `CanBeHarmful` and `DoHarmful` behavior
- Bola `Consume()` behavior
- Animation and moving effect behavior
- `Timer.DelayCall` callbacks
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-324 Bola Guard Repair
```
