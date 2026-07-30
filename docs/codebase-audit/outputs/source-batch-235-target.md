# SOURCE-BATCH-235 SwordsAndShackles Book Gump Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-235`
- Candidate: `SB235-CAND-001`
- Behavior: add stale/null/mobile/source-item and gump-response guards to `SwordsAndShackles.OnDoubleClick(Mobile from)` and `SwordsAndShacklesGump.OnResponse(NetState state, RelayInfo info)`.
- System: `Items:Books / SwordsAndShackles`
- Source file: `Data/Scripts/Items/Books/SwordsAndShackles.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Books/SwordsAndShackles.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.
- Return immediately from `OnResponse` when `state == null || state.Mobile == null || state.Mobile.Deleted`.

## Must Stay Unchanged

- Range 4 or weight override opening rule.
- `CloseGump` / `SendGump` flow.
- `Server.Gumps.MyLibrary.readBook(this, from)` tracking.
- Page navigation and close sound behavior.
- Book name, hue, item ID, and weight.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-235 SwordsAndShackles Book Gump Guard Repair

Implement SB235-CAND-001 from source-batch-235-candidate-discovery.csv. Add stale/null/mobile/source-item and gump-response guards to Data/Scripts/Items/Books/SwordsAndShackles.cs while preserving range/opening behavior, gump page navigation, sound, library-read tracking, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard SwordsAndShackles interactions.
```
