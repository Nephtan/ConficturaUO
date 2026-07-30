# SOURCE-BATCH-219 ValentinesCard Guard Repair

## Target

- Candidate: `SB217-CAND-003`
- Behavior: add stale/null/mobile/source-card and target-callback guards to `ValentinesCard.OnDoubleClick(Mobile from)` and `OnTarget(Mobile from, object targeted)`.
- System: `Items:Special / ValentinesCard`
- File: `Data/Scripts/Items/Special/ValentinesCard.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source cards before backpack checks, target assignment, signer reads, or property mutation. Return immediately for deleted mobile targets before reading target names or assigning card fields.

## Must Stay Unchanged

- Unsigned-card gating.
- Backpack-use message `1080063`.
- Target prompt `1077497`.
- Player/self/NPC/invalid-target messages `1077498`, `1077495`, `1077496`, and `1077488`.
- `To`/`From` assignment and property invalidation.
- Label, blessed loot behavior, hue, and randomized label metadata.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted `OnDoubleClick` and `OnTarget` guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
