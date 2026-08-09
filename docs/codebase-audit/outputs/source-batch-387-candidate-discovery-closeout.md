# SOURCE-BATCH-387 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-387` ran fresh non-gated candidate discovery after `SOURCE-BATCH-386` and selected `SB387-CAND-001`.

## Recommended Target

- Candidate: `SB387-CAND-001`
- Batch: `SOURCE-BATCH-387`
- System: `Items:Technology / MedicalRecord`
- File: `Data/Scripts/Items/Technology/MedicalRecord.cs`
- Behavior: add stale/null mobile, source-record, and gump response guards to `MedicalRecord.OnDoubleClick(Mobile e)` and `MedicalRecordGump.OnResponse(NetState state, RelayInfo info)` before gump operations or sound playback.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`MedicalRecord.OnDoubleClick` is a narrow read-only interaction that opens a record gump and plays a sound. `MedicalRecordGump.OnResponse` also plays a sound through the response mobile. The guard repair prevents invalid interaction state from dereferencing a null/deleted mobile, deleted source record, or null response state without changing randomized record data, gump layout/text, sound behavior, serialization, or policy behavior.

## Deferred Or Excluded Work

Medical record content changes, randomized name/text generation changes, gump layout redesign, staff/access policy, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
