# FIX-03: consistent caps and repeatable origin selection

2026-10-20, `main`, accepted baseline `e27a34891466ab85c27d3b60c0768ed716730a30`. The six-file candidate passed isolated component, full-world, real-client and supervised Qwen acceptance. Independent reviews cover the source, fixtures and resulting evidence. No deployment or autonomous play is claimed. See the [design](FIX-03-DESIGN.md), [source review](../codebase-audit/outputs/subagent-findings/fix03-design-review.md), [client review](../codebase-audit/outputs/subagent-findings/fix03-client001-review.md) and [Resident feedback](FIX-03-RESIDENT-FEEDBACK.md). The controller handoff records the focused local commit after Git completes.

## Resulting behavior

The supported cap is origin plus configured/saved boost plus Ether. Origins are10000/11000/13000/40000, boost is0..10000 in1000 increments, and Ether is0 or5000, all in tenths. Origin setup, display, load reconciliation, diagnostics and Ether completion share that calculation. The gypsy's comparison text still intentionally excludes an actor's existing Ether bonus; it describes the origin/configuration cap.

Staff-returned characters may select an origin again and receive its existing intended resets. A fresh selection is allowed regardless of skill total, age or prior origin. Each gump response is bound to its actor, race, displayed page and original opening map/location, including across navigation. Current account discovery and configured destination eligibility are rechecked without changing another character's discovery record. Invalid human special-card setup is rejected before discovery/travel, without publishing the origin early and thereby skipping its region handler.

The source-defined reset differences remain: human ordinary cards only travel/discover; alien setup clears bases/backpack and changes equipment; savage setup retains bases/backpack while changing equipment; wanted setup retains bases/backpack while setting its story/profession/kills. Existing same-origin savage/alien region skips remain. Creature choices retain their existing equipment/backpack reset, with fugitive cap/setup only on cards2/4. A cap reduction alone deletes no skill points; FIX-02 rejects further positive grants while the real sum exceeds cap.

Loading preserves supported saved boosts when configuration falls and raises them to the supported configured floor when it rises. Legacy inference applies only when all three stored components are zero and the old cap is one of eight recognized tiers. Unknown or conflicting records remain unchanged for review. No saved field, type, version, constructor or read/write order changed.

Ether completion requires a live nearby actor/tile, owned completed tip, all four element flags and consistent supported cap components. It publishes the Ether marker/cap/stat cap before reward effects. Repeated completion cannot add another cap increment or reward batch, even if stat cap is subsequently reduced. This is at-most-once completion, not an atomic transaction across every downstream reward callback; a later callback exception could leave partial rewards and must not be hidden by clearing the marker.

## Source traces

