# System Name: Clone Offline Player Characters

## Overview

Clone Offline Player Characters is a custom `EventSink`-driven system under `Server.Custom.Confictura.CloneOfflinePlayerCharacters`. Its intended runtime behavior is to leave a combat-capable `BaseCreature` replica (`CharacterClone`) in the world when a normal player logs out, then delete that clone when the original player logs back in.

The clone is not just a visual placeholder. It copies the original player's appearance, base stats, skill bases, equipped items, backpack contents, and current mount. It uses `OmniAI`, can fight, can teach through the normal RunUO teaching flow, and can be hired by another player for gold.

Code-Verified: 2026-05-07

## Script Inventory

| Script | Namespace | Role |
| --- | --- | --- |
| `Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters/CloneOfflinePlayerCharacters.cs` | `Server.Custom.Confictura.CloneOfflinePlayerCharacters` | Entry point, `EventSink.Login`/`EventSink.Logout` hooks, clone creation/deletion, startup/manual synchronization. |
| `Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters/CharacterClone.cs` | `Server.Custom.Confictura.CloneOfflinePlayerCharacters` | `BaseCreature` replica of a `PlayerMobile`; combat, paperdoll display, hiring, skill teaching, death handling, and serialization. |
| `Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters/CloneThings.cs` | `Server.Custom.Confictura.CloneOfflinePlayerCharacters` | Reflection-based helper for cloning mobile state, equipped items, backpack trees, and mounts. |
| `Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters/BackpackClone.cs` | `Server.Custom.Confictura.CloneOfflinePlayerCharacters` | `Backpack` subclass used for cloned pack contents and access denial messaging. |
| `Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters/MountClone.cs` | `Server.Custom.Confictura.CloneOfflinePlayerCharacters` | `BaseMount` subclass copied from the original player's current mount. |
| `Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters/EtherealMountClone.cs` | `Server.Custom.Confictura.CloneOfflinePlayerCharacters` | `EtherealMount` subclass copied from the original player's current ethereal mount. |
| `Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters/CheckClonesCommand.cs` | `Server.Custom.Confictura.CloneOfflinePlayerCharacters` | Administrator command wrapper around startup clone synchronization. |

`Data/Scripts/Scripts.csproj` currently lists all seven C# scripts from this folder for Visual Studio project hygiene. Live runtime script compilation still comes from recursive `.cs` discovery under `Data/Scripts`.

## Administrator Surface

| Command | Access | Usage attribute | Description attribute | Parameters | Behavior |
| --- | --- | --- | --- | --- | --- |
| `[CheckClones` | `Administrator` | `CheckClones` | `Creates clones of all offline players who don't currently have one.` | None | Calls `CloneOfflinePlayerCharacters.CheckFirstRun()` and sends `Clone check initiated.` to the command caller. |

The system does not define a custom `Gump`, packet handler, XMLSpawner attachment, or configurable persistence item. Runtime behavior is driven by static initialization plus the command above.

## Clone Eligibility

The normal logout path creates a clone only when all of these checks pass:

| Check | Required value |
| --- | --- |
| `e.Mobile` type | Not a `CharacterClone` and castable to `PlayerMobile`. |
| Player state | `Alive == true`. |
| Access level | Exactly `AccessLevel.Player`. |
| Region filter | Not in `StartRegion`, `PublicRegion`, `CrashRegion`, `PrisonArea`, or `SafeRegion`. |

The public `CreateCloneOf(Mobile mobile)` helper uses the same eligibility checks.

## Lifecycle

### Initialization

`CloneOfflinePlayerCharacters.Initialize()` subscribes to `EventSink.Logout` and `EventSink.Login`, then immediately calls `CheckFirstRun()`.

### Logout

When an eligible player logs out, the system:

1. Creates a `CharacterClone` with the original `Mobile`.
2. Copies core mobile properties and skill bases.
3. Copies equipped items except the live backpack.
4. Creates a `BackpackClone` and recursively copies the original backpack contents.
5. Creates a `MountClone` or `EtherealMountClone` if the player was mounted.

### Login

On login, `DeleteClonesOf(e.Mobile)` scans `World.Mobiles.Values` and deletes every `CharacterClone` whose `Original` reference equals the logging-in mobile. If that clone is mounted, its cloned mount rider is cleared and the cloned mount is deleted before the clone mobile is deleted.

### Startup and Manual Synchronization

`CheckFirstRun()` performs a world-wide synchronization pass:

1. Builds a map of existing `CharacterClone` instances keyed by their original `PlayerMobile`.
2. For each existing clone with `RaceID != 0`, clears `HueMod`.
3. For every alive player-access `PlayerMobile` without a clone, moves the player to `LogoutMap`/`LogoutLocation`, nudges `X` by `+1` then `-1`, and counts it if the resulting region is valid.
4. Repeats the alive player-access scan and calls `CreateCloneOf()` for each player without a clone.
5. Deletes clones found in start, public, crash, prison, or safe regions.
6. Moves every alive player-access `PlayerMobile` back to `LogoutLocation` and `Map.Internal`.

