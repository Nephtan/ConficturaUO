# Syth Magic

## Overview
Syth Magic is a custom `SpellbookType.Syth` spell line using spell IDs `270` through `279`. The system is built around a bound `SythSpellbook`, ten `SythDatacron##` `SpellScroll` Items, `HellShard` reagent charges, player cast commands, quick-bar `Gump` support, Magic Forge speech hooks, and the shared `SythSpell` base class.

The spell line uses `SkillName.Psychology` as `CastSkill` and `SkillName.Swords` as `DamageSkill`. Casts reject positive Karma, require enough mana, require enough hell shards stored as `crystals` in a caster-owned `SythSpellbook`, and use Swords, Tactics, and negative Karma to scale several powers.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `SythSpell` | `Spell` subclass | Shared Syth cast rules, crystal accounting, fizzle effects, spell metadata, command dispatch, and power formulas. |
| `SythSpellbook` | `Spellbook` subclass | Bound datacron. Uses `SpellbookType.Syth`, `BookOffset = 270`, and `BookCount = 11`. Opens the custom gump only for its `owner`. |
| `SythSpellbookGump` | `Gump` | Displays learned powers, help pages, clue pages, quick-bar controls, and laser sword construction buttons. |
| `SythDatacron01` through `SythDatacron10` | `SpellScroll` subclasses | Mysticron scroll Items that teach spell IDs `270` through `279` to the Syth spellbook. |
| `HellShard` | `BaseReagent` subclass | Crystal charge Item. Dropping it on a datacron increases `SythSpellbook.crystals` up to `50000`. |
| `CastSythSpells` | command registrar | Registers one player command per Syth power. |
| `MagicForges` | speech-reactive `Item` | Grants the initial datacron in Malak's tomb and contains the broken mysticron grant logic. |
| `DeathKnightDemon` | vendor-like `Mobile` | Converts gold into `HellShard` stacks for eligible Syth players. |
| `DropRelic` creature death logic | loot helper | Drops `HellShard` stacks from Exorcism-slayer victims for eligible Syth killers. |
| `SoulOrb` | resurrection `Item` | Reused by `CloneBody` as the cloning crystal implementation. |

`Initializer` registers spell IDs `270` through `279`, and `Spellbook.GetTypeForSpell` maps `270 <= spellID < 280` to `SpellbookType.Syth`.

## Acquisition
### Starting Datacron
The starting datacron is granted by `MagicForges.OnSpeech` when the speech reaches the `the Tomb of Malak the Syth Lord` branch.

| Requirement | Compiled condition |
| --- | --- |
| Region keyword | The Mobile's region must be part of `the Tomb of Malak the Syth Lord`, which sets the speech keyword to `Anakasu Arrii Venaal`. |
| Spoken phrase | Speech must contain `Anakasu Arrii Venaal`. |
| Existing assets | Every `SythSpellbook` in `World.Items` with `owner == m` is deleted before the eligibility check below. |
| Karma | `m.Karma < 0`. |
| Psychology | `m.Skills[SkillName.Psychology].Base > 0`. |
| Granted item | `new SythSpellbook((ulong)0, m)` is added to the Mobile's backpack. |

The new book starts with no spells, `crystals = 0`, `page = 0`, `names = 0`, `gem = 0`, and `steel = 0`.

### Being Treated As A Syth
`GetPlayerInfo.isSyth(m, checkSword)` returns true only when both checks pass:

| Check | Compiled behavior |
| --- | --- |
| Bound datacron | `Spellbook.FindSyth(m)` must find a `SythSpellbook` whose `owner == m`. |
| Syth legality | `SythSpell.SythNotIllegal(m, checkSword)` must return true. |

`SythNotIllegal` requires all of the following:

| Requirement | Compiled behavior |
| --- | --- |
| Sword | If `checkSword == true`, the Mobile must have a `BaseSword` in `Layer.OneHanded` or `Layer.TwoHanded`. If `checkSword == false`, the sword check is skipped. |
| Syth item | The Mobile must wear at least one qualifying item with a name containing `Syth`: shield in `Layer.TwoHanded`, allowed helm/hat art in `Layer.Helm`, robe in `Layer.OuterTorso`, cloak in `Layer.Cloak`, or talisman ItemID `0x4CDE` in `Layer.Talisman`. |
| Minimum skills | `Psychology.Base >= 10`, `Swords.Base >= 10`, and `Tactics.Base >= 10`. |

Casting additionally rejects `Caster.Karma > 0`.

### Learning Powers
The ten mysticron classes exist and can be learned by dropping a single compatible `SythDatacron##` `SpellScroll` onto a `SythSpellbook`. The shared `Spellbook.OnDragDrop` path requires the scroll spell ID to map to `SpellbookType.Syth`, the book not to already contain that spell, and `spellID - BookOffset` to be within the book range. On success, the matching content bit is set, the book count increments, the scroll is deleted, and sound `0x558` is played.

The `SythSpellbookGump` clue pages expose the following mysticron data. The Magic Forge grant path for these mysticrons is present in source but not reachable in compiled logic; see Known Issues.

| Spell ID | Mysticron item | Power | Required skill | Mana | Crystals | Mantra | Clue location | Syth Lord |
| ---: | --- | --- | ---: | ---: | ---: | --- | --- | --- |
| `270` | `SythDatacron01` | Psychokinesis | `10` | `5` | `6` | `Dzwol Hyal` | the Fires of Hell, Land of Sosaria | Prince Myrhal of Rax |
| `271` | `SythDatacron02` | Death Grip | `20` | `8` | `16` | `Zayin Kun` | Dungeon Doom, Land of Sosaria | Lady Kath of Naelex |
| `272` | `SythDatacron03` | Projection | `30` | `12` | `24` | `Rhak Skuri` | the Ancient Pyramid, Land of Sosaria | Saint Kargoth |
| `273` | `SythDatacron04` | Throw Sword | `40` | `16` | `12` | `Chwit Sutta` | Dungeon Exodus, Land of Sosaria | Sir Maeril of Naelax |
| `274` | `SythDatacron05` | Speed | `50` | `20` | `80` | `Qyasik Tukata` | Dungeon Clues, Land of Sosaria | Lord Monduiz Dephaar |
| `275` | `SythDatacron06` | Syth Lightning | `60` | `24` | `32` | `Sutta Wo` | the Mausoleum, Island of Umber Veil | Lord Androma of Gara |
| `276` | `SythDatacron07` | Absorption | `70` | `28` | `100` | `Taral Wai` | the City of the Dead, Land of Ambrosia | Sir Farian of Lirtham |
| `277` | `SythDatacron08` | Psychic Blast | `80` | `32` | `48` | `Wai Kusk` | Dungeon Wrong, Land of Lodoria | Lord Thyrian of Naelax |
| `278` | `SythDatacron09` | Drain Life | `90` | `36` | `52` | `Derriphan Tyuk` | the Lodoria Catacombs, Land of Lodoria | Sir Minar of Darmen |
| `279` | `SythDatacron10` | Clone | `100` | `40` | `75` | `Itsu Sutta` | Dungeon Deceit, Land of Lodoria | Sir Rezinar of Haxx |

## Crystal Economy
`HellShard` is a stackable `BaseReagent` with `ItemID = 0x3003`, hue `0x86C`, and name `hell shard`.

| Source | Compiled behavior |
| --- | --- |
| DeathKnightDemon conversion | Dropping `Gold` with `Amount >= 5` on a `DeathKnightDemon` while `GetPlayerInfo.isSyth(from, false)` is true creates `new HellShard(dropped.Amount / 5)` in the player's backpack and deletes the entire gold stack. |
| Exorcism-slayer victim | In `DropRelic`, if `SlayerName.Exorcism` slays the killed creature, the credited killer resolves to a `PlayerMobile`, and `Utility.RandomBool()` passes, an eligible Syth killer receives a corpse drop of `HellShard(Utility.RandomMinMax(minhs, maxhs))`. `minhs = max(1, from.Fame / 600)` and `maxhs = max(3, from.Fame / 400)`. |
| Datacron charging | Dropping `HellShard` on a `SythSpellbook` adds its amount to `crystals`, capped at `50000`. If the stack would overfill the book, only the needed amount is consumed and the remainder stays in the dropped stack. |
| Lower reagent cost | `SythSpell.GetTithing` and `SythSpell.DrainCrystals` each treat `AosAttribute.LowerRegCost > Utility.Random(100)` as a zero-crystal roll. |

