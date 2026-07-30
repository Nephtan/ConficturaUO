# Stonecrafting

## Overview

Stonecrafting is the `DefMasonry` `CraftSystem`. It uses `SkillName.Carpentry` as its main skill, opens the localized masonry menu `1044500`, consumes granite resources, and creates stone containers, decorations, furniture, statues, tombstones, and several multi-tile addon deeds.

The traced system does not register a dedicated stonecrafting `[Command]`, packet handler, `EventSink` hook, or XMLSpawner attachment. Normal administration uses standard RunUO construction and property tools such as `[add StoneCrafter]`, `[add MasonryBook]`, `[add StoneMiningBook]`, `[add MalletAndChisel]`, or `[add <recipe type>]`.

Code-Verified: 2026-05-08

## Core Scripts

| Script | Namespace | Role |
| --- | --- | --- |
| `Data/Scripts/Trades/Crafting/DefMasonry.cs` | `Server.Engines.Craft` | Masonry craft system, skill gates, recipe list, resource options, sound, and ending messages. |
| `Data/Scripts/Items/Trades/Tools/MalletAndChisel.cs` | `Server.Items` | `BaseTool` entry point returning `DefMasonry.CraftSystem`. |
| `Data/Scripts/Items/Trades/Specialized/MasonryBook.cs` | `Server.Items` | Player unlock item for the `PlayerMobile.Masonry` flag. |
| `Data/Scripts/Items/Trades/Specialized/StoneMiningBook.cs` | `Server.Items` | Related unlock item for `PlayerMobile.StoneMining`, needed to gather granite through mining. |
| `Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs` | `Server.Items` | `BaseGranite` and concrete granite resource Items. |
| `Data/Scripts/Mobiles/Civilized/Vendors/StoneCrafter.cs` | `Server.Mobiles` | Vendor that stocks stonecrafting books, tools, and related boards/logs. |
| `Data/Scripts/Trades/Stone/BaseStatue.cs` | `Server.Items` | Shared direct `Item` base for stone statues, decorations, furniture, and tombstones. |
| `Data/Scripts/Trades/Stone/BaseStatueDeed.cs` | `Server.Items` | Shared `BaseAddon`/`BaseAddonDeed` implementation for multi-tile stone assemblies. |
| `Data/Scripts/Trades/Stone/Assemblies.cs` | `Server.Items` | Multi-tile `BaseStatueDeed` recipe classes. |
| `Data/Scripts/Trades/Stone/Furniture.cs` | `Server.Items` | Direct `BaseStatue` furniture and decoration classes. |
| `Data/Scripts/Trades/Stone/Gravestones.cs` | `Server.Items` | Direct `BaseStatue` tombstone classes. |
| `Data/Scripts/Trades/Stone/Statues.cs` | `Server.Items` | Direct `BaseStatue` statue classes. |

`Data/Scripts/Scripts.csproj` explicitly compiles the masonry craft system, tool, vendor, masonry book, and all `Trades/Stone` scripts.

## Unlock And Entry Flow

The player opens the masonry menu by using a `MalletAndChisel`. The tool's `CraftSystem` property returns `DefMasonry.CraftSystem`.

`DefMasonry.CanCraft` allows crafting only when all of the following are true:

| Gate | Compiled condition |
| --- | --- |
| Tool exists and is not worn out | `tool != null`, `!tool.Deleted`, and `tool.UsesRemaining >= 0`. |
| Tool is valid for caller | `BaseTool.CheckTool(tool, from)`. |
| Caller is a player with masonry unlocked | `from is PlayerMobile && ((PlayerMobile)from).Masonry`. |
| Carpentry gate | `from.Skills[SkillName.Carpentry].Base >= 100.0`. |
| Tool is on the caller | `BaseTool.CheckAccessible(tool, from)`. |

`MasonryBook` is the unlock item. On double-click it must be in the caller's backpack, the caller must be a `PlayerMobile`, and the caller's base Carpentry must be at least `100.0`. If the player has not already learned masonry, it sets `pm.Masonry = true`, sends the learned message, and deletes the book.

`StoneCrafter` vendors are in the `MinersGuild`, carry Carpentry skill `85.0..100.0`, and add `SBStoneCrafter` stock. Their buy list can include:

| Vendor stock | Type | Price | Quantity |
| --- | --- | ---: | ---: |
| `Making Valuables With Stonecrafting` | `MasonryBook` | `10625` | `Utility.Random(1, 15)` |
| `Mining For Quality Stone` | `StoneMiningBook` | `10625` | `Utility.Random(1, 15)` |
| `1044515` | `MalletAndChisel` | `3` | `Utility.Random(1, 15)` when `MyServerSettings.SellChance()` passes |
| `Jade Statue Maker` | `JadeStatueMaker` | `50000` | `1` when `MyServerSettings.SellChance()` passes |
| `Marble Statue Maker` | `MarbleStatueMaker` | `50000` | `1` when `MyServerSettings.SellChance()` passes |
| `Bronze Statue Maker` | `BronzeStatueMaker` | `50000` | `1` when `MyServerSettings.SellChance()` passes |

## Granite Resources

Every masonry recipe consumes one of the configured granite resource types. The default subresource is `Granite`; higher-tier granite choices require the listed main-skill threshold.

| Resource Item | Craft resource | Required skill |
| --- | --- | ---: |
| `Granite` | `CraftResource.Iron` | `0.0` |
| `DullCopperGranite` | `CraftResource.DullCopper` | `65.0` |
| `ShadowIronGranite` | `CraftResource.ShadowIron` | `70.0` |
| `CopperGranite` | `CraftResource.Copper` | `75.0` |
| `BronzeGranite` | `CraftResource.Bronze` | `80.0` |
| `GoldGranite` | `CraftResource.Gold` | `85.0` |
| `AgapiteGranite` | `CraftResource.Agapite` | `90.0` |
| `VeriteGranite` | `CraftResource.Verite` | `95.0` |
| `ValoriteGranite` | `CraftResource.Valorite` | `99.0` |
| `NepturiteGranite` | `CraftResource.Nepturite` | `99.0` |
| `ObsidianGranite` | `CraftResource.Obsidian` | `99.0` |
| `MithrilGranite` | `CraftResource.Mithril` | `109.0` |
| `XormiteGranite` | `CraftResource.Xormite` | `109.0` |
| `DwarvenGranite` | `CraftResource.Dwarven` | `110.0` |

`BaseGranite` is stackable, uses item ID `0x2158`, labels as localized number `1044607`, and stores its `CraftResource` in serialization. It names new resources with `MaterialInfo.GetResourceName(resource) + "granite"`.

Crafted masonry outputs retain color from selected granite. Direct `BaseStatue` items store `Crafter`, store a string such as `Dull Copper Granite`, and set `Hue` from `MaterialInfo.GetMaterialColor`. `BaseStatueDeed` outputs call `Statues.SetStatue`, which stores statue ID, hue, material string, maker name, and name on the deed before later building a `BaseStatueAddon`.

## Crafting Math

`DefMasonry.GetChanceAtMin` returns `0.0`, so the shared `CraftItem.GetSuccessChance` formula reduces to:

```text
if any required skill is below its minimum:
    success = 0.0
else:
    success = (Carpentry - recipeMin) / (recipeMax - recipeMin)
if Carpentry == recipeMax and all required skills are satisfied:
    success = 1.0
```

For `StoneCoffin` and `StoneCasket`, Forensics is an extra required skill. It must be at least `75.0`; `CraftItem.CheckSkills` still computes success from the main Carpentry range after all required skills pass.

Exceptional chance uses the default `CraftECA.ChanceMinusSixty` path:

```text
exceptionalChance = successChance - 0.6
```

The system plays sound `0x65A` during crafting. On completion it uses standard RunUO craft messages for worn-out tools, failure with or without material loss, normal success, and exceptional success.

## Recipe Catalog

All recipes consume the listed amount of selected granite. The primary skill column is the Carpentry min/max added by `AddCraft`.

