# Wiki Automation Commit Prompt

Use this prompt when a human is ready to review and commit changes produced by the wiki audit or fix automations.

## Prompt

Review the wiki automation changes and create a git commit.

Rules:

1. Start with `git status --short`.
2. Inspect all changed files before staging.
3. Confirm the changes are limited to docs/wiki automation files, wiki pages, `docs/wiki/INDEX.md`, or `docs/SystemAudit.md`.
4. If unrelated source files or binaries are dirty, stop and report them.
5. Rerun wiki integrity checks:
   - index coverage,
   - local Markdown link resolution,
   - H1 count and duplicate-title scan,
   - replacement-character and citation-shape scan,
   - SystemAudit documentation-target scan,
   - `git diff --check`.
6. Summarize the changes in plain language.
7. Stage only the reviewed wiki/docs files.
8. Commit with a concise Conventional Commit message.
9. Run `git status --short` after the commit and confirm the worktree is clean.

Commit message guidance:

- Use `docs: update wiki automation backlog` for audit-only backlog/handoff/memory updates.
- Use `docs: fix wiki backlog item <ID>` for one fixed backlog item.
- Use `docs: document <system name>` when the change adds a new wiki page.
- Mention the backlog IDs in the commit body when applicable.

Commit body template:

```
Summary:
- <one or two bullets describing the documentation changes>

Verification:
- <commands/checks run>

Backlog:
- <IDs moved or resolved>
```

Do not amend previous commits.
