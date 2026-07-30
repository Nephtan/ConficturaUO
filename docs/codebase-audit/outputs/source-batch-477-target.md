# SOURCE-BATCH-477 Dolphin Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-477`
- Candidate: `SB477-CAND-001`
- System: `Mobiles:Animals / Dolphin`
- File: `Data/Scripts/Mobiles/Animals/Misc/Dolphin.cs`
- Behavior: add stale/null mobile and deleted source dolphin guards to `Dolphin.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add an early return in `Dolphin.OnDoubleClick(Mobile from)` when:

- `from == null`
- `from.Deleted`
- `Deleted`

## Must Stay Unchanged

- `Dolphin` creature identity
- AI/fight mode
- stats, resistances, skills, fame/karma, armor, swimming/walking flags
- meat metadata
- existing `AccessLevel.GameMaster` jump eligibility for valid mobiles
- `Jump()` random animation, sound, and location effect behavior
- `OnThink()` slim random jump behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access policy for valid mobiles
- economy/reward tuning
- region/map policy
- reorganization state
