# Real skill cap and region reset investigation

**Current FIX-05 checkpoint (2026-10-19):** Required isolated acceptance and independent review passed on candidate004, including client011 house customization entry/rendering/normal exit. Tracker reconciliation passed; the accepted implementation is committed locally as `69cc69b6ac2f7e474a9e2aee4ca5dead7bb95fb7`. All owned runtimes are stopped; full copied saves/profiles and all 14 protected records were restored and verified. FIX-02, FIX-03 and FIX-04 follow in order. See [current validation](FIX-05-RESUME-VALIDATION.md).

**Ordered continuation (2026-10-18):** The user authorized development, isolated tests and focused local commits in the order FIX-05, FIX-02, FIX-03, FIX-04. See the [durable goal](REPAIR-SEQUENCE-GOAL.md) and [FIX-05 candidate report](FIX-05-REPORT.md). FIX-05 acceptance passed; later repairs await its reviewed local commit. Earlier deferred/no-authorization statements describe the FIX-01 scope. No push, deployment or production-save operation is authorized.

**Current repair status (2026-10-18):** [FIX-01 protection repair](FIX-01-REPORT.md) is locally verified and implemented in commit `1b49df74e8b972eb53805a790797f1f4872ab001`, not pushed or deployed. This evidence batch accompanies that implementation. Region verification now preserves progress and writes bounded diagnostics. The investigation below describes the historical `75bc9dfb` baseline; its wipe fixtures are historical evidence. FIX-02 through FIX-04 remain separate deferred work. The directly authorized local continuation passed bounded Player/Qwen progress acceptance; the report retains the inventory discrepancy and verified full rollback.

Baseline: `main` at `75bc9dfb`. Investigation date: 2026-10-05. Production source, public APIs, serialization formats, and player saves are unchanged. This is a dedicated investigation, not completion of the repository's separate fourteen-phase reorganization program.

**The region wipe is confirmed, but a wipe does not prove that the player gained too many real points.** The verifier also rejects several caps the configuration supports, and it can reject a character with the wrong profession/virtue state. Normal Create Food training did not cross the total cap in the production-method fixtures. Conditional automatic rewards, XML actions, corrupted accounting/flags, and cap reductions provide other routes to invalid state.

**The food pile is a credible, separate explanation for the area disconnects and failed reconnects.** Backpack rejection places new objects at the caster's feet; a house can prevent their decay; nearby/login transmission can overflow a client's 96 KiB pending-send queue. The production queue component reproduced that overflow. No connected-client or live-shard pile was reproduced.

The supplied player account is the only live-incident evidence. Current saves, deployed saved spawner/quest state, and incident logs are unavailable, as the user confirmed. Nothing in this report identifies which conditional route actually affected that character, how far over cap they were, or whether their real total was excessive before the wipe.

## Evidence and completeness

Evidence labels used below:

- **Source:** established by the unchanged baseline source or checked-in configuration.
- **Fixture:** exercised against the actual compiled production classes in an isolated process, with the substitutions listed under verification.
- **Incident:** reported by the user/player; not independently verified against historical server state.

The inventory scans all **6,598 runtime C# script files** and **120 core C# files**, excluding generated `bin`/`obj` trees. Runtime compilation recursively includes the script files; project inclusion is not used as an execution filter. On this baseline, all 6,598 also appear in `Scripts.csproj`, and there are no missing project source entries. No runtime Visual Basic source files were found.

Compiled IL inspection covers **112,809 methods/constructors**, including **111,518 method bodies**. It records **187 direct mutation references**, **40 reflection write references**, and **5,297 calls to the NPC-only `BaseCreature.SetSkill` overloads**, with no unresolved member operands. Each direct/reflection reference is assigned exactly one of 35 reviewed families. This catches aliases, multiline assignments, property setters, constructors, backing-field stores, compiler-generated code, and methods not found by searching for `Skills.*.Base` alone.

