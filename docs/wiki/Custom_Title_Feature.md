# Custom Title Feature

## Scope

This page documents the player custom title feature exposed through `Server.Engines.Help.HelpGump`.
The feature is a Help Gump option that opens `Server.Gumps.CustomTitleGump` and writes directly to the caller's core `Mobile.Title` property.
It does not define a custom `Item`, `Mobile`, packet handler, EventSink hook, XMLSpawner attachment, or player command.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Adds the Help Gump `Custom Title` option when custom titles are enabled, opens `CustomTitleGump`, and provides the info text page. |
| `Data/Scripts/System/Misc/Naming.cs` | Defines `CustomTitleGump`, its text entry, reply buttons, filtering, and direct `Mobile.Title` mutation. |
| `Data/Scripts/System/Help/ServerSettings.cs` | Describes the administrator-facing `Allow Custom Titles` server setting in the settings gump catalog. |
| `Data/Scripts/System/Misc/Settings.cs` | Stores and loads the `S_AllowCustomTitles` flag used by `HelpGump`. |
| `Data/System/Source/Mobile.cs` | Owns the `Mobile.Title` property, object-property suffix behavior, click-name suffix behavior, and base serialization for the title string. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Serializes an additional player-specific copy of `Title` in the `PlayerMobile` save stream. |
| `Data/Scripts/System/Misc/Titles.cs` | Appends `Mobile.Title` to computed title strings and suppresses the skill title fallback while a custom title is present. |

## Enablement

Custom titles are enabled by default. `MyServerSettings` initializes `S_AllowCustomTitles` to `true`, reads XML setting `52` into that flag, and exposes it through `AllowCustomTitles()`.

The Help Gump checks `MyServerSettings.AllowCustomTitles()` before drawing the `Custom Title` row. When enabled, the row adds:

| Button ID | Purpose |
| ---: | --- |
| `80` | Opens `CustomTitleGump`. |
| `97` | Opens the `InfoHelpGump` text page for `Custom Title`. |

The settings gump catalog labels this toggle as `Allow Custom Titles`, with default `true`, category `3`, and a note that it allows characters to set a custom title from the Help section.

## Player Flow

There is no required player command for this feature. The compiled path is:

1. Player opens Help.
2. `HelpGump` draws `Custom Title` if `AllowCustomTitles()` returns `true`.
3. Button ID `80` sends `new CustomTitleGump(from)`.
4. `CustomTitleGump` shows explanatory text and a single `AddTextEntry` with the default text `Type here...` and max length `25`.
5. The submit button uses button ID `1`; the right-side close/cancel button uses button ID `0`.

## Reply Logic

`CustomTitleGump.OnResponse(NetState sender, RelayInfo info)` reads text entry ID `1`, trims it, and then applies this decision tree:

| Input condition | Result |
| --- | --- |
| Text equals `Type here...` | No title mutation. The gump closes Help and sends `HelpGump(from, 12)`. |
| Text contains a blocked phrase listed below | Sends `The words you used are not allowed.` and returns without changing `Mobile.Title`. |
| Text is non-empty and not exactly `clear` | Sends `Your title is now {0}.`, assigns `from.Title = name`, and returns. |
| Text is empty or exactly `clear` | Assigns `from.Title = null`, sends `Your title has been removed.`, closes Help, and sends `HelpGump(from, 12)`. |

The code does not branch on `info.ButtonID`, so the cancel button can still set or clear a title if the text entry value is not the untouched default.

## Blocked Phrases

The title filter is a literal `string.Contains` deny list. It checks each skill-rank word with a trailing space in title case and lowercase, plus `Titan ` and `titan `.

| Block group | Literal strings checked |
| --- | --- |
| Skill ranks, title case | `Legendary `, `Elder `, `Grandmaster `, `Master `, `Adept `, `Expert `, `Journeyman `, `Apprentice `, `Novice `, `Neophyte ` |
| Skill ranks, lowercase | `legendary `, `elder `, `grandmaster `, `master `, `adept `, `expert `, `journeyman `, `apprentice `, `novice `, `neophyte ` |
| Titan | `Titan `, `titan ` |

No `NameVerification.Validate()` call is made by this gump, and no profanity-protection helper is called.

## Display Behavior

The stored value is the core `Mobile.Title` string.

`Titles.ComputeTitle()` trims `beheld.Title`; if it has content, the custom title is appended to the generated fame/karma/champion title text. The skill-title fallback only runs when no custom title is present.

Core `Mobile` name properties also use `Title` as the suffix when `PropertyTitle` is enabled, and click names use it as the suffix when `ClickTitle` is enabled. Setting the property calls `InvalidateProperties()`.

## Serialization Notes

`Mobile` owns `m_Title`. Base mobile serialization writes `m_Title` after `m_Player` and before `m_Profile`, and deserialization reads the same sequence.

Current `PlayerMobile` serialization version is `37`. The `PlayerMobile` stream also writes `Title` in its legacy version `33` block after `m_NONPK`, and deserialization reads that value back through the `Title` property.

That means current player saves contain the custom title twice: once in the base `Mobile` stream and once in the `PlayerMobile` stream. The read and write order is aligned, so this is not currently a save-corrupting mismatch, but future serializer edits must preserve or explicitly version both positions.

## Known Issues

| Issue | Impact |
| --- | --- |
| `CustomTitleGump.OnResponse()` reads `sender.Mobile` and calls `from.SendSound()` before checking whether `from` is null. | An invalid `NetState` or missing `Mobile` can throw before the guard runs. |
| `GetString()` can return null, but `OnResponse()` calls `name.Contains(...)` without a null fallback. | A malformed or missing text entry can null-reference the reply handler. |
| `info.ButtonID` is ignored. | The cancel/close button can still set or clear a title if the text entry contains a changed value. |
| The title text is not validated with `NameVerification`, profanity protection, or an explicit server-side length check in `OnResponse()`. | Players can set punctuation, digits, mixed-case reserved words, or longer forged relay text unless blocked elsewhere by the gump packet/client path. |
| The reserved-word filter only checks specific case variants with trailing spaces. | Words such as a rank at the end of the title, all-caps variants, or punctuation-adjacent variants can bypass the deny list. |
| `Title` is serialized both by base `Mobile` and by `PlayerMobile`. | The stream is aligned today, but this duplicate field is easy to mishandle during future version upgrades. |
