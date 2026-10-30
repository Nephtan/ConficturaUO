# Boss gold placement repair — October 30, 2026

The watched Zanthus clear produced the promised 15,000 shared gold, but native item inspection found 25 stacks at only five positions. Twenty-one stacks occupied the original boss anchor. The boss had died away from that anchor, outside the finishing player's view of the reward area. Quantity evidence alone had missed both presentation defects.

The original `ArachnidOutbreakPlacement.TryFindLoot` started at a random index in the full connected-terrain cache, then returned the first nearby eligible tile. Most starting indices were outside the local reward area, making its first eligible tile disproportionately likely. `ArachnidOutbreakController.TryBossRewards` repeated that search independently without excluding previous choices.

## Repair

`ArachnidOutbreakSite.cs: ArachnidOutbreakPlacement.TryFindLootPoints` gathers eligible cached terrain within the requested radius, removes duplicate X/Y locations, then randomly samples without replacement. `ArachnidOutbreakController.cs: TryBossRewards` requests 25 points around the actual champion death location, first within 12 tiles and then within 56 if necessary. It validates the complete set before queuing boss rewards or creating gold. Insufficient distinct safe ground retains the cancellation path rather than stacking or using unchecked coordinates.

Each pile remains 600 gold, for 15,000 shared gold total. Terrain fit, house and portal checks remain. Player/corpse occupancy is allowed. Eligibility, glands, skull/artifact routing, duplicate-death guards and cooldown rules are unchanged. No serializer, runtime hook, project inclusion, core executable or activation flag changes. Delivery remains disabled.

Gold still appears through immediate `MoveToWorld`; falling coins, timed effects and a victory announcement are separate presentation work. The held showcase client runs the original source and must not be presented as a native demonstration of this repair.

## Verification

`tests/ArachnidOutbreakRewardRegression.cs` exercises the actual boss-death path. It checks 25 distinct 600-gold piles, placement near a death displaced from the spawn anchor, the wider safe-ground fallback, duplicate cached coordinates, portal exclusion, insufficient distinct ground, no partial boss payout, collectible routing and repeated death callbacks.

The new distinct-tile check fails against the accepted pre-repair runtime while its 15,000 quantity check passes. Private reproduction: `D:/ConficturaUOTestTools/outbreak-gold-scatter-repro-20261030-001/`.

Final verification passes in `D:/ConficturaUOTestTools/outbreak-gold-scatter-20261030-003/`: forced runtime compilation plus 679 legacy champion assertions, 63 controller assertions and 343 outbreak reward assertions (1,085 total). All 6,742 recorded production inputs match; the unchanged core artifact matches every accepted core/config input. The current static delivery inventory also passes with all 13 outbreak scripts included and activation disabled. [GOLD_SCATTER_VERIFICATION.json](GOLD_SCATTER_VERIFICATION.json) records source/fixture/assembly hashes and evidence limits.

The first candidate's runtime compile caught an unsupported `HashSet` reference; the implementation now uses the shard's existing `Dictionary` pattern. The second candidate compiled, but the default broad suite stopped on the older lifecycle fixture's pre-approval assumption that every catalog site remains unapproved. That same assertion fails against the unchanged accepted baseline; it is an existing fixture assumption, not evidence of a gold regression. The focused gold suite covers legacy champion rewards, controller behavior and outbreak rewards. Failed attempts remain retained separately.

Reproduce the focused candidate using `tools/champion-outbreak/VerifyApprovedChanges.py --baseline <source-matching accepted candidate> --output <fresh private directory> --assets <installed assets> --gold-scatter`. It requires exactly the two placement/controller source deltas, verifies every recorded core/config input, forces runtime script compilation and runs save-free component fixtures. This is separate from gameplay, world reload and native visual acceptance.

Private observed evidence: `outbreak-zanthus-showcase-20261030-001/evidence/gold-stacking-confirmed.json`, `native-ground-gold-inspection.json`, `native-gold-stacking-observed.png` and `feedback-gold-stacking.json` under `D:/ConficturaUOTestTools/`. The copied client reconnected and walked normally for inspection; no gold was created or collected and the completed fight was not replayed. The fixture/controller was subsequently disarmed while the client/server remained available for review.
