# Arachnid tuning cycle 2

Authorized by Jeremy after the guided demonstration, November 1, 2026 local time. Work stays on `ChampRevamp`; no push, production activation, main-branch change, global Level migration, or automatic guided presentation. The demonstration was a mixed result, not a balance pass. [Previous measured evidence](TERRITORY_POLISH_VERIFICATION.md) remains separate.

## Cleanup checkpoint

Demo005's four clients, server and owned recorder helpers were closed after an ordinary copied `World.Save`; port57913 was verified closed. Closure was forced, not graceful. All files remain. The retained20-file checkpoint is `D:/ConficturaUOTestTools/infestation-polish-20261101-005/checkpoints/cleanup-demonstration-01`; process proof is `natural/evidence/demo-closed.json` in that run. Automatic review first rejected the closure; Jeremy then directly approved the exact scope. No workaround was used.

## Firm requirements and alternatives

| Requirement | Direction for this cycle |
| --- | --- |
| Hostile, manageable frontier creatures | Audit actual ordinary AI acquisition, visibility, home reach and legal harmful checks. Test unprovoked acquisition and ordinary pursuit/escape, as well as kills. Nearby creatures retain active AI. |
| Developing characters can contribute incrementally | Weak kills and attackable sacs must lead to comprehensible local reclamation. Inspect real weapons, skills and finite recovery; do not repair poor kits with grants. |
| Substantially less central pressure | First compare lower population in the existing300-by-300 footprint. Do not also double the footprint or reduce all creature stats without evidence. Measure active enemies, acquisition, replacement births, damage and control effects. |
| Queen engages when favored and retreats when losing | Use observable combat health/loss and nearby allies/opponents, with tells, commitment and different entry/reentry thresholds. Preserve normal movement, control effects and finite backup. |
| Indefinite potential spread | Keep geographic growth independent of actor admission targets; retain shared shard-wide work budgets. |
| More representative builds | Review the copied player base, select progression/archetype cohorts beyond the previous four, and audit usable tools/resources. Zanthus remains the top solo mage and must actively fight. A win is not required. |
| Useful Level/archetype information | Audit the current consumers and score limitations first. Add bounded readiness diagnostics/test selection rather than silently changing global character levels or encounter gates. |
| Loot throughout participation | Give small, bounded, useful early progress; preserve crafting/trade roles, existing support attribution and finite victory rewards. Do not add a universal artifact lottery. |
| Comprehensible settings | Expose a small set of validated administrator controls with documented defaults, ranges, interactions, activation timing and save behavior. Technical scheduler limits remain shared safeguards. |

Two concrete alternatives require evaluation rather than simultaneous implementation:

- **Escalation miniboss:** the final local wave could become one named defender. Its defeat could suppress a bounded nearby area, offering a visible middle objective. Risks: accidental beginner punishment, repeatable farming, credit theft, and clearing occupied combatants in view. A proposal must specify source, warning, pursuit, one-time entitlement, clear radius, independent daughters and existing-combatant handling. Compare against the existing nine-birth response before selecting it.
- **Spawn-suppression objectives:** existing sacs already cancel their own pending response when destroyed; inner guardians already retire their own patches. A readable queen rally source can make finite backup interruptible. This builds on existing territory state and avoids immediately adding a second boss and another reward pool. Source removal must cancel pending births, not delete defenders already fighting.

The first miniboss comparison design is a named800-health **Brood Warden** replacing the last four-defender wave, not being added on top. Its telegraphed defeat would retire at most25 connected same-colony cells in a24-tile square neighborhood, leaving active creatures and independent territory outside it intact. It would spend the last four units of that nest's existing allowance, require a saved one-time completion receipt, and provide a bounded material/trophy entitlement rather than another artifact roll. Its danger exceeds the weak-spider beginner task; it would need a deliberate attackable activation objective or a long retreat opportunity. This is a specified alternative, not implemented content. It is deferred until the simpler reclamation and rally-source design is measured.

The implemented objective candidate uses the already living inner guardian selected as the queen's rally source. A visible tell and delay identify the source; killing it interrupts pending births and suppresses that patch, without deleting defenders already present. An alternative permanent **Brood Conduit** objective could instead remove one three-guard call from the queen's total allowance for the attempt. That would need separate persistent objective identities and credit. It is not stacked onto the current source-interruption rule in the first comparison.

