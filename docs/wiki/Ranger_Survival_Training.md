# Ranger Survival Training

## Scope

This page documents the RunUO ranger survival tutorial exposed by `Server.Mobiles.Ranger`.
It covers the ranger `Mobile`, its context-menu `SpeechGump`, the survival text returned by `SpeechFunctions.SpeechText(..., "Ranger")`, the matching ranger vendor stock, and the concrete camping/trap mechanics referenced by the tutorial.

The tutorial is not a player command system. It does not register `[Command]` handlers, EventSink hooks, packet handlers, or XMLSpawner attachments.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Civilized/Vendors/Ranger.cs` | Ranger `Mobile`, skill profile, vendor stock registration, context-menu tutorial entry, and serialization. |
| `Data/Scripts/System/Misc/Talk.cs` | Shared `SpeechGump` and `SpeechFunctions.SpeechText()` text provider for the `"Ranger"` and `"CampTent"` tutorial pages. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | `SBRanger` buy/sell catalog for study books, animals, archery supplies, ranger armor, tents, and trap kits. |
| `Data/Scripts/Items/Explorers/Kindling.cs` | Campfire setup gates, enemy checks, camping skill gain helper, and 10-minute campfire cooldown. |
| `Data/Scripts/Items/Explorers/Campfire.cs` | Temporary campfire `Item`, status lifecycle, and health/stamina rest ticks. |
| `Data/Scripts/Items/Explorers/Bedroll.cs` | Bedroll setup gates, 10-minute bedroll cooldown, and creation of `BedrolledOut`. |
| `Data/Scripts/Items/Explorers/BedrolledOut.cs` | Laid-out bedroll rest ticks, owner-only interaction, auto-roll-up, and serialization cleanup. |
| `Data/Scripts/Items/Explorers/SmallTent.cs` | Small tent charge handling, wilderness/combat gates, setup spell, temporary addon, and serialization. |
| `Data/Scripts/Items/Explorers/CamperTent.cs` | Camping tent charge handling, outdoor/dungeon gates, public-door teleport setup, and serialization. |
| `Data/Scripts/Items/Traps/TrapKit.cs` | Trapping tools, metal-based trap power formula, charge handling, and trap placement. |
| `Data/Scripts/Items/Traps/SetTrap.cs` | Placed trap `Item`, owner immunity, avoidance call, physical damage formula, decay, and serialization. |
| `Data/Scripts/Items/Traps/HiddenTrap.cs` | Shared trap avoidance helpers used by player-set traps. |
| `Data/Scripts/Items/Doors/PublicDoor.cs` | Public door return path used by camping tent interiors. |

## Ranger Mobile

`Ranger` derives from `BaseVendor`, uses the vendor title `"the ranger"`, and belongs to `NpcGuild.RangersGuild`.

Its constructor assigns these vendor skills:

| Skill | Range |
| --- | ---: |
| `Camping` | 65.0 to 88.0 |
| `Searching` | 65.0 to 88.0 |
| `Hiding` | 45.0 to 68.0 |
| `Marksmanship` | 65.0 to 88.0 |
| `Tracking` | 65.0 to 88.0 |
| `Veterinary` | 65.0 to 88.0 |

`InitSBInfo()` adds sea-town catalogs when `Worlds.IsSeaTown(Location, Map)` is true, then always adds `SBRanger`, `SBLeatherArmor`, and `SBStuddedArmor`.

`InitOutfit()` keeps the normal vendor outfit and adds a random green `FeatheredHat` plus a `Bow`.

## Tutorial Entry Point

The ranger adds `SpeechGumpEntry` to the standard context menu after `base.GetContextMenuEntries()`.

| Entry point | Behavior |
| --- | --- |
| Context menu entry | `SpeechGumpEntry(from, this)` with cliloc/menu ID `6146` and range/order value `3`. |
| Caller gate | `OnClick()` returns unless the stored caller is a `PlayerMobile`. |
| Duplicate gump gate | If the player already has a `SpeechGump`, no new gump is sent. |
| NPC greeting | `IntelligentAction.SayHey(m_Giver)` runs before the gump is opened. |
| Gump title | `Camping Safely`. |
| Text source | `SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Ranger")`. |

