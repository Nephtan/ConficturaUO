# SOURCE-BATCH-260 WallTorch Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-260`
- Candidate: `SB260-CAND-001`
- System: `Items:Special / Heritage Items / WallTorch`
- Source file: `Data/Scripts/Items/Special/Heritage Items/WallTorch.cs`
- Behavior: add stale/null/mobile/source-component guard to `WallTorchComponent.OnDoubleClick(Mobile from)` before range checking, item-id toggling, sound playback, and reach-message paths.

## Allowed Source Change

- In `WallTorchComponent.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Range 2 reach rule.
- Localized reach failure `1019045`.
- Item-id toggle states `0x3D98`/`0x3D9B` and `0x3D94`/`0x3D97`.
- Sound `0x3BE`.
- `WallTorchAddon` and `WallTorchDeed` behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file save-compat rows `PBJ-0974`, `PBJ-0975`, and `PBJ-0976` remain `IntentionalLegacy`; they do not approve serializer edits and do not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-260 WallTorch Guard Repair`
