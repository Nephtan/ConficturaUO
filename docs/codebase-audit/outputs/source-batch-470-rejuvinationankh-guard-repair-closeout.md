# SOURCE-BATCH-470 RejuvinationAnkh Guard Repair Closeout

## Summary

`SOURCE-BATCH-470` implemented `SB470-CAND-001`, a non-gated guard repair for rejuvination ankh interaction and delayed callback paths.

## Source Change

File changed: `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`

Guarded interactions:

- `RejuvinationAddonComponent.OnDoubleClick(Mobile from)`
- `RejuvinationAddonComponent.ReleaseUseLock_Callback(object state)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source addon component
- malformed delayed-callback state
- stale delayed-callback mobile

## Preserved Behavior

- `RejuvinationAddonComponent` item IDs
- `BeginAction` lock type
- `FixedEffect` `0x373A`
- random restore selection
- Hits/Mana/Stam restore behavior
- localized messages `500801`, `500802`, `500803`, and `500807`
- two-hour `DelayCall`
- `EndAction` behavior for valid mobiles
- `BaseRejuvinationAnkh` movement message
- addon components
- `Serial` constructors
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`: `0`

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved restore/cooldown behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-470` source commit: pending. `SOURCE-BATCH-471+` should run fresh candidate discovery after `SOURCE-BATCH-470`.
