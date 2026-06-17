# SOURCE-BATCH-017 Candidate Discovery Closeout

Reviewed at: 2026-06-16T18:59:28.2920934-05:00

## Summary

`SOURCE-BATCH-017` discovery ran after `SOURCE-BATCH-016` exhausted `docs/codebase-audit/outputs/source-batch-012-candidate-discovery.csv`.

The discovery identified five additional narrow, non-gated, zero-overlay guard repair candidates. `SB017-CAND-001` / `SOURCE-BATCH-017 PromotionalToken Guard Repair` is the recommended next implementation target.

## Candidate Summary

| Candidate | Proposed batch | Target | Gate hits | Active overlay rows | Recommendation |
| --- | --- | --- | ---: | ---: | --- |
| `SB017-CAND-001` | `SOURCE-BATCH-017` | `Data/Scripts/Items/Misc/PromotionalToken.cs` | 0 | 0 | Recommended next target |
| `SB017-CAND-002` | `SOURCE-BATCH-018` | `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs` | 0 | 0 | Strong follow-up |
| `SB017-CAND-003` | `SOURCE-BATCH-019` | `Data/Scripts/Items/Misc/Dyes/AllDyeTubsArmor.cs` | 0 | 0 | Good follow-up |
| `SB017-CAND-004` | `SOURCE-BATCH-020` | `Data/Scripts/Items/Misc/Dyes/AllDyeTubsWeapon.cs` | 0 | 0 | Good follow-up |
| `SB017-CAND-005` | `SOURCE-BATCH-021` | `Data/Scripts/Items/Misc/Dyes/AllDyeTubsFurniture.cs` | 0 | 0 | Good follow-up |

## Exclusions

- `Moonstone` was excluded despite zero gate/overlay counts because it touches world/region/travel escape behavior.
- `Bola`, `LuckyHorseShoes`, `SlayerDeed`, and level/god weapon files were excluded because they touch combat, progression, or equipment policy surfaces better handled by a separate decision.
- `MagicCandle`, `MagicTorch`, `MagicLantern`, `PlayerVendorDeed`, `KeyRing`, `Origami`, and `ColoringBook` were excluded from this clean list because active overlay rows or broader state-review concerns make them less suitable for this strict zero-overlay discovery batch.
- Project/config/data, XML/config/data, serializer migration, staff/access, balance/economy, region/map, and reorganization work remain excluded.

## Verification

- `source-change-executive-decision-intake.csv` confirms `EXEC-0001=Ask for candidate discovery list`.
- `source-change-executive-decision-intake.csv` confirms `EXEC-0002=Sequential runner only for non-gated rows`.
- `source-batch-controller-roadmap-status.csv` confirms `SOURCE-BATCH-017+` is `PendingConcreteSourceTarget`.
- Candidate files were checked against `post-batch-y-source-change-gate-register.csv`; selected candidates have `PostBatchYGateHitCount=0`.
- Candidate files were checked against `post-audit-active-backlog-status.csv` with normalized slash/backslash paths; selected candidates have `ActiveOverlayRows=0`.
- No source, project, XML/config/data, serializer, namespace/type, gameplay, staff/access, region/map, or reorganization files changed in this discovery step.
- `git diff --check` passed for the discovery artifacts.

## Result

- `SOURCE-BATCH-017` discovery is complete.
- `SOURCE-BATCH-017 PromotionalToken Guard Repair` is the recommended next implementation target.
- Source implementation should happen in the next source batch, with one commit after verification.