`SythSpell.GetCrystals` and `DrainCrystals` scan all `World.Items` for any `SythSpellbook` whose `owner == caster`; the datacron does not need to be equipped, but it must exist in world item storage and have the caster as owner.

## Datacron Item Transformation
Dropping eligible Items onto a `SythSpellbook` can transform or configure Syth equipment:

| Dropped Item | Result |
| --- | --- |
| Single `Ruby`, `Amber`, `Amethyst`, `Citrine`, `Emerald`, `Diamond`, `Sapphire`, `StarSapphire`, or `Tourmaline` | Sets `SythSpellbook.gem` to the dropped gem hue, or to a hard-coded default hue when the gem has no hue, then deletes the gem. |
| `MaterialInfo.IsMagicTalisman(dropped)` | Sets `ItemID = 0x4CDE`, `Name = "Syth Exacron"`, and a random Syth hue after the transform effect. |
| Item with name containing `Durasteel`, hue `0x7A9`, and `steel < 1` | Sets `SythSpellbook.steel = 1` and deletes the item. |
| `BaseHat` or `MagicHat` | Converts the hat to Syth headwear. ItemID toggles from `0x4CDA` to `0x4CDC`, otherwise to `0x4CDA`; name becomes `Syth hood` or `Syth cowl`. |
| `BaseShield` | Sets `ItemID = 0x1BC3`; name becomes `Syth shield`. |
| Item in `Layer.OuterTorso` | Sets `ItemID = 0x2B69`; name becomes `Syth robe`. |
| Cloak-layer item with one of the allowed cloak ItemIDs | Sets `ItemID = 0x1515`; name becomes `Syth cloak`. |
| `BaseArmor` in `Layer.Helm` | Sets `ItemID = 0x2FBB`; name becomes `Syth helm`. |

The method blocks artifact alteration when `MyServerSettings.AlterArtifact(dropped)` returns false.

## Shared Casting Rules
All Syth spells inherit these rules unless the individual spell adds more checks:

| Rule | Compiled behavior |
| --- | --- |
| Cast skill | `SkillName.Psychology`. |
| Damage skill | `SkillName.Swords`. |
| Clear hands | `ClearHandsOnCast = false`. |
| Cast recovery base | `7`. |
| Karma gate | `Caster.Karma > 0` rejects the cast. |
| Required skill gate | `GetSythSkillMax(Caster) < RequiredSkill` rejects the cast. This helper returns the higher of Swords and Psychology values. |
| Crystal gate | `GetCrystals(Caster) < RequiredTithing` or current tithing rejects the cast. |
| Mana gate | `Caster.Mana < GetMana()` rejects the cast. `GetMana()` returns `ScaleMana(RequiredMana)`. |
| Speed region gate | If no-mount regions are enabled, `SythSpeed` is blocked in `AnimalTrainer.IsNoMountRegion`. |
| Cast skill range | `GetCastSkills` returns `RequiredSkill` through `RequiredSkill + 20.0`. |

Shared power formulas:

| Formula | Compiled behavior |
| --- | --- |
| Syth damage power | `karma = clamp(Caster.Karma * -1, 0, 15000); power = (karma / 120) + Tactics.Value + Swords.Value`. |
| Unused power helper | `ComputePowerValue(from, div) = (int)Math.Sqrt((from.Karma * -1) + 20000 + (from.Skills.Psychology.Fixed * 10)) / div`. No Syth spell calls it. |

