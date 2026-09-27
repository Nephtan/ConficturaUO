# Rebalancing while preserving the game

[Start here](README.md) · [Connections](connections-and-balance.md) · [Coverage](coverage-and-evidence.md)

## The goal you chose

Keep a mix of **lasting power, mastery, and discovery**. This is a design brief for later work. It does not approve a nerf, a wipe, a change to old items, a new cap, or a change to anyone's saved character.

The first goal should be a game where several kinds of effort lead somewhere worthwhile. A veteran should retain meaningful advantages. Preparation and skilled play should still affect outcomes. Finding a new place or learning an interaction should sometimes be as valuable as repeating the best known farm.

## Define balance as a set of promises

Choose these promises before choosing new numbers. The examples below are questions to decide, not current shard rules or recommended numeric targets.

| Promise | Decision to make | Evidence needed |
| --- | --- | --- |
| Earned power | How much stronger should a mature build be than a newly competent one? | Damage, sustain, survival, utility, and cost profiles at each stage. |
| Role | What should melee, casters, tamers, stealth characters, crafters, and hybrids each do especially well? | Encounter outcomes and contribution, including support and control. |
| Weakness | What preparation, tradeoff, or ally should a strong build still need? | Skill-cap opportunity costs, school exclusions, resistances, resource limits, pet exposure. |
| Time | Which milestones should take sessions, weeks, or a long career? | Actual active time, waiting time, material cost, failure rate, and player preferences. |
| Discovery | What should remain rewarding after the world is understood? | First-clear rewards versus repeat rewards; route concentration. |
| Cooperation | When should grouping help, and how should rewards scale? | Per-character and total group rewards, contribution rules, pet credit, multicharacter behavior. |
| Economy | Which activities create money, which transfer it, and which remove it? | Faucet and sink ledger by source and destination. |
| Legacy | How should existing earned power be respected if a rule changes? | Saved item/character distribution, migration options, reversibility, player communication. |

“Every build is equally good at everything” is not necessary. A healthier target is that different builds have useful roles, understandable costs, and viable ways to progress.

## Measure more than damage

For a build, record this compact profile:

| Dimension | Plain meaning | Useful measure |
| --- | --- | --- |
| Burst | What happens before the opponent can react? | Damage over a declared short window; largest hit; control chain. |
| Sustained damage | How well does it keep fighting? | Damage over a long fight, including resource downtime. |
| Survival | How much danger can it handle? | Incoming damage, healing, deaths, escapes, time spent unable to act. |
| Sustain | How expensive is another fight? | Health/mana/stamina recovery, consumed supplies, repair cost, recovery time. |
| Control and support | How much does it help others or suppress enemies? | Interrupts, debuffs, prevented damage, healing others, pet/tank contribution. |
| Access | What content can it reach or unlock? | Travel, lock/trap gates, spell exclusions, region/objective prerequisites. |
| Flexibility | How easily can it answer a new problem? | Role switches, spare skills, equipment swaps, damage types, pet choices. |
| Production | How much value does a session create? | Net currency, useful items, materials, equipment XP, progression after costs. |

The displayed character level is one descriptor in this profile. It is not a sufficient matchmaker or denominator for power.

## Split time into useful categories

For each activity, distinguish:

1. **Active challenge:** fights, choices, navigation, cooperation, or solving something.
2. **Productive repetition:** repeated actions that build a skill, collection, stock, or business.
3. **Waiting:** cooldowns, growth timers, recovery, spawn respawns, or offline study.
4. **Handling:** sorting, transporting, clicking, restocking, and other administration.
5. **Discovery:** time spent learning a route, mechanic, objective, or combination for the first time.

Do not assume everyone dislikes repetition. Ask which repetition feels satisfying and which feels obligatory. A convenience improvement can preserve the milestone while changing its cost; that is a balance change and should be measured.

### Example: why a damage change may barely affect a run

This is an illustrative model, not a measured Confictura run.

| Time in one run | Before | With 25% more effective damage |
| --- | ---: | ---: |
| Travel and handling | 6 minutes | 6 minutes |
| Fighting | 4 minutes | 3.2 minutes |
| Total | 10 minutes | 9.2 minutes |

If all other conditions stay equal, throughput rises about 8.7%, even though damage rose 25%. If leech also removes recovery time, the change can be larger. If incoming damage repeatedly interrupts healing, the improvement can cross a survival threshold. This is why actual whole-run outcomes are needed.

## Collect a baseline before changing rules

Use an isolated copy with the intended configuration and representative saved items. Keep the build, settings, gear, route, and encounter definitions identifiable so the comparison is reproducible. Current production behavior and player populations were not measured in this study.

### Character samples

Include newly established, intermediate, mature, and exceptional characters. Those are research groups, not labels inferred from level. Record:

- origin and special progression; creature race and active transformation;
- base/effective skills, total skill cap, raw/effective stats, and vital maxima;
- gear properties, levelable-item levels and points, relevant artifacts;
- pet types, skills, control slots, loyalty/bonding, command success;
- buffs, food, consumables, party size, consent mode, and staff intervention;
- intended build, player experience, active playtime, and available wealth where voluntarily supplied.

An analysis can use anonymized character identifiers. Individual player rankings are not needed to answer the balance questions.

