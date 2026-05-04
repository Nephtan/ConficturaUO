# Agent Instructions for Confictura RunUO Shard

This document provides instructions for completing tasks within this repository. Please adhere to these guidelines carefully.

# Primary Objective

The agent's primary objective is to assist with the development and maintenance of the Confictura RunUO shard. This includes, but is not limited to:

* Creating new C# scripts for items, mobiles (monsters), and game systems.

* Refactoring and optimizing existing code for clarity and performance.

* Analyzing the codebase to answer questions or identify potential issues.

* Assisting with any other C#-based coding tasks related to the RunUO environment.

## Environment Setup

* **Framework:** .NET Framework 4.8

* **IDE:** The project is developed using Visual Studio Community 2022.

* **Core Dependencies:** The `Data/Scripts/Scripts.csproj` project depends on the core engine project at `Data/System/Source/Server.csproj`. Keep this relationship in mind when analyzing or modifying code.

* **Line Endings:** Use (LF) line endings to remain compatible with the server environment.

## Known Build Workflows

* **Preferred Build Entry Point:** Open `ConficturaUO.sln` in Visual Studio 2022. This is the maintained server build workflow. The solution includes `Data/System/Source/Server.csproj` and `Data/Scripts/Scripts.csproj`, and the `Scripts` project depends on `Server`.

* **Command-Line Solution Build:** From a Visual Studio Developer PowerShell/Command Prompt, use `msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform=x86` or `msbuild ConficturaUO.sln /p:Configuration=Release /p:Platform=x86`. If `msbuild` is not on PATH, use the Visual Studio 2022 MSBuild executable directly. The solution maps the `Server` project to `x86` and the `Scripts` project to `Any CPU`.

* **Command-Line Project Builds:** For narrower builds, use `msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86` or `msbuild Data/System/Source/Server.csproj /p:Configuration=Release /p:Platform=x86` for the server, or `msbuild Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU` or `msbuild Data/Scripts/Scripts.csproj /p:Configuration=Release /p:Platform=AnyCPU` for scripts. `Scripts.csproj` has a project reference to `..\System\Source\Server.csproj`, so building scripts also pulls in the server project.

* **Project Outputs:** `Data/System/Source/Server.csproj` builds the maintained server executable as `ConficturaServer.exe`. `Debug|x86` outputs `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` to the repository root; `Release|x86` outputs to `Data/System/Source/bin/Release/`. `Data/Scripts/Scripts.csproj` builds `ClassLibrary.dll`; `Debug|AnyCPU` outputs to `Data/Scripts/` and `Release|AnyCPU` outputs to `Data/Scripts/bin/Release/`.

* **Legacy Manual Compile Reference:** `Data/System/Source/README` still documents an old direct `csc` command that outputs `World.exe`. Treat that executable name as stale documentation only; the current production/server artifact produced by the maintained Visual Studio/MSBuild workflow is `ConficturaServer.exe`.

## Coding Conventions

* **Style Source:** Follow the style already used by maintained Confictura scripts in `Data/Scripts/Custom/` before applying generic C# preferences. When extending an imported custom system, match the nearby local style even if it differs from Confictura-native code.

* **Naming:** All new C# files and classes must use PascalCase (e.g., `MyNewItem.cs`).

* **Namespaces:** New Confictura-native scripts should use `Server.Custom.Confictura` or a child namespace such as `Server.Custom.Confictura.Mobiles`. When extending an existing RunUO or imported custom system, match the nearby namespace such as `Server.Items`, `Server.Mobiles`, `Server.Commands`, or `Server.Engines.*`.

* **File Location:** Place new Confictura custom scripts within the `Data/Scripts/Custom/` directory and organize them into logical subdirectories based on their function (e.g., `Items/`, `Mobiles/`, `Systems/`). `Scripts.csproj` explicitly lists source files, so add new `.cs` files to that project file with a `Compile Include` entry unless Visual Studio has already done it.

* **Local Variables:** Prefer explicit local variable types over `var`, including collection declarations and temporary values. Use explicit casts or `as` assignments followed by null checks where that is the surrounding pattern.

* **Formatting:** Use Allman braces for namespaces, classes, methods, switches, and multi-line condition blocks. Keep single-line guard clauses only when the nearby file already uses that RunUO style; otherwise include braces for guarded blocks, especially when adding logic.

* **Null Checks and Compatibility:** Use `== null` and `!= null` for null checks. Avoid newer null syntax such as `is null`, `is not null`, and null-conditional `?.` in shard scripts unless the nearby subsystem already depends on it.

* **Gumps:** Build custom gumps in their constructors with the standard RunUO calls such as `AddPage`, `AddBackground`, `AddHtml`, `AddLabel`, `AddButton`, and related helpers. Prefer named button enums or clear button ID ranges, and handle replies in `OnResponse(NetState sender, RelayInfo info)` or `OnResponse(NetState state, RelayInfo info)` by taking `Mobile from = sender.Mobile` or `state.Mobile`, guarding invalid state, then switching on `info.ButtonID`.

* **Commands, Events, and Network Handlers:** Register commands and event hooks from `Initialize`. Use named static handler methods such as `Thing_OnCommand`, include `[Usage]` and `[Description]` attributes where command files already do so, and guard invalid mobiles early. Packet handlers should be named methods that receive `NetState` and `PacketReader`, read packet fields explicitly, and delegate shared behavior to helper methods when needed.

