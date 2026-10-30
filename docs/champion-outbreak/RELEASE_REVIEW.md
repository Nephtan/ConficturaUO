# Arachnid Outbreak prototype review

Status: completed local prototype. The user explicitly approved all six footprints; six reservation flags and derived site descriptors are reviewed. Production activation remains disabled. See GEOGRAPHY_APPROVAL.md for actual-policy verification. No production save, live server launch or deployment occurs.

## Approved geography

The user approved the six permanent161x161 wilderness reservations below. Each contains the113x113 combat area plus24 tiles of buffer. They are alternate locations for one available outbreak. An approved reservation must stay protected from housing while the event is idle or removed; city founding also checks maximum future growth. That explicit reply completes the exact-footprint decision.

| Source ID | Center on Sosaria | Connected combat tiles |
| --- | --- | ---: |
| central-woodland | 2514,2088 | 12,478 |
| northern-woodland | 2242,652 | 12,534 |
| eastern-woodland | 3746,876 | 12,439 |
| northwestern-woodland | 1208,928 | 11,553 |
| southeastern-woodland | 3762,2744 | 12,422 |
| western-woodland | 1122,1704 | 12,634 |

The existing [local reservation atlas](D:/ConficturaUOTestTools/outbreak-map-survey-20261028/proposed-reservations.png) displays these exact centers/counts. Magenta is proposed permanent protection; gold is combat; blue marks brood anchors. Short map aliases north/northeast correspond to northern/eastern above. The selected CSV and current source coordinate tuples match. The image is a local review artifact, not published copied-save data.

The user-selected October17 snapshot passes all six current preflights with186 static Sosaria regions: no house areas, maximum city-growth areas, valid legacy encounter areas or portal items intersect the checked reserves. All six native routes pass1,356 steps. Six boss and two brood presentation samples retain western tree/eastern foreground overlap; the red warning/message remains discernible in those samples. These are normal foliage caveats, not universal visibility or approval claims. See [RESERVATIONS.md](RESERVATIONS.md), [SITE_TRAVERSAL.md](SITE_TRAVERSAL.md) and [PRESENTATION.md](PRESENTATION.md).

The approved catalog/reservation flags are populated and fresh preflight003 passes using actual protection predicates. DeliveryReady remains false. Later deployment still needs its own decision and checks for changes since October17. The inactive malformed legacy controller reported in RESERVATIONS.md remains untouched; its intended saved area must not be guessed.

## Requirement reconciliation

[ACCEPTANCE_REQUIREMENTS.csv](ACCEPTANCE_REQUIREMENTS.csv) retains44 individual source/test/evidence records and adds a final-review disposition. Evidence classes remain distinct. The former four geography/operational rows now record ApprovedPrototype; other rows remain accepted only within their recorded scope.

| Requirements | Reconciled evidence and limits |
| --- | --- |
| R01-R08 | Separate director, lifecycle/cooldown, placement and legacy coexistence have component/native records. R03/R04 now include explicit approval and real-policy preflight; R05 is clearance on the explicitly selected snapshot. No public marker/broadcast is added by the outbreak source. |
| R09-R13 | Saved stage effort, finite reinforcement budgets, death/deletion/ownership guards and bounds have component/fresh-process evidence and actual40-enemy clears. Only outbreak spider breath is disabled; ordinary Dread Spider and shared AI remain. |
| R14-R18 | Current92 production-method web assertions, natural live003 reaction/self-escape, live-dodge0022AP avoidance, controlled ally rescue and separately correlated Computer Use samples cover the named interactions. Stopped-scheduler rendering never supplies timing/AI proof. |
| R19-R23 | Damage/pet/provocation, meaningful crowd qualification, genuine wound/poison provenance and bard time have current-source regression records. Six controlled native role types are independently verified; synthetic setup, paused AI and real-clock bard sampling remain explicit. |
| R24-R28 | Stage awards/claims, shared gold/skull/artifact routing, no new scrolls and recipe/failure rules have component/fresh-process evidence. Actual archmage clear queues6 glands/1 skull and drops15,000 gold. Native005 claim visibility/collection uses a declared scripted stage entitlement; it is not called combat earning. |
| R29-R33 | Immunity/provenance/order, charges/expiry/reuse/party/pets, serialization and status have distinct component, reload and native cases. Native004 confirms ally treatment, cure and prevention. Reconnect behavior retains its persistence/source evidence; no new native reconnect claim is made. Two complete references use ordinary cures without antivenom. |
| R34-R36 | Full toolkit review includes20 controlled native actions plus travel/pet rescue/regeneration. Frozen legal mounted-v4 completes three comparable clears; median24.426736 minutes. Actual original Zanthus macro supplies133.473s stationary partial and277.502s assisted clear, with no defeat/minimum-duration requirement or repeatable-farm inference. |
| R37-R39 | Automatic tuning passes0. Commands, hooks, positional serializers and project/runtime inclusion are reconciled below. Core artifact matches accepted source. Broader NPC AI remains read-only. |
| R40-R41 | Chosen-snapshot clearance, traversal, native interaction categories and presentation samples are recorded. R40 approval and real-policy preflight are complete. Foliage and narrower native scopes remain visible. |
| R42 | Three finite supply/base-price observations and two bounded archmage records are complete as an observational record. Gold/corpse/claim pickup in references is unknown, not0; no profit, causal antivenom saving, market demand or economic-balance pass is claimed. |
| R43/D01 | Reproducible fixtures, preserved failures, exact owned teardown and focused commits are retained. Operating/rollback steps exist. The approved catalog is completed/verified; no additional AI change is approved or needed for the accepted pacing evidence. |

