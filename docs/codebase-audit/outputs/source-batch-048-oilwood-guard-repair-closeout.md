# SOURCE-BATCH-048 OilWood Guard Repair Closeout

## Summary

SOURCE-BATCH-048 implemented the guard-only OilWood repair in `Data/Scripts/Items/Potions/Oils/OilWood.cs`.

`OnDoubleClick` now returns for null/deleted mobiles and treats deleted source oil, missing backpacks, or source oil outside the backpack as the existing backpack-use failure.

`OilTarget.OnTarget` now returns for null/deleted mobiles, treats null/deleted/out-of-backpack source oil state as the existing backpack-use failure, treats deleted target items as the existing invalid target outcome, and avoids dereferencing a missing/deleted oil cloth before applying the oil.

## Scope Preserved

- carpentry or bowcraft skill threshold `90`
- wood item eligibility
- artifact rejection
- oil cloth requirement and deletion
- `Bottle` return
- `m_Oil.Consume()` behavior
- `MorphingItem.MorphMyItem` behavior
- weapon and armor resource updates
- messages and sound `0x23E`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files

## Verification

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Targeted source scan confirmed the new guards and preserved behavior markers.
- Serializer diff scan found no serialization changes.
- Forbidden-surface diff scan found no forbidden-surface changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated root build artifacts were restored before staging.

## Result

SOURCE-BATCH-048 is complete. The `source-batch-046-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-049+` remains pending candidate discovery for the next clean non-gated target.
