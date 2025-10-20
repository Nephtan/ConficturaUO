# Garden Golem System Test Plan – Pass 2 - Results

## 1. Scope and Objectives
- Validate the Garden Golem feature set located under `Data/Scripts/Custom/GardenGolem/` after the post-pass fixes.
- Confirm the crafted golem bonding workflow, planter usability, gump feedback, and loot adjustments operate as intended.
- Regress core wild golem functionality to ensure no collateral regressions were introduced while addressing pass-one issues.

## 2. Test Environment Prerequisites
1. Configure a Confictura RunUO shard with GM access and command logging enabled.
2. Prepare two accounts:
   - **Caretaker (GM or Seer):** Responsible for spawning, crafting, and managing garden golems.
   - **Observer (Player):** Validates unauthorized access handling and message clarity.
3. Stock the caretaker with:
   - Seeds spanning multiple `PlantType`/`PlantHue` combinations and at least one second-generation hue to exercise fallback text.
   - 3× `FreshGardenSoil`, 20× `FertileDirt`, 25× `MandrakeRoot` (plus reserves for repeat attempts).
   - A pack beetle or spare container to capture corpse loot for drop-quantity validation.
4. Enable periodic world saves and restart at least once during the test session to cover serialization.

## 3. Pass-One Retrospective and Critique
- **Bonding failure:** Previous testing accurately surfaced that the crafted golem never bonded. Root cause traced to `GardenGolemCore` not setting `IsBonded`, loyalty, or bonding timers after `SetControlMaster`. The fix explicitly sets those values; the new plan exercises bonding edge cases to confirm resolution.
- **Seed drag/drop rejection:** The reported "Item must be in backpack" block occurred because the drop handler required the seed to remain parented to the backpack, which is invalid once the client lifts the item. The fix now accepts seeds whose root parent is the caretaker. Additional negative tests confirm world/observer drops still fail.
- **Caretaker gump feedback:** The first pass noted missing seed text and an unclosable gump. While the label technically existed, it lacked context and the gump forcibly reopened even when closed. The updated gump now presents explicit cultivation, stored yield, and fertility information and only refreshes after actionable button presses. Tests below verify that the close button works and that data updates without reopening loops.
- **Loot parity concern:** Crafted golems mirroring wild loot made reagent conversion too generous. The crafted variant now drops a reduced soil bundle (1–3). Tests will confirm both crafted and wild distributions while watching for regression in the seed drop logic.
- **Unverified combat logs:** The earlier feedback cited missing logging tooling. No code change was required, but the second pass will focus on verifying damage types via `[props` and in-combat system messages rather than depending on unavailable logs.

## 4. Pass-Two Strategy Overview
- Prioritize re-testing all areas touched by code changes: crafting, planter interaction, gump UX, and loot.
- Execute targeted negative tests around each fix (e.g., closing the gump, dragging seeds from the ground) to ensure defensive messaging remains intact.
- Perform a light regression sweep of wild golem behavior and persistence to confirm no unintended differences.
- Capture screenshots of the caretaker gump after major state changes for defect documentation.

## 5. Test Cases

### 5.1 Fresh Garden Soil and Loot Distribution
1. `[add FreshGardenSoil` → confirm default amount 1, weight 1.0, hue `0x8A5`.
    - **Result:** Passed
2. Stack multiple soil items → verify stacking behavior and hue retention.
    - **Result:** Passed
3. Kill a **wild** golem → record soil quantity (expect 3–5) and presence of Rich/Average loot packs plus 60% seed.
    - **Result:** Passed
4. Kill a **crafted** golem → record soil quantity (expect 1–3) and verify planter seed, if present, is returned alongside normal loot.
    - **Result:** Passed. However, there should be no loot pack and no random seed given. Needs change to resolve. Did not test with planter seed.
5. Repeat crafted kill with no seed loaded to confirm absence of spurious seed drops.
    - **Result:** Failed. Received spurious seed drop. Did not test with planter seed.

### 5.2 Garden Golem Core Activation & Bonding
1. Attempt double-click on ground → expect backpack requirement message.
    - **Result:** Passed
2. Double-click in backpack with Alchemy below 90 → expect failure message; ensure reagents remain.
    - **Result:** Passed
3. Attempt activation while follower cap exceeded → expect refusal, golem not spawned, reagents untouched.
    - **Result:** Passed
4. With requirements met, double-click core without reagents → ensure reagent-specific errors appear (remove each ingredient in turn).
    - **Result:** Passed
