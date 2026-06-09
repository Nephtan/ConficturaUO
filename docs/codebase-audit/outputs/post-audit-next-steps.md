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

Completed source subbatch: `POST-BATCH-D-07A` fixed `Custom:Invasion System` `PirateCaptain.cs` pooled enumerable ownership.

- 2 rows were reviewed and fixed: `RB-04723` and `RB-04724`.
- `PirateCaptain.OnThink` and `PirateCaptain.SpawnPirate` now pair `GetItemsInRange` and `GetMobilesInRange` results with `try/finally Free`.
- The touched nested pirate crew scans also now free through `finally`; existing enemy-boat selection, pirate count behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `PirateCaptain.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-08A` fixed `Custom:Invasion System` `SubChamps` pooled enumerable ownership.

- 9 rows were reviewed and fixed: `RB-04725` through `RB-04733`.
- SubChamp spawn-count helpers now pair `this.GetMobilesInRange(10)` results with `try/finally Free`.
- Existing helper-count thresholds, spawn behavior, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `SubChamps` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Custom:Invasion System` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-09A` fixed `Custom:OmniAI` pooled enumerable ownership.

- 4 rows were reviewed and fixed: `RB-04734` through `RB-04737`.
- OmniAI target, corpse, random-target, and dispel-evil scans now pair range results with `try/finally Free`.
- Existing target filtering, corpse selection, cast detection, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Custom:OmniAI` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Custom:OmniAI` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-10A` fixed `Custom:RandomEncounters` pooled enumerable ownership.

- 3 rows were reviewed and fixed: `RB-04738` through `RB-04740`.
- `RandomEncounterCleanupTimer.MaybeRemove` now frees the assigned `GetClientsInRange` enumerable through `finally`, including the early-return cleanup grace path.
- Existing cleanup grace behavior, staff-player filtering, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Timers.cs` ownership check found the assigned enumerable paired with `finally`, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Custom:RandomEncounters` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-11A` fixed `Custom:XMLSpawner` `BaseXmlSpawner.cs` pooled enumerable ownership.

- 3 rows were reviewed and fixed: `RB-04741` through `RB-04743`.
- `PLAYERSINRANGE` player-count scans now pair item/mobile `GetMobilesInRange` results with `try/finally Free`.
- The sibling split-line `refobject` mobile scan in the same touched branch was also fixed; existing player counting, keyword behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `BaseXmlSpawner.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-12A` fixed `Custom:XMLSpawner` `XmlSpawner2.cs` pooled enumerable ownership.

- 2 rows were reviewed and fixed: `RB-04744` and `RB-04745`.
- `NearbyPlayerCount` and `OnTick` proximity player scans now pair `GetMobilesInRange` results with `try/finally Free`.
- Existing player counting, proximity trigger checks, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `XmlSpawner2.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-13A` fixed `Custom:XMLSpawner` `XmlAttachments/XmlPoints.cs` pooled enumerable ownership.

- 3 rows were reviewed and fixed: `RB-04746` through `RB-04748`.
- Duel availability and duel return pet scans now pair `GetMobilesInRange` results with `try/finally Free`.
- Existing occupied-duel-area early return, controlled pet collection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `XmlPoints.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-14A` reviewed `Custom:XMLSpawner` PvP pooled enumerable ownership.

- 4 rows were reviewed: `RB-04749` through `RB-04752`.
- 3 live rows were fixed in `BaseChallengeGame.cs`, `KingOfTheHillGauntlet.cs`, and `TeamKotHGauntlet.cs` by pairing arena and hill `GetMobilesInRange` scans with `try/finally Free`.
- `RB-04751` was classified `FalsePositive` because the reported `LastManStandingGauntlet.cs` loop is inside a disabled block comment and is not compiled or executed.
- Existing arena clearing, hill scoring, non-participant collection, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: raw PvP direct-scan check returned only the disabled `LastManStandingGauntlet.cs` comment hit, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Custom:XMLSpawner` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-15A` fixed `Items:Boats` pooled enumerable ownership.

