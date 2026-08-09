# Jester Magic

## Overview
Jester Magic is a command-and-gump driven trick system using spell IDs `260` through `269`. It has no dedicated `Spellbook` subclass and no scroll-learning path in this trace. Player access is centered on `BagOfTricks`, the `GetPlayerInfo.isJester` identity check, the shared `JesterSpell` base class, and ten player commands registered by `CastJesterSpells`.

The spell line uses `SkillName.Begging` as `CastSkill` and `SkillName.Psychology` as `DamageSkill`. Every trick has `RequiredSkill = 10`, uses `GetCastSkills()` range `10.0` through `60.0`, keeps hands equipped while casting, and uses `CastRecoveryBase = 2`.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `JesterSpell` | `Spell` subclass | Shared cast gates, prank-point spending, mana spending, fizzle messages, mantra, skill window, and trick scaling formulas. |
| `BagOfTricks` | `Item` | Stores `PrankPoints`, opens the main trick `Gump`, invokes trick commands from button IDs, and accepts gold near jester NPCs to refill prank points. |
| `BagOfTricksGump` | `Gump` | Main trick menu with cast buttons, info buttons, quick-bar launch buttons, and embedded jester help text. |
| `TricksLargeRow`, `TricksLargeColumn`, `TricksSmallRow`, `TricksSmallColumn` | `Gump` | Reusable quick bars whose buttons invoke spell IDs `260` through `269`. |
| `InfoJester` | `Gump` | Per-trick help panel showing name, details, prank point cost, and mana cost from `BagOfTricks.JokeInfo`. |
| `CastJesterSpells` | command registrar | Registers one `AccessLevel.Player` command per trick. |
| `SummonedJoke` | `BaseCreature` | Controlled temporary Mobile used by `CanOfSnakes` and `RabbitInAHat`. |
| `SummonedPrank` | `BaseCreature` | Controlled explosive Mobile used by `PoppingBalloon` and `SurpriseGift`. |
| `Clown` | `BaseCreature` | Temporary mirror-image style Mobile used by `Clowns`. |
| `Jester` and `ChucklesJester` | NPC `Mobile` classes | Count as nearby jesters for loading prank points into a bag. `ChucklesJester.DoJokes` is also called by `Hilarity`. |

`Initializer` registers the trick classes at IDs `260` through `269`. `SpellRegistry` includes the `Jester` namespace string, but `Spellbook.GetTypeForSpell` does not map IDs `260` through `269` to a spellbook type, so the compiled player path is the commands and bag gumps rather than a spellbook.

## Becoming A Jester
`GetPlayerInfo.isJester(from)` returns true only when the caller is a `PlayerMobile`, has a backpack, and accumulates more than two jester identity points.

| Identity point source | Compiled behavior |
| --- | --- |
| Bag | Any `BagOfTricks` found recursively in the player's backpack sets `points = 1`. |
| Skill | Adds one point if either `Begging.Value > 10` or `Psychology.Value > 10`. Both skills above 10 still add only one point. |
| Outer torso | Adds one point for ItemID `0x1f9f`, `0x1fa0`, `0x4C16`, `0x4C17`, `0x2B6B`, or `0x3162`. |
| Middle torso | Adds one point for the same six torso ItemIDs. |
| Helm | Adds one point for ItemID `0x171C` or `0x4C15`. |
| Shoes | Adds one point for ItemID `0x4C27`. |

The final condition is `points > 2`, not a named guild membership check.

## Bag Of Tricks And Prank Points
`BagOfTricks` is a weightless-display `Item` with item ID `0x1E3F`, name `bag of tricks`, weight `1.0`, and a random color hue. Double-clicking requires the bag to be inside the caller's backpack, then opens `BagOfTricksGump`.

`PrankPoints` are stored directly on each bag. `BagOfTricks.GetPranks(m)` sums all bags in `m.Backpack`, and `BagOfTricks.UsePranks(m, amount)` drains points across those bags until the requested amount is paid.

