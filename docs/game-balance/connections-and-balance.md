# Connections and balance questions

[Start here](README.md) · [Systems](systems-at-a-glance.md) · [Research plan](balance-research-plan.md)

## The central finding

Confictura has many local limits, but a player's strength is the **combination** of those systems. A limit inside one system does not establish a limit on the whole build. Some rewards make the next reward easier to obtain; others open a new activity or provide identity without much combat power.

The source supports investigating that compounding. It does not yet establish how far ahead typical veterans are, which build dominates, or whether players find a particular long goal satisfying.

## The connections that matter most

| Connection | What the code establishes | What it can mean for play | What would settle the balance question |
| --- | --- | --- | --- |
| Damage → leech → longer fights → more rewards | Leech draws on delivered damage; weapon damage has several layers. | Damage can improve offense, survival, and repeatability together. | Whole-run damage, healing, resource use, recovery, and reward comparisons. |
| Kills → several equipped items gain XP → stronger kit | Each equipped eligible levelable item is visited; the same kill is not divided into one fixed item-XP pool. | A complete leveling kit can improve together. | Time and useful power gained with one eligible item versus a full kit. |
| Levelable gear → repeated repair → lower wear cost | Repair paths restore eligible gear during combat. | A durable endgame kit may need fewer replacement purchases. | Actual breakage, repair, and replacement spending by equipment family. |
| Gold → selected equipment properties → faster earning | Guild enhancement is a priced, persistent upgrade path. | Prior wealth can improve future income. | Effective benefit per gold spent at several gear stages; saturated properties. |
| Mercantile/Begging/guilds → larger payouts from the same goods | NPC sales, antiques, cargo, and shop cash-out apply distinct benefits. | Economic skills may be useful specialization or compulsory chores. | Skill opportunity cost, merchant transfers, sale-value distribution, player choices. |
| Origin → broad skill budget → more compatible bonuses | Total caps differ greatly; many mechanics use several skills together. | A broad build may cover weaknesses that a narrow build must accept. | Equal-investment comparisons and actual cost of origin restrictions/death. |
| Training budget → character rating → race/encounter behavior | Rating divides some trained values by their caps; race and encounter systems consume it. | A larger unlocked cap can temporarily lower the rating before retraining. | Before/after Titan traces, equipped bonuses, and encounter-selection outputs. |
| Race → effective skills/Luck → combat/rewards | Templates supply bonuses; selected benefits scale with derived level. | Identity is also a power/economic choice. | Full template comparisons at several development stages and relevant content. |
| Scrolls/study → training → stronger encounters/characters | Scroll ceilings and probabilistic study both feed the shared skill system. | The convenient route can change pacing without any explicit character XP award. | Active versus offline progress, book costs, near-cap behavior, and acquisition time. |
| Nest participation → independently created rewards | Nearby PlayerMobile recipients each receive loot in the activation path. | Total money can rise with nearby characters. | Recipient accounts/contribution, total event payout, event frequency. |
| Champion participation → fixed scroll pool plus ground gold | Six scrolls are shared among credited recipients; ground gold is its own path. | Group participation has a different per-person return from nests. | Per-person and whole-event yields, completion time, rights, collection rate. |
| Treasure tier → reward bundle → gear/access value | Chest level is raised internally; artifact chance and additional fills have thresholds. | A nominal tier increase can have a disproportionate reward effect. | Effective tiers, intended tier names, full reward quality, route difficulty/time. |
| Crafting → BOD rewards/runics → better production | Production can award tools and skill-cap increases used in production. | Professions have their own lasting progression loop. | Cost per useful output and demand compared with loot and gold upgrades. |
| Storage/property → stockpiles → fewer costs between attempts | Checked-in house rules favor persistence; player-vendor upkeep is small. | Wealth and supplies can accumulate, improving convenience and throughput. | Active/inactive property, stored value, consumption, and effective maintenance costs. |
| Travel/scouting → safer/faster access → higher returns | Region restrictions, detection, stealth, locks, and traps change access/risk. | Utility is a form of power that a damage chart misses. | End-to-end route comparisons, failures, first discovery versus repeated runs. |
| City rules/regions/events → conflict permission | Government and event exceptions participate in harmful-action decisions. | A mode label may not describe every player/pet interaction. | The exact consent × region × actor/target × event test matrix. |

Evidence: [progression cards PROG-02–17](character-and-progression.md), [combat source review](combat-and-builds.md), [economy sources E01–E30](adventure-and-economy.md), [world source trace](world-and-exploration.md). These are traced relationships; the possible player effects in the third column remain hypotheses until measured.

## Why stacking is not the same as adding

Consider a simplified character who deals 20% more useful damage and can fight 20% longer before needing to stop. If both improvements operate independently, output over the relevant activity can rise by `1.2 × 1.2 = 1.44`, or 44%. That is an illustration of multiplication, not a measured 44% bonus in this game.

Conversely, two sources of the same capped property may substitute for one another. A spell adding Weapon Damage can provide little extra damage to a build already at that property's cap. The correct questions are **which stage gets the bonus, what is already present, and which limit applies there?**

