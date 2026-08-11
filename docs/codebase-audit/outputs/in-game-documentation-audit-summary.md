# In-Game Player Documentation Audit Summary

Audit target: current main runtime/data snapshot, last changed at 6b447099977f81d66585e8401e3ef904be7da3d9

Historical synchronization point: cbd03db1f1be73c2fa345ac6b4ee5abfa63ae840 (2023-10-25), the last comprehensive informational-gump and talk-text revision

## Outcome

The current player-facing feature set has been reconciled into 297 audit rows and 136 actionable incorporation, refresh, or runtime-evidence items. External wiki pages were treated only as candidate and source-trace evidence; they did not satisfy in-game coverage.

The audit scanned 6718 current runtime/source .cs files into 167 system boundaries, reconciled 114 canonical wiki topics, found 160 direct AccessLevel.Player command registrations, parsed 92 relevant checked-in XML files including 58 lore records, validated 29 static Library entry declarations, and mapped 4002 current paths touched after the baseline. Another 320 post-baseline paths no longer resolve in the current tree and are retained as historical drift evidence rather than current capability claims.

No live-world availability is claimed. The checkout has checked-in spawn, decoration, configuration, and lore data but no world save; rows that depend on actual placement, vendor stock, existing player unlocks, or server configuration remain RuntimeUnknown or require owner staging evidence.

## Census by row kind

| Row kind | Count |
| --- | ---: |
| CanonicalTopic | 114 |
| Capability | 15 |
| HistoricalDrift | 1 |
| SystemBoundary | 167 |

## Coverage

| Coverage | Count |
| --- | ---: |
| Complete | 15 |
| IntentionalDiscovery | 94 |
| Missing | 16 |
| NotApplicable | 35 |
| Partial | 121 |
| RuntimeUnknown | 6 |
| Stale | 10 |

## Decisions

| Decision | Count |
| --- | ---: |
| Incorporate | 121 |
| KeepAsIs | 55 |
| NeedsRuntimeEvidence | 6 |
| NoDocumentationNeeded | 85 |
| RefreshExisting | 30 |

## Action backlog by priority

| Priority | Count |
| --- | ---: |
| P1 | 11 |
| P2 | 34 |
| P3 | 91 |

## Highest-risk gaps

| Priority | Audit row | Capability | Decision | Surface |
| --- | --- | --- | --- | --- |
| P1 | IGD-DOC-docs-wiki-pvp-consent-system-md | PvP Consent System | Incorporate | Help |
| P1 | IGD-CAP-government-authority-economy | Player government founding, authority, elections, treasury, maintenance, conflict, and destructive actions | RefreshExisting | SystemHelpGump |
| P1 | IGD-SYS-custom-offline-skill-training | Current source boundary: Custom:Offline Skill Training | Incorporate | Help |
| P1 | IGD-CAP-character-level-service | Character Level persistent progression and player service commands | Incorporate | LibraryReference |
| P1 | IGD-SYS-custom-vhaerun-s-crl-homestead-system-2-0 | Current source boundary: Custom:Vhaerun's CRL Homestead System [2.0] | Incorporate | Help |
| P1 | IGD-CAP-direct-player-command-discovery | Complete direct Player command discovery and syntax | RefreshExisting | Help |
| P1 | IGD-CAP-onboarding-help-guide-library | New-player onboarding, Help, Guide to Adventure, and Personal Library routing | RefreshExisting | Help |
| P1 | IGD-CAP-town-crier-privacy-bridge | Town Crier private speech and external publication boundary | RefreshExisting | Help |
| P1 | IGD-CAP-auto-loop-taming | Auto-loop animal taming and explicit stop controls | Incorporate | Help |
| P1 | IGD-CAP-crafting-queue-batch-containers-search | Crafting search, batch amount, queue, source/destination containers, and cancellation | Incorporate | SystemHelpGump |
| P1 | IGD-CAP-auto-loop-harvesting | Auto-loop harvesting, captcha/failure behavior, and stop control | Incorporate | Help |
| P2 | IGD-DOC-docs-wiki-book-publisher-md | Book Publisher | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-bulk-order-system-md | Bulk Order System | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-farmable-crops-system-md | Farmable Crops System | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-flip-deed-md | Flip Deed | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-grave-robbing-trade-md | Grave Robbing Trade | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-harvest-resource-system-md | Harvest Resource System | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-homestead-system-md | Homestead System | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-librarian-trade-md | Librarian Trade | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-relic-items-md | Relic Items | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-runic-tools-crafting-md | Runic Tools Crafting | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-shoppes-vendors-md | Shoppes Vendors | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-stonecrafting-md | Stonecrafting | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-taxidermy-md | Taxidermy | Incorporate | LibraryReference |
| P2 | IGD-DOC-docs-wiki-technology-items-md | Technology Items | Incorporate | LibraryReference |

