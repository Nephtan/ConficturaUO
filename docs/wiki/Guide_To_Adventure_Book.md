# Guide To Adventure Book

## Scope

This page documents the `Guide to Adventure` `DynamicBook` implementation. It covers the concrete `Item` classes, reading `Gump` flow, text source, acquisition paths, vendor stock, and serialization behavior traced from the C# source.

This system does not register commands, EventSink hooks, packet handlers, XMLSpawner attachments, or custom `Mobile` AI. Its behavior is driven by `Item.OnDoubleClick`, vendor buy lists, and helper gumps.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Items/Books/DynamicBook.cs` | Defines `DynamicBook`, its reading gumps, `BasicHelp()`, `SetStaticText()`, `LoreGuidetoAdventure`, and `BookGuideToAdventure`. |
| `Data/Scripts/Items/Containers/GypsyShelf.cs` | `GypsyShelf` replacement path that grants owner-bound `BookGuideToAdventure` copies. |
| `Data/Scripts/Mobiles/Civilized/Guilds/LibrarianGuildmaster.cs` | Adds `SBLibraryGuild` vendor stock to librarian guildmasters. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | Defines `SBLibraryGuild`, which sells `LoreGuidetoAdventure`. |
| `Data/Scripts/System/Commands/Player/Basics.cs` | `BasicsGump` reuses `DynamicBook.BasicHelp()` outside the physical book. |
| `Data/Scripts/System/Commands/Player/MyLibrary.cs` | Provides the built-in Library `Basics` button and receives `DynamicBook.readBook()` notifications after book reads. |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Help text points new players toward the Guide to Adventure book. |

## Book Item Classes

Both guide variants derive from `DynamicBook`, use the title `Guide to Adventure`, call `SetBookCover(5, this)`, and populate their text through `SetStaticText(this)`.

`SetBookCover(5, this)` maps to `BookCover = 0x4F5`, labeled by the source comment as the sword-and-shield cover.

| Class | Constructable | ItemID | Weight | Author | Owner field | Intended source |
| --- | --- | --- | ---: | --- | --- | --- |
| `LoreGuidetoAdventure` | Yes | `Utility.RandomList(0x4FDF, 0x4FE0)` | `1.0` | `RandomThings.GetRandomAuthor()` | None | Vendor-bought copy. |
| `BookGuideToAdventure` | Yes | `Utility.RandomList(0x4FDF, 0x4FE0)` | `1.0` | `RandomThings.GetRandomAuthor()` | `Mobile owner`, exposed as `Owner` to `GameMaster` | `GypsyShelf` replacement copy. |

The base `DynamicBook` constructor can create a random book when `BookTitle` is empty, but both guide subclasses assign all guide metadata in their own constructors after the base constructor has run.

## Reading Flow

`DynamicBook.OnDoubleClick(Mobile e)` allows a read when either condition is true:

| Gate | Code behavior |
| --- | --- |
| Library/temp item bypass | `Weight == -50.0` bypasses range, visibility, and line-of-sight checks. `MyLibrary` uses this weight before opening temporary book instances. |
| World item read | Reader must be within range `5`, must be able to see the item, and must have line of sight to it. |

The Guide item IDs are `0x4FDF` or `0x4FE0`, so they use the normal `DynamicBookGump` branch rather than the special Syth (`0x4CDF`) or Jedi (`0x543C`) gumps.

`DynamicBookGump` displays:

| Element | Value |
| --- | --- |
| Origin | `base(100, 100)` |
| Title region | `book.BookTitle` |
| Author region | `by ` + `book.BookAuthor` |
| Text region | Scrollable HTML area containing `book.BookText` |
| Primary color | `#d6c382` |
| Cover art | `book.BookCover` rendered over the gump art |

After opening a book gump, `DynamicBook.OnDoubleClick()` calls `Server.Gumps.MyLibrary.readBook(this, e)`. The traced `readBook()` name map does not include a `Guide to Adventure` branch, so reading the physical guide does not appear to unlock a numbered player-library entry.