Gold is converted to prank points by dropping `Gold` onto the bag:

| Rule | Compiled behavior |
| --- | --- |
| Nearby NPC requirement | The bag scans mobiles within `20` tiles and requires at least one `Jester` or `ChucklesJester`. |
| Maximum storage | A bag caps at `50000` prank points. |
| Under cap | If `PrankPoints + dropped.Amount < 50000`, the full gold stack is deleted and the same amount is added as prank points. |
| Over cap | The bag adds only the amount needed to reach `50000` and subtracts that amount from the gold stack. |
| Full bag | No points are added when `PrankPoints >= 50000`. |

`SBJester` vendor stock includes `BagOfTricks`, jester clothing, throwing gloves, throwing weapons, and conditional study-book or circus-tent deed entries.

## Shared Casting Rules
All tricks inherit these rules unless an individual trick adds or overrides checks:

| Rule | Compiled behavior |
| --- | --- |
| Cast skill | `SkillName.Begging`. |
| Damage skill | `SkillName.Psychology`. |
| Required skill | Constant `10`. |
| Cast skill window | `min = 10.0`; `max = 60.0`. |
| Hands | `ClearHandsOnCast = false`. |
| Cast recovery | `2`. |
| Pre-cast prank gate | `CheckCast()` requires total prank points at least `RequiredTithing`. |
| Jester gate | `CheckCast()` and `CheckFizzle()` both require `GetPlayerInfo.isJester(Caster)`. |
| Mana gate | `CheckCast()` and `CheckFizzle()` require `Caster.Mana >= ScaleMana(RequiredMana)`. |
| Prank-point reduction | During `CheckFizzle()`, `LowerRegCost > Utility.Random(100)` changes the spent prank cost to `0`; otherwise the spell spends `RequiredTithing`. The initial `CheckCast()` still requires the full listed prank total. |
| Spend timing | Prank points are drained before `base.CheckFizzle()`. Mana is drained only after `base.CheckFizzle()` succeeds. |
| Mantra | The spell's `SpellInfo.Mantra` is spoken overhead as a regular public message. |
| Fizzle text | Normal fizzle plays a gendered sound and says `*sigh*`; hurt fizzle and disturb paths use `*oops*`. |

### Scaling Formulas
`JesterSpell.Buff(m, category)` uses the caster's current `Begging.Value` and `Psychology.Value`. Each call rolls its own divisor:

| Divisor roll | Compiled behavior |
| --- | --- |
| Best roll | If `Psychology.Value >= Utility.RandomMinMax(1, 400)`, `var = 1.5`. |
| Middle roll | Else if `Psychology.Value >= Utility.RandomMinMax(1, 200)`, `var = 1.8`. |
| Default | Otherwise, `var = 2.0`. |

| Category | Formula before integer truncation |
| --- | --- |
| `time` | `(10 + (Begging / 2) + Psychology) / var` |
| `strength` | `(10 + (Begging / 4) + (Psychology / 2)) / var` |
| `skills` | `(20 + (Begging / 2) + Psychology) / var` |
| `damage` | `(1 + (Begging / 25) + (Psychology / 15)) / var` |
| `poison` | `((Psychology / 25) + 1) / var` |
| `hurts` | `10 + (Begging / 4) + (Psychology / 2)`; no divisor. |
| `range` | `(Psychology / 25) + 1`; no divisor. |

Because the helper rerolls each time it is called, one spell resolution can calculate duration, damage, poison, skill, and strength from different divisor rolls.

## Commands
All Jester commands are registered at `AccessLevel.Player`. Each handler checks `Multis.DesignContext.Check(e.Mobile)`, then checks `GetPlayerInfo.isJester(from)` before constructing and casting the spell. No Jester-specific administrator command was found.

