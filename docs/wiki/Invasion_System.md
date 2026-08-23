# Organic City Invasions

## Overview

Organic City Invasions are persistent, player-driven conflicts for Britain, Montor, and Devil Guard on `Map.Sosaria`. Evil characters build corruption through bounded real crimes, choose an invading faction, and pursue a multi-stage siege. A successful invasion replaces the city's ordinary services with an evil occupation that remains until players complete a liberation campaign or an administrator safely aborts it.

The engine and all three cities ship disabled. Staff must validate each city and review legacy objects before staging it.

## Player Commands

- `[InvasionInfo]` shows each city's qualitative condition, the visible invasion phase and faction, and the caller's current side, entry grace, or conflict-exit lock.
- Exact dormant corruption is intentionally hidden from players.
- `[Invasion]` is an Administrator-only control console.

## Who Can Corrupt A City

An eligible evil contributor is either:

- a murderer with at least five long-term murders; or
- a character with karma at or below `-5000` and at least `50.0` base Necromancy or Knightship.

The offender account and any player-victim account must both be at least seven days old with 24 hours of total game time. Staff, same-account interactions, invasion-owned NPCs, and valid opposing-side invasion combat never earn corruption.

## Corruption Rules

Only crimes committed inside the configured city rectangles count. Each city becomes ritual-ready at 100 corruption.

| Crime | Corruption | Deduplication and cap |
| --- | ---: | --- |
| Reported murder of an established player | 25 | One offender/victim pair per 24 hours; 50 murder points per offender/city/24 hours |
| Successful theft from an established player | 5 | One pair per hour; 20 theft points per offender/city/24 hours |
| Serious criminal aggression against an innocent established player | 2 | One pair per 30 minutes; 10 aggression points per offender/city/24 hours |
| Kill a non-invasion town guard | 8 | Source/type deduplication; shared NPC cap |
| Kill a non-invasion vendor or citizen | 4 | Source/type deduplication; shared NPC cap |

NPC crimes are capped at 10 points and all sources together are capped at 80 points per offender, city, and rolling 24 hours. Dormant corruption decays by 10 per real day. It stops decaying when a ritual becomes available.

Town-wide warnings occur at 25, 50, 75, and 100 corruption without exposing the exact meter.

## Ritual And Siege

An eligible ritual leader must have personally contributed at least ten corruption in the current city cycle. At the ritual focus the leader chooses the Clockwork Dominion, Blood Court, or Abyssal Legion.

1. **Ritual:** Invader presence advances a ten-minute active ritual. A defender can disrupt it with an uninterrupted 60-second action, returning the city to 50 corruption and imposing a six-hour ritual lock.
2. **Encirclement:** Three camps appear. Invaders must hold at least two standards for five cumulative active minutes. Defenders can defeat a camp lieutenant and complete a 30-second cleanse at its standard; cleansing two camps repels the invasion.
3. **Breach:** Three civic wards gain progress while only invaders hold the surrounding ground and lose progress while only defenders hold it. Two wards must reach five minutes of progress during a 15-active-minute phase.
4. **Final battle:** The faction commander and city Civic Warden appear. The commander's death saves the city. The Warden's death causes occupation. If both die in the same resolution window, defenders win.

After 15 minutes with no players, ritual/siege timers pause and ordinary field troops are removed. Persisted progress resumes when a player returns. Field forces scale from established accounts active during the preceding five minutes, from 10 ordinary troops for one participant to a maximum of 24 for eight participants.

The factions are mechanically distinct: Clockwork forces use durable construct bodies and repair artificers, Blood Court troops are fast and use life drain and curses, and Abyssal forces apply fire/poison pressure and summon reinforcements. Ordinary field caps can shrink with recent participation; commander and Civic Warden scaling only rises after the boss manifests.

Each encirclement camp is a reversible faction scene rather than a bare standard. Tents, barriers, supplies, lights, and faction-specific machinery, cages, trophies, or summoning props occupy the rear and flanks. The capture ring and two troop approaches remain clear. Every prop carries exact current-session ownership metadata, so reconciliation can repair a partial scene and cleanup cannot remove ordinary world decoration.

## Conflict Boundary

The conflict area is the city's existing region rectangles plus 24 tiles around external invasion anchors. Devil Guard's remote farmland region is not included.

- Entering an active boundary grants ten seconds of grace.
- Moving more than three tiles, casting, attacking, aiding, or interacting with an objective/service ends grace.
- Eligible evil characters join the Invaders; everyone else joins the Defenders. Pets and summons inherit their master's side.
- Opposing sides can fight automatically. Same-side harm and cross-side aid are blocked.
- Ordinary PvE characters are temporarily changed from `NONPK` to `NONPKinEvent`. The engine records and restores only states it changed.
- Leaving restores consent immediately unless the character fought recently; that exit lock lasts two minutes from the latest hostile action or ends on death.
- An invader who attacks an allied invasion NPC becomes a Defender/traitor for at least 30 minutes and loses occupation-service access.

The engine does not change existing player-corpse loot rules.

## Occupation And Liberation