## Power List
| Spell ID | Command | Power | Required skill | Mana | Crystals | Cast delay | Primary behavior |
| ---: | --- | --- | ---: | ---: | ---: | ---: | --- |
| `270` | `[Psychokinesis` | Psychokinesis | `10` | `5` | `6` | 0.5s | Telekinesis/use support for `ITelekinesisable`, containers, and movable loose Items. |
| `271` | `[DeathGrip` | Death Grip | `20` | `8` | `16` | 0.5s | Applies initial and periodic physical damage to a harmful Mobile. |
| `272` | `[Projection` | Projection | `30` | `12` | `24` | 0.75s | Summons a `SythProjection` clone and hides the caster. |
| `273` | `[ThrowSword` | Throw Sword | `40` | `16` | `12` | 0.5s | Throws the equipped `BaseSword` at a harmful target. |
| `274` | `[SythSpeed` | Speed | `50` | `20` | `80` | 0.5s | Sends mount-speed movement to the caster for a timed duration. |
| `275` | `[SythLightning` | Syth Lightning | `60` | `24` | `32` | 0.5s | Damages a limited number of Mobiles near a target point with energy damage. |
| `276` | `[Absorption` | Absorption | `70` | `28` | `100` | 3.0s | Sets `Caster.MagicDamageAbsorb` to a random absorb value. |
| `277` | `[PsychicBlast` | Psychic Blast | `80` | `32` | `48` | 0.5s | Delayed harmful target energy damage. |
| `278` | `[DrainLife` | Drain Life | `90` | `36` | `52` | 0.5s | Drains life from a harmful target and heals the caster. |
| `279` | `[CloneBody` | Clone | `100` | `40` | `75` | 0.5s | Creates a resurrection `SoulOrb` named `cloning crystal`. |

## Commands
All command handlers are registered at `AccessLevel.Player` and have `[Usage]` plus `[Description]` attributes. Each command checks `Multis.DesignContext.Check(e.Mobile)`, verifies the caster has the matching spell in a compatible spellbook, and sends localized message `500015` when the spell is missing.

| Usage | Description | Spell ID | Handler action |
| --- | --- | ---: | --- |
| `Psychokinesis` | Casts Psychokinesis | `270` | `new Psychokinesis(e.Mobile, null).Cast()` |
| `DeathGrip` | Casts Death Grip | `271` | `new DeathGrip(e.Mobile, null).Cast()` |
| `Projection` | Casts Projection | `272` | `new Projection(e.Mobile, null).Cast()` |
| `ThrowSword` | Casts Throw Sword | `273` | `new ThrowSword(e.Mobile, null).Cast()` |
| `SythSpeed` | Casts Speed | `274` | `new SythSpeed(e.Mobile, null).Cast()` |
| `SythLightning` | Casts Syth Lightning | `275` | `new SythLightning(e.Mobile, null).Cast()` |
| `Absorption` | Casts Absorption | `276` | `new Absorption(e.Mobile, null).Cast()` |
| `PsychicBlast` | Casts Psychic Blast | `277` | `new PsychicBlast(e.Mobile, null).Cast()` |
| `DrainLife` | Casts Drain Life | `278` | `new DrainLife(e.Mobile, null).Cast()` |
| `CloneBody` | Casts Clone | `279` | `new CloneBody(e.Mobile, null).Cast()` |

No Syth-specific administrator command was found in the trace.

## Spell Details
### Psychokinesis
`Psychokinesis` assigns a target with range `10` under ML or `12` otherwise. It handles three target categories:

| Target type | Behavior |
| --- | --- |
| `ITelekinesisable` | Turns the caster toward the target and calls `obj.OnTelekinesis(Caster)`. |
| `Container` | Handles inaccessible containers, snooping containers rooted under another Mobile, corpse loot checks, and region double-click permission. Successful region use calls `item.OnItemUsed(Caster, item)`. |
| Loose `Item` | Moves a single movable Item into the caster's backpack when `item.Amount <= 1`, `item.Weight <= Caster.Int / 20`, and `item.RootParentEntity == null`. |

