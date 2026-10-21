# FIX-04: controlled skill accounting and XML mutation

Started after accepted FIX-03 local commit `2a886f4dc16d29c6075d642af2e7b941a03c6337`, following FIX-05 and FIX-02. Work remains on main. This design is implemented and verified in the [acceptance report](FIX-04-REPORT.md); source and completed evidence passed [independent review](../codebase-audit/outputs/subagent-findings/fix04-candidate-review.md).

## Source boundary

`Data/System/Source/Skills.cs`: make Total genuinely getter-only, so full-trust reflection cannot invoke a private setter. Deliberate raw Base/BaseFixedPoint changes recompute from every base before callbacks and in finally afterward, including a no-op setter used to reconcile accounting. They retain staff/NPC/load semantics, caps, locks and all points. Accepted TryGrant already validates independent totals, publishes complete donor/target state and reconciles after callbacks; retain it. Rejected/no-op grants do not invent a progression correction: a deliberately injected old corrupt cache can remain diagnosable until an actual mutation/save/load reconciliation.

`Data/Scripts/System/Skills/SkillCheck.cs`: derive gain probability from GetBaseTotal, so a high/low/negative stale cache cannot alter that probability. Retain the existing PlayerMobile-or-Player classification and FIX-02 transaction. No automatic Player flag rewrite or skill/stat deletion.

`Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs`: check the final resolved object/property before conversion, logging or mutation. Refresh ownership classification per invocation even with a cached PropertyInfo. Protect Mobile.Player for every Mobile; protect player aggregate totals/caps, cap component property aliases, and raw skill bases/individual caps. Identify real PlayerMobile even when Player=false; also protect other Mobile objects explicitly marked Player. Traverse NPC-to-player references safely; preserve reads, ordinary properties and NPC skill/cap initialization.

The automatic SKILL keyword is the deliberate bounded player reward route: positive assignments use the accepted TryGrant policy; no-op succeeds without mutation; reduction is rejected. Generic reflected player Base/BaseFixedPoint is protected even for a positive request. Explicit staff property/Skills tools remain separate and audited. Administrator XmlSet uses this same automatic path, with no separate provenance; neither null spawner nor a non-null staff argument creates an override. Clarify its help text in `XmlSpawner2.cs`.

No saved type, version, field or read/write order changes. Rebuild and validate a matching production Release/x86 core executable in isolation and include it with the source commit. Do not commit test overlays, credentials, copied saves, logs or symbols.

## Acceptance

Exercise direct final setter, nested aliases, cached and fallback lookup paths, case variations, player=false, actual player-owned Skill/Skills and NPC-to-player chains. Protected writes must reject without changing independent bases, cache, locks, caps/components, identity, stats or resources. Test positive/no-op/reduced/malformed/NaN/infinite SKILL operations, exact-cap and lock refusals, ordinary property/read access and intentional NPC initialization. Keep separate direct staff overrides usable and observable.

Inject high/low/negative private cache values only in external fixtures because Total is no longer writable. Verify real sums after valid grants/transfers/raw setters, exceptions and reentrant callbacks; rejected operations preserve points and remain diagnosable. Retain FIX-01 non-wipe and FIX-02 donor/resource assertions, adapting only cache injection and the explicitly changed XML policy, with prior expectations/failures preserved. Validate gain-probability inputs, unchanged serializer layout, typed load and complete disposable-world save/reload.

Run forced runtime compilation and focused compatibility checks. Complete actual ordinary Player/native rendering/interactions, a supervised client-visible Qwen choice and authenticated recovery using the existing isolated guarded client. Recheck sources, all14 protected inputs, saved copies, process ownership and loopback ports before launch. Stop owned runtimes and restore all disposable state on completion or blocker. Independently review, reconcile only the existing FIX-04 tracker record and make a focused local commit. No push, deployment, live Mira control, production-save or original Resident/Shardmind change.