- 7 rows were reviewed and fixed: `RB-04753` through `RB-04759`.
- Boat cleanup, ship proximity, enemy-ship lookup, shipwright/caster checks, docking lantern search, and plank close scans now pair range results with `try/finally Free`.
- Existing crew cleanup detection, obstacle counting, shipwright/caster counting, dock access checks, blocked-close early return, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Items/Boats` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Boats` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-16A` fixed `Items:Books` pooled enumerable ownership.

- 4 rows were reviewed and fixed: `RB-04760` through `RB-04763`.
- Merchant proximity, near-book validation, and vendor speech scans now pair range results with `try/finally Free`.
- Existing vendor eligibility detection, purchase validation, vendor speech behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Items/Books` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Books` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-17A` fixed `Items:Containers` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04764`.
- `FillableContent.Acquire` now pairs the nearest-vendor `map.GetMobilesInRange(loc, 20)` scan with `try/finally Free`.
- Existing nearest vendor content selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Items/Containers` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Containers` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-18A` fixed `Items:Doors` pooled enumerable ownership.

- 4 rows were reviewed and fixed: `RB-04765` through `RB-04768`.
- Nearby door lock/unlock/open and near-gate item scans now pair range results with `try/finally Free`.
- Existing door collection, gate detection early return, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Items/Doors` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Doors` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-19A` fixed `Items:Explorers` pooled enumerable ownership.

- 5 rows were reviewed and fixed: `RB-04769` through `RB-04773`.
- Bedroll, bedrolled-out, campfire, enemy-nearby, and camp-nearby scans now pair range results with `try/finally Free`.
- Existing bedroll owner checks, rest collection, hostile-creature checks, active-camp early returns, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Items/Explorers` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Explorers` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-20A` fixed `Items:Gifts` pooled enumerable ownership.

- 2 rows were reviewed and fixed: `RB-04774` and `RB-04775`.
- Christmas and Halloween holiday speech-handler vendor scans now pair range results with `try/finally Free`.
- Existing gift/treat selection, vendor responses, early returns, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Items/Gifts` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Gifts` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-21A` fixed `Items:Houses` `TavernTable.cs` pooled enumerable ownership.

- 4 rows were reviewed and fixed: `RB-04776` through `RB-04779`.
- Patron removal, patron counting, lawn visitor detection, and shanty visitor detection scans now pair range results with `try/finally Free`.
- Existing patron collection/deletion, count accumulation, visitor early returns, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `TavernTable.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-22A` fixed `Items:Houses` `Monopoly/Items/TownHouse.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04780`.
- Delete-time sign item scan now pairs the range result with `try/finally Free`.
- Existing item visibility restoration, house cleanup, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `TownHouse.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-23A` reviewed `Items:Houses` `Monopoly/Items/TownHouseSign.cs` pooled enumerable ownership.

- 5 rows were reviewed: `RB-04781` through `RB-04785`.
- 4 live rows were fixed by pairing sign-hiding, item-bounds, and door-linking scans with `try/finally Free`.
- `RB-04785` was classified `FalsePositive` because the reported `Map.GetItemsInBounds` loop is inside a disabled line-comment block.
- Existing sign visibility changes, converted item collection, door linking/relinking, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: raw `TownHouseSign.cs` range-scan check returned only the disabled comment hit, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-24A` fixed `Items:Houses` `Monopoly/Misc/GumpResponse.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04786`.
- Nearby town-house collection now pairs the range result with `try/finally Free`.
- Existing gump response validation flow, owner-house lookup, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `GumpResponse.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-25A` fixed `Items:Houses` `Remodeling` pooled enumerable ownership.

- 6 rows were reviewed and fixed: `RB-04787` through `RB-04792`.
- Lawn/shanty house lookup and visitor cleanup scans now pair range results with `try/finally Free`.
- Existing owner-house lookup early returns, visitor collection/deletion, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Items/Houses/Remodeling` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Houses` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-26A` fixed `Items:Misc` `AcidSlime.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04793`.
- Damage target collection now pairs the range result with `try/finally Free`.
- Existing transient hazard behavior, damage filtering, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `AcidSlime.cs` direct-scan check returned no matches, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-27A` fixed `Items:Misc` `MagicForges.cs` pooled enumerable ownership.

- 8 rows were reviewed and fixed: `RB-04794` through `RB-04801`.
- Serpent reward, dark core validation/enchantment, Golden Ranger, poison, cold, energy, and fire forge scans now pair range results with `try/finally Free`.
- Existing reward, validation, item morphing, forge effect behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `MagicForges.cs` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.

