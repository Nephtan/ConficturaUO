# Searching, Hiding, and Stealth

This is the player and staff reference for Confictura's Searching, Hiding, and Stealth rules. It starts with the useful answers, then works down to the exact compiled formulas and source paths.

Use the [offline Searching, Hiding, and Stealth calculator](../tools/searching-hiding-stealth-calculator.html) for exact percentages. Download or open that single HTML file in any modern browser; it has no network dependency.

> **Rules snapshot:** This page describes the current Confictura checkout with Samurai Empire, Mondain's Legacy, and Stygian Abyss-era rules active, plus creature searching enabled. “Exact” means exact for the values and state entered. An opponent's hidden skills and a creature's Searching are normally not visible to players, so PvP and PvM predictions require a scenario estimate.

## Player guide

### The short version

- **Hiding** makes you hidden. A normal attempt succeeds roughly `Hiding / 126`, not `Hiding / 100`. At 100.0 that is 79.37%; at 120.0 it is 95.24%; at 125.0 it is 99.21%.
- **Stealth** lets a hidden character move quietly. You must already be hidden and have at least 30.0 displayed Hiding. Armor can sharply reduce the activation chance or make it impossible.
- **Searching** is aimed at a selected point. It may reveal hidden players and can find hidden traps, secret doors, and hidden chests. Ordinary Searching contests the target's Hiding; the target's Stealth does not enter that contest.
- **Creatures** use a different automatic scan. Their Searching competes against the better of your Hiding-derived and Stealth-derived concealment, with an extra benefit for base Stealth over 100.
- **Displayed skill matters** for almost all of these rules. The one explicit base-skill term is the creature-scan bonus for base Stealth above 100.
- Skill powerscrolls help only after the skill is actually trained above 100. Raising a cap without raising the current skill changes none of these percentages.

### Cooldowns and ranges

| Action | Reuse delay | Range or area |
|---|---:|---|
| Hiding | 2 seconds after a resolved attempt; 1 second when blocked by combat or a spell | Combat checks use the computed Hiding combat radius |
| Stealth | 2 seconds | No target; applies quiet-movement steps to the user |
| Searching | 6 seconds | Select a point within 12 tiles; search area is a square around that point |
| Gem of Seeing | Item targeting, 50 charges | Select within 12; fixed Searching 100 and radius 10, or radius 22 in a friend house |
| Reveal spell | Spell timing | Target within 10; reveal radius is `1 + floor(Magery / 20)` |

Distances in the Searching formulas are **Chebyshev distance**: `max(abs(dx), abs(dy))`. In plain language, the searched area is a square, so a target five tiles diagonally from the selected point is still five tiles away.

## Hiding

### Normal attempts

Outside a friend-owned house and without combat blockers:

```text
P(hide) = clamp(Hiding / 126, 0, 1)
```

Confictura's central skill-check wrapper changes every requested maximum of 100 or more to 126. That is why a 100.0 skill is not a guaranteed success and 125.0 still has a small failure chance. A displayed skill of 126.0 or more is guaranteed, although the normal published training range ends at 125.

Repeated independent attempts are:

```text
P(at least one success in n attempts) = 1 - (1 - P(hide))^n
```

This does not mean repeated attempts are always safe or possible; combat state, cooldowns, and revealing actions can change between attempts.

If a spell is currently assigned to the character, Hiding is blocked for one second and does not make an attempt. Under the current ML rules, starting Hiding also cancels an existing target cursor.

### Friend-owned houses

If you are inside a house where you are a friend, the **final Hiding roll succeeds automatically**. Combat is checked first, so an eligible attacker can still block the attempt. A one-sided current combatant can still require the preliminary combat roll described below.

### Hiding during combat

The combat inspection radius is:

```text
R = min(trunc((100 - Hiding) / 2) + 8, 18)
```

`trunc` means truncate toward zero, matching C#. The result is intentionally not clamped to zero.

