# Turn-Based Combat In-Game Verification Matrix

Run this matrix on an isolated copy of a current world save with the production port changed or blocked. Keep `Enabled=false` in the deployed configuration until every launch gate is signed off.

## Staff setup

1. Regenerate the register with `./scripts/Generate-TurnBasedCombatCompatibility.ps1`.
2. Start the isolated shard with `ConficturaServer.exe -service -nocache`.
3. Log in as an Administrator and run `[TurnCombat Status`, `[TurnCombat Compatibility`, and `[TurnCombat SelfTest`; require disabled, zero groups, compatibility pass, and `bridge: ready`.
4. Run `[TurnCombat Enable`, set your current `Hits` value back to the same value, and wait ten seconds. Verify `[TurnCombat Status` reports enabled, mode `PvPOnly`, zero groups, zero test-armed mobiles, and `bridge: ready`.
5. Use `[TurnCombat Inspect`, `[TurnCombat ForceEnd`, and `[TurnCombat ForceDissolve` during the scenarios below.
6. Use `[TurnCombat Disable` as the first rollback action after any unsafe result.

Record the shard build, catalog commit, save backup, tester, timestamp, result, and relevant `Logs/TurnBasedCombat.log` event names for each test ID.

## Scheduler, AP, and group lifecycle

| Test ID | Scenario | Expected result |
| --- | --- | --- |
| TBC-PVE-NATIVE | Fight an ordinary fox, Orc, and random-encounter creature. | Combat remains fully native, no HUD opens, and group count stays zero. |
| TBC-GROUP-OPEN | Attack a legal player-controlled opposing side from outside combat, then reverse attack order. | No pre-initiative hit; one group forms and both sides roll once. |
| TBC-PRINCIPAL | Exercise player/player, player/pet, pet/player, and pet/pet hostile initiation. | Different effective player principals seed one group; masters and eligible followers join without bypassing legality. |
| TBC-PVE-ARM | Arm one isolated wild NPC with `[TurnCombat ArmTest`, then attack it. | The process-local regression exception seeds one group, appears in Status, and clears on Disable. |
| TBC-INIT-TIE | Repeat controlled spawns until totals tie. | Higher raw Dexterity wins; equal Dexterity uses lower Serial. |
| TBC-ROUND-JOIN | During active PvP, have a wild NPC legally attack within 12 tiles and LOS; repeat outside the boundary. | Nearby intent joins for next round; distant or unseen intent is rejected. |
| TBC-DISENGAGE | Move hostile endpoints beyond 18 tiles and out of LOS, including a graph with multiple components. | Edges prune without AP or turn ownership; isolated actors leave and disconnected components split without duplicate turns. |
| TBC-GROUP-MERGE | Current actor targets a participant in another group. | Initiating actor finishes; acted/unacted state is retained without duplicate turns. |
| TBC-AP-MOVE | Turn, walk, run, mount, fly, and use altered movement speeds. | Turning costs 0 AP; successful steps use rounded native delay; rejected steps refund. |
| TBC-AP-RUN-STOP | Exhaust AP while sending running movement. | Running flag and fast-walk queue clear; no movement leaks into another actor's turn and the client resynchronizes. |
| TBC-AP-WEAPON | Swing melee and ranged weapons with several delays. | Cost is native delay divided by 0.25, rounded up and clamped 1..20; ammo/durability stay native. |
| TBC-AP-SPELL | Cast short and over-five-second spells. | Cast plus recovery determines AP; only excess beyond the actor interval remains as cooldown. |
| TBC-TARGET | Cancel, timeout, select an invalid target, fizzle, miss, resist, and select a valid target. | Cancel/invalid refunds; legal failure consumes; cursor does not extend the turn deadline. |
| TBC-TIMEOUT | Let a player turn expire with and without a cursor. | Cursor cancels, reservation refunds, and the turn passes at 30 seconds. |
| TBC-DISCONNECT | Disconnect on turn, reconnect, then exceed the grace period. | Turn passes immediately; reconnect retains next eligibility; permanent absence removes the actor. |
| TBC-ESCAPE | Attempt escape near, visible, distant-hidden, and with 0 AP. | Only distant and out-of-LOS succeeds; success consumes remaining AP and removes the actor. |
| TBC-RELOCATE | Use tactical teleport, staff move, map transfer, and internalize. | Cataloged short in-callback movement remains; forced/long/map/internal relocation removes and logs. |

## Native action coverage

