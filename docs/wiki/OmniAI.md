# OmniAI

## Overview
OmniAI provides modular combat behavior for creatures. It reacts to field spells, swaps weapons, heals itself, and invokes abilities from multiple skill systems such as magery, necromancy, bushido, knightship, and ninjitsu depending on the creature's skills.

## Usage
1. In the creature class, override `ForcedAI` to return `new OmniAI(this)`.
2. Raise the relevant skills above 10.0 to enable modules. For example, `Magery` enables spellcasting and `Bushido` unlocks samurai stances.
3. Equip the creature with any weapons or gear. OmniAI automatically swaps melee weapons when appropriate.

## Behavior
- Skill checks enable magery, necromancy, bushido, knightship, and ninjitsu features when their skills exceed 10.0.
- The `Think()` loop periodically checks for field spells and re-arms the mobile.
- While wandering, OmniAI hides, looks for targets, meditates if low on mana, and attempts self‑healing.
- Healing prioritizes magery cures, necromancy's spiritualism, knightship's Close Wounds, or bandages based on skill proficiency.
- When Bushido is present, the AI chooses stances like Evasion or Counter Attack or executes moves such as Lightning Strike and Momentum Strike according to Bushido skill.

## Example
```csharp
protected override BaseAI ForcedAI
{
    get { return new OmniAI(this); }
}
```
Setting skills like `Magery` or `Bushido` above 100 grants the AI access to corresponding spell or combat abilities.

## Audience
Staff