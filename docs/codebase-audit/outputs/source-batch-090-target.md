# SOURCE-BATCH-090 NecroSkinPotion Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-090`
- Candidate: `SB087-CAND-004`
- Behavior: add stale/null/mobile/backpack/source-potion guards to `NecroSkinPotion.OnDoubleClick(Mobile from)`.
- System: `Items:Potions / Special / NecroSkinPotion`
- Source file: `Data/Scripts/Items/Potions/Special/NecroSkinPotion.cs`

## Fence Result

- POST-BATCH-Y gate hits for `NecroSkinPotion.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/NecroSkinPotion.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source potion is deleted.
- Treat missing backpacks or source potions outside the backpack as the existing backpack-use failure using message `This must be in your backpack to use.`

## Must Stay Unchanged

- Race restriction message.
- Backpack failure message.
- Hue values `0x47E` and `0xB70`.
- `RecordSkinColor`, `RecordHairColor`, and `RecordBeardColor` restore behavior.
- Necromancy `>= 100` gate and thirst fallback.
- `this.Delete()` and `AddToBackpack(new Jar())` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-090 NecroSkinPotion Guard Repair

Implement SB087-CAND-004 from source-batch-087-candidate-discovery.csv. Add stale/null/mobile/backpack/source-potion guards to Data/Scripts/Items/Potions/Special/NecroSkinPotion.cs while preserving race restrictions, hue/hair restore, Necromancy threshold, thirst fallback, messages, Delete and Jar return behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard NecroSkinPotion interactions.
```