### Encounter samples

Use several kinds of tests because a single training dummy rewards only one part of a build:

- isolated ordinary enemy; mixed pack; caster enemy; high resistance enemy;
- boss with sustained pressure; burst-danger boss; repeated expedition with travel and restocking;
- tamer control and pet-loss scenarios; party of mixed roles;
- traps, locked treasure, stealth access, gathering, and production sessions;
- consensual PvP and every important consent/region/pet exception in a separate test lane;
- optional turn-based mode only in its own enabled-and-accepted test environment.

For randomized encounters, use enough repeated trials to describe variation. Report sample count, median, spread, failures, and the conditions tested. Do not turn one lucky drop or one perfect kill into a balance conclusion.

### Minimal observation sheet

| Field | Why record it? |
| --- | --- |
| Build/revision, settings snapshot, date | Explains what rules were actually tested. |
| Anonymized build ID and gear/pet snapshot | Separates character power from displayed level. |
| Map, region, encounter source/type | Separates authored creatures, scaled creatures, random events, and staff objects. |
| Active combat, travel, handling, waiting, recovery seconds | Shows where effort goes. |
| Damage dealt/taken, healing, resource use, deaths/escapes | Measures strength, sustain, and failure cost. |
| Rewards created, recipient count, currency spent/deleted/transferred | Measures individual and whole-economy effects. |
| Equipment XP, skill/stat changes, new access | Captures power that does not appear as gold. |
| First attempt / repeat route / prior knowledge | Separates discovery from farming. |
| Outcome and player notes | Reveals frustrating or satisfying work that numbers miss. |

## Order of work

### 1. Resolve rule and measurement ambiguities

Check registry-based versus direct-constructor spell access, integer arithmetic in costs/rewards, origin/cap verification, resurrection branches, and actual deployment settings. These can make a system appear weak, expensive, or inaccessible for reasons unrelated to intended balance.

Deliverable: agreed current-rule sheet with reproduction cases. This step can remain documentation-only until fixes are separately approved.

### 2. Establish the character power envelope

Measure representative combinations of skills, equipment, race, spells, buffs, and pets. Include beyond-100 values, reached caps, and points invested past caps. Determine where investment produces diminishing returns and where it crosses a large threshold.

Deliverable: a build comparison with a stated acceptable veteran advantage and explicit roles.

### 3. Establish the reward and loss envelope

Compare ordinary hunts, champions, nests, treasure, quests, production, trading, and gathering over whole sessions. Measure total rewards created per event as well as each recipient's share. Count losses that actually occur; a theoretically destructible item that is automatically repaired is a different sink.

Deliverable: reward-per-effort comparisons, faucet/sink ledger, and the best-known repeatable routes.

### 4. Choose small experiments

Change one coherent cause at a time in a test environment. For example, investigate sustain, then measure its effect on both fight duration and supply spending. Do not simultaneously alter enemy health, player damage, skill gain, drop chance, and sale prices; the result would be difficult to interpret.

Deliverable: before/after evidence, reversibility plan, and a decision to keep, revise, or reject the experiment.

### 5. Protect earned progress and verify compatibility

Before any actual implementation, decide how old items and characters should behave. Options to discuss include changing future rewards, capped benefits with retained prestige, alternative uses for excess investment, compensation, optional conversion, or a supported migration. Each has different fairness and economic effects; none is selected here.

Any serialized changes require the repository's migration rules. Source changes need the appropriate build and forced script compile. Passing those checks proves technical viability; live gameplay acceptance still requires the intended play tests.

## Options that fit the chosen spirit

| Option to explore | Keeps | Risk to investigate |
| --- | --- | --- |
| Predictable progress alongside rare discoveries | Long goals and exciting finds. | Guaranteed rewards can raise the supply floor too much. |
| Stronger specialization with useful alternatives | Mastery, build identity, and new things to learn. | Existing broad builds may lose flexibility unless handled carefully. |
| Diminishing returns on tightly linked combat benefits | Lasting gains with a controllable gap. | Players may feel their old investment lost value; caps can waste rewards. |
| Distinct rewards for distinct activities | Discovery, crafting, exploration, and social play. | “Unique” rewards can make disliked activities mandatory. |
| Less repetitive handling while keeping meaningful costs | Time for active play and chosen long goals. | Higher throughput changes supply even if nominal drop rates stay the same. |
| More ways to earn an important milestone | Agency and different play styles. | The cheapest route can still dominate; equivalence must include risk and time. |
| Long-term cosmetic, property, collection, or civic goals | Lasting identity and achievement. | These will not substitute for combat progress for every player. |

## Decisions that remain open

The chosen direction is clear. Numeric targets are still open: desired veteran/newcomer gap, solo-versus-group expectations, acceptable repetition, rare-item availability, economic lifetime, and treatment of existing exceptional gear. Measurements should make those decisions concrete before anyone edits the game.

## Basis

This chapter is a proposed research method informed by the source findings in [progression](character-and-progression.md), [combat](combat-and-builds.md), [economy](adventure-and-economy.md), and [world rules](world-and-exploration.md). Its target-setting questions and hypothetical examples are analysis, not hidden claims about current player behavior.
