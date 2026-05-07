# Standard Crafting

## Overview
Standard crafting is implemented as concrete `CraftSystem` subclasses in `Server.Engines.Craft`. A player normally enters the system by double-clicking a `BaseTool`; the tool exposes a `CraftSystem` property, the system validates the `Mobile` and tool state through `CanCraft`, and the shared `CraftGump` renders the catalog supplied by `InitCraftList`.

This page covers the compiled standard trade-skill catalogs:

- `DefAlchemy`
- `DefBlacksmithy`
- `DefBowFletching`
- `DefCarpentry`
- `DefCartography`
- `DefCooking`
- `DefGlassblowing`
- `DefInscription`
- `DefMasonry`
- `DefTailoring`
- `DefTinkering`

The same folder also contains specialized catalogs such as `DefDruidism`, `DefGodCrafting`, `DefShelves`, `DefWands`, and `DefWitchery`. Those are not part of this standard crafting pass.

## Core Scripts
| Script | Role |
| --- | --- |
| `Data/Scripts/Trades/Crafting/DefAlchemy.cs` | Alchemy potions, elixirs, mixtures, and hair potions. |
| `Data/Scripts/Trades/Crafting/DefBlacksmithy.cs` | Blacksmith armor, shields, weapons, dragon scale armor, metal resource selection, repair, resmelt, and enhance support. |
| `Data/Scripts/Trades/Crafting/DefBowFletching.cs` | Bowcraft materials, ammunition, bows, wood resource selection, repair, and enhance support. |
| `Data/Scripts/Trades/Crafting/DefCarpentry.cs` | Carpentry furniture, containers, instruments, house items, addons, wood resource selection, repair, and enhance support. |
| `Data/Scripts/Trades/Crafting/DefCartography.cs` | Blank scrolls and map items. |
| `Data/Scripts/Trades/Crafting/DefCooking.cs` | Ingredients, preparations, baking, barbecue, heat, oven, and mill requirements. |
| `Data/Scripts/Trades/Crafting/DefGlassblowing.cs` | Glassblowing recipes gated by learned glassblowing, 100.0 Alchemy, and forge proximity. |
| `Data/Scripts/Trades/Crafting/DefInscription.cs` | Spell scrolls and spellbooks with spellbook ownership checks for scroll crafting. |
| `Data/Scripts/Trades/Crafting/DefMasonry.cs` | Masonry/stonecraft recipes gated by learned masonry and 100.0 Carpentry. |
| `Data/Scripts/Trades/Crafting/DefTailoring.cs` | Fur, cloth, leather, studded, bone, bags, leather resource selection, repair, and enhance support. |
| `Data/Scripts/Trades/Crafting/DefTinkering.cs` | Components, jewelry, tools, utensils, traps, utility items, metal resource selection, repair, and enhance support. |
| `Data/Scripts/Items/Trades/Tools/BaseTool.cs` | Shared `Item` entry point, durability persistence, maker quality persistence, and `CraftGump` launch path. |
| `Data/Scripts/Items/Trades/Tools/*.cs` | Concrete player tools such as `MortarPestle`, `SmithHammer`, `FletcherTools`, `Saw`, `SewingKit`, `TinkerTools`, `ScribesPen`, `MapmakersPen`, `Skillet`, `Blowpipe`, and `MalletAndChisel`. |
| `Data/Scripts/Trades/Runics/*.cs` | Runic tool variants that bind back to standard `CraftSystem` singletons. |

## Entry Points
Each standard system exposes a lazy static `CraftSystem` singleton. The constructor passes `(1, 1, 1.25)` to the `CraftSystem` base class, which means one craft-effect tick with a 1.25 second delay before completion.

Normal player flow:

1. The player double-clicks a concrete `BaseTool` `Item`.
2. `BaseTool` checks that the tool is in the `Mobile` backpack or equipped.
3. The tool's `CraftSystem` property returns the concrete `Def*` singleton.
4. `CraftSystem.CanCraft(from, tool, null)` validates tool durability, containment/equipping, and any system-specific gate.
5. `CraftGump` opens for allowed craft systems.
6. On craft completion, the concrete `CanCraft` runs again before resources are consumed and the item is created.

There are no in-game admin commands registered by these standard `Def*` crafting scripts.

