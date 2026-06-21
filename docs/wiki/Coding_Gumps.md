# Coding Gumps

## Scope

This guide is the practical companion to [Gump Framework](Gump_Framework.md).
`Gump_Framework.md` describes the engine pipeline. This page is about reading, sketching, designing, and safely coding gumps in Confictura RunUO scripts.

The target reader is a maintainer who can read C# but does not yet have a strong mental model for Ultima Online gump layout code. The examples are intentionally small. Treat them as patterns, not as a single template to paste everywhere.

## What A Gump Is

A gump is a temporary server-built client UI. It is not an `Item`, `Mobile`, world controller, save object, or persistent data store.

The usual flow is:

1. Server code constructs a `Gump` subclass.
2. The constructor calls layout helpers such as `AddPage`, `AddBackground`, `AddImageTiled`, `AddAlphaRegion`, `AddLabel`, `AddHtml`, `AddButton`, `AddCheck`, `AddRadio`, and `AddTextEntry`.
3. `Mobile.SendGump(gump)` sends the compiled layout to the caller's `NetState` and tracks the gump in `NetState.Gumps`.
4. The client replies with packet `0xB1` when a reply button is pressed, a text entry submits, or the user closes the gump.
5. The packet handler matches the response by gump serial and type ID, validates switch and text-entry counts, removes the gump from `NetState.Gumps`, and calls `OnResponse(NetState state, RelayInfo info)`.

Because the gump is removed before `OnResponse`, any follow-up screen must be sent again. Because the client can reply after the world changed, every `OnResponse` must treat constructor fields as stale until revalidated.

Gumps do not serialize themselves. If a gump displays or edits persistent state, that state belongs to some other object, such as `PlayerMobile`, an `Item`, a city controller, a guild, a crafting process, XML data, or a world collection.

## Mental Model

Gump code is immediate, fixed-pixel drawing. There is no layout engine, no flow layout, no automatic wrapping outside the controls that explicitly support it, and no responsive behavior.

| Concept | Practical meaning |
| --- | --- |
| Origin | `base(x, y)` sets the screen position of the whole gump. Coordinates in `Add...` calls are local to that origin. |
| Draw order | Entries are drawn in the order they are added. Backgrounds and tiled regions go first; text, buttons, and items go later. |
| Art IDs | Backgrounds, buttons, images, and item art use client art IDs. The server does not know their visual intent. |
| Bounds | Most helpers take `x, y, width, height`; the client clips or scrolls only for controls that support it. |
| Pages | `AddPage(n)` starts a page section. `GumpButtonType.Page` changes the visible client page without submitting to `OnResponse`. |
| Replies | `GumpButtonType.Reply` submits `ButtonID` to `OnResponse` and closes that tracked gump instance. |
| Button `0` | Usually means close, cancel, or no-op. Many gumps intentionally let `0` fall through as cancel. |
| Switches | `AddCheck` and `AddRadio` create switch IDs. Read them with `info.IsSwitched(id)` or `info.Switches`. |
| Text entries | `AddTextEntry` creates an entry ID. Read it with `info.GetTextEntry(id)`. |
| String table | Labels and HTML are interned into the gump packet string table. Reusing text is cheap, but layout is still fixed. |

The engine validates that the response does not contain more switches or text entries than the gump compiled. It does not validate that a button ID is meaningful, that an index still exists, that the player still owns the item, or that the player still has access to the action.

## Reading Gumps Visually From Code

The fastest way to understand a gump is to sketch the screen in passes. Do not start with `OnResponse`. Start with the constructor and build the image in the same order the client receives it.

### 1. Find The Frame

Start with the constructor signature and base call:

```csharp
public AddGump(Mobile from, string searchString, int page, Type[] searchResults, bool explicitSearch)
    : base(50, 50)
{
```

`base(50, 50)` means the gump opens 50 pixels from the left and 50 pixels from the top of the client viewport. Every later coordinate is local to that origin.