The shared `SpeechGump` is a server-side `Gump` with one scrollable HTML region and a close button. Its response handler only plays sound `0x4A`; it does not perform any choice handling.

## Tutorial Text

The `"Ranger"` speech text is a single static HTML string built from the player name and ranger name.

| Topic | Code-backed content |
| --- | --- |
| Camping skill | The tutorial tells players to practice `Camping` to live off the land. |
| Bedroll and campfire | It says a bedroll and simple campfire allow safe rest. The actual rest behavior is implemented by `BedrolledOut` and `Campfire`, documented below. |
| Small tents | It says small tents are short-term emergency safety and only work outside. The code requires wilderness use and creates a five-minute temporary addon. |
| Camping tents | It says camping tents support longer-term rest, underground use for skilled rangers, bank organization, and provisioner/ranger availability. The code teleports users to public interior regions and stores a return point. |
| Trapping tools | It says tools place a trap at the user's feet, owner avoids their own trap, higher `RemoveTrap` improves trap strength, and better metals increase effectiveness. The trap kit code implements those points directly. |

`CampersTent` also exposes its own context-menu `SpeechGumpEntry`, titled `Camping Tent`, using `SpeechFunctions.SpeechText(..., "CampTent")`. That page documents the tent's 40.0 Camping requirement, pack use for private entry, ground use for group entry, 30-second shared entrance, and return-to-origin behavior.

## Ranger Vendor Stock

`SBRanger.InternalBuyInfo` uses server setting chance helpers for most stock. The default chances are configured in `MyServerSettings` as regular `50%`, common `80%`, rare `25%`, and very rare `5%`; the helper methods compare those values against `Utility.RandomMinMax(1, 100)`.

Survival-related stock includes:

| Item or animal | Price | Quantity | Chance helper |
| --- | ---: | ---: | --- |
| `StandardTrackingStudyBook` | 7,500 | 1 | `SellChance()` |
| `StandardCampingStudyBook` | 7,500 | 1 | `SellChance()` |
| `StandardVeterinaryStudyBook` | 7,500 | 1 | `SellChance()`; listed twice in the buy catalog |
| `StandardMarksmanshipStudyBook` | 7,500 | 1 | `SellChance()` |
| `StandardSearchingStudyBook` | 7,500 | 1 | `SellChance()` |
| `Cat` | 138 | random 1 to 15 | `SellChance()` |
| `Dog` | 181 | random 1 to 15 | Always added by `if (1 > 0)` |
| `PackLlama` | 491 | random 1 to 15 | `SellChance()` |
| `PackHorse` | 606 | random 1 to 15 | `SellChance()` |
| `PackMule` | 10,000 | 1 | `SellVeryRareChance()` |
| `Bandage` | 2 | random 10 to 60 | `SellChance()` |
| `SmallTent` | 200 | random 1 to 5 | `SellChance()` |
| `CampersTent` | 500 | random 1 to 5 | `SellCommonChance()` |
| `MyTentEastAddonDeed` | 1,000 | 1 | `SellRareChance()` |
| `MyTentSouthAddonDeed` | 1,000 | 1 | `SellRareChance()` |
| `TrapKit` | 420 | random 1 to 5 | `SellChance()` |

The ranger sell-back catalog can buy survival items when `BuyChance()` passes:

| Item | Sell-back price |
| --- | ---: |
| `MyTentEastAddonDeed` | 200 |
| `MyTentSouthAddonDeed` | 200 |
| `SmallTent` | 50 |
| `CampersTent` | 100 |
| `TrapKit` | 210 |

## Campfire Mechanics

Players use `Kindling.OnDoubleClick(Mobile from)` to place a campfire.

