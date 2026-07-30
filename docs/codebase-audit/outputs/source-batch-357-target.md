# SOURCE-BATCH-357 ObeliskOnCorpse Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-357`
- Candidate: `SB357-CAND-001`
- System: `Quests:Pagan / ObeliskOnCorpse`
- File: `Data/Scripts/Quests/Pagan/ObeliskOnCorpse.cs`

## Intended Source Change

Add a local guard to `ObeliskOnCorpse.OnDoubleClick(Mobile from)` so stale/null interaction state cannot proceed into the existing PlayerMobile check, ObeliskTip scan/grant, discovery logging, or source item deletion path.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Item metadata, `OnDragLift` delegation, `PlayerMobile` eligibility, Titan `StatCap` rejection behavior, duplicate `ObeliskTip` return behavior, `SetupObelisk` default fields and ownership assignment, `AddToBackpack` behavior, messages, sound `0x3D`, `LoggingFunctions.LogGeneric` call, source item `Delete` behavior, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