Then find the largest background:

```csharp
AddBackground(0, 0, 420, 280, 5054);
```

That tells you the visible outer box is roughly `420 x 280`. When auditing or editing, write that down before you inspect any rows:

```text
origin: 50,50
local bounds: 0,0 to 420,280
```

If a gump has multiple backgrounds, treat the largest one as the frame and the smaller ones as panels.

### 2. Sketch Panels Before Content

Most maintained gumps use tiled art and alpha regions to create panel surfaces:

```csharp
AddImageTiled(10, 40, 400, 200, 2624);
AddAlphaRegion(10, 40, 400, 200);
```

Sketch this as a rectangle from `10,40` to `410,240`. It is the content body. If a header or footer is also tiled, sketch those separately:

```text
0,0  +------------------------------------------+ 420,280
     | header: 10,10 400x20                    |
     |                                          |
     | body:   10,40 400x200                   |
     |                                          |
     | footer: 10,250 400x20                   |
     +------------------------------------------+
```

Alpha regions are visual overlays. They do not change hitboxes or clipping. They matter because they darken or soften the art below, which affects text readability.

### 3. Overlay Labels, HTML, Buttons, And Items

Read `AddLabel`, `AddHtml`, `AddItem`, and `AddButton` as placed objects on top of the panels.

```csharp
AddButton(10, 9, 4011, 4013, 1, GumpButtonType.Reply, 0);
AddTextEntry(44, 10, 180, 20, 0x480, 0, searchString);
AddHtmlLocalized(230, 10, 100, 20, 3010005, 0x7FFF, false, false);
```

That row is a search button at the left, a text field beginning at `x=44`, and a localized label at `x=230`. The search text entry is 180 pixels wide, so it ends at `224`. The label begins at `230`, leaving a 6 pixel gap.

Buttons are not rectangles in code, but they still occupy the art dimensions of the normal and pressed art IDs. If a label starts too close to a button, the text may visually collide even though the numeric coordinates do not overlap.

### 4. Decode Grids And Repeated Rows

Most useful gumps are grids. Look for arithmetic:

```csharp
AddLabel(15 + (304 * column), 65 + (25 * row), 0x481, handler.Description);
AddLabel(184 + (304 * column), 65 + (25 * row), 0x481, cost.ToString());
AddButton(270 + (304 * column), 62 + (25 * row), 4023, 4024, 1000 + i, GumpButtonType.Reply, 0);
```

Translate that into layout constants:

| Value | Meaning |
| ---: | --- |
| `15` | left column label X |
| `304` | column width |
| `65` | first row Y |
| `25` | row height |
| `184` | gold column offset |
| `270` | button column offset |
| `1000 + i` | reply button range tied to source definition index |

Then calculate the maximum visible area. With rows `0` through `11`, the last label starts at `65 + 25 * 11 = 340`. If the label is about 20 pixels tall, it ends near `360`. A panel ending at `383` has room. A thirteenth row would start at `365` and risk crowding the footer or panel edge.

When reading loops, always ask:

- Does the row counter stop before it leaves the panel?
- Does the column counter stop before it draws past the frame?
- Does the reply button encode the display index, the source index, or both?
- Does `OnResponse` decode the same ID scheme that the constructor encoded?
- Does the gump handle more rows than fit by paging, scrolling, or refusing overflow?

### 5. Separate Page Buttons From Reply Buttons

This button changes client page and does not call `OnResponse`:

```csharp
AddButton(485, 260, 4005, 4007, 0, GumpButtonType.Page, page + 1);
```

This button submits to server code:

```csharp
AddButton(270, 402, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
```

Page buttons are good for pure navigation inside an already-built gump. Reply buttons are required when the server must change state, rebuild data, validate permissions, run commands, or send a different gump.

### 6. Check Text Surfaces

