# SOURCE-BATCH-079 BottleOfAcid Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-079`
- Candidate: `SB078-CAND-002`
- Behavior: add stale/null/mobile/backpack/source-acid and deleted-target guards to `BottleOfAcid.OnDoubleClick(Mobile from)` and `UnlockTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Potions / Special / BottleOfAcid`
- Source file: `Data/Scripts/Items/Potions/Special/BottleOfAcid.cs`

## Fence Result

- POST-BATCH-Y gate hits for `BottleOfAcid.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/BottleOfAcid.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source acid is deleted.
- Treat missing backpacks or source acid outside the backpack as the existing backpack-use failure.
- Treat deleted item targets as the existing invalid acid target path without mutating lock/trap/head state or consuming acid.

## Must Stay Unchanged

- Backpack message `1060640`.
- Target range `1` and `CheckLOS`.
- Lock/trap and dungeon-door eligibility.
- House door, BookBox, artifact, curse, and generic rejection behavior.
- Head skull conversion behavior.
- `Jar`/`Bottle` return behavior.
- Sound/reveal behavior.
- `m_Key.Consume()` behavior.
- Existing messages for valid and invalid live targets.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-079 BottleOfAcid Guard Repair

Implement SB078-CAND-002 from source-batch-078-candidate-discovery.csv. Add stale/null/mobile/backpack/source-acid and deleted-target guards to Data/Scripts/Items/Potions/Special/BottleOfAcid.cs while preserving lock/trap, door, head, return-container, message, consume, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard BottleOfAcid interactions.
```