| Displayed Hiding | Combat radius |
|---:|---:|
| 0.0 to 80.0 | 18 (upper cap) |
| 90.0 | 13 |
| 100.0 | 8 |
| 110.0 | 3 |
| 115.0 | 1 |
| 116.0 to 117.0 | 0 |
| 118.0 and above | Negative; see the compiled quirk below |

The combat cases are:

1. **No relevant combatant:** only the final Hiding roll applies.
2. **Your current combatant is nearby and in line of sight, but nobody in the radius is actively fighting you:** a preliminary `Hiding / 126` roll must succeed, followed by the final roll. Outside a friend house this is `(Hiding / 126)^2`; in a friend house it is one `Hiding / 126` roll.
3. **Any nearby mobile in line of sight has you as its combatant:** the attempt fails after the preliminary handling. This includes a creature or player actively fighting you.
4. **Smoke Bomb or Egg Bomb override:** the combat checks are skipped; only the final normal or house result remains.

The direct-current-combatant and nearby-attacker tests both require range and line of sight. The scan is not limited to players.

The preliminary `Hiding / 126` check is executed on every dispatched Hiding use, even when no combat state makes its result relevant. It changes success only for the one-sided-current-combatant case, but skill-check/gain hooks can still observe that extra check before the final check.

#### High-skill negative-radius quirk

At 118.0 Hiding and above, the compiled formula produces a negative combat radius. The range and line-of-sight combat tests then find no valid nearby mobiles, so the normal final Hiding roll proceeds even in combat. This guide and calculator preserve that behavior exactly; it is a source-confirmed quirk, not an intended-balance claim.

### Smoke Bombs and Egg Bombs

Both items temporarily turn on Hiding's combat override while they invoke Hiding. Their item checks require the bomb in the user's backpack, 50.0 Ninjitsu, no active skill delay, and 10 mana. They skip Hiding's combat blockers but do not change the final Hiding probability. Friend-house automatic final success still applies. Once the Hiding skill dispatch starts, the item spends 10 mana and consumes one bomb even when the final Hiding roll fails.

### Pets

On a successful Hiding attempt, the character leaves war mode and every controlled creature whose `ControlMaster` is that character is also set hidden. The compiled loop examines the world mobile collection and does not add a map or range restriction.

### What reveals you

Hiding is a state, not permanent invisibility. Common revealing actions include:

- speaking normally;
- making a weapon swing;
- casting or using an ability that explicitly reveals its user;
- pushing through another mobile when that push succeeds;
- failing Stealth, being unable to use Stealth because of its gates, or moving in a way that exhausts its allowance;
- being found by Searching, a creature scan, Tracking, Reveal, Eagle Eye, Mind's Eye, a Gem of Seeing, or another system that sets `Hidden` to false;
- item-specific actions whose scripts call `RevealingAction`.

There is no single global “all item interactions” list: individual spells, abilities, items, and systems decide whether to call the reveal action. When in doubt, assume an aggressive, noisy, or magical action may reveal you.

### Quiet door, chest, and corpse interactions

Confictura gives three special interactions the same chance to leave you hidden:

- opening or closing a door;
- interacting with a dungeon chest;
- opening a corpse.

```text
P(stay hidden) = clamp(floor(Hiding), 0, 125) / 125
```

Examples: 50.9 displayed Hiding uses 50 and gives 40%; 100.0 gives 80%; 120.0 gives 96%; 125.0 is guaranteed. This rule is separate from the normal `Hiding / 126` attempt.

## Stealth

### Prerequisites and failure consequences

Under the active ML/AOS rules, using Stealth requires:

- you are already hidden;
- displayed Hiding is at least 30.0;
- the Stealth action is not locked;
- armor value is below 42.

Failing any of these checks reveals the character. A failed Stealth skill roll also reveals the character. The action's cooldown is 2 seconds.

Backstab and Surprise Attack place a five-second action lock on Stealth when their attack validation succeeds. Trying Stealth during that lock reveals the character. Those attacks also reveal on resolution, including their explicit failure paths.

