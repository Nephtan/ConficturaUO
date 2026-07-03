# SOURCE-BATCH-448 Wreath Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-448`
- Candidate: `SB448-CAND-001`
- System: `Items:Special / Holiday / Wreath`
- Source file: `Data/Scripts/Items/Special/Holiday/Wreath.cs`
- Behavior: add stale/null mobile, source item, dye tub, gump response, deed, and placement guards to Wreath holiday decoration interaction paths.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- `WreathAddon` item IDs `0x232C` and `0x232D`
- hue/random hue behavior
- `Movable = false`
- house co-owner rule
- range checks `3` and `1`
- redeed gumps and button IDs
- `DyeTub` hue assignment
- `WreathDeed` placement target
- wall-orientation choice
- placement and range messages `1062838`, `502092`, `1042001`, `1042036`, `1062840`, and `500295`
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

Implement one local guard-only source edit, verify with targeted source/gate/overlay/changed-line-serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard Wreath interactions`.
