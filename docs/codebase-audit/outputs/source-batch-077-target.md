# SOURCE-BATCH-077 MysticalPearl Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-077`
- Candidate: `SB077-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-pearl guards to `MysticalPearl.OnDoubleClick(Mobile from)` and `PearlTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Gems / MysticalPearl`
- Source file: `Data/Scripts/Items/Gems/MysticalPearl.cs`

## Fence Result

- POST-BATCH-Y gate hits for `MysticalPearl.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Gems/MysticalPearl.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source pearl is deleted.
- Return immediately when the target callback has a null/deleted source pearl.
- Treat missing backpacks or source pearls outside the backpack as the existing backpack-use failure.

## Must Stay Unchanged

- Backpack message `1060640`.
- Tinkering threshold `90`.
- Target range `1`.
- Valid jewelry eligibility.
- Pearl jewelry name selection.
- `MorphingItem.MorphMyItem` and `MorphingTemplates.TemplatePearlJewelry("misc")`.
- `RevealingAction`, sound `0x242`, invalid-target messages, and `m_Pearl.Consume()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-077 MysticalPearl Guard Repair

Implement SB077-CAND-001 from source-batch-077-candidate-discovery.csv. Add stale/null/mobile/backpack/source-pearl guards to Data/Scripts/Items/Gems/MysticalPearl.cs while preserving jewelry eligibility, morphing names/templates, messages, reveal/sound behavior, pearl consumption, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard MysticalPearl interactions.
```
