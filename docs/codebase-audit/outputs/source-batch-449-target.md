# SOURCE-BATCH-449 Mistletoe Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-449`
- Candidate: `SB449-CAND-001`
- System: `Items:Gifts / Holiday / Christmas / Mistletoe`
- Source file: `Data/Scripts/Items/Gifts/Holiday/Christmas/Christmas Gifts/Mistletoe.cs`
- Behavior: add stale/null mobile, source item, dye tub, gump response, deed, and placement guards to Mistletoe holiday decoration interaction paths.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- `MistletoeAddon` item IDs `0x2374` and `0x2375`
- hue/random hue behavior
- `Movable = false`
- Winter 2004 label
- house co-owner rule
- range checks `3` and `1`
- redeed gumps and button IDs
- `DyeTub` hue assignment
- `MistletoeDeed` placement target
- wall-orientation choice
- placement and range messages `1062838`, `502092`, `1042001`, `1042036`, `1070883`, and `500295`
- `BaseHouse.Addons` insertion
- `Delete()` behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/changed-line-serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard Mistletoe interactions`.
