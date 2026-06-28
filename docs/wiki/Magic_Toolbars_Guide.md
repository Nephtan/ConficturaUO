# Magic Toolbars Guide

## Scope

This page documents the player-facing magic toolbar guide exposed through `Server.Engines.Help.HelpGump` page 7.
The system is a `Gump` and player command dispatcher for spell-bar editor gumps, open toolbar gumps, and close commands.
It does not define a new `Mobile`, `Item`, packet handler, EventSink hook, or XMLSpawner attachment.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Adds the `Magic Toolbars` Help Gump page and maps button IDs to toolbar editor/open/close commands. |
| `Data/Scripts/System/Commands/Player/SpellBarsManage.cs` | Defines the `SetupBars*` editor gumps and player commands that open each editor. |
| `Data/Scripts/System/Commands/Player/SpellBarsDisplay.cs` | Defines the visible `SpellBars*` casting gumps and player commands that open each toolbar. |
| `Data/Scripts/System/Commands/Player/SpellBarsCommands.cs` | Defines close commands and the `[ancient` book/bag toggle. |
| `Data/Scripts/System/Commands/Player/SpellBarsFunctions.cs` | Reads, initializes, and toggles string-backed toolbar settings. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Persists toolbar strings and the `UsingAncientBook` flag. |
| `Data/Scripts/Magic/Research/ResearchFunctions.cs` | Supplies Ancient spell-bar defaults and transfers stored Ancient bars from a `ResearchBag`. |
| `Data/Scripts/Scripts.csproj` | Compiles the spell-bar command, display, function, and editor files. |

## Help Gump Entry Point

The Help Gump left navigation includes a `Magic Toolbars` entry. When page `7` is selected, the constructor draws one row per supported toolbar. Each row has:

| Button action | Result |
| --- | --- |
| Bar name button | Opens that bar's `SetupBars*` editor gump. |
| `Open Toolbar` button | Invokes the bar's player command, such as `[magetool1`. |
| `Close Toolbar` button | Invokes the close command, such as `[mageclose1`. |

Open and close buttons use button IDs `200` through `400`. `HelpGump.OnResponse()` reopens Help page `7`, then calls `InvokeCommand()`, which passes the command through `CommandSystem.Handle()` using the current shard command prefix.

Editor buttons use direct `switch` cases. They close the existing editor gump for that bar and send a new `SetupBars*` gump with origin `1`, which returns the user to Help page `7` when the editor closes.

## Toolbar Catalog

All toolbar commands are registered at `AccessLevel.Player`.

| Help row | Editor command(s) | Open command(s) | Close command(s) | PlayerMobile field(s) | Cast entries | Settings entries |
| --- | --- | --- | --- | --- | ---: | ---: |
| Ancient Spell Bar I-IV | `[archspell1` through `[archspell4` | `[archtool1` through `[archtool4` | `[archclose1` through `[archclose4` | `SpellBarsArch1` through `SpellBarsArch4` | 64 | 2 |
| Bard Songs Bar I-II | `[bardsong1`, `[bardsong2` | `[bardtool1`, `[bardtool2` | `[bardclose1`, `[bardclose2` | `SpellBarsBard1`, `SpellBarsBard2` | 16 | 2 |
| Knight Spell Bar I-II | `[knightspell1`, `[knightspell2` | `[knighttool1`, `[knighttool2` | `[knightclose1`, `[knightclose2` | `SpellBarsKnight1`, `SpellBarsKnight2` | 10 | 2 |
| Death Knight Spell Bar I-II | `[deathspell1`, `[deathspell2` | `[deathtool1`, `[deathtool2` | `[deathclose1`, `[deathclose2` | `SpellBarsDeath1`, `SpellBarsDeath2` | 14 | 2 |
| Elemental Spell Bar I-II | `[elementspell1`, `[elementspell2` | `[elementtool1`, `[elementtool2` | `[elementclose1`, `[elementclose2` | `SpellBarsElly1`, `SpellBarsElly2` | 32 | 2 |
| Magery Spell Bar I-IV | `[magespell1` through `[magespell4` | `[magetool1` through `[magetool4` | `[mageclose1` through `[mageclose4` | `SpellBarsMage1` through `SpellBarsMage4` | 64 | 2 |
| Monk Ability Bar I-II | `[monkspell1`, `[monkspell2` | `[monktool1`, `[monktool2` | `[monkclose1`, `[monkclose2` | `SpellBarsMonk1`, `SpellBarsMonk2` | 10 | 2 |
| Necromancer Spell Bar I-II | `[necrospell1`, `[necrospell2` | `[necrotool1`, `[necrotool2` | `[necroclose1`, `[necroclose2` | `SpellBarsNecro1`, `SpellBarsNecro2` | 17 | 2 |
| Priest Prayer Bar I-II | `[holyspell1`, `[holyspell2` | `[holytool1`, `[holytool2` | `[holyclose1`, `[holyclose2` | `SpellBarsPriest1`, `SpellBarsPriest2` | 14 | 2 |

