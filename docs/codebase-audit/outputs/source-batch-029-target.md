# SOURCE-BATCH-029 Target: Key Interaction Guard Repair

Reviewed at: 2026-06-16T20:14:30-05:00

## Target

- Batch: `SOURCE-BATCH-029`
- Candidate: `SB028-CAND-002`
- Behavior: add stale/null/mobile/source-key/backpack/target guards to `Key.OnDoubleClick`, `RenamePrompt.OnResponse`, `UnlockTarget.OnTarget`, and `CopyTarget.OnTarget` before mobile backpack state, key state, target lock state, or copy-destruction logic is evaluated.
- System: `Items:Misc / Key`
- File: `Data/Scripts/Items/Misc/Key.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Key.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Key.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `KeyValue`, `Link`, and `MaxRange` behavior.
- `UseLocks` behavior.
- `RenamePrompt` flow.
- Unlock target behavior.
- Copy target behavior.
- Tinkering copy skill check.
- 10 percent key destruction chance.
- Existing messages.
- `CheckLOS = false` behavior.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
