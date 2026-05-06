# NPC Control Commands

## Overview
NPC Control Commands are Counselor-level staff utilities for assuming mobile appearances, temporarily controlling NPC bodies, creating visible staff clones, refreshing mobiles, forcing speech, and listening to global speech.

The system is command-driven. It has no XMLSpawner definitions and no standalone NPC engine. The persistent runtime artifacts are the blessed `CloneItem` and `ControlItem` restore tokens created while a staff member is cloned or controlling an NPC.

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
`PlayerMobile.OnBeforeDeath()` calls `Server.Commands.CloneCommands.UncontrolDeath((Mobile)this)`. When a controlled staff body dies, `UncontrolDeath` releases the control token, restores the staff mobile's hits, stamina, and mana from their matching max values, kills the cached controlled NPC if it still exists, and returns `false` so the staff player's normal death path is skipped.

## Shared Guardrails
`[Clone` and `[Control` both reject deleted targets, targets on `Map.Internal`, clone-system mobiles such as `CharacterClone` and `MountClone`, and ridden mount targets. `[Clone` cannot start while the staff member has a `ControlItem`; `[Control` cannot start while the staff member has a `CloneItem`.

NPC Control inventory helpers skip restore tokens and clone artifacts instead of copying or moving them as ordinary body inventory. The skipped item classes are `CloneItem`, `ControlItem`, `BackpackClone`, and `EtherealMountClone`; `MountClone` mobiles are rejected as control/clone targets.

## `[Clone`
`[Clone` assigns a target cursor. The target must be a different `Mobile` with a lower access level than the staff member.

Before changing the staff body, `[Clone` prebuilds detached copies of the target's worn items and backpack tree through `CloneThings.CloneItem`. If any non-system target item cannot be cloned safely, the command deletes any partial copies, reports the failing item, and aborts before the staff member's inventory is changed.

When cloning succeeds, the command:

1. Creates a duplicate of the staff mobile as the restoration backup.
2. Moves the staff member's real worn items, backpack contents, and mount to that backup.
3. Copies the target's mobile state, appearance, stats, skills, hunger/thirst, followers, hair, and body data onto the staff member through `CloneThings.CloneMobileProperties`.
4. Sets `BodyMod` to the target body for non-player targets and to `0` for player targets.
5. Adds the prebuilt target item copies to the staff body and backpack.
6. Clones supported target mounts through the offline clone mount helper.
7. Drops a blessed `Clone Item` into the rebuilt staff backpack.
8. Internalizes the staff backup and updates the `PlayerMobile` staff disguise.

Double-clicking the `Clone Item`, dropping it, or moving it away from its owner deletes the token. Token deletion is guarded against recursion and calls `EndClone` once. `EndClone` restores the saved staff body, keeps the staff member at the current clone location and direction, deletes the internalized backup, and clears the staff disguise.

## `[Control`
`[Control` accepts no options. Supplying any argument, including the old `NoStats`, `NoSkills`, or `NoItems` flags, sends the usage message and does not assign a target. `ControlItem` still serializes the old flag fields for save compatibility, but new runtime control is always full-copy.

`[Control` assigns a target cursor and only works for `PlayerMobile` staff. Targeting the staff member's own `ControlItem` releases control. Targeting a `PlayerMobile` is rejected, so the command controls NPC mobiles only.

On first control, the command:

1. Saves the staff member's original map, location, and direction.
2. Creates an internalized duplicate of the staff member for restoration.
3. Records the NPC's original `Hidden`, `CantWalk`, `Frozen`, and `Paralyzed` flags.
4. Moves the staff member's real worn items, backpack contents, and mount to the saved staff duplicate.
5. Copies the NPC's mobile state onto the staff member.
6. Moves the real NPC worn items, backpack contents, and mount onto the staff body.
7. Drops a blessed `Control Item` into the rebuilt staff backpack.
8. Internalizes the real NPC and saved staff duplicate.
9. Sets staff hunger and thirst to `20` for Counselor+ controllers and updates the staff disguise.

Issuing `[Control` again while already controlling restores the old NPC from the current staff body, restores the old NPC's recorded flags, then moves the new NPC's real inventory and mount onto the staff body and updates the same `ControlItem`.

Double-clicking, dropping, or moving the `Control Item` away from its owner deletes it. Token deletion is guarded against recursion and calls `EndControl` once. `EndControl` restores the controlled NPC from the staff body, restores the staff member from the internalized backup, moves the staff member back to the saved original map/location/direction, deletes the backup, and clears the staff disguise.

If the controlled staff body dies, `UncontrolDeath` caches the NPC reference, deletes the control token to run the same release path, restores `Hits`, `Stam`, and `Mana` from `HitsMax`, `StamMax`, and `ManaMax`, then kills the cached NPC only if it still exists and is not deleted.

## Other Staff Commands
### `[CloneMe`
Creates a `CharacterClone` from the staff member, copies mobile properties, safely clones worn items and backpack contents, clones supported mounts, makes the clone visible, and hides the original staff mobile. `CloneMe` cannot run while the staff member is already cloned or controlling an NPC. It skips NPC Control restore tokens and clone artifacts, reports how many items were skipped, and deletes a partial clone if copying throws.

### `[GmMe`
Blesses the staff member, ensures a backpack exists, moves items from common equipment layers into the backpack, equips `DragonRobe`, `ClothHood`, and `BootsofHermes`, sets every skill base to `120`, and sets `RawStr`, `RawDex`, and `RawInt` to `100`. It does not change the mobile body.

### `[HearAll`
Toggles the staff member in a static listener list. While enabled, speech events send the listener `(RawName): speech`. The listener list prunes null, deleted, offline, and no-longer-authorized listeners before sending speech.

### `[Refresh`
Targets a mobile within range `12`. If the staff member can see the target, the command sets `Hits`, `Mana`, and `Stam` to their maximum values. Null and deleted casters or targets are ignored.

### `[SayThis <text>`
Requires a non-empty argument string. Targeting a live mobile calls `Say(text)`. Targeting a live item sends a regular public overhead message from the item. Null and deleted casters, mobiles, or items are ignored.

## Serialization
`CloneItem` writes version `0`, then its owner mobile and saved player mobile.

`ControlItem` writes version `4`, then:

1. Original map, location, and direction.
2. Stored `Stats`, `Skills`, and `Items` flags retained for old saves.
3. Owner, saved player, and NPC mobiles.
4. Original hidden, cant-walk, frozen, and paralyzed flags.

`ControlItem.Deserialize` reads these fields behind version gates for versions `1` through `4`. New control sessions write the old flag fields as `true`, because runtime control no longer supports partial stat, skill, or item copying.
