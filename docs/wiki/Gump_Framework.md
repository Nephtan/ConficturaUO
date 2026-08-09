# Gump Framework

## Overview
The Gump Framework is the server-side UI pipeline built around `Server.Gumps.Gump`. It covers the engine classes in `Data/System/Source/Gumps`, the response packet handler in `Data/System/Source/Network/PacketHandlers.cs`, and reusable script-level gumps in `Data/Scripts/System/Gumps`.

Gumps are not world objects. They are temporary `NetState` UI objects sent to connected clients, tracked in `NetState.Gumps`, and removed when a response packet is accepted or when server code closes them.

## Runtime Flow
| Stage | Compiled behavior |
| --- | --- |
| Construction | `Gump(int x, int y)` assigns a nonzero serial, stores the screen origin, calculates `TypeID` from `type.FullName.GetHashCode()`, and initializes entry/string lists. |
| Layout building | Script constructors call helpers such as `AddPage`, `AddBackground`, `AddButton`, `AddHtml`, `AddLabel`, and `AddTextEntry`; each helper wraps a `GumpEntry` subclass and attaches it to the gump. |
| Sending | `Mobile.SendGump(Gump g)` calls `g.SendTo(m_NetState)`. `Gump.SendTo` stores the gump on the `NetState` and sends the compiled display packet. |
| NetState cap | `NetState.AddGump` allows up to `NetState.GumpCap`, default `512`; exceeding the cap writes a console warning and disposes the client connection. |
| Packet selection | `Gump.Compile(NetState ns)` sends `DisplayGumpPacked` packet `0xDD` when `ns.Unpack` is true; otherwise it sends `DisplayGumpFast` packet `0xB0`. |
| Layout flags | `Dragable`, `Closable`, `Disposable`, and `Resizable` compile as `nomove`, `noclose`, `nodispose`, and `noresize` when disabled. |
| Entry serialization | Each `GumpEntry.AppendTo` writes one layout command. Text strings are interned into the gump's string table and written as big-endian Unicode strings. |
| Client response | Packet `0xB1` reads serial, type ID, button ID, switch IDs, and text entries; it accepts the response only for a matching gump already tracked on that `NetState`. |
| Response validation | Switch count must be between `0` and the compiled switch count. Text-entry count must be between `0` and the compiled text-entry count. Individual text responses longer than `239` characters disconnect the client. |
| Dispatch | A valid response removes the gump from the `NetState`, profiles the gump type, then calls `OnResponse(NetState state, RelayInfo info)`. |
| Server close | `Mobile.CloseGump(Type type)` sends a `CloseGump` packet, removes the tracked gump, and calls `OnServerClose(NetState owner)`. `CloseAllGumps()` does the same for every open gump. |

## Core Entry Types
| Helper | Entry class | Client command / response role |
| --- | --- | --- |
| `AddPage(page)` | `GumpPage` | Starts a page section for client-side page buttons. |
| `AddBackground(x, y, w, h, id)` | `GumpBackground` | Adds a background gump art region. |
| `AddAlphaRegion(x, y, w, h)` | `GumpAlphaRegion` | Adds a translucent region. |
| `AddButton(...)` | `GumpButton` | Adds a button. `GumpButtonType.Page` changes client page; `GumpButtonType.Reply` submits `ButtonID`. |
| `AddCheck(...)` | `GumpCheck` | Adds a checkbox and increments the allowed switch count. |
| `AddRadio(...)` | `GumpRadio` | Adds a radio control and increments the allowed switch count. |
| `AddGroup(group)` | `GumpGroup` | Groups radio controls. |
| `AddTooltip(number)` | `GumpTooltip` | Adds a localized tooltip. |
| `AddHtml(...)` | `GumpHtml` | Adds raw HTML text using an interned string. |
| `AddHtmlLocalized(...)` | `GumpHtmlLocalized` | Adds localized cliloc HTML, optionally with color or token arguments. |
| `AddImage(...)` | `GumpImage` | Adds static gump art. |
| `AddImageTiled(...)` | `GumpImageTiled` | Adds repeated gump art. |
| `AddImageTiledButton(...)` | `GumpImageTileButton` | Adds a button with tile art and optional localized tooltip. |
| `AddItem(...)` | `GumpItem` | Renders an item art ID, optionally hued. |
| `AddLabel(...)` | `GumpLabel` | Adds text using an interned string. |
| `AddLabelCropped(...)` | `GumpLabelCropped` | Adds text clipped to a rectangle. |
| `AddTextEntry(...)` | `GumpTextEntry` / `GumpTextEntryLimited` | Adds a text field and increments the allowed text-entry count. |

`RelayInfo` exposes the submitted `ButtonID`, raw `Switches`, raw `TextEntries`, `IsSwitched(int switchID)`, and `GetTextEntry(int entryID)`. Gump code is responsible for validating button ranges and any stored object state.

