# Post-Audit Next Steps

Generated: 2026-06-07T13:37:53.0802751-05:00

## Current State

The audit phase runner completed Phases 0 through 14 and the worktree was clean at the start of this post-audit batch. The post-audit live-runtime context commits supersede the original Phase 13 execution order:

- `db0eef4f docs: document live runtime script compile model`
- `9dce70de docs: record source build baseline`
- `09b7b7e5 feat: add compile-only script verification`
- `3259f43b fix: harden packet handler paths`
- `ef8d35b9 docs: reconcile post-audit backlog state`
- `5396b5d1 docs: triage XMLSpawner save compatibility`
- `c69dc894 docs: triage remaining XMLSpawner serializers`
- `54e10f06 docs: triage obsolete serializers`
- `53ea0b48 docs: triage civilized mobile serializers`
- `bf8673c1 docs: triage construct mobile serializers`
- `9ea68a00 docs: triage cultist mobile serializers`
- `7eed1dcc docs: triage daemon mobile serializers`
- `3d5aca5a docs: triage dragon mobile serializers`
- `48fc844d docs: triage elemental mobile serializers`
- `9107fc1f docs: triage goliath mobile serializers`
- `236faf68 docs: triage kobold mobile serializers`
- `60639ced docs: triage minotaur mobile serializers`
- `019f7ecf docs: triage mystical mobile serializers`
- `6204addc docs: triage pirate mobile serializers`
- `91a7eed3 docs: triage serpent mobile serializers`
- `cd5d7a22 docs: accept transient item save exceptions`
- `fa85d6e7 docs: triage house serializers`
- `8890cfdc docs: triage bulk order serializers`
- `6565ed8d docs: triage magical item serializers`
- `be567028 docs: triage special item serializers`
- `ddb3ea5d docs: triage base mobile serializers`
- `83e3adfc docs: triage book publisher serializer`
- `5fa81ec4 docs: triage gm hiding stone serializer`
- `1f4ca8b4 docs: triage champion serializers`
- `d19db2a5 docs: triage offline clone serializers`
- `7a29ca06 docs: triage government serializers`
- `aba2143b docs: triage pandoras gift box serializer`
- `d3b5276f docs: triage skill stone serializer`
- `011c244c docs: triage staff toolbar serializer`
- `e6061f4e docs: triage voting serializers`
- `cbf43c56 docs: triage armor serializers`
- `51cbb89d docs: triage boat serializer`
- `c245cdac docs: triage book serializers`
- `40d81975 docs: triage clothing serializers`
- `e6f1d1cf docs: triage container serializer`
- `c36085d8 docs: triage deed serializer`
- `ff886d27 docs: triage food serializers`
- `1db981db docs: triage weapon serializers`

`Server.csproj` Debug/x86 build passed in the source-build baseline. Runtime script inventory found 6,581 live-visible `.cs` files under `Data/Scripts`, excluding `bin` and `obj`.

The old full-startup smoke remains unsafe for this checkout because the server startup path compiles scripts, then loads `Saves`, invokes script `Initialize`, creates `MessagePump`, initializes `NetState`, and binds listeners. `compile-only-verification-baseline.md` now records a safe runtime script compile verification path.

## Backlog Position

The repair backlog remains the main implementation source of truth:

| Metric | Count |
| --- | ---: |
| Total backlog rows | 6,815 |
| P0 rows | 375 |
| P1 rows | 4,699 |
| P2 rows | 1,429 |
| P3 rows | 298 |
| P4 rows | 14 |

P0 runtime-risk categories are:

| Category | P0 count | Execution meaning |
| --- | ---: | --- |
| Save compatibility | 304 | Review before serializer edits or moves; migration approval required for layout changes. |
| Packet handlers | 17 | Critical network-facing handlers; smallest high-risk code review batch after compile verification. |
| Runtime hooks | 17 | High-risk startup/global hooks requiring guard and side-effect review. |
| PlayerMobile coupling | 8 | Shared save/runtime core coupling; review with serializer and hook context. |
| Project include drift | 29 | IDE/project hygiene only after live-runtime context; no longer first runtime blocker. |

