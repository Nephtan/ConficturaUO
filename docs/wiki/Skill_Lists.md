# Skill Lists

## Overview

Skill Lists is a player-facing alternate skill display. It registers the `[skilllist` command and opens `Server.Gumps.SkillListingGump`, a compact `Gump` that shows selected skills with their base and enhanced values.

This system does not replace the normal RunUO skill management gump. The compiled code only reads each `Skill.Lock`, `Skill.Base`, and `Skill.Value`; it does not change skill locks, skill caps, skill gains, or skill titles.

## Source Files

| File | Type | Purpose |
| --- | --- | --- |
| `Data/Scripts/System/Commands/Player/SkillListing.cs` | Player command and `Gump` | Registers `[skilllist`, opens/closes `SkillListingGump`, and renders the compact list. |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Help/settings `Gump` | Toggles whether locked skills appear and provides a button to open the list. |
| `Data/Scripts/System/Commands/Player/QuickBar.cs` | Player quickbar `Gump` | Quickbar button set `40` toggles the Skill List gump. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Player state and refresh hooks | Stores `SkillDisplay`, serializes it, and refreshes the gump on inventory changes. |
| `Data/Scripts/Mobiles/Base/BaseCreature.cs` | Vendor/trainer hook | Refreshes the gump after a creature starts teaching a player. |
| `Data/Scripts/Scripts.csproj` | Build project | Includes `System\Commands\Player\SkillListing.cs` in the scripts build. |

No `Item`, `Mobile`, XMLSpawner attachment, packet handler, loot table, spawn timer, or damage formula was found for this mechanic.

## Entry Points

| Entry point | Access | Compiled behavior |
| --- | --- | --- |
| `[skilllist` | `AccessLevel.Player` | Calls `SkillListingGump.OpenSkillList(e.Mobile)`. |
| Help `Settings` page | Player help gump page `12` | Button ID `982` toggles `PlayerMobile.SkillDisplay`; button ID `983` opens the Skill List. |
| QuickBar button set `40` | Player quickbar selection | If the Skill List is open, closes it; otherwise invokes the `skilllist` command. |

The command metadata is:

| Attribute | Value |
| --- | --- |
| `[Usage]` | `skilllist` |
| `[Description]` | `Shows the player the skills they want to watch.` |

## Player State

`PlayerMobile` stores the locked-skill display option in the public integer field `SkillDisplay`, exposed to GameMasters as the command property `Skill_Display`.

| Value | Meaning in current code |
| --- | --- |
| `0` | Show only skills whose `Skill.Lock == SkillLock.Up`. |
| Any value greater than `0` | Show skills whose lock is `Up`, plus skills whose lock is `Locked`. |

The Help `Settings` page only toggles between `0` and `1`. Values greater than `1` can still exist through direct property edits and are treated like enabled.

`SkillDisplay` is persisted by `PlayerMobile.Serialize` and restored by `PlayerMobile.Deserialize`. `SkillListingGump` itself has no serialization block and holds no persistent state.

## Gump Rendering

`SkillListingGump` is constructed at screen position `(25, 25)` and is closable, disposable, draggable, and non-resizable. It draws static art as a two-column background and extends the art by 24 pixels per row after the first six displayed skills.

Each eligible skill row displays:

| Column | X position | Value |
| --- | ---: | --- |
| Skill label | `8` | Hard-coded display text such as `Alchemy` or `Magic Resist`. |
| Base value | `140` | `Skill.Base`. |
| Effective value | `198` | `Skill.Value`. |

Normal rows use hue `1153`. Rows for locked skills use hue `0x31`, but only when `SkillDisplay > 0` allows locked skills to appear.

The gump has no interactive row buttons. Any `OnResponse(NetState sender, RelayInfo info)` response closes the gump.

## Display Filter

Every rendered skill uses the same condition:

```csharp
fromSkill.Lock == SkillLock.Up
    || (fromSkill.Lock == SkillLock.Locked && SkillDisplay > 0)
```

Skills with `SkillLock.Down` never appear. Locked skills appear only when the player's `SkillDisplay` setting is enabled.

## Rendered Skill Roster

The compiled gump manually checks and renders these skills:

| Rendered labels |
| --- |
| Alchemy, Anatomy, Arms Lore, Begging, Blacksmithing |
| Bludgeoning, Bowcrafting, Bushido, Camping, Carpentry |
| Cartography, Cooking, Discordance, Druidism, Elementalism |
| Fencing, Fist Fighting, Focus, Forensics, Healing |
| Herding, Hiding, Knightship, Inscription, Lockpicking |
| Lumberjacking, Magery, Magic Resist, Marksmanship, Meditation |
| Mercantile, Mining, Musicianship, Necromancy, Ninjitsu |
| Parrying, Peacemaking, Poisoning, Provocation, Psychology |
| Remove Trap, Seafaring, Searching, Snooping, Spiritualism |
| Stealing, Stealth, Swordsmanship, Tactics, Tailoring |
| Taming, Tasting, Tinkering, Tracking, Veterinary |

## Refresh Behavior

`SkillListingGump.RefreshSkillList(Mobile from)` only redraws the gump when `from` is a `PlayerMobile` and that player already has `SkillListingGump` open. It closes the existing instance and sends a new one.

Compiled refresh hooks found during the trace:

| Caller | Refresh condition |
| --- | --- |
| `PlayerMobile.OnSubItemAdded(Item item)` | Any sub-item addition after quickbar and reagent-bar refresh. |
| `PlayerMobile.OnItemAdded(Item item)` | Any item addition after stat/light/play-style processing. |
| `PlayerMobile.OnItemRemoved(Item item)` | Any item removal after stat/light processing. |
| `BaseCreature` teaching flow | After a creature starts teaching a `Mobile` a skill. |
| Help `Settings` button ID `982` | Immediately after toggling `SkillDisplay`. |

The in-game help text says the list does not update in real time, and the compiled code matches that: it refreshes on selected hooks, not on every skill value change.

## Known Issues

* `SkillListingGump` omits `SkillName.Mysticism`, `SkillName.Imbuing`, and `SkillName.Throwing`, even though those skills exist in the core `SkillName` enum. Those skills cannot appear in the alternate list.
* `SkillListingGump.OnResponse` assigns `Mobile from = sender.Mobile` and immediately calls `from.CloseGump(...)` without checking `sender` or `sender.Mobile` for null.
* The Help `Settings` button handler casts `from` to `PlayerMobile` before toggling `SkillDisplay`. The main command and refresh entry points guard `from is PlayerMobile`, but this reply path depends on the Help Gump only being used by player mobiles.
* The skill roster is duplicated manually in separate count and render blocks. This has already drifted from the core skill enum and makes future skill additions easy to miss.
