# Librarian Trade

## Overview
The librarian trade is an Inscribe-based `HarvestSystem` in `Server.Engines.Harvest`. Players use a `Monocle` `Item`, target static library/book tiles in dangerous regions, and the system rolls harvest-bank resources, optional ML bonus resources, and a secondary shelf/book reward path.

There are no custom `CommandSystem.Register` handlers, `[Usage]` attributes, `Gump` classes, `NetState` packet handlers, `EventSink` hooks, or XMLSpawner attachments in the traced Librarian scripts. Normal entry is item double-click targeting. Administration uses ordinary RunUO construction such as `[add Monocle]`, `[add LibraryScroll1]`, `[add LibraryScroll6]`, or targeted construction of the underlying reward items.

## Core Scripts
| Script | Role |
| --- | --- |
| `Data/Scripts/Trades/Librarian/Librarian.cs` | `HarvestSystem` singleton, harvest definition, library tile whitelist, resource/vein tables, target restrictions, and secondary shelf/book reward logic. |
| `Data/Scripts/Trades/Librarian/Monocle.cs` | `Shovel` subclass whose `HarvestSystem` property returns `Librarian.System`; default tool has `50` uses. |
| `Data/Scripts/Trades/Librarian/LibraryScrolls.cs` | Six `UnknownScroll` subclasses used as higher-skill librarian harvest results. |
| `Data/Scripts/Items/Unknown/UnknownScroll.cs` | Base unidentified scroll item; serializes `ScrollLevel`, `ScrollType`, and `ScrollOrigin`, and delegates double-click identification to `ItemIdentification.IDItem`. |
| `Data/Scripts/Trades/Harvest/HarvestSystem.cs` | Shared harvest timing, target handling, skill checks, item construction, delivery, tool wear, and bonus-resource handling. |
| `Data/Scripts/Trades/Harvest/HarvestDefinition.cs` | Harvest-bank lookup, tile validation, weighted vein selection, bonus-resource selection, and bank map. |
| `Data/Scripts/Trades/Harvest/HarvestBank.cs` | Per-map/per-cell resource count, respawn timer, and vein reset behavior. |

## Player Entry Flow
1. The player double-clicks a `Monocle`.
2. The inherited harvest-tool path calls `Librarian.System.BeginHarvesting(from, tool)`.
3. The shared harvest system checks tool wear, then either assigns a `HarvestTarget` or sends a captcha first depending on `MyServerSettings.AllowMacroResources()`.
4. The player targets a static library/book tile.
5. `HarvestTarget.OnTarget` calls `Librarian.StartHarvesting`.
6. `Librarian` accepts only `StaticTarget` objects whose tile ID validates against `m_LibraryTiles`.
7. The player must be in a `DungeonRegion`, `DeadRegion`, `CaveRegion`, `BardDungeonRegion`, or `OutDoorBadRegion`.
8. The player cannot search while mounted.
9. The player cannot search while body-modded into a non-human body when `RaceID < 1`.
10. The harvest timer plays action `4`, sound `0x55` or `0x4F`, then resolves the resource and post-finish shelf reward.

## Harvest Definition
| Setting | Compiled value |
| --- | --- |
| Harvest bank size | `1 x 1` tile. |
| Bank total | Random `1..2` resources per bank. |
| Bank respawn | Random `50..70` minutes after first consumption. |
| Skill | `SkillName.Inscribe`. |
| Target range | `1` tile. |
| Normal consumed amount | `1 * MyServerSettings.Resources()`. |
| Isles of Dread consumed amount | Normal consumed amount plus half the normal amount plus `2`. |
| Effect action | `4`. |
| Effect sounds | `0x55`, `0x4F`. |
| Effect count | `1`. |
| Effect delay | `0.0` seconds. |
| Effect sound delay | `0.1` seconds. |
| No resource/fail message | Localized `501756` (`Nothing worth taking`). |
| Out-of-range message | Localized `500446`. |
| Pack full message | Localized `500720`. |
| Tool broke message | Localized `1044038`. |
| Vein randomization | `Core.ML`; otherwise vein choice is deterministic from map/x/y. |

