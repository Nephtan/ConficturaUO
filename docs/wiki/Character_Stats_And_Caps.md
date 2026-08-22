# Character Stats And Caps

## Overview

Your paperdoll has two different character-information views:

- **Status** is the standard client status window. It shows your current and maximum vital resources, effective attributes, carried weight and gold, followers, defenses, luck, weapon damage, and tithing points.
- **Info** is Confictura's expanded character-information window. It shows how your attributes split into raw and bonus values, plus progression, reputation, survival, equipment attributes, timing values, bank gold, and other combat details. Clicking **Info** on the paperdoll or entering `[status` opens this same window.

This page covers every value supplied to those two views. The separate **Skills** menu and its individual skill caps are outside this page; see [Skill Scripts](Skill_Scripts.md) for that system.

Unless a row says otherwise, a percentage is a percentage-point equipment total. The number shown by a window may be a display clamp rather than the limit used by every spell or combat calculation.

## How To Read The Limits

| Limit type | Meaning |
| --- | --- |
| Hard cap | The value is clamped before the window receives or displays it. |
| Standard cap | Normal player progression or the normal player-facing route stops here, although staff tools or special systems may set a different value. |
| Display clamp | This window stops counting above the listed number. It does not automatically prove that every gameplay calculation uses the same cap. |
| Formula-derived | The limit depends on attributes, race, equipment, configuration, or a temporary effect instead of one fixed number. |
| No explicit cap | The display source does not impose a separate gameplay maximum. Storage limits and situational rules may still apply. |

## Caps At A Glance