| Test ID | Scenario | Expected result |
| --- | --- | --- |
| TBC-ACTION-SKILL | Exercise every registered direct and targeted skill. | One cataloged reservation; native reuse delay uses actor time; invalid/canceled targets refund. |
| TBC-ACTION-ITEM | Use consumables, potions, wands, tools, doors, and combat items. | Native prerequisite/resource results remain authoritative; nested spell callbacks reuse the parent lease. |
| TBC-ACTION-INVENTORY | Equip, lift, split, drop to world/container/mobile, and provoke bounce/rejection. | Successful native attempt commits the explicit cost; rejection refunds and never loses the held item. |
| TBC-ACTION-TRAVEL | Try Recall, Gate, Sacred Journey, Nature's Passage, Mushroom Gateway, Ethereal Travel, and Astral Travel. | Every listed long-range route blocks and refunds while participating. |
| TBC-ACTION-AOE | Test direct, ground-targeted, and parallel area damage, including conflagration, frostbite, and monster splatter. | Native target/region/PvP checks run first; one lease authorizes only the committed callback. |
| TBC-ACTION-UI | Open paperdoll, status, help, informational gumps, and the combat HUD. | Explicit zero-cost informational UI remains usable and does not affect turn state. |

## Effects, AI, and legality

| Test ID | Scenario | Expected result |
| --- | --- | --- |
| TBC-EFFECT-REGEN | Damage Hits/Stam/Mana and advance several personal turns. | Due native-rate ticks run chronologically at actor turn start. |
| TBC-EFFECT-BANDAGE | Start bandaging, advance actor turns, then disengage and bandage again. | One AP lease covers every entry path; application uses healer time and exit preserves only the exact remaining delay with no wait-message leak. |
| TBC-EFFECT-STATUS | Apply poison, paralysis, and freezing before and during combat; test natural poison expiry and a successful cure. | Native timers suspend only for the owner; poison tick counts display/log and clear only on native expiry or cure; all durations resume correctly on exit/disable. |
| TBC-EFFECT-SUMMON | Enter combat with a near-expiry summon, merge groups, then disable. | Canonical remaining expiry follows the summon actor clock and rebases safely on exit. |
| TBC-EFFECT-BACKSTOP | Trigger a known direct Hits/Stam/Mana/status mutation outside an action/effect/admin scope. | Mutation blocks and logs instead of changing participant state. |
| TBC-AI-STOCK | Test stock melee, ranged, mage, controlled `Obey`, barded, passive, and searching actors. | Pulses follow live `CurrentSpeed` and preserve `OnThink`, bard, `Think`/`Obey`, searching, and sector lifecycle order. |
| TBC-AI-SPECIAL | Test OmniAI, CharacterClone, FactionGuardAI, mirror clone, clown, AITester, and a runtime AI switcher. | Live `AIObject` is reread; cataloged shell acts; missing classification ends and logs without guessing. |
| TBC-AI-ENCOUNTER | Test champions, invasions, XMLSpawner encounters, vendors, and encounter-specific `OnThink`. | Noncombat/service behavior stays wall-clock; participant mutation and AP guards remain enforced. |
| TBC-LEGAL-PVP | Matrix NONPK/PK/event status, PvP consent, Government bans/wars, guild relations, challenge/XMLPoints regions, pets, and summons. | Existing legality is authoritative; queries alone never form a group. |

## Save, fault, performance, and rollback gates

| Test ID | Scenario | Expected result |
| --- | --- | --- |
| TBC-SAVE-ACTIVE | Save while several groups have pending targets and effects. | Save completes; in-memory groups continue; no serializer version or field order changes. |
| TBC-RESTART | Stop without another save, restart from that save, and inspect native effects/summons. | Save loads normally with zero groups; canonical fields recreate native scheduling. |
| TBC-RELOAD | Reload with zero groups and then during combat. | Empty reload succeeds and bumps catalog version; active-combat reload refuses. |
| TBC-UNKNOWN | Add a temporary uncataloged action/AI/effect in the isolated tree and regenerate/omit as appropriate. | Compatibility drift or runtime fail-closed guard blocks and logs. Remove the temporary test code afterward. |
| TBC-HANDLER-FAULT | Inject a test-only handler exception. | Triggering action blocks; exactly one emergency dissolve is scheduled; status reports the latched fault; re-enable refuses until restart; adapted timers restore. Remove the injection afterward. |
| TBC-SOAK | Run uncapped mass combat at representative peak density. | Scheduler stays bounded per slice; no sustained queue growth, timer leak, or unacceptable latency. |
| TBC-ROLLBACK | Disable, stop, restore previous executable/scripts/catalogs, and load the backed-up save. | Previous build loads because no save contract contains turn-group state. |

Production activation requires all rows to pass, an explained performance ceiling, a verified real-save backup, and a rehearsed executable/script rollback.