The shared `HarvestBank` consumes the delivered item amount. Stackable `BlankScroll` results receive a randomized amount. Non-stackable book and scroll results keep their constructed item amount. When the `Monocle` succeeds through the shared item-delivery path, its `UsesRemaining` is decremented and the tool is deleted at zero uses.

## Tool Acquisition
| Source | Compiled behavior |
| --- | --- |
| Construction/admin | `[add Monocle]` creates a `Monocle` with `50` uses. `[add Monocle 25]` uses the constructable integer constructor. |
| Glassblowing craft list | Adds `Monocle` under category `1044050`, named `monocle`, with skill range `5.0..55.0`, consuming `1` `Sand`. |
| Vendor stock | Glass-related vendor lists can sell `Monocle` for `24` gold and buy it for `12` gold, subject to server sell/buy chance gates in some lists. |
| Dungeon tools loot table | `Monocle` appears in `DungeonLoot.DungeonToolsTypes`. |

## Primary Resources
Every primary result uses `SkillName.Inscribe`. `ReqSkill` is checked against base skill. The success roll uses `from.CheckSkill(SkillName.Inscribe, MinSkill, MaxSkill)` after fallback selection.

The "effective primary chance" column is the vein weight after fallback chance, before `CheckSkill`, base-skill gates, bank depletion, and target restrictions.

| Result type | ReqSkill | Skill check | Vein weight | Fallback chance | Effective primary chance |
| --- | ---: | --- | ---: | ---: | ---: |
| `BlankScroll` | `0.0` | `0.0..100.0` | `45%` | `0%` | `45%` |
| `BlueBook` | `65.0` | `25.0..105.0` | `15%` | `50%` | `7.5%` |
| `SomeRandomNote` | `70.0` | `30.0..110.0` | `11%` | `50%` | `5.5%` |
| `ScrollClue` | `75.0` | `35.0..115.0` | `8%` | `50%` | `4%` |
| `LibraryScroll1` | `80.0` | `40.0..120.0` | `6%` | `50%` | `3%` |
| `LibraryScroll2` | `85.0` | `45.0..125.0` | `5%` | `50%` | `2.5%` |
| `LibraryScroll3` | `90.0` | `50.0..130.0` | `4%` | `50%` | `2%` |
| `LibraryScroll4` | `95.0` | `55.0..135.0` | `3%` | `50%` | `1.5%` |
| `LibraryScroll5` | `99.0` | `59.0..139.0` | `2%` | `50%` | `1%` |
| `LibraryScroll6` | `100.1` | `69.0..140.0` | `1%` | `50%` | `0.5%` |

All fallback resources are `BlankScroll`. A player whose current effective Inscribe skill is below the selected primary resource's `ReqSkill` or `MinSkill` receives the fallback resource before the skill check is made.

## Stackable Amounts
`BlankScroll` is the only stackable primary reward in this definition. Its amount starts from `ConsumedPerHarvest`, then is randomized to:

| Area | Amount formula |
| --- | --- |
| Normal area | `RandomMinMax(ConsumedPerHarvest, ConsumedPerHarvest + (Inscribe.Value / 10))`. |
| `Worlds.GetMyWorld(...) == "the Isles of Dread"` | `ConsumedPerIslesDreadHarvest`, overriding the earlier randomized blank-scroll amount. |

The code declares `bool inIslesDread = (map == Map.IslesDread);` but does not use that variable.

## ML Bonus Resources
When `Core.ML` is enabled, `Librarian` installs this bonus-resource table. The shared harvest system rolls it after a successful primary item delivery and only constructs a non-null bonus item when base Inscribe is at least the bonus `ReqSkill`.

| Bonus result | ReqSkill | Chance |
| --- | ---: | ---: |
| Nothing | `0.0` | `80%` |
| `LoreBook` | `40.0` | `10%` |
| `DDRelicScrolls` | `60.0` | `5%` |
| `DDRelicBook` | `60.0` | `5%` |

The inherited harvest code increases `DDRelicScrolls.RelicGoldValue` and `DDRelicBook.RelicGoldValue` by `RandomMinMax(1, (int)(Inscribe.Value * 2))` when those items are delivered through the normal item classification path.

