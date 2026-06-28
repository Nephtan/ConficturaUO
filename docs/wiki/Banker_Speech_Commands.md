# NPC Banker Speech Commands

## Scope

This page documents the RunUO banking speech handled by `Server.Mobiles.Banker`.
It covers the `withdraw`, `balance`, `bank`, and `check` speech keywords, related `BankBox` and `BankCheck` behavior, context menu access, static helper methods, and persistence.

The compiled system is a `Mobile` vendor implementation. It is not a player command system, does not register `[Command]` handlers, and does not use XMLSpawner attachments.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs` | Banker `Mobile`, keyword speech handling, context menu entries, static banking helpers, and banker serialization. |
| `Data/Scripts/Mobiles/Civilized/Vendors/GypsyBanker.cs` | `Banker` subclass that inherits the same speech banking behavior. |
| `Data/Scripts/Items/Misc/BankCheck.cs` | Blessed bank-check `Item` with `Worth`, display text, double-click redemption, and serialization. |
| `Data/System/Source/Items/Containers.cs` | Core `BankBox` container, open/access rules, owner persistence, and display behavior. |
| `Data/System/Source/Mobile.cs` | `Mobile.BankBox` auto-creation and `FindBankNoCreate()` lookup used by banker speech and helpers. |
| `Data/Scripts/System/Misc/ContextMenu.cs` | `OpenBankEntry` context menu path for opening a bank box. |

## Speech Entry Point

`Banker.HandlesOnSpeech(Mobile from)` returns `true` when the speaker is within 12 tiles of the banker. `OnSpeech(SpeechEventArgs e)` then requires the speech event to be unhandled and the speaking `Mobile` still within 12 tiles.

The banker does not parse raw words directly. It loops over `SpeechEventArgs.Keywords`, which are speech keyword IDs decoded by the network speech packet handler for encoded client speech.

| Keyword ID | Commented keyword | Required spoken shape | Result |
| --- | --- | --- | --- |
| `0x0000` | `*withdraw*` | `withdraw <amount>` | Attempts to move gold from bank to backpack. |
| `0x0001` | `*balance*` | `balance` | Reports current bank gold total. |
| `0x0002` | `*bank*` | `bank` | Opens the speaker's bank box. |
| `0x0003` | `*check*` | `check <amount>` | Attempts to create a `BankCheck` in the bank box and consume bank gold. |

When a handled keyword is found, `e.Handled` is set to `true`, but the loop continues through any remaining keyword IDs already present in the same speech event.

## Speech Mechanics

### `withdraw <amount>`

The speech handler splits the original speech string on spaces and parses `split[1]` as an `int`.
If parsing fails or no second token exists, no feedback is sent.

| Gate | Behavior |
| --- | --- |
| `amount > 5000` when `Core.ML == false` | Banker says cliloc `500381`: cannot withdraw so much at one time. |
| `amount > 60000` when `Core.ML == true` | Banker says cliloc `500381`: cannot withdraw so much at one time. |
| Backpack is `null`, deleted, at or above max weight, or at or above max items | Banker says cliloc `1048147`: backpack cannot hold anything else. |
| `amount <= 0` | No withdrawal and no feedback. |
| Bank box missing or `ConsumeTotal(typeof(Gold), amount)` fails | Banker says cliloc `500384`: not enough gold. |
| Success | The bank consumes `amount` gold, the backpack receives `new Gold(amount)`, the wealth bar refreshes, and the banker says cliloc `1010005`. |

The speech withdrawal consumes only `Gold` items from the `BankBox`; it does not break or reduce `BankCheck` items.

### `balance`

The speech handler calls `e.Mobile.FindBankNoCreate()`.

| Bank state | Response |
| --- | --- |
| Bank box exists | Banker says cliloc `1042759` with `box.TotalGold.ToString()`. |
| Bank box does not exist | Banker says cliloc `1042759` with `0`. |

`box.TotalGold` is the core container gold total. It counts `Gold` items, including nested gold, but does not count `BankCheck.Worth`.

### `bank`

The speech handler accesses `e.Mobile.BankBox`, which auto-creates a `BankBox` on `Layer.Bank` if one does not already exist. It then calls `BankBox.Open()`.

`BankBox.Open()` sets the internal open flag, sends the owner a private overhead message with item and stone totals, sends an `EquipUpdate`, and displays the bank box gump to the owner.

### `check <amount>`

The speech handler splits the original speech string on spaces and parses `split[1]` as an `int`.
If parsing fails or no second token exists, no feedback is sent.

| Gate | Behavior |
| --- | --- |
| `amount < 5000` | Banker says cliloc `1010006`: checks cannot be created for such a small amount. |
| `amount > 1000000` | Banker says cliloc `1010007`: policy prevents checks worth that much. |
| Bank box cannot accept the new `BankCheck` | Banker says cliloc `500386`, deletes the new check, and does not consume gold. |
| Bank box accepts the check but cannot consume `amount` `Gold` | Banker says cliloc `500384` and deletes the new check. |
| Success | A `BankCheck(amount)` remains in the bank box, `amount` gold is consumed from the bank, the wealth bar refreshes, and the banker says cliloc `1042673` with the amount appended. |

Like `withdraw`, the speech check path consumes only `Gold` items. It does not convert existing `BankCheck` value.

## Static Banking Helpers

`Banker.cs` also exposes static helper methods used by other systems. These helpers are separate from the player speech paths above.

| Helper | Behavior |
| --- | --- |
| `GetBalance(Mobile from)` | Calls the overload below and returns the calculated balance. |
| `GetBalance(Mobile from, out Item[] gold, out Item[] checks)` | Finds the bank without creating it, recursively finds `Gold` and `BankCheck` items, adds gold `Amount`, and adds each check's `Worth`. |
| `Withdraw(Mobile from, int amount)` | Uses helper balance, consumes gold piles first, then deletes or reduces `BankCheck.Worth` until `amount` is covered. |
| `Deposit(Mobile from, int amount)` | Finds an existing bank box, deposits `Gold` for amounts below 5000, one `BankCheck` for 5000 through 1000000, or multiple 1000000 checks for larger amounts. Returns `false` and deletes items created during the call if a later drop fails. |
| `DepositUpTo(Mobile from, int amount)` | Deposits the same gold/check chunks as `Deposit`, stopping on first failed drop and returning the amount actually deposited. |
| `Deposit(Container cont, int amount)` | Drops the same gold/check chunks directly into the supplied container without bank ownership checks or hold validation. |

## BankCheck Redemption

`BankCheck` is a blessed `Item` with GM-editable `Worth`. Its constructor randomizes the item art between `0x02DD` and `0x201A`, sets weight to `1.0`, hue to `0xB51`, and stores the supplied worth.

A player can redeem a check only by double-clicking it while it is inside their bank box.

| Step | Behavior |
| --- | --- |
| Check is not inside the caller's bank box | The caller receives cliloc `1047026`: the check must be in the bank box to use it. |
| Check is inside the caller's bank box | The original check is deleted before gold placement begins. |
| `m_Worth` exceeds 60000 | The code creates 60000-gold piles until the remainder is 60000 or less. |
| Bank accepts a gold pile | `deposited` increases by that pile amount. |
| Bank rejects a gold pile | The failed pile is deleted, a replacement `BankCheck(toAdd)` is added to the caller's backpack, and redemption stops. |
| Final remainder is positive | The code attempts to drop one final `Gold(toAdd)` pile or reissues a `BankCheck(toAdd)` to the backpack on failure. |
| Completion | The caller receives cliloc `1042672` with the amount of gold deposited. |

## Context Menu Access

`Banker.AddCustomContextEntries()` adds `OpenBankEntry` only when the caller is alive, has `Kills < 1`, and is not criminal.

`OpenBankEntry.OnClick()` checks that the caller is alive. It rejects criminals with cliloc `500378`; otherwise it opens `Owner.From.BankBox`.

`Banker.GetContextMenuEntries()` also adds `SpeechGumpEntry`, which is available to `PlayerMobile` callers and opens a `SpeechGump` titled `Copper and Silver Coins` using `SpeechFunctions.SpeechText(..., "Banker")`.

## Vendor Catalog

`Banker.InitSBInfo()` adds `SBBanker`, so bankers are still normal vendor mobiles in addition to their speech banking behavior.
The `SBBanker` catalog sells employment contracts, vendor rental contracts, commodity deeds, and safe/vault items, and buys treasure pile deeds plus safe/vault items.

## Serialization

| Type | Version | Serialized fields after version |
| --- | --- | --- |
| `Banker` | `0` | None. |
| `GypsyBanker` | `0` | None beyond inherited `Banker` data. |
| `BankCheck` | `0` | `m_Worth` as `int`. During deserialize it also forces blessed loot type, valid item art, and hue `0xB51`. |
| `BankBox` | `0` | Owner `Mobile`, then open flag `bool`. If the owner is missing on load, the bank box deletes itself. |

## Admin Commands

No in-game admin or player commands are registered by this system.

`BankCheck.Worth` is exposed as a `[CommandProperty(AccessLevel.GameMaster)]`, so staff can inspect or edit an individual check's worth through the normal property system.

## Known Issues

| Issue | Impact |
| --- | --- |
| Speech banking bypasses the context menu's alive, criminal, and murderer gates. | The context menu path restricts access, but the `withdraw`, `balance`, `bank`, and `check` speech paths only require range and an unhandled speech event. |
| Speech `balance`, `withdraw`, and `check` ignore `BankCheck.Worth`. | The static helper balance and withdrawal methods include checks, but the player speech paths report and consume only `Gold`. |
| `withdraw` does not use `Container.CheckHold()` or `TryDropItem()` for the backpack deposit. | It checks only current backpack totals before calling `DropItem(new Gold(amount))`, so the added gold pile can exceed the backpack's true item or weight capacity. |
| `BankCheck.OnDoubleClick()` deletes the original check before all replacement value is safely placed. | If the bank cannot accept generated gold, the remainder is reissued through `AddToBackpack()`; that method can fall back to moving the new check into the world when the backpack cannot hold it. |
| Multiple banker keyword IDs in one speech event can execute in one pass. | `e.Handled` is set per matched keyword, but the loop continues through any later keyword IDs already decoded for the same speech. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0121.
- Backlog rows: RB-06662.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs (CurrentFile)
- Data/Scripts/Mobiles/Civilized/Vendors/GypsyBanker.cs (CurrentFile)
- Data/Scripts/Items/Misc/BankCheck.cs (CurrentFile)
- Data/System/Source/Items/Containers.cs (CurrentFile)
- Data/System/Source/Mobile.cs (CurrentFile)
- Data/Scripts/System/Misc/ContextMenu.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Event=10; Gump=4; Movement=3; Speech=9; Timer=13; WorldLoad=1.
- Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs:L233 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs:L368 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs:L408 Gump SendGump access=Internal
- Data/System/Source/Mobile.cs:L1458 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/System/Source/Mobile.cs:L1657 Event EventSink access=GlobalOrInternal
- Data/System/Source/Mobile.cs:L1994 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/System/Source/Mobile.cs:L2014 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/System/Source/Mobile.cs:L2034 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/System/Source/Mobile.cs:L2054 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/System/Source/Mobile.cs:L2069 Event EventSink access=GlobalOrInternal
- Data/System/Source/Mobile.cs:L2079 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/System/Source/Mobile.cs:L2096 Timer CustomTimerSubclass access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 5.
- Data/Scripts/Items/Misc/BankCheck.cs:Server.Items.BankCheck version=0 serialize=L33 deserialize=L42 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs:Server.Mobiles.Banker version=0 serialize=L422 deserialize=L429 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Civilized/Vendors/GypsyBanker.cs:Server.Mobiles.GypsyBanker version=0 serialize=L89 deserialize=L96 alignment=AlignedByCountAndKnownTypes
- Data/System/Source/Items/Containers.cs:Server.Items.BankBox version=0 serialize=L76 deserialize=L86 alignment=AlignedByCountAndKnownTypes
- Data/System/Source/Mobile.cs:Server.Mobile version=35 serialize=L6183 deserialize=L5700 alignment=CountMismatch:Writes=98;Reads=104

### Project And Runtime Coverage

- Data/Scripts/Items/Misc/BankCheck.cs=Keep
- Data/Scripts/Items/Misc/BankCheck.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/GypsyBanker.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/GypsyBanker.cs=Keep
- Data/Scripts/System/Misc/ContextMenu.cs=Keep
- Data/Scripts/System/Misc/ContextMenu.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