| Value | Limit shown by checked-in source | Type and important exceptions |
| --- | --- | --- |
| Total raw attributes | `250` normally; `300` after Ether progression | Standard cap. This is the combined raw Strength, Dexterity, and Intelligence budget. |
| One trainable raw attribute | `150`; `175` when total Stat Cap is above `250` | Stat-gain cap. The three raw attributes must still fit under the total Stat Cap. |
| Effective Strength, Dexterity, or Intelligence | `150` | Hard cap for ordinary players under the shard's ML rules, even when raw values or bonuses would be higher. |
| Maximum Hits, Stamina, and Mana | Attribute multiplied by the configured player-level modifier, then applicable bonuses | Formula-derived. The checked-in configuration uses `2.0`; deployed configuration can differ between `0.5` and `2.0`. |
| Item Bonus Hits | `25` for ordinary players | Hard cap in maximum-Hits calculation. Mystical Fox and Grey Wolf forms add another `20` Hits. |
| Physical, Fire, Cold, Poison, and Energy resistance | `70` normally | Hard cap with effect/race exceptions: Curse lowers non-physical caps to `60`; an Elf then receives `+5` Energy cap; Research Rock Flesh sets Physical to `90`. |
| Maximum weight | Human: `100 + floor(3.5 x Strength)`; others: `40 + floor(3.5 x Strength)` | Formula-derived. Four stones above the displayed maximum are tolerated before overload fatigue. |
| Follower slots | `5` normally; up to `8` | Formula-derived from four animal skills; see [Followers](#followers). |
| Character Level | `1` to `100` | Hard cap. See [Character Level Recon Report](Character_Level_Recon_Report.md) for the complete progression calculation. |
| Fame | `0` to `15,000` | Standard award range. |
| Karma | `-15,000` to `15,000` | Standard award range. |
| Hunger and Thirst | `0` to `20` during normal play | Standard scale; `20` is full. |
| Potion Enhance | `80%` | Hard calculation cap: up to `50%` from equipment plus up to `30%` from Alchemy. |
| Tithing points | `100,000` through shrine donation | Standard player-facing donation cap. |
| Resurrection cost | Base cost `0` to `2,000`; final display up to `4,000` for Fugitives or `6,000` for Alien-origin characters | Formula-derived. The origin multiplier is applied after the base cost is capped. |
| Low Reagent / Low Mana | `100%` / `40%` | Info display clamps. |
| Hit Chance / Defend Chance | `45%` / `45%` | Info display clamps. |
| Swing Speed + / Damage Increase / Reflect Damage | `100%` each | Info display clamps. |
| Fast Cast / Cast Recovery | `4` / `4` | Info display clamps, not a promise that every spell family accepts the same effective Fast Cast. |
| Bandage Speed | No faster than `5.0` seconds in Info | Formula-derived display floor. |
| Spell Damage + | Configured monster cap; `200%` in the checked-in configuration | Info display clamp. The checked-in player-target cap is separately configured at `100%` and is not the value used by Info. |
| Luck, carried gold, bank gold, murders, regeneration, weapon damage, and absorption pools | No explicit cap in these displays | Situational or uncapped by the window source. Alien-origin characters are a Luck exception and always display `0`. |

## Status Window

The Status button opens the standard client window. Expansion and client support can affect its layout, but the server supplies the following fields for your own character.

| Field | What it means | Cap or rule |
| --- | --- | --- |
| Name | Your character name. | Identity field, not a stat. |
| Sex and race indicators | Identity data used by clients that support the extended status packet. | Client-dependent; not capped stats. |
| Hits | Current Hits and `HitsMax`. | Current Hits cannot remain above the calculated maximum. See [Maximum resources](#maximum-resources). |
| Strength | Effective Strength after current modifiers. | Displayed effective value is capped at `150` for ordinary players. |
| Dexterity | Effective Dexterity after current modifiers. | Displayed effective value is capped at `150` for ordinary players. |
| Intelligence | Effective Intelligence after current modifiers. | Displayed effective value is capped at `150` for ordinary players. |
| Stamina | Current Stamina and `StamMax`. | Maximum is formula-derived from Dexterity, configuration, and Bonus Stamina. |
| Mana | Current Mana and `ManaMax`. | Maximum is formula-derived from Intelligence, configuration, and Bonus Mana. |
| Gold | Gold carried in your character's inventory tree. | No explicit display cap. This is not Bank Gold. |
| Physical resistance / Armor | Physical resistance under the active AOS rules; the legacy non-AOS packet branch sends Armor Rating instead. | Physical resistance is normally capped at `70`; see [Resistances](#resistances). |
| Weight | Current weight and, for supported clients, maximum weight. Current weight includes the character's `14`-stone body weight plus carried/equipped item weight. | Maximum is race- and Strength-derived. |
| Stat Cap | Maximum combined raw Strength, Dexterity, and Intelligence. | `250` normally or `300` with the Ether exception. |
| Followers | Slots currently occupied and maximum follower slots. | Maximum is normally `5` and can rise to `8`. |
| Fire resistance | Final Fire resistance after base values, modifiers, and equipment. | Normally capped at `70`; Curse can lower the cap to `60`. |
| Cold resistance | Final Cold resistance after base values, modifiers, and equipment. | Normally capped at `70`; Curse can lower the cap to `60`. |
| Poison resistance | Final Poison resistance after base values, modifiers, and equipment. | Normally capped at `70`; Curse can lower the cap to `60`. |
| Energy resistance | Final Energy resistance after base values, modifiers, and equipment. | Normally `70`; Elves normally reach `75`, or `65` while Curse has lowered the base cap. |
| Luck | Total Luck from active equipment. | No general display cap. Alien-origin characters display `0` regardless of equipment Luck. |
| Damage | Minimum and maximum status damage reported by the currently equipped weapon. | Weapon- and character-derived; no universal display cap. An unavailable weapon result is sent as `0` to `0`. |
| Tithing points | Current points available to systems that spend tithe. | Shrine donation stops at `100,000`. |

## Info Window

The Info window's header shows your name and either your custom title or current skill title. It also marks you **Innocent** or **Guilty** according to the shard's wanted-status check. The button beside that indicator opens the wanted information view.

### Attributes, Reputation, And Resources

| Info label | What the displayed number means | Cap or rule |
| --- | --- | --- |
| Strength | `Raw Strength + (effective Strength - raw Strength)`. | The second number is the current net modifier. Effective total is capped at `150` for ordinary players. |
| Dexterity | `Raw Dexterity + (effective Dexterity - raw Dexterity)`. | Same raw-plus-net-modifier format and `150` effective cap. |
| Intelligence | `Raw Intelligence + (effective Intelligence - raw Intelligence)`. | Same raw-plus-net-modifier format and `150` effective cap. |
| Fame | Exact Fame total. | Normal awards clamp to `0..15,000`. |
| Karma | Exact Karma total; negative values are evil and positive values are good. | Normal awards clamp to `-15,000..15,000`. |
| Tithe | Current tithing points. | Shrine donation stops at `100,000`. |
| Hunger | Current fullness. | Normal play uses `0..20`; `20` is full. |
| Thirst | Current hydration. | Normal play uses `0..20`; `20` is full. |
| Potion Enhance | Effective potion enhancement from equipment plus the Alchemy contribution. | `80%` maximum: `50%` equipment plus `30%` skill contribution. |
| Bank Gold | Spendable balance calculated by the Banker system. | No explicit Info display cap. It is separate from carried Gold in Status. |

### Progression And Recovery

| Info label | What the displayed number means | Cap or rule |
| --- | --- | --- |
| Level | Overall Character Level derived from the character's strongest progression signals. | `1..100`. |
| Hits | **Current** Hits shown as `(current Hits - item Bonus Hits) + item Bonus Hits`. | This is not `HitsMax`; use Status for current/maximum Hits. |
| Stamina | **Current** Stamina shown as `(current Stamina - item Bonus Stamina) + item Bonus Stamina`. | This is not `StamMax`. |
| Mana | **Current** Mana shown as `(current Mana - item Bonus Mana) + item Bonus Mana`. | This is not `ManaMax`. |
| Hits Regen | Total equipped `RegenHits` attribute. | Info imposes no separate display cap. |
| Stamina Regen | Total equipped `RegenStam` attribute. | Info imposes no separate display cap. |
| Mana Regen | Total equipped `RegenMana` attribute. | Info imposes no separate display cap. |
| Low Reagent | Lower Reagent Cost from equipment. | Displayed at no more than `100%`. |
| Low Mana | Lower Mana Cost from equipment. | Displayed at no more than `40%`. |
| Resurrect Cost | Gold cost produced by the shard resurrection formula. | Base `0..2,000`; final origin-adjusted result can be `4,000` for a Fugitive or `6,000` for an Alien-origin character. |
| Murders | Long-term murder count (`Kills`). | Cannot be set below `0`; no explicit upper display cap. |

### Combat And Casting

| Info label | What the displayed number means | Info limit |
| --- | --- | --- |
| Hit Chance | Total equipped Hit Chance Increase. | Clipped at `45%` for this display. |
| Defend Chance | Total equipped Defense Chance Increase. | Clipped at `45%` for this display. |
| Swing Speed | Current delay returned by the equipped weapon after its delay calculation. | Shown in seconds and clipped at `100` seconds as a defensive display maximum. |
| Swing Speed + | Total equipped Swing Speed Increase. | Clipped at `100%` for this display. |
| Bandage Speed | `5.0 + 0.5 x ((120 - Dexterity) / 10)` seconds. | Never shown faster than `5.0` seconds. This is the Info formula, not a guarantee that every healing action ignores its own rules. |
| Damage Increase | Total equipped Weapon Damage Increase. | Clipped at `100%` for this display. |
| Reflect Damage | Total equipped Reflect Physical Damage. | Clipped at `100%` for this display. |
| Fast Cast | Total equipped Cast Speed. | Clipped at `4` for this display. Individual spell families can apply stricter effective rules. |
| Cast Recovery | Total equipped Cast Recovery. | Clipped at `4` for this display. |
| Spell Damage + | Total equipped Spell Damage Increase. | Clipped to the configured monster-target limit. The checked-in setting is `200%`; player-target spell damage separately uses a checked-in `100%` setting. |
| Magic/Melee Absorb | Current `MagicDamageAbsorb` and `MeleeDamageAbsorb` pools, shown as `magic/melee`. | No fixed Info display cap; these are situational pools. |

## Formulas And Exceptions

### Raw, Effective, And Total Attributes

- **Raw Strength, Dexterity, and Intelligence** are the trainable base values counted against Stat Cap.
- **Effective attributes** include current stat modifiers and are the values shown in Status. Info shows each as `raw + net modifier`.
- Normal stat gain stops when the raw total reaches Stat Cap. It also stops one raw attribute at `150`, or `175` when Stat Cap is above `250`.
- For ordinary players under the current expansion rules, the effective getter for each attribute returns no more than `150`. The higher raw training ceiling therefore does not create an effective Status value above `150` by itself.

### Maximum Resources

The checked-in `Info/settings.xml` sets the player-level modifier to `2.0`. The server constrains that setting to `0.5..2.0`, so a deployed shard can intentionally use another value.

- `HitsMax = configured multiplier x effective Strength + Bonus Hits`.
- `StamMax = configured multiplier x base Stamina maximum + Bonus Stamina`.
- `ManaMax = configured multiplier x base Mana maximum + Bonus Mana`.
- Ordinary players receive at most `25` item Bonus Hits in `HitsMax`. Mystical Fox and Grey Wolf transformations add another `20` Hits.
- These maximum formulas do not make Info's Hits, Stamina, and Mana rows maximums; those three Info rows decompose the **current** resource value.

### Carrying Capacity

- ML Human: `MaxWeight = 100 + floor(3.5 x effective Strength)`.
- Other races: `MaxWeight = 40 + floor(3.5 x effective Strength)`.
- Status current weight is `14 + carried/equipped item weight` because the packet includes body weight.
- The movement system allows four stones above MaxWeight before treating the character as overloaded.

### Followers

Follower slots depend on the **base** values of Herding, Veterinary, Druidism, and Taming. All four must meet the same tier:

| All four skill values | Maximum followers |
| --- | --- |
| Below `60.0` | `5` |
| At least `60.0` | `6` |
| At least `90.0` | `7` |
| At least `120.0` | `8` |

### Resistances

The server adds base resistance, active resistance modifiers, and equipment resistance, then clamps the result to the character's current minimum and maximum.

- The normal player maximum is `70` for all five resistances.
- Curse reduces the Fire, Cold, Poison, and Energy caps to `60` while active.
- Elves receive `+5` Energy maximum after that adjustment: normally `75`, or `65` while cursed.
- Research Rock Flesh sets the Physical maximum to `90` while active.
- Magic Resist can establish a minimum resistance floor; it does not raise these normal maximums.

### Reputation, Survival, And Resurrection

- Normal Fame awards stay between `0` and `15,000`; normal Karma awards stay between `-15,000` and `15,000`.
- Food and drink routes normally clamp Hunger and Thirst at `20`. Both values begin at `20` on a new character and are restored to `20` through standard resurrection handling.

The resurrection cost uses integer arithmetic in this order:

1. Fame contributes up to `15,000`.
2. Inverse Karma contributes `-Karma`, capped only when that contribution is above `15,000`. Good Karma can therefore reduce the result.
3. Total skills, stored in tenths, are capped at `10,000` (`1,000.0` displayed skill points) and multiplied by `1.5`, for up to `15,000`.
4. Raw Strength + Dexterity + Intelligence is capped at `250` for this formula and multiplied by `60`, for up to `15,000`.
5. The four contributions are divided by `600`; `10` is subtracted; the result is multiplied by `1.12` and truncated. That tier is clamped to `1..100`, then multiplied by `20`, producing a base cost of `20..2,000`.
6. The base cost becomes `0` while total skills are `200.0` or lower, or while the raw attribute total is `90` or lower.
7. After the base cap, a Fugitive pays double and an Alien-origin character pays triple. Those final displayed ceilings are therefore `4,000` and `6,000`.

## Configuration Note

The checked-in configuration currently sets:

- player resource multiplier: `2.0`;
- Spell Damage Increase against monsters: `200%`;
- Spell Damage Increase against players: `100%`.

Info clips **Spell Damage +** using the monster setting, not the player setting. The settings code constrains either spell-damage setting to `25..200`. A deployed server can load different values from its own `Info/settings.xml`, so these checked-in numbers should not be treated as a live-server measurement without deployment verification.

## Technical Source Trace

| Behavior | Source |
| --- | --- |
| Standard Status packet fields | `Data/System/Source/Network/Packets.cs` — `MobileStatus` |
| Status request path | `Data/System/Source/Mobile.cs` — `OnStatsQuery` |
| Info command, fields, formulas, display clamps, wanted indicator, and paperdoll Info handler | `Data/Scripts/System/Misc/Players.cs` — `StatsGump`, `GetPlayerInfo`, and `QuestButton` |
| Player resource maximums, effective attribute cap, follower tiers, weight, resistance exceptions, and Luck exception | `Data/Scripts/Mobiles/Base/PlayerMobile.cs` |
| New-character Stat Cap and starting Hunger/Thirst | `Data/Scripts/System/Misc/CharacterCreation.cs` |
| Raw stat total and individual stat-gain caps | `Data/Scripts/System/Skills/SkillCheck.cs` — `CanRaise` |
| Ether Stat Cap exception | `Data/Scripts/Quests/Pagan/ApproachObsidian.cs` and `PlayerMobile.SkillVerification` |
| Overall Character Level range | `Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs` |
| Resource and spell-damage configuration | `Data/Scripts/System/Misc/Settings.cs` and `Info/settings.xml` |
| Fame and Karma award ranges | `Data/Scripts/System/Misc/Titles.cs` |
| Potion enhancement calculation | `Data/Scripts/Items/Potions/BasePotion.cs` — `EnhancePotions` |
| Tithing donation limit | `Data/Scripts/System/Gumps/TithingGump.cs` |
| Overload allowance | `Data/Scripts/System/Misc/WeightOverloading.cs` |

## Related Pages

- [Character Level Recon Report](Character_Level_Recon_Report.md)
- [Skill Scripts](Skill_Scripts.md)
- [Guide To Adventure Book](Guide_To_Adventure_Book.md)
- [Creature Transformation Guide](Creature_Transformation_Guide.md)
