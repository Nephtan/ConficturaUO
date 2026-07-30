# Static Gump Tool

## Scope

This page documents the staff-facing static editing gump package under `Data/Scripts/Custom/StaffTools/StaticGumpTool/`.
It covers the alternative Oz'Thoth static tool gump, its categorized static picker gumps, and the staff commands those gumps dispatch.

The package is not a player feature. It is a Game Master building aid for adding, moving, inspecting, and deleting static items through existing RunUO command surfaces.

## Entry Points

| Command | Access | Behavior |
| --- | --- | --- |
| `StaticsTool` | GameMaster | Opens `OzThothsStaticOtherGump`, the main static editing panel. |
| `STool` | GameMaster | Alias for the main static editing panel. |
| `ST` | GameMaster | Short alias for the main static editing panel. |

The main gump can also open the package's category picker gumps. Each category picker has its own Game Master command, such as `AddArch`, `AddWall`, `AddFloor`, `AddFurniture`, `AddDungStatic`, `AddMiscStatic`, and `AddAnimationStatic`.

## Main Gump Actions

| Area | Buttons dispatch |
| --- | --- |
| Single height | `inc z` adjustments by `1`, `3`, `5`, `10`, or `20`, in positive or negative direction. |
| Area height | `area inc z` adjustments by `1`, `3`, `5`, `10`, or `20`, in positive or negative direction. |
| Single direction | `inc x` and `inc y` adjustments by `1`, `3`, `5`, `10`, or `20`. |
| Area direction | `area inc x` and `area inc y` adjustments by `1`, `3`, `5`, `10`, or `20`. |
| Utility actions | `Get ItemID`, `Props`, `Wipe`, `Delete`, `M Delete`, and `Area Delete`. |
| Window controls | Close the main gump or collapse it into `MinStaticGump2`. |

The main panel does not implement static editing directly. Its reply handler builds command strings with the active command prefix and sends them through `CommandSystem.Handle`.

## Static Catalogs

The package includes picker gumps for these static categories:

| Category | Command |
| --- | --- |
| Arches | `AddArch` |
| Doors | `AddDoor` |
| Fences | `AddFence` |
| Floors | `AddFloor` |
| Plants | `AddPlants` |
| Signs | `AddSign` |
| Stairs | `AddStair` |
| Rocks | `AddRock` |
| Roofs | `AddRoof` |
| Walls | `AddWall` |
| Gear | `AddGearStatic` |
| Food and cooking | `AddCookingStatic` |
| Furniture | `AddFurniture` |
| Dungeon | `AddDungStatic` |
| Lights | `AddLightsStatic` |
| Ground | `AddGroundStStatic` |
| Sign posts | `AddSignPStatic` |
| Decoration | `AddDecoStatic` |
| Nature | `AddNatureStatic` |
| Miscellaneous | `AddMiscStatic` |
| Custom | `AddCustomStatic` |
| Animation | `AddAnimationStatic` |

Picker gumps render pages of item art from static ID arrays. Selecting an entry dispatches `M Add Static <itemID>` and then reopens the picker on the same page.

## Staff Notes

The included README warns not to run both versions of the imported static gump package at the same time. This checkout documents the alternative version represented by `StaticGump(Alternative).cs` and the `Side Gumps` directory.

Because the tool drives existing add, move, inspect, and delete commands, staff should treat it like direct command access. Use area operations carefully and prefer narrow selections before invoking `Wipe`, `Delete`, `M Delete`, or `Area Delete`.

The tool does not define a world controller, persistent item, timer, save format, packet handler, or player command.

## Known Issues

| Issue | Impact |
| --- | --- |
| The main command handler advertises `[Usage("StaticTool")]`, but the registered command is `StaticsTool`. | Staff help text can point to the wrong command spelling. |
| Main and side gump command/reply handlers dereference `e.Mobile` or `sender.Mobile` without null or stale-state guards. | Malformed command context or stale gump replies can throw instead of failing quietly. |
| Side gumps index their static catalog arrays from reply button IDs without bounds checks after the item-selection offset is removed. | Invalid or stale reply IDs can throw `IndexOutOfRangeException`. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0073.
- Backlog rows: RB-06778.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/StaffTools/StaticGumpTool/ (CurrentDirectory)

### Runtime Evidence

- Hook summary: Command=23; Gump=153; Initialize=22.
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddAnimation.cs:L16 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddAnimation.cs:L18 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddAnimation.cs:L30 Gump SendGump access=Internal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddAnimation.cs:L736 Gump OnResponse access=Internal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddAnimation.cs:L767 Gump SendGump access=Internal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddArch.cs:L16 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddArch.cs:L18 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddArch.cs:L30 Gump SendGump access=Internal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddArch.cs:L402 Gump OnResponse access=Internal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddArch.cs:L433 Gump SendGump access=Internal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddCooking.cs:L16 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddCooking.cs:L18 Command CommandSystem.Register access=Unknown
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- No serialized classes matched the reviewed source set in serialization-register.csv.

### Project And Runtime Coverage

- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddAnimation.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddAnimation.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddArch.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddArch.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddCooking.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddCooking.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddCustom.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddCustom.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddDeco.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddDeco.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddDungeon.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddDungeon.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/addfence.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/addfence.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/addfloor.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/addfloor.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddFurniture.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddFurniture.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddGear.cs=Keep
- Data/Scripts/Custom/StaffTools/StaticGumpTool/Side Gumps/AddGear.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
