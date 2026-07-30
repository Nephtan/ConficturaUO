# SOURCE-BATCH-447 Candidate Discovery Closeout

`SOURCE-BATCH-447` ran fresh non-gated candidate discovery after `SOURCE-BATCH-446`.

## Result

Recommended implementation target: `SB447-CAND-001` / `BankCheck` guard repair.

## Evidence

- Expected source file: `Data/Scripts/Items/Misc/BankCheck.cs`
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Existing source evidence: `BankCheck.OnSingleClick` dereferenced `from` before guard, and `BankCheck.OnDoubleClick` called `from.FindBankNoCreate()` before guard.
- Completed-file evidence: `TaxidermyKit.cs` was excluded because `SOURCE-BATCH-076` already committed its guard repair.

## Skips

- `TaxidermyKit.cs` was skipped as already completed.
- staff/admin and powerscroll purchase candidates were skipped as staff workflow or economy/reward policy.
- addon, pet, potion, fishing, travel, and spell target families were skipped as policy-sensitive.
- remaining Government, Invasion, Homestead, StaffTools, economy/reward tuning, region/map, serializer, project/config/data, XML/config/data, and reorganization surfaces remain outside this runner.

## Decision

Proceed with `SOURCE-BATCH-447 BankCheck Guard Repair`. Keep `SOURCE-BATCH-448+` pending fresh discovery after the source batch commits.