| Gate | Behavior |
| --- | --- |
| Player only | The method body runs only when `from is PlayerMobile`. |
| Move/reach | `VerifyMove(from)` must pass, and the kindling must be within 2 tiles. |
| Boat | `Worlds.IsOnBoat(from)` blocks setup. |
| Combat | A current combatant within 20 tiles and line of sight blocks setup. |
| Existing camp | Any non-off `Campfire` within 20 tiles blocks setup. |
| Nearby enemies | `Kindling.EnemiesNearby(from)` blocks setup. |
| Cooldown | `PlayerMobile.Camp` must be expired; success sets it to `DateTime.Now + 10 minutes`. |
| Placement | `GetFireLocation()` requires `CampAllowed()` and picks the kindling location if world-placed, or a random cardinal adjacent fitting/LOS tile if carried. |
| Skill check | `from.CheckSkill(SkillName.Camping, 0.0, 100.0)` gates success. Failure still calls `RaiseCamping(from)`. |

`CampAllowed()` rejects `PublicRegion`, `ProtectedRegion`, `PirateRegion`, and `SafeRegion`.

On success, one kindling is consumed, any remaining kindling stack is placed back in the backpack when appropriate, a new `Campfire` is moved to the chosen tile, and `RaiseCamping(from)` is called.

`RaiseCamping(Mobile m)` performs ten `m.CheckSkill(SkillName.Camping, 0, 125)` calls.

## Campfire Rest

`Campfire` starts as a non-movable burning item with `LightType.Circle300`.

| Age | Status |
| ---: | --- |
| 0 to 59 seconds | `Burning` (`ItemID 0xDE3`, `Circle300`) |
| 60 to 89 seconds | `Extinguishing` (`ItemID 0xDE9`, `Circle150`) |
| 90 to 99 seconds | `Off` (`ItemID 0xDEA`, `ArchedWindowEast`) |
| 100 seconds or more | Deleted |

Every second while not off or deleted, the campfire gathers `PlayerMobile` instances within 3 tiles that do not have enemies nearby. Each qualifying mobile rests only when both `Hunger > 4` and `Thirst > 4`.

| Rest target | Per-tick behavior |
| --- | --- |
| Stamina | If below max, add `MyServerSettings.PlayerLevelMod(2, m)`, minimum 1 after the local campfire check. |
| Hits | If below max, add `MyServerSettings.PlayerLevelMod(2, m)`, minimum 1 after the local campfire check. |
| Skill | Calls `m.CheckSkill(SkillName.Camping, 0, 125)`. |

## Bedroll Mechanics

`Bedroll.OnDoubleClick(Mobile from)` is another camping rest path.

| Gate | Behavior |
| --- | --- |
| Player only | The method body runs only when `from is PlayerMobile`. |
| Region | `PublicRegion`, `ProtectedRegion`, and `SafeRegion` block use. |
| Duplicate bedroll | An existing `BedrolledOut` owned by the same mobile within 20 tiles blocks setup. |
| Nearby enemies | `Kindling.EnemiesNearby(from)` blocks setup. |
| Cooldown | `PlayerMobile.Bedroll` must be expired; success sets it to `DateTime.Now + 10 minutes`. |
| Move/reach | `VerifyMove(from)` must pass, and the bedroll must be within 2 tiles. |
| Skill check | `from.CheckSkill(SkillName.Camping, 0.0, 125.0)` gates success. Failure still calls `RaiseCamping(from)`. |
| Placement | `GetBedLocation()` uses the same region and adjacent-tile pattern as kindling. |

On success, the carried bedroll is deleted and a `BedrolledOut` item is moved to the chosen location.

`BedrolledOut` is non-movable, stores its owner, ticks every second, and rolls itself up after 100 seconds. Rolling up adds a fresh `Bedroll` to the owner's backpack when the owner and backpack still exist, then deletes the laid-out item.