`repair-backlog.csv` is preserved as historical Phase 13 generated evidence. Active post-audit dispositions are tracked in `post-audit-active-backlog-status.csv` so completed rows can be reconciled without rewriting historical generation output.

## Superseded Historical Plan

`phase-13-batch-plan.csv` is preserved as historical audit evidence. Its first batch placed `Scripts.csproj` project drift before packet handlers and save compatibility because the live server startup compile model was not yet known. For implementation after `9dce70de`, use `post-audit-batch-plan.csv` instead.

## Immediate Implementation Sequence

Completed: `POST-BATCH-000` added and verified `.\ConficturaServer.exe -compileonly -nocache`.

Completed runtime-risk batch: `POST-BATCH-A` source-reviewed the 17 P0 packet handler rows in `post-batch-a-packet-handler-review.csv`. The batch applies only source-confirmed packet-handler fixes:

- XMLSpawner `UseReq` now preserves the core action throttle update and Counselor bypass threshold while keeping attachment hooks.
- XMLSpawner book content override now enforces writable, range, and accessibility checks before accepting page edits.
- Monopoly gump response fallback now restores the packet reader position before delegating unmatched responses to the previous/core handler.

Verification passed for this batch:

- `New-RuntimeHookMap.ps1` completed with 17 packet rows.
- `Server.csproj` Debug/x86 build passed.
- `.\ConficturaServer.exe -compileonly -nocache` exited 0 with compile-only success and no listener output.

Active backlog reconciliation:

- `post-audit-active-backlog-status.csv` maps `RB-03235` through `RB-03251` to the packet-handler review artifact.
- Active packet-handler disposition is 3 `Fixed` rows and 14 `ReviewedNoChange` rows.
- `post-audit-active-backlog-status.csv` also maps 281 reviewed save-compatibility rows from `POST-BATCH-B-30A` and prior `POST-BATCH-B` subbatches to the save triage artifact.
- The canonical Phase 13 `repair-backlog.csv` remains unchanged as historical generated evidence.

Started: `POST-BATCH-B` P0 save compatibility triage in `post-batch-b-save-compatibility-triage.csv`.

- The triage file scopes all 304 P0 critical save-compatibility rows.
- Source-reviewed decisions cover 300 rows across completed `POST-BATCH-B` groups through `Magic:Druidism`.
- Current reviewed decisions are 1 `ConfirmedIssue`, 52 `FalsePositive`, 57 `IntentionalLegacy`, 2 `NeedsHumanDecision`, 188 `SafeNoChange`.
- The remaining 4 rows are queued for later source review and do not approve source edits.
- No serialized type name, namespace, field order, version, or file-location change is approved by this triage batch.

Completed review-only subbatch: `POST-BATCH-B-02A` reviewed XMLSpawner central persistence rows in `BaseXmlSpawner.cs`, `XmlAttachment.cs`, `XmlSpawner2.cs`, and `XmlQuestPoints.cs`.

- 10 rows were reviewed with no source edits.
- 6 rows were classified `FalsePositive` where the generated risk came from helper/interface serializers with no applicable base serializer requirement.
- 4 rows were classified `SafeNoChange` where source review confirmed current write/read alignment.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-02B` reviewed the remaining XMLSpawner attachment, item, mobile, challenge, and quest serializer rows.

- 20 rows were reviewed with no source edits.
- 15 rows were classified `SafeNoChange` where source review confirmed current write/read alignment.
- 5 rows were classified `IntentionalLegacy` where source review confirmed old save shapes are consumed but no longer written by the current format.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-02C` reviewed `System:Obsolete` serializers in `Data/Scripts/System/Obsolete/Obsolete.cs`.

