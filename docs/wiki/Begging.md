# Begging

Begging in Confictura is a survival, bargaining, and Jester skill. You can ask human NPCs for a little gold, plead with dangerous creatures to stop fighting, and adopt a begging demeanor that changes many prices and rewards. Jester tricks also use Begging as their casting skill.

Its biggest benefits are **cheaper services, better payments, and a chance to escape a creature**. Its biggest costs are **lost reputation, unreliable pleas, and the risk of accepting worse prices when your Mercantile is stronger**.

> **Rules checked:** September 22, 2026, against the Confictura source and checked-in settings. This guide describes those rules; live NPC inventories, item availability, and server overrides can differ. Source references and verification scope are at the end.

Jump to: [gold](#asking-npcs-for-gold), [creatures](#pleading-with-creatures), [players](#pleading-with-players), [prices and rewards](#the-begging-demeanor-prices-and-rewards), [reputation](#reputation-costs), [Jester magic](#begging-and-jester-magic), [training](#training-and-skill-increases), [equipment and elixirs](#equipment-and-elixirs), or [skill milestones](#choosing-how-much-begging-to-keep).

## Quick reference

| What you want | What to do | Main limitation |
|---|---|---|
| Ask for gold | Use Begging and target a suitable human NPC within 2 tiles | Small payments, negative-karma refusals, and a shared skill delay |
| Escape a creature | Use Begging and target an eligible creature within 12 tiles | Success depends on difficulty; some creatures cannot be calmed |
| Get bargaining benefits | Use Begging and target yourself to turn the demeanor on | Many interactions cost karma; some replace Mercantile instead of adding to it |
| Stop bargaining | Use Begging and target yourself again | The demeanor stays set until you toggle it; it is saved with your character |
| Perform Jester tricks | Carry a stocked Bag of Tricks, qualify as a Jester, and use the bag or trick commands | Requires mana, prank points, and each trick's normal conditions |

**Ordinary Begging needs no instrument, reagent, gold payment, or mana.** Using the skill reveals you. You must be alive, able to use skills in your region, and free of the shared skill delay and an active spell. Turn-based encounters also apply their normal action rules.

The demeanor benefits require **at least 1.0 effective Begging**: the game drops the decimal part of your skill when deciding whether you count as a beggar. Toggling it on at 0.0 does not activate those benefits.

## Reading the numbers

Throughout this guide:

- **B** means your current effective Begging, including equipment and temporary bonuses. It is not just the amount you have trained.
- **Base Begging** is the permanent trained amount. Your personal skill cap controls training, not every equipment bonus.
- **P** means effective Psychology, **K** means current karma, and **F** means current fame.
- `floor(x)` means round down: `floor(12.9) = 12`.
- `trunc(x)` means remove the decimal part: `trunc(-12.9) = -12`. This distinction matters for negative karma.
- `clamp(x, low, high)` means keep a number between the two limits.
- A chance of `0.8` means **80%**, or about eight successes in ten attempts over time. It does not promise a particular sequence.

Begging receives **no ordinary Strength, Dexterity, or Intelligence contribution** to its effective value. Its normal skill-gain entry also has no direct stat-gain chance. Equipment and elixirs can increase effective Begging above 125; there is no single benefit cap that applies to all uses.

## Asking NPCs for gold

### How to use it

1. Stand within **2 tiles** of a human-bodied NPC that qualifies for ordinary begging.
2. Activate Begging from your skill list or a normal client Use Skill macro.
3. Target the NPC. You bow, and the request resolves about **2 seconds** later.
4. After resolution, the shared skill delay is **10 more seconds**. Allow roughly **12 seconds per completed request**, plus targeting time, in ordinary real-time play.

The initial target cursor reaches 12 tiles, but a money request still requires you to be within 2. Normal visibility, line-of-sight, and same-map targeting checks apply.

The NPC must have a backpack containing enough actual gold. Payments remove that gold from its pack; this is not an unlimited payout or a withdrawal from the vendor's shop account. With fewer than 10 gold remaining, it cannot give anything through this skill.

Players do not donate gold through the skill. Nonhuman creatures generally cannot donate either. A dangerous target may enter the pleading rules instead, even if it looks human. The begging demeanor does **not** need to be enabled to ask for money.

### Chance of success

For an NPC with a backpack, there are two separate checks:

```text
Skill success chance = clamp(B / 126, 0, 1)

Trust chance = 1                         if K >= 0
Trust chance = clamp(0.5 + K / 8570, 0, 1) if K < 0

Chance of a payment = Skill success chance × Trust chance
```

The payment formula assumes the NPC still has at least 10 gold. Otherwise the payment chance is zero regardless of skill.

**Why divide by 126?** Confictura changes skill checks whose requested upper limit is 100 or more to an upper limit of 126. The Begging action requests 125, but the shared rule makes the actual limit 126.

| Effective Begging | Skill success, before the trust check |
|---:|---:|
| 25.0 | 19.84% |
| 50.0 | 39.68% |
| 75.0 | 59.52% |
| 100.0 | 79.37% |
| 120.0 | 95.24% |
| 125.0 | 99.21% |
| 126.0 or more | 100% |

Negative karma has a sharp effect: **going from 0 karma to -1 immediately adds slightly more than a 50% refusal chance**. Positive karma does not add a further trust bonus; it simply avoids this refusal check.

| Karma | Chance the NPC allows the skill roll | Payment chance at 100.0 Begging |
|---:|---:|---:|
| 0 or positive | 100% | 79.37% |
| -1 | 49.99% | 39.67% |
| -1,000 | 38.33% | 30.42% |
| -2,459 | 21.31% | 16.91% |
| -4,285 or lower | 0% | 0% |

In plain language, being very skilled does not make an untrusting NPC overlook your reputation. At 126 Begging and -1,000 karma, the skill check is certain, but only about **38.33%** of requests pass the trust check.

### How much gold you get

Let **G** be the gold currently in the NPC's backpack:

```text
Payment limit = clamp(10 + floor(F / 2500), 10, 14)
Gold received = min(floor(G / 10), Payment limit)
```

| Your fame | Most gold per successful request |
|---:|---:|
| 0–2,499 | 10 |
| 2,500–4,999 | 11 |
| 5,000–7,499 | 12 |
| 7,500–9,999 | 13 |
| 10,000 or more | 14 |

For example, an NPC with 95 gold can give **9 gold**. One with 1,000 gold can give **10–14**, depending on your fame. Higher Begging improves the chance of receiving money, **not the amount of each payment**.

A successful payment costs karma under the [reputation rules](#reputation-costs). An ordinary money request does not itself cost fame. A failed request still uses the delay, but does not apply that successful-payment karma penalty.

This makes direct begging a modest income source and a training option, with diminishing returns as the NPC runs out of money and your own karma falls.

## Pleading with creatures

### What happens

Use Begging and target an eligible creature within **12 tiles**. You do not need an instrument, and you do not need the begging demeanor enabled. The creature does not have to be attacking you at that exact moment: eligibility is based on its type and behavior settings.

On success, its combat target and war mode are cleared, and it is **pacified** for a calculated period. Ordinary AI waits instead of continuing its normal fighting behavior. Use the opening to leave danger.

There are several limits:

- Town-person and vendor families, citizens, player-run vendors, and barkeepers are excluded from this creature-pleading route.
- Controlled creatures and creatures configured to fight only aggressors, or not fight at all, are excluded from this route.
- A creature marked uncalmable refuses automatically. The default uncalmable rule includes bard immunity and dead pets; individual creatures can add their own immunity.
- An already pacified creature cannot have its peace refreshed through this action.
- **Damage can break the peace early.** It is not protection for attacking a helpless target indefinitely.
- A resolved plea takes **10 seconds of shared skill delay**, whether it succeeds, fails, or finds the creature immune/already pacified.
- These attempts cost **karma and fame**, including failed and refused pleas. See [reputation costs](#reputation-costs).

### Creature difficulty

The game measures the creature's maximum resources and trained skills, then adds difficulty for certain abilities.

```text
R = 1.6 × maximum hits + maximum stamina + maximum mana
    + floor(total base skill points)

If R > 700:
    R = 700 + floor((R - 700) × 3 / 11)

Add to R:
    100 if it uses Mage AI and has more than 5.0 base Magery
    100 if it has a breath attack
    100 if it has any poison immunity
    100 if it is a Vampire Bat or Vampire Bat Familiar
     20 × its hit-poison tier, if it has poisonous melee hits

D0 = R / 10
Add 40 to D0 if it is a Paragon
D0 = min(D0, 160)
```

Hit-poison tiers are **1 for lesser, 2 for regular, 3 for greater, 4 for deadly, and 5 for lethal poison**. Each tier therefore adds 2 difficulty points after division by 10. The final 160 limit applies under this checkout's expansion settings.

In plain language: tougher, more skilled creatures are harder to persuade. Very large resource totals are softened before the ability bonuses are added. Each listed 100-point ability bonus becomes **10 difficulty points**, and Paragon adds **40** before the final cap.

Maximum hits are used, so wounding the target does not directly lower this resource part of the difficulty. Damage can instead make existing peace easier to break. Aura damage and undead summoning are mentioned in an unimplemented source comment; they do not currently receive their own extra difficulty surcharge here.

### Your chance to persuade it

First apply Begging's own adjustments:

```text
D = D0 - 10 - 0.5 × max(B - 100, 0)
L = D - 25

H = D + 25   if D + 25 < 100
H = 126      otherwise

Chance = 0                         if B < L
Chance = 1                         if B >= H
Chance = (B - L) / (H - L)          otherwise
```

**D0** is the creature's difficulty from the previous section. **D** is that difficulty after your Begging bonus. **L** and **H** are the lower and upper ends of the final skill check.

Every point over 100 helps twice: your skill is higher, and the difficulty falls by another half-point. The shared 126 rule still applies, which is why using a simple 50-point success window for every creature gives incorrect answers.

| Creature difficulty D0 | At B = 80 | At B = 100 | At B = 120 | At B = 125 | At B = 126 |
|---:|---:|---:|---:|---:|---:|
| 60 | 100% | 100% | 100% | 100% | 100% |
| 100 | 24.59% | 57.38% | 91.55% | 98.64% | 100% |
| 130 | 0% | 16.13% | 85.37% | 97.70% | 100% |
| 160 | 0% | 0% | 45.45% | 92.59% | 100% |

These are the chances **after eligibility and immunity checks**, with skill held constant. A skill gain during the preliminary training check can slightly change the value used by the following persuasion check.

Worked example: with **120 Begging against D0 = 100**, your adjusted difficulty is `100 - 10 - 10 = 80`. The lower limit is `55`, and the upper limit becomes `126`. Your chance is `(120 - 55) / (126 - 55) = 65 / 71`, or **91.55%**.

There is an unusual boundary: when `D + 25` reaches 100, the upper limit jumps to 126. Chances can change abruptly there. The piecewise formula above includes that behavior.

### How long the peace lasts

```text
Peace duration in seconds = clamp(100 - D / 1.5, 10, 120)
```

Lower adjusted difficulty means longer peace. Above 100 Begging, each extra point adds about **one-third of a second**, unless the 10- or 120-second limit is already controlling the duration.

| Creature difficulty D0 | Duration at B = 100 | Duration at B = 120 | Duration at B = 150 |
|---:|---:|---:|---:|
| 60 | 66.67 seconds | 73.33 seconds | 83.33 seconds |
| 100 | 40 seconds | 46.67 seconds | 56.67 seconds |
| 130 | 20 seconds | 26.67 seconds | 36.67 seconds |
| 160 | 10 seconds | 10 seconds | 16.67 seconds |

Durations describe a **successful** plea; they do not imply that the listed skill can succeed against that target. These creature timers use ordinary elapsed time, while shared skill reuse follows the combat clock used by the shard.

When a pacified creature takes damage, the normal creature damage handler checks:

```text
Chance to break peace on that damage event
    = clamp(current missing hit points / 1000, 0, 1)
```

That is based on its **total missing health at the check**, not merely the size of the latest hit. Missing 100 hits means a 10% break chance; missing 500 means 50%; missing 1,000 or more means certainty. Additional creature-specific behavior may also matter.

## Pleading with players

Begging can clear another player's combat target and turn off their war mode. It **does not paralyze a normal player, impose a lasting truce, prevent retargeting, or replace the shard's PvP rules**.

Two paths exist:

| Target player | How the plea is checked |
|---|---|
| Criminal, or has at least one kill | Enters the first hostile-target path; uses the difficulty-based persuasion check above |
| Neither criminal nor carrying a kill | Must currently have you selected as their combatant **and** be in war mode; then passes the Intelligence gate below before the difficulty-based check |

For the second path:

```text
Intelligence gate chance = clamp(target Intelligence / (floor(B) + 1), 0, 1)
Overall success chance = Intelligence gate chance × persuasion chance
```

The game rolls a whole number from 0 through `floor(B)` and continues only if the target's Intelligence is greater than that roll. At B = 100 against 50 Intelligence, this gate passes **50 / 101 = 49.50%** of the time, before the persuasion check.

This is an unusual rule: higher target Intelligence helps the gate, and increasing your Begging makes that preliminary gate harder while improving the later persuasion check. Do not assume this is a dependable PvP escape skill. A failed Intelligence gate can produce no extra failure message, but still consumes the delay and reputation cost.

## The begging demeanor: prices and rewards

Use Begging on yourself to turn the demeanor on. The game confirms, **“You set your demeanor to begging.”** Use it on yourself again to turn it off.

The toggle itself has **no karma or fame cost**, makes no training roll, and does not impose a new ten-second delay. You still need to be able to activate the skill, and doing so reveals you. The saved setting survives logging out; it has no automatic expiration.

Most demeanor benefits use your **current** skill when the price or reward is calculated. Equipping a bonus after toggling changes the calculation; the toggle does not save a snapshot of your skill. Demeanor benefits do not require a successful persuasion roll.

### Selling ordinary items to NPCs

For standard NPC sell-list transactions, Begging can replace Mercantile as the bargaining skill. **They do not add together, and the game does not automatically choose whichever is better.**

The selection works as follows:

1. Start with whole-number effective Mercantile.
2. If it is below 100 and you share the vendor's nonempty NPC guild, use 100 and keep that guild benefit.
3. Otherwise, if your begging demeanor is active, replace it with whole-number effective Begging.
4. Limit the bargaining contribution to 100 for the ordinary sale calculation.

Let **Q** be the item's vendor valuation after quality/material adjustments but before the normal halving, and **b** the selected bargaining value:

```text
Ordinary sale price per item
    = max(1, floor(floor(Q / 2) × (1 + 0.03 × clamp(b, 0, 100))))
```

| Bargaining value | Payment multiplier on the halved valuation |
|---:|---:|
| 0 | 1× |
| 25 | 1.75× |
| 50 | 2.5× |
| 75 | 3.25× |
| 100 or more | 4× |

For Q = 100, the ordinary no-skill payment is 50 gold. At 50 Begging it is 125 gold; at 100 Begging it is 200 gold.

**A weaker Begging skill can hurt your price.** With 100 Mercantile and 50 Begging, turning the demeanor on changes that example from 200 gold to 125 gold. Even membership in the vendor's guild does not preserve the earlier fallback when your whole-number Mercantile was already at least 100.

**Opening the sell list can cost karma for each eligible item entry whose price uses Begging. Completing sales can apply further costs per submitted item entry, including entries where a guild fallback supplied the price.** These are not simply one reputation charge per shopping session. Turning the demeanor off prevents its future charges; it does not refund losses already applied.

Begging is **not a universal discount on the ordinary NPC Buy window**. Its purchase discounts come from the specific services and catalogue interactions below. It also does not override player-vendor prices or an NPC's access restrictions.

### Relics and stolen containers

These hand-ins use a different formula from ordinary sell-list items:

```text
Payment = V + round(V × B / 100)
```

**V** is the hand-in's base value. `round` means nearest whole gold; exact .5 ties go to the nearest even whole number.

For a relic worth 1,000 gold, Begging produces **2,000 at B = 100**, **2,200 at B = 120**, or **2,500 at B = 150**. This formula has no separate 100-point Begging cap.

- Standard vendor relic hand-ins normally use Mercantile; sharing the vendor's NPC guild adds another base value and prevents the Begging replacement. Without that guild match, active Begging replaces the Mercantile bonus.
- The Artist's relic hand-in uses the Begging bonus directly; that route does not add the ordinary vendor Mercantile/guild bonuses.
- Thief NPC hand-ins for the designated stolen boxes/bags use **V = 500**. Stolen chests use the container's recorded value. These thief hand-ins also require at least 10 whole-number effective Stealing.
- Qualifying Begging hand-ins cost karma and make a Begging training check.

### Cargo and museum antiques: bonuses that stack

Here Begging **does** add to the other listed bonuses, provided the demeanor is active:

| Reward | Extra gold from Begging | At B = 100 | At B = 120 |
|---|---|---:|---:|
| Cargo | `floor(base cargo value × B / 300)` | About +33.33% of base | +40% of base |
| Museum antique | `floor(base antique value × B / 400)` | +25% of base | +30% of base |

Cargo adds separate Seafaring and Mercantile bonuses, each calculated like `floor(base value × skill / 300)`, plus applicable Fishermen's Guild and port bonuses of 25% of base each. Museum antiques add Mercantile at `floor(base value × Mercantile / 400)` and a 25% base-value Merchants Guild bonus.

For example, a **1,200-gold cargo** receives **400 extra gold from 100 Begging**, on top of its other applicable bonuses. A **1,000-gold antique** receives **250 extra**. Each bonus is calculated from the original base value, not from a total already increased by other bonuses.

These Begging bonuses have no separate 100-point limit. Both systems apply a karma cost when their payment calculation requests it. **Opening the cargo value window also calls the cost-bearing calculation**, so viewing it while begging can lower karma even without selling. The ordinary antique preview uses the version without that karma charge; the hand-in applies it. Cargo and antique Begging charges can be silent.

### Discounts on services and merchant catalogues

The common service formula is:

```text
Discount = floor(original charge × B / 200)
New charge = original charge - Discount
```

Each skill point is worth **half a percentage point of discount** before whole-gold rounding. Most of these services enforce a minimum charge of 1 gold, but some use different zero-charge handling.

| Effective Begging | Nominal discount | Price of a 1,000-gold charge |
|---:|---:|---:|
| 25 | 12.5% | 875 |
| 50 | 25% | 750 |
| 75 | 37.5% | 625 |
| 100 | 50% | 500 |
| 120 | 60% | 400 |
| 125 | 62.5% | 375 |
| 150 | 75% | 250 |

The discount can be applied to a **unit price before multiplication**. A repair priced at 10 gold per durability point becomes 4 gold per point at B = 125: the discount is `floor(10 × 125 / 200) = 6`, so the final reduction is 60%, not exactly 62.5%.

These are the affected service families; each NPC still accepts only its usual item types:

| Service | Examples of providers or interactions |
|---|---|
| Equipment repairs and item identification | Blacksmiths, weapon/armor merchants, bowyers, carpenters, tailors, weavers, tanners, leatherworkers, fur traders, tinkers, jewelers, bards, Archer Guildmasters, and Garth, where that provider implements the relevant service |
| Unknown potion/keg identification | Alchemists and Alchemist Guildmasters |
| Unknown reagent identification | Herbalists |
| Scroll, wand, artifact, or clue identification/deciphering | Scribes, sages, and Librarian Guildmasters, depending on the item |
| Relic appraisal | Provisioners |
| Treasure-map deciphering | Mapmakers and Cartographers Guildmasters |
| Wand charging | Mages, Holy Mages, enchanters, and Roscoe |
| Familiar crystal-ball charging | Mage, Necromancer, and Elemental Guildmasters |
| Pack-animal item tending | Druid Guildmasters |
| Opening eligible locked containers | Thief NPCs |
| Making a hat, robe, or shoes into Jester attire | Jester NPCs |
| Revealing the meaning of supported documents | Gypsy Lady services for clues, search pages, books, notes, and data pads |
| Painting your portrait | Give an Artist a painting canvas; the base service costs 5,000 gold |
| Merchant catalogue purchases | Use the appropriate merchant book and meet its normal nearby-merchant/book conditions; compatible older sales books have the same discount |
| Resurrecting a dead henchman item | Healer service for fighter, wizard, archer, and creature henchmen |

Most discounted paid services also charge karma and offer a Begging training check. **The shown healer henchman resurrection, Gypsy Lady revelation, and Provisioner appraisal routes do not add that Begging reputation charge.** This does not remove any other consequence belonging to the service itself.

There are real exceptions to broad “all repairs are cheaper” claims. For example, the LeatherWorker's bear/deer-mask and whip/fighting/throwing-glove repair routes still charge karma for begging but do **not** reduce their actual repair charge. Check the price for the specific service.

Very high bonuses are not a promise of free service. At B = 200 the raw formula reaches zero, but merchant catalogues and many services floor the result to 1 gold. Some other services refuse a zero charge or interpret it as nothing needing repair/resurrection. Do not extrapolate the table into a universal free-service rule.

### Buying a shoppe and recruiting a prisoner

These use a **flat gold reduction**, not the percentage formula:

```text
Gold saved = min(floor(25 × B), 3000)
```

| Effective Begging | Gold saved | Price of a new 10,000-gold shoppe |
|---:|---:|---:|
| 25 | 625 | 9,375 |
| 50 | 1,250 | 8,750 |
| 100 | 2,500 | 7,500 |
| 120 or more | 3,000 | 7,000 |

The same reduction applies to the price of recruiting an eligible caged prisoner as a henchman. It does **not** increase the reward for simply freeing the prisoner. These two discount routes do not themselves deduct Begging karma/fame or make a Begging training check. Normal eligibility and payment rules still apply; for example, the shoppe seller refuses characters with kills.

## Reputation costs

The often-seen “-40” is the **starting penalty**, not always the number removed from your character.

### Karma

Most Begging charges are applied only while your current karma is **greater than -2,459**. When a normal, unlocked-karma character receives one such charge:

```text
New karma = clamp(K - 40 - trunc(K / 100), -15000, 15000)
```

For a character whose karma is locked toward evil, the shared reputation rules multiply the negative starting penalty twice. The same Begging charge becomes:

```text
New karma = clamp(K - 160 - trunc(K / 100), -15000, 15000)
```

| Karma before the charge | Loss with normal karma | Loss with evil-locked karma |
|---:|---:|---:|
| 5,000 | 90 | 210 |
| 0 | 40 | 160 |
| -1,000 | 30 | 150 |
| -2,458 | 16 | 136 |
| -2,459 or lower | No new standard Begging charge | No new standard Begging charge |

The threshold is checked **before** the loss. At -2,458, a normal charge takes you to -2,474. Thus -2,459 is a stopping condition, not a floor that prevents crossing below it.

Karma loss can affect other reputation-dependent systems. For direct money requests, it immediately worsens trust once you become negative. For individual services, reputation charges may occur on a quote, preview, or each item rather than only after receiving gold.

### Fame

Creature/player pleading attempts also apply a fame charge:

```text
New fame = max(0, F - 40 - floor(F / 100))
```

At 5,000 fame, one plea costs **90 fame**. At 10,000 it costs **140**. Fame does not go below zero through this routine. These losses apply to failed pleas and immune/already-pacified creature responses too.

Ordinary gold begging and the usual demeanor bargaining charges do **not** add this fame penalty. Losing fame through combat pleas can nevertheless reduce the maximum gold you later receive from ordinary begging.

## Begging and Jester magic

Begging is the casting skill for all ten tricks: **Can of Snakes, Clowns, Flower Power, Hilarity, Insult, Jump Around, Popping Balloon, Rabbit in a Hat, Seltzer Bottle, and Surprise Gift**. Psychology supplies many secondary effects.

A practical starting setup is a **Bag of Tricks in your backpack**, **more than 10 effective Begging or Psychology**, and **at least one recognized piece of Jester clothing**. The identity check actually counts qualifying features; wearing additional recognized pieces can also satisfy it. You still need a bag containing enough prank points to cast. Being in the Jesters Guild and enabling the begging demeanor are not requirements for tricks.

Use the bag or the corresponding command, such as `[Hilarity`, `[JumpAround`, or `[CanOfSnakes`. The [Jester Magic guide](Jester_Magic.md) covers individual commands, mana/prank costs, targets, follower slots, and other restrictions.

### Casting chance

```text
Jester casting skill chance = clamp((B - 10) / 50, 0, 1)
```

The window is 10–60, so it does not trigger the shared 126 rule. At B = 20 the skill check has a 20% chance; at 35 it is 50%; at **60 it reaches 100%**. Other casting restrictions can still stop a trick.

Begging above 60 remains useful because it strengthens the tricks. Prank points are normally spent **before** the fizzle check, so a failed skill roll can still consume them. Lower Reagent Cost can waive the spend, although you must initially have enough points. Jester casting does not itself call the ordinary Begging karma/fame penalty.

### Direct Begging effects

| Trick | What Begging changes |
|---|---|
| Can of Snakes | One extra-snake roll at `clamp(floor(B) / 200, 0, 1)`; Psychology adds its own rolls |
| Clowns | One extra-clown roll at that same chance; Psychology adds its own roll |
| Rabbit in a Hat | Two separate extra-rabbit rolls at that same chance; Psychology adds its own rolls |
| Flower Power and Seltzer Bottle | Base damage is `1 + floor(B / 5 + P / 3)` before defenses; a ground-splatter roll uses `clamp((floor(B) - 49) / 251, 0, 1)` |
| Hilarity | Duration is `max(5, floor(P + (P + B) / 8) - target level)` seconds |
| Insult | Begging improves the shared duration value below; it does not directly increase the mana drained per tick |
| Popping Balloon and Surprise Gift | Begging improves summoned lifetime and the explosion's base damage through the shared formulas below |
| Jump Around | Begging affects casting success; it does not grant extra travel permission or a Begging-scaled teleport distance |

At B = 100, each extra-summon roll supplied by Begging has a **50%** chance. At 120 it is **60%**. These rolls do not bypass follower-space limits.

The splatter chance at B = 100 is `51 / 251 = 20.32%`; at 120 it is `71 / 251 = 28.29%`. A splatter is only placed if the spell's nearby-splatter check permits it. With B = 100 and P = 100, the direct attack starts at `1 + floor(20 + 33.333...) = 54` damage before defenses.

Hilarity is a separate paralysis effect. Its behavior should not be confused with the pacification applied by directly using Begging on a creature.

### Shared summon and duration math

Each call chooses a divisor **r**:

- `r = 1.5` if Psychology passes an integer roll from 1 to 400.
- Otherwise, `r = 1.8` if Psychology passes a new integer roll from 1 to 200.
- Otherwise, `r = 2`.

A smaller divisor means a stronger result. At P = 100, those possibilities occur **25%, 37.5%, and 37.5%** of the time respectively. Each helper call rolls separately, so one summon's duration and strength can receive different results.

| Shared value | Exact formula |
|---|---|
| Duration | `floor((10 + floor(B / 2) + floor(P)) / r)` |
| Strength value | `floor((10 + floor(B / 4) + floor(P / 2)) / r)` |
| Skill value | `floor((20 + floor(B / 2) + floor(P)) / r)` |
| Damage value | `floor((1 + floor(B / 25) + floor(P / 15)) / r)` |
| Explosion base damage | `10 + floor(B / 4) + floor(P / 2)` |
| Poison value | `floor((floor(P / 25) + 1) / r)`; no direct Begging term |
| Range value | `floor(P / 25) + 1`; no direct Begging term |

Can of Snakes and Rabbit in a Hat use the shared summon values. Insult lasts `floor(Duration / 2) + 1` seconds and drains `Range + 1` mana per 1.5-second tick. Balloon/gift summons use the duration, explosion, and range values. These are inputs to the relevant effects, not a promise of final damage against every opponent.

For B = 100 and P = 100, the shared duration is **80, 88, or 106 seconds**, depending on the divisor, and explosion base damage is **85**. Raising Begging helps even though casting success was already capped at 60.

## Training and skill increases

### Getting started

Set Begging to **Up** in your skill list and leave room under your total skill cap, or deliberately mark another skill Down. Your trained Begging must also be below its own cap.

Jester NPCs and Jester Guildmasters have Begging and can offer normal NPC teaching. Use their teaching/context option and pay the quoted gold. Normal teaching aims for one-third of the teacher's base skill, up to 42.0, subject to your cap and available skill room. Their generated Begging ranges mean roughly **21.6–29.3** from an ordinary Jester or **28.3–33.3** from a Jester Guildmaster, depending on that NPC's skill. The teaching price is **one gold per 0.1 point learned**; a 10.0 increase costs 100 gold. These teaching prices do not receive the service discount described above.

Practice can come from ordinary money requests, creature pleas, many bargaining/service checks, and Jester casting. You do not need to have money transferred on every normal NPC skill check to have a chance to train. However, a trust refusal or missing NPC backpack happens before that money-begging skill check.

Useful distinctions:

- Toggling the demeanor alone does **not** train Begging.
- Many demeanor interactions call a practice check without making the discount depend on its success.
- The first hostile-target pleading path makes an initial practice check before checking immunity and, for eligible targets, a separate persuasion check. These are opportunities, not guaranteed skill increases.
- Jester casting stops offering its Begging skill-roll training once effective Begging reaches 60, because the cast is no longer a challenge. Use other training methods after that.
- Money begging and many service practice checks cease being a challenge at effective Begging 126. Heavy bonus gear can therefore stop those checks from raising your base skill even when your base is below its cap.
- Normal gain amounts above the beginner range are 0.1, with separate accelerated-gain effects where applicable. Gaining is probabilistic and depends on your remaining skill room and cap.
- Thieves Guild and Jesters Guild membership qualify Begging for the shared guild skill-gain benefit.

The checked-in configuration disables the general anti-macro gain limiter. If a server enables it, Begging is one of the affected skills: eligible repeated target/location gain checks use a three-use allowance and a five-minute record window. That is a gain restriction, not a change to the basic success formula, and it is not a statement about the shard's rules for unattended play.

### Offline study and power scrolls

Begging study books exist in three tiers:

| Book | Training ceiling supplied by the book |
|---|---:|
| Standard | 70.0 |
| Advanced | 100.0 |
| Legendary | 120.0 |

Put the book in your backpack, set Begging Up, double-click it while out of combat, and log out to start the scheduled study. Your character's own cap still applies. The ordinary bookbinder stock includes standard Begging books when its stock roll includes them; this is not a guarantee that every vendor has one.

Study makes repeated gain attempts, rather than awarding a fixed amount per hour. A full 30 seconds supplies one potential 0.1-point attempt before book/session limits and the post-100 slowdown. Its per-attempt chance is `clamp(0.9 - 0.8 × effective skill / skill cap, 0.1, 0.9)`. See [Offline Skill Training](offline-skill-training.md) for the complete process and restrictions. Bonus gear can reduce study effectiveness because the calculation uses effective skill.

Begging power scrolls can raise its training cap, including to 125. **Raising the cap does not immediately raise your skill.** Begging is not listed in the power-scroll handler's four shrine-specific skill groups, so those particular shrine restrictions do not apply to it.

## Equipment and elixirs

### Equipment

- **Beggar's Robe:** +30 Begging and +100 Luck in its item definition. A compatible older robe type has the same listed skill/luck bonuses.
- **Jesters Guild ring:** +10 Begging, +10 Hiding, +10 Psychology, and +10 Stealth. It is bound to its recorded owner for equipping.
- Begging is also available to applicable generic skill-bonus item systems. Check the actual item's properties; a skill's presence in a loot/enchantment list does not guarantee a particular drop or vendor stock.

These equipment skill bonuses can exceed the normal 125 effective-skill limit. For example, **100 base Begging plus a +30 robe gives 130 effective Begging**, absent other modifiers. That reaches the direct gold-begging skill guarantee and improves the uncapped price/duration formulas, while the normal sale-price contribution is still capped at 100.

Luck is not a term in the direct Begging success, payment, or service-discount formulas. The robe's Luck property does not itself improve those rolls.

### Elixir of Begging

Drink an Elixir of Begging to add a temporary skill bonus. You cannot take another Begging elixir while that effect is active, and the shared elixir check allows at most two active elixir types. **When the Begging elixir expires, its removal routine also reveals you.**

Its Alchemy recipe lists **3 fairy eggs, 1 empty bottle, 1 pig iron, and 1 tourmaline**, with an Alchemy difficulty range of 60–120. These are crafting requirements; drinking the finished elixir does not require that much Alchemy.

Let **C** be effective Cooking, **T** effective Tasting, and **E** the potion-enhancement total:

```text
Potential bonus = 10 + floor(E / 8) + floor(C / 5) + floor(T / 5)
Actual added Begging = min(Potential bonus, max(1, 125 - floor(base Begging)))

Duration in whole minutes
    = floor((120 + 2 × E + 2 × floor(C) + 2 × floor(T)) / 120)
```

**E** is the equipment Enhance Potions total, limited to 50, plus an Alchemy bonus of 0/10/20/30. In the current implementation those Alchemy thresholds are **3.3, 6.6, and 9.9 effective skill**, because the comparison uses tenths of a skill point.

For example, at Cooking 100, Tasting 100, and E = 30, the potential bonus is `10 + 3 + 20 + 20 = 53`, and duration is `floor(580 / 120) = 4 minutes`. At base Begging 100, only **+25** is added; at base 120, **+5**; at base 125, **+1**.

The bonus calculation looks at **base** Begging when limiting the amount, and the temporary modifier can stack beyond 125 with equipment. At base 125, the minimum +1 can give **126 effective Begging** even without a robe. With zero Cooking, Tasting, and enhancement, duration starts at **1 minute** in the current formula; do not rely on older book text promising a different minimum.

## Other character effects

Begging contributes strongly to the Jester archetype in the Character Level system and is a supporting skill in the Thief archetype. That system gives a skill full archetype credit at 100 effective skill; raising Begging beyond 100 does not increase its normalized contribution there. It can still increase the Begging effects described elsewhere on this page.

For the Jester archetype specifically:

```text
Normalize a skill: n(skill) = clamp(effective skill / 100, 0, 1)

Jester power = 0.65 × n(Begging)
              + 0.20 × average(n(Psychology), n(Hiding), n(Stealth))
              + 0.15 if the character qualifies as a Jester
```

That is one input to character evaluation, not a direct “gain this many levels” award. Depending on the character-level/encounter configuration, training useful skills can also change how the game evaluates your character's strength. See the [Character Level reference](Character_Level_Recon_Report.md).

Begging's normal profession title is **Beggar**. When the character qualifies as a Jester, the title formatter changes Beggar to **Jester**; the corresponding Psychology/Scholar title becomes **Joker**. A title does not itself activate the demeanor or improve prices.

## Choosing how much Begging to keep

| Effective skill milestone | What it accomplishes |
|---:|---|
| 1 | Demeanor-based benefits become active when the toggle is on |
| Above 10 | Can supply the skill qualification in the practical Jester setup |
| 60 | Jester casting skill checks reach 100%; trick strength can still improve |
| 100 | Ordinary NPC sale-price contribution reaches its cap; common service discount is 50%; full normalized archetype credit |
| Above 100 | Additional difficulty reduction and longer peace for creature pleading |
| 120 | Shoppe/prisoner flat discount reaches its 3,000-gold cap |
| 125 | High-end trained skill supported by the power-scroll system; direct gold-begging skill chance is still 99.21% |
| 126 | Direct gold-begging skill check and eligible capped-difficulty creature persuasion checks reach 100% |
| Above 126 | Can still improve many service/reward formulas, creature peace duration, and Jester effects; each has its own limits |

Begging suits a Jester, a character who frequently buys NPC services, or an adventurer who wants another way to interrupt a creature. It can also supplement cargo and antique income without replacing Mercantile in those two systems.

If preserving positive karma and fame matters to your character, use it selectively. Turn the demeanor off when its automatic charges or lower bargaining value would outweigh the savings. Direct gold begging alone pays little, and a successful plea gives you an escape opportunity rather than lasting safety.

## Troubleshooting

| What you see | Likely explanation |
|---|---|
| Demeanor is on but nothing changes | Effective Begging is below 1.0, or that interaction does not use the demeanor |
| “Thou dost not look trustworthy...” | Negative karma failed the trust check before the skill roll |
| NPC has no money to give | Less than 10 backpack gold, or no payable gold remained |
| A creature cannot be persuaded | It may be excluded, uncalmable, already pacified, or above your effective ability |
| An enemy starts fighting again early | Peace expired, damage broke it, or additional creature behavior intervened |
| A player attacks again immediately | Player pleading clears a combat target/war mode; it creates no lasting restraint |
| My prices became worse | The demeanor replaced a stronger Mercantile value |
| Karma fell before a sale | Sell-list pricing or cargo valuation may already have charged it |
| The advertised service discount did not match exactly | Whole-gold rounding, unit pricing, minimum charges, or a route-specific exception |
| My trained skill stopped rising while wearing bonuses | The effective skill may have made the check automatic, or reached the study/skill cap used by that training path |
| I cannot start another skill after opening the cursor | Finish or cancel the target cursor; its temporary pending-action reservation is not a normal six-hour cooldown |

## Source trace and verification

This appendix is for maintaining the guide. The player instructions above do not require reading code.

Reviewed against source baseline **`62f5ebeb`**, branch **`Invasions`**. The runtime compiler recursively includes `.cs` files beneath `Data/Scripts`; `Scripts.csproj` was also checked for the Begging handler and both robe definitions. A file being under `Obsolete` does not by itself remove it from runtime compilation.

| Coverage | Current source and important symbols |
|---|---|
| Player entry point, toggle, hostile-target selection, difficulty, plea effects, gold timer, penalties | [Begging.cs](../../Data/Scripts/System/Skills/Begging.cs#L13): `Initialize`, `OnUse`, `IsGonnaAttack`, `GetBaseDifficulty`, `InternalTarget.OnTarget`/`OnTargetFinish`, `InternalTimer.OnTick`. Skill callback 6; player skill use, no special staff command or confirmation gump |
| Actual 126 upper limit | [Mobile.cs](../../Data/System/Source/Mobile.cs#L13647): `CheckSkill`, `CheckTargetSkill` |
| Check delegation, success/gain separation | [SkillCheck.cs](../../Data/Scripts/System/Skills/SkillCheck.cs#L72): `Initialize`, `Mobile_SkillCheckLocation`, `Mobile_SkillCheckTarget`, `CheckSkill`, `IsGuildSkill`, `AllowGain`, `Gain`; [XmlSpawnerSkillCheck.cs](../../Data/Scripts/Custom/XMLSpawner/XmlSpawnerSkillCheck.cs#L72): delegates the ordinary result and reports skill use to XML hooks |
| Effective value, lack of stat contribution, activation guards, titles | [Skills.cs](../../Data/System/Source/Skills.cs#L641): `TotalSkillValue`, Begging `SkillInfo` at line 847, `UseSkill` at 1506, title replacement at 300; [Target.cs](../../Data/System/Source/Targeting/Target.cs#L44): 12-tile cursor's inherited visibility/LOS/map enforcement |
| Expansion and settings | [CurrentExpansion.cs](../../Data/Scripts/System/Misc/CurrentExpansion.cs#L8): SA, hence SE/AOS rules; [Settings.cs](../../Data/Scripts/System/Misc/Settings.cs#L167): positional `Info/settings.xml` setting 7 and `NoMacroing`; [settings.xml](../../Info/settings.xml#L29): limiter disabled in this checkout |
| Saved demeanor | [PlayerMobile.cs](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs#L3874): `CharacterBegging`; current serializer version 37, read in version-29 segment at 4708 and written at 5103. Shared skill serialization is in `Skills.cs`. No save formats changed for this guide |
| Pacification and damage break | [BaseCreature.cs](../../Data/Scripts/Mobiles/Base/BaseCreature.cs#L796): `Uncalmable`; `OnDamage` at 5843; `Pacify` at 11991. [Behavior.cs](../../Data/Scripts/Mobiles/Base/Behavior.cs#L12042): `DoBardPacified` and dispatch at 13138 |
| Reputation scaling | [Titles.cs](../../Data/Scripts/System/Misc/Titles.cs#L14): `AwardFame`, `KarmaForEvil`, `AwardKarma` |
| Demeanor gate, quote/sale charges, relic/thief hand-ins | [BaseVendor.cs](../../Data/Scripts/Mobiles/Base/BaseVendor.cs#L226): `BeggingPose`, `BeggingKarma`, `VendorSell` at 1211, relic bonus at 1410, stolen-container bonuses at 2002, sale charges at 3015; [GenericSell.cs](../../Data/Scripts/Mobiles/Base/GenericSell.cs#L279): halving, bargaining cap, multiplier, minimum |
| Artist portrait and separate relic route | [Artist.cs](../../Data/Scripts/Mobiles/Civilized/Vendors/Artist.cs#L161): `OnDragDrop`, relic calculation at 287; its local `BeggingKarma` also calls the practice check |
| Cargo bonus and preview cost | [Cargo.cs](../../Data/Scripts/Items/Boats/Cargo.cs#L438): valuation window invokes `CargoTotalValue`; payment at 2667; `CargoBeggingGold`/`CargoTotalValue` at 2891–2935 |
| Museum stacking and optional charge | [Museum.cs](../../Data/Scripts/Quests/Museum/Museum.cs#L223): `AntiqueMerchantGold`, `AntiqueBeggingGold`, `AntiqueGuildGold`, `AntiqueTotalValue`, `GiveAntique` |
| Catalogue buying | [MerchantsBook.cs](../../Data/Scripts/Items/Books/MerchantsBook.cs#L926): purchase response, floor at 937, charge at 1409; [Obsolete.cs](../../Data/Scripts/System/Obsolete/Obsolete.cs#L37762): legacy `SalesBookGump.OnResponse`, purchase buttons below `NumItemsPlusOne`, page buttons at 100000+ |
| Flat discounts | [BaseShoppe.cs](../../Data/Scripts/Trades/Shoppes/BaseShoppe.cs#L233): 10,000-gold fee and capped reduction; [Prisoner.cs](../../Data/Scripts/Quests/Prisoners/Prisoner.cs#L810): response 1 frees for a reward, response 2 recruits with the reduction |
| Henchman resurrection | [BaseHealer.cs](../../Data/Scripts/Mobiles/Base/BaseHealer.cs#L237): `HealingTarget.OnTarget`, four henchman item branches; zero/negative result handling |
| Representative repair/service formulas and exceptions | [Blacksmith.cs](../../Data/Scripts/Mobiles/Civilized/Vendors/Blacksmith.cs#L218), [LeatherWorker.cs](../../Data/Scripts/Mobiles/Civilized/Vendors/LeatherWorker.cs#L301), [Jester.cs](../../Data/Scripts/Mobiles/Civilized/Vendors/Jester.cs#L162), [Provisioner.cs](../../Data/Scripts/Mobiles/Civilized/Vendors/Provisioner.cs#L196), [GypsyLady.cs](../../Data/Scripts/Mobiles/Civilized/Vendors/GypsyLady.cs#L337); catalogue of additional providers below |
| Jester identity and spells | [Players.cs](../../Data/Scripts/System/Misc/Players.cs#L166): `GetPlayerInfo.isJester`; [JesterSpell.cs](../../Data/Scripts/Magic/Jester/JesterSpell.cs#L12): skill window, costs, `Buff`; [Spell.cs](../../Data/Scripts/Magic/Base/Spell.cs#L931): base `CheckFizzle`; [BagOfTricks.cs](../../Data/Scripts/Magic/Jester/BagOfTricks.cs) and individual [trick scripts](../../Data/Scripts/Magic/Jester/Spells) |
| Jester effect consumers | [SummonedJoke.cs](../../Data/Scripts/Magic/Jester/SummonedJoke.cs#L159), [SummonedPrank.cs](../../Data/Scripts/Magic/Jester/SummonedPrank.cs#L88), [Hilarity.cs](../../Data/Scripts/Magic/Jester/Spells/Hilarity.cs#L152), [FlowerPower.cs](../../Data/Scripts/Magic/Jester/Spells/FlowerPower.cs#L63), [SeltzerBottle.cs](../../Data/Scripts/Magic/Jester/Spells/SeltzerBottle.cs#L63), [Insult.cs](../../Data/Scripts/Magic/Jester/Spells/Insult.cs#L62) |
| Teaching and study | [BaseCreature.cs](../../Data/Scripts/Mobiles/Base/BaseCreature.cs#L8302): `CheckTeachSkills`, `Teach`; [JesterGuildmaster.cs](../../Data/Scripts/Mobiles/Civilized/Guilds/JesterGuildmaster.cs#L27); [StudyBook.cs](../../Data/Scripts/Custom/Offline%20Skill%20Training/Items/StudyBook.cs#L110); `StandardBeggingStudyBook`, `AdvancedBeggingStudyBook`, `LegendaryBeggingStudyBook` in their respective [study-book files](../../Data/Scripts/Custom/Offline%20Skill%20Training/Items) |
| Training caps | [PowerScroll.cs](../../Data/Scripts/Items/Books/PowerScrolls/PowerScroll.cs#L180): `CanUse` shrine lists and `Use` changes the cap |
| Equipment | [Artifact_BeggarsRobe.cs](../../Data/Scripts/Items/Magical/Artifacts/Artifact_BeggarsRobe.cs#L9), [Obsolete_BeggarsRobe.cs](../../Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_BeggarsRobe.cs#L9), [GuildRings.cs](../../Data/Scripts/Items/Magical/GuildRings.cs#L204); [AOS.cs](../../Data/Scripts/System/Misc/AOS.cs#L1033): equipment skill modifiers do not obey the training cap |
| Elixir | [Elixirs.cs](../../Data/Scripts/Items/Potions/Elixirs/Elixirs.cs#L659): `ElixirBegging`, level 0 duration, `RemoveEffect`; [BaseElixir.cs](../../Data/Scripts/Items/Potions/Elixirs/BaseElixir.cs#L37): `Buff`, `DrankTooMuch`; [BasePotion.cs](../../Data/Scripts/Items/Potions/BasePotion.cs#L280): `EnhancePotions`; [Mobile.cs](../../Data/System/Source/Mobile.cs#L107): default skill modifier cap behavior; [DefAlchemy.cs](../../Data/Scripts/Trades/Crafting/DefAlchemy.cs#L553): recipe |
| Character evaluation | [CharacterLevelService.cs](../../Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs#L461): Thief/Jester archetypes, normalization at 558; [RandomEncounters/Helpers.cs](../../Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs#L175): older Thief support calculation, not a universal independent Begging damage bonus |

Additional service-provider implementations reviewed under `Data/Scripts/Mobiles/Civilized/`: `Vendors/Alchemist.cs`, `Armorer.cs`, `Bard.cs`, `Bowyer.cs`, `Carpenter.cs`, `Enchanter.cs`, `Furtrader.cs`, `Herbalist.cs`, `HolyMage.cs`, `Jeweler.cs`, `Mage.cs`, `Mapmaker.cs`, `Sage.cs`, `Scribe.cs`, `Tailor.cs`, `Tanner.cs`, `Thief.cs`, `Tinker.cs`, `Weaponsmith.cs`, `Weaver.cs`; `Guilds/AlchemistGuildmaster.cs`, `ArcherGuildmaster.cs`, `CartographersGuildmaster.cs`, `DruidGuildmaster.cs`, `ElementalGuildmaster.cs`, `LibrarianGuildmaster.cs`, `MageGuildmaster.cs`, `NecromancerGuildmaster.cs`; and `Special/Garth.cs`, `Special/Roscoe.cs`. Follow each provider's `BeginRepair` and target/payment handler rather than assuming its quotation covers every branch.

Cross-tree searches also classified generic skill lists, scroll/enchantment selections, race/shoppe skill-number mappings, and title entries. Their inclusion of Begging does not create a separate Begging-specific combat effect. The damage-aura/summoning difficulty TODO was distinguished from implemented behavior, and the player-only plea branch's creature/paralysis code was not described as an effect on normal `PlayerMobile` targets.

Verification for this documentation change: source and call-site review; independently calculated probability, price, duration, and reputation examples; local Markdown source/link/anchor checks; and `git diff --check`. No gameplay code or save data changed. No server build, runtime script compile, or live gameplay test was run for this documentation-only update. Custom live XML attachments, saved NPC modifications, and special creature overrides are outside the ordinary formulas documented here.
