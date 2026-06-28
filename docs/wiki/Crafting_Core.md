# Crafting Core

## Overview
The crafting core lives in `Server.Engines.Craft` and provides the shared RunUO machinery used by the concrete `Def*` crafting systems. `BaseTool.OnDoubleClick` is the normal player entry point: the tool must be in the player's backpack or equipped, macro-resource CAPTCHA may gate the action, the tool's `CraftSystem.CanCraft` result is checked, and `CraftGump` is opened when crafting is allowed.

The core does not define the blacksmithing, tailoring, carpentry, or other recipe lists by itself. Those are supplied by subclasses of `CraftSystem` through `InitCraftList`, `GetChanceAtMin`, `PlayCraftEffect`, `PlayEndingEffect`, and `CanCraft`.

## Core Scripts
| Script | Role |
| --- | --- |
| `Data/Scripts/Items/Trades/Tools/BaseTool.cs` | Item entry point, tool use count, tool quality, crafter persistence, and `ICraftable` implementation for crafted tools. |
| `Data/Scripts/Trades/Core/CraftSystem.cs` | Abstract craft catalog, groups, items, sub-resources, per-`Mobile` context table, and shared registration helpers. |
| `Data/Scripts/Trades/Core/CraftContext.cs` | Runtime per-`Mobile` state for last resources, last group, color toggle, maker's mark option, and last ten crafted items. |
| `Data/Scripts/Trades/Core/CraftItem.cs` | Resource checks, resource consumption, skill chance math, craft timer, item creation, maker's mark, post-processing, magic item roll, faction imbuing, and tool durability loss. |
| `Data/Scripts/Trades/Core/Gumps/CraftGump.cs` | Main category, selection, last-ten, resource, repair, resmelt, enhance, and maker's mark option gump. |
| `Data/Scripts/Trades/Core/Gumps/CraftGumpItem.cs` | Item detail gump with required skills, materials, success chance, exceptional chance, recipe lockout, expansion requirement, and make button. |
| `Data/Scripts/Trades/Core/Gumps/QueryMakersMarkGump.cs` | Post-success prompt for applying or skipping a maker's mark. |
| `Data/Scripts/Trades/Core/Recipes.cs` | Static recipe registry plus staff commands for teaching or forgetting all recipes. |
| `Data/Scripts/Trades/Core/Repair.cs` | Repair targeting for weapons, armor, clothing, golems, and repair deed creation. |
| `Data/Scripts/Trades/Core/Resmelt.cs` | Metal item recycling target flow. |
| `Data/Scripts/Trades/Core/Enhance.cs` | Special material enhancement target flow for weapons and armor. |
| `Data/Scripts/Trades/Core/CustomCraft.cs` | Abstract delayed custom-craft hook for craft entries that need their own targeting or completion logic. |

## Player Entry Flow
1. The player double-clicks a `BaseTool`.
2. If `MyServerSettings.AllowMacroResources()` is false, `CaptchaGump.sendCaptcha` redirects back into `BaseTool.OnDoubleClickRedirected`.
3. The tool must be in the player's backpack or equipped as the item parent.
4. The concrete `CraftSystem` is taken from the tool's `CraftSystem` property.
5. `CraftSystem.CanCraft(from, tool, null)` is checked.
6. If `CanCraft` returns a positive localized message other than the special blacksmithing anvil/forge case under `Core.SE`, the message is sent instead of opening the gump.
7. Otherwise `CraftGump` opens.

## Craft Catalog Model
| Type | Stored data | Notes |
| --- | --- | --- |
| `CraftSystem` | `CraftItemCol`, `CraftGroupCol`, `CraftSubResCol`, `CraftSubResCol2`, options for resmelt, repair, maker's marks, and enhance | Constructed with min/max craft effect count and per-tick delay, then immediately calls `InitCraftList`. |
| `CraftGroup` | Group name plus `CraftItemCol` | `CraftSystem.AddCraft` automatically creates or reuses the group by `TextDefinition`. |
| `CraftItem` | Item type, group/name text, required resources, required skills, optional recipe, optional hit/mana/stamina costs, expansion gate, sub-resource selector, and special flags | This is the unit actually crafted by the gumps. |
| `CraftRes` | Resource type, display name, amount, and missing-resource message | The first matching resource type can be mutated to the selected sub-resource. |
| `CraftSkill` | Skill name plus min/max skill | Success chance is calculated from the main skill while all listed skills are passively checked for gains. |
| `CraftSubRes` | Alternate resource type, display name, required main skill, generic name, and rejection message | Used for materials such as special ingots, leather, scales, wood, or any concrete craft system's custom material list. |
| `CraftContext` | Last resource index, second resource index, last group index, `DoNotColor`, `CraftMarkOption`, and last ten items | Stored only in the `CraftSystem` runtime dictionary keyed by `Mobile`; it is not serialized. |