- 28 rows were reviewed with no source edits.
- 12 rows were classified `FalsePositive` where source review confirmed helper/static serializers with no applicable base serializer requirement.
- 11 rows were classified `SafeNoChange` where source review confirmed current write/read/version alignment.
- 5 rows were classified `IntentionalLegacy` where source review confirmed old save shapes are consumed but no longer written by the current format.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03A` reviewed `Custom:Mobiles/Civilized` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03B` reviewed `Custom:Mobiles/Constructs` serializers.

- 5 rows were reviewed with no source edits.
- 5 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03C` reviewed `Custom:Mobiles/Cultists` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03D` reviewed `Custom:Mobiles/Daemons` serializers.

- 4 rows were reviewed with no source edits.
- 4 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03E` reviewed `Custom:Mobiles/Dragons` serializers.

- 4 rows were reviewed with no source edits.
- 4 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03F` reviewed `Custom:Mobiles/Elementals` serializers.

- 7 rows were reviewed with no source edits.
- 7 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03G` reviewed `Custom:Mobiles/Goliaths` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03H` reviewed `Custom:Mobiles/Kobolds` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03I` reviewed `Custom:Mobiles/Minotaurs` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03J` reviewed `Custom:Mobiles/Mystical` serializers.

- 4 rows were reviewed with no source edits.
- 4 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03K` reviewed `Custom:Mobiles/Pirates` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03L` reviewed `Custom:Mobiles/Serpents` serializers.

- 2 rows were reviewed with no source edits.
- 2 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03M` reviewed `Custom:Mobiles/Spiders` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: $batch reviewed Custom:Mobiles/Titans serializers.

- 5 rows were reviewed with no source edits.
- 5 rows were classified SafeNoChange where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified ConfirmedIssue, NeedsMigrationPlan, or NeedsHumanDecision.
Completed review-only subbatch: `POST-BATCH-B-03O` reviewed `Custom:Mobiles/Undead` serializers.

- 19 rows were reviewed with no source edits.
- 19 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03P` reviewed `Custom:Mobiles/Vendors` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-04A` reviewed Homestead wine-grape and hops resource serializers.

- 2 rows were reviewed with no source edits.
- 2 rows were classified `IntentionalLegacy` because current version 1 write/read is aligned while version 0 branches consume old info-id save shapes.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-04B` reviewed Homestead craft beverage and juice serializers.

- 5 rows were reviewed with no source edits.
- 5 rows were classified `IntentionalLegacy` because current version 2 write/read is aligned while older version branches consume legacy poison/fill/crafter/quality/variety save shapes.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-04C` reviewed Homestead crop serializers.

- 11 rows were reviewed with no source edits.
- 11 rows were classified `SafeNoChange` where source review confirmed crop field/order alignment or version-only plant wrapper serializers.
- Homestead save-compatibility triage is complete with 18 reviewed rows and no source edits.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-05A` reviewed `System:Misc` helper serializers in `AOS.cs`, `ShardPoller.cs`, and `TextDefinition.cs`.

- 6 rows were reviewed with no source edits.
- 6 rows were classified `FalsePositive` because source review confirmed helper serializer/reader-constructor patterns where base serializer override checks do not apply.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-05B` reviewed `System:Misc/Guilds.cs` serializers.

- 6 rows were reviewed with no source edits.
- 5 rows were classified `FalsePositive` for helper serializer/reader-constructor patterns or duplicate generated rows.
- 1 row was classified `IntentionalLegacy` for the main `Guild` serializer because current version 5 write/read is aligned and older versions are intentionally consumed through fall-through branches.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-05C` reviewed `System:Misc/Spawning.cs` serializers.