### Death Grip
`DeathGrip` targets a visible Mobile at range `10` under ML or `12` otherwise. On success it computes:

```text
duration = GetSythDamage(Caster) / 5 seconds
min = 5
max = (int)(GetSythDamage(Caster) / 25) + 5
damage = Utility.RandomMinMax(min, max)
```

It deals an initial 100 percent physical AOS damage hit, then starts `GripTimer`. The timer ticks every `0.1` seconds, deals another damage roll whenever its internal counter is greater than `60`, and stops at the duration deadline.

### Projection
`Projection` requires the caster to be unmounted, have room for `3` follower slots, and not be under `HorrificBeastSpell`. On success it creates a `SythProjection` at the caster's location and sets `Caster.Hidden = true`.

`SythProjection` copies the caster's body, hue, sex, name, title, kills, hair, facial hair, skills, and visible item art. It is a summoned, uncommandable, non-dispellable `BaseCreature` using `AI_Melee`, `FightMode.Closest`, `ControlSlots = 3`, `SetDamage(1, 1)`, physical damage type, physical/fire/cold/poison resist ranges `20..40`, and hits randomly selected between `60` and `GetSythDamage(caster)` with a minimum max of `60`.

The summon duration is `GetSythDamage(caster) / 2` seconds.

### Throw Sword
`ThrowSword` requires an equipped `BaseSword` in one-handed or two-handed layers. On a harmful target it sends a moving effect using the sword art and deals AOS damage:

| Damage part | Formula |
| --- | --- |
| Minimum | `equippedSword.MinDamage + 10` |
| Maximum | `equippedSword.MaxDamage + (GetSythDamage(Caster) / 7)` |
| Damage roll | `Utility.RandomMinMax(min, max)` |
| Damage types | Uses the sword's AOS physical/cold/fire/energy/poison distribution; if all are `0`, physical becomes `100`. |

### Speed
`SythSpeed` rejects mounted casters, `BootsofHermes`, `Artifact_BootsofHermes`, `Artifact_SprintersSandals`, and racial `HikingBoots`. On success it applies `SpeedControl.MountSpeed`, records the caster in `TableSythRunning`, adds `BuffIcon.Speed`, and starts a timer.

Duration is `TotalTime = (int)(GetSythDamage(Caster) * 4)`, with a minimum of `600` seconds.

### Syth Lightning
`SythLightning` targets a point at range `12`, requires the target point to be visible, requires `SpellHelper.CheckTown`, and scans Mobiles within `5` tiles of the target point.

Targets must be in the same `Region` as the caster, must not be the caster, and must not resolve to the caster as a controlled creature's master. House-region targets are skipped at damage time.

| Formula | Compiled behavior |
| --- | --- |
| Base damage roll | `Utility.RandomMinMax(20, (int)(GetSythDamage(Caster) / 5))`. |
| Maximum struck foes | `foes = (int)(GetSythDamage(Caster) / 50)`, minimum `1`. |
| Target-count scaling | `1 target = 100%`, `2 = 90%`, `3 = 80%`, `4 = 70%`, `5 = 60%`, `6 = 50%`, more than `6 = 40%`. |
| Damage type | `100` percent energy. |

### Absorption
`Absorption` calls `DefensiveSpell.EndDefense(Caster)` and rejects the cast when `Caster.MagicDamageAbsorb > 0`. On success it sets:

```text
Caster.MagicDamageAbsorb = Utility.RandomMinMax(15, (int)(GetSythDamage(Caster) / 4))
```

It then applies particles, plays sound `0x64C`, and adds `BuffIcon.Absorption`.

### Psychic Blast
`PsychicBlast` targets a visible Mobile at range `10` under ML or `12` otherwise. On success it queues a `0.1` second delayed callback. If `caster.HarmfulCheck(defender)` still passes, the callback deals 100 percent energy AOS damage:

```text
min = 26
max = (int)(GetSythDamage(Caster) / 3)
if (max > 125) max = 125
damage = Utility.RandomMinMax(min, max)
```

`GetSlayerDamageScalar()` returns `1.0`, so the spell is not affected by slayer spellbooks.