Use `AddLabel` for short fixed labels. Use `AddLabelCropped` when the text may be too long but must stay in a row. Use `AddHtml` for multi-line content, coloring, centering, scroll regions, or HTML-backed clipping.

Common checks:

- A 20 pixel row is usually enough for one label line.
- A 25 pixel row gives more breathing room for buttons and label baselines.
- `AddHtml(..., scrollbar: true)` needs enough width for the scrollbar.
- `AddHtml(..., background: true)` adds a client background; do not also assume the surrounding panel is the only visual background.
- User-controlled or data-controlled names should usually be cropped or placed in a scrollable HTML region.
- Long button labels should not be drawn with plain `AddLabel` if the label can run into the next column.

### 7. Read The Negative Space

A good gump is mostly spacing discipline. After you sketch content, inspect the empty space:

- Outer margin: usually 8 to 15 pixels.
- Gutter between panels: often 4 to 10 pixels.
- Header height: commonly 20 to 26 pixels.
- Row rhythm: usually 20, 22, 25, or 28 pixels.
- Button-to-label gap: usually at least 5 pixels, more when the button art is wide.
- Footer controls: keep them below dynamic content, not inside the last dynamic row.

If the code uses bare numbers everywhere, infer the layout grid and consider introducing local constants when you touch the gump for functional work. Do not do a style-only rewrite just to make old imported gumps pretty.

## Designing A New Gump

Start with behavior, then layout. The most reliable order is:

1. Define who opens it and why.
2. Define what state it displays.
3. Define which actions mutate state.
4. Define button ID ranges.
5. Define the fixed frame and panels.
6. Define row and column constants.
7. Build one static version.
8. Add dynamic rows and paging.
9. Implement `OnResponse` with guards before mutations.
10. Verify compile and the client-visible layout.

For new Confictura-native scripts, follow the repository naming and namespace rules from `AGENTS.md`. Gumps generally live with the system they serve. Do not move existing serialized systems or imported systems just to put their gumps elsewhere.

### Layout Constants

Small constants make layout easier to read and safer to change:

```csharp
private const int Width = 420;
private const int Height = 280;
private const int Margin = 10;
private const int HeaderY = 10;
private const int HeaderHeight = 20;
private const int RowY = 44;
private const int RowHeight = 22;
private const int ButtonOffset = 100;
```

They are most useful when the same math appears in the constructor and `OnResponse`. Avoid over-abstracting one-off art placement in old static imported gumps unless you are already fixing a real bug.

### Button ID Ranges

Reserve stable ranges. Do not let unrelated actions and dynamic row buttons collide.

```csharp
private enum Button
{
    Cancel = 0,
    Refresh = 1,
    PreviousPage = 2,
    NextPage = 3
}

private const int RowButtonOffset = 100;
```

If each row has multiple actions, encode them predictably. `BaseGridGump` uses this style:

```csharp
public int GetButtonID(int typeCount, int type, int index)
{
    return 1 + (index * typeCount) + type;
}
```

The important part is not the exact formula. The important part is that the decode path can reject invalid values before indexing a list.

### Dynamic Rows

When drawing dynamic lists, decide what happens when the list is too large:

- Use pages when each row has buttons and server actions.
- Use a scrollable `AddHtml` region for read-only text.
- Use columns only when the maximum item count is known and bounded.
- Use `AddLabelCropped` for names that can exceed the column width.
- Avoid drawing rows beyond the panel and relying on the client to hide them.

The `EnhancementGump` pattern is a useful two-column layout example because its coordinates make the grid obvious. Its old risk was also instructive: if the number of eligible entries exceeds the visible two-column capacity, rows can overflow or overlap unless the code bounds, pages, or otherwise handles the extra entries.

## Interaction Model

### Sending And Replacing Gumps

Use `from.CloseGump(typeof(MyGump))` before sending a replacement when only one instance should exist. `AddGump` does this in its constructor so repeated searches do not stack the same search window.

