# Relic Items

## Scope

This page documents the collectible relic package under `Data/Scripts/Items/Relics/`.
It covers randomized decorative relics, relic value identification, vendor and henchman turn-ins, special relic currency, and relic reward entry points from loot, harvesting, grave robbing, fishing, and theft systems.

The package defines items and helper value logic. It does not register commands, packet handlers, or a standalone crafting system.

## Source Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Items/Relics/DDIdItems.cs` | Shared `Server.Misc.RelicItems` helper for recognizing relic classes, estimating value, and routing relics to interested buyer types. |
| `Data/Scripts/Items/Relics/DDRelicAlchemy.cs` | Random alchemy-flask relic with a stored gold value. |
| `Data/Scripts/Items/Relics/DDRelicArmor.cs` | Decorative shield and armor relics with flippable art. |
| `Data/Scripts/Items/Relics/DDRelicArts.cs` | Small decorative art-object relics. |
| `Data/Scripts/Items/Relics/DDRelicBanner.cs` | Tapestry, painting, and oriental painting relics. |
| `Data/Scripts/Items/Relics/DDRelicBearRugs.cs` | Bearskin-rug addon and deed relics. |
| `Data/Scripts/Items/Relics/DDRelicBook.cs` | Random titled book relics. |
| `Data/Scripts/Items/Relics/DDRelicCloth.cs` | Decorative cloth-bundle relics. |
| `Data/Scripts/Items/Relics/DDRelicCoins.cs` | Old coin relics with flippable coin art. |
| `Data/Scripts/Items/Relics/DDRelicDrink.cs` | Rare drink bottle relics. |
| `Data/Scripts/Items/Relics/DDRelicFur.cs` | Fur-bundle relics. |
| `Data/Scripts/Items/Relics/DDRelicGem.cs` | Gem relics. |
| `Data/Scripts/Items/Relics/DDRelicGrave.cs` | Grave, torture-device, and memorial relics. |
| `Data/Scripts/Items/Relics/DDRelicInstrument.cs` | Decorative instrument relics. |
| `Data/Scripts/Items/Relics/DDRelicJewels.cs` | Jewelry relics. |
| `Data/Scripts/Items/Relics/DDRelicLeather.cs` | Leather-bundle relics. |
| `Data/Scripts/Items/Relics/DDRelicLight.cs` | Three light-source relic variants. |
| `Data/Scripts/Items/Relics/DDRelicMoney.cs` | Stackable alternate money and treasure items such as copper, silver, xormite, jewels, gemstones, and gold nuggets. |
| `Data/Scripts/Items/Relics/DDRelicOrbs.cs` | Orb relics for magic-themed buyers. |
| `Data/Scripts/Items/Relics/DDRelicPainting.cs` | Painting relics. |
| `Data/Scripts/Items/Relics/DDRelicReagent.cs` | Rare reagent relics. |
| `Data/Scripts/Items/Relics/DDRelicRug.cs` | Carpet addon and deed relics. |
| `Data/Scripts/Items/Relics/DDRelicScrolls.cs` | Random named spell-scroll relics. |
| `Data/Scripts/Items/Relics/DDRelicStatue.cs` | Statue and oriental sculpture relics. |
| `Data/Scripts/Items/Relics/DDRelicTablet.cs` | Search-tablet relics that can point players toward artifact locations. |
| `Data/Scripts/Items/Relics/DDRelicVase.cs` | Vase relics. |
| `Data/Scripts/Items/Relics/DDRelicWeapon.cs` | Decorative weapon relics with flippable art and oriental variants. |

## Relic Families

| Family | Items | Main behavior |
| --- | --- | --- |
| Decorative relics | Armor, weapons, art, banners, paintings, statues, vases, lights, graves, rugs, and bear rugs | Generate random art, hue, quality text, and `RelicGoldValue`. Many can be flipped when used from the backpack or placed as addons. |
| Scholarly relics | Books, tablets, and scrolls | Store a base relic value, receive Inscribe-based value scaling during identification or sale, and use generated lore names or tablet clue data. |
| Trade-material relics | Alchemy flasks, reagents, cloth, fur, leather, drinks, instruments, gems, jewels, coins, and orbs | Serve as specialist turn-in items for alchemists, tailors, fur traders, leather workers, tavern keepers, bards, jewelers, minters, mages, and related henchmen. |
| Addon deeds | `DDRelicRugAddonDeed`, `DDRelicBearRugsAddonDeed` | Preserve value, hue, quality, item ID, and rug layout metadata, then create the matching placeable addon. |
| Relic money | `DDCopper`, `DDSilver`, `DDXormite`, `DDJewels`, `DDGemstones`, `DDGoldNuggets` | Stackable alternate valuables used by vendors, henchmen, hidden treasure, magic pools, summon carriers, robot schematics, and other reward systems. |
| Oriental relic variants | `DDRelicWeapon.MakeOriental`, `DDRelicStatue.MakeOriental`, `DDRelicBanner.MakeOriental` | Theft and reward paths can convert selected relics into oriental-themed names and item art when the player profile qualifies. |

## Player Workflows