Completed source subbatch: `POST-BATCH-D-28A` fixed remaining `Items:Misc` pooled enumerable ownership.

- 4 rows were reviewed and fixed: `RB-04802` through `RB-04805`.
- Morph item refresh, acid pool damage collection, warning-neighbor propagation, and game-board client sound fan-out now pair range results with `try/finally Free`.
- Existing movement trigger, transient hazard, warning propagation, packet release flow, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted four-file direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Misc` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-29A` fixed `Items:Potions` pooled enumerable ownership.

- 5 rows were reviewed and fixed: `RB-04806` through `RB-04810`.
- Alchemist counting, monster splatter counting, conflagration target collection, confusion blast effects, and frostbite target collection now pair range results with `try/finally Free`.
- Existing potion targeting, effect application, transient potion behavior, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted five-file direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Potions` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-30A` fixed `Items:Special` `DemonPrison.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04811`.
- Monster-splatter counting now pairs the item range result with `try/finally Free`.
- Existing melee-hit splatter behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `DemonPrison.cs` direct-scan check returned no matches, explicit pooled-variable check showed a matching `Free` call, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Special` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-31A` fixed `Items:Technology` `Landmine.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04812`.
- Nearby landmine counting now pairs the item range result with `try/finally Free`.
- Existing landmine placement limits, harmful-region check, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Landmine.cs` direct-scan check returned no matches, explicit pooled-variable check showed a matching `Free` call, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Technology` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-32A` fixed `Items:Trades` `RubyPickaxe.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04813`.
- Nearby hydra detection now pairs the mobile range result with `try/finally Free`.
- Existing mining skill, hydra block, charge behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `RubyPickaxe.cs` direct-scan check returned no matches, explicit pooled-variable check showed a matching `Free` call, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Trades` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-33A` fixed `Items:Traps` pooled enumerable ownership.

- 4 rows were reviewed and fixed: `RB-04814` through `RB-04817`.
- Flame spurt refresh, spike trap movement damage, stone face damage, and trap kit nearby trap counting now pair range results with `try/finally Free`.
- Existing gem/jewel early return, player detection, damage application, nearby trap limit behavior, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted four-file direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Traps` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-34A` fixed `Items:Weapons` `BaseWeapon.cs` pooled enumerable ownership.

- 3 rows were reviewed and fixed: `RB-04818` through `RB-04820`.
- Pack-instinct counting, mirror-image diversion, and area-attack target collection now pair range results with `try/finally Free`.
- Existing pack bonus thresholds, mirror-image break behavior, area-target collection, empty-list return, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `BaseWeapon.cs` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Items:Weapons` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-35A` fixed `Magic:Bard` spell pooled enumerable ownership.

- 11 rows were reviewed and fixed: `RB-04821` through `RB-04831`.
- Nearby song target collection in Bard spell files now pairs range results with `try/finally Free`.
- Existing beneficial target filters, Magic Finale summoned/control-slot filters, song effects, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Magic/Bard/Spells` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the 11 touched spell files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Bard` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-36A` fixed `Magic:Bushido` `MomentumStrike.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04832`.
- Momentum target collection now pairs the range result with `try/finally Free`.
- Existing target filtering, mana checks, damage transfer behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `MomentumStrike.cs` direct-scan check returned no matches, explicit pooled-variable check showed a matching `Free` call, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Bushido` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-37A` fixed `Magic:Druidism` `TreefellowSpell.cs` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04833`.
- Summoned treefellow nearby vortex cleanup now pairs the range result with `try/finally Free`.
- Existing Core.SE/Summoned guard, vortex filtering, dispel cleanup, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `TreefellowSpell.cs` direct-scan check returned no matches, explicit pooled-variable check showed a matching `Free` call, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Druidism` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-38A` fixed `Magic:Elementalism` pooled enumerable ownership.

- 10 rows were reviewed and fixed: `RB-04834` through `RB-04843`.
- Elemental mobile cleanup, elemental field collision, and Elemental Apocalypse target collection now pair range results with `try/finally Free`.
- Existing summoned guards, vortex filters, field target queueing, apocalypse harmful target filters, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Magic/Elementalism` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the 10 touched files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Elementalism` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-39A` fixed `Magic:Jester` pooled enumerable ownership.

