# SOURCE-BATCH-274 ComputerDatabase Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-274`
- Candidate: `SB274-CAND-001`
- System: `Items:Technology / ComputerDatabase`
- Source file: `Data/Scripts/Items/Technology/ComputerDatabase.cs`
- Behavior: add stale/null/mobile/netstate guards to `ComputerDatabase.OnDoubleClick(Mobile from)` and `ComputerDatabaseGump.OnResponse(NetState state, RelayInfo info)` before range checks, gump opening, sound playback, button handling, appearance mutation, feature recording, or gump redisplay.

## Allowed Source Change

- In `ComputerDatabase.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `ComputerDatabaseGump.OnResponse(NetState state, RelayInfo info)`, return immediately when `state == null || info == null`.
- In `ComputerDatabaseGump.OnResponse(NetState state, RelayInfo info)`, return immediately when `state.Mobile` is null or deleted.

## Must Stay Unchanged

- Terminal range `4`, `ComputerDatabaseGump` opening, and open sound `0x54D`.
- Gump layout, text, items, and button IDs `1` through `32`.
- Random skin and hair color choices and exact hue mappings.
- `Hue`, `HairHue`, `FacialHairHue`, `RecordSkinColor`, `RecordHairColor`, and `RecordBeardColor` mutation behavior.
- `RecordFeatures(true)`, gump redisplay, response sound `0x54B`, and fallback sound `0x54D`.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, appearance policy, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit rows remain `Documented`, `ReviewedNoChange`, and `SafeNoChange`; they do not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-274 ComputerDatabase Guard Repair`
