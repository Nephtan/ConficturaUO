# Combat and builds

[Start here](README.md) · [Systems at a glance](systems-at-a-glance.md) · [Connections](connections-and-balance.md) · [Coverage](coverage-and-evidence.md)

## Reading this report

This is a source review for the game balance discovery work, dated 2026-09-27, against `main` at baseline `6568058a`. No game, configuration, project, or save data was changed. This is a gameplay study separate from the historical audit phase controller.

**The central finding:** power comes from several systems that reinforce one another. More damage can mean faster kills, more health and mana returned by leech, faster item progression, and less time exposed to enemy attacks. Skill breadth matters because several skills add damage even when they are not the weapon's attack skill. Equipment caps therefore describe only individual parts of a much larger calculation.

This supports investigating the user's concern about effort becoming power. It does not prove that every build or encounter is poorly balanced, or that removing lasting progression would improve the game. Current source also contains deliberate balance work: creature profiles, item costs and caps, pet costs, school restrictions, and a narrow AI targeting rollout.

Evidence labels used here:

- **Source confirmed:** inspected current implementation and relevant caller.
- **Indexed:** found and mapped, but individual behavior has not been fully reviewed.
- **Candidate issue:** source behavior deserving a focused check before numerical tuning; no repair is proposed or applied here.
- **Live unknown:** cannot be established from this checkout, such as players' actual equipment or elapsed acquisition time.

### Configuration used for the examples

`Data/Scripts/System/Misc/CurrentExpansion.cs:8-12` selects SA; the AOS, SE, and ML branches are therefore active in this source configuration (`Data/System/Source/Main.cs:269-286`). `Data/TurnBasedCombat/TurnBasedCombat.cfg:2` has `Enabled=false`. Examples describe ordinary realtime combat. The turn bridge remains in the call paths, but an enabled deployment would need its own timing analysis.

The configuration review mapped `Info/settings.xml` to `Settings.Configure`: `PlayerLevelMod=2.0`, spell damage increase ceiling 200 against monsters and 100 against players, `DamageToPets=1.0`, and `CriticalToPets=5`. Relevant consumer methods are `Data/Scripts/System/Misc/Settings.cs:772-825,853-880`. These are checked-in settings, not observation of a running shard.

## 1. The fight at a glance

| Stage | What a player experiences | Main determinants | Main source |
|---|---|---|---|
| Permission | Can I attack or help this target? | Alive/blessed status, regions, consent, ownership, events, government, guilds | `Data/System/Source/Mobile.cs:8894-8930`; `Data/Scripts/System/Misc/Notoriety.cs:62-285` |
| Opportunity | Can I act now? | Swing or cast delay, movement, paralysis, peace, mana, ammunition | `Data/Scripts/Items/Weapons/BaseWeapon.cs:1636-1740,1777-1822`; `Data/Scripts/Items/Weapons/Bows/BaseRanged.cs:77-130`; `Data/Scripts/Magic/Base/Spell.cs:616-816` |
| Contact | Does a weapon hit? | Attacker skill and hit chance versus defender weapon skill and defense chance | `BaseWeapon.cs:1528-1633` |
| Power | How large is the attack before resistance? | Base roll, stats, multiple skills, equipment, abilities, slayer, sneak attack, spell-specific additions | `BaseWeapon.cs:2132-2340,3300-3615`; `Spell.cs:194-243` |
| Avoidance | Does it get blocked or redirected? | Parry, Bushido, shield/weapon choice, evasion, clones | `BaseWeapon.cs:1855-2044,2173-2194`; `Spell.cs:372-416` |
| Mitigation | How much damage gets through? | Damage-type mix, five resistances, armor-ignore/direct exceptions, barding | `Data/Scripts/System/Misc/AOS.cs:148-302` |
| Consequences | What else happens? | Leech, hit spells, debuffs, interruption, bandage slips, fatigue, region hooks | `BaseWeapon.cs:2489-2701`; `PlayerMobile.cs:2860-2895`; `Mobile.cs:5818-5920` |
| Recovery | Can I sustain the fight? | Vitals, regeneration, leech, bandages, spells, forms, resources | `PlayerMobile.cs:1620-1668`; `RegenRates.cs`; `Bandage.cs:451-664` |
| Reward | What permanent power did I earn? | Creature rewards plus experience to equipped levelable items | `BaseCreature.cs:5901-5910`; `LevelItemManager.cs:84-222` |

Short paths above name files under these roots unless the full path is shown: `BaseWeapon.cs` in `Data/Scripts/Items/Weapons`; `Spell.cs` in `Data/Scripts/Magic/Base`; `PlayerMobile.cs` and `BaseCreature.cs` in `Data/Scripts/Mobiles/Base`; `Mobile.cs` in `Data/System/Source`; `RegenRates.cs` in `Data/Scripts/System/Misc`; `Bandage.cs` in `Data/Scripts/Items/Trades/Misc`; `LevelItemManager.cs` in `Data/Scripts/Items/Magical/God`.

### Why small percentage changes can become large practical changes

For a simple sustained weapon comparison, think of:

`damage per second ≈ hit probability × damage that gets through / seconds per swing`

Then ask whether the attacker survives long enough and has enough mana/stamina to maintain it. This is an explanatory model, not the full engine: procs, area attacks, target movement, interrupts, pet damage, and timers add more terms.

At 70 resistance, 30% of ordinary damage of that type gets through. At 90, 10% gets through. Raising a resistance from 70 to 90 cuts that incoming damage to one third; it is not merely a 20% improvement. At 200 HP that corresponds to about 667 versus 2,000 points of raw same-type damage before healing. Physical 90 is a specific Research Rock Flesh exception, not the normal cap.

## 2. Weapons, damage, and defense

### Hit chance

In the active AOS path:

`chance = ((attackSkill + 20) × (100 + attackBonus)) / (2 × (defenseSkill + 20) × (100 + defenseBonus))`

Both final bonuses have an upper clamp of 45. The attack bonus includes equipment, Divine Fury (+10), wolf/fox form (+20), hit-lower-attack (-25), and ability bonuses. Defense includes equipment, Divine Fury (-20), hit-lower-defense (-25), Block, Surprise Attack, and a Discordance lookup. The arithmetic chance has a 2% floor before `attacker.CheckSkill`; it is not an additional final 45% chance ceiling. Source: `BaseWeapon.cs:1528-1633`.

