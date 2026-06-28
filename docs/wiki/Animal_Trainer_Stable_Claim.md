# NPC Animal Trainer Stable/Claim

## Scope

This page documents the RunUO pet stable and claim behavior handled by `Server.Mobiles.AnimalTrainer`.
It covers animal trainer speech keywords, context-menu entries, the claim-list `Gump`, stable-slot math, stable/claim state changes, related no-mount helper methods, and persistence.

The compiled system is a `BaseVendor` `Mobile` implementation. It is not a player command system, does not register `[Command]` handlers, does not use packet handlers or EventSink hooks, and does not use XMLSpawner attachments.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Civilized/Vendors/AnimalTrainer.cs` | Main trainer `Mobile`, speech handling, context menu entries, `ClaimListGump`, slot formula, stable/claim mutations, mount helper methods, and serializer. |
| `Data/Scripts/Mobiles/Civilized/Vendors/GypsyAnimalTrainer.cs` | `AnimalTrainer` subclass with gypsy title/outfit differences; inherits stable and claim behavior. |
| `Data/System/Source/Mobile.cs` | Owns and serializes each player's `Stabled` mobile list. |
| `Data/Scripts/Mobiles/Base/BaseCreature.cs` | Provides `IsStabled`, pet control fields, loyalty, summon state, and delete-timer interaction. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Uses `AnimalTrainer.GetMaxStabled()` for automatic pet stabling and auto-claim paths. |
| `Data/Scripts/System/Misc/Settings.cs` | Supplies `MyServerSettings.ExtraStableSlots()`, clamped from `0` through `20`. |

## Mobile Definition

`AnimalTrainer` derives from `BaseVendor`, uses the title `"the animal trainer"`, and belongs to `NpcGuild.DruidsGuild`.

On construction the trainer receives these skill ranges:

| Skill | Minimum | Maximum |
| --- | ---: | ---: |
| `Druidism` | 64.0 | 100.0 |
| `Taming` | 90.0 | 100.0 |
| `Veterinary` | 65.0 | 88.0 |

`InitSBInfo()` picks the regional animal-trainer shop list from the trainer's current world or crypt state, then always adds `SBAnimalTrainer` and `SBBuyArtifacts`.

| Region/world gate | Added shop list |
| --- | --- |
| `"the Serpent Island"` | `SBGargoyleAnimalTrainer` |
| `"the Land of Lodoria"` | `SBElfAnimalTrainer` |
| `"the Isles of Dread"` | `SBBarbarianAnimalTrainer` |
| `"the Savaged Empire"` | `SBOrkAnimalTrainer` |
| `Worlds.IsCrypt(Location, Map)` | `SBDeadAnimalTrainer` |
| Fallback | `SBHumanAnimalTrainer` |

`InitOutfit()` calls the base vendor outfit setup and then equips either a `QuarterStaff` or a `ShepherdsCrook`.

## Player Interaction Surface

### Context Menus

`GetContextMenuEntries()` always adds three custom entries after base vendor entries:

| Entry class | Caller gate | Result |
| --- | --- | --- |
| `ClaimingGumpEntry` | Caller must be `PlayerMobile` | Calls `BeginClaimList()` to open the claim-list `Gump`. |
| `SpeechGumpEntry` | Caller must be `PlayerMobile`; no existing `SpeechGump` | Opens `SpeechGump(mobile, "Animal Companions", SpeechFunctions.SpeechText(..., "Pets"))`. |
| `RidingGumpEntry` | Caller must be `PlayerMobile`; no existing `Veterinarian.RidingGump` | Opens `Veterinarian.RidingGump`. |

`AddCustomContextEntries()` adds the stable entry only when the caller is alive. It also adds `ClaimAllEntry` when `from.Stabled.Count > 0`.

| Entry class | Gate | Result |
| --- | --- | --- |
| `StableEntry` | Caller alive | Calls `BeginStable()`. |
| `ClaimAllEntry` | Caller alive and `from.Stabled.Count > 0` | Calls `Claim(from)`, attempting to claim all eligible stabled pets. |

### Speech Keywords

`HandlesOnSpeech(Mobile from)` returns `true`, so nearby speech events that reach the trainer are passed to `OnSpeech()`.

| Keyword ID | Commented keyword | Speech shape | Result |
| --- | --- | --- | --- |
| `0x0008` | `*stable*` | Speech containing the encoded stable keyword | Closes any `ClaimListGump` and calls `BeginStable()`. |
| `0x0009` | `*claim*` | `claim` | Closes any `ClaimListGump` and calls `Claim(from)`. |
| `0x0009` | `*claim*` | `claim <pet name>` | Passes the substring after the first space to `Claim(from, petName)`. |

There are no bracket commands such as `[stable` or `[claim`; this is speech-keyword and context-menu driven.

## Stable Slot Formula

`GetMaxStabled(Mobile from)` uses the caller's `Taming`, `Druidism`, `Veterinary`, and `Herding` values.

First it sums the four skill values:

```text
skillSum = Taming + Druidism + Veterinary + Herding
```

Then it assigns the base stable capacity:

| `skillSum` gate | Base slots |
| ---: | ---: |
| `>= 400.0` | 7 |
| `>= 300.0` | 6 |
| `>= 240.0` | 5 |
| `>= 200.0` | 4 |
| `>= 160.0` | 3 |
| `< 160.0` | 2 |

After that, each individual skill at or above `100.0` adds:

```text
(int)((skillValue - 90.0) / 10)
```

The cast truncates fractional values toward zero. For example, a skill from `100.0` through `109.9` adds `1`, `110.0` through `119.9` adds `2`, and so on.

The final value adds `MyServerSettings.ExtraStableSlots()`. That setting returns `S_Stables` clamped to the range `0..20`. There is no final cap after the individual skill bonuses and extra-slot setting are added.

## Stable Flow

`BeginStable(Mobile from)` aborts when the trainer is deleted or `from.CheckAlive()` fails.

The trainer reports:

```text
You are currently using <from.Stabled.Count> out of <GetMaxStabled(from)> stable slots.
```

It then requires at least 30 gold in either the caller's backpack or bank box. A bank box is found with `FindBankNoCreate()`, so this check does not create a missing bank box.

| Funds state | Result |
| --- | --- |
| Backpack lacks 30 gold and bank is missing or lacks 30 gold | Trainer says cliloc `1042556`: not enough gold. |
| Backpack or bank has at least 30 gold | Caller receives cliloc `1042558`, and `from.Target` becomes a new `StableTarget`. |

`StableTarget` accepts only a `BaseCreature` target. Targeting the caller says cliloc `502672`; targeting anything else says cliloc `1048053`.

`EndStable(Mobile from, BaseCreature pet)` applies the stable gates in this order:

| Gate | Failure response |
| --- | --- |
| Trainer deleted or caller not alive | Return silently. |
| `pet.Body.IsHuman` | Cliloc `502672`: trainer is not an inn. |
| `!pet.Controlled` | Cliloc `1048053`: cannot stable that. |
| `pet.ControlMaster != from` | Cliloc `1042562`: caller does not own that pet. |
| `pet.IsDeadPet` | Cliloc `1049668`: living pets only. |
| `pet.Summoned` | Cliloc `502673`: cannot stable summoned creatures. |
| `PackLlama`, `PackHorse`, or `Beetle` with non-empty backpack | Cliloc `1042563`: unload the pet. |
| `pet.Combatant != null`, combatant within 12 tiles, and same map | Cliloc `1042564`: pet is busy. |
| `from.Stabled.Count >= GetMaxStabled(from)` | Cliloc `1042565`: too many pets in the stables. |
| Backpack and bank fail to consume 30 gold at final payment time | Cliloc `502677`: not enough bank funds. |

On success, payment is consumed from the backpack first and then from the bank if backpack payment fails. The pet is then changed as follows:

| Field/action | New value |
| --- | --- |
| `Language` | `null` |
| `ControlTarget` | `null` |
| `ControlOrder` | `OrderType.Stay` |
| Location | `Internalize()` moves the pet off-world. |
| `ControlMaster` | `null` via `SetControlMaster(null)` |
| `SummonMaster` | `null` |
| `IsStabled` | `true` |
| `Loyalty` | `BaseCreature.MaxLoyalty` when `Core.SE` is true |
| Owner list | Pet is appended to `from.Stabled`. |

The trainer then says cliloc `1049677` on AOS cores, or `502679` otherwise, and sends the updated stable-slot usage message.

## Claim-List Gump

`BeginClaimList(Mobile from)` aborts when the trainer is deleted or the caller is not alive. It scans `from.Stabled`, removes invalid entries, and builds a `List<BaseCreature>` for valid pets.

If the list is non-empty, the caller receives `ClaimListGump`. If it is empty, the trainer says cliloc `502671`: no animals stabled.

`ClaimListGump` is built at `(25, 25)`, plays sound `0x0EB`, uses `PlayerSettings.GetGumpHue(from)`, and renders each pet with a button and a bonding-status suffix.

| Pet bonding state | Display text |
| --- | --- |
| `pet.IsBonded == true` | `Bonded` |
| `pet.BondingBegin == DateTime.MinValue` | `not bonding` |
| `BondingBegin + BondingDelay` is in the future | `<days> days, <hours> hours and <minutes> minutes until it bonds` |
| Bonding delay has elapsed | `Ready to bond!` |

Clicking a pet button calls `EndClaimList(m_From, m_List[index])`. The gump path validates:

| Gate | Behavior |
| --- | --- |
| Pet is null/deleted | Return silently. |
| Caller map does not match trainer map | Return silently. |
| Caller's `Stabled` list does not contain the pet | Return silently. |
| Caller is not alive | Return silently. |
| Caller is not within 14 tiles of the trainer | Caller receives cliloc `500446`: too far away. |
| Caller lacks follower slots | Trainer says cliloc `1049612` with pet name. |
| Caller has follower slots | `DoClaim()` runs, then the pet is removed from `from.Stabled`. |

## Speech Claim Flow

`Claim(Mobile from)` delegates to `Claim(from, null)`.

`Claim(Mobile from, string petName)` aborts when the trainer is deleted or the caller is not alive. It then iterates `from.Stabled`.

| Step | Behavior |
| --- | --- |
| Invalid pet entry | Removes the entry from `from.Stabled` after attempting to clear `IsStabled`. |
| `petName != null` and `Insensitive.Equals(pet.Name, petName)` is false | Skips that pet. |
| `CanClaim(from, pet)` is true | Calls `DoClaim()`, removes the pet from `from.Stabled`, and marks the claim as successful. |
| `CanClaim(from, pet)` is false | Trainer says cliloc `1049612`: pet remained in stables because of follower limit. |

After the loop:

| Final state | Response |
| --- | --- |
| At least one pet was claimed | Trainer says cliloc `1042559`: here you go. |
| No valid stabled pets were counted | Trainer says cliloc `502671`: no animals stabled. |
| A name was requested but no matching/claimable pet was claimed | Opens the claim-list gump. |

`CanClaim(Mobile from, BaseCreature pet)` returns:

```text
from.Followers + pet.ControlSlots <= from.FollowersMax
```

`DoClaim(Mobile from, BaseCreature pet)` changes the pet as follows:

| Field/action | New value |
| --- | --- |
| `ControlMaster` | Caller, via `SetControlMaster(from)` |
| `SummonMaster` | Caller only if `pet.Summoned` is true |
| `Language` | `null` |
| `ControlTarget` | Caller |
| `ControlOrder` | `OrderType.Follow` |
| Location | `MoveToWorld(from.Location, from.Map)` |
| `IsStabled` | `false` |
| `Loyalty` | `BaseCreature.MaxLoyalty` when `Core.SE` is true |

## No-Mount Region Helpers

`AnimalTrainer` also contains static helpers used by movement and mount handling outside the direct trainer NPC flow.

| Helper | Behavior |
| --- | --- |
| `DismountPlayer(Mobile m)` | Clears claim-list mount markers, moves ethereal mounts to owner state, or internalizes a `BaseMount`, clears its control master, marks it stabled, sets `Language = "mount"`, and appends it to `m.Stabled`. |
| `IsBeingFast(Mobile from)` | Returns true for mounted players, light speed footwear, Syth `SythSpeed`, Jedi `Celerity`, Mystic `WindRunner`, or Shinobi `CheetahPaws`. |
| `CleanClaimList(Mobile from)` | Clears stale `"mount"` language markers from stabled pets and attempts to remove invalid entries. |
| `GetLastMounted(Mobile from)` | Searches `from.Stabled` for a pet marked `Language == "mount"` and, if follower slots allow, restores it to the player and mounts it. If no physical mount is restored, it scans world items for `EtherealMount` items owned by the player and assigns their rider/owner fields. |
| `CanGetLastMounted(Mobile from, BaseCreature pet)` | Uses the same follower-slot formula as `CanClaim()`. |
| `IsNoMountRegion(Mobile m, Region reg)` | Blocks mounts in several named regions and in most protected/cave/dungeon/public-style regions outside `"the Port"`. |
| `AllowMagicSpeed(Mobile m, Region reg)` | Allows speed effects in `"the Port"`, `ProtectedRegion`, `PublicRegion`, `UmbraRegion`, and `CaveRegion`. |

## Persistence

| Type | Version | Serialized fields after version |
| --- | --- | --- |
| `AnimalTrainer` | `0` | None. |
| `GypsyAnimalTrainer` | `0` | None beyond inherited `AnimalTrainer` data. |
| `Mobile` | Core mobile versioned format | Writes the `m_Stabled` strong mobile list. |
| `BaseCreature` | Core creature versioned format | Persists `IsStabled` through normal creature state; when `IsStabled` is true, setting the property stops the creature delete timer. |
| `PlayerMobile` | Core player versioned format | Also persists `m_AutoStabled` for automatic no-mount-zone stabling. |

`AnimalTrainer` has no custom stable ledger of its own. Stabled pets belong to the player's `Mobile.Stabled` list, not to an individual trainer instance.

## Admin Commands

No in-game admin or player commands are registered by this system.

Use normal RunUO construction for trainer placement:

| Command surface | Result |
| --- | --- |
| `[add AnimalTrainer` | Creates the standard animal trainer vendor. |
| `[add GypsyAnimalTrainer` | Creates the gypsy subclass that inherits the same stable and claim behavior. |

## Known Issues

| Issue | Impact |
| --- | --- |
| Several stabled-list cleanup paths dereference `pet.IsStabled` inside `if (pet == null || pet.Deleted)` blocks. | If `from.Stabled` ever contains a null or non-`BaseCreature` entry, `CleanClaimList()`, `GetLastMounted()`, `BeginClaimList()`, or `Claim()` can throw instead of removing the invalid entry. |
| `Claim(Mobile, string)` has no explicit trainer map/range check. | The gump claim path requires same map and 14-tile range, but direct speech claim relies only on the engine's speech delivery range before moving pets to the caller. |
| `EndStable()` has no explicit trainer map/range check after target selection. | A player can begin the stable target from speech range and complete the target without the stable method verifying that the player is still near the trainer. |
| `DismountPlayer()` appends physical mounts to `m.Stabled` without checking `GetMaxStabled()` or charging the 30-gold stable fee. | No-mount-region handling can exceed normal stable slot limits and bypass the paid trainer stable flow. |
| The stable success text says the trainer will sell the pet after one real-world week, but no expiration or sale-off logic was found in the traced stable code. | Stabled creatures are stored on `Mobile.Stabled`, and cleanup tasks explicitly skip internal-map creatures when `IsStabled` is true, so the weekly warning appears stale or incomplete. |
| `IsNoMountRegion()` computes a `world` string and never uses it. | Harmless runtime waste, but it suggests the region helper was partially edited or left with dead logic. |
