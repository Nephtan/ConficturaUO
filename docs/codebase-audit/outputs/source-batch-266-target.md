# SOURCE-BATCH-266 DDRelicMoney Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-266`
- Candidate: `SB266-CAND-001`
- System: `Items:Relics / DDRelicMoney`
- Source file: `Data/Scripts/Items/Relics/DDRelicMoney.cs`
- Behavior: add stale/null/mobile/source-item guards to DDRelicMoney currency `OnDoubleClick(Mobile from)` paths before bank-box lookup, conversion, `AddToBackpack`, messages, or deleting the source item.

## Allowed Source Change

- In each DDRelicMoney currency `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Bank-box-only eligibility and localized failure `1047026`.
- `DDCopper` exchange rate `10` and change return behavior.
- `DDSilver` exchange rate `5` and change return behavior.
- `DDJewels` conversion to `Amount * 2` gold.
- `DDXormite` conversion to `Amount * 3` gold.
- `DDGemstones` conversion to `Amount * 2` gold.
- `DDGoldNuggets` conversion to `Amount` gold.
- `AddToBackpack` reward destination.
- Source item `Delete()` semantics.
- Constructor metadata, serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file documentation row remains `Documented`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-266 DDRelicMoney Guard Repair`