Examples with equal 100 skills, no other modifiers:

| Attack bonus | Defense bonus | Computed hit chance |
|---:|---:|---:|
| 0 | 0 | 50% |
| 45 | 0 | 72.5% |
| 45 | 45 | 50% |
| 0 | 45 | About 34.48% |

The defender normally uses the skill selected by its equipped weapon; animal/monster selection has additional handling (`BaseWeapon.cs:1480-1520`). This is why damage, hit chance, and defense should be measured separately.

### Swing speed

With ML enabled, the shared calculation is:

`ticks = floor((weaponSpeed × 4 - floor(currentStamina/30)) × 100/(100 + speedBonus))`

`seconds = max(5, ticks) × 0.25`

The final speed bonus is capped at 60 and includes relevant buffs/debuffs. Current stamina, not maximum stamina, is used. The floor of 5 ticks means 1.25 seconds per ordinary swing. Source: `BaseWeapon.cs:1636-1701`.

For a weapon whose `Speed` is 3.5:

- 120 current stamina and 0 speed bonus: 2.5 seconds.
- 120 current stamina and 60 speed bonus: 1.5 seconds.
- 180 current stamina and 60 speed bonus: 1.25 seconds.

This makes stamina restoration an offensive benefit. Ranged weapons also require 0.25 seconds standing still under SE unless using Moving Shot, then check ammunition through `OnFired` before resolving the hit (`Data/Scripts/Items/Weapons/Bows/BaseRanged.cs:77-130,273-380`). Custom ammunition and throwing weapons have additional branches; not every projectile is an ordinary arrow.

### Weapon damage has several independent layers

1. Roll the weapon's base damage. A creature's explicit `DamageMin/Max` takes precedence; nonhuman fists have a Strength fallback (`BaseWeapon.cs:3300-3332`).
2. Add stat and skill bonuses. `GetBonus(v,s,t,o) = (v×s + o when v>=t)/100` (`BaseWeapon.cs:3335-3342`).
3. Add item/buff WeaponDamage, whose combined pool is upper-clamped at 100, plus the separate weapon quality/damage-level bonus.
4. Add sneak damage, then apply a separate pool of ability, slayer, Enemy of One, pack instinct, double-strike, honor/perfection bonuses.
5. Invoke creature-specific damage overrides and parry/attachment handling.
6. Select damage types, apply armor-ignore where appropriate, and call `AOS.Damage`.
7. Apply leech, hit effects, other abilities, and durability effects.

Sources: `BaseWeapon.cs:2132-2701,3402-3615`.

#### Stat and skill contributions

Values below are percent damage additions before later multipliers. The threshold bonus starts at 100 in each listed case.

| Input | Addition | Applies to |
|---|---|---|
| Strength | `0.3 × Str`, plus 5 at 100 | Shared calculation |
| Anatomy | `0.5 × skill`, plus 5 | Shared calculation |
| Tactics | `0.625 × skill`, plus 6.25 | Shared calculation |
| Arms Lore | `0.625 × skill`, plus 6.25 | Shared calculation |
| Ninjitsu | `0.625 × skill`, plus 6.25 | Shared calculation |
| Lumberjacking | `0.2 × skill`, plus 10 | Axes |
| Mining | `0.2 × skill`, plus 10 | Bashing |
| Seafaring | `0.2 × skill`, plus 10 | Harpoon variants |
| Bushido | `0.625 × skill`, plus 6.25 | Axe, slashing, polearm |
| Necromancy and Magery | Each `0.625 × skill`, plus 6.25 | Selected staff/wand/scepter families |
| Bowcraft | `0.625 × skill`, plus 6.25 | Wooden ranged weapons |

These contributions have no clamp inside `ScaleDamageAOS`; that does not mean player stats or skills themselves have no limits. The function also calculates an Elementalism bonus but does not include it in `totalBonus` (`BaseWeapon.cs:3527-3532,3569-3615`), a candidate implementation omission.

Example: base roll 20, Strength/Anatomy/Tactics/Arms Lore/Ninjitsu all 100, no type-specific bonus, ordinary quality, no other modifiers:

- Five inputs add 296.25%: `20 + floor(20 × 2.9625) = 79`.
- Add capped +100 WeaponDamage: `20 + floor(20 × 3.9625) = 99`.
- A matching slayer adds +100 in the later pool: 198 before resistance.
- 70 resistance to the entire attack type reduces that to 59 by integer division.

This example describes a successful unblocked hit and excludes all later effects. It illustrates why an item's displayed damage increase is not a complete description of character power.

**The later pool is capped at +300%, which means up to 4 times its input.** The nearby comment says “x3,” but the implementation uses `100 + percentageBonus` after `Math.Min(percentageBonus,300)` (`BaseWeapon.cs:2211-2340`). Sneak attack happens before that multiplication and can add up to 125% melee or 62.5% ranged (`BaseWeapon.cs:2132-2162`).

### Active weapon abilities

`WeaponAbility.cs` registers 55 non-null abilities, including ordinary UO actions and custom stat drains, elemental strikes, protections, multiattacks, and death blows. Weapons can expose five slots. Required skill is the configured starting requirement plus 0/10/20/30/40 for slots one through five, with an ML Tactics requirement (`Data/Scripts/System/Skills/Weapon Abilities/WeaponAbility.cs:54-139,232-290,304-362`). This is a discovery index, not review of all 55 effects.

Mana cost subtracts 5 when the listed combat-skill sum reaches 200, or 10 at 300; applies the 40% lower-mana-cost cap; and doubles when another ability context is still active, described by the code as within three seconds (`WeaponAbility.cs:74-110,172-195`). Thus broader skill training can reduce ability costs as well as improve base damage.

### Parry, resistance, and armor ignore