### Drain Life
`DrainLife` targets a visible Mobile at range `10` under ML or `12` otherwise. If the target is a `BaseCreature`, the spell refuses creatures slain by `SlayerName.Silver`, `SlayerName.ElementalBan`, or `SlayerName.GolemDestruction`.

On success it computes:

```text
duration = GetSythDamage(Caster) / 5 seconds
min = 7
max = (int)(GetSythDamage(Caster) / 25)
drain = Utility.RandomMinMax(min, max)
```

The initial hit heals the caster by `drain`, deals 100 percent physical AOS damage to the target, and applies `BuffIcon.DrainLifeGood` to the caster plus `BuffIcon.DrainLifeBad` to the target. `DrainTimer` ticks every `0.1` seconds, repeats the drain whenever its internal counter is greater than `45`, and removes both buffs when the duration expires or the caster is no longer alive.

### Clone
`CloneBody` scans `World.Items` for existing `SoulOrb` Items whose `m_Owner == Caster`, deletes them, then creates a new `SoulOrb`:

| Field | Value |
| --- | --- |
| `m_Owner` | `Caster` |
| `Name` | `cloning crystal` |
| `ItemID` | `0x705` |
| Location | Added to caster backpack |
| Registration | `SoulOrb.OnSummoned(Caster, iOrb)` |

`SoulOrb.OnSummoned()` stores the owner/orb pair in a static resurrection dictionary and adds `BuffIcon.Resurrection`. On player death, the orb queues a resurrection callback after `30` seconds, resurrects the player if still dead, applies the death penalty, removes the resurrection buff, and deletes the orb.

## Spellbook Gump And Quick Bars
`SythSpellbook.OnDoubleClick()` opens `SythSpellbookGump` only when `owner == from` and the datacron is directly equipped by the Mobile or directly in the Mobile's backpack.

`SythSpellbookGump` has these main pages:

| Page | Behavior |
| ---: | --- |
| `1` | Status page with Syth skill, max mana, crystal count, power, quick-bar buttons, laser sword page button, and one row per power. |
| `2` | Introductory help text, including the direct player commands. |
| `6` | Laser sword construction page. |
| `270` through `279` | Per-power pages showing learned state, cost data, description, and clue text for unlearned powers. |

The row and column quickbars are non-closable draggable gumps that render only learned powers. Pressing a quickbar power button calls `SythSpell.CastSpell(from, info.ButtonID)` and then reopens the same bar if `GetPlayerInfo.isSyth(from, true)` still passes.

The player Quick Bar command also has Syth support on button `38`, toggling between the Syth vertical and horizontal quickbar when the caster has an owned `SythSpellbook` directly in the backpack.

### Laser Sword Construction
The construction page displays create buttons only when all of the following are true:

| Requirement | Compiled condition |
| --- | --- |
| Syth identity | `GetPlayerInfo.isSyth(from, false)`. |
| Skills | `Psychology.Value >= 100`, `Tactics.Value >= 100`, and `Swords.Value >= 100`. |
| Fame | `from.Fame >= 15000`. |
| Karma | `from.Karma <= -15000`. |
| Region | `Region.Find(from.Location, from.Map).IsPartOf("the Tomb of Malak the Syth Lord")`. |
| Durasteel | `m_Book.steel >= 1`. |
| Gem | `m_Book.gem >= 1`. |
| Gold displayed | `GetWealth(from) >= 10000`, where `GetWealth` counts only backpack `Gold`. |

Button `7` creates a `LevelLaserSword`; button `8` creates a `LevelDoubleLaserSword`. The response handler consumes `10000` gold from the backpack, sets the sword hue to `m_Book.gem`, sets the caster's Fame and Karma to `0`, clears `m_Book.gem` and `m_Book.steel`, names the sword through `SwordName()`, adds it to the backpack, and logs `LoggingFunctions.LogCreatedSyth(from, sword.Name)`.