The register includes mutations of real base values, individual caps, total caps, cached totals, progression components, and the `Mobile.Player` flag used by the gain guard. It includes initialization and loss-only paths so those are explicitly accounted for. `cap-safe` means safe with respect to **new aggregate real-total excess** under the row's stated preconditions; it does not certify every other behavior of that subsystem. `conditional bypass` on a primitive or reset identifies a conditional invalid-state route, not an independently reachable player command.

Artifacts:

- [Mutation-path register](mutation-path-register.csv): every direct reference, source method/property declaration, IL offset, trigger/target, guards, maximum excess, and classification.
- [Reflection-write register](reflection-write-register.csv): every compiled property/field reflection writer and its reviewed access/target family.
- [NPC setter register](npc-skill-setter-register.csv): all 5,297 typed NPC setter calls, including exact caller signatures and source locations.
- [Reviewed families](review-rules.json): the explicit classification rules; export fails for missing, overlapping or unused rules.
- [Region-entry register](region-entry-register.csv): all 47 compiled region types and whether their effective `OnEnter` call chain reaches verification. Parent-region entry is a separate consideration.
- [Source coverage](source-coverage.csv), [project omissions](runtime-scripts-omitted-from-project.csv), [data coverage](data-coverage.csv), [data mutation literals](data-mutation-literals.csv), and [coverage summary](coverage-summary.json).
- [Additional route register](route-register.md): upstream entry points, non-sink behavior, disabled features, and unresolved incident state.
- [Repair recommendations](REPAIR_RECOMMENDATIONS.md) and [verification/run log](RUN_LOG.md).

All 240 available runtime-oriented XML/NPC/CFG/JSON/INI inputs in the scanned roots were checked for direct skill/cap/accounting mutation literals. The manifest and exact expression are included. One broad match was a harmless English comment, `skill, material`, in `Info/settings.xml:221`; no configured mutation action was found. This does **not** rule out an action in a current world save, generated strings, or a deployed file unavailable here. Source references, not a text scan alone, establish how the XML engine can mutate skills.

External framework, OrbServerSDK and UOArchitectInterface binaries listed in `Data/System/CFG/Assemblies.cfg` are dependencies, not part of the 6,718 source-file claim. The inventory does not establish arbitrary external binary behavior, memory corruption, malicious code injection, or every possible future compile configuration. There are no unclassified identified source mutation routes; deployed dynamic actions and the actual incident remain explicitly unresolved.

## What the cap actually measures

`Skill.BaseFixedPoint` is real skill in tenths. A character with 1000.1 real points has an independent sum of `10001`; cap 1000.0 is `10000`. The cached `Skills.Total` normally equals that sum. `Mobile.SkillsTotal` reads the cache. Equipment/stat/temporary bonuses affect `Skill.Value`; they do not add to the real-base sum.

The core base setter clamps only to the `ushort` storage range, 0..65535 tenths, then updates the cached total and invokes the skill-change notification. It enforces neither lock, individual cap nor aggregate cap. `Base` converts a double to integer tenths, so fractional truncation matters. The individual cap setter does not raise base skill. `Skills.Cap` and `Skills.Total` are writable integers without reconciliation.

Source: `Data/System/Source/Skills.cs:556-618,641-714,1447-1456`; `Data/System/Source/Mobile.cs` properties `SkillsTotal` and `SkillsCap`. `PlayerMobile.OnSkillChange` at `Data/Scripts/Mobiles/Base/PlayerMobile.cs:5703-5713` updates young/MyRunUO state, not another hidden grant. There are 58 skill slots (`Skills.cs:839`); the raw storage limit is much larger than any intended gameplay cap.

## Destructive region verification (historical baseline)

Source: `Data/Scripts/System/Regions/BaseRegion.cs:234-255` calls `PlayerMobile.SkillVerification`, implemented at `Data/Scripts/Mobiles/Base/PlayerMobile.cs:1774-1901`.

