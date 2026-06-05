# System: Bulk Orders

## Classification

GameplayLayer

## Summary

Bulk order crafting objective and reward system.

## Source Files

Matched source files: 60.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/DragonShapeChangeStone-body.cs | GumpUI | OnSpeech | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Armor/ArmorEnums.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Clothing/MiddleTorso.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Containers/BuriedBody.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/DeadBodyDeedEW.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/DeadBodyDeedNS.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Magical/Gifts/Clothing/GiftMiddleTorso.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Magical/God/Clothing/LevelMiddleTorso.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/BodyParts.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/BonePile.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/Head.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/LeftArm.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/LeftLeg.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/RibCage.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/RightArm.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/RightLeg.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/Torso.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs | StartupWiring | CustomTimerSubclass;Initialize | Yes |  |
| Data/Scripts/Items/Misc/Bodies/Corpses/CorpseNameAttribute.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Misc/Bodies/Corpses/DecayedCorpse.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Items/Misc/Bodies/Corpses/Packets.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/LivingDead/DarkHeart.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Misc/Bodies/LivingDead/SummonCorpse.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/PurityOfBodyScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Spells/PurityOfBody.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Syth/Spells/Clone.cs | CombatPolicy |  | No |  |
| Data/Scripts/Mobiles/Base/BaseVendor.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Undead/Bodak.cs | Persistence |  | Yes |  |
| Data/Scripts/Quests/Frankenstein/FrankenItem.cs | Persistence |  | Yes |  |
| Data/Scripts/System/Commands/BodyValues.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/System/Commands/Docs.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/System/Gumps/Properties/SetBodyGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Trades/Bulk Orders/BulkMaterialType.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Bulk Orders/LargeBOD.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Trades/Bulk Orders/LargeBODTarget.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Bulk Orders/LargeBulkEntry.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/LargeSmithBOD.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/LargeTailorBOD.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/Rewards.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Bulk Orders/SmallBOD.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Trades/Bulk Orders/SmallBODTarget.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Bulk Orders/SmallBulkEntry.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Bulk Orders/SmallSmithBOD.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/SmallTailorBOD.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/Books/BOBFilter.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/Books/BOBLargeEntry.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/Books/BOBLargeSubEntry.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/Books/BOBSmallEntry.cs | Persistence |  | Yes |  |
| Data/Scripts/Trades/Bulk Orders/Books/BODType.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Bulk Orders/Books/BulkOrderBook.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Trades/Bulk Orders/Books/Gumps/BOBFilterGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Trades/Bulk Orders/Books/Gumps/BOBGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Trades/Bulk Orders/Books/Gumps/BODBuyGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Trades/Bulk Orders/Gumps/LargeBODAcceptGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Trades/Bulk Orders/Gumps/LargeBODGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Trades/Bulk Orders/Gumps/SmallBODAcceptGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Trades/Bulk Orders/Gumps/SmallBODGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/System/Source/Attributes.cs | StartupWiring |  | No |  |
| Data/System/Source/Body.cs | StartupWiring |  | No |  |

## Data Files

Data/System/Bulk Orders/{0}/{1}.cfg;Data/System/CFG/body.cfg;models/models.txt

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Commands/BodyValues.cs; Line=22; LikelySystem=System:Commands; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Commands/BodyValues.cs:22 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Commands/Docs.cs; Line=18; LikelySystem=System:Commands; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Commands/Docs.cs:18 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;Initialize;Initialize;OnSpeech;Timer.DelayCall

## Serialized State

Serialized marker files: 37. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Crafting Core; vendors; rewards; gumps

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Bulk_Order_System.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
