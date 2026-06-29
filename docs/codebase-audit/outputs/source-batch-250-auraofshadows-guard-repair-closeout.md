# SOURCE-BATCH-250 AuraOfShadows Guard Repair Closeout

## Summary

`SOURCE-BATCH-250` created fresh candidate discovery and implemented `SB250-CAND-001` in `Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AuraOfShadows.cs`.

`AuraOfShadows.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before backpack checking and ItemID toggle/message paths.

## Preserved Behavior

- Backpack requirement, localized message `1042001`, ItemID `2597`/`2594` toggle behavior, existing fallback messages, artifact name property, construction attributes, random hue selection, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AuraOfShadows.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed. Guard and preserved behavior markers were found.
- Serializer diff scan: passed. No `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff lines were present.
- Forbidden-surface diff scan: passed. No command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, target, or reorganization diff lines were present.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated root build artifacts restoration: completed.

## Result

Verified and ready for focused commit.
