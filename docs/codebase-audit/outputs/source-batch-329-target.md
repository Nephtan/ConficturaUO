# SOURCE-BATCH-329 GrapplingHook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-329`
- Candidate: `SB329-CAND-001`
- Behavior: add stale/null/mobile/source-hook/backpack/map/deleted-target guards to the grappling hook interaction path.
- System: `Items:Boats / GrapplingHook`
- File: `Data/Scripts/Items/Boats/GrapplingHook.cs`

## Allowed Source Change

Add guard-only checks to `GrapplingHook.OnDoubleClick(Mobile from)` and `HookTarget.OnTarget(Mobile from, object targeted)`.

The guards may return early for null/deleted mobiles, deleted source hooks, missing backpacks, source hooks outside the backpack, null mobile maps, or deleted target creatures before dereferencing those values. Missing backpacks should use the existing backpack failure message; deleted target creatures should use the existing invalid-target message.

## Must Stay Unchanged

- Target range `20`
- Localized backpack message `1060640`
- Target prompt
- Invalid-target message
- `BaseCreature` target eligibility
- `BaseBoat.GetPirateShip` lookup
- Valid `loc.X`/`loc.Y` rule
- `DoTeleport` call
- Sound randomization
- `BaseCreature.TeleportPets`
- `MoveToWorld`
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-329 GrapplingHook Guard Repair
```