While active, it rests only the owner within 3 tiles, with the same hunger/thirst gate and hit/stamina/skill tick behavior as `Campfire`.

## Small Tent Mechanics

`SmallTent` is a blessed `Item` named `a small tent`, weighs `2.0`, starts with `10` charges, and uses a randomized cloth hue.

| Gate | Behavior |
| --- | --- |
| Backpack | Must be in the caller's backpack. |
| Boat | `Worlds.IsOnBoat(from)` blocks setup. |
| Region | `Worlds.IsMainRegion(Worlds.GetRegionName(from.Map, from.Location))` must be true; otherwise the code says it can only be set up in the wilderness. |
| Combat | Current combatant within 20 tiles and line of sight blocks setup. |
| Minimum skill | `Validate()` requires `Camping >= 10.0`. |
| Skill check | Success requires `from.CheckSkill(SkillName.Camping, 0.0, 50.0)`. |
| Charges | Success and failed setup both consume one charge after validation. When charges reach zero, the tent deletes itself. |

On skill success, `SmallTent` casts an inner `SmallTentSpell`. The spell has no mana cost, no reagents, no fizzle, zero recovery, a 5-second cast delay, and a 1-second base delay. On cast completion, it creates a `SmallTentAddon` at the caster's current location with the tent hue and caster serial.

`SmallTentAddon` builds a 21-component temporary tent and decays after 5 minutes. If the owner uses one of its components, it calls `Kindling.RaiseCamping(from)`, plays sound `0x55`, and deletes the addon.

## Camping Tent Mechanics

`CampersTent` is an `Item` named `camping tent`, weighs `5.0`, starts with `10` charges, and uses a randomized cloth hue.

| Gate | Behavior |
| --- | --- |
| Minimum skill | `Camping >= 40.0`; otherwise the caller is told they must be a novice explorer. |
| Public region | `PublicRegion` blocks use with only flavor text. |
| Boat | `Worlds.IsOnBoat(from)` blocks setup. |
| Spaceship | `Worlds.IsOnSpaceship(from.Location, from.Map)` blocks setup. |
| Combat | Current combatant within 20 tiles and line of sight blocks setup. |
| Dungeon use | `DungeonRegion` and `BardDungeonRegion` require `Camping >= 90.0`. |
| Non-dungeon restricted regions below 90 | If not a main/outdoor/bad-outdoor/village region, use is blocked. |
| Non-dungeon restricted regions at 90+ | If not dungeon, bard dungeon, main, outdoor, bad-outdoor, or village, use is blocked. |
| Skill check | Success requires `from.CheckSkill(SkillName.Camping, 0.0, 125.0)`. |
| Charges | Success and failed setup both consume one charge. Failure deletes the tent immediately if charges fall below 1; success leaves a zero-charge tent until a later use attempts to delete it. |

On success, the tent records the caller's origin as `PlayerMobile.CharacterPublicDoor` in the form `X#Y#Z#Map#Zone`.

| Condition | Destination |
| --- | --- |
| `DungeonRegion` or `BardDungeonRegion` | `(3686, 3330, 0)` on `Map.Sosaria`, zone `the Dungeon Room` |
| Camping skill greater than 66.0 outside those dungeon regions | `(3790, 3966, 0)` on `Map.Sosaria`, zone `the Camping Tent` |
| Other allowed use | `(3709, 3974, 0)` on `Map.Sosaria`, zone `the Camping Tent` |

If the tent is used from the caller's backpack, the player and pets teleport directly to the interior destination and no world entrance is created.

If a world-placed tent is used within 3 tiles, the code consumes a charge, creates a temporary `PublicDoor`-derived entrance named `camping tent` at the tent's world location, returns the original rolled tent to the caller's backpack, and teleports the caller to the interior destination. The temporary entrance deletes itself after 30 seconds.

