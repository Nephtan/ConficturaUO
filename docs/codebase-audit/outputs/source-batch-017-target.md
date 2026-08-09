# SOURCE-BATCH-017 Target: PromotionalToken Guard Repair

Reviewed at: 2026-06-16T19:03:28.5856904-05:00

## Target

- Batch: `SOURCE-BATCH-017`
- Candidate: `SB017-CAND-001`
- Behavior: add stale/null/backpack/gump-response guards to `PromotionalToken.OnDoubleClick(Mobile from)` and `PromotionalTokenGump.OnResponse(NetState sender, RelayInfo info)` before mobile, `NetState`, token, bank box, reward creation, or token deletion state is evaluated.
- System: `Items:Misc / PromotionalToken`
- File: `Data/Scripts/Items/Misc/PromotionalToken.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/PromotionalToken.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/PromotionalToken.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `OnDoubleClick` pack message `1062334`.
- `PromotionalTokenGump` layout, buttons, and text.
- ButtonID `1` redemption path.
- `CreateItemFor` behavior.
- `BankBox.AddItem` delivery.
- `TextDefinition.SendMessageTo` receive message.
- Token `Delete()` semantics.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-017 PromotionalToken Guard Repair

Implement SB017-CAND-001 from docs/codebase-audit/outputs/source-batch-017-candidate-discovery.csv.

Add stale/null/backpack/gump-response guards to Data/Scripts/Items/Misc/PromotionalToken.cs:
- Return immediately when Mobile from is null or deleted.
- Treat deleted token state, missing backpacks, or token outside the backpack as the existing localized pack failure 1062334.
- Return from stale gump responses when NetState, RelayInfo, mobile, token, or bank box state is invalid.
- Preserve gump layout, ButtonID 1 redemption, CreateItemFor, BankBox.AddItem, receive message, token Delete, serialization, and layout.

Verify POST-BATCH-Y gate hits=0, active overlay rows=0, targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, and generated root artifact restoration before staging.
```