### Armor value

Stealth armor value is **not** the displayed physical armor rating. It is the sum of fixed table values for equipped `BaseArmor` pieces. A piece with the Mage Armor property contributes zero. Clothing and other equipped items that are not `BaseArmor` are ignored. Every compiled shield entry is zero.

| Material | Gorget | Gloves | Helmet | Arms | Legs | Chest | Shield |
|---|---:|---:|---:|---:|---:|---:|---:|
| Cloth | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Leather | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Studded | 2 | 2 | 0 | 4 | 6 | 10 | 0 |
| Bone | 0 | 5 | 10 | 10 | 15 | 25 | 0 |
| Spined, Horned, Barbed, Necrotic, Volcanic, or Frozen | 1 | 1 | 0 | 2 | 3 | 5 | 0 |
| Goliath, Draconic, Hellish, Dinosaur, or Alien | 2 | 2 | 0 | 4 | 6 | 10 | 0 |
| Ringmail | 0 | 5 | 0 | 10 | 15 | 25 | 0 |
| Chainmail | 0 | 0 | 10 | 0 | 15 | 25 | 0 |
| Plate | 5 | 5 | 10 | 10 | 15 | 25 | 0 |
| Dragon | 0 | 5 | 10 | 10 | 15 | 25 | 0 |

The calculator's armor builder exposes every compiled row separately, including rows with identical values, and a Mage Armor toggle for each body slot.

### Activation chance

Let `A` be the armor value and `S` be displayed Stealth.

```text
if A >= 42: impossible
minimum = -20 + 2A
raw maximum = 60 + 2A
effective maximum = 126 when raw maximum >= 100, otherwise raw maximum

P(activate) = 0                              when S < minimum
P(activate) = 1                              when S >= effective maximum
P(activate) = (S - minimum) /
              (effective maximum - minimum) otherwise
```

The curve changes abruptly at armor 20 because the raw maximum becomes 100 and the central wrapper rewrites it to 126. More armor can therefore cause a larger drop than the visible `+2 per armor point` suggests. Armor 42 is a guaranteed failure before any skill roll.

### Quiet steps, walking, running, and mounting

On a successful activation:

```text
allowed steps = max(1, floor(displayed Stealth / 5))
```

Current player movement has additional Samurai Empire behavior:

- **Walking:** each completed walking tile consumes one point. When a later walking move begins with zero points left, the server automatically calls Stealth again. Success refreshes the allowance; failure reveals you. The original allowance therefore covers that many completed walking tiles before an automatic recheck on the next tile.
- **Running:** each completed running tile consumes two points. You are revealed on the running tile that reduces the allowance to zero or below. The number of running tiles that can finish while still hidden is `max(0, ceil(steps / 2) - 1)`.
- **Mounted movement:** reveals immediately on the next completed movement.
- **Stealth below 25.0:** even if a very low-skill activation happens to succeed, the next completed movement reveals the player because the movement handler requires at least 25.0 Stealth.
- **Turning in place:** does not consume a step because the movement hook runs only for a completed tile move.
- **House customization:** the special player stealth-movement handling is bypassed while customizing a house.

These movement details are easy to miss: the base `Mobile` class has a simpler reveal rule, but current player characters use the `PlayerMobile` Samurai Empire override.

Fresh Hiding normally leaves the quiet-step counter at zero, so the first completed walking move automatically attempts Stealth for a player with at least 25.0 Stealth. That triggering tile does not consume a point from the newly granted allowance. The movement handler calls `Stealth.OnUse` directly, so this automatic retry does not pass through the ordinary skill-delay gate; its returned two-second delay is ignored. Running from a fresh zero counter reveals instead of auto-attempting.

### Skill cap comparison

Powerscrolls raise a cap, not the current value. The normal unarmored Stealth check has an effective maximum of 60, so any displayed Stealth of at least 60 automatically activates when all prerequisites are met. Above 100, the main direct benefits are more allowed steps and better resistance to creature scans; armor still changes the activation curve.

