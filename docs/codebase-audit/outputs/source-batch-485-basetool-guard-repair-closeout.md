# SOURCE-BATCH-485 BaseTool Guard Repair Closeout

## Summary

`SOURCE-BATCH-485` implemented `SB485-CAND-001`, a non-gated guard repair for shared crafting-tool label and double-click interaction paths.

## Source Change

File changed: `Data/Scripts/Items/Trades/Tools/BaseTool.cs`

Guarded interactions:

- `BaseTool.OnSingleClick(Mobile from)`
- `BaseTool.OnDoubleClick(Mobile from)`
- `BaseTool.OnDoubleClickRedirected(Mobile from, object o)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source tool
- null/non-tool captcha callback object
- deleted callback tool

## Preserved Behavior

- BaseTool item identity
- `ToolQuality` enum behavior
- `Crafter`, `Quality`, and `UsesRemaining` properties
- `GetUsesScalar`, `ScaleUses`, and `UnscaleUses` behavior
- `ShowUsesRemaining` behavior
- `CraftSystem` binding
- `GetProperties` uses/exceptional labels
- `DisplayDurabilityTo` label text
- `CheckAccessible` and `CheckTool` semantics
- `CaptchaGump` macro-resource policy
- backpack/equipped eligibility
- `CraftSystem.CanCraft` results
- `CraftGump` dispatch
- `TomeOfWands` sound `0x55`
- localized backpack-use failure `1042001`
- `OnDoubleClickRedirected` valid callback behavior
- `OnCraft` behavior
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Tools/BaseTool.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Trades/Tools/BaseTool.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Trades/Tools/BaseTool.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import: `Rows=1`, `CandidateId=SB485-CAND-001`, `PostBatchYGateHitCount=0`, `ActiveOverlayRows=0`, required fields present
- targeted source scan confirmed the new `OnSingleClick`, `OnDoubleClick`, and `OnDoubleClickRedirected` guards and preserved durability label, captcha dispatch, craft-gump dispatch, backpack-use failure, TomeOfWands sound, and serializer presence
- exact-file POST-BATCH-Y gate scan: `0`
- exact-file active overlay scan: `0`
- changed-line serializer diff scan: no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines
- changed-line forbidden-surface diff scan: no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, access-policy, economy, or reorganization changes
- `git diff --check` passed with expected LF-to-CRLF warnings only
- `Data/System/Source/Server.csproj` Debug/x86 build passed
- `.\ConficturaServer.exe -compileonly -nocache` passed
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-485` source commit: `44b627f8`. `SOURCE-BATCH-486+` should run fresh candidate discovery after `SOURCE-BATCH-485`.
