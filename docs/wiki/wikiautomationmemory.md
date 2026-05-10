# Wiki Automation Memory

This file stores stable state for the wiki audit and fix automation loop. Keep it concise and durable.

## Schedule

- Audit automation: daily at 02:10 America/Chicago.
- Fix automation: hourly at minute 01.
- Human commit review: manual, after one or more automation runs leave reviewed wiki changes.

## Run Locks

- Active lock: none.
- Last audit run: 2026-05-10 18:32:30 -05:00 America/Chicago; manual audit run found no new backlog items; WIKI-0015 remains Ready.
- Last fix run: 2026-05-10 18:06:58 -05:00 America/Chicago; WIKI-0004 fixed by manual automation run.
- Last setup run: 2026-05-10.

## Backlog State

- Last issued ID: WIKI-0016.
- Next ID: WIKI-0017.
- Initial backlog source: SystemAudit Missing/Stub rows and duplicate documentation-link groups.
- Last fixed ID: WIKI-0004; Obsolete Script Collection documented and linked from the technical reference index.

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
- `NPC: Banker Speech Commands` and `Mechanic: Banking Speech Commands` intentionally share `docs/wiki/Banker_Speech_Commands.md`; the mechanic row is a low-priority audit alias for the same banker speech system.
- `NPC: Real Estate Broker Appraisal` and `Real Estate Broker Deed Appraisal` intentionally share `docs/wiki/Real_Estate_Broker_Appraisal.md`; the deed row is a low-priority audit alias for the same broker appraisal/buyback flow.

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
- Treat source-behavior mentions of TODO, stub, placeholder, or generated text in wiki pages as documentation content unless the wording is automation provenance or generated status metadata.
- Do not treat `Creature_AI_Core.md`'s `Target Acquisition` heading or discussion as stale audit provenance.

## Verification Baseline

At setup time, these checks were clean:

- index coverage for non-index wiki pages,
- local Markdown link resolution,
- H1 count and duplicate-title scan,
- replacement-character and malformed citation scan.
