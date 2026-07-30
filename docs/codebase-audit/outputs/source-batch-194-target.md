# SOURCE-BATCH-194 NinthAnniversaryCoin Guard Repair

## Target

- Candidate: `SB194-CAND-001`
- Behavior: add stale/null/mobile/source-token/backpack guards to `NinthAnniversaryCoin.OnDoubleClick(Mobile from)`.
- System: `Items:Gifts / NinthAnniversaryCoin`
- File: `Data/Scripts/Items/Gifts/NinthAnniversaryCoin.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately when `from == null`, `from.Deleted`, or the token is deleted. Treat missing backpack or out-of-backpack source token as the existing localized backpack-use failure.

## Must Stay Unchanged

- Ninth anniversary token item ID, name, stackable setting, weight, and blessed loot type.
- Localized backpack-use failure message `1042001`.
- Valid in-backpack `NinthAnniversaryCoinGump` open behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted guard/backpack/message/gump scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
