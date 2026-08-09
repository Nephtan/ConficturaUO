# SOURCE-BATCH-258 MagicLantern Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-258`
- Candidate: `SB258-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `MagicLantern.OnDoubleClick(Mobile from)`.
- System: `Items:Magical / MagicLantern`
- Source file: `Data/Scripts/Items/Magical/MagicLantern.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/MagicLantern.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- `Layer.TwoHanded` equip/unequip flow.
- Backpack requirement.
- Messages.
- ItemID and GraphicID transforms.
- Sounds `0x4BB` and `0x47`.
- `AddItem`/`AddToBackpack` behavior.
- `OnEquip`/`OnRemoved` calls.
- `ProcessClothing`.
- Construction attributes and random hue selection.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-258 MagicLantern Guard Repair

Implement SB258-CAND-001 from source-batch-258-candidate-discovery.csv. Add stale/null/mobile/source-item guard to Data/Scripts/Items/Magical/MagicLantern.cs while preserving Layer.TwoHanded equip/unequip flow, backpack requirement, messages, ItemID/GraphicID transforms, sounds, AddItem/AddToBackpack behavior, OnEquip/OnRemoved calls, ProcessClothing, construction attributes, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard MagicLantern interactions.
```
