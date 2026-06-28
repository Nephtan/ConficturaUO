# SOURCE-BATCH-049 OilAmethyst Guard Repair Closeout

## Summary

SOURCE-BATCH-049 implemented the OilAmethyst guard repair in `Data/Scripts/Items/Potions/Oils/OilAmethyst.cs`.

`OnDoubleClick` now guards null/deleted mobiles and deleted/out-of-backpack source oils before backpack or skill state is evaluated. `OilTarget.OnTarget` now guards null/deleted mobiles, null/deleted/out-of-backpack source oils, missing backpacks, deleted target items, and missing/deleted oil cloth state before target, morph, consume, or deletion behavior is evaluated.

## Preservation

Preserved behavior:

- Blacksmith skill threshold `90`
- Metal item eligibility
- Artifact rejection
- Oil cloth requirement and deletion
- `Bottle` return
- `m_Oil.Consume`
- `MorphingItem.MorphMyItem`
- Weapon and armor resource updates
- Messages and sound `0x23E`
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Oils/OilAmethyst.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Potions/Oils/OilAmethyst.cs`: 0
- Gate crossed: none

## Verification

- Targeted source scan confirmed null/deleted mobile guards, source oil guards, missing backpack guard, deleted target guard, oil cloth local guard, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, `Bottle` return, sound `0x23E`, and oil cloth deletion remain present.
- Serializer diff scan showed no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild after rerunning outside the sandbox because the first sandboxed attempt could not read `C:\Users\nepht\AppData\Local\Microsoft SDKs`.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-049 is closed as a committed non-gated source batch. `SOURCE-BATCH-050+` remains pending the next concrete non-gated source target from `source-batch-049-candidate-discovery.csv`.
