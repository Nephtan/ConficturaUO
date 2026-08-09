# SOURCE-BATCH-337 ShepherdsCrook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-337`
- Candidate: `SB337-CAND-001`
- System: `Items:Weapons / Staves / ShepherdsCrook`
- Source file: `Data/Scripts/Items/Weapons/Staves/ShepherdsCrook.cs`
- Behavior: add stale/null/mobile/source-crook/deleted-creature guards to `ShepherdsCrook.OnDoubleClick(Mobile from)`, `HerdingTarget.OnTarget(Mobile from, object targ)`, and `InternalTarget.OnTarget(Mobile from, object targ)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted, or the crook is deleted.
- Return immediately from `HerdingTarget.OnTarget` when `from` is null/deleted or the targeted creature is already deleted.
- Return immediately from `InternalTarget.OnTarget` when `from` is null/deleted or the stored creature is null/deleted.

## Must Stay Unchanged

- Weapon abilities, stat requirements, damage/speed values, hit durability, `Weight`, and `Resource`.
- Target prompt `502464`, target ranges, and target flags.
- `IsHerdable` paragon/tamable eligibility.
- Controlled-animal message `502467`, destination prompt `502475`, failure messages `502468`/`502472`, and success message `502479`.
- Herding skill math based on `MinTameSkill`, `from.CheckTargetSkill(SkillName.Herding, m_Creature, min, max)`, and `m_Creature.TargetLocation` mutation.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-337 ShepherdsCrook Guard Repair`
