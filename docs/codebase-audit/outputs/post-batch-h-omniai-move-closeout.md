# POST-BATCH-H OmniAI Move Closeout

Generated: 2026-06-13T22:56:37.1593591-05:00

## Summary

`POST-BATCH-H-04A` executed the approved Phase 12 OmniAI containment move from `Data/Scripts/Custom/OmniAI` to `Data/Scripts/Custom/ThirdParty/OmniAI`. The batch moved the complete eight-script workspace, preserved all namespaces/classes/APIs, updated eight `Scripts.csproj` compile include rows, updated current source-trace docs, and regenerated path-sensitive audit outputs.

The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. The one serialized type, `Server.Mobiles.AITester`, remains in the same namespace/type with version `0`; this move did not introduce a save-format migration.

## Scope

| Evidence | Value |
| --- | --- |
| Backlog row | $BacklogId |
| Original path | $OldDir |
| Target path | $TargetDir |
| Workspace files moved | $(Data/Scripts/Custom/ThirdParty/OmniAI/AITester.cs Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Bushido.cs Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Core.cs Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Knightship.cs Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Necromancy.cs Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Ninjitsu.cs Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Shared.cs.Count) |
| Runtime-visible files moved | $RuntimeFileCount |
| Namespace/type/API changes | `None` |
| Serialized OmniAI rows | $(.Count) |
| Serialized type | $(@{System=Custom:ThirdParty; CardSystems=OmniAI; File=Data/Scripts/Custom/ThirdParty/OmniAI/AITester.cs; Namespace=Server.Mobiles; Class=Server.Mobiles.AITester; TypeName=AITester; BaseType=BaseCreature; Kind=Mobile; IncludedInScriptsProject=True; MissingCompileTarget=False; UnincludedSource=False; HasSerialConstructor=True; SerializeLine=123; DeserializeLine=129; BaseSerializeLine=125; BaseDeserializeLine=131; BaseCallStatus=BaseCallsFirst; CurrentVersion=0; VersionWriteLine=126; VersionReadLine=132; VersionHandling=SingleVersionOnly; OlderVersions=; WriteCount=1; ReadCount=1; Writes=L126:Write[int]:(int)0; Reads=L132:ReadInt[Int]:; FieldAlignment=AlignedByCountAndKnownTypes; DeletedFields=; MoveRenameRisk=NamespaceOrTypeRenameDanger; MoveRenameRiskReason=World saves bind serialized namespace and type name; do not rename without a migration plan.; CommentNeeded=No; CommentReason=; ReviewStatus=MachineMapped; ReviewReasons=; Evidence=Serialize:L123;Deserialize:L129;SerialCtor:L120;VersionWrite:L126;VersionRead:L132}.Class) version $(@{System=Custom:ThirdParty; CardSystems=OmniAI; File=Data/Scripts/Custom/ThirdParty/OmniAI/AITester.cs; Namespace=Server.Mobiles; Class=Server.Mobiles.AITester; TypeName=AITester; BaseType=BaseCreature; Kind=Mobile; IncludedInScriptsProject=True; MissingCompileTarget=False; UnincludedSource=False; HasSerialConstructor=True; SerializeLine=123; DeserializeLine=129; BaseSerializeLine=125; BaseDeserializeLine=131; BaseCallStatus=BaseCallsFirst; CurrentVersion=0; VersionWriteLine=126; VersionReadLine=132; VersionHandling=SingleVersionOnly; OlderVersions=; WriteCount=1; ReadCount=1; Writes=L126:Write[int]:(int)0; Reads=L132:ReadInt[Int]:; FieldAlignment=AlignedByCountAndKnownTypes; DeletedFields=; MoveRenameRisk=NamespaceOrTypeRenameDanger; MoveRenameRiskReason=World saves bind serialized namespace and type name; do not rename without a migration plan.; CommentNeeded=No; CommentReason=; ReviewStatus=MachineMapped; ReviewReasons=; Evidence=Serialize:L123;Deserialize:L129;SerialCtor:L120;VersionWrite:L126;VersionRead:L132}.CurrentVersion) |
| Runtime hook rows | $( .Count) |
| Runtime script inventory target rows | $(       .Count) |

## Project Truth Result

| Evidence | Count |
| --- | ---: |
| Scripts.csproj compile includes | 6581 |
| Script source files | 6581 |
| Missing compile targets | 0 |
| Unincluded source files | 0 |
| Project cleanup backlog rows | 0 |

## Verification

- New-ProjectTruthRegister.ps1: passed with 6581 includes, 6581 sources, 0 missing compile targets, 0 unincluded sources, and 0 cleanup backlog rows.
- runtime-script-compile-inventory.csv: regenerated with 6581 runtime-visible script rows, 8 OmniAI target rows, and 0 old-path rows.
- serialization-register.csv: regenerated with one OmniAI serialized type, Server.Mobiles.AITester, version 0, with AlignedByCountAndKnownTypes.
- runtime-hook-map.csv: regenerated with 2 OmniAI hook rows (1 Event, 1 Timer).
- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.
- git diff --check: Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files
- ConficturaUO.sln Debug/Any CPU: Passed with existing warnings; ConficturaUO.sln Debug/Any CPU built Server and Scripts, including Custom\ThirdParty\OmniAI scripts; 53 warnings and 0 errors
- Server.csproj Debug/x86: Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors
- .\ConficturaServer.exe -compileonly -nocache: Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done

## Rollback

Move $TargetDir back to $OldDir, restore the previous Scripts.csproj include paths, rerun project truth and path-sensitive audit generators, and revert OmniAI documentation/source-trace paths.
