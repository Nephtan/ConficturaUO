# NPC Control Commands

## Overview
NPC Control Commands are Counselor-level staff utilities for cloning staff bodies, temporarily disguising staff as mobiles, refreshing targets, and forcing speech. The compiled command surface is spread across `Data/Scripts/Custom/NPC Control/CloneCommands.cs` and the `StaffCommands` files in the same folder. These scripts also depend on the Confictura clone helpers in `Data/Scripts/Custom/CloneOfflinePlayerCharacters/CloneThings.cs`.

The system is command-driven. It has no XMLSpawner definitions and no standalone NPC engine. The only persistent items in this system are the blessed `CloneItem` and `ControlItem` restore tokens created while a staff member is cloned or controlling a mobile.

## Compiled Scripts
`Data/Scripts/Scripts.csproj` includes these NPC Control sources:

| Script | Purpose |
| --- | --- |
| `CloneCommands.cs` | Registers `[Clone` and `[Control`, implements clone/control lifecycle, and defines `CloneItem` and `ControlItem`. |
| `StaffCommands/CloneMe.cs` | Registers `[CloneMe`. |
| `StaffCommands/GmMe.cs` | Registers `[GmMe`. |
| `StaffCommands/HearAll.cs` | Registers `[HearAll` and hooks global speech. |
| `StaffCommands/Refresh.cs` | Registers `[Refresh`. |
| `StaffCommands/SayThis.cs` | Registers `[SayThis`. |

## PlayerMobile Death Hook
The old `installation.txt` describes manually adding an `OnBeforeDeath` hook. In this shard, `PlayerMobile.OnBeforeDeath()` already calls `Server.Commands.CloneCommands.UncontrolDeath((Mobile)this)`. If that method handles a controlled-body death, `OnBeforeDeath()` returns `false` and prevents the staff player's normal death path.

## `[Clone`
`[Clone` assigns a target cursor. The target must be a `Mobile`, must not be the caster, and, for a real command target, must have a lower access level than the staff member.

When cloning succeeds, the command:

* Creates a backup duplicate of the staff mobile.
* Places a blessed `Clone Item` in the staff member's backpack after the backpack is rebuilt.
* Copies the target's appearance, stats, skills, worn items, backpack contents, and mount onto the staff member through the clone helper routines.
* Deletes the staff member's current worn items and backpack contents before applying cloned items.
* Sets `BodyMod` to the target body for non-player targets and to `0` for player targets.
* Updates the staff disguise state on `PlayerMobile`.

Double-clicking, dropping, or moving the `Clone Item` so it is no longer rooted in the owner deletes the item. Deleting it calls `EndClone`, which restores the saved staff duplicate and clears the staff disguise.

## `[Control [NoStats] [NoSkills] [NoItems]]`
`[Control` assigns a target cursor and only works when the caster is a `PlayerMobile`. The target must be a `Mobile`; targeting a `PlayerMobile` is rejected with "You can't control players." Targeting an existing `ControlItem` deletes it and releases control.

On first control, the command:

* Saves the staff member's original map, location, and direction.
* Creates an internalized duplicate of the staff member for later restoration.
* Creates a blessed `Control Item` linking the staff member, the saved staff duplicate, and the controlled NPC.
* Records the NPC's original `Hidden`, `CantWalk`, `Frozen`, and `Paralyzed` flags.
* Copies the NPC onto the staff member and internalizes the real NPC.
* Sets staff hunger and thirst to `20` for Counselor+ controllers.

Issuing `[Control` again while already controlling restores the old NPC state, updates the same `Control Item`, copies the new NPC onto the staff member, and internalizes the new NPC.

Deleting or double-clicking the `Control Item` calls `EndControl`: the current staff state is copied back onto the NPC, the NPC's recorded hidden/walk/frozen/paralyzed flags are restored, the staff duplicate is copied back onto the player, and the player is moved back to the saved original map/location/direction.

### Current Rework Issue
The `NoStats`, `NoSkills`, and `NoItems` arguments are parsed and stored on `ControlItem`, but the active copy path ignores them. `StartControl` and `ChangeControl` call `CloneTarget.SimulateTarget`, and that path always copies mobile properties, skills, worn items, backpack contents, and mounts. As compiled, these options are status text and serialized flags, not functioning copy filters.

`UncontrolDeath` also restores staff hits and stamina to max but assigns `Mana = StamMax` instead of `ManaMax` before killing the controlled NPC. That death cleanup path should be reviewed.

## Other Staff Commands
### `[CloneMe`
Creates a `CharacterClone` from the staff member, copies properties, worn items, backpack contents, and mount, makes the clone visible, and hides the original staff mobile.

### `[GmMe`
Blesses the staff member, moves items from common equipment layers into the backpack, equips `DragonRobe`, `ClothHood`, and `BootsofHermes`, sets every skill base to `120`, and sets `RawStr`, `RawDex`, and `RawInt` to `100`. It does not change the mobile body.

### `[HearAll`
Toggles the staff member in a static listener list. While enabled, every speech event sends the listener a message formatted as `(RawName): speech`.

### `[Refresh`
Targets a mobile within range `12`. If the staff member can see the target, the command sets `Hits`, `Mana`, and `Stam` to their maximum values.

### `[SayThis <text>`
Requires a non-empty argument string. Targeting a mobile calls `Say(text)`. Targeting an item sends a regular public overhead message from the item.

## Serialization
`CloneItem` writes version `0`, then its owner mobile and saved player mobile.

`ControlItem` writes version `4`, then:

1. Original map, location, and direction.
2. Stored `Stats`, `Skills`, and `Items` flags.
3. Owner, saved player, and NPC mobiles.
4. Original hidden, cant-walk, frozen, and paralyzed flags.

`ControlItem.Deserialize` reads these fields behind version gates for versions `1` through `4`.
