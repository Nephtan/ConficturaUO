# Barbaric Play Style

## Overview
The Barbaric play style is a player-facing flavor toggle stored on `PlayerMobile.CharacterBarbaric`. It has two compiled effects:

* It grants or removes an owner-bound `BarbaricSatchel` that can restyle selected equipment by changing `ItemID`, `Name`, and sometimes `Layer`/`Hue`.
* It changes the skill title text returned by `Skill.CharacterTitle(...)` for a fixed list of base skill titles.

No combat damage, resistance, stat, skill-gain, loot-drop, spawn, or XMLSpawner attachment logic was found in this trace.

## Source Files

| File | Type | Purpose |
| --- | --- | --- |
| `Data/Scripts/System/Commands/Player/PlayBarbaric.cs` | Player command | Registers `[barbaric` and toggles `PlayerMobile.CharacterBarbaric`. |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Gump | Renders the `Settings` play-style row, opens the Barbaric info page, and handles button ID `984`. |
| `Data/Scripts/Items/Special/BarbaricSatchel.cs` | Item and Gumps | Defines `BarbaricSatchel`, `BarbaricSatchelGump`, `BarbaricAlterGump`, conversion logic, and satchel persistence. |
| `Data/Scripts/System/Misc/Players.cs` | Title bridge | Reads `CharacterBarbaric` through `GetPlayerInfo.BarbaricPlay(Mobile)` and passes it into `Skill.CharacterTitle(...)`. |
| `Data/System/Source/Skills.cs` | Core engine | Applies the title replacement table when `isBarbaric > 0`. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Player state | Stores and serializes `CharacterBarbaric`; exposes it as `Character_Conan`. |
| `Data/Scripts/Mobiles/Base/PlayerSettings.cs` | Starter setup helper | `SetSavage(Mobile)` sets Barbaric state, grants a satchel, and gives a starter savage equipment set. |

## Entry Points

| Entry point | Access | Compiled behavior |
| --- | --- | --- |
| `[barbaric` | `AccessLevel.Player` | Toggles the Barbaric play style. The command has no parameters. |
| Help `Settings` Gump | Player help gump page `12` | The `Barbaric` row uses button ID `984`; the adjacent info button opens `InfoHelpGump` page `198`. |
| `PlayerSettings.SetSavage(Mobile)` | Internal helper | Sets Barbaric mode during the savage starter path, gives a satchel, and equips starter gear. |
| `PlayerMobile.OnItemAdded(Item)` | Player equipment hook | Calls `BarbaricSatchel.BarbaricRobe(item, this)` whenever an item is added to the player. |

The command metadata is:

| Attribute | Value |
| --- | --- |
| `[Usage]` | `barbaric` |
| `[Description]` | `Enables or disables the barbaric play style.` |

## Player State

`PlayerMobile` stores the toggle in the public integer field `CharacterBarbaric`, exposed to GameMasters as the command property `Character_Conan`.

| Value | Meaning in current code |
| --- | --- |
| `0` | Barbaric play is off. |
| `1` | Barbaric play is on with normal Barbarian/Chieftain title replacements. |
| `2` | Barbaric play is on with Amazon/Valkyrie title replacements for the relevant title groups. |
| Any value greater than `2` | Treated as Barbaric-on by `BarbaricPlay(Mobile)` and as the Amazon/Valkyrie branch by `Skill.CharacterTitle(...)`. |

## Toggle Flow

When `[barbaric` is used:

| Current state | Additional condition | Result |
| --- | --- | --- |
| `CharacterBarbaric == 1` | `Mobile.Female == true` | Sets `CharacterBarbaric = 2` and sends the Amazon-title message. |
| `CharacterBarbaric > 0` | Any other case | Sets `CharacterBarbaric = 0`, deletes all satchels owned by that Mobile, and sends the disabled message. |
| `CharacterBarbaric == 0` | Any | Sets `CharacterEvil = 0`, `CharacterOriental = 0`, `CharacterBarbaric = 1`, gives a fresh `BarbaricSatchel`, and sends the enabled message. |

The Help `Settings` Gump button ID `984` follows the same state transitions, but it does not send the command feedback messages; it redraws `HelpGump(from, 12)`.

The normal, evil, and oriental play-style buttons in the Help `Settings` Gump clear `CharacterBarbaric` and call `BarbaricSatchel.GetRidOf(from)`.

## Barbaric Satchel