## Main Gump Behavior
`CraftGump` has fixed button ID packing: `buttonID = 1 + type + (index * 7)`. The response handler reverses that into `type = (ButtonID - 1) % 7` and `index = (ButtonID - 1) / 7`.

| Button type | Meaning |
| --- | --- |
| `0` | Select a craft group and save it to `CraftContext.LastGroupIndex`. |
| `1` | Craft an item from the current group. |
| `2` | Open the item detail gump for an item from the current group. |
| `3` | Craft an item from the last-ten list. |
| `4` | Open item details for an item from the last-ten list. |
| `5` | Select a resource from the current resource-selection page. |
| `6` | Miscellaneous actions: resource list, resmelt, make last, last ten, color toggle, repair, maker's mark toggle, second resource list, or enhance. |

The main gump always shows categories, selections, notices, exit, and make-last. Optional buttons appear only when the concrete `CraftSystem` has enabled `MarkOption`, `Resmelt`, `Repair`, `CanEnhance`, `CraftSubRes.Init`, or `CraftSubRes2.Init`.

`CraftContext.OnMade` maintains the last-ten list by removing the item if already present, trimming the list to ten entries, then inserting the new item at the front.

## Item Detail Gump
`CraftGumpItem` closes the main gump and shows one item. It displays:
- item graphic from `CraftItem.ItemIDOf`
- required skills and displayed minimum values
- success chance
- exceptional chance only when `DrawItem` enables `m_ShowExceptionalChance`; in the current code this display path appears unreachable, as noted in Known Issues
- paged material requirements, four resource rows per page
- retained-color note when applicable
- required expansion note
- learned-recipe lockout
- "makes as many as possible" note for `UseAllRes`

If the player has not learned the recipe for a recipe-gated item, the detail gump greys out the make button. A direct craft attempt still rechecks recipe ownership before starting the craft timer.

## Resource And Requirement Checks
| Requirement | Code behavior |
| --- | --- |
| Backpack | `CraftItem.ConsumeRes` immediately fails when `from.Backpack` is null. |
| Heat source | `NeedHeat` requires one of the hardcoded heat item/static ID ranges within two tiles and overlapping Z. |
| Oven | `NeedOven` uses its own hardcoded oven item/static ID ranges. |
| Mill | `NeedMill` uses hardcoded mill item/static ID ranges. |
| Sub-resource | If a required resource type equals the active `CraftSubResCol.ResType` and the gump selected a type, the required resource mutates to that selected type. |
| Sub-resource skill | The player's main skill must meet the selected `CraftSubRes.RequiredSkill`; otherwise that sub-resource's message is shown. |
| Equivalent resource groups | Logs/boards, leather/hides, cloth/uncut cloth, blank maps/scrolls, some food containers, and similar pairs are consumed through grouped type arrays. |
| `IHasQuantity` resources | Quantity-based resources use `ConsumeQuantity` and `GetQuantity`; non-water `BaseBeverage` entries are ignored. |
| Hit/mana/stamina costs | `ConsumeAttributes` first verifies the `Mobile` has enough of each required pool, then subtracts them only when called with `consume = true`. |

Resource hue retention is based on the consumed resource stack that supplies the largest consumed amount. The hue is only retained for item/resource combinations allowed by `RetainsColorFrom`, unless the player has toggled `CraftContext.DoNotColor` from the resource page.

`UseAllRes` changes the output amount to the maximum craftable amount. On success, all required resources are multiplied by that maximum. On failure, the consume mode becomes `Half`, with each resource amount divided by two and floored to at least one.

## Skill And Quality Math
The core success formula is:

```text
chance = craftSystem.GetChanceAtMin(item)
       + ((mainSkillValue - minMainSkill) / (maxMainSkill - minMainSkill))
       * (1.0 - craftSystem.GetChanceAtMin(item))
```

If any required skill is below its minimum, `allRequiredSkills` is false and chance becomes `0.0`. If all required skills pass and the main skill equals the max skill, chance is forced to `1.0`. While starting a craft, skill gains are disabled for the preview check; during completion, every listed skill receives a passive `from.CheckSkill(skill, min, max)` call when `gainSkills` is true.

Exceptional chance starts from the success chance and applies the concrete system's `CraftECA` mode:

| `CraftECA` | Exceptional adjustment |
| --- | --- |
| `ChanceMinusSixty` | `chance - 0.6` |
| `FiftyPercentChanceMinusTenPercent` | `(chance * 0.5) - 0.1` |
| `ChanceMinusSixtyToFourtyFive` | `chance - offset`, where `offset = 0.60 - ((mainSkill - 95.0) * 0.03)` clamped between `0.45` and `0.60` |

If `CraftItem.ForceNonExceptional` is set, exceptional chance is always `0.0`.

## Craft Timer And Completion
Starting a craft requires `from.BeginAction(typeof(CraftSystem))`. The delay loop uses `InternalTimer` with a random tick count between the concrete system's min and max craft effect values, then calls `PlayCraftEffect` on each non-final tick.

On the final tick:
1. `EndAction(typeof(CraftSystem))` is called.
2. `CanCraft` is checked again.
3. The skill check is rerun without additional skill gain.
4. `CustomCraft` entries are instantiated and handed to `EndCraftAction`; otherwise the normal completion path continues.
5. Exceptional quality can request a maker's mark only when the main skill is at least `100.0` and the item type is markable.
6. `QueryMakersMarkGump` is shown only when `CraftContext.MarkOption` is `PromptForMark`.
7. `CraftItem.CompleteCraft` consumes resources and attributes, decrements the tool, creates the item or `IndecipherableMap` fallback for invalid map areas, applies `ICraftable.OnCraft`, applies hue, runs shard-specific post-processing, places the item in the backpack, logs staff crafting, calls `PlayEndingEffect`, and may offer faction imbuing for `IFactionItem`.

If the final skill check fails, `ConsumeOnFailure` decides whether each resource type is lost, `UseAllRes` failures consume half, tool durability still drops by one, and `PlayEndingEffect` is called with `failed = true`.

## Maker's Marks
`CraftMarkOption` has three states:

| State | Behavior |
| --- | --- |
| `MarkItem` | Exceptional markable items are marked automatically. |
| `DoNotMark` | Exceptional markable items are not marked. |
| `PromptForMark` | `QueryMakersMarkGump` asks whether to mark before calling `CompleteCraft`. |

Markable types include armor, weapons, clothing, instruments, dragon barding deeds, tools, harvest tools, fukiya darts, shuriken, spellbooks, runebooks, and quivers. `BaseMagicStaff` is explicitly excluded, and `ForceNonExceptional` also hides maker's mark handling.

## Recipes And Staff Commands
`Recipe.Initialize` registers two GameMaster commands.

| Command | Access | Usage | Behavior |
| --- | --- | --- | --- |
| `LearnAllRecipes` | `GameMaster` | `[LearnAllRecipes` | Prompts for a target. If the target is a `PlayerMobile`, every key in the static recipe dictionary is passed to `AcquireRecipe`. |
| `ForgetAllRecipes` | `GameMaster` | `[ForgetAllRecipes` | Prompts for a target. If the target is a `PlayerMobile`, `ResetRecipes` is called. |

Recipe IDs are globally unique. Constructing a duplicate `Recipe` throws an exception. `Recipe.LargestRecipeID` tracks the highest ID inserted into the static dictionary.

## Repair
`Repair.Do` assigns a target and supports either a live `BaseTool` path or a `RepairDeed` path.

| Target | Behavior |
| --- | --- |
| `Golem` with tinkering | Requires at least `60.0` tinkering or deed skill, consumes iron ingots from the backpack, heals up to a skill-scaled amount, and starts a 12-second golem repair action cooldown. |
| `BaseWeapon` | Must be craftable by the current system or accepted by hardcoded special-case repair rules, must be in backpack, must be damaged, and may lose max durability before the repair attempt. |
| `BaseArmor` | Same repair structure as weapons, with armor-specific special cases. |
| `BaseClothing` | Same repair structure as weapons, with clothing-specific special cases. |
| `BlankScroll` without deed | Requires at least `50.0` in the craft system's main skill, consumes one blank scroll, and creates a `RepairDeed` in the backpack. |

Repair formulas:

| Formula | Code |
| --- | --- |
| Weaken chance | `(40 + (maxHits - curHits)) - (skill / 10)` where `skill` is deed skill or current skill value. |
| Repair difficulty | `(((maxHits - curHits) * 1250) / Math.Max(maxHits, 1)) - 250`, then multiplied by `0.1` for the skill window. |
| Deed success window | deed skill below `difficulty - 25.0` fails; at or above `difficulty + 25.0` succeeds; otherwise chance is `(skill - minSkill) / (maxSkill - minSkill)`. |
| Live repair skill check | `mob.CheckSkill(skill, difficulty - 25.0, difficulty + 25.0)`. |

