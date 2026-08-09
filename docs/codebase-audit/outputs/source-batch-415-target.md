# SOURCE-BATCH-415 WindChimes Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-415`
- Candidate: `SB415-CAND-001`
- System: `Items:Misc / WindChimes`
- File: `Data/Scripts/Items/Misc/WindChimes.cs`
- Behavior: add stale/null mobile, deleted source chime, and gump response guard coverage around `BaseWindChimes.OnDoubleClick` and `OnOffGump.OnResponse`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Owner-only use rule.
- `OnOffGump` layout.
- OK/cancel button behavior.
- Locked-down reminder.
- Cancel message.
- `TurnedOn` toggle.
- Movement sound behavior.
- Sound table.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
