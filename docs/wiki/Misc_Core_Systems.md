# Misc Core Systems

## Overview
The Misc Core Systems audit entry covers the shard restart and automatic save infrastructure under `Server.Misc`. The active runtime pieces are `AutoRestart`, `AutoRestarter`, `AutoSave`, `CrashGuard`, and the core `Core.Kill` / `World.Save` handoff.

The system is timer-driven. `AutoRestart.Initialize()` registers administrator restart commands, builds the restart warning schedule, and starts a one-second timer. `AutoSave.Initialize()` starts the automatic save timer and registers `[SetSaves]`.

## Source Files
| File | Type | Purpose |
| --- | --- | --- |
| `Data/Scripts/System/Misc/AutoRestart.cs` | Timer and Item | Schedules automatic or manual restarts, sends warning broadcasts, persists settings through `AutoRestarter`, and calls `Core.Kill(true)`. |
| `Data/Scripts/System/Misc/AutoSave.cs` | Timer/helper | Performs scheduled world saves, automatic backup rotation, and pre-save cleanup of special temporary creatures. |
| `Data/Scripts/System/Misc/CrashGuard.cs` | Crash event handler | Generates crash reports, copies a crash backup, and can spawn a replacement process after a crash. |
| `Data/System/Source/Main.cs` | Core process control | Implements `Core.Kill(bool restart)`, including replacement process launch and current process termination. |
| `Data/System/Source/World.cs` | Core persistence | Implements `World.Save`, save strategy dispatch, `WorldSave` event invocation, and NetState pause/resume during saves. |
| `Data/Scripts/System/Misc/Settings.cs` | Server settings | Supplies the automatic save interval through `MyServerSettings.ServerSaveMinutes()`. |
| `Data/Scripts/Trades/Gardening/PlantSystem.cs` | Dependent subsystem | Hooks `WorldSave` growth only when `AutoRestart.Enabled` is false at configure time. |

## Entry Points
| Entry point | Access | Compiled behavior |
| --- | --- | --- |
| `AutoRestart.Initialize()` | Script startup | Registers restart commands, initializes warning delays, recomputes warning flags, and starts `new AutoRestart()`. |
| `[Restart [x]]` | Administrator | Schedules a server restart for now or approximately `x` minutes from now. |
| `[AutoRestartOn]`, `[AR-On]` | Administrator | Sets the static restart scheduler enabled flag to true. |
| `[AutoRestartOff]`, `[AR-Off]` | Administrator | Sets the static restart scheduler enabled flag to false. |
| `[AutoRestartWhen]`, `[AR-When]` | Administrator | Reports the current computed restart time and whether the scheduler is disabled. |
| `[AutoRestartTime]`, `[AR-Time]` | Administrator | Parses the first argument as hours, stores it in `RestartTimeOfDay`, and recomputes the next scheduled restart. |
| `[AutoRestartColor]`, `[AR-Color]` | Administrator | Sets the warning hue from a named color or integer string. |
| `[AutoRestartText]`, `[AR-Text]` | Administrator | Replaces the warning broadcast text with the full argument string. |
| `[AutoRestartSave]`, `[AR-Save]` | Administrator | Drops an `AutoRestarter` settings Item into a `PlayerMobile` backpack. |
| `[AutoRestartLoad]`, `[AR-Load]` | Administrator | Opens a target cursor and loads settings from a targeted `AutoRestarter` Item. |
| `AutoSave.Initialize()` | Script startup | Starts the automatic save timer and registers `[SetSaves]`. |
| `[SetSaves <true | false>]` | Administrator | Enables or disables the automatic save timer path. |
| `CrashGuard.Initialize()` | Script startup | If enabled, subscribes `CrashGuard_OnCrash` to `EventSink.Crashed`. |

## Restart Scheduler Defaults
| Setting | Compiled default |
| --- | --- |
| Frequency | `RestartType.Weekly` |
| Weekly day | `DayOfWeek.Wednesday` |
| Time of day | `TimeSpan.FromHours(9)` |
| Restart delay after save | `TimeSpan.Zero` |
| Enabled | `true` |
| Warning hue | `0x22` |
| Warning message | `The server will be restarting for routine maintenance` |
| Warning delays in minutes | `1, 2, 5, 10, 15, 20, 25, 30, 45` |

`ResetWarningDelayBools(true)` computes `m_RestartDateTime` from the current date plus `RestartTimeOfDay`. If that time has already passed, one day is added. For weekly restarts, the date is then advanced until `m_RestartDateTime.DayOfWeek == RestartDay`.

