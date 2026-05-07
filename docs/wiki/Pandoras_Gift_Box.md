# Pandora's Gift Box

## Overview

Pandora's Gift Box is a constructable staff-managed `Item` that inherits from a custom `RandomGiftBox` container. When active, each placed box listens for `EventSink.CharacterCreated` and `EventSink.Login` and attempts to give eligible players duplicated copies of every `Item` currently inside the box.

The script does not randomly select one reward from the box contents. The random behavior is limited to the gift box container art and hue when a `RandomGiftBox` is created or duplicated.

## Source

| Script | Purpose |
| --- | --- |
| `Data/Scripts/Custom/PandorasGiftBox/PandorasGiftBox.cs` | Defines `RandomGiftBox` and `PandorasGiftBox`. |

## Runtime Types

| Type | Namespace | Base Type | Role |
| --- | --- | --- | --- |
| `RandomGiftBox` | `Server.Items` | `BaseContainer` | Gift-box container with randomized item art, hue, and container gump. |
| `PandorasGiftBox` | `Server.Customs` | `RandomGiftBox` | Non-movable staff box that duplicates its contents to players on login or character creation. |
| `EnableContextMenuEntry` | nested in `PandorasGiftBox` | `ContextMenuEntry` | Staff-only context entry that toggles `Active`. |

## RandomGiftBox Container Appearance

`RandomGiftBox` randomizes only its own container appearance.

| Roll | ItemID Range | Approximate Chance | Hue Source | Notes |
| --- | --- | ---: | --- | --- |
| `Utility.RandomDouble() > 0.75d` | `0x232A` through `0x232B` | 25% | `GiftBoxHues.RandomNeonBoxHue` | Uses the neon gift box hue path. |
| otherwise | `0x46A2` through `0x46A7` | 75% | `GiftBoxHues.RandomGiftBoxHue` | Uses the standard gift box hue path. |

The same appearance randomization is repeated in `RandomGiftBox.OnAfterDuped(Item newItem)`, so duplicated random gift boxes reroll their art and hue.

## Staff Configuration

Pandora's Gift Box exposes these `CommandProperty` values at `AccessLevel.GameMaster`.

| Property | Type | Default | Effect |
| --- | --- | --- | --- |
| `OnePerAccount` | `bool` | `true` | When true, each account username can receive gifts once from this box. When false, each `Mobile` can receive gifts once from this box. |
| `Active` | `bool` | `false` | Enables or disables gift attempts from login and character-created events. |
| `Message` | `string` | `null` | Optional prefix added to the delivery message sent to the player. |

There is no custom command registered by this script. Staff create and configure the item through normal RunUO item/admin tooling, such as adding the constructable `PandorasGiftBox` item and editing its command properties.

## Event Flow

Each `PandorasGiftBox` instance calls `Initialize()` from both constructors. Initialization sets `Movable = false`, sets `Name = "Pandora's Gift Box"`, and registers instance handlers for:

| EventSink Hook | Handler | Behavior |
| --- | --- | --- |
| `EventSink.CharacterCreated` | `EventSink_CharacterCreated(CharacterCreatedEventArgs e)` | Calls `TryGift(e.Mobile)`. |
| `EventSink.Login` | `EventSink_Login(LoginEventArgs e)` | Calls `TryGift(e.Mobile)`. |

Because the event handlers are instance methods, each existing active box can independently attempt gift delivery for the same login or character creation event.

## Eligibility Checks

`TryGift(Mobile m)` returns without giving items when any of the following checks fail:

| Check | Result When Failing |
| --- | --- |
| `m == null` | No gift. |
| `m.Backpack == null` | No gift. |
| `m.BankBox == null` | No gift. |
| `Active == false` | No gift. |
| `Items.Count <= 0` | No gift. |
| `OnePerAccount == true` and `m_AccountsGivenTo` contains `m.Account.Username` | No gift. |
| `OnePerAccount == false` and `m_MobilesGivenTo` contains `m` | No gift. |