| Usage | Description attribute | Spell ID | Handler action |
| --- | --- | ---: | --- |
| `CanOfSnakes` | `Casts Can Of Nuts` | 260 | `new CanOfSnakes(e.Mobile, null).Cast()` |
| `Clowns` | `Casts Clowns` | 261 | `new Clowns(e.Mobile, null).Cast()` |
| `FlowerPower` | `Casts Flower Power` | 262 | `new FlowerPower(e.Mobile, null).Cast()` |
| `Hilarity` | `Casts Hilarity` | 263 | `new Hilarity(e.Mobile, null).Cast()` |
| `Insult` | `Casts Insult` | 264 | `new Insult(e.Mobile, null).Cast()` |
| `JumpAround` | `Casts Jump Around` | 265 | `new JumpAround(e.Mobile, null).Cast()` |
| `PoppingBalloon` | `Casts Popping Balloon` | 266 | `new PoppingBalloon(e.Mobile, null).Cast()` |
| `RabbitInAHat` | `Casts Rabbit In A Hat` | 267 | `new RabbitInAHat(e.Mobile, null).Cast()` |
| `SeltzerBottle` | `Casts Seltzer Bottle` | 268 | `new SeltzerBottle(e.Mobile, null).Cast()` |
| `SurpriseGift` | `Casts Surprise Gift` | 269 | `new SurpriseGift(e.Mobile, null).Cast()` |

## Trick Reference
| ID | Command | Trick | Delay | Pranks | Base mana | Target | Compiled effect |
| ---: | --- | --- | ---: | ---: | ---: | --- | --- |
| 260 | `[CanOfSnakes` | Can of Snakes | 3.0s | 20 | 40 | Self/location | Requires one free follower slot. Summons at least one controlled `SummonedJoke` snake at the caster with 50 physical and 50 poison damage. Up to three extra snakes are rolled from one Begging check and two Psychology checks against `Utility.RandomMinMax(1, 200)`. |
| 261 | `[Clowns` | Clowns | 3.0s | 15 | 25 | Self/location | Requires one free follower slot, rejects mounted casters, and rejects `HorrificBeastSpell` transformation. Summons a mirror-image `Clown` for `30 + (Psychology.Fixed / 40)` seconds. Up to two extra clowns are rolled from Begging against `1..200` and Psychology against `1..100`. |
| 262 | `[FlowerPower` | Flower Power | 2.0s | 20 | 20 | Harmful Mobile | Deals `1 + (Begging / 5) + (Psychology / 3)` damage as 50 physical and 50 poison. A Begging roll against `Utility.RandomMinMax(50, 300)` can add one `MonsterSplatter` named `poisonous slime` if no splatter is already within 10 tiles of the target. |
| 263 | `[Hilarity` | Hilarity | 2.0s | 20 | 50 | Harmful Mobile | Paralyzes the target for `Psychology + ((Psychology + Begging) / 8) - targetLevel`, minimum `5` seconds. Then it paralyzes additional harmful targets in `Buff("range")` tiles and makes each react. Calls `ChucklesJester.DoJokes(Caster)` after the target pass. |
| 264 | `[Insult` | Insult | 2.0s | 20 | 40 | Harmful Mobile | Starts a timer on the target for `(Buff("time") / 2) + 1` seconds, ticking every `1.5` seconds. Each tick drains `Buff("range") + 1` mana, floors at zero, and shows `BuffIcon.Insult`. |
| 265 | `[JumpAround` | Jump Around | 0.5s | 20 | 20 | Point | Checks overload and travel rules, then teleports the caster to a valid spawnable point within target range. Player casters also teleport pets to the destination. |
| 266 | `[PoppingBalloon` | Popping Balloon | 3.0s | 30 | 20 | Self/location | Requires three free follower slots. Summons a moving `SummonedPrank` balloon with random hue, `ControlSlots = 3`, explosion damage equal to `Buff("hurts")`, radius equal to `Buff("range")`, and 100 physical damage split. |
| 267 | `[RabbitInAHat` | Rabbit in a Hat | 3.0s | 30 | 30 | Self/location | Requires one free follower slot. Summons at least one controlled `SummonedJoke` rabbit with 100 physical damage and no poison. Up to four extra rabbits are rolled from two Begging checks and two Psychology checks against `Utility.RandomMinMax(1, 200)`. |
| 268 | `[SeltzerBottle` | Seltzer Bottle | 2.0s | 20 | 20 | Harmful Mobile | Deals `1 + (Begging / 5) + (Psychology / 3)` damage as 50 physical and 50 cold. A Begging roll against `Utility.RandomMinMax(50, 300)` can add one `MonsterSplatter` named `freezing water` if no splatter is already within 10 tiles of the target. |
| 269 | `[SurpriseGift` | Surprise Gift | 3.0s | 30 | 20 | Self/location | Requires three free follower slots. Summons a stationary `SummonedPrank` present with body `1027..1030`, `ControlSlots = 3`, explosion damage equal to `Buff("hurts")`, radius equal to `Buff("range")`, and damage split 40 physical, 40 fire, 20 energy. |

