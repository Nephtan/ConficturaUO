# SOURCE-BATCH-073 MasonryBook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-073`
- Candidate: `SB073-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-book guards to `MasonryBook.OnDoubleClick(Mobile from)`.
- System: `Items:Trades / Specialized / MasonryBook`
- Source file: `Data/Scripts/Items/Trades/Specialized/MasonryBook.cs`

## Fence Result

- POST-BATCH-Y gate hits for `MasonryBook.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Specialized/MasonryBook.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted.
- Treat deleted source books, missing backpacks, or source books outside the backpack as the existing backpack-use failure before calling `IsChildOf(from.Backpack)`.
- Send the Grandmaster Carpenter failure message through `from` so the non-PlayerMobile failure path does not dereference a null `PlayerMobile` cast.

## Must Stay Unchanged

- Backpack message `1042001`.
- Grandmaster Carpentry threshold `100.0`.
- PlayerMobile-only learning and `PlayerMobile.Masonry` success assignment.
- Existing failure/success messages and book `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-073 MasonryBook Guard Repair

Implement SB073-CAND-001 from source-batch-073-candidate-discovery.csv. Add stale/null/mobile/backpack/source-book guards to Data/Scripts/Items/Trades/Specialized/MasonryBook.cs while preserving learning behavior, messages, deletion, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard MasonryBook interactions.
```