`BarbaricSatchel` is a blessed `Item` with base item ID `0x27BE`, display name `barbaric satchel`, `Weight = 1.0`, hidden weight display, and hidden loot-type display.

The satchel stores a public `Mobile owner` field exposed as the GameMaster command property `Owner`.

| Method | Behavior |
| --- | --- |
| `GivePack(Mobile from)` | Deletes existing satchels owned by `from`, creates a new satchel, assigns `owner = from`, adds it to the backpack, and sends `A barbaric satchel has been added to your pack.` |
| `GetRidOf(Mobile from)` | Scans `World.Items.Values` for every `BarbaricSatchel` whose `owner == from` and deletes each one. |
| `OnDoubleClick(Mobile from)` | Deletes the satchel if `from != owner && Weight > 0`; otherwise, if the caller is in range `4`, opens `BarbaricSatchelGump` and plays sound `0x48`. |
| `OnDragDrop(Mobile from, Item dropped)` | Deletes the satchel on non-owner use, rejects artifacts when `MyServerSettings.AlterArtifact(dropped)` returns false, then converts eligible equipment or opens `BarbaricAlterGump`. It always calls `from.ProcessClothing()` and returns `false`. |

## Conversion Rules

The satchel only enters conversion logic when `dropped` is a `BaseArmor`, `BaseClothing`, `BaseJewel`, or `BaseHat` and the artifact gate allows alteration.

### Direct Conversions

| Input condition | Output |
| --- | --- |
| `Layer.Gloves` and `BaseArmor`, not already `0x564E` | `ItemID = 0x564E`, name fragment `guantlets`. |
| `Layer.Gloves` and `BaseArmor`, already `0x564E`, metal or wood | `ItemID = 0x1414`, name fragment `guantlets`. |
| `Layer.Gloves` and `BaseArmor`, fallback | `ItemID = 0x13C6`, name fragment `gloves`. |
| Item ID `0x2B68`, `0x567B`, or `0x2790` | `ItemID = 0x55DB`, name fragment `royal loin cloth`. |
| Item ID `0x55DB` | `ItemID = 0x567B`, name fragment `belt`. |
| `BaseWaist` | `ItemID = 0x2B68`, name fragment `loin cloth`. |
| `Layer.Neck` | `ItemID = 0x5650`, name fragment `amulet`. |
| Item ID `0x1541` or `0x1542` | `ItemID = 0x0409`, name fragment `sash`. |
| Item ID `0x0409` | `ItemID = 0x1541`, name fragment `sash`. |
| Supported cloak IDs on `Layer.Cloak`, except `0x5679` | `ItemID = 0x5679`, name fragment `fleece`. |
| Cloak item ID `0x5679` | `ItemID = 0x1515`, name fragment `cloak`. |
| Shoe item ID `0x0406` | `ItemID = 0x170D`, name fragment `sandals`. |
| Shoe item ID `0x170D`, metal armor shoes | `ItemID = 0x170B`, name fragment `boots`. |
| Shoe item ID `0x170D`, non-metal shoes | `ItemID = 0x2B67`, name fragment `boots`. |
| Other `Layer.Shoes` item | `ItemID = 0x0406`, name fragment `boots`. |

### Gump Conversions

These item groups open `BarbaricAlterGump` and wait for a reply button.

| Input group | Button IDs | Outputs |
| --- | --- | --- |
| Hat or cloth helm | `1..7` | `cap 22088`, `circlet 11119`, `headband 5439`, `hood 0x2B71`, `cowl 0x3176`, `mask 5449`, `mask 5451`. |
| Arm armor | `8..9` | `bracers 22093`, `bracers 5198`. |
| Helm armor | `10..15` | `helm 22083`, `circlet 11119`, `horned helm 7947`, `skull helm 5201`, `helmet 12219`, `helmet 5134`. |
| Shield | `16..19` | `shield 7026`, `shield 7032`, `shield 7034`, `shield 7035`. |
| Pants or inner legs armor | `20..23` | `breeches 22095`, `greaves 5202`, `skirt 7176`, `shorts 7168`. |
| Shirt or inner torso armor | `24..29` | `armor 22097`, female-only `bustier 7178`, female-only `bustier 7180`, female-only `bustier 7172`, female-only `bustier 7170`, `armor 5199`. |
| Outer torso robe path | `30..34` | `mantle 0x5652` or female `mantle 0x563E`, `scant mantle 0x567A`, `robe 0x1F03`, `shoulder belt 0x1541`, `cross belt 0x0409`. Buttons `30..32` set `Layer.OuterTorso`; buttons `33..34` set `Layer.MiddleTorso`. |

