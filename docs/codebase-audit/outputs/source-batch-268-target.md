# SOURCE-BATCH-268 SpaceJunk Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-268`
- Candidate: `SB268-CAND-001`
- System: `Items:Technology / SpaceJunk`
- Source file: `Data/Scripts/Items/Technology/SpaceJunk.cs`
- Behavior: add a stale/null/mobile/source-item guard to `SpaceJunk.OnDoubleClick(Mobile from)` before smeltable-item checks, forge lookup, backpack checks, messages, ingot reward creation, or deleting the source junk item.

## Allowed Source Change

- In `SpaceJunk.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Randomized junk names, item IDs, hues, and `RandomCondition()` prefixes.
- Smeltable item eligibility for item IDs `0x3544`, `0x34BC`, and `0x34D8`.
- `DefBlacksmithy.CheckAnvilAndForge(from, 2, out anvil, out forge)` behavior.
- Backpack-use failure message `1060640`.
- Forge failure message and apprentice blacksmith failure message.
- Blacksmith skill threshold `50`.
- `IronIngot` reward amount `Utility.RandomMinMax(1, 5)`.
- Sound `0x208`, success message, and source item `Delete()` semantics.
- Constructor metadata, serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-268 SpaceJunk Guard Repair`