## `CharacterClone` Mobile

`CharacterClone` inherits `BaseCreature` and constructs its base AI settings as:

| BaseCreature constructor argument | Value |
| --- | --- |
| `AIType` | `AI_Melee` |
| `FightMode` | `Aggressor` |
| `RangePerception` | `18` |
| `RangeFight` | `CalculateRangeFight(original)` |
| `ActiveSpeed` | `0.2` |
| `PassiveSpeed` | `0.3` |

Additional clone behavior:

| Member | Behavior |
| --- | --- |
| `ForcedAI` | Returns `new OmniAI(this)`. |
| `GetMaxResistance(ResistanceType type)` | Always returns `70`. |
| `CanRegenHits` | Returns `true`. |
| `CanTeach` | Returns `true`. |
| `OnDoubleClick(Mobile from)` | Displays the clone paperdoll, then calls base behavior. |
| `GetProperties(ObjectPropertyList list)` | Delegates to `Original.GetProperties(list)` when `Original` is not null. |
| `FinalizeClone()` | Calculates hire pay, sets hit/stamina/mana maxima from doubled Str/Dex/Int, recalculates damage, fills Hits/Stam/Mana, sets `ControlSlots = 4`, and moves from `Map.Internal` to `LogoutMap` if needed. |
| `OnThink()` | Recalculates fight range and base damage every think cycle. |
| `OnBeforeDeath()` | Deletes a cloned mount if the clone is mounted. |
| `OnDeath(Container container)` | Recursively sets all corpse-container items to `Movable = false`. |

## Fight Range Formula

`CalculateRangeFight(Mobile original)` starts with `maxRangeFight = 1` and inspects the original player's one-handed or two-handed layer item.

| Condition | Result |
| --- | --- |
| `Marksmanship >= meleeSkillSum`, `Marksmanship >= Magery`, and equipped weapon is `BaseRanged` | `RangeFight = 10` |
| `meleeSkillSum >= Marksmanship`, `meleeSkillSum >= Magery`, and equipped weapon is `BaseMeleeWeapon` | `RangeFight = 1` |
| Otherwise, if `Magery > 0.0` | `RangeFight = 9` |
| All other cases | `RangeFight = 1` |

`meleeSkillSum` is:

```text
Swords + Bludgeoning + Fencing + Bushido + Ninjitsu + Knightship
```

## Damage Formula

`CalculateBaseDamage()` reads skills and strength from `Original`, then applies RunUO-style weapon damage bonuses to the clone's equipped weapon.

| Component | Formula below 100 | Formula at 100 or higher |
| --- | --- | --- |
| Tactics bonus | `tactics / 1.6` | `(tactics / 1.6) + 6.25` |
| Anatomy bonus | `anatomy / 2` | `(anatomy / 2) + 5` |
| Lumberjacking bonus | `lumberjacking / 5` | `(lumberjacking / 5) + 10` |
| Strength bonus | `strength * 0.3` | `(strength * 0.3) + 5` |

```text
totalDamageBonus = tacticsBonus + anatomyBonus + lumberjackingBonus + strengthBonus
minFinalDamage = (int)(minBaseDamage + (minBaseDamage * totalDamageBonus / 100))
maxFinalDamage = (int)(maxBaseDamage + (maxBaseDamage * totalDamageBonus / 100))
```

If the clone has a `BaseWeapon` equipped, the formula uses that weapon's `MinDamage` and `MaxDamage`. Otherwise it falls back to `5` through `10`.

## Property and Skill Cloning

`CloneMobileProperties(source, target)` copies:

| Category | Copied values |
| --- | --- |
| World state | `Location`, `Map`, `Direction`; clone `Hidden` is forced to `false`. |
| Stats and fame | `Dex`, `Int`, `Str`, `Fame`, `Karma`. |
| Identity | `NameHue`, `SpeechHue`, `Criminal`, `Name`, `Title`, `Female`, `Body`, `BodyValue`, `Hue`. |
| Resources | `Hits`, `Stam`, and `Mana`, clamped against the source maximum values. |
| Needs | `Hunger`, `Thirst`. |
| Followers | `FollowersMax`, `Followers`. |
| Hair | `HairItemID`, `FacialHairItemID`, `HairHue`, `FacialHairHue`. |
| Skills | Every `Skills[i].Base` value. |

If the target is a `CharacterClone`, `FinalizeClone()` runs after these values are copied.

## Item, Backpack, and Mount Cloning

Equipped items are cloned from `source.Items`, excluding the source `Backpack`. Non-stackable items with `Amount > 1` are split into one cloned item per amount.

Backpacks are cloned by adding a new `BackpackClone` to the target and recursively cloning every item from the original backpack container tree. Nested containers are recursively copied when both the source and cloned item are containers.

`CloneItem(Item item)` has two construction paths:

| Item type | Construction path |
| --- | --- |
| `StaffFiveParts` | Uses the item's concrete type constructor with `Staff_Owner` and `Staff_Magic`, then copies properties. |
| Other items | Requires a public parameterless constructor, then copies writable public properties. |

