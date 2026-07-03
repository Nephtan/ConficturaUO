# SOURCE-BATCH-411 SackOfHolding Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-411`
- Candidate: `SB411-CAND-001`
- System: `Items:Containers / SackOfHolding`
- File: `Data/Scripts/Items/Containers/SackOfHolding.cs`
- Behavior: add stale/null mobile, deleted source-sack, stale context-menu, and stale gump-response guard coverage around `SackOfHolding.OnDoubleClick`, `GetContextMenuEntries`, `BagMenu.OnClick`, and `BagGump.OnResponse`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- SackOfHolding randomized item setup.
- Owner assignment.
- Open behavior.
- BagGump text/layout/sound.
- Context menu label.
- Container rejection rules.
- Weight reduction behavior.
- MaxItems behavior.
- SackOwner serialization.
- Deserialize normalization.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
