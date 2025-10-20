# Garden Golem System Test Plan – Pass 3

## 1. Scope and Objectives
- Validate the Garden Golem feature set in `Data/Scripts/Custom/GardenGolem/` after the loot and seed-loading code changes.
- Confirm crafted golem corpses now respect their bespoke loot profile while wild golems retain legacy drops.
- Demonstrate that caretakers can load seeds from their inventory without reopening pass-two defects and that unauthorized sources remain blocked.
- Regress planter management, caretaker gump workflows, and serialization to ensure the new logic did not introduce collateral issues.

## 2. Pass-Two Retrospective and Critique
- **Loot parity defect:** The second pass correctly identified that crafted golems still produced wild-quality loot, but the run stopped short of quantifying the extra reagents entering the shard economy. This pass will explicitly log container contents to prove the removal of loot packs and random seeds for crafted variants.
- **Seed drag/drop failure:** QA captured the incorrect backpack requirement messaging, yet the investigation ended before exploring root-cause clues (e.g., bounce data, held item state). The updated code leverages that context, so testers must now cover positive and negative drops to avoid another premature closure.
- **Untested observer access:** Pass two skipped the crafted golem observer interaction case. Because the planter logic changed, we need to reintroduce this scenario to make sure new message paths did not regress.
- **Serialization gap:** Restart coverage was listed but never executed. Given that planter state serialization gates both loot and seed handling, the new pass will execute a full save/restart cycle before sign-off.

## 3. Test Environment Prerequisites
1. GM-enabled shard with command logging to capture loot counts (`[global log` recommended).
2. Accounts:
   - **Caretaker (GM/Seer):** Performs spawning, crafting, and planter operations.
   - **Observer (Player):** Exercises unauthorized messaging and access controls.
3. Inventory requirements for the caretaker:
   - `FreshGardenSoil` ×3, `FertileDirt` ×20, `MandrakeRoot` ×25 per craft attempt (stock extras for repeats).
   - Mixed `Seed` stack featuring first- and second-generation hues for fallback text validation.
   - Empty containers for corpse-loot auditing (pack beetle optional but not required).
4. Enable periodic world saves and plan at least one manual restart mid-pass.

## 4. Risk Assessment
- Crafted golem loot suppression is high-impact: incorrect configuration reintroduces the original exploit. Loot tables require meticulous inspection.
- The new seed validation can block legitimate caretakers if bounce metadata behaves differently on production hardware. Negative tests must document any unexpected messaging for engineering follow-up.
- Regression pressure on planter timers and gump refresh remains moderate; functionality depends on the planter state ticking while the golem is controlled.

## 5. Strategy Overview
- Execute focused confirmation tests for each code change before expanding into regression coverage.
- Capture log snippets (`[where` on corpse, `[props` on golem) whenever an expected value changes state so that defects can reference concrete evidence.
- Maintain a shared spreadsheet or checklist to ensure no pass-two scenario is skipped a second time.

## 6. Test Cases

### 6.1 Crafted vs. Wild Loot Validation
1. Spawn a wild golem with `[add GardenGolem` and kill it.
   - Record soil amount (expect 3–5), confirm presence of Rich/Average loot entries, and verify a 60% chance random seed can appear.
2. Spawn a crafted golem via `GardenGolemCore`.
   - Kill immediately with no planter seed loaded.
   - Expected: soil 1–3, **no** Rich/Average loot packs, **no** random seed; corpse should contain only soil (and gold if any external system injects it).
3. Load a planter seed into a new crafted golem (Section 6.2) and then kill it.
   - Confirm planter seed is returned on the corpse alongside reduced soil.
4. Repeat Steps 1–3 after a server restart to validate persistence.

### 6.2 Seed Loading and Access Control
1. Drag a seed from the caretaker backpack to the crafted golem.
   - Expect successful absorption message and gump refresh.
2. Drag the same seed type from the world floor.
   - Expect the `1042664` backpack requirement message; seed should remain on the ground.
3. Attempt to drop a seed owned by another player or removed from an observer’s pack.
   - Expect refusal messaging identical to Step 2.
4. Try loading a seed from a caretaker-controlled pack animal without first moving it to the backpack.
   - Document behavior; if refusal occurs, confirm that moving the seed to the backpack allows success (ensures testers understand the current limitation).
5. Observer performs a drag/drop attempt to confirm caretaker-only messaging is intact.
6. Load a seed, eject it via the gump button, and confirm the duplicate seed lands in the caretaker backpack with correct hue/type metadata.

### 6.3 Caretaker Gump and Planter Maintenance
1. With a seed loaded, advance time or use staff commands to tick growth until harvest is ready.
2. Verify gump lines for cultivation status, stored yield, moisture, infestation, and fertility reflect the planter state before and after each button press (Water, Treat, Harvest, Eject).
3. Ensure closing the gump keeps it closed until manually reopened (no forced refresh loop).
4. Attempt gump actions while standing more than two tiles away; expect distance warnings without state changes.

### 6.4 Control, Observer Interaction, and Combat
1. Exercise follow, stay, guard, and release commands on the crafted golem to ensure bonding remains intact.
2. Observer double-clicks the crafted golem; expect caretaker-only message with no gump display.
3. Confirm wild golem taming remains gated by `MinTameSkill` and that damage types (physical/poison mix) are unchanged via combat logs or `[props`.

### 6.5 Persistence and Edge Scenarios
1. With a seeded, partially grown crafted golem active, run `[save`, restart the shard, and relog.
   - Verify control master, bonding flags, planter seed data, moisture, infestation, stored yields, and fertility bonus.
2. Repeat selected seed-loading and gump actions post-restart to ensure state machine continuity.
3. Delete the caretaker while the crafted golem is active to confirm the golem becomes uncontrolled and planter interactions are blocked until re-tamed.
4. Stress-test gump button spam during combat to ensure no crashes or state desync occurs.

## 7. Reporting Expectations
- Capture corpse content screenshots or logs for each loot validation case.
- Attach chat logs demonstrating both acceptance and rejection seed-drop messaging.
- File defects with precise reproduction steps referencing the sections above; include control master name, golem serial, and planter state dump when applicable.

## 8. Exit Criteria
- All Section 6 scenarios executed with evidence attached.
- Any high-severity issues logged, fixed, and re-tested; no open blockers tied to loot, bonding, or planter interaction remain.
- Stakeholders acknowledge the crafted golem loot nerf and seed workflow as production-ready.
