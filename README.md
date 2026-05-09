# ConficturaUO

ConficturaUO is the source repository for **Confictura: Legend & Adventure**, a heavily customized RunUO shard. It uses classic Ultima Online mechanics as the base for a multi-world sandbox RPG with custom character starts, expanded progression, dynamic PvE, player-run systems, homesteads, boats, magic paths, and staff tooling.

This README is the repository hub for contributors, staff, and returning maintainers. For the broader gameplay overview, start with the [Confictura Introduction](docs/wiki/Confictura_Introduction.md).

## What This Repository Contains

- A maintained Visual Studio solution: [ConficturaUO.sln](ConficturaUO.sln).
- The RunUO engine project: [Data/System/Source/Server.csproj](Data/System/Source/Server.csproj), built as `ConficturaServer.exe`.
- The active game script project: [Data/Scripts/Scripts.csproj](Data/Scripts/Scripts.csproj), built as `ClassLibrary.dll` and dependent on the server project.
- Custom shard systems under [Data/Scripts/Custom](Data/Scripts/Custom), plus important gameplay code in `Magic`, `Mobiles`, `Items`, `Trades`, `Quests`, and `System`.
- Staff, player, and technical documentation under [docs](docs), including the wiki index and system audit.
- Spawn, XML, and shard data folders such as [Spawns](Spawns), [DioXMLSpawn](DioXMLSpawn), and [Info](Info).

## Quick Start

1. Use Windows with Visual Studio Community 2022 or another Visual Studio 2022 edition with .NET Framework build tools.
2. Open [ConficturaUO.sln](ConficturaUO.sln).
3. Build the solution using `Debug|x86` or `Release|x86`.
4. For script work, keep in mind that `Scripts.csproj` references `Server.csproj`; building scripts also builds the server dependency.
5. Review [AGENTS.md](AGENTS.md) before changing code. It records repository conventions, serialization rules, and current build expectations.

## Build Workflows

The preferred build entry point is the solution:

```powershell
msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform=x86
msbuild ConficturaUO.sln /p:Configuration=Release /p:Platform=x86
```

Narrow server build:

```powershell
msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86
msbuild Data/System/Source/Server.csproj /p:Configuration=Release /p:Platform=x86
```

Narrow scripts build:

```powershell
msbuild Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU
msbuild Data/Scripts/Scripts.csproj /p:Configuration=Release /p:Platform=AnyCPU
```

Output expectations:

- `Server.csproj` targets .NET Framework 4.8 and emits `ConficturaServer.exe`.
- `Debug|x86` server builds output `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` at the repository root.
- `Release|x86` server builds output under `Data/System/Source/bin/Release/`.
- `Scripts.csproj` targets .NET Framework 4.8 and emits `ClassLibrary.dll`.
- `Debug|AnyCPU` script builds output under `Data/Scripts/`.
- `Release|AnyCPU` script builds output under `Data/Scripts/bin/Release/`.

There is no dedicated test command. For C# changes, run the narrowest relevant MSBuild command when Visual Studio 2022 MSBuild is available.

## Repository Map

| Path | Purpose |
| --- | --- |
| [Data/System/Source](Data/System/Source) | RunUO server engine source and `ConficturaServer` project. |
| [Data/Scripts](Data/Scripts) | Active shard scripts compiled into `ClassLibrary.dll`. |
| [Data/Scripts/Custom](Data/Scripts/Custom) | Confictura-specific systems and imported custom packages. |
| [docs/wiki](docs/wiki) | Player, staff, and technical guide pages grouped by system. |
| [docs/SystemAudit.md](docs/SystemAudit.md) | Broad system inventory with documentation status and known rework notes. |
| [docs/AI_OVERHAUL_AUDIT.md](docs/AI_OVERHAUL_AUDIT.md) | AI overhaul audit and notes for creature AI work. |
| [Spawns](Spawns) | Spawn data used by the shard. |
| [DioXMLSpawn](DioXMLSpawn) | XML spawn-related data and support files. |
| [Info](Info) | Additional shard information and data files. |

## Documentation

Start here:

- [Confictura Introduction](docs/wiki/Confictura_Introduction.md) for the shard identity and major gameplay concepts.
- [ConficturaUO Game Systems](docs/wiki/INDEX.md) for the wiki index.
- [System Audit](docs/SystemAudit.md) for the broadest system inventory and current rework notes.

High-signal technical and gameplay references:

- [Character Level Recon Report](docs/wiki/Character_Level_Recon_Report.md)
- [Random Encounter Engine](docs/wiki/Random_Encounter_Engine.md)
- [OmniAI](docs/wiki/OmniAI.md)
- [Creature AI Core](docs/wiki/Creature_AI_Core.md)
- [Government System](docs/wiki/Government_System.md)
- [Homestead System](docs/wiki/Homestead_System.md)
- [XML Spawner Enhancements](docs/wiki/XML_Spawner_Enhancements.md)

## Development Notes

- Follow the style already used by nearby Confictura scripts before applying generic C# preferences.
- New Confictura-native scripts normally belong under `Data/Scripts/Custom/` and should use the appropriate `Server.Custom.Confictura` namespace or a nearby existing namespace.
- `Scripts.csproj` explicitly lists compiled source files; add new `.cs` files to the project file unless Visual Studio has already done it.
- Use RunUO positional serialization carefully. Any changed `Serialize` write order must be matched exactly by `Deserialize`.
- Prefer LF line endings for repository files.
- The legacy `Data/System/Source/README` direct `csc` workflow refers to `World.exe`; the maintained build artifact is `ConficturaServer.exe`.

## License

See [LICENSE](LICENSE).