- 2 rows were reviewed with no source edits.
- 2 rows were classified `IntentionalLegacy` because current version 4 write/read is aligned while older version branches consume legacy spawner save shapes.
- `System:Misc` save-compatibility triage is complete with 14 reviewed rows and no source edits.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-06A` reviewed `Items:Trades` serializers.

- 13 rows were reviewed with no source edits.
- 7 rows were classified `SafeNoChange` where source review confirmed write/read alignment for runes, runebook entries, arrow resources, and disguise persistence.
- 5 rows were classified `IntentionalLegacy` because current sharpening stone version 1 write/read is aligned while version 0 branches consume a retired owner serial.
- 1 row was classified `FalsePositive` because `RunebookEntry` is a helper serializer with a paired `GenericReader` constructor and no applicable base serializer override.
- `Items:Trades` save-compatibility triage is complete with 13 reviewed rows and no source edits.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-07A` reviewed `Items:Misc` serializers.

- 12 rows were reviewed with no source edits.
- 5 rows were classified `SafeNoChange`, 3 rows were classified `IntentionalLegacy`, and 1 row was classified `FalsePositive`.
- `SERIAL-0964` / `AcidSlime`, `SERIAL-0973` / `FirebombField`, and `SERIAL-1006` / `PoolOfAcid` were initially blocked pending save-policy review because they are transient `Item` subclasses with empty or no-payload serializer overrides.
- `POST-BATCH-B-07B` applies the human decision: these three classes are approved transient no-payload hazard-effect exceptions that should not survive world save/load.
- The three transient rows are now classified `SafeNoChange`, not `FalsePositive`, because the serializer finding was real but accepted by policy.
- `Items:Misc` save-compatibility triage is complete with 12 reviewed rows and no source edits.
- No row remains classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-08A` reviewed `Items:Houses` serializers.

- 12 rows were reviewed with no source edits.
- 9 rows were classified `SafeNoChange` where source review confirmed current write/read/version alignment, including paired helper formats and the established `HouseFoundation` base-last save layout.
- 2 rows were classified `FalsePositive` because `SecureInfo` and `DesignState` base-call findings are helper serializer noise with paired `GenericReader` constructors and no applicable base serializer override.
- 1 row was classified `IntentionalLegacy` because `TownHouse` version 3 write/read is aligned while version 2 and older saves intentionally consume the old region-rect list shape.
- `Items:Houses` save-compatibility triage is complete with 12 reviewed rows and no source edits.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-09A` reviewed `Trades:Bulk Orders` serializers.

- 11 rows were reviewed with no source edits.
- 6 rows were classified `SafeNoChange` where source review confirmed helper or item write/read alignment for bulk-order book filters, entries, the bulk-order book, and large bulk entries.
- 5 rows were classified `FalsePositive` because generated base-call findings were helper serializer noise for book-entry or embedded bulk-entry formats with paired `GenericReader` constructors and no applicable base serializer override.
- `Trades:Bulk Orders` save-compatibility triage is complete with 11 reviewed rows and no source edits.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-10A` reviewed `Items:Magical` serializers.

- 11 rows were reviewed with no source edits.
- 9 rows were classified `SafeNoChange` where source review confirmed version-only serializers or arcane-charge bool sentinel write/read alignment.
- 2 rows were classified `IntentionalLegacy` because `BaseGiftShoes` and `BaseLevelShoes` current version 2 writes no resource payload while older version branches still consume legacy resource data.
- `Items:Magical` save-compatibility triage is complete with 11 reviewed rows and no source edits.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-11A` reviewed `Items:Special` serializers.

