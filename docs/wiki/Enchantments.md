# Enchantments

## Overview
Enchantments is a compiled `Server.Spells.Enchantments` spell namespace with a shared `EnchantMagic` base class, fourteen `Spell` subclasses, and one summoned `BaseCreature` named `Devil_Pact`.

The system is not wired like the other player magic schools. It has no dedicated `Spellbook`, `Gump`, `SpellScroll`, acquisition path, or player command wrappers found in the code trace. `Initializer` attempts to register spell IDs `800` through `813`, but the shared `SpellRegistry` array only has 700 entries, so numeric spell registration and numeric casting for these IDs fail before storing the spell types.

The practical access path found in source is string-based staff or XMLSpawner casting. The generic counselor command `[Cast <name>]` can instantiate spells by class name through `SpellRegistry.NewSpell(string, Mobile, Item)` because the registry's namespace search list includes `Enchantments`.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `EnchantMagic` | `Spell` subclass | Shared casting rules for this spell family. |
| `BanishMagic` through `WrathMagic` | `Spell` subclasses | Fourteen targeted, self, or point-target spell effects. |
| `Devil_Pact` | `BaseCreature` subclass | Summoned devil Mobile used by `DevilPactMagic`. |
| `Initializer` | spell registry bootstrap | Attempts to register IDs `800` through `813`. |
| `SpellRegistry.NewSpell(string, ...)` | factory helper | Can instantiate enchantment classes by name through the `Enchantments` namespace. |
| Generic `[Cast]` command | staff command | Counselor-level command that calls `SpellRegistry.NewSpell(name, caster, null)`. |
| XMLSpawner spell action | XMLSpawner integration | Can call `SpellRegistry.NewSpell(keyword, caster, cwand)` for string spell names. Numeric IDs `800` through `813` are blocked by the registry size issue. |

## Entry Points And Access
| Access path | Compiled behavior |
| --- | --- |
| Spell IDs | `Initializer` attempts to register IDs `800` through `813` for the fourteen enchantment classes. |
| Numeric spell lookup | `SpellRegistry.Register` and `SpellRegistry.NewSpell(int, ...)` reject IDs greater than or equal to `m_Types.Length`. `m_Types` is `new Type[700]`, so all IDs `800` through `813` are rejected. |
| Spellbook lookup | `SpellbookType` has no Enchantments value, and `Spellbook.GetTypeForSpell` has no range for IDs `800` through `813`. |
| Player commands | No enchantment-specific player command registrar was found. |
| Staff command | `[Cast BanishMagic]`, `[Cast DemonicTouchMagic]`, and the other exact class names can be resolved by the generic counselor-level `[Cast <name>]` command. |
| XMLSpawner | XMLSpawner's string spell path can instantiate by class name. Its numeric spell path is blocked for `800` through `813`. |

## Shared Casting Rules
All spells inherit these rules from `EnchantMagic` unless the individual spell has more restrictive logic:

| Rule | Compiled behavior |
| --- | --- |
| Namespace | `Server.Spells.Enchantments` |
| `SpellInfo` | Every spell uses `new SpellInfo("", "", 0)`, so the class name is the reliable source name. |
| Cast recovery | `CastRecoveryBase = 7` |
| Hands | `ClearHandsOnCast = false` |
| `CheckCast()` | Returns `true` in the base class. `DevilPactMagic` and `OrbOfOrcusMagic` add extra checks. |
| Mana | `GetMana()` returns `0`. Individual `RequiredMana` properties also return `0`. |
| Skill window | `GetCastSkills(out min, out max)` sets both values to `0`. Individual `RequiredSkill` properties also return `0.0`. |
| Cast skill | Inherited from `Spell`: `SkillName.Magery`. |
| Damage skill | Inherited from `Spell`: `SkillName.Psychology`. |
| `CastingSkill()` | Returns constant `100.0`. |
| `ComputePowerValue(from, div)` | Returns `floor(sqrt(30000)) / div`, or `0` when `from == null`. Since `floor(sqrt(30000))` is `173`, `ComputePowerValue(1)` is `173`. |

