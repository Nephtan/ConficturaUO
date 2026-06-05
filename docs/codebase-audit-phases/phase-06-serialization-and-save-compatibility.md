# Phase 6: Serialization And Save Compatibility

## Purpose

Phase 6 protects world saves. RunUO serialization is positional: write order,
read order, field types, version gates, and type names matter. This phase must
complete before source moves or serializer edits in high-risk systems.

## Required Inputs

- Serialization marker scans.
- CrossTreeRuntimeInventory.
- System Cards.
- Root `AGENTS.md` serialization standards.
- Current project truth data.

## Required Outputs

- Serialization Register.
- High-risk serializer list.
- Move/rename risk list.
- Serializer comment target list.
- Save compatibility repair backlog.

## Serialization Register Table

| Field | Meaning |
| --- | --- |
| `Class` | Serialized type name. |
| `File` | Source file. |
| `BaseType` | Parent type. |
| `Kind` | Item, Mobile, Gump support, controller, addon, deed, spawner, other. |
| `Version` | Current version written. |
| `Writes` | Ordered writes after base call. |
| `Reads` | Ordered reads after base call. |
| `VersionHandling` | switch, if gates, or none. |
| `DeletedFields` | Consumed but no longer stored. |
| `TypeNameRisk` | Risk if moved or renamed. |
| `CommentNeeded` | Yes/no and reason. |

## Subphase 6.1: Type Discovery

Find every class that:

- overrides `Serialize`;
- overrides `Deserialize`;
- has a `Serial` constructor;
- writes version integers;
- reads version integers.

Completion gate:

- Every serialized type is discoverable without manual browsing.

## Subphase 6.2: Pairing Serialize And Deserialize

For each class:

1. Confirm `Serialize` exists when `Deserialize` exists.
2. Confirm `Deserialize` exists when `Serialize` exists.
3. Confirm `base.Serialize(writer)` comes before class writes.
4. Confirm `base.Deserialize(reader)` comes before class reads.
5. Confirm version write/read exists.

Completion gate:

- Missing or asymmetric serializers are backlogged.

## Subphase 6.3: Ordered Field Map

For every serializer:

1. List every `writer.Write`.
2. List every corresponding `reader.Read`.
3. Match types exactly.
4. Record conditional reads.
5. Record default values for old versions.

Completion gate:

- A reviewer can compare write and read order without re-parsing the file.

## Subphase 6.4: Version Upgrade Review

Classify version handling:

- `SwitchGotoCase`
- `IfVersionGates`
- `SingleVersionOnly`
- `NoVersionFound`
- `SuspiciousOrder`

Review:

- new fields added at the right version;
- removed fields still consumed for older versions;
- defaults initialized for old saves;
- version comments match code.

Completion gate:

- Version upgrade logic is explicit and reviewed.

## Subphase 6.5: Move And Rename Risk

For every serialized type, classify:

- `SafeToMoveWithoutRename`
- `NamespaceOrTypeRenameDanger`
- `FileMoveOnlyLikelySafe`
- `UnknownUntilSaveTest`
- `DoNotMove`

Notes:

- Moving a file without changing namespace/type is usually lower risk.
- Renaming a class or namespace can break world loading.
- Imported systems may have reflection or config references.

Completion gate:

- Reorganization proposals include save compatibility status.

## Subphase 6.6: Comment Targets

Add comments where:

- field order is non-obvious;
- old version reads look strange;
- deleted fields are intentionally consumed;
- timers are recreated after deserialize;
- linked items/mobiles are repaired after load;
- moving/renaming is dangerous.

Completion gate:

- Fragile serializers carry useful inline warnings.

## Review Checklist

- `Serial` constructors present.
- Base calls first.
- Version integer written/read.
- Writes and reads aligned.
- Old versions handled.
- Move risks classified.
- Comment targets identified.

## Exit Criteria

Phase 6 is complete when high-risk serialized systems have save maps detailed
enough that a future maintainer can change or move code without guessing about
world-save consequences.
