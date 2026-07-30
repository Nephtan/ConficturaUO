# SOURCE-BATCH-066 SmokeBomb Guard Repair

## Target

- Candidate: `SB064-CAND-003`
- Behavior: add stale/null/mobile/backpack/source-item guards to SmokeBomb interactions.
- System: `Items:Trades / Ninjitsu / SmokeBomb`
- Source file: `Data/Scripts/Items/Trades/Ninjitsu/SmokeBomb.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none
- Approval required: none beyond normal non-gated preflight

## Allowed Change

Add guard-only checks in `SmokeBomb.OnDoubleClick(Mobile from)` before mobile, backpack, skill, timing, mana, hiding, effect, or consume state is evaluated.

## Must Stay Unchanged

Ninjitsu threshold `50.0`, `NextSkillTime` wait behavior, mana cost `10`, `SkillHandlers.Hiding.CombatOverride` flow, `UseSkill(SkillName.Hiding)`, particle and sound effects, `Consume()` semantics, localization IDs, serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-066 SmokeBomb Guard Repair
```
