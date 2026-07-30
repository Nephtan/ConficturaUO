# SOURCE-BATCH-027 Closeout: ArtifactManual Guard Repair

Reviewed at: 2026-06-16T20:14:00-05:00

## Summary

`SOURCE-BATCH-027` implemented `SB025-CAND-003`, a non-gated `ArtifactManual` guard repair in `Data/Scripts/Items/Magical/ArtifactManual.cs`.

The batch adds stale/null safety before mobile, source manual, backpack, target item, charge decrement, item conversion, item transfer, or deletion state is evaluated. It preserves existing charge behavior, item-identification outcomes, messages, persistence, layout, and gated systems.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles or deleted manual state before dereferencing state.
- `OnDoubleClick(Mobile from)` now checks for a missing backpack before backpack containment.
- `BookTarget.OnTarget(Mobile from, object targeted)` now returns for null/deleted mobiles before dereferencing state.
- `BookTarget.OnTarget(Mobile from, object targeted)` now treats null/deleted/out-of-backpack source manual state as the existing localized pack failure before lookup or charge decrement.
- `LookupTheItem(Mobile from, object targeted)` now returns `false` for null/deleted mobiles or missing backpacks.
- `LookupTheItem(Mobile from, object targeted)` now treats null/deleted target item state as the existing cannot-find-information outcome before item conversion logic.

## Preserved Behavior

- Charges behavior and decrement timing after successful lookup.
- `UnknownReagent`, `UnknownScroll`, `UnknownLiquid`, `UnknownKeg`, `UnknownWand`, `UnidentifiedArtifact`, and `UnidentifiedItem` outcomes.
- Item transfer and deletion semantics.
- Existing messages.
- Target range.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/ArtifactManual.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/ArtifactManual.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source manual, backpack, and deleted-target guards.
- Targeted source scan confirmed `m_Book.Charges = m_Book.Charges - 1`, `UnknownReagent`, `UnknownScroll`, `UnknownLiquid`, `UnknownKeg`, `UnknownWand`, `UnidentifiedArtifact`, `UnidentifiedItem`, `AddToBackpack`, `Delete()`, `writer.Write`, and `reader.ReadInt` markers remain present.
- POST-BATCH-Y gate scan returned `0`.
- Active overlay scan returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-027` is complete.
- No gated behavior changed.
- The `source-batch-025-candidate-discovery.csv` implementation queue is exhausted.
- `SOURCE-BATCH-028+` remains pending candidate discovery for the next clean non-gated target.
