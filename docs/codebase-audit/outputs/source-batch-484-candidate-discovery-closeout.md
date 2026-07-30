# SOURCE-BATCH-484 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-484+` was pending fresh candidate discovery after `SOURCE-BATCH-483`. Discovery selected one zero-gate, zero-overlay display guard candidate for implementation.

## Recommended Candidate

`SB484-CAND-001` / `SOURCE-BATCH-484` / `DecayedCorpse Label Guard Repair`

- File: `Data/Scripts/Items/Misc/Bodies/Corpses/DecayedCorpse.cs`
- System: `Items:Misc / Bodies / DecayedCorpse`
- Behavior: add stale/null mobile and deleted source item guards to `DecayedCorpse.OnSingleClick(Mobile from)` before sending the localized remains label.
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`

## Skipped Candidate Families

- Broad base item classes such as `BaseArmor.cs`, `BaseClothing.cs`, and `BaseRunicTool.cs`: wide shared behavior or crafting/economy surfaces.
- Boat, house, moongate, teleporter, runebook, recall-rune, and waypoint files: travel/housing/command/policy surfaces.
- `BagOfSending.cs`: region/translocation and charge-use behavior.
- `UnknownLiquid.cs` and `UnknownKeg.cs`: exact-file gate/overlay clean, but already have committed source-batch rows from `SOURCE-BATCH-232` and `SOURCE-BATCH-233`.
- Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization candidates.

## Result

`SOURCE-BATCH-484` should implement `SB484-CAND-001` only if pre-commit verification confirms the source diff stays guard-only and does not alter decay timer behavior, container/content behavior, valid label text, or serialization.
