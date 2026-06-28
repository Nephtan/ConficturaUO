# SOURCE-BATCH-089 GenderPotion Guard Repair Closeout

## Summary

`SOURCE-BATCH-089` implemented `SB087-CAND-003`, the `GenderPotion` guard repair from `source-batch-087-candidate-discovery.csv`.

`Data/Scripts/Items/Potions/Special/GenderPotion.cs` now guards null/deleted mobiles, deleted source potions, missing backpacks, and out-of-backpack source potions before race, backpack, body, hair, sound, delete, or bottle-return state is evaluated.

## Preservation

The batch preserved:

- Race restriction message.
- Backpack failure message.
- Body and `BodyValue` values `0x190` and `0x191`.
- `Female` flag updates.
- `Utility.AssignRandomHair`, facial hair item ID behavior, and `RecordsHair` behavior.
- Messages, sound, `this.Delete()`, and `AddToBackpack(new Bottle())` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Special/GenderPotion.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/GenderPotion.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-087-candidate-discovery.csv` still imported successfully.
- Targeted source scan confirmed the new mobile/source-potion/backpack guards and preserved body, hair, message, sound, delete, and bottle-return evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-089` is closed. `SOURCE-BATCH-090+` remains pending the next concrete non-gated source target from `source-batch-087-candidate-discovery.csv`.
