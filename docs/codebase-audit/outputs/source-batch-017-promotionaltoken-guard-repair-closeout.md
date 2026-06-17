# SOURCE-BATCH-017 Closeout: PromotionalToken Guard Repair

Reviewed at: 2026-06-16T19:03:28.5856904-05:00

## Summary

`SOURCE-BATCH-017` implemented `SB017-CAND-001`, a non-gated `PromotionalToken` interaction and gump-response guard repair in `Data/Scripts/Items/Misc/PromotionalToken.cs`.

The batch adds stale/null/backpack/bankbox safety before mobile, `NetState`, token, reward creation, bank delivery, or token deletion state is evaluated. It does not change reward creation, delivery, messages, persistence, layout, or gated systems.

## Source Changes

- `PromotionalToken.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null or deleted.
- `PromotionalToken.OnDoubleClick(Mobile from)` now uses the existing pack failure when the token is deleted, the backpack is missing, or the token is outside the backpack.
- `PromotionalTokenGump.OnResponse(NetState sender, RelayInfo info)` now returns for null `NetState`, null `RelayInfo`, non-OKAY button responses, null/deleted mobiles, or null/deleted/out-of-backpack token state.
- `PromotionalTokenGump.OnResponse(NetState sender, RelayInfo info)` now returns before reward creation when the mobile has no bank box.

## Preserved Behavior

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

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/PromotionalToken.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/PromotionalToken.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source token, backpack, gump response, and bank box guards.
- Targeted source scan confirmed `CreateItemFor`, `from.BankBox.AddItem(i)`, `TextDefinition.SendMessageTo(from, m_Token.ItemReceiveMessage)`, `m_Token.Delete()`, ButtonID `1`, and serialization markers remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Misc/PromotionalToken.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Misc/PromotionalToken.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named gump-response guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-017` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-018+` remains pending the next concrete non-gated source target.
