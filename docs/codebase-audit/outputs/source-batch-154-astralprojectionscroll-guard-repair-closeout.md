# SOURCE-BATCH-154 AstralProjectionScroll Guard Repair Closeout

## Summary

`SOURCE-BATCH-154` created fresh candidate discovery in `source-batch-154-candidate-discovery.csv` and implemented `SB154-CAND-001` in `Data/Scripts/Magic/Mystic/Scrolls/AstralProjectionScroll.cs`.

`AstralProjectionScroll.OnDoubleClick` now returns immediately for null/deleted mobiles or a deleted source scroll before comparing owner, sending messages, or deleting the scroll. `AstralProjectionScroll.OnDragLift` now returns `false` for the same stale state before comparing owner, sending the crumble message, or deleting the scroll.

## Preserved Behavior

- Valid non-owner state still sends `"The parchement crumbles in your hand."` and deletes the scroll.
- Valid owner double-click still sends `"These writings need to be added to a monk's tome."`
- Valid-state `OnDragLift` still returns `true`.
- Owner serialization and read/write order are unchanged.
- Namespace/type/file layout and project/config/data files are unchanged.

## Fence Evidence

- POST-BATCH-Y gate hits for `AstralProjectionScroll.cs`: 0
- Active overlay rows for `AstralProjectionScroll.cs`: 0
- Gated approval crossed: No

## Verification

- Candidate discovery CSV import: passed with 9 complete candidates.
- Targeted source scan: passed; both new guards are present and preserved behavior strings/delete/return behavior remain.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface source diff scan: passed; no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check`: passed with expected LF-to-CRLF warnings only.
- Generated root build artifacts restored before staging.

## Next Batch

`SOURCE-BATCH-155+` is pending `SB154-CAND-002` / `AstralTravelScroll` fresh preflight.
