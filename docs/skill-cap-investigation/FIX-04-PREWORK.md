# FIX-04 read-only preparation

2026-10-20. This is source planning during the user's client/Qwen/native-control pause. No FIX-04 production change, implementation acceptance or commit has occurred. The ordered gate remains FIX-03 acceptance and local commit before FIX-04 implementation. Recheck these traces against that accepted snapshot before editing.

## Current evidence

| Surface | Current behavior / remaining work |
| --- | --- |
| `Data/System/Source/Skills.cs:1461,1480` | Total still has a public setter. Accepted FIX-02 `TryGrant` uses `GetBaseTotal`, publishes the complete transaction, and recomputes the cache after callbacks. A falsified cache does not bypass its cap check. Avoid duplicating that accepted repair. |
| `Data/Scripts/System/Skills/SkillCheck.cs:679` | Normal grants already use `from is PlayerMobile || from.Player`; false Player flags on PlayerMobile no longer select the NPC raw-grant branch. The gain-probability calculation at206 still reads cached `Skills.Total`, so a corrupt cache can affect whether a gain attempt occurs even though transaction enforcement rejects invalid excess. |
| `Data/System/Source/Skills.cs:556` | The explicit raw base setter adjusts cache by delta from the existing cached value. A previously corrupt cache therefore remains corrupt after an ordinary raw-setter change. These setters are used by deliberate initialization/staff/load operations; do not indiscriminately apply player grant locks to them. |
| `Data/System/Source/Skills.cs:1775` and constructor with reader | Serialization and deserialization reconstruct cache from stored bases. Preserve byte layout and legitimate load semantics. |
| `Data/System/Source/Mobile.cs:1539,1545,6866` | SkillsTotal is read-only, SkillsCap aliases aggregate cap, and Player remains writable. Component aliases are `PlayerMobile.Skill_Start/Skill_Boost/Skill_Ether` at4534/4547/4560. Protect actual resolved objects/properties, including nested aliases. |
| `Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs:44,64` | The protected type/name list currently covers only Mobile.AccessLevel and Item.StaffLevel. It lacks accounting, identity, caps/components and raw skill properties. |
| Same file `SetPropertyValue:1291` | Nested/property-cache/fallback routes check the type/name list, then call the public `InternalSetValue`. `SKILL,name` and reflected Base/BaseFixedPoint positive player changes already route through FIX-02's transaction. Scripted reductions remain raw. |
| Same file `InternalSetValue:1238` | Final setter does not independently enforce the protected list. A central final-object/property guard is needed so direct callers and path aliases cannot bypass protection. Preserve ordinary read paths. |
| Same file `SetPropertyObject:1542` | Mobile-reference assignment has its own traversal and setters. It cannot directly assign integer/bool properties, but traversal/reference aliases must be covered in the route review. |

Static searches found the explicit Player assignments in CharacterCreation (true) and NPC clone setup (false); these deliberate staff/initialization paths should remain. A bounded search of checked-in XML/NPC/CFG/text spawn inputs found no literal occurrences of the listed protected cap/cache aliases or `Player/false`; this is not proof that runtime-saved/configured expressions do not exist.

## Candidate boundary to evaluate after FIX-03

Use instance-aware protection at the resolved XML mutation boundary for PlayerMobile/reliably identified player ownership, rather than globally blocking NPC skill/cap initialization by property name. Cover Mobile.Player/SkillsCap, Skills.Total/Cap, Skill.Base/BaseFixedPoint/Cap/CapFixedPoint, and PlayerMobile component aliases. Preserve bounded positive SKILL reward operations through the accepted transaction; generic raw reductions/overrides belong to explicit staff tools, not an accidental automatic reflection exception. Reconcile this proposed distinction against all callers and existing configured content before implementation.

Keep explicit staff property tools and serializer paths distinct from automatic XML actions. A caller passing a non-null staff object is not sufficient provenance by itself to waive an automatic XML restriction. Enforce the final leaf guard even when a property lookup cache or alias reaches it directly. Reads and ordinary nonprotected properties should continue to work.

For accounting, evaluate an internally controlled cache update/reconciliation operation used by raw setters and the gain-probability calculation while keeping stored bases unchanged. FIX-02 already handles successful transactions/callback cache repair. Decide whether rejected grants should merely preserve/report a corrupt cache or reconcile it without changing bases, and document the choice; do not use cache repair as a pretext for point deletion. Any core change requires an isolated matching Release/x86 executable in its focused local commit.

## Meaningful acceptance to prepare

Test direct/nested/cached/fallback XML writes and direct final-setter calls, including property case differences and all aliases. Verify protected player mutations reject without touching bases, locks, stats, caps, identity, markers or resources; ordinary properties/read access and intentional NPC initialization remain usable. Verify bounded SKILL positive grants, exact-cap refusal, locked target, no-op, malformed/NaN/infinite values and rejected raw reductions. Keep explicit staff restoration behavior available and audited.

Use PlayerMobile with Player=false and low/high/negative cached totals. Check independent base sums before/after grants, transfers, callback exceptions/reentrancy, raw staff changes, serialization and full load. Preserve unknown cap components and the FIX-01 no-wipe guarantee. Reuse accepted FIX-02 transactional cases instead of replacing them with weaker tests. Real-client/Qwen acceptance stays paused until the user resumes it.