- Shield parry: `(Parry - nonracial Bushido)/400`, floored at zero, then +0.05 when Parry or Bushido reaches 100. Dexterity below 80 reduces it; Evasion modifies it. This makes shield and weapon-parry builds different.
- Weapon parry uses `(Parry × Bushido)/48000` one-handed or `/41140` two-handed, threshold bonuses, and comparison against the older `Parry/800 + threshold bonus` chance. Bows/fists do not use this weapon-parry route.
- A successful AOS parry zeroes ordinary incoming weapon damage and can trigger Counter Attack or Confidence recovery.
- AOS typed damage is `floor(rawDamage × sum(typePercent × (100-resistance))/10000)`, followed by applicable direct/quiver/barding adjustments. Ordinary positive damage has a minimum of 1 after mitigation.
- The shared ML player-versus-player armor-ignore branch caps this stage at 35, or 70 for the flagged death-strike route. Other later hooks can still alter final damage. This is not a universal cap on every kind of direct damage in the game.

Sources: `BaseWeapon.cs:1855-2044`; `Data/Scripts/System/Misc/AOS.cs:211-302`.

Normal player maximum resistance is 70 (`Data/System/Source/Mobile.cs:1046-1051,1118-1126`). Player exceptions include Curse nonphysical maximum 60, elf energy +5 afterward, and Research Rock Flesh physical maximum 90 (`PlayerMobile.cs:889-909`). Magic Resist also sets a minimum resistance: 40 at skill 100, rising by one per five skill points thereafter (`PlayerMobile.cs:970-988`).

### Sustain from dealing damage

On a successful property proc, weapon life leech returns 30% of damage dealt, stamina leech 100%, mana leech 40%. The property value is the chance, not the amount returned. Curse Weapon adds 50% life leech, Vampiric Embrace adds 20%, and Wraith adds `5 + floor(15×Spiritualism/100)` percent mana leech. Sources: `BaseWeapon.cs:2491-2535`.

Example: with 50 Hit Life Leech and a 100-damage hit, the ordinary property returns 30 HP half the time, averaging 15 per successful hit before other effects. This is why increasing damage can also reduce healing-resource use. Hit effects and area effects are additional surfaces; their actual combined uptime needs build testing.

## 3. Vitals, recovery, and control

### Health, stamina, and mana pools

With the checked-in `PlayerLevelMod=2.0`, ordinary player health is twice current Strength plus applicable bonus health; bonus health from AOS attributes is capped at 25 for ordinary players. Wolf/fox form can add another 20 afterward. Stamina and mana apply the same configured multiplier to their base maxima, then add bonus stamina/mana (`PlayerMobile.cs:1620-1668`; `Settings.cs:853-880`). This setting is a global multiplier despite its name; it is not itself character-level progression.

The shared AOS attribute aggregator adds attributes from equipped weapons, armor, jewelry, race items, instruments, clothing, spellbooks, and quivers. It does not centrally clamp everything; consumers apply different caps (`Data/Scripts/System/Misc/AOS.cs:356-432`). A per-item upgrade cap and a final combat cap are separate rules.

### Regeneration is split into different pools

`Data/Scripts/System/Misc/RegenRates.cs:14-182`:

- **Health:** normally `0.1 × (1+points)` HP/sec. Item/racial points are capped at 18 for players, then Horrific Beast adds 20 and cat/dog forms add Ninjitsu-based points afterward. Creature and paragon additions have separate rules.
- **Stamina:** normally `0.1 × (2+points)` stamina/sec. Focus contributes `floor(Focus×0.1)` outside the item/form pool capped at 24 for players.
- **Mana:** normally `0.1 × (2+floor(totalPoints))` mana/sec. Meditation points depend on Intelligence and three times Meditation, multiplied by 0.025 below Meditation 100 or 0.0275 at/above it. Focus adds `Focus×0.05`. Active meditation adds up to another 13. Any meditation-blocking armor removes the Meditation contribution. The player item/form pool is capped at `PlayerLevelMod(36)=72` in current config.

Examples, without special forms: 18 health points means 1.9 HP/sec. At Intelligence100/Meditation100/Focus100, no blocking armor, no gear regen, the mana contribution is `11+5=16`, giving 1.8 mana/sec. At the 72 item/form pool ceiling that becomes 9 mana/sec. These are theoretical shared-handler rates, not observed uptime in combat.

### Bandages and a representative healing spell

Bandages use Healing/Anatomy on ordinary humanoid/player patients and Veterinary/Druidism on applicable monster/animal patients. Henchmen have their own classification exception (`Bandage.cs:435-449`).

| Action | Source-confirmed rule |
|---|---|
| Normal heal chance | `(primarySkill+10)/100 - slips×0.02` |
| Normal amount | `secondary/2 + primary/2 + random[50,100)`; animal/monster patients also get `HitsMax/100` |
| Slip loss | Each slip subtracts 35% of the pre-slip amount; final minimum 1 |
| Cure | Both skills >=60; chance `(primary-30)/50 - poisonLevel×0.1 - slips×0.02` |
| Resurrection | Both skills >=80, ordinary chance `(primary-68)/50 - slips×0.02`; additional pet/faction/location rules |
| Self heal time | `5 + 0.05×(120-Dex)` seconds in AOS |
| Veterinary time | 2 seconds on another applicable patient |
| Heal another person | `3.2 - 2.5×sin(Dex/130)` below Dex204, otherwise 0.7sec; resurrection adds5sec |

Sources: `Bandage.cs:477-664,745-785`. Bandage resolution cures poison or stops bleeding before normal healing; Mortal Strike prevents ordinary healing at this point. Player damage greater than18 from a player or greater than25 from another source causes a slip (`PlayerMobile.cs:2860-2877`).

At Healing100/Anatomy100 and no slips, a bandage restores 150-199 HP. At Dex120, self use takes5 seconds. Greater Heal at Magery100 restores82-100 HP with current 2.0 multiplier, is blocked by poison/Mortal Strike, and has its own cast/mana/beneficial gates (`Data/Scripts/Magic/Magery/Magery 4th/GreaterHeal.cs:42-72`). Comparing only nominal amounts misses interruption risk, range, cost, and throughput.

### Control changes how much enemy damage happens at all

