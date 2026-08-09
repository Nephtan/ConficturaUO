# Magery Spell Color Setting

## Scope

This page documents the player spell-effect hue setting exposed through `Server.Engines.Help.HelpGump` and the `[spellhue` player command.
The system stores one integer on `PlayerMobile` and only affects effects that explicitly call `Server.Misc.PlayerSettings.GetMySpellHue()`.
It does not define a new `Mobile`, `Item`, packet handler, EventSink hook, XMLSpawner attachment, or damage formula.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Draws the Help Gump setting row, opens the `Magery Spell Color` info page, and handles fixed-color button IDs `500` through `507`. |
| `Data/Scripts/System/Commands/Player/SpellHue.cs` | Registers the `[spellhue` player command and writes a custom integer hue to `PlayerMobile.MagerySpellHue`. |
| `Data/Scripts/Mobiles/Base/PlayerSettings.cs` | Provides `GetMySpellHue(bool mod, Mobile m, int hue)`, the shared helper used by spell and effect code. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Stores `MagerySpellHue`, exposes it as the `Magery_SpellHue` `CommandProperty`, and serializes it. |
| `Data/Scripts/System/Misc/Broadcast.cs` | Copies `MagerySpellHue` from a legacy `CharacterDatabase` item into online `PlayerMobile` instances during login processing. |
| `Data/Scripts/System/Obsolete/Obsolete.cs` | Defines the legacy `CharacterDatabase` item that also serializes `MagerySpellHue`. |
| `Data/Scripts/Magic/Magery/` | Contains Magery spell effects that call `GetMySpellHue()`. |
| `Data/Scripts/Magic/Research/` | Contains researched spell effects that also call `GetMySpellHue()`. |
| `Data/Scripts/Items/Traps/` and `Data/Scripts/Items/Magical/SoulOrb.cs` | Contain non-spell effect callers that reuse the same hue helper. |

## Player Controls

The Help Gump setting row is labeled `Magery Spell Color`. Its info button opens page `96`, whose text explains that the setting changes Magery spell effect colors, offers fixed options, and points players to `[spellhue` for custom hue numbers.

The fixed Help Gump buttons write these values directly to `PlayerMobile.MagerySpellHue`:

| Button ID | Label | Stored hue |
| ---: | --- | ---: |
| `500` | White | `0x47E` |
| `501` | Black | `0x94E` |
| `502` | Blue | `0x48D` |
| `503` | Red | `0x48E` |
| `504` | Green | `0x48F` |
| `505` | Purple | `0x490` |
| `506` | Yellow | `0x491` |
| `507` | Default | `0` |

The selected fixed option is shown with gump art ID `4018`; unselected options use gump art ID `3609`.
After a fixed option is selected, `HelpGump.OnResponse()` sends a new `HelpGump(from, 12)`.

## Command

| Command | Access level | Usage attribute | Actual parser | Effect |
| --- | --- | --- | --- | --- |
| `[spellhue` | `AccessLevel.Player` | `spellhue [<name>]` | Reads `e.GetInt32(0)` when at least one argument exists. | Stores the parsed integer on `((PlayerMobile)e.Mobile).MagerySpellHue` and sends `You have changed your magery spell effects color.` |

If no argument is supplied, the command stores `0`, which restores default caller-provided hues.
There is no hue-range validation.

## Hue Resolution

All runtime behavior goes through:

```csharp
PlayerSettings.GetMySpellHue(bool mod, Mobile m, int hue)
```

The helper only overrides the supplied `hue` when `m is PlayerMobile` and `((PlayerMobile)m).MagerySpellHue > 0`.

| Caller input | Stored `MagerySpellHue` | Return value |
| --- | ---: | ---: |
| Any `mod` value, non-player caster | Any value | Original caller-supplied `hue` |
| Any `mod` value, player caster | `0` or less | Original caller-supplied `hue` |
| `mod == false`, player caster | Greater than `0` | Stored `MagerySpellHue` |
| `mod == true`, player caster | Greater than `0` | Stored `MagerySpellHue - 1` |

The `mod == true` path means `[spellhue 1` returns `0` for those callers.
The Help Gump's fixed values are the stored values; many particle-effect callers receive one less than the listed value because they pass `mod == true`.
Object hue callers, such as field items and gates, pass `mod == false` and receive the stored value exactly.

## Effect Coverage

This setting is not globally injected by the spell engine.
It only affects scripts that manually call `GetMySpellHue()`.

| Caller category | Observed behavior |
| --- | --- |
| Magery instant and target effects | Many Magery spells pass `mod == true`, replacing the effect hue with the stored hue minus one. |
| Magery field and travel objects | Field items and gates set their `Item.Hue` through `mod == false`, so they use the stored hue exactly. |
| Research magic | Researched spells in multiple schools call the same helper, often with non-zero default hues that are overridden when a player hue is set. |
| Traps and magical items | Trap visuals and `SoulOrb` reuse the helper, so they can also inherit the triggering player or owner hue when they call it. |

## Serialization Notes

`PlayerMobile` stores the value as public integer field `MagerySpellHue` with a Game Master-visible `CommandProperty` named `Magery_SpellHue`.
Current `PlayerMobile` serialization writes version `37`; `MagerySpellHue` is part of the older version `29` data block and is written after `SkillDisplay` and before `ClassicPoisoning`.
The corresponding `Deserialize()` block reads the values in the same order, so the current player save stream is aligned for this field.

The legacy `CharacterDatabase` item in `Obsolete.cs` also writes and reads `MagerySpellHue`.
During login processing, `Broadcast.World_Login()` scans online `PlayerMobile` instances with a `CharacterDatabase` in their bank box and copies that database value back onto each matching `PlayerMobile` object.

