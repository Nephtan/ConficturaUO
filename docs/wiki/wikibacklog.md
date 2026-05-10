# Wiki Backlog

This backlog is maintained by the wiki audit and fix automation loop. IDs are stable and must not be renumbered.

## Status Key

| Status | Meaning |
| --- | --- |
| Open | Confirmed issue that needs scoping or work. |
| Ready | Scoped issue that the fixer automation can pick up. |
| In Progress | A fixer run is actively working this item. |
| Blocked | Cannot proceed without human input or a clean worktree. |
| Fixed | Fixed by automation but not necessarily committed. |
| Committed | Fixed and included in a human commit. |
| Intentional | Confirmed non-issue or accepted duplicate. |

## Items

| ID | Status | Priority | Category | Files | Evidence | Recommended Fix | Last Seen | Fix Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| WIKI-0001 | Ready | P2 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Book Publisher` is `Stub` and has no wiki documentation link. | Inspect `Data/Scripts/Custom/Book Publisher [2.0]/Publisher.cs`, create or expand a wiki page, add it to `INDEX.md`, and update the SystemAudit row. | 2026-05-10 | Seeded by setup. |
| WIKI-0002 | Ready | P2 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `NPC Dialogue: Mage Advice` is `Missing` and has no wiki documentation link. | Inspect `Talk.cs`, `Mage.cs`, and related wand/toolbar files, then create a focused wiki page and update index/audit metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0003 | Ready | P3 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Static Gump Tool` is `Stub` and has no wiki documentation link. | Inspect `StaticGump(Alternative).cs`, create or expand a wiki page, and update index/audit metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0004 | Ready | P3 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Obsolete Script Collection` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/System/Obsolete/Obsolete.cs`, document whether this should be a wiki page or an intentional no-doc entry, and update metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0005 | Ready | P1 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Base Spell Framework` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/Magic/Base/Spell.cs`, create a technical reference wiki page, and update index/audit metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0006 | Ready | P1 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Magery Spell System` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/Magic/Magery/`, create a wiki page, and update index/audit metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0007 | Ready | P2 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Misc Magic Spells` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/Magic/Misc/`, create a wiki page, and update index/audit metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0008 | Ready | P2 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Technology Items` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/Items/Technology/`, create a wiki page, and update index/audit metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0009 | Ready | P2 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Relic Items` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/Items/Relics/`, create a wiki page, and update index/audit metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0010 | Ready | P3 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Explorer Camping Gear` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/Items/Explorers/`, create a wiki page or reconcile with existing ranger/sailing docs, and update metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0011 | Ready | P2 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Farmable Crops System` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/Items/Farming/`, create a wiki page or reconcile with existing homestead/gardening docs, and update metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0012 | Ready | P2 | Coverage | `docs/SystemAudit.md`; `docs/wiki/INDEX.md` | SystemAudit row `Goliath Monsters` is `Missing` and has no wiki documentation link. | Inspect `Data/Scripts/Mobiles/Goliaths/`, create a wiki page, and update index/audit metadata. | 2026-05-10 | Seeded by setup. |
| WIKI-0013 | Ready | P2 | Duplicate Inventory | `docs/SystemAudit.md`; `docs/wiki/Banker_Speech_Commands.md` | `Banker_Speech_Commands.md` is linked by both `NPC: Banker Speech Commands` and `Mechanic: Banking Speech Commands`. | Decide whether this duplicate SystemAudit row is intentional; if not, merge or mark one row as a duplicate with a clear note. | 2026-05-10 | Seeded by setup. |
| WIKI-0014 | Ready | P2 | Duplicate Inventory | `docs/SystemAudit.md`; `docs/wiki/Real_Estate_Broker_Appraisal.md` | `Real_Estate_Broker_Appraisal.md` is linked by both `NPC: Real Estate Broker Appraisal` and `Real Estate Broker Deed Appraisal`. | Decide whether this duplicate SystemAudit row is intentional; if not, merge or mark one row as a duplicate with a clear note. | 2026-05-10 | Seeded by setup. |

## Backlog Maintenance Notes

- Add new items at the bottom with the next ID from `wikiautomationmemory.md`.
- Do not delete fixed items; move them through `Fixed`, then `Committed`.
- Use `Intentional` for accepted legacy pages or accepted duplicate inventory rows.
