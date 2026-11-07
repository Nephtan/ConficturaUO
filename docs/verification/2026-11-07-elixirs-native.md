# Native skill-elixir verification

Run date: November 6–7, 2026. Baseline: `543a09da8d9ae1f9d50ada0a7218ceac42418b69`. The operational Release x86 server and native ClassicUO ran on a copied world on loopback port 57846. Xeron and Poison came from the copied saves and played at Player access. Client interactions initiated purchases, looting, crafting, drinking, and gameplay. Disclosed fixtures set up supplies, targets, travel, and skill boundaries. Production timers ran at normal speed.

| Coverage | Result |
| --- | --- |
| Skill elixirs | 49/49 native recipes, drinks, natural expirations, post-expiry reuse, and baseline/active/expired gameplay consumers |
| Reagent acquisition | Seven distinct source-verified routes exercised in the native client |
| Keg mapping | 21 native round trips and all 49 production-component mappings, the representative scope accepted by the owner |
| Shared cases | Native queue cancellation, per-slot missing materials, source/output pouches, minimum skill, failed rolls, final mortar use, keg capacity and bottle conservation, NPC elixir resale, dual effects, movement and inventory access, death, and reconnect |
| Builds | Release x86 operational build, forced startup script compilation with all four repairs, final forced compile-only pass, 1,609 component assertions passed |

## Repairs

| Source | Baseline and verified candidate behavior |
| --- | --- |
| [PotionKeg.cs](../../Data/Scripts/Items/Potions/PotionKeg.cs) | A rejected pour into a full backpack consumed a bottle. The candidate restores the exact empty container or stack amount on rejection. Retested native rejection and subsequent successful pour. Jar and stack variants were covered by components. |
| [CraftGumpItem.cs](../../Data/Scripts/Trades/Core/Gumps/CraftGumpItem.cs) | The first ingredient page used page zero, which showed through later pages. The candidate numbers ingredient pages from one and aligns the page destinations. Native Next and Back and all 49 component layouts passed. |
| [DynamicBook.cs](../../Data/Scripts/Items/Books/DynamicBook.cs) | All 49 *Alchemical Elixirs* entries assigned the other duration band. The candidate corrects the labels. Native reopening and all 49 source range comparisons passed. |
| [DefAlchemy.cs](../../Data/Scripts/Trades/Crafting/DefAlchemy.cs) | A zero-use mortar could make one more elixir. The candidate rejects exhausted mortars when opening the menu or submitting a stale craft request. A mortar with one remaining use still made exactly one elixir and broke. |

## Acceptance status

**Blocked on active-effect persistence policy.** The copied world saved and reloaded inventory, but Anatomy returned from 176 to its equipment-supported 175 when the shard restarted, before the active 12-minute timer was due. Same-process reconnect preserved active effects, and effects expired at their original deadlines while the player was offline. The candidate preserves the current serialization format and elapsed-time rules. The run also records that expiration can reveal a hidden player and that the book's broad wording about prerequisites and strength can mislead. Crafting was available before opening the newly purchased book, with no prerequisite in the recipe path.

The evidence includes actual outcomes and production chances. Finite random samples describe the observed rolls. One failed character switch left Poison active on the copied shard and briefly applied a skill fixture to the wrong character. The explicit restored snapshot at the original base showed the elixir still active, and the original timer expired 360.074 seconds after drinking. A manual evidence check resolved the analyzer's stale-snapshot flag. Each elixir has separate conclusive native expiry and reuse evidence.

## Evidence and rollback

Local run: `D:\ConficturaUOTestTools\elixirs-native-20261106-151304`. See `reports/acceptance.html`, `reports/case-register.json`, `evidence/coverage.json`, `evidence/events.tsv`, and `patches/` for exact input snapshots, screenshots, candidates, baseline copies, and SHA-256 manifests. The protected-save and original-profile hash checks and test-process shutdown records are under `evidence/`.

While the changes are staged and uncommitted, restore the four listed paths from the recorded baseline only if their current hashes still match the candidate and the rollback has been approved. The patch directory contains the baseline bytes. The repository record is separate from the fixture, copied saves, and private client setup.
