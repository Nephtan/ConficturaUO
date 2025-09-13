# Agent Instructions for Confictura RunUO Shard

This document provides instructions for completing tasks within this repository. Please adhere to these guidelines carefully.

## Primary Objective

The agent's primary objective is to assist with the development and maintenance of the Confictura RunUO shard. This includes, but is not limited to:

* Creating new C# scripts for items, mobiles (monsters), and game systems.

* Refactoring and optimizing existing code for clarity and performance.

* Analyzing the codebase to answer questions or identify potential issues.

* Assisting with any other C#-based coding tasks related to the RunUO environment.

## Environment Setup

* **Framework:** .NET Framework 4.8

* **IDE:** The project is developed using Visual Studio Community 2022.

* **Core Dependencies:** All custom scripts located in the `Data/Scripts/` directory depend on the core engine files located in `System/Source/`. Keep this relationship in mind when analyzing or modifying code.

* **Line Endings:** Use Windows-style (CRLF) line endings to remain compatible with the server environment.

## Coding Conventions

* **Style Guide:** Adhere to standard C# naming and style conventions.

* **Naming:** All new C# files and classes must use PascalCase (e.g., `MyNewItem.cs`).

* **Namespaces:** All new custom scripts must belong to the `Server.Custom.Confictura` namespace.

* **File Location:** Place all new scripts within the `Data/Scripts/Custom/` directory. Organize them into logical subdirectories based on their function (e.g., `Items/`, `Mobiles/`, `Systems/`).

* **Comments:** Your code must be well-commented. Include comments to explain the purpose of classes, methods, and any complex logic to ensure clarity for other developers.

* **Searching:** When searching the codebase, prefer the `rg` (ripgrep) tool over `grep -R` or similar commands for better performance.

## Testing & Verification

* **Test Command:** None.

* **CRITICAL NOTE:** The agent's operating environment (Ubuntu-based) is incompatible with the project's Windows-based execution environment. **The agent cannot compile or run the server to test its changes.** All code modifications must be syntactically correct and logically sound based on a static analysis of the existing codebase.

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

  * **Files:** `【F:<file_path>†L<line_start>(-L<line_end>)?】`

  * **Terminal Output:** `【<chunk_id>†L<line_start>(-L<line_end>)?】`

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
