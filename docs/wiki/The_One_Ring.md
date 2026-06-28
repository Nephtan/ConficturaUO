# The One Ring

## Overview

The One Ring is a constructable cursed `BaseRing` artifact implemented by `Server.Items.TheOneRing`. It only equips for `PlayerMobile` instances. A successful equip consumes one `Charges` value, hides the player, gives a randomized `AllowedStealthSteps` value, may directly subtract hit points, and may spawn one or two hostile `Server.Mobiles.RingWraith` mobiles at the player's current location.

There are no custom `CommandSystem.Register` entries, `Gump` classes, `NetState` handlers, `EventSink` hooks, XMLSpawner attachments, or scripted loot sources in the traced package. Staff administration uses normal RunUO constructable-item tooling.

Code-Verified: 2026-05-07

## Source

| Script | Type | Role |
| --- | --- | --- |
| `Data/Scripts/Custom/LOTR - The One Ring/TheOneRing.cs` | `Server.Items.TheOneRing` | Cursed ring item with equip-triggered stealth, damage, charge use, and `RingWraith` spawning. |
| `Data/Scripts/Custom/LOTR - The One Ring/RingWraith.cs` | `Server.Mobiles.RingWraith` | Hostile melee `BaseCreature` spawned by the ring. |

Both scripts are explicitly included by `Data/Scripts/Scripts.csproj`.

## Constructable Types

| Type | Constructable | Base Type | ItemID / Body | Namespace |
| --- | --- | --- | ---: | --- |
| `TheOneRing` | Yes | `BaseRing` | `0x108A` | `Server.Items` |
| `RingWraith` | Yes | `BaseCreature` | `400` | `Server.Mobiles` |

## TheOneRing Item

| Property | Value / Behavior |
| --- | --- |
| Name | `The One Ring` |
| Weight | `1` |
| Layer | Inherited from `BaseRing`, which constructs as `Layer.Ring`. |
| LootType | `Cursed` |
| Initial `Charges` | `Utility.RandomMinMax(2, 4)` |
| Staff property | `Charges`, exposed with `[CommandProperty(AccessLevel.GameMaster)]` |

The script does not override object property display, so the custom charge count is available through the command property system rather than a custom item tooltip row.

## Equip Entry Point

`TheOneRing.OnEquip(Mobile from)` is the only runtime entry point in the item script.

| Check / Step | Compiled Behavior |
| --- | --- |
| Mobile type | Returns `false` unless `from is PlayerMobile`. |
| Charge gate | The effect block only runs when `Charges >= 1`. |
| Damage roll | `Utility.Random(3)` has one handled case, so one equip attempt has a `1 / 3` chance to subtract `15..30` from `from.Hits`. |
| Damage message | The player receives `You feel pain surge throughout your body...` only when the damage case runs. |
| Hide pass 1 | Sets `from.Hidden = true`; sets `from.AllowedStealthSteps = Utility.RandomMinMax(10, 30)`. |
| Wraith roll 1 | Independent `Utility.Random(5)` case `0`; on success creates `RingWraith`, moves it to `from.Location`/`from.Map`, and sets `mob.Combatant = from`. |
| Hide pass 2 | Repeats `from.Hidden = true`; assigns a second `Utility.RandomMinMax(10, 30)` stealth-step value. This second assignment is the final stored value. |
| Wraith roll 2 | Repeats the independent `Utility.Random(5)` case `0` spawn path. |
| Charge decrement | Subtracts one charge after all effect rolls. |
| Return value | Returns `true` after a charged effect block; otherwise falls through to `false`. |

If a ring has exactly one charge, the equip succeeds, the effect block runs, and `Charges` becomes `0`. The item is not deleted, consumed, or force-removed.

## Equip Probabilities

Each successful charged equip attempt uses the following effective probabilities.

| Outcome | Formula | Chance |
| --- | ---: | ---: |
| Direct hit-point loss | `1 / 3` | `33.33%` |
| No direct hit-point loss | `2 / 3` | `66.67%` |
| First wraith roll succeeds | `1 / 5` | `20%` |
| Second wraith roll succeeds | `1 / 5` | `20%` |
| No wraiths spawn | `(4 / 5) * (4 / 5)` | `64%` |
| Exactly one wraith spawns | `(1 / 5 * 4 / 5) + (4 / 5 * 1 / 5)` | `32%` |
| Two wraiths spawn | `(1 / 5) * (1 / 5)` | `4%` |
| At least one wraith spawns | `1 - (4 / 5)^2` | `36%` |

## RingWraith Mobile

`RingWraith` is a melee `BaseCreature` constructed with `AIType.AI_Melee`, `FightMode.Closest`, range perception argument `10`, range fight `1`, active speed `0.2`, and passive speed `0.4`. Core `BaseCreature` remaps a perception argument of `10` to its default perception range of `16`.

| Attribute | Value / Range |
| --- | ---: |
| Name | `A Ring Wraith` |
| CorpseName | `a ring wrath` |
| Body | `400` |
| Hue | `1` |
| Strength | `250..300` |
| Dexterity | `250..300` |
| Intelligence | `250..300` |
| Hits | `200..300` |
| Base damage | `8..18` |
| Direction | Random value from `0..7` |

### Equipment

| Item | Construction | Movable |
| --- | --- | --- |
| `HoodedShroudOfShadows` | Created with its default constructor and added to the mobile. | `false` |
| `Longsword` | Created with its default constructor and added to the mobile. | `false` |

### Damage Types

