# SOURCE-BATCH-326 BaseMixture Guard Repair Closeout

## Summary

`SOURCE-BATCH-326` implemented `SB326-CAND-001` as a focused, non-gated `BaseMixture` guard repair.

## Source Change

- File: `Data/Scripts/Items/Potions/Mixtures/BaseMixture.cs`
- Added null/deleted mobile and deleted source potion guards before backpack checks, follower checks, or throw target assignment.
- Added target callback guards for null/deleted mobiles, null/deleted/internal source potions, null target points, and null mobile maps before target-point construction or slime checks.
- Moved `Point3D` construction after the null `IPoint3D` guard.

## Preserved Behavior

- Localized backpack message `1060640`
- Commented `Region.AllowHarmful` policy block
- Follower-cap gate
- Target prompt and duplicate target guard
- `RevealingAction`
- Target range `12`
- Range, LOS, and action-state checks
- `SpellHelper.GetSurfaceTop`
- `AlchemicSlime.MakeSlime` behavior and arguments
- Potion `Consume()` behavior
- Empty `Jar` return
- Slime field persistence
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Potions/Mixtures/BaseMixture.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Potions/Mixtures/BaseMixture.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and backpack, follower, target, slime creation, consume, jar-return, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Potions/Mixtures/BaseMixture.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-327+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-326` is committed.
