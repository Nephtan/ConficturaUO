# Wiki Audit Automation Prompt

Use this prompt for the daily wiki audit automation.

## Purpose

Find structural and documentation-integrity problems in `docs/wiki` and record them in the wiki automation files. This run is an audit pass only. Do not fix wiki pages, do not stage files, and do not commit.

## Schedule

- Intended cadence: daily.
- Preferred time: 02:10 America/Chicago.
- If this prompt is run manually at a different time, record the actual time in `wikihandoff.md`.

## Files You May Edit

Only edit these files:

- `docs/wiki/wikibacklog.md`
- `docs/wiki/wikihandoff.md`
- `docs/wiki/wikiautomationmemory.md`

Do not edit `docs/wiki/INDEX.md`, individual wiki pages, `docs/SystemAudit.md`, or source files during an audit run. If those files need changes, add or update backlog items instead.

## Safety Rules

1. Run `git status --short` first.
2. If unrelated non-wiki files are dirty, stop without editing and record the blocker in `wikihandoff.md` only if it is already dirty for wiki automation work.
3. If only wiki automation files are dirty, continue carefully and summarize the pre-existing dirty state in `wikihandoff.md`.
4. Never run `git add`, `git commit`, `git reset`, `git checkout --`, or destructive cleanup commands.
5. Prefer `rg` for searches.
6. Keep line endings LF.

## Audit Checks

Run or reproduce these checks:

- Wiki index coverage: every `docs/wiki/*.md` file except `INDEX.md` is indexed or listed as an intentional exception in `wikiautomationmemory.md`.
- Local links: every local Markdown link target resolves from the source file directory, including case-sensitive filename checks.
- Headings: every page has exactly one H1 and no unintended duplicate H1 title.
- Citation health: no Unicode replacement characters; citation markers that appear in wiki pages use a real file path, dagger separator, numeric line start, and optional numeric line end.
- SystemAudit alignment: every documentation target in `docs/SystemAudit.md` exists; Missing or Stub rows have backlog items; duplicate documentation links have backlog items unless memory marks them intentional.
- Metadata hygiene: legacy slug pages point to canonical pages; stale target-acquisition notes and generated-looking status text are backlog candidates.

## Backlog Rules

Add new items to `wikibacklog.md` with stable IDs using the next available `WIKI-####` number. Do not renumber existing items.

Statuses:

- `Open`: confirmed issue that can be worked.
- `Ready`: scoped issue with enough detail for the fixer automation.
- `In Progress`: reserved for a fixer run currently working the item.
- `Blocked`: cannot be fixed without missing context or a conflicting dirty tree.
- `Fixed`: fixed by automation but not necessarily committed.
- `Committed`: fixed and included in a human commit.
- `Intentional`: confirmed non-issue or accepted duplicate.

Use concise evidence. Include file paths, row names, and command findings, but do not paste large command outputs.

## Handoff Rules

Rewrite `wikihandoff.md` each run with:

- run timestamp and run type,
- dirty-worktree summary,
- checks performed,
- new or updated backlog IDs,
- files changed by the audit run,
- recommended next action for the fixer automation,
- any blockers.

## Memory Rules

Update `wikiautomationmemory.md` only for stable facts:

- intentional exceptions,
- false positives,
- canonical/legacy slug mapping,
- last issued backlog ID,
- last audit timestamp,
- last fixer timestamp if discovered,
- run lock state.

Do not store one-off command output in memory.

## Final Response

Report:

- whether the audit completed,
- backlog IDs added or changed,
- files edited,
- verification checks run,
- whether the worktree is intentionally dirty.

Do not stage or commit.