- Paralyze in `Data/Scripts/Magic/Magery/Magery 5th/Paralyze.cs:59-91` adds `floor(Magery/2)` seconds for a player caster to `floor(Psychology/10 - targetResist/10)`, then triples nonplayer duration. At100/100 versus Resist100, this gives50 seconds on a player or150 seconds on an NPC. Positive damage clears paralysis (`Data/System/Source/Mobile.cs:5835-5851`), so this is separation/positioning/control duration, not guaranteed duration while attacking.
- Discordance applies a negative resistance amount and proportional stat/skill reductions, `-floor(Discordance/5)`, halved at base barding difficulty>=160 (`Data/Scripts/System/Skills/Discordance.cs:225-275`). At100 against an eligible ordinary target, the raw reduction is20 resistance points and20% of affected stats/skills. Weapon damage and speed also query its effect. Eligibility and continued bard proximity matter.
- Peacemaking and Provocation have target/immunity/harmful checks (`Data/Scripts/System/Skills/Peacemaking.cs:103-235`; `Provocation.cs:50,116-180`). Spell schools also add sleep, charm, fear, fields, summons, and debuffs. Those individual duration/stacking rules are not all reviewed here.

## 4. Magic: every registered family, separate from every effect

The adjacent `2026-09-27-gameplay-combat-spells.json` indexes all **346** `Initializer.Register` declarations and maps each to its definition file and class line. It scanned574 `.cs` files under `Data/Scripts/Magic`, including support, scroll, book, gump, and effect files; this is not574 spells. All346 registrations resolved to one class-name match.

**Coverage boundary:** family bases and selected concrete effects below were read. The individual mechanics of all346 registrations were not fully audited. The index explicitly labels each effect as indexed and needing individual balance review. Command-created and item-created spells can also exist outside registry access.

| Family / registrations | Identity and shared inputs | Main gates/resources | Reviewed base |
|---|---|---|---|
| Magery /64, IDs0-63 | Broad spellbook toolkit. Cast Magery, shared damage Psychology. | Circle mana4/6/9/11/14/20/40/50; reagents or arcane charges; scroll changes skill band. | `Data/Scripts/Magic/Magery/MagerySpell.cs:7-45,97-107`; `Spell.cs:70-76` |
| Necromancy /17,100-116 | Transformations, drains, curses, undead. Cast Necromancy, damage Spiritualism. | Individual mana/skill/reagents; negative karma awards; scroll changes fizzle rule. | `Data/Scripts/Magic/Necromancy/NecromancerSpell.cs:7-65` |
| Witch brews /16,131-146 | Necromantic liquids; cast Necromancy, damage Forensics. | Required skill/mana; liquid/scroll lifecycle; may lower karma by50 when above-2459. | `Data/Scripts/Magic/Witch/UndeadSpell.cs:13-62` |
| Druid mixtures /16,147-162 | Nature protection, healing, control, travel/summons. Druidism/Veterinary. | Required skill, per-effect mana and cast delay; hands stay equipped. | `Data/Scripts/Magic/Druidism/HerbalistSpell.cs:7-41,100-113` |
| Knightship /10,200-209 | Martial holy abilities. Knightship for cast/damage. | Karma>=0; tithing; mana paid in CheckFizzle; individual skill roll. | `Data/Scripts/Magic/Knight/PaladinSpell.cs:8-97,136-145` |
| Mystic /10,250-259 | Monk powers. Fist Fighting. | Tithing/mana; monk robe, compatible equipment, base Focus100 and Meditation100, except CreateRobe gate exception. | `Data/Scripts/Magic/Mystic/MysticSpell.cs:9-180` |
| Jester /10,260-269 | Pranks, tricks and unusual combat support. Begging/Psychology. | BagOfTricks prank pool and isJester recognition; skill band10-60; mana. | `Data/Scripts/Magic/Jester/JesterSpell.cs:9-103,145-153`; `Data/Scripts/System/Misc/Players.cs:166-230` |
| Syth /10,270-279 | Dark psychic/sword powers. Psychology/Swords; Tactics/negative karma also feed damage helper. | Karma<=0, maximum of Swords/Psychology meets effect requirement, datacron crystals, mana. | `Data/Scripts/Magic/Syth/SythSpell.cs:360-408,411-528,629-669` |
| Jedi /10,280-289 | Light psychic/sword powers. Psychology/Swords; Tactics/positive karma also feed damage helper. | Karma>=0, maximum of Swords/Psychology meets effect requirement, datacron crystals, mana. | `Data/Scripts/Magic/Jedi/JediSpell.cs:368-416,419-526,627-667` |
| Shinobi /8,290-297 | Additional ninja powers. Ninjitsu. | Individual skill, tithing, mana; speed power checks mount restrictions. | `Data/Scripts/Magic/Shinobi/ShinobiSpell.cs:15-170` |
| Elementalism /32,300-331 | Four element variants across eight circles. Elementalism for cast/damage. | Mana5/7/10/14/19/24/40/50; stamina; armor fizzle; no reagents; school incompatibility. | `Data/Scripts/Magic/Elementalism/ElementalSpell.cs:12-125,1024-1104` |
| Bard songs /16,351-366 | Ongoing support/debuff toolkit. Musicianship; helper sums four bard skills. | Individual required skill/mana; songs implement their own target/timer behavior. | `Data/Scripts/Magic/Bard/SongSpells.cs:59-110` |
| Bushido /6,400-405 | Martial attacks, parry, confidence, evasion, counters. Bushido. | Individual skill/mana; fast-cast scalar0; some registry entries are SpecialMove rather than Spell. | `Data/Scripts/Magic/Bushido/SamuraiSpell.cs:10-125`; registry definitions in JSON |
| Ninjitsu /8,500-507 | Stealth attacks, forms, clones. Ninjitsu. | Individual skill/mana; spell versus SpecialMove distinction matters. | `Data/Scripts/Magic/Ninjitsu/NinjaSpell.cs:10-124`; registry definitions in JSON |
| Research /64,600-663 | Learned/prepared broader magic; helpers draw from multiple magic skills. | Maximum(Magery,Necromancy) meets requirement; prepared spell or ancient tome; tome paper/quill/reagents; mana; no ordinary random base CheckFizzle after gates. | `Data/Scripts/Magic/Research/ResearchSpell.cs:24-94,121-201` |
| Misc item magic /7,700-706 | Item-oriented summon/attack/identify/travel helpers. Focus. | Per-effect mana; base consumes no reagents. Registry rejects these IDs; direct item construction may still work. | `Data/Scripts/Magic/Misc/MagicalSpell.cs:7-45` |
| Death Knight /14,750-763 | Dark knight abilities. Knightship. | Karma<=0; souls in lantern; mana/individual skill. Registry rejects IDs; commands/gump instantiate directly. | `Data/Scripts/Magic/Death Knight/DeathKnightSpell.cs:12-174`; `DeathKnightCommands.cs:98`; gump:689 |
| Holy Man /14,770-783 | Prayers/healing/protection. Spiritualism/Healing. | Karma>=2500; piety in symbol; mana/individual skill. Registry rejects IDs; commands/gump instantiate directly. | `Data/Scripts/Magic/Holy Man/HolyManSpell.cs:12-172`; `HolyManCommands.cs:86,257`; gump:632,668 |
| Enchantment effects /14,800-813 | Item-associated versions of dark powers. | Shared base returns0 mana and a fixed power helper; item access must be traced. Registry rejects IDs. | `Data/Scripts/Magic/Enchantments/EnchantMagic.cs:12-63` |

