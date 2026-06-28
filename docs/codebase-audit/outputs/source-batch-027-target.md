# SOURCE-BATCH-027 Target: ArtifactManual Guard Repair

Reviewed at: 2026-06-16T20:14:00-05:00

## Target

- Batch: `SOURCE-BATCH-027`
- Candidate: `SB025-CAND-003`
- Behavior: add stale/null/backpack/source-manual/target guards to `ArtifactManual.OnDoubleClick`, `BookTarget.OnTarget`, and `LookupTheItem` before mobile backpack state, source manual state, target item state, charge decrement, reward creation, item transfer, or deletion is evaluated.
- System: `Items:Magical / ArtifactManual`
- File: `Data/Scripts/Items/Magical/ArtifactManual.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/ArtifactManual.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/ArtifactManual.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- Charges behavior.
- `UnknownReagent`, `UnknownScroll`, `UnknownLiquid`, `UnknownKeg`, `UnknownWand`, `UnidentifiedArtifact`, and `UnidentifiedItem` outcomes.
- Item transfer and deletion semantics.
- Existing messages.
- Target range.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
