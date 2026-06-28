# Skill Stone

## Overview
The Skill Stone is a `Server.Items.SkillStone` item that lets one assigned `PlayerMobile` spend stored stone points to raise existing skills through a `SetSkillsGump`. It has no custom command handler, no `EventSink` hook, no `Mobile`, and no XMLSpawner integration.

The compiled script is `Data/Scripts/Custom/Skill Stone/SkillStone.cs`, and `Data/Scripts/Scripts.csproj` explicitly includes it.

## Creation
Staff can create the stone through the normal RunUO `[add skillstone` flow because the default constructor is marked `[Constructable]`.

| Property | Default value |
| :--- | :--- |
| Class | `SkillStone : Item` |
| Namespace | `Server.Items` |
| ItemID | `0x1870` |
| `Name` | `Skill Stone` |
| `Hue` | `2704` |
| `LootType` | `Blessed` |
| `Movable` | `false` |
| `SkillPoints` | `1500` |
| `SkillMaxLevel` | `50` |
| `AssignedPlayer` | `null` |

The script also defines `SkillStone(int skillPoints, double skillMaxLevel)`, but that overload is not marked `[Constructable]`.

## Staff Properties
The following properties are exposed with `CommandProperty(AccessLevel.GameMaster)`:

| Property | Type | Behavior |
| :--- | :--- | :--- |
| `SkillPoints` | `int` | Remaining point pool on the stone. The Gump text describes one point as `0.1` skill. |
| `SkillMaxLevel` | `double` | Maximum target skill value the stone allows. |
| `AssignedPlayer` | `PlayerMobile` | The only player allowed to open and spend the stone after first assignment. |

## Double-Click Flow
`OnDoubleClick(Mobile from)` closes any existing `SetSkillsGump` for the caller, checks whether `from.Backpack` is non-null, then assigns or validates stone ownership.

| State | Result |
| :--- | :--- |
| `from.Backpack == null` | Sends `This must be in your backpack to function.` and stops. |
| `AssignedPlayer == null` | Casts `from` to `PlayerMobile`, stores it as `AssignedPlayer`, renames the item to `{player name}'s Skill Stone`, and sends `The skill stone has been assigned to you!`. |
| `AssignedPlayer != from` | Sends `This skill stone does not belong to you!`. |
| `AssignedPlayer == from` and `from.AccessLevel >= GameMaster` | Sends `Debug: Stone is working.` before opening the Gump. |
| `AssignedPlayer == from` | Opens `SetSkillsGump(from, this, SkillInfo.Table, null)`. |

The method does not call `IsChildOf(from.Backpack)`. The message says the stone must be in the user's backpack, but the compiled check only verifies that the mobile has a backpack object.

## SetSkillsGump Layout
`SetSkillsGump` stores the caller `Mobile` and `SkillStone` references, then renders the current `SkillInfo.Table`.

| Gump detail | Behavior |
| :--- | :--- |
| Page size | 15 skills per page. |
| Skill label | `{skill index}. {skill name}`. |
| Text entry ID | `skillCount + 1`. |
| Text entry value | `player.Skills[skillCount].Base.ToString()`. |
| Skill button ID | `skillCount + 1`. |
| Navigation | Uses `GumpButtonType.Page` for next and previous pages. |
| Bottom button | Labeled with localized `CANCEL`, but uses reply button ID `0x2`. |

`OnResponse(NetState state, RelayInfo info)` does not select a single skill from `info.ButtonID`. Any reply button other than ID `0` enters the default branch and processes every posted `TextRelay` on the current Gump page.

## Skill Raise Rules
For each posted text entry, the target skill is resolved as:

```csharp
targetSkill = m_Player.Skills[textEntry.EntryID - 1];
```

The submitted text is parsed with `Convert.ToDouble(textEntry.Text)`. If parsing throws, the script sets `newSkillValue` to `0`, sends an error message, and then treats the entry as no-op because new value `0` is ignored.

