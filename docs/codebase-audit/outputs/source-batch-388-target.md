# SOURCE-BATCH-388 WetClothes Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-388`
- Candidate: `SB388-CAND-001`
- System: `Items:Trades / Fishing / WetClothes`
- File: `Data/Scripts/Items/Trades/Fishing/WetClothes.cs`

## Intended Source Change

Add a local guard to `WetClothes.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference an invalid mobile or mutate a deleted wet clothing item before the existing drying behavior runs.

Allowed change:

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

WetClothes randomized adjective, clothing type, item ID, hue, name, weight, squeeze message, sound `0x026`, every dry clothing output mapping, `Utility.RandomDyedHue` output behavior, `AddToBackpack` behavior, source `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
