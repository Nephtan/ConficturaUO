# SOURCE-BATCH-483 BasePiece Label Guard Repair Closeout

## Summary

`SOURCE-BATCH-483` implemented `SB483-CAND-001`, a non-gated guard repair for board-piece single-click label interactions.

## Source Change

File changed: `Data/Scripts/Items/Misc/Games/BasePiece.cs`

Guarded interaction:

- `BasePiece.OnSingleClick(Mobile from)`

Added guard coverage before final base label dispatch:

- null mobile
- deleted mobile
- deleted source board piece

## Preserved Behavior

- `BasePiece` item identity and `IsVirtualItem` behavior
- `Board` property behavior
- existing orphan-board `Delete` behavior
- existing reparent-to-board `DropItem` behavior
- valid base `OnSingleClick` behavior
- `OnDragLift` board checks
- `CanTarget=false`
- `DropToMobile`, `DropToItem`, and `DropToWorld` behavior
- `GetLiftSound=-1`
- chess/checkers derived piece identities
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Games/BasePiece.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Misc/Games/BasePiece.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Misc/Games/BasePiece.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import: `Rows=1`, `CandidateId=SB483-CAND-001`, `PostBatchYGateHitCount=0`, `ActiveOverlayRows=0`, required fields present
- targeted source scan confirmed the new guard after existing board cleanup/reparent checks and preserved base label dispatch, drag/drop behavior, targeting behavior, and serializer presence
- exact-file POST-BATCH-Y gate scan: `0`
- exact-file active overlay scan: `0`
- changed-line serializer diff scan: no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines
- changed-line forbidden-surface diff scan: no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, access-policy, economy, or reorganization changes
- `git diff --check` passed with expected LF-to-CRLF warnings only
- `Data/System/Source/Server.csproj` Debug/x86 build passed
- `.\ConficturaServer.exe -compileonly -nocache` passed
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-483` source commit: pending. `SOURCE-BATCH-484+` should run fresh candidate discovery after `SOURCE-BATCH-483`.
