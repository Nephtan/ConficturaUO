# OmniAI

## Overview
OmniAI provides modular combat behavior for creatures. It reacts to field spells, swaps weapons, heals itself, and invokes abilities from multiple skill systems such as magery, necromancy, bushido, knightship, ninjitsu, bard songs, elementalism, mysticism, holy man prayers, research spells, shinobi techniques, and syth powers depending on the creature's skills.

## Usage
1. In the creature class, override `ForcedAI` to return `new OmniAI(this)`.
2. Raise the relevant skills above 10.0 to enable modules. For example, `Magery` enables spellcasting, `Bushido` unlocks samurai stances, `Musicianship` enables bard songs, and `Elementalism` or `Spiritualism` unlock their respective spellbooks.
3. Equip the creature with any weapons or gear. OmniAI automatically swaps melee weapons when appropriate.

## Behavior
- Skill checks enable magery, necromancy, bushido, knightship, ninjitsu, bard song, elementalism, mysticism, holy man, research, shinobi, and syth features when their skills exceed 10.0.
- The `Think()` loop periodically checks for field spells and re-arms the mobile.
- While wandering, OmniAI hides, looks for targets, meditates if low on mana, and attempts self‑healing.
- Healing prioritizes the most capable skill set: elementalism and holy man cures, magery healing, necromancy spiritualism, mystic touch-based healing, research restorations, syth absorption or drain life, knightship's Close Wounds, or bandages based on skill proficiency.
- When Bushido is present, the AI chooses stances like Evasion or Counter Attack or executes moves such as Lightning Strike and Momentum Strike according to Bushido skill.

## Custom Spell Support
- **Bard Songs** (Musicianship): uses paeons for group healing, carols/threnodies for debuffs, and offensive requiems.
- **Elementalism**: leverages Elemental Sanctuary, Mend, Armor, and Bolt for protection and ranged pressure.
- **Mysticism** (FistFighting): balances Purity of Body, Gentle Touch, Psychic Wall, and offensive psionics.
- **Holy Man** (Spiritualism): cures with Purge, heals via Touch of Life, defends with Sacred Boon, and smites foes.
- **Research** (Inscribe): heals with Intervention/Healing Touch and attacks with flame bolts when safe.
- **Shinobi** (Ninjitsu): uses deception, evasive escapes, empowering strikes, and mystic shuriken while keeping hidden where possible.
- **Syth** (Psychology): shields with Absorption, drains life when pressured, and hurls psychic/lightning attacks when healthy, falling back to Speed when idle.

## Example
```csharp
protected override BaseAI ForcedAI
{
    get { return new OmniAI(this); }
}
```
Setting skills like `Magery` or `Bushido` above 100 grants the AI access to corresponding spell or combat abilities.

## Audience
Shard staff and scripters who need configurable combat AI across custom spell systems.