## Current source and carry-forward

The October30 [gold placement repair](GOLD_SCATTER.md) supersedes the earlier quantity-only presentation evidence: the watched clear contained21 overlapping piles at its fixed anchor. Current source samples25 distinct safe tiles around the actual death. Its focused verification and limits are recorded separately; the held showcase client still uses the earlier runtime.

[DELIVERY_INVENTORY.json](DELIVERY_INVENTORY.json) checks all6,742 current production inputs,13 outbreak project entries,44 source/test path pairs, matching core, six approved reserves/catalog entries and disabled delivery. This is static inventory, not a substituted runtime build. The approved-geography candidate and fresh copied startup001 pass forced runtime script compilation and world startup; GEOGRAPHY_VERIFICATION.json records their separate scopes. Earlier copied006 retains its original test-extension evidence. Earlier operational Release/x86, project hygiene and fresh-process reload evidence retain their original scope.

The accepted tracked core is dbb7b2360c435834b32e0f1f4b0854a859cd9086971a4c270981b58d8f2f3ddb. Subsequent production changes include the null-safe rejected-outsider log, the two-file static geography approval/catalog batch, and the October30 gold placement repair. GEOGRAPHY_APPROVAL.md records the unchanged central private-fixture policy and fresh actual-policy tests; that geography batch changed no combat/reward rule. [TBC_REJECTION_FIX.md](TBC_REJECTION_FIX.md) reproduces the old fault and verifies the actual manager/core bridge. Real-time pacing and Zanthus cases have TBC disabled, returning Native before that branch; offline preflight never enables it. Those records are carried forward with original hashes, not relabeled as replays. Web92 and corrected native chained presentation additionally pass. The later gold repair has its own forced compile/component record and has no native visual acceptance yet.

Source review confirms that placement excludes four temporary spell-gate families, while active structural safety ignores those short-lived gates so player travel cannot cancel an outbreak. Permanent portals remain structural conflicts. The existing PortalClearance regressions exercise both policies; no gate/player ability change was needed.

Positional serialization remains: controller version2 writes base, state, creature/sign references, next-search/last-site/boss-created, optional ledger and wounds; old reads guard ledger at version1 and wounds at2. Claim version1 retains owner/attempt/stage, kind and the reward-reference list; old reads default kind before1 and consume the same reward-list position. State/ledger/wounds and the treatment/effect/web/creature types retain their existing versioned layouts. Adaptive qualification derives from existing ledger data without a new serialized field. No class/namespace move, field reorder or serializer change occurs in the final repair/test batches.

Entry points remain scoped: controller Initialize subscribes resolved health/poison/death observers and registers the Administrator command; claim and antivenom status are Player commands with caller/alive/backpack/recipient guards. Controller timers require readiness; startup restores loaded state without creating/enabling a director. Test extensions live under tests/, are copied only into guarded private TestOnly trees, and add no production project entries. Relevant sources are recorded per requirement and in the feature evidence documents.

## Operation and limits

The source-verified [operating table](DELIVERY.md#prototype-operation-and-enabledisable-gates) permits an Administrator to create one disabled controller even while DeliveryReady=false. Start currently refuses. Stop removes only that attempt's owned entities and does not remove permanent protection or erase independent pending claims. Player OutbreakClaim and BroodAntivenom remain ordinary guarded interfaces. Future approved activation must validate matching source/core, forced compile, copied-world reload, start/status/stop and legacy coexistence; private fixture reflection is never an operational procedure. Rollback must retain serialized classes needed by saves.

[ECONOMICS.md](ECONOMICS.md), [COOPERATIVE.md](COOPERATIVE.md), [NATIVE_ACCEPTANCE.md](NATIVE_ACCEPTANCE.md) and the dated delivery history preserve exclusions and failed attempts. Forced no-save termination is not graceful shutdown. Original saves, profiles, macros and modules are preserved. Prior delivery verification stopped its disposable processes; the subsequent October30 watched copy remains open for review with its fixture/controller disarmed. Three unrelated untracked user files remain untouched. Enjoyment, sustained market demand, broader beginner progression and live deployment are not established by this prototype.
