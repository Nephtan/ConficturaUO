# POST-BATCH-B Save Compatibility Closeout

Generated: 2026-06-08T01:54:18.0000000-05:00

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
| `ConfirmedIssue` | 2 |
| `FalsePositive` | 47 |
| `Fixed` | 5 |
| `IntentionalLegacy` | 52 |
| `ReviewedNoChange` | 14 |
| `SafeNoChange` | 182 |

`NeedsMigrationPlan` and `NeedsHumanDecision` do not remain in the triage CSV. The two Druidism rows are fixed in the active overlay after `POST-BATCH-B-30B`.

## Stop Point

Stop before `POST-BATCH-C`. Two active save-compatibility source issues remain, and one requires a human save-policy decision before source repair.

| Source ID | System | File | Evidence | Decision Needed | Next Safe Action |
| --- | --- | --- | --- | --- | --- |
| `SERIAL-0032` | `Custom:Government System` | `Data/Scripts/Custom/Government System/Items/Stones/CityResurrectionStone.cs` | `Serialize` dereferences `m_ghosts.Count` while a newly constructed unused stone can leave `m_ghosts` null. | None if source fix only initializes `m_ghosts` or writes zero count while preserving version and field order. | Repair in a focused source-safe save batch after the `SERIAL-1356` policy decision. |
| `SERIAL-1356` | `Quests:Summon` | `Data/Scripts/Quests/Summon/SummonPrison.cs` | `Serialize` writes `PrisonerFullNameUsed` and `PrisonerClothColorUsed` before `PrisonerSerial`; `Deserialize` reads `PrisonerSerial` before those integers. | Approve reader-order repair that preserves current writer order/version, or require a migration branch if older production saves used the reader order as the intended shape. | Do not patch until this save-policy decision is made. |

## Verification

- `git status --short` was clean before each focused review batch.
- `rg --files -g AGENTS.md` was rerun for each batch.
- CSV invariant checks passed after each subbatch: 304 total rows; reviewed + queued equals 304; reviewed rows have decisions, evidence, actions, verification, batch IDs, and timestamps; queued rows have no decision or reviewed batch.
- Active overlay invariant checks passed: one save overlay row exists for each reviewed non-ServerCore save disposition.
- Review-only batches confirmed `git diff --name-only -- Data` was empty.
- `git diff --check` and `git diff --cached --check` passed for each committed batch, with only expected CRLF warnings from this checkout before staging.

## Recommendation

Run a focused save-source repair batch next for `SERIAL-1356` and `SERIAL-0032` only after the `SERIAL-1356` save-policy decision is made. Verify that batch with `New-SerializationRegister.ps1`, `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache`. Do not begin `POST-BATCH-C` until these active save issues are fixed or explicitly accepted/deferred.
