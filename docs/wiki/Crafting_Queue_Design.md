# Crafting Queue Design

## Purpose

This is the Phase 5 design document for a full multi-item crafting queue. It is intentionally design-only: code should not be written until this design is reviewed against the crafting QoL roadmap and the gump safety rules.

The queue should reduce repetitive setup for players who already have materials, tools, and recipes available. It must not shorten crafting timers, craft while offline, bypass skill checks, or hide normal failure feedback.

## Design Decision

The first queue implementation should be runtime-only. Store queue state in the active crafting context for the current player, craft system, and tool session. Do not add `PlayerMobile` serialization, world-save data, account data, or project-file changes for persistence in the first pass.

Persistent queues can be considered later only after a separate design review. Persisting queue rows would require versioned serialization, stale recipe recovery, source/destination recovery, and stricter abuse controls.

## Player Goals

- Queue several recipes without returning to the recipe list after every item.
- Set a target attempt count for each queued recipe.
- Start the queue and let it perform one normal craft attempt at a time.
- Stop cleanly without interrupting or duplicating the current attempt.
- See which recipe is active, which rows are disabled, and why the queue stopped.
- Reuse the active Phase 4 source and output containers without selecting them on every row.

## Non-Goals

- No instant bulk crafting.
- No background or offline crafting.
- No queue persistence in the first implementation.
- No per-row source or output container overrides in the first implementation.
- No suppression of normal crafting success, failure, repair, resmelt, enhance, maker's mark, or resource feedback.
- No crowded extension of the existing `CraftGump` footer.

## UI Overview

Build a separate `CraftQueueGump` instead of cramming queue controls into the existing craft menu. The queue gump may use the same 530x437 frame rhythm as `CraftGump`, but it should replace the active crafting view while open rather than draw more controls into the current footer.

Recommended fixed layout:

- Frame: origin `(40,40)`, size `530x437`.
- Title: `(10,10,510,22)`.
- Active status and container header: `(10,37,510,42)`.
- Queue row table: `(10,84,510,236)`.
- Selected-row editor: `(10,325,510,55)`.
- Footer controls: `(10,385,510,42)`.

Use 8 visible rows per page at 28 pixels per row. Recipe names must be cropped or clipped within the row label area. Page controls belong below the row table, and dynamic rows must never draw beyond the row table bounds.

The header should show:

- Current recipe running, or `Idle`.
- Active source container label.
- Active output container label.
- Current page and total pages.
- Stop-requested state when applicable.

The footer should show clear command buttons for `Back`, `Start`, `Stop`, `Clear`, and `Add Current` if the queue is opened from a selected recipe. The `Stop` button is enabled only while the queue is running or stop has been requested.

## Queue Row Model

Each queue row should be a runtime record, not serialized save data.

Required fields:

- `RowId`: monotonic runtime id for stable button decoding.
- `CraftSystem`: runtime craft system for the active tool session.
- `CraftItem`: runtime recipe reference.
- `ItemType`: crafted item type used for stale recipe validation and display recovery.
- `DisplayName`: cropped recipe name for gump rows.
- `PrimaryResource`: selected primary resource type at the time the row is added.
- `SecondaryResource`: selected secondary resource type, if the craft system supports one.
- `TargetAttempts`: requested attempts for the row.
- `CompletedAttempts`: attempts already performed.
- `Successes`: successful craft attempts.
- `Failures`: failed craft attempts.
- `Enabled`: whether the row participates when the queue runs.
- `State`: `Pending`, `Running`, `Disabled`, `Complete`, `Blocked`, or `StopRequested`.
- `LastMessage`: short reason for the last block, stop, or validation failure.
- `SortIndex`: current order in the queue.

`TargetAttempts` means attempts, not guaranteed successful outputs. A failed skill roll still consumes one attempt if the normal crafting pipeline treats it as an attempt.

## Target Counts And Caps

Use one selected-row amount editor, not one text entry per row. The selected-row editor should expose a single `AddTextEntry` for `TargetAttempts`, an `Apply` button, and the selected row's current progress.

Initial limits:

- Per-row `TargetAttempts`: `1..100`.
- Maximum queue rows: `25`.
- Maximum total pending attempts across enabled rows: `500`.

Invalid amount input should reopen the queue gump with a short validation message and preserve the previous valid value. Completed rows cannot reduce `TargetAttempts` below `CompletedAttempts` unless the player explicitly resets or removes the row.

## Paging And Controls

Use stable button ranges that do not overlap existing `CraftGump` recipe packing.

Recommended button ranges:

- `1..99`: gump commands such as Back, Start, Stop, Clear, Previous Page, Next Page, Add Current, Apply Count, and Reset Complete.
- `1000 + RowId`: select row.
- `2000 + RowId`: toggle enabled.
- `3000 + RowId`: move row up.
- `4000 + RowId`: move row down.
- `5000 + RowId`: remove row.

On response, decode the range first, resolve `RowId` against the current runtime queue, and reject stale row ids with feedback. Page numbers must be clamped before drawing. Row commands must not rely on visible index alone because the queue can change between gump open and response.

Per-row controls:

- Select row.
- Toggle enabled.
- Move up.
- Move down.
- Remove.

Disable or reject controls that would mutate the active running row. Pending rows may be reordered or removed while idle. While running, row order should be locked except for `Stop` unless a later design explicitly supports live edits.

## Enable And Disable States

Disabled rows stay visible but are skipped when the queue runs. Skipped disabled rows do not count as failures and should appear in the final summary as skipped.

Toggling a pending row changes only that row. Toggling the active running row should be rejected with feedback or translated into a stop request after the current attempt; the first implementation should reject it and direct the player to use `Stop`.

Complete rows remain visible until the player clears, removes, or resets them. Re-running a complete row requires an explicit reset so players do not accidentally restart finished work.

## Execution Model

The queue executes one normal craft attempt at a time. It should reuse the same craft timer, action locks, resource checks, skill checks, tool-use checks, and result placement path as manual crafting and Make Amount.

Execution order:

1. Find the first enabled row where `CompletedAttempts < TargetAttempts`.
2. Validate player, tool, craft system, recipe, selected resources, source container, output container, and costs.
3. Start exactly one normal craft attempt.
4. Let normal crafting complete and report normal success or failure.
5. Increment row attempt, success, and failure counters from the normal result.
6. Continue to the same row until its target is reached, then advance to the next enabled row.
7. Stop when no enabled incomplete rows remain or a hard stop condition occurs.

The queue must not run a tight loop, bypass `BeginAction`, pre-consume resources in bulk, or produce output outside the normal craft result path.

## Stop Conditions

The queue stops after the current attempt completes when the player presses `Stop`.

The queue stops immediately before starting the next attempt when:

- No enabled incomplete rows remain.
- The player is deleted, dead, disconnected, or no longer valid for crafting.
- The tool is missing, deleted, inaccessible, broken, or out of uses.
- The backpack is missing when the craft path requires one.
- The recipe is no longer valid for the player's expansion, race, guild, skill, or other gating rule.
- The selected resource is invalid or unavailable.
- Required materials cannot be found in the active source container or backpack fallback.
- The source container is deleted, distant, inaccessible, or no longer allowed.
- The output container is deleted, distant, inaccessible, full, or no longer allowed, and backpack fallback also fails.
- A maker's mark, faction imbue, repair, resmelt, enhance, or custom craft handoff requires player targeting or a separate decision.
- A stale queue row can no longer resolve to a valid `CraftItem`.
- A stale gump response mutates a row that no longer exists.

Normal skill failures do not stop the queue by themselves. They count as failed attempts unless the underlying craft system treats the failure as a hard stop.

## Summary Behavior

Show a final summary when the queue completes, stops, or blocks. The summary may be a small gump or a concise message plus a reopened queue gump.

The summary should include:

- Overall state: complete, stopped, or blocked.
- Stop or block reason.
- Enabled row count and skipped disabled row count.
- Total completed attempts and total target attempts.
- Total successes and failures.
- Per-row recipe name, completed attempts, target attempts, successes, failures, and last message.

