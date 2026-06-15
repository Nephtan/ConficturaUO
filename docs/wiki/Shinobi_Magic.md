# Shinobi Magic

## Overview
Shinobi Magic is a Ninjitsu-based ability system built around spell IDs `290` through `297`, the owner-bound `ShinobiScroll` item, direct player commands, and scroll-driven `Gump` quickbars. It is not wired into `SpellbookType`; learned abilities are checked by scanning the caster's backpack for a `ShinobiScroll` owned by that Mobile.

`Initializer` registers the eight Shinobi spell classes. `CastShinobiSpells.Initialize()` registers one `AccessLevel.Player` command per ability, and the `ShinobiScroll` pages and quickbars cast by dispatching those commands through `CommandSystem.Handle`.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `ShinobiSpell` | `Spell` subclass | Shared Ninjitsu cast and damage skill, tithing/mana gates, mantra, fizzle sounds, cast skill window, and the Cheetah Paws no-mount-region gate. |
| `ShinobiScroll` | `Item` subclass | Owner-bound scroll item using item ID `0x5C15`; stores one integer learned flag per Shinobi ability plus the last viewed page. |
| `ShinobiScrollGump` | `Gump` | Ten-page scroll interface: page 1 help, page 2 quickbar controls, pages 3 through 10 ability pages. |
| `ShinobiRow` / `ShinobiColumn` | `Gump` | Horizontal and vertical ability bars showing only learned abilities. |
| `CastShinobiSpells` | command registrar | Registers the eight player commands and checks `ShinobiScroll.GetShinobi()` before casting. |
| `SummonedTiger` | `BaseCreature` | The 2-control-slot summoned creature used by `TigerStrength`. |

## Registered Abilities
| Spell ID | Ability | Class | Command | Required Ninjitsu | Tithing points | Base mana | Cast delay |
| ---: | --- | --- | --- | ---: | ---: | ---: | --- |
| `290` | Cheetah Paws | `CheetahPaws` | `[CheetahPaws` | `80` | `65` | `60` | 3.0 seconds |
| `291` | Deception | `Deception` | `[Deception` | `30` | `20` | `15` | 3.0 seconds |
| `292` | Eagle Eye | `EagleEye` | `[EagleEye` | `70` | `55` | `50` | 3.0 seconds |
| `293` | Espionage | `Espionage` | `[Espionage` | `20` | `15` | `10` | 3.0 seconds |
| `294` | Ferret Flee | `FerretFlee` | `[FerretFlee` | `50` | `35` | `30` | 0.5 seconds |
| `295` | Monkey Leap | `MonkeyLeap` | `[MonkeyLeap` | `40` | `25` | `20` | 0.5 seconds |
| `296` | Mystic Shuriken | `MysticShuriken` | `[MysticShuriken` | `60` | `45` | `40` | 1.0 second |
| `297` | Tiger Strength | `TigerStrength` | `[TigerStrength` | `90` | `75` | `70` | 3.0 seconds |

The required values are returned as strings by `ShinobiScroll.ShinobiInfo()` and parsed by each spell's `RequiredSkill`, `RequiredTithing`, and `RequiredMana` properties.

## Learning And Unlock Items
The constructable `ShinobiScroll` starts unbound with no learned abilities. On first double-click, an unowned scroll binds `owner = from`. If the player already owns another `ShinobiScroll`, the existing owned scroll is moved to the player's backpack, the new scroll is deleted, and the player receives the duplicate-scroll message.

The scroll must be in the owner's backpack to open. Learned abilities are stored as integer flags named after the ability, and `AddNameProperties()` displays the sum as "`N` Abilities" plus the owner name.

To learn an ability, a `SummonItems` quest item with an exact matching `Name` is dropped onto the `ShinobiScroll`. On success the matching learned flag is set to `1`, the item is deleted, the gump closes, and the player is told which ability was learned.

| Ability | Sosaria unlock item | Sosaria location | Lodoria unlock item | Lodoria location |
| --- | --- | --- | --- | --- |
| Cheetah Paws | `chest of suffering` | `the Ancient Pyramid` | `egg of the harpy hen` | `Dungeon Covetous` |
| Deception | `braclet of war` | `Dungeon Clues` | `face of the ancient king` | `the Lodoria Catacombs` |
| Eagle Eye | `stump of the ancients` | `Dardin's Pit` | `wand of Talosh` | `Dungeon Deceit` |
| Espionage | `dark blood` | `Dungeon Doom` | `head of Urg` | `Dungeon Despise` |
| Ferret Flee | `firescale tooth` | `the Fires of Hell` | `crown of Vorgol` | `the City of Embers` |
| Monkey Leap | `ichor of Xthizx` | `the Mines of Morinia` | `claw of Saramon` | `Dungeon Hythloth` |
| Mystic Shuriken | `heart of a vampire queen` | `the Perinian Depths` | `horn of the frozen hells` | `the Ice Fiend Lair` |
| Tiger Strength | `hourglass of ages` | `the Dungeon of Time Awaits` | `elemental salt` | `Dungeon Shame` |

