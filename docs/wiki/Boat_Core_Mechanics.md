# Boat Core Mechanics

## Overview
The boat system provides core functionality for all drivable boats and magic carpets. It manages components such as the tiller man, boarding planks, hold, and optional boat doors. Boats can be named, decay over time, and sink to create shipwrecks if destroyed.

## Components
- **Tiller Man** – controls speech commands and reports status.
- **Hold** – internal container for cargo.
- **Planks** – port and starboard planks let passengers board or disembark.
- **Boat Door** – optional hatch for cabin access on larger vessels.
- **Anchor** – prevents movement when lowered.

## Movement & Commands
While aboard and holding the tiller, players can issue speech commands to control the vessel:

| Command | Action |
| --- | --- |
| "forward", "backward", "left", "right" | Start moving in the given direction. |
| "stop" | Halt all movement. |
| "turn right", "turn left", "come about" | Rotate the vessel. |
| "raise anchor", "drop anchor" | Toggle the anchor state. |
| "single #", "goto #" | Navigate to numbered map pins. |

Navigation commands use pins on a map placed in the hold. The boat can follow a full course (`start`/`continue`) or a single leg to a specific pin (`single #`).

## Decay and Sinking
Unmoved boats eventually decay. Lowering the anchor stops the boat and updates the decay timer. When a boat is destroyed it leaves behind a sunken ship containing cargo and possible relics.

## Example
1. Place a filled map in the hold and say "start" to begin the course.
2. Say "turn left" or "forward" to manually steer.
3. Say "drop anchor" to stop and prevent drifting.

## Audience
Players and Staff