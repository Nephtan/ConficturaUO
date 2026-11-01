# Arachnid infestation: territory, resistance, and queen movement

Design brief, November 1, 2026. Branch: `ChampRevamp`. This document separates Jeremy's requested experience from proposed tuning and verification. It is not a deployment or acceptance announcement. See [the implementation plan](TERRITORY_POLISH_PLAN.md) for delivery gates.

## 1. The experience we want

An unchecked infestation may spread across an entire connected landmass. Its geographical reach should tell the story of a colony that has been allowed to grow. That does not mean every occupied tile becomes endgame content.

Most occupied countryside should remain passable. A traveler sees scattered weak spiders and occasional signs of infestation, can recognize the danger, and can continue traveling. Players who deliberately fight the brood attract a local response. Players who venture toward the queen enter increasingly dangerous, more concentrated defenses.

Beginners should have a useful way to participate without needing to defeat the queen or become strong enough for her inner territory. Veterans should retain the benefits of their developed characters while still having to prepare, make decisions, and sometimes cooperate.

## 2. Territory and difficulty

Jeremy's requested boundary is a **300-by-300-tile square centered on the queen**. Everything outside that square counts as outskirts, however old the colony is or how far it has spread. This is a 150-tile extent on each side, not a 300-tile radius.

| Area | Requested character | Initial implementation proposal |
| --- | --- | --- |
| Outskirts | Sparse, very weak spiders; sparse webs; recognizable but easy to travel through | Tier 0; selected cells have one roaming spider or an egg sac; many cells have neither |
| Outer inner-territory band | First of the three highest tiers | Tier 2, between 100 and 150 tiles from the queen in square distance |
| Middle band | Stronger brood and clearer resistance | Tier 3, between 50 and 100 tiles |
| Queen's center | Highest tier, concentrated around the queen | Tier 4, inside the central 100-by-100 square |

The three 50-tile bands and their population targets are tuning proposals. The 300-by-300 boundary, sparse weak outskirts, and inward increase in danger are the requested design.

Population, visual decoration, and territory ownership are different things. A colony can own a large area without filling every cell with creatures or webs. A technical actor target must not silently become the maximum territorial extent. Work can be scheduled fairly, but players, pets, and relevant NPCs must meet real creatures running their ordinary AI.

## 3. Travel must remain practical

Mere presence in the outskirts must not trigger strong reinforcements. Road users, gatherers, and people passing another player's fight should not inherit that player's escalation. Web placement and stationary structures must leave practical routes around them, including narrow passages.

This is an acceptance requirement, not a promise of absolute immunity from an ordinary weak spider. A beginner should be able to notice, avoid, fight, or escape a small encounter without an event occupying the whole landmass becoming a travel lockout.

**Additional recommendation:** test actual routes, not just average density. One badly placed web or a cluster at a bridge can matter more than hundreds of empty cells. Check both directions, retreat paths, pets, low stamina, and characters with poor equipment.

## 4. A local response to deliberate resistance

Jeremy proposed stronger brood reinforcements as a character kills more spiders. The response should make fighting back feel consequential without punishing ordinary travel or turning a single accidental kill into an unavoidable death spiral.

Recommended rules:

- Attribute damage and kills to the responsible character, including controlled or summoned pets. Cooperating players contribute to the same local disturbance.
- Warn before a stronger wave arrives. Give contributors time and space to withdraw.
- Escalate recent local activity rather than maintain a permanent hidden punishment score.
- Use finite waves from a local source, with cooldowns and a total response allowance. Do not manufacture an endless army or a profitable reinforcement farm.
- Direct the response at contributors and their pets. Protect uninvolved travelers from automatic acquisition while preserving ordinary NPC interactions.
- Limit pursuit locally. Leaving should be possible through ordinary movement; defenders should return through ordinary AI, without vanishing in view.

Initial proposed tuning is 3, 6, and 10 recent kills within a local area; a 10-second warning; waves of 2, 3, and 4 increasingly strong defenders; a 60-second cooldown; and at most 9 response births per nest. These numbers need client testing. They are not part of the original request.

**Important tension:** a modest beginner may need several weak-spider kills to reach an egg sac. The response must not make the intended beginner activity self-defeating. Destroying the sac itself should not trigger a punishment wave, and destroying its source should cancel waves that have not yet arrived.

## 5. Something a beginner can reclaim

Add a recognizable, attackable **egg sac** in selected outskirts locations. It should have little enough health and no meaningful attack that an inexperienced character with basic equipment can destroy it.

Destroying it should produce an understandable local result: the associated patch stops producing brood, its decorative webs are cleared, and it stays suppressed for that outbreak. Existing independent daughter patches remain; one weak structure should not remotely erase a continent. Existing engaged creatures remain real combatants.

This gives beginners a concrete contribution: reclaiming a place, opening a route, or helping a larger assault. It should not depend on rolling a rare artifact. Any future payment or contribution credit needs separate economic review; do not create an unlimited gold or claim loop around cheaply destroyed structures.

**Additional recommendation:** make the structure visually distinct from a fighting spider, and make the reclaimed ground visibly different. A technical name change is insufficient. The current prototype uses a pale creature sprite as a damageable structure; its appearance still requires visual approval.

