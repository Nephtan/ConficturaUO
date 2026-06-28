# SOURCE-BATCH-076 TaxidermyKit Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-076`
- Candidate: `SB076-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-kit guards to `TaxidermyKit.OnDoubleClick(Mobile from)` and `CorpseTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Trades / Carpenter Items / TaxidermyKit`
- Source file: `Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs`

## Fence Result

- POST-BATCH-Y gate hits for `TaxidermyKit.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source kit is deleted.
- Return immediately when the target callback has a null/deleted source kit.
- Treat missing backpacks or source kits outside the backpack as the existing backpack-use failure.

## Must Stay Unchanged

- Backpack, invalid-target, visited-corpse, skill, board, review, and use-up messages.
- Carpentry skill threshold `90.0`.
- Target range `3`.
- Board cost `10`.
- `TrophyInfo` table and `TrophyDeed` creation.
- `BigFish` fisher/weight/consume behavior.
- `Corpse.VisitedByTaxidermist` behavior.
- Kit `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-076 TaxidermyKit Guard Repair

Implement SB076-CAND-001 from source-batch-076-candidate-discovery.csv. Add stale/null/mobile/backpack/source-kit guards to Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs while preserving target eligibility, messages, board consumption, trophy creation, fish/corpse behavior, kit deletion, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard TaxidermyKit interactions.
```
