# NPC Mage Advice

## Scope

This page documents the mage advice exposed through the shared `SpeechGump` dialogue system.
It covers the `Mage` vendor context-menu entry, the `"Mage"` branch of `SpeechFunctions.SpeechText()`, the memorized chat entry, the wand-charging service offered by the same vendor, and the spell-bar commands referenced by the dialogue.

This is not a new magic engine or spell reference. For broader spellcasting mechanics, see [Mage Spellcasting Tips](Mage_Spellcasting_Tips.md) and [Magic Toolbars Guide](Magic_Toolbars_Guide.md).

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/System/Misc/Talk.cs` | Shared `SpeechGump` UI and `SpeechFunctions.SpeechText(..., "Mage")` dialogue text provider. |
| `Data/Scripts/Mobiles/Civilized/Vendors/Mage.cs` | Mage vendor, context-menu tutorial entry, wand-charge context-menu entry, target flow, vendor stock, skills, and serialization. |
| `Data/Scripts/System/Commands/Player/MyChat.cs` | Memorizes the `"Mage"` conversation as player chat entry `27`. |
| `Data/Scripts/Items/Wands/BaseMagicStaff.cs` | Shared charged magic-wand base class with `Charges`, equip use gates, display properties, and serialization. |
| `Data/Scripts/System/Commands/Player/SpellBarsManage.cs` | Registers `[magespell1` through `[magespell4` editor commands referenced by the dialogue. |
| `Data/Scripts/System/Commands/Player/SpellBarsDisplay.cs` | Registers `[magetool1` through `[magetool4` display commands for the four Magery spell bars. |
| `Data/Scripts/System/Commands/Player/SpellBarsCommands.cs` | Registers `[mageclose1` through `[mageclose4` close commands. |
| `Data/Scripts/System/Commands/Player/SpellBarsFunctions.cs` | Initializes and stores the string-backed Magery toolbar settings on `PlayerMobile`. |

## Dialogue Entry Point

| Step | Behavior |
| --- | --- |
| Context menu construction | `Mage.GetContextMenuEntries()` always adds `SpeechGumpEntry(from, this)` after base vendor entries. |
| Caller gate | `SpeechGumpEntry.OnClick()` returns unless the stored caller is a `PlayerMobile`. |
| Duplicate gump gate | If the player already has a `SpeechGump`, no new mage-advice gump is sent. |
| Greeting | `IntelligentAction.SayHey(m_Giver)` runs before the gump opens. |
| Gump title | `The Mystical Art Of Wizardry`. |
| Text source | `SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Mage")`. |
| Memorized chat | `MyChat.speechText("Mage", from)` maps the conversation to chat entry `27` and records it in player chat settings. |

The shared `SpeechGump` has one scrollable HTML panel and a close button. Its response handler only plays sound `0x4A`; it has no reply choices.

## Mage Vendor Profile

`Mage` derives from `BaseVendor`, uses vendor title `"the mage"`, and belongs to `NpcGuild.MagesGuild`.

| Vendor skill | Assigned range |
| --- | ---: |
| `Psychology` | 65.0 to 88.0 |
| `Inscribe` | 60.0 to 83.0 |
| `Magery` | 64.0 to 100.0 |
| `Meditation` | 60.0 to 83.0 |
| `MagicResist` | 65.0 to 88.0 |
| `FistFighting` | 60.0 to 80.0 |

`InitSBInfo()` adds `SBMage`, `RSScrolls`, `SBRuneCasting`, `SBBuyArtifacts`, and `SBPaganReagents`.
Mages in `"the Land of Lodoria"` also add `SBElfWizard`.

## Dialogue Topics

The `"Mage"` speech text is a single HTML string built from the player name and mage name.

| Topic | Dialogue-backed behavior |
| --- | --- |
| Wand charging | The mage says players can hire the vendor to add `5` wand charges for `100` gold per spell circle. |
| `Create Food` | The mage calls out food and water creation as a survival tip. |
| `Remove Trap` | The mage says the spell can create a backpack orb that passively checks floor, wall, and container traps. |
| `Telekinesis` | The mage says the spell can move some out-of-reach loose objects into the backpack, but not stacks or items inside containers. |
| `Reveal` | The mage says the spell reveals hidden creatures, traps, and hidden treasure containers. |
| Spell bars | The mage says Magery can use up to four custom spell bars from the paperdoll Help menu. |
| Magic-school conflict | The mage warns that Elementalism and Magery cannot be carried by the same spellcaster without spell failures. |
| Forgetful crystals | The mage points players to Sorcerer Cave in Sosaria or Conjurer's Cave in Lodoria for crystals that help forget one magic school. |

## Wand Charging Service

The mage exposes a second context-menu action through `AddCustomContextEntries()` when the caller is alive and not blessed.
Selecting that entry calls `BeginRepair()`, quotes the price, and assigns a `RepairTarget` with range `12`.

The target flow accepts `BaseMagicStaff` items when the caller has a backpack and rejects `IdentifyStaff`.
It derives the spell circle from the target's `BaseWeapon.IntRequirement`, then adds `5` charges after consuming gold from the caller's backpack.

| `IntRequirement` | Treated circle | Eligible when `Charges <=` | Base cost |
| ---: | ---: | ---: | ---: |
| `10` | 1 | 30 | 100 gold |
| `15` | 2 | 23 | 200 gold |
| `20` | 3 | 18 | 300 gold |
| `25` | 4 | 15 | 400 gold |
| `30` | 5 | 12 | 500 gold |
| `35` | 6 | 9 | 600 gold |
| `40` | 7 | 6 | 700 gold |
| `45` | 8 | 3 | 800 gold |

Begging can reduce the quoted price and the final charge price. Successful charging while begging also applies the vendor begging karma loss.

## Spell-Bar References

The mage dialogue points players to the paperdoll Help menu for faster casting.
The backing commands are player-level commands:

| Bar | Editor | Open | Close |
| --- | --- | --- | --- |
| Magery bar 1 | `[magespell1` | `[magetool1` | `[mageclose1` |
| Magery bar 2 | `[magespell2` | `[magetool2` | `[mageclose2` |
| Magery bar 3 | `[magespell3` | `[magetool3` | `[mageclose3` |
| Magery bar 4 | `[magespell4` | `[magetool4` | `[mageclose4` |

`ToolBarUpdates.InitializeToolBar()` creates missing Magery toolbar strings on `PlayerMobile` for `SetupBarsMage1` through `SetupBarsMage4`.

## Serialization

| Type | Version | Serialized fields after version |
| --- | ---: | --- |
| `Mage` | 0 | None. Standard `BaseVendor` data only. |
| `BaseMagicStaff` | 0 | `MagicStaffEffect` as `int`, then `Charges` as `int`. |

## Known Issues

| Issue | Impact |
| --- | --- |
| `SpeechFunctions.SpeechText()` reads `from.Name`, `m.Name`, and records chat before validating either `Mobile`. | Null or stale caller/NPC state can throw before the dialogue string is returned. |
| `SpeechGump.OnResponse()` dereferences `sender.Mobile` without checking `sender` or `sender.Mobile`. | Malformed gump replies can throw in the shared close path. |
| `Mage.SpeechGumpEntry.OnClick()` only checks the stored caller type. | Stale context-menu entries do not revalidate range, deletion state, liveness, visibility, or network state before opening the advice gump. |
| `FixEntry.OnClick()` and `BeginRepair()` do not revalidate range, `from.Blessed`, or vendor availability after the context menu is built. | The wand-charge target can remain available from stale context state. |
| `RepairTarget.OnTarget()` does not require the target wand to be equipped by, carried by, or owned by the paying player. | A player can pay to charge any reachable `BaseMagicStaff`, including one outside their possession. |
| The service allows `Charges <= threshold`, then adds `5` without clamping. | Wands can end above the documented threshold after a successful service. |
| The final begging discount is not clamped after recalculation. | Extreme Begging values can reduce the final cost to `0` or below, causing the service to return without feedback. |

## Admin Notes

There are no dedicated admin commands for the mage advice dialogue.
Staff can inspect wand `Charges` and `Effect` through command properties on `BaseMagicStaff` items.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0124.
- Backlog rows: RB-06731.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/System/Misc/Talk.cs (CurrentFile)
- Data/Scripts/Mobiles/Civilized/Vendors/Mage.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/MyChat.cs (CurrentFile)
- Data/Scripts/Items/Wands/BaseMagicStaff.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/SpellBarsManage.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/SpellBarsDisplay.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/SpellBarsCommands.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/SpellBarsFunctions.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=134; Gump=883; Initialize=67; Timer=1.
- Data/Scripts/Items/Wands/BaseMagicStaff.cs:L106 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Civilized/Vendors/Mage.cs:L109 Gump SendGump access=Internal
- Data/Scripts/System/Commands/Player/MyChat.cs:L75 Gump OnResponse access=Internal
- Data/Scripts/System/Commands/Player/MyChat.cs:L174 Gump OnResponse access=Internal
- Data/Scripts/System/Commands/Player/MyChat.cs:L183 Gump SendGump access=Internal
- Data/Scripts/System/Commands/Player/MyChat.cs:L188 Gump SendGump access=Internal
- Data/Scripts/System/Commands/Player/MyChat.cs:L193 Gump SendGump access=Internal
- Data/Scripts/System/Commands/Player/MyChat.cs:L198 Gump SendGump access=Internal
- Data/Scripts/System/Commands/Player/SpellBarsCommands.cs:L32 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Commands/Player/SpellBarsCommands.cs:L34 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Commands/Player/SpellBarsCommands.cs:L43 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Commands/Player/SpellBarsCommands.cs:L67 Initialize Initialize access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 2.
- Data/Scripts/Items/Wands/BaseMagicStaff.cs:Server.Items.BaseMagicStaff version=0 serialize=L144 deserialize=L152 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Civilized/Vendors/Mage.cs:Server.Mobiles.Mage version=0 serialize=L306 deserialize=L313 alignment=AlignedByCountAndKnownTypes

### Project And Runtime Coverage

- Data/Scripts/Items/Wands/BaseMagicStaff.cs=Keep
- Data/Scripts/Items/Wands/BaseMagicStaff.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/Mage.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/Mage.cs=Keep
- Data/Scripts/System/Commands/Player/MyChat.cs=Keep
- Data/Scripts/System/Commands/Player/MyChat.cs=Keep
- Data/Scripts/System/Commands/Player/SpellBarsCommands.cs=Keep
- Data/Scripts/System/Commands/Player/SpellBarsCommands.cs=Keep
- Data/Scripts/System/Commands/Player/SpellBarsDisplay.cs=Keep
- Data/Scripts/System/Commands/Player/SpellBarsDisplay.cs=Keep
- Data/Scripts/System/Commands/Player/SpellBarsFunctions.cs=Keep
- Data/Scripts/System/Commands/Player/SpellBarsFunctions.cs=Keep
- Data/Scripts/System/Commands/Player/SpellBarsManage.cs=Keep
- Data/Scripts/System/Commands/Player/SpellBarsManage.cs=Keep
- Data/Scripts/System/Misc/Talk.cs=Keep
- Data/Scripts/System/Misc/Talk.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
