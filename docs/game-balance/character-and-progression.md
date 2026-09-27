# Character and progression

[Start here](README.md) · [Systems at a glance](systems-at-a-glance.md) · [Connections](connections-and-balance.md) · [Coverage](coverage-and-evidence.md)

Analysis date: 2026-09-27. Baseline: `main`, `6568058a`. Scope: character creation, skill/stat progression, displayed character level, races, reputation, major permanent cap rewards, study, progression consumables, and death. This is documentation from current source, not a live-server balance verdict. No game files were changed and no server was started.

## Read this first

**Confictura has several overlapping forms of progression. There is no single character XP bar governing them.** A character can gain trained skills, a larger training budget, individual skill ceilings, spell access, equipment power, racial bonuses, fame/karma, and quest permissions. These advance through different activities and follow different limits.

The design direction you chose is to preserve a mixture of **lasting power, mastery, and discovery**. The source already supports all three. The balance question is how much each activity contributes, what it costs, and whether the resulting advantages combine sensibly. Removing lasting rewards is not a prerequisite for studying that question.

At a glance:

| What changes? | What it means | Lasting power | Main limit |
|---|---|---|---|
| Base skill | What the character has actually trained | Usually permanent; some resurrection paths reduce it | Individual cap and shared total skill budget |
| Effective skill | Skill after equipment, racial effects, elixirs, debuffs | Depends on the source | Normally 125, with a significant modifier exception |
| Individual skill cap | Permission to train a particular skill higher | Permanent cap unlock | Power scroll value, typically up to 125 |
| Total skill cap | How many trained points can coexist | Permanent breadth | Origin, server skill boost, Titan reward |
| Raw stats | Trained Strength, Dexterity, Intelligence | Usually permanent | Shared 250/300 budget; training checks allow 150/175 individually |
| Effective stats | Raw stats plus current modifiers | Mixed | Ordinary player getters clamp each at 150 in the configured SA ruleset |
| Character level | A derived estimate of current development | Recalculated; no XP pool | 1–100; combines skills, raw stats, reputation |
| Creature race | A persisted identity plus an equipped racial template | Mixed; bonuses scale with character level | Race selection/settings and template-specific benefits |
| Fame/karma | Reputation and moral direction | Persistent but changeable | Standard award helpers clamp at 15,000 and ±15,000 |
| Discovery/quest state | Access and completed objectives | Usually persistent | Quest-specific ownership, location and objective gates |
| Item level/points | A separate progression track on equipment | Item-specific | Covered by the equipment analysis; do not confuse with character level |

### Four definitions prevent most misunderstandings

1. **100 points in a skill is different from a cap of 100.** The cap allows training; it does not grant points.
2. **The total cap is a shared budget.** At a 1,000-point budget, ten skills at 100 consume everything; eight skills at 125 also consume everything.
3. **A displayed level of 100 does not mean every skill, stat, item, pet, or spell is maximized.** The calculation clips each skill's archetype contribution at 100.
4. **Source uses tenths for many totals.** `Skills.Total = 10000` means 1,000.0 skill points. `SkillsTotal` is also in tenths. Raw stats do not use this conversion.