Only a `PlayerMobile` below `AccessLevel.Counselor` is checked. The character survives only if the cached total is at or below the cap **and** the following hard-coded conditions pass:

| Total cap in points | Additional condition |
| --- | --- |
| 1000 or 1500 | None |
| 1100 or 1600 | None |
| 1300 or 1800 | `Profession == 1` or `PlayerSettings.GetKeys(m, "Virtue")` |
| 4000 or 4500 | None |
| Any other cap | Always fails, even when real total is far below it |

On an accepted non-fugitive tier the verifier sets profession to zero. If the recorded component sum disagrees with the cap, it overwrites the origin start and, for upper tiers, Ether with 5000. It does not consistently clear incompatible boost/Ether components or fully normalize their sum. Equal numeric cap tiers can therefore conceal a different origin/configuration history.

On failure it calls `SkillBegin("default", pm)`, sets profession to zero and writes **every base skill to zero**, ignoring all locks. `SkillBegin` uses default origin plus current configured boost plus existing Ether. If Ether is not 5000 and stat cap is not 250, stat cap becomes 250 and raw Strength, Dexterity and Intelligence become 20. If Ether is 5000 and stat cap is not 300, stat cap becomes 300 and all three raw stats become 20. Correct existing stat caps avoid this attribute reset even when skills are wiped. The method does not preserve the pre-reset skills or log a detailed pre-reset snapshot.

**Fixture:** 1000.1 total at cap 1000.0 becomes zero; with stat cap 260 it also becomes 250 and all raw stats become 20. An under-cap character with total 500.0 and cap 1200.0 also becomes zero. Counselor access preserves the excessive state.

Configuration conflicts are concrete. `MyServerSettings.SkillBoost()` permits settings 0..10 in increments of 100.0 skill points (`Data/Scripts/System/Misc/Settings.cs:1366-1384`); `SkillBegin` at 1405-1418 adds that boost to the origin cap. The checked-in `Info/settings.xml:340` sets boost to zero. For a default-origin character with profession zero/no virtue key, the fixture tried **every allowed boost setting**: only 0, 1, 5 and 6 survived. Settings 2, 4, 7, 9 and 10 produced unsupported numeric caps; 3 and 8 produced 1300/1800 but failed the profession/virtue requirement. These are valid configuration inputs rejected by verification, not proof of excess or cheating.

### Where entry runs

`Region.OnRegionChange` (`Data/System/Source/Region.cs:887-918`) calls `OnExit`/`OnEnter` for the changing region chain up to the common ancestor. `Mobile.UpdateRegion` at `Mobile.cs:8767` is reached by location/map updates and load-related placement; movement, teleport/recall/gate and map transfer can therefore share the same final verification. Re-entering the same region without a region-chain change does not inherently call it again. Logging in can cause region entry as placement changes; it is not a separate universal verifier hook.

`BaseRegion`, ordinary descendants that inherit it, and overrides that call through to it verify. This includes `HouseRegion.OnEnter:71-73`, outdoor/dungeon regions and the origin regions. `GuardedRegion.OnEnter:234-241` does **not** call the base implementation, so its descendants such as `TownRegion`, `PlayerCityRegion`, `CityMarketRegion` and `ChallengeGameRegion` do not reach the verifier through that handler. `StartRegion.OnEnter:70-84` also skips it. A newly entered parent can still verify, regardless of the child handler. The full compiled table is in the region-entry register; the GuardedRegion exception is fixture-tested.

The base entry handler also cleans buff icons, invokes marching-order/morality/ghost handling, and can set kills to one for profession one before verification. Those are surrounding entry effects; they do not add real skill points. The verifier's own collateral changes are the skills, origin components/cap, profession and conditional attributes described above.

## Ways invalid totals can arise

