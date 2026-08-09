# SOURCE-BATCH-330 BoatStain Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-330`
- Candidate: `SB330-CAND-001`
- Behavior: add stale/null/mobile/source-stain/backpack/deleted-target guards to the boat stain interaction path.
- System: `Items:Boats / BoatStain`
- File: `Data/Scripts/Items/Boats/BoatStain.cs`

## Allowed Source Change

Add guard-only checks to `BoatStain.OnDoubleClick(Mobile from)` and `DyeTarget.OnTarget(Mobile from, object targeted)`.

The guards may return early for null/deleted mobiles, deleted source stains, null source stain state, missing backpacks, source stains outside the backpack, or deleted target items before dereferencing those values. Missing backpacks should use the existing backpack failure message; deleted target items should use the existing invalid-target message.

## Must Stay Unchanged

- Target range `1`
- Localized backpack message `1060640`
- Target prompt
- Docked-ship-in-pack message
- Invalid-target message
- `BaseBoatDeed` and `BaseDockedBoat` eligibility
- Hue `0x5BE` assignment
- `RevealingAction`
- Sound `0x23E`
- Non-consuming source stain behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-330 BoatStain Guard Repair
```
