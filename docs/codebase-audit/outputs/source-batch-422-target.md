# SOURCE-BATCH-422 BaseImprisonedMobile Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-422`
- Candidate: `SB422-CAND-001`
- System: `Items:Special / Imprisoned mobile crystal base`
- File: `Data/Scripts/Items/Special/BaseImprisonedMobile.cs`
- Behavior: add stale/null mobile and deleted source item guards before BaseImprisonedMobile backpack and confirmation gump interactions.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Backpack requirement.
- `ConfirmBreakCrystalGump` creation/send.
- Localized backpack-use message `1042001`.
- `Summon` abstract property.
- `Release` virtual hook.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
