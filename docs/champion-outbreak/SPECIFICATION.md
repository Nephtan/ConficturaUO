# Arachnid Outbreak: first Champion Spawn delivery

## Agreed direction

Build a recurring wilderness threat across **mainland Sosaria**, alongside the existing champion system.

Players discover webs and brood activity through exploration and word of mouth, defeat three brood guardians in any order, then confront Mephitis. Each completed stage rewards useful participation, making a partial expedition worthwhile.

The encounter targets **20–35 minutes for a fixed, artifact-equipped veteran benchmark**, excluding travel. Zanthus tests extreme power, farming efficiency, and macro behavior; faster completion by him is acceptable. Preserve earned character advantages.

## Encounter and lifecycle

- Use **only permanently non-housing locations**, distributed across Sosaria; aim for six reviewed sites, accepting fewer if suitable land is limited. Each needs connected walkable combat space, safe approaches and exits, and clearance from houses, towns, services, portals, and other active events. Validate terrain and inspect each site in the isolated client.
- Runtime placement selects from this catalog and checks current occupancy. Skip unsuitable sites; never fall back to an arbitrary coordinate. Require existing permanent non-housing status across the encounter area and clearance buffer, established by unconditional region rules or a verified immutable-terrain prohibition covering supported housing shapes and positions. Empty land, temporary restrictions, conditional housing permissions and a failed single house-placement attempt do not qualify. Retain dynamic exclusion of houses, city regions, services and portals. The user has selected consideration of new permanent wilderness reservations for the larger layout. Such reservations require explicit definitions, copied-world occupancy checks, native review and a current-world preflight; they must not displace existing homes or depend on an encounter being active.
- Keep **one outbreak available**. An undiscovered outbreak remains in place. After engagement, thirty minutes without meaningful encounter activity ends the attempt. Completion or abandonment starts a random **30–60-minute cooldown** before choosing another site.
- Preserve cleared broods and participation through death, retreat, and restart. Presence alone does not refresh the recovery window.
- Each brood has a guardian and a finite reinforcement budget. Killing its guardian permanently stops further reinforcements and pays that stage. Clearing all three reveals Mephitis.
- Initial combat defaults: **3,000 health per guardian, 9,000 for Mephitis**, using existing arachnid combat behavior as the base. Each brood permits four initial defenders and two reinforcements at each of the guardian’s 75%, 50%, and 25% health thresholds. Mephitis permits two reinforcements at each threshold. Thresholds trigger once.
- Reinforcements retain appropriate ordinary loot, but cannot respawn indefinitely. Event creatures remain within the reviewed footprint.

**Mephitis’s distinctive mechanic:** a visible web warning marks the targeted ground three seconds before impact. Moving out avoids it. A hit restrains movement for at most four seconds; attacking, healing, and casting remain available. Players can spend an action freeing themselves or another participant. Repeated webs cannot extend an existing restraint indefinitely.

In turn-based combat, warnings must allow a legal reaction opportunity, and attacks, escapes, and effects must respect the existing action rules. Wall-clock callbacks cannot bypass them.

## Rewards, participation, and crafting

| Result | Reward |
|---|---|
| Each defeated brood guardian | One venom gland per qualifying participant |
| Mephitis defeated | Three glands per qualifying boss participant |
| Full personal clear | Six glands, sufficient for twelve treatments |
| Boss gold shower | **15,000 gold total**, shared on the ground |
| Collectible rewards | One skull and the existing Mephitis artifact opportunity |

The new encounter awards **no power scrolls**. Existing champions retain their current behavior and rewards.

**Participation is assessed separately for each stage.** Credit player damage, attributed pet/provocation damage, effective healing, relevant cures, poison prevention, rescues, and effective bard support. Initial qualification is ten contribution points:

- Damage earns one point per 1% of the stage’s defined enemy-health budget.
- Healing earns one point per 10% of the patient’s maximum health actually restored from encounter damage.
- A relevant cure, consumed antivenom protection, or web rescue earns two points.
- Effective bard control or debuffing earns one point per ten seconds of useful effect against engaged event enemies; refreshing an existing effect adds no extra credit.

Self-inflicted damage, overhealing, idle proximity, unrelated combat, and repeated treatment of an already-resolved condition earn nothing. Support credit applies to helping other participants or participating pets. A substantial single attack can qualify; token tagging cannot.

Bard time is shared across effects and enemies within each character's stage: overlapping songs and refreshes cannot multiply it. Useful time requires another participant's recent actual combat involvement in that stage (damage dealt, encounter harm received, or encounter poison applied/prevented, including participating pets). The observation window is fifteen enemy actor seconds; an idle or unrelated fight cannot sustain it. Turn-based support requires the caster and affected enemy to share a battle; switching combat modes cannot substitute another clock's elapsed time.

Each character receives each stage award once. Earned rewards survive death, disconnect, and a full backpack through a persistent claim queue. Boss collectible recipients come from qualifying boss participants, including support players. Administrative deletion and cleanup never count as victory.

**Brood Antivenom:**

