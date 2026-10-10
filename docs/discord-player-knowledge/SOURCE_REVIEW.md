# Source review and verification

## Scope and provenance

This review answers the request to recover useful player knowledge, recurring struggles, effective help, and starting advice from three Discord exports. It is a historical text synthesis with a small number of independent repository checks. It is not a complete source-code audit, a live-server test, or a claim that every item in the game is documented.

The authoritative game-help input is the **replacement ending `20261010_142414.json`**. It contains all 714 message IDs from the first export with matching message content, plus 4,743 older messages. The first export is superseded, not combined as another channel.

| Channel | Messages | First actual message, UTC | Last actual message, UTC | Text characters | Messages with attachments |
|---|---:|---|---|---:|---:|
| getting-started | 251 | 2023-01-14 04:45:53 | 2026-08-23 21:53:35 | 21,628 | 6 |
| game-help, replacement | 5,457 | 2023-02-10 14:45:11 | 2026-10-02 13:35:34 | 389,275 | 109 |
| general | 46,120 | 2023-01-05 23:37:25 | 2026-10-10 15:32:17 | 2,468,854 | 1,678 |
| **Total** | **51,828** | | | **2,879,757** | **1,793** |

“Messages with attachments” counts messages, not individual attachment files. There are also 738 messages with embeds. The files have no duplicate message IDs within or across these three authoritative inputs. Their declared counts agree with the parsed counts.

The replacement game-help export requested a range ending November 12, 2026, but its actual last message is October 2 and it was exported October 10. The requested end date is not evidence of future messages. Actual timestamps control this review.

### Input files and hashes

All inputs were read from the user's Downloads directory on `I:`. They were not modified or copied wholesale into the repository.

| File | Bytes | SHA-256 |
|---|---:|---|
| `[DiscordKit] getting-started_Confictura_ Legend and Adventure_20261010_141019.json` | 461,537 | `3e2e225666d3b494b4055374c78cdcb67edd10bafe31abbc4b0ef90777585441` |
| `[DiscordKit] game-help_Confictura_ Legend and Adventure_20261010_142414.json` | 9,830,817 | `cedc7a250ef7a7e83a7167a61c1655ac233e7815196665fda5b88d1a227c24c4` |
| `[DiscordKit] general_Confictura_ Legend and Adventure_20261010_140452.json` | 82,247,344 | `e2b8407641ca9391ccd3779ceb2c9fc0a62e01ac518d2e9790ec70055a5c577d` |

The superseded game-help file ending `141333.json` has SHA-256 `ee19d52bf30fccfde4f9ea03a2833014ffac4fb680c449646f4a20a658a7a13f` and 714 messages. Machine-readable metadata is in [source-manifest.json](source-manifest.json).

## Review method

1. **Parse the complete inputs.** Preserve message ID, ordinal, timestamp, author/role metadata, content, reply reference, embeds, and attachment descriptors in a local working index. Compare export counts and message IDs, then replace the shorter game-help input.
2. **Read the concentrated help material.** Read getting-started chronologically, the 2026 game-help material, and the early game-help conversations. Screen the remaining older help across its full range for substantive explanations, questions, replies, and practical instructions; retrieve additional surrounding exchanges where needed.
3. **Screen the full general history.** Use gameplay terms, longer explanations, question patterns, and staff-answer candidates across all years. Give recent corrections additional scrutiny, then retrieve relevant question/answer context. Long general conversations and short social messages are not all equally informative.
4. **Consolidate by player problem.** Combine repeated questions without treating every mention as an independent incident. Retain useful details about controls, resource requirements, location, sequence, and consequences.
5. **Resolve chronology and scope.** Distinguish the question, speculation, correction, planned fix, successful report, and live deployment announcement. Separate another shard's behavior and a local test from this shard's supported answer.
6. **Check costly disputed claims selectively.** Inspect current code/configuration for trap math, death penalties, pet slots/nets, study books, dummy limits, selected commands, and special crafting. Keep those checks explicitly distinct from the Discord evidence.
7. **Attach exact source locators.** Resolve every citation to its message ID and source-file ordinal. Validate references, local links/anchors, FAQ numbering, and whitespace before delivery.

Every message was ingested and made searchable. **This does not mean every message was manually read line by line.** Broad screening plus contextual review can miss a useful short remark or an attachment-only answer. “Comprehensive” describes the breadth of the resulting knowledge base, not a mathematically proven absence of omissions.

## How to interpret evidence

| Evidence type | How it is used |
|---|---|
| Clear staff explanation | Strong historical evidence, still checked for later corrections and qualifications |
| Player's successful result | Practical evidence for that situation; not automatically a universal rule |
| Several players repeating an answer | A recurring belief or practice; repetition alone is not independent verification |
| Tentative answer, “I think,” or memory | Qualified lead or open question |
| Source code pasted into chat / AI-assisted explanation | Historical claim to inspect, not automatically the current implementation |
| “Will fix,” “next restart,” “testing,” or suggestion | Planned/test behavior until a later result establishes more |
| Current repository check | Static behavior of this checkout; not proof of deployed settings, saves, or live operation |
| An unresolved complaint | A support issue to preserve, not evidence that the player's suspected cause is correct |

