# Garden Golem System Test Plan

## 1. Scope and Objectives
- Validate the full feature set for the Garden Golem system located under `Data/Scripts/Custom/GardenGolem/`.
- Confirm that planter interactions, creature behaviors, item drops, and serialization operate as designed.
- Ensure regressions are detectable by exercising both success and failure paths defined in the scripts.

## 2. Test Environment Prerequisites
1. Configure a RunUO shard using the Confictura server binaries with GM access.
2. Enable command logging for traceability of steps and results.
3. Prepare two test accounts:
   - **Caretaker**: GM or Seer level for spawning and controlling the golem.
   - **Observer**: Regular player used to validate access restrictions.
4. Stock a GM toolset with gardening seeds, fertilizers, and reagents:
   - Seeds representing multiple `PlantType`/`PlantHue` combinations.
   - 3× `FreshGardenSoil`, 20× `FertileDirt`, 25× `MandrakeRoot`.
5. Position the testing in a safe area with world-save cycles enabled (for persistence checks).

## 3. Test Cases

### 3.1 Fresh Garden Soil Item
1. Spawn `FreshGardenSoil` via `[add` to confirm default amount is 1 and weight is 1.0.
2. Stack two soil items and verify they combine and retain hue `0x8A5`.
3. Delete inventory and confirm soil persists through logout/login when stored in bank.
4. Kill a spawned Garden Golem (see Section 3.4) and verify corpse contains 3–5 soil units.

### 3.2 Garden Golem Core Item
1. Spawn `GardenGolemCore` and verify appearance (itemID `0x1EA8`) and hue `0x48E`.
2. Attempt to double-click while item is on ground; expect system message requiring backpack.
3. Place core in backpack and double-click with Alchemy below 90.0; expect failure message.
4. Raise Alchemy to ≥90.0 and ensure follower slots are available:
   - Attempt with follower cap exceeded and validate rejection message.
5. With prerequisites met but without reagents, double-click and verify reagent-specific error messages for missing soil, fertile dirt, and mandrake (remove each requirement separately).
6. With all reagents present, double-click and confirm:
   - Core deletes from inventory.
   - 3× soil, 20× fertile dirt, 25× mandrake consumed.
   - Garden Golem spawns at player location, plays sound `0x241`, and bonds to caretaker.

### 3.3 Wild Garden Golem Behavior
1. Spawn an untamed Garden Golem via `[add GardenGolem`.
2. Confirm taming requirements: attempt to double-click and interact as Observer and verify refusal due to wild status.
3. Tame the golem with GM powers; ensure control slots usage is 3 and `MinTameSkill` gating works by attempting as a low-skill player (should fail).
4. Verify combat stats by provoking the golem into combat and observing damage types (physical/poison mix) using combat logs.
5. Kill the wild golem and validate loot packs (Rich + Average) drop alongside soil bundle and 60% chance seed.

### 3.4 Crafted Garden Golem Behavior
1. Animate a crafted golem via Section 3.2 and confirm `IsCrafted` property is true with `[props`.
2. Transfer control to the caretaker and ensure Observer cannot issue commands.
3. Log out caretaker to check golem persistence and auto-stable rules per shard configuration.
4. Kill the crafted golem and verify planter seed (if any) is returned to corpse alongside soil and random seed drop logic.

### 3.5 Planter Seed Management
1. As caretaker, double-click the controlled golem; confirm caretaker gump displays seed “None”, moisture 2/4, infestation 0/4, next growth timestamp.
2. Attempt gump access as Observer and confirm rejection message.
3. Drag-drop a seed from caretaker’s backpack onto the golem:
   - Validate seed deletion and gump refresh shows correct plant type/hue.
4. Attempt to add a second seed and verify refusal message.
5. Eject the seed via gump and ensure a duplicate seed is placed in caretaker’s backpack (requires free space); confirm planter returns to empty state.

### 3.6 Growth Cycle Simulation
1. With a seed loaded, advance time via `[set time` or shard time manipulation to exceed 12-hour growth interval.
2. Confirm `OnThink` broadcasts emote when harvest becomes available while caretaker is set.
3. Observe moisture decrement per tick (should decrease by 1 each interval) and infestation increment when moisture ≤1.
4. Validate that harvest does not accumulate beyond 3 stored yields by advancing multiple intervals without harvesting.

### 3.7 Care Actions via Gump
1. Water Button:
   - Reduce moisture to 0 using repeated time advancement.
   - Use Water button and confirm moisture resets to 4.
2. Treat Button:
   - Allow infestation to reach ≥1, then use Treat button.
   - Verify infestation resets to 0 and fertility bonus increases (check via subsequent harvest increment reaching 2 when bonus active).
   - Attempt treat when infestation is 0 and confirm informational message only.
3. Harvest Button:
   - Harvest when infestation <4 to receive either plant resource or fallback seed into backpack.
   - Attempt harvest with full backpack and confirm harvest deletion with warning.
   - Allow infestation to reach 4 and ensure harvest attempt fails with pest warning.
4. Eject Seed Button:
   - Confirm seed returned to backpack and planter cleared; ensure operation blocked when backpack lacks space.

### 3.8 Unauthorized Interaction Safeguards
1. Observer attempts drag-dropping seed on caretaker’s golem; confirm rejection message.
2. Caretaker attempts interactions while out of 2-tile range; verify localized distance message.
3. Attempt planter actions while golem uncontrolled or ControlMaster null (release pet and try); expect refusal messages.

### 3.9 Persistence and Serialization
1. Load seed, water, and let planter accumulate partial growth.
2. Force world save and restart server.
3. After login, confirm planter state (seed type, moisture, infestation, stored yields, fertility bonus) persists.
4. Verify `IsCrafted` flag persists across save/load for crafted golems.

### 3.10 Failure Recovery and Edge Scenarios
1. Delete caretaker while golem remains; ensure golem becomes uncontrolled and planter locks interactions until retamed.
2. Attempt to drop non-seed items onto golem and confirm default drag-drop handling (should fall back to base behavior).
3. Evaluate behavior when caretaker backpack is overloaded (weight limit) during harvest/eject operations.
4. Test interaction timing during active combat to ensure gump operations do not cause crashes or unexpected behavior.

## 4. Reporting
- Capture screenshots of gump states and system messages for each major step.
- Record timestamps, commands used, and outcomes in a shared test log.
- File defects with reproduction steps referencing this plan’s section numbers.

## 5. Exit Criteria
- All test cases in Section 3 executed with pass/fail results documented.
- Critical defects resolved and retested.
- Stakeholder sign-off confirming Garden Golem system readiness for release.