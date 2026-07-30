# SOURCE-BATCH-082 ClothingBlessDeed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-082`
- Candidate: `SB082-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-deed/deleted-target guards to `ClothingBlessDeed.OnDoubleClick(Mobile from)` and `ClothingBlessTarget.OnTarget(Mobile from, object target)`.
- System: `Items:Deeds / ClothingBlessDeed`
- Source file: `Data/Scripts/Items/Deeds/ClothingBlessDeed.cs`

## Fence Result

- POST-BATCH-Y gate hits for `ClothingBlessDeed.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Deeds/ClothingBlessDeed.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source deed is deleted.
- Return immediately when the target callback has a null/deleted source deed, missing backpack, or source deed outside the user's backpack.
- Treat deleted item targets as the existing invalid-target path using localized message `500509`.
- Treat missing backpacks or source deeds outside the backpack in `OnDoubleClick` as the existing backpack-use failure using localized message `1042001`.

## Must Stay Unchanged

- Localized messages `1042001`, `1005018`, `1005019`, `1045113`, `1045114`, `500509`, and `1010026`.
- Base clothing and magic cloak/robe/boots eligibility.
- Arcane, already-blessed, non-regular loot type, insurance, and root-parent rejection rules.
- `LootType.Blessed` assignment.
- Source deed `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-082 ClothingBlessDeed Guard Repair

Implement SB082-CAND-001 from source-batch-082-candidate-discovery.csv. Add stale/null/mobile/backpack/source-deed/deleted-target guards to Data/Scripts/Items/Deeds/ClothingBlessDeed.cs while preserving blessing eligibility, messages, LootType assignment, deed Delete behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard ClothingBlessDeed interactions.
```
