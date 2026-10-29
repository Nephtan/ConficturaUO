# Territorial survival and pursuit review

Status: revised read-only proposal. No NPC AI change, mobility grant, production activation or general AI rollout is applied.

## Direction and correction

The monitor relayed guidance that Mephitis should try to survive while staying in her territory, and that pursuit must consider mounts and actual speed abilities. This is a design input, not approval to apply the earlier stand-and-fight snippet. That snippet is withdrawn from the current recommendation. The next sequence is to validate the existing mobility toolkit, then review a concrete outbreak-only territorial-retreat policy.

The unmounted reference010 stops alive/incomplete at2261.441 seconds with4,841 actual running requests. This establishes prolonged pursuit for that policy. It does not establish adequate pursuit with the build's full available toolkit. Separately, actual copied Zanthus completes route002 in277.502 seconds; the current boss can be killed. Neither result justifies weakening earned player capabilities.

## Mobility facts and next tests

- PlayerMobile.ComputeMovementSpeed selects mounted movement when Mount is present. Core defaults are0.2 seconds per running foot step and0.1 per running mounted step. This is a source-level allowance, not proof of measured path throughput or a successful mounted clear.
- A normal mount must pass BaseMount.OnDoubleClick/CheckMountAllowed: ownership or summoning, appropriate body, range, healthy mount and applicable regional/building restrictions. Ordinary CagedHorse stock exists conditionally in StoreSalesList; acquisition and release must be verified. Do not set Rider directly to manufacture a native mounting result.
- Celerity is a real candidate for this swords build. JediSpell.GetJediSkillMax returns the higher of Swords and Psychology, not the lower. Its data entry requires skill50; the trained reference's Swords clears that skill gate despite Psychology0. Positive Karma also clears the alignment gate. A learned/usable datacron, crystals, mana, footwear and regional restrictions still need actual inventory/entry-path checks. Base data lists20 mana and80 crystals; actual scaling/consumption is not yet measured. It is not accepted as currently available merely from the skill check.
- Wind Runner is not a free interchangeable option. MysticSpell.CheckCast requires MonkNotIllegal, which rejects the reference's ordinary sword; its equipment/ability checks matter in addition to the listed skill70. Syth Speed rejects this reference's positive Karma. Shinobi/Animal Form and speed footwear require their own eligibility, acquisition and build-tradeoff review.
- Speed powers send a client speed-control packet and have explicit server movement-throttle handling. Native effects and travel time must be measured; spell names and a buff icon alone are insufficient.

First validate ordinary acquisition/ownership and native mounting in a disposable copy, then measure the same recovery route and actual chase. Freeze a new mobility policy before any complete reference series. Review Celerity's real learned-power/resource path and record why each option is used or omitted. Existing partial pilots remain retained; complete reference clears0/3 and tuning0. Faster character movement is a player capability, not NPC tuning.

Follow-up mounted-navigation001 passes native cage release/mounting, ownership/rider and unchanged other reference capabilities. One declared vendor-obtainable cage is provided as a fixture asset rather than a claimed purchase. Recovery takes21.179 seconds with86 running requests; the earlier foot004 sample is26.454 seconds. Actual chase/full combat is still unproven. Celerity's mounted-caster rejection makes it an omitted alternative for mounted v4 rather than an additional boost. Its normal Player command also requires HasSpell; actual unmounted learning/resources remain a separate test.

## Outbreak-only territorial survival candidate

Use the existing113x113 combat footprint as the territory; its surrounding permanent-reservation buffer is not extra combat space. A retreat should seek a useful destination within that footprint, preserve a valid hostile target and reevaluate its choice after a bounded actor-time interval. Prefer existing defenders or useful cover when available. If no useful retreat exists or the player catches Mephitis, ordinary attacks/webs remain available. Do not turn fleeing into an indefinite wait for half health.

Preserve normal movement legality/AP, web warnings/costs, damage, finite reinforcement budgets, rewards and player power. Do not add healing, health resets, invulnerability, new reinforcement waves, forced dismounts or a minimum clear duration. The existing out-of-bounds correction remains a safety guard rather than the normal retreat route. Ordinary world Mephitis stays unchanged.

The concrete policy still needs implementation-seam, path, cornering, target-legality, TBC/AP and restart review before source changes. AI behavior remains outside the automatic HP/damage/interval tuning allowance. No further design answer is necessary to investigate mobility; a bounded retreat specification will be made reviewable after those findings.

## General NPC AI exploration

There is already an AI-overhaul project, not a blank slate. Its handoff records phase4 complete and role-specialist mage/healer/squad work next. Current AITacticalTargeting source whitelists exact HeadlessOne/Ratman/Lizardman melee types and RatmanArcher/LizardmanArcher skirmishers. Its default-deny resolver excludes controlled/summoned actors and nonmatching AI/fight modes. Mephitis and the outbreak subclass are absent. Preserve legality-first targeting and the documented ForcedAI/civilian/service/encounter exceptions.

A broader investigation should distinguish fleeing to a useful refuge from merely running away; assess when creatures regroup, use existing control/healing, protect allies or resume fighting; and preserve differences between animals, mindless creatures and trained combatants. Existing abilities, perception, resources, telegraphs and player counterplay should drive those policies. Broad changes or a whitelist expansion are not authorized by this review. Do not silently route every creature through the outbreak experiment.

## Source and evidence

- [PlayerMobile.cs](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs): ComputeMovementSpeed and MovementThrottle_Callback.
- [Mobile.cs](../../Data/System/Source/Mobile.cs): mounted/foot movement defaults.
- [BaseMount.cs](../../Data/Scripts/Mobiles/Base/BaseMount.cs), [AnimalCages.cs](../../Data/Scripts/Items/Containers/AnimalCages.cs), [StoreSalesList.cs](../../Data/Scripts/Mobiles/Base/StoreSalesList.cs): mounting, caged horses and conditional stock.
- [Celerity.cs](../../Data/Scripts/Magic/Jedi/Spells/Celerity.cs), [JediSpell.cs](../../Data/Scripts/Magic/Jedi/JediSpell.cs): power284, maximum-of-two skill gate, alignment/resources/region checks.
- [WindRunner.cs](../../Data/Scripts/Magic/Mystic/Spells/WindRunner.cs), [MysticSpell.cs](../../Data/Scripts/Magic/Mystic/MysticSpell.cs), [SythSpell.cs](../../Data/Scripts/Magic/Syth/SythSpell.cs): distinct speed-power eligibility.
- [Behavior.cs](../../Data/Scripts/Mobiles/Base/Behavior.cs): MeleeAI below20% flee selection and above50% recovery; ordinary action/TBC dispatch.
- [ArachnidOutbreakController.cs](../../Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakController.cs): KeepCreatureInBounds.
- [AITacticalTargeting.cs](../../Data/Scripts/Custom/Combat/AIOverhaul/AITacticalTargeting.cs), [AI_HANDOFF.md](../../Data/Scripts/Custom/Combat/AIOverhaul/AI_HANDOFF.md): current rollout and existing roadmap; no project phase is executed here.
- [BENCHMARK.md](BENCHMARK.md), [PALADIN_TOOLKIT_REVIEW.md](PALADIN_TOOLKIT_REVIEW.md), [ARCHMAGE_STRESS.md](ARCHMAGE_STRESS.md): retained separate evidence classes and incomplete reference series.
