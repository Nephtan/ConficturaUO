# Necromancy

## Overview
Necromancy is the `SpellbookType.Necromancer` spell line registered at spell IDs `100` through `116`. It uses `Necromancy` as the cast skill and `Spiritualism` as the damage skill, so most spell power, duration, or secondary scaling comes from `Spiritualism` while the fizzle check uses `Necromancy`.

The system is built from the shared `Spell` framework, `NecromancerSpell`, `NecromancerSpellbook`, seventeen `SpellScroll` item classes, four transformation spells, corpse and revenant summons, and the `SummonFamiliarGump`.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `NecromancerSpell` | `Spell` subclass | Shared Necromancy casting rules, skill windows, reagent fallback, mana cost, and Karma award formula. |
| `NecromancerSpellbook` | `Spellbook` subclass | `SpellbookType.Necromancer`, `BookOffset = 100`, and `BookCount = 16` or `17` on `Core.SE`. |
| `AnimateDeadSpell` | `NecromancerSpell` | Raises a `SummonedCorpse` from a `Corpse` owned by a `BaseCreature`. |
| `TransformationSpell` | `NecromancerSpell`, `ITransformationSpell` | Shared body, hue, resistance, and tick behavior for the four necromancy forms. |
| `SummonFamiliarSpell` | `NecromancerSpell` | Opens `SummonFamiliarGump` and summons one selected `BaseFamiliar`. |
| `Revenant` | `BaseCreature` | Summoned Mobile used by `VengefulSpiritSpell`. It pursues one target and deletes on world load. |
| `SummonedCorpse` | `BaseCreature` | Animated dead Mobile with copied/scaled stats, damage, resistances, skills, and poison state. |
| `BaseWeapon` hooks | weapon damage logic | Applies `CurseWeapon`, `VampiricEmbrace`, and `WraithForm` leech behavior during weapon damage. |
| `PlayerMobile` and `BaseCreature` damage hooks | damage and poison logic | Resolve `BloodOath` reflection and `EvilOmen` damage/poison amplification. |

## Acquisition And Learning
`NecromancerSpellbook` is constructable and starts empty unless a content bitmask is passed. It uses item ID `0x2253`, equips on `Layer.Talisman`, and can only be equipped by a Mobile with at least `30.0` base `Necromancy`.

`MyNecromancerSpellbook` is a loot-style subclass that randomizes hue, name, book properties, and `Content` from `0` through `0x1FFFF`.

Scroll learning uses the base `Spellbook.OnDragDrop` path:

| Rule | Compiled behavior |
| --- | --- |
| Accepted item | A single `SpellScroll` with `Amount == 1`. |
| Book type | `Spellbook.GetTypeForSpell(scroll.SpellID)` must equal `SpellbookType.Necromancer`. |
| Duplicate spell | The drop is rejected if `HasSpell(scroll.SpellID)` is already true. |
| Content bit | `val = scroll.SpellID - BookOffset`; when `0 <= val < BookCount`, the matching bit is set and the scroll is deleted. |
| Exorcism gate | Spell ID `116` requires `BookCount == 17`, so it is learnable only when `Core.SE` is active. |

Necromancer and Necromancer Guild vendors sell `NecromancerSpellbook`, the five necromancy reagents, and low-tier scrolls directly. The loot table `Loot.NecromancyScrollTypes` contains all seventeen scroll types.

## Shared Casting Rules
| Rule | Compiled behavior |
| --- | --- |
| Cast skill | `SkillName.Necromancy`. |
| Damage skill | `SkillName.Spiritualism`. |
| Hands | `ClearHandsOnCast` is `false`, so Necromancy does not clear held items through the shared cast path. |
| Fast cast | `CastDelayFastScalar` uses the base value only on `Core.SE`; otherwise it returns `0`. |
| Skill window | `min = RequiredSkill`; `max = RequiredSkill + 40.0`, but scroll casts set `max = min`. |
| Mana | `GetMana()` returns the spell's `RequiredMana`; `Spell.ScaleMana` still applies `MindRot` and lower mana cost. |
| Reagents | `ConsumeReagents()` first uses normal reagents, then falls back to one `ArcaneGem` charge. |
| Karma award | `-(40 + (10 * (CastDelayBase.TotalSeconds / 0.25)))`. Example: `0.75s` awards `-70`, `1.5s` awards `-100`, and `2.0s` awards `-120`. |
| Mixing restriction | `Spell.CantMixSpell` blocks player casts when `Elementalism >= 1` and the requested spell is `ElementalSpell` or `NecromancerSpell`. |

