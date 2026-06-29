# SOURCE-BATCH-236 PatchBoard Guard Repair Closeout

## Summary

`SOURCE-BATCH-236` created fresh candidate discovery and implemented `SB236-CAND-001` in `Data/Scripts/Items/Books/BulletinBoards/PatchBoard.cs`.

`PatchBoard.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before range, gump, and speech text state is evaluated. `SpeechGumpEntry.OnClick()` now guards stale/null/deleted cached mobile state before PlayerMobile and browser-launch behavior is evaluated.

## Preserved Behavior

- Range 4 rule, `SpeechGump` availability check, `SpeechFunctions.SpeechText(from, from, "Patch")` lookup, context-menu entry behavior, PlayerMobile-only browser launch behavior, property text, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Books/BulletinBoards/PatchBoard.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed; the `OnDoubleClick` and `OnClick` guards exist and range behavior, `SpeechGump` availability check, `SpeechFunctions.SpeechText` lookup, browser launch, property text, `Serialize`, and `Deserialize` remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff lines.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization diff lines.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated root build artifacts restoration: completed for `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.

## Result

Verified and ready for focused `SOURCE-BATCH-236` commit.