The special `[ancient` command toggles `PlayerMobile.UsingAncientBook`. When the flag becomes `true`, Ancient toolbar casting uses the Ancient spellbook path; when it becomes `false`, it uses the `ResearchBag` path.

## Editor Gumps

Each `SetupBars*` editor gump stores one string on `PlayerMobile`. The string is a `#`-delimited list of integer flags. A spell or ability entry set to `1` appears on the toolbar when the character knows or can use the matching spell; an entry set to `0` is hidden.

The last two settings are display options:

| Setting position | Meaning |
| ---: | --- |
| `cast entries + 1` | Show spell names when the toolbar is vertical. |
| `cast entries + 2` | Toolbar orientation. `1` is vertical; `0` is horizontal. |

The editor buttons call `ToolBarUpdates.UpdateToolBar(Mobile m, int nChange, string ToolBar, int nTotal)`. That helper initializes a missing toolbar string, toggles only the requested setting, and writes the rebuilt string back to the matching `PlayerMobile` field.

When an editor closes with button ID `0` and origin `1`, it reopens `HelpGump(from, 7)`.
Ancient editor gumps also support a higher origin path that returns to the `ResearchBag.ResearchGump` when a valid owner bag is found.

## Display Gumps

The visible `SpellBars*` gumps are `Closable = false`, `Disposable = true`, `Dragable = true`, and `Resizable = false`.
Close commands explicitly close both the editor gump and display gump for the selected bar.

Display gumps read the same settings with `ToolBarUpdates.GetToolBarSetting()`.
If the orientation setting is greater than `0`, the toolbar is built vertically. In vertical mode, the show-name setting controls whether text labels are added beside selected spell icons.
If the orientation setting is `0`, the toolbar is built horizontally and only icons are drawn.

Each displayed cast button checks the corresponding known-spell gate before adding the button. Ancient bars use `ResearchSettings.HasSpell()`, which branches between Ancient spellbook lookup and `ResearchBag` lookup based on `UsingAncientBook`.

## Serialization Notes

Toolbar settings are stored directly on `PlayerMobile` as public string fields with `CommandProperty(AccessLevel.GameMaster)` wrappers.

Current `PlayerMobile` serialization version is `37`. Version `36` added `UsingAncientBook` plus `SpellBarsArch1` through `SpellBarsArch4`. The older version `29` block still writes and reads the other toolbar strings and also writes and reads `SpellBarsArch1` through `SpellBarsArch3`.

That means current saves contain `SpellBarsArch1`, `SpellBarsArch2`, and `SpellBarsArch3` twice: once in the version `36` section and once in the legacy version `29` section. The read order matches the write order, so this is not a save-corrupting mismatch, but administrators should be aware that only `SpellBarsArch4` is stored exclusively in the newer section.

Older saves without version `36` data leave `SpellBarsArch4` null until the toolbar helper initializes it.

## Known Issues

| Issue | Impact |
| --- | --- |
| Fresh Ancient and Magery toolbar defaults contain 64 cast entries but omit the two display-option entries used by their editors. | The first click on `Show Spell Names When Vertical` or the orientation toggle can only append/populate a missing `0` entry; a second click is needed before the option can turn on. |
| `HelpGump.OnResponse()` uses `state.Mobile` immediately and many toolbar paths cast `Mobile` directly to `PlayerMobile`. | Invalid `NetState` state or non-`PlayerMobile` callers can throw before any guard runs. |
| `ResearchSettings.GetAncientTome()` and `ResearchSettings.ResearchMaterials()` call `m.Backpack.FindItemByType(...)` without checking for a backpack. | Ancient toolbar book/bag checks can null-reference on a mobile with no backpack. |
| Ancient toolbar fields `SpellBarsArch1` through `SpellBarsArch3` are serialized twice in the current `PlayerMobile` save stream. | The stream is aligned, but duplicated fields make manual inspection and future serializer edits easier to get wrong. |
