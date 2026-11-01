# Unnamed creature death crashes - 2026-11-01

## Result

The reported crash was reproduced through an actual local client in the copied
world's real Atlantis area. A normal Meteor Swarm cast by an existing saved mage
killed an existing hidden, null-named `BaseAssembly` near Triton. Original code
produced the same `NullReferenceException` and `DropReagent -> OnBeforeDeath ->
AOS.Damage -> SpellDamageTimerAOS` stack.

The same action in a fresh patched copy completed the creature's death, credited
the mage and created a corpse without crashing. Nearby enemies subsequently killed
the test mage, while the server continued responding. This is death-pipeline
acceptance, not a boss-fight win. Both preservation/outcome verifiers passed.
Original and private saves were unchanged; owned processes/listeners were stopped.
Production deployment and a current-live-world inspection were not performed.

## Diagnosis and changes

The supplied report, timestamped 2026-11-01 04:01:14, matches original
`Behavior.cs:6402` in starting commit `fc7f145d`:

```csharp
if ((monster.Name).Contains("beetle"))
```

`Mobile.Name` can return null. The creature itself was already dereferenced by
the fame/slayer checks before this statement. An isolated production-method call
also reproduced the null-name exception.

`DropReagent` now captures `monster.Name ?? String.Empty` for the beetle,
werewolf/wolf man, frog/toad and spider groups. Unnamed creatures skip name matches
and retain type-based loot. Stored names, case sensitivity, random calls,
quantities and independent rule groups are preserved.

A native Fireball test with an explicitly null-named fixture Triton exposed a
second crash later in death processing: `LogBattles` called `mob.Name.Split('[')`
before its fame filter. `Logs.cs` now substitutes `an unnamed creature` for a null
name. Fame filtering, public/private policy, titles and bracket removal remain.

These are script-only fixes. Core source/executable, project membership,
namespaces, serialized fields/versions and save formats are unchanged. Tests
remain outside the live `Data/Scripts` tree.

## Actual Atlantis evidence and conditions

Matching-serializer inventory of the October 17 offline test world found 521
Atlantis mobiles, including one null name:

| Saved object | Original state |
| --- | --- |
| `BaseAssembly`, serial `411573` (`0x647B5`) | Atlantis `(4270,2619,-127)`, hidden, body 0, null name, unblessed, 1/1 health |
| Triton, serial `107129` | Atlantis `(4254,2610,-127)`, named `Triton`, titled `the Cursed King`, 13,500/13,500 health |

`BaseAssembly`'s constructable constructor does not assign a name. The natural
test setup did not create, rename, reveal, weaken, freeze or relocate either NPC. Its
normal AI moved the assembly to `(4269,2620,-127)` before the lethal cast. This
establishes a local historical-world reproduction; the supplied report alone
cannot establish the live victim's serial or the player's exact spell.

The comparison preserved the saved mage's skills, equipment and supplies, and all
NPCs, AI, spawners and regions. Setup temporarily routed the existing copied test
login to that character and staged initial travel to `(4268,2621,-127)`, a nearby
position verified to have line of sight. A bounded ClassicAssist macro sent
normal client Meteor Swarm and ground-target packets at `(4270,2619,-127)`.
The observer did not cast, answer targeting, apply damage or kill a creature.
Loopback binding, disabled autosave/integrations/restart and eight documented
isolation overlays were confined to disposable copies.

## Verification

Starting worktree was clean on `main`, cwd `D:\ConficturaUO-main`.

- `& ./scripts/Test-DropReagent.ps1`: isolated Release/x86 `Server.csproj` build,
  forced `-compileonly -nocache` runtime script compile and **47 assertions passed**.
  Coverage: null/empty names and repeated calls; all name groups/quantities; case
  sensitivity; unnamed undead, blood elemental, plant and pixie drops; Triton;
  and battle-log fame/public/private/formatting rules. Logs:
  `output/drop-reagent-verification/e6f5bcaf46174f82b33ce361bfa8a660/`.
- **Original native reproduction passed:**
  `D:\ConficturaUOTestTools\atlantis-natural-baseline-20261101-004\evidence\natural-verification.json`.
  Both changed scripts were restored to original `HEAD` blobs in this copy.
