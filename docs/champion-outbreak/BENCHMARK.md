# Arachnid Outbreak combat benchmark

## Reference v1, frozen before combat or tuning

This is an established sword-and-shield paladin with obtainable named artifacts, not a claim about the strongest possible character. Zanthus remains the separate extreme-power stress case. The copied-world fixture grants the documented earned capabilities during preparation; it does not simulate training or acquiring the equipment.

- Ordinary unmounted human, Player access, no Ether or skill-cap boost; 1,000.0 base skill cap and 250 raw stat cap.
- Raw Strength 125, Dexterity 100, Intelligence 25; Fame/Karma 10,000 each and 10,000 starting tithing points.
- Base skills: Swords/Tactics/Anatomy/MagicResist 120 each; Parry/Healing/Knightship/Focus/Alchemy 100 each. Total 980.0. Individual caps permit the documented base values and equipment bonuses. Skills/stats are locked.
- Named equipment: Blade of Insanity, Achilles Shield, Conan's Helm, Aegis tunic/arms/gorget/gloves/leggings, Ring of Health and Bracelet of Health. Real constructors run once, with no attribute or durability selection. Every type must belong to `Loot.ArtyTypes` and equip successfully.
- Source-derived instantiated values in private copy `outbreak-benchmark-20261029-001`: Strength 130, Dexterity 100, Intelligence 25; health 285, stamina 200, mana 50; resistances 70/44/44/44/44; effective Swords/Parry/Healing 125. Starting item weight is 277; the native status display confirms the character is below its carrying limit.
- Supplies: complete ordinary Chivalry book, 500 bandages, 100 Greater Cure potions, 75 Greater Heal potions, 10 Orange Petals. Antivenom runs additionally receive 12 treatments. No-antivenom runs receive none.

The authoritative instance is `evidence/benchmark-reference.xml`, including item serials, actual attributes and rolled durability. `reference-freeze.json` binds it to the fixture, driver, policy and production-input hashes before the first encounter starts. Preparation is blessed; combat explicitly removes blessing. Health/stamina/mana are filled only in preparation. No character ability, creature AI, damage or resource pool is overridden during combat.

## Client policy v1

The copied installed ClassicAssist plugin sends ordinary client attacks, item uses, targets, spells and native pathfinding requests. It waits for an explicit private start command. Server instrumentation supplies ownership, stage/anchor information, health/status and actual bandage context; attacks are restricted to enemies present in the client's mobile collection. This is an instrumented scripted benchmark, not autonomous perception or evidence of AI learning.

Keep attacking the current enemy while adjacent, otherwise prioritize an adjacent enemy and then the nearest visible owned enemy. Travel to uncleared brood anchors in index order, then the boss anchor. Reissue attack at most every two seconds. Native pathfinding performs movement; the server fixture never moves the combatant during a run.

At one-second item intervals, prioritize self-escape from restraint, cure when poisoned, Greater Heal below 55% health with a ten-second interval, and bandage below 90% when no bandage is active. When stable, use Brood Antivenom if charges are exhausted and at least 120 seconds have passed since the last attempt; use Orange Petals at five-minute intervals. All source item cooldowns, eligibility checks and target validation still apply.

While adjacent and not automatically walking, attempt spells no more than once per two seconds: Close Wounds below 35% health when unpoisoned with at least 10 mana; otherwise Enemy of One every 120 seconds with at least 15 mana; otherwise Consecrate Weapon every nine seconds with at least 10 mana. Normal spell costs, fizzle, casting and recovery rules apply. On a newly observed web warning, allow 750 ms before requesting a two-tile eastward escape through native pathfinding; use native restraint self-escape if caught. Attempts and failures remain in the client log.

No direct damage, forced target damage, direct combat movement, resurrection, resource refills, stat changes, AI suppression or reinforcement suppression is allowed during a measured run. Death, abort, driver failure or forced stop cannot count as a full clear. Resource usage, actual health changes and deaths are recorded separately from item-use attempts. A cooldown alone cannot establish victory: an owned boss death is required.

## Acceptance still required

