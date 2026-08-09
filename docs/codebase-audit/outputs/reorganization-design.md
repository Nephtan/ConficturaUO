# Reorganization Design

Generated: 2026-06-05T18:35:10.5279801-05:00

## Principles

- Runtime role beats folder nostalgia.
- Folder moves do not authorize namespace or serialized type renames.
- Framework roots stay intact unless a later batch proves a custom extension can move safely.
- Imported packages may be contained, but not restyled.
- Every executed move needs `Scripts.csproj`, docs, dependency, serialization, verification, and rollback updates.

## Proposed Custom Layout

| Folder | Ownership Rule | Move Policy |
| --- | --- | --- |
| Data/Scripts/Custom/Core | Small Confictura-native shared services that are not RunUO framework roots and do not own broad persistence surfaces. | Design candidate; avoid PlayerMobile and framework roots. |
| Data/Scripts/Custom/Progression | Confictura-native character progression systems and active-play progression helpers. | Allowed only as small batches with project truth updates. |
| Data/Scripts/Custom/PvE | Confictura-native automated PvE objective systems. | Keep reward/balance review tied to Phase 9/10 findings. |
| Data/Scripts/Custom/PvP | Confictura-native PvP consent, event gates, and combat-policy extensions. | Requires consent/region/government policy review. |
| Data/Scripts/Custom/Combat | Confictura-native combat behavior extensions that are not magic schools or core weapons. | Imported AI packages may be contained without restyling. |
| Data/Scripts/Custom/Magic | Confictura-native magic extensions outside established `Data/Scripts/Magic` school roots. | Do not move established Magic root schools without separate save/build review. |
| Data/Scripts/Custom/Economy | Confictura-native economy systems that are not RunUO vendors, banking, or Trades frameworks. | Requires economy-loop review. |
| Data/Scripts/Custom/Crafting | Confictura-native custom crafting packages that are not existing Trades frameworks. | Preserve imported package structure. |
| Data/Scripts/Custom/Housing | Custom housing extensions that do not belong in RunUO `Items/Houses` framework. | Do not move core house items or save-heavy house signs. |
| Data/Scripts/Custom/Travel | Custom travel tools outside RunUO boat and door framework code. | Keep core boats in `Items/Boats`. |
| Data/Scripts/Custom/Government | Confictura government package after persistence and project truth cleanup. | Folder move only; namespace/type rename forbidden without migration. |
| Data/Scripts/Custom/Events | Staff or automated event packages that create temporary shard-wide gameplay. | Review staff/event balance and save risk first. |
| Data/Scripts/Custom/StaffTools | Staff-only commands, gumps, and world-editing tools. | Access review required before moves. |
| Data/Scripts/Custom/Integrations | Bridge code between Confictura systems and imported services. | Keep dependency comments and docs source traces aligned. |
| Data/Scripts/Custom/Legacy | Compatibility shims retained for saves, commands, or old behavior. | Do not delete or disable from this phase. |
| Data/Scripts/Custom/ThirdParty | Imported packages whose upstream shape matters to maintenance. | Contain or document; avoid style-only rewrites. |

## Move Design Status

14 move proposals were generated. All are `DesignOnlyNotExecuted`; no files were moved in Phase 12.

## Hard Gates

- Repair or explicitly plan around Phase 2 project truth drift before executing moves.
- Do not rename namespaces or serialized types without migration approval.
- Run project truth, serialization, dependency, documentation truth, and build verification for each executed batch.
