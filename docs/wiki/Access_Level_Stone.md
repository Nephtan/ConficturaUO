# Access Level Stone

## Overview
The Access Level Stone lets staff members quickly swap between their normal staff access level and standard player permissions. Each use toggles the owner's `AccessLevel` between the stored staff level and `Player`.

## Creating the Stone
- Staff can create the stone with `[add AccessLevelStone`.
- The item spawns blessed, weighs 1 stone, and appears with hue `0x38` named "Staff To Player To Staff To ...".

## Usage
1. Place the stone in your backpack.
2. Double‑click the stone to toggle access levels.
3. On first use, the stone records the user as its owner. Staff with higher access than the current owner can also claim it.
4. When toggling to player mode, the stone stores the original staff level and sets the owner to `Player`.
5. When toggling back, the stored level is restored and the stone resets to store `Player`.
6. Unauthorized attempts by non‑owners below Counselor delete the stone with a warning.

## Example
1. `[add AccessLevelStone`
2. Double‑click the stone to switch from staff to player.
3. Double‑click again to return to your previous staff level.

## Audience
Staff