| Workflow | Entry point | Notes |
| --- | --- | --- |
| Identify a relic personally | Use Item Identification or Arms Lore on a relic item. | The skill target calls `RelicItems.IsRelicItem()` and reports `IdentifyRelicValue()`. |
| Pay for appraisal | Ask a provisioner to identify an item, then target a relic. | The provisioner charges gold and says which buyer type values the relic. |
| Sell to a specialist vendor | Drop the relic on a matching vendor or guildmaster. | `BaseVendor.OnDragDrop()` calls `RelicItems.RelicValue()`, pays gold plus Mercantile, guild, or begging adjustments, then deletes the relic. |
| Sell to a henchman | Drop a relic or alternate-value item on a henchman. | Henchman code accepts the same relic-value helper and also handles several relic money classes. |
| Place rug relics | Double-click a carpet or bearskin rug deed in a house. | The deed creates an addon with the deed's stored relic value, hue, quality, and layout ID. |
| Read a relic tablet clue | Double-click a tablet from the backpack with enough Intelligence. | The tablet opens a gump naming a search region and artifact clue. |
| Flip decorative art | Double-click supported relics from a backpack or accessible house context. | Flippable items toggle between stored art IDs while preserving relic value. |
| Convert alternate coinage | Double-click copper or silver coin stacks from a bank box, or drop alternate valuables on eligible NPCs. | Copper and silver can convert into gold/change; xormite, jewels, gemstones, and nuggets are handled by vendor, quest, and player-account systems. |

## Reward Sources

| Source | Relic behavior |
| --- | --- |
| `Loot.RandomRelic()` | Constructs a weighted random relic from the global relic type table. |
| Treasure, paragon, sunken, hoard, boat, and container loot paths | Call `Loot.RandomRelic()` directly or as one reward option. |
| Fishing | Rolls a relic from a fishing reward table and boosts `RelicGoldValue` from fishing skill and guild state. |
| Grave robbing | Adds relics as bonus harvest resources for higher-skill grave robbing. |
| Librarian harvesting | Adds relic scrolls and books as rare bonus resources, and can boost book/scroll value by Inscribe skill. |
| Theft quests | Can put random relics into stolen containers and convert selected relics to oriental variants. |
| Search and quest systems | Use relic tablets as artifact-location clues and relic money as alternate treasure. |

## Valuation Rules

| Rule | Detail |
| --- | --- |
| Base value | Most relic constructors set `RelicGoldValue` from `RelicItems.RelicValue()`, which returns 80 to 500 gold. Some special relics set or scale their own values. |
| Buyer routing | `RelicItems.RelicValue(Item, Mobile)` maps relic classes to buyer roles such as armorer, weaponsmith, jeweler, mage, scribe, sage, artist, variety dealer, fur trader, and shipwright. |
| Henchmen | Henchman fighter, archer, wizard, and monster classes are treated as generic relic buyers. |
| Skill scaling | Books, tablets, and scrolls receive Inscribe-based scaling during appraisal and turn-in. Vendor turn-ins also add Mercantile, matching guild, or begging adjustments. |
| Artist bonus | Artists pay double for paintings and painting-like banner relics. |
| Alternate money | Copper, silver, xormite, jewels, gemstones, and gold nuggets are handled by the wider vendor, henchman, account, quest, and loot economy rather than by `IsRelicItem()`. |

## Serialization Notes

| Type | Serialized data |
| --- | --- |
| Simple relics | Version `0` plus `RelicGoldValue`. |
| Flippable relics | Version `0`, `RelicGoldValue`, `RelicFlipID1`, and `RelicFlipID2`. |
| `DDRelicTablet` | Version `0`, `RelicGoldValue`, flip IDs, description, search dungeon, search type, search item, and required Intelligence threshold. |
| `DDRelicRugAddon`, `DDRelicRugAddonDeed`, `DDRelicBearRugsAddon`, `DDRelicBearRugsAddonDeed` | Version `0`, value, main item ID, rug ID, found flag, color, and quality string. |
| Relic money classes | Version `0`; identity fields such as name, hue, and light are reset in `Deserialize()`. Stack amounts are handled by base item serialization. |

## Known Issues

| Issue | Impact |
| --- | --- |
| `DDRelicTablet.SearchLocation()` builds a `SearchBase` list, then calls `Utility.RandomMinMax(1, aCount)` without guarding the empty-list case. | A world with no `SearchBase` items can fail while constructing a new tablet. |
| `DDRelicTablet.OnDoubleClick()` flips a movable tablet in an accessible house before considering backpack reading and sends the failure message `This table looks quite old.` | House-placed tablets prioritize art flipping over clue reading, and the low-Intelligence message has a text typo. |
| `RelicItems.RelicValue()` checks `relics.Name.Contains("painting")` for banner relics without guarding a null `Name`. | A staff-created or corrupted banner with no name could throw during artist or henchman turn-in. |
| `ObsidianStone` has relic-value sale support for stone crafters, but `RelicItems.IsRelicItem()` does not include it. | It can be paid out through turn-in logic while normal relic identification paths do not classify it as a relic. |

## Admin Notes

Relic items are a cross-system loot and economy layer rather than one isolated item family.
When debugging a relic report, start with the concrete item class, then check `RelicItems` for buyer/value routing and the relevant reward source such as fishing, grave robbing, librarian harvesting, thief loot, or `Loot.RandomRelic()`.
