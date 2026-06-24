# Crafting QoL Roadmap

## Purpose

This roadmap turns crafting quality-of-life work into staged, reviewable changes. The main goal is to reduce repeated clicks while preserving normal crafting time, resource checks, skill checks, tool wear, failure risk, and the existing RunUO crafting flow.

The roadmap is also a UI contract. Crafting is a high-use player workflow, so every phase must be judged by how clear it feels in the client, how little it disrupts existing muscle memory, and how safely it handles stale gump replies.

Use this roadmap alongside [Coding Gumps](Coding_Gumps.md), [Gump Framework](Gump_Framework.md), and [Crafting Core](Crafting_Core.md).

## Goals

- Reduce repetitive clicking for common crafting sessions.
- Preserve normal craft timers and one-attempt-at-a-time server behavior.
- Preserve stock conveniences that already exist: Make Last, Last Ten, resource counts, remembered resource selection, maker's mark controls, repair, resmelt, and enhance hooks.
- Keep the first implementation phases small enough to test per craft skill.
- Make every UI change explicit: fixed coordinates, button IDs, response handling, validation, and client-visible feedback.

## Non-Goals

- Do not add instant batch crafting.
- Do not add a full multi-item queue in the first implementation cycle.
- Do not add source-container or destination-bag routing as part of Make Amount.
- Do not change recipe definitions, tool serialization, player serialization, or world-save layout for early phases.
- Do not hide important failure messages until a separate summary/suppression phase is designed and tested.

## Current Baseline

The shared crafting system is already close to the legacy RunUO stock workflow. `CraftGump` displays categories, selections, notices, Exit, and Make Last. Optional buttons appear when a concrete `CraftSystem` enables sub-resources, maker's marks, resmelt, repair, second sub-resources, or enhance. `CraftContext.OnMade` maintains a runtime Last Ten list, with the most recent crafted item first.

The current item-detail gump shows item art, success chance, required skills, material requirements, retained-color notes, expansion requirements, recipe lockout, and the Make Now action. Material lists are already paged at four rows per page so long recipes do not overwrite the lower panels.

The known risks to account for before adding new QoL are stale gump state, stale resource indices, missing response guards, and legacy fixed-coordinate layouts that can overlap when controls are added without a layout pass.

## UI Standards For Every Phase

### Fixed Layout First

Before code changes, sketch the exact gump bounds:

- `CraftGump` frame: `530 x 437`.
- Header band: top title area.
- Left panel: categories and Last Ten entry.
- Right panel: selections, resource pages, or Last Ten items.
- Notice band: current feedback.
- Footer/action band: Exit, Make Last, and any new controls.

Any new control must fit those surfaces or the phase must explicitly redesign the frame. Do not squeeze new controls into a dynamic row. Footer controls belong in the footer.

### Text And Control Surfaces

- Use short labels for fixed commands.
- Use cropped labels or HTML regions for text that can vary by craft system, recipe, resource, or player input.
- Keep at least a small visual gap between button art and labels.
- For text entries, define the entry ID, visible width, maximum accepted length, valid value range, and invalid-input feedback.
- If a client-side cliloc label is reused, verify it renders correctly in the supported client version. Otherwise use explicit shard text.

### Button IDs

Existing `CraftGump` buttons use:

```text
buttonID = 1 + type + (index * 7)
type = (buttonID - 1) % 7
index = (buttonID - 1) / 7
```

New controls must either fit this scheme without collision or reserve a separate documented range. The decode path must reject invalid type/index pairs before indexing any list.

### Gump Lifecycle

- Close the same gump type before replacing it when stacking would confuse the player.
- Reply buttons close the current tracked gump before `OnResponse`; send a replacement gump when the workflow should continue.
- If an action opens a target cursor, prompt, status gump, or different gump, the transition must be visible and intentional.
- Cancel or Stop must have explicit behavior. Do not rely on a silent close unless no cleanup is needed.

### Defensive Responses

Every touched `OnResponse` path must revalidate:

