# SOURCE-BATCH-003 Firebomb Interaction Guard Repair Target

Recorded at: 2026-06-15T19:34:03.4831541-05:00

## Target

- Batch: `SOURCE-BATCH-003`
- Behavior: add stale/null/backpack guards to Firebomb interaction paths.
- System: `Items:Misc / Firebomb`
- Expected file: `Data/Scripts/Items/Misc/Firebomb.cs`
- Commit message: `fix: guard Firebomb interactions`

## Gate And Overlay Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Firebomb.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Firebomb.cs`: `3`
- Active overlay result:
  - `RB-01034`: `SafeNoChange`; FirebombField transient no-payload serialization is approved as intentional.
  - `RB-01033`: `IntentionalLegacy`; Firebomb fixed-format legacy serializer must be preserved.
  - `RB-01035`: `IntentionalLegacy`; FirebombField no-payload serializer must be preserved.

No gated approval is crossed. Staff/access, command-access, economy/reward tuning, region/map behavior, `HouseFoundation` serializer migration, broader serializer/migration work, project/XML/config/data edits, and reorganization remain excluded.

## Allowed Edits

- In `OnDoubleClick(Mobile from)`, return before dereferencing state when the Firebomb is deleted, `from` is null/deleted, `from.Backpack` is null, or the Firebomb is no longer in `from.Backpack`.
- In `OnFirebombTarget(Mobile from, object obj)`, return before target processing when the Firebomb is deleted, `from` is null/deleted, `from.Backpack` is null, the Firebomb is no longer in `from.Backpack`, or the Firebomb map is invalid/internal.

## Must Stay Unchanged

- Firebomb and FirebombField serialization.
- FirebombField transient/no-payload behavior.
- Timer scheduling and timer callback behavior.
- Damage values, range, effects, messages, target range, targeting flow, item internalization, and valid-user throw behavior.
- Region/map policy, economy/reward tuning, staff/access behavior, command access, folder/namespace/type layout, project files, XML/config/data files, and reorganization state.

## Verification Plan

- Targeted source scan proving the new `OnDoubleClick` and `OnFirebombTarget` guards.
- POST-BATCH-Y gate scan proving no gated Firebomb row exists.
- Active overlay scan proving Firebomb rows are serializer no-change or intentional-legacy rows only.
- Serializer diff scan proving no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface scan proving no timer, hook, command, gump, packet, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated tracked root build artifacts before staging.