Each warning delay receives a matching boolean. The boolean starts true only when `m_RestartDateTime - delay` is still in the future at reset time. Missed warnings are not replayed.

## Restart Flow
| Stage | Compiled behavior |
| --- | --- |
| Timer tick guard | `AutoRestart.OnTick()` returns immediately when `m_Restarting` is true or `Enabled` is false. |
| Warning check | The timer iterates `WarningDelays` and sends one pending warning per tick when `DateTime.Now > m_RestartDateTime - delay`. |
| Warning broadcast | `World.Broadcast(WarningColor, true, "{0} in {1} minutes.", RestartMessage, time)`. |
| Restart time reached | If the current time is at or after `m_RestartDateTime`, `AutoSave.Save()` is called. |
| Restart latch | After the save call returns, `m_Restarting` is set to true. |
| Delayed callback | `Timer.DelayCall(RestartDelay, Restart_Callback)` schedules the process restart. The default delay is zero. |
| Process restart | `Restart_Callback()` calls `Core.Kill(true)`. `Core.Kill(true)` starts `Core.ExePath` with `Core.Arguments`, then kills the current process. |

Manual `[Restart]` uses the same timer path. It parses the optional first argument as a double minute count, sets `m_Enabled = true`, sets `m_RestartDateTime = DateTime.Now + TimeSpan.FromMinutes(minutes)`, and resets warning booleans without recomputing the automatic daily or weekly date.

## Warning Color Resolution
| Input | Result |
| --- | --- |
| `red` | `Utility.RandomRedHue()` |
| `pink` | `Utility.RandomPinkHue()` |
| `blue` | `Utility.RandomBlueHue()` |
| `yellow` | `Utility.RandomYellowHue()` |
| `green` | `Utility.RandomGreenHue()` |
| `orange` | `Utility.RandomOrangeHue()` |
| `brown` | `Utility.RandomBirdHue()` |
| Any parseable integer string | `int.Parse(color)` |
| Anything else | `Utility.RandomDyedHue()` |

## AutoRestarter Item
`AutoRestarter` is a `[Flipable(0x1810, 0x1811)]` `Item` named `Auto-Restart Control Item`. It is constructable directly or through `[AutoRestartSave]`.

| Property | Command access | Purpose |
| --- | --- | --- |
| `Enabled` | GameMaster | Saved scheduler enabled flag. |
| `Restarting` | GameMaster | Saved restart latch. Loading this value copies it directly into `AutoRestart`. |
| `RestartFrequency` | GameMaster | `Daily` or `Weekly`. |
| `RestartDay` | GameMaster | Day used by weekly restart scheduling. |
| `RestartMessage` | GameMaster | Broadcast message prefix. |
| `RestartDateTime` | GameMaster | Saved exact restart date/time, but the loader recomputes it immediately from time-of-day settings. |
| `NextWarningTime` | GameMaster | Saved display field; not used by `AutoRestart.LoadSettings()`. |
| `WarningColor` | GameMaster | Warning broadcast hue. |
| `RestartDelay` | GameMaster | Delay between the pre-restart save and `Core.Kill(true)`. |
| `RestartTimeOfDay` | GameMaster | Time-of-day component used by scheduler recomputation. |
| `WarningDelaysList` | GameMaster | Comma-separated delay list parsed into `List<double>`. |

The default constructable item is disabled, not restarting, daily on Monday, uses a 2-hour restart time, hue `0x22`, zero restart delay, and warning delays `1, 2, 5, 10`.

## AutoRestarter Serialization
The item writes version `0` after `base.Serialize(writer)`. Its read order matches its write order.

| Order | Serialized value | Reader |
| --- | --- | --- |
| 1 | Version integer `0` | `ReadInt()` |
| 2 | `m_Enabled` | `ReadBool()` |
| 3 | `m_Restarting` | `ReadBool()` |
| 4 | `m_RestartFrequency` as int | `ReadInt()` cast to `RestartType` |
| 5 | `m_RestartDay` as int | `ReadInt()` cast to `DayOfWeek` |
| 6 | `m_RestartMessage` | `ReadString()` |
| 7 | `m_RestartDateTime` | `ReadDateTime()` |
| 8 | `m_NextWarningTime` | `ReadDateTime()` |
| 9 | `m_WarningColor` | `ReadInt()` |
| 10 | `m_RestartDelay` | `ReadTimeSpan()` |
| 11 | `m_RestartTimeOfDay` | `ReadTimeSpan()` |
| 12 | Warning delay count | `ReadInt()` |
| 13+ | Each warning delay as double | `ReadDouble()` repeated count times |

