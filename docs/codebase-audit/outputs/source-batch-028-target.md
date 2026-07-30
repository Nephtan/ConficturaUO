# SOURCE-BATCH-028 Target: DyeTub Guard Repair

Reviewed at: 2026-06-16T20:06:10-05:00

## Target

- Batch: `SOURCE-BATCH-028`
- Candidate: `SB028-CAND-001`
- Behavior: add stale/null/mobile/source-tub/target guards to `DyeTub.OnDoubleClick` and `DyeTub.InternalTarget.OnTarget` before mobile range checks, target assignment, source tub state, target item state, backpack containment, or hue mutation is evaluated.
- System: `Items:Misc / Dyes / DyeTub`
- File: `Data/Scripts/Items/Misc/Dyes/DyeTub.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/DyeTub.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/DyeTub.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- DyeTub serialization layout/versioning.
- `TargetMessage` and `FailMessage` behavior.
- Target range.
- `IDyable.Dye` behavior.
- `DyeTub.Redyable` behavior.
- Furniture locked-down/co-owner behavior.
- `Runebook` and `RecallRune` hue handling.
- `ILevelable` and `IGiftable` hue handling.
- Hollow book, ribbon tree, orb, addon deed, monster statuette, armor, and weapon eligibility.
- Hue assignment.
- Sound `0x23E`.
- Localized messages.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