- 8 rows were reviewed with no source edits.
- 4 rows were classified `SafeNoChange` where source review confirmed aligned raffle, helper-entry, and veteran reward timer write/read formats.
- 3 rows were classified `IntentionalLegacy` for `SpecialScroll` and derived special scrolls that intentionally consume old inserted-scroll save shapes.
- 1 row was classified `FalsePositive` because `RaffleEntry` is a helper record with a paired `GenericReader` constructor and no applicable base serializer override.
- `Items:Special` save-compatibility triage is complete with 8 reviewed rows and no source edits.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-20A` reviewed `Custom:Staff Toolbar [2.0]` serializers.

- 1 row was reviewed with no source edits.
- 1 row was classified `IntentionalLegacy` because current version 130 write/read is aligned while version 100 is still intentionally consumed through the legacy read branch.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.


Completed review-only subbatch: `POST-BATCH-B-21A` reviewed `Custom:Voting` serializers.

- 4 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed aligned `VoteItem`, `VoteSite`, and `VoteStone` write/read formats.
- 1 row was classified `FalsePositive` because `VoteSite` is an embedded helper/property serializer with no applicable base serializer override.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.


Completed review-only subbatch: `POST-BATCH-B-22A` reviewed `Items:Armor` serializers.

- 3 rows were reviewed with no source edits.
- 2 rows were classified `IntentionalLegacy` because current armor formats are aligned while older armor or arcane payload shapes remain intentionally consumed.
- 1 row was classified `SafeNoChange` where source review confirmed the `LeatherGloves` arcane sentinel and optional charge payload are aligned.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.


Completed review-only subbatch: `POST-BATCH-B-23A` reviewed `Items:Boats` serializers.

- 1 row was reviewed with no source edits.
- 1 row was classified `IntentionalLegacy` because current version 1 write/read is aligned while version 0 still consumes an old trailing uint.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.


Completed review-only subbatch: `POST-BATCH-B-24A` reviewed `Items:Books` serializers.

- 4 rows were reviewed with no source edits.
- 2 rows were classified `IntentionalLegacy` because current book and power-scroll formats are aligned while older save shapes remain intentionally consumed.
- 2 rows were classified `FalsePositive` because `BookPageInfo` is an embedded helper record with a paired `GenericReader` constructor.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.


Completed review-only subbatch: `POST-BATCH-B-25A` reviewed `Items:Clothing` serializers.

- 5 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed aligned arcane sentinel and optional charge payloads.
- 2 rows were classified `IntentionalLegacy` because current clothing/base-shoe formats are aligned while older save shapes remain intentionally consumed.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-26A` reviewed `Items:Containers` serializers.

- 1 row was reviewed with no source edits.
- 1 row was classified `SafeNoChange` because `FillableContainer` current version 1 writes content type plus a bool-gated respawn delta and deserializes the same conditional payload with a version 0 fall-through.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-27A` reviewed `Items:Deeds` serializers.

- 1 row was reviewed with no source edits.
- 1 row was classified `IntentionalLegacy` because current version 1 writes exceptional, crafter, and resource fields in order while the version 0 branch consumes an older extra integer before the resource.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-28A` reviewed `Items:Food` serializers.

