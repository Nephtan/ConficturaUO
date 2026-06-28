# Mage Spellcasting Tips

## Scope

This page documents the player-facing wizardry tutorial and wand-charging service exposed by the `Server.Mobiles.Mage` vendor `Mobile`.
It covers the mage context-menu `SpeechGump`, the `"Mage"` branch of `SpeechFunctions.SpeechText()`, the concrete spell behaviors named by that text, the mage `BaseMagicStaff` charge service, and the Magery spell-bar commands referenced by the tutorial.

This is not a new spell engine. It does not register packet handlers, EventSink hooks, XMLSpawner attachments, or standalone admin commands.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Civilized/Vendors/Mage.cs` | Mage vendor `Mobile`, skill profile, merchant stock registration, tutorial context-menu entry, wand-charge context-menu entry, charge target, and serialization. |
| `Data/Scripts/System/Misc/Talk.cs` | Shared `SpeechGump` and `SpeechFunctions.SpeechText(..., "Mage")` text provider. |
| `Data/Scripts/System/Commands/Player/MyChat.cs` | Marks the `"Mage"` conversation as memorized in the player's chat log. |
| `Data/Scripts/Items/Wands/BaseMagicStaff.cs` | Shared magic-wand `Item` base with `Charges`, use delay, equip/double-click gates, and serialization. |
| `Data/Scripts/Items/Wands/` | Concrete `BaseMagicStaff` subclasses for Magery spells and circle-specific intelligence requirements. |
| `Data/Scripts/Magic/Magery/Magery 1st/CreateFood.cs` | Implements the food/drink behavior named in the mage tutorial. |
| `Data/Scripts/Magic/Magery/Magery 2nd/RemoveTrap.cs` | Implements trapped-container removal and summoned `TrapWand` behavior. |
| `Data/Scripts/Magic/Magery/Magery 3rd/Telekinesis.cs` | Implements telekinesis container use and loose-item pickup behavior. |
| `Data/Scripts/Magic/Magery/Magery 6th/Reveal.cs` | Implements hidden mobile, trap, hidden-door, and hidden-chest reveal behavior. |
| `Data/Scripts/Magic/Base/Spell.cs` | Enforces the Magery/Elementalism conflict described by the tutorial. |
| `Data/Scripts/Magic/Base/ForgetfulGem.cs` | Implements cave crystals that can zero Magery, Necromancy, or Elementalism skill. |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Exposes the `Magic Toolbars` Help page and Magery toolbar buttons. |
| `Data/Scripts/System/Commands/Player/SpellBarsManage.cs` | Registers `[magespell1` through `[magespell4` and builds `SetupBarsMage*` editor gumps. |
| `Data/Scripts/System/Commands/Player/SpellBarsDisplay.cs` | Registers `[magetool1` through `[magetool4` and builds `SpellBarsMage*` casting gumps. |
| `Data/Scripts/System/Commands/Player/SpellBarsCommands.cs` | Registers `[mageclose1` through `[mageclose4`. |
| `Data/Scripts/System/Commands/Player/SpellBarsFunctions.cs` | Initializes, reads, toggles, and writes string-backed Magery toolbar settings. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Persists `SpellBarsMage1` through `SpellBarsMage4` and the memorized chat string. |

## Mage Mobile

`Mage` derives from `BaseVendor`, uses vendor title `"the mage"`, and belongs to `NpcGuild.MagesGuild`.

Its constructor assigns these vendor skills:

| Skill | Range |
| --- | ---: |
| `Psychology` | 65.0 to 88.0 |
| `Inscribe` | 60.0 to 83.0 |
| `Magery` | 64.0 to 100.0 |
| `Meditation` | 60.0 to 83.0 |
| `MagicResist` | 65.0 to 88.0 |
| `FistFighting` | 60.0 to 80.0 |

`InitSBInfo()` adds `SBMage`, `RSScrolls`, `SBRuneCasting`, `SBBuyArtifacts`, and `SBPaganReagents`.
When `Worlds.GetMyWorld(this.Map, this.Location, this.X, this.Y)` returns `"the Land of Lodoria"`, it also adds `SBElfWizard`.

`InitOutfit()` keeps the base vendor outfit and has a random chance to add one of these carried weapons: `GnarledStaff`, `BlackStaff`, `WildStaff`, or `QuarterStaff`.

## Tutorial Entry Point

The mage adds two context-menu paths:

| Context-menu path | Entry class | Gate | Result |
| --- | --- | --- | --- |
| Wizardry tutorial | `SpeechGumpEntry` | `m_Mobile is PlayerMobile`; no existing `SpeechGump` on that player | Calls `IntelligentAction.SayHey(m_Giver)` and sends `SpeechGump(mobile, "The Mystical Art Of Wizardry", SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Mage"))`. |
| Wand charging | `FixEntry` | Added only when `from.Alive && !from.Blessed` during menu construction | Calls `BeginRepair(m_From)`, speaks the per-circle price, and assigns `from.Target = new RepairTarget(this)`. |

The shared `SpeechGump` is a `Gump` with one scrollable HTML region and a close button. Its `OnResponse(NetState sender, RelayInfo info)` only plays sound `0x4A`; it has no reply choices.

Opening the mage tutorial calls `MyChat.speechText("Mage", from)`. That maps `"Mage"` to memorized chat entry `27`; if the player has not memorized it yet, `PlayerSettings.SetChatConfig()` toggles that entry, plays an effect, and sends a memorization message.

## Tutorial Text Topics

The `"Mage"` speech text is one static HTML string built from the player name and mage name.

| Topic | Code-backed content |
| --- | --- |
| Wand charging | Says the mage can charge magic wands for `100` gold per spell circle and add `5` charges. The service is implemented by `Mage.RepairTarget`. |
| `Create Food` | Says the spell creates food and a bottle of water. The code creates one random food item and a `WaterFlask`, except blood-drinker races get `BloodyDrink` and brain-eater races get `FreshBrain`. |
| `Remove Trap` | Says the spell can create a magical orb in the backpack. Targeting the caster deletes that caster's existing `TrapWand` items, creates a new `TrapWand`, sets `WandPower = (Magery / 2) + 25` capped at `100`, and adds a 30-minute buff. |
| `Telekinesis` | Says the spell can grab reachable loose objects into the backpack, but not stacks or items inside other containers. The code also blocks immovable items, items heavier than `Caster.Int / 20`, and any item with a root parent entity. |
| `Reveal` | Says the spell reveals more than hidden creatures. The code scans nearby items for `BaseTrap`, hardcoded hidden-door `BaseDoor` item IDs, `HiddenTrap`, and `HiddenChest`, then separately checks hidden mobiles. |
| Spell bars | Says mages can use up to four custom spell bars. The actual Magery bar commands are `[magespell1` through `[magespell4`, `[magetool1` through `[magetool4`, and `[mageclose1` through `[mageclose4`. |
| Magic-school conflict | Says Elementalism and Magery cannot be held by the same spellcaster. `Spell.CantMixSpell()` blocks `ElementalSpell` or `MagerySpell` casts when both Elementalism and Magery skill values are at least `1`. |
| Forgetful crystals | Says crystals in the Sorcerer Cave in Sosaria or Conjurer's Cave in Lodoria can help a spellcaster forget one school. `ForgetfulGem.CrystalGump` can set the selected skill's `BaseFixedPoint` to `0`. |

## Wand-Charging Service

The charge service targets `BaseMagicStaff` items and explicitly rejects `IdentifyStaff`.
The target range is `12` tiles. The service requires the caller to have a backpack because payment is consumed from `from.Backpack`.

The service derives circle and eligibility from the target `BaseWeapon.IntRequirement`, not from class name or a separate circle field.

| `IntRequirement` | Treated spell circle | Eligible when current `Charges <=` | Base cost | Charges added |
| ---: | ---: | ---: | ---: | ---: |
| `10` | 1 | 30 | 100 gold | 5 |
| `15` | 2 | 23 | 200 gold | 5 |
| `20` | 3 | 18 | 300 gold | 5 |
| `25` | 4 | 15 | 400 gold | 5 |
| `30` | 5 | 12 | 500 gold | 5 |
| `35` | 6 | 9 | 600 gold | 5 |
| `40` | 7 | 6 | 700 gold | 5 |
| `45` | 8 | 3 | 800 gold | 5 |

If the target has `IntRequirement < 1`, the mage says it does not need the service.
If the target's current charges are above the threshold, the mage says it has too many charges already.
If the target uses a positive, unrecognized `IntRequirement`, `spellCircle` remains `0`, `toConsume` remains `0`, and the target path returns without feedback.

### Begging Discount

When `BaseVendor.BeggingPose(from) > 0`, the displayed and charged price is reduced by:

```text
discount = (int)((from.Skills[SkillName.Begging].Value * 0.005) * baseCost)
charge = baseCost - discount
```

The initial spoken per-circle quote clamps to at least `1` gold, but the final `toConsume` calculation does not clamp after applying the discount.
On successful service while begging, `Titles.AwardKarma(from, -BeggingKarma(from), true)` runs.

## Magic Wand Runtime

`BaseMagicStaff` stores `MagicStaffEffect Effect` and `int Charges`.
New wands randomize charges through their constructor's `minCharges` and `maxCharges`, set `Resource = CraftResource.None`, and randomize their wand `ItemID` among eight graphics.

On double-click:

| Gate | Behavior |
| --- | --- |
| `from.CanBeginAction(typeof(BaseMagicStaff))` fails | Returns without using the wand. |
| `Parent == from` and `Charges > 0` | Calls `OnMagicStaffUse(from)`. |
| `Parent != from` | Sends localized message `502641`, "You must equip this item to use it." |

`ConsumeCharge(Mobile from)` decrements `Charges` and sends localized message `1019073` only when the charge count becomes exactly `0`.
`BaseMagicStaff.Serialize()` writes version `0`, `m_MagicStaffEffect`, and `m_Charges`; `Deserialize()` reads the same values and resets `Attributes.SpellChanneling = 0`.

## Magery Spell Bars

The Help Gump `Magic Toolbars` page includes four Magery rows. Each row has an editor button, an open-toolbar button, and a close-toolbar button.

All Magery toolbar commands are registered at `AccessLevel.Player`.

| Bar | Editor command | Open command | Close command | Editor `Gump` | Display `Gump` | PlayerMobile field |
| --- | --- | --- | --- | --- | --- | --- |
| Magery Spell Bar I | `[magespell1` | `[magetool1` | `[mageclose1` | `SetupBarsMage1` | `SpellBarsMage1` | `SpellBarsMage1` |
| Magery Spell Bar II | `[magespell2` | `[magetool2` | `[mageclose2` | `SetupBarsMage2` | `SpellBarsMage2` | `SpellBarsMage2` |
| Magery Spell Bar III | `[magespell3` | `[magetool3` | `[mageclose3` | `SetupBarsMage3` | `SpellBarsMage3` | `SpellBarsMage3` |
| Magery Spell Bar IV | `[magespell4` | `[magetool4` | `[mageclose4` | `SetupBarsMage4` | `SpellBarsMage4` | `SpellBarsMage4` |

`ToolBarUpdates.InitializeToolBar()` initializes a missing Magery toolbar string to `66` zero entries separated by `#`: 64 spell flags plus two display-option flags.
The visible `SpellBarsMage*` gumps check `Spellbook.Find(from, spellID)` and only display buttons for known spells.

The Help Gump's explanatory text states that each school has two toolbars except Magery, which has four because of its larger spell count. It also states that toolbars can be vertical or horizontal, can optionally show spell names in vertical mode, are moved as gumps, cast on icon click, hide spells the player lacks, and must be closed through the Help command button or typed command.

## Serialization Notes

`Mage.Serialize()` writes version `0` after `base.Serialize(writer)` and has no custom fields.

`BaseMagicStaff.Serialize()` writes version `0`, the `MagicStaffEffect`, and the current `Charges`.

`PlayerMobile` currently writes save version `37`. `SpellBarsMage1`, `SpellBarsMage2`, `SpellBarsMage3`, and `SpellBarsMage4` are part of the older version `29` player data block and are written/read in the same order, followed by Necromancy, Knightship, Death Knight, Bard, Priest, Ancient, Monk, and Elemental toolbar strings.

## Known Issues

| Issue | Impact |
| --- | --- |
| `Mage.SpeechGumpEntry.OnClick()` only verifies that the originally stored caller is a `PlayerMobile`; it does not revalidate `m_Giver`, deletion state, range, liveness, or `NetState` before opening the `SpeechGump`. | Stale context-menu entries can open tutorial UI from invalid or out-of-range NPC/player state. |
| `SpeechFunctions.SpeechText()` dereferences `from.Name` and `m.Name` before any null guard. `SpeechGump.OnResponse()` also dereferences `sender.Mobile` without validating `sender` or `sender.Mobile`. | Unexpected or malformed `Mobile`/`NetState` state can null-reference in shared tutorial paths. |
| `FixEntry.OnClick()` and `BeginRepair()` do not revalidate range, `from.Blessed`, or vendor availability after the context menu is built. | The wand-charge target can remain available from stale context state. |
| `RepairTarget.OnTarget()` does not require the targeted `BaseMagicStaff` to be equipped by, carried by, or owned by the paying player. | A player can pay to charge any reachable `BaseMagicStaff` target, not necessarily one in their possession. |
| The charge service allows `Charges <= threshold`, then adds `5` without clamping to the threshold. | A first-circle wand at `30` charges becomes `35`; similar over-threshold final values are possible for every circle. |
| Begging price reduction is not clamped in the final `toConsume` calculation. | Extremely high Begging values could reduce the final gold cost to `0` or below, causing the service to return silently because `toConsume == 0`. |
| `MeteorSwarmMagicStaff` constructs with `IntRequirement = 45` while its properties and deserialize repair say it should require `40` intelligence. | Newly constructed Meteor Swarm wands are treated as eighth-circle wands by the mage charge table until deserialized or manually corrected. |
| `HelpGump.OnResponse()` dereferences `state.Mobile` and calls `from.SendSound()` before validation; many toolbar paths cast `Mobile` directly to `PlayerMobile`. | Nonstandard `NetState` or non-player callers can throw before defensive checks run. |