The most urgent lane is authoritative information that can prevent persistent loss or confusion: onboarding and Guide acquisition, the comprehensive Player command list, government authority and treasury rules, persistent character progression, PvP consent, offline training, homestead/property decisions, crafting queues and batch consumption, and auto-loop start/stop controls. Those topics belong in direct Help or a system help gump before flavor expansion.

## Historical drift

The 2023-10-25 synchronization point remains useful because it marks the last broad Help/talk rewrite, but it is not the census boundary. Current system rows cover every current source bucket; their PostBaselineChanges fields identify later enhancements to both new and old systems. The explicit historical-drift row records changed paths that were deleted, renamed, reverted, generated, or otherwise absent at current HEAD so removed work is not advertised.

Notable current drift reviewed separately includes auto-loop taming and harvesting, crafting search/queue/batch/container controls, Character Level service commands, Random Encounters, Monster Nests, dynamic lore/library unlocks, dungeon difficulty help, race/creature-character options, government rules, Town Crier privacy behavior, and boss-taming restrictions. Internal AI changes, staff tooling, pure defect repairs, disabled Test Center/turn-combat configuration, and ordinary content variants are explicit exclusions unless they expose a separate player control or risk.

## Recommendations by surface

- **Help:** refresh onboarding, Guide acquisition, the direct Player command list, persistent settings, privacy behavior, automated-action stop controls, and disabled-state messaging.
- **LibraryReference:** add concise durable references for substantial progression, crafting/harvesting, combat schools, Searching/Hiding/Stealth, and other mechanics that are learnable but hard to reconstruct.
- **SystemHelpGump:** update government first; retain apiculture and gardening as the model for point-of-use help. Put authority, cost, persistence, and destructive rules before secondary detail.
- **LoreBook and NPCSpeech:** preserve discovery for encounters, nests, rare artifacts, and world systems. Teach clue paths and danger without exact locations, solutions, or reward tables.
- **QuestJournal and ContextualFeedback:** close state-specific gaps: explain the next clue, why an action failed, whether resources were consumed, and how to stop or recover.

## Owner-run verification checklist

1. Use a fresh ordinary account and character; open Help and traverse every page, button, command example, Library link, and support path.
2. Verify how a new player actually acquires the Guide to Adventure and whether the greeter, sage, librarian, or creation flow matches the text.
3. Run every direct Player command from the audit, including invalid syntax, prerequisites, persistent state, and stop or undo behavior.
4. Open the Personal Library before and after representative lore discoveries; confirm built-in entries, unlock IDs, duplicate prevention, persistence across save/restart, and inaccessible-entry behavior.
5. Confirm physical guide, learning, and lore book vendor stock or world placement; record facets, NPC types, item IDs/types, and acquisition requirements without using staff-only reachability as proof.
6. Exercise government founding, services, treasury, maintenance, elections, authority, bans, wars/alliances, and destructive actions on disposable staging data.
7. Exercise crafting search, batch amount, queue, source/destination container, house-container, cancel, failure, and resource-consumption paths with inexpensive materials first.
8. Exercise auto-loop harvesting and taming start, interruption, captcha/failure, logout/death/range behavior, and explicit stop commands.
9. Verify discovery systems through ordinary clues and contextual feedback; confirm the audit does not require exposing locations, passwords, solutions, or rewards.
10. Confirm disabled systems remain disabled and do not advertise unavailable player controls. Reclassify any configuration- or placement-dependent row with captured staging evidence.

## Reproduction

Generate the four artifacts:

    & ./docs/codebase-audit/tools/New-InGamePlayerDocumentationAudit.ps1

Validate the committed artifacts byte-for-byte without writing:

    & ./docs/codebase-audit/tools/New-InGamePlayerDocumentationAudit.ps1 -ValidateOnly

Validation fails on duplicate IDs, invalid controlled values, missing evidence paths, duplicate lore/library IDs, unreconciled system buckets, canonical topics, Player commands, current post-baseline paths, or output drift.