## Library Scroll Results
`LibraryScroll1` through `LibraryScroll6` all derive from `UnknownScroll`. Each constructor uses the same scroll-type distribution:

| ScrollType | Label used in origin text | Cases | Chance |
| --- | --- | ---: | ---: |
| `1` | `magery` | `0..5` | `6 / 11` |
| `2` | `necromancy` | `6..7` | `2 / 11` |
| `3` | `bardic` | `8` | `1 / 11` |
| `7` | `elemental` | `9..10` | `2 / 11` |

All six classes set `ScrollLevel = 1`, so `ScrollOrigin` always uses `a plainly written ... scroll` wording. The `ItemIdentification` path explicitly sets the identification `bonus` back to `0` for `LibraryScroll1` through `LibraryScroll6`, so player Inscribe skill does not raise their scroll level during self-identification.

## Secondary Shelf Reward
After the shared harvest code finishes, `Librarian.OnHarvestFinished` runs a second reward path for `StaticTarget` objects.

This path only runs when:

| Gate | Compiled behavior |
| --- | --- |
| Target object | Must be a `StaticTarget`. |
| Excluded item IDs | Raw target item ID must not be `0x0C16`, `0x12F3`, `0x12FF`, `0x1305`, `0x130B`, `0x1311`, `0x1317`, `0x131D`, `0x134E`, `0x1398`, `0x1399`, or `0x1E20..0x1E25`. |
| Special remaps | Raw `0x3084` or `0x3085` becomes `0x2DEF`; raw `0x3086` or `0x3087` becomes `0x2DF0`. |
| Resource check | The selected harvest resource must still equal the vein's primary resource. |
| Map check | `from.Map` must be non-null. |
| Search roll | `(int)Inscribe.Value > Utility.Random(5000)`. |
| Skill roll | `from.CheckSkill(SkillName.Inscribe, 0, 125)`. |

The search roll is roughly `Inscribe.Value / 5000` before the second skill check. For example, `100.0` displayed Inscribe gives `100 / 5000`, or `2%`, before the `CheckSkill(0, 125)` call.

When it passes, the code first creates a `BasicShelf`, names it `book shelf`, assigns the target-derived `ItemID`, and applies hue `0xABE` when the remapped `itemID >= 0x4FDB`. It then rolls `Utility.Random(100)`:

| Roll case | Reward | Displayed message name |
| ---: | --- | --- |
| Default (`12..99`) | `BasicShelf` using the target-derived item ID | `a book shelf` |
| `0` | `MyNecromancerSpellbook` | `necromancer spellbook` |
| `1` | `MySpellbook` | `a magery spellbook` |
| `2` | `MyNinjabook` | `a book of the ninja` |
| `3` | `MySamuraibook` | `a book of the samurai` |
| `4` | `MyPaladinbook` | `a book of knightship` |
| `5` | `MySongbook` | `a book of bardic songs` |
| `6` | `ArtifactManual` | `an artifact manual` |
| `7` | `DDRelicTablet`, with `RelicGoldValue += RandomMinMax(1, (int)(Inscribe.Value * 2))` | `a stone tablet` |
| `8` | `PowerScroll.RandomPowerScroll()` | `a scroll of power` |
| `9` | One of six boat deeds with equal `1 / 6` sub-rolls | `a deed to a ship` |
| `10` | `TreasureMap` at the player's current map/location | `a treasure map` |
| `11` | `MyElementalSpellbook` | `elemental spellbook` |

The treasure map roll uses this weighted level list:

| Treasure map level | Weight | Chance inside case `10` |
| ---: | ---: | ---: |
| `1` | `48` | `48 / 94` |
| `2` | `24` | `24 / 94` |
| `3` | `12` | `12 / 94` |
| `4` | `6` | `6 / 94` |
| `5` | `3` | `3 / 94` |
| `6` | `1` | `1 / 94` |

## Target Tile IDs
`Librarian.Initialize()` sorts this exact tile whitelist before harvest validation.