## Text Source

`SetStaticText(DynamicBook book)` assigns `book.BookText = BasicHelp()` for both `BookGuideToAdventure` and `LoreGuidetoAdventure`.

`BasicHelp()` builds one long HTML string. Its content covers these sections:

| Section | Code-backed content |
| --- | --- |
| `BASICS OF THE GAME` | Client movement, right-click movement, closing windows, first login UI, and general interaction basics. |
| `PAPERDOLL` | Paperdoll slots, character name/title, Help, Options, Log Out, Stats, Skills, Guild, Peace, Status, account-age scroll, party scroll, and backpack. |
| `MENU BAR` | Mini map, paperdoll, inventory, journal, disabled chat button, help button, and the old information button. |
| `BACKPACK` | Backpack opening, carried weight, nested containers, and container behavior across world travel. |
| `OPTIONS` | Sound/music, fonts, macros, profanity filtering, pathfinding, war mode, targeting, menu bar, and UI window offsets. |
| `STATS` | Stat bonuses, karma/fame, hunger/thirst, bandage/cast speed, murder counts, and tithing points. |
| `SKILLS` | Skill list, 1,000-point total cap, gain/lock/down arrows, magic item modifiers, real/cap display, scrolls of power, and custom groups. |
| `STATUS` | Attributes, hit/stam/mana, luck, carried weight/gold, followers, damage, stat cap, resistance values, and command examples such as `[c`, `[status`, and `[motd`. |
| `CHAT` | Character-specific chat through `[c`, offline messages, channels, and privacy controls. |
| `CITIZENS` | Context menus, training, hire options, war-mode risk, murder status, and murder decay text. |
| Conditional bribe paragraph | Included only when `MyServerSettings.AllowBribes() >= 1000`; the text uses `AllowBribes()` as the listed gold price and says assassin guild members pay half. |
| `BANKS` | Bank-box storage, banker `bank` speech, non-gold currency exchange, double-click currency conversion in bank, and `check <amount>` speech. |
| `INNS & TAVERNS` | Safe rest areas, no spellcasting or attacks, instant logout, tavern games, henchmen, and rumors. |
| `PRACTICE` | Training dummies, archery buttes, and thief pickpocketing practice dummies. |
| `COMBAT` | War/peace toggle, double-click attack, melee/ranged range expectations, followers, spells, retreating, murder counts, and highlight color cues. |
| `WEAPONS` | Tooltips, damage type, swing/shoot rate, strength requirement, combat skill, one-handed/two-handed handling, magic properties, durability, and repairs. |
| `ARMOR` | Body slots, strength requirement, durability, equip failure causes, meditation/stealth penalties, leather armor, and mage armor. |
| `MAGIC GATES` | Gate colors, wizard-summoned gates, necromantic black gates, destination discovery, and rare monster-gate rewards. |
| `MAGIC RUNES` | Marked recall runes, world-colored runes, runebooks, 16 rune capacity, and scroll charges. |
| `BOOKS OF MAGIC` | Spellbooks, necromancer spellbooks, scroll scribing, consumable scroll casts, knightship/samurai/ninja books, spell icons, components, and sage/scribe convenience options. |
| `DEATH` | Resurrection options, penalties, healing/magic/potion resurrection, healer/shrine recovery, bank/tithe cost dependency, and free resurrection threshold for low-stat/low-skill characters. |
| `FINALLY` | Exploration, sages, resource/skill instruction scrolls, dungeon looting, trap evasion, reagents, food, and drink. |

## Acquisition

### Gypsy Shelf

`GypsyShelf.OnDoubleClick(Mobile from)` checks the caller's backpack for `BookGuideToAdventure`.

| Condition | Result |
| --- | --- |
| Backpack already contains `BookGuideToAdventure` | Sends `The other books here seem uninteresting to you.` |
| Backpack does not contain one | Calls `GiveBook(from)`. |

`GiveBook(from)` performs these steps:

| Step | Behavior |
| --- | --- |
| Duplicate cleanup | `GetRidOf(from)` scans `World.Items.Values`, finds every `BookGuideToAdventure` whose `owner == from`, and deletes it. |
| New item | Creates a new `BookGuideToAdventure`. |
| Feedback | Plays sound `0x02E`. |
| Ownership | Sets `book.owner = from`. |
| Placement | Calls `from.AddToBackpack(book)`. |
| Message | Sends `You take a book from the gypsy's shelf.` |

### Librarian Guildmaster Stock

`LibrarianGuildmaster.InitSBInfo()` adds `SBLibraryGuild`. `SBLibraryGuild.InternalBuyInfo` sells `LoreGuidetoAdventure`.

| Stock entry | Value |
| --- | --- |
| Type | `typeof(LoreGuidetoAdventure)` |
| Price | `5` gold |
| Amount expression | `Utility.Random(5, 15)` |
| Actual amount range | `5` through `19`, because `Utility.Random(from, count)` returns `from + Random(count)` |
| Display ItemID | `0x4FDF` |
| Hue | `Utility.RandomColor(0)` |

## Library And Basics Gump

The physical guide is not the only code path to the same text.

| Entry point | Behavior |
| --- | --- |
| `BasicsGump` | Renders `DynamicBook.BasicHelp()` in a scrollable help gump and can return to `HelpGump` when opened from a Help origin. |
| `MyLibrary` button `400` | Labeled `Basics`; closes any existing `BasicsGump` and opens `new BasicsGump(from, 0)`. |

## Serialization

`DynamicBook` serialization is positional and versioned with a plain `int` version `0`.

| Order | Written field | Read method |
| ---: | --- | --- |
| 1 | `BookCover` | `ReadInt()` |
| 2 | `BookTitle` | `ReadString()` |
| 3 | `BookAuthor` | `ReadString()` |
| 4 | `BookText` | `ReadString()` |
| 5 | `BookRegion` | `ReadString()` |
| 6 | `BookMap` | `ReadMap()` |
| 7 | `BookWorld` | `ReadString()` |
| 8 | `BookItem` | `ReadString()` |
| 9 | `BookTrue` | `ReadInt()` |
| 10 | `BookPower` | `ReadInt()` |

`LoreGuidetoAdventure.Serialize()` calls `base.Serialize(writer)` and then writes an encoded subclass version `0`. Its `Deserialize()` reads that encoded version and calls `SetStaticText(this)`, so the current compiled `BasicHelp()` text replaces the saved text after loading.

`BookGuideToAdventure.Serialize()` calls `base.Serialize(writer)`, writes an encoded subclass version `0`, and then writes `(Mobile)owner`. Its `Deserialize()` reads the encoded version, calls `SetStaticText(this)`, and then restores `owner` with `reader.ReadMobile()`.

## Known Issues

