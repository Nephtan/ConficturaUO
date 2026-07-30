# SOURCE-BATCH-440 EssenceOrb Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-440`
- Candidate: `SB440-CAND-001`
- System: `Items:Misc / Dyes / EssenceOrb`
- Source file: `Data/Scripts/Items/Misc/Dyes/Essence/EssenceOrb.cs`
- Behavior: add a stale/null/mobile/source-orb guard to `EssenceOrb.OnDoubleClick(Mobile from)` before owner comparison, owner assignment, morph state mutation, sounds, particles, and messages.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- owner comparison and assignment
- `m_OriginalName` value
- morph status toggle
- `TurnOtherOrbsOff` behavior
- player hue, hair hue, and facial hair hue assignment
- item name/hue changes
- sound `0x659`
- particle effect `0x373A`
- success and failure messages
- serialized owner, morph, status, and type fields
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard EssenceOrb interactions`.