There are no version upgrades beyond version `0`.

## AutoSave Timer
`AutoSave` runs at `MyServerSettings.ServerSaveMinutes()`, clamped by `Settings.cs` to the inclusive range `10` through `240` minutes. The timer priority is `TimerPriority.OneMinute`.

| Stage | Compiled behavior |
| --- | --- |
| Tick guard | Returns when automatic saves are disabled or `AutoRestart.Restarting` is true. |
| Save warning | The warning path exists, but `m_Warning` is compiled as `TimeSpan.Zero`, so the timer immediately saves instead of scheduling a warning. |
| Pre-save cleanup | Deletes `BaseCreature` mobiles with `ControlSlots == 666` and no `Combatant`, after sending particles and sound at their location. |
| Save call | Calls `AutoSave.Save(true)`, permitting background writes. |
| Manual save helper | `AutoSave.Save()` calls `AutoSave.Save(false)`, blocking until disk write completion. |
| Restart save | `AutoRestart.OnTick()` calls `AutoSave.Save()` before latching `m_Restarting`. |

## Backup Rotation
Before each `World.Save`, `AutoSave.Save(bool permitBackgroundWrite)` waits for any prior disk write to finish, then calls `Backup()`.

| Slot order | Rotation behavior |
| --- | --- |
| `Sixth Backup` | Deleted when present. |
| `Fifth Backup` | Moved to `Sixth Backup (<timestamp>)`. |
| `Fourth Backup` | Moved to `Fifth Backup (<timestamp>)`. |
| `Third Backup` | Moved to `Fourth Backup (<timestamp>)`. |
| `Second Backup` | Moved to `Third Backup (<timestamp>)`. |
| `Most Recent` | Moved to `Second Backup (<timestamp>)`. |
| Current `Saves` directory | Moved to `Most Recent (<current timestamp>)`. |

The backup root is `Backups/Automatic` under `Core.BaseDirectory`. After moving `Saves`, the backup copies flat files from `Info`, `Info/Articles`, and `Info/Custom` into the backup tree. Backup exceptions are caught and logged, and the world save still proceeds.

## World Save Handoff
`World.Save(message, permitBackgroundWrite)` performs the core persistence operation:

| Stage | Compiled behavior |
| --- | --- |
| Re-entry guard | Returns when `m_Saving` is already true. |
| Network preparation | Flushes all `NetState` instances, then pauses network traffic. |
| Disk-write wait | Blocks on `World.WaitForWriteCompletion()`. |
| Save state | Sets `m_Saving = true`, resets the disk write handle, optionally broadcasts the save start message, and acquires the save strategy. |
| Directory setup | Ensures `Saves/Mobiles`, `Saves/Items`, and `Saves/Guilds` exist. |
| Strategy save | Calls `strategy.Save(null, permitBackgroundWrite)`. |
| World save event | Invokes `EventSink.WorldSave` through `EventSink.InvokeWorldSave(new WorldSaveEventArgs(message))`. |
| Completion | Clears `m_Saving`, notifies disk write completion when not background writing, runs safety queues and decay, optionally broadcasts completion, resumes `NetState`, and writes diagnostics. |

## CrashGuard Path
`CrashGuard_OnCrash` is separate from the normal restart timer. When enabled, it generates a crash report, waits for any active disk write, copies a crash backup, and then either closes the service process or launches a replacement process and kills the current one. The crash backup copies core save files from `Saves` plus the same `Info`, `Info/Articles`, and `Info/Custom` helper files used by `AutoSave`.

## Cross-System Effects
| System | Effect |
| --- | --- |
| Gardening | `PlantSystem.Configure()` only hooks `EventSink.WorldSave` when `Misc.AutoRestart.Enabled` is false at configure time. Because `AutoRestart.Enabled` defaults true, world-save plant growth is skipped in the default configuration. |
| Staff save commands | Core staff save handlers call `AutoSave.Save()` or `AutoSave.Save(true)`, so manual saves share the same backup and diagnostic path as automatic saves. |
| Console save commands | Console save paths call `Misc.AutoSave.Save()`. |