| Path / symbol | Scope |
| --- | --- |
| `Data/Scripts/System/Misc/Settings.cs:1381`, `SkillOrigin`, `TryGetSkillCap`, `SkillBegin`, `RestoreSkillCap`, `TryCompleteEther`, `SkillBase` | Shared supported calculation, cap-only setup, load policy and completion boundary. Settings class remains assembly-internal. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs:1774`, `SkillVerification`; `:4929`, `Deserialize` | Component-based non-destructive diagnostics; fugitive profession check uses origin; reconciliation after the unchanged version fall-through. |
| `Data/Scripts/Mobiles/Base/PlayerSettings.cs:51,141,195`, `SetSavage/SetSpaceMan/SetWanted` | Validate live player/backpack and cap setup before existing reset effects. |
| `Data/Scripts/Mobiles/Civilized/ShardGreeter.cs:72,203,903,1182` | Context opening, actor/page/anchor guards, current eligibility and finite response dispatch. The transport also independently removes its connection-owned gump before response (`Data/System/Source/Network/PacketHandlers.cs:1273`). |
| `Data/Scripts/Quests/Pagan/ApproachObsidian.cs:24` | Guard actual completion and invoke shared marker/cap transition before tip deletion/rewards. Existing natural riches timers retained. |
| `Data/Scripts/Quests/Pagan/ObeliskOnCorpse.cs:37` | Acquisition also recognizes the stored completion marker. |

Existing registration and access remain unchanged. Ordinary players reach the gump through the greeter context menu and regions through travel; the tile is triggered by movement. New guards cover actor/deletion/life/backpack/map/range/current page/current components. No new command, hook, project include, runtime-visible test script or core-source change was added.

## Verified evidence

External evidence root: `D:\ConficturaUOTestTools\ordered-repairs-20261018\fix03`.

| Evidence | Result and boundary |
| --- | --- |
| `nonui-candidate-004/build-validation.json`, `evidence/runtime-compile-service-console.log` | **RuntimeScriptCompileTruth:** forced `-service -compileonly -nocache` passed in43.615s. All6598 production runtime scripts compiled without evaluator overlays or world loading. |
| `nonui-candidate-004/core-provenance.json` | Accepted FIX-02 Release/x86 core reused after comparing all125 core inputs. No new `Server.csproj` build was needed for this script-only candidate. Root tracked executable remains unchanged. |
| `nonui-candidate-004/fixtures/caps-001` |933 component assertions passed: all88 origin/boost/Ether combinations, configuration floors, diagnostics, excess preservation/grant rejection, legacy component values, stale/foreign/undisplayed/consumed/race/discovery/navigation cases, actual origin methods and completion/acquisition methods. No socket, loaded save, client or timer thread. |
| `nonui-candidate-004/fixtures/legacy-001` |704 assertions across11 synthetic typed-format records passed through real `PlayerMobile.Deserialize`: versions34/35/36/37; recognized and unknown pre35 caps; supported, conflicting and invalid later components; exact stream/sentinel and every base/cap/lock. These are synthetic old-format records, not historical world replay. |
| `world-001-evidence/prepare-result.json` |71855 checks passed in an actual disposable initialized world: real registered destination regions and same-origin skips, creature reset differences, exact five souvenirs/one gate, all-owned-tip removal and foreign-tip preservation, natural riches timers and repeat rejection, plus full save. Most checks compare inventory rows. |
| `world-001-evidence/verify-result.json` |223 checks passed after full reload:110 fixture players, including all88 supported combinations,8 recognized all-zero legacy component records and5 unsupported records, plus origin/Ether controls. Real full-world serialization and reload; no authenticated client. |
| Original/reloaded inventory and progression files under `world-001-evidence` | Independent review matched70281 root-owner/serial/type/amount rows and1398 progression rows. Progression means the fixture's explicit `State()` fields: cap components/cap, profession/stat cap/raw stats/player flag/stat locks/keys/cache and every skill base/cap/lock. This is not an assertion that every serialized field was compared. |
| `serialization-call-comparison.json` and independent source diff | Ordered reader/writer calls and saved versions unchanged. No schema migration or project-source inclusion change. |

The full-world fixture records startup normalization separately:787 previously empty bank boxes were removed by existing startup cleanup and795 materialized through the normal getter before the strict baseline. All original non-box rows and recorded progression remained exact. The later strict comparisons include the normalized containers. Riches evidence proves natural timers produced a nonzero result and that a repeated completion created no additional counted riches; it does not assert an exact total currency amount or every potential map cell.

Preserved compatibility checks on candidate002 passed1853 Food,46 Research,817 FIX-01 protection plus60 registered-region transitions, and the unchanged2493-assertion FIX-02 grant fixture. The core and those tested production implementations remain source-identical in004; only greeter response validation changed afterward and was recompiled/retested above. Do not describe those earlier runs as fresh004 runs. Accepted FIX-05/FIX-02 client/network evidence remains their own accepted scope, not new FIX-03 client evidence.

Rejected attempts are retained. Candidate001 reproduced the rolling navigation-anchor failure with the corrected fixture;002 fixed it. The expanded human special-card fixture fails002 because invalid setup could partially travel;004 passes the same fixture. Candidate003 failed script compilation on a local variable-name collision;004 renames the local and passes. No failing assertion was weakened. Earlier fixture-only reflection adaptation kept the production settings class internal.

## Candidate identity and cleanup

Candidate004 source manifest SHA-256: `67e8fd7c6cd80eb8bd30d3d76b0fdc4b768ddd1b30a61803eb834c5d547da8da`. Production script assembly: `fe1bbb93134b7ac0706daf7816386894b6bdc273e134b594d54416f96ea22ee6`. Unchanged core: `9f0351b5af5ea4ccafa5e897150551c6a80bbb4167061747c1df9e5852eda8c3`. Six source hashes are recorded in the manifest and independent review. World evaluator/isolation overlays and each actual runtime assembly have separate pins; they are not production deliverables.

World processes37952 and42828 stopped after their milestones by exact-owned forced termination, not graceful shutdown. Exercised saves are archived and all20 copied save files restored (`world-001-evidence/restoration.json`,22:09:29Z). The22:10Z client pause was explicitly lifted at23:01Z. Fresh read-only native discovery and preflight checks preceded client001 startup; all14 protected inputs, sources, core and five assigned ports passed.

## Real-client acceptance

`client-001` was freshly prepared from the restored client004 configuration lineage, accepted FIX-02 lifecycle tools and candidate004 production scripts. The only evaluator/isolation scripts are external. Endpoint127.0.0.1:25215, ordinary Player Nephtest521, existing guarded ClassicAssist bridge25291/control25292 and local Qwen21447 were verified. Saved login worked without manual credentials. The exact guarded macro was selected and started through native controls. Native screenshots in the controller history show cards, destination rendering, Ether riches, disconnect, reauthenticated actor and restored actor.

| Actual-client check | Independent result |
| --- | --- |
| Ordinary human card,23:22:43Z | Actor moved to2999,1030,0 Sosaria; origin13000/Ether5000/cap18000, real total17000 and backpack marker retained. Displayed gump consumed. |
| Alien card, supervised Qwen then23:27:00Z check | Client-visible observed gump192/button113 selected; actual arrival4109,3775,2 Sosaria, origin40000/Ether5000/cap45000, bases zero, prior backpack marker deleted and equipment reset. Native shuttle rendering inspected. |
| Same-origin alien again,23:27:27Z | Actual same destination/gump consumption; newly seeded base123 and marker23 retained, proving existing region skip. |
| Savage card,23:27:44Z | Actual251,1949,-28 SavagedEmpire; origin11000/Ether5000/cap16000, base123 and marker retained; native hut/equipment view inspected. |
| Wanted card,23:28:07Z | Actual4104,3232,0 Sosaria; origin13000/Ether5000/cap18000, base123 and marker retained, profession1/kills1; native jail view inspected. |
| Ether first crossing,23:28:50Z | Real client made one adjacent step onto actual tile; independent crossing counter1, tip deleted, Ether5000/cap18000/stat cap300, exactly five souvenirs/one gate. Native riches/gate view inspected. |
| Ether repeated crossing,23:29:10Z | Stat cap deliberately reduced to250, new completed tip and a second actual movement/crossing2. Tip retained, cap/Ether unchanged, still five souvenirs/one gate. |
| Authenticated recovery,23:32:22Z | Deliberate server disconnect observed in native client and independently as detached old NetState. Fresh client process authenticated to the same Player, with a distinct running NetState and exact expected cap, bases, inventory and location. Guarded macro reattached successfully. |
| Online restoration,23:33:02Z | Original scalar/progression/inventory parent/layer/owner and location/logout controls restored. Original cap10000/Ether0/real total1011. Pending world queue0; pending bytes122 at this response, so no zero-output claim. |

Raw evidence: `client-001/evidence/actual-client.jsonl`, request/response XML, `acceptance-checks.json`, model request/response files and `gameplay-assembly.json`. Actual gameplay assembly SHA-256 `6135f4c37389ff6701ce4bc7d93814bb60c3e39da7f3677ef51cbc1c69c18d23`; manifest `b3d3766036117d6e7749095199b445bcfb6a8434a1b3cb0d72fbd01fbb7e32c6`. All6598 production scripts match candidate004;6599 includes the external evaluator. All8 tools and92 runtime-code files were independently rehashed. Clean compile/startup had no region creation errors. Test overlays are not committed.

Failures remain recorded. An initial evaluator compile failed on unsupported collection Contains and was corrected with explicit iteration; production candidate unchanged. A clipped native window made the first ordinary-card click ineffective; the unchanged assertion failed until the window was repositioned and the actual card clicked. Guarded observations initially took4.29–4.73s, exceeding the unchanged3s freshness limit. The copied guard previously parsed the same scene JSON on every path node; the scoped copy now refreshes an immutable parsed allowlist at each identity check when file metadata changes, failing closed for missing/malformed prepared scenes. Every observation field and identity/freshness/STOP/deadline rule remains. A fixture-only tuple/float typo was caught and corrected before reattachment. Corrected complete snapshot0.975s includes waypoint subfield0.664s. No original Resident change or new MCP endpoint.

Recovery qualification: the connection-lost dialog did not respond to supported acknowledgement attempts. Closing the initial client raised ClassicAssist's existing Newtonsoft.Json JToken.ToString(Formatting) MissingMethodException during options save; initial exit3762504530 was abnormal. A fresh saved-login process supplied the successful authenticated recovery. This is not clean same-process reconnect or normal-shutdown proof. Initial client also logged four packet78 'need more data' warnings; replacement logged none. The checked FIX-03 outcomes passed, but broad client/protocol compatibility is not established.

Final cleanup at23:33:55Z archived exercised saves/profiles under `client-001/evidence/cleanup-20261020T233355Z`, restored full copied Saves/settings/DataProfiles/ClassicAssistProfiles, and verified all14 protected originals/backups. Exact-owned server31984/client43632 were stopped at the milestone; initialclient24728 had already exited. Resident/model stopped and all recorded identities/subtree processes were absent; assigned runtime ports free. No live/user process was stopped. Native bindings were reset. Independent acceptance review found no remaining scoped FIX-03 blocker, with the recovery/warning/queue qualifications above.

## Tracker and commit boundary

Only existing `POST-SKILL-FIX-03` changes from Deferred to Fixed:6824 records,956 Fixed,1 Deferred (FIX-04). CSV/workbook reconciliation, native-part preservation and normal-zoom rendered checks are recorded in external `fix03/backlog`. Artifact Tool authoring/recalculation was used; its known wildcard COUNTIF date limitation remains explicitly uncredited. Native formulas are retained and all date/count caches independently reconciled against CSV. No native Excel recalculation claim.

This is a script-only repair; the accepted matching core executable and125 source inputs remain unchanged. No project include or serialized layout changed. Required source/doc/link/diff and integrity checks precede the local commit. FIX-04 implementation starts only afterward. Unrelated HTML and FIX-01 harness files remain excluded. No push, live restart, production-save or original Resident/Shardmind changes.
