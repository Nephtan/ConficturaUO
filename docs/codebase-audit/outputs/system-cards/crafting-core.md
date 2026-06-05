# System: Crafting Core

## Classification

SharedService

## Summary

Shared crafting framework and gump surface.

## Source Files

Matched source files: 21.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Trades/Core/CraftContext.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftGroup.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftGroupCol.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftItem.cs | StartupWiring | CustomTimerSubclass | No | SendGump |
| Data/Scripts/Trades/Core/CraftItemCol.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftItemIDAttribute.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftRes.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftResCol.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftSkill.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftSkillCol.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftSubRes.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftSubResCol.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CraftSystem.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/CustomCraft.cs | Crafting |  | No |  |
| Data/Scripts/Trades/Core/Enhance.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Trades/Core/Recipes.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Trades/Core/Repair.cs | StartupWiring | Timer.DelayCall | No | SendGump |
| Data/Scripts/Trades/Core/Resmelt.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Trades/Core/Gumps/CraftGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Trades/Core/Gumps/CraftGumpItem.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Trades/Core/Gumps/QueryMakersMarkGump.cs | GumpUI |  | No | OnResponse |

## Data Files

No XML/config/text/json references were found in Phase 1 string-reference markers.

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Trades/Core/Recipes.cs; Line=13; LikelySystem=Trades:Core; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Trades/Core/Recipes.cs:13 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Trades/Core/Recipes.cs; Line=18; LikelySystem=Trades:Core; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Trades/Core/Recipes.cs:18 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;Initialize;Timer.DelayCall

## Serialized State

No serialization marker rows were found in matched files.

## Dependencies

Trades; resources; items; bulk orders

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Crafting_Core.md;docs/wiki/Standard_Crafting.md

## Verification Status

NeedsRuntimeReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
