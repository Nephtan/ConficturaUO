# SOURCE-BATCH-039 MusicBox Guard Repair Closeout

## Summary

SOURCE-BATCH-039 implemented the guard-only MusicBox repair in `Data/Scripts/Items/Misc/MusicBox.cs`.

`OnDoubleClick` now returns before sending music packets/messages or advancing `Mplay` state when the interacting mobile is null/deleted or the music box item is deleted.

## Scope Preserved

- Name `Lute of Many Songs`
- `Mplay` property
- all `PlayMusic.GetInstance` calls and message labels
- `Mplay` increment behavior
- `Mplay` reset to `1`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/MusicBox.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Misc/MusicBox.cs`: 0
- Gated approval crossed: no

## Verification

- Targeted source scan confirmed the new `OnDoubleClick` guard.
- Behavior preservation scan confirmed Name `Lute of Many Songs`, first and fallback `PlayMusic.GetInstance` calls, 67 music sends, 67 message sends, `Mplay` increments, `Mplay = 1`, serializer write, and serializer read remain present.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-039 is complete. The `source-batch-037-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-040+` remains pending candidate discovery for the next clean non-gated target.