For survival, thresholds can be larger still. If a build can sustain all incoming damage, it can remain in a fight that previously exhausted its supplies. Small numerical changes around that point can change the entire play pattern. Compare resource use and failures as well as average damage.

## Rules to resolve before tuning around them

These are current-source findings or sharply scoped uncertainties. They are not evidence of player abuse, and no repair is approved here. Their order is a proposed investigation order because they can invalidate later balance measurements.

| ID | Demonstrated rule or discrepancy | Why it matters | Minimal confirming exercise |
| --- | --- | --- | --- |
| GB-01 | Character level is derived; archetype skill contributions clip at 100. | Same-level comparisons can conceal major equipment and skill differences. | Hold rating constant; compare gear, above-100 skills, and pet contribution. |
| GB-02 | Skill verification accepts specific total-cap values and can reset trained skills for other values. | Changing a cap alone could damage saved progression. | Isolated copies at all accepted caps and proposed cap, then region entry; inspect exact mutations. |
| GB-03 | Positive non-cap-obeying skill bonuses bypass the ordinary effective-125 clamp. | “125 is the cap” is an incomplete power model. | Base125 with/without elixir and stacked item/race modifiers; inspect each consumer. |
| GB-04 | Common skill-check overloads normalize supplied maxima ≥100 to126. | A nominal GM threshold can still have a failure chance. | Compare100/125/126 effective skill and direct-chance versus min/max overloads. |
| GB-05 | The 700-entry spell registry rejects registrations at 700 and above; some schools also construct spells directly. | A route failure can be mistaken for a missing or weak school. | For each affected spell, test command, book, toolbar, and registry route separately. |
| GB-06 | Elemental stamina-cost reduction uses integer division. | A small Lower Reagent Cost property can erase the calculated stamina cost in that method. | Evaluate LRC0/1/99/100; trace final cast validation and consumption. |
| GB-07 | Shoppes cash-out divides integer barter by 100. | Economic returns jump rather than rise smoothly in this path. | Compare 99/100 and guild member 74/75 Mercantile, including fractional skill. |
| GB-08 | Treasure chest tier is increased by 4 and its scroll term uses integer division. | Displayed acquisition tier and reward tier can diverge; scroll chance does not scale as its expression might suggest. | Compare supplied versus effective levels and all reward conditions at each tier. |
| GB-09 | Resurrection fee protection and penalty dialogs use different fixed-point novice thresholds; one old tithe branch invokes the penalty twice. | Death cost cannot be represented by one universal percentage. | Origin × tribute × novice boundary × resurrection path; verify exact persistent changes. |
| GB-10 | SkillGain/BondDays getters do not use many in-range configured values as their names suggest. | A configuration experiment could appear to do nothing or disagree with help text. | Map raw input → getter return → caller behavior at boundary values. |
| GB-11 | Shared caps and additional modifiers occur at different stages. | A displayed property total is not a universal damage/healing/regen ceiling. | Record the actual inputs/output of each selected pipeline stage. |
| GB-12 | Consent decisions include owner, government, event, region, and actor-type paths. | Mutual player opt-in is not a complete description of every combat interaction. | Direct player, controlled pet, summon, wild creature, aid, theft, and event cases. |

Detailed evidence and the limits of each finding are in the linked chapters. The source census and source-reference checks verify that the cited files exist; runtime exercises above are still proposed work.

## Questions that require player and economy data

| ID | Hypothesis, not a verdict | Needed comparison |
| --- | --- | --- |
| GB-13 | Repetition pays more general power than challenge or discovery. | Net useful progress per active hour across tasks, including risk, failure, and waiting. |
| GB-14 | Mature damage/sustain builds make ordinary content irrelevant. | Broad build samples against varied enemies and repeated expeditions. |
| GB-15 | Broad skill budgets erase specialization. | Real build choices, unused skills, role substitution, and group dependence by origin. |
| GB-16 | Some direct payouts dominate the money supply. | Currency creation by source and actual event frequency, not theoretical single-event maximum alone. |
| GB-17 | Durable gear and property leave too few meaningful sinks. | Wealth change, consumable use, taxes versus destruction, upgrade saturation, and item loss. |
| GB-18 | Crafting loses its purpose once loot/paid upgrades are established. | Useful output, material/tool costs, trade demand, and gear replacement at each stage. |
| GB-19 | Group and multicharacter rewards grow faster than challenge. | Event totals, unique participants, contribution, group size, and completion rate. |
| GB-20 | Discovery eventually becomes a narrow optimum route. | Route concentration, first/repeat rewards, collection completion, alternative activities chosen. |

These questions preserve the distinction between **the code allowing a pattern** and **that pattern dominating the game**. A player survey and a small, representative observation set are more useful next evidence than a blanket damage or gold nerf.

## What this study cannot decide for the owner

There is no objective source-code answer for the ideal amount of repetition, the fair reward for years of play, or whether solo players should eventually overcome content intended for a group. Those are design choices. The source analysis tells us which rules implement those choices today and which other systems a future change would touch.

The chosen direction—lasting power, mastery, and discovery—supports incremental changes. Start with accurate rules and representative measurements, choose one connected problem, and preserve reversibility and earned-investment decisions throughout.
