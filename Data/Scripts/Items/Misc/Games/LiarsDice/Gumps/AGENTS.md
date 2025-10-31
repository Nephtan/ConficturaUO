# AGENTS.md (Codex-Specific / Gumps)


    **Purpose**: This file **extends** the root `AGENTS.md` and is the **single source of truth** for how **Codex** must operate when developing **GUMP (Graphical User Menu Popup)** scripts. It encodes constraints specific to GUMP design, state management, and interaction handling.


## 0. High-Signal Summary (Read First)



* **Inheritance**: This file **extends** the root `AGENTS.md`. All root rules apply unless explicitly overridden here.
* **Runtime/Stack**: C# / .NET Framework **4.8** (Windows-only server runtime).
* **Editor**: Visual Studio 2022 (Community).
* **Project Domain**: **RunUO**-based Confictura shard.
* **Project Sub-Domain**: C# GUMP (`Server.Gumps.Gump`) development. Focus on stateless, server-side packet generation and `OnResponse` logic.
* **Testing in Agent Env**: **Not possible**. Static analysis only. Codex **cannot** visually verify GUMP layouts.
* **Search Tool**: Prefer `rg` (ripgrep) for code search.


## 1. Setup & Environment (Idempotent)

(Inherited from root `AGENTS.md`. No changes.)


## 2. Run, Lint, Format, Test

(Inherited from root `AGENTS.md`. No changes.)


## 3. Coding Conventions & File Layout



* **C# Style**: Standard C# conventions; **PascalCase** for classes/files (e.g., `MyNewGump.cs`).
* **Namespaces**: Place new custom scripts under `Server.Custom.Confictura`.
* **File Placement**: New GUMP scripts **MUST be placed in <code>Data/Scripts/Custom/Gumps/</code>**. Other custom scripts go in logical subfolders under `Data/Scripts/Custom/`.
* **Core Dependency Rule**: Scripts in `Data/Scripts/` depend on `System/Source/`—maintain compatibility.
* **Comments**: Provide class/method purpose and explain complex `OnResponse` logic or button ID encoding.
* **Search**: Prefer `rg` over `grep -R` for performance & fidelity.


### 3.1 GUMP-Specific Conventions (MANDATORY)

These rules are critical due to the stateless, packet-based nature of the RunUO GUMP system.



1. **Inheritance**: All GUMPs MUST inherit from `Server.Gumps.Gump`.
2. **Statelessness**: GUMPs are stateless packet generators. **Do NOT store persistent state in GUMP class member variables.** State (e.g., the item being viewed, the current page) MUST be passed into the GUMP's constructor or managed via a separate, external context object.
3. **Constructor**: The constructor builds the UI.
    * It MUST call `from.CloseGump(typeof(YourGumpClass));` (where `YourGumpClass` is the class name) near the beginning to prevent UI stacking.
    * It MUST call `AddPage(0);` to establish the base layer.
4. **Response Handling**: `OnResponse` handles all user input.
    * It MUST gracefully handle `info.ButtonID &lt;= 0`. This branch (cancellation) MUST be present in all GUMPs.
5. **Button IDs**:
    * Use non-zero, unique `buttonID`s for all `GumpButtonType.Reply` buttons.
    * For clarity, use an `enum` or `const int`s for Button IDs.
    * For complex lists, use an encoding scheme (e.g., `buttonID = 1000 + index`) and document it.
6. **Pagination**: For lists too long for one screen, **MUST** use the "Offset/Index Method" (passing a `int page` index into the constructor).
    * Do **NOT** use `GumpButtonType.Page`. This method is incompatible with a stateless refresh model.
    * Use LINQ `.Skip()` and `.Take()` in the constructor to display the correct "slice" of data.
    * "Next" / "Previous" buttons MUST be `GumpButtonType.Reply` and re-send the GUMP with an adjusted page index: `from.SendGump(new YourGumpClass(from, ...state..., newPage));`.
7. **Localization**: Prefer `AddHtmlLocalized` (with CliLoc ID) over `AddLabel` or `AddHtml` (with string literals) for all static UI text.
8. **Advanced Patterns**: For complex GUMPs (multiple views, deep state), Codex MUST propose refactoring using a **Builder** (to clean the constructor) or **State** (to manage different views) design pattern.


