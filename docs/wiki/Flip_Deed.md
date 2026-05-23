# Flip Deed

## Overview

Flip Deed is a player-facing house decoration mechanic inside the `InteriorDecorator` item named `Homeowner Tools`. It lets a house co-owner target certain directional deed-style `Item` instances that have been placed on the floor inside the caller's house, then replaces or mutates the targeted deed so it faces the opposite supported direction.

The compiled implementation is not a standalone player command. It is `DecorateCommand.Deed`, selected from the `InteriorDecorator.InternalGump` button labeled `Flip Deed`.

Code-Verified: 2026-05-09

## Core Scripts

| Script | Namespace | Role |
| --- | --- | --- |
| `Data/Scripts/Items/Houses/InteriorDecorator.cs` | `Server.Items` | `Homeowner Tools` item, decorator gump, targeting validation, `DecorateCommand.Deed`, and the hardcoded deed replacement table. |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | `Server.Engines.Help` | `InfoHelpGump` page `1000`, titled `Flip Deed`, opened from the decorator help button. |
| `Data/Scripts/Items/Construction/Addons/BaseAddonDeed.cs` | `Server.Items` | Base addon-deed placement behavior for most affected deed classes after a flipped deed is placed from the backpack. |
| `Data/Scripts/Items/Misc/FlipableAttribute.cs` | `Server.Items` | Separate GM-only `[Flip` command and generic `FlipableAttribute`; this is related terminology but is not the player `Flip Deed` replacement table. |
| `Data/Scripts/Trades/Stone/BaseStatueDeed.cs` | `Server.Items` | Special `BaseStatueDeed` flip behavior used by the decorator. |
| `Data/Scripts/Items/Relics/DDRelicBearRugs.cs` | `Server.Items` | Special relic bear-rug deed state copied during the decorator flip. |
| `Data/Scripts/Items/Houses/TentsEast.cs`, `TentsSouth.cs`, `CircusTentsEast.cs`, `CircusTentsSouth.cs` | `Server.Items` | Special tent and circus-tent deed hue state copied during the decorator flip. |

No XMLSpawner attachment, `EventSink` hook, packet handler, spawn timer, damage formula, or loot drop formula was found for this mechanic.

## Player Entry Flow

| Step | Compiled behavior |
| --- | --- |
| Open tools | Double-clicking `InteriorDecorator` calls `CheckUse(this, from)` and opens `InternalGump` only when the caller is a co-owner of the house at the caller's current location. |
| Select option | Button ID `11` maps to `DecorateCommand.Deed`. |
| Assign target | For any non-`None` and non-close decorator command, the gump assigns `sender.Mobile.Target = new InternalTarget(m_Decorator)`. |
| Retain tool gump | The command is stored on `m_Decorator.Command`, then the internal gump is reopened unless the close button was used. |
| Target item | `InternalTarget.OnTarget` accepts only `Item` targets and rechecks house co-owner access. |

The help button in the decorator gump uses button ID `99` and opens `InfoHelpGump(from, 1000, 999)`. Page `1000` is titled `Flip Deed` and describes rotating deed-style items with two possible directions.

## Target Validation

The selected target must pass the following gates before `Deed(Item item, Mobile from)` runs.

| Gate | Compiled condition or failure |
| --- | --- |
| Target type | Must be an `Item`. Non-item targets are ignored, then the decorator target is reassigned. |
| User location | `BaseHouse.FindHouseAt(from)` must return a house and `house.IsCoOwner(from)` must be true. Otherwise the caller receives localized message `502092`. |
| Item location | The target must have `Parent == null` and `house.IsInside(item)` unless it is a matching lawn or shanty item. Otherwise the caller receives localized message `1042270`. |
| Vendor rental contract | `VendorRentalContract` is rejected with localized message `1062491`. |
| Lawn and shanty items | Lawn and shanty items cannot use `Flip Deed`; they receive `You can only move these items up, down, north, south, east, or west.` |

`CheckLOS` is disabled on the decorator target.

## Replacement Mechanics

For most supported deed classes, `Deed(Item item, Mobile from)` creates a new opposite-direction deed in the caller's backpack and deletes the targeted floor item:

```text
from.AddToBackpack(new OppositeDirectionDeed());
item.Delete();
```

This means most flipped deeds are returned to the caller's backpack, not left on the floor. The player must place the new deed again through normal `BaseAddonDeed` placement.

If the target is an unsupported `BaseAddonDeed`, the caller receives:

```text
That deed cannot be flipped to face another direction!
```

If the target is not one of the supported deed classes and is not a `BaseAddonDeed`, the caller receives:

```text
This only flips deeds that are on the floor in your home!
```

## Supported Standard Deed Pairs

