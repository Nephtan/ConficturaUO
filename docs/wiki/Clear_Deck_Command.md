# Clear Deck Command

## Overview
The **Clear Deck** command removes lingering corpses from the deck of the boat you are standing on. It prevents clutter after battles at sea while preserving player loot.

## Usage
1. Stand on your boat.
2. Type `[ClearDeck` in the command bar.
3. All corpses on your boat within 18 tiles are deleted.
4. If you are not on a boat, you will see *"You must be on a boat to use this command."*

## How It Works
- The command checks that you are aboard a boat using `BaseBoat.FindBoatAt`.
- Corpses on the same boat are eligible for removal.
- Player corpses containing items are ignored so belongings remain safe.
- After all valid corpses are removed, the system reports *"Deck cleared."*

## Example
```
[ClearDeck
```
Running the command above while on a ship removes any empty player corpses and NPC bodies from your deck.

## Audience
Players