This table states entry families and resources. It does not assert that every family has equal access cost, equal effect coverage, or equal power. Names alone are not reliable evidence: this Mystic system is a Fist Fighting monk system, and Research is not ordinary Magery with another book.

### Rules shared by many spells

**Circle success:** Magery/Elementalism use zero-based circle `c`, a band from `(100/7)×c -20` to `(100/7)×c +20`; a non-null scroll reduces `c` by2. The actual skill-roll handler still controls check normalization. Do not translate every band to an ordinary UO success table without checking that handler.

**Cast speed:** base spell handling caps fast casting at2 for Magery, Elementalism, Necromancy, and Knightship when Magery>=70, otherwise4. Protection subtracts2 after that clamp. Each point normally removes0.25sec; generic minimum0.25sec. Recovery normally starts at6 quarter-seconds minus Faster Cast Recovery, floored at0; family overrides change the base. Druid and martial overrides also differ. Sources: `Spell.cs:983-1077`; the family bases above.

**Resources:** base mana scaling caps Lower Mana Cost at40 and subtracts it from Mind Rot's scalar, then truncates. Reagent saving checks Lower Reagent Cost against a random0-99 (`Spell.cs:306-334,949-963`). A value100 guarantees this reagent-saving branch where used. Some schools return pre-scaled mana or charge it in CheckFizzle; that makes school-specific payment traces necessary.

**Incompatible schools:** for players, Elementalism>=1 combined with Necromancy>=1 blocks Elemental/Necromancer spells. Elementalism>=1 combined with Magery>=1 blocks Elemental/Magery spells. Elementalism>=1 blocks Research. These are current modified skill values in this check, not a generic class designation (`Spell.cs:892-924`).

**Underworld:** player Magery/Research/Elemental spells have a two-in-three preliminary fizzle chance in the logical Underworld unless an allowed Orb of the Abyss/Codex Wisdom item is found in the tested equipment layers. This occurs in the early cast check before ordinary commitment (`Spell.cs:627-637,823-889`). Other schools are not covered by this particular check.

### Shared spell damage is only part of spell damage

For `GetNewAosDamage`, ignoring integer-rounding steps for readability:

`diceRoll × (1 + inscriptionBonus + floor(Int/10)/100 + cappedSDI/100) × (0.30 + 0.009×damageSkill) × targetScalar`

Inscription bonus is `(fixedInscription + 1000×floor(fixedInscription/1000))/200` percentage points; fixed skill is skill×10. At100 Inscription this is10%. Current SDI limits are200 versus monsters and100 versus players. The settings clamp their limits to25..200, so the old nearby comment about a15% PvP limit is not the configured rule (`Spell.cs:194-243`; `Settings.cs:802-825`).

The target scalar includes creature overrides, spellbook slayer, regional changes, and single-target evasion (`Spell.cs:372-466`). Typed SpellHelper damage calls creature overrides then `AOS.Damage` and leech; an untyped overload directly calls `Mobile.Damage` and does not run the AOS resistance calculation (`SpellHelper.cs:1137-1159,1261-1337`).

Example shared calculation: raw roll40, Inscription100, Intelligence100, damage skill100, no slayer/region/evasion. With SDI0 it returns57 after truncation; with SDI200 it returns153; with PvP-limited SDI100 it returns105. An all-energy hit against70 energy resistance then returns17/45/31 through the basic AOS stage, before later hooks.

Concrete effect differences already found:

- Lightning adds `floor(Magery/5)` **after** `GetNewAosDamage(23,1,4,target)` for player casters, then deals100% energy (`Data/Scripts/Magic/Magery/Magery 4th/Lightning.cs:51-59,94`).
- Elemental Bolt adds `floor(Elementalism/5)` after `GetNewAosDamage(10,1,4,target)` and splits50% physical/50% chosen element (`Data/Scripts/Magic/Elementalism/Sphere 1/Elemental_Bolt.cs:41-82`).
- Research's `DamagingSkill` sums Necromancy, Magery, Spiritualism, Psychology and divides by2; individual Research effects choose whether/how to use it (`ResearchSpell.cs:76-94`).
- Jedi/Syth damage helpers combine Tactics, Swords, and up to125 from signed karma. Their base cast gate uses the **higher** of Swords/Psychology, despite messages saying both. Separate helper `GetJediSkill`/`GetSythSkill` checks both and may be used by access paths, so context matters (`JediSpell.cs:627-667`; `SythSpell.cs:629-669`).

## 5. Equipment growth and temporary enchantment

### Equipment is another character-progression system

Levelable equipment stores its own level, experience, maximum level, and spending points. The sampled `BaseLevelSword` is Blessed and serializes this state; equivalent families cover weapons, armor, shields, clothing, and jewelry (`Data/Scripts/Items/Magical/God/Weapons/Swords/BaseLevelSword.cs:12-52,110-167`). Individual family parity remains to be checked before treating every item identically.