## Serialization
| Type | Serialized fields |
| --- | --- |
| `SythSpellbook` | Version `0`, then `owner`, `crystals`, `page`, `names`, `gem`, and `steel`. |
| `SythDatacron01` through `SythDatacron10` | Version `0` only, after `SpellScroll` base serialization. |
| `HellShard` | Version `0` only, after `BaseReagent` base serialization. |
| `SythProjection` | Encoded version `0`, then `m_Caster`; `Deserialize()` calls `MirrorImage.AddClone(m_Caster)`. |
| `SoulOrb` | Version `0` only, then `Deserialize()` deletes the orb so clone crystals do not survive a world restart. |

## Known Issues
| Issue | Impact |
| --- | --- |
| `Scripts.csproj` lists `Magic\Syth\SythSpellBookGump.cs`, but the file exists under `Magic\Syth\Gumps\SythSpellBookGump.cs`. | A project build that relies on the listed path may fail to compile the Syth gump. |
| The Magic Forge mysticron grant logic is inside `if (this.Name == "Priest Grave")`, but each Syth branch checks `this.Name == "<mantra>" && this.Name == keyword`. | The traced speech path cannot grant `SythDatacron01` through `SythDatacron10`; powers can only be learned if the mysticron Items are supplied by some other means. |
| The Malak tomb branch deletes all existing owned `SythSpellbook` Items before checking `m.Karma < 0` and Psychology. | Speaking the correct phrase while no longer eligible can delete the player's old datacron without granting a replacement. |
| `SythSpell.GetSythSkillMax()` returns the higher of Swords and Psychology, while messages and gump text say both skills are required. | Casting can pass the skill gate with one high skill and one low skill, as long as the lower skill is at least the legality minimum. |
| `SythSpell.GetMana()` returns `ScaleMana(RequiredMana)`, and base `Spell.CheckSequence()` scales `GetMana()` again. | Spells that use `CheckSequence()` can charge double-scaled mana instead of the displayed base cost. |
| `SythSpell.FinishSequence()` always calls `DrainCrystals(Caster, RequiredTithing)`, and most spell success paths also call `DrainCrystals()` directly. | Successful casts can consume crystals twice, and failed/canceled target sequences can still consume crystals when `FinishSequence()` runs. |
| Several spells call `CheckSequence() && CheckFizzle()`. | The second fizzle check can fail after base `CheckSequence()` has already consumed mana and awarded Karma. |
| `SythSpeed.OnCast()` and `CloneBody.OnCast()` use `CheckFizzle()` directly instead of `CheckSequence()`. | These spells bypass base mana subtraction and Karma award behavior. |
| `CloneBody.OnCast()` never calls `FinishSequence()`. | The caster's spell state may remain uncleared after creating or failing to create a cloning crystal. |
| `SythSpellbookGump.OnResponse()` checks `HasSpell(from, info.ButtonID)` for overview cast buttons `370` through `379`, then subtracts `100` afterward. | The overview-page cast buttons are unreachable because the spellbook does not contain spell IDs `370` through `379`; typed commands and quickbars still use IDs `270` through `279`. |
| Laser sword construction conditions are checked when rendering page `6`, but button responses `7` and `8` only re-check backpack gold consumption. | A stale gump can create a laser sword after the caster leaves Malak's tomb, loses requirements, or changes the datacron's gem/steel state. |
| `SythSpeed` starts a new timer on each successful cast but does not store or stop old timers. | Recasting before the old timer expires can let the old timer remove the refreshed speed effect early. |
| `SythSpellbook.OnDragDrop()` does not verify `from == owner` before charging, transforming gear, setting gem, or consuming durasteel. | A reachable datacron can be modified by a non-owner. |
| `DrainLife.DrainTimer.OnTick()` checks `!m_Caster.Alive` before checking `m_Caster == null`. | If the timer ever holds a null caster reference, the null check is too late to prevent a null-reference failure. |
| `SythProjection.Deserialize()` calls `MirrorImage.AddClone(m_Caster)` without checking whether `m_Caster` is null. | A corrupted or ownerless saved projection can re-register a null caster reference. |