## Known Issues

| Issue | Impact |
| --- | --- |
| `SpellHue.cs` declares `[Usage("spellhue [<name>]")]` but parses the first argument with `e.GetInt32(0)`. | Player-facing command metadata says `name`, while the command actually requires an integer hue. |
| `[spellhue` and the Help Gump fixed-color handlers cast `Mobile` directly to `PlayerMobile` without a guard. | Non-`PlayerMobile` callers can throw an invalid-cast exception if these paths are invoked outside the normal player Help Gump flow. |
| `HelpGump.OnResponse()` uses `state.Mobile` immediately and calls `from.SendSound()` before validating `state` or `from`. | Malformed or unexpected `NetState` state can null-reference before any defensive logic runs. |
| `[spellhue` accepts any integer and stores it directly. | Negative or out-of-range hue values can be persisted and later passed into visual-effect calls. |
| The helper only affects scripts that call `GetMySpellHue()`. | Some spell or magic-adjacent effects may keep their original colors if their scripts do not opt into this helper. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0038; PBN-0047; PBN-0057; PBN-0066; PBN-0123.
- Backlog rows: RB-06716; RB-06717; RB-06718; RB-06719; RB-06720.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/System/Help/Gumps/HelpGump.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/SpellHue.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/PlayerSettings.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/PlayerMobile.cs (CurrentFile)
- Data/Scripts/System/Misc/Broadcast.cs (CurrentFile)
- Data/Scripts/System/Obsolete/Obsolete.cs (CurrentFile)
- Data/Scripts/Magic/Magery/ (CurrentDirectory)
- Data/Scripts/Magic/Research/ (CurrentDirectory)
- Data/Scripts/Items/Traps/ (CurrentDirectory)
- Data/Scripts/Items/Magical/SoulOrb.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=14; Event=21; Gump=199; Initialize=25; Login=1; Logout=3; Movement=12; Packet=2; Region=5; Speech=3; Timer=85; WorldSave=2.
- Data/Scripts/Items/Magical/SoulOrb.cs:L39 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Items/Magical/SoulOrb.cs:L41 Event EventSink access=GlobalOrInternal
- Data/Scripts/Items/Magical/SoulOrb.cs:L94 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Traps/BaseTrap.cs:L55 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Items/Traps/BaseTrap.cs:L57 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Items/Traps/DroppedPack.cs:L39 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Items/Traps/FlameSpurtTrap.cs:L29 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Traps/FlameSpurtTrap.cs:L179 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Items/Traps/FlameSpurtTrap.cs:L181 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Items/Traps/HiddenTrap.cs:L3606 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Items/Traps/MushroomTrap.cs:L203 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Traps/SetTrap.cs:L160 Timer Timer.DelayCall access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 287.
- Data/Scripts/Items/Magical/SoulOrb.cs:Server.Items.SoulOrb version=0 serialize=L160 deserialize=L166 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/BaseTrap.cs:Server.Items.BaseTrap version=0 serialize=L89 deserialize=L96 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/Blocker.cs:Server.Items.StopBlock version=0 serialize=L21 deserialize=L27 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/BookBox.cs:Server.Items.BookBox version=0 serialize=L36 deserialize=L42 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/BrokenGear.cs:Server.Items.BrokenGear version=0 serialize=L29 deserialize=L35 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/CurseItem.cs:Server.Items.CurseItem version=0 serialize=L38 deserialize=L44 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/DroppedPack.cs:Server.Items.DroppedContainer version=0 serialize=L25 deserialize=L31 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/FireColumnTrap.cs:Server.Items.FireColumnTrap version=1 serialize=L146 deserialize=L157 alignment=CountMatchNeedsTypeReview:UnknownWrites=3
- Data/Scripts/Items/Traps/FlameSpurtTrap.cs:Server.Items.FlameSpurtTrap version=0 serialize=L206 deserialize=L215 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/GasTrap.cs:Server.Items.GasTrap version=0 serialize=L189 deserialize=L198 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/GiantSpikeTrap.cs:Server.Items.GiantSpikeTrap version=0 serialize=L70 deserialize=L77 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Traps/HiddenTrap.cs:Server.Items.HiddenTrap version=0 serialize=L1315 deserialize=L1322 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Items/Magical/SoulOrb.cs=Keep
- Data/Scripts/Items/Magical/SoulOrb.cs=Keep
- Data/Scripts/Items/Traps/BaseTrap.cs=Keep
- Data/Scripts/Items/Traps/BaseTrap.cs=Keep
- Data/Scripts/Items/Traps/Blocker.cs=Keep
- Data/Scripts/Items/Traps/Blocker.cs=Keep
- Data/Scripts/Items/Traps/BookBox.cs=Keep
- Data/Scripts/Items/Traps/BookBox.cs=Keep
- Data/Scripts/Items/Traps/BrokenGear.cs=Keep
- Data/Scripts/Items/Traps/BrokenGear.cs=Keep
- Data/Scripts/Items/Traps/CurseItem.cs=Keep
- Data/Scripts/Items/Traps/CurseItem.cs=Keep
- Data/Scripts/Items/Traps/DroppedPack.cs=Keep
- Data/Scripts/Items/Traps/DroppedPack.cs=Keep
- Data/Scripts/Items/Traps/FireColumnTrap.cs=Keep
- Data/Scripts/Items/Traps/FireColumnTrap.cs=Keep
- Data/Scripts/Items/Traps/FlameSpurtTrap.cs=Keep
- Data/Scripts/Items/Traps/FlameSpurtTrap.cs=Keep
- Data/Scripts/Items/Traps/GasTrap.cs=Keep
- Data/Scripts/Items/Traps/GasTrap.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