The shared settings are default maximum100, hard maximum100, per-kill experience cap enabled,5 points per gained level, double points when the resulting level is a multiple of10 (`LevelableItemSettings.cs:8-16`; `LevelItemManager.cs:185-222`).

Cumulative experience needed for level `L` is `100×L² -100`: level2 needs300, level10 needs9,900, level100 needs999,900. This follows `ExpTable[L-1] = ExpToLevel(L-1)`; the function's argument is not the resulting displayed level (`LevelItemManager.cs:24-44,174-181`).

Kill experience uses the target's **current** Hits/Stam/Mana plus all base skills, compresses the amount over700 by dividing that excess by3.66667, adds bonuses for mage/breath/poison traits, divides by10, and truncates (`LevelItemManager.cs:84-111`). This is not a fixed XP value on a creature definition or a maximum-health-only formula. At the lethal-damage callback, the old/current resource state still matters.

`BaseCreature.OnDamage` calls `CheckItems` for nonsummoned lethal victims and credits the player attacker or the attacking creature's master. `CheckItems` scans equipped layers0..24; every levelable item receives the calculated award, rather than dividing one award among the equipment (`BaseCreature.cs:5901-5910`; `LevelItemManager.cs:124-134`). Party contribution/loot rights are not used in this particular callback.

### Upgrade budget is a choice, but account for all equipped slots

Examples from `LevelAttributes.cs` and the actual increment/spend handler (`Gumps/ItemExperienceGump.cs:652-675`):

| Property | Points per +1 | Per-item upgrade ceiling |
|---|---:|---:|
| Weapon damage | 5 | 50 |
| Swing speed | 6 | 40 |
| Spell damage | 4 | 25 |
| Lower mana/reagent cost | 5 | 50 each |
| Hit chance / defense chance | 10 /8 | 15 each |
| Health/stamina/mana regeneration | 5 | 5 each |
| Strength/Dexterity/Intelligence | 10 | 10 each |
| Health/stamina/mana bonus | 5 | 20 each |
| Life/stamina/mana leech chance | 3 | 50 each on applicable weapon |
| Armor/clothing/jewelry resistance | 2 | 20 per type |

Sources: `Data/Scripts/Items/Magical/God/LevelAttributes.cs:20-126,149-168,327-404,410-430`. Category and item-type eligibility constrain purchases; this is not an assertion every row is buyable on every item. Some per-item limits exceed the final consumer cap, such as Lower Mana Cost50 versus shared cap40.

**Maintenance:** `RepairItems` restores all equipped levelable armor/weapons/clothing, plus magical staffs/wands, to100/100 durability. It is called in weapon/armor combat paths (`LevelItemManager.cs:138-170`; `BaseWeapon.cs:1978-1979,2030-2031,2577-2579`). This strongly changes the long-term wear/material sink for these items. Ordinary equipment still has other durability paths.

### Temporary enchantment can substitute for permanent item stats

Research Enchant requires a weapon in the caster's backpack and records its prior attributes in a tracking stone. It adds100 WeaponDamage, with duration derived from Research DamagingSkill/2 and a timer ceiling120 minutes (`Data/Scripts/Magic/Research/Spells/Enchanting/ResearchEnchant.cs:73-154`). Because the shared weapon-damage pool caps at100, this can replace much of an equipment investment in that same property; it does not automatically add another uncapped100% to all damage.

Other equipment power channels include artifact fixed properties, runic/crafting materials, sharpening, item enchantment spells, XML attachments, gifts, and monster-balance reward equipment. These are indexed from the tree or related caller seams; this report does not claim full review of every reward or crafting route. The economy/crafting track owns those acquisition costs.

## 6. Taming, followers, pets, and summons

| Mechanic | Source-confirmed behavior | Balance meaning |
|---|---|---|
| Follower slots | 5 default;6 when base Herding/Veterinary/Druidism/Taming all>=60;7 at all>=90;8 at all>=120 | Skill breadth raises simultaneous companion budget |
| First tame | Usually90% of original skills,86% if paralyzed during taming; eligible StatLossAfterTame creatures also get50% stats | Wild numbers are not always pet numbers |
| Tame skill band | Minimum tame requirement +6 for each previous owner, with Druidism widening success band; mastery/previous owner exceptions | Access and reliable ownership are distinct |
| Control | Taming plus higher-of-Taming/Druidism contribution versus creature difficulty, then loyalty penalty | Barely acquiring a pet does not mean reliable commands |
| Bonding | Enabled for nonsummoned pets; feed checks and configured bonding delay; death/resurrection path | Long-lived ownership has maintenance/recovery costs |
| Summons | Temporary, follower-constrained per spell, often dispellable; shared helper can scale stats/duration with Magery | Compare duration, slot efficiency, dispel exposure, and action cost |

Sources: `PlayerMobile.cs:862-886`; `Data/Scripts/System/Skills/Taming.cs:157-184,319-325,602-657`; `BaseCreature.cs:432-450,2743-2806,7336-7352,12141-12186`; `SpellHelper.cs:501-536`. Full tamable creature, stable, henchman, and summon-specific catalogs are remaining work.

### Control chance in plain terms

For ordinary non-mastered pets above MinTameSkill29.1, calculate skill and lore advantages in tenths. The skill term uses Taming; the lore term starts at Taming and uses Druidism if higher. Positive advantages are weighted6. Taming deficits are weighted28 and lore deficits14. Half their sum is added to700. A nonnegative result below200 is raised to200; anything above990 is capped990. Then each missing loyalty point subtracts10. Divide by1,000 for probability (`BaseCreature.cs:2743-2806`).

At Taming100, Druidism100, minimum tame skill100, full loyalty, control chance is70%. With both skills105 it rises to99% after the cap. Falling below the requirement is penalized more strongly than going above it is rewarded. Summons, staff, mastery, and low-minimum creatures have special paths.

### Pet damage risks differ from player damage risks

Wild nonsummoned/uncontrolled creatures attacking a recognized pet use `DamageToPets` when above1 and can double damage under `CriticalToPets`. The checked-in values mean no general multiplier and a5% double-damage check. There is also a separate20% controlled-pet scalar check whose base scalar is1.0, with possible subclass overrides (`BaseCreature.cs:809-811,2851-2889`). Raising ordinary monster damage therefore also changes pet burst deaths, which cannot be inferred from player-only testing.

