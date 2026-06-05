# System: Staff Toolbar

## Classification

StaffEventTool

## Summary

Staff-facing toolbar and administrative convenience surface.

## Source Files

Matched source files: 1.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/Staff Toolbar [2.0]/Toolbar.cs | StartupWiring | EventSink;Initialize;OnLogin | Yes | OnResponse;SendGump |

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

EventSink;Initialize;OnLogin

## Serialized State

Serialized marker files: 1. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Access levels; staff commands; gumps

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Staff_Toolbar.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
