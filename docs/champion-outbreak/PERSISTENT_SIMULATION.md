# Persistent spreading encounters

Current local arachnid candidate, October 31, 2026. Production activation remains disabled. Acceptance continues. This is the current contract; fixed reservations/stages and the provisional 16-patch/400-birth/two-hour limits are historical.

## Behavior and budgets

The queen exists and is vulnerable immediately. Actual founders must reach adjoining connected ground and survive 45 seconds before establishing patches. Five ranks provide weak frontier, stronger interior, caster/melee variants, poison and different silhouettes. Giant art is reserved for mature guardians. Actual guard/citizen combat, regional harmful rules and ordinary spawners remain authoritative. No virtual offscreen NPC victory or global guard/AI rewrite occurs. Houses, doors, boats and permanent portals cause local conflict handling; the old six reservations impose no active infestation housing restriction.

Queen death ends owned growth/entities. Three suppression milestones and queen credit retain existing participation/claim rules, antivenom, boss collectibles, 15,000 scattered ground gold and the cosmetic shower. NPC-only outcomes and cleanup mint no player victory rewards. Winning is not a test requirement.

One shared 250 ms round-robin scheduler serves all directors. Per-second allowances: 4,096 terrain checks, two routes, six new actors and 24 scenery parts, with a five-millisecond background terrain yield. Ordinary admission targets are 160 actors and 240 scenery parts across directors. Waiting origins reserve two bootstrap admission slots each and retry quota deferral after two seconds. These budgets defer new work; they never delete, freeze or slow existing player-relevant actors.

Denied resource demand survives quota-window boundaries in per-resource FIFO queues. Unused demand is withdrawn after the director's next visit and unregister removes all pending demand. A granted director can finish its current bounded work batch; owed player admission bypasses background queues. This repairs a reproduced fixed-window alias where equal visits still starved actual terrain/route grants. WorkGrants reports actual grants per registered director (terrain/routes/actor/scenery indices 0–3); it is not a capacity metric.

Players, controlled pets/summons, pursuit, effects, unexpired damage/aggression and relevant NPC contests preserve actual ordinary AI. Combined player envelopes apply. Owed entry admission can exceed the soft target. Scoped counted sector leases keep simulated event actors active. Sector.MobileEntering runs before entry snapshots even in active sectors; player/companion admission uses a 56-tile halo, relevant NPCs 32. Quiet compaction requires 90 seconds without interest plus conservative state checks. Hidden, other-facet, off-patch or modified/equipped actors and ongoing effects prevent it. Registry counts are leased actors, not every running AI timer; ordinary queen sector-deactivation grace remains.

## Persistence and limits

Twelve-tile indexed territory is bounded by finite Sosaria geography, with indexed frontier/live work lists and attempt tombstones against repeated occupation/reward farming. No lifetime nest/birth/time cap replaces background budgets. Compacted survivors retain logical GUID, role/tier, wounds, maximum health, stamina/mana, position, special-action time and exact detached inventory. Hydration transfers that inventory. Deleted references cannot reset a survivor. Physical serials may change or be reused across processes; logical identity is the contract. The queen remains an actual Mobile. Downtime advances neither growth nor refill.

Controller version 3 preserves its prior prefix. Embedded state/patch formats read version 0 and write version 1; resident values use version 0. Creature version 1 appends identity/slot/relevance/special-action data and reads version 0. No serialized type/namespace rename occurs; source write/read order is authoritative.

The synthetic geography envelope contains 109,392 cells/1,422,096 residents and 118,362,208 serialized bytes, with roughly 225 MiB peak working set. It excludes a loaded shard, actual retained world inventories, terrain and live admission. It does not prove full-map capacity. Core is x86 without LargeAddressAware; machine RAM does not expand its address space. Actual inventories and multiplayer admission remain additional costs.

## Evidence register