## Craft System Matrix
| Craft system | Main skill | Recipe entries | Min success at min skill | Exceptional mode | Entry and completion gates | Enabled options |
| --- | --- | ---: | ---: | --- | --- | --- |
| `DefAlchemy` | `Alchemy` | 89 | 0% | `ChanceMinusSixty` | Tool must be usable and accessible. Completion refreshes `RegBar`; failed potions return bottles, while failed liquids/mixtures return a bottle and jar. | None. |
| `DefBlacksmithy` | `Blacksmith` | 129 | 0% | `ChanceMinusSixtyToFourtyFive` | Tool must not conflict with another equipped tool, must be accessible, and final craft requires an anvil plus forge within two tiles. | `Resmelt`, `Repair`, `MarkOption`, `CanEnhance` under `Core.AOS`. |
| `DefBowFletching` | `Bowcraft` | 14 | 50% | `FiftyPercentChanceMinusTenPercent` | Tool must be usable and accessible. | `Repair`, `MarkOption`, `CanEnhance` under `Core.AOS`. |
| `DefCarpentry` | `Carpentry` | 95 | 50% | `ChanceMinusSixty` | Tool must be usable and accessible. | `Repair`, `MarkOption`, `CanEnhance` under `Core.AOS`. |
| `DefCartography` | `Cartography` | 6 | 0% | `ChanceMinusSixty` | Tool must be usable and accessible. Blank scroll crafting uses all available bark fragments. | None. |
| `DefCooking` | `Cooking` | 96 | 0% | `ChanceMinusSixtyToFourtyFive` | Tool must be usable and accessible. Individual recipes can require a mill, oven, or generic heat source. | None. |
| `DefGlassblowing` | `Alchemy` | 13 | 0% | `ChanceMinusSixty` | Caller must be a `PlayerMobile` with `Glassblowing`, at least 100.0 base Alchemy, no conflicting equipped tool, an accessible tool, and a forge within two tiles. | None. |
| `DefInscription` | `Inscribe` | 6 | 0% | `ChanceMinusSixty` | Tool must be usable and accessible. Scroll recipes instantiate the scroll type and require the caster to have the spell in a `Spellbook`. | `MarkOption`. |
| `DefMasonry` | `Carpentry` | 105 | 0% | `ChanceMinusSixty` | Caller must be a `PlayerMobile` with `Masonry`, at least 100.0 base Carpentry, no conflicting equipped tool, and an accessible tool. | Granite resource selection. |
| `DefTailoring` | `Tailoring` | 202 | 50% | `ChanceMinusSixtyToFourtyFive` | Tool must be usable and accessible. Selected cloth hue is retained only for configured goza mat deeds. | `MarkOption`, `Repair` under `Core.AOS`, `CanEnhance` under `Core.AOS`. |
| `DefTinkering` | `Tinkering` | 218 | 50% for potion keg and faction trap removal kit; otherwise 0% | `ChanceMinusSixty` | Tool must be usable and accessible. Faction trap deeds and faction trap removal kits require the crafter to belong to a faction. Silver is not consumed on failure. | `MarkOption`, `Repair`, `CanEnhance` under `Core.AOS`. |

## Craft Groups
| Craft system | Major groups found in source |
| --- | --- |
| `DefAlchemy` | Potions, Elixirs, Mixtures, Hair Potion. |
| `DefBlacksmithy` | Chainmail, Ringmail, Platemail, Royal, Dragon Scale Armor, Helmets, Shields, Bladed, Axes, Pole Arms, Bashing. |
| `DefBowFletching` | Materials, Weapons. |
| `DefCarpentry` | Tools, containers, furniture, addons, instruments, training dummies, archery buttes, house items, magical carpentry, and multi-skill furniture/fixtures. |
| `DefCartography` | Blank Scrolls, Maps. |
| `DefCooking` | Ingredients, `Preperations` recipe category, Baking, Barbecue. |
| `DefGlassblowing` | Glass items. |
| `DefInscription` | Books & Scrolls. |
| `DefMasonry` | Containers, furniture, statues, stone addons, stone armor/statue pieces, and granite items. |
| `DefTailoring` | Furs, Hats, Shirts, Pants, Misc, Footwear, Leather Armor, Studded Armor, Bone Armor, Bags. |
| `DefTinkering` | Multi-Component Items, Jewelry, Misc, Parts, Tools, Utensils, Wizards, Wooden Items. |

