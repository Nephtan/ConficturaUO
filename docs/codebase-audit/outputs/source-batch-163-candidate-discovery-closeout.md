# SOURCE-BATCH-163 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-163+` discovery found a clean Bard scroll guard queue after the `source-batch-154-candidate-discovery.csv` Mystic scroll queue was exhausted.

The discovery output is `docs/codebase-audit/outputs/source-batch-163-candidate-discovery.csv`.

## Recommended Target

- Candidate: `SB163-CAND-001`
- Proposed batch: `SOURCE-BATCH-163`
- Target: `ArmysPaeonScroll`
- File: `Data/Scripts/Magic/Bard/Scrolls/ArmysPaeon.cs`
- Behavior: add a stale/null/mobile/source-scroll guard to `ArmysPaeonScroll.OnDoubleClick(Mobile from)` before sending the sheet-music message.

## Candidate Queue

The discovery queue contains 16 one-file Bard scroll candidates:

- `ArmysPaeonScroll`
- `EnchantingEtudeScroll`
- `EnergyCarolScroll`
- `EnergyThrenodyScroll`
- `FireCarolScroll`
- `FireThrenodyScroll`
- `FoeRequiemScroll`
- `IceCarolScroll`
- `IceThrenodyScroll`
- `KnightsMinneScroll`
- `MagesBalladScroll`
- `MagicFinaleScroll`
- `PoisonCarolScroll`
- `PoisonThrenodyScroll`
- `SheepfoeMamboScroll`
- `SinewyEtudeScroll`

## Fence Evidence

- POST-BATCH-Y gate hits for each candidate file: 0
- Active overlay rows for each candidate file: 0
- Previously completed source-batch register matches for each candidate file: 0
- Gated approval crossed: No

## Exclusions

Discovery did not select staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization work.

## Next Step

Implement `SOURCE-BATCH-163` / `SB163-CAND-001` / `ArmysPaeonScroll Guard Repair` if fresh preflight remains zero-gate and zero-overlay.