Three complete reference runs, including at least one without Brood Antivenom, must meet the specification's 20–35 minute median excluding world travel. These are not accepted merely because preparation, runtime compilation or client login passes. Preserve all failures. Freeze the reference and policy before tuning; do not weaken either to hit a duration target. Automatic tuning retains the specification's health/damage/interval limits and three-pass ceiling.

This fixture privately reviews the central reserve only inside its disposable process. It does not approve any site or modify the production housing policy. Current-world preflight, all-site warning visibility and the remaining native/cooperative/stress/economics gates remain separate.

## Source and verification

- `tests/ArachnidOutbreakBenchmark.cs`: exact-root guard, no-save setup, real artifact constructors/pool validation, private controller setup, normal lifecycle timer and observational telemetry. This file remains outside `Data/Scripts`; preparation copies it into the disposable runtime only.
- `tools/champion-outbreak/PrepareNativeAcceptance.py --fixture benchmark`: fresh copied-world/profile preparation; only this explicit fixture enables its existing copied ClassicAssist DLL and private macro.
- `tools/champion-outbreak/BenchmarkClient.py`: guarded copied-process client policy and action/error log.
- `Data/Scripts/Items/Containers/Loot.cs`: `ArtyTypes` and named artifact construction pool; individual constructors under `Data/Scripts/Items/Magical/Artifacts/`.
- `Data/Scripts/Items/Books/PowerScrolls/PowerScroll.cs`: obtainable individual skill-cap upgrades and shrine requirements; `System/Misc/Settings.cs`: recognized normal skill-origin cap components.
- `Data/Scripts/Magic/Knight/PaladinSpell.cs`: Chivalry uses `SkillName.Knightship`; normal mana/tithing/cast checks. `Items/Potions/BasePotion.cs`: free-hand rule is disabled by default.
- `Data/Scripts/Custom/Champions/Outbreak/`: production creatures, web/poison actions, ledger, rewards and lifecycle. `DeliveryReady` and source reservation review flags remain false.

Initial preflight retained two compile failures (death-event signature, then local skill/lock APIs) and a native macro import failure. The corrected fixture passed forced compile-only verification and the private server completed forced runtime compilation, world load and loopback listening. The repaired macro started and waited without initiating combat. These are preparation evidence, not encounter acceptance.

## Retained first run, 2026-10-29

The no-antivenom run died after **130.215 seconds**, with one guardian killed, no defender deaths and no boss reached. It consumed 13 bandages, 44 Greater Cure potions, five Greater Heal potions and one Orange Petal. Observations recorded 1,875 incoming health loss, 1,590 actual restored hits and 3,360 owned-enemy health loss; the latter is not asserted to be exclusively player-attributed damage. The client log contains no driver or pathfinding exception. Native observations show real combat and the death/resurrection-choice screen; no resurrection was performed.

This is a failed reference run. The v1 sticky-target rule stayed on the guardian while ten defenders accumulated. Prioritizing nearby defenders is the next policy experiment, not a demonstrated cure for the failure. Source inspection also found inherited HP-scaled breath damage and ordinary paralysis; see [INHERITED_SPIDER_REVIEW.md](INHERITED_SPIDER_REVIEW.md). No encounter parameter was tuned. Three complete runs and pacing/economics acceptance remain open.

Evidence is retained under `D:/ConficturaUOTestTools/outbreak-benchmark-20261029-001/evidence/`: `reference-freeze.json`, byte-matching `frozen-inputs/` snapshots, `benchmark-reference.xml`, `benchmark-client.jsonl`, `run-001-final.xml` and `benchmark-verification.json`. The frozen policy/document snapshots remain authoritative for this historical run even as the working documentation evolves.

Reproduce the provenance/outcome check with:

```powershell
python tools/champion-outbreak/VerifyBenchmark.py D:/ConficturaUOTestTools/outbreak-benchmark-20261029-001
```