| Route | Classification | Result and evidence |
| --- | --- | --- |
| Direct staff editing, conversion and staff tools | staff-only | Core setters permit arbitrary real points. SetSkill/SetAllSkills require GM; GmMe/Clone start at Counselor; ConvertPlayers requires Administrator. |
| XML `SKILL,Magery` or nested base property action | conditional bypass | Direct real grant ignores locks and both caps. Fixture takes total 1000.0 to 1000.1. Requires a relevant configured action/target. |
| XML changes `Skills.Total` | conditional bypass | Understates cached total; later ordinary gains exceed the independent real sum. Fixture: real 1000.0/cache 999.9 becomes real 1000.1/cache 1000.0. |
| `PlayerMobile` whose persisted `Player` flag is false | conditional bypass | `Gain` bypasses the aggregate check because it treats the recipient as a nonplayer. Fixture reproduces +0.1 at cap. No ordinary player route to falsify this flag was found. |
| Enabled government election reward | conditional bypass | A vote adds 0.1 Forensics directly to the **candidate**, even if Locked and at total cap. `NeedsForensics=false` in this baseline. |
| Origin change / direct cap reduction | conditional bypass | Existing points stay the same while the cap drops; fixture changes 2000.0 total/cap 4000.0 to cap 1100.0. |
| Player load recomputes cap from inconsistent progression fields | conditional bypass | Raw bases survive while saved cap is replaced by component sum. Source-confirmed; full PlayerMobile load not fixture-tested. |
| Legacy veteran login cap adjustment | conditional bypass | Can lower a legacy cap without reconciling skills; entire feature disabled in baseline. |
| Ordinary gains, accelerated gains, teaching, scrolls and skill transfers | cap-safe, with stated valid-state preconditions | No new aggregate excess found; boundary fixtures pass. Normal/accelerated gains can exceed an **individual** cap. |
| Saved deployed actions and actual incident sequence | unresolved | Current saved spawners, before/after skill data and incident logs are unavailable. |

No unconditional aggregate-gain bypass was found for an ordinary player with a truthful cache, `Player=true`, valid cap state and the checked-in configuration. That bounded finding must not be read as excluding the conditional routes in the table.

### Automatic rewards are different from direct staff edits

`VotingStoneGump.OnResponse` (`Data/Scripts/Custom/Government System/Gumps/VotingStoneGump.cs:86-138`) rewards `RunnerOne`/`RunnerTwo`, not the voter. After a normal check, it directly increments Forensics when the candidate's base is between 0 and 99.9. The direct increment ignores skill locks, individual cap and total cap. A unique voter can contribute up to 0.1; repeating the same voter's response is blocked by `Voters.Contains`. Subsequent distinct votes can continue until Forensics reaches 100.0; double-to-fixed truncation can make some increments smaller.

`PlayerGovernmentSystem.NeedsForensics` is false at `Data/Scripts/Custom/Government System/PlayerGovernmentSystem.cs:25`. Thus the bypass is real when enabled but is not the normal baseline behavior. CityVotingStone's gump-opening eligibility is at `Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs:79-102`; the response does not repeat all location/citizenship/candidate checks. That stale-state weakness does not remove the same-voter duplicate guard. **Fixture:** disabled branch grants nothing; enabled branch crosses total cap with candidate Forensics locked; repeat vote grants nothing further.

XML property actions similarly need staff configuration, but later execute automatically for a player. `BaseXmlSpawner.SetPropertyValue` at `Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs:1353-1370` implements `SKILL,name` by assigning `Skill.Base`. Nested property traversal at 1427-1506 and generic writes at 1262/1275 allow paths such as `Skills.Total`. The protected list at 64-68 contains only mobile AccessLevel and item StaffLevel, not skill/accounting/cap/Player properties.

