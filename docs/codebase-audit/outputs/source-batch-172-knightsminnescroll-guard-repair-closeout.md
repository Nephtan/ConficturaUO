# SOURCE-BATCH-172 KnightsMinneScroll Guard Repair Closeout

## Summary

SOURCE-BATCH-172 implemented SB163-CAND-010 in Data/Scripts/Magic/Bard/Scrolls/KnightsMinne.cs.

KnightsMinneScroll.OnDoubleClick now returns immediately for null/deleted mobiles or a deleted source scroll before sending the sheet-music message.

## Preserved Behavior

- Valid state still sends "The sheet music must be in your music book."
- Constructor spell ID 360, item ID 0x1F31, hue 0x96, and Stackable = true are unchanged.
- Serialization and read/write order are unchanged.
- Namespace/type/file layout and project/config/data files are unchanged.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for Data/Scripts/Magic/Bard/Scrolls/KnightsMinne.cs: 0
- Exact-file active overlay rows for Data/Scripts/Magic/Bard/Scrolls/KnightsMinne.cs: 0
- Gated approval crossed: No

## Verification

- Targeted source scan: passed; the new guard is present and preserved behavior strings/metadata remain.
- Serializer diff scan: passed; no Serial, Serialize, Deserialize, writer.Write, or reader.Read changes.
- Forbidden-surface source diff scan: passed; no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Data/System/Source/Server.csproj Debug/x86 build: passed.
- .\ConficturaServer.exe -compileonly -nocache: passed with Scripts: Compile-only verification completed successfully.
- git diff --check: passed with expected LF-to-CRLF warnings only.
- Generated root build artifacts restored before staging.

## Next Batch

SOURCE-BATCH-173+ is pending the next Bard scroll candidate from source-batch-163-candidate-discovery.csv.