Long summaries should page or cap visible rows. Do not suppress normal craft messages in the first implementation; the summary should explain the batch result, not replace immediate feedback.

## Source And Destination Containers

The first implementation should use the active Phase 4 source and output containers from the runtime crafting context. Do not add per-row source or destination containers in the first queue version.

Rules:

- The queue header shows the current source and output labels.
- Source and output selections are validated before every attempt, not only when the queue starts.
- If the source container becomes invalid, resource checks fall back to backpack behavior only when that fallback is already allowed by Phase 4 rules.
- If fallback cannot satisfy materials, the queue stops with a missing-material or invalid-source reason.
- If the output container becomes invalid, output placement falls back to backpack behavior only when that fallback is already allowed by Phase 4 rules.
- If output cannot be placed in either the selected destination or backpack fallback, the queue stops with an invalid-output reason.
- Changing source or output containers while the queue is idle affects the next run.
- Changing source or output containers while the queue is running should be disallowed in the first implementation; the player can stop, change containers, then restart.

Per-row source and destination overrides remain a future feature because they require more row state, more controls, and more stale-container testing.

## Defensive Response Handling

Every `OnResponse` path must validate:

- `NetState` and `Mobile`.
- Player deleted, dead, access, and backpack state.
- Tool existence, ownership/access, location, and uses.
- Craft context existence and active craft system.
- Page index bounds.
- Button ID range and decoded `RowId`.
- Row existence, row state, and command eligibility.
- Text-entry existence, parse result, and numeric limits.
- Resource selections, recipe availability, and material costs immediately before mutation.
- Source and output container validity before each attempt.

Invalid responses should reopen the queue or craft gump with feedback. They should not throw, mutate the wrong row, or silently reset player choices.

## Acceptance Tests

Layout and gump safety:

- Review fixed-pixel bounds in the target client before code is accepted.
- Confirm 8 rows fit without overlap and long recipe names crop cleanly.
- Confirm footer buttons do not overlap selected-row amount controls.
- Confirm previous/next page controls work with 0, 1, 8, 9, and 25 rows.
- Confirm stale page numbers clamp instead of throwing.

Queue row behavior:

- Add one row from the current recipe.
- Add several rows from different recipe categories.
- Select rows across pages.
- Move first, middle, and last rows up and down.
- Toggle pending rows enabled and disabled.
- Remove selected rows and stale rows.
- Clear complete rows and clear the whole queue while idle.

Target count behavior:

- Apply blank, text, `0`, negative, `1`, `10`, `100`, and over-cap values.
- Confirm invalid values keep the old valid count and show feedback.
- Confirm a completed row cannot be reduced below completed attempts without explicit reset.

Execution behavior:

- Run an empty queue and confirm Start is rejected cleanly.
- Run one row for Amount 1 and Amount 10.
- Run multiple rows and confirm order is preserved.
- Confirm each attempt uses normal crafting time.
- Confirm no tight loop, instant bulk output, or action-lock bypass occurs.
- Confirm skill failures count as attempts and do not hide normal failure messages.
- Press Stop during the first attempt and after several attempts; confirm the current attempt finishes normally.

Failure and stale-state behavior:

- Exhaust resources mid-row.
- Break or delete the tool mid-queue.
- Move the tool out of reach mid-queue.
- Delete, move, or lock down the source container after queue start.
- Delete, move, or fill the output container after queue start.
- Use nearby house chests as source and output containers.
- Remove the backpack if possible in a staff test.
- Change selected resources from a stale craft gump.
- Submit stale row buttons after the row has been removed.
- Test recipe-gated, expansion-gated, race-gated, and skill-gated recipes.
- Test maker's mark or other target handoffs and confirm the queue stops or pauses with a clear reason.

## Open Questions

- Is `25` the right maximum row count for the first implementation?
- Should the queue gump replace `CraftGump` entirely while open, or allow a direct return to the selected recipe page?
- Should completed rows remain by default or auto-collapse into the summary?
- Should a later queue version support per-row source and output containers?
- Is runtime-only sufficient, or do staff want a future persistent queue with explicit serialization review?
