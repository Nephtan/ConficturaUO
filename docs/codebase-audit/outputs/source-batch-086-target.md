# SOURCE-BATCH-086 PotionOfDexterity Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-086`
- Candidate: `SB084-CAND-003`
- Behavior: add stale/null/mobile/backpack/source-potion guards to `PotionOfDexterity.OnDoubleClick(Mobile from)`.
- System: `Items:Potions / Special / PotionOfDexterity`
- Source file: `Data/Scripts/Items/Potions/Special/PotionOfDexterity.cs`

## Fence Result

- POST-BATCH-Y gate hits for `PotionOfDexterity.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/PotionOfDexterity.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source potion is deleted.
- Treat missing backpacks or source potions outside the backpack as the existing backpack-use failure using localized message `1060640`.

## Must Stay Unchanged

- Localized message `1060640`.
- `StatCap > RawStatTotal` gate.
- `Utility.RandomMinMax` chance thresholds and `EnhancePotions(from)` contribution.
- `AvailPoints` behavior.
- `RawDex` increment behavior.
- Success and no-effect messages.
- `BasePotion.PlayDrinkEffect(from)`.
- `this.Consume()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-086 PotionOfDexterity Guard Repair

Implement SB084-CAND-003 from source-batch-084-candidate-discovery.csv. Add stale/null/mobile/backpack/source-potion guards to Data/Scripts/Items/Potions/Special/PotionOfDexterity.cs while preserving stat-cap checks, chance thresholds, RawDex mutation, messages, drink effect, Consume behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard PotionOfDexterity interactions.
```