## Spell Reference
| ID | Spell | Delay | Skill | Mana | Reagents | Target and actual effect |
| --- | --- | --- | --- | --- | --- | --- |
| 100 | Animate Dead | 1.5s | 40.0 | 23 | Grave Dust, Daemon Blood | Targets a `Corpse`. The corpse owner must be a `BaseCreature`; undead (`Silver`) and constructs (`GolemDestruction`) are rejected. `level_caster = min(100, (Necromancy + Spiritualism) / 2)` and must be at least `IntelligentAction.GetCreatureLevel(corpseOwner)`. Control slots are `clamp(level_corpse / 20, 1, 5)`. Duration is `((Necromancy + Spiritualism) / 2) * 9` seconds. |
| 101 | Blood Oath | 1.5s | 20.0 | 13 | Daemon Blood | Harmful `PlayerMobile` or `BaseCreature` target, not self. Duration is `((Spiritualism - target MagicResist) / 8) + 8` seconds. Damage from the target to the caster is increased by 10% and reflected back to the target's attacker through the `PlayerMobile`/`BaseCreature` damage hooks. Player reflection has a 35 damage cap when reflected to another `PlayerMobile`. |
| 102 | Corpse Skin | 1.5s | 20.0 | 11 | Bat Wing, Grave Dust | Harmful Mobile target. Duration is `((Spiritualism - target MagicResist) / 2.5) + 40`; self-targeting uses `0.0` resist. Applies Fire `-15`, Poison `-15`, Cold `+PlayerLevelMod(10, caster)`, and Physical `+PlayerLevelMod(10, caster)`. |
| 103 | Curse Weapon | 0.75s | 0.0 | 7 | Pig Iron | Requires `Caster.Weapon` to be a `BaseWeapon` and not `Fists`. Sets `weapon.Cursed = true` for `(Spiritualism / 3.4) + 1 + Necromancy` seconds for player casters, or without the Necromancy addend for non-player casters. A cursed weapon adds 50% life leech during `BaseWeapon` damage. |
| 104 | Evil Omen | 0.75s | 20.0 | 11 | Bat Wing, Nox Crystal | Harmful `BaseCreature` or `PlayerMobile` target. If target Magic Resist base is above `50.0`, adds a non-relative Magic Resist skill mod fixed at `50.0`. Duration is `(Spiritualism / 12) + 1` seconds, but the effect also ends on the next hooked damage or poison event. Damage is multiplied by `1.25`; poison level is increased by one. |
| 105 | Horrific Beast | 2.0s | 40.0 | 11 | Bat Wing, Daemon Blood | Transformation spell. Body `126`, hue `0`, no resistance offsets. Adds the `HorrificBeast` buff, updates weapon damage/stat timers, grants `+20` hit regeneration points through `RegenRates`, and grants `+25%` weapon damage through `BaseWeapon`. |
| 106 | Lich Form | 2.0s | 70.0 | 23 | Grave Dust, Daemon Blood, Nox Crystal | Transformation spell. Body `24`, hue `0`, Fire `-10`, Cold `+10`, Poison `+10`, tick rate `2.5s`. Each tick decrements Hits by 1. Mana regeneration gains `PlayerLevelMod(26, from)` capped points through `RegenRates`. |
| 107 | Mind Rot | 1.5s | 30.0 | 17 | Bat Wing, Pig Iron, Daemon Blood | Harmful Mobile target. Duration is `(((Spiritualism - target MagicResist) / 5.0) + 20.0) * (target.Player ? 1.0 : 2.0)`. Sets spell mana scalar to `1.25` for players and `2.00` for non-players. `Spell.ScaleMana` reads this scalar for later casts. |
| 108 | Pain Spike | 1.0s | 20.0 | 5 | Grave Dust, Pig Iron | Harmful Mobile target with immediate physical-style direct damage through `Mobile.Damage`. Damage is `((Spiritualism - target MagicResist) / 10) + (target.Player ? 18 : 30) + Necromancy / 5` for player casters, minimum `1`. First application starts a 10 second restore timer; recasts while active deal random `3..7` damage and extend the timer by 2 seconds. |
| 109 | Poison Strike | `Core.ML ? 1.75s : 1.5s` | 50.0 | 17 | Nox Crystal | Harmful Mobile target with radius `2`. Base damage is `RandomMinMax(Core.ML ? 32 : 36, 40) * ((300 + Spiritualism * 9) / 1000) + Necromancy / 10 + Poisoning / 4`. Spell Damage Increase is capped by server settings and divided by `25` before multiplying damage. Center target takes full damage, range 1 takes half, range 2 takes one third; damage is 100% Poison. |
| 110 | Strangle | 2.0s | 65.0 | 29 | Daemon Blood, Nox Crystal | Harmful Mobile target. `spiritLevel = Spiritualism / 10`, plus `Necromancy / 25` for player casters; timer count is `max(4, (int)spiritLevel)`. First hit is after 5 seconds; later delay drops toward 1 second. Per-tick damage is `Random(minBase, maxBase) * (3 - (target.Stam / target.StamMax) * 2)`, minimum `1`, multiplied by `1.75` against non-player targets, 100% Poison. |
| 111 | Summon Familiar | 2.0s | 30.0 | 17 | Bat Wing, Grave Dust, Daemon Blood | Opens `SummonFamiliarGump` after `CheckSequence()`. The selected familiar is summoned for one day at the caster location. Its Magic Resist is set to the caster's Magic Resist, damage min/max each gain `(Necromancy + Spiritualism) / 25`, and Hits are set to `HitsMax + (Necromancy + Spiritualism) / 2`. |
| 112 | Vampiric Embrace | 2.0s | 99.0 | 23 | Bat Wing, Nox Crystal, Pig Iron | Transformation spell. Human casters use body `606` female or `605` male and hue `0xB70`; `RaceID > 0` uses body `125` and hue `0`. Fire `-25`. If current Necromancy is at least `99.0`, cast window is `80.0..120.0`. Adds 20% life leech to weapon and spell damage hooks, adds stamina and mana regeneration bonuses, and auto-cures poison below lethal level on AOS poison ticks. |
| 113 | Vengeful Spirit | 2.0s | 80.0 | 41 | Bat Wing, Grave Dust, Pig Iron | Harmful Mobile target. `CheckCast()` requires three free follower slots. Duration is `((Spiritualism * 80) / 120) + 10 + Necromancy / 2` seconds for player casters, otherwise without the Necromancy addend. Summons a `Revenant` on the target for `duration + 2` seconds. |
| 114 | Wither | 1.5s | 60.0 | 23 | Nox Crystal, Grave Dust, Pig Iron | Self-centered area damage. Range is `4` on `Core.ML`, otherwise `5`. Targets must be in LOS, valid indirect targets, and harmful-eligible. Damage is `RandomMinMax(30, 35) * (300 + target.Karma / 100 + Spiritualism * 10) / 1000`, then Spell Damage Increase is applied with a 15% PvP cap on `Core.SE`, then player casters add `Necromancy / 5`. Damage is 100% Cold. |
| 115 | Wraith Form | 2.0s | 20.0 | 17 | Nox Crystal, Pig Iron | Transformation spell. Body `84`, hue `0`, Physical `+15`, Fire `-5`, Energy `-5`. Player casters set `PlayerMobile.IgnoreMobiles = true`. Weapon hits steal target mana and grant caster mana equal to `5 + ((15 * Spiritualism) / 100)` percent of weapon damage; spell damage leech grants caster mana through `SpellHelper.DoLeech`. |
| 116 | Exorcism | 2.0s | 80.0 | 40 | Nox Crystal, Grave Dust | `Core.SE` book spell. Requires `Spiritualism >= 100.0`. Targets a visible `BaseCreature` that is undead (`Silver`) or demon (`Exorcism`). Bonded creatures are rejected. Non-dispellable demons and creatures with Fame `>= 23000` damage the caster instead. Otherwise success is `Necromancy >= RandomMinMax(1, (target.Fame / 200) + 10)`, then a 1.5 second timer deletes the target if still alive; failure damages the caster. |

