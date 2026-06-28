# SOURCE-BATCH-091 HairOilPotion Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-091`
- Candidate: `SB087-CAND-005`
- Behavior: add stale/null/mobile/backpack/source-potion and gump-response guards to `HairOilPotion.Drink(Mobile from)` and `PotionGump.OnResponse(NetState sender, RelayInfo info)`.
- System: `Items:Potions / Special / HairOilPotion`
- Source file: `Data/Scripts/Items/Potions/Special/HairOilPotion.cs`

## Fence Result

- POST-BATCH-Y gate hits for `HairOilPotion.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/HairOilPotion.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source potion is deleted.
- Treat missing backpacks or source potions outside the backpack as the existing backpack-use failure using localized message `1060640`.
- Return from stale gump responses when sender/mobile, relay info, cached mobile, source potion, backpack, or source-potion containment is invalid.

## Must Stay Unchanged

- Race restriction message.
- Backpack localized message `1060640`.
- `PotionGump` layout, hair style IDs, and button IDs.
- Male/female hair option behavior.
- `ConsumeCharge`, `RevealingAction`, `BasePotion.PlayDrinkEffect`, sound, and consume behavior.
- Hair mutation and success message behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-091 HairOilPotion Guard Repair

Implement SB087-CAND-005 from source-batch-087-candidate-discovery.csv. Add stale/null/mobile/backpack/source-potion and gump-response guards to Data/Scripts/Items/Potions/Special/HairOilPotion.cs while preserving race restrictions, gump layout/button IDs, hair style IDs, hair mutation behavior, messages, drink effect, sound, consume behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean except the named gump-response guard, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard HairOilPotion interactions.
```