* **Comments:** Add comments for non-obvious gameplay rules, serialization behavior, and complex logic. Avoid comments that merely restate straightforward C#.

* **Searching:** When searching the codebase, prefer the `rg` (ripgrep) tool over `grep -R` or similar commands for better performance.

## RunUO Serialization Standards

RunUO serialization is positional: the order and type of every `writer.Write(...)` call defines the save-file format, and `Deserialize` must read the same values in the same order with matching `reader.Read...()` methods. A mismatched read order can corrupt the world save by shifting every later value.

For every `Item`, `Mobile`, or other RunUO object that overrides serialization:

* Include the `Serial` constructor required for world loading:

  ```csharp
  public MyThing(Serial serial)
      : base(serial)
  {
  }
  ```

* In `Serialize(GenericWriter writer)`, call `base.Serialize(writer);` before writing this class's data.
* Immediately after the base call, write this class's version integer. New serializers start at version `0`.

  ```csharp
  public override void Serialize(GenericWriter writer)
  {
      base.Serialize(writer);

      writer.Write((int)0); // version

      writer.Write(m_Field);
  }
  ```

* In `Deserialize(GenericReader reader)`, call `base.Deserialize(reader);` before reading this class's data.
* Immediately after the base call, read the version integer.

  ```csharp
  public override void Deserialize(GenericReader reader)
  {
      base.Deserialize(reader);

      int version = reader.ReadInt();

      m_Field = reader.ReadString();
  }
  ```

* Write and read fields with matching types and in identical order. For example, `writer.Write((Mobile)m_Owner)` must be matched by `reader.ReadMobile()`, `writer.Write((bool)m_Enabled)` by `reader.ReadBool()`, and enum values written as `int` must be read with `reader.ReadInt()` and cast back to the enum.
* Do not reorder, remove, or change the type of existing serialized values. When adding serialized data to an existing class, increment the version number and add guarded deserialization for the new version.
* Prefer the established RunUO fall-through pattern for versioned upgrades:

  ```csharp
  public override void Serialize(GenericWriter writer)
  {
      base.Serialize(writer);

      writer.Write((int)1); // version

      writer.Write(m_NewField); // version 1
      writer.Write(m_OldField); // version 0
  }

  public override void Deserialize(GenericReader reader)
  {
      base.Deserialize(reader);

      int version = reader.ReadInt();

      switch (version)
      {
          case 1:
          {
              m_NewField = reader.ReadString();
              goto case 0;
          }
          case 0:
          {
              m_OldField = reader.ReadInt();
              break;
          }
      }
  }
  ```

* If an existing class already uses `if (version >= N)` gates instead of `switch`/`goto case`, preserve that local style and keep the reads aligned with the write order.
* If a previously serialized field is removed, stop writing it only in a bumped version, but keep a conditional read for older versions to consume and discard the old value.
* For old versions that did not contain a new field, initialize a safe default through field initializers, constructors, or explicit `if (version < N)` fallback logic.
* Even classes with no custom fields should still write and read a version integer after the base call.

## Testing & Verification

* **Test Command:** None.

* **Build Verification:** This repository targets Windows and .NET Framework. For C# changes, run the narrowest relevant MSBuild command from the build workflow above when Visual Studio 2022/MSBuild is available. If MSBuild is unavailable in the current agent environment, state that clearly and rely on static analysis.

## Git & Version Control

If your task requires modifying or creating files, follow these steps:

1. **Work on the Main Branch:** Do not create new branches. All work should be done on the current checked-out branch.

2. **Commit Your Changes:** Use git to commit all your changes. Uncommitted code will not be evaluated.

3. **Handle Pre-Commit Hooks:** If a pre-commit hook fails, you must fix the reported issues and retry the commit until it succeeds.

4. **Maintain a Clean Worktree:** Run `git status` to confirm your commit. Your worktree must be in a clean state when you are finished.

5. **Do Not Amend History:** Do not modify or amend existing commits.

6. **Commit Messages:** Use brief, descriptive commit messages. When possible, follow the [Conventional Commits](https://www.conventionalcommits.org/) style (e.g., `docs: improve AGENTS instructions`).

## Citation Guidelines

If you browse files or use terminal commands to generate your solution, you must add citations to your final response.

* **Purpose:** Citations provide a reference for the information you used to generate your response.

* **Formats:**

  * **Files:** `?F:<file_path>†L<line_start>(-L<line_end>)??`

  * **Terminal Output:** `?<chunk_id>†L<line_start>(-L<line_end>)??`

* **Rules:**

  * File paths must be relative to the repository root.

  * Line numbers are 1-indexed. A single line number is acceptable if the citation refers to one line.

  * Ensure line numbers are correct and the cited content is directly relevant to the clause preceding the citation.

  * Prefer file citations for code and documentation references. Use terminal citations for results of commands (e.g., test output).

## AGENTS.md Interpretation Rules

This section contains meta-instructions on how to interpret AGENTS.md files.

* **Scope:** The rules in an AGENTS.md file apply to the entire directory tree where the file is located.

* **Precedence:**

  1. Direct instructions from the user prompt override all AGENTS.md files.

  2. More deeply nested AGENTS.md files take precedence over those in parent directories.

* **Compliance:** For every file you modify, you must obey the instructions in any AGENTS.md file whose scope includes that file.
