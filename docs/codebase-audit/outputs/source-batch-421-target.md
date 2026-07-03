# SOURCE-BATCH-421 Guillotine Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-421`
- Candidate: `SB421-CAND-001`
- System: `Items:Misc / Guillotine`
- File: `Data/Scripts/Items/Misc/Guillotine.cs`
- Behavior: add stale/null mobile, deleted source guillotine, and delayed-callback deleted-source guard coverage around Guillotine interactions.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Range and line-of-sight check.
- Localized reach failure message.
- `Visible`, `ItemID`, and `m_NextUse` eligibility.
- Damage chance and `Utility.Dice(2, 10, 5)` damage amount.
- Hurt sound.
- `Ouch!` public overhead message.
- Blade sound `0x387`.
- Blood creation.
- `Timer.DelayCall` timings.
- Item ID down/reset behavior.
- Deserialize reset behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
