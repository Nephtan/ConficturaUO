# SOURCE-BATCH-337 ShepherdsCrook Guard Repair Closeout

## Summary

`SOURCE-BATCH-337` implemented `SB337-CAND-001`, a non-gated guard repair for `ShepherdsCrook`.

## Source Change

- File: `Data/Scripts/Items/Weapons/Staves/ShepherdsCrook.cs`
- `ShepherdsCrook.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles or deleted crooks before prompt and target assignment.
- `HerdingTarget.OnTarget(Mobile from, object targ)` now returns safely for null/deleted mobiles and deleted target creatures before herdable checks, overhead messages, destination prompt, or internal target assignment.
- `InternalTarget.OnTarget(Mobile from, object targ)` now returns safely for null/deleted mobiles or null/deleted stored creatures before herding skill math, skill checks, or `TargetLocation` mutation.

## Preserved Behavior

- Weapon abilities, stat requirements, damage/speed values, hit durability, `Weight`, and `Resource`.
- Target prompt `502464`, target ranges, and target flags.
- `IsHerdable` paragon/tamable eligibility.
- Controlled-animal message `502467`, destination prompt `502475`, failure messages `502468`/`502472`, and success message `502479`.
- Herding skill math based on `MinTameSkill`, `from.CheckTargetSkill(SkillName.Herding, m_Creature, min, max)`, and `m_Creature.TargetLocation` mutation.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Weapons/Staves/ShepherdsCrook.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Weapons/Staves/ShepherdsCrook.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms the new mobile/source/creature guards and preserved prompt, target ranges, herdable eligibility, controlled-animal message, skill math, `CheckTargetSkill`, `TargetLocation`, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-337` committed as `28480965` with `fix: guard ShepherdsCrook interactions`. `SOURCE-BATCH-338+` should run fresh candidate discovery before any further source edits.
