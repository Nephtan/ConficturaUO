# SOURCE-BATCH-005 Target

Reviewed at: 2026-06-16T11:22:52.2093583-05:00

## Target

- Batch: `SOURCE-BATCH-005`
- Candidate: `SB004-CAND-002`
- Behavior: add stale/null/backpack guards to PowerCrystal interaction paths.
- System: `Items:Misc / PowerCrystal`
- Expected file: `Data/Scripts/Items/Misc/PowerCrystal.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/PowerCrystal.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Misc/PowerCrystal.cs`: 0
- Gated approval crossed: No

## Must Stay Unchanged

- Golem porter target eligibility.
- Charge cap.
- Charge increment behavior.
- Messages.
- Sound/revealing behavior.
- `InvalidateProperties` call.
- Crystal consumption.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files.
- XML/config/data files.
- Staff/access behavior.
- Economy/reward tuning.
- Region/map behavior.

## Ready Goal Command

```text
/goal SOURCE-BATCH-005 PowerCrystal Target Guard Repair

Implement stale/null/backpack guards in Data/Scripts/Items/Misc/PowerCrystal.cs for PowerCrystal OnDoubleClick and PowerTarget.OnTarget only. Preserve golem porter target eligibility, charge cap, charge increment behavior, messages, sound/revealing behavior, InvalidateProperties call, crystal consumption, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, and region/map behavior. Verify POST-BATCH-Y gate hits=0, active overlay rows=0, serializer diff unchanged, forbidden surfaces unchanged, Server.csproj Debug/x86 build, compile-only runtime verification, artifact restoration, and git diff --check.
```