Occupation has no automatic timeout. The engine records qualifying civilized spawners inside the town, stops them, removes only their tracked mobiles, and later restores their original running state. A captured spawner that was already stopped remains stopped after restoration. Mixed, unresolved, and non-civilian spawners are left untouched and reported as validation warnings.

An occupation supplies an evil-only banker, healer, provisioner, fence, ritual quartermaster, patrols, three supply anchors, and an initially invulnerable occupation commander.

- Each anchor starts at 100 integrity.
- Defenders must kill its captain and then complete repeated 30-second actions that remove 10 integrity.
- Invaders can reinforce a living anchor by 5 per action, with a maximum of 25 restored per anchor per UTC day.
- Destroyed anchors cannot be rebuilt. Each one reduces the patrol budget by 25%.
- Destroying all three makes the occupation commander vulnerable. Killing the commander liberates the city.

Liberation or an administrator abort restores captured spawners, deletes only current-session entities, restores tracked PvP state, resets corruption, and protects the city for 72 hours.

## Rewards

Invasion rewards exist only on hostile NPC corpses. There are no crime rewards, objective payments, PvP-kill rewards, victory parcels, tokens, or new currencies.

- Ordinary troops carry difficulty-appropriate gold and generated equipment.
- Lieutenants and supply captains carry 1,000-2,500 gold plus high-quality generated equipment.
- Commanders and Civic Wardens carry 8,000-15,000 gold, two high-tier generated items, and a 10% chance for a third.
- Standard RunUO damage rights and corpse-looting rules apply.

## Staff Activation And Recovery

Use `[Invasion]` as an Administrator:

1. Run **Legacy scan**. The console records that the scan was reviewed and displays the exact serial, type, map, and location of every candidate. A city cannot be enabled before this review.
2. Legacy cleanup has a separate confirmation screen and deletes only the displayed legacy invasion stones and invasion-named spawners/waypoints. It never runs automatically.
3. Run **Validate all**. Validation checks the configured region chain, constructs every city/faction force and camp-scene combination, snaps each anchor to a legal tile within 12 tiles, checks connected objective footprints, camp approaches, elevation, saved spawner home points, doors, travel objects, working fixtures, and the 32-tile external player-house safety envelope, reports mixed or unresolved civilian-spawner candidates, and blocks a city when an overlapping legacy object remains.
4. Enable the global engine, then enable only one staged city.
5. Use `+25 corruption`, `Advance`, `Pause`, `Abort`, and `Liberate` only for staged verification or recovery.
6. Enable the remaining cities only after pathing, services, PvP boundaries, save/restart, and restoration have been accepted.

`Abort` deletes only entities carrying the matching city/session ownership metadata, restores captured spawners and PvP states, returns the city to Dormant, and applies the normal 72-hour protection. A city cannot be disabled while its invasion remains active.

If more than one `InvasionWorldState` is found during world load, the engine disables itself and does not merge or delete either state item.

## Legacy Compatibility

The old `Data/Scripts/Custom/Invasion System/` classes remain compiled so existing invasion stones and saved types can deserialize. Their `[invasion]` command registration is disabled and their start/stop gumps are no longer reachable through the supported workflow. No serialized legacy type or namespace was renamed.

## Source Trace

- Engine initialization, corruption, crime ledgers, conflict consent and PvP decisions: `Data/Scripts/Custom/PvE/Invasions/InvasionService.cs`
- Lifecycle, objectives, occupation, spawner rollback, validation and legacy scan: `Data/Scripts/Custom/PvE/Invasions/InvasionLifecycle.cs`
- Versioned singleton and per-city save state: `Data/Scripts/Custom/PvE/Invasions/InvasionWorldState.cs`, `InvasionTypes.cs`
- Typed objectives, troops, bosses and occupation services: `Data/Scripts/Custom/PvE/Invasions/InvasionEntities.cs`
- Typed, serialized faction camp scenes: `Data/Scripts/Custom/PvE/Invasions/InvasionCampScenes.cs`
- Player/staff commands and guarded gumps: `Data/Scripts/Custom/PvE/Invasions/InvasionCommands.cs`, `Data/Scripts/Custom/Gumps/Invasions/InvasionGumps.cs`
- Current-save map review and accepted coordinates: `docs/wiki/Invasion_Placement_Audit.md`
- Reported-murder hook: `Data/Scripts/System/Gumps/ReportMurderer.cs`
- Successful-theft hook: `Data/Scripts/System/Skills/Stealing.cs`
- NPC crime hook: `Data/Scripts/Mobiles/Base/BaseCreature.cs`
- Harmful/beneficial conflict precedence: `Data/Scripts/System/Misc/Notoriety.cs`

## Verification Status

The implementation passes the Visual Studio solution build, exact project/source truth comparison, the server's `-service -nocache -compileonly` runtime-script compiler, and a disposable empty-save `-service -nocache` startup through listener/console readiness on non-production port 4599. Current-save world geometry, pathing, service behavior, save/restart recovery, combat feel, loot balance, and gump layout still require owner-run staging and acceptance before any city is enabled.

## Audience

Players and staff