## Familiars
All familiars derive from `BaseFamiliar`, are bard immune, poison immune to `Poison.Deadly`, not commandable, have `DispelDifficulty = 80.0`, `DispelFocus = 20.0`, and delete on deserialization after dropping backpack contents. Their base AI follows the master and attacks the master's combatant when not hidden.

| Familiar | Required Necromancy / Spiritualism | Base role and special behavior |
| --- | --- | --- |
| `HordeMinionFamiliar` | 30.0 / 30.0 | Pack-style familiar. Creates a backpack and every 5-10 seconds tries to pick up as many as three movable stackable items within range 2. |
| `ShadowWispFamiliar` | 50.0 / 50.0 | Energy wisp. Every 5-30 seconds flares, then grants mana to nearby alive non-positive-Karma player Mobiles that are not staff and are not in an aggressor relationship with the caster. Mana gain is `1 - (Karma / 1000)`. |
| `DarkWolfFamiliar` | 60.0 / 60.0 | Melee wolf. Every 2 seconds adds 1 Stamina to its `ControlMaster` or `SummonMaster`. |
| `DeathAdder` | 80.0 / 80.0 | Snake familiar. Hit poison is 80% Greater and 20% Deadly. While the caster has a live Death Adder familiar, double-clicking a `DeathAdderCharmable` creature can assign that creature a harmful target. |
| `VampireBatFamiliar` | 100.0 / 100.0 | Melee bat. In `BaseWeapon` damage, the bat's damage heals its caster when the caster is within range 2 on the same map; otherwise the bat heals itself. |