- 4 rows were reviewed with no source edits.
- 1 row was classified `FalsePositive` because `BaseBeverage.Deserialize` delegates to `InternalDeserialize(reader, true)`, where the base call and read payload are present.
- 1 row was classified `SafeNoChange` because `BaseBeverage` version 1 writes poisoner, poison, content, and quantity and reads the same payload through `InternalDeserialize`.
- 2 rows were classified `IntentionalLegacy` because `Pitcher` and `Food` current formats are aligned while older save branches remain intentionally consumed.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-29A` reviewed `Items:Weapons` serializers.

- 2 rows were reviewed with no source edits.
- 2 rows were classified `IntentionalLegacy` because `BaseWeapon` and `BaseRanged` current save formats are aligned while older version branches remain intentionally consumed.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Blocked review-only subbatch: `POST-BATCH-B-30A` reviewed `Magic:Druidism` transient spell-effect serializers.

- 2 rows were reviewed with no source edits.
- `SERIAL-1298` writes version 1, remaining duration, owner, and squelch state in `BlendWithForrestSpell.InternalItem.Serialize`, but `Deserialize` reads version, then owner and squelch state without consuming the duration value before deleting the item.
- `SERIAL-1300` writes version 1 and remaining duration in `GraspingRootsSpell.InternalItem.Serialize`, but `Deserialize` reads only the version.
- Decision needed: approve these internal Druidism items as transient no-save/no-payload exceptions, or approve a save-compatibility policy to consume/discard the currently written duration payload during deserialize before deleting or restoring behavior.

Completed source subbatch: `POST-BATCH-B-30B` applied the Druidism transient-effect save policy.

- The likely human decision was applied: these internal timed-effect helper items remain transient on load, but any existing serialized duration payload must be consumed to preserve stream alignment.
- `SERIAL-1298` now consumes the version 1 duration before reading owner and squelch state, restores the owner state when present, and deletes the transient effect item.
- `SERIAL-1300` now consumes the version 1 duration and deletes the transient effect item on load.
- Writer order, version numbers, serialized type names, namespaces, and file locations were unchanged.
- Verification passed: `New-SerializationRegister.ps1`, `Server.csproj` Debug/x86 build through Visual Studio MSBuild, and `.\ConficturaServer.exe -compileonly -nocache` with no `Listening:` output.

Completed review-only subbatch: `POST-BATCH-B-31A` reviewed `Mobiles:Animals` serializers.

- 1 row was reviewed with no source edits.
- `SERIAL-1319` was classified `IntentionalLegacy` because current `EtherealMount` version 3 writes and reads donation flag, reward flag, mounted ID, regular ID, and rider in aligned order, while version 1 intentionally consumes an old discarded integer before the shared version 0 payload.
- No row was classified `NeedsMigrationPlan` or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-32A` reviewed `Quests:Summon` serializers.

- 1 row was reviewed with no source edits.
- `SERIAL-1356` was classified `ConfirmedIssue` because current `SummonPrison.Serialize` writes `PrisonerFullNameUsed` and `PrisonerClothColorUsed` before `PrisonerSerial`, while current `Deserialize` reads `PrisonerSerial` before those two integers.
- No row was classified `NeedsMigrationPlan` or `NeedsHumanDecision`; the later source fix should preserve writer order and version unless older-save evidence requires a migration branch.

Completed review-only subbatch: `POST-BATCH-B-33A` reviewed `System:Regions` serializers.

- 2 rows were reviewed with no source edits.
- `SERIAL-1494` and `SERIAL-1496` were classified `FalsePositive` because `SpawnEntry` is a helper serializer called by `SpawnPersistence`; the paired `Deserialize(reader, version)` and `Remove(reader, version)` methods consume the payload written by `SpawnEntry.Serialize`.
- `POST-BATCH-B` save compatibility triage has now reviewed all 304 rows.

Completed source subbatch: `POST-BATCH-B-34A` fixed the remaining active save serializer issues.

- `SERIAL-1356` applied the likely human save-policy decision: current `SummonPrison` writer order is authoritative, so the reader now consumes `PrisonerFullNameUsed`, `PrisonerClothColorUsed`, then `PrisonerSerial` without changing writer order or version.
- `SERIAL-0032` now writes zero ghost entries when `m_ghosts` is null in `CityResurrectionStone.Serialize`, preserving the version 1 count/entry/sign layout.
- Writer order, version numbers, serialized type names, namespaces, and file locations were unchanged.
- Verification passed: `New-SerializationRegister.ps1`, `Server.csproj` Debug/x86 build through Visual Studio MSBuild, and `.\ConficturaServer.exe -compileonly -nocache` with no `Listening:` output.

Closeout status: `POST-BATCH-B` is complete and unblocked.

- No queued rows remain in the triage CSV.
- No `NeedsMigrationPlan`, `NeedsHumanDecision`, or active save `ConfirmedIssue` rows remain.

Completed review-only subbatch: `POST-BATCH-C-01A` reviewed P0 runtime hooks and `PlayerMobile` coupling rows.