| Deed pair |
| --- |
| `AlchemistTableEastDeed` <-> `AlchemistTableSouthDeed` |
| `AnvilEastDeed` <-> `AnvilSouthDeed` |
| `ArcaneBookshelfEastDeed` <-> `ArcaneBookshelfSouthDeed` |
| `ArcanistStatueEastDeed` <-> `ArcanistStatueSouthDeed` |
| `BrocadeGozaMatEastDeed` <-> `BrocadeGozaMatSouthDeed` |
| `BrocadeSquareGozaMatEastDeed` <-> `BrocadeSquareGozaMatSouthDeed` |
| `BrownBearRugEastDeed` <-> `BrownBearRugSouthDeed` |
| `DarkFlowerTapestryEastDeed` <-> `DarkFlowerTapestrySouthDeed` |
| `DartBoardEastDeed` <-> `DartBoardSouthDeed` |
| `DeadBodyEWDeed` <-> `DeadBodyNSDeed` |
| `ElvenBedEastDeed` <-> `ElvenBedSouthDeed` |
| `ElvenDresserEastDeed` <-> `ElvenDresserSouthDeed` |
| `ElvenLoveseatEastDeed` <-> `ElvenLoveseatSouthDeed` |
| `ElvenSpinningwheelEastDeed` <-> `ElvenSpinningwheelSouthDeed` |
| `ElvenStoveEastDeed` <-> `ElvenStoveSouthDeed` |
| `ElvenWashBasinEastDeed` <-> `ElvenWashBasinSouthDeed` |
| `ESpikePostEastDeed` <-> `ESpikePostSouthDeed` |
| `EvilFireplaceEastFaceAddonDeed` <-> `EvilFireplaceSouthFaceAddonDeed` |
| `FancyElvenTableEastDeed` <-> `FancyElvenTableSouthDeed` |
| `FlourMillEastDeed` <-> `FlourMillSouthDeed` |
| `GozaMatEastDeed` <-> `GozaMatSouthDeed` |
| `GrayBrickFireplaceEastDeed` <-> `GrayBrickFireplaceSouthDeed` |
| `halloween_block_eastAddonDeed` <-> `halloween_block_southAddonDeed` |
| `halloween_coffin_eastAddonDeed` <-> `halloween_coffin_southAddonDeed` |
| `LargeBedEastDeed` <-> `LargeBedSouthDeed` |
| `LargeForgeEastDeed` <-> `LargeForgeSouthDeed` |
| `LargeStoneTableEastDeed` <-> `LargeStoneTableSouthDeed` |
| `LightFlowerTapestryEastDeed` <-> `LightFlowerTapestrySouthDeed` |
| `LoomEastDeed` <-> `LoomSouthDeed` |
| `MarlinSouthAddonDeed` <-> `MarlinEastAddonDeed` |
| `SawMillSouthAddonDeed` <-> `SawMillEastAddonDeed` |
| `MediumStoneTableEastDeed` <-> `MediumStoneTableSouthDeed` |
| `MediumStretchedHideEastDeed` <-> `MediumStretchedHideSouthDeed` |
| `MongbatDartBoardEastDeed` <-> `MongbatDartBoardSouthDeed` |
| `OrnateElvenTableEastDeed` <-> `OrnateElvenTableSouthDeed` |
| `PickpocketDipEastDeed` <-> `PickpocketDipSouthDeed` |
| `PolarBearRugEastDeed` <-> `PolarBearRugSouthDeed` |
| `SandstoneFireplaceEastDeed` <-> `SandstoneFireplaceSouthDeed` |
| `SmallBedEastDeed` <-> `SmallBedSouthDeed` |
| `SmallStretchedHideEastDeed` <-> `SmallStretchedHideSouthDeed` |
| `SpinningwheelEastDeed` <-> `SpinningwheelSouthDeed` |
| `SquareGozaMatEastDeed` <-> `SquareGozaMatSouthDeed` |
| `SquirrelStatueEastDeed` <-> `SquirrelStatueSouthDeed` |
| `StoneFireplaceEastDeed` <-> `StoneFireplaceSouthDeed` |
| `StoneOvenEastDeed` <-> `StoneOvenSouthDeed` |
| `TallElvenBedEastDeed` <-> `TallElvenBedSouthDeed` |
| `TrainingDummyEastDeed` <-> `TrainingDummySouthDeed` |
| `TrainingDaemonEastDeed` <-> `TrainingDaemonSouthDeed` |
| `TigerRugEastDeed` <-> `TigerRugSouthDeed` |
| `WhiteTigerRugEastDeed` <-> `WhiteTigerRugSouthDeed` |
| `WarriorStatueEastDeed` <-> `WarriorStatueSouthDeed` |
| `WaterTroughEastDeed` <-> `WaterTroughSouthDeed` |
| `DolphinEastLargeAddonDeed` <-> `DolphinSouthLargeAddonDeed` |
| `DolphinEastSmallAddonDeed` <-> `DolphinSouthSmallAddonDeed` |
| `RoseEastLargeAddonDeed` <-> `RoseSouthLargeAddonDeed` |
| `RoseEastSmallAddonDeed` <-> `RoseSouthSmallAddonDeed` |
| `SkullEastLargeAddonDeed` <-> `SkullSouthLargeAddonDeed` |
| `SkullEastSmallAddonDeed` <-> `SkullSouthSmallAddonDeed` |
| `GothicCandelabraA` <-> `GothicCandelabraB` |
| `BurningScarecrowA` <-> `BurningScarecrowB` |
| `DaemonDartBoardSouthDeed` <-> `DaemonDartBoardEastDeed` |