## Active Searching

### Selecting the search point

Searching asks you to select a point, mobile, or item within 12 tiles. The radius is measured from that selected point, not automatically from your character. Blessed characters are blocked before they receive the target cursor.

```text
full radius = floor(Searching / 10)
P(full radius) = clamp(Searching / 126, 0, 1)
half radius = floor(full radius / 2)
```

The server first rolls for full radius. A failure uses the half radius. In a house where the searcher is a friend, the radius is always 22 and the radius roll is skipped.

For a target at distance `d`:

```text
P(included) = 1                    when half radius > 0 and d <= half radius
P(included) = P(full radius)       when d <= full radius and the rule above did not apply
P(included) = 0                    when d > full radius
```

A computed radius of zero does not scan: the handler enters its mobile/item loops only when `range > 0`. This means Searching below 10 cannot scan at all outside a friend house. From 10.0 through 19.9, a successful full-radius roll can scan radius 1, but the failed half-radius branch becomes zero and scans nothing—even at the selected tile.

### Player-versus-player or player-versus-creature contest

For every hidden mobile inside the chosen area, including hidden players and creatures, the server independently rolls an integer from -10 through +10 for both sides:

```text
reveal when Searching + searcher roll >= target Hiding + target roll
```

There are 441 equally likely roll pairs. The exact contest chance is the number of winning pairs divided by 441. Equal Searching and Hiding gives `231 / 441 = 52.380952...%` because ties favor the searcher.

```text
P(overall detection) = P(target included) × P(contest won)
```

Target Stealth has no effect on ordinary active Searching. Staff access checks still apply. In a friend house, a hidden target inside the house is revealed automatically when included by the radius-22 search.

The active mobile enumeration does not make a per-target line-of-sight check. The selected square, access check, house condition, and skill contest determine the result. The full-radius skill check still executes in a friend house even though its range result is overwritten by 22; this can matter to skill-check/gain hooks but not to the detection percentage.

### Traps, secret doors, and hidden chests

Active Searching checks items inside the selected square and can detect:

- `BaseTrap` items;
- the hard-coded hidden-door art IDs used by the search handler;
- `HiddenTrap` items;
- `HiddenChest` markers.

Once an eligible item is inside the actual full or half radius, there is no second player-style skill contest. A `HiddenChest` marker creates its reward and deletes itself.

Hidden-chest search level is `clamp(floor(Searching / 10), 1, 10)`, followed by a uniform random level from 1 through that value. Reward selection then uses several random branches. The broad first split is two chances in three for currency/resources and one chance in three for a hidden box. Currency is uniform from 100 through `200 × chosen level`; map and region affect the actual resource/rare-item branches.

### Passive hidden-chest discovery

Walking over a hidden-chest marker makes a separate passive check only for a living `PlayerMobile` at Player access who is not Blessed:

1. Searching succeeds with `Searching / 126`.
2. If that fails, Night Sight may succeed.

The Night Sight threshold is:

```text
night threshold = 2 × Night Sight item attribute
                + 2 when the character's LightLevel is positive
P(night fallback) = clamp(night threshold, 0, 100) / 100

P(passive discovery) = 1 - (1 - P(searching)) × (1 - P(night fallback))
```

This passive roll is not the active Searching radius/contest system. The marker deletes after the crossing whether discovery succeeds or fails, so a failed passive check produces no reward and no later retry on that marker.

### Gem of Seeing

A Gem of Seeing has 50 charges. The gem must be within three tiles of the user. A valid use spends a charge before the target resolves, supplies a fixed Searching value of 100, and searches radius 10 from a point selected within 12 tiles (radius 22 in a friend house). It uses the same `±10` contest against target Hiding and the same eligible-item detection helper. HiddenChest reward level still comes from the user's actual displayed Searching, not the gem's fixed contest value. Attempting to use a zero-charge gem deletes it.

## Automatic creature detection

