# SOURCE-BATCH-236 PatchBoard Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-236`
- Candidate: `SB236-CAND-001`
- Behavior: add stale/null/mobile/source-item and context-menu mobile guards to `PatchBoard.OnDoubleClick(Mobile from)` and `SpeechGumpEntry.OnClick()`.
- System: `Items:Books / BulletinBoards / PatchBoard`
- Source file: `Data/Scripts/Items/Books/BulletinBoards/PatchBoard.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Books/BulletinBoards/PatchBoard.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.
- Return immediately from `SpeechGumpEntry.OnClick` when `m_Mobile == null || m_Mobile.Deleted`.

## Must Stay Unchanged

- Range 4 rule.
- `SpeechGump` availability check.
- `SpeechFunctions.SpeechText(from, from, "Patch")` lookup.
- Context-menu entry behavior.
- PlayerMobile-only browser launch behavior.
- Property text.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-236 PatchBoard Guard Repair

Implement SB236-CAND-001 from source-batch-236-candidate-discovery.csv. Add stale/null/mobile/source-item and context-menu mobile guards to Data/Scripts/Items/Books/BulletinBoards/PatchBoard.cs while preserving range behavior, SpeechGump flow, SpeechText Patch lookup, context-menu behavior, browser launch, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard PatchBoard interactions.
```
