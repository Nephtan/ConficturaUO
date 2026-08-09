# SOURCE-BATCH-485 BaseTool Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-485`
- Candidate: `SB485-CAND-001`
- System: `Items:Trades / Tools / BaseTool`
- File: `Data/Scripts/Items/Trades/Tools/BaseTool.cs`
- Behavior: add stale/null mobile and deleted source tool guards to label and crafting interaction paths.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add early returns in:

- `BaseTool.OnSingleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`
- `BaseTool.OnDoubleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`
- `BaseTool.OnDoubleClickRedirected(Mobile from, object o)` when `from == null`, `from.Deleted`, `o` is null/not a `BaseTool`, or the callback tool is deleted

## Must Stay Unchanged

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
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
