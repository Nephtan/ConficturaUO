# SOURCE-BATCH-018 Target: MagicalDyes Guard Repair

Reviewed at: 2026-06-16T19:08:27.3028568-05:00

## Target

- Batch: `SOURCE-BATCH-018`
- Candidate: `SB017-CAND-002`
- Behavior: add stale/null/backpack guards to `MagicalDyes.OnDoubleClick(Mobile from)` and `DyeTarget.OnTarget(Mobile from, object targeted)` before mobile, source dye, target backpack, hue mutation, Bottle return, or dye consumption state is evaluated.
- System: `Items:Misc / Dyes / MagicalDyes`
- File: `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- Randomized dye names and hues.
- Backpack-use message `1060640`.
- Target range.
- Backpack-as-target rule.
- In-backpack dye rule.
- Stackable and item ID exclusions.
- Hue assignment, including `0x2EF` reset.
- `RevealingAction`.
- Sound `0x23E`.
- Bottle return.
- `Consume()` semantics.
- Success/failure messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-018 MagicalDyes Guard Repair

Implement SB017-CAND-002 from docs/codebase-audit/outputs/source-batch-017-candidate-discovery.csv.

Add stale/null/backpack guards to Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs:
- Return immediately when Mobile from is null or deleted.
- Treat deleted MagicalDyes state, missing backpacks, or the dye outside the backpack as the existing localized pack failure 1060640.
- Treat null/deleted target item state as the existing cannot-dye failure.
- Preserve randomized names/hues, backpack rules, exclusions, hue assignment/reset, RevealingAction, sound, Bottle return, Consume, messages, serialization, and layout.

Verify POST-BATCH-Y gate hits=0, active overlay rows=0, targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, and generated root artifact restoration before staging.
```
