# SOURCE-BATCH-083 HairRestylingDeed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-083`
- Candidate: `SB082-CAND-002`
- Behavior: add stale/null/mobile/backpack/source-deed/gump-response guards to `HairRestylingDeed.OnDoubleClick(Mobile from)` and `InternalGump.OnResponse(NetState sender, RelayInfo info)`.
- System: `Items:Deeds / HairRestylingDeed`
- Source file: `Data/Scripts/Items/Deeds/HairRestylingDeed.cs`

## Fence Result

- POST-BATCH-Y gate hits for `HairRestylingDeed.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Deeds/HairRestylingDeed.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source deed is deleted.
- Treat missing backpacks or source deeds outside the backpack in `OnDoubleClick` as the existing backpack-use failure using localized message `1042001`.
- Return immediately from `InternalGump.OnResponse` when the stored mobile or source deed is null/deleted.
- Treat missing backpacks or source deeds outside the backpack during gump response as the existing backpack-use failure using localized message `1042001`.

## Must Stay Unchanged

- Localized message `1042001`.
- `InternalGump` layout arrays and button IDs.
- `HumanArray` and `ElvenArray` hair IDs.
- `PlayerMobile.SetHairMods`, `HairItemID`, and `RecordsHair` behavior.
- Source deed `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Shape

```text
/goal SOURCE-BATCH-083 HairRestylingDeed Guard Repair

Implement SB082-CAND-002 from source-batch-082-candidate-discovery.csv. Add stale/null/mobile/backpack/source-deed/gump-response guards to Data/Scripts/Items/Deeds/HairRestylingDeed.cs while preserving localized message 1042001, gump layout and button IDs, HumanArray/ElvenArray hair IDs, PlayerMobile hair update behavior, deed Delete behavior, serialization, layout, and all gated policy surfaces. Verify gate hits=0, active overlay rows=0, serializer diff clean, forbidden surfaces clean, Server.csproj Debug/x86 build, runtime compile-only, git diff --check, then commit as: fix: guard HairRestylingDeed interactions.
```
