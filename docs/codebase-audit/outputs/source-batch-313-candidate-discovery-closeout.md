# SOURCE-BATCH-313 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-313+` selected a clean WeaponEngravingTool guard repair.

## Recommended Target

- Candidate: `SB313-CAND-001`
- Batch: `SOURCE-BATCH-313`
- Source file: `Data/Scripts/Items/Special/Veteran Rewards/WeaponEngravingTool.cs`
- Behavior: add stale/null/mobile/backpack/target/gump response guards to `WeaponEngravingTool.OnDoubleClick`, `Recharge`, `Find`, `TargetWeapon.OnTarget`, `InternalGump.OnResponse`, and `ConfirmGump.OnResponse` before reward checks, backpack diamond lookups, target gump creation, text mutation, charge decrement, recharge, or response messaging.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- None. `SOURCE-BATCH-314+` requires fresh candidate discovery after `SOURCE-BATCH-313` is committed.

## Result

`SB313-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change `RewardSystem.CheckIsUsableBy`, target prompt, `BaseWeapon` eligibility, gump layout/text flow, 64-character truncation, blank engraving removal, localized messages, `UsesRemaining` decrement, blue diamond and 100000 gold recharge rules, `IsRewardItem` persistence, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
