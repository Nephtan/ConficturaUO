# SOURCE-BATCH-338 LevelShepherdsCrook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-338`
- Candidate: `SB338-CAND-001`
- System: `Items:Magical / God / Weapons / Staves / LevelShepherdsCrook`
- Source file: `Data/Scripts/Items/Magical/God/Weapons/Staves/LevelShepherdsCrook.cs`
- Behavior: add stale/null/mobile/source-crook/deleted-creature guards to `LevelShepherdsCrook.OnDoubleClick(Mobile from)`, `HerdingTarget.OnTarget(Mobile from, object targ)`, and `InternalTarget.OnTarget(Mobile from, object targ)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted, or the crook is deleted.
- Return immediately from `HerdingTarget.OnTarget` when `from` is null/deleted or the targeted creature is already deleted.
- Return immediately from `InternalTarget.OnTarget` when `from` is null/deleted or the stored creature is null/deleted.

## Must Stay Unchanged

- Level-staff abilities, stat requirements, damage/speed values, hit durability, `Weight`, and `Resource`.
- Target prompt `502464`, target ranges, and target flags.
- `bc.Body.IsAnimal` eligibility.
- Controlled-animal message `502467`, destination prompt `502475`, failure messages `502468`/`502472`, and success message `502479`.
- `from.CheckTargetSkill(SkillName.Herding, m_Creature, 0, 125)` and `m_Creature.TargetLocation` mutation.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-338 LevelShepherdsCrook Guard Repair`
