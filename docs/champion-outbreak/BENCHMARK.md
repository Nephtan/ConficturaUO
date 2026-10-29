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