## Special Cases

| Target type | Compiled result |
| --- | --- |
| `BaseStatueDeed` | Calls `Statues.FlipStatue((BaseStatueDeed)item, ((BaseStatueDeed)item).StatueID)`. This mutates the targeted deed in place instead of creating a replacement in the backpack. Odd `StatueID` values increment by `1`; even values decrement by `1`. The name text is changed between `(east)` and `(south)`. |
| `DDRelicBearRugsAddonDeed` | Copies `RelicGoldValue`, `RelicMainID`, `RelicRugID`, `RelicColor`, `RelicQuality`, and `RelicFound`, maps rug IDs `1<->2`, `3<->4`, `5<->6`, creates a new `DDRelicBearRugsAddonDeed`, and deletes the old item. |
| `MyTentEastAddonDeed` | Copies `TentColor`, creates `MyTentSouthAddonDeed(TentHue, 1)`, and deletes the old item. |
| `MyTentSouthAddonDeed` | Copies `TentColor`, creates `MyTentEastAddonDeed(TentHue, 1)`, and deletes the old item. |
| `MyCircusTentEastAddonDeed` | Copies `TentColor1` and `TentColor2`, creates `MyCircusTentSouthAddonDeed(TentHue1, TentHue2, 1)`, and deletes the old item. |
| `MyCircusTentSouthAddonDeed` | Copies `TentColor1` and `TentColor2`, creates `MyCircusTentEastAddonDeed(TentHue1, TentHue2, 1)`, and deletes the old item. |

## Related Admin Command

`Flip Deed` itself has no player `[Command]` and no dedicated staff command. A separate GM-only command exists in `FlipableAttribute.cs`:

| Command | Access | Usage | Description | Behavior |
| --- | --- | --- | --- | --- |
| `[Flip` | `AccessLevel.GameMaster` | `Flip` | `Turns an item.` | Targets an `Item`, reads the first `FlipableAttribute`, and applies `FlipableAttribute.Flip(item)`. It does not use the `InteriorDecorator.Deed` replacement table. |

This GM command is useful for ordinary `FlipableAttribute` items. It does not create opposite-direction addon deed classes, preserve tent/relic state, or run the house-floor validation used by the player-facing `Flip Deed` option.

## Serialization

`InteriorDecorator` is a regular `Item` with a `Serial` constructor. It writes version `0` after `base.Serialize(writer)` and reads the version after `base.Deserialize(reader)`. The selected `DecorateCommand` is not serialized, so the tool does not persist the last selected gump command across world saves.

Most replacement targets inherit their own serialization from `BaseAddonDeed`, which writes version `0`. The special tent, circus tent, relic bear-rug, and stone statue deed classes store their own custom fields separately.

## Known Issues

* `InteriorDecorator.CheckUse(InteriorDecorator tool, Mobile from)` ignores the `tool` argument and only checks the caller's current house co-owner status. A target cursor opened before the tool is moved or deleted can continue to run as long as the caller remains a house co-owner.
* `InteriorDecorator.InternalGump.OnResponse` dereferences `sender.Mobile` and later assigns `sender.Mobile.Target` without guarding `sender`, `sender.Mobile`, `m_Decorator`, or deleted decorator state.
* The supported deed list is a hardcoded `else if` chain. Directional `BaseAddonDeed` classes not explicitly listed receive `That deed cannot be flipped to face another direction!`, even if they are otherwise directional or `FlipableAttribute`-decorated.
* `Flip Deed` behavior is inconsistent for `BaseStatueDeed`: regular deed pairs are replaced into the caller's backpack and delete the old floor item, but `BaseStatueDeed` is mutated in place.
* `InfoHelpGump.OnResponse` also dereferences `sender.Mobile` without a null guard when closing the help page.
