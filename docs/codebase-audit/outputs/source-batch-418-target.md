# SOURCE-BATCH-418 HolidayBells Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-418`
- Candidate: `SB418-CAND-001`
- System: `Items:Special / Holiday / HolidayBells`
- File: `Data/Scripts/Items/Special/Holiday/HolidayBells.cs`
- Behavior: add stale/null mobile, deleted source bell, and stale gump response guard coverage around HolidayBells interactions.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- House owner rule.
- `OnOffGump` layout.
- OK/cancel button behavior.
- Locked-down reminder.
- Cancel message.
- `TurnedOn` toggle.
- Movement sound behavior.
- Sound table.
- Holiday bell item definitions.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
