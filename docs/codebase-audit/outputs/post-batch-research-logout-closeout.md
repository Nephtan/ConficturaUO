# Research Spell Logout Repair

Batch: `POST-BATCH-RESEARCH-LOGOUT`

Date: 2026-10-17

Status: Complete; source, regression runner, and backlog evidence committed together.

## Scope and outcome

The user explicitly selected `RB-01750` and `RB-01751` for repair on `main`.
Baseline: `89426b1c4801308614b1582426cf46a665c174f5`; initial worktree clean, with
no unrelated pre-existing changes. Root `AGENTS.md` applies to both scripts,
`scripts/`, and `tests/`; `docs/codebase-audit/AGENTS.md` additionally applies
to these audit artifacts.

Both spells previously subscribed an instance handler to the global Logout
event from `OnCast`, retaining one callback per cast. Enchant then traversed
`World.Items.Values` twice per callback. Sneak could reveal a logging-out
mobile even when it had no Sneak effect. These defects are now fixed:

- Each spell registers one static handler from `Initialize`. Removing that
  same static handler before adding it makes repeat initialization idempotent
  without clearing other subscribers. Casting no longer changes subscriptions.
- Enchant tracks its live `ResearchEnchantStone` instances in memory. Logout
  checks only these orbs, selects those belonging to the logging-out mobile,
  and finds each weapon through `World.FindItem(serial)`. No world-item scan
  remains in this file. Orb tracking survives moves out of the backpack and
  is removed when an orb is deleted.
- Each Enchant expiry/load timer cleans its own orb. A timer for a deleted
  orb cannot remove a later enchantment. Loaded orbs restore their individual
  weapons rather than deleting every orb while retaining only one weapon's
  original properties. The public `EndEffects(null)` all-effects entry point
  remains available and now restores every matched weapon independently.
- Sneak's logout handler requires an active `m_Table` entry before invoking
  the existing cleanup. The spell recipient's logout still removes both skill
  modifiers, releases the action, and reveals the recipient. An unrelated
  player or the caster of another player's Sneak is left alone.

`POST-BATCH-E-37A` and `POST-BATCH-E-38A` previously marked these IDs Fixed
after adding null/deleted-mobile guards. Those historical results remain
unchanged; they did not repair subscription accumulation. The canonical
repair-backlog rows, risk findings, and active overlay now reference this
verified repair. The focused review rows are `RLO-01` and `RLO-02` in
[`post-batch-research-logout-review.csv`](post-batch-research-logout-review.csv).

## Runtime entry points and source traces

| Source and symbol | Trigger / access | Guards and behavior |
| --- | --- | --- |
| `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchEnchant.cs:54`, `ResearchEnchant.Initialize` | Internal script initialization | One static Logout subscription, even after repeat initialization. |
| `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchEnchant.cs:65`, `ResearchEnchant.OnLogout` | Global Logout dispatch | Null event, null/deleted mobile checks; selects orbs by `EnchantOwner`. No map/range gate is appropriate for cleanup after a weapon/orb moves. |
| `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchEnchant.cs:199`, `InternalTimer`; `:334`, `ResearchEnchantStone.RunTime` | Expiry; ten seconds after loading an orb | The timer retains its own orb; null/deleted orbs do nothing. Ordinary duration and load-delay values are unchanged. |
| `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchEnchant.cs:236`, `EndEffect`; `:268`, `FindByOwner`; `:292`, `OnAfterDelete` | Logout, expiry, loaded-orb cleanup, deletion | Active-orb snapshot; direct serial lookup; deleted/missing weapon and null name guards; original name-marker condition retained; delete orb and unregister it. |
| `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchSneak.cs:48`, `ResearchSneak.Initialize`; `:62`, `OnLogout` | Internal initialization; global Logout dispatch | One static subscription; null event/mobile and deleted mobile guards; requires the logging-out mobile's active Sneak entry. |
| `Data/Scripts/Magic/Research/ResearchCommands.cs:174`, `CastEnchant`; `:30`, `CastSneak` | Existing Player commands | Command eligibility, spell resources, targeting and effects remain in their existing paths. No new command or access policy. |
| `Data/System/Source/World.cs:1134`, `World.FindItem`; `Data/System/Source/EventSink.cs:1315`, `InvokeLogout` | Existing framework services | Dictionary lookup by serial and synchronous event dispatch; neither core file changed. |

## Save compatibility

`Server.Items.ResearchEnchantStone` retains its namespace, type, `Serial`
constructor, version 1, fields, and exact positional write/read sequence:

| Write order after `base.Serialize` | Matching read after `base.Deserialize` |
| --- | --- |
| `(int)1` | `ReadInt()` version |
| `(Mobile)EnchantOwner` | `ReadMobile()` |
| `EnchantSerial` (int) | `ReadInt()` |
| `EnchantName` (string) | `ReadString()` |
| `EnchantDmg` (int) | `ReadInt()` |
| `EnchantHue` (int) | `ReadInt()` |

Current serializer locations: `ResearchEnchant.cs:306` / `:318`. Registration
in the memory-only orb list occurs after the existing reads; no new value is
saved or consumed. Before/after ordered-write/read comparison passed. The
fixture round-trips actual orb records, verifies all five fields and a trailing
sentinel, and exercises cleanup after deserialization for two owners with
different original weapon properties.

No migration is introduced. The separate historical-version review
`RB-01376` remains outside this batch; its migration-policy status is unchanged.
The older heuristic serialization register incorrectly qualified the orb under
`Server.Spells.Research`; current source declares it under `Server.Items`.
This repair does not rename it or claim compatibility with unprovided historic
save variants.

## Verification

All commands ran from `D:\ConficturaUO-main` unless stated otherwise.

- `powershell -NoProfile -ExecutionPolicy Bypass -File scripts/Test-ResearchLogout.ps1`:
  passed. The runner copies runtime `.cs` sources and required local dependencies
  to a fresh ignored directory, with no saves or service configuration.
- Source build: Visual Studio 2022 MSBuild `Data/System/Source/Server.csproj`,
  Release/x86, with output and intermediate directories redirected to that
  isolated directory: passed, exit 0.
- `RuntimeScriptCompileTruth`: the freshly built `ConficturaServer.exe
  -compileonly -nocache` passed, exit 0, and printed
  `Scripts: Compile-only verification completed successfully.` This is the
  server's recursive runtime compiler, not a `Scripts.csproj` build.
- Production-method fixture: passed 46 assertions covering repeat initialization,
  repeated/cancelled/failed casts, unrelated subscribers, null/deleted event
  inputs, ownership, moved orbs/weapons, repeated cleanup, expiry, stale timers,
  deleted/unnamed weapons, serialization, independent load cleanup, all-effects
  cleanup, and Sneak recipient/ordinary-hiding behavior. Only eligibility/resource
  checks are overridden for deterministic targeting. After strengthening the
  distinct-original-properties case, the fixture was recompiled against the same
  verified script assembly and all 46 assertions passed again. An initial manual
  rerun used a relative path that `csc` misparsed; the absolute-path rerun passed.
- Fresh inventory plus `New-RuntimeHookMap.ps1` in
  `output/research-logout-audit`: passed. Inventory: 6,718 C# files, 6,598 runtime
  scripts/project includes, zero missing/unlisted sources. Hook scan: 6,695
  markers, including 11 rows for these two files. The existing owner maps were
  supplied to the hook mapper; this focused batch does not regenerate unrelated
  canonical audit tables. The table above records the manually verified guards
  rather than treating the scanner's file-wide heuristic labels as proof.
- Static checks: unchanged serializer write/read order/version; zero `World.Items`
  references in Enchant; exactly one Logout subscription per spell, outside
  `OnCast`; no namespace/type/project/path changes; CSV schema/row preservation,
  changed Markdown links/source locators, and `git diff --check`: passed.

Ignored evidence directory:
`output/research-logout-verification/438619ee9aa14e618a71ffd0c1306626/`, containing
`server-build.log`, `runtime-compile.log`, `fixture-build.log`, and `regression.log`.
Fresh scan evidence is under `output/research-logout-audit/`. No generated
executable, cache, save, or log is included in the commit.

Full shard initialization/listeners, connected-client gameplay, and production
profiling were not run: verification deliberately used an isolated compiler
and fixture with no world saves or listeners. The fixture invokes both changed
initializers and real event dispatch directly. No live deployment or measured
live performance claim is made. Optional solution/Scripts project builds were
not needed because project files and shared core code were unchanged.

## Deployment and rollback

Deploy both scripts together and restart the server so old instance subscribers
in the running process are discarded and the new static initializers execute.
On a staging shard, cast each spell repeatedly, cancel targeting, log out an
unaffected player and each effect recipient, and reload a disposable save with
two enchanted weapons. Confirm the correct effects expire and original weapon
properties return. Existing orphaned weapons whose tracking orbs were lost
before this repair cannot be reconstructed from missing data.

Rollback is a revert of this focused commit followed by a restart. Save format
and project visibility are unchanged; rollback also restores the original bugs.
