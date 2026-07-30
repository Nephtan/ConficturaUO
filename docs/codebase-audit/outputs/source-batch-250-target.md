# SOURCE-BATCH-250 AuraOfShadows Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-250`
- Candidate: `SB250-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `AuraOfShadows.OnDoubleClick(Mobile from)`.
- System: `Items:Magical / Artifacts / Obsolete / AuraOfShadows`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AuraOfShadows.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AuraOfShadows.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Backpack requirement.
- Localized backpack-use message `1042001`.
- ItemID `2597`/`2594` toggle behavior.
- Existing fallback messages.
- Artifact name property.
- Construction attributes and random hue selection.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-250 AuraOfShadows Guard Repair

Implement SB250-CAND-001 from source-batch-250-candidate-discovery.csv. Add stale/null/mobile/source-item guard to Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AuraOfShadows.cs while preserving backpack requirement, localized message 1042001, ItemID toggle behavior, fallback messages, artifact property, construction attributes, serialization, layout, and all gated policy surfaces. Verify gate hits=0, unresolved overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard AuraOfShadows interactions.
```
