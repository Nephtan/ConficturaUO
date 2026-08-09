# SOURCE-BATCH-470 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-470` ran fresh non-gated candidate discovery after `SOURCE-BATCH-469` and selected one clean guard repair candidate.

## Recommended Target

- Candidate: `SB470-CAND-001`
- Batch: `SOURCE-BATCH-470`
- System: `Items:Construction / Addons / RejuvinationAnkh`
- File: `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`
- Behavior: add stale/null mobile, deleted source component, and malformed delayed-callback state guards to `RejuvinationAddonComponent` interaction paths.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Construction/Addons/RejuvinationAnkhs.cs`: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Candidate Notes

`RejuvinationAnkhs.cs` was selected because its double-click path and delayed callback could dereference invalid mobile or callback state. The existing restore randomization, two-hour lock, movement messaging, addon components, and serializers remain unchanged.

Discovery continued to exclude staff commands, travel/gate behavior, thief quest rewards, merchant/economy cash-out behavior, taxidermy target/corpse flows, serializer layout changes, project/config/data changes, XML/config/data changes, and reorganization candidates.

## Result

Proceed with `SOURCE-BATCH-470 RejuvinationAnkh Guard Repair`.
