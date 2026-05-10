# Technology Items

## Scope

This page documents the science-fiction item package under `Data/Scripts/Items/Technology/`.
It covers special weapons, utility tools, explosives, medical and lore records, alien quest items, space dyes, and randomized space-junk loot.

The package defines items only. It does not register commands, event sinks, packet handlers, or a standalone technology crafting system.

## Source Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Items/Technology/AlienEgg.cs` | Quest-style alien egg item, status gump, xormite payment tracking, veterinary handoff, and bonded `Alien` pet hatching. |
| `Data/Scripts/Items/Technology/BaseKilrathi.cs` | Ranged weapon base for Kilrathi bowcasters using Marksmanship and `Krystal` ammunition. |
| `Data/Scripts/Items/Technology/Chainsaw.cs` | Charged tool that converts logs into boards with Lumberjacking checks. |
| `Data/Scripts/Items/Technology/ComputerDatabase.cs` | Static computer terminal gump for choosing medical-record skin and hair colors. |
| `Data/Scripts/Items/Technology/DataPad.cs` | Random lore datapad item with persisted subject, title, author, and text ID. |
| `Data/Scripts/Items/Technology/DoubleLaserSword.cs` | Two-handed laser sword with random elemental damage profile. |
| `Data/Scripts/Items/Technology/DuctTape.cs` | Stackable repair consumable for damaged armor and weapons. |
| `Data/Scripts/Items/Technology/FirstAidKit.cs` | One-use container that spills bandages and random healing/cure/refresh pills into the user's backpack. |
| `Data/Scripts/Items/Technology/KilrathiGun.cs` | One-handed Kilrathi bowcaster using `Krystal` ammo. |
| `Data/Scripts/Items/Technology/KilrathiHeavyGun.cs` | Two-handed heavy Kilrathi bowcaster using `Krystal` ammo. |
| `Data/Scripts/Items/Technology/Krystal.cs` | Stackable glowing ammunition for Kilrathi bowcasters. |
| `Data/Scripts/Items/Technology/Landmine.cs` | Landmine setup item and immovable timed landmine trap. |
| `Data/Scripts/Items/Technology/LightSword.cs` | One-handed laser sword with random elemental damage profile. |
| `Data/Scripts/Items/Technology/MaterialLiquifier.cs` | Charged machine that converts recognizable space-age material items into `SpaceDyes`. |
| `Data/Scripts/Items/Technology/MedicalRecord.cs` | Lore medical-record item that generates patient and planet text. |
| `Data/Scripts/Items/Technology/PlasmaGrenade.cs` | Stackable thrown explosive with countdown targeting and energy damage. |
| `Data/Scripts/Items/Technology/PlasmaTorch.cs` | Consumable torch for melting many locks and traps on containers and dungeon doors. |
| `Data/Scripts/Items/Technology/PortableSmelter.cs` | Charged tool that converts ore into ingots with Mining checks. |
| `Data/Scripts/Items/Technology/RomulanAle.cs` | Stackable drink item that delegates to `DrinkingFunctions.OnDrink()`. |
| `Data/Scripts/Items/Technology/SpaceDyes.cs` | Dye vial that recolors eligible backpack items and returns an empty bottle. |
| `Data/Scripts/Items/Technology/SpaceJunk.cs` | Randomized broken space item plus `SpaceJunkA` through `SpaceJunkZ` value variants. |
| `Data/Scripts/Items/Technology/ThermalDetonator.cs` | Stackable thrown explosive with countdown targeting and energy damage. |

## Item Families

| Family | Items | Main behavior |
| --- | --- | --- |
| Energy weapons | `LightSword`, `DoubleLaserSword` | Generate as one of five hue profiles: cold, fire, poison, energy, or all physical. They play a power-up sound on equip and persist hue so elemental damage can be restored after load. |
| Kilrathi bowcasters | `BaseKilrathi`, `KilrathiGun`, `KilrathiHeavyGun`, `Krystal` | Use Marksmanship as a ranged skill, consume `Krystal` ammunition from quiver or backpack for players, and fire a moving projectile effect. |
| Industrial tools | `Chainsaw`, `PortableSmelter` | Roll 50 to 100 charges, require backpack use, target nearby logs or ore, run skill checks, and break into space junk when charges reach zero. |
| Repair and access tools | `DuctTape`, `PlasmaTorch` | Duct tape repairs backpack weapons and armor at the cost of max durability. Plasma torches consume one torch to unlock or disarm supported doors and containers. |
| Consumables | `FirstAidKit`, `RomulanAle` | First aid kits unpack bandages and random pill-bottle potions, then delete themselves. Romulan Ale uses the shared drinking helper. |
| Explosives and traps | `PlasmaGrenade`, `ThermalDetonator`, `LandmineSetup`, `Landmine` | Grenades and detonators arm a throw target and explode after a countdown; landmines are placed at the user's feet and damage non-owner movers until triggered or decayed. |
| Dyes and materials | `MaterialLiquifier`, `SpaceDyes` | Liquifier charges destroy dropped items and, when the material name or hue is recognized and an empty bottle exists, produce a matching dye vial. |
| Records and lore | `DataPad`, `MedicalRecord`, `ComputerDatabase` | Datapads and records display lore gumps. The computer terminal updates stored player appearance records through a color-selection gump. |
| Space junk | `SpaceJunk`, `SpaceJunkA` through `SpaceJunkZ` | Generates one of 60 broken item appearances with a condition prefix; selected metal junk can be smelted near a forge for iron ingots. |
| Alien egg | `AlienEgg` | Starts locked until activated at the Savaged Empire crash-site coordinates, tracks rare quest pieces and xormite, and can hatch a bonded `Alien` through an animal trainer or veterinarian. |

