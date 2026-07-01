# SOURCE-BATCH-338 LevelShepherdsCrook Guard Repair Closeout

## Summary

`SOURCE-BATCH-338` implemented `SB338-CAND-001`, a non-gated guard repair for `LevelShepherdsCrook`.

## Source Change

- File: `Data/Scripts/Items/Magical/God/Weapons/Staves/LevelShepherdsCrook.cs`
- `LevelShepherdsCrook.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles or deleted crooks before prompt and target assignment.
- `HerdingTarget.OnTarget(Mobile from, object targ)` now returns safely for null/deleted mobiles and deleted target creatures before animal eligibility checks, overhead messages, destination prompt, or internal target assignment.
- `InternalTarget.OnTarget(Mobile from, object targ)` now returns safely for null/deleted mobiles or null/deleted stored creatures before herding skill checks or `TargetLocation` mutation.

## Preserved Behavior

- Level-staff abilities, stat requirements, damage/speed values, hit durability, `Weight`, and `Resource`.
- Target prompt `502464`, target ranges, and target flags.
- `bc.Body.IsAnimal` eligibility.
- Controlled-animal message `502467`, destination prompt `502475`, failure messages `502468`/`502472`, and success message `502479`.
- `from.CheckTargetSkill(SkillName.Herding, m_Creature, 0, 125)` and `m_Creature.TargetLocation` mutation.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Magical/God/Weapons/Staves/LevelShepherdsCrook.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Magical/God/Weapons/Staves/LevelShepherdsCrook.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms the new mobile/source/creature guards and preserved prompt, target ranges, animal eligibility, controlled-animal message, `CheckTargetSkill`, `TargetLocation`, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-338` is ready to commit as `fix: guard LevelShepherdsCrook interactions`. `SOURCE-BATCH-339+` should run fresh candidate discovery before any further source edits.
