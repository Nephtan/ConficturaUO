# Government System

## Overview
The Government System lets players found and manage their own cities. Mayors control city growth, taxes, and civic services while citizens contribute population and funds. The system progresses through six ranks, each expanding the city's size and capabilities.

## City Setup
- **City Hall Placement:** Place a city hall deed at least 260 tiles from other cities. The starting treasury is 150,000 gold and the first update occurs after 24 hours.
- **City Levels:** Each level enlarges city limits and raises lockdown capacity. Requirements are:
  | Level | Citizens Required | Title |
  |-------|------------------|-------|
  | 1 | 6 | Outpost |
  | 2 | 12 | Village |
  | 3 | 18 | Township |
  | 4 | 24 | City |
  | 5 | 30 | Metropolis |
  | 6 | 36 | Empire |
- **Membership Rules:** Any character with a house inside city limits may join, but characters cannot belong to multiple cities. Every character on an account counts toward the population.
- **City Limits & Restrictions:**
  - City names must be unique; duplicate names are blocked.
  - Cities must be at least 260 tiles apart and each map has a cap on active cities (Lodor/Sosaria 20, Underworld 10, Serpent Island 10, Isles Dread 5). Underworld placement is disabled by default and must be explicitly enabled.
  - A city can support up to 100 citizens and ban up to 50 players (setting the ban limit to zero disables banning).
  - If enabled, mayoral candidates must have at least 35.0 Forensics skill before running for office.

## City Management
The city management stone controls membership, taxes, and services.

- **Treasury:** Holds city funds used for weekly maintenance and can be withdrawn by the mayor.
- **Taxes:** Income, housing, and travel taxes are debited from citizen banks during city updates.
- **Services:** Mayors can enable banks, taverns, healers, stables, and moongates. Cities may also register to appear on public moongate lists.
- **Lockdowns:** Decorative items can be secured within city limits; lockdown capacity scales with city level.
- **Bans:** Mayors can ban players, marking them criminal within city boundaries.
- **War & Allegiance:** Cities can declare wars or form alliances through the war department menu.
- **Assistant Mayor:** The mayor can appoint an assistant. If the mayor leaves or becomes inactive, the assistant automatically assumes the role. If neither is available, a random citizen is promoted and a new election is scheduled within 24 hours.

### City Stone Commands
Use these phrases while in the city:
- `I wish to lock this down` – secure an item in the city.
- `I wish to release this` – release a locked item.
- `I ban thee` – ban a non-member from the city.

## Elections and Updates
- **Elections:** Every 14 days, citizens vote for mayor. If only one candidate runs or votes tie, the incumbent remains.
- **City Updates:** Weekly updates handle taxes, maintenance, and level progression.

## Staff & Player Commands
| Command | Access | Purpose |
|---------|--------|---------|
| `GovHelp` | Player | Open in-game help for the system |
| `AdminAdd` | GameMaster | Administrative item creation gump |
| `FindCities` | GameMaster | Locate all cities |
| `GovTesting on/off` | GameMaster | Toggle accelerated timers for testing long-running mechanics |
| `UpgradeCitySystem` | Administrator | Upgrade existing cities to the latest version |

## Example
1. Buy a city hall deed and place it at a large open location.
2. Recruit at least six citizens within the first day and deposit funds into the treasury.
3. Configure taxes and civic services with the city stone.
4. Hold regular elections and manage wars or alliances with neighboring cities.

## Audience
Players acting as mayors or citizens and staff who configure and maintain the system.