```csharp
from.CloseGump(typeof(MyGump));
from.SendGump(new MyGump(from, page));
```

Use `HasGump` or `FindGump` when the behavior depends on an existing open instance, such as a command that toggles or delegates to an open toolbar gump.

Do not call `CloseAllGumps` unless the feature intentionally clears every open UI for that player.

### Reopening After A Reply

A reply button closes the tracked gump before `OnResponse` runs. If the action should keep the user in the workflow, send a new gump:

```csharp
case (int)Button.Refresh:
{
    from.SendGump(new MyGump(from, m_Page));
    break;
}
```

If the action opens a target, prompt, hue picker, or another gump, make that transition explicit. Do not leave the player with a silent close unless cancel/no-op is intended.

### Text Entries

Read text entries defensively:

```csharp
TextRelay relay = info.GetTextEntry(0);
string value = relay == null ? String.Empty : relay.Text.Trim();

if (value.Length == 0 || value.Length > 40)
{
    from.SendMessage("Enter a value between 1 and 40 characters.");
    from.SendGump(new MyGump(from));
    return;
}
```

The packet handler rejects very long raw text responses, but gameplay code still needs domain limits, parsing checks, uniqueness checks, profanity or formatting rules where appropriate, and ownership/access checks before applying the value.

### Checks And Radios

Use stable switch IDs. For radio controls, call `AddGroup(group)` before related radios, then read the chosen switch with `info.IsSwitched(id)`.

Do not infer that a switch exists just because it was drawn last time. The client reply tells you what was submitted, and the server still needs to validate the resulting state change.

## Defensive Response Handling

Most serious gump bugs are response-time bugs, not drawing bugs. The audit found repeated patterns around stale state, missing bounds checks, missing deleted checks, and missing access or ownership checks.

Start `OnResponse` with state guards:

```csharp
public override void OnResponse(NetState state, RelayInfo info)
{
    if (state == null || info == null)
        return;

    Mobile from = state.Mobile;

    if (from == null || from.Deleted)
        return;

    // Decode and validate buttons only after the caller is valid.
}
```

Then revalidate every object captured in the constructor:

```csharp
if (m_Item == null || m_Item.Deleted)
    return;

if (from.Backpack == null || !m_Item.IsChildOf(from.Backpack))
{
    from.SendMessage("That item must be in your backpack.");
    return;
}
```

For world objects and services, validate the relevant state:

| State | Typical guard |
| --- | --- |
| Caller | `from != null`, `!from.Deleted`, expected `AccessLevel` |
| Item target | not null, not deleted, still owned or still in backpack |
| Mobile target | not null, not deleted, still alive if required |
| Map/location | maps match, map is not null/internal unless intended |
| Range | caller is still close enough to the object or NPC |
| Region/city/guild | membership and permissions still match |
| Lists | decoded index is within current list bounds |
| Text | entry exists, trimmed length is valid, parse succeeds |
| Money/resources | recheck cost and availability immediately before spending |
| Action eligibility | recompute the predicate used to draw the button |

Never trust a button ID just because the constructor only drew valid buttons. A stale or crafted reply can send any integer.

```csharp
int index = info.ButtonID - RowButtonOffset;

if (index < 0 || index >= m_Options.Count)
    return;

OptionEntry option = m_Options[index];

if (!CanUseOption(from, option))
    return;

ApplyOption(from, option);
```

If the button ID encodes a source definition index, validate against the source definition list. If it encodes a display index, validate against the current display list. Be clear which one you are using.

## Patterns And Anti-Patterns

### Good Patterns

- Close the same gump type before opening a replacement when stacking would confuse the workflow.
- Keep button ID ranges documented with constants or a small enum.
- Keep layout math local and obvious.
- Use pages for actionable lists that can grow.
- Use `AddLabelCropped` or scrollable `AddHtml` for untrusted long text.
- Recompute permissions and costs before mutation.
- Send a replacement gump after validation errors when the workflow should continue.
- Keep old imported layout style when touching an imported system narrowly.

