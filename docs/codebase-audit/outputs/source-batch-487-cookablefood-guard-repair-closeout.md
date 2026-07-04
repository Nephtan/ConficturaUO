# SOURCE-BATCH-487 CookableFood Guard Repair Closeout

## Summary

`SOURCE-BATCH-487` implemented `SB487-CAND-001`, a non-gated guard repair for CookableFood interaction and delayed cooking callback paths.

## Source Change

File changed: `Data/Scripts/Items/Food/CookableFood.cs`

Guarded interactions:

- `CookableFood.OnDoubleClick(Mobile from)`
- `InternalTarget.OnTarget(Mobile from, object targeted)`
- `InternalTimer.OnTick()`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source food before target handling
- null source food target state
- null delayed timer food reference

## Preserved Behavior

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
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Food/CookableFood.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Food/CookableFood.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Food/CookableFood.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import: `Rows=1`, `CandidateId=SB487-CAND-001`, `PostBatchYGateHitCount=0`, `ActiveOverlayRows=0`, required fields present
- targeted source scan confirmed the new `OnDoubleClick`, `OnTarget`, and `OnTick` guards and preserved target assignment, heat-source handling, `BeginAction`/`EndAction`, item consume, 5-second timer delay, burn message, skill check, cooked-food backpack placement, dough interactions, and serializer presence
- exact-file POST-BATCH-Y gate scan: `0`
- exact-file active overlay scan: `0`
- changed-line serializer diff scan: no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines
- changed-line forbidden-surface diff scan: no command, event hook, gump policy expansion, packet handler, region, startup, project, XML/config/data, access-policy, economy, or reorganization changes
- `git diff --check` passed with expected LF-to-CRLF warnings only
- `Data/System/Source/Server.csproj` Debug/x86 build passed
- `.\ConficturaServer.exe -compileonly -nocache` passed
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-487` source commit: pending. `SOURCE-BATCH-488+` should run fresh candidate discovery after `SOURCE-BATCH-487`.