## Administrative Add Gumps
| Command | Access | Usage | Compiled behavior |
| --- | --- | --- | --- |
| `[Add [<name> [params] [set {<propertyName> <value> ...}]]]` | GameMaster | Generic command usage string is `Add [<name> [params] [set {<propertyName> <value> ...}]]`. | With no arguments, opens `CategorizedAddGump`. With a valid type name, targets a location and calls `Server.Commands.Add.Invoke`. With an invalid type name, opens `AddGump` search when the search term is at least three characters. |
| `[AddMenu [searchString]]` | GameMaster | Optional initial search string. | Opens `AddGump`. An empty search shows no results; a search shorter than three characters sends "Invalid search string."; longer terms search constructable `Item` and `Mobile` types. |
| `[Tile <name> [params] [set ...]]` | GameMaster | Targets a bounding box. | Calls `Add.Invoke` across the selected box. |
| `[TileRXYZ <x> <y> <w> <h> <z> <name> [params] [set ...]]` | GameMaster | Relative rectangle from caller location. | Calls `Add.Invoke` for a rectangle whose origin is offset from the caller. |
| `[TileXYZ <x> <y> <w> <h> <z> <name> [params] [set ...]]` | GameMaster | Absolute rectangle. | Calls `Add.Invoke` for an absolute world rectangle. |
| `[TileZ <z> <name> [params] [set ...]]` | GameMaster | Targets a bounding box at a fixed Z. | Calls `Add.Invoke` after forcing both target points to the requested Z. |

### `AddGump`
`AddGump` is a search-driven GM menu. It displays ten results per page and uses button IDs `1` for search, `2` for previous page, `3` for next page, and `4 + index` for result selection.

Search matching scans `ScriptCompiler.Assemblies` and `Core.Assembly` type caches. A result must be assignable to `Item` or `Mobile`, its type name must contain the lowercase search term, it must not already be in the result list, and it must have a zero-parameter constructor marked with `[Constructable]`.

Selecting a result assigns `AddGump.InternalTarget` to the caller. The target accepts any `IPoint3D`; targeted `Item` objects are converted to `GetWorldTop()`, targeted `Mobile` objects use `Mobile.Location`, and the final point is passed to `Server.Commands.Add.Invoke`. After a successful target selection, the target is assigned again so staff can place repeated copies until cancellation. Pressing escape returns to the same `AddGump` page.

### `CategorizedAddGump`
`CategorizedAddGump` loads `CAGCategory.Root` from `Data/System/XML/objects.xml` the first time the root is requested. The XML parser accepts:

| XML node / attribute | Compiled behavior |
| --- | --- |
| `<category title="...">` | Creates a `CAGCategory` with child `category` and `object` nodes. |
| `<object type="...">` | Resolves the type with `ScriptCompiler.FindTypeByFullName(xml.Value, false)`. |
| `gfx="..."` | Stores an item art ID preview. |
| `hue="..."` | Stores a hue value, although the current preview path calls `AddItem(..., itemID)` without applying the stored hue. |
| Category title `Docked` | Renamed in code to `Docked 2`. |

The gump displays fifteen entries per page. Button `1` moves to the parent category, `2` moves to the previous page, `3` moves to the next page, and `4 + row` activates the selected node. Category nodes open a child `CategorizedAddGump`; object nodes execute `[Add <TypeName>]` through `CommandSystem.Handle` and then reopen the current category page.

## Reusable Base Gumps
| Class | Purpose | Response model |
| --- | --- | --- |
| `BaseConfirmGump` | Non-closable warning dialog with localized title/label, two radio choices, and one confirm button. | Confirm button calls `Confirm(state.Mobile)` when the `Break` radio is switched; otherwise it calls `Refuse(state.Mobile)`. Derived classes override those virtual methods. |
| `BaseGridGump` | Helper for table-like layouts with tracked current X/Y/page, standard art IDs, headers, entries, text fields, and buttons. | `GetButtonID(typeCount, type, index)` encodes IDs as `1 + (index * typeCount) + type`; `SplitButtonID` reverses that mapping. |
| `BaseImageTileButtonsGump` | Tile-art button picker with a header, cancel button, page navigation, and a `2 x 5` default grid. | Button IDs start at `100 + index`. Valid buttons call `HandleButtonResponse`; all other button IDs call `HandleCancel`. |

## Persistence And Serialization
Gumps in this framework do not implement RunUO world serialization. Their state is constructor/private-field state held in memory while the gump is tracked on a `NetState`. Server restarts, disconnects, and gump closure discard that state.

The only related persistence is external data used to build gumps, such as `Data/System/XML/objects.xml` for `CategorizedAddGump`. Because `CAGCategory.Root` is cached in a static field after first load, changes to that XML file are not reloaded by this code path until the process restarts.