- 5 rows were reviewed and fixed: `RB-04844` through `RB-04848`.
- Jester proximity, prank explosion, splatter-count, and Hilarity target scans now pair range results with `try/finally Free`.
- Existing prank point checks, explosion target filters, splatter creation gates, Hilarity creature/non-creature filters, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Magic/Jester` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the five touched files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Jester` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-40A` fixed `Magic:Knight` pooled enumerable ownership.

- 3 rows were reviewed and fixed: `RB-04849` through `RB-04851`.
- Dispel Evil, Holy Light, and Noble Sacrifice target scans now pair range results with `try/finally Free`.
- Existing harmful/beneficial target filters, Dispel Evil controlled-creature handling, Noble Sacrifice range TODO, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Magic/Knight` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the three touched files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Knight` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-41A` fixed `Magic:Magery` pooled enumerable ownership.

- 3 rows were reviewed and fixed: `RB-04852` through `RB-04854`.
- Magic Trap, Fire Field, and Earthquake range scans now pair range results with `try/finally Free`.
- Existing trap counting, fire field target queueing, earthquake harmful target filters, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Magic/Magery` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the three touched files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Magery` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-42A` fixed `Magic:Misc` pooled enumerable ownership.

- 2 rows were reviewed and fixed: `RB-04855` and `RB-04856`.
- Summon Dragon and Summon Snakes summoned-vortex cleanup scans now pair range results with `try/finally Free`.
- Existing summoned guards, vortex filters, cleanup thresholds, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Magic/Misc` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the two touched files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Misc` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-43A` fixed `Magic:Necromancy` pooled enumerable ownership.

- 2 rows were reviewed and fixed: `RB-04857` and `RB-04858`.
- Poison Strike and Wither target scans now pair range results with `try/finally Free`.
- Existing harmful target filters, splash damage collection, wither target collection, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Magic/Necromancy` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the two touched files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Necromancy` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-44A` fixed `Magic:Research` pooled enumerable ownership.

- 9 rows were reviewed and fixed: `RB-04859` through `RB-04867`.
- Death, Enchanting, Sorcery, and Wizardry research spell range scans now pair range results with `try/finally Free`.
- Existing corpse selection, nearby hazard counts, beneficial target collection, field target queueing, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Magic/Research/Spells` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the nine touched files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Magic:Research` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-45A` fixed `Mobiles:Animals` pooled enumerable ownership.

- 2 rows were reviewed and fixed: `RB-04868` and `RB-04869`.
- Infected blood splatter and stirge drain target scans now pair range results with `try/finally Free`.
- Existing splatter counting, drain target filters, serialization, namespaces, type names, save versions, and file locations were preserved.
- Verification passed: targeted `Mobiles/Animals` direct-scan check returned no matches in the touched files, explicit pooled-variable check showed matching `Free` calls across the two touched files, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Animals` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-46A` fixed `Mobiles:Base` `BaseCreature.cs` pooled enumerable ownership.

- 5 rows were reviewed and fixed: `RB-04870` through `RB-04874`.
- Breath splash, team-size, boat-link, rummage, and pet-teleport scans now pair range results with `try/finally Free`.
- Existing target filters, early break behavior, boat-link keep/delete behavior, pet selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `BaseCreature.cs` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the five touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `BaseCreature.cs` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-47A` fixed `Mobiles:Base` `BaseVendor.cs` pooled enumerable ownership.

- 2 rows were reviewed and fixed: `RB-04875` and `RB-04876`.
- Vendor action and player-sight scans now pair range results with `try/finally Free`.
- Existing vendor action object handling, visible player counting, enemy detection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `BaseVendor.cs` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the two touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `BaseVendor.cs` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-48A` fixed `Mobiles:Base` `Behavior.cs` pooled enumerable ownership.

- 9 rows were reviewed and fixed: `RB-04877` through `RB-04885`.
- AI summon-count, cleanup, proximity, friend-guard, aggressor, marching-order, searching, need-finding, and dispel scans now pair range results with `try/finally Free`.
- Existing target filters, continue paths, combatant scoring, hidden-player detection math, need priority, dispel priority, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Behavior.cs` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the nine touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Behavior.cs` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-49A` fixed `Mobiles:Base` `PlayerMobile.cs` pooled enumerable ownership.

- 2 rows were reviewed and fixed: `RB-04886` and `RB-04887`.
- Staff-message client and enemy notoriety scans now pair range results with `try/finally Free`.
- Existing packet acquisition/release, staff access filters, enemy notoriety update packets, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `PlayerMobile.cs` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the two touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `PlayerMobile.cs` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-50A` fixed `Mobiles:Civilized` pooled enumerable ownership.