The constructor calls `SetDamageType(ResistanceType type, int min, int max)`, which stores one random value in the matching damage-type property.

| Type | Random Range |
| --- | ---: |
| Physical | `40..80` |
| Fire | `40..80` |
| Poison | `40..80` |
| Cold | Not assigned by this script |
| Energy | Not assigned by this script |

`BaseCreature` starts physical damage at `100`, but `RingWraith` overwrites physical damage with a random `40..80` value. Fire and poison are also assigned random `40..80` values. The script does not normalize these damage-type values to total `100`.

### Resistances

| Resistance | Random Range |
| --- | ---: |
| Physical | `40..80` |
| Cold | `40..80` |
| Fire | `40..80` |
| Energy | `40..80` |
| Poison | `40..80` |

### Skills

| Skill | Random Range |
| --- | ---: |
| `MagicResist` | `100.0..120.0` |
| `Tactics` | `100.0..120.0` |
| `Swords` | `100.0..120.0` |

### Combat Overrides

| Override | Behavior |
| --- | --- |
| `AlwaysMurderer` | Always returns `true`. |
| `AutoDispel` | Always returns `true`. |
| `BardImmune` | Always returns `true`. |
| `Uncalmable` | Always returns `true`. |
| `Unprovokable` | Always returns `true`. |
| `GetWeaponAbility()` | Randomly returns `ArmorIgnore`, `Disarm`, or `Dismount` with equal `1 / 3` selection. |
| `OnGaveMeleeAttack(Mobile defender)` | Calls base, then applies extra direct `defender.Damage(Utility.Random(5, 10), this)`, subtracts `Utility.Random(5, 10)` stamina, and subtracts `Utility.Random(5, 10)` mana. |

`Utility.Random(5, 10)` uses the RunUO overload with minimum `5` and count `10`, so the extra damage, stamina loss, and mana loss rolls are `5..14`, not `5..10`.

## Loot and Rewards

`RingWraith` does not override `GenerateLoot()`, does not call `PackGold`, and does not call `PackItem` except for the non-movable worn shroud and longsword. `BaseCreature` calls `GenerateLoot(true)` during construction, but the base `GenerateLoot()` method is empty unless a subclass overrides it.

The script does not assign `Fame`, `Karma`, `VirtualArmor`, `Tamable`, `ControlSlots`, `Summoned`, or a timed cleanup rule.

## Administration

| Task | In-Game Administration |
| --- | --- |
| Create the ring | Use normal RunUO constructable item tooling for `TheOneRing`, such as the standard add command available to staff. |
| Create a wraith manually | Use normal RunUO constructable mobile tooling for `RingWraith`. |
| Inspect or adjust charges | Use the `Charges` command property on `TheOneRing`; it is exposed at `AccessLevel.GameMaster`. |

There are no package-specific `[Usage]` or `[Description]` command attributes because the package does not register custom commands.

## Serialization

### TheOneRing

`TheOneRing.Serialize(GenericWriter writer)` calls `base.Serialize(writer)` and writes version `0`. It does not write `i_charges` or any other custom field.

`TheOneRing.Deserialize(GenericReader reader)` calls `base.Deserialize(reader)`, reads the version integer, then assigns `Charges = Utility.RandomMinMax(1, 4)`.

| Version | Serialized Payload After Version | Load Behavior |
| ---: | --- | --- |
| `0` | None | Loads with a new random `Charges` value from `1..4`. |

### RingWraith

`RingWraith.Serialize(GenericWriter writer)` calls `base.Serialize(writer)` and writes version `0`. `Deserialize(GenericReader reader)` reads only that version integer after `base.Deserialize(reader)`.

| Version | Serialized Payload After Version |
| ---: | --- |
| `0` | None |

## Technical Trace

* Target acquisition: `docs/SystemAudit.md` listed `The One Ring` as `Missing` and pointed to `Data/Scripts/Custom/LOTR - The One Ring/TheOneRing.cs`.
* Entry points: `TheOneRing.OnEquip(Mobile from)` and the constructable `RingWraith` constructor are the complete custom runtime surface found for the package.
* Compile inclusion: `Data/Scripts/Scripts.csproj` includes both LOTR package scripts.
* Commands: no `CommandSystem.Register`, `[Usage]`, or `[Description]` command definitions were found in the package.
* Persistence: both classes write version `0`; only `RingWraith` has no custom fields. `TheOneRing` has a custom `Charges` field but does not persist it.

## Known Issues

* `TheOneRing` does not serialize `i_charges`. Every world load rerolls saved rings to `1..4` charges, regardless of the charge count before save.
* The no-charge feedback block is unreachable because it is placed after `return true` inside the charged branch. A `PlayerMobile` equipping a ring with `Charges < 1` receives no message and the method returns `false`.
* The equip logic duplicates the hide and wraith-spawn block. This causes two independent `20%` wraith rolls per charged equip, allowing two `RingWraith` mobiles from one charge.
* `Charges` has no setter validation. Staff can set zero or negative charges through the command property, making the ring silently unequippable for players.
* Damage is applied with `from.Hits -= Utility.RandomMinMax(15, 30)` rather than `Mobile.Damage(...)`, so the ring effect has no attacker/source in the normal damage call path.
* Spawned `RingWraith` mobiles are not marked `Summoned`, have no owner, and have no cleanup timer. If they are not killed, the script leaves them as persistent world mobiles.
* `RingWraith` assigns three independent damage-type values of `40..80`, so the total melee damage-type percentages can exceed `100`.
* The corpse name string is `a ring wrath`, which appears to be a typo for `a ring wraith`.