## 7. PvP consent, crime, helping, and regions

The consent state is persistent (`PlayerMobile.cs:54-61,318-325,4674,5084`). It has Null, NONPK, NONPKinEvent, and PK states. The choice gump sets states; an event gate toggles NONPK/NONPKinEvent (`Data/Scripts/Custom/PvPConsent/Gumps/PKNONPKGUMP.cs:44-71`; `NONPKEventMoongate.cs:103-109`).

### Permission is an ordered decision tree

Core attack permission first rejects invalid/deleted/dead/blessed targets and delegates to region permission. The default region chain reaches the registered harmful handler. The handler itself is ordered (`Data/Scripts/System/Misc/Notoriety.cs:136-285`):

1. Valid same map required.
2. Government `CanAttackBanned` can allow immediately.
3. Resolve pet/summon owners and player consent states.
4. Staff exceptions.
5. XML opponents can allow immediately.
6. Bard-provoke special case.
7. NONPK players may attack NPCs not owned by players; ordinary uncontrolled NPCs can attack players/pets.
8. NONPK protection and owner/pet restrictions.
9. Government and guild conditions.
10. Default allow, including PK versus Null.

Do not summarize this as “PvE can never be harmed,” “both players must opt in,” or “a gray/red name determines attack permission.” Display notoriety and harmful permission are separate implementations. Player-v-player NONPK display returns Invulnerable; murder display uses `Kills>=1` in this implementation (`Notoriety.cs:532-561`). Region overrides can further deny actions.

Beneficial handling has its own rules: same map, staff, NPC allowance, XML team restriction, NONPK-player helping PK-player denial, government bans/wars/allies, guild checks (`Notoriety.cs:62-133`). Healing access is part of PvP balance; a support character's consent/government status can matter as much as the spell amount.

**Candidate asymmetry:** the branch labeled as preventing NONPK players or pets from initiating PvP additionally requires `pmAttacker != null`, so it does not directly reject a NONPK owner's pet against a PK target. Later branches protect NONPK targets but do not obviously mirror the owner's initiating restriction. Bard owner resolution also only occurs for controlled/summoned attackers. This is a source-level concern, not proof that every pet/provocation command can exploit it; targeted AI/order/region route tests are needed.

**Caller boundary:** `AOS.Damage` and `Mobile.Damage` are damage application functions, not universal consent enforcement. Normal weapons use `HarmfulCheck`; normal targeted spells use sequence/target checks. Custom effects that call raw damage must be reviewed at their own caller. This is why a central consent file alone is insufficient for a full PvP audit.

## 8. Monsters, scaling, and AI

### A creature definition is only the starting point

The runtime creature may be altered by:

- its constructor and equipped weapon;
- exact-type balance profile calls;
- spawn-region difficulty and quest overrides;
- paragon conversion;
- configured extra HP;
- taming conversion or summon scaling;
- custom `AlterMeleeDamage*`, `AlterSpellDamage*`, breath/poison/timer abilities;
- AI selection, movement cadence, and target legality;
- live spawner properties and saved state.

`BaseCreature.ChangeAIType` honors `ForcedAI` before the ordinary melee/animal/berserk/archer/healer/vendor/mage/predator/thief switch (`BaseCreature.cs:7415-7457`). `Data/Scripts/Mobiles/Base/Behavior.cs` holds the stock AI implementations; `Custom/ThirdParty/OmniAI` supplies another family. A mob's name or base class is therefore not a full difficulty rating.

### Regional scaling multiplies several aspects at once

`OnAfterSpawn` gets world difficulty, applies named exceptions, excludes people/vendors/control/summon from Heat, then calls `BeefUp`, followed by `ExtraHP` (`BaseCreature.cs:3093-3134,5479-5494`). `BeefUp` keeps the original difficulty for an additional HP increase even when Fame limits its other buffs:

- `up` capped by Fame:>=20,000 becomes0;>=18,000 max1;>=15,000 max2;>=10,000 max3.
- For eligible nonparagons: HP seed/Strength +10% per effective up; Intelligence/Dexterity/skills +30% per up; base damage min/max +up; Fame/Karma/gold +10% per up; taming requirement +15% per up.
- Additional HP uses the **original** difficulty and adds10% per level when difficulty>1.
- `ExtraHP` then applies the global HPModifier to noncitizens.

Sources: `BaseCreature.cs:2932-3066`. Example for ordinary eligible low-Fame creature with original/effective difficulty3 and explicit100 HP seed: the first HP stage gives130, the additional stage169, before `ExtraHP` and any other overrides. Skill values increase90%, which can also increase hit rate and damage through shared combat formulas. Difficulty is not just a health multiplier.

Paragon source defines HP seed×5, Strength×1.05, Intelligence/Dexterity/skills×1.2, faster active/passive cadence by dividing by1.2, base damage+5, and Fame/Karma×1.4 (`Data/Scripts/Mobiles/Base/Paragon.cs:11-74`). Conversion eligibility is separate; not every monster receives it. Regeneration also grants paragons additional points.

### Balance work already exists, with bounded scope

`Data/Scripts/Custom/PvE/MobileBalance/MobileBalanceCatalog.cs:34-317` applies exact-type damage and skill profiles; `319-502` gives defined reward drops; item factory/configuration follows. Concrete mobiles such as `Custom/Mobiles/Undead/WolfgangsDragon.cs:31,42,58` call these routines at construction, loot, and load paths. This proves explicit balancing mechanisms exist; it does not prove all entries or the whole game have a balanced outcome.

`Data/Scripts/Custom/Combat/AIOverhaul/AITacticalTargeting.cs:24-64` limits the current tactical profile rollout to five exact types: HeadlessOne, Ratman, Lizardman (Bruiser), RatmanArcher and LizardmanArcher (Skirmisher), only when uncontrolled, nonsummoned, stock melee/archer, FightMode.Closest. It is not a global AI upgrade.