## Casting And Resource Rules
`ShinobiSpell` uses `SkillName.Ninjitsu` for both `CastSkill` and `DamageSkill`, keeps hands uncleared on cast, and uses `CastRecoveryBase = 2`. Its cast skill range is `RequiredSkill` through `RequiredSkill + 30.0`.

The shared cast gate checks the base spell gate, required tithing points, required Ninjitsu, scaled mana, and, for `CheetahPaws`, no-mount-region restrictions. The shared fizzle gate can waive tithing when `LowerRegCost > Utility.Random(100)`, then subtracts tithing before calling the base fizzle check.

`ScaleMana()` applies Mind Rot and Lower Mana Cost. Lower Mana Cost is capped at 40 percent by the base `Spell` implementation.

## Scroll And Quickbar Gumps
`ShinobiScrollGump` has ten logical pages:

| Page | Behavior |
| ---: | --- |
| `1` | Introductory help text, including the direct player commands. |
| `2` | Opens the horizontal bar with button `21`, opens the vertical bar with button `22`, or closes both bars with button `23`. |
| `3` | Cheetah Paws page for spell ID `290`. |
| `4` | Deception page for spell ID `291`. |
| `5` | Eagle Eye page for spell ID `292`. |
| `6` | Espionage page for spell ID `293`. |
| `7` | Ferret Flee page for spell ID `294`. |
| `8` | Monkey Leap page for spell ID `295`. |
| `9` | Mystic Shuriken page for spell ID `296`. |
| `10` | Tiger Strength page for spell ID `297`. |

On ability pages, learned abilities display a clickable ability icon. Unlearned abilities display the same icon as a static image and show the two required quest-item/location options. The row and column quickbars are non-closable draggable gumps that render only the learned ability buttons; pressing a button calls `DoShinobi()` and then reopens the same bar if the scroll still belongs to the Mobile.

## Ability Mechanics

### Cheetah Paws
Cheetah Paws refuses mounted casters, `BootsofHermes`, `Artifact_BootsofHermes`, `Artifact_SprintersSandals`, and racial `HikingBoots`. On success it applies `SpeedControl.MountSpeed`, records the caster in `TableCheetahPaws`, adds `BuffIcon.CheetahPaws`, and starts a timer for `Ninjitsu.Value * 5` seconds. Recasting while already under the effect removes the existing effect before starting a new one.

The shared `ShinobiSpell` gate also blocks Cheetah Paws in configured no-mount regions. `PlayerMobile` removes the Cheetah Paws speed effect when entering such a region.

### Deception
Deception is a disguise effect. It refuses faction sigil carriers, active incognito, active Deception, existing disguise timers, polymorph conflicts, and mismatched race body mods.

On success, the caster begins a `Deception` action, stops any existing disguise timer, and changes identity:

| Caster state | Body/name behavior |
| --- | --- |
| `RaceID != 0` | Sets `HueMod = 0`, chooses `BodyMod` from `593`, `597`, or `598`, and chooses a dwarf name. |
| `RaceID == 0` | Picks a random race skin hue, male/female name, hair, facial hair, and hair hues through the caster's `Race` object when available. |

The duration is `((6 * Ninjitsu.Fixed) / 50) + 1` seconds, capped at `144` seconds. When the timer expires, player hair mods are reset for non-race players, `BodyMod`, `HueMod`, and `NameMod` are cleared, `RaceBody()` is called, the `Deception` action ends, and armor/clothing validation runs.

### Eagle Eye
Eagle Eye targets a visible point at range `10` on ML or `12` otherwise. Its item scan radius is `1 + (int)(Ninjitsu.Value / 20.0)`.

The item scan reports nearby `BaseTrap` instances, specific hidden `BaseDoor` item IDs, and `HiddenTrap` items. It also searches `HiddenChest` markers. For each hidden chest marker, a level is chosen from `1` through `min(6, (int)(Ninjitsu.Value / 16))`. Two out of three rolls do nothing. On the remaining roll, level `5` or `6` creates a `HiddenBox` of level `1` or `2`; lower levels create `Gold` worth `Utility.RandomMinMax(50, 100) * level`. The hidden marker item is then deleted.