## Transformation Rules
`TransformationSpellHelper` stores one `TransformContext` per Mobile in a static table. Casting a different transformation removes the old one first. Casting the same transformation again removes it and plays the removal effect instead of reapplying it.

When a transformation is active, the helper sets `BodyMod`, `HueMod`, resistance mods, starts a `TransformTimer`, and calls the spell's `DoEffect`. The timer ticks at the spell's `TickRate` and removes the context if the Mobile is deleted, dead, no longer has the expected body, or no longer has the expected hue. Removal resets `HueMod`, `BodyMod`, hair hue, facial hair hue, and race body, then calls the spell's `RemoveEffect`.

Silver-slayer weapon damage gets a 25% bonus against every `NecromancerSpell` transformation except `HorrificBeast`.

## Commands
No Necromancy-specific `[Command]` registrar was found under `Data/Scripts/Magic/Necromancy`. Player casting goes through client spellbook requests, spellbar entries, or double-clicked `SpellScroll` items.

The shared spellbook command applies to Necromancy books:

| Usage | Access | Description |
| --- | --- | --- |
| `[AllSpells]` | `GameMaster` | Begins a target cursor. Targeting a `Spellbook` fills its `Content` bitmask with every spell supported by that book's `BookCount`; for a `NecromancerSpellbook` this fills 16 spells normally or 17 on `Core.SE`. |

## Serialization And Persistence
| Class | Serialized data |
| --- | --- |
| `NecromancerSpellbook` | Calls `base.Serialize(writer)` and writes version `1`; no subclass fields are stored. |
| `MyNecromancerSpellbook` | Calls `base.Serialize(writer)` and writes version `0`; randomized name, hue, content, and base book properties are handled by inherited item serialization. |
| Necromancy scroll classes | Each calls `SpellScroll` serialization, then writes version `0`. `SpellScroll` stores `m_SpellID` in its own version `0` block. |
| `Revenant` | Writes version `0`; `Deserialize` reads it and immediately deletes the Mobile. |
| `BaseFamiliar` | Writes version `0`; `Deserialize` reads it, drops backpack contents, and deletes the Mobile. |
| `SummonedCorpse` | Writes version `0`; it does not serialize `BCPoison` or `BCImmune`, and it does not delete itself on load. |

Active curses, transformations, Mind Rot scalars, Pain Spike timers, Curse Weapon timers, and familiar ownership tables are held in static `Hashtable` or `Dictionary` fields. Those runtime tables are not explicitly serialized by the Necromancy scripts.

## Known Issues
| Issue | Evidence from trace |
| --- | --- |
| `AnimateDeadSpell.Target` bypasses `CheckSequence()` and `CheckHSequence()`. | `OnCast()` only assigns a target, while `Target(object obj)` performs corpse validation and creates/deletes the summon without the normal sequence check. This can skip reagent consumption, mana spend, scroll consumption, skill fizzle, and Karma award on successful animation. |
| `AnimateDeadSpell` scaling comments conflict with the compiled cap. | The code caps `level_caster` at `100`, then says full stats require `125` in both skills. With the cap, level 100 corpses cannot reach `mod = 1.0`; they top out at `0.50`. |
| `SummonedCorpse` loses poison state on world load. | `BCPoison` and `BCImmune` drive `HitPoison` and `PoisonImmune`, but `Serialize` only writes a version integer and `Deserialize` only reads it. Unlike `Revenant` and `BaseFamiliar`, it does not delete itself on deserialize. |
| `CurseWeaponSpell` leaves stale timer entries. | `m_Table` is keyed by `weapon`, but `ExpireTimer.OnTick()` calls `m_Table.Remove(this)` instead of removing the weapon key. |
| `BloodOathSpell` leaves stale target entries. | `m_Table` stores `m_Table[target] = timer`, but `ExpireTimer.DoExpire()` removes `m_Table.Remove(m_Caster)` rather than `m_Target`. |
| `PoisonStrikeSpell`, `WitherSpell`, `HordeMinionFamiliar`, and `ShadowWispFamiliar` leak pooled range enumerables. | They iterate `GetMobilesInRange()` or `GetItemsInRange()` directly; those APIs return `IPooledEnumerable` and should be freed after use. |
| `StrangleSpell` adds a buff even when the harmful sequence fails. | Its buff duration and `BuffInfo.AddBuff` logic run after the `if (CheckHSequence(m))` block rather than inside it. |
| `ExorcismSpell` can self-damage without the normal sequence check. | The non-dispellable demon and high-Fame branches call `SpellHelper.Damage` before the `CheckHSequence(m)` branch, so they can bypass the resource/fizzle path used by successful exorcisms. |
| `SummonFamiliarGump` trusts stale constructor state and hides failures. | `OnResponse` uses stored `m_From` and `m_Spell` rather than `sender.Mobile`, does not guard deleted/dead/stale caster state, and wraps familiar construction in an empty `catch`. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0099.
- Backlog rows: RB-06728.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Magic/Necromancy (CurrentDirectory)