## Resource Selection
The first selected sub-resource mutates matching recipe resource types before material checks. `DefBlacksmithy` also registers a second sub-resource selector for dragon scale armor; recipes marked with `SetUseSubRes2` read from the scale selector instead of the metal selector.

### Metal Ingots
Used by `DefBlacksmithy` and `DefTinkering`.

| Resource type | Required main skill |
| --- | ---: |
| `IronIngot` | 0.0 |
| `DullCopperIngot` | 65.0 |
| `ShadowIronIngot` | 70.0 |
| `CopperIngot` | 75.0 |
| `BronzeIngot` | 80.0 |
| `GoldIngot` | 85.0 |
| `AgapiteIngot` | 90.0 |
| `VeriteIngot` | 95.0 |
| `ValoriteIngot` | 99.0 |
| `NepturiteIngot` | 99.0 |
| `ObsidianIngot` | 99.0 |
| `SteelIngot` | 99.0 |
| `BrassIngot` | 105.0 |
| `MithrilIngot` | 110.0 |
| `XormiteIngot` | 115.0 |
| `DwarvenIngot` | 120.0 |

### Wood Boards
Used by `DefBowFletching` and `DefCarpentry`.

| Resource type | Required main skill |
| --- | ---: |
| `Board` | 0.0 |
| `AshBoard` | 65.0 |
| `CherryBoard` | 70.0 |
| `EbonyBoard` | 75.0 |
| `GoldenOakBoard` | 80.0 |
| `HickoryBoard` | 85.0 |
| `MahoganyBoard` | 90.0 |
| `DriftwoodBoard` | 90.0 |
| `OakBoard` | 95.0 |
| `PineBoard` | 100.0 |
| `GhostBoard` | 100.0 |
| `RosewoodBoard` | 100.0 |
| `WalnutBoard` | 100.0 |
| `PetrifiedBoard` | 100.0 |
| `ElvenBoard` | 100.1 |

### Leather
Used by `DefTailoring`.

| Resource type | Required main skill |
| --- | ---: |
| `Leather` | 0.0 |
| `HornedLeather` | 55.0 |
| `BarbedLeather` | 60.0 |
| `NecroticLeather` | 65.0 |
| `VolcanicLeather` | 70.0 |
| `FrozenLeather` | 75.0 |
| `SpinedLeather` | 80.0 |
| `GoliathLeather` | 85.0 |
| `DraconicLeather` | 90.0 |
| `HellishLeather` | 95.0 |
| `DinosaurLeather` | 99.0 |
| `AlienLeather` | 99.0 |

### Granite
Used by `DefMasonry`.

| Resource type | Required main skill |
| --- | ---: |
| `Granite` | 0.0 |
| `DullCopperGranite` | 65.0 |
| `ShadowIronGranite` | 70.0 |
| `CopperGranite` | 75.0 |
| `BronzeGranite` | 80.0 |
| `GoldGranite` | 85.0 |
| `AgapiteGranite` | 90.0 |
| `VeriteGranite` | 95.0 |
| `ValoriteGranite` | 99.0 |
| `NepturiteGranite` | 99.0 |
| `ObsidianGranite` | 99.0 |
| `MithrilGranite` | 109.0 |
| `XormiteGranite` | 109.0 |
| `DwarvenGranite` | 110.0 |

### Dragon Scales
Used as the second blacksmithing sub-resource for dragon scale armor. Every listed scale type has a required main skill of `0.0`.

| Resource type |
| --- |
| `RedScales` |
| `YellowScales` |
| `BlackScales` |
| `GreenScales` |
| `WhiteScales` |
| `BlueScales` |
| `DinosaurScales` |

