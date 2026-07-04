# SOURCE-BATCH-478 GraveStones Rename Guard Repair Closeout

## Summary

`SOURCE-BATCH-478` implemented `SB478-CAND-001`, a non-gated guard repair for grave stone rename interaction.

## Source Change

File changed: `Data/Scripts/Mobiles/Elementals/Necromental.cs`

Guarded interactions:

- `GraveStones.OnDoubleClick(Mobile from)`
- `RenamePrompt.OnResponse(Mobile from, string text)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source grave stone
- null prompt item
- deleted prompt item

## Preserved Behavior

- `Necromental` creature identity
- Necromental stats, resistances, skills, breath/combat behavior, loot, and grave stone drop behavior
- `GraveStones` item identity
- randomized grave stone item ID selection
- valid rename prompt message
- valid rename prompt assignment to `from.Prompt`
- valid `RenamePrompt` name assignment and confirmation message
- `Serial` constructors
- `Serialize` and `Deserialize` layout/versioning for both classes
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Mobiles/Elementals/Necromental.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Mobiles/Elementals/Necromental.cs`: `0`
- Intake register completed rows for `Data/Scripts/Mobiles/Elementals/Necromental.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import: `Rows=1`, `CandidateId=SB478-CAND-001`, `PostBatchYGateHitCount=0`, `ActiveOverlayRows=0`, required fields present
- targeted source scan confirmed the new `OnDoubleClick` and `RenamePrompt.OnResponse` guards and preserved prompt message, prompt assignment, valid name assignment, confirmation message, Necromental behavior symbols, and serializer presence
- exact-file POST-BATCH-Y gate scan: `0`
- exact-file active overlay scan: `0`
- changed-line serializer diff scan: no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines
- changed-line forbidden-surface diff scan: no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, creature combat/loot, or reorganization changes
- `git diff --check` passed with expected LF-to-CRLF warnings only
- `Data/System/Source/Server.csproj` Debug/x86 build passed
- `.\ConficturaServer.exe -compileonly -nocache` passed
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-478` source commit: pending. `SOURCE-BATCH-479+` should run fresh candidate discovery after `SOURCE-BATCH-478`.