The `PublicDoor` exit path reads `CharacterPublicDoor` while the player is in a `PublicRegion`, clears it, and teleports the player back to the stored origin. When entering a door named `camping tent`, `PublicDoor` uses zone `the Camping Tent`.

## Trap Kit Mechanics

`TrapKit` is an `Item` named `trapping tools`, weighs `5.0`, defaults to metal `"Iron"`, and starts with `25` charges.

`OnDoubleClick(Mobile from)` applies these gates:

| Gate | Behavior |
| --- | --- |
| Backpack | Must be in the caller's backpack. |
| Charges | Must have `Charges > 0`; zero-charge tools give no feedback and are not deleted by this branch. |
| Local trap count | Counts `SetTrap` items within 10 tiles; if the count is greater than 2, placement is blocked. |
| Harm rules | `from.Region.AllowHarmful(from, from)` must be true. |
| Skill present | `from.Skills[SkillName.RemoveTrap].Value > 0` is required. |

On success, one charge is consumed and a `SetTrap` is created at the caller's location and map. Trap hue matches the tools.

Trap power starts with:

```text
floor(RemoveTrap skill value / 2) + 1 + metal bonus
```

| Metal string | Bonus |
| --- | ---: |
| `Iron` or unrecognized metal | 0 |
| `Dull Copper` | 3 |
| `Shadow Iron` | 6 |
| `Copper` | 9 |
| `Bronze` | 12 |
| `Gold` | 15 |
| `Agapite` | 18 |
| `Verite` | 21 |
| `Valorite` | 24 |
| `Nepturite` | 27 |
| `Obsidian` | 30 |
| `Steel` | 33 |
| `Brass` | 36 |
| `Mithril` | 39 |
| `Xormite` | 42 |
| `Dwarven` | 78 |

When charges reach zero after use, the kit sends `These tools have been worn out.` and deletes itself.

## Set Trap Mechanics

`SetTrap` is a non-movable `Item` named `a trap`, stores `Owner` and `Power`, and decays after 180 seconds.

The owner can move over the trap without triggering it. Non-owners trigger `OnMoveOver(Mobile m)` as follows:

| Step | Behavior |
| --- | --- |
| Air Walk | A `PlayerMobile` under `ResearchAirWalk` gets an air-walk visual and sound; this branch does not damage the mobile or delete the trap. |
| Eligible player | Player must be alive, unblessed, `AccessLevel.Player`, without the gem/jewel pocket avoidance helpers. |
| Eligible creature | `BaseCreature` must be unblessed. |
| Avoidance | Calls `HiddenTrap.CheckTrapAvoidance(m, this)`. A result of `0` means the trap was avoided or disabled. |
| Damage | If avoidance returns positive, physical damage is rolled and the trap deletes itself. |
| Owner notice | If avoidance succeeds and owner still exists, owner receives `Your trap seems to have been thwarted!`. |

Damage formula:

```text
StrMax = Power
StrMin = floor(Power / 2)
Damage = floor(Utility.RandomMinMax(StrMin, StrMax) * (100 - target.PhysicalResistance) / 100) + 10
```

The damage source passed to `m.Damage()` is the trap owner.

`HiddenTrap.CheckTrapAvoidance()` can prevent a `SetTrap` from firing through normal `RemoveTrap` skill checks, a matching owned `TrapWand`, lucky-player checks, or a `TenFootPole`. Because `SetTrap` is not a `HiddenTrap`, the server-wide `FloorTrapTrigger()` chance is not applied to player-set traps.

## Serialization

