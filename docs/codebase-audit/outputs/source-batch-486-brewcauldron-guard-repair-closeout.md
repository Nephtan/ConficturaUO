# SOURCE-BATCH-486 BrewCauldron Guard Repair Closeout

## Summary

`SOURCE-BATCH-486` implemented `SB486-CAND-001`, a non-gated guard repair for the BrewCauldron double-click interaction.

## Source Change

File changed: `Data/Scripts/Items/Potions/Special/BrewCauldron.cs`

Guarded interaction:

- `BrewCauldron.OnDoubleClick(Mobile from)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source cauldron

## Preserved Behavior

- BrewCauldron item identity
- random pool/use initialization
- `ItemID`, `Hue`, `Name`, `Weight`, and `Movable` metadata
- `AddToBackpack` potion selection by pool
- bottle amount/consume behavior
- range failure message
- empty-bottle failure message
- fill success sound/message
- `m_Uses` decrement behavior
- empty-cauldron `ItemID` and `Name` updates
- `OnAfterSpawn` Z adjustment
- decay timer behavior
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Potions/Special/BrewCauldron.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Potions/Special/BrewCauldron.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Potions/Special/BrewCauldron.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import: `Rows=1`, `CandidateId=SB486-CAND-001`, `PostBatchYGateHitCount=0`, `ActiveOverlayRows=0`, required fields present
- targeted source scan confirmed the new `OnDoubleClick` guard and preserved range check, backpack bottle lookup, bottle consume behavior, potion creation, use-count mutation, empty-cauldron visual update, decay timer behavior, and serializer presence
- exact-file POST-BATCH-Y gate scan: `0`
- exact-file active overlay scan: `0`
- changed-line serializer diff scan: no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines
- changed-line forbidden-surface diff scan: no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, access-policy, economy, or reorganization changes
- `git diff --check` passed with expected LF-to-CRLF warnings only
- `Data/System/Source/Server.csproj` Debug/x86 build passed
- `.\ConficturaServer.exe -compileonly -nocache` passed
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-486` source commit: pending. `SOURCE-BATCH-487+` should run fresh candidate discovery after `SOURCE-BATCH-486`.
