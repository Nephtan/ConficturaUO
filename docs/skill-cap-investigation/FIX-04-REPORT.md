# FIX-04: controlled accounting and bounded XML rewards

2026-10-20 local / 2026-10-21 UTC. Baseline `main` at FIX-03 `2a886f4dc16d29c6075d642af2e7b941a03c6337`, after accepted FIX-05 and FIX-02. Four source files and a matching production Release/x86 executable form this repair. Isolated source/component/world/client acceptance is complete; independent evidence review found no remaining scoped blocker. Tracker reconciliation and the focused local source/artifact commit are recorded in the handoff. No deployment or live-shard claim. See [design](FIX-04-DESIGN.md), [independent review](../codebase-audit/outputs/subagent-findings/fix04-candidate-review.md) and [Resident feedback](FIX-04-RESIDENT-FEEDBACK.md).

## Resulting behavior and source traces

| Source / symbol | Behavior and boundary |
| --- | --- |
| `Data/System/Source/Skills.cs:556`, raw base setter | Explicit staff/NPC/load base changes retain raw semantics, points, caps and locks. Recompute the sum before notifications and in finally after callbacks, including exceptions/reentrant raw changes. An equal-value raw assignment also reconciles the cache. |
| `Skills.cs:1470`, Total / RecalculateTotal / GetBaseTotal | Total has a getter and no setter, including a private reflection setter. Internal accounting derives from independent bases. Existing TryGrant at1493 retains its transactional real-sum/cap/lock/donor policy. Rejected/no-op grants preserve an injected corrupt cache for diagnosis; they do not delete points or silently rewrite identity. |
| `Data/Scripts/System/Skills/SkillCheck.cs:206` | Gain probability uses GetBaseTotal instead of the cached Total. Existing PlayerMobile-or-Player classification and accepted FIX-02 transaction remain. |
| `Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs:71`, IsProtectedInstance | Classifies the final resolved Mobile/Skills/Skill owner on every invocation, including cached PropertyInfo and NPC-to-player chains. Mobile.Player is protected for all mobiles; player aggregate cap/total, cap component aliases, raw skill bases and individual caps are protected. A real PlayerMobile remains protected with Player=false. |
| `BaseXmlSpawner.cs:1272`, InternalSetValue and lookup callers | Final guard runs before conversion/logging/mutation; public cached/fallback/nested routes also check the resolved instance. Read access, ordinary properties and intentional NPC skill/cap initialization remain usable. Existing AccessLevel/StaffLevel protections remain. |
| `BaseXmlSpawner.cs:1559`, SetSkillBase | Positive player SKILL requests use TryGrant; unchanged values succeed without mutation; reductions reject. Generic reflected Base/BaseFixedPoint is protected even for positive requests. The unchanged INC parser reads effective Skill.Value and truncates to whole units; it is not a promise of exactly one base point. |
| `Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs:4705`, XmlSetCommand | Administrator XmlSet shares the automatic path; null spawner or a staff argument is not an override. Help directs progression overrides to explicit staff property/skill tools. No new command/hook/access entitlement is introduced. |

No serialized type, namespace, version, field, write/read order or project include changed. Skills serialization remains version3. No migration or automatic Player-flag rewrite is introduced. Existing raw staff/Skills tools remain deliberate privileged boundaries; this is not a general sandbox against arbitrary trusted C# or every reflection operation.

## Frozen candidate and verification

Evidence root: `D:\ConficturaUOTestTools\ordered-repairs-20261018\fix04`. Candidate `nonui-candidate-001` pins all production scripts and125 core build inputs. Its source-manifest SHA-256 is `c93d1d5e0506f6b04dcb9ddb6f3a5cf3fe1939672cc3f7656deb789cc32a6460`.

| Artifact | SHA-256 |
| --- | --- |
| Skills.cs | `24fcfa0850c5f559a9304c2ab60c73143fe985eebd969ca0e85b2ab01e4ad9db` |
| SkillCheck.cs | `3e42bb6fffb2ca8094851f7bae7c0139ec6ce8e9496c7dec0fa081f136e7b6e8` |
| BaseXmlSpawner.cs | `3c99be1e1f2ef689de45612364b4186b8491b359043edbde4afd7a6eb0f624d5` |
| XmlSpawner2.cs | `a165efddb1a7c86bf6385914d416683e1fd13faad26bd74b36a9240eedd1cb09` |
| Production Release/x86 core | `a6d8a30f17d9718158fa505a9fdf99805d42956da2e6ed53c37d7a920eb345a9` |
| Forced production script compile | `0b1ffff437456cf5087c22e8112878a8c26f8d724fc6a66bec42540290eff20e` |