## Summoned Mobiles
### `SummonedJoke`
`SummonedJoke` is a controlled melee `BaseCreature` with `ControlSlots = 1`, `DeleteCorpseOnDeath = true`, `DeleteOnRelease = true`, no Fame/Karma, `AlwaysAttackable = true`, `BardImmune = true`, and `BleedImmune = true`.

`MakeJoker` calculates:

| Property | Formula |
| --- | --- |
| Lifetime | `Buff("time")` seconds. |
| Strength / Intelligence / Hits / Mana | `Buff("strength")`. |
| Dexterity / Stamina | `Buff("strength") / 2`. |
| Poison level | `Buff("poison")`, unless the caller passed `poisons < 1`, which forces `0`. |
| Combat skills | `Buff("skills")` for Poisoning, Anatomy, MagicResist, Tactics, and FistFighting. |
| Damage max | `Buff("damage")`. |
| Damage min | `max(1, Buff("damage") / 2)`. |
| Resist seed | `min(70, Buff("strength") / 2)`, with cold/fire adjusted downward when the Mobile's damage split includes the opposite element. |

The Mobile is spawned at the caster or within one tile after up to ten `map.CanFit` attempts, then set `Controlled = true`, `ControlMaster = from`, and `ControlOrder = OrderType.Guard`.

### `SummonedPrank`
`SummonedPrank` is a controlled explosive `BaseCreature` with `ControlSlots = 3`, `DeleteCorpseOnDeath = true`, `DeleteOnRelease = true`, no Fame/Karma, `AlwaysAttackable = true`, `BardImmune = true`, `BleedImmune = true`, and `PoisonImmune = Poison.Lethal`.

The constructor stores explosion damage in Strength/RawStr, explosion radius in Dexterity/RawDex, and prank type in Intelligence/RawInt as `10 + type`. A type above zero sets `CantWalk = true`, which makes `SurpriseGift` stationary.

`BlowUp` is called when the prank gives or receives melee damage or before it dies. It targets harmful mobiles in `RawDex` tiles, excludes itself and the control/summon master, deals `RawStr` damage, then deletes the prank. `PoppingBalloon` uses 100 physical damage. `SurpriseGift` uses 40 physical, 40 fire, and 20 energy.

### `Clown`
`Clown` copies the caster's body, hue, sex, name, title, kills, hair, facial hair, skills, and worn item art into generic cloned `Item` instances. It is `Summoned`, follows its `SummonMaster`, is not commandable, is not dispellable, deletes itself when damaged, deletes its corpse on death, and adds/removes a `MirrorImage` clone count for the caster.

## Serialization
| Class | Serialized fields | Load behavior |
| --- | --- | --- |
| `BagOfTricks` | Writes version `1`, then `PrankPoints`. | Reads an `int version`, then always reads `PrankPoints`. |
| `SummonedJoke` | Writes version `0`. | Reads version and schedules deletion after 10 seconds. |
| `SummonedPrank` | Writes version `0`. | Reads version and schedules deletion after 10 seconds. |
| `Clown` | Writes encoded version `0`, then `m_Caster`. | Reads `m_Caster` and calls `MirrorImage.AddClone(m_Caster)`. |

