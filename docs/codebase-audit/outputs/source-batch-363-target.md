# SOURCE-BATCH-363 Prisoner Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-363`
- Candidate: `SB363-CAND-001`
- System: `Quests:Prisoners / Prisoner`
- File: `Data/Scripts/Quests/Prisoners/Prisoner.cs`

## Intended Source Change

Add a local guard to `Prisoner.OnDoubleClick(Mobile from)` so stale/null interaction state cannot close gumps, send `PrisonerGump`, or construct the gump with a deleted source prisoner.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Prisoner randomized setup, `PrisonerReward`, `PrisonerJoin`, `PrisonerType`, `PrisonerName`, `PrisonerTitle`, `PrisonerBody`, `PrisonerSound`, `PrisonerGump` layout/text/buttons, sound `0x0EC`, reward gold behavior, join behavior, prisoner deletion behavior, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
