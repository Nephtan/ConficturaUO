# Clear Deck Command

## Overview
The **Clear Deck** command is a player command that removes eligible corpse items from the boat the caller is standing on. It is a cleanup command, not an ownership-gated boat command: the compiled C# only verifies that the caller's current location is inside a `BaseBoat`.

## Usage
1. Stand on a boat.
2. Type `[ClearDeck` in the command bar.
3. If the command cannot find a boat at your location, it sends *"You must be on a boat to use this command."*
4. If a boat is found, eligible corpses on that same boat are deleted and the command sends *"Deck cleared."* even if no corpses were removed.

## How It Works
- `ClearDeck` is registered at `AccessLevel.Player` and takes no arguments.
- The command finds the boat with `BaseBoat.FindBoatAt(from, from.Map)`.
- It scans `playerBoat.GetItemsInRange(18)`, which is a range-18 item scan centered on the boat item, then filters each corpse through `BaseBoat.FindBoatAt(corpse, corpse.Map)` so only corpses on the same boat are considered.
- It deletes every corpse whose owner is not a `PlayerMobile`.
- It deletes a `PlayerMobile` corpse only when that corpse has no contained items.
- It preserves player corpses that still contain items.
- This script has no custom serialization and is compiled directly through `Scripts.csproj`; it is not an XMLSpawner definition.

## Current Code Note
The current implementation does not call `Free()` on the pooled enumerable returned by `GetItemsInRange(18)`. The command works functionally, but the C# should be reworked to free that enumerable after the scan.

## Example
```
[ClearDeck
```
Running the command above while standing on a boat removes NPC corpses and empty player corpses found on that same boat. Player corpses with items remain in place.

## Audience
Players