## Special Requirements
| Requirement | Code behavior |
| --- | --- |
| Tool durability | Standard `CanCraft` implementations reject `tool == null`, deleted tools, and tools with `UsesRemaining < 0`. |
| Tool accessibility | Most systems require `BaseTool.CheckAccessible(tool, from)`, which accepts tools contained by the `Mobile` or equipped directly by the `Mobile`. |
| Equipped tool conflicts | `DefBlacksmithy`, `DefGlassblowing`, and `DefMasonry` also call `BaseTool.CheckTool`, rejecting another equipped `BaseTool` unless it is an `AncientSmithyHammer`. |
| Blacksmithing anvil and forge | `DefBlacksmithy.CheckAnvilAndForge` scans nearby items and static tiles within range `2`. Nearby items must overlap the crafter's Z range and be in line of sight. |
| Glassblowing forge | `DefGlassblowing` calls the blacksmith anvil/forge scanner but only requires the forge result. |
| Learned glassblowing | `DefGlassblowing` requires `from is PlayerMobile`, `((PlayerMobile)from).Glassblowing`, and at least 100.0 base Alchemy. |
| Learned masonry | `DefMasonry` requires `from is PlayerMobile`, `((PlayerMobile)from).Masonry`, and at least 100.0 base Carpentry. |
| Cooking stations | Individual cooking recipes use `SetNeedMill`, `SetNeedOven`, or `SetNeedHeat`; those checks are enforced by the shared `CraftItem` resource path. |
| Inscription spell ownership | For scroll recipes, `DefInscription.CanCraft` creates the scroll type, reads its `SpellID`, finds the matching `Spellbook`, and rejects the craft if the player does not know that spell. |
| Tinkering faction recipes | Faction trap deeds and faction trap removal kits require `Faction.Find(from) != null`. |

## Ending Effects
Most standard systems use the shared ending-message pattern:

| Result | Message behavior |
| --- | --- |
| Tool breaks | Sends localized message `1044038`. |
| Failed and material lost | Returns localized message `1044043`. |
| Failed without material lost | Returns localized message `1044157`. |
| Success with below-average quality | Returns localized message `502785`. |
| Exceptional success with maker's mark | Returns localized message `1044156`. |
| Exceptional success without maker's mark | Returns localized message `1044155`. |
| Normal success | Returns localized message `1044154`. |

System-specific differences:

| Craft system | Difference |
| --- | --- |
| `DefAlchemy` | Refreshes the reagent bar before ending. Failed potions return an empty `Bottle`; failed liquids/mixtures return both a `Bottle` and a `Jar`. Successful potions play the filling sound and return potion-specific success messages. |
| `DefGlassblowing` | Successful crafts play a glass-breaking sound before the standard quality message. |
| `DefInscription` | Scroll failures return `501630`; scroll successes return `501629`. Non-scroll entries use the standard quality messages. |

## Persistence
The `Def*` craft catalogs, recipe entries, groups, and selected runtime `CraftContext` values are not serialized by these scripts. They are rebuilt from source when the script assembly loads.

Persistence for normal crafting tools lives in `BaseTool`. Version `1` writes:

| Field | Reader |
| --- | --- |
| `Crafter` | `ReadMobile()` |
| `ToolQuality` | `ReadInt()` cast to `ToolQuality` |
| `UsesRemaining` | `ReadInt()` |

Most concrete tool subclasses write only their own version `0` after `BaseTool` serialization. Runic tools add their selected `CraftResource` in `BaseRunicTool`.

## Known Issues
| Issue | Impact |
| --- | --- |
| `DefBlacksmithy.IsForge(object obj)` calls `obj.GetType()` without an `obj == null` guard. | Direct null calls to the public helper can throw before normal forge checks run. |
| `DefInscription.CanCraft` uses `Activator.CreateInstance(typeItem)` without a constructor constraint or exception handling. | A misconfigured recipe type can throw during `CanCraft`, which can occur from gump detail/chance checks as well as final crafting checks. |
| `DefCooking` contains a source TODO saying meat pie support must include chicken and lamb legs, but the actual `UnbakedMeatPie` recipe only accepts `RawRibs`. | Meat pie crafting does not support the alternative raw meat types named by the source comment. |
| `DefBowFletching` and `DefCarpentry` both leave TODO comments beside wood sub-resource required-skill tables. | Wood material skill gates are explicitly marked as needing verification in source. |
| `DefMasonry` granite material gates diverge from the ingot ladder used by blacksmithing and tinkering. | `MithrilGranite` and `XormiteGranite` require 109.0, and `DwarvenGranite` requires 110.0, while ingot equivalents require 110.0, 115.0, and 120.0. |
| Several cooking recipes use the visible category string `Preperations`. | The misspelling appears in the craft gump category label for those entries. |