After the item scan, Eagle Eye scans hidden Mobiles in the same radius. Mobiles are revealed when they are hidden, are players or lower-access staff than the caster, and pass the difficulty check.

The reveal chance is:

| Target state | Compiled result |
| --- | --- |
| Target has `InvisibilitySpell` timer | Always succeeds. |
| Target `Hiding.Fixed + Stealth.Fixed > 0` | `50 * (caster.Ninjitsu.Fixed + caster.Searching.Fixed) / (target.Hiding.Fixed + target.Stealth.Fixed) > Utility.Random(100)`. |
| Target has no Hiding or Stealth fixed value | Chance is treated as `100 > Utility.Random(100)`. |

If nothing is found, the caster plays the fizzle sound, says `*groans*`, and receives "Your don't notice anything."

### Espionage
Espionage targets within range `2`. It plays unlock sound `0x241`, then handles the target type:

| Target | Behavior |
| --- | --- |
| `Mobile` | Reports that it did not need to be unlocked. |
| `BaseHouseDoor` | Refuses; says the ability is for certain containers and other doors. |
| `BookBox` | Refuses; says the cursed box can never be unlocked. |
| `UnidentifiedArtifact`, `UnidentifiedItem`, `CurseItem` | Refuses; says the ability is used to unlock containers. |
| Dungeon `BaseDoor` | If locked, sets `Locked = false` and calls `DoorType.UnlockDoors()`. If already unlocked, reports no unlock needed. |
| Other `BaseDoor` | Reports no unlock needed. |
| Non-`LockableContainer` | Sends localized "You can't unlock that!" |
| Secure house container | Refuses. |
| Unlocked `LockableContainer` | Reports no unlock needed. |
| `LockLevel == 0` | Sends localized "You can't unlock that!" |
| Other `LockableContainer` | Computes `level = min(50, (int)Ninjitsu.Value + 20)`. If `level >= RequiredSkill` and the target is not a `TreasureMapChest` above level 2, unlocks the container and resets `LockLevel` from `-255` to `RequiredSkill - 10`. Otherwise it says the ability has no effect. |

### Ferret Flee
Ferret Flee only tries to work while the caster is paralyzed or frozen. It uses `CheckBSequence(Caster)`, then rolls:

`(int)Ninjitsu.Value > Utility.Random(120)`

On success, it plays the success sound, clears `Paralyzed` and `Frozen`, cleans buff icons, and says "You freed yourself!" On failure, it plays the failure sound and says "You failed to free yourself!"

### Monkey Leap
Monkey Leap targets a point at range `11` on ML or `12` otherwise. The pre-cast gate refuses overloaded casters and checks `TravelCheckType.TeleportFrom`. The target path repeats overload and travel checks, checks `TeleportTo`, requires `map.CanSpawnMobile()`, refuses multis through `SpellHelper.CheckMulti()`, and then calls `CheckSequence()`.

On success, player pets are teleported with the caster, the caster turns toward the original target, particles and sound play at the origin, the caster's `Location` is assigned to the destination, `ProcessDelta()` runs, and the same particles and sound play again at the destination.

### Mystic Shuriken
Mystic Shuriken is a delayed-damage harmful target spell. It targets a visible Mobile at range `10` on ML or `12` otherwise, calls `CheckHSequence()`, turns toward the target, sends moving particles item `0x27AC`, plays sound `0x5D2`, and deals `100` percent physical spell damage.

Damage uses the shared `Spell.GetNewAosDamage(40, 1, 5, target)` formula. The base roll is `Utility.Dice(1, 5, 40)` before shared spell-damage scaling, Int bonus, Inscribe bonus, `SpellDamage` item bonus caps, and Ninjitsu-based damage scaling.

### Tiger Strength
Tiger Strength first runs the shared cast gate, then refuses casts where `Caster.Followers + 2 > Caster.FollowersMax`. On success it summons a `SummonedTiger` for `Ninjitsu.Value * 12` seconds.

`SummonedTiger` is a melee `BaseCreature` with `WeaponAbility.BleedAttack`, body `340`, hue `0x54F`, `ControlSlots = 2`, `DeleteCorpseOnDeath = true`, `DispelDifficulty = 117.5`, `DispelFocus = 45.0`, 200 Strength, 70 Dexterity, 70 Intelligence, 180 Hits, 14-21 damage, 65-75 physical resistance, 40-50 elemental resistances, 65.0 MagicResist, 100.0 Tactics, 90.0 FistFighting, and 34 virtual armor.

