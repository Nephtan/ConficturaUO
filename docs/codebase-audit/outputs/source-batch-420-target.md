# SOURCE-BATCH-420 HalloweenGraves Rename Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-420`
- Candidate: `SB420-CAND-001`
- System: `Items:Gifts / Holiday / Halloween / Grave rename decorations`
- Files:
  - `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave1.cs`
  - `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave2.cs`
  - `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave3.cs`
- Behavior: add stale/null mobile and deleted source grave guards before Halloween grave rename prompt assignment.

## Fence

- POST-BATCH-Y exact-file gate hits: `0` for all three files.
- Active overlay rows: `0` for all three files.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Grave item IDs.
- `Flipable` attributes.
- `Furniture` attributes.
- Default `Name` and `Weight`.
- Rename prompt message.
- Prompt assignment.
- Prompt response stale-state guard.
- Name assignment.
- Confirmation message.
- Old `Weight == 4.0` normalization.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
