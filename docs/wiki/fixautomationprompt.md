# Wiki Fix Automation Prompt

Use this prompt for the hourly wiki fixer automation.

## Purpose

Pick one actionable item from `wikibacklog.md`, fix it, verify the fix, and update the automation files. This run may edit wiki documentation, but it must not stage or commit.

## Schedule

- Intended cadence: hourly.
- Preferred minute: 40.
- If this prompt is run manually at a different time, record the actual time in `wikihandoff.md`.

## Allowed Edit Scope

You may edit:

- the wiki page or documentation file needed for the selected backlog item,
- `docs/wiki/wikibacklog.md`,
- `docs/wiki/wikihandoff.md`,
- `docs/wiki/wikiautomationmemory.md`.

Do not edit C# source files, project files, generated binaries, or unrelated documentation.

## Safety Rules

1. Run `git status --short` first.
2. If unrelated non-wiki files are dirty, stop without editing.
3. If wiki files are dirty, inspect them and work with them. Do not overwrite another run's changes.
4. Check `wikiautomationmemory.md` for a fresh lock. If another fixer or audit run appears active, stop and update handoff only if safe.
5. Never run `git add`, `git commit`, `git reset`, `git checkout --`, or destructive cleanup commands.
6. Fix exactly one backlog item.
7. Keep line endings LF.

## Item Selection

Choose the first backlog item, in file order, that meets all criteria:

- status is `Ready` or `Open`,
- priority is `P0`, `P1`, or `P2` before `P3`,
- it is not marked blocked by memory,
- it can be fixed within the allowed edit scope.

If an `Open` item is too vague, convert it to `Blocked` with a clear reason instead of guessing.

## Fix Rules

- Prefer mechanical fixes over broad rewrites.
- Preserve canonical pages and intentional legacy slug pages.
- When creating a new wiki page from a Missing or Stub SystemAudit row, inspect the relevant source files before making source-backed claims.
- Update `INDEX.md` and `docs/SystemAudit.md` only when the selected backlog item explicitly requires coverage metadata changes.
- Mark the backlog item `Fixed` only after verification passes.
- If verification fails, leave the best useful notes in `wikihandoff.md` and mark the item `Blocked` or keep it `In Progress` only if a human must resume immediately.

## Verification

After the fix, run the relevant checks:

- local Markdown link resolution for `docs/wiki`,
- H1 count and duplicate title scan,
- citation/replacement-character scan,
- SystemAudit documentation-target scan,
- `git diff --check`.

Run broader checks if the change touches `INDEX.md`, `SystemAudit.md`, or multiple wiki pages.

## Handoff Rules

Rewrite `wikihandoff.md` with:

- selected backlog ID,
- files changed,
- summary of fix,
- verification commands and results,
- remaining risk or blocker,
- whether the worktree is intentionally dirty for human review.

## Final Response

Report:

- backlog ID selected,
- files edited,
- verification results,
- remaining dirty files,
- reminder that the run did not stage or commit.
