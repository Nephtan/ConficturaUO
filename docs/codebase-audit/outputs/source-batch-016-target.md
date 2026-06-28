# SOURCE-BATCH-016 Target: MagicPigment Guard Repair

Reviewed at: 2026-06-16T18:52:04.9029004-05:00

## Target

- Batch: `SOURCE-BATCH-016`
- Candidate: `SB012-CAND-005`
- Behavior: add stale/null/backpack guards to `MagicPigment.OnDoubleClick(Mobile from)` and `DyeTarget.OnTarget(Mobile from, object targeted)` before mobile, source pigment, target ownership/backpack, or hue mutation state is evaluated.
- System: `Items:Misc / Dyes / MagicPigment`
- File: `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- Randomized pigment names.
- Backpack-use message `1060640`.
- Target range.
- Backpack-as-target rule.
- In-backpack paint rule.
- Stackable and item ID exclusions.
- Hue assignment, including `0x2EF` reset.
- `RevealingAction`.
- Sound `0x23F`.
- Success/failure messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-016 MagicPigment Guard Repair

Implement SB012-CAND-005 from docs/codebase-audit/outputs/source-batch-012-candidate-discovery.csv.

Add stale/null/backpack guards to Data/Scripts/Items/Misc/Dyes/MagicPigment.cs:
- Return immediately when Mobile from is null or deleted.
- Treat deleted MagicPigment state, missing backpacks, or the pigment outside the backpack as the existing localized pack failure 1060640.
- Treat null/deleted target item state as the existing cannot-paint failure.
- Preserve randomized names, backpack rules, exclusions, hue assignment/reset, RevealingAction, sound, messages, serialization, and layout.

Verify POST-BATCH-Y gate hits=0, active overlay rows=0, targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, and generated root artifact restoration before staging.
```