5. With all reagents and follower slots available, activate core → confirm:
   - Core deletes and reagents consumed (3 soil, 20 fertile dirt, 25 mandrake).
       - **Result:** Passed
   - Golem spawns at caretaker location, plays sound `0x241`.
       - **Result:** Passed
   - `IsBonded = true`, `BondingBegin = DateTime.MinValue`, loyalty at 100, and caretaker listed as control master via `[props`.
       - **Result:** Passed
6. Logout/login caretaker → ensure bonded crafted golem remains loyal and under control.
    - **Result:** Passed

### 5.3 Wild Garden Golem Regression
1. `[add GardenGolem` (wild) → verify spawn.
    - **Result:** Passed
2. Attempt observer interaction (double-click, commands) → expect wild refusal messages.
    - **Result:** Passed
3. Attempt taming with low skill (without GM override) → expect failure tied to `MinTameSkill`.
    - **Result:** Passed
4. Trigger combat (e.g., spawn target dummy) → validate mix of physical and poison damage using combat messages and `[props DamageTypes`.
    - **Result:** Passed

### 5.4 Crafted Garden Golem Control Flow
1. After crafting, issue commands (follow, stay) → ensure obedience and no "not bonded" warnings.
    - **Result:** Passed
2. Transfer control via `[givepet Observer` → expect refusal (crafted golem should reject non-caretaker control).
    - **Result:** Passed
3. Logout caretaker with golem active → confirm auto-stable/persistence mirrors shard expectations.
    - **Result:** Passed
4. Observer double-click attempt → verify caretaker-only message and no gump display.
    - **Result:** Not tested

### 5.5 Planter Seed Management
1. Open caretaker gump → confirm cultivation line reads "No seed loaded" on new golem.
    - **Result:** Passed
2. Drag-drop seed from caretaker backpack → ensure acceptance message, seed deletion, gump refresh showing seed type/hue, stored yields reset to 0.
    - **Result:** Failed. Dragging seed from caretaker backpack to crafted Garden Golem produces the following message, "You must have the object in your backpack to use it."
3. Drag-drop seed from ground → expect rejection and seed remains on ground.
    - **Result:** Passed
4. Observer attempts to drag-drop seed → expect caretaker restriction message.
    - **Result:** Passed
5. Attempt to load second seed while one is present → expect refusal.
    - **Result:** Unable to test. Unable to proceed with test plan.
6. Use "Eject seed" button → verify duplicate seed placed in caretaker backpack (respecting capacity) and gump returns to "No seed loaded".

### 5.6 Care Gump Actions and Feedback
1. With seed loaded, advance shard time to trigger growth → confirm stored yield counter increments and gump refreshes to "Harvest ready" after button press.
2. Reduce moisture to 0 (time advance) and press "Water" → ensure moisture resets to 4 and infestation does not change.
3. Allow infestation to rise ≥1, press "Treat" → verify infestation resets to 0, fertility bonus increments by +1, and gump displays updated bonus.
4. Harvest with available yield and free backpack space → ensure produce/seed delivered and stored yield resets to 0.
5. Attempt harvest with full backpack → confirm warning and no orphaned items.
6. Attempt harvest when infestation at 4 → expect refusal message.
7. Close gump using the window close button → ensure it stays closed until re-opened manually.

### 5.7 Persistence and Serialization
1. Load seed, accrue partial growth, adjust fertility, then `[save` and restart server.
2. After relog, verify golem retains control state, bonding, seed type/hue, moisture, infestation, stored yields, and fertility bonus.
3. Kill golem post-restart → ensure corpse drops align with Section 5.1 expectations.

### 5.8 Failure Recovery & Edge Scenarios
1. Delete caretaker while crafted golem active → ensure golem becomes uncontrolled and planter interactions are blocked until retamed.
2. Attempt planter actions while outside 2-tile range → confirm distance message.
3. Attempt to drop non-seed items onto golem → expect fallback to base drag/drop behavior (item returns or falls to ground without special handling).
4. Stress-test gump buttons during combat to ensure no crashes or unexpected state desync.

## 6. Reporting
- Capture chat log excerpts, gump screenshots, and corpse loot inventories for each major verification.
- Document defects with reproduction steps referencing this plan's section numbers and include `SetControlMaster`/bonding screenshots when relevant.
- Update shard QA tracker with bonding regression risk assessment if any edge cases fail.

## 7. Exit Criteria
- All Section 5 test cases executed with documented pass/fail results.
- Critical and high-severity issues resolved, re-tested, and closed.
- Crafted golem bonding, planter management, loot drops, and gump UX verified as stable by QA and gameplay stakeholders.