| Group | Recipe type | Menu name | Carpentry | Granite | Extra skill |
| --- | --- | --- | ---: | ---: | --- |
| Containers | `StoneCoffin` | sarcophagus, woman | `90.0..115.0` | 10 | Forensics `75.0..80.0` |
| Containers | `StoneCasket` | sarcophagus, man | `90.0..115.0` | 10 | Forensics `75.0..80.0` |
| Containers | `RockUrn` | urn | `80.0..105.0` | 5 |  |
| Containers | `RockVase` | vase | `80.0..105.0` | 5 |  |
| Containers | `StoneOrnateUrn` | urn | `90.0..110.0` | 6 |  |
| Containers | `StoneOrnateTallVase` | vase | `95.0..120.0` | 8 |  |
| Decorations | `StoneVase` | vase | `42.5..92.5` | 2 |  |
| Decorations | `StoneLargeVase` | vase, large | `52.5..102.5` | 4 |  |
| Decorations | `StoneAmphora` | amphora | `42.5..92.5` | 2 |  |
| Decorations | `StoneLargeAmphora` | amphora, large | `52.5..102.5` | 4 |  |
| Decorations | `StoneOrnateVase` | vase, ornate | `52.5..102.5` | 4 |  |
| Decorations | `StoneOrnateAmphora` | amphora, ornate | `52.5..102.5` | 4 |  |
| Decorations | `StoneGargoyleVase` | vase, gargoyle | `62.5..112.5` | 6 |  |
| Decorations | `StoneBuddhistSculpture` | sculpture, Buddhist | `62.5..122.5` | 8 |  |
| Decorations | `StoneMingSculpture` | sculpture, Ming | `52.5..122.5` | 6 |  |
| Decorations | `StoneYuanSculpture` | sculpture, Yuan | `52.5..122.5` | 6 |  |
| Decorations | `StoneQinSculpture` | sculpture, Qin | `52.5..122.5` | 6 |  |
| Decorations | `StoneMingUrn` | urn, Ming | `42.5..92.5` | 3 |  |
| Decorations | `StoneQinUrn` | urn, Qin | `42.5..92.5` | 3 |  |
| Decorations | `StoneYuanUrn` | urn, Yuan | `42.5..92.5` | 3 |  |
| Furniture | `StoneChairs` | `1024635` | `55.0..105.0` | 4 |  |
| Furniture | `StoneBenchLong` | bench, long | `55.0..105.0` | 8 |  |
| Furniture | `StoneBenchShort` | bench, short | `55.0..105.0` | 5 |  |
| Furniture | `StoneTableLong` | table, long | `65.0..115.0` | 12 |  |
| Furniture | `StoneTableShort` | table, short | `65.0..115.0` | 10 |  |
| Furniture | `StoneWizardTable` | table, wizard | `95.0..125.0` | 15 |  |
| Furniture | `StoneSteps` | steps | `55.0..105.0` | 5 |  |
| Furniture | `StoneBlock` | block | `55.0..105.0` | 5 |  |
| Furniture | `StoneSarcophagus` | sarcophagus | `65.0..125.0` | 10 |  |
| Furniture | `StoneColumn` | column | `65.0..125.0` | 10 |  |
| Furniture | `StoneGothicColumn` | column, gothic | `85.0..135.0` | 20 |  |
| Furniture | `StonePedestal` | pedestal | `65.0..125.0` | 5 |  |
| Furniture | `StoneFancyPedestal` | pedestal, fancy | `70.0..130.0` | 7 |  |
| Furniture | `StoneRoughPillar` | pillar | `85.0..135.0` | 15 |  |
| Small Statues | `SmallStatueAngel` | angel statue | `55.0..105.0` | 4 |  |
| Small Statues | `SmallStatueDragon` | dragon statue | `55.0..105.0` | 4 |  |
| Small Statues | `StatueGargoyleBust` | gargoyle bust | `60.0..110.0` | 6 |  |
| Small Statues | `GargoyleStatue` | gargoyle statue | `55.0..105.0` | 4 |  |
| Small Statues | `StatueBust` | man bust | `55.0..105.0` | 4 |  |
| Small Statues | `SmallStatueMan` | man statue | `55.0..105.0` | 4 |  |
| Small Statues | `SmallStatueNoble` | noble statue | `55.0..105.0` | 4 |  |
| Small Statues | `SmallStatuePegasus` | pegasus statue | `55.0..105.0` | 4 |  |
| Small Statues | `SmallStatueSkull` | skull idol | `55.0..105.0` | 4 |  |
| Small Statues | `SmallStatueWoman` | woman statue | `55.0..105.0` | 4 |  |
| Medium Statues | `StatueAdventurer` | adventurer statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueAmazon` | amazon statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueDemonicFace` | demonic face | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueDruid` | druid statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueElvenKnight` | elf knight statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueElvenPriestess` | elf priestess statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueElvenSorceress` | elf sorceress statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueElvenWarrior` | elf warrior statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueFighter` | fighter statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueGargoyleTall` | gargoyle statue | `65.0..115.0` | 8 |  |
| Medium Statues | `GargoyleFlightStatue` | gargoyle statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueGryphon` | gryphon statue | `65.0..115.0` | 10 |  |
| Medium Statues | `SmallStatueLion` | lion statue | `65.0..115.0` | 8 |  |
| Medium Statues | `MedusaStatue` | medusa statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueMermaid` | mermaid statue | `65.0..115.0` | 10 |  |
| Medium Statues | `StatueNoble` | noble statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatuePriest` | priest statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueSeaHorse` | sea horse statue | `65.0..115.0` | 10 |  |
| Medium Statues | `SphinxStatue` | sphinx statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueSwordsman` | swordsman statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueWolfWinged` | winged wolf statue | `65.0..115.0` | 8 |  |
| Medium Statues | `StatueWizard` | wizard statue | `65.0..115.0` | 8 |  |
| Large Statues | `StatueDwarf` | dwarf statue | `75.0..125.0` | 16 |  |
| Large Statues | `StatueDesertGod` | god statue | `75.0..125.0` | 16 |  |
| Large Statues | `StatueHorseRider` | horse rider | `75.0..125.0` | 16 |  |
| Large Statues | `MediumStatueLion` | lion statue | `75.0..125.0` | 16 |  |
| Large Statues | `StatueMinotaurDefend` | minotaur statue, defend | `75.0..125.0` | 16 |  |
| Large Statues | `StatueMinotaurAttack` | minotaur statue, attack | `75.0..125.0` | 16 |  |
| Large Statues | `LargePegasusStatue` | pegasus statue | `75.0..125.0` | 16 |  |
| Large Statues | `StatueWomanWarriorPillar` | woman warrior statue | `75.0..125.0` | 16 |  |
| Huge Statues | `StatueAngelTall` | angel statue | `85.0..125.0` | 24 |  |
| Huge Statues | `StatueDaemon` | daemon statue, tall | `85.0..125.0` | 24 |  |
| Huge Statues | `LargeStatueLion` | lion statue | `85.0..125.0` | 24 |  |
| Huge Statues | `TallStatueLion` | lion statue, tall | `85.0..125.0` | 24 |  |
| Huge Statues | `StatueCapeWarrior` | warrior statue | `85.0..125.0` | 24 |  |
| Huge Statues | `StatueWiseManTall` | wise man statue | `85.0..125.0` | 24 |  |
| Huge Statues | `LargeStatueWolf` | wolf statue | `85.0..125.0` | 24 |  |
| Huge Statues | `StatueWomanTall` | woman statue | `85.0..125.0` | 24 |  |
| Giant Statues | `StatueGateGuardian` | gate guardian statue | `95.0..135.0` | 32 |  |
| Giant Statues | `StatueGuardian` | guardian statue | `95.0..135.0` | 32 |  |
| Giant Statues | `StatueGiantWarrior` | warrior statue | `95.0..135.0` | 32 |  |
| Tombstones | `StoneTombStoneA` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneB` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneC` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneD` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneE` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneF` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneG` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneH` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneI` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneJ` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneK` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneL` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneM` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneN` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneO` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneP` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneQ` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneR` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneS` | tombstone | `45.0..95.0` | 3 |  |
| Tombstones | `StoneTombStoneT` | tombstone | `45.0..95.0` | 3 |  |

## Direct Statue Items

Direct recipes inherit `BaseStatue`, which is an `Item` marked `[Furniture]`. Crafted direct items record:

| Field | Behavior |
| --- | --- |
| `Crafter` | Stored as a string. Added to object properties as `crafted by ~1_NAME~` when non-null and the item is not a gravestone item ID. |
| `Resource` | Stored as a string, defaulting to `Granite`. Added as `Made From <resource>` when the item is not a gravestone item ID. |
| `Hue` | Set from selected granite in `CraftItem.CompleteCraft`. |
| `Name` | Any direct `BaseStatue` can be renamed by double-clicking it and answering the `Prompt`. |

Tombstones are excluded from the crafter/resource property display by the hardcoded `BaseStatue.IsNotGraveStone` item-ID list, but they still inherit the double-click rename prompt.

## Addon Deed Assemblies

Large multi-tile results inherit `BaseStatueDeed`, which is a `BaseAddonDeed`. A crafted deed stores `StatueID`, `StatueColor`, `StatueMaterial`, `StatueMaker`, and `StatueName`. Its `Addon` property builds a `BaseStatueAddon` with the same values.

The concrete assembly classes in `Assemblies.cs` use `Weight` as a temporary statue ID before crafting finalization. For example, `StatueGateGuardian` sets `Weight = 1`, `StoneBenchLong` sets `Weight = 11`, and `StatueDaemon` sets `Weight = 43`. During `CraftItem.CompleteCraft`, `Statues.SetStatue` reads `(int)item.Weight` as the final `StatueID`, applies material/maker/name data, and resets `Weight = 10`.

`BaseStatueAddon` maps statue IDs `1..44` to hardcoded `AddonComponent` layouts. When chopped through `BaseAddon.OnChop`, the addon's `Deed` property recreates a `BaseStatueDeed` preserving the stored statue ID, color, material, maker, and name.

Placement uses standard `BaseAddonDeed` targeting: the deed must be in the caller's backpack, it creates the addon, checks `CouldFit`, moves it into the world on valid placement, adds it to the containing `BaseHouse.Addons`, and deletes the deed. Mayors inside their player city bypass the normal house fit result and have the addon added to `CityManagementStone.AddOns`.

## Serialization

| Class | Version written | Serialized data |
| --- | ---: | --- |
| `MalletAndChisel` | 0 | No local fields after `BaseTool`. |
| `MasonryBook` | 0 | No local fields after `Item`. |
| `StoneMiningBook` | 0 | No local fields after `Item`. |
| `StoneCrafter` | 0 | No local fields after `BaseVendor`; deserialize also renames old title `the stonecrafter` to `the stone crafter`. |
| `BaseGranite` | 1 | `CraftResource` as `int`; concrete granite subclasses then write their own version `0`. |
| `BaseStatue` | 3 | `m_Crafter`, then `m_Resource`. |
| `BaseStatueAddon` | 0 | `StatueID`, `StatueColor`, `StatueMaterial`, `StatueMaker`, `StatueName`. |
| `BaseStatueDeed` | 0 | `StatueID`, `StatueColor`, `StatueMaterial`, `StatueMaker`, `StatueName`. |
| Concrete stone classes | 0 | No local fields after the base class serialization. |

## Known Issues

1. `MasonryBook.OnDoubleClick` checks `pm == null || from.Skills[Carpentry] < 100.0`, but the error branch calls `pm.SendMessage(...)`. A non-`PlayerMobile` caller that reaches this branch can dereference `pm == null`.
2. Any direct `BaseStatue` item can be renamed by any caller who double-clicks it. The rename path does not check backpack containment, house ownership, access level, range, deletion state, or whether the item is a tombstone.
3. `BaseStatue.Serialize` writes version `3`, but `Deserialize` ignores the version and always reads `m_Crafter` and `m_Resource`. If older saved versions ever had a different field layout, the loader has no guarded migration path.
4. The multi-tile statue deed rotation helper is effectively dead. `Statues.FlipStatue` updates `StatueID` and direction text, but no call site invokes it; the inherited `[Flip]` command only applies `BaseStatueDeed`'s `[Flipable(0x32F0, 0x32F0)]`, which leaves the deed item ID unchanged and does not update `StatueID`.
5. Assembly deeds use `Item.Weight` as a hidden temporary statue ID before `CraftItem.CompleteCraft` resets it to `10`. This works for the current constructors, but it is a fragile coupling between craft finalization and item weight.
6. `BaseAddonDeed` placement casts `from` to `PlayerMobile` without a type guard before checking player-city mayor placement. Stone assembly deeds inherit that path.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0016; PBN-0024; PBN-0131.
- Backlog rows: RB-06779; RB-06780; RB-06781.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Trades/Crafting/DefMasonry.cs (CurrentFile)
- Data/Scripts/Items/Trades/Tools/MalletAndChisel.cs (CurrentFile)
- Data/Scripts/Items/Trades/Specialized/MasonryBook.cs (CurrentFile)
- Data/Scripts/Items/Trades/Specialized/StoneMiningBook.cs (CurrentFile)
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs (CurrentFile)
- Data/Scripts/Mobiles/Civilized/Vendors/StoneCrafter.cs (CurrentFile)
- Data/Scripts/Trades/Stone/BaseStatue.cs (CurrentFile)
- Data/Scripts/Trades/Stone/BaseStatueDeed.cs (CurrentFile)
- Data/Scripts/Trades/Stone/Assemblies.cs (CurrentFile)
- Data/Scripts/Trades/Stone/Furniture.cs (CurrentFile)
- Data/Scripts/Trades/Stone/Gravestones.cs (CurrentFile)
- Data/Scripts/Trades/Stone/Statues.cs (CurrentFile)
- Data/Scripts/Scripts.csproj (CurrentFile)

### Runtime Evidence

- Hook summary: Gump=2; Timer=1.
- Data/Scripts/Mobiles/Civilized/Vendors/StoneCrafter.cs:L86 Gump SendGump access=Internal
- Data/Scripts/Trades/Crafting/DefMasonry.cs:L77 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Trades/Stone/BaseStatue.cs:L137 Gump OnResponse access=Internal

### Serialization Evidence

- Serialized rows matched: 121.
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.AgapiteGranite version=0 serialize=L245 deserialize=L252 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.BaseGranite version=1 serialize=L22 deserialize=L29 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.BronzeGranite version=0 serialize=L187 deserialize=L194 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.CopperGranite version=0 serialize=L158 deserialize=L165 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.DullCopperGranite version=0 serialize=L100 deserialize=L107 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.DwarvenGranite version=0 serialize=L390 deserialize=L397 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.GoldGranite version=0 serialize=L216 deserialize=L223 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.Granite version=0 serialize=L71 deserialize=L78 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.MithrilGranite version=0 serialize=L361 deserialize=L368 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.NepturiteGranite version=0 serialize=L443 deserialize=L450 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.ObsidianGranite version=0 serialize=L332 deserialize=L339 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs:Server.Items.ShadowIronGranite version=0 serialize=L129 deserialize=L136 alignment=AlignedByCountAndKnownTypes
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs=Keep
- Data/Scripts/Items/Trades/Resources/Masonry/Granite.cs=Keep
- Data/Scripts/Items/Trades/Specialized/MasonryBook.cs=Keep
- Data/Scripts/Items/Trades/Specialized/MasonryBook.cs=Keep
- Data/Scripts/Items/Trades/Specialized/StoneMiningBook.cs=Keep
- Data/Scripts/Items/Trades/Specialized/StoneMiningBook.cs=Keep
- Data/Scripts/Items/Trades/Tools/MalletAndChisel.cs=Keep
- Data/Scripts/Items/Trades/Tools/MalletAndChisel.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/StoneCrafter.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/StoneCrafter.cs=Keep
- Data/Scripts/Trades/Crafting/DefMasonry.cs=Keep
- Data/Scripts/Trades/Crafting/DefMasonry.cs=Keep
- Data/Scripts/Trades/Stone/Assemblies.cs=Keep
- Data/Scripts/Trades/Stone/Assemblies.cs=Keep
- Data/Scripts/Trades/Stone/BaseStatue.cs=Keep
- Data/Scripts/Trades/Stone/BaseStatue.cs=Keep
- Data/Scripts/Trades/Stone/BaseStatueDeed.cs=Keep
- Data/Scripts/Trades/Stone/BaseStatueDeed.cs=Keep
- Data/Scripts/Trades/Stone/Furniture.cs=Keep
- Data/Scripts/Trades/Stone/Furniture.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