Add `--require-clear` to require actual successful combat; it exits nonzero for this retained death. The default verifies preservation and reports the failed outcome explicitly. All 6,742 current production inputs, all 20 private save files and protected original saves/settings/profiles match. Both owned copied processes and loopback listener 63928 are confirmed gone after forced no-save teardown. The first transient teardown snapshot remains retained; no graceful shutdown is claimed. The final private compatibility register scans 6,612 scripts with Unknown 0. Production activation and all source reservation review flags remain false.

## Policy v2 experiment: defender priority

The next no-antivenom run keeps reference-v1 character capabilities, equipment types, supplies, spells, item rules and enemy profiles. Its sole combat-policy change is to prioritize visible owned defenders named `a dread spider` within eight tiles over the guardian/boss. Keep the current defender while adjacent; otherwise select the nearest eligible defender. If no such defender exists, use v1 targeting. This uses server-observed public names alongside the already disclosed ownership instrumentation. No enemy ability, including inherited breath, is suppressed.

Additional status/heartbeat observation records effective Strength/Dexterity/Intelligence and ordinary paralysis/freeze. These fields observe normal debuffs and do not modify them. Fresh constructors retain ordinary NPC stat/spell randomness and gear durability rolls, so this is not an identical-seed causal experiment. Each actual reference/driver/fixture instance must be frozen again before starting; the v1 frozen snapshots and failure remain intact. No tuning occurs during this policy experiment.

The v2 copy002 cleared the first brood alive, then repeatedly received `false` from native pathfinding to the distant next anchor. It was stopped and retained as a navigation-failed partial run, not a complete reference or a pacing result. The next driver uses short waypoints along the placement's connected terrain mesh. A separately requested `start-navigation` mode creates no encounter, retains staging blessing and traverses all three inter-anchor legs through actual client pathfinding. It cannot count as combat acceptance. Repeated rejected navigation now terminates with a driver-failure marker instead of retrying indefinitely.

[PALADIN_TOOLKIT_REVIEW.md](PALADIN_TOOLKIT_REVIEW.md) reassesses all ten Chivalry spells, all five actual sword abilities, cure alternatives, preparation/passives and OmniAI's broader but incomplete decisions. The character is legal; v1/v2 policy experiments are not yet a qualified full-toolkit benchmark. Loaded spell/ability costs and skill gates are now exported in preflight. Successful native use and a competent frozen policy are still required before encounter-balance conclusions.

Navigation-only003 passes all three short inter-anchor routes in68.923seconds with zero rejected walks, no combat actions/damage/supply use, observed intermediate waypoints and final arrival within one tile of the boss anchor. `VerifyBenchmark.py D:/ConficturaUOTestTools/outbreak-benchmark-20261029-003 --require-navigation` checks this specific gate and preservation/teardown; it explicitly leaves combat acceptance false. Both copied processes and listener50549 are gone after acknowledged cleanup and forced no-save termination. The v2 partial run's corresponding copied listener60721 is also gone. Original source/profile/save hashes match for both copies.

After the approved profile/qualification source changes, append `--production-ref 08e1292e` when reproducing these historical copies. The verifier resolves changed production inputs from that commit and verifies the complete frozen input manifest; it does not conflate historical navigation/policy experiments with the new enemy profile. The next gameplay candidate is `D:/ConficturaUOTestTools/outbreak-approved-20261029-002`; new complete runs must use its approved source snapshot or a subsequently verified candidate.

