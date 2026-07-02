# SOURCE-BATCH-397 FireworksWand Guard Repair Closeout

## Result

`SOURCE-BATCH-397` implemented `SB397-CAND-001`, a non-gated guard repair for `FireworksWand`.

## Source Change

- File: `Data/Scripts/Items/Weapons/Maces/FireworksWand.cs`
- `FireworksWand.BeginLaunch(Mobile from, bool useCharges)` now returns immediately when `from` is null, `from` is deleted, or the source wand is deleted.
- The guard runs before reading `from.Map`, consuming charges, sending launch messages, creating effects, or scheduling delayed finish effects.

## Preserved Behavior

- `OnDoubleClick` still delegates to `BeginLaunch(from, true)`.
- Valid BlackJack and HiLoCards reward callers still use `BeginLaunch(m, true)`.
- Charge display, charge decrement behavior for valid launch attempts, empty-charge message `502412`, launch message `502615`, moving effect, randomized endpoint, delayed finish timer, sound, hue, render, and location effect behavior are unchanged.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guard and preserved FireworksWand launch behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-397` source commit: `14faa7f6` (`fix: guard FireworksWand interactions`). `SOURCE-BATCH-398+` should run fresh candidate discovery after `SOURCE-BATCH-397`.
