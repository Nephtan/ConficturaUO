# System: Harvest System

## Classification

SharedService

## Summary

Resource harvesting framework and economy input surface.

## Source Files

Matched source files: 21.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Items/Magical/Gifts/Weapons/Swords/GiftBoneHarvester.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Magical/God/Weapons/Swords/LevelBoneHarvester.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Trades/Harvest Tools/BaseHarvestTool.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Trades/Harvest Tools/GargoylesPickaxe.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Trades/Harvest Tools/ProspectorsTool.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Trades/Harvest Tools/Shovel.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Trades/Harvest Tools/SturdyPickaxe.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Trades/Harvest Tools/SturdyShovel.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Weapons/Swords/BoneHarvester.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Harvest/BonusHarvestResource.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Harvest/Fishing.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Trades/Harvest/HarvestBank.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Harvest/HarvestDefinition.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Harvest/HarvestResource.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Harvest/HarvestSoundTimer.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Trades/Harvest/HarvestSystem.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Harvest/HarvestTarget.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Harvest/HarvestTimer.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Trades/Harvest/HarvestVein.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Harvest/Lumberjacking.cs | StartupWiring | Initialize | No |  |
| Data/Scripts/Trades/Harvest/Mining.cs | Crafting |  | No |  |

## Data Files

No XML/config/text/json references were found in Phase 1 string-reference markers.

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review items, NPCs, speech, regions, and gumps in later phases. |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;Initialize;Timer.DelayCall

## Serialized State

Serialized marker files: 9. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Crafting; resources; map/region rules

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Harvest_Resource_System.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
