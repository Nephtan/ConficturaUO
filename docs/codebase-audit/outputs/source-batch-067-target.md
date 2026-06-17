# SOURCE-BATCH-067 EggBomb Guard Repair

## Target

- Candidate: `SB064-CAND-004`
- Behavior: add stale/null/mobile/backpack/source-item guards to EggBomb interactions.
- System: `Items:Trades / Ninjitsu / EggBomb`
- Source file: `Data/Scripts/Items/Trades/Ninjitsu/EggBomb.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none
- Approval required: none beyond normal non-gated preflight

## Allowed Change

Add guard-only checks in `EggBomb.OnDoubleClick(Mobile from)` before mobile, backpack, skill, timing, mana, hiding, effect, or consume state is evaluated.

## Must Stay Unchanged

Ninjitsu threshold `50.0`, `NextSkillTime` wait behavior, mana cost `10`, `SkillHandlers.Hiding.CombatOverride` flow, `UseSkill(SkillName.Hiding)`, particle and sound effects, `Consume()` semantics, localization IDs, `ItemID == 0x2809` deserialize compatibility, serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-067 EggBomb Guard Repair
```
