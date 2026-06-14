# System: OmniAI

## Classification

ImportedPackage

## Summary

Imported AI package with multiple ability modules.

## Source Files

Matched source files: 8.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/OmniAI/AITester.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/OmniAI/OmniAI Bushido.cs | Unknown |  | No |  |
| Data/Scripts/Custom/OmniAI/OmniAI Core.cs | Unknown |  | No |  |
| Data/Scripts/Custom/OmniAI/OmniAI Knightship.cs | Unknown |  | No |  |
| Data/Scripts/Custom/OmniAI/OmniAI Magery.cs | Unknown |  | No |  |
| Data/Scripts/Custom/OmniAI/OmniAI Necromancy.cs | Unknown |  | No |  |
| Data/Scripts/Custom/OmniAI/OmniAI Ninjitsu.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Custom/OmniAI/OmniAI Shared.cs | StartupWiring | EventSink | No |  |

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

EventSink;Timer.DelayCall

## Serialized State

Serialized marker files: 1. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Mobiles; combat; magic abilities

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/codebase-audit/outputs/system-cards/omniai.md;docs/wiki/OmniAI.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