| Tile IDs |
| --- |
| `0x4A97`, `0x4A98`, `0x4A99`, `0x4A9A`, `0x4A9B`, `0x4A9C` |
| `0x4C14`, `0x4C15`, `0x4C16` |
| `0x52F3`, `0x52FF`, `0x5305`, `0x530B`, `0x5311`, `0x5317`, `0x531D`, `0x534E`, `0x5398`, `0x5399` |
| `0x59FF`, `0x5A00`, `0x5E20`, `0x5E21`, `0x5E22`, `0x5E23`, `0x5E24`, `0x5E25` |
| `0x6DEF`, `0x6DF0`, `0x7084`, `0x7085`, `0x7086`, `0x7087` |
| `0x7BF9`, `0x7BFA`, `0x7BFB`, `0x7BFC`, `0x7BFD`, `0x7BFE` |
| `0x7C15`, `0x7C16`, `0x7C2B`, `0x7C2C`, `0x7C2D`, `0x7C2E`, `0x7C33`, `0x7C34` |
| `0x7C5F`, `0x7C60`, `0x7C61`, `0x7C62`, `0x7C73`, `0x7C74`, `0x7C79`, `0x7C7A` |
| `0x7CA5`, `0x7CA6`, `0x7CA7`, `0x7CA8`, `0x7CAF`, `0x7CB0`, `0x7CDB`, `0x7CDC` |
| `0x7CEB`, `0x7CEC`, `0x7CED`, `0x7CEE`, `0x7CFD`, `0x7CFE`, `0x7D05`, `0x7D06` |
| `0x9004`, `0x9005`, `0x900C`, `0x900D`, `0x9012`, `0x9013`, `0x902C`, `0x902D`, `0x9038`, `0x9039`, `0x903A`, `0x903B` |
| `0x5004`, `0x5005`, `0x500C`, `0x500D`, `0x5012`, `0x5013`, `0x502C`, `0x502D`, `0x5038`, `0x5039`, `0x503A`, `0x503B` |

## Serialization
| Type | Serialized behavior |
| --- | --- |
| `Librarian` | No instance serialization; the singleton definition and harvest-bank cache are rebuilt in memory. |
| `Monocle` | Calls `base.Serialize(writer)`, then writes version `0`. Deserialization calls base and reads the version. Custom fields are not added by `Monocle`; use count is inherited. |
| `LibraryScroll1` through `LibraryScroll6` | Override `Serialize` and `Deserialize`, but only call the base `UnknownScroll` methods. |
| `UnknownScroll` | Writes version `0`, then `ScrollLevel`, `ScrollType`, and `ScrollOrigin`; deserialization reads the same fields and normalizes invalid `ItemID` values back to `0x4CC4` or `0x4CC5`. |

## Known Issues
| Issue | Impact |
| --- | --- |
| `LibraryScroll1` through `LibraryScroll6` all set `ScrollLevel = 1`. | Higher-tier librarian scroll classes require higher harvest skill, but they identify as level-1 scrolls. Because `ItemIdentification` resets the Inscribe identification bonus to `0` for library scrolls, player skill cannot raise these harvested library scrolls above their fixed level. |
| `LibraryScroll1` through `LibraryScroll6` override serialization without writing a subclass version. | Current fields survive through `UnknownScroll`, but the subclasses do not follow the local RunUO version-in-every-override convention and would need care before adding subclass fields. |
| `Librarian.OnHarvestFinished` catches all exceptions with an empty `catch { }`. | Failures in the secondary shelf reward path are silently hidden, making broken reward constructors or backpack insertion failures difficult to diagnose. |
| The secondary shelf reward path can run after the shared harvest code reports a failed primary item roll. | `HarvestSystem.FinishHarvesting` calls `OnHarvestFinished` even when `type == null`, so the shelf reward can be granted without primary item delivery, bank consumption, or tool wear if its independent search and skill rolls pass. |
| Inherited ML bonus-resource failure deletes the primary item instead of the failed bonus item. | If a bonus resource is constructed but cannot be placed, `HarvestSystem` calls `item.Delete()` rather than deleting `bonusItem`; this affects Librarian's ML `LoreBook`, `DDRelicScrolls`, and `DDRelicBook` bonus table. |
| `m_Offsets` and `inIslesDread` are dead code. | These hardcoded values do not affect runtime behavior and appear to be leftovers from earlier harvest logic. |