### Anti-Patterns

- Indexing arrays or lists directly from `info.ButtonID` without range checks.
- Assuming `sender.Mobile` is non-null in `OnResponse`.
- Assuming an item, city, guild, or process captured in the constructor still exists.
- Drawing dynamic rows past the visible panel.
- Using reply buttons for pure page navigation.
- Using page buttons for actions that require server validation.
- Letting text overlap because a name is normally short.
- Assuming `Scripts.csproj` controls runtime visibility. Live script compile recursively gathers `.cs` files under `Data/Scripts`.
- Treating a gump close as a confirmed cancel when the underlying object needed cleanup. Use `OnServerClose` only when you intentionally need server-close behavior.

## Useful Existing Examples

| Source | What to study |
| --- | --- |
| `Data/Scripts/System/Gumps/AddGump.cs` | Search field layout, fixed results page, close-before-open pattern, text entry handling, target handoff. |
| `Data/Scripts/System/Gumps/BaseGridGump.cs` | Reusable row/column helpers, button ID encoding and decoding, dynamic background sizing. |
| `Data/Scripts/Trades/Core/Gumps/CraftGump.cs` | Large multi-page production gump with categories, item lists, navigation, and crafting actions. |
| `Data/Scripts/Trades/Guild/Gumps/EnhancementGump.cs` | Compact two-column grid and button IDs tied to enhancement definition indexes. |
| `Data/Scripts/Custom/StaffTools/StaticGumpTool/StaticGump(Alternative).cs` | Dense staff-tool panel with many hand-placed controls; useful for learning how old hardcoded layouts read. |
| `docs/wiki/Gump_Framework.md` | Engine lifecycle, packet flow, helper-to-entry mapping, and known framework issues. |
| `docs/wiki/Static_Gump_Tool.md` | Documentation style for staff-facing gump tools and source-traced known issues. |

## Verification Checklist

For documentation-only gump work:

- Confirm source claims against code or audit outputs.
- Run `git diff --check`.
- Confirm new wiki links resolve.
- Do not claim build or runtime script compile success unless they were actually run.

For source gump changes:

- Run a targeted static search for the touched gump's send sites and response path.
- Check every `OnResponse` path for null, deleted, access, range, map, ownership, bounds, and text guards.
- Check dynamic row capacity, page navigation, and button ID decode logic.
- Run the narrowest source build available for the touched area.
- For scripts under `Data/Scripts`, run a time-boxed server startup script compile smoke with `-nocache` when practical.
- If Visual Studio project files were changed, label that as IDE/project hygiene, not live runtime proof.

## Source Trace

This guide was prepared from current source and audit documentation as a practical coding companion, not as a new runtime system page.

### Source Files And Docs Reviewed

- `docs/wiki/Gump_Framework.md`
- `docs/wiki/Static_Gump_Tool.md`
- `Data/System/Source/Gumps/Gump.cs`
- `Data/System/Source/Network/PacketHandlers.cs`
- `Data/System/Source/Mobile.cs`
- `Data/Scripts/System/Gumps/AddGump.cs`
- `Data/Scripts/System/Gumps/BaseGridGump.cs`
- `Data/Scripts/Trades/Core/Gumps/CraftGump.cs`
- `Data/Scripts/Trades/Guild/Gumps/EnhancementGump.cs`
- `Data/Scripts/Custom/StaffTools/StaticGumpTool/StaticGump(Alternative).cs`

### Audit Outputs Used

- `docs/codebase-audit/outputs/phase-05-gump-response-risk-register.csv`
- `docs/codebase-audit/outputs/runtime-hook-map.csv`
- `docs/codebase-audit/outputs/post-batch-o-gump-guard-closeout.md`

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy are changed by this guide.