Property copying skips `Parent`, `TotalWeight`, `TotalItems`, and `TotalGold`, then calls `item.OnAfterDuped(newItem)` and sets the clone's `Parent = null`. It also recursively copies attribute objects on `BaseWeapon`, `BaseArmor`, `BaseJewel`, and `BaseClothing`.

Mount cloning runs only when `source.Mounted` is true:

| Source mount type | Clone type | Behavior |
| --- | --- | --- |
| `BaseMount` | `MountClone` | Calls the `BaseMount` constructor with the original mount's name, body, item, AI, fight mode, perception/fight ranges, and speeds; then copies public writable `BaseMount` properties except `Rider`. |
| `EtherealMount` | `EtherealMountClone` | Calls the `EtherealMount` constructor with the original regular/mounted IDs; then copies public writable `EtherealMount` properties except `Rider`. |

The cloned mount's `Rider` is set to the clone mobile.

## Hiring and Teaching

`CharacterClone` exposes hire behavior through context-menu and drag/drop interactions.

### Hire Price

`Payday(CharacterClone clone)` sums these skill values, casts each to `int`, then multiplies the total by `4`:

| Included skills |
| --- |
| Anatomy, Tactics, Bludgeoning, Swords, Fencing, Marksmanship, MagicResist, Healing, Magery, Parry, Bushido, Knightship, Necromancy, Ninjitsu, Spiritualism, Psychology, Stealth, Hiding |

```text
m_Pay = 4 * sum((int)clone.Skills[listedSkill].Value)
```

### Hire Flow

When a player drops `Gold` on an uncontrolled clone:

1. The clone first tries normal RunUO teaching if `CheckTeachingMatch(from)` is true.
2. If teaching does not consume the gold, the clone requires `item.Amount >= m_Pay`.
3. The clone rejects the hire if the same player already has a live hire in the static `HireTable`.
4. `AddHire(from)` calls `SetControlMaster(from)` and sets `IsHired = true`.
5. The entire dropped gold item is placed into `Original.BankBox`.
6. `m_HoldGold` is increased by the dropped gold amount.
7. A `PayTimer` starts on a 60-minute interval.

The clone tells the hirer it will work for `(int)item.Amount / m_Pay` hours.

### Pay Timer

Each timer tick:

| Condition | Result |
| --- | --- |
| `HoldGold <= Pay` and the clone still has an owner | Says a departure message, clears `ControlMaster`, sets `IsHired = false`, and removes the owner from `HireTable`. |
| `HoldGold > Pay` | Subtracts `Pay` from `HoldGold`. |

## Cross-System Protections

Other scripts include clone-specific guards:

| Script | Guard |
| --- | --- |
| `Data/Scripts/System/Skills/Stealing.cs` | Blocks stealing when the root parent is `CharacterClone`. |
| `Data/Scripts/System/Misc/Logs.cs` | Labels clone killers as `clone of <name>` in death logging. |
| `Data/Scripts/Mobiles/Base/Behavior.cs` | Skips `DropBackpack()` when releasing a `CharacterClone`. |
| `Data/Scripts/Custom/NPC Control/CloneCommands.cs` | Rejects `CharacterClone` and `MountClone` as shared command targets, and treats clone artifact items as control artifacts. |
| `Data/Scripts/Custom/NPC Control/StaffCommands/CloneMe.cs` | Reuses `BackpackClone` and `CloneThings.CloneItem()` while filtering system clone items and clone mounts. |

## Serialization

| Class | Serialized version | Custom serialized data |
| --- | --- | --- |
| `CharacterClone` | `0` | `Original` as a `Mobile`. On deserialize, a clone with `Original == null` deletes itself. |
| `BackpackClone` | `0` | No custom fields. |
| `MountClone` | `0` | No custom fields. |
| `EtherealMountClone` | `0` | No custom fields. |

`CharacterClone` does not serialize `m_Pay`, `m_IsHired`, `m_HoldGold`, `m_PayTimer`, or the static `HireTable`.

## Known Issues

* `CheckFirstRun()` does not check `PlayerMobile.NetState` or any other offline marker. When `[CheckClones` is run while players are online, it can create clones for online players and then move every alive player-access `PlayerMobile` to `Map.Internal`.
* `CharacterClone.OnThink()` and `CalculateBaseDamage()` dereference `Original` without a null/deleted guard. Deserialization deletes clones whose `Original` is already null, but a GM property edit, deleted original, or broken reference after load can still leave runtime null-reference paths.
* Hire accounting is not restored after world load. The clone writes only `Original`; pay rate, held gold, hire flag, pay timer, and `HireTable` are runtime-only, so an already-controlled clone can reload without a restarted pay timer.
* `CloneThings.CloneItem()` silently swallows cloning exceptions. Unsupported or broken item types disappear from the clone without logging, which makes equipment/backpack copy failures hard to diagnose.
* `MountClone` and `EtherealMountClone` copy writable properties through reflection without a try/catch. A throwing mount property setter can abort clone creation during logout or synchronization.