## Spell Reference
| ID | Class | Target | Delay | Circle | Effect |
| ---: | --- | --- | ---: | --- | --- |
| 800 | `BanishMagic` | Harmful `BaseCreature` | 1s | Third | Deletes dispellable creatures using `(50 + ((100 * (100.0 - DispelDifficulty)) / (DispelFocus * 2))) / 100` as the delete chance. `ControlSlots == 666` creatures use `100 > Utility.RandomMinMax(1, 100)`, effectively 99%. |
| 801 | `DemonicTouchMagic` | Beneficial `Mobile` | 1s | First | Heals the target for `PlayerLevelMod(50, Caster)`. |
| 802 | `DevilPactMagic` | Point target | 1s | Eighth | Requires four free follower slots, checks spawn and town rules, then summons a `Devil_Pact`. A player caster gets a `+50` second duration bonus: AOS duration is `140s` for players and `90s` for non-players; non-AOS duration is `Utility.Random(80, 40) + bonus`. |
| 803 | `GrimReaperMagic` | Self | 0.5s | Second | Starts an Enemy of One-style player state by setting `PlayerMobile.EnemyOfOneType = null` and `WaitingForEnemy = true`. Duration is `clamp(ComputePowerValue(1) / 60, 1.5, 3.5)` minutes, which is about `2.88` minutes with the constant base helper. |
| 804 | `HagHandMagic` | Beneficial `Mobile`, `BookBox`, or `CurseItem` | 1s | First | On a Mobile, computes a Karma-derived chance and calls `RemoveCurseSpell.RemoveBadThings(m)` on success. On `BookBox` or `CurseItem`, moves all contained items to the caster's backpack, sends feedback, and deletes the container. |
| 805 | `HellfireMagic` | Harmful `Mobile` | 1s | Sixth | Deals `20` fire damage immediately, then starts a burn timer. Burn ticks every `2s` for five ticks, using levels `5` down to `1`; each tick deals `Utility.RandomMinMax(level, level * 2)`, doubled against non-player targets. |
| 806 | `LucifersBoltMagic` | Harmful `Mobile` | 1s | First | Rejects already frozen, paralyzed, or currently casting AOS targets. Checks reflect at circle `4`, then paralyzes for `7.0 + (100 * 0.2)`, or `27s`. |
| 807 | `OrbOfOrcusMagic` | Self | 1s | Seventh | Ends the current `DefensiveSpell`, rejects an existing positive `MagicDamageAbsorb`, then sets `Caster.MagicDamageAbsorb = 25` and adds the Orb of Orcus buff icon. |
| 808 | `ShieldOfHateMagic` | Beneficial `Mobile` | 1s | Fifth | Adds resistance mods for `100s`: Fire `+0`, Poison `+0`, Cold `+0`, Physical `+100`. Recasting on a stored target expires the old timer before applying a new one. |
| 809 | `SoulReaperMagic` | Harmful `Mobile` | 1s | Third | Starts a `30s` timer ticking every `1.5s`. Each tick subtracts `10` Mana, or sets Mana to `0` if the target has less than `10`. |
| 810 | `StrengthOfSteelMagic` | Beneficial `Mobile` | 1s | First | Adds a Strength stat bonus of `PlayerLevelMod((int)(100 / 3), Caster)` for `10` minutes. |
| 811 | `StrikeMagic` | Harmful `Mobile` | 1s | First | Deals `50` energy damage. |
| 812 | `SuccubusSkinMagic` | Beneficial `Mobile` | 3s | Second | For `100s`, heals every `4s` for `PlayerLevelMod(Utility.RandomMinMax(5, 10), target)`. The first timer tick can heal almost immediately because `NextTick` starts at its default value. |
| 813 | `WrathMagic` | Point target, radius `5` | 1s | Third | Collects mobiles in range, excluding the caster and the caster's controlled pets. Damage is `GetNewAosDamage(32, 1, 4, Caster.Player && playerVsPlayer) + 20` for player casters, then `(damage * 2) / targetCount`, dealt as energy damage to each target outside a `HouseRegion`. |

