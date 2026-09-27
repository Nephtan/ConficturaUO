# Verification record

[Start here](README.md) · [Coverage](coverage-and-evidence.md)

## Scope and baseline

- Request: analyze and document the game's systems from the ground up to prepare for rebalancing; preserve a mix of lasting power, mastery, and discovery.
- Baseline: `main` at `6568058a` on September 27, 2026.
- Initial `git status --short`: clean. There were no pre-existing changes to preserve or overlapping edits to resolve.
- Root `AGENTS.md` and `docs/codebase-audit/AGENTS.md` were read. Parallel review agents were limited to analysis outputs under the root instruction's read-only review policy.
- Scope is supplemental gameplay documentation. Historical phase completion and repair approvals were not changed. No source/configuration/project/save/spawn files were edited.

## Artifacts and evidence

The new guide has an overview, a system map, three detailed domain chapters, world/access analysis, interactions, a research plan, and an explicit coverage statement. Dated review records preserve source evidence. The source census records SHA-256 per C# file; additional tables index skills, settings, regions, spell registrations, and race templates.

`build_inventory.py` generates six census outputs. `publish_chapters.py` publishes the three review chapters with correct relative links. Both support `--check` for comparison without writes. `verify_docs.py` checks local Markdown links, line-range bounds, explicit source paths, JSON entries and count consistency. These checks establish document consistency; they do not establish that every line was semantically audited.

## Checks

| Check | Result / meaning |
| --- | --- |
| Current source traces | Main implementations, relevant callers, settings and meaningful alternate paths inspected; review depth is documented per domain. |
| Inventory reproducibility | Six outputs reproduce exactly with `build_inventory.py --check`. |
| Chapter reproducibility | Three chapters reproduce exactly with `publish_chapters.py --check`. |
| Local links and source references | `verify_docs.py` passed: 13 Markdown documents, 311 local links, 1,257 source line ranges, 106 distinct fully qualified backtick source paths, and index count reconciliation. |
| Numeric examples | Derived from cited arithmetic, including skill probabilities, character rating, hit/swing/damage examples, Luck, enhancement cost, city upkeep and champion geometry. They are not live observations. |
| Spell index | 346 registrations, with 297 inside registry capacity and 49 outside; direct-construction paths are separately explained. |
| Race/skill index | 172 encoded race rows and 58 skill slots; encoded variants do not imply distinct live player choices. |
| Interactive overview | Local browser preview inspected; selecting Character level updates the explanation and selected state. The overview summarizes 24 system families and does not simulate gameplay. |
| Whitespace and scope | `git diff --check`; staged paths reviewed to remain documentation/data/documentation tooling only. |
| Source build | Not run: no executable source changed. |
| Forced startup script compile | Not run: no script or startup changes; a server start would not prove gameplay balance. |
| IDE/project build | Not run: no project file changed. |
| Live gameplay/economy | Unverified: no deployed server observation, player telemetry, market sample, representative save analysis, or gameplay experiment in this task. |

## Reproduction

Run from `D:\ConficturaUO-main`:

```powershell
python docs/game-balance/tools/build_inventory.py --check
python docs/game-balance/tools/publish_chapters.py --check
python docs/game-balance/tools/verify_docs.py
git diff --check
```

The tools need Python 3 and `rg` on PATH. The inventory reads the existing checkout and writes only `docs/game-balance/data`; the publisher writes only its three guide chapters. Check modes do not write outputs. Document comparisons accept Git's LF/CRLF checkout conversion. Source SHA-256 values identify the actual source bytes read, so a source newline conversion changes those hashes and requires a reviewed inventory refresh. No additional package, game build, database, or running server is required.

## Remaining boundary

This is a source-grounded explanation and an investigation baseline. Individual effects and content variants, deployed availability, actual progression time, economic dominance, and player experience remain at the depths stated in [coverage](coverage-and-evidence.md). Those remaining questions are not silently marked as complete by the census or document checks.
