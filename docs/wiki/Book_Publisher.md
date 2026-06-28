# Book Publisher

## Overview

The Book Publisher is a `BaseVendor` NPC named `the publisher`. Players publish writable books by dropping a `BaseBook` onto the vendor. The system converts accepted books into `PublishedBook` items, saves the content to XML under `data/books`, pays a random royalty, and can later stock published titles for resale.

The package does not register custom commands, gumps, packet handlers, or event hooks. Staff add and place the `Publisher` vendor through normal RunUO construction/admin tooling.

## Source

| Script | Role |
| --- | --- |
| `Data/Scripts/Custom/Book Publisher [2.0]/Publisher.cs` | Vendor entry point, drag-drop publishing flow, royalty payout, republish handling, and vendor guild assignment. |
| `Data/Scripts/Custom/Book Publisher [2.0]/PublishedBook.cs` | Published book item, publishability checks, content hashing, contributor tracking, XML reload constructor, properties, and serialization. |
| `Data/Scripts/Custom/Book Publisher [2.0]/XmlBook.cs` | XML serialization model, `data/books` path helper, save/load helpers, and random published-book ID selection. |
| `Data/Scripts/Custom/Book Publisher [2.0]/SBPublisher.cs` | Publisher shop inventory that sells random published books and buys blank scrolls. |
| `Data/Scripts/Custom/Book Publisher [2.0]/BookBuyInfo.cs` | Vendor buy-info wrapper that prices and instantiates `PublishedBook` stock from XML IDs. |

## Player Publishing Flow

1. A player drops a `BaseBook` onto a `Publisher`.
2. `Publisher.OnDragDrop` detects the dropped book and calls `Publish`.
3. If the dropped book is already a `PublishedBook`, the vendor attempts the republish flow.
4. Otherwise the vendor copies the book into a new `PublishedBook`, adds the player's name as a contributor, and checks whether the book is publishable.
5. A publishable book is saved through `XmlBook.Save`.
6. The vendor pays one of three random royalty tiers, returns the new `PublishedBook` to the player's backpack, and deletes the original dropped book.

The vendor refuses criminal speakers in `OnSpeech`, then delegates to the base vendor speech path. The source still has a TODO for publishing-help speech; normal publishing entry is the drag-drop path.

## Publishability Rules

`PublishedBook.IsPublishable()` rejects a book when any of these conditions apply:

| Gate | Requirement |
| --- | --- |
| Writable state | The book must still be writable. |
| Title length | Trimmed title must be at least 5 characters. |
| Author length | Trimmed author must be at least 5 characters. |
| Default title | Trimmed lower-case title cannot be `title`. |
| Default author | Trimmed lower-case author cannot be `author`. |
| Body text | Combined trimmed page-line content must be at least 100 characters. |
| Name validation | Title and author must pass `NameVerification.Validate` with profanity protection lists. |

If the copied book fails these gates, the vendor deletes the copied `PublishedBook` and tells the contributor to write something of interest first.

## Royalty Payouts

Initial publication uses `Utility.Random(3)` to select one of three payout branches:

| Roll case | Gold reward | Vendor message tone |
| ---: | ---: | --- |
| `0` | `1500..3000` | Excellent material, likely to sell well. |
| `1` | `1000..1250` | Decent material, hoped to sell well. |
| `2` | `100..500` | Marginal material, uncertain sales. |

Each successful initial publication also plays sound `0x2E5` at the contributor when `Publisher.PlaySound` is true.

## Republishing

Dropping an existing `PublishedBook` runs `Republish` instead of creating a new published copy.

| Condition | Result |
| --- | --- |
| Not publishable | Vendor asks for more interesting material. |
| Content hash differs from the last publish hash | Contributor is added if new, XML is saved, `RePublish()` updates the hash/date, and the contributor receives `100` gold. |
| Content hash is unchanged | Vendor says the story looks the same as the last printed version. |
| XML save fails | Vendor says the machines are not working right. |

`PublishedBook.IsModified()` compares the current MD5 hash of title, author, and page text against the stored publish hash.

## XML Storage

`XmlBook.Save(PublishedBook publishedBook)` writes one XML file per published book under:

```text
data/books/<publish-guid>.xml
```

The XML captures:

| Data | Source |
| --- | --- |
| `id` | Existing `PublishID` parsed as a `Guid`, or a new `Guid` assigned on first save. |
| `title` | `PublishedBook.Title`. |
| `author` | `PublishedBook.Author`. |
| `created` | First-published date formatted with `ToString("u").Replace(' ', 'T')`. |
| `modified` | Last-published date formatted the same way. |
| contributors | Contributor names copied from `PublishedBook.Contributors`. |
| `hue`, `huedItemId`, `itemId` | Published-book display fields. |
| contents | Every non-empty page line, stored with 1-based page and line attributes. |

The package `Info.txt` mentions an older absolute RunUO data path, but the compiled `XmlBook.DataPath` uses the server-relative `data/books` path.

## Vendor Stock

`SBPublisher.InternalBuyInfo` asks `XmlBook.RandomBookIds(10)` for up to 10 published book IDs when the vendor buy list is built. Each ID becomes a `BookBuyInfo`.

| Vendor behavior | Compiled value |
| --- | --- |
| Buy item type | `PublishedBook`. |
| Display name | Loaded XML title. |
| Display art/hue | Loaded XML item ID and hue. |
| Price | Number of non-empty XML content lines. |
| Current amount | `5`. |
| Maximum amount | `20`. |
| Constructed item | `Activator.CreateInstance(Type, new object[] { id, pageCount })`. |

The sell list buys `BlankScroll` for `1` gold. The source still has commented-out stock entries for blank colored books.

## Serialization

| Type | Version | Serialized fields after version |
| --- | ---: | --- |
| `Publisher` | `0` | No custom fields. |
| `PublishedBook` | `0` | Contributor names joined by `;`, first-published date, last-published date, original content hash, and publish ID. |

Loaded `PublishedBook` instances reconstruct contributor arrays, publish dates, the original hash, and the publish ID. The XML-loading constructor can also rebuild a read-only book from a published XML ID and page count.

## Administration Notes

* Place `Publisher` vendors where player book publishing should be available.
* The server process must be able to create and write `data/books`.
* Published book XML files are the source used for vendor resale stock.
* Removing or corrupting XML files can affect vendor stock construction.
* Restarting or rebuilding vendor stock may be needed after direct XML maintenance.

## Known Issues

| Issue | Impact |
| --- | --- |
| `XmlBook.RandomBookIds(int max)` chooses random indexes with `new Random().Next(0, files.Length - 1)`. | When the XML file count exactly equals the requested stock count, the loop asks for more unique IDs than the selectable index range can provide. With more files than requested, the final XML file is never selected. |
| `XmlBook.Load(string id)` returns `null` on any exception, but `BookBuyInfo` and the XML constructor for `PublishedBook` dereference the returned book without null guards. | Missing, malformed, or unreadable XML can break vendor stock construction or purchased-book reconstruction. |
| `Publisher.OnSpeech` has a TODO for publishing information and only adds the criminal refusal before base vendor speech. | Players receive no custom in-game explanation of the publishing requirements from the vendor speech path. |