## Known Issues
1. `GumpEntry.Parent` and `Gump.Remove(GumpEntry)` recurse when an attached entry is detached or moved through the public parent/remove API. `Gump.Remove` sets `g.Parent = null`; the setter calls `m_Parent.Remove(this)` before clearing `m_Parent`, which can recurse until stack overflow.
2. `CategorizedAddGump` calls `owner.CloseGump(typeof(WhoGump))` in its constructor instead of closing `typeof(CategorizedAddGump)`, so repeated `[Add` menu openings can stack categorized add gumps on the client.
3. `CAGCategory.Load` closes its `XmlTextReader` only when it finds and returns the first root category. If `objects.xml` exists but has no root category, or an exception interrupts parsing, the reader is not closed.
4. `CAGCategory` hard-codes the title `Docked` to `Docked 2`, which is a data-specific workaround embedded in the framework parser.
5. `CAGObject` stores the XML `hue` attribute, but the preview renderer calls `AddItem(..., itemID)` without the hue overload, so categorized add previews ignore configured hues.
6. `GumpHtmlLocalized` contains an unresolved source comment asking whether multiple arguments and non-ASCII arguments are supported. The argument path writes `m_Args` directly into the layout string.
7. The `[Add` path used by categorized object selection and targeted placement ultimately uses `Server.Commands.Add.Invoke`; that shared command path contains a hex parsing defect where parsed hex values are coerced through `Convert.ToInt32`, even for wider numeric constructor parameter types.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0094.
- Backlog rows: RB-06701.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/System/Source/Gumps (CurrentDirectory)
- Data/System/Source/Network/PacketHandlers.cs (CurrentFile)
- Data/Scripts/System/Gumps (CurrentDirectory)

### Runtime Evidence

- Hook summary: Command=5; Event=29; Gump=357; Initialize=5; Timer=3.
- Data/Scripts/System/Gumps/AddGump.cs:L18 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Gumps/AddGump.cs:L20 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Gumps/AddGump.cs:L52 Gump SendGump access=Internal
- Data/Scripts/System/Gumps/AddGump.cs:L247 Gump SendGump access=Internal
- Data/Scripts/System/Gumps/AddGump.cs:L251 Gump OnResponse access=Internal
- Data/Scripts/System/Gumps/AddGump.cs:L265 Gump SendGump access=Internal
- Data/Scripts/System/Gumps/AddGump.cs:L269 Gump SendGump access=Internal
- Data/Scripts/System/Gumps/AddGump.cs:L277 Gump SendGump access=Internal
- Data/Scripts/System/Gumps/AddGump.cs:L286 Gump SendGump access=Internal
- Data/Scripts/System/Gumps/BanDurationGump.cs:L81 Gump OnResponse access=Internal
- Data/Scripts/System/Gumps/BanDurationGump.cs:L267 Gump SendGump access=Internal
- Data/Scripts/System/Gumps/BaseConfirmGump.cs:L67 Gump OnResponse access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 1.
- Data/System/Source/Network/PacketHandlers.cs:Unknown version=Unknown serialize=L deserialize=L alignment=AlignedByCountAndKnownTypes

### Project And Runtime Coverage

- Data/Scripts/System/Gumps/AddGump.cs=Keep
- Data/Scripts/System/Gumps/AddGump.cs=Keep
- Data/Scripts/System/Gumps/BanDurationGump.cs=Keep
- Data/Scripts/System/Gumps/BanDurationGump.cs=Keep
- Data/Scripts/System/Gumps/BaseConfirmGump.cs=Keep
- Data/Scripts/System/Gumps/BaseConfirmGump.cs=Keep
- Data/Scripts/System/Gumps/BaseGridGump.cs=Keep
- Data/Scripts/System/Gumps/BaseGridGump.cs=Keep
- Data/Scripts/System/Gumps/BaseImageTileButtonsGump.cs=Keep
- Data/Scripts/System/Gumps/BaseImageTileButtonsGump.cs=Keep
- Data/Scripts/System/Gumps/CategorizedAddGump.cs=Keep
- Data/Scripts/System/Gumps/CategorizedAddGump.cs=Keep
- Data/Scripts/System/Gumps/ClientGump.cs=Keep
- Data/Scripts/System/Gumps/ClientGump.cs=Keep
- Data/Scripts/System/Gumps/ClueGump.cs=Keep
- Data/Scripts/System/Gumps/ClueGump.cs=Keep
- Data/Scripts/System/Gumps/ConfirmBreakCrystalGump.cs=Keep
- Data/Scripts/System/Gumps/ConfirmBreakCrystalGump.cs=Keep
- Data/Scripts/System/Gumps/ConfirmHouseResize.cs=Keep
- Data/Scripts/System/Gumps/ConfirmHouseResize.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