Creatures with positive Searching periodically inspect hidden, alive players inside their `RangePerception` and line of sight. Let:

- `M` = creature Searching;
- `H` = player's displayed Hiding;
- `S` = player's displayed Stealth;
- `B` = player's base Stealth.

```text
concealment = min(H / 4, S / 2.5)
power-scroll bonus = max((B - 100) / 5, 0)

P(detected per scan) = clamp(
    (M - concealment - power-scroll bonus) / 250,
    0.05,
    0.95)
```

The 5% floor and 95% ceiling apply after all terms. After `n` qualifying scans:

```text
P(detected at least once) = 1 - (1 - P(per scan))^n
```

The exact formula applies only when all gates are true: creature Searching is positive, the target is a hidden living player, the target is in perception range, and line of sight exists.

With creature searching enabled, processed creatures at Searching 10 or below are assigned `creature level + 10`. A creature source that already assigns more than 10 keeps that explicit value. Confictura's creature-level calculation is a separate fame, karma, skill, and stat score clamped from 1 to 125; the calculator lets you use `level + 10` or enter an explicit Searching value.

The exact creature-level derivation is:

```text
fame  = min(Fame, 15000)
karma = min(abs(Karma), 15000)
skills = trunc(1.5 × min(Skills.Total, 10000))
stats  = 60 × min(RawStr + RawDex + RawInt, 250)

first level = trunc((fame + karma + skills + stats) / 600)
level = clamp(trunc((first level - 10) × 1.12), 1, 125)
```

`Skills.Total` is the engine's summed fixed-tenths skill total. Normal creature data keeps Fame nonnegative; the compiled fame term has only an upper clamp, while Karma is explicitly made positive.

The AI delay is randomized after each scan. It first computes `floor(15000 / Intelligence)`, caps that value at 60, and chooses an integer from zero through that value. Because scheduling, target acquisition, line of sight, movement, and zero-second rolls all matter, time-based risk is only an approximation. “After N scans” is exact for N qualifying scans.

## Other ways to find hidden targets

The percentages below are the hidden-target difficulty checks after the spell or ability itself has successfully started and the selected point is valid. They do not include spell-casting success, mana, reagent, school, or item-use prerequisites. Range and access gates still apply. Reveal, Eagle Eye, and Mind's Eye enumerate their squares without a per-target line-of-sight test.

### Reveal spell

Spell-based Invisibility is automatically detected. Otherwise the spell calculates with fixed-point skill values (skill × 10, truncated to the stored tenths):

```text
chance integer = floor(
    50 × (MageryFixed + SearchingFixed)
       / (HidingFixed + StealthFixed)
)
```

If the divisor is zero, chance is 100. The test is `chance integer > random integer 0..99`, so the exact probability is the chance integer clamped from 0 to 100, divided by 100. Reveal checks mobiles in a square radius of `1 + floor(Magery / 20)` around the selected point.

### Eagle Eye

Eagle Eye uses the same fixed-point integer structure, replacing Magery with Ninjitsu:

```text
chance integer = floor(
    50 × (NinjitsuFixed + SearchingFixed)
       / (HidingFixed + StealthFixed)
)
```

Spell-based Invisibility is again automatic.

Eagle Eye's mobile and item radius is `1 + floor(Ninjitsu / 20)` around the selected point. Its current ML target range is 10.

### Mind's Eye

Mind's Eye first computes:

```text
jedi damage = trunc(clamp(Karma, 0, 15000) / 120) + Tactics + Swords
chance integer = floor(
    50 × (trunc(jedi damage) + PsychologyFixed)
       / (HidingFixed + StealthFixed)
)
```

The mixture of whole-number Jedi damage and fixed-tenths Psychology is the compiled rule. The calculator preserves it rather than “correcting” the units. Spell-based Invisibility is automatic.

Mind's Eye uses `1 + floor(Jedi damage / 20)` for hidden mobiles but `1 + floor(Psychology / 20)` for hidden items. Its current ML target range is 10.

