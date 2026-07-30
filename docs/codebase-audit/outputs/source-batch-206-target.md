# SOURCE-BATCH-206 QuestSouvenir Guard Repair

## Target

- Candidate: `SB206-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `QuestSouvenir.OnDoubleClick(Mobile from)`.
- System: `Items:Gifts / Rewards / QuestSouvenir`
- File: `Data/Scripts/Items/Gifts/Rewards/QuestSouvenir.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately when `from == null`, `from.Deleted`, or the souvenir item is deleted before reading souvenir state, playing sounds, sending messages, or toggling item IDs.

## Must Stay Unchanged

- Bell, candle, book, scales, orb, lantern, and cube messages.
- Bell sound selection.
- `0x1A7F` / `0x1A80` item-ID toggle behavior.
- `GiveReward`, `AddNameProperties`, light assignment, command properties, and serialized fields.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted guard/effect scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