| Type | Version | Serialized fields after version | Load behavior |
| --- | ---: | --- | --- |
| `Ranger` | 0 | None | Standard `BaseVendor` load. |
| `Kindling` | 0 | None | Standard item load. |
| `Campfire` | 0 | None | Deletes itself during deserialize. |
| `Bedroll` | encoded 0 | None | Standard item load. |
| `BedrolledOut` | 0 | Owner `Mobile` | Reads owner, then calls `RollUp()`, which can restore a fresh `Bedroll` to the owner and deletes the laid-out item. |
| `SmallTent` | 0 | `SmallTentEffect` as `int`, charges as `int` | Restores effect and charge count. |
| `SmallTentAddon` | 0 | Decay `DateTime`, owner serial as encoded `int` | Refreshes decay timer, then reads owner serial. |
| `CampersTent` | 0 | `CamperTentEffect` as `int`, charges as `int` | Restores effect and charge count. |
| `CampersTent.InternalItem` | inherited only | None | Deletes itself during deserialize. |
| `TrapKit` | 0 | Metal `string`, charges as `int` | Restores metal and charge count. |
| `SetTrap` | 0 | Decay `DateTime`, owner `Mobile`, power `int` | Refreshes decay timer, then restores owner and power. |
| `HiddenTrap` | 0 | Hidden trap type as `int` | Restores hidden trap type. |

## Admin Commands

None. The traced ranger survival tutorial and related camping/trap items do not register in-game commands.

Staff can inspect or edit individual item fields exposed through `[CommandProperty]`, including tent charges, trap-kit metal/charges, and set-trap owner/power.

## Known Issues

| Issue | Impact |
| --- | --- |
| `SpeechGump.OnResponse()` reads `sender.Mobile` and calls `from.SendSound()` without null checks. | A malformed or stale `NetState` response can null-reference the shared speech gump close handler. |
| `SpeechFunctions.SpeechText()` reads `from.Name`, `m.Name`, and calls `MyChat.speechText()` before validating either mobile. | Any caller that passes a null or invalid mobile can throw before returning tutorial text. |
| `Ranger.SpeechGumpEntry.OnClick()` validates only that the stored caller is a `PlayerMobile`; it does not re-check that the ranger giver still exists, is visible, is alive, or remains in range. | A stale context-menu click can still attempt to greet through `IntelligentAction.SayHey()` and open the tutorial. |
| Several range scans use pooled `GetItemsInRange()` or `GetMobilesInRange()` enumerables without calling `Free()`. | `Kindling`, `Campfire`, `Bedroll`, `BedrolledOut`, and `TrapKit` can leak pooled enumerables on repeated camping/trapping use. |
| `CampersTent.OnDoubleClick()` casts `from` to `PlayerMobile` after only skill/region checks. | Non-player `Mobile` callers with sufficient Camping can reach an invalid cast after a charge is consumed. |
| Successful `CampersTent` use consumes the final charge but does not delete the tent until a later use attempts to use the zero-charge item. | A worn-out camping tent can remain in inventory after its last successful trip. |
| `TrapKit.OnDoubleClick()` gives no feedback and does not delete the item when `Charges <= 0` at the start of use. | Staff-edited or deserialized zero-charge trap kits silently do nothing on double-click. |
| `SBRanger.InternalBuyInfo` lists `StandardVeterinaryStudyBook` twice behind separate `SellChance()` rolls. | Rangers can stock duplicate veterinary study-book entries while other listed study books appear once. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0128.
- Backlog rows: RB-06756.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Mobiles/Civilized/Vendors/Ranger.cs (CurrentFile)
- Data/Scripts/System/Misc/Talk.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/StoreSalesList.cs (CurrentFile)
- Data/Scripts/Items/Explorers/Kindling.cs (CurrentFile)
- Data/Scripts/Items/Explorers/Campfire.cs (CurrentFile)
- Data/Scripts/Items/Explorers/Bedroll.cs (CurrentFile)
- Data/Scripts/Items/Explorers/BedrolledOut.cs (CurrentFile)
- Data/Scripts/Items/Explorers/SmallTent.cs (CurrentFile)
- Data/Scripts/Items/Explorers/CamperTent.cs (CurrentFile)
- Data/Scripts/Items/Traps/TrapKit.cs (CurrentFile)
- Data/Scripts/Items/Traps/SetTrap.cs (CurrentFile)
- Data/Scripts/Items/Traps/HiddenTrap.cs (CurrentFile)
- Data/Scripts/Items/Doors/PublicDoor.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Gump=4; Movement=1; Timer=6.
- Data/Scripts/Items/Explorers/BedrolledOut.cs:L23 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Explorers/CamperTent.cs:L110 Gump SendGump access=Internal
- Data/Scripts/Items/Explorers/CamperTent.cs:L374 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Items/Explorers/Campfire.cs:L28 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Explorers/SmallTent.cs:L368 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Explorers/SmallTent.cs:L448 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Traps/HiddenTrap.cs:L3606 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Items/Traps/SetTrap.cs:L160 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Civilized/Vendors/Ranger.cs:L85 Gump SendGump access=Internal
- Data/Scripts/System/Misc/Talk.cs:L52 Gump OnResponse access=Internal
- Data/Scripts/System/Misc/Talk.cs:L642 Gump SendGump access=Internal