#### Hidden items found by the three reveal abilities

Reveal, Eagle Eye, and Mind's Eye also inspect the same broad hidden-item families: traps, configured hidden doors, `HiddenTrap`, and `HiddenChest`. For a magical HiddenChest:

- Reveal derives maximum level as `clamp(floor(Magery / 16), 1, 6)`;
- Eagle Eye uses `clamp(floor(Ninjitsu / 16), 1, 6)`;
- Mind's Eye uses `clamp(floor(Jedi damage / 50), 1, 6)`;
- a chosen level is then uniform from 1 through that maximum;
- two outcomes in three delete the marker without a reward;
- on the remaining one-in-three branch, chosen level 1–4 gives `50..100 × level` gold, level 5 gives a level-1 HiddenBox, and level 6 gives a level-2 HiddenBox.

This deliberately weaker magical reward path is separate from active Searching's richer level-1–10 reward path.

### Tracking players

Tracking has an initial category-evidence check before any player contest:

```text
P(open category evidence) = Tracking / 21.1 below 21.1
                            1 at 21.1 or above
candidate-list range = 25 + floor(Tracking / 2)
active arrow range = 2 × candidate-list range
```

The skill reuse delay is 5 seconds. Candidates must also be alive, match the selected category, be in the same major-land grouping or named cave/dungeon region, and be inside the list range. At most 16 are displayed. The tracking arrow checks every 2.5 seconds and ends if the target changes map or moves outside the doubled arrow range. Tracking locates a qualifying hidden target; it does **not** set that target visible.

Tracking's player-target contest uses:

```text
divisor = HidingFixed + StealthFixed
chance integer = floor(
    50 × (2 × TrackingFixed + SearchingFixed) / divisor
)
```

Necromancy forms modify the divisor:

- Horrific Beast subtracts 200;
- Vampiric Embrace raises a divisor below 500 to 500;
- Wraith Form adds 200 when the divisor is 2000 or less.

A divisor of zero or less gives chance 100. The result is tested against a random integer from 0 through 99, so clamp the integer result to 0–100 for its exact percentage.

For a particular eligible player to appear, both the initial category-evidence check and the player contest must succeed. The handler also makes a separate passive-gain check from 21.1 to 100 after category selection; its boolean result is ignored and does not change the target list.

## Reading calculator results correctly

- **Exact per attempt** means the source formula has been evaluated for one qualifying attempt with the entered values.
- **Exact repeated probability** means independent attempts or qualifying scans were combined with `1 - (1-p)^n`.
- **Guaranteed gate** means source control flow returns success before a random roll, such as a friend-house final Hiding check.
- **Impossible** means a source gate returns failure, such as Stealth armor 42 or more.
- **Approximate time risk** estimates how many creature scans may happen in a period. It cannot predict AI scheduling, line of sight, or movement.
- Percentages do not predict skill gains. Skill-gain logic is related to checks but is not the same question as whether this attempt succeeds.

---

## Staff and maintainer reference

### Current-rule assumptions

- `Data/Scripts/System/Misc/CurrentExpansion.cs` selects `Expansion.SA`; therefore `Core.AOS`, `Core.SE`, and `Core.ML` branches described above are active.
- `Info/settings.xml` enables creature searching. Runtime configuration should be rechecked after any settings deployment.
- Displayed `Skill.Value` is used unless a formula explicitly names `Skill.Base`. The monster power-scroll term uses base Stealth.
- The guide deliberately preserves compiled integer division, truncation, fixed-point skill values, and the central max-126 rewrite.

### Central skill-check trace

| Source | Important symbol | Rule |
|---|---|---|
| `Data/System/Source/Mobile.cs` | `CheckSkill`, `CheckTargetSkill` | Any supplied maximum `>= 100.0` is replaced with `126.0` before the delegate runs |
| `Data/Scripts/System/Skills/SkillCheck.cs` | `Mobile_SkillCheck`, `CheckSkill` | Below minimum fails, at/above maximum succeeds, otherwise linear chance |
| `Data/Scripts/Custom/XMLSpawner/XmlSpawnerSkillCheck.cs` | skill-check wrapper | Preserves the default result while providing custom event integration |