- 23 rows were reviewed and fixed: `RB-04888` through `RB-04910`.
- Tradesman, training, working-spot, familiar, and pack-beast scans now pair range results with `try/finally Free`.
- Existing work actions, cleanup lists, familiar filters, pack-beast eligibility checks, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Civilized` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the 23 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Civilized` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-51A` fixed `Mobiles:Constructs` pooled enumerable ownership.

- 3 rows were reviewed and fixed: `RB-04911` through `RB-04913`.
- WaxSculpture, Mutant, and IronCobra scans now pair range results with `try/finally Free`.
- Existing stamina-drain targeting, toxic blood counting, stone-effect targeting, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Constructs` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the three touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Constructs` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-52A` fixed `Mobiles:Daemons` pooled enumerable ownership.

- 6 rows were reviewed and fixed: `RB-04914` through `RB-04919`.
- Splatter-counting, demon-gate hiding, and drain-life scans now pair range results with `try/finally Free`.
- Existing splatter selection, gate visibility handling, drain-life target filters, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Daemons` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the six touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Daemons` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-53A` fixed `Mobiles:Dragons` pooled enumerable ownership.

- 6 rows were reviewed and fixed: `RB-04920` through `RB-04925`.
- Dragon splatter-counting and VampiricDragon drain-life scans now pair range results with `try/finally Free`.
- Existing splatter selection, drain-life target filters, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Dragons` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the six touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Dragons` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-54A` fixed `Mobiles:Elementals` pooled enumerable ownership.

- 8 rows were reviewed and fixed: `RB-04926` through `RB-04933`.
- MudMan, StormCloud, and elemental splatter-counting scans now pair range results with `try/finally Free`.
- Existing target filters, splatter selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Elementals` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the eight touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Elementals` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-55A` fixed `Mobiles:Hellish` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-04934`.
- ShadowFiend hidden-player reveal now pairs the range result with `try/finally Free`.
- Existing reveal filters, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Hellish` direct-scan check returned no matches, explicit pooled-variable check showed a matching `Free` call, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Hellish` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-56A` fixed `Mobiles:Humanoids` pooled enumerable ownership.

- 12 rows were reviewed and fixed: `RB-04935` through `RB-04946`.
- Stone, spawn-count, provoke, sailor, savage dance, and splatter-counting scans now pair range results with `try/finally Free`.
- Existing target filters, list-building behavior, splatter selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Humanoids` direct-scan check returned no matches with qualified and unqualified `Get*InRange` calls covered, explicit pooled-variable check showed matching `Free` calls across the 12 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Humanoids` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-57A` fixed `Mobiles:Insects` pooled enumerable ownership.

- 20 rows were reviewed and fixed: `RB-04947` through `RB-04966`.
- Insect, alien, and antaur splatter-counting scans now pair range results with `try/finally Free`.
- Existing splatter selection, before-death effect gating, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Insects` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the 20 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Insects` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-58A` fixed `Mobiles:Mystical` pooled enumerable ownership.

- 9 rows were reviewed and fixed: `RB-04967` through `RB-04975`.
- Sphinx stone-target, dryad peace/undress, and satyr provoke scans now pair range results with `try/finally Free`.
- Existing petrification target collection, dryad target filters, provoke filters and first-target break behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Mystical` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the nine touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Mystical` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-59A` fixed `Mobiles:Plants` pooled enumerable ownership.

- 6 rows were reviewed and fixed: `RB-04976` through `RB-04981`.
- BloodLotus drain-life, BogThing bogling collection, and plant splatter-counting scans now pair range results with `try/finally Free`.
- Existing target collection, bogling deletion order, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Plants` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the six touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Plants` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-60A` fixed `Mobiles:Reptilian` pooled enumerable ownership.

- 7 rows were reviewed and fixed: `RB-04982` through `RB-04988`.
- Stone, poison, teleport, shock, drain-life, and splatter-counting scans now pair range results with `try/finally Free`.
- Existing target collection, first valid teleport target selection, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Reptilian` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the seven touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Reptilian` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-61A` fixed `Mobiles:Slimes` pooled enumerable ownership.