| Issue | Impact |
| --- | --- |
| `GypsyShelf.OnDoubleClick()` dereferences `from.Backpack` without checking `from` or `from.Backpack` for null. | A non-player caller or malformed mobile state can throw before the shelf can grant the book. |
| `GypsyShelf.GiveBook()` does not validate `from.AddToBackpack(book)` success. | If the backpack path fails, the method still plays the sound and sends the success message after assigning ownership. |
| `GetRidOf(from)` scans all `World.Items.Values` on each shelf grant. | The duplicate cleanup is global and potentially expensive on large worlds. |
| Help text says a new player can buy a replacement Guide to Adventure from a sage, but the traced vendor stock only found `LoreGuidetoAdventure` in `SBLibraryGuild`; `Sage.InitSBInfo()` uses `SBSage`, whose traced stock does not include the guide. | Player-facing directions may point to the wrong vendor type. |
| `DynamicBookGump`, `DynamicSythGump`, `DynamicJediGump`, `BasicsGump`, and `MyLibrary.OnResponse()` use `state.Mobile` or `sender.Mobile` without null guards. | Stale gump responses can null-reference if the `NetState` or `Mobile` is invalid. |
| No `BookGuideToAdventure` grant was found in `CharacterCreation.cs`; the traced startup pack gives gold, water, a weapon, food, lights, and UI settings. | The audit note calls this a starter book, but the traced code does not show a new-character grant path. |
| `MyLibrary.readBook()` does not include a `Guide to Adventure` branch. | Opening the physical guide does not appear to memorize the guide as a numbered library book, although `MyLibrary` has a separate built-in `Basics` button for the same help text. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0093.
- Backlog rows: RB-06700.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Items/Books/DynamicBook.cs (CurrentFile)
- Data/Scripts/Items/Containers/GypsyShelf.cs (CurrentFile)
- Data/Scripts/Mobiles/Civilized/Guilds/LibrarianGuildmaster.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/StoreSalesList.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/Basics.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/MyLibrary.cs (CurrentFile)
- Data/Scripts/System/Help/Gumps/HelpGump.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Event=1; Gump=92; Initialize=1; Timer=2.
- Data/Scripts/Items/Books/DynamicBook.cs:L77 Gump OnResponse access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L122 Gump OnResponse access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L181 Gump OnResponse access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L452 Gump SendGump access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L460 Gump SendGump access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L468 Gump SendGump access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L858 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Books/DynamicBook.cs:L900 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Civilized/Guilds/LibrarianGuildmaster.cs:L196 Gump SendGump access=Internal
- Data/Scripts/System/Commands/Player/Basics.cs:L53 Gump OnResponse access=Internal
- Data/Scripts/System/Commands/Player/Basics.cs:L59 Gump SendGump access=Internal
- Data/Scripts/System/Commands/Player/MyLibrary.cs:L185 Gump OnResponse access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 26.
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.AlchemicalElixirs version=Unknown serialize=L1523 deserialize=L1529 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.AlchemicalMixtures version=Unknown serialize=L1563 deserialize=L1569 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.BookBottleCity version=Unknown serialize=L1017 deserialize=L1023 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.BookGuideToAdventure version=Unknown serialize=L977 deserialize=L984 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.BookofDeadClue version=Unknown serialize=L1071 deserialize=L1077 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.BookOfPoisons version=Unknown serialize=L1603 deserialize=L1609 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookDruidicHerbalism version=Unknown serialize=L888 deserialize=L894 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookElvesandOrks version=Unknown serialize=L1151 deserialize=L1157 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookNecroticAlchemy version=Unknown serialize=L847 deserialize=L853 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookTheLostTribeofSosaria version=Unknown serialize=L1327 deserialize=L1333 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookTombofDurmas version=Unknown serialize=L1111 deserialize=L1117 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.DynamicBook version=0 serialize=L482 deserialize=L498 alignment=CountMatchNeedsTypeReview:UnknownWrites=10
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Items/Books/DynamicBook.cs=Keep
- Data/Scripts/Items/Books/DynamicBook.cs=Keep
- Data/Scripts/Items/Containers/GypsyShelf.cs=Keep
- Data/Scripts/Items/Containers/GypsyShelf.cs=Keep
- Data/Scripts/Mobiles/Base/StoreSalesList.cs=Keep
- Data/Scripts/Mobiles/Base/StoreSalesList.cs=Keep
- Data/Scripts/Mobiles/Civilized/Guilds/LibrarianGuildmaster.cs=Keep
- Data/Scripts/Mobiles/Civilized/Guilds/LibrarianGuildmaster.cs=Keep
- Data/Scripts/System/Commands/Player/Basics.cs=Keep
- Data/Scripts/System/Commands/Player/Basics.cs=Keep
- Data/Scripts/System/Commands/Player/MyLibrary.cs=Keep
- Data/Scripts/System/Commands/Player/MyLibrary.cs=Keep
- Data/Scripts/System/Help/Gumps/HelpGump.cs=Keep
- Data/Scripts/System/Help/Gumps/HelpGump.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
