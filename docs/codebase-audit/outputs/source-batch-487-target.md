# SOURCE-BATCH-487 CookableFood Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-487`
- Candidate: `SB487-CAND-001`
- System: `Items:Food / CookableFood`
- File: `Data/Scripts/Items/Food/CookableFood.cs`
- Behavior: add stale/null mobile, deleted source food, and delayed-timer guards to cooking interaction paths.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add early returns in:

- `CookableFood.OnDoubleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`
- `InternalTarget.OnTarget(Mobile from, object targeted)` when `from == null`, `from.Deleted`, `m_Item == null`, or `m_Item.Deleted`
- `InternalTimer.OnTick()` when `m_From == null`, `m_From.Deleted`, or `m_CookableFood == null`

## Must Stay Unchanged

- CookableFood item identity
- `CookingLevel` property
- `Movable` gating
- target assignment for valid users
- heat-source eligibility
- `BeginAction`/`EndAction` behavior for valid mobile timers
- `PlaySound(0x225)`
- `m_Item.Consume()` behavior
- `InternalTimer` 5-second delay
- burn message `500686`
- cooking skill check range
- cooked food `AddToBackpack` behavior
- `DoughTargetInteractions` recipes/messages/consumption
- `RawPig` behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
