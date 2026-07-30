# SOURCE-BATCH-179 DeathSkulls Guard Repair Closeout

## Summary

SOURCE-BATCH-179 created fresh candidate discovery and implemented SB179-CAND-001 in Data/Scripts/Magic/Death Knight/DeathSkulls.cs.

All 14 DeathKnightSkull OnDoubleClick methods now return immediately for null/deleted mobiles or a deleted source skull before sending the skull message.

## Preserved Behavior

- Valid state still sends "This skull is from a long dead death knight."
- Spell IDs 750-763, item ID randomization, hue 0xB9A, names, and AddNameProperties labels are unchanged.
- Serialization and read/write order are unchanged.
- Namespace/type/file layout and project/config/data files are unchanged.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for Data/Scripts/Magic/Death Knight/DeathSkulls.cs: 0
- Exact-file active overlay rows for Data/Scripts/Magic/Death Knight/DeathSkulls.cs: 0
- Gated approval crossed: No

## Verification

- Candidate CSV import: passed; four zero-gate, zero-overlay candidates recorded.
- Targeted source scan: passed; 14 OnDoubleClick methods, 14 guards, and 14 preserved skull messages are present.
- Serializer diff scan: passed with zero-context diff; no Serial, Serialize, Deserialize, writer.Write, or reader.Read changes.
- Forbidden-surface source diff scan: passed; no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Data/System/Source/Server.csproj Debug/x86 build: passed.
- .\ConficturaServer.exe -compileonly -nocache: passed with Scripts: Compile-only verification completed successfully.
- git diff --check: passed with expected LF-to-CRLF warnings only.
- Generated root build artifacts restored before staging.

## Next Batch

SOURCE-BATCH-180+ is pending SB179-CAND-002 / HolySymbols fresh preflight.