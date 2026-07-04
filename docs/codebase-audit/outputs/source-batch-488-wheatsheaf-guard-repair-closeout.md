# SOURCE-BATCH-488 WheatSheaf Guard Repair Closeout

## Summary

`SOURCE-BATCH-488` implemented `SB488-CAND-001`, a non-gated guard repair for the WheatSheaf flour-mill target flow.

## Source Change

File changed: `Data/Scripts/Items/Food/Cooking.cs`

Guarded interactions:

- `WheatSheaf.OnDoubleClick(Mobile from)`
- `WheatSheaf.OnTarget(Mobile from, object obj)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source wheat sheaf
- deleted resolved target item

## Preserved Behavior

- WheatSheaf item identity
- constructors and metadata
- `Movable` gating
- target range/flags
- `AddonComponent` to addon handling
- `IFlourMill` target eligibility
- `MaxFlour`/`CurFlour` needs calculation
- `mill.CurFlour` increment
- `Consume(needs)` behavior
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Food/Cooking.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Food/Cooking.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Food/Cooking.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import: `Rows=1`, `CandidateId=SB488-CAND-001`, `PostBatchYGateHitCount=0`, `ActiveOverlayRows=0`, required fields present
- targeted source scan confirmed the new `OnDoubleClick` and `OnTarget` guards and preserved target assignment, AddonComponent-to-addon handling, `IFlourMill` target eligibility, flour quantity math, `mill.CurFlour` increment, wheat `Consume(needs)` behavior, and serializer presence
- exact-file POST-BATCH-Y gate scan: `0`
- exact-file active overlay scan: `0`
- changed-line serializer diff scan: no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines
- changed-line forbidden-surface diff scan: no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, access-policy, economy, or reorganization changes
- `git diff --check` passed with expected LF-to-CRLF warnings only
- `Data/System/Source/Server.csproj` Debug/x86 build passed
- `.\ConficturaServer.exe -compileonly -nocache` passed
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-488` source commit: pending. `SOURCE-BATCH-489+` should run fresh candidate discovery after `SOURCE-BATCH-488`.
