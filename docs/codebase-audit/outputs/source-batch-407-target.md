# SOURCE-BATCH-407 NewPlayerTicket Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-407`
- Candidate: `SB407-CAND-001`
- System: `Items:Deeds / NewPlayerTicket pairing`
- File: `Data/Scripts/Items/Deeds/NewPlayerTicket.cs`
- Behavior: add stale/null mobile, deleted source-ticket, missing-backpack, deleted target-ticket, and stale gump-response guard coverage around `NewPlayerTicket.OnDoubleClick`, `InternalTarget.OnTarget`, and `InternalGump.OnResponse`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- NewPlayerTicket label/properties.
- Blessed loot type.
- Owner persistence.
- Owner-only use rule.
- Backpack-use message.
- Target range.
- Pairing messages.
- Gump layout.
- Reward choices.
- Reward messages.
- `AddToBackpack` behavior.
- Ticket `Delete` semantics.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