Controlled native toolkit copy `outbreak-toolkit-20261029-002` subsequently passes20/20 actions with preserved source/save/profile hashes and forced no-save teardown. Details and limitations are in [PALADIN_TOOLKIT_REVIEW.md](PALADIN_TOOLKIT_REVIEW.md#controlled-native-toolkit-checks-2026-10-29). These stopped-target checks establish player action/resource/effect execution, not normal-AI combat competence, isolated ability DPS or any completed encounter. The corrected four-second target wait currently applies to toolkit cases; the older combat-policy branch remains an unqualified v2 experiment and must be revised before a new gameplay freeze. Sacred Journey preparation, cooperative Noble Sacrifice and the final resource-aware policy remain open.

## Reference/policy v3 - finite preparation and resource-aware pilot

Keep v1's raw stats, base skills, named equipment and ordinary constructors. Add finite stocks of20 Greater Strength,20 Greater Agility,20 Total Refresh and one pre-marked wilderness rune. This models obtainable/traded preparation; it does not grant Magery or Mark. Alchemy100 supports crafting the potion stock. Optional antivenom12 must be selected before preparation/freeze; the run cannot add it afterward. Existing bandage/cure/heal/petal stock remains500/100/75/10. Never refill any stock during combat.

`prepare-player` (or `prepare-antivenom`) lets the copied client actually drink Strength/Agility, use petals and bandage to full HP. The fixture remains blessed without an encounter during this phase. On actual150 Strength/150 Dexterity/full325 HP and consumed petals, it rewrites the pre-combat manifest and becomes ready. A fresh v3 combat run requires this preparation. The manifest freezes actual remaining supplies, real stats/equipment and the rune's location/checked return waypoints. No health, mana, stamina, skill, blessing or AI override occurs after combat begins.

The v3 pilot keeps defender-first targeting, connected short routes, ordinary autoattack and the existing warning sidestep/restraint escape. Self-targeted spells now queue a normal target response without blocking observation for the whole cast. Wait for the actual current spell, target and recovery state rather than blindly interrupting/recasting. No encounter tuning accompanies this policy revision.

- Cure first with bandaging when health permits; urgently use Greater Cure below50% HP, or below65% with lethal poison. Use a potion if bandaging remains active/poisoned beyond five seconds. Include Cleanse when cure stock falls below25, HP exceeds45%, mana is at least24 and a bandage is not near completion. Ordinary paralysis blocks spells, while item actions remain available subject to native rules.
- Use Greater Heal below55% HP when unpoisoned/unwounded and its ten-second cooldown permits; bandage missing HP below92%; use Close Wounds below40% when legal. Remove negative stat curses/mortal wounds when castable with12 mana. These uses spend the survival reserve when needed.
- Refresh stamina below60%; maintain finite Strength/Agility preparations when no negative stat curse is present; maintain petals and optional antivenom using their actual state and reuse intervals. Failed/native-denied attempts remain attempts in the log.
- Reserve18 mana after ordinary offensive costs. Set Enemy of One only when its observed type differs and it is not already waiting: before the boss follow the selected spider; during the boss stage switch only to the boss, avoiding repeated switches back to reinforcement types. Its other-type vulnerability remains real. Maintain Consecrate/Divine Fury only when absent and enough mana remains. The suit's110 raw defense bonus keeps Fury above the45 cap after its penalty.
- Use Holy Light with at least three enemies within three tiles and30 mana; try Dispel once per15 seconds under crowd pressure (four nearby enemies and HP below55%) with24 mana. Fear is not guaranteed and must be observed during normal AI.
- Use Double Whirlwind with at least two adjacent targets, stamina at least150 and12 mana plus the18 reserve; otherwise prefer Fire Strike with9 plus the reserve. Space selection/observed consumption by at least five seconds to limit the three-second doubled-cost window. This is a source-based single-target priority, not a DPS conclusion from aggregate controlled health loss. Armor Ignore, Double Strike and Death Blow remain validated alternatives omitted from this pilot because of expected damage/mana tradeoffs; revise only with retained normal-AI evidence. Noble Sacrifice is omitted from solo combat because it has no ally to rescue and sacrifices the caster's resources.
- At HP below25%, with Greater Heal on cooldown, attempt Sacred Journey if casting is legal and at least6 mana remains. Arrival at the real refuge ends this pilot as a retained retreat, not a clear or pacing result. Native walking return has been checked separately; automatic recovery/re-entry remains a policy gate if retreat is needed. A failed/retreated pilot cannot be counted toward the required three complete clears.

The pilot is not yet accepted as competent or as a pacing run. Inspect normal-AI actions, failures, control, actual resources and survival first. Freeze the actual source, policy and reference before starting; keep all failures. After policy qualification, require three complete runs on the same reference capabilities/policy (at least one without antivenom) before evaluating median20–35 minutes. Production remains disabled and automatic tuning passes remain zero.