Repair deeds are deleted only when `toDelete` is set after an actual repair or repair failure on a supported target.

## Resmelt
`Resmelt.Do` assigns a two-tile target when the craft system allows recycling. The target path supports `BaseArmor`, `BaseWeapon`, and `DragonBardingDeed`.

Resmelt requires:
- the item's `CraftResource` type to be metal
- the resource info to have at least one resource type
- the item type to exist in the active `CraftSystem.CraftItems`
- the first craft resource amount to be at least `2`
- the player's `Mining` skill to meet the metal difficulty

| Metal resource | Mining difficulty |
| --- | --- |
| Dull Copper | `65.0` |
| Shadow Iron | `70.0` |
| Copper | `75.0` |
| Bronze | `80.0` |
| Gold | `85.0` |
| Agapite | `90.0` |
| Verite | `95.0` |
| Valorite, Nepturite, Obsidian, Steel | `99.0` |
| Brass | `105.0` |
| Mithril | `110.0` |
| Xormite | `115.0` |
| Dwarven | `120.0` |

On success, the original item is deleted and the player receives `craftResource.Amount / 2` ingots, floored to at least one.

## Enhance
Enhancement is enabled only for craft systems with `CanEnhance = true`. `Enhance.BeginTarget` requires a selected primary sub-resource whose type maps to a non-`None` `CraftResource`.

`Enhance.Invoke` accepts only `BaseArmor` and `BaseWeapon` targets in the player's backpack. It rejects levelable items, arcane equipment that is currently arcane, standard resources, already enhanced items, unknown craft entries, missing resource attributes, insufficient skill, and insufficient resources.

Enhancement result logic:

| Outcome | Resource effect | Item effect |
| --- | --- | --- |
| `Success` | Consumes all required resources | Sets weapon or armor resource to the selected special material; weapons may take elemental hue. |
| `Failure` | Consumes half the required resources | Item remains. |
| `Broken` | Consumes half the required resources | Item is deleted. |

Each applicable material bonus calls `CheckResult`. If the result is still success, a random number from `0` to `99` is rolled. Rolls below `10` become `Failure`; otherwise a roll below the supplied chance becomes `Broken`.

The base chance starts at `20`. At main skill `100.0` or higher, it is reduced by `(skill - 90) / 10`, where `skill` is the fixed skill value divided by ten. Existing item properties add to the break chance: resists, durability, luck, lower requirements, and weapon damage all feed separate `CheckResult` calls when that material grants the matching bonus.

## Persistence
The craft catalog, `CraftItem` objects, `CraftContext`, and recipe registry are runtime structures and do not serialize themselves into the world save.

`BaseTool` is the persistent item layer for normal crafting tools. Its version `1` serializer writes:
- `Crafter`
- `ToolQuality`
- `UsesRemaining`

Version `0` tools only stored `UsesRemaining`; version `1` falls through to read the old value after reading crafter and quality.

## Known Issues
- `CraftGump` reopens `CraftContext.LastGroupIndex` whenever it is greater than `-1`, but `CreateItemList` calls `CraftGroupCol.GetAt(selectedGroup)` without checking that the stale index is still below `CraftGroups.Count`. A stale context can throw instead of showing the gump.
- `CraftGumpItem` checks only `resIndex > -1` before calling `res.GetAt(resIndex)` in success-chance rendering, material rendering, and make-button handling. A stale resource index can throw if the active sub-resource list is shorter than the saved context index.
- `CraftGumpItem.DrawItem` sets `m_ShowExceptionalChance` only inside an `else if` that can run only when `type == typeof(BaseMagicStaff)`, but `CraftItem.IsMarkable` returns false for that exact type. The item detail gump therefore does not appear able to show exceptional chance.
- `Repair.OnTarget` calls `targeted.GetType()` before checking whether `targeted` is null or an `Item`, so a null target object can dereference before the normal invalid-target messages.
- `CraftItem.EnchantItem` casts `Mobile from` directly to `PlayerMobile`; any non-player `Mobile` that reaches magical crafting can throw before guild and skill bonuses are calculated.
- `Enhance.Invoke` consumes one equipped `AncientSmithyHammer` use for blacksmith enhancement before outcome resolution and without checking that the hammer is the actual tool passed into the target flow. This differs from the normal crafting path, which guards the extra ancient hammer decrement with `hammer != tool`.