- A known Alchemy recipe uses one gland, two orange petals, six garlic, and two bottles to produce **two treatments**.
- Use an **80–100 Alchemy** success range, guaranteed at 100, with no exceptional potency bonus. Failed attempts preserve the gland and follow ordinary failure consumption for common ingredients.
- Each treatment prevents **three deadly or lethal poison applications**, lasts up to **five minutes**, and imposes a **two-minute recipient reuse delay**. It cannot stack or refresh an active treatment.
- It does not cure existing poison. Existing immunity takes precedence.
- Apply within two tiles and line of sight to yourself, a party ally, or a controlled pet belonging to either, subject to existing beneficial-action restrictions.
- Protection applies to wild-creature poison, excluding player-controlled, summoned, or player-provoked sources. Preserve existing garlic restrictions.
- Show remaining protection and expiry clearly. Persist remaining charges and cooldown correctly across reconnects and restarts.

Materials and treatments are tradable. Ordinary cures and existing preparations must remain sufficient to complete the encounter; its own reward cannot become an entrance requirement.

## Implementation and automated delivery

Add a separate, versioned outbreak controller, site catalog, stage ledger, encounter creatures, materials, and treatment effect. Keep existing serialized champion types and XML controllers intact.

Add narrowly scoped interfaces where needed:

- Champion reward hooks whose defaults preserve legacy behavior.
- Post-resolution healing attribution and poison-prevention hooks, with existing eligibility and immunity checks preceding the new effect.
- Staff controls for status, starting a reviewed site, and stopping an owned encounter; a player command for claiming pending rewards.

Freeze the design, tuning values, reference-character manifest, and acceptance cases before tuning. The veteran benchmark must use legitimate obtainable equipment and a documented combat policy. Keep Zanthus’s copied character data private.

Reuse the [foundation fixture runner](/D:/ConficturaUO-main/scripts/Test-Foundation.ps1) and the isolated client workflow documented in the [previous acceptance results](/D:/ConficturaUO-main/docs/codebase-audit/outputs/foundation-batch-3-results.md). Parameterize those tools instead of building another testing framework.

Run the delivery loop in focused batches:

1. Lifecycle, placement, persistence, and staff controls.
2. Stage participation, rewards, and crafting.
3. Combat mechanics, client verification, and pacing.

For each batch: implement, run relevant checks, repair failures, record evidence, review the diff, and make a focused local commit. Include the matching executable whenever core changes require it.

Scripts handle repeated setup, combat exercises, measurements, and cleanup. Computer Use verifies visible behavior and investigates failures. Qwen gameplay may provide additional observations, but does not gate delivery or establish learning.

Automatic tuning may adjust health by **±25%**, damage by **±15%**, and attack intervals within **12–20 seconds**. It may not shorten the three-second warning, extend restraints, increase reinforcement budgets, change rewards, alter established character abilities, or weaken the benchmark. After three unsuccessful tuning passes, report the design conflict rather than silently expanding those limits.

## Acceptance and boundaries

Required verification covers:

- Placement rejection, unavailable sites, coexistence with legacy champions, discovery signs, recovery, cooldown, cleanup, and restart during every encounter phase.
- Stage awards, pet ownership, independent support eligibility, manufactured contribution, dead/offline recipients, full backpacks, and duplicate-delivery prevention.
- Crafting yield and failures; poison severity, provenance, immunity, charges, cooldown, pet treatment, persistence, and turn-based behavior.
- Native client confirmation of readable warnings, movement avoidance, self-escape, ally rescue, reward visibility, and treatment feedback.
- Three complete benchmark runs with a **20–35-minute median**; at least one uses ordinary preparations without Brood Antivenom.
- Zanthus runs that measure faster clears and stationary macro behavior without requiring his defeat or imposing a minimum duration.
- Runtime script compilation, relevant existing regressions, save/reload, and verified restoration of isolated test inputs.

Record combat time, deaths, retreats, supplies, gold, artifacts, glands, and treatment use separately. Passing tests establishes implementation behavior; player enjoyment and sustained market demand still require observation.

Deliver a tested local prototype, committed source and required executable, reproducible fixtures, results, and enable/disable instructions. **Production activation remains disabled.** Preserve unrelated worktree changes.

The larger loot economy and beginner progression remain separate work. This pilot gives champion hunting a clearer identity and a crafting connection; it does not claim to solve either broader problem.


## Placement design review pending - 2026-10-28

[The map-backed survey](SITE_SURVEY.md) identifies a conflict between the provisional universal square footprint/buffer and the existing protected overworld geography. Central Orc Fort has 4,041 connected tiles within its permanent housing ban but different dimensions. Site-specific protected layouts are proposed for the future event family; neither that revision nor new wilderness reservations is approved here. Existing requirements and disabled activation remain in force pending that design decision.


### Wilderness reservation prototype - 2026-10-28

Six full-size candidate reservations are recorded in [reservation-proposals.csv](reservation-proposals.csv) and `WildernessEncounterReserve.cs`. Each has a 161x161 permanent-protection proposal around the original 113x113 combat layout. Source proposals remain unreviewed/inactive: no new housing prohibition or encounter activation is enabled by the prototype.

Once approved, reservation protection is world geography independent of encounter creation, stop, cooldown, deletion and restart. Housing enforcement must reject a multi crossing a reservation even when its center is outside, preserve existing staff placement policy and remain effective beneath overlapping region definitions. City founding/growth and legacy XML champion placement must respect the reservations before any proposal is enabled. Native review must confirm terrain, staging, approaches and compatibility with surrounding content.
