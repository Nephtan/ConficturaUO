# SOURCE-BATCH-397 FireworksWand Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-397`
- Candidate: `SB397-CAND-001`
- System: `Items:Weapons / Maces / FireworksWand`
- File: `Data/Scripts/Items/Weapons/Maces/FireworksWand.cs`
- Behavior: add stale/null mobile and deleted source-wand guard coverage before `FireworksWand.BeginLaunch` reads mobile/map state, consumes charges, sends launch messages, creates effects, or schedules delayed finish effects.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- FireworksWand label and blessed loot type.
- Charge display and `Charges` property behavior.
- Existing charge decrement timing for valid launch attempts.
- Empty-charge message `502412`.
- Launch message `502615`.
- Moving effect, randomized endpoint, delayed finish timer, sound, hue, render, and location effect behavior.
- BlackJack and HiLoCards reward caller behavior for valid mobiles and live wands.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