- 5 rows were reviewed and fixed: `RB-04989` through `RB-04993`.
- Drain-life, oil-burn, and splatter-counting scans now pair range results with `try/finally Free`.
- Existing target collection, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Slimes` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the five touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Slimes` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-62A` fixed `Mobiles:Summoned` pooled enumerable ownership.

- 7 rows were reviewed and fixed: `RB-04994` through `RB-05000`.
- DeathVortex and GasCloud random target acquisition plus summoned-vortex and swarm cleanup scans now pair range results with `try/finally Free`.
- Existing random filters, target list damage scaling, cleanup list trimming behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Summoned` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the seven touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Summoned` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-63A` fixed `Mobiles:Undead` pooled enumerable ownership.

- 12 rows were reviewed and fixed: `RB-05001` through `RB-05012`.
- Spawn-count, drain-life, and green-blood splatter-counting scans now pair range results with `try/finally Free`.
- Existing spawn thresholds, drain-life target collection, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Undead` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the 12 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Undead` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-64A` fixed `Mobiles:Unique` pooled enumerable ownership.

- 16 rows were reviewed and fixed: `RB-05013` through `RB-05028`.
- Death-gate, spawn-count, and splatter-counting scans now pair range results with `try/finally Free`.
- Existing quest item checks, reward effects, Titan kill-permission fallback checks, spawn thresholds, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Unique` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the 16 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Unique` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-65A` fixed `Mobiles:Unusual` pooled enumerable ownership.

- 11 rows were reviewed and fixed: `RB-05029` through `RB-05039`.
- Drain-life, petrification, teleport-target, alien blood splatter-counting, nearby-attacker, and gold-carrier scans now pair range results with `try/finally Free`.
- Existing target filters, first valid teleport target selection, splatter-counting behavior, nearby-attacker detection, gold-carrier selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Mobiles/Unusual` direct-scan check returned no matches, explicit pooled-variable check showed matching `Free` calls across the 11 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Mobiles:Unusual` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-66A` fixed quest pooled enumerable ownership.

- 11 rows were reviewed and fixed: `RB-05040` through `RB-05050`.
- Power-coil, hoard, nearby-monster, Balinor blocker, PremiumSpawner, town-detection, and rune-gate scans now pair range results with `try/finally Free`.
- Existing proximity checks, target collection, location override, town detection, rune gate collection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted quest direct-scan check returned no matches in the touched files, explicit pooled-variable check showed matching `Free` calls across the 11 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- Quest queued pooled-enumerable rows through `RB-05050` are complete.

Completed source subbatch: `POST-BATCH-D-67A` fixed `System:Commands` pooled enumerable ownership.

- 4 rows were reviewed and fixed: `RB-05051` through `RB-05054`.
- Banker-spawner, client broadcast, corpse search, and moon gate search scans now pair range results with `try/finally Free`.
- Existing banker detection, packet visibility checks, nearest-corpse selection, moon gate selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `System:Commands` direct-scan check returned no matches in the touched files, explicit pooled-variable check showed matching `Free` calls across the four touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `System:Commands` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-68A` fixed `System:Misc` pooled enumerable ownership.

- 7 rows were reviewed and fixed: `RB-05055` through `RB-05061`.
- Death healer/shrine, party staff-message, PremiumSpawner activation, boat detection, and boat-town proximity scans now pair range results with `try/finally Free`.
- Existing healer/shrine selection, staff listener filtering, packet send behavior, spawner activation, boat detection, town proximity early return, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `System:Misc` direct-scan check returned no matches in the touched files, explicit pooled-variable check showed matching `Free` calls across the seven touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `System:Misc` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-69A` fixed `System:Obsolete` pooled enumerable ownership.

- 13 rows were reviewed and fixed: `RB-05062` through `RB-05074`.
- Faction trap placement, Holy Bless/Curse area effects, Ethics ankh detection, faction object existence checks, glowing-goo splatter counting, dispel target acquisition, TurnStone target collection, and SalesBook nearby/speech scans now pair range results with `try/finally Free`.
- Existing placement rejection, target filtering, early return/break behavior, dispel prioritization, target lists, vendor speech gating, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `System:Obsolete` direct-scan check returned no matches in the touched files, explicit pooled-variable check showed matching `Free` calls across the 13 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `System:Obsolete` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-70A` fixed `System:Regions` pooled enumerable ownership.