- **Patched native regression passed:**
  `D:\ConficturaUOTestTools\atlantis-natural-patched-20261101-001\evidence\natural-verification.json`.
  At `2026-11-01T17:36:30.9983855Z`, the observer recorded completed subject death,
  killer `0xD5B8`, corpse `0x40004A68`. Fresh telemetry/client rendering continued
  after the subsequent ordinary player death.
- Both verifiers confirmed unchanged original/private saves, production and
  isolation/fixture inputs, native dependencies and saved capabilities/encounter
  at staging; normal client spell activity; expected outcomes; and owned teardown.
  Forced crashed/no-save exits are not graceful logout claims.
- Static checks: PowerShell parsing, unsafe-name scans, local documentation links
  and `git diff --check`. IDE builds were unnecessary: no project files or
  production source membership changed.

Retained failed/partial attempts include: incorrect test AI-stop API, early
launcher lifetime failure, sandbox loopback timeout, encumbered-actor supply
placement, Blessed-caster prohibition, the follow-on logger crash, premature
offline travel/death, missing native zlib login dependencies, and the first
natural cast's lack of line of sight followed by normal player death. None is
counted as native success. Cleanup's strict exited-server guard and JSON date
comparison rejections were retained; final cleanup independently checked exact
PID/path/hash/start time and private ports before/after forced termination.

Preparation now includes both existing native zlib DLLs. Atomic request and
ownership guard helpers/patterns were reused from `ChampRevamp` commit
`535ffaf21705e9f520e221fd06dbd0cea768e701`; no branch gameplay code was imported.
Reusable tools, provenance, scoped feedback and retained evidence are under
`D:\ConficturaUOTestTools\tools\drop-reagent\` and the run directories.

Tested production SHA-256:

- `Behavior.cs`: `4DAE32622F5264467E5F41E9982D16F8BBF4E65A1D205FAEDE060E86FB8183AE`
- `Logs.cs`: `558DF8ECE3ED2899D05305E9248F70243986320074E6486209D8208C29CB13B2`

## Source traces and entry points

| File | Symbols/evidence |
| --- | --- |
| [Behavior.cs](../../Data/Scripts/Mobiles/Base/Behavior.cs) | `DropReagent`, null-safe name capture at line 6358; `FameBasedLevel` |
| [Logs.cs](../../Data/Scripts/System/Misc/Logs.cs) | `LogBattles`, null-safe label at line 921 before `Split` |
| [BaseCreature.cs](../../Data/Scripts/Mobiles/Base/BaseCreature.cs) | Player-attributed `OnBeforeDeath` helper call at line 10049; `OnDeath` logger call at line 10613 |
| [Mobile.cs](../../Data/System/Source/Mobile.cs) | Nullable `Name`; `Damage`/`Kill` pipeline |
| [BaseAssembly.cs](../../Data/Scripts/Custom/Invasion%20System/Add%20Ins/Metal%20Assemblies/ASSEMBLY/BaseAssembly.cs) | Constructor without name; existing serial constructor/version 0 serialization unchanged; base `OnDeath` delegation |
| [Triton.cs](../../Data/Scripts/Custom/Mobiles/Godfrey2026/Triton.cs) | Constructor assigns name/title; base death delegation |
| [MeteorSwarm.cs](../../Data/Scripts/Magic/Magery/Magery%207th/MeteorSwarm.cs) | Five-tile region/LOS/blessed filtering and delayed damage |
| [DropReagentRegression.cs](../../tests/DropReagentRegression.cs) | Production helper/logger assertions, reflection for internal enclosing classes |
| [Test-DropReagent.ps1](../../scripts/Test-DropReagent.ps1) | Reproducible isolated build/compiler/component runner |

Ordinary player-attributed creature deaths trigger these existing helper calls.
No command, gump, hook registration, access, eligibility, attribution, map or range
rule changed. No save migration or new serialization surface is required.

## Deployment

Deploy both `Behavior.cs` and `Logs.cs`, then restart under the normal shard
procedure so runtime scripts compile. The tracked core executable needs no
replacement. Deployment was not performed. Rollback is a focused source revert
and restart; it requires no migration and restores the original crash risks.