- `NetState`, `Mobile`, and deleted state.
- The tool still exists and is usable from the player.
- Backpack access still exists when the action requires it.
- Group, recipe, Last Ten, and resource indices are still in range.
- Text entries exist, trim cleanly, parse successfully, and obey domain limits.
- Costs, resources, skill gates, recipe gates, and expansion gates immediately before mutation.
- Any target/container/object captured when the gump was constructed.

## Phased Implementation

### Phase 0 - UI And Safety Audit

Inventory the existing crafting UI before adding features.

Goal command:

```text
Using docs/wiki/Crafting_QoL_Roadmap.md as the source of truth, complete Phase 0 - UI And Safety Audit: inventory current CraftGump and CraftGumpItem layouts, response paths, button and text-entry IDs, and stale group/resource index risks; document findings and blockers; do not add player-facing crafting features.
```

Deliverables:

- Document current `CraftGump` and `CraftGumpItem` layouts with coordinates for panels, footer controls, resource pages, Last Ten, and item detail pages.
- Trace send sites and response paths for `CraftGump`, `CraftGumpItem`, `QueryMakersMarkGump`, repair, resmelt, and enhance target handoffs.
- List all button ID ranges and text-entry IDs currently in use.
- Fix or explicitly schedule stale group-index, resource-index, and response-guard issues.

Acceptance criteria:

- No new player-facing feature is added in this phase.
- Staff can review a coordinate sketch and response-path inventory.
- Known stale-index crash paths are either fixed or called out as blockers for later phases.

Phase 0 audit results:

- `CraftGump` opens at `(40, 40)` and draws a `530 x 437` frame. Fixed regions are: title `(10,10,510,22)`, categories `(10,37,200,250)`, selections `(215,37,305,250)`, notices header `(10,292,150,45)`, notice body `(165,292,355,45)`, and footer/actions `(10,342,510,85)`.
- `CraftGump` footer/action controls are fixed at these rows: Exit `(15,402)` with label `(50,405)`, Make Last `(270,402)` with label `(305,405)`, Resmelt `(15,342)`, Repair `(270,342)`, primary resource `(15,362)`, maker's mark `(270,362)`, secondary resource `(15,382)`, and Enhance `(270,382)`.
- `CraftGump` left-panel rows start with Last Ten at `(15,60)` and craft groups at `(15,80 + index * 20)`. The list is not paged, so any future craft system with enough groups to reach the lower action band must add paging or a visible row cap before more controls are added.
- `CraftGump` right-panel recipe, Last Ten, and resource rows use ten rows per page at `(220,60 + index * 20)` with labels beginning at `(255,63 + index * 20)`. Recipe/detail and Last Ten/detail buttons use `(480,60 + index * 20)`. Page controls sit at y `260`.
- `CraftGumpItem` opens at `(40, 40)` and draws a `530 x 417` frame. Fixed regions are: title `(10,10,510,22)`, item art `(10,37,150,148)`, item/chance `(165,37,355,90)`, skills `(165,132,355,80)`, materials `(165,217,355,80)`, other notes `(165,302,355,80)`, and footer `(10,387,510,22)`.
- `CraftGumpItem` footer controls are Back `(15,387)` with label `(50,390)` and Make Now `(270,387)` with label `(305,390)`. After Phase 1, material pages use three resource rows at `y = 219, 239, 259` so page buttons at `y = 277` have a reserved navigation row.
- `QueryMakersMarkGump` is a separate `(100,200)` confirmation gump with a `220 x 170` frame, Continue button ID `1` at `(20,100)`, and Cancel button ID `0` at `(20,125)`.
- `CraftGump` reply IDs are packed as `1 + type + (index * 7)`: type `0` group, `1` make recipe, `2` recipe details, `3` make Last Ten, `4` Last Ten details, `5` resource selection, and `6` misc. Misc index values are `0` primary resource, `1` resmelt, `2` Make Last, `3` Last Ten, `4` toggle resource hue, `5` repair, `6` maker's mark option, `7` secondary resource, and `8` enhance.
- `CraftGumpItem` uses reply ID `0` for Back and reply ID `1` for Make Now. After Phase 1, unexpected reply IDs return the player to `CraftGump` with clear feedback instead of being treated as Make Now.
- Core crafting gumps currently have no `AddTextEntry` controls. Phase 2 must reserve and document the first text-entry IDs, visible bounds, parsing rules, invalid-input feedback, and stale-gump behavior.
- Send and response paths are: `BaseTool.OnDoubleClick` and captcha redirect open `CraftGump`; `CraftGump.OnResponse` routes group/item/resource/misc replies; `CraftGumpItem.OnResponse` routes Back or Make Now; `CraftItem.Craft` starts the normal craft timer; the timer either opens `QueryMakersMarkGump` or calls `CompleteCraft`; repair, resmelt, and enhance leave the gump through target cursors and send a replacement `CraftGump` with feedback.
- Phase 1 resolved the stale `CraftGump` group/resource context guards, stale `CraftGumpItem` material display and Make Now guards, `Repair.OnTarget` null-target guard, touched response-path sender/mobile/deleted checks, and explicit `CraftGumpItem` reply handling. Remaining before repeated crafting: broaden tool-access checks where Phase 2 touches new response paths and complete client verification for older clients.