The account-mode check uses `m.Account.Username`; the method does not guard `m.Account` before reading it.

## Gift Delivery

When a `Mobile` passes eligibility, the box duplicates every contained `Item` into one destination.

| Destination Decision | Delivery Target | Message Suffix |
| --- | --- | --- |
| `m.Backpack.CheckHold(m, this, false)` returns true | `m.Backpack` | `A gift has been placed in your backpack.` |
| backpack hold check returns false | `m.BankBox` | `A gift has been placed in your bank box.` |

If `Message` is not null, it is prepended to the suffix with a space. Otherwise only the suffix is sent. Messages use hue `0x4`.

After delivery, the box records the recipient:

| Mode | Stored Value |
| --- | --- |
| `OnePerAccount == true` | `m.Account.Username` in `m_AccountsGivenTo` |
| `OnePerAccount == false` | `Mobile` reference in `m_MobilesGivenTo` |

## Item Duplication Rules

`DupeContainer(Container from, Container to)` walks every `Item` in the source container and calls `Dupe(to, item)`.

| Duplication Step | Behavior |
| --- | --- |
| Constructor lookup | Requires a public zero-parameter constructor for the source item type. |
| Normal item copy | Creates the item, copies writable public properties by reflection, then calls `copy.OnAfterDuped(newItem)`. |
| `HalloweenBag` special case | Creates a new `HalloweenBag` directly and skips the generic property-copy path. |
| Parent handling | Sets `newItem.Parent = null` before dropping it into the destination container. |
| Nested containers | If the source item is a `Container`, recursively duplicates its contents into the copied container. |
| Failure path | Writes a console warning that all items in a Pandora's Gift Box must have a zero-parameter constructor. |

Property copy failures are swallowed silently on a per-property basis.

## Player and Staff Interaction

| Interaction | Access | Behavior |
| --- | --- | --- |
| Double-click | `AccessLevel.GameMaster` or higher | Opens the base container gump. |
| Double-click | Below `GameMaster` | No base double-click behavior is invoked and no denial message is sent. |
| Object property list | Any viewer | Displays the base name plus `One per Account` or `One per Character`, and `Active` or `Disabled`. |
| Context menu | `AccessLevel.GameMaster` or higher | Adds a toggle entry using cliloc `1011034` when activation is available or `1011035` when deactivation is available. |

The script does not define a custom `Gump` or `NetState` response flow. Staff opening uses the inherited container gump from `BaseContainer`.

## Serialization

Both `RandomGiftBox` and `PandorasGiftBox` use versioned RunUO serialization.

### RandomGiftBox

| Version | Serialized Data |
| --- | --- |
| `0` | No custom fields beyond the version integer. |

### PandorasGiftBox

`PandorasGiftBox.Serialize(GenericWriter writer)` writes version `0`, then writes:

| Order | Field | Writer Method | Reader Method |
| ---: | --- | --- | --- |
| 1 | `m_OnePerAccount` | `writer.Write(bool)` | `reader.ReadBool()` |
| 2 | `m_Active` | `writer.Write(bool)` | `reader.ReadBool()` |
| 3 | `m_Message` | `writer.Write(string)` | `reader.ReadString()` |
| 4 | `m_MobilesGivenTo` | `writer.WriteMobileList(...)` | `reader.ReadMobileList()` |
| 5 | account count | `writer.Write(int)` | `reader.ReadInt()` |
| 6 | each account username | `writer.Write(string)` | `reader.ReadString()` |

`Deserialize(GenericReader reader)` only handles version `0`. The mobile history list is cleared before and after `ReadMobileList()` before loaded mobiles are copied into `m_MobilesGivenTo`.

## Administrator Notes

* The box must contain at least one item and `Active` must be true before it gives anything.
* Each box tracks its own recipient history; multiple boxes can each grant gifts independently.
* Account mode stores account usernames as strings rather than `Account` references.
* Per-character mode stores serialized `Mobile` references.
* Staff should place only items with zero-parameter constructors inside the box unless the item is known to duplicate safely through this reflection path.
