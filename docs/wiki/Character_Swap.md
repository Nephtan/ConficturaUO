# Character Swap

## Overview
`Character Swap` is an administrator-only account management gump opened by the `[CharControl` command. The script lets staff resolve two `Account` objects by name, inspect a selected character through `PropertiesGump`, delete a character after `WarningGump` confirmation, move a `Mobile` to the first open slot on the other account, or exchange two selected character slots between the two accounts.

This is not a player-facing instant character-switch feature. Both the move path and the completed swap path dispose any connected `NetState`, which forcibly disconnects the affected character session.

## Entry Point
- Command: `[CharControl`
- Access level: `AccessLevel.Administrator`
- Initial UI: `CharacterControlGump`

## Gump Flow
1. The administrator runs `[CharControl`.
2. `CharControl_OnCommand` sends a fresh `CharacterControlGump`.
3. The first page accepts two account names through text entries `2` and `3`.
4. Button `1` resolves those names with `Accounts.GetAccount(...) as Account`.
5. The gump then renders each non-null character on both accounts with action buttons.

If either account cannot be resolved, the gump keeps any previously selected account object and prints an inline error message rather than throwing.

## Character Rows
For each displayed `Mobile`, the gump shows the character name and serial plus three actions.

| Label | Button ID suffix | Behavior |
| --- | --- | --- |
| Select / Props | `5` | Outside swap mode, opens `PropertiesGump` for the selected `Mobile`. During swap mode, this button becomes the first or second swap selection. |
| Swap | `6` | Starts a two-step swap selection workflow if both accounts exist and neither side is completely empty. |
| Del | `7` | Opens a `WarningGump`; confirmation disconnects the character if needed and then calls `Delete()`. |
| Move | `8` | Removes the `Mobile` from the current account slot and inserts it into the first free slot on the other account. |

The left column uses button IDs below `100`; the right column adds `100`. The script decodes the source side and slot index from the button ID in `OnResponse(NetState state, RelayInfo info)`.

## Account Resolution Rules
The script does not require both names to be re-entered on every reply.

| Condition | Result |
| --- | --- |
| Both text entries empty | Shows `Please enter in an Account name`. |
| One typed account missing, the other blank | Shows a single-account not-found message. |
| Both typed accounts missing | Shows a two-account not-found message. |
| A prior `a1` or `a2` was already loaded and the current lookup fails | Reuses the prior loaded account object for that side. |

Before processing any action buttons, the gump also nulls out `a1` or `a2` if `Accounts.GetAccounts().Contains(a)` no longer contains that account.

## Swap Workflow
Swapping is a two-click operation spread across both account columns.

1. Press a row's `Swap` button, which creates `SwapInfo(a1, a2)` and stores the selected slot index on the initiating account side.
2. The gump reopens in swap mode.
3. Click the other character's select button.
4. `SwapInfo.SwapEm()` exchanges the two `Account` slot references.
5. If either selected character is connected, its `NetState` is disposed.

`SwapInfo.SwapEm()` returns `false` only if either account reference is null or either selected slot resolves to null. Otherwise it performs a direct slot exchange and returns `true`.

## Move Workflow
The move path is one-way.

1. The script calls `HasSpace(secondAcct)`.
2. If the result is negative, the destination account is treated as full.
3. Otherwise it clears the source slot, writes the `Mobile` into the returned destination slot, makes the `Mobile` say `I've been moved to another Account!`, and disposes its `NetState` if present.
4. The action is logged through `CommandLogging.WriteLine(...)`.

## Delete Workflow
Deletion is confirmation-gated but otherwise permanent.

1. The script opens a `WarningGump` with a callback to `CharacterDelete_Callback`.
2. On confirmation, it disposes the character's `NetState` if present.
3. It then calls `mob.Delete()`.
4. The callback logs successful deletions and reopens the control gump with a status message.

The warning text explicitly notes that deleting the character removes items carried by that character but does not remove any standing house.

## Technical Notes
- The system has no custom `Serialize` or `Deserialize` logic. It is a command/gump script only.
- All account membership changes happen by assigning the `Account` indexer directly.
- The account indexer itself updates `Mobile.Account` when a slot is assigned or cleared, so move and swap operations rely on core account setter behavior instead of manually rewriting the account reference.

## Current Code Note
This script needs C# rework. `CharacterControlGump` renders six visible character rows per side, but `HasSpace(Account)` only scans slots `0` through `4`. Meanwhile the core `Account` implementation allocates `new Mobile[7]` and supports account limits of `5`, `6`, or `7` depending on expansion state. In practice, move and swap availability checks can ignore a valid sixth or seventh slot.

## Audience
Staff

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0083.
- Backlog rows: RB-06675.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/Character Swap/CharacterSwap.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=1; Gump=8; Initialize=1.
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L12 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L14 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L27 Gump SendGump access=Internal
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L194 Gump OnResponse access=Internal
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L216 Gump SendGump access=Internal
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L251 Gump SendGump access=Internal
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L329 Gump SendGump access=Internal
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L363 Gump SendGump access=Internal
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L435 Gump SendGump access=Internal
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs:L500 Gump SendGump access=Internal

### Serialization Evidence

- No serialized classes matched the reviewed source set in serialization-register.csv.

### Project And Runtime Coverage

- Data/Scripts/Custom/Character Swap/CharacterSwap.cs=Keep
- Data/Scripts/Custom/Character Swap/CharacterSwap.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