### System source trace

| Behavior | Source and symbols |
|---|---|
| Hiding use, combat radius, house bonus, pets | `Data/Scripts/System/Skills/Hiding.cs`: `Hiding.OnUse`, `Hiding.InternalOnUse`, `Hiding.CombatOverride` |
| Smoke/Egg overrides | `Data/Scripts/Items/Trades/Ninjitsu/SmokeBomb.cs`, `EggBomb.cs`: item `OnDoubleClick`/target flow around `CombatOverride` |
| Quiet door/chest/corpse | `Data/Scripts/Items/Doors/BaseDoor.cs`, `Data/Scripts/Items/Containers/DungeonChest.cs`, `Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs` |
| Stealth armor and activation | `Data/Scripts/System/Skills/Stealth.cs`: `ArmorTable`, `GetArmorRating`, `OnUse` |
| Player movement consumption | `Data/Scripts/Mobiles/Base/PlayerMobile.cs`: `OnMove`; `Data/System/Source/Mobile.cs`: `Move`, `Hidden` |
| Backstab/Surprise Attack lock | `Data/Scripts/Magic/Ninjitsu/Backstab.cs`, `SurpriseAttack.cs`: `BeginAction(typeof(Stealth))` |
| Active Searching and item detection | `Data/Scripts/System/Skills/Searching.cs`: `OnUse`, `SearchTarget`, `DetectSomething` |
| Hidden-chest passive discovery/rewards | `Data/Scripts/Items/Containers/HiddenChest.cs`: `OnMoveOver`, `SpotInTheDark`, reward construction |
| Gem of Seeing | `Data/Scripts/Items/Magical/Artifacts/Minor/GemOfSeeing.cs` |
| Creature scanning and level assignment | `Data/Scripts/Mobiles/Base/Behavior.cs`: hidden-player scan and `IntelligentAction.GetCreatureLevel`; `Data/Scripts/Mobiles/Base/BaseCreature.cs`: level-plus-10 assignment |
| Reveal | `Data/Scripts/Magic/Magery/Magery 6th/Reveal.cs` |
| Eagle Eye | `Data/Scripts/Magic/Shinobi/Spells/EagleEye.cs` |
| Mind's Eye | `Data/Scripts/Magic/Jedi/Spells/MindsEye.cs`; `Data/Scripts/Magic/Jedi/JediSpell.cs`: `GetJediDamage` |
| Tracking | `Data/Scripts/System/Skills/Tracking.cs`: player tracking chance |
| Staff skill inspection | `Data/Scripts/System/Commands/Skills.cs`: `SkillsCommand.Initialize`; GM-only `[SetSkill <name> <value>` and `[GetSkill <name>` target a mobile |

