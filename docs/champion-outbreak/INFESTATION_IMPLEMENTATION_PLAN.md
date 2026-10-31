# Arachnid infestation implementation and acceptance

October 31 revision: [PERSISTENT_SIMULATION.md](PERSISTENT_SIMULATION.md) is current. This initial plan is historical; its 16-patch/400-birth/per-attempt limits and two-hour lifetime are superseded by shared background budgets, persistent compact territory and protected ordinary actors. Five ranks and concurrent directors are implemented. Rows below do not represent current gate dispositions.

Authorized October 30, 2026 in the existing loot/champion development session. Baseline is `da99f391`; the three pre-existing untracked user files remain excluded. The prior runs are closed and their evidence is OLD encounter evidence. Production activation and deployment remain out of scope.

## Intended behavior and reversible defaults

- Random suitable Sosaria origin, queen and root brood present immediately. No nest shield or boss-last gate. No permanent reservations or reserved-house/city/legacy-placement hooks.
- A small director owns connected brood patches. Expansion requires a living parent brood and an actual surviving frontier founder that reaches adjoining ground and survives its establishment interval. Guard kills can stop expansion; time alone cannot upgrade a defended frontier.
- Begin with a modest root population, expand toward a visible colony with at most 16 established patches, 160 living event creatures including queen/guardians/founders, 400 creature births and 240 decorative web pieces per attempt. At most one founder and two local route requests per director tick; individual routes stay below the existing fast-path range. Final measured limits and any revisions will be recorded.
- Newest patches stay weak; older interior patches mature only after successful establishments. Distinct weak spider/scorpion, mature defender and guardian silhouettes will be selected from audited local bodies/stats. Disable inherited paralysis breath explicitly. Decorative webs do not bind; sparse frontier and dense interior scenery remain separate from the existing telegraphed combat restraint.
- Dead/destroyed established broods retire their branch for this attempt. No automatic nest regrowth. Reinforcement births consume a finite attempt budget with a refill delay; clearing creatures creates real breathing room. Founder retries are bounded. Population, scenery, terrain probes, route requests and reconciliation work have separate bounds.
- Town streets can be invaded. Real guards, citizens, harmful checks and ordinary NPC respawn mechanisms remain authoritative. No global AI/guard/safe-region override. Actual protected regions and NPC invulnerability conflicts must be reported before any policy-changing workaround.
- Existing houses, doorways, boats and public portals are excluded locally. New construction or a new portal causes local retirement/rerouting, rather than cancelling an entire invasion or taking permanent real estate away from players.
- Queen death stops growth and removes only attempt-owned entities/scenery. Keep existing immediate unique gold delivery and cosmetic shower. Retain persistent earned claims and antivenom. Three capped suppression milestones plus queen credit reuse the four serialized contribution buckets; bucket meanings become milestones, not fixed geographical stages. NPC-only deaths and repeated/retired nests must not mint player rewards.
- Downtime freezes growth; no accumulated catch-up wave. Save origin, patch identity, establishment state, budgets and ownership. Bound untouched attempt lifetime to two hours; retain a 30–60 minute cooldown. Earlier engaged-abandonment behavior will be reconciled explicitly with ongoing NPC containment.

These numbers and roster choices are implementation defaults, not claimed balance acceptance. The confirmed choices are immediate queen vulnerability and genuine town overrun through existing combat rules.

## Delivery batches

1. Trace targeting, regional harmful rules, movement edges, creature abilities, reservations and serializers. Record material conflicts and the migration/write-read map before source changes.
2. Add bounded growth state/director and owned actors/scenery. Wire random origin, immediate queen, branch maturation/retirement, local conflicts and capped milestone scoring into the existing controller. Remove permanent-reservation side effects. Keep serialized legacy types and old claims readable; safely retire old active stage-based attempts without inventing victory.
3. Force runtime compilation in a fresh source-matching isolated candidate. Run focused component and old/new serializer fixtures, duplicate/restart/early-queen/ownership/budget/abuse checks, plus project-source checks for new scripts. Preserve failures and exact input/build hashes.
4. In disposable copied worlds, exercise connected growth, defended frontier containment, actual guard/citizen combat and ordinary respawn, dynamic house/door/portal conflicts and bounded cleanup. Native client verification must inspect creature silhouettes, guardians and Web/GiantWeb artwork. Run the audited character toolkit with driver errors distinguished from balance outcomes. This is private acceptance work, not a new user demonstration.
5. Reconcile acceptance by evidence class, preserve old reports as historical, commit focused local changes and stop all owned clients/servers/macros/background jobs. Verify original saves/profiles/macros/modules remain unchanged. Never label incomplete gates live-ready.

