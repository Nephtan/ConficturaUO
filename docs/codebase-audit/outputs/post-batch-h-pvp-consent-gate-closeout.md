# POST-BATCH-H PvP Consent Gate Closeout

Generated: 2026-06-13T23:43:09.9206912-05:00

## Summary

POST-BATCH-H-08A did not move Data/Scripts/Custom/PvPConsent to Data/Scripts/Custom/PvP/PvPConsent. The row was classified DeferredMoveGate because its Phase 12 gate requires PvP Consent, Government, Regions, XMLPoints/XMLSpawner, and PlayerMobile policy review, and current audit state still contains deferred/human-gated policy decisions for the Government and XMLPoints/XMLSpawner interactions.

No source, project, XML/config, namespace, serialization, command, hook, public API, or gameplay behavior changed.

## Evidence

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06806` |
| Current source path | `Data/Scripts/Custom/PvPConsent` |
| Proposed target path | `Data/Scripts/Custom/PvP/PvPConsent` |
| Runtime-visible C# files | `5` |
| Total package files | `7` |
| Nested AGENTS.md scopes | `Data/Scripts/Custom/PvPConsent/Gumps/AGENTS.md` |
| ScriptsProjectTruth rows | `5` includes, `5` sources, `0` missing, `0` unincluded |
| Runtime hook rows | `13` (Command=1; Gump=6; Initialize=1; Movement=1; Speech=1; Timer=3) |
| Serialization rows | `5` (Server.Mobiles.GoddessOfProtection:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.NONPKEventConfirmationMoongate:v0:Switch:CountMatchNeedsTypeReview:UnknownWrites=1; Server.Items.NONPKEventMoongate:v1:IfVersionGates:CountMatchNeedsTypeReview:UnknownWrites=3; Server.Items.NONPKSword:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.PKSword:v0:SingleVersionOnly:AlignedByCountAndKnownTypes) |

## Gate Decision

- Satisfied supporting reviews: RB-05382=ReviewedNoChange (PvP Consent PlayerMobile coupling, PHC-023); RB-05383=ReviewedNoChange (PvP Consent PlayerMobile coupling, PHC-024); RB-05403=ReviewedNoChange (PvP Consent PlayerMobile coupling, PHC-025); RB-01754=Fixed (PvP Consent PlayerMobile lifecycle hook, PHE-156); RB-01755=Fixed (PvP Consent PlayerMobile lifecycle hook, PHE-157); RB-01756=Fixed (PvP Consent PlayerMobile lifecycle hook, PHE-158); RB-01757=Fixed (PvP Consent PlayerMobile lifecycle hook, PHE-159); RB-01817=ReviewedNoChange (Regions hook review, PHE-216); RB-05609=DeferredBalanceDecision (PvP Consent <-> Government balance/policy, PBF-007); RB-05610=DeferredBalanceDecision (PvP Consent <-> XMLSpawner/XMLPoints balance/policy, PBF-008); RB-05631=NeedsHumanDecision (XMLSpawner staff tooling policy decision, PBF-029)
- Remaining move blockers: RB-05609:PvP Consent <-> Government balance/policy; RB-05610:PvP Consent <-> XMLSpawner/XMLPoints balance/policy; RB-05631:XMLSpawner staff tooling policy decision
- Decision: `DeferredMoveGate`.

## Verification

- No path-sensitive outputs were regenerated because no move was executed.
- RuntimeScriptCompileTruth and ScriptsProjectTruth remain unchanged and explicitly separate.
- git diff --check: Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors

## Next Safe Action

Resolve the deferred/human-gated PvP Consent policy interactions, then rerun the gate review before any file move. Continue POST-BATCH-H with the next eligible folder cleanup row while this row remains dispositioned.