For comparison only, upstream RunUO uses [Hiding.cs](https://raw.githubusercontent.com/runuo/runuo/master/Scripts/Skills/Hiding.cs), [Stealth.cs](https://raw.githubusercontent.com/runuo/runuo/master/Scripts/Skills/Stealth.cs), and [DetectHidden.cs](https://raw.githubusercontent.com/runuo/runuo/master/Scripts/Skills/DetectHidden.cs). The Confictura checkout is authoritative. Notable local differences include the Searching name/handler, the central `>=100 → 126` rewrite, extra armor materials, hidden items and chest rewards, creature auto-searching, quiet interactions, custom reveal abilities, and the Hiding combat override items.

### Known compiled quirks

- A requested skill-check maximum of 100, 125, or 250 all becomes 126. This affects normal Hiding, the preliminary combat roll, Searching's radius roll, passive chest discovery, and Stealth once its raw maximum reaches 100.
- Hiding's combat radius is not lower-clamped. It is zero at 116–117 and negative at 118+, effectively bypassing the combat-mobile tests.
- Hiding's preliminary 0–250 check is normalized to 0–126 and executes on every dispatched use, although it changes success only when the direct-current-combatant blocker was set.
- Active Search uses target Hiding but not target Stealth; creature search and magical reveal formulas use both.
- Active Search skips all mobile/item loops when the selected full or failed-half radius is zero; a friend-house override still supplies radius 22.
- Equal active Search/Hiding favors the searcher on ties, giving 231 winning pairs out of 441.
- Stealth's curve discontinuity starts at armor 20; armor 42 fails before the skill check.
- The `PlayerMobile` SE movement override auto-retries Stealth after the walking allowance. Do not describe current players using only the simpler base `Mobile.OnMove` rule.
- The monster delay uses integer division and a random integer that can be zero seconds. Time estimates are therefore not deterministic.
- Mind's Eye mixes whole-number Jedi damage with fixed-tenths Psychology.
- Tracking has a separate low-maximum opening check before its fixed-point hidden-target contest; it tracks but does not reveal the target.

### Owner-run in-game verification checklist

These checks require an isolated test/staging server and, where noted, two clients. They were not performed as part of this documentation-only change.

1. Use `[GetSkill Hiding`, `[GetSkill Stealth`, and `[GetSkill Searching`, then target the character, to record the current values. Use `[SetSkill <SkillName> <value>`, then target the character, to set controlled test values. Both commands require Game Master access. Restore the character afterward.
2. **Normal Hiding:** at Hiding 100, perform a large counted sample outside a house and combat. Confirm results trend near 100/126 rather than 100%. Repeat at 125.
3. **House Hiding:** friend one test character to a house; verify final success is automatic without combat, then repeat with a second client actively targeting the hider.
4. **Combat radius:** test an active attacker immediately inside and outside the expected radius at Hiding 100, 115, 116, and 118. Record LOS and exact tile distance.
5. **Combat override:** repeat a blocked combat attempt with Smoke Bomb and Egg Bomb, confirming item prerequisites, mana use, and that only the final roll remains.
6. **Pets:** control pets on the same map and, if safe, a different map; hide and confirm the source-observed global controlled-pet behavior.
7. **Quiet interactions:** at chosen integer Hiding values, count door, dungeon-chest, and corpse interactions; confirm 125 is guaranteed and fractional displayed skill floors.
8. **Stealth prerequisites:** verify not-hidden, Hiding below 30, action-lock, armor 42, and failed-roll paths all reveal.
9. **Armor matrix:** equip one known piece at a time, inspect its material/body position and Mage Armor property, and compare a counted activation sample with the calculator. Test armor values 19, 20, 41, and 42 explicitly.
10. **Movement:** after successful Stealth, count walking steps to the automatic retry, running step consumption, mounted reveal, below-25 Stealth movement, and turning in place.
11. **Active Search, two clients:** place the hidden target at the half/full boundary and just beyond it. Test equal skill, unequal skill, friend-house radius 22, and confirm target Stealth changes do not alter ordinary contests.
12. **Items:** place known hidden trap, hidden door, and hidden chest fixtures just inside/outside the selected radius. Verify active and passive chest flows separately and remove test rewards.
13. **Creature scan:** use a creature with known level/Search/Intelligence. Confirm the positive-Search, alive, hidden, perception, and LOS gates; then compare a counted number of qualifying scans rather than only elapsed time.
14. **Related methods:** with two clients, test Reveal, Eagle Eye, Mind's Eye, Tracking forms, and Gem charges using recorded skills. Test spell-based Invisibility separately because it is an automatic result for the three reveal abilities.
15. Save and restart only if normal staging procedure requires it. These mechanics add no new serialized state in this documentation change.

When publishing observations, record skill **Value and Base**, armor pieces and Mage Armor flags, coordinates/distance, house friendship, combatants, line of sight, expansion/settings state, number of trials, and the exact server revision.
