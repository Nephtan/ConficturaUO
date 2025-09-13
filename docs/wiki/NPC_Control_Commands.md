# NPC Control Commands

## Overview
The NPC Control Commands provide staff with tools to impersonate or take control of in-game mobiles. These commands enable event facilitation and debugging by allowing staff to clone appearances or temporarily possess NPCs. Only accounts with at least `Counselor` access may use these commands.

## Installation
To ensure controlled characters properly release on death, add the following override to `PlayerMobile`:

```csharp
public override bool OnBeforeDeath()
{
    if (Server.Commands.CloneCommands.UncontrolDeath(this))
        return base.OnBeforeDeath();
    else
        return false;
}
```

## Commands
### `[Clone]`
Clones the targeted mobile's appearance, stats, and skills onto the caster. The command only works on targets with a lower access level than the user.

### `[Control [NoStats] [NoSkills] [NoItems]]`
Possesses the targeted NPC. A blessed **Control Item** is placed in the user's backpack. Double‑clicking this item returns the user to their original body. Optional flags determine what characteristics are copied:
- `NoStats` – do not copy attributes.
- `NoSkills` – do not copy skills.
- `NoItems` – do not copy equipment.

Targeting the Control Item deletes it and releases control. Issuing another `[Control]` while controlling swaps to a new target.

### `[CloneMe]`
Creates a hidden duplicate of the caster at their current location. The clone retains the caster's appearance, stats, skills, equipment, and backpack contents.

### `[GmMe]`
Sets the caster's body to a standard GM form, equips GM robes, and maximizes all skills to 120 with 100 attributes.

### `[HearAll]`
Toggles global speech monitoring. When active, the user receives messages spoken anywhere in the world.

### `[Refresh]`
Targets a mobile and restores its Hits, Mana, and Stamina to maximum values.

### `[SayThis "<text>"]`
Forces the targeted mobile or item to say the provided text.

## Example Workflow
1. `[Control NoSkills]` – choose an orc to possess without copying its skills.
2. Role‑play using the orc's body.
3. Double‑click the **Control Item** in your backpack to return to your original form.

## Audience
Staff