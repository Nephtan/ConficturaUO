# AGENTS.md (Codex-Only)

> **Purpose**: This file is the **single source of truth** for how OpenAI **Codex** must operate in this repository. It encodes setup constraints, run/test realities, modification protocols, and inheritance rules. **No persona** rules—functional operation only.

---

## 0. High-Signal Summary (Read First)

* **Runtime/Stack**: C# / .NET Framework **4.8** (Windows-only server runtime).
* **Editor**: Visual Studio 2022 (Community).
* **Project Domain**: **RunUO**-based Confictura shard; custom scripts under `Data/Scripts/` depend on core engine in `System/Source/`.
* **Testing in Agent Env**: **Not possible** (agent runs on Ubuntu). Perform **static analysis only**; changes must be syntactically correct and logically sound.
* **Search Tool**: Prefer `rg` (ripgrep) for code search.
* **Inheritance**: Root `AGENTS.md` is **base**. Subdirectory `AGENTS.md` files **extend** (inherit) root rules.

---

## 1. Setup & Environment (Idempotent)

Codex MUST assume **no Windows build or execution** is possible in its environment.

* **Agent (Linux) Reality**

  * Use repository search + static analysis only (no compile/run).
  * Prefer `rg` (ripgrep) for code search.
  * Line endings: **LF**.
  * Respect dependency flow: `Data/Scripts/*` ? `System/Source/*`.

> If a change implies a Windows build or runtime validation, Codex MUST scope work to **static diffs** and provide the **Windows operator** with the commands in §2.

---

## 2. Run, Lint, Format, Test

> The server cannot be compiled/executed in the agent environment. Perform **static checks only**.

### 2.1 Agent (Linux) Actions

* **Build**: *Not executable in agent env*
* **Unit Tests**: *None available*
* **Lint/Format**: Enforce STYLEGUIDE manually in diffs; prefer idiomatic C#.

> Codex cannot execute builds here. For human/local build & run steps, see **OPERATOR.md**.

---

## 3. Coding Conventions & File Layout

* **C# Style**: Standard C# conventions; **PascalCase** for classes/files.
* **Namespaces**: Place new custom scripts under `Server.Custom.Confictura`.
* **File Placement**: New scripts go in `Data/Scripts/Custom/` with logical subfolders (e.g., `Items/`, `Mobiles/`, `Systems/`).
* **Core Dependency Rule**: Scripts in `Data/Scripts/` depend on `System/Source/`—maintain compatibility.
* **Comments**: Provide class/method purpose and explain complex logic.
* **Search**: Prefer `rg` over `grep -R` for performance & fidelity.

---

## 4. Repository Rules (Git & Version Control)

When a task requires file changes, Codex MUST:

1. **Work on the current main branch**; do **not** create branches.
2. **Commit all changes**; uncommitted work is not evaluated.
3. **Fix pre-commit failures** and retry until clean.
4. **Maintain a clean worktree** (`git status` clean when done).
5. **Do not amend history**.
6. Use **brief, descriptive commit messages**; Conventional Commits encouraged.

---

## 5. Agent Behavior Contract (Deterministic, No Persona)

Codex MUST:

* **Summarize intent** of the change before proposing edits.
* Prefer **minimal, safe diffs**; limit blast radius.
* **Ask a clarifying question only** when ambiguity blocks correctness; otherwise proceed conservatively.
* **Never fabricate runtime results**; if a runtime proof is required, provide a Windows operator checklist.

Codex MUST NOT:

* Add new dependencies without rationale and file edits documenting why/where.
* Modify protected/generated/vendor areas (if present) without explicit instruction.

---

## 6. Task Playbooks (Concrete Procedures)

### A) Add Feature `<X>`

1. Place files under `Data/Scripts/Custom/<Area>/...` per layout.
2. Use namespace `Server.Custom.Confictura`; document public APIs.
3. Include targeted defensive checks; provide example usage in comments.
4. Validate via static analysis; note Windows validation steps for operator.
5. Commit with `feat: <scope> <summary>`.

### B) Refactor `<Module>`

1. Preserve public API shape unless the task requires changes.
2. Improve readability/perf; add comments for non-obvious logic.
3. Re-scan for references with `rg` and adjust call sites if needed.
4. Perform static sanity checks; list potential runtime risks.
5. Commit with `refactor: <scope> <summary>`.

---

## 7. Testing & Verification Reality

* **No test command** available; rely on **static analysis**.
* The agent environment is **Ubuntu**; the project is **Windows-only** to run. Provide operator instructions where runtime proof is necessary.

---

## 8. Citations in Final Responses (if applicable)

If Codex references repository files in its output, include explicit file paths and 1-indexed line ranges, e.g. `Data/Scripts/Custom/Items/Foo.cs:L12-L34`. Avoid special markup; use plain text so humans can follow and verify.

---

## 9. Interpretation, Scope & Inheritance

* **Scope**: This file applies to the entire directory tree beneath its location.
* **Precedence/Inheritance** (this repo’s policy):

  * **User prompt** > AGENTS.md (always).
  * **Subdirectory AGENTS.md** files **extend** this root (inheritance). Where conflicts exist, the **more specific (deeper) file’s explicit rule wins**.
* **Compliance**: For every modified file, obey all AGENTS.md files whose scope includes that path.

---

## 10. Continuous Improvement

This is a **living document**. If Codex detects conflicts, errors, or missing instructions while completing a task, it MUST propose an update (exact text to add/remove/change) as part of its final output.

---

### Appendix: Project Snapshot (Stable Facts)

* **Primary Objective**: Assist development/maintenance of Confictura RunUO shard: new items/mobiles/systems, refactors, analysis, and C# tasks.
* **Key Structure**: `Data/Scripts/` (custom) ? `System/Source/` (engine).
* **Style**: Standard C#; PascalCase; document complex logic.