## 6. The queen should seek shelter within her territory

The queen should seek a reachable interior position in the largest connected body of occupied territory. She should move there normally, favoring places where her brood can surround her. She should reconsider as the colony expands or players break it apart.

The previous observation that she stayed near her starting point warrants investigation; it is not enough to show that a path planner selected a coordinate. Verification needs visible departure, ordinary movement, actual arrival, and sensible behavior when routes are obstructed.

**Additional recommendation:** avoid a circular definition of safety. If the queen's personal escort makes her current location the densest place, choosing the densest existing escort will always tell her to stay home. The prototype therefore uses the extent and interior of established occupied patches to find shelter, independently of her own defender concentration. We should judge whether this produces the intended visible behavior, rather than equate territory area with a proven count of living protectors.

Commit to a reasonable route instead of oscillating between nearly equal goals. Prefer reachable occupied ground over a mathematical center that lies in water, outside the territory, or across an impassable obstacle. Combat, crowding, and control effects must remain real; movement must not become teleportation or immunity.

## 7. Questions and risks worth resolving

1. **What does reclaiming the wilderness achieve?** If the queen can replace every destroyed outskirts patch immediately, beginner participation is cosmetic. Suppression must last long enough to matter, with rules for future outbreaks documented separately.
2. **What happens when the queen moves toward travelers?** Her difficulty bands move too. Reclassification must not refill health or resources, abruptly upgrade an engaged weak spider, or erase visible defenders. A moving threat needs readable transitions.
3. **Can players weaponize a response against others?** Test bystanders, cooperative damage, pets, and deliberate luring. A harmless passerby must not become eligible merely because they are near another character's kills.
4. **Can a colony overwhelm towns through its outskirts policy?** Sparse low-tier travelers are only one part of the problem. Observe normal citizens, vendors, guards, and their respawns; do not mask bad behavior with test-only immunity or reduced AI.
5. **Is the inner territory too large or exhausting?** A 300-by-300 square is substantial. Distinct creature roles, approaches, pauses, retreat routes, and preparation should create decisions rather than a long uniform wall of health.
6. **Does broad spread remain interesting after repeated outbreaks?** Different terrain, broken territorial connections, local reclamation, and a moving queen can provide variety. More variants should eventually have distinct behavior, not merely different colors and health values.
7. **Are rewards supporting distinct activities?** Keep hunting, crafting, exploration, and trade useful. Any reward revision should address actual effort and usefulness; another universal artifact lottery would not by itself solve beginner poverty or encounter repetition.

## 8. What would count as convincing evidence?

Use copied saves and actual saved novice, intermediate, and veteran characters with their own equipment and finite supplies. Record their real starting condition, including buffs from other players. Winning is not required.

- A novice crosses and returns through outskirts, and can retreat from an encounter.
- A novice can destroy an egg sac and visibly suppress its local patch.
- Deliberate repeated kills produce the warned, finite escalation; withdrawal works; bystanders are not automatically attacked.
- Pets and cooperating characters receive correct attribution without bypassing escalation.
- The queen leaves an unsuitable edge, walks toward a suitable interior, and arrives; blocked alternatives and changed territory are exercised.
- Normal NPCs and respawns interact with the brood without special test protection.
- Suppression and response allowances survive an actual copied-world save and reload; old saves still load.
- Shared scheduling permits growth across instances without sacrificing visible creature behavior.

Constructed territory is useful for focused boundary and migration tests. It does **not** prove natural growth. A separate colony must originate through the normal random search and expand by ordinary founder travel and establishment timers under the ordinary server clock, observed through a real client. Accelerated week-long simulations remain supplemental evidence.

## Source and evidence boundaries

Candidate policy and runtime paths:

- `Data/Scripts/Custom/Champions/Outbreak/ArachnidTerritoryPolicy.cs`: `TierAt`, sparse population selection, safe ground and quiet reconciliation.
- `Data/Scripts/Custom/Champions/Outbreak/ArachnidInfestation.cs`: random origin, founder travel/establishment, admission, local retirement and population service.
- `Data/Scripts/Custom/Champions/Outbreak/ArachnidInfestationActors.cs`: actual creature and egg-sac behavior and tier changes.
- `Data/Scripts/Custom/Champions/Outbreak/ArachnidBroodResistance.cs`: principal attribution, warnings, finite responses and reinforcement targeting.
- `Data/Scripts/Custom/Champions/Outbreak/ArachnidQueenShelter.cs` and `QueenBroodMassPlanner.cs`: occupied-territory selection, routes and ordinary queen AI.
- `Data/Scripts/Custom/Champions/Outbreak/ArachnidInfestationState.cs`: positional serializers, patch retirement and persisted response births.
- `Data/Scripts/Custom/Champions/Outbreak/SpreadingEncounterScheduler.cs`: shared admission and work scheduling.
- `tests/ArachnidTerritoryRegression.cs` and `tests/ArachnidTerritoryNativeAcceptance.cs`: separate component and guarded private-client evidence surfaces.

The delivery gate remains disabled in `ArachnidOutbreakController.DeliveryReady`. Current implementation and testing status belongs in the implementation plan and measured evidence, not inferred from this design brief.