| Check | Result / scope |
| --- | --- |
| Operational Server.csproj Release/x86 build | Passed,22.337s, isolated output; no instrumentation in the delivered core. |
| RuntimeScriptCompileTruth | Forced `-service -compileonly -nocache` passed,41.889s. All6598 production C# scripts compiled. Compile-only does not establish world startup. |
| Accounting007 | 3,972 assertions passed. Direct/cached/fallback/nested/case/alias XML paths; final guards before conversion/log; false Player flag and low/high/negative cache; NPC/player ownership transitions; raw/callback/reentrant/throw accounting; actual gain probability controls; bounded SKILL/INC acceptance and individual/aggregate/lock refusal; staff primitive and typed serialization checks. |
| Compatibility | Food1,853; Research46; FIX-01 protection817 plus60 registered region transitions; unchanged FIX-03 cap/origin/Ether933; synthetic legacy-format704 across11 v34/35/36/37 actors. All passed on this candidate. |
| Network | 2,771 passed in actual loopback/component fixture with production callbacks manually drained. Distinct from real client acceptance below. |
| Complete disposable world | Prepare/save71,903; reload235 passed. Original70,281 inventory rows and1,398 progression rows byte-identical after reload, for the explicitly selected fields. Six new accounting players cross true/false flags with three corrupt caches; bounded reward, protected writes, locked rejection, full save reconciliation and reload state all pass. Prior region/Ether/legacy controls remain. |

External fixture adaptations are explicit: eleven Total assignments became private-field cache injection because the setter no longer exists; the FIX-01 fixture has two such adaptations. Original equality assertions remain. XML positive raw assignment/reduction expectations changed only for this declared FIX-04 policy. Earlier accounting001 regex/compile,003/004 INC expectation and005 nonexistent StaffLevel fixture failures remain archived. INC was corrected to assert the source-defined effective-value target, including a separate aggregate-only refusal; production parser unchanged. Synthetic StaffLevel tests do not invent a real Item API.

World inputs retain the prior test-only startup empty-bankbox normalization and passive Focus/Meditation control for original offline players; compare the exact inventory/progression fields documented in the fixture. They are not every serialized field or historical incident evidence. World executables were stopped by exact-owned forced termination after milestones. Copied Saves were archived and restored at00:05:36Z. No graceful shutdown claim.

## Real client and supervised Qwen

Client001 derives from restored accepted FIX-03 client001 and the verified client004 configuration lineage. All6598 production scripts match the candidate; eight isolation overlays plus one external evaluator provide loopback routing, disabled external/restart facilities and finite fixture control. The existing corrected immutable scene-cache guard remains; no new MCP endpoint, permission change or original Resident modification. Assigned endpoints: shard127.0.0.1:25215, guarded bridge25291, controls25292, Qwen21447. Actor: ordinary Player Nephtest521, authenticated account and live NetState checked.

Saved login worked through non-secret native controls. The exact FIX04 isolated acceptance macro was selected and started through native Computer Use. Autonomy remained disabled. The visible test gump is privileged **external setup**, not a new production reward feature; its actual client response invokes the production XML pipeline. During each atomic server response, the fixture temporarily sets Player=false and injects a corrupt cache, then restores the true flag in finally.

