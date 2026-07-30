# SOURCE-BATCH-069 MagicSkeltonsKey Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-069`
- Candidate: `SB068-CAND-002`
- Behavior: add stale/null/mobile/backpack/source-key guards to `MagicSkeltonsKey.OnDoubleClick(Mobile from)` and `UnlockTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Containers / MagicSkeltonsKey`
- Source file: `Data/Scripts/Items/Containers/MagicSkeltonsKey.cs`

## Fence Result

- POST-BATCH-Y gate hits for `MagicSkeltonsKey.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Containers/MagicSkeltonsKey.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source key is deleted in `OnDoubleClick`.
- Treat a missing backpack as the existing backpack-use failure before calling `IsChildOf(from.Backpack)`.
- Return immediately when `from` is null/deleted or the target-held source key is null/deleted in `UnlockTarget.OnTarget`.
- Treat a missing backpack as the existing backpack-use failure before calling `m_Key.IsChildOf(from.Backpack)`.

## Must Stay Unchanged

- Target range `1` and `CheckLOS = true`.
- Backpack message `1060640` and target prompt text.
- Existing magic-key wear roll behavior and all conditional `Consume()` calls.
- Self-target, house-door, `BookBox`, artifact, curse-item, spaceship door, dungeon door, card-slot, key-hole, secure-item, lock-level, lockpicker, sound, reveal, and key consumption behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-069 MagicSkeltonsKey Guard Repair

Implement SB068-CAND-002 from source-batch-068-candidate-discovery.csv. Add stale/null/mobile/backpack/source-key guards to Data/Scripts/Items/Containers/MagicSkeltonsKey.cs while preserving lock/unlock eligibility, messages, sounds, magic-key wear/consumption behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard MagicSkeltonsKey interactions.
```
