# Access Level Stone

## Overview
The Access Level Stone is a blessed staff utility item that directly toggles a mobile's `AccessLevel` between `Player` and a stored staff access level. It has no command handler of its own, no XMLSpawner dependency, and no custom math.

The compiled script is `Data/Scripts/Custom/Access Level Stone [2.0]/AccessLevelStone.cs`.

## Creation
Staff can create the stone with `[add AccessLevelStone`, subject to the normal `[add` command and `[Constructable]` access checks.

The constructor creates item ID `0x1870` and sets:

| Property | Value |
| :--- | :--- |
| `Weight` | `1.0` |
| `Hue` | `0x38` |
| `Name` | `Staff To Player To Staff To ...` |
| `LootType` | `Blessed` |
| `NextAccessLevel` / stored level | `AccessLevel.Player` |

`Owner` and `NextAccessLevel` are exposed as read-only `CommandProperty(AccessLevel.GameMaster)` properties.

## Use Flow
The stone must be in the user's backpack. If it is not a child of `m.Backpack`, `OnDoubleClick` sends localized message `1042001` and stops.

When a permitted user double-clicks the stone:

1. If the stone has no owner and the user is above `AccessLevel.Player`, the user becomes `Owner`.
2. If the stone has an owner and the user has a higher current `AccessLevel` than that owner, the user becomes the new `Owner`.
3. If `NextAccessLevel` is `Player`, the stone stores the owner's current access level and sets the owner to `Player`.
4. Otherwise, it restores the owner to `NextAccessLevel` and resets the stored level to `Player`.
5. The double-clicking mobile receives `AccessLevel changed!`.

## Ownership And Deletion Rules
The ownership check is:

```csharp
Owner == null || Owner == m || m.AccessLevel > Owner.AccessLevel
```

Inside that branch, the code only assigns `m_Owner` on an unowned stone when the user is above `AccessLevel.Player`, or on an owned stone when the user has higher access than the current owner.

If a lower-access non-owner fails the outer ownership check, the stone sends `That is not yours!` and deletes itself. There is also an inner branch intended to send `You are unable to use that!` and delete the stone for non-owner users below Counselor.

## Serialization
`AccessLevelStone` overrides RunUO item serialization.

Serialized order:

1. `base.Serialize(writer)`
2. Version integer `1`
3. `m_StoredAccessLevel` written as an `int`
4. A `bool` indicating whether `m_Owner` is present
5. `m_Owner` as a `Mobile`, only when the bool is true

Deserialization reads version `1` in the same order and restores the stored access level plus optional owner mobile.

## Known Rework Risks
* An unowned stone used by a `Player` enters the permitted outer branch because `Owner == null`, but does not assign `m_Owner`; the later toggle then dereferences `m_Owner.AccessLevel`.
* A higher-access staff member can seize the stone while its owner is toggled down to `Player`, because the comparison uses the owner's current `AccessLevel`. That can leave the original staffer stuck at `Player`.

## Audit Notes
* Wiki claimed staff can create the stone with `[add AccessLevelStone` -> traced `AccessLevelStone` constructor, `ConstructableAttribute`, and `Add.IsConstructable` -> code compiles the item, uses default `[Constructable]` access, and `[add` construction is gated by normal add-command checks.
* Wiki claimed the item spawns blessed, weight 1, hue `0x38`, and named `Staff To Player To Staff To ...` -> traced constructor -> code also sets item ID `0x1870` and initial stored level `Player`.
* Wiki claimed backpack-only use -> traced `OnDoubleClick` -> code rejects non-backpack use with localized message `1042001`.
* Wiki claimed first use records the owner and toggles to player -> traced owner assignment and toggle branch -> code records only staff users on first use, stores `m_Owner.AccessLevel`, then sets `m_Owner.AccessLevel = Player`.
* Wiki claimed toggling back restores the stored level -> traced restore branch -> code assigns `m_Owner.AccessLevel = NextAccessLevel` and resets stored level to `Player`.
* Wiki claimed higher-access staff can claim it and unauthorized users delete it -> traced ownership branches -> code compares against the owner's current access level, can reassign owner, and deletes the item on failed ownership checks.
* Wiki omitted serialization -> traced `Serialize` and `Deserialize` -> code writes version `1`, stored access level, owner-present flag, and optional owner mobile.
* Code trace revealed rework risks -> unowned player use can dereference a null owner, and higher-access seizure while the original owner is in player mode can strand the original owner at `Player`.