| Actual evidence | Independent result |
| --- | --- |
| Positive reward,00:12:50Z | One supervised Qwen choice selected observed gump188/button1. Production `object/SKILL,Magery/51` accepted: base500→510, independent/cache total5500→5510 at cap5510; two tokens→one. Seven protected writes rejected without changing the selected state. Native view showed the 1.0 Magery increase to51.0. |
| At-cap refusal,00:14:22Z | Exact observed gump189/button1 through the real guarded client. Production request52 rejected at aggregate cap, with individual headroom and Up lock. Full selected progress/cache/inventory unchanged during refusal; token retained. Seven more protected writes rejected. Explicit raw no-op then reconciled the injected cache without changing points. |
| Native rendering | Both test prompts and result text rendered; client skills total showed551.0. Two initial native response clicks in a partly off-screen window produced no response; the early check-refusal failure is retained. Guarded client responses supplied the actual interaction evidence. Native skills display and window controls worked. |
| Same-process authenticated recovery,00:16:59Z | Deliberate server disconnect observed and old NetState detached. Native Maximize corrected off-screen placement; connection-loss acknowledgement returned to saved login. Original client PID37804 and creation identity reauthenticated on the exact loopback endpoint. Distinct running NetState, exact progress/token inventory/location and zero queues at the check. Guard failed closed on disconnection and was manually reattached only after fresh authentication. Fresh inspect passed. |
| Online restoration,00:17:53Z | Original progression, inventory, parents/layers, scalar properties and location restored. Cap10000, real/cache1011, Magery0; native skills display101.1 and original equipment/backpack rendered. Pending world0, pending bytes493: no zero-output claim for restoration. |

Client evaluator SHA `9352223385607db60df8e57a481d7032adf4e2ec13687483f9e096ded070a997`; gameplay script assembly `e9da11c85557e6a2be68b5c6146699cae953b658f0ebf21fdcad5dae4171c34b`. Compile-only client assembly is separately recorded; startup is checked for region creation errors. Prelaunch review corrected only Skills.Total's exact read-only refusal text, preserving unchanged-state assertions and the old prepared fixture.

Raw client evidence: `client-001/evidence/acceptance-checks.json`, `actual-client.jsonl`, `xml-actions.txt`, request/response XML, `same-process-reconnect.json`, `reconnected-endpoint.json`, `gameplay-assembly.json` and `cleanup-result.json`. Native screenshots are in the sole controller chat history; reviewer verification of recorded server/bridge results is separate from native observation.

Observation limitation: the first interaction after-snapshot timestamp00:12:50.793Z preceded the server action00:12:50.822Z by about29ms. It still showed gold2, Magery50.0 and total550.0; it cannot prove post-action convergence. The next fresh client inspect before the second response showed one, with server response count still one; it occurred81.6s later. An initial closeout checker that assumed immediate cache convergence is archived. Neither immediate cache convergence nor a bounded update latency is claimed. The actual reward and retained-token assertions are supported by independent server state and subsequent real client observations.

Client stdout retains16 packet11/D6/DD/1C `need more data` warnings, deliberate-disconnect socket errors, container warnings during temporary inventory restoration, asset/BAR warnings and288 scissor/viewport messages. Both gumps rendered, production responses and recovery passed; packet formats were not changed by FIX-04. This is not warning-free general protocol/rendering compatibility or normal-shutdown proof. FIX-03's historical replacement-process recovery limitation remains unchanged; this later FIX-04 run separately proves same-process recovery.

## Restoration and delivery

At00:18:07Z, finite supervisors stopped owned server30752/client37804; Resident and its model closed. Native bindings reset. Cleanup00:18:23Z verified recorded identities/subtree processes absent and assigned ports free, archived exercised copies, restored complete disposable Saves/settings/DataProfiles/ClassicAssistProfiles and verified all14 protected original/backup records. Original progression/inventory text comparisons are exact. No live/user process, production save, original Resident or Shardmind file changed.

The required matching core replaces the tracked executable only after current125-input source verification and confirming no production process uses that repository path. Artifact provenance is retained externally; no evaluator, overlay, symbols, cache, credentials or copied saves enter Git. Tracker changes only existing POST-SKILL-FIX-04:6824 records,957 Fixed,0 Deferred. Native workbook formulas/features are preserved; CSV, numeric totals and all date caches reconciled. The known Artifact Tool wildcard COUNTIF/shared-formula limitation is not credited as a pass; native Excel recalculation was not run.

No IDEProjectHygiene build was needed because project files and membership did not change. No live deployment, production-save test, unobserved persisted XML configuration audit, arbitrary reflection sandbox proof, autonomous Qwen acceptance or normal application shutdown is claimed. Intentional compatibility change: saved/deployed automatic XML expressions that write protected player fields now reject; use the bounded SKILL route or explicit staff restoration tools according to their purpose.
