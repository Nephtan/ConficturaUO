# SOURCE-BATCH-004 Target

Reviewed at: 2026-06-16T11:15:37.0000000-05:00

## Target

- Batch: `SOURCE-BATCH-004`
- Candidate: `SB004-CAND-001`
- Behavior: add stale/null/backpack guards to ArcaneGem interaction paths.
- System: `Items:Misc / ArcaneGem`
- Expected file: `Data/Scripts/Items/Misc/ArcaneGem.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/ArcaneGem.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Misc/ArcaneGem.cs`: 0
- Gated approval crossed: No

## Must Stay Unchanged

- Arcane charge math.
- Tailoring thresholds.
- Item eligibility and resource restrictions.
- Hue assignment.
- Messages.
- Gem amount/delete behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files.
- XML/config/data files.
- Staff/access behavior.
- Economy/reward tuning.
- Region/map behavior.

## Ready Goal Command

```text
/goal SOURCE-BATCH-004 ArcaneGem Interaction Guard Repair

Implement stale/null/backpack guards in Data/Scripts/Items/Misc/ArcaneGem.cs for ArcaneGem OnDoubleClick and OnTarget only. Preserve arcane charge math, tailoring thresholds, item eligibility, resource restrictions, hue assignment, messages, gem amount/delete behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, and region/map behavior. Verify POST-BATCH-Y gate hits=0, active overlay rows=0, serializer diff unchanged, forbidden surfaces unchanged, Server.csproj Debug/x86 build, compile-only runtime verification, artifact restoration, and git diff --check.
```