## Hag Hand Karma Chance
`HagHandMagic` uses `int karma = Caster.Karma * -1`, then applies this chance table before calling `RemoveCurseSpell.RemoveBadThings(m)`:

| Inverted Karma range | Chance formula |
| ---: | --- |
| `< -5000` | `0` |
| `< 0` | `(int)Math.Sqrt(20000 + karma) - 122` |
| `< 5625` | `(int)Math.Sqrt(karma) + 25` |
| `>= 5625` | `100` |

## Summoned Devil_Pact Mobile
`Devil_Pact` is a summoned melee `BaseCreature` with `DeleteCorpseOnDeath = Summoned`, `DispelDifficulty = 80.0`, `DispelFocus = 20.0`, `BleedImmune = true`, and `PoisonImmune = Poison.Lethal`.

| Property | Value |
| --- | --- |
| AI / fight mode | `AIType.AI_Melee`, `FightMode.Closest` |
| Body / title | Body `102`, title `the Devil` |
| Name | `NameList.RandomName("demonic")` |
| Stats | Str `200`, Dex `200`, Int `100` |
| Hits | `140` on SE cores, otherwise `70` |
| Stam / Mana | Stam `250`, Mana `0` |
| Damage | `14` to `17`, 100% Energy |
| Resistances | Physical `60-70`, Fire `40-50`, Cold `40-50`, Poison `40-50`, Energy `90-100` |
| Skills | MagicResist `99.9`, Tactics `100.0`, FistFighting `120.0` |
| Virtual armor | `40` |
| Control slots | `4` |
| Fight ranking | `200 / Math.Max(GetDistanceToSqrt(m), 1.0)` |

## Serialization
The spell classes do not serialize state. Runtime effects are held in static `Hashtable` entries and `Timer` instances, so active enchantment timers are transient.

`Devil_Pact` includes the required `Serial` constructor and writes version `0` after `base.Serialize(writer)`. `Deserialize` reads the same version integer after `base.Deserialize(reader)` and has no custom persisted fields.

## Known Issues
| Issue | Impact |
| --- | --- |
| `SpellRegistry` cannot hold IDs `800` through `813`. | `Initializer` attempts to register Enchantment Magic, but `SpellRegistry.Register` rejects every ID because `m_Types` has only 700 entries. Numeric casting and spellbook requests for these IDs cannot work. |
| No spellbook, scroll, gump, acquisition path, or player command wrappers were found. | The system is effectively staff/XMLSpawner-only by string class name unless additional wiring exists outside the traced C# files. |
| `SpellbookType` and `GetTypeForSpell` do not include Enchantments. | Even if the registry size were fixed, the normal spellbook learning/casting path has no book type for IDs `800` through `813`. |
| `DevilPactMagic` tells the caster they can double-click the summon to dispel it, but `BaseCreature.isVortex` checks `DevilPact`, not `Devil_Pact`. | The summoned `Devil_Pact` Mobile is not included in the double-click dispel whitelist used by `BaseCreature.OnDoubleClick`. |
| `SoulReaperMagic` overwrites `m_Table[target]` with a new timer without stopping any previous timer. | Recasting can leave older timers running and cause duplicate Mana drains until those timers expire naturally. |
| `HellfireMagic.DoBurn` calls `m.Damage(damage, m)` instead of using the stored caster. | Burn tick damage is attributed to the target Mobile, not the caster. |
| `StrengthOfSteelMagic` applies the stat bonus to the target but removes/adds the buff icon on `Caster`. | Beneficial casts on another Mobile can show or clear the buff on the wrong Mobile. |
| `HagHandMagic.TargetItem` has no ownership, backpack, Karma, or skill gate before moving contents from a targeted `BookBox` or `CurseItem`. | A valid target in range can have its contents moved to the caster's backpack and the container deleted without the safeguards present in some other curse-removal paths. |
| All Enchantment spell `SpellInfo` names and mantras are blank, and all per-spell `RequiredSkill` and `RequiredMana` values are zero. | User-facing spell metadata is incomplete, and the base casting path treats the line as zero-mana, zero-skill Magery casts. |