## Known Issues
- Loading an `AutoRestarter` with `Restarting = true` copies that value into the static `AutoRestart` latch. `AutoRestart.OnTick()` then returns before scheduling `Core.Kill(true)`, while `AutoSave.Save()` also returns when `AutoRestart.Restarting` is true. This can leave the shard with both the restart timer and autosaves suppressed.
- `AutoRestart.LoadSettings()` copies `AutoRestarter.RestartDateTime`, then immediately calls `ResetWarningDelayBools(true)`, which recomputes `m_RestartDateTime` from `RestartTimeOfDay` and `RestartDay`. The persisted exact restart date/time is effectively ignored.
- `AutoRestarter` computes `m_NextWarningTime` with `m_WarningDelays[m_WarningDelays.Count]`, an out-of-range index. The exception is swallowed and `NextWarningTime` falls back to the restart time.
- `WarningDelaysList` clears the existing delay list before parsing. A bad value can leave the item with an empty or partially parsed warning schedule, and loading that item can disable warning broadcasts.
- `[Restart]` and `[AutoRestartTime]` parse double values with minimal validation. Negative, `NaN`, infinity, or very large values are not explicitly rejected before time arithmetic.
- `AutoRestartLoadTarget` silently ignores non-`AutoRestarter` targets, so staff get no feedback on invalid targets.
- `Core.Kill(true)` starts the replacement process without a local try/catch. If `Process.Start(ExePath, Arguments)` throws, the restart callback can fail before the current process is killed.
- `World.Save` pauses `NetState` traffic before several operations that can throw, but the resume call is not protected by a `finally` block. A save exception before `NetState.Resume()` can leave network state paused and `m_Saving` uncleared.
- Backup rotation swallows directory delete/move exceptions. A failed rotation is only reflected in diagnostic counts, not in staff-facing command feedback.
- `PlantSystem.Configure()` checks `AutoRestart.Enabled` only once during configure. Later `[AutoRestartOff]` use does not add the `WorldSave` plant growth hook.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0017.
- Backlog rows: RB-06724.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/System/Misc/AutoRestart.cs (CurrentFile)
- Data/Scripts/System/Misc/AutoSave.cs (CurrentFile)
- Data/Scripts/System/Misc/CrashGuard.cs (CurrentFile)
- Data/System/Source/Main.cs (CurrentFile)
- Data/System/Source/World.cs (CurrentFile)
- Data/Scripts/System/Misc/Settings.cs (CurrentFile)
- Data/Scripts/Trades/Gardening/PlantSystem.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=18; Event=10; Initialize=3; Timer=4; WorldLoad=2; WorldSave=6.
- Data/Scripts/System/Misc/AutoRestart.cs:L28 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/System/Misc/AutoRestart.cs:L30 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Misc/AutoRestart.cs:L32 Command CommandSystem.Register access=Administrator
- Data/Scripts/System/Misc/AutoRestart.cs:L33 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/AutoRestart.cs:L38 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/AutoRestart.cs:L43 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/AutoRestart.cs:L48 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/AutoRestart.cs:L53 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/AutoRestart.cs:L58 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/AutoRestart.cs:L63 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/AutoRestart.cs:L68 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/AutoRestart.cs:L73 Command CommandSystem.Register access=Administrator
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 4.
- Data/Scripts/System/Misc/AutoRestart.cs:Server.Misc.AutoRestarter version=0 serialize=L640 deserialize=L665 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Trades/Gardening/PlantSystem.cs:Unknown version=Unknown serialize=L deserialize=L alignment=AlignedByCountAndKnownTypes
- Data/System/Source/Main.cs:Unknown version=Unknown serialize=L deserialize=L alignment=AlignedByCountAndKnownTypes
- Data/System/Source/World.cs:Unknown version=Unknown serialize=L deserialize=L alignment=AlignedByCountAndKnownTypes

### Project And Runtime Coverage

- Data/Scripts/System/Misc/AutoRestart.cs=Keep
- Data/Scripts/System/Misc/AutoRestart.cs=Keep
- Data/Scripts/System/Misc/AutoSave.cs=Keep
- Data/Scripts/System/Misc/AutoSave.cs=Keep
- Data/Scripts/System/Misc/CrashGuard.cs=Keep
- Data/Scripts/System/Misc/CrashGuard.cs=Keep
- Data/Scripts/System/Misc/Settings.cs=Keep
- Data/Scripts/System/Misc/Settings.cs=Keep
- Data/Scripts/Trades/Gardening/PlantSystem.cs=Keep
- Data/Scripts/Trades/Gardening/PlantSystem.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
