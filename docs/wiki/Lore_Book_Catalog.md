# Lore Book Catalog

## Purpose

Staff can add or edit generated lore books by editing `Data/Scripts/Custom/LoreBooks/LoreBooks.xml`. The lore book catalog is data-driven: each `<Book>` entry defines the book title, author, cover, text, library behavior, and loot behavior.

The server loads the catalog during script initialization. After startup, it checks the XML file every 15 seconds and reloads it when the file timestamp changes. Staff can also force a reload with `[ReloadLoreBooks]`.

## Quick Start

1. Open `Data/Scripts/Custom/LoreBooks/LoreBooks.xml`.
2. Copy an existing `<Book>` block or use the template below.
3. Give the new entry a unique stable `id`.
4. Omit `legacyId` for new entries.
5. Set `sort` to control where the book appears in the player library.
6. Put the book body inside the `<Text><![CDATA[...]]></Text>` block.
7. Save the XML file.
8. Wait up to 15 seconds for the automatic reload, or run `[ReloadLoreBooks]`.
9. Spawn or find a new `LoreBook`, read it, and confirm it unlocks correctly in the player library.

## XML Template

```xml
<Book
  id="my-new-lore-book"
  sort="90"
  title="My New Lore Book"
  author="Aldus the Chronicler"
  cover="1"
  library="true"
  loot="true"
  lootWeight="1">
  <Text><![CDATA[
Write the book text here.

Use <BR><BR> when you want visible paragraph breaks inside the in-game book gump.
  ]]></Text>
</Book>
```

Insert the new block inside the root `<LoreBooks>` element. Keep the XML well formed: every `<Book>` must have a closing `</Book>`, and every `<Text>` must have a closing `</Text>`.

## Attribute Reference

| Attribute | Required | Default | Use |
| --- | --- | --- | --- |
| `id` | Yes | None | Stable catalog key. Must be unique. Use lowercase words separated by hyphens, such as `fall-of-the-red-moon`. Do not rename after players can unlock it. |
| `title` | Yes | None | The in-game book title and item name. Escape XML attribute characters, such as `&amp;` for `&` and `&quot;` for quotes. |
| `author` | No | Empty | The author displayed by the dynamic book. |
| `legacyId` | No | `-1` | Compatibility key for the original hard-coded books. New staff-authored books should omit it. |
| `sort` | No | `legacyId`, or `100000` when no `legacyId` exists | Player library display order. Use larger numbers for entries that should appear later. |
| `cover` | No | `0` | Cover index passed to `DynamicBook.SetBookCover`. Existing entries provide examples. |
| `itemID` | No | Random book graphic | Specific item graphic. Hex values like `0x543C` are accepted. |
| `hue` | No | Random color | Specific hue. Omit this for normal random book coloring. |
| `light` | No | No light | `LightType` enum name, such as `Circle225`. Invalid names are ignored and logged. |
| `library` | No | `true` | Whether reading this book can unlock it in the player library. |
| `libraryTitle` | No | `title` | Optional shorter title for the player library list. |
| `loot` | No | `true` | Whether random `LoreBook` construction can choose this entry. |
| `lootWeight` | No | `1` | Relative weight among loot-enabled lore books. Higher numbers make the entry more likely. `0` prevents loot selection even when `loot="true"`. |

## Library And Loot Behavior

The player library uses stable catalog IDs. Once a player unlocks a book, the unlock is tied to the `id`, not to the display title. For that reason, treat `id` as permanent after the book is live.

Use `sort` to place a new book near related entries. The current seeded books use the old library order. A new entry with `sort="90"` will appear after the original set; lower values place it earlier.

Set `library="false"` for lore books that should exist in the world but should not be listed or unlockable in the player library. Set `loot="false"` for lore books that should be opened only by special scripts, staff placement, quest rewards, or custom spawns.

`lootWeight` only compares against other `LoreBook` catalog entries after the loot system has already selected `typeof(LoreBook)`. It does not change the overall chance that dungeon loot picks a lore book instead of another book item.

## Reloading Changes

The catalog reloads automatically after the file timestamp changes. The reload timer checks every 15 seconds, so a saved XML edit is not instant.

Use `[ReloadLoreBooks]` when you want immediate feedback. If the reload succeeds, staff receive `Lore book catalog reloaded.` If it fails, staff receive `Lore book catalog reload failed; keeping the previous catalog.`

Existing world books keep their serialized data, but catalog-backed lore books refresh text by `BookItem` catalog ID or title when the script asks the catalog to apply text. New random `LoreBook` items use the latest loaded catalog.

## Validation Checklist

Before saving:

- `id` is unique and stable.
- `title` is present.
- `<Text>` exists and is not empty.
- New entries do not use `legacyId`.
- XML attribute values are escaped correctly.
- Long book text is inside CDATA.
- Paragraph breaks use `<BR><BR>` when the in-game gump should show spacing.
- `sort` does not accidentally place the book in the middle of an unrelated group.
- `loot` and `library` match the intended distribution.
- Special `itemID`, `hue`, and `light` values were tested in game.

## Troubleshooting

| Symptom | Likely Cause | Fix |
| --- | --- | --- |
| Reload says it failed | Malformed XML or unreadable file | Check the server console for the `LoreBooks: Failed to load...` message and fix the XML. |
| Book does not appear in library | `library="false"`, missing unlock, or duplicate `id` ignored | Confirm the entry is loaded, read the book on the character, and verify the `id` is unique. |
| Book never drops randomly | `loot="false"`, `lootWeight="0"`, or the outer loot table did not choose `LoreBook` | Set `loot="true"` and a positive `lootWeight`; remember this only affects lore-book selection after `LoreBook` is chosen. |
| Entry is ignored | Missing `id`, `title`, or non-empty `<Text>` | Add the missing required value. |
| Duplicate entry is ignored | Another book already uses the same `id` | Rename the new entry before it goes live. |
| Integer setting acts like default | Invalid `sort`, `cover`, `itemID`, `hue`, `legacyId`, or `lootWeight` | Use decimal numbers or `0x` hex for `itemID`. |
| Light does not work | Invalid `LightType` name | Use a valid enum name, such as `Circle225`, or omit `light`. |