## Known Issues
| Issue | Evidence from trace |
| --- | --- |
| Direct spell construction can null-reference casters without backpacks. | `JesterSpell.CheckCast()` and `CheckFizzle()` call `BagOfTricks.GetPranks(Caster)` before verifying `GetPlayerInfo.isJester(Caster)`, while `GetPranks` directly enumerates `m.Backpack`. |
| Several pooled range enumerables are not freed. | `BagOfTricks.OnDragDrop`, `FlowerPower.Target`, `SeltzerBottle.Target`, `Hilarity.Target`, and `SummonedPrank.BlowUp` all use `GetMobilesInRange` or `GetItemsInRange` in `foreach` form without an explicit `Free()`. |
| `Insult` recasts stack timers inconsistently. | `Insult.Target` overwrites `m_Table[m]` with a new timer without stopping an existing timer, so old timers can keep draining mana and later remove the table entry for a newer effect. |
| The bag info gump displays the wrong mana cost for `Insult`. | `BagOfTricks.JokeInfo` reports `60` mana for the Insult help panel, but `Insult.RequiredMana` returns `40`. |
| `JumpAround.CheckCast()` bypasses the shared Jester pre-cast checks. | The override checks overload/travel only and does not call `base.CheckCast()`, so prank-point, Jester, and mana validation is delayed until `CheckSequence()`. |
| `BagOfTricks` serialization has no version branch. | The class writes version `1`, but `Deserialize` ignores the version value and always reads `PrankPoints`. |
| The compiled identity check is looser than the bag help text implies. | `isJester` returns true at `points > 2`, so a player with a bag and enough qualifying clothing layers can qualify even if neither Begging nor Psychology is above 10. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0096.
- Backlog rows: RB-06711.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Magic/Jester/JesterSpell.cs (CurrentFile)
- Data/Scripts/Magic/Jester/JesterCommandList.cs (CurrentFile)
- Data/Scripts/Magic/Jester/BagOfTricks.cs (CurrentFile)
- Data/Scripts/Magic/Jester/SummonedJoke.cs (CurrentFile)
- Data/Scripts/Magic/Jester/SummonedPrank.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=1; Gump=17; Initialize=1; Timer=4.
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L57 Gump SendGump access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L698 Gump OnResponse access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L712 Gump SendGump access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L720 Gump SendGump access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L724 Gump SendGump access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L728 Gump SendGump access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L732 Gump SendGump access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L770 Gump OnResponse access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L777 Gump SendGump access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L806 Gump OnResponse access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L813 Gump SendGump access=Internal
- Data/Scripts/Magic/Jester/BagOfTricks.cs:L842 Gump OnResponse access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 3.
- Data/Scripts/Magic/Jester/BagOfTricks.cs:Server.Items.BagOfTricks version=1 serialize=L116 deserialize=L123 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Magic/Jester/SummonedJoke.cs:Server.Mobiles.SummonedJoke version=0 serialize=L246 deserialize=L252 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Jester/SummonedPrank.cs:Server.Mobiles.SummonedPrank version=0 serialize=L226 deserialize=L232 alignment=AlignedByCountAndKnownTypes

### Project And Runtime Coverage

- Data/Scripts/Magic/Jester/BagOfTricks.cs=Keep
- Data/Scripts/Magic/Jester/BagOfTricks.cs=Keep
- Data/Scripts/Magic/Jester/JesterCommandList.cs=Keep
- Data/Scripts/Magic/Jester/JesterCommandList.cs=Keep
- Data/Scripts/Magic/Jester/JesterSpell.cs=Keep
- Data/Scripts/Magic/Jester/JesterSpell.cs=Keep
- Data/Scripts/Magic/Jester/SummonedJoke.cs=Keep
- Data/Scripts/Magic/Jester/SummonedJoke.cs=Keep
- Data/Scripts/Magic/Jester/SummonedPrank.cs=Keep
- Data/Scripts/Magic/Jester/SummonedPrank.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
