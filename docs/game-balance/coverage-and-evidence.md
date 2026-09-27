# Coverage and evidence

[Start here](README.md) · [Systems](systems-at-a-glance.md) · [Verification](VERIFICATION.md)

## What is complete in this package

This package provides a ground-up map of the game, detailed review of shared mechanics and major gameplay systems, explicit interactions, and a practical foundation for a rebalance. It includes a reproducible census of the stated C# source roots. It is **not an exhaustive per-effect balance certification of nearly two million lines of code**. The distinction matters: indexed content, reviewed rules, and measured player outcomes answer different questions.

### Source census at the study baseline

| Root | C# files |
| --- | ---: |
| `Data/Scripts/Custom` | 1,146 |
| `Data/Scripts/Items` | 3,029 |
| `Data/Scripts/Magic` | 574 |
| `Data/Scripts/Mobiles` | 1,102 |
| `Data/Scripts/Quests` | 117 |
| `Data/Scripts/System` | 471 |
| `Data/Scripts/Trades` | 159 |
| `Data/System/Source` | 120 |
| **Total** | **6,718** |

That is **6,598 scripts**, **120 engine files**, **1,896,644 physical source lines**, and **162 directory families**. A directory family is a navigation group, not a claim of an independent system. The census excludes generated `bin` and `obj` and follows `rg` visibility. It does not establish successful compilation, project inclusion, deployed state, or all client-side behavior.

Additional indexes cover **58 skill slots**, **421 region definitions across seven facets**, **195 region XML spawn objects**, and **61 whitelisted gameplay settings**. Settings are raw checked-in inputs: getters, callers, startup order, and saved state can change their effect.

## Review depth by domain

| Domain | Reviewed in detail | Indexed or sampled | What remains before declaring all aspects evaluated |
| --- | --- | --- | --- |
| Character foundations | Creation, raw/effective skills and stats, origin caps, gain, major cap rewards. | Starting-world variants and identity restrictions. | Every starting route and saved legacy character state. |
| Progression | Derived-level arithmetic and consumers; study, scrolls, elixirs, soulstones, reputation, loss paths. | All skill slots; training consumable families. | Every skill's activity rates and all acquisition sources. |
| Races | Template/scaling engine and refresh paths. | All 172 encoded rows, including visual variants. | Each selectable race's full effective kit, availability, interactions, and measured outcomes. |
| Weapons/defense | Shared hit, speed, damage, parry, resistance and leech pipelines. | All 55 registered weapon abilities; representative overrides. | Every weapon, ammunition, ability, proc, and target override combination. |
| Magic | Shared casting/cost/damage; school identities and exclusions; selected effects; registry boundaries. | All 346 registrations across 19 families mapped to definitions. | Every individual effect, access path, duration, target rule, resource cost, and combination. |
| Gear | Item XP, allocation, caps, repair; selected temporary upgrades; guild enhancement. | Artifacts, ordinary magic gear, runic/material families. | Full acquisition-to-effective-benefit catalog; actual legacy item distribution. |
| Pets | Taming/control/followers, bonding, damage rules, contribution links. | Creature templates, summons, mounts. | Every tameable/summoned creature's final stats, special behavior, upkeep and loss. |
| PvP/crime | Ordered harmful/beneficial rules, consent, key government/event/pet interactions. | Faction/duel/legacy systems, theft variants. | Full actor/target/region/event matrix, enabled modes, actual live outcomes. |
| Enemies | Shared creature scaling, AI selection, explicit balance profiles. | Named bosses, Goliaths, regional and encounter families. | Every live spawn/override and its complete danger/reward/time profile. |
| Rewards | Ordinary loot/Luck; treasure, champion, nest, ordinary job, cargo, antique and artifact-search paths. | Full loot/artifact and quest catalogs. | Every reward source, actual frequency, usefulness, recipients, and economy impact. |
| Production | Harvesting loop/banks; shared craft chance; representative runics, BODs and chosen upgrades. | Individual recipes/materials, crops, wine, gardening, apiculture. | Complete cost/yield/success/time/demand model for every production branch. |
| Property/social | Housing settings, storage decay, city upkeep and treasury transfers, referral/voting paths. | Elections, diplomacy, rentals/refunds, guild/social tools and expression. | Actual ownership, participation, transactions, and every service/access path. |
| World/access | Startup boundary, maps/regions, key travel paths, searching/stealth/tracking/locks/traps/theft. | Every transport object, ocean interaction, secret/story gate. | Placed-world accessibility and complete route timing/risk. |
| Staff/integrations | Their role as world/reward configuration surfaces. | Entire source families included in census. | Current staff policy, placed XML attachments, event configuration, external services. |
| Runtime/player experience | No live run was part of this study. | Proposed measurement design. | Deployed build/settings, representative saves, play observations, preferences and telemetry. |

