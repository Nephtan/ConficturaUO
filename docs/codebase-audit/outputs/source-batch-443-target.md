# SOURCE-BATCH-443 Lockpick Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-443`
- Candidate: `SB443-CAND-001`
- System: `Items:Trades / Thieving / Lockpick`
- Source file: `Data/Scripts/Items/Trades/Thieving/LockPick.cs`
- Behavior: add stale/null/mobile/source-lockpick/deleted-target/timer-state guards to `Lockpick.OnDoubleClick(Mobile from)`, `InternalTarget.OnTarget(Mobile from, object targeted)`, and `InternalTimer.OnTick()` before dereferencing mobile, source lockpick, target item, or delayed timer state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- lockpick item IDs
- stack/weight behavior
- target prompt localized message `502068`
- target range
- spaceship/key-card rules
- dungeon-door rules
- lockpicking skill thresholds
- sounds `0x54B`, `0x241`, `0x549`, `0x3A4`, and `0x4A`
- success/failure messages
- lockpick `Consume()` behavior
- `TreasureMapChest` failure side effects
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard Lockpick interactions`.
