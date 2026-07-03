# SOURCE-BATCH-440 EssenceOrb Guard Repair Closeout

## Summary

`SOURCE-BATCH-440` implemented `SB440-CAND-001`, a non-gated guard repair for EssenceOrb double-click interaction.

## Source Change

File changed: `Data/Scripts/Items/Misc/Dyes/Essence/EssenceOrb.cs`

`EssenceOrb.OnDoubleClick(Mobile from)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source orb is `Deleted`

The guard runs before owner comparison, owner assignment, morph state mutation, sounds, particles, and messages.

## Preserved Behavior

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

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Dyes/Essence/EssenceOrb.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Misc/Dyes/Essence/EssenceOrb.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guard and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `EssenceOrb.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-440` source commit: `e2b9b79a`. `SOURCE-BATCH-441+` should run fresh candidate discovery after `SOURCE-BATCH-440`.
