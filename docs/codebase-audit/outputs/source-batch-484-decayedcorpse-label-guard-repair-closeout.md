# SOURCE-BATCH-484 DecayedCorpse Label Guard Repair Closeout

## Summary

`SOURCE-BATCH-484` implemented `SB484-CAND-001`, a non-gated guard repair for the decayed corpse single-click label interaction.

## Source Change

File changed: `Data/Scripts/Items/Misc/Bodies/Corpses/DecayedCorpse.cs`

Guarded interaction:

- `DecayedCorpse.OnSingleClick(Mobile from)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source decayed corpse

## Preserved Behavior

- `DecayedCorpse` item/container identity
- random corpse item IDs
- `Movable=false` behavior
- `Name` assignment
- `GumpID` and `DropSound` values
- `BeginDecay` behavior
- `InternalTimer` delete behavior
- `OnAfterDelete` timer stop behavior
- `CheckContentDisplay=false`
- `DisplaysContent=false`
- `AddNameProperty` localized remains label
- valid `OnSingleClick` localized label behavior
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Bodies/Corpses/DecayedCorpse.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Misc/Bodies/Corpses/DecayedCorpse.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Misc/Bodies/Corpses/DecayedCorpse.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import: `Rows=1`, `CandidateId=SB484-CAND-001`, `PostBatchYGateHitCount=0`, `ActiveOverlayRows=0`, required fields present
- targeted source scan confirmed the new `OnSingleClick` guard and preserved localized label behavior, `AddNameProperty` label behavior, content-display flags, decay timer behavior, metadata, and serializer presence
- exact-file POST-BATCH-Y gate scan: `0`
- exact-file active overlay scan: `0`
- changed-line serializer diff scan: no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines
- changed-line forbidden-surface diff scan: no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, access-policy, economy, or reorganization changes
- `git diff --check` passed with expected LF-to-CRLF warnings only
- `Data/System/Source/Server.csproj` Debug/x86 build passed
- `.\ConficturaServer.exe -compileonly -nocache` passed
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-484` source commit: pending. `SOURCE-BATCH-485+` should run fresh candidate discovery after `SOURCE-BATCH-484`.
