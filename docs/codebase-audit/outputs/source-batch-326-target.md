# SOURCE-BATCH-326 BaseMixture Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-326`
- Candidate: `SB326-CAND-001`
- Behavior: add stale/null/mobile/source-potion/backpack/target-point guards to the base mixture potion interaction path.
- System: `Items:Potions / Mixtures / BaseMixture`
- File: `Data/Scripts/Items/Potions/Mixtures/BaseMixture.cs`

## Allowed Source Change

Add guard-only checks to `BaseMixture.Drink(Mobile from)` and `BaseMixture.ThrowTarget.OnTarget(Mobile from, object targeted)`.

The guards may return early for null/deleted mobiles, deleted source potions, missing backpacks, null source potion state, internal-map source potions, null target points, or null mobile maps before dereferencing those values. Missing backpacks should use the existing backpack failure message.

## Must Stay Unchanged

- Localized backpack message `1060640`
- Commented `Region.AllowHarmful` policy block
- Follower-cap gate
- Target prompt and duplicate target guard
- `RevealingAction`
- Target range `12`
- Range, LOS, and action-state checks
- `SpellHelper.GetSurfaceTop`
- `AlchemicSlime.MakeSlime` behavior and arguments
- Potion `Consume()` behavior
- Empty `Jar` return
- Slime field persistence
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-326 BaseMixture Guard Repair
```