Doubling the inner radius would cover four times the area. With the current per-cell population policy, changing only the radius could increase total residents instead of spreading the same number out. A genuine wider-layout alternative needs a normalized total population, new travel-impact review and distinct site acceptance. The first comparison therefore retains the300-by-300 footprint and changes population density. Area damage and charm benefits also change with density, so difficulty cannot be assumed to fall in a simple linear ratio.

## Baseline source findings (before this cycle's changes)

- `ArachnidInfestationActors.cs`: tier0 has30 health, damage2-8 before stat bonuses, perception6 and `FightMode.Closest`. `IsEnemy`/`CanBeHarmful` reject a target outside24 tiles of home. Sacs use `FightMode.None`, zero damage and stationary ordinary AI. These settings do not prove observed unprovoked hostility.
- `Mobiles/Base/Behavior.cs`, `MeleeAI.DoActionWander` and `BaseAI.AcquireFocusMob`: normal acquisition checks visibility, life/dead-bonded-pet state, staff access, enemy status, legal harm and LOS. The nearest eligible enemy can be another NPC. `BaseCreature.IsEnemy` ordinarily accepts players. No global AI rewrite is warranted from the presentation alone.
- `ArachnidTerritoryPolicy.PopulationTarget` currently assigns3/5/7 total residents per inner cell. `RefillInfestationPatch` can replace a resident after five seconds. Population and replacement rate both matter to sustained pressure; lowering density alone may not lower replenishment throughput.
- `ArachnidQueenShelterAI.DoActionCombat` prioritizes shelter and declines distant pursuit. It has no advantage/loss assessment or tactical backup decision.
- `CharacterLevelService`: individual effective-skill credit saturates at100; overall power mixes strongest adventure lane, total skill/cap progress, raw stats/cap and fame/karma. Equipment, available spells/reagents/tithing, consumables and actual controlled-pet strength are absent. `GetDiagnostics` can report noncombat lanes as its best archetype. Hybrid and support scores are useful indicators, not demonstrated solo capability.
- Level consumers include `System/Misc/Players.cs` display/difficulty buckets and `Custom/PvE/RandomEncounters/Helpers.cs` encounter aliases. Creature-facing `IntelligentAction` calculations are a separate system. Preserve these boundaries unless a later reviewed change is justified.
- `ArachnidOutbreakClaim`: existing stage sealing protects the gland/artifact/skull flow. A new repeated-objective reward needs a durable per-character entitlement, because deleting a collected claim alone is not a paid-reward receipt.

## Delivery and verification sequence

1. Retain cleanup/protected-input hashes and establish the clean branch baseline.
2. Survey actual saved builds with the canonical diagnostics plus gear, recovery, spell access and pet evidence. Keep identifying inventories private; commit classification rules and uncertainty, not a player dump.
3. Implement the selected density/replacement policy, targeted frontier acquisition repair if demonstrated, queen tactical state/tells/finite source-dependent backup, and a bounded participation reward. Keep configuration and serializer write/read maps explicit.
4. Verify geometry, real AI acquisition, pursuit release, suppressible objectives, finite births, bystanders/pets, settings validation and old/current save sentinels. Run forced runtime script compilation and the applicable project checks.
5. Use fresh isolated copied worlds and ordinary AI/clock for natural encounter comparisons. Separate natural growth from accelerated/staged geometry, staff travel from successful escape, and driver defects from balance outcomes. Compare a density candidate before combining alternatives. Keep clone fidelity/harness changes in separate commits.
6. Exercise selected actual saved builds across progression/archetypes with audited finite resources; preserve negative results and death-prevention consumption. Record nearby/engaged pressure, replacement births, health/mana, spell/control actions, objective suppression and supply costs. No victory requirement and no automatic tour.
7. Consolidate measured results, remaining release gates, settings documentation, focused commits and worktree status. Production stays disabled until its separate release gates are met.

This plan is a working specification. Candidate numbers and design choices will be updated from source and measured results; no new natural-client acceptance has yet been claimed for this cycle.
