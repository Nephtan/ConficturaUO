# POST-BATCH-H Invasion Gate Closeout

Generated: 2026-06-14T00:02:00.3519907-05:00

## Summary

POST-BATCH-H-10A did not move Data/Scripts/Custom/Invasion System to Data/Scripts/Custom/Events/InvasionSystem. The row was classified DeferredMoveGate because its Phase 12 gate requires staff workflow documentation plus serializer review, and current audit state still contains human-gated staff workflow rows plus serializer caution evidence.

No source, project, XML/config, namespace, serialization, command, hook, public API, or gameplay behavior changed.

## Evidence

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06808` |
| Current source path | `Data/Scripts/Custom/Invasion System` |
| Proposed target path | `Data/Scripts/Custom/Events/InvasionSystem` |
| Runtime-visible C# files | `100` |
| Total package files | `102` |
| Nested AGENTS.md scopes | `Data/Scripts/Custom/Invasion System/Gump/AGENTS.md` |
| RuntimeScriptCompileTruth rows | `100` old-path rows, `6581` total runtime-visible rows |
| ScriptsProjectTruth rows | `100` includes, `100` sources, `0` missing, `0` unincluded |
| Runtime hook rows | `172` (Command=1; Gump=153; Initialize=1; Movement=3; Timer=14) |
| Serialization rows | `76` (NoVersionFound, AlignedByCountAndKnownTypes, NamespaceOrTypeRenameDanger=1; SingleVersionOnly, AlignedByCountAndKnownTypes, NamespaceOrTypeRenameDanger=72; SingleVersionOnly, AlignedByCountAndKnownTypes, UnknownUntilSaveTest=1; Switch, CountMatchNeedsTypeReview:UnknownWrites=1, NamespaceOrTypeRenameDanger=1; Switch, CountMatchNeedsTypeReview:UnknownWrites=7, NamespaceOrTypeRenameDanger=1) |

## Gate Decision

- Current staff/balance gate rows: RB-05605=DeferredBalanceDecision (Random Encounters <-> Invasion balance/policy, PBF-003); RB-05620=DeferredBalanceDecision (Invasion <-> Champions balance/policy, PBF-018); RB-05630=NeedsHumanDecision (Invasion staff workflow ownership, PBF-028); RB-05632=NeedsHumanDecision (Government <-> Invasion staff workflow ownership, PBF-030); RB-05634=NeedsHumanDecision (XMLSpawner <-> Invasion staff workflow ownership, PBF-032); RB-05645=NeedsHumanDecision (Invasion staff tooling policy, PBF-043)
- Serializer caution rows: Server.Mobiles.BaseSpecialCreature:NoVersionFound:AlignedByCountAndKnownTypes:NamespaceOrTypeRenameDanger; Server.Items.GargoyleCityInvasionStone:SingleVersionOnly:AlignedByCountAndKnownTypes:UnknownUntilSaveTest
- Decision: `DeferredMoveGate`.

## Verification

- No path-sensitive outputs were regenerated because no move was executed.
- RuntimeScriptCompileTruth and ScriptsProjectTruth remain unchanged and explicitly separate.
- git diff --check: Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors

## Next Safe Action

Resolve the staff workflow ownership decisions and complete focused serializer source review, then rerun the Invasion move gate before any file move. Continue POST-BATCH-H with the next eligible folder cleanup row while this row remains dispositioned.