## Acceptance register

| Gate | Required evidence | Initial status |
| --- | --- | --- |
| Terrain/origin | Random origins; movement-valid connected edges; obstacle exclusions; bounded searches | Pending |
| Growth/containment | Surviving founder required; actual guard kills contain frontier; weak outside/strong inside | Pending |
| Town overrun | Actual targeting/damage/death of guards/citizens and unchanged normal NPC respawn | Pending |
| Presentation | Client-observed distinct bodies/guardian and Web/GiantWeb1–6 art; sparse/dense gradient | Pending |
| Bounds | Measured creature/scenery/birth/probe/path/reconciliation maxima and downtime freeze | Pending |
| Dynamic conflicts | New houses, doors, portals; local handling and independent event/legacy ownership | Pending |
| Persistence | Old claims/controller saves readable; new reload has no duplicate queen/nests or catch-up burst | Pending |
| Rewards | Early queen kill; finite milestone payouts; support/pet/bard provenance; NPC/regrowth/replay abuse | Pending |
| Combat/toolkit | Full audited toolkit; movement-only web warning/dodge/self/ally rescue and antivenom | Pending |
| Closeout | Exact source/runtime provenance, retained failures, local commits and verified owned teardown | Pending |

The next record will identify source traces, migration details and concrete region/NPC conflicts. No prior sparse-stage pacing result validates this design.

## Current source trace and migration review

`ArachnidOutbreakController` becomes a partial class without changing its full serialized name or base type. Controller version3 retains the complete version2 state/creature/sign/search/site/boss/ledger/wounds prefix, then appends optional infestation state. Versions0–2 consume their original layout and default the new field to null. Old active staged attempts retire without victory; independently stored claims remain. `ArachnidOutbreakState` version1 retains its complete version0 prefix then appends the infestation-mode bool, allowing a live queen before suppression bits are set. Old version0 boss validation still requires the former seven brood bits.

The new infestation value state/version0 writes origin, successful-growth epoch, total births, reconciliation cursor, online start/next-growth/last-tick timestamps and a bounded patch list. Each patch/version0 writes identity/parent/birth/failure/birth budgets, center, guardian/founder references, establishment/retirement flags and establishment/refill timestamps. New owned creature/version0 follows BaseCreature with controller, attempt, patch, tier, variant, guardian/founder flags. New decorative web/version0 follows Item with controller, attempt and patch. No map queries or reconstruction occur during Deserialize; reconstruction waits until the graph is loaded. Existing creature, claim, antivenom and combat-web serializers/types remain unchanged.

`TownGuards.IsEnemy` uses `IntelligentAction.GetMyEnemies` and actual configured patrol/death-sentence rules. The helper excludes citizens/vendors as *targets*; the event actors need a narrowly scoped town-target decision that still checks `CanBeHarmful`, visibility and LOS. Guard targeting itself remains unchanged. `Citizens.IsEnemy` returns false and its AI is noncombat; citizens are intended victims rather than substituted fighters. `BaseVendor.IsInvulnerable` defaults false. Actual `ProtectedRegion`/`SafeRegion` harmful denials remain barriers, rather than being silently bypassed.

Movement uses `Server.Movement.Movement.CheckMovement` for connected edges, then the existing short `PathFollower` for founder travel. New event actors remain active without nearby players, bounded by the event population, rather than silently freezing colony behavior with sector sleep. This is event-local, not a shared AI rollout. The first compile candidate retained two implementation errors (a shadowed local and the shard's renamed FistFighting skill); those are corrected in the working candidate and not accepted evidence.

Local roster values audited from LargeSpider (20–30HP/2–8 damage), Tarantula (146–160HP/10–20 damage), MonstrousSpider (346–360HP/22–28 damage), Scorpion/DeadlyScorpion and AbyssCrawler bodies. Event actors explicitly configure their own stats and breath policy; they do not inherit every imported ability. The custom 200HP/100–150 damage Spiderling is not used. GiantWeb constructors are the authority for component IDs/orientation; native appearance remains a pending gate.
