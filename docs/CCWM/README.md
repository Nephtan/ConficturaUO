# Confictura Codex World Map

CCWM is a simulated-player continuity project. It is not a generic shard wiki.
Use it to continue a single player-facing route through Confictura with evidence
that a normal client user could have seen, remembered, or inferred.

## Artifact Roles

- `Confictura_Codex_World_Map.md` is the historical play log. Append to it only
  when a simulated run actually happens.
- `Simulation_State.yaml` is the current playable state for the next run. Keep it
  lean: location, visible UI, target cursor, vitals, inventory, follower state,
  recent visible scan, route intent, immediate blockers, and next decision.
- `live-state/` is a committed snapshot of saved world state. Treat it as
  canonical until a manual refresh is explicitly requested.
- `client-worldmap-markers/` is the player's visible world-map overlay. Markers
  are route knowledge, not proof that entities or safe paths exist on screen.
- `generated/` contains derived navigation aids. Regenerate them with
  `tools/ccwm_index.py`; do not hand-maintain them as source of truth.
- `tools/` contains CCWM-only maintenance tools. These tools must not be placed
  under `Data/Scripts` or added to `Data/Scripts/Scripts.csproj`.

## Normal Run Flow

1. Run `python docs/CCWM/tools/ccwm_verify.py`.
2. Read `docs/CCWM/ccwm-autoprompt.md`.
3. Load `Simulation_State.yaml`, `live-state/manifest.yaml`, the live-state
   JSONL files, the relevant scan YAML, and the current map marker CSV files.
4. Confirm snapshot freshness risk, current UI, target cursor status, open
   context menu or gump state, follower state, visible scan, and next
   client-visible decision.
5. Simulate at most the next small normal-player beat or macro-travel intention.
6. Append the result to `Confictura_Codex_World_Map.md`.
7. Update only current playable continuity in `Simulation_State.yaml`.
8. Run `python docs/CCWM/tools/ccwm_index.py` and then
   `python docs/CCWM/tools/ccwm_verify.py`.

Do not derive the latest run number from memory. Always read it from the log and
state files.

## Manual Refresh Policy

Live-state files are exported artifacts. Do not hand-edit `live-state/*.jsonl`,
`live-state/manifest.yaml`, or `live-state/scans/*.yaml` during ordinary play.
Refresh them only with the CCWM-only exporter under
`docs/CCWM/tools/live-state-exporter/` or by an explicitly documented manual
process.

The live-state exporter is intentionally outside `Data/Scripts`. The server
script loader compiles files from `Data/Scripts`; CCWM tooling is offline
documentation tooling and must not be loaded by the shard.

## Dirty Worktree Handling

The map log and state may have active uncommitted run changes. Before editing,
check `git status --short`. Do not overwrite current run/state changes. For
maintenance work, prefer adding or updating derived docs and tooling around the
current state instead of rewriting the active run files.

## Manual Capture Blockers

Some text exists only in the client at runtime. Randomized gump, note, vendor,
or assignment text must be captured exactly while visible. If the exact text is
lost, add or keep a `manual_capture_required` entry in the derived knowledge
index and do not route that branch as if the missing text were known.

The known model case is the thief-note assignment line: the exact first
assignment sentence was not preserved, so the thief-job route stays blocked
until the visible note text is captured again or the branch is abandoned.