This table is the controlling statement of depth. A short overview row or a path in a CSV does not promote a sampled family to fully reviewed status.

## Data files

| File | Use | Interpretation |
| --- | --- | --- |
| [Source inventory](data/source-inventory.csv) | Find every enumerated C# file, line count, family, and SHA-256. | Every row is `LexicalInventory`; hashes identify the files read. |
| [Source families](data/source-families.csv) | See all 162 navigation groups and their sizes. | Folder grouping only; cross-system dependencies remain in the chapters. |
| [Skills](data/skills.csv) | All 58 enum slots/labels and textual `SkillName` reference paths. | Includes comments and declarations; numeric/dynamic references can be missed. |
| [Gameplay settings](data/gameplay-settings.csv) | Compare source defaults and checked-in positional XML inputs. | Gameplay whitelist only; no network/account identifiers copied. |
| [Regions](data/regions.csv) | Find every checked-in XML region and its spawn-object count. | Spawn definitions are not active-world population. |
| [Inventory summary](data/inventory-summary.json) | Machine-readable totals and exclusions. | Snapshot census, not runtime test results. |
| [Spell registration index](../codebase-audit/outputs/subagent-findings/2026-09-27-gameplay-combat-spells.json) | Every registration, family, source definition, registry-range status. | Registration reachability is different from direct construction or individual spell balance. |
| [Progression and race index](../codebase-audit/outputs/subagent-findings/2026-09-27-gameplay-progression.json) | 18 progression cards, 58 skill slots, 172 raw race rows. | Encoded rows include variants; they are not 172 proven live choices. |

## Canonical guide versus review records

The files under `docs/game-balance/` are the reader's guide. Three detailed chapters are generated from the dated review records:

- [Progression review](../codebase-audit/outputs/subagent-findings/2026-09-27-gameplay-progression.md)
- [Combat review](../codebase-audit/outputs/subagent-findings/2026-09-27-gameplay-combat.md)
- [Economy/world review](../codebase-audit/outputs/subagent-findings/2026-09-27-gameplay-economy-world.md)

Those records are the evidence snapshots. The publisher adjusts navigation and source links for the guide and omits agent-process verification prose. `--check` verifies that the generated chapters still match. They are deliberate published copies, not competing independently maintained descriptions.

The historical codebase audit and wiki are navigation inputs. Current claims were retraced; old balance-risk tables, issue classifications, comments, and wiki prose were not treated as proof. In particular, the current champion map gate was checked against the September source rather than carrying forward the older Lodor-only conclusion.

## Updating this study

From the repository root:

```powershell
python docs/game-balance/tools/build_inventory.py
python docs/game-balance/tools/publish_chapters.py
python docs/game-balance/tools/build_inventory.py --check
python docs/game-balance/tools/publish_chapters.py --check
python docs/game-balance/tools/verify_docs.py
git diff --check
```

Rebuilding a census does **not** refresh the semantic analysis. When source changes, use file hashes and exact references to select the affected claims, re-read their callers and exceptions, update the dated review, then publish and verify the guide. A new gameplay baseline must be reflected in the prose as well as the generated metadata.

## Open work for a genuine whole-game rebalance

1. Finish individual effect, recipe, reward, enemy, and acquisition records, prioritizing the routes players actually use.
2. Resolve the scoped formula/access discrepancies before tuning around them.
3. Compare the deployed configuration and representative saved builds to this source baseline.
4. Measure complete activities, including losses, group distribution, and economic transfers.
5. Agree the desired veteran advantage, role tradeoffs, pacing, and treatment of existing earned power.

The [research plan](balance-research-plan.md) makes those steps concrete. This documentation is ready to use for understanding the game and choosing the next investigations; it does not imply that all of that remaining evidence has already been collected.