### Serialization Evidence

- Serialized rows matched: 13.
- Data/Scripts/Items/Doors/PublicDoor.cs:Server.Items.PublicDoor version=0 serialize=L314 deserialize=L322 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Items/Explorers/Bedroll.cs:Server.Items.Bedroll version=Unknown serialize=L24 deserialize=L30 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Explorers/BedrolledOut.cs:Server.Items.BedrolledOut version=0 serialize=L130 deserialize=L137 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Explorers/CamperTent.cs:Server.Items.CampersTent version=0 serialize=L395 deserialize=L403 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Explorers/CamperTent.cs:Server.Items.InternalItem version=Unknown serialize=L363 deserialize=L368 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Explorers/Campfire.cs:Server.Items.Campfire version=0 serialize=L152 deserialize=L158 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Explorers/Kindling.cs:Server.Items.Kindling version=0 serialize=L28 deserialize=L34 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Explorers/SmallTent.cs:Server.Items.SmallTent version=0 serialize=L180 deserialize=L188 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Explorers/SmallTent.cs:Server.Items.SmallTentAddon version=0 serialize=L451 deserialize=L459 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Items/Traps/HiddenTrap.cs:Server.Items.HiddenTrap version=0 serialize=L1315 deserialize=L1322 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Items/Traps/SetTrap.cs:Server.Items.SetTrap version=0 serialize=L163 deserialize=L172 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Items/Traps/TrapKit.cs:Server.Items.TrapKit version=0 serialize=L200 deserialize=L208 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Items/Doors/PublicDoor.cs=Keep
- Data/Scripts/Items/Doors/PublicDoor.cs=Keep
- Data/Scripts/Items/Explorers/Bedroll.cs=Keep
- Data/Scripts/Items/Explorers/Bedroll.cs=Keep
- Data/Scripts/Items/Explorers/BedrolledOut.cs=Keep
- Data/Scripts/Items/Explorers/BedrolledOut.cs=Keep
- Data/Scripts/Items/Explorers/CamperTent.cs=Keep
- Data/Scripts/Items/Explorers/CamperTent.cs=Keep
- Data/Scripts/Items/Explorers/Campfire.cs=Keep
- Data/Scripts/Items/Explorers/Campfire.cs=Keep
- Data/Scripts/Items/Explorers/Kindling.cs=Keep
- Data/Scripts/Items/Explorers/Kindling.cs=Keep
- Data/Scripts/Items/Explorers/SmallTent.cs=Keep
- Data/Scripts/Items/Explorers/SmallTent.cs=Keep
- Data/Scripts/Items/Traps/HiddenTrap.cs=Keep
- Data/Scripts/Items/Traps/HiddenTrap.cs=Keep
- Data/Scripts/Items/Traps/SetTrap.cs=Keep
- Data/Scripts/Items/Traps/SetTrap.cs=Keep
- Data/Scripts/Items/Traps/TrapKit.cs=Keep
- Data/Scripts/Items/Traps/TrapKit.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
