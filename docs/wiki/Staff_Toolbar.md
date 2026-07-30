# Staff Toolbar

## Overview
The Staff Toolbar is Joeku's command-toolbar system implemented in `Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs`. It is compiled through `Data/Scripts/Scripts.csproj` and lives in the `Joeku` namespace.

The system consists of one command/login helper, one persistent storage item, account-keyed toolbar profiles, and two gumps:

| Class | Role |
| --- | --- |
| `ToolbarHelper` | Registers `[Toolbar`, hooks staff login, and sends the toolbar gump. |
| `ToolbarInfos` | Hidden persistence item that stores all `ToolbarInfo` profiles. |
| `ToolbarInfo` | Per-account toolbar settings: dimensions, entries, skin, marker points, font, and switches. |
| `Toolbar` | The compact staff command bar. |
| `ToolbarEdit` | The customization gump for layout, entries, and switches. |
| `GumpIDs` | Static art/font/skin metadata used by both gumps. |

There are no mobiles, XMLSpawner definitions, or separate engine scripts for this system.

## Access and Launch
`ToolbarHelper.Initialize()` registers `[Toolbar` at `AccessLevel.Counselor` and hooks `EventSink.Login`. On login, any mobile with `AccessLevel.Counselor` or higher has its existing `Toolbar` gump closed and a new one sent. The command performs the same close-and-send flow.

Opening the toolbar calls `ReadInfo`, which looks up a `ToolbarInfo` by account username. If none exists, `ToolbarInfo.CreateNew` creates one. `EnsureMaxed` also compares the mobile's access level to the account access level and raises the account access level to the mobile's level when the mobile is higher, logging the adjustment to the console.

## Default Layouts
New profiles are generated from the mobile's current access level:

| Access level | Default size | Default entries |
| --- | --- | --- |
| Player | `0 x 0` | None. Players cannot use `[Toolbar` because the command is Counselor-gated. |
| Counselor | `5 x 1` | `[Who`, `[Where`, `[Tele`, `[M Tele`, `[SpeedBoost` |
| GameMaster / Seer | `5 x 2` | Counselor entries, four unused slots, then `[Add`, `[Move`, `[Delete`, `[Wipe`, `[Kill` |
| Administrator / Developer / Owner | `5 x 2` | `[Admin`, `[Where`, `[Tele`, `[M Tele`, `[SpeedBoost`, four unused slots, then `[Add`, `[Move`, `[Delete`, `[Wipe`, `[Kill` |

The backing entry list is padded with `-*UNUSED*-` placeholders for the edit grid.

## Toolbar Behavior
The compact toolbar renders `Columns * Rows` buttons. Each visible button displays its configured entry text.

When a button is pressed:

* Entries beginning with `CommandSystem.Prefix` are echoed to the staff member and executed through `CommandSystem.Handle`.
* Other entries are sent through `Mobile.DoSpeech` as regular speech using the mobile's speech hue.
* The toolbar gump is sent again before the entry runs.
* Clicking a placeholder entry therefore speaks `-*UNUSED*-`; placeholders are not ignored by special-case logic.

The toolbar includes a minimize/maximize page toggle and a customize button. If `Lock` is enabled, the gump sets `Closable = false`, preventing normal right-click close behavior.

## Customization
The customize button opens `ToolbarEdit`. The edit gump exposes:

* **Skin:** two skins, indexed internally as `0` and `1`.
* **Rows:** adjustable from `1` through `15` using the gump controls.
* **Columns:** adjustable from `1` through `9` using the gump controls.
* **Font:** twelve HTML font wrappers, from no wrapper through bold/italic/small/big combinations.
* **Phantom:** adds white base font markup and alpha regions to toolbar buttons.
* **Stealth:** changes the toolbar's page setup so it returns to the minimized page after a toolbar button is used.
* **Reverse:** places the minimize/customize bar above the command grid instead of below it.
* **Lock:** persists the `Closable = false` behavior.
* **Expanded View:** doubles the edit-cell dimensions for easier editing.

The edit grid is organized as nine pages in a 3-by-3 navigation layout. Each page edits a 3-column by 5-row block, giving 135 editable command slots.

The edit response buttons work as follows:

| Button | Code behavior |
| --- | --- |
| Default | Rebuilds default access-level entries, filling default unused slots from existing text where possible, then keeps later custom entries. |
| Okay | Saves changes, closes the edit gump, and refreshes the compact toolbar. |
| Cancel | Closes the edit gump without saving. |
| Apply | Saves changes, reopens the edit gump, and refreshes the compact toolbar. |

## Persistence
`ToolbarInfos` overrides RunUO serialization and cannot be deleted through `Delete()`, which returns immediately.

Current saves write `ToolbarHelper.Version`, currently `130`, then the number of stored profiles. Each profile writes:

1. Version 1.3 fields: `Font`, `Phantom`, `Stealth`, `Reverse`, and `Lock`.
2. Version 1.0 fields: `Account`, `Dimensions`, `Entries`, `Skin`, and `Points`.

Deserialization sets `ToolbarHelper.Infos` to the loaded persistence item and supports version `130` with a fall-through to the legacy `100` layout. Legacy version `100` profiles default to font `0`, phantom enabled, stealth disabled, reverse disabled, and lock enabled.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0044; PBN-0072.
- Backlog rows: RB-06774; RB-06775.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs (CurrentFile)
- Data/Scripts/Scripts.csproj (CurrentFile)

### Runtime Evidence

- Hook summary: Event=1; Gump=23; Initialize=1; Login=1.
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L28 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L38 Event EventSink access=GlobalOrInternal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L44 Login OnLogin access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L71 Gump SendGump access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L680 Gump OnResponse access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L687 Gump SendGump access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L708 Gump SendGump access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L710 Gump SendGump access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L932 Gump OnResponse access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L950 Gump SendGump access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L964 Gump SendGump access=Internal
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:L978 Gump SendGump access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 1.
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs:Joeku.ToolbarInfos version=Unknown serialize=L129 deserialize=L167 alignment=CountMismatch:Writes=15;Reads=14

### Project And Runtime Coverage

- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs=Keep
- Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]/Toolbar.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
