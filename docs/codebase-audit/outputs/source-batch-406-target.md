# SOURCE-BATCH-406 Scissors Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-406`
- Candidate: `SB406-CAND-001`
- System: `Items:Trades / Tailor Items / Scissors targeting`
- File: `Data/Scripts/Items/Trades/Tailor Items/Scissors.cs`
- Behavior: add stale/null mobile and deleted source-scissors guard coverage around `Scissors.OnDoubleClick`, `Scissors.InternalTarget.OnTarget`, and `Scissors.InternalTarget.OnNonlocalTarget`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Scissors item ID and weight.
- Target prompt message.
- Target range.
- `TargetFlags.None`.
- Self-scissor localized messages.
- Running-with-scissors localized message.
- Movable/nonmovable `IScissorable` dispatch.
- Invalid-target message.
- Success sound `0x248`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