- 25 rows were reviewed with no source edits.
- The 17 runtime-hook rows reconcile to the earlier `POST-BATCH-A` packet-handler review and focused fixes.
- Active dispositions are 3 prior `Fixed` rows, 21 `ReviewedNoChange` rows, and 1 `SafeNoChange` row.
- `RB-05228` / `SkillStone` is classified `SafeNoChange` because the source writes the assigned player through the Mobile object-reference format and reads it back through `reader.ReadMobile()` with a `PlayerMobile` cast.
- `RB-05352` / `PlayerMobile` count mismatch is classified `ReviewedNoChange` because source review found the current version 37 fall-through mirrors `Serialize`; the generated mismatch is conservative extractor noise around helper serializers, conditional loops, and legacy branches.
- The remaining `PlayerMobile Core` and `PvP Consent` coupling rows are classified `ReviewedNoChange` as intentional shared player state coupling, still migration-gated for future edits.
- No row remains classified `NeedsMigrationPlan`, `NeedsHumanDecision`, or active `ConfirmedIssue`.

Next:

Completed source subbatch: `POST-BATCH-D-01A` fixed `Custom:Champions` pooled enumerable ownership.

- 8 rows were reviewed and fixed: `RB-04689` through `RB-04696`.
- Direct `foreach` loops over `GetItemsInRange`, `GetMobilesInRange`, and `GetClientsInRange` were replaced with local `IPooledEnumerable` variables and `try/finally Free`.
- Existing loop filtering, break behavior, target collection, damage/message side effects, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Custom:Champions` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-02A` fixed `Custom:ClearDeckCommand` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04697`.
- `ClearDeckCommand.ClearDeck_OnCommand` now pairs `playerBoat.GetItemsInRange(18)` with `try/finally Free`.
- Existing corpse filtering, boat matching, deletion behavior, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `ClearDeckCommand` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-03A` fixed `Custom:Government System` pooled enumerable ownership.

- 11 rows were reviewed and fixed: `RB-04698` through `RB-04708`.
- `CityDeed.FinishPlacement`, `CityLandLord.CreateRandomVendors`, and `CityLandLord.Deserialize` now pair spawner `GetMobilesInRange(0)` results with `try/finally Free`.
- Existing landlord lookup, initialization, vendor freeze/direction behavior, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Custom:Government System` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-04A` fixed `Custom:Invasion System` `BaseSpecialCreature.cs` pooled enumerable ownership.

- 3 rows were reviewed and fixed: `RB-04709` through `RB-04711`.
- `BaseSpecialCreature.Earthquake`, `BaseSpecialCreature.BreathStart`, and `BaseSpecialCreature.OnDamagedBySpell` now pair `GetMobilesInRange` results with `try/finally Free`.
- Existing target collection, filtering, damage behavior, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `BaseSpecialCreature.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-05A` fixed `Custom:Invasion System` `Lord BlackThorn Clone.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04712`.
- `BlackthornClone.SpawnMech` now pairs `this.GetMobilesInRange(10)` with `try/finally Free`.
- Existing `RunicGolemInvader` count behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Lord BlackThorn Clone.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-06A` fixed `Custom:Invasion System` `MobileFeatures.cs` pooled enumerable ownership.

- 10 rows were reviewed and fixed: `RB-04713` through `RB-04722`.
- Mass peace, mass provoke, revealer, area attack, and drain effect methods now pair `GetMobilesInRange` results with `try/finally Free`.
- Existing target collection, reveal/drain side effects, damage behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `MobileFeatures.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Next:

1. Continue `POST-BATCH-D` at the next queued pooled-enumerable backlog rows after `RB-04722`, continuing `Custom:Invasion System` with `PirateCaptain.cs`.
2. Keep batches focused by system/file; repair only confirmed ownership leaks with `try/finally Free`.
3. Verify each source batch with the relevant pooled enumerable scan, `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache`.

## Reorganization Status

Reorganization remains deferred. No source file move should be executed until runtime compile verification is available, save compatibility for affected types is reviewed, documentation/source traces are updated, and rollback is recorded.