The effective raise amount is:

```csharp
skillDiff = newSkillValue - targetSkill.Base;

if (skillDiff < 0)
{
    skillDiff = 0;
}
```

Validation runs in this order:

| Order | Check | Failure result |
| :--- | :--- | :--- |
| 1 | `targetSkill.Base == newSkillValue` or `newSkillValue == 0` | No-op. |
| 2 | `newSkillValue > m_SkillStone.SkillMaxLevel` | Sends max-level rejection. |
| 3 | `targetSkill.Base >= newSkillValue` | Sends current-skill-higher rejection. |
| 4 | `targetSkill.Cap < newSkillValue` | Sends skill-cap rejection and also sends `newSkillValue.ToString()`. |
| 5 | `m_Player.Skills.Cap < (m_Player.SkillsTotal + (skillDiff * 10))` | Sends total skill cap rejection. |
| 6 | `(newSkillValue * 10) > m_SkillStone.SkillPoints` | Sends insufficient stone points rejection. |
| 7 | All checks pass | Sets `targetSkill.Base = newSkillValue`, subtracts `(int)(skillDiff * 10)` from `SkillPoints`, and sends a success message. |

Actual point deduction uses the incremental increase:

```csharp
m_SkillStone.SkillPoints -= (int)(skillDiff * 10);
```

The affordability check does not use the same formula. It checks `newSkillValue * 10`, so a stone can reject a valid small raise when the target value is high but the incremental cost is affordable.

After all posted text entries are processed, `OnResponse` deletes the stone if `SkillPoints <= 0`; otherwise, it opens a fresh `SetSkillsGump`.

## Skill Storage
The target assignment uses `Skill.Base`. In the core server, `Skill.Base` stores values through `BaseFixedPoint = (int)(value * 10.0)`, so skill bases are persisted in tenths.

The stone's total skill cap check compares against fixed-point totals:

| Value | Meaning |
| :--- | :--- |
| `m_Player.SkillsTotal` | Core `Mobile.SkillsTotal`, already stored in tenths. |
| `m_Player.Skills.Cap` | Core skill collection cap, also stored in tenths. |
| `skillDiff * 10` | Requested increase converted to tenths for the cap check. |

## Serialization
`SkillStone` implements the required `Serial` constructor and overrides RunUO item serialization.

Current serialized order:

1. `base.Serialize(writer)`
2. Version integer `0`
3. `m_SkillPoints` as `int`
4. `m_SkillMaxLevel` cast to `int`
5. `m_AssignedPlayer` as `PlayerMobile`

Deserialization reads version `0` in the same order, except `m_SkillMaxLevel` is read with `reader.ReadInt()` and assigned back to the `double` field.

### Serialization Quirk
`SkillMaxLevel` is a `double` property, but saves cast it to `int`. Any GameMaster-configured decimal max level is truncated on world save and reload.

## Known Issues
* The backpack requirement is not enforced. `OnDoubleClick` only checks whether `from.Backpack` exists and never checks whether the stone is inside it.
* `OnDoubleClick` casts `from` to `PlayerMobile` without a type check. A non-player `Mobile` activating the item can throw an invalid cast exception.
* The bottom button is labeled `CANCEL`, but it uses reply ID `0x2`; `OnResponse` only treats button ID `0` as cancel, so the visible cancel button processes Gump text entries and reopens the Gump.
* Skill button IDs are not used to select a single skill. The response handler processes every text entry posted from the current page.
* The point affordability check uses `newSkillValue * 10`, while the actual deduction uses `(newSkillValue - currentSkill) * 10`. This can block legitimate incremental raises after the stone has been partially spent.
* `SetSkillsGump.OnResponse` uses stored `m_Player` and `m_SkillStone` references and does not re-check `state.Mobile`, stone deletion, assignment ownership, or item containment before changing skills.
* `SkillMaxLevel` serialization truncates the `double` value to an `int`.