Evidence: [Skills.cs:38–98, 556–619, 635–715](../../Data/System/Source/Skills.cs#L38); [Mobile.cs:1538–1551](../../Data/System/Source/Mobile.cs#L1538); [CharacterLevelService.cs:258–265, 558–615, 678–701](../../Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs#L258).

## Reusable system cards

These cards describe mechanisms and boundaries. They do not declare any playstyle overpowered without comparative encounter and player evidence.

### PROG-01 — Character creation and starting identity

**Purpose:** create a character with a starting allocation, then place that character into the shard's customized world/origin structure.

**Inputs → outputs:** client creation choices → initial stats, selected base skills, Human engine race, equipment and starting possessions. Later origin choices can change budgets and restrictions.

- `CharacterCreation` sets the raw stat cap to **250**, invokes `SkillBegin("default")`, forces `Race.Human`, and sets `Young=false`. Do not assume a normal UO young-player safety period.
- Creation allocates total raw stats of **90** for the newer creation protocol or **80** for the older protocol. Values must be 10–60 each; invalid post-normalization allocations fall back to 10/10/10.
- Custom skill selections accept a total of **100 or 120**, with each selection 0–50 and no duplicate positive skill. Default creation excludes Stealth except for the Ninja preset, and excludes Remove Trap and Elementalism.
- Presets grant two skills: Warrior Healing/Swords 50/50; Magician Magery/Meditation 50/50; Blacksmith Mining/Blacksmith 50/50; Necromancer Necromancy/Spiritualism 50/50; Paladin Knightship/Swords 51/49; Samurai Bushido/Swords 50/50; Ninja Ninjitsu/Hiding 50/50. These are initial allocations, not permanent class locks.

**Boundary:** the later gypsy/greeter/origin flow, world placement, starter item benefits and every identity-specific restriction require the corresponding world and combat chapters. `Profession == 1` is later reused for fugitives; it must not be interpreted globally as the Warrior creation preset.

Evidence: [CharacterCreation.cs:160–216, 358–379, 429–549](../../Data/Scripts/System/Misc/CharacterCreation.cs#L160); [ShardGreeter.cs:1006–1029](../../Data/Scripts/Mobiles/Civilized/ShardGreeter.cs#L1006).

### PROG-02 — Total training budgets and origin tradeoffs

**Purpose:** choose how broadly one character can develop.

| Origin/state | Base total budget | With Titan of Ether | Source-established tradeoff or distinction |
|---|---:|---:|---|
| Default | 1,000 | 1,500 | Standard baseline |
| Savage | 1,100 | 1,600 | Savage origin/starting region and equipment |
| Fugitive | 1,300 | 1,800 | `Profession=1`, minimum one murder maintained on region entry; standard resurrection price doubled |
| Alien | 4,000 | 4,500 | `SkillStart=40000`; Luck getter always returns 0; paid/helper resurrection still invokes permanent loss; standard price tripled |

Server `SkillBoost` adds 100 skill points per configured unit, clamped to 0–10 units. The checked-in value is **0**. These are source/check-in values, not a read of the running shard.

**Important implementation conflict:** `SkillVerification` only recognizes the eight exact total caps in the table and also requires trained total not to exceed the cap. An unrecognized cap causes it to reset all base skills to zero, set default origin, and sometimes reset raw stats to 20/20/20. `BaseRegion.OnEnter` calls it. Several nonzero global skill-boost choices would create unrecognized totals. Some boosts happen to collide with an accepted total, which introduces different origin-classification hazards instead. A future cap change must first reconcile this validator; blindly editing a number is unsafe.

**Effort/power implication:** an alien's larger budget buys simultaneous access to far more synergies. Extra training time and resurrection consequences are the explicit tradeoffs found here; whether they compensate for combat/economic breadth is a measurement question.

Evidence: [Settings.cs:1366–1423](../../Data/Scripts/System/Misc/Settings.cs#L1366); [Info/settings.xml:338–344](../../Info/settings.xml#L338); [PlayerSettings.cs:51–69, 137–159, 312–316](../../Data/Scripts/Mobiles/Base/PlayerSettings.cs#L51); [PlayerMobile.cs:1774–1900, 3677–3685](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs#L1774); [BaseRegion.cs:234–255](../../Data/Scripts/System/Regions/BaseRegion.cs#L234); [Players.cs:923–982](../../Data/Scripts/System/Misc/Players.cs#L923).

### PROG-03 — Base skill, effective skill and the real ceilings

**Purpose:** distinguish learned capability from temporary or equipped capability.

- `Base` is stored in tenths. `Value` returns `TotalSkillValue` after modifiers.
- Positive relative modifiers are separated by `ObeyCap`. Cap-obeying bonuses can fill toward the skill's cap. Non-cap-obeying bonuses are added separately.
- Final effective value is clamped to **125 only when the summed non-cap-obeying bonus is not positive**. A positive bonus in that category bypasses the final 125 clamp. Therefore 125 is not a universal effective limit.
- `DefaultSkillMod` starts with `ObeyCap=false`. Elixirs use that default. Racial/item/spell modifier types must each be checked before claiming how their values stack.
- Core skill code contains natural stat contributions below 100, but `CurrentExpansion.Configure` selects **SA** and invokes `AOS.DisableStatInfluences`, zeroing all skill stat scales. Do not count those natural bonuses in this checked-in ruleset.

**Example:** a character at base 125 drinks an applicable elixir. `BaseElixir.Buff` floors its computed bonus at 1, so its default non-cap-obeying modifier can produce effective 126. A character already carrying other bonuses may exceed that. This is a source-derived path to test, not proof of live player usage.

**Balance lens:** base-cap progression, equipment skill bonuses, racial bonuses and consumable bonuses must be evaluated together. A raw skill cap alone does not describe the final ceiling.

Evidence: [Skills.cs:556–619, 635–715](../../Data/System/Source/Skills.cs#L556); [Mobile.cs:96–127](../../Data/System/Source/Mobile.cs#L96); [CurrentExpansion.cs:8–25](../../Data/Scripts/System/Misc/CurrentExpansion.cs#L8); [AOS.cs:19–29](../../Data/Scripts/System/Misc/AOS.cs#L19); [BaseElixir.cs:37–76](../../Data/Scripts/Items/Potions/Elixirs/BaseElixir.cs#L37); [Elixirs.cs:56–88](../../Data/Scripts/Items/Potions/Elixirs/Elixirs.cs#L56).

### PROG-04 — Learning skills through use

**Purpose:** turn suitable actions into chances for permanent base-skill growth.

**Inputs → outputs:** use a skill at a relevant difficulty → success/failure and possibly a base-skill increase. Skills must be set Up to increase. Down skills can make room at the total cap; Locked skills are protected from that ordinary atrophy path.

The ordinary location/target check computes:

`success chance p = (effective skill − minimum) / (maximum − minimum)`

Below minimum returns failure; at or above maximum returns success before the ordinary gain routine. **Core `Mobile.CheckSkill` and `CheckTargetSkill` replace any supplied maximum of 100 or higher with 126.** A script saying `(0,100)` or `(0,125)` therefore usually means `(0,126)` through these overloads. Direct chance overloads do not receive this normalization.

**Example, minimum 0:** effective 100 has about **79.4%** success; 125 has about **99.2%** success; 126 succeeds automatically. This alone makes beyond-GM progression meaningful in many utility checks.

For the standard SA gain route, let:

- `H = unused total-budget fraction + unused individual-cap fraction`;
- `g = 2` normally, or randomly one of 1.0–1.5 for a relevant NPC-guild skill;
- `successTerm = (1 − p) × 0.5` on a successful action, and 0 on failure under AOS;
- `q = max(0.01, ((H / g) + successTerm) / g × skill gain factor)`.

If alive, an eligible gain roll calls `Gain`; base below 10 receives special treatment. Controlled pets double `q`. Under the ordinary routine, gains above the novice range are **0.1**. At base ≤10 a successful gain call awards **0.1–0.4** before acceleration. Seafaring at base 50 or more can only gain through this route while on a boat.

For example, total 500/1000, target 50/100, `p=0.5`, `g=2`, gain factor 1: `q` is 37.5% after a successful action and 25% after a failed action, giving 31.25% overall chance of a gain attempt. This assumes no anti-macro/faction/lock/cap obstruction. It is not a claim about minutes per point, because action rates and resource costs vary by skill.

**Details that matter:**

- `Gain` blocks jail, dead pets and creature Focus gains.
- Total-cap space is checked before applying the increase. Individual cap is checked before determining the eventual accelerated amount; a near-cap multi-point gain deserves testing for overshoot.
- The atrophy test uses integer `skills.Total / skills.Cap`, so it does not smoothly rise from 0 to 1 as the character approaches cap; below cap it evaluates to 0.
- `SkillCheck.Initialize` registers XMLSpawner wrappers; those call the ordinary handler and then the XML skill-use trigger. Attached/spawned content can therefore respond to skill use.
- Checked-in anti-macro setting is **false**. If enabled, configured skills allow three uses of the same remembered target/location, expire after five minutes, and use 5-tile location cells.
- `Settings.SkillGain()` currently returns 0 for configured values **1 through 10**, as well as 0. Its local variable is never assigned the in-range setting. A value above 10 returns 1.0. The checked-in setting is 0, so no gain acceleration comes from that control today.

Evidence: [Mobile.cs:13640–13677](../../Data/System/Source/Mobile.cs#L13640); [SkillCheck.cs:11–94, 115–246, 627–735](../../Data/Scripts/System/Skills/SkillCheck.cs#L11); [XmlSpawnerSkillCheck.cs:26–123, 265–290](../../Data/Scripts/Custom/XMLSpawner/XmlSpawnerSkillCheck.cs#L26); [Settings.cs:1426–1436](../../Data/Scripts/System/Misc/Settings.cs#L1426); [Info/settings.xml:29–39, 342–344](../../Info/settings.xml#L29).

### PROG-05 — NPC teaching and training fixtures

**Purpose:** purchase or practice an early foundation before difficult play.

NPC teaching targets one third of the teacher's base skill, capped at **42.0**, and refuses a teacher target below 20.0. It respects the student's individual cap and Up lock, and can lower Down skills to create total-budget space. Stealth teaching requires Hiding 50 under the configured SE-or-later rules. Actual teacher inventory, teaching eligibility and price flow are NPC-dependent.

Checked-in fixture controls are **25.0** for training dummies/daemons/archery buttes, **50.0** for pickpocket dips, and a **1×** gain-check multiplier. These are a separate source of early practice; they do not define general skill ceilings.

**Balance lens:** training difficulty is partly purchase/access friction, partly repetition, and partly task difficulty. These should be timed separately rather than called one generic grind.

Evidence: [BaseCreature.cs:8272–8408, 8607–8630](../../Data/Scripts/Mobiles/Base/BaseCreature.cs#L8272); [Info/settings.xml:305–315](../../Info/settings.xml#L305); fixture implementations: [TrainingDummies.cs](../../Data/Scripts/Items/Construction/Addons/TrainingDummies.cs), [TrainingDaemons.cs](../../Data/Scripts/Items/Construction/Addons/TrainingDaemons.cs).

### PROG-06 — Raw stats, usable stats and vitals

**Purpose:** train the attributes that feed survivability, physical ability and magic resources.

- Standard raw total cap is **250**; Titan reward makes it **300**.
- Ordinary gain permits each raw stat below **150** at a total cap ≤250, or below **175** above 250. Up/Down locks and the total budget still apply. A Down stat must be above 10 to be reduced.
- Stat checks occur inside `SkillCheck.Gain`; they can occur even when the selected skill itself is at its individual cap, provided it is Up and the gain routine is reached. Strength is checked first, Dexterity second, Intelligence third using each skill's gain weights and `StatGain` divisor.
- Checked-in stat divisor is **33.3**, with player delay **0 minutes** and pet delay **5 minutes**. These are not an absolute per-hour guarantee; gain opportunities depend on activities. `StatGainDelayNum` can mutate the setting to a minimum 5 for its display-oriented accessor, while the skill system captures its own static delay. Configuration initialization/call order should be verified when tuning it.
- In the configured SA ruleset, normal player **effective Str/Dex/Int getters clamp to 150**. Training a Titan raw stat to 175 therefore does not imply 175 usable stat through those getters. Some consumers read raw stats directly and still see 175.
- The separate setting called `PlayerLevelMod` multiplies player vitals and several restoration effects; it is **2.0** in checked-in settings. It has nothing to do with the 1–100 character level.
- Normal HP is `floor(effective Strength × vital multiplier) + item BonusHits`, with BonusHits capped at 25 for ordinary players under ML and a +20 addition for the specified Ninjitsu forms. Stamina and mana use the multiplier plus their separate item bonuses.

**Example:** at effective Strength 100 and BonusHits 20, the checked-in multiplier gives **220 HP**. At raw Strength 175 but effective Strength capped 150, the same bonus gives **320 HP**, not 370, absent a special override.

**Balance lens:** compare points that change a raw budget, points that change usable stats, and points that directly change HP. Their exchange rates differ.

Evidence: [SkillCheck.cs:716–735, 738–913](../../Data/Scripts/System/Skills/SkillCheck.cs#L716); [PlayerMobile.cs:1618–1711](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs#L1618); [Settings.cs:588–641, 853–880](../../Data/Scripts/System/Misc/Settings.cs#L588); [Info/settings.xml:33–43, 117–119](../../Info/settings.xml#L33); [CurrentExpansion.cs:8–12](../../Data/Scripts/System/Misc/CurrentExpansion.cs#L8); [Main.cs:269–286](../../Data/System/Source/Main.cs#L269).

### PROG-07 — Offline study

**Purpose:** trade book access and time away from play for chances to gain a selected skill.

**Inputs → outputs:** arm a study book in the backpack, log out, then log in → a batch of probabilistic `SkillCheck.Gain` calls; possibly a post-study acceleration buff.

- Standard books target **70**, advanced books **100**, legendary books **120**. The character's own individual cap still matters.
- Nominally one attempt is budgeted per **30 seconds**. The attempt count is clipped to remaining book headroom, reduced for the portion above 100, then capped at **350** per session. The raw 350-attempt time is **2 h 55 m** before book/headroom restrictions.
- Each attempt succeeds with `clamp(0.9 − 0.8 × effectiveSkill / individualCap, 0.1, 0.9)` and calls the shared gain routine. This is not guaranteed +35 skill.
- A normal one-hour session budgets 120 attempts before headroom adjustments. At starting effective skill 50 and cap 100, the initial chance is 50%; at ordinary 0.1 per successful attempt, approximately 6 points is only an initial-rate estimate, which falls as the skill rises.
- A session of **5 hours** can grant **30 minutes** of accelerated gains for the studied skill. A **5%** book-disintegration roll occurs before that buff; a destroyed book returns early.
- Study setup rejects combat, full individual skill and non-Up locks. One-book enforcement and login/logout scanning inspect top-level backpack items. Intended safe-region restrictions are commented out.
- Active study state, reader, start timestamp and pending logout flag are serialized (version 0), so this is persistent item state.

**Risks to test:** early returns inside `EndStudy` can bypass cleanup; a loop comparison uses effective whole points against a book ceiling stored in tenths; acceleration can alter gain amounts; headroom clipping happens before above-100 halving. Therefore neither a advertised book maximum nor the `SkillGainMax` comment is a guarantee of final gain. Verify near-cap sessions, existing acceleration, nested containers and relog/restart behavior before treating offline training as a predictable hourly economy.

Evidence: [StudyBook.cs:13–19, 101–246, 257–295, 315–420](../../Data/Scripts/Custom/Offline%20Skill%20Training/Items/StudyBook.cs#L13); [StandardStudyBooks.cs](../../Data/Scripts/Custom/Offline%20Skill%20Training/Items/StandardStudyBooks.cs); [AdvancedStudyBooks.cs](../../Data/Scripts/Custom/Offline%20Skill%20Training/Items/AdvancedStudyBooks.cs); [LegendaryStudyBooks.cs](../../Data/Scripts/Custom/Offline%20Skill%20Training/Items/LegendaryStudyBooks.cs). Example vendor book listing: [SBStudyBookbinder.cs:33–78](../../Data/Scripts/Custom/Offline%20Skill%20Training/Vendors/SBStudyBookbinder.cs#L33), 7,500 base-price standard manuals with randomized stocking.

### PROG-08 — Power scrolls: greater ceiling, no immediate points

**Purpose:** turn rare rewards into permission to train beyond 100.

Tiers are **105 Wondrous, 110 Exalted, 115 Mythical, 120 Legendary, 125 Power**. Use requires a higher value than the character's existing cap, the common special-scroll requirements and the relevant shrine for listed skills. `Use` sets that skill's cap to the scroll value; it does not increase base skill or the total skill budget.

| Shrine | Listed families |
|---|---|
| Strength | Melee/unarmed, Bushido, Tactics, Parry, Lumberjacking, Mining, Blacksmith, Carpentry, Bowcraft |
| Intelligence | Magery/Elementalism/Necromancy, Magic Resist, Meditation, Psychology, Anatomy, Arms Lore and several crafts |
| Dexterity | Bard offensive skills, Marksmanship, hiding/thieving/lock/trap/search skills, Ninjitsu |
| Wisdom | Spiritualism/Knightship, Peacemaking, Tracking, pet skills, Poisoning, Focus, Seafaring, Healing |

The common shrine checks are enumerations. Skills omitted from all four lists do not receive a shrine restriction from this method; do not extend the table to every skill by assumption.

`RandomPowerScroll` has two independent selections: a uniform skill choice 1–50 and a tier roll 1–100. Tier chances from its cutoffs are **49%, 20%, 15%, 10%, 6%**. This distribution applies to this factory, not every reward source; `CreateRandom` uses a different tier mechanism and a narrower skill list. Drop availability, champion map gates and other supply routes belong in the reward-source analysis.

**Example:** with total budget already full, a 125 Swords scroll does not let the character train Swords from 100 to 125 without lowering other skills or gaining a larger total budget.

Evidence: [PowerScroll.cs:40–130, 149–292, 382–406](../../Data/Scripts/Items/Books/PowerScrolls/PowerScroll.cs#L40); the five tier class files in [PowerScrolls](../../Data/Scripts/Items/Books/PowerScrolls).

### PROG-09 — Alacrity and Transcendence: speed versus direct points

**Alacrity:** consumes a scroll for **15 minutes** of accelerated gains for one skill. `SkillCheck.Gain` multiplies each eligible gain amount by random **2–5**, so this changes amount per awarded gain, not a flat probability multiplier. The scroll requires skill below cap, Up lock and no active acceleration.

**Transcendence:** directly adds the scroll's `Value` to base skill, clipped to the individual cap. At a full total budget it searches for a single Down skill with enough base points and lowers that skill. It rejects use during active acceleration. Successful use consumes the scroll.

**Balance lens:** supply of acceleration items and direct-point items affects the time cost of training. Prices/drop rates can change the importance of effort without changing the eventual skill ceiling, but those are future design choices, not changes made here.

Evidence: [ScrollofAlacrity.cs:76–170](../../Data/Scripts/Items/Special/Special%20Scrolls/ScrollofAlacrity.cs#L76); [SkillCheck.cs:656–697](../../Data/Scripts/System/Skills/SkillCheck.cs#L656); [ScrollofTranscendence.cs:76–170](../../Data/Scripts/Items/Special/Special%20Scrolls/ScrollofTranscendence.cs#L76).

### PROG-10 — Elixirs: temporary power linked to cooking and tasting

**Purpose:** temporarily raise effective skills rather than permanently training them.

Shared helper computes:

- `powerBudget = 10 + floor(EnhancePotions / 8) + floor(Cooking / 5) + floor(Tasting / 5)`;
- bonus `= min(powerBudget, max(1, 125 − floor(targetBaseSkill)))`;
- normal duration in minutes `= floor((120 + 2×EnhancePotions + 2×floor(Cooking) + 2×floor(Tasting)) / 120)`;
- stronger `level>0` duration uses divisor 60 instead.

Up to **two** tracked different elixir effects can coexist; repeating an active type is rejected. The helper uses effective Cooking/Tasting, so bonuses to those skills can improve further elixir effects. The implementation uses a temporary `DefaultSkillMod`; expiry removes it and reveals the character. The blanket comments claiming specific maxima should not be trusted without checking effective-skill and EnhancePotions limits.

**Example:** Cooking 100, Tasting 100 and Enhance Potions 0 produce a +50 budget and four-minute normal duration. A target skill at base 100 receives +25; base 50 receives +50; base 125 receives +1. Other modifiers can make the final effective value differ.

Evidence: [BaseElixir.cs:37–76, 81–285](../../Data/Scripts/Items/Potions/Elixirs/BaseElixir.cs#L37); representative [Elixirs.cs:35–110](../../Data/Scripts/Items/Potions/Elixirs/Elixirs.cs#L35); [Mobile.cs:107–127](../../Data/System/Source/Mobile.cs#L107).

### PROG-11 — Soulstones: move prior training

**Purpose:** preserve or transfer previously earned base skill so a character/account can change roles.

Putting a skill into the stone sets the source skill to zero; restoring assigns the stored base value and clears the stored amount. The restoration checks individual cap, Up lock and total-cap headroom, using Down skills to make space. It is transfer/reallocation, not an additional total-cap unlock.

The common gate checks accessibility, two-tile range, account binding when present, two minutes since combat, criminal state, safe logout area, alive state, faction sigil, casting, poison, paralysis and acceleration. Acquisition and fragment/special-stone variants were not fully audited here.

**Balance lens:** easy role switching changes the cost of specialization and the value of multiple characters. Treat account-wide access separately from simultaneous power on one character.

Evidence: [SoulStone.cs:168–243, 429–444, 572–697](../../Data/Scripts/Items/Special/SoulStone.cs#L168).

### PROG-12 — Titan of Ether: a major permanent breadth reward

**Purpose:** convert a multi-part discovery/combat quest into more build space.

An owned `ObeliskTip` with the four completed element flags enters the `ApproachObsidian` tile. It consumes that character's tips, triggers rewards, adds **500.0** to the total skill cap, records `SkillEther=5000`, and sets the stat cap to **300**. It also grants quest souvenirs and an Obsidian Gate. Entry acquisition (`ObeliskOnCorpse`) refuses characters whose stat cap is already above 250 and retrieves existing owned tips instead of issuing another one.

**Why this preserves the game's spirit:** it combines discovery, completion and a lasting power reward. The size of the reward can be evaluated against the number of new combinations it enables, not simply against the hours needed to complete it.

**Unexpected coupling:** the level estimator divides current skill/stat totals by their caps. A fully developed standard character with maximum reputation and a best archetype score of 1 goes from level 100 to approximately **91 immediately after earning Titan**, before training any new points: the skill fraction becomes 1000/1500 and the stat fraction becomes 250/300. That derived level is used elsewhere, including racial bonuses. This is a confirmed consequence of the formulas; its actual gameplay impact still needs a live controlled comparison.

Evidence: [ApproachObsidian.cs:24–75](../../Data/Scripts/Quests/Pagan/ApproachObsidian.cs#L24); [ObeliskOnCorpse.cs:26–83](../../Data/Scripts/Quests/Pagan/ObeliskOnCorpse.cs#L26); [CharacterLevelService.cs:258–265, 566–615](../../Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs#L258).

### PROG-13 — Character level: an estimate with real consumers

**Purpose:** summarize development for encounters and diagnostics. This service has no XP balance, spendable level points, training cost or saved level-up counter.

Let `A` be the strongest adventure archetype score, `S=baseSkillTotal/totalSkillCap`, `T=rawStatTotal/statCap`, and `R=(min(Fame,15000)+min(abs(Karma),15000))/30000`, with all fractions clipped 0–1.

`overall power = 0.45×A + 0.15×S + 0.25×T + 0.15×R`

`level = round(1 + 99×power, midpoint away from zero), clipped 1–100`

Each skill used for an archetype is **effective Value /100**, clipped 0–1. Most archetypes use 70% primary skill plus 30% mean support skills. Pair archetypes use 55% lower primary, 25% higher primary, 20% support mean. Karma/identity archetypes have separate weights.

| Archetype | Primary or special inputs | Supporting inputs |
|---|---|---|
| Martial | Best Swords/Fencing/Bludgeoning/Fist Fighting | Tactics, Anatomy, Parry, Focus, Healing |
| Archer | Marksmanship | Bowcraft, Tactics, Anatomy, Focus |
| Assassin | Best Fencing/Poisoning | Hiding, Stealth, Tactics, Anatomy |
| Ninja | Ninjitsu | Hiding, Stealth, Tactics, Focus, Fencing |
| Samurai | Bushido | Swords, Tactics, Parry, Focus |
| Knight | 60% Knightship;20% positive karma | 20% mean Swords/Tactics/Healing/Spiritualism |
| Death Knight | 55% Knightship;20% negative karma | 25% mean Swords/Tactics/Necromancy/Spiritualism |
| Arcane Mage | Magery | Alchemy, Inscription, Meditation, Magic Resist, Psychology |
| Elementalist | Elementalism | Meditation, Psychology, Focus |
| Necromancer | 70% Necromancy;10% negative karma | 20% mean Spiritualism/Poisoning/Magic Resist |
| Witch | Pair Necromancy/Forensics | Poisoning, Spiritualism, Tasting, negative karma |
| Druid | Druidism | Veterinary, Taming, Herding, Cooking |
| Holy Man | 60% Spiritualism;20% positive karma | 20% mean Healing/Anatomy |
| Mystic Monk | 70% Fist Fighting;10% monk identity | 20% mean Focus/Anatomy/Healing/**Mysticism enum skill** |
| Jedi | Pair Psychology/Swords | Tactics, positive karma, owned/legal Jedi identity |
| Syth | Pair Psychology/Swords | Tactics, negative karma, owned/legal Syth identity |
| Researcher | Inscription | Alchemy, Magery, Elementalism, Meditation, Psychology |
| Ranger | Tracking | Camping, Cartography, Marksmanship, Taming |
| Bard | Musicianship | Provocation, Discordance, Peacemaking |
| Thief | Best Stealing/Snooping | Lockpicking, Hiding, Stealth, Begging, Searching, Remove Trap |
| Jester | 65% Begging;15% Jester identity | 20% mean Psychology/Hiding/Stealth |

Additional diagnostic-only archetypes: Crafter, Gatherer, Merchant, Seafarer, CreatureRace and Alien. These are not included in the strongest-adventure maximum. Their skills can still affect total-skill fraction. Crafter uses 60% strongest and 40% mean of ten crafting skills. CreatureRace is `0.25+0.75×bestAdventure` when RaceID>0. Alien is `0.25+0.75×skillFraction` for the alien origin.

**Worked comparisons:**

- Best adventure score 1, full total skills/stats, no fame/karma → **level 85**.
- Same character with maximum fame and either maximum positive or negative karma → **level 100**.
- Same 1,000 trained skill points and full stats/reputation, but an alien's 4,000-point cap → about **level 89**, despite equal currently trained skills.
- Raising a relevant skill from 100 to 125 does not increase its archetype contribution. It still may increase the total base skill fraction, and can materially improve combat even when the level does not change.

**Direct and indirect consumers:**

| Consumer | Effect |
|---|---|
| `GetPlayerInfo.GetPlayerLevel` | Canonical wrapper and player statistics display |
| `RandomEncounters.Helpers.CalculateLevelForMobile` | Uses selected encounter alias for players; non-players retain legacy fame/karma/skill/stat score |
| `GetPlayerInfo.GetPlayerDifficulty` | Converts levels ≥25/50/75/95 into difficulty 0–4; quest selection consumers include standard, search, assassination and courier routes |
| `BaseRace.SetProperties` | Scales existing racial bonuses |
| `Hilarity.Timed` | Target player level shortens Jester control duration, with a minimum of 5 seconds |
| GM `[CharLevel` / `[CharLevelTarget` | Shows caps, scores and identity diagnostics; GameMaster access |

**Balance risks to measure:** gear damage/resistances/procs and pets are absent from this score; extra skill beyond 100 is clipped for archetypes; larger unfilled caps can lower the score; racial bonuses feed effective skills while the level then feeds racial bonuses. The service is useful for explaining current decisions, but is not a complete power meter.

Evidence: [CharacterLevelService.cs:74–180, 258–705](../../Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs#L74); [CharacterLevelCommands.cs:12–79](../../Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs#L12); [Players.cs:893–920, 1576, 1722](../../Data/Scripts/System/Misc/Players.cs#L893); [RandomEncounters/Helpers.cs:112–122](../../Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs#L112); [Hilarity.cs:152–181](../../Data/Scripts/Magic/Jester/Spells/Hilarity.cs#L152). Difficulty call sites: `StandardQuestFunctions.cs:330`, `SearchPage.cs:243`, `AssassinFunctions.cs:254`, `Courier.cs:573`.

### PROG-14 — Creature races: identity plus scaling bonuses

**Purpose:** play a creature identity with its own appearance, family, alignment, starting area, food rules and bonuses.

This is separate from engine `Race.Human`/`Race.Elf`. Custom identity uses `RaceID` and a `BaseRace` item on `Layer.Special`. `RaceDefined` contains **172 encoded template rows**, including visual variants. The companion JSON records every row; this is not a claim of 172 mechanically distinct or currently selectable races.

Templates define five resistances; Str/Dex/Int and vital bonuses; regeneration; night sight; hit/defense chance; casting; potion/cost modifiers; Luck; reflection; damage/speed; two skills; food and gender metadata. Most raw positive encoded values become multiples of 5; Luck uses ×300; the two selected skills start at +10. Missing skills use sentinel 100.

Only attributes already positive in the template receive level scaling:

| Property | Added at level L | Example at level 100 |
|---|---:|---:|
| Existing resistance, primary-stat, attack/defense chance, spell/weapon damage | floor(L×0.1) | +10 |
| Existing HP/stamina/mana, lower mana/reagent cost | floor(L×0.3) | +30 |
| Existing cast speed/recovery, regeneration | floor(L×0.03) | +3 |
| Existing potion enhancement | floor(L×0.4) | +40 |
| Existing Luck | L×5 | +500 |
| Existing reflection/weapon speed | floor(L×0.2) | +20 |
| Each existing skill bonus | floor(L/2) | +50, making the template bonus of 10 become 60 |

Other systems may cap or ignore part of these bonuses. The table describes racial attribute values, not uncapped final damage or defense.

Racial synchronization occurs on login/death/resurrection and through the **7-second thirst and 11-second hunger timers**, which request level updates. Some species do not eat, do not drink, consume blood or brains, or receive night sight. These link character identity directly to survival upkeep and consumable demand.

**Balance lens:** evaluate actual race templates at several progression stages and real combined equipment caps. A +60 racial skill bonus can substitute for substantial training or cross individual ceilings depending on modifier policy, so race is a build component rather than a cosmetic choice.

Evidence: [BaseRace.cs:26–37, 62–269, 2327–2546, 2551–2614, 3194 onward](../../Data/Scripts/Mobiles/Races/BaseRace.cs#L26); [HitsDecay.cs:16–56](../../Data/Scripts/Items/Food/HitsDecay.cs#L16); [StamDecay.cs:16–56](../../Data/Scripts/Items/Food/StamDecay.cs#L16); [Info/settings.xml:277 onward](../../Info/settings.xml#L277).

### PROG-15 — Fame and karma: progression, alignment and prices

**Purpose:** remember reputation and moral direction, with real mechanical consequences.

Standard `AwardFame` clamps to 0–15,000. A positive award is reduced by `currentFame/100`, while a negative award subtracts that quantity again. Thus equal enemies stop being meaningful fame sources as reputation rises.

Standard `AwardKarma` clamps to −15,000…15,000, subtracts `currentKarma/100` from the requested offset, and changes behavior when `KarmaLocked` is set. In the locked path a positive nominal award becomes negative, while negative nominal awards are doubled in `KarmaForEvil` and doubled again in `AwardKarma` before the current-karma adjustment.

Ordinary eligible unsummoned creature kills begin with fame `creatureFame/100` and karma `−creatureKarma/100`, grant only to looting-rights recipients and split within parties before the award helpers. This is not a character XP award.

**Example:** an enemy worth 10,000 fame produces 100 nominal fame; a player at 5,000 fame receives 50 after the helper adjustment, and at 10,000 receives 0. This creates a soft ceiling per enemy reward rather than a universal linear grind rate.

**Dependencies:** displayed level counts both positive and negative karma magnitude equally; some magical identities favor one sign; Rune of Virtue/Corruption requires compatible sign; NPC/world interactions and resurrection cost use different reputation rules. `GetResurrectCost` uses **negative karma**, not absolute karma, so virtue and evil can have different prices even when derived character levels match.

Evidence: [Titles.cs:11–157](../../Data/Scripts/System/Misc/Titles.cs#L11); [BaseCreature.cs:10716–10805](../../Data/Scripts/Mobiles/Base/BaseCreature.cs#L10716); [CharacterLevelService.cs:582–630](../../Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs#L582); [Players.cs:923–982](../../Data/Scripts/System/Misc/Players.cs#L923).

### PROG-16 — Rune quest and the two meanings of virtue

**Purpose:** reward collecting eight quest runes with reputation, identity resolution and a growing talisman.

An owned RuneBox with all eight flags is opened in either designated chamber. The virtue path directly sets Fame/Karma to 15,000/15,000, marks the Virtue key, clears kills/criminal status and can remove fugitive `Profession=1` while retaining a validation path for its cap. The corruption path sets 15,000/−15,000. Both grant a personal `RuneOfVirtue` talisman, souvenir/currency rewards, then consume the quest box.

The talisman inherits `LevelTalismanHoly`, so its item progression belongs with levelable gear. Ownership and karma sign gate equip; at **zero karma either sign passes these comparisons**. Region entry rechecks morality and moves an incompatible rune to the backpack.

This must be distinguished from the old **eight-virtue point system** still present inside `System/Obsolete/Obsolete.cs`. Its helpers and some call sites remain runtime-visible source, but `VirtueGump.Initialize` explicitly has a disabled body, so a class being present does not prove the usual virtue UI is available. Do not promise all stock UO virtue abilities from the file name or familiar vocabulary.

**Balance lens:** the quest combines discovery with a reputation jump and an ongoing item progression track. Its importance is larger than the initial currency reward alone.

Evidence: [RuneBox.cs:148–210, 228–254, 355–379](../../Data/Scripts/Quests/Runes/RuneBox.cs#L148); [RuneOfVirtue.cs:12, 55–124](../../Data/Scripts/Items/Magical/RuneOfVirtue.cs#L12); [BaseRegion.cs:240–255](../../Data/Scripts/System/Regions/BaseRegion.cs#L240); [Obsolete.cs:34936–34968, 35145–35246](../../Data/Scripts/System/Obsolete/Obsolete.cs#L34936).

### PROG-17 — Death, resurrection, protection and permanent loss

**Purpose:** impose recovery cost and risk after defeat. The permanent loss is attached to particular resurrection paths, not a blanket mutation in `PlayerMobile.OnDeath`.

| Path | Standard origin | Alien origin |
|---|---|---|
| Ordinary death | Corpse/inventory/buff/faction consequences; no direct `Death.Penalty` call in the inspected OnDeath | Same distinction; helper or resurrection path matters |
| Paid healer/shrine in `ResurrectCostGump` | Pays gold/tithe; `Penalty(false)` does no permanent reduction | Pays and receives normal permanent reduction |
| Warned no-tribute or instant resurrection | `Penalty(true)` when its threshold passes | Stronger `Penalty(true)` |
| Soul orb / automatic resurrection potion | Calls `Penalty(false)`, no reduction by this helper | Calls `Penalty(false)`, normal reduction |
| Life fountain / resurrection tile / staff helper callers | Calls `Penalty(false)` | Can reduce permanently without the paid gump confirmation |

Normal permanent reduction uses **0.95× raw stats and base skills**; stronger alien reduction uses **0.90×**. It never lowers a skill when the resulting value would be ≤35, and never applies its stat multiplication when the resulting value would be ≤10. Individual/total caps are not lowered. Skills can therefore be retrained within their existing ceilings.

Fame and **positive** karma pass nominal 10% or 20% loss into the standard award helpers. Those helpers subtract another current/100, so unlocked positive reputation loses approximately **11% or 21%**, subject to integer rounding. Negative karma is not touched by `Death.Penalty` at all. The gump prose claiming simple 10%/20% fame/karma loss is incomplete.

**Worked ordinary full penalty:** base skill 100 → 95; raw stat 100 → 95; fame 10,000 → 8,900; unlocked positive karma 10,000 → 8,900. Base skill 36 remains 36 because 36×0.95≤35. Base skill 40 becomes 38.

**Price:** the paid cost uses the older score based on Fame, `−Karma`, base total skills clipped at 1,000, and raw total stats clipped at 250. It scales its final 1–100 index by 20 gold: ordinarily 20–2,000; fugitives pay double; aliens pay triple. It is waived if total skill ≤200.0 or raw stats ≤90. Positive karma reduces this older index; negative karma increases it.

**Threshold mismatch:** the instant/no-tribute penalty checks use `SkillsTotal>200` = **20.0** total skill, plus raw stats >90. They do not use the paid-price novice threshold 2000 = 200.0. Do not describe a single consistent novice exemption across all resurrection paths.

**Concrete duplicate call:** the older `ResurrectGump` tithe-payment branch calls `Penalty(false)` twice, while its bank branch calls once. This matters to aliens; ordinary characters' false calls do nothing. Whether that gump is used for a particular live healer must be established through its caller/placement.

**Fountain exception:** `LifeFountain.OnDoubleClickDead` calls `Ankhs.Resurrect`, which opens a confirmation gump when eligible, then immediately calls `Penalty(false)`. That penalty call does not wait for acceptance or successful resurrection. For an alien, this path can reduce permanent values even if the confirmation is declined or the ankh helper refuses the location. The separate fountain movement path resurrects immediately within 15 tiles and then applies its false penalty.

The complete direct-call search found **16 calls across 10 files**: four in `Death.cs`; three in `ResurrectGump.cs`; two in `LifeFountain.cs`; and one each in `SoulOrb.cs`, `AutoResPotion.cs`, `ResurrectTile.cs`, `ClientGump.cs`, `Commands/Interface.cs`, `Commands/Commands.cs` and `Commands/Gumps/AdminGump.cs`. This is a call-site inventory for this one permanent-loss helper; other resurrection mechanics can have independent effects.

**Separate temporary loss:** faction handling includes a one-third base-skill negative modifier for 20 minutes and suppresses ordinary skill gain during faction skill loss. This is separate from permanent raw/base mutation. Player death calls faction handling unless an XML challenge exemption applies; faction participation and alternate duel rules need their own audit.

Evidence: [PlayerMobile.cs:3263–3352](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs#L3263); [Death.cs:474–521, 779–878](../../Data/Scripts/System/Misc/Death.cs#L474); [Players.cs:923–982](../../Data/Scripts/System/Misc/Players.cs#L923); [Titles.cs:14–42, 85–125](../../Data/Scripts/System/Misc/Titles.cs#L14); [ResurrectGump.cs:180–221](../../Data/Scripts/System/Gumps/ResurrectGump.cs#L180); [SoulOrb.cs:75–138](../../Data/Scripts/Items/Magical/SoulOrb.cs#L75); [AutoResPotion.cs:135](../../Data/Scripts/Items/Potions/Special/AutoResPotion.cs#L135); [LifeFountain.cs:39–48](../../Data/Scripts/Items/Misc/LifeFountain.cs#L39); [ResurrectTile.cs:30](../../Data/Scripts/Items/Misc/ResurrectTile.cs#L30); [Obsolete.cs:11186–11234, 11433](../../Data/Scripts/System/Obsolete/Obsolete.cs#L11186); [SkillCheck.cs:629–630](../../Data/Scripts/System/Skills/SkillCheck.cs#L629).

Additional caller evidence: [Ankhs.cs:30–84](../../Data/Scripts/Items/Construction/Ankhs.cs#L30); [ClientGump.cs:193–203](../../Data/Scripts/System/Gumps/ClientGump.cs#L193); [Interface.cs:700–708](../../Data/Scripts/System/Commands/Commands/Interface.cs#L700); [Commands.cs:1063–1077](../../Data/Scripts/System/Commands/Commands/Commands.cs#L1063); [AdminGump.cs:4315–4324](../../Data/Scripts/System/Commands/Gumps/AdminGump.cs#L4315).

### PROG-18 — Side channels into permanent progression

**Referral stat reward:** `ReferrerReward` inherits `StatBall`; the default ball adds up to 10 to a chosen raw stat, caps that stat at 125 and respects the total stat budget. It consumes itself. This bypasses training time, not the total budget. Reward qualification/supply was not reviewed in this track.

**Government voting:** when `PlayerGovernmentSystem.NeedsForensics` is enabled, a vote performs a skill check on the candidate and also directly adds 0.1 Forensics when base is ≤99.9. That direct increment does not call the normal gain gate. Flag this as a cross-system progression edge; inspect the setting and election eligibility before describing live availability or abuse.

**Staff/test/XML controls:** test-center commands, staff stat/skill setters and XML property assignment can change values outside ordinary progression. These must be separated from legitimate player acquisition when auditing saves. `Info/settings.xml` has TestCenter false.

Evidence: [StatBall.cs:30–59, 179–248](../../Data/Scripts/Custom/TellAFriend/Rewards/StatBall.cs#L30); [TellAFriend.cs:382–409](../../Data/Scripts/Custom/TellAFriend/TellAFriend.cs#L382); [VotingStoneGump.cs:93–138](../../Data/Scripts/Custom/Government%20System/Gumps/VotingStoneGump.cs#L93); [BaseXmlSpawner.cs:1368](../../Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs#L1368); [Info/settings.xml:358–360](../../Info/settings.xml#L358).

## The skill vocabulary, at a glance

The core enum and `SkillInfo.Table` have **58 slots**. The following is a navigation glossary, not a claim that every skill has been independently combat-tested. Existing file names often preserve old UO names even when the visible skill has changed. The last three slots need special care: searches found framework/naming/race support, but no ordinary player training implementation for Imbuing or Throwing, and Mystic monk spells use Fist Fighting rather than the Mysticism enum.

| ID | Visible skill | Plain purpose / where to look |
|---:|---|---|
| 0 | Alchemy | Make potions; `Trades/Crafting/DefAlchemy.cs` and potion bases |
| 1 | Anatomy | Inspect bodies; healing/combat support; `System/Skills/Anatomy.cs`, Bandage/BaseWeapon consumers |
| 2 | Druidism | Animal lore/control support and druid spellcraft; `System/Skills/Druidism.cs`, `Magic/Druidism`, `DefDruidism.cs` |
| 3 | Mercantile | Identify items and affect trade; implementation still named `System/Skills/ItemIdentification.cs`; BaseVendor consumers |
| 4 | Arms Lore | Inspect equipment; craft/combat-related consumers; `System/Skills/ArmsLore.cs` |
| 5 | Parrying | Weapon/shield defense; BaseWeapon consumers |
| 6 | Begging | NPC interactions and Jester spell support; `System/Skills/Begging.cs`, `Magic/Jester` |
| 7 | Blacksmithy | Metal equipment crafting; `DefBlacksmithy.cs` |
| 8 | Bowcrafting | Ranged equipment crafting and Archer score support; `DefBowFletching.cs` |
| 9 | Peacemaking | Bard calming/control; `System/Skills/Peacemaking.cs` |
| 10 | Camping | Camp safety/exploration support; `Items/Explorers/Campfire.cs` |
| 11 | Carpentry | Wood items/equipment; `DefCarpentry.cs` |
| 12 | Cartography | Maps and exploration dependencies; `DefCartography.cs` |
| 13 | Cooking | Food and multiple consumable buffs; `DefCooking.cs`, Homestead package, BaseElixir |
| 14 | Searching | Detect hidden creatures, doors, traps and objects; `System/Skills/Searching.cs` |
| 15 | Discordance | Bard debuffs; `System/Skills/Discordance.cs` |
| 16 | Psychology | Intelligence/mana assessment and magical/Jedi/Syth/Jester support; `System/Skills/Psychology.cs` |
| 17 | Healing | Bandage healing/cure/resurrection support; `Items/Trades/Misc/Bandage.cs` |
| 18 | Seafaring | Fishing/sea activity; harvesting code still named `Trades/Harvest/Fishing.cs`; ordinary gains require a boat at base 50 or above |
| 19 | Forensics | Corpse examination/harvest and witch/government consumers; `System/Skills/Forensics.cs` |
| 20 | Herding | Herd animals; pet-management consumers; `Items/Weapons/Staves/ShepherdsCrook.cs` |
| 21 | Hiding | Become hidden; `System/Skills/Hiding.cs` |
| 22 | Provocation | Turn creatures against targets; `System/Skills/Provocation.cs` |
| 23 | Inscription | Write magical material and research support; `System/Skills/Inscribe.cs`, `DefInscription.cs` |
| 24 | Lockpicking | Open locked containers; lockpick item/treasure-container consumers |
| 25 | Magery | Wizard spells; `Magic/Base` and circle spell families |
| 26 | Magic Resistance | Resist magic and minimum resistance floor; PlayerMobile.GetMinResistance |
| 27 | Tactics | Weapon damage/ability support; BaseWeapon/WeaponAbility consumers |
| 28 | Snooping | Inspect others' containers; `System/Skills/Snooping.cs` |
| 29 | Musicianship | Instrument performance and bard support; BaseInstrument and bard skill handlers |
| 30 | Poisoning | Apply poison and strengthen poison-related effects; `System/Skills/Poisoning.cs` |
| 31 | Marksmanship | Ranged accuracy; `Items/Weapons/Bows/BaseRanged.cs` |
| 32 | Spiritualism | Spirit interaction/self-healing and sacred/necromantic support; `System/Skills/Spiritualism.cs` |
| 33 | Stealing | Item theft and custom stealing rewards; `System/Skills/Stealing.cs` |
| 34 | Tailoring | Cloth/leather crafting; `DefTailoring.cs` |
| 35 | Taming | Acquire/control creature allies; `System/Skills/Taming.cs`, BaseCreature control |
| 36 | Tasting | Examine food and foraging; improves elixir strength/duration; `System/Skills/Tasting.cs`, BaseElixir |
| 37 | Tinkering | Tools, devices and related crafting; `DefTinkering.cs` |
| 38 | Tracking | Locate targets and ranger identity; `System/Skills/Tracking.cs` |
| 39 | Veterinary | Heal/revive animals and pet support; Bandage/BaseCreature consumers |
| 40 | Swordsmanship | Sword weapon accuracy and several specialist paths; BaseWeapon and sword classes |
| 41 | Bludgeoning | Blunt weapon accuracy; BaseWeapon and mace/staff classes |
| 42 | Fencing | Thrusting weapon accuracy; BaseWeapon and fencing classes |
| 43 | Fist Fighting | Unarmed combat and **Mystic monk** spells; `Magic/Mystic/MysticSpell.cs:20–24` |
| 44 | Lumberjacking | Gather wood and some weapon support; `Trades/Harvest/Lumberjacking.cs` |
| 45 | Mining | Gather ore/resources; `Trades/Harvest/Mining.cs` |
| 46 | Meditation | Mana recovery; `System/Skills/Meditation.cs`, RegenRates |
| 47 | Stealth | Move while hidden; `System/Skills/Stealth.cs` |
| 48 | Remove Trap | Disable traps; `System/Skills/RemoveTrap.cs` |
| 49 | Necromancy | Necromantic spells and witch/death-knight support; `Magic/Necromancy` |
| 50 | Focus | Regeneration and multiple specialist supports; `System/Misc/RegenRates.cs` |
| 51 | Knightship | Holy/death knight magic paths; `Magic/Knight`, `Magic/Death Knight` |
| 52 | Bushido | Samurai combat abilities; `Magic/Bushido` |
| 53 | Ninjitsu | Ninja abilities and forms; `Magic/Ninjitsu` |
| 54 | Elementalism | Elemental magic; `Magic/Elementalism`; excluded from custom creation allocation |
| 55 | Mysticism | Framework slot; level score references it, but monk spells actually use Fist Fighting; ordinary training path not established |
| 56 | Imbuing | Framework slot; ordinary player skill implementation not established by current reference search |
| 57 | Throwing | Framework slot; ordinary player skill implementation not established by current reference search |

Names/IDs are established by [Skills.cs:38–98, 839–1018](../../Data/System/Source/Skills.cs#L38). Handler registration is at the beginning of the named `System/Skills` files. Stronger non-obvious examples: [ItemIdentification.cs:875–1161](../../Data/Scripts/System/Skills/ItemIdentification.cs#L875); [Searching.cs:119–273](../../Data/Scripts/System/Skills/Searching.cs#L119); [Tasting.cs:44–127](../../Data/Scripts/System/Skills/Tasting.cs#L44); [Spiritualism.cs:16–56, 199–248](../../Data/Scripts/System/Skills/Spiritualism.cs#L16); [MysticSpell.cs:18–24](../../Data/Scripts/Magic/Mystic/MysticSpell.cs#L18). Combat/craft descriptions are navigation summaries; their exact coefficients belong in the corresponding combat/economy chapters.

## What this says about effort and balance

The source supports the user's concern that accumulated effort purchases durable power, but it does not establish that every system was created without balance considerations. Explicit caps, origin penalties, probability curves, resource costs and ownership gates show constraints exist. The unresolved issue is whether those constraints remain coherent after customization.

The largest visible interaction chains in this track are:

1. **More total skill budget → more simultaneous support skills → stronger magic/pets/crafts/consumables → faster acquisition.** Measure combinations, not only one skill's marginal increase.
2. **Discovery quest → larger caps → initially lower derived level → changed race/encounter/control behavior → later retraining.** An achievement and its estimator can move in opposite directions.
3. **Racial skill bonus → effective archetype score → character level → larger racial skill bonus.** Test convergence and how much trained investment each template needs.
4. **Cooking/Tasting/potion enhancement → stronger temporary skill boosts → more reliable actions.** Support crafting can substitute for trained combat/utility points and enable more support power.
5. **Higher permanent skill → more effective farming → more skill-advancing rewards.** Actual loop strength depends on reward sources and trade, outside this track's formulas.
6. **Loss on selected resurrection paths → retraining time → return to the same ceiling.** This can be recovery friction rather than a lasting control on mature power; measure experienced players' avoidance options.

To preserve lasting power, mastery and discovery, record three distinct outcomes for every activity:

| Outcome | Practical question | Evidence needed |
|---|---|---|
| Lasting power | What can the character do tomorrow that they could not do yesterday? | Skill/cap/item/spell/state changes and actual benefit |
| Mastery | What becomes possible through better decisions at the same build strength? | Encounter execution, positioning, preparation and failure recovery comparisons |
| Discovery | What new place, rule, combination, story or choice does this reveal? | Access flags, quest sequence and novel options; do not reduce all exploration to hourly currency |

This is a framework for later comparison, not an approved rebalance plan.

## Findings ranked by what is demonstrated

| ID | Demonstrated source behavior | Balance/maintenance implication | Status |
|---|---|---|---|
| P-01 | Origin caps vary from 1,000 to 4,000 and Titan adds 500 | Large differences in simultaneous build breadth | Confirmed mechanism; comparative balance unmeasured |
| P-02 | Effective skill 125 clamp is bypassed by positive non-cap-obeying modifiers | A single published hard ceiling is misleading | Confirmed mechanism |
| P-03 | Skill-check maxima ≥100 become 126 | Nominal GM does not imply certainty | Confirmed mechanism |
| P-04 | Titan raw-stat training permits 175 but player stat getters clamp at 150 | Trained stat can be unusable to some consumers | Confirmed; audit raw-versus-effective consumers |
| P-05 | Skill budget validator accepts a fixed list and resets unsupported totals | Broad cap/config edits can erase trained progress | Confirmed source hazard; existing live occurrence unknown |
| P-06 | `SkillGain()` ignores values in the range 1–10 | Intended tuning control does not behave as described | Confirmed source defect; current value 0 unaffected |
| P-07 | Derived level clips archetype skills at 100 and omits gear/pets | Level cannot stand alone as total power metric | Confirmed metric limitation |
| P-08 | Bigger empty caps lower derived level | Cap reward can temporarily lower race bonuses | Formula consequence; live magnitude unmeasured |
| P-09 | Race templates scale bonuses with derived level | Potential self-reinforcing effective-skill loop | Confirmed dependency; stability/advantage hypothesis |
| P-10 | Study grants probabilistic calls, not promised flat points | Offline training needs measured rates | Confirmed; edge behavior needs runtime tests |
| P-11 | Penalty reputation helpers add another current/100 and ignore negative karma | UI explanation and moral-route recovery costs differ | Confirmed source/UI discrepancy |
| P-12 | One tithe path doubles `Penalty(false)` | Alien recovery may lose twice | Confirmed duplicate; affected live path needs confirmation |
| P-13 | Fee novice threshold 2000 versus penalty threshold 200 | New-player protection is path-dependent | Confirmed units mismatch |
| P-14 | Core virtue UI initialization disabled while helpers remain | File presence is insufficient for player availability | Confirmed partial activation boundary |
| P-15 | Three framework skill slots lack established ordinary training paths; level includes Mysticism | Score can depend on a skill not used by the named playstyle | Source search gap and confirmed monk/enum mismatch |
| P-16 | Government can directly grant Forensics outside common gain routine | A noncombat activity can modify permanent build state | Confirmed conditional edge; setting and eligibility need review |

## Coverage boundaries and next evidence

**Traced in detail:** creation/cap mutation, base/effective skill rules, ordinary use-based gain and wrappers, NPC teaching, stats/vitals, study lifecycle, power-scroll use and one random factory, acceleration/direct-point consumables, elixir helper and representative implementation, soulstone transfer, Titan reward gate, complete CharacterLevelService arithmetic and direct consumers, race template/scaling machinery, fame/karma award helpers and ordinary kill entry, rune reward/morality, permanent death helper and all textual direct callers.

**Inventoried but not exhaustively reviewed:** the first 55 gameplay-oriented skill families, every racial variant's selection gate/abilities, every spellbook/research/quest unlock, every item/consumable source, legacy virtue ability reachability, faction/duel variants, referral reward qualification, government prerequisites, all resurrection spells/items that never call `Death.Penalty`, map-specific starter paths, character account age/reward paths, transformations, corruption/evil playstyle restrictions, and all saved quest/title strings. These remain part of the overall game atlas rather than being silently declared complete here.

**Unknown without live data:** deployed commit and startup configuration; players' origin/cap distribution; legacy saved caps/modifiers; item/race/book ownership; actual spawner/vendor placement; time to reach 70, 100 and 125 in each skill; effective modifier combinations; death routes used; reward prices/supply; group size; macro usage; veteran versus new-player effective output. No source table establishes actual player distribution or what is fun.

**Useful first measurement cohort:** one new standard character, one established standard character, its identical pre/post-Titan copy, an alien with the same trained skills, and representative racial templates. Compare base/effective skills, raw/effective stats, derived level, vitals, encounter tier, damage/control/uptime, resource spend and recovery time at matching gear. Saved-state copies and isolated test data are required before such tests; none were run here.

## Review record

Published from the [dated source review](../codebase-audit/outputs/subagent-findings/2026-09-27-gameplay-progression.md). The [verification record](VERIFICATION.md) applies to the complete documentation package. Regenerate this chapter with `python docs/game-balance/tools/publish_chapters.py` after changing its review record.