## 4. Repository Rules (Git & Version Control)

(Inherited from root `AGENTS.md`. No changes.)


## 5. Agent Behavior Contract (Deterministic, No Persona)

Codex MUST:



* **Summarize intent** of the change before proposing edits.
* Prefer **minimal, safe diffs**; limit blast radius.
* **Ask a clarifying question only** when ambiguity blocks correctness; otherwise proceed conservatively.
* **Never fabricate runtime results**; if a runtime proof is required, provide a Windows operator checklist.

Codex MUST NOT:



* Add new dependencies without rationale and file edits documenting why/where.


## 6. Task Playbooks (Concrete Procedures)


### A) Add General Feature `&lt;X>`

(Inherited from root `AGENTS.md`.)


### B) Refactor `&lt;Module>`

(Inherited from root `AGENTS.md`.)


### C) Add or Modify GUMP `&lt;GumpName>`



1. **Analyze State**: Identify all required state (e.g., `Mobile from`, target item/mobile, page index, selected category). Ensure this state is passed into the GUMP's constructor.
2. **File Placement**: Place new file in `Data/Scripts/Custom/Gumps/GumpName.cs`.
3. **Constructor**:
    * Implement `public GumpName(Mobile from, ...other_state...) : base(x, y)`.
    * Call `from.CloseGump(typeof(GumpName));` (Rule 3.1.3).
    * Call `AddPage(0);` (Rule 3.1.3).
    * Build the static layout (`AddBackground`, `AddImageTiled`, `AddHtmlLocalized` for headers).
    * Build the dynamic content (iterating lists, calculating `y` offsets, adding `AddButton` with encoded IDs).
4. **Response Handler (<code>OnResponse</code>)**:
    * Get the `Mobile from = sender.Mobile;`.
    * Implement a `switch (info.ButtonID)` statement.
    * **Case 0**: Handle cancellation (must be present).
    * **Other Cases**: Handle each `GumpButtonType.Reply`. Decode button IDs if necessary.
    * **State Transition**: Any action that refreshes the GUMP MUST call `from.SendGump(new GumpName(from, ...next_state...));` (Rule 3.1.2 / 3.1.6).
5. **Static Analysis**: Verify all `Add...` calls have correct (plausible) coordinates and art/Gump IDs. Ensure all `OnResponse` logic paths are covered.
6. **Commit**: Use `feat: gump &lt;summary>` or `fix: gump &lt;summary>`.


## 7. Testing & Verification Reality



* **No test command** available; rely on **static analysis**.
* The agent environment is **Ubuntu**; the project is **Windows-only** to run.
* **GUMP-Specific Reality**: For GUMP development, this means Codex **cannot visually verify layouts**. Verification MUST be done by static analysis of coordinates, `Add...` method parameters, and robustly checking all logical branches within `OnResponse`.


## 8. Citations in Final Responses (if applicable)

If Codex references repository files in its output, include explicit file paths and 1-indexed line ranges, e.g. `Data/Scripts/Custom/Gumps/MyGump.cs:L12-L34`. Avoid special markup; use plain text so humans can follow and verify.


## 9. Interpretation, Scope & Inheritance



* **Scope**: This file applies to the entire directory tree beneath its location (`Data/Scripts/Custom/Gumps/`).
* **Precedence/Inheritance**:
    * **User prompt** > AGENTS.md (always).
    * **This file** (`Gumps/AGENTS.md`) **overrides** the root `AGENTS.md` on GUMP-specific matters.
* **Compliance**: For every modified file, obey all AGENTS.md files whose scope includes that path.


## 10. Continuous Improvement

(Inherited from root `AGENTS.md`. Codex MUST propose updates if rules are conflicting or missing.)


### Appendix: Project Snapshot (Stable Facts)



* **Primary Objective**: Assist development/maintenance of Confictura RunUO shard: new items/mobiles/systems, refactors, analysis, and C# tasks, **especially the creation and maintenance of <code>Server.Gumps.Gump</code> UIs.**
* **Key Structure**: `Data/Scripts/` (custom) ? `System/Source/` (engine).
* **Style**: Standard C#; PascalCase; document complex logic.