- 1 row was reviewed and fixed: `RB-05075`.
- `GuardedRegion.CheckGuardCandidate` now pairs the nearby-mobile scan with `try/finally Free`.
- Existing guard-candidate timing, nearest fake guard-caller selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `System:Regions` direct-scan check returned no matches in `GuardedRegion.cs`, explicit pooled-variable check showed matching `Free` calls, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `System:Regions` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-71A` fixed `System:Skills` pooled enumerable ownership.

- 10 rows were reviewed and fixed: `RB-05076` through `RB-05085`.
- Hiding, Peacemaking, Spiritualism, Stealing, Tracking, FrenziedWhirlwind, LightningArrow, WhirlwindAttack, and DoubleWhirlwindAttack scans now pair range results with `try/finally Free`.
- Existing combat detection, area peace filters, corpse first-match behavior, thief witness notices, tracking filters, weapon ability target snapshots, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `System:Skills` direct-scan check returned no matches in the touched files, explicit pooled-variable check showed matching `Free` calls across the 10 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `System:Skills` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-72A` fixed `Trades:Guild` pooled enumerable ownership.

- 10 rows were reviewed and fixed: `RB-05086` through `RB-05095`.
- GuildCarpentry, GuildFletching, GuildHammer, GuildSewing, and GuildTinkering now pair their nearby guildmaster and shoppe scans with `try/finally Free`.
- Existing guildmaster count, owned-shoppe count, map fallback authorization, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Trades:Guild` direct-scan check returned no matches in the touched files, explicit pooled-variable check showed matching `Free` calls across the 10 touched loops, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `Trades:Guild` queued pooled-enumerable rows are complete.

Completed source subbatch: `POST-BATCH-D-73A` fixed `Trades:Harvest` pooled enumerable ownership and closed `POST-BATCH-D`.

- 1 row was reviewed and fixed: `RB-05096`.
- `HarvestSystem.Give` now pairs the drop-at-feet item scan with `try/finally Free`.
- Existing at-feet item snapshot, stack merge behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted `Trades:Harvest` direct-scan check returned no matches in `HarvestSystem.cs`, explicit pooled-variable check showed a matching `Free` call, `Server.csproj` Debug/x86 build passed, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output.
- `POST-BATCH-D` is complete: all 408 pooled-enumerable rows are reviewed, with 406 fixes and 2 false positives.

Next:

Completed source subbatch: `POST-BATCH-E-01A` started `POST-BATCH-E` with the first Boats P1 runtime-hook/gump-guard group.

- 18 rows were reviewed: `RB-01729`, `RB-01938`, `RB-02172`, `RB-02281`, `RB-02829`, `RB-02830`, `RB-02284`, `RB-02282`, `RB-02831`, `RB-02283`, `RB-01730`, `RB-03325`, `RB-03784`, `RB-03785`, `RB-03328`, `RB-03326`, `RB-03786`, and `RB-03327`.
- 8 rows were fixed by adding stale/null/deleted response guards to passive boat gump/prompt handlers, adding stale-state validation to `ConfirmDryDockGump`, and moving the `DockedBoat` null check before `boat.Hue` in `BaseBoat.EndDryDock`.
- 10 rows were reviewed with no source change because source review confirmed non-user WorldSave/timer behavior, existing speech/login guard behavior, or duplicate gump-send marker rows covered by companion response guard fixes.
- Verification passed: targeted Boats hook/gump guard scans, Visual Studio MSBuild `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output. Generated root executable artifacts were restored.

Completed source subbatch: `POST-BATCH-E-02A` continued `POST-BATCH-E` with the first Bulk Orders file slice, `DragonShapeChangeStone-body.cs`.

- 6 rows were reviewed and fixed: `RB-01894`, `RB-01895`, `RB-02208`, `RB-02544`, `RB-03252`, and `RB-03499`.
- `DragonShapeShiftStone.OnSpeech` now guards null/deleted mobiles and missing backpacks before keyword handling.
- `BodyValueGump.OnResponse` now guards null `NetState`, null/deleted or stale mobiles, and deleted shift stones before accepting button replies.
- The context-menu `ChangeBodyValue.OnClick` path now guards stale/deleted stored Mobile and stone state before sending the gump.
- Verification passed: targeted Dragon shape-stone guard scan, Visual Studio MSBuild `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output. Generated root executable artifacts were restored.

Completed source subbatch: `POST-BATCH-E-02B` continued `POST-BATCH-E` with Bulk Orders `BOBFilterGump.cs`.

- 12 rows were reviewed and fixed: `RB-02441`, `RB-03214`, `RB-03215`, `RB-03216`, `RB-03217`, `RB-03218`, `RB-03485`, `RB-04169`, `RB-04170`, `RB-04171`, `RB-04172`, and `RB-04173`.
- `BOBFilterGump.OnResponse` now guards null `NetState`, stale/non-player responders, deleted players, deleted books, and null active filters before filter mutation or gump resend branches.
- Verification passed: targeted BOB filter guard scan, Visual Studio MSBuild `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output. Generated root executable artifacts were restored.

Completed source subbatch: `POST-BATCH-E-02C` completed the Bulk Orders P1 runtime-hook/gump-guard file slices across the large/small BOD accept and display gumps.

- 12 rows were reviewed and fixed: `RB-02442`, `RB-03486`, `RB-02443`, `RB-03219`, `RB-03487`, `RB-04174`, `RB-02444`, `RB-03488`, `RB-02445`, `RB-03220`, `RB-03489`, and `RB-04175`.
- `LargeBODAcceptGump`, `LargeBODGump`, `SmallBODAcceptGump`, and `SmallBODGump` now guard null `NetState`, stale responders, deleted mobiles, deleted deeds, missing backpacks, and stale backpack ownership before accept, cancel, combine, or gump resend branches.
- Verification passed: targeted large/small BOD gump guard scan, Visual Studio MSBuild `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output. Generated root executable artifacts were restored.

Completed source subbatch: `POST-BATCH-E-03A` fixed the Champions P1 movement-hook rows in `PlagueBeastLord.cs`.

- 2 rows were reviewed and fixed: `RB-01997` and `RB-01998`.
- `PlagueBeastLord.OnMovement` now guards null/deleted movers and deleted backpacks before `IsAccessibleTo` and `SendRemovePacket`.
- Existing valid-mover behavior, serialization, public APIs, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted PlagueBeastLord movement-hook guard scan, Visual Studio MSBuild `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output. Generated root executable artifacts were restored.

Completed source subbatch: `POST-BATCH-E-04A` fixed the Clone Offline Player Characters P1 login/logout hook rows.

- 2 rows were reviewed and fixed: `RB-01709` and `RB-01710`.
- `CloneOfflinePlayerCharacters.OnLogout` and `OnLogin` now guard null event args, null mobiles, and deleted mobiles before clone creation/deletion logic.
- Existing valid login/logout behavior, serialization, public APIs, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted Clone Offline login/logout hook guard scan, Visual Studio MSBuild `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output. Generated root executable artifacts were restored.

Completed source subbatch: `POST-BATCH-E-05A` fixed the Crafting Core P1 repair target/CraftGump rows.

- 2 rows were reviewed and fixed: `RB-03221` and `RB-04176`.
- `Repair.InternalTarget.OnTarget` now guards null/deleted targeters, missing craft systems, null targets, and stale/deleted tools before repair processing and CraftGump resend.
- Existing valid repair target behavior, serialization, public APIs, namespaces, type names, save versions, and file location were preserved.
- Verification passed: targeted Repair.cs guard scan, Visual Studio MSBuild `Server.csproj` Debug/x86 build, and `.\ConficturaServer.exe -compileonly -nocache` exited 0 with no `Listening:` output. Generated root executable artifacts were restored.

Next:

1. Continue `POST-BATCH-E` with the next P1 runtime-hook/gump-guard group by deterministic order, currently `Custom:AnimalSystem`.
2. Keep P2 Boats and Bulk Orders command-access rows queued until the P2 command-access pass unless source evidence makes them an urgent local blocker.
3. Preserve serialization, public APIs, namespaces, type names, save versions, and file locations; verify source fixes with `Server.csproj` Debug/x86 build and `.\ConficturaServer.exe -compileonly -nocache`.

## Reorganization Status

Reorganization remains deferred. No source file move should be executed until runtime compile verification is available, save compatibility for affected types is reviewed, documentation/source traces are updated, and rollback is recorded.