Skill-check delegates install XML wrappers (`Data/Scripts/System/Skills/SkillCheck.cs:83-106`). `XmlSpawnerSkillCheck.cs:26-121,266` performs the ordinary check and then dispatches skill-use triggers, including checks too easy to train. `XmlSpawner2.cs:3895-3935` checks running state, player trigger eligibility, proximity, refractory/time-of-day conditions and skill-range/success requirements before triggering the configured action. This is the **source-supported conditional connection** between repeated casting and a custom skill grant/cache change. No such deployed action is evidenced for this incident.

### Changing the cap without gaining points

`SkillBegin` does not inspect or reduce existing skills. `SetSavage` calls it directly (`Data/Scripts/Mobiles/Base/PlayerSettings.cs:51-54`); SavageRegion checks origin identity but not an experience threshold. WantedRegion calls `SetWanted` after base verification, and that setup reapplies the fugitive cap. GypsyTarotGump's `EnterLand` at `Data/Scripts/Mobiles/Civilized/ShardGreeter.cs:878` reapplies fugitive caps for pages 2/4 at 1014/1027; response IDs >=100 reach it at 1140 without rechecking all UI eligibility. Returning to an applicable greeter/origin area is the reachability condition; current world travel/access state is unavailable. Exact region definitions and upstream routes are in the additional route register.

`SetSpaceMan` at `PlayerSettings.cs:137-185` explicitly zeros skills and clears the backpack during alien origin setup; that is a separate intentional origin reset. CrashRegion can invoke it when the starting-origin field differs. It is not evidence of a skill-total gain.

Pagan completion (`Data/Scripts/Quests/Pagan/ApproachObsidian.cs:25-58`) normally adds 500.0 to cap and records Ether=5000, with no base-point grant. It deletes all owned tips. The acquisition path (`ObeliskOnCorpse.cs:37`) rejects stat caps above 250, but the completion handler lacks its own already-Ether check. A duplicate/stale eligible completion is a conditional source of inconsistent cap/components, not a proven ordinary repeat exploit.

`PlayerMobile.Deserialize:4603-4659` reads origin/boost/Ether, migrates legacy values, raises an old boost to the current configured minimum and sets `Skills.Cap` to the component sum. It does not reduce bases. A consistent stored boost is not automatically reduced simply because the configuration is lower; an origin reset uses current configuration and can reduce it. Legacy veteran rewards in `Data/Scripts/System/Obsolete/Obsolete.cs:28190-28191,29155-29171` are disabled and only handle 700/705/710/715/720 caps; their adjustment is nevertheless retained in the register.

## Paths that do not establish aggregate excess

`SkillCheck.Gain` (`Data/Scripts/System/Skills/SkillCheck.cs:645-714`) requires Up and a base below its individual cap. It computes any Down loss, applies acceleration, then checks the **resulting total** before adding. Insufficient Down points prevent a new total excess. The division used to choose atrophy is integer division; acceleration is applied after selecting the Down loss. Consequently an accelerated attempt can remove points and fail to grant, but does not exceed the total cap with truthful state.

There is no final individual-cap clamp. **Fixture:** 9.9 with individual cap 10.0 becomes 10.3; accelerated 99.9 with cap 100.0 becomes 100.4. The largest low-skill accelerated increment is 2.0 points, so a one-tenth gap can overshoot an individual cap by 1.9. Those outcomes remain within the total cap and do not alone explain this reported real-total excess.

Offline StudyBook ends study through `SkillCheck.Gain` (`Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:110-168`). NPC teaching computes free/Down points and checks the final state (`Data/Scripts/Mobiles/Base/BaseCreature.cs:8302-8408`). ScrollofTranscendence checks Up, individual cap, free total and sufficient Down loss before addition (`Data/Scripts/Items/Special/Special Scrolls/ScrollofTranscendence.cs:97-170`). SoulStone transfer rechecks current values and lowers eligible points before writing the target (`Data/Scripts/Items/Special/SoulStone.cs:568-696`). SkillStone checks each requested increase against the current total and individual/stone caps (`Data/Scripts/Custom/Skill Stone/SkillStone.cs:369-503`), although it does not honor skill locks and has separate response-authorization weaknesses. Repeated responses do not produce new aggregate excess in the exercised cases.