| Record | Scope and limits |
| --- | --- |
| candidate006 | Forced runtime compilation and seven method suites pass; final routine-body revision and resource fairness repair. No live balance/capacity inference |
| expanded protection/fairness | 911 source-method checks include combined players, companion state, approach, active-sector teleport, harmful intent and actual aligned terrain/route/actor/scenery grants; synthetic state, no packet-order/projectile execution proof |
| native003/candidate002 | 16 actual directors establish; quiet compaction; connected return restores 11 to 36 actors without increasing 40 births; all 36 AI running. Earlier art/guards, uneven growth, no throughput claim |
| native005/candidate004 | One natural colony exceeds 16 established non-retired cells and reaches all five ranks; no moved founders/removed defenders. Connected return 75 to 212, births unchanged at 233, all 212 ordinary AI running. Town/respawn and selected cohorts continue; not final resource-fairness source |
| native007/candidate006 | Fresh final-source 16-director grant telemetry acceptance underway; no pass inferred from launching a runner |
| Dual save/load | Three separate processes preserve queen, compact/live identity/resources/inventory and once-only claims; fresh fixture world, not full gameplay initialization |
| old writer/current reader | Prior controller/infestation/actor format loads and retains claims; exact candidate provenance applies |
| Zanthus/candidate004 | Original macro/helper, 473.54-second queen kill, 10,968 direct + 457 pet damage, 15,000 gold/four glands/one skull. Idle gaps and obscuring gumps retained |
| Sargar/candidate004 | Saved seven cats plus saved mount; 33.645-second driver-limited loss retained. Corrected policy completes in 265.229 seconds with actual pet/veterinary/finite items/native walking. Reported gold zero is unassessed because the old inspector checks original bounds while this fight moved far away |
| selected offline builds | Separate clone fidelity commit/21 method checks; native beginner/caster/martial/hybrid/group cases pending. Companion and owner-bound book gaps remain explicit |
| earlier town/conflict cases | Actual deaths, founder containment, XmlSpawner respawn/local conflicts, but earlier capped source; final-source revalidation pending |

Overlapping local runtimes are not clean throughput experiments. The user-reported six-vCPU/20-GB/NVMe/AMD-9684X VPS has not been accessed or load-tested. Typed October 17 census: 14 regular controllers, ten configured active, 12 with surviving waves, no living bosses, two restart timers and 20 related XML controllers. These are distinct historical categories, not a current production census. Sixteen arachnid test directors are real shared-infrastructure stress, not dummy future variants or a configured limit.

## Source traces and remaining work

- `Data/Scripts/Custom/Champions/Outbreak/SpreadingEncounterScheduler.cs`: Initialize, timer/Service, facet claims, Sector.MobileEntering/EventSink.AggressiveAction, interest/busy guards, leases. Engine hooks grant no player privilege.
- `ArachnidInfestation.cs`: guarded origins, founder travel/hold, compaction/hydration, retirement, milestone/queen outcome. `ArachnidInfestationState.cs`: indexed positional formats/Resume. `ArachnidInfestationActors.cs`: ordinary AI, five ranks, harmful/visibility/LOS town targeting.
- `ArachnidOutbreakController.cs`: Administrator create/start/status/stop and explicit serial when multiple; ownership cleanup, contribution hooks, version 3. Start refuses while DeliveryReady=false.
- `Data/System/Source/Sector.cs`, `Map.cs`: counted leases and entry-before-snapshot dispatch, commits `4baaff4c`/`ca74d8b5`, matching core SHA `8746b2340e5116ad3b980afb0c1aef4466d86a9d658b8dc022d74f04e7cbf31b`.
- `Data/Scripts/Scripts.csproj`: scheduler include; recursive ScriptCompiler discovery still defines runtime visibility.
- `tests/PersistentInfestationRegression.cs`, `InfestationStorageEnvelope.cs`, `LegacyChampionControllerInventory.cs`, `ArachnidInfestationWorldReload.cs`, `ArachnidInfestationNativeAcceptance.cs`, `ArachnidArchmagePreparation.cs`: separate method/storage/typed-save/native/client evidence. Fixtures enter guarded copies only.
- `tools/champion-outbreak/FixtureHeartbeat.py`: exact root/core hash/launch PID/path/immutable instance/freshness; refuses stale/rebound fixtures. File liveness is not an OS process-handle proof; no privilege fallback.
- `SHARED_SIMULATION_BASELINE.json`, `SECTOR_ACTIVITY_VERIFICATION.json`, `PERSISTENT_SIMULATION_VERIFICATION.json`: source/runtime manifests. Private saves/accounts/build inventories remain outside outgoing history.

Complete sustained fairness review, final-source town/presentation/conflict cases, selected clone cohorts and audited active-client coverage. Preserve failures/deaths and original files, commit reviewed allowlists and verify exact owned teardown. No production activation, deployment, push, broad AI rewrite or VPS access is authorized. Old staged benchmarks do not validate this infestation.