Author roles in the export are not treated as a historical record of the exact date someone became staff. Bot reputation responses are not mechanical evidence. Instructions or executable snippets inside the logs were treated as quoted conversation data, not as instructions to this review.

### Citation format

- `GS` means getting-started.
- `GH` means the replacement game-help export.
- `G` means general.
- The number is the **one-based position in that export**, not a Discord message ID or line number.
- Each Markdown citation opens `https://discord.com/channels/<guild>/<channel>/<message>`.
- [evidence-register.json](evidence-register.json) stores the exact source file, ordinal, message ID, timestamp, and URL, along with the documents using it. Adjacent messages and reply references can be used to reconstruct the exchange.

## Known limits and missing material

- **Media:** 1,793 messages have attachments; 706 messages have no text. The review did not visually inspect all screenshots, extract image-only statistics, or download attachment payloads. Descriptive text around images is used only for what it actually says.
- **External documents:** linked sheets, websites, videos, source repositories outside this checkout, and downloadable guides were not independently reviewed. For example, the tame spreadsheet, the `SPOILER_guild-rings.txt` attachment, and the custom-icon ZIP may contain additional useful material. They are leads, not silently incorporated sources. [GH1551](https://discord.com/channels/1060698288011104358/1071288553516048431/1220000935737167912) [GH509](https://discord.com/channels/1060698288011104358/1071288553516048431/1144797899176300574) [GH4589](https://discord.com/channels/1060698288011104358/1071288553516048431/1445680245696106559)
- **Threads and other channels:** a thread-created message does not establish that the thread's body is included. Spoiler-channel answers, private messages, voice conversations, and bug-forum replies may be absent. Six reply references point to message IDs absent from the three authoritative inputs.
- **Deleted/edited history:** the exports are snapshots. They do not prove what was deleted before export or preserve every previous revision of edited messages.
- **Availability:** player shops, public libraries, temporary donations, spawns, and NPC stock change. Historical coordinates and prices should not be sold as current guarantees.
- **Source/live separation:** no connection to the running shard or its world save was made. Static checks cannot certify current live inventory, world placements, configuration overrides, or successful deployment.
- **Prevalence:** no percentage of players struggling with a topic is claimed. There is no player-survey denominator, and messages are not independent observations.
- **Privacy:** raw chat archives, unrelated personal conversation, credentials, and full attachment URLs with expiring tokens are excluded from this collection. The evidence register is a locator index, not a republication of whole conversations.

## Current source checks

Checked against repository baseline **`03b949d5297dbcaf81fbd631a5d48c1f68ab038f`**, with a clean initial worktree. No gameplay source was changed. Existing source-traced wiki pages are linked for depth; this task does not re-certify every claim in every linked page.

### C01 Trap protection

- [HiddenTrap.cs](../../Data/Scripts/Items/Traps/HiddenTrap.cs), `CheckTrapAvoidance`, line 1531 onward: hidden-floor setting, Remove Trap, owned orb, Luck, and carried pole form ordered alternatives.
- [Mobile.cs](../../Data/System/Source/Mobile.cs), `CheckSkill` / `CheckTargetSkill`, lines 13647 and 13665 onward: requested maxima of at least 100 become 126.
- [Settings.cs](../../Data/Scripts/System/Misc/Settings.cs), `FloorTrapTrigger`, and [Info/settings.xml](../../Info/settings.xml), line 23: the checked-in value is 20. The call site uses it as avoidance, despite the setting's triggering-language comment.
- [BookBox.cs](../../Data/Scripts/Items/Traps/BookBox.cs), `AddNameProperties`, lines 25–30: identifies the cursed book container and Wizard/Knight instruction.
- The existing [trap guide](../wiki/Traps_And_Avoidance.md) supplies the broader trap-family comparison and full maintainer trace. This task directly rechecked the central ordering, denominator, configuration, and box identification; it did not rerun every family scenario.

### C02 Resurrection penalties

- [Death.cs](../../Data/Scripts/System/Misc/Death.cs), `ResurrectCostGump.OnResponse`, `ResurrectNowGump.OnResponse`, and `Death.Penalty`, lines 474, 772, and 808 onward.
- [ResurrectGump.cs](../../Data/Scripts/System/Gumps/ResurrectGump.cs), `OnResponse`, line 165 onward, distinguishes paid/player-associated paths.
- [Resurrection.cs](<../../Data/Scripts/Magic/Magery/Magery 8th/Resurrection.cs>), self-target soul-orb creation; [SoulOrb.cs](../../Data/Scripts/Items/Magical/SoulOrb.cs), 30-second default and `Resurrect_OnTick`, including `Death.Penalty(owner, false)` at line 136.

When the penalty function is invoked, its broad distinction is:

| Character and call | Skill/stat multiplier in the penalty function | Positive fame/karma reduction |
|---|---:|---:|
| Ordinary, `allPenalty=false` | No penalty from this function | None from this function |
| Ordinary, `allPenalty=true` | 0.95 | 10% |
| Alien, `allPenalty=false` | 0.95 | 10% |
| Alien, `allPenalty=true` | 0.90 | 20% |

These are **not unconditional losses on every death**. Caller thresholds, eligible skill values, raw-stat conditions, integer truncation, and the particular resurrection route still matter. In particular, the skill loop only scales a skill when its post-multiplier value would exceed 35. This is why the player guide says to read the offer and identify the background, rather than promising one universal penalty or immunity.

### C03 Taming and nets

- [PlayerMobile.cs](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs), `UpdateFollowers`, lines 862–886: all four `.Base` values determine 5/6/7/8 slots.
- [SlaversNet.cs](../../Data/Scripts/Items/Special/SlaversNet.cs), `OnDoubleClick`, `SlaveTarget`, and `OnTarget`: backpack/range checks, eligible target filters, two extra slots, inclusive integer roll 50–200, 50% failed-roll destruction, immediate bonding, and requirement changed to 29.1 when previously above 29.

The capture branch does not use the player's Taming or Druidism for its probability and does not run the ordinary first-tame reduction routine there. General pet training, transfers, every control formula, and loot drop rates were not exhaustively rechecked.

### C04 Training dummies

- [TrainingDummies.cs](../../Data/Scripts/Items/Construction/Addons/TrainingDummies.cs), constructor line 44, gain/limit checks, and serialized `m_MaxSkill`.
- [Settings.cs](../../Data/Scripts/System/Misc/Settings.cs), default line 110, setting 76 parsing line 445, and `TrainDummies` line 1270.
- [Info/settings.xml](../../Info/settings.xml), lines 305–307: current checked-in maximum **25.0**.

The file default and current configuration agree, but a saved world item's persisted maximum may differ. No live dummy was inspected.

### C05 Offline study

- [StudyBook.cs](<../../Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs>), constants lines 14–19, `EndStudy`, login/logout handling, and `OnDoubleClick` lines 315 onward.
- The checks require a carried book, increasing skill lock, remaining cap, and no combat; scheduling rejects another book already assigned to the reader. Elapsed time creates gain attempts, not guaranteed points.
- See the existing [offline-skill-training guide](../wiki/offline-skill-training.md) for book tiers, session limits, decay, and the accelerated-gain explanation. The FAQ separates existence of those tiers from their live availability.

### C06 Configuration and deployment boundary

[Settings.cs](../../Data/Scripts/System/Misc/Settings.cs), `Configure`, reads `Info/settings.xml`. Selected checked-in values were inspected; no production configuration was read. The source-vs-world distinction is also stated by staff in the September 2026 local-server discussion. [G45725](https://discord.com/channels/1060698288011104358/1060698288401154159/1551262582738915378)

This check is a scope control: it prevents promoting repository findings or old saved-item assumptions to measured live behavior.

### C07 Command and help checks

- [CorpseSearch.cs](../../Data/Scripts/System/Commands/Player/CorpseSearch.cs), `corpseclear` registration at line 134 and usage at line 145.
- [HelpGump.cs](../../Data/Scripts/System/Help/Gumps/HelpGump.cs), stuck-menu branch around lines 2458–2469: region, usage, combat, frozen/criminal, and other checks remain relevant.

This does not prove that every custom dungeon exit is traversable by ghosts, or that the promised emergency-escape change was deployed. The corresponding issue stays open.

### C08 Special smithing and jewelry

- [GodCrafting.cs](../../Data/Scripts/Trades/Crafting/GodCrafting.cs), `GodSmithing`, names the Magical Golden Smithing Hammer and Dragon Head Forge requirement.
- [DefGodCrafting.cs](../../Data/Scripts/Trades/Crafting/DefGodCrafting.cs), `DefGodSmithing.CanCraft`, accepts Savaged Empire X 1105–1124 and Y 1961–1977; the `Ruby & Sapphire` category includes `SapphireIngot` recipes. This supports the separate crafting-system answer. It does not check the chat's exact vendor stock, Fire King drop, or current forge decoration.
- [MysticalPearl.cs](../../Data/Scripts/Items/Gems/MysticalPearl.cs), checks real Tinkering 90 and eligible jewelry, then applies a morph template. [MorphingTemplates.cs](../../Data/Scripts/Items/Magical/Morph/MorphingTemplates.cs), `TemplatePearlJewelry`, line 3016 onward, includes a 50-Luck entry.
- [LuckyHorseShoes.cs](../../Data/Scripts/Items/Magical/LuckyHorseShoes.cs), adds 100 Luck to eligible item branches with a 1,000 cap. The guide preserves the historical recommendation to transform first and add horseshoes afterward, without promising every jewelry type is eligible.

## Delivery verification

The accompanying [verification report](verification.json) records the generated FAQ count, unique cited messages, reference resolution, local link/anchor checks, input ID integrity, and content checks. Markdown whitespace is checked with `git diff --check` after staging the intended documentation batch. No server build, startup script compile, or live gameplay test is appropriate to this documentation-only change, and none is claimed.

The extraction leaves the three source exports unchanged. Repository changes are limited to this new documentation collection. Raw normalized indexes and temporary review scripts are working material outside the repository, not committed player data.