Character creation starts a fresh character, sets its origin cap, validates skill choices or uses bounded profession presets (`CharacterCreation.cs:180-182,427-546`). TestCenter's skill setter checks the resulting fixed-point total (`TestCenter.cs:177-213`); its RaiseCaps and PowerScroll change individual caps only. Death loss, forgetting and soulstone storage only remove real points. Skill-reward quests eventually enter the registered scroll/stone/gain primitives; no additional compiled base-writing quest path was found.

BaseCreature/pet/monster initialization, taming, paragon changes, resurrection losses, golem upgrades, summon balancing and mirror/decoy copies target NPC skill collections. `PlayerMobile` does not derive from BaseCreature. Offline-character clone copying targets `CharacterClone : BaseCreature`, not the original player. Staff clone/control and account-swap commands retain staff gates. Equipment skill bonuses remain effective-value modifiers. These families and their individual callers are all retained in the registers rather than omitted as irrelevant.

## Persistence and accounting

`Skills.Serialize` (`Data/System/Source/Skills.cs:1621-1643`) resets and recomputes cached total while writing each base; it does not clamp an excessive base sum. Its reader at 1659-1711 reconstructs the sum from raw bases and ignores legacy saved cache values. **Fixture:** real excess survives an actual Skills serialization/deserialization roundtrip, and a falsified low cache is repaired by serialization. This is a component roundtrip, not a full shard restart.

Logout alone does not normalize skills. State can persist in memory, be saved, then be exposed by a later cache recomputation, cap recomputation or verifying region entry. `Mobile.Deserialize:6414-6441` places normal `Player=true` characters on the Internal map before updating their region; later login placement can expose the state. A false Player flag changes that load placement as well as gain eligibility. If the cached total is falsely high, region verification can wipe a real under-cap player; if falsely low, it can miss a real excess until the cache is corrected.

Normal message handling and timer callbacks are processed through the main loop (`Data/System/Source/Main.cs:641-642`). `World.Save` pauses network processing before save strategy execution (`World.cs:1013,1046,1087`). DualSaveStrategy serializes mobiles on the main path and joins the parallel work; ParallelSaveStrategy waits for its consumers; DynamicSaveStrategy completes its mobile-serialization work before returning, while later background work commits bytes. No ordinary macro/save race was found that would let a normal skill check observe the partially recomputed total. This does not assert that exceptions, arbitrary third-party callbacks or corrupted saves are impossible.

## Create Food and the area disconnects

**Source:** `Data/Scripts/Magic/Magery/Magery 1st/CreateFood.cs` normal branch delivers one random food plus a WaterFlask; race-specific blood/brain branches differ. `Mobile.AddToBackpack` (`Data/System/Source/Mobile.cs:11812-11839`) tries the pack, then calls `MoveToWorld` at the caster's location on rejection. That fallback does not run manual ground-drop stacking or enforce a per-tile object budget. WaterFlask is nonstackable (`Data/Scripts/Items/Food/WaterFlask.cs`, constructor); even stackable food arrives as separate world entities through this fallback.

**Fixture:** with a full one-item backpack, 25 calls to the real spell output method created **50 separate ground objects**, including 25 WaterFlasks, on one tile. Real total stayed unchanged. This bounded fixture bypasses cast timing/CheckSequence solely for output delivery; actual casting checks were tested separately.

HouseRegion suppresses decay when HouseStorage is enabled (`Data/Scripts/System/Regions/HouseRegion.cs:423-432`); the checked-in setting is true (`Info/settings.xml:255`). This makes hours of accumulation plausible. A script that deletes selected foods but fails to stop can also leave rejected objects outside its backpack cleanup loop. The user's actual macro was not supplied or executed.