`ChangeItem(Item item, int itemID, string NewName, Mobile from)` then:

1. Builds a material prefix from `MaterialInfo.GetSpecialMaterialName(item)`, or falls back to `metal`, `wooden`, or `leather`.
2. Recolors some `BaseArmor` results when the resource and current hue match the hard-coded cases.
3. Assigns either `GetRandomBarbaric() + " " + material + NewName` or `material + NewName + " " + GetRandomBarbarian()`.
4. Sets `item.ItemID = itemID`, plays sound `0x55`, moves the item into the caller's backpack, and sends `The item has been changed.`

## Skill Title Replacements

`GetPlayerInfo.GetSkillTitle(Mobile)` passes `CharacterBarbaric` into `Skill.CharacterTitle(...)`. The title function only applies this block when `isBarbaric > 0` and the base skill title contains one of the checked strings.

| Base title text contains | Barbaric result |
| --- | --- |
| `Alchemist` | `Herbalist` |
| `Naturalist` | `Beastmaster` |
| `Shepherd` | `Beastmaster` |
| `Sailor` | `Atlantean`, or `Sea Captain` when Seafaring is at least `100.0`. |
| `Veterinarian` | `Beastmaster` |
| `Explorer` | `Wanderer` |
| `Knight` | `Death Knight` when karma is negative, otherwise `Valkyrie` when `isBarbaric > 1`, otherwise `Chieftain`. |
| `Tactician` | `Warlord` |
| `Duelist` | `Defender` |
| `Necromancer` | `Witch Doctor` |
| `Bard` | `Chronicler` |
| `Wizard` | `Shaman` |
| `Archer` | `Amazon` when `isBarbaric > 1`, otherwise `Barbarian`. |
| `Fencer` | `Amazon` when `isBarbaric > 1`, otherwise `Barbarian`. |
| `Bludgeoner` | `Amazon` when `isBarbaric > 1`, otherwise `Barbarian`. |
| `Swordsman` | `Amazon` when `isBarbaric > 1`, otherwise `Barbarian`. |
| `Ranger` | `Hunter` |
| `Man-at-arms` | `Gladiator` |

Because this block runs before the later Archmage, monk, Jedi, Syth, Jester, Oriental, and evil title branches, a matched Barbaric title prevents those later `else if` branches from running for that same title evaluation.

## Persistence

`PlayerMobile` serializes `CharacterBarbaric` in the same settings block that writes `QuickBar`, quest strings, `SkillDisplay`, `MagerySpellHue`, `CharacterEvil`, and `CharacterOriental`. Current deserialization reads it in the matching position.

`BarbaricSatchel` serializes version `1` and writes only the `owner` Mobile after the version integer.

## Known Issues

* `Data/Scripts/Scripts.csproj` includes `System\Help\HelpGump.cs`, but the traced source file is `Data/Scripts/System/Help/Gumps/HelpGump.cs`. Under the documented MSBuild workflow, this stale project path can keep the Help Gump out of the maintained project build.
* `[barbaric` casts `CommandEventArgs.Mobile` directly to `PlayerMobile` without a null or type guard.
* The Help `Settings` Gump casts `Mobile from` directly to `PlayerMobile` while rendering and handling the Barbaric play-style controls.
* `[evil` and `[oriental` clear `CharacterBarbaric` but do not call `BarbaricSatchel.GetRidOf(m)`, so switching styles by command can leave an owned Barbaric satchel behind. The Help Gump buttons do remove it.
* `BarbaricAlterGump.OnResponse` does not re-check whether `m_Item` still exists, still belongs to the caller, still passes the artifact gate, or still matches the item group that originally opened the Gump before calling `ChangeItem(...)`.
* `BarbaricSatchel` writes serialization version `1`, but `Deserialize` unconditionally reads `owner` for every version value. If any version `0` satchel save exists without that field, the read order would be wrong.
* Amazon/Valkyrie title behavior is based on `CharacterBarbaric > 1`, not on current gender. The player command only sets value `2` through the female branch, but GameMaster property edits or later gender changes can leave value `2` on any `PlayerMobile`.
* The generated gloves name fragment is spelled `guantlets` in two direct conversion branches.