## Player Workflows

| Workflow | Entry point | Notes |
| --- | --- | --- |
| Cut logs with a chainsaw | Double-click `Chainsaw`, target `BaseLog` within 2 tiles. | Lumberjacking difficulty follows the log resource. Success converts the whole stack to matching boards; failure can reduce or delete the logs. |
| Smelt ore with a portable smelter | Double-click `PortableSmelter`, target `BaseOre` within 2 tiles. | Mining difficulty follows the ore resource. Large ore piles are capped to 30000 units before ingot creation. |
| Repair with duct tape | Double-click `DuctTape`, target damaged backpack armor or weapon. | Successful repairs set hit points to the new max and lower max durability by 1. |
| Melt locks or traps | Double-click `PlasmaTorch`, target a supported dungeon door, lockable container, or trapable container. | Treasure-map chests consume the torch but resist the mechanism change. |
| Throw explosives | Double-click `PlasmaGrenade` or `ThermalDetonator`, then choose a target point. | The item becomes non-stackable, counts down, internalizes on throw, then explodes at the chosen point. |
| Place a landmine | Double-click `LandmineSetup` in backpack. | Placement requires harmful actions in the region and allows up to three nearby landmines before refusing more. |
| Extract dye | Drop material onto `MaterialLiquifier`. | The machine consumes one charge and deletes the dropped item. If a color match succeeds, it consumes one empty bottle and creates a `SpaceDyes` vial. |
| Dye an item | Double-click `SpaceDyes`, target an eligible backpack item. | The target receives the vial hue, the dye is consumed, and an empty bottle is returned. |
| Read logs or records | Double-click `DataPad` or `MedicalRecord`. | The items open gumps and play the computer sound effect. Datapads store their selected lore ID across saves. |
| Recolor player record | Double-click `ComputerDatabase` within 4 tiles. | The terminal updates skin or hair hue and calls `RecordFeatures(true)` after a selection. |
| Hatch an alien egg | Activate the egg at the crash-site coordinates, collect the required pieces through search flows, add xormite, then drop it onto the right animal trainer or veterinarian. | On success, the player receives a bonded controlled `Alien` if follower slots allow it. |

## Serialization Notes

| Type | Serialized data |
| --- | --- |
| `AlienEgg` | Version `1`; rod, yellow crystal, red crystal, growth potion, xormite paid, xormite required, trainer region, quest-piece location, and quest-piece rumor. |
| `Chainsaw` and `PortableSmelter` | Version `0`; remaining charges. |
| `DataPad` | Version `0`; data ID, title, author, and subject. The displayed body text is regenerated from the ID. |
| `MedicalRecord` | Version `0`; generated patient and planet names. |
| `Landmine` | Version `0`; decay time, owner mobile, and power. |
| `LightSword` and `DoubleLaserSword` | Version `0`; hue only, then damage profile is reconstructed from that hue on load. |
| `MaterialLiquifier` | Version `1`; remaining charges. |
| `SpaceDyes` | Version `0`; dye hue. |
| `ComputerDatabase`, `DuctTape`, `FirstAidKit`, `Krystal`, `LandmineSetup`, `PlasmaGrenade`, `PlasmaTorch`, `RomulanAle`, `SpaceJunk`, `SpaceJunkA` through `SpaceJunkZ`, and `ThermalDetonator` | Version-only serialization. |

## Known Issues

| Issue | Impact |
| --- | --- |
| `AlienEgg.ProcessAlienEgg()` counts `HaveRod`, `HaveYellowCrystal`, `HaveRedCrystal`, and `HavePotion` as present when they are `>= 0`, but new eggs initialize those values to `0`. | Hatching can treat the four rare quest pieces as already collected; practical gating falls mostly to xormite, trainer location, and follower slots. |
| `AlienEgg.GetRandomVet()` chooses a random entry from the collected trainer/vendor list without guarding the empty-list case. | If a world has no eligible trainer or veterinarian in the accepted regions, constructing a new egg can fail. |
| `MaterialLiquifier.Deserialize()` always reads `ItemCharges` after version `1` was introduced. | Any older saved liquifier from a version without serialized charges would not load through a guarded upgrade path. |
| `PlasmaTorch.UnlockTarget` casts the generic `ILockable` target to both `LockableContainer` and `TrapableContainer` before confirming those interfaces/classes are actually present. | A future or custom `ILockable` implementation outside those container classes could throw during torch use. |
| `PortableSmelter.InternalTarget` uses `m_Ore.Amount < 2 && m_Ore.ItemID == 0x19B8 || m_Ore.ItemID == 0x19BA` without explicit parentheses. | The `0x19BA` branch applies regardless of amount, which may be broader than the adjacent small-pile failure logic intended. |

## Admin Notes

Technology items mix loot flavor, special tools, combat items, and quest objects rather than one unified system.
When debugging a report, start from the concrete item script and then follow any cross-system entry point, especially `SearchBase` for alien-egg pieces, `BaseVendor.OnDragDrop` for egg hatching, and the normal RunUO targeting callbacks for tools and explosives.