Casting calls Psychology and Magery checks (`Data/Scripts/Magic/Base/Spell.cs:70-76,931-944`). The first-circle Magery difficulty is -20..20 (`MagerySpell.cs:28-38`), so sufficiently high effective Magery skips the ordinary gain opportunity for that check. Mana regeneration can check Meditation/Focus (`Data/Scripts/System/Misc/RegenRates.cs:29-40,112-129`). These all use the installed skill delegates and ordinary total-checked gain, plus the separately discussed configured XML trigger opportunity. **Fixture:** 200 actual CreateFood `CheckFizzle` and mana-regeneration callback iterations at total cap left the independent real sum unchanged.

`Mobile.SendEverything` (`Data/System/Source/Mobile.cs:8604-8659`) sends visible nearby entities without a per-view item-count budget; `Item.SendInfoTo:3109-3120` sends world item data and applicable properties. `Data/System/Source/Network/SendQueue.cs:189,229-231` limits pending data to 96*1024 bytes and throws when exceeded. `NetState.cs:789-792` logs `Too much data pending, disconnecting...` and disposes that client. Login sends the nearby world at `PacketHandlers.cs:2218/2241`, before LoginComplete/Login event at 2251/2256; returning to the same crowded view can repeat the pressure.

**Fixture:** an undrained real SendQueue accepted 96 chunks of 1024 bytes and rejected the next chunk. **Inference:** thousands of visible ground objects could trigger this path for nearby and reconnecting clients. The actual object packet sizes, compression, send/drain timing, client behavior and disconnect logs are absent; 20,000 objects is not a measured threshold. No client crash or specific network exception from this incident is established.

There is no discovered direct real-skill write in food creation, backpack rejection, ground placement, stacking, house decay, item transmission or queue disposal. Their connection to skills is the ordinary casting/regeneration checks and optional configured XML skill-use actions. Treat the pile/connection problem and the real-total/reset problem as separate findings unless historical evidence links them.

## Verification and reproduction

Run from a checkout with .NET Framework 4.8/VS 2022 build tools and the repository dependencies:

```powershell
.\scripts\Test-SkillCapInvestigation.ps1
python .\scripts\Export-SkillCapInvestigation.py <printed-run-directory>
```

The runner copies only runtime source and required compile/fixture dependencies into a new ignored `output/skill-cap-investigation/<guid>` directory; builds `Data/System/Source/Server.csproj` Release/x86 there; executes `-compileonly -nocache`; compiles the two test files **outside** the runtime script tree; then runs the isolated fixture. Processes are bounded by time limits. `Main.cs:600-604` exits compile-only before script Configure, Region.Load, World.Load, Initialize and listener startup. No latest saves, accounts or production port are used. Client map files are read only for the bounded world-placement fixture.

The fixture initializes in-memory world dictionaries, maps, SkillCheck delegates and XML's protected-property list only. It uses deterministic randomness, locks stats, and independently sums base points. Soulstone `CheckUse` is overridden only to bypass account/location/combat eligibility; the production transfer response is unchanged. Gump NetState objects have no connected socket. The food-output fixture overrides CheckSequence, while the separate skill-check fixture calls the real CheckFizzle. The queue test deliberately prevents draining and therefore proves the queue guard, not a connected-client failure rate. Source tracing covers full world save/load orchestration, region-chain dispatch and deployed entry prerequisites that are outside these component fixtures.

The final outcomes, command results, assertion count, output directory and limitations are recorded in the run log. The fixtures cover below/exact/over-cap tenths; all target locks; normal/accelerated gain; available and insufficient Down points; repeated and stale responses; configured and supported cap tiers; cap reduction; actual Skills roundtrip; ordinary spell/offline/regeneration gain; full-backpack output; and bounded transmission pressure. No live pile was created.

The source investigation is complete for the baseline and the identified mutation/cap-change paths. **Incident attribution remains unresolved**, explicitly because the historical evidence is absent. The recommended next code changes are separate, reviewable work; none were applied here.
