# SOURCE-BATCH-303 RepairPotion Guard Repair Closeout

## Summary

`SOURCE-BATCH-303` implemented `SB303-CAND-001` from `source-batch-303-candidate-discovery.csv`.

The batch adds stale/null/mobile/source-potion/target-item/backpack guards to `Data/Scripts/Items/Potions/Special/RepairPotion.cs` without crossing POST-BATCH-Y gates or changing repair behavior.

## Source Changes

- `RepairPotion.Drink(Mobile m)` now returns immediately when the mobile is null/deleted or the potion is deleted before range checks or target assignment.
- `ConsumeCharge(RepairPotion potion, Mobile from)` now returns immediately when the source potion or mobile is null/deleted before consumption, revealing, or sound.
- `RepairTarget.OnTarget(Mobile from, object targeted)` now returns immediately for stale mobile/source-potion state before target inspection or repair mutation.
- Deleted target armor/weapons and missing backpacks use the existing backpack-repair failure message path before mutation.

## Preserved Behavior

- One-tile range check.
- Prompt and failure/success messages.
- `BaseArmor` and `BaseWeapon` eligibility.
- Target item backpack requirement.
- Full-repair check.
- `MaxHitPoints = MaxHitPoints - 1` and `HitPoints = MaxHitPoints` repair behavior.
- Sound `0x23E`.
- `RevealingAction`.
- Repair potion `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Potions/Special/RepairPotion.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Items/Potions/Special/RepairPotion.cs`: `0`.
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval was crossed.

## Verification

- Candidate CSV import: passed; `SB303-CAND-001` is recommended with gate hits `0` and active overlay rows `0`.
- Targeted source scan: passed; mobile, potion, target, deleted item, backpack, range, full-repair, repair mutation, sound, consume, and serializer-preservation evidence found.
- POST-BATCH-Y exact-file gate scan: passed with `0` hits.
- Active overlay exact-file scan: passed with `0` rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Changed-file scan: passed; no project/config/data files changed.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-304+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-303` is committed. `DurabilityPotion` remains a sibling candidate from this discovery, but it still requires exact-file gate/overlay preflight before source edits.
