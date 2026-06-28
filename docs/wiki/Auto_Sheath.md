# Auto Sheath

## Overview
Auto Sheath is a player-facing weapon convenience system. When enabled for a `PlayerMobile`, changing out of war mode moves the currently equipped weapon into the Mobile's backpack. Changing back into war mode tries to re-equip the last weapon that the system stored for that player.

The compiled implementation is `Server.Items.AutoSheatheWeapon`, a static command/helper class in `Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs`. It is not an `Item`, `Mobile`, `Gump`, or XMLSpawner attachment.

## Entry Points

| Entry point | Access | Behavior |
| :--- | :--- | :--- |
| `[sheathe` | `AccessLevel.Player` | Toggles `PlayerMobile.CharacterSheath` between `0` and `1` when `AutoSheatheWeapon.Config.AllowPlayerToggle` is `true`. |
| Help `Settings` Gump | Player help gump page `12` | Button ID `52` toggles `PlayerMobile.CharacterSheath` and redraws the settings page. |
| `PlayerMobile.OnWarmodeChanged()` | Core player hook | Calls `AutoSheatheWeapon.From(this)` every time a player changes war mode. |
| `EventSink.Logout` | Server event hook | Removes the player's cached last-weapon reference from the static `PlayerWeapons` dictionary. |

## Configuration

| Field | Default | Compiled effect |
| :--- | :--- | :--- |
| `AutoSheatheWeapon.Config.SendOverheadMessage` | `false` | When set to `true`, the system sends local emote overhead text when sheathing or unsheathing. |
| `AutoSheatheWeapon.Config.AllowPlayerToggle` | `true` | Registers the `[sheathe` command and makes `CharacterSheath == 1` required before the warmode hook does anything. If this is `false`, `From(Mobile m)` ignores `CharacterSheath` and runs for every `PlayerMobile` caller. |

There is no external config file or admin command for these values; they are static C# fields.

## Player State

`PlayerMobile` stores the feature toggle in the public integer field `CharacterSheath`, exposed as the GameMaster command property `Character_Sheath`.

| Value | Meaning in current code |
| :--- | :--- |
| `0` | Auto sheathing is off when `AllowPlayerToggle` is `true`. |
| `1` | Auto sheathing is on when `AllowPlayerToggle` is `true`. |
| Any other value | Treated as off by the warmode hook because the code checks `CharacterSheath != 1`. |

## Command Flow

`Initialize()` registers `[sheathe` only when `Config.AllowPlayerToggle` is `true`.

```text
[sheathe
```

The command has no parameters. Its compiled metadata is:

| Attribute | Value |
| :--- | :--- |
| `[Usage]` | `sheathe` |
| `[Description]` | `Enables or disables the weapon auto-sheathe feature.` |

When `CharacterSheath == 1`, the command sets it to `0` and sends:

```text
You have disabled the weapon auto-sheathe feature.
```

Otherwise, it sets `CharacterSheath` to `1` and sends:

```text
You have enabled the weapon auto-sheathe feature.
```

## Help Gump Flow

The Help `Settings` page renders an `Auto Sheath` row. The status button uses graphic `4018` when `CharacterSheath == 1` and `3609` otherwise. Pressing the row's toggle button submits reply button ID `52`, flips `CharacterSheath` between `0` and `1`, and reopens `HelpGump(from, 12)`.

The adjacent help/info button opens page `100`, titled `Auto Sheath`, which describes automatic sheathing out of battle and automatic weapon draw when war mode is restored.

## War Mode Mechanics

`PlayerMobile.OnWarmodeChanged()` calls `AutoSheatheWeapon.From(this)` after scheduling ammunition recovery when leaving war mode.

`From(Mobile m)` exits immediately when:

| Check | Result |
| :--- | :--- |
| `m.Backpack == null` | No sheathe or unsheathe action. |
| `Config.AllowPlayerToggle == true` and `((PlayerMobile)m).CharacterSheath != 1` | No sheathe or unsheathe action. |

After those gates, the helper:

1. Uses `m.Serial.Value` as the dictionary key.
2. Checks `Layer.OneHanded` for the active item.
3. If the one-handed layer is empty or holds a non-movable item, checks `Layer.TwoHanded`.
4. Reads any cached last weapon from `PlayerWeapons[key]`.

### Entering War Mode

When `m.Warmode` is `true`, the system tries to re-equip the cached last weapon only if all of these conditions pass:

| Condition |
| :--- |
| The current one-handed/two-handed item is `null` or is allowed to stay equipped. |
| `lastWeapon != null` |
| `lastWeapon.IsChildOf(m.Backpack)` |
| `lastWeapon.Movable` |
| `lastWeapon.Visible` |
| `!lastWeapon.Deleted` |

If the checks pass, the system calls `m.EquipItem(lastWeapon)`.

### Leaving War Mode

When `m.Warmode` is `false`, the system moves the equipped weapon into the backpack only if:

| Condition |
| :--- |
| A one-handed or two-handed item was found. |
| The item is not in the keep-equipped allowlist. |

The move is performed with:

```csharp
m.Backpack.DropItem(weapon);
PlayerWeapons[key] = weapon;
```

## Keep-Equipped Allowlist

`AllowedToKeep(Item item)` returns `true` for items assignable to the listed types, plus any `BaseEquipableLight`.

| Type |
| :--- |
| `GoldRing` |
| `LevelGoldRing` |
| `GiftGoldRing` |
| `BaseShield` |
| `PugilistGlove` |
| `PugilistGloves` |
| `ThrowingGloves` |
| `LevelPugilistGloves` |
| `LevelThrowingGloves` |
| `GiftPugilistGloves` |
| `GiftThrowingGloves` |
| `Spellbook` |
| `BaseEquipableLight` |

These items are not dropped during the peace-mode sheathing pass. They also do not block an unsheathe attempt when the cached weapon is still in the backpack.

## Persistence

The auto-sheath toggle persists through `PlayerMobile` serialization as part of the version `29` player-settings block. Current serialization writes `CharacterSheath` after `CharacterDiscovered` and before `CharacterGuilds`; deserialization reads it in the same position.

The static `PlayerWeapons` dictionary is not serialized. It is only an in-memory cache and is cleared per player on `EventSink.Logout`.

## Known Issues

* `[sheathe` casts `CommandEventArgs.Mobile` directly to `PlayerMobile` without a null or type guard.
* The Help `Settings` page also casts `Mobile from` directly to `PlayerMobile` while rendering and handling the Auto Sheath button.
* `Config.AllowPlayerToggle == false` disables command registration, but `From(Mobile m)` then ignores `CharacterSheath` entirely and auto-sheathes every `PlayerMobile` that reaches the warmode hook.
* `DisabledPlayers` is declared but never read or written after initialization.
* `PlayerWeapons` stores a static in-memory `Item` reference keyed only by serial. Logout removes the entry, but other item lifecycle events do not. The warmode equip path guards deleted, invisible, immovable, and non-backpack items, so stale references usually fail safely, but the cache can still retain irrelevant references until overwritten or logout.
* The system tracks only one cached item per player. It intentionally leaves allowlisted off-hand items such as shields equipped, but it does not preserve a full equipment set or support multiple sheathed weapons.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0078.
- Backlog rows: RB-06661.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=1; Event=1; Initialize=1.
- Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs:L37 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs:L39 Event EventSink access=GlobalOrInternal
- Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs:L42 Command CommandSystem.Register access=Unknown

### Serialization Evidence

- No serialized classes matched the reviewed source set in serialization-register.csv.

### Project And Runtime Coverage

- Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs=Keep
- Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