### Runtime Evidence

- Hook summary: Gump=4; Timer=9.
- Data/Scripts/Magic/Necromancy/BloodOathSpell.cs:L151 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/CorpseSkin.cs:L123 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/CurseWeapon.cs:L95 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/CurseWeapon.cs:L114 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/EvilOmen.cs:L80 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/Exorcism.cs:L131 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/MindRot.cs:L158 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/PainSpike.cs:L124 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/Strangle.cs:L142 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Necromancy/SummonFamiliar.cs:L65 Gump SendGump access=Internal
- Data/Scripts/Magic/Necromancy/SummonFamiliar.cs:L207 Gump OnResponse access=Internal
- Data/Scripts/Magic/Necromancy/SummonFamiliar.cs:L233 Gump SendGump access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 18.
- Data/Scripts/Magic/Necromancy/NecromancerSpellbook.cs:Server.Items.NecromancerSpellbook version=1 serialize=L36 deserialize=L43 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/AnimateDeadScroll.cs:Server.Items.AnimateDeadScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/BloodOathScroll.cs:Server.Items.BloodOathScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/CorpseSkinScroll.cs:Server.Items.CorpseSkinScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/CurseWeaponScroll.cs:Server.Items.CurseWeaponScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/EvilOmenScroll.cs:Server.Items.EvilOmenScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/ExorcismScroll.cs:Server.Items.ExorcismScroll version=0 serialize=L27 deserialize=L34 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/HorrificBeastScroll.cs:Server.Items.HorrificBeastScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/LichFormScroll.cs:Server.Items.LichFormScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/MindRotScroll.cs:Server.Items.MindRotScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/PainSpikeScroll.cs:Server.Items.PainSpikeScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Necromancy/Scrolls/PoisonStrikeScroll.cs:Server.Items.PoisonStrikeScroll version=0 serialize=L20 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Magic/Necromancy/AnimateDeadSpell.cs=Keep
- Data/Scripts/Magic/Necromancy/AnimateDeadSpell.cs=Keep
- Data/Scripts/Magic/Necromancy/BloodOathSpell.cs=Keep
- Data/Scripts/Magic/Necromancy/BloodOathSpell.cs=Keep
- Data/Scripts/Magic/Necromancy/CorpseSkin.cs=Keep
- Data/Scripts/Magic/Necromancy/CorpseSkin.cs=Keep
- Data/Scripts/Magic/Necromancy/CurseWeapon.cs=Keep
- Data/Scripts/Magic/Necromancy/CurseWeapon.cs=Keep
- Data/Scripts/Magic/Necromancy/EvilOmen.cs=Keep
- Data/Scripts/Magic/Necromancy/EvilOmen.cs=Keep
- Data/Scripts/Magic/Necromancy/Exorcism.cs=Keep
- Data/Scripts/Magic/Necromancy/Exorcism.cs=Keep
- Data/Scripts/Magic/Necromancy/HorrificBeast.cs=Keep
- Data/Scripts/Magic/Necromancy/HorrificBeast.cs=Keep
- Data/Scripts/Magic/Necromancy/LichForm.cs=Keep
- Data/Scripts/Magic/Necromancy/LichForm.cs=Keep
- Data/Scripts/Magic/Necromancy/MindRot.cs=Keep
- Data/Scripts/Magic/Necromancy/MindRot.cs=Keep
- Data/Scripts/Magic/Necromancy/NecromancerSpell.cs=Keep
- Data/Scripts/Magic/Necromancy/NecromancerSpell.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