## Serialization
`ShinobiScroll.Serialize()` writes version `1`, then the eight learned flags, `Page`, and `owner`. `Deserialize()` reads those values in that order, but it does not branch on the `version` value.

`SummonedTiger` uses standard version `0` serialization with no custom fields.

## Known Issues
| Issue | Impact |
| --- | --- |
| `ShinobiSpell.CheckFizzle()` subtracts mana itself, then successful `Spell.CheckSequence()` subtracts mana again. Because `ShinobiSpell.GetMana()` also returns already-scaled mana, the second base subtraction scales the mana value a second time. | Successful Shinobi casts can charge more mana than the displayed base cost and interact oddly with Lower Mana Cost. |
| `ShinobiSpell.CheckFizzle()` subtracts tithing before `base.CheckFizzle()` returns. | A normal spell fizzle can still consume tithing points. |
| `ShinobiScroll.GetShinobi()` assumes `m.Backpack` is not null. | Direct command casting can null-reference if invoked by a Mobile without a backpack. |
| `ShinobiScroll.OnDragDrop()` does not verify that `from` is the scroll owner before setting learned flags. | A reachable scroll can be modified by a non-owner dropping a matching `SummonItems` item. |
| `ShinobiScroll.Deserialize()` reads the version but always consumes the current version-1 field layout. | Any older save layout with fewer or different fields would not be handled safely. |
| `CheetahPaws` starts a new timer on each successful cast but does not store or stop old timers. | Recasting before the old timer expires can let the old timer remove the refreshed speed effect early. |
| `Espionage.InternalTarget` checks `this.GetType()` against `TreasureMapChest`, `ParagonChest`, and `PirateChest` instead of checking the targeted container. | The explicit Paragon/Pirate/Treasure branch messages are unreachable; only the later high-level treasure-map chest check still applies. |
| `Deception.CheckCast()` and `MonkeyLeap.CheckCast()` do not call `base.CheckCast()`. | Those abilities can enter targeting/cast delay without the shared Shinobi pre-checks; the later sequence still performs shared spending and failure checks. |
| Eagle Eye's failure text says "Your don't notice anything." | Player-facing typo. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0106.
- Backlog rows: RB-06767.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Magic/Shinobi/ShinobiSpell.cs (CurrentFile)
- Data/Scripts/Magic/Shinobi/ShinobiCommandList.cs (CurrentFile)
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs (CurrentFile)
- Data/Scripts/Magic/Shinobi/Spells/ (CurrentDirectory)

### Runtime Evidence

- Hook summary: Command=1; Gump=9; Initialize=1; Timer=2.
- Data/Scripts/Magic/Shinobi/ShinobiCommandList.cs:L15 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Magic/Shinobi/ShinobiCommandList.cs:L58 Command CommandSystem.Register access=Unknown
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L204 Gump SendGump access=Internal
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L1073 Gump OnResponse access=Internal
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L1097 Gump SendGump access=Internal
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L1102 Gump SendGump access=Internal
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L1119 Gump SendGump access=Internal
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L1182 Gump OnResponse access=Internal
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L1189 Gump SendGump access=Internal
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L1252 Gump OnResponse access=Internal
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:L1259 Gump SendGump access=Internal
- Data/Scripts/Magic/Shinobi/Spells/CheetahPaws.cs:L117 Timer CustomTimerSubclass access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 1.
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs:Server.Items.ShinobiScroll version=1 serialize=L431 deserialize=L447 alignment=CountMatchNeedsTypeReview:UnknownWrites=9

### Project And Runtime Coverage

- Data/Scripts/Magic/Shinobi/ShinobiCommandList.cs=Keep
- Data/Scripts/Magic/Shinobi/ShinobiCommandList.cs=Keep
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs=Keep
- Data/Scripts/Magic/Shinobi/ShinobiScroll.cs=Keep
- Data/Scripts/Magic/Shinobi/ShinobiSpell.cs=Keep
- Data/Scripts/Magic/Shinobi/ShinobiSpell.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/CheetahPaws.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/CheetahPaws.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/Deception.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/Deception.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/EagleEye.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/EagleEye.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/Espionage.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/Espionage.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/FerretFlee.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/FerretFlee.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/MonkeyLeap.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/MonkeyLeap.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/MysticShuriken.cs=Keep
- Data/Scripts/Magic/Shinobi/Spells/MysticShuriken.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
