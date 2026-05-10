# Wiki Automation Memory

This file stores stable state for the wiki audit and fix automation loop. Keep it concise and durable.

## Schedule

- Audit automation: daily at 02:10 America/Chicago.
- Fix automation: hourly at minute 40.
- Human commit review: manual, after one or more automation runs leave reviewed wiki changes.

## Run Locks

- Active lock: none.
- Last audit run: not run by scheduled automation yet.
- Last fix run: not run by scheduled automation yet.
- Last setup run: 2026-05-10.

## Backlog State

- Last issued ID: WIKI-0014.
- Next ID: WIKI-0015.
- Initial backlog source: SystemAudit Missing/Stub rows and duplicate documentation-link groups.

## Intentional Exceptions

- `docs/wiki/Confictura_Introduction.md` is an orientation page and is intentionally not linked from `docs/SystemAudit.md`.
- `docs/wiki/pvp-consent-system.md` is a preserved legacy slug for `docs/wiki/PvP_Consent_System.md`.
- `docs/wiki/voting-system.md` is a preserved legacy slug for `docs/wiki/Voting_Rewards.md`.
- `docs/wiki/auditautomationprompt.md` is an automation support file and is intentionally not listed in the player-facing systems index.
- `docs/wiki/fixautomationprompt.md` is an automation support file and is intentionally not listed in the player-facing systems index.
- `docs/wiki/wikibacklog.md` is an automation support file and is intentionally not listed in the player-facing systems index.
- `docs/wiki/wikihandoff.md` is an automation support file and is intentionally not listed in the player-facing systems index.
- `docs/wiki/wikiautomationmemory.md` is an automation support file and is intentionally not listed in the player-facing systems index.
- `docs/wiki/wikicommitprompt.md` is an automation support file and is intentionally not listed in the player-facing systems index.

## Canonical Slug Map

| Legacy Page | Canonical Page | Status |
| --- | --- | --- |
| `pvp-consent-system.md` | `PvP_Consent_System.md` | Intentional legacy slug. |
| `voting-system.md` | `Voting_Rewards.md` | Intentional legacy slug. |

## Stable Audit Rules

- Do not treat `INDEX.md` as needing to be indexed by itself.
- Do not treat intentional legacy slug pages as duplicate-topic failures if they point to canonical pages.
- Do not treat `Confictura_Introduction.md` as missing from SystemAudit.
- Treat duplicate SystemAudit documentation links as backlog candidates unless listed here as intentional.
- Treat Missing or Stub SystemAudit rows without documentation links as backlog candidates.

## Verification Baseline

At setup time, these checks were clean:

- index coverage for non-index wiki pages,
- local Markdown link resolution,
- H1 count and duplicate-title scan,
- replacement-character and malformed citation scan.