### Phase 1 - Stock QoL Polish

Preserve and polish the conveniences that already exist.

Goal command:

```text
Using docs/wiki/Crafting_QoL_Roadmap.md as the source of truth, complete Phase 1 - Stock QoL Polish: preserve crafting semantics while polishing Make Last, Last Ten, resource counts, remembered material selection, maker's mark, repair, resmelt, and enhance surfaces; improve unclear labels, cropping, and feedback; do not add batch controls.
```

Deliverables:

- Confirm Make Last and Last Ten are visible, understandable, and do not overlap selection rows.
- Confirm resource counts and remembered material selection work across all major craft skills with sub-resources.
- Confirm maker's mark, repair, resmelt, and enhance buttons appear only where the underlying craft system supports them.
- Improve unclear labels, cropped text, and validation messages without changing craft semantics.

UI requirements:

- Keep current layout rhythm unless Phase 0 shows a specific overlap problem.
- Use replacement gumps after validation errors so players stay in the workflow.
- Do not add batch controls yet.

Acceptance criteria:

- Each major craft skill has a quick manual pass for normal craft, Make Last, Last Ten, material selection, and any enabled misc buttons.
- Any button that appears must lead to a visible action, visible target cursor, or clear feedback message.

Phase 1 implementation notes:

- Keep the stock crafting semantics intact: no batch controls, no timer shortcuts, and no new saved state.
- Guard stale remembered group/material selections before drawing or acting on them.
- Keep item-detail material pages to three visible rows so the existing page controls have their own space.
- Treat only item-detail reply ID `1` as Make Now; return unexpected replies to the main craft gump with feedback.
- Return repair/enhance invalid target paths to the craft gump with clear localized feedback.

### Phase 2 - Make Amount

Add the first custom click-reduction feature: a finite Make Amount field.

Goal command:

```text
Using docs/wiki/Crafting_QoL_Roadmap.md as the source of truth, implement Phase 2 - Make Amount: add a finite amount field with a 1..100 accepted range, count values as attempts, preserve normal craft timers with one attempt active at a time, and apply the amount to selected recipe rows, Last Ten, Make Last, and item-detail Make Now.
```

Deliverables:

- Add a compact `AMOUNT` text entry to the main craft gump footer and item-detail footer.
- Apply the amount to selected recipe rows, Last Ten rows, Make Last, and item-detail Make Now.
- Count the value as attempts, not successful outputs.
- Clamp the accepted value to a conservative range, such as `1..100`.
- Use normal crafting timers and start only one craft attempt at a time.

UI requirements:

- Define exact footer coordinates before implementation.
- Keep Exit/Back, Amount, and Make Last/Make Now visually distinct.
- Invalid amount input must show feedback and reopen the correct gump.
- The amount field must not obscure notices, recipe rows, or navigation buttons.

Acceptance criteria:

- A request for `10` performs at most ten normal craft attempts.
- Missing resources, invalid skill, broken tool, lost tool access, recipe lockout, or expansion lockout stop the sequence cleanly.
- Single-attempt crafting remains unchanged when amount is `1`.

Phase 2 implementation notes:

- Main `CraftGump` footer uses `AMOUNT` label `(145,403,56,18)`, input frame `(204,400,58,22)`, input well `(206,402,54,18)`, and text entry `(210,402,46,18)` between Exit and Make Last.
- `CraftGumpItem` footer uses `AMOUNT` label `(145,390,56,18)`, input frame `(204,387,58,22)`, input well `(206,389,54,18)`, and text entry `(210,389,46,18)` between Back and Make Now.
- Amount text-entry ID is `9000`; accepted numeric input is clamped to `1..100`, and non-numeric or blank input reopens the current gump with `Enter an amount from 1 to 100.`
- Repeated crafting is runtime-only `CraftContext` state. The first attempt starts normally; each subsequent attempt is scheduled only after the previous craft timer completes and `EndAction(typeof(CraftSystem))` has run.
- The sequence stops on hard validation failures, deleted/broken tools, recipe/expansion/skill lockouts, missing resources, special faction imbue handoff, or custom craft handoff.

### Phase 3 - Batch Status And Summary

Make repeated crafting understandable while it is running.

Goal command:

```text
Using docs/wiki/Crafting_QoL_Roadmap.md as the source of truth, implement Phase 3 - Batch Status And Summary: add visible batch progress UI with recipe name, current/total attempts, success/failure counts, current state, an explicit Stop action that allows the current attempt to finish normally, and final summary feedback.
```

Deliverables:

- Add a small batch status gump or status panel showing recipe name, current attempt, total attempts, successes, failures, and current state.
- Add an explicit Stop button.
- Stop requests finish the current attempt normally, then prevent the next attempt.
- Add final summary feedback when the batch ends.

UI requirements:

- The status gump must have stable button IDs and close-before-open behavior.
- Long recipe names must be cropped or shown in an HTML area.
- The Stop button must be visually separate from normal craft commands.
- Important failure messages should remain visible in the first pass; message suppression can be evaluated later.

Acceptance criteria:

- Players can see that repeated crafting is active.
- Players can stop without losing the current attempt's normal outcome.
- Staff can tell from summary output why a batch ended.

Phase 3 implementation notes:

- `CraftBatchStatusGump` is a separate `(580,40)` status window sized `270x174`; it closes only prior batch status gumps, not the main craft gump.
- Stop uses button ID `1`, sets a runtime stop flag, and prevents only the next attempt after the current craft timer or maker's mark prompt completes.
- Final feedback uses a summary status gump plus a message containing the end reason, completed attempts, total attempts, successes, and failures.

### Phase 4 - Container QoL

Plan source containers and destination bags as a separate feature after Make Amount is stable.

Goal command:

```text
Using docs/wiki/Crafting_QoL_Roadmap.md as the source of truth, implement Phase 4 - Container QoL separately from Make Amount: add source-container and destination-bag selection with target handoffs, visible active container state, reset controls, ownership/access validation, stale-container fallback, and runtime-only state unless persistence is separately approved.
```

Deliverables:

- Add a design for selecting one approved source container for resources.
- Add a design for selecting one destination bag for crafted output.
- Define reset controls, invalid-container fallback, and ownership/access rules.
- Decide whether these settings are runtime-only or persisted elsewhere before implementation.

UI requirements:

- Container selection should use explicit target handoffs and confirmation feedback.
- The active source/destination must be visible somewhere in the crafting workflow.
- Invalid or stale containers must fall back to backpack behavior with a clear message.

Acceptance criteria:

- No container setting can bypass ownership, range, access, or backpack requirements unless deliberately approved.
- Make Amount still works correctly when selected containers become invalid mid-session.

Implementation notes:

- Store source and output container choices in runtime `CraftContext` only; no player serialization or project-file changes are needed.
- Main `CraftGump` uses misc button indexes `9`, `10`, and `11` for source, output, and reset controls in the center footer column.
- Source and output selections accept only the backpack or containers inside the backpack; deleted, inaccessible, world, bank, and house containers clear back to backpack behavior.
- Resource counts, resource checks, and resource consumption resolve against the active source container; crafted results try the active output bag and fall back to the backpack when it becomes invalid or cannot hold the item.

### Phase 5 - Full Queue Evaluation

Evaluate a full multi-item crafting queue only after the smaller QoL phases have proven stable.

Goal command:

```text
Using docs/wiki/Crafting_QoL_Roadmap.md as the source of truth, complete Phase 5 - Full Queue Evaluation: write a separate queue design before code, covering queue rows, paging, per-row controls, enable/disable states, target counts, stop conditions, summary behavior, source/destination interaction, and acceptance tests.
```

Deliverables:

- Write a separate queue design before code.
- Define queue row model, paging, per-row enable/disable, target counts, stop conditions, summary behavior, and source/destination interaction.
- Decide whether queue state is runtime-only or persistent.

UI requirements:

- Use pages for queue rows with server actions.
- Use cropped labels for recipe names.
- Reserve separate button ID ranges for queue row actions.
- Provide clear Start, Stop, Move Up, Move Down, Remove, and Clear controls if the queue is approved.

Acceptance criteria:

- The queue design is reviewed as a new system, not slipped into the existing craft gump as a crowded extension.

## Test Matrix

Every implementation phase should test:

- Normal success.
- Skill failure.
- Missing material.
- Tool reaches zero uses.
- Tool is deleted or moved after the gump opens.
- Player lacks a backpack.
- Player changes selected resource after opening a stale gump.
- Button ID is stale or out of range.
- Recipe-gated item.
- Expansion-gated item.
- Maker's mark prompt.
- Resource page navigation.
- Last Ten page navigation.
- Repair, resmelt, and enhance target handoffs.

Batch phases also test:

- Amount `1`, `2`, `5`, `10`, max value, blank, text, zero, negative, and over max.
- Stop during the first attempt.
- Stop after multiple attempts.
- Resource exhaustion mid-batch.
- Tool break mid-batch.
- Player closes status gump while batch continues or stops, depending on the chosen behavior.
- No tight loops and no instant bulk output.

## Verification

For documentation-only roadmap changes:

- Review Markdown rendering.
- Confirm wiki index links resolve.
- Run `git diff --check`.

For source phases:

- Run a targeted static search for touched gump send sites and response paths.
- Run the narrow scripts build.
- Run a server script-compile smoke test when practical.
- Verify layout in the actual supported client, not only by source inspection.
- Record which craft skills were manually tested.

## Rollout Notes

Roll out one phase at a time. Do not merge Make Amount, status UI, source containers, destination bags, and queues in one change. Each phase should leave the crafting system in a complete, testable state.

Start with high-use craft systems such as blacksmithy, tailoring, carpentry, tinkering, inscription, and cooking. Record any craft-system-specific oddities before enabling broader behavior.

Players should be told that Make Amount reduces clicks, not crafting time. Staff should watch for reports of confusing labels, overlapping controls, missing failure messages, or batches that appear to continue without visible state.

## Source Trace

- `docs/wiki/Coding_Gumps.md`
- `docs/wiki/Gump_Framework.md`
- `docs/wiki/Crafting_Core.md`
- `crafting-qol-improvements.md`
- `Data/Scripts/Items/Trades/Tools/BaseTool.cs`
- `Data/Scripts/Trades/Core/Gumps/CraftGump.cs`
- `Data/Scripts/Trades/Core/Gumps/CraftGumpItem.cs`
- `Data/Scripts/Trades/Core/Gumps/QueryMakersMarkGump.cs`
- `Data/Scripts/Trades/Core/CraftContext.cs`
- `Data/Scripts/Trades/Core/CraftItem.cs`
- `Data/Scripts/Trades/Core/CraftSystem.cs`
- `Data/Scripts/Trades/Core/Enhance.cs`
- `Data/Scripts/Trades/Core/Repair.cs`
- `Data/Scripts/Trades/Core/Resmelt.cs`
