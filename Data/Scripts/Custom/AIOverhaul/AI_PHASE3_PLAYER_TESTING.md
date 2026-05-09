# Phase 3 Live Testing

This is the player-facing documentation package for the live phase-3 AI-overhaul rollout. It includes a ready-to-post Discord announcement, a live-shard testing guide, and staff triage notes for sorting incoming reports.

## Discord Post Copy

```text
Phase 3 of the AI Overhaul is now live on the main shard.

This rollout is intentionally small and safe, and it is now easier to find in normal gameplay. We now have matching random overworld Ratman and Lizardman nest spawns on both Sosaria and Lodoria.

The main thing we want feedback on is target choice during combat, especially when more than one valid player is involved.

The most important phase-3 mobs to watch are:
- Headless One
- Ratman
- Lizardman
- Ratman Archer
- Lizardman Archer

If you fight around the new nests, you may also see supporting family mobs like Ratman Mage or Reptalar. Those are expected in nest fights, so please include them in reports if they seem to affect the fight.

Examples of useful things to watch for:
- a mob suddenly swapping between players in a weird or erratic way
- a bruiser-type mob feeling too flaky or too easy to peel off
- an archer-type mob seeming unusually eager or unusually unwilling to change targets when someone is low health or casting
- mobs dropping aggro or freezing during a nest fight
- the same mob family feeling very different between Sosaria and Lodoria

This is not the movement pass yet, so archer spacing, kiting, and combat-distance feel are still later-phase work. If an archer keeps firing while you are close, that is still useful feedback, but treat it as movement feedback unless the target selection itself looks wrong.

If you spot something odd, please post:
- the map
- the mob name
- whether it happened at a Ratman Nest, Lizardman Nest, or outside a nest
- where it happened
- whether it was solo, duo, or group combat
- who the mob was on first
- what changed
- whether it repeated
- a screenshot or clip if you have one

Thank you for helping us test this carefully without breaking the shard around it.
```

## What Is Live Right Now

Phase 3 still has a narrow core target-choice cohort. The main live target-selection test mobs are:

- Headless One
- Ratman
- Lizardman
- Ratman Archer
- Lizardman Archer

That is the core phase-3 targeting cohort. This phase is only about better target choice during combat. It is not a full brain rewrite, not a boss pass, and not a general AI change across the shard.

The live encounter surface is broader than that core list now because players can find random overworld Ratman and Lizardman nests on both Sosaria and Lodoria.

Nest fights can include these expected family members:

- Ratman nests: `Ratman`, `RatmanArcher`, `RatmanMage`
- Lizardman nests: `Lizardman`, `LizardmanArcher`, `Reptalar`

`RatmanMage` and `Reptalar` are expected nest support content. They are not the primary phase-3 target-choice validation cohort, but they are part of live nest encounters and can still matter if they make a whitelisted mob behave strangely.

`Headless One` remains a valid non-nest comparison mob for phase-3 testing.

The expected feel by group is:

- Bruisers: Headless One, Ratman, and Lizardman should feel more committed and less erratic when several nearby players are pressuring them.
- Skirmishers: Ratman Archer and Lizardman Archer should feel more willing to favor low-health or clearly casting targets.

The best live tests are:

- small-group fights
- situations where one player starts the fight and a second joins late
- situations where one player is visibly low on health
- situations where one player is obviously casting
- repeat pulls against the same mob family so players can tell whether the behavior is consistent
- comparisons between the same family on Sosaria and Lodoria

## What We Want Players To Test

Ask players to focus on target choice, not raw difficulty.

Good phase-3 test scenarios are:

- A player finds a Ratman Nest or Lizardman Nest on Sosaria or Lodoria, starts a fight, and a second player joins late.
- Two players attack the same whitelisted mob in or around a nest and watch whether the mob swaps targets in a sensible way.
- One player gets low on health while fighting a Ratman Archer or Lizardman Archer and the group watches whether the archer seems more interested in finishing that target.
- One player is clearly casting while another is only swinging in melee and the group watches whether an archer seems more interested in the caster.
- A melee whitelist mob gets pressured by multiple players and the group watches whether it stays reasonably committed instead of flickering unpredictably between targets.
- Players compare whether Ratman, Lizardman, Ratman Archer, or Lizardman Archer feel consistent between Sosaria and Lodoria.

Tell players to note whether the report came from:

- a `Ratman Nest`
- a `Lizardman Nest`
- a non-nest encounter

If `RatmanMage` or `Reptalar` was involved, players should say whether the problem was:

- on that support mob itself
- on a whitelisted mob that behaved strangely because of the mixed fight
- unclear because the whole pull looked wrong

Things players should specifically watch out for:

- wild target thrashing
- mobs dropping aggro for no clear reason
- mobs freezing mid-fight
- mobs getting stuck on a bad target when another obvious target is right there
- whitelisted mobs behaving clearly differently from similar nearby mobs in a way that feels broken instead of intentional
- the same family feeling inconsistent between Sosaria and Lodoria
- nest support mobs making a fight feel broken instead of just harder
- excluded families seeming to change when they were not supposed to

What counts as especially valuable feedback:

- repeated behavior
- side-by-side comparisons between solo and group combat
- side-by-side comparisons between Sosaria and Lodoria
- short clips
- exact mob names
- exact location or nearest landmark
- whether the second player joined late
- whether anyone was low health
- whether anyone was casting
- whether the issue seemed like target choice, movement, or both

## What Is Not In Phase 3

Phase 3 is not the movement pass.

If players notice archers holding their ground in melee, not backing off enough, or not feeling “kitey” enough, that is useful feedback, but it should be treated primarily as phase-4 movement and spacing feedback unless the target choice itself looks wrong.

Expected nest support content in live fights includes:

- Ratman Mage
- Reptalar

Those are valid things to mention in reports, but they are not the primary phase-3 target-choice validation cohort.

Phase 3 still does not include these families unless something is unexpectedly spilling over:

- Orcish Mage
- Orc
- Urc
- vendors
- healers
- town NPCs
- service NPCs
- pets
- summons
- magical constructs such as Energy Vortex
- bosses
- scripted encounters
- anything that feels custom, phased, or event-specific

If players report one of the excluded families, staff should ask whether the behavior looked like unexpected spillover from the new targeting layer, not whether that family "passed" a phase-3 test it was never meant to be part of.

## How To Report Feedback

Players should be given this copy-paste report template:

```text
Map / World: Sosaria / Lodoria / Unsure
Was this at a nest: Yes / No / Unsure
If yes, which nest: Ratman / Lizardman / Unsure
Mob name:
Nearest town, landmark, or region:
Solo / Duo / Group:
Who was on the mob first:
Did someone join late:
Was someone low health:
Was someone clearly casting:
Which mob looked wrong:
Who did the mob target:
Was the issue target choice, movement/spacing, or both:
What felt wrong or different:
Did it happen more than once:
Screenshot or clip:
```

When possible, ask players to keep reports concrete:

- one main mob family per report
- one location per report
- one short description of what the mob chose to do
- one short description of why that felt wrong
- one note on whether it happened on Sosaria or Lodoria

## Staff Triage Notes

Treat incoming reports using these buckets:

- Likely phase-3 targeting: target thrash, odd swaps, failure to respect obvious low-health or casting pressure on whitelist mobs, frozen combat, dropped aggro, or spillover into an excluded family.
- Likely phase-4 movement: archers holding ground in melee, spacing and backoff feel, chase or pathing complaints, or general kiting expectations.
- Likely nest support-content issue: `RatmanMage` or `Reptalar` acting strangely without a clear whitelist target-choice failure.
- Likely excluded-family issue: Orcish Mage, Orc, Urc, summons, vendors, healers, pets, bosses, scripted encounters, or anything custom/event-driven.

Internal verification notes:

- Reproduce first on the same map the player reported before deciding whether the issue is map-specific or general.
- Staff/admin characters are not valid stock acquisition targets, so reproductions should be run with normal player characters, not by standing on an admin toon and expecting aggro.
- The live phase-3 whitelist is exact-type and default-deny. If staff need to verify a live mob directly, `Props` can confirm `TacticalTargetProfile` and `UsesAITacticalTargeting`.
- The current primary phase-3 targeting cohort is only `HeadlessOne`, `Ratman`, `Lizardman`, `RatmanArcher`, and `LizardmanArcher`.
- `RatmanMage` and `Reptalar` are expected nest support content, not the primary target-choice validation cohort.
- The public post should stay non-technical. Terms such as `ResolveProfile`, `AIObject`, and `AcquireFocusMob` should stay in staff-only discussion.
