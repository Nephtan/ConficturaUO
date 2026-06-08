# POST-BATCH-B Save Compatibility Closeout

Generated: 2026-06-08T14:34:52.8340561-05:00

## Summary

`POST-BATCH-B` reviewed all 304 P0 critical save-compatibility rows. No queued rows remain in `post-batch-b-save-compatibility-triage.csv`.

Decision counts:

| Decision | Count |
| --- | ---: |
| `ConfirmedIssue` | 4 |
| `FalsePositive` | 54 |
| `IntentionalLegacy` | 58 |
| `SafeNoChange` | 188 |

Active overlay status:

| Active Status | Count |
| --- | ---: |
| `FalsePositive` | 47 |
| `Fixed` | 7 |
| `IntentionalLegacy` | 52 |
| `ReviewedNoChange` | 14 |
| `SafeNoChange` | 182 |

`NeedsMigrationPlan`, `NeedsHumanDecision`, and active save `ConfirmedIssue` rows do not remain. The two Druidism rows are fixed in the active overlay after `POST-BATCH-B-30B`, and the two remaining active save issues are fixed after `POST-BATCH-B-34A`.

## Source Fix Closeout

`POST-BATCH-B-34A` applied the likely human save-policy decision for `SERIAL-1356` and repaired both remaining active save issues without changing writer layout, version numbers, serialized type names, namespaces, or file locations.

| Source ID | System | File | Fix | Verification |
| --- | --- | --- | --- | --- |
| `SERIAL-0032` | `Custom:Government System` | `Data/Scripts/Custom/Government System/Items/Stones/CityResurrectionStone.cs` | `Serialize` writes a zero ghost count when `m_ghosts` is null, preserving version 1 count/entry/sign order. | `New-SerializationRegister.ps1`; `Server.csproj` Debug/x86 build; `.\ConficturaServer.exe -compileonly -nocache` with no `Listening:` output. |
| `SERIAL-1356` | `Quests:Summon` | `Data/Scripts/Quests/Summon/SummonPrison.cs` | `Deserialize` now reads `PrisonerFullNameUsed`, `PrisonerClothColorUsed`, then `PrisonerSerial`, matching the current writer order and preserving version 1. | `New-SerializationRegister.ps1`; `Server.csproj` Debug/x86 build; `.\ConficturaServer.exe -compileonly -nocache` with no `Listening:` output. |

## Verification

- `git status --short` was clean before each focused review batch.
- `rg --files -g AGENTS.md` was rerun for each batch.
- CSV invariant checks passed after each subbatch: 304 total rows; reviewed + queued equals 304; reviewed rows have decisions, evidence, actions, verification, batch IDs, and timestamps; queued rows have no decision or reviewed batch.
- Active overlay invariant checks passed: one save overlay row exists for each reviewed non-ServerCore save disposition.
- Review-only batches confirmed `git diff --name-only -- Data` was empty.
- `git diff --check` and `git diff --cached --check` passed for each committed batch, with only expected CRLF warnings from this checkout before staging.
- `POST-BATCH-B-34A` regenerated serialization outputs, built `Data/System/Source/Server.csproj` Debug/x86, and passed `.\ConficturaServer.exe -compileonly -nocache` without listener output.

## Recommendation

`POST-BATCH-B` is complete and no longer blocks `POST-BATCH-C`. Start `POST-BATCH-C` with P0 runtime hooks and `PlayerMobile` coupling review, and keep any source fixes narrow enough to avoid serialization, public API, namespace, type-name, and file-location changes.