Skirmishers favor casting/low-health targets with a bounded bonus and seek a3..6-tile band constrained by weapon range. Bruisers favor close targets. The shared tactical score addition caps at4 (`AITacticalTargeting.cs:66-173`). `AI_HANDOFF.md` describes broader future phases, but source whitelist evidence is the current behavior boundary.

## 9. Issues to resolve before trusting tuning numbers

These are source-backed review candidates, not approved fixes or live exploit findings.

| ID | Observation | Why it matters | Evidence / focused next check |
|---|---|---|---|
| COM-01 | Registry capacity700 silently rejects49 declared spell IDs>=700 | Access routes can disagree; spells still reachable by direct constructors | `SpellRegistry.cs:14,74-77,117-124`; `Initializer.cs:332-405`; test registry and direct command/book paths separately |
| COM-02 | Elemental stamina reduction uses integer `(100-LRC)/100` | Any positive LRC1..100 gives0 stamina cost; balance cost is not the apparent percentage curve | `ElementalSpell.cs:103-125`; test LRC0/1/50/100 with exact mana/stamina deltas |
| COM-03 | Some GetMana overrides already ScaleMana, then shared CheckSequence scales again | Research/Holy/Death/Jedi/Syth can receive repeated LMC reduction; use payment-path-specific budgets | Relevant family GetMana; `Spell.cs:1095-1144`; test RequiredMana100 at LMC0/40: ordinary100/60, repeated100/36 before other modifiers |
| COM-04 | Shinobi CheckFizzle deducts scaled mana while GetMana also returns scaled mana to shared deduction | Successful CheckSequence path charges two pieces; at LMC0 this is twice RequiredMana, at40 about0.96× before truncation | `ShinobiSpell.cs:87,123-126,152-154`; `Spell.cs:1097,1139-1144`; e.g. TigerStrength uses CheckSequence at65 |
| COM-05 | Weapon damage multiplier comment says x3 while code allows +300%=x4 | Documentation-based balancing would understate ceiling | `BaseWeapon.cs:2211-2215,2336-2340`; use code calculation |
| COM-06 | Elementalism melee bonus calculated but absent from total | A displayed/intended skill synergy may not exist | `BaseWeapon.cs:3527-3532,3600-3615`; fixture staff damage varying only Elementalism |
| COM-07 | Defender defense-chance block queries Discordance on attacker | Debuff target appears inconsistent with comment | `BaseWeapon.cs:1600-1604`; compare identical attack with each side separately discorded |
| COM-08 | Consent branches have pet/provocation/early-government exceptions | PvP and support testing must include owners and regions, not just two players | `Notoriety.cs:148-285`; explicit owner-state/event/government matrix |
| COM-09 | Spell-specific flat damage/control bonuses sit outside generic helper | A single global SDI change will not scale all spells equally | Lightning, Elemental Bolt, Paralyze examples above; inventory every concrete effect |
| COM-10 | Equipment XP uses current resources and lethal callback; every worn item receives XP | “XP per creature” and group leveling rate differ from a simple encounter table | `BaseCreature.cs:5901-5910`; `LevelItemManager.cs:84-134`; test target resource states, exact-lethal versus overkill, pet owner and party cases |

The core supplies `willKill` as `newHits < 0`, not `<=0` (`Mobile.cs:5920`). That makes the exact-lethal case in COM-10 worth testing before assuming every death grants this particular gear award. No code was changed to investigate it.

## 10. A balance study that preserves effort, mastery, and discovery

The source supports three distinct things players can earn:

| Earned value | Examples here | What to measure before changing it |
|---|---|---|
| Lasting power | Skill breadth, levelable gear, better companion access, higher follower budget | Power gained per active hour; time to useful thresholds; strength gap at matched skill execution |
| Mastery | Positioning, damage-type choice, control timing, resource use, pet orders, spell selection | Success against equal-power opponents; avoidable damage; resource waste; effect of player decisions |
| Discovery | Finding spells, slayers, useful regions, rare reward gear, alternate schools | Whether knowledge opens viable alternatives; whether discovery only grants another mandatory numerical upgrade |

A practical next study should compare actual saved builds and a representative encounter set using several measures together:

1. Time to kill, expected damage per second, burst window, and effective durability.
2. Health/mana/stamina restored per damage dealt; resource exhaustion time; potion/bandage/reagent/ammunition expense.
3. Control uptime, immunity coverage, interrupts, mobility, and how much enemy behavior is bypassed.
4. Pet/summon contribution per follower slot, reliability, replacement/recovery cost, and owner actions required.
5. Gear XP and other permanent rewards per active minute; party and pet credit paths.
6. Distinct viability of early, established, and heavily invested builds, with each tested at comparable execution quality.
7. Exact rules for PvE, consensual PvP, events, government conflict, and assistance.

These measurements let later decisions change runaway interactions or ineffective choices while retaining satisfying long-term achievements. This report does not choose new caps, prices, drop rates, or a new progression philosophy.

### Remaining coverage and evidence needed

- Individual damage, duration, target rules, area scaling, cost, cooldown, stacking, counters, and access for all346 registered magic entries and55 weapon abilities. The JSON is the starting checklist, not a completed effect audit.
- Every creature's constructor, spawn overrides, special timers, resistances, immunity, reward, and actual spawn distribution. Shared BaseCreature review is not a review of every monster.
- Every artifact, levelable family, fixed/custom reward, random-affix generator, XML attachment, sharpening/enchantment, and stacking interaction.
- Every pet/summon/henchman family's slots, scaling, AI, transfer/bonding/death rules, and stable infrastructure.
- Full harmful/beneficial caller audit for custom damage, fields, poison, retaliation, proc, area, pet, bard, government, duel/event, and region routes.
- Live build/gear distribution, historical item versions, saved modifiers, active world placements, real acquisition effort, economic demand, player skill and automation patterns.
- Controlled runtime measurements. All numerical examples here are calculations from current source, not gameplay telemetry or proof of deployment behavior.

## Review record

Published from the [dated source review](../codebase-audit/outputs/subagent-findings/2026-09-27-gameplay-combat.md). The [verification record](VERIFICATION.md) applies to the complete documentation package. Regenerate this chapter with `python docs/game-balance/tools/publish_chapters.py` after changing its review record.
