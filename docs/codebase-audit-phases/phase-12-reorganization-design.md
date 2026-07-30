# Phase 12: Reorganization Design

## Purpose

Phase 12 designs the target organization before moving files. It separates
architecture decisions from mechanical moves so reviewers can evaluate risk,
save compatibility, project updates, and documentation changes first.

## Required Inputs

- System Cards.
- Dependency Graph.
- Serialization Register.
- Project Truth Register.
- Documentation Truth Table.
- Risk-specific findings.

## Required Outputs

- Target layout proposal.
- Move proposal table.
- Keep-in-place decision table.
- Third-party containment plan.
- Save compatibility notes.
- Project update plan.

## Subphase 12.1: Organization Principles

Use these principles:

- Runtime role beats folder nostalgia.
- Save-sensitive types require extra approval.
- Imported packages may be isolated instead of restyled.
- Framework roots such as `Items`, `Mobiles`, `Magic`, `System`, and `Trades`
  should not be emptied just to force everything into `Custom`.
- Documentation must move with code.
- Project includes must be updated with code moves.

Completion gate:

- Reviewers agree on how target folders are chosen.

## Subphase 12.2: Proposed Custom Layout

Evaluate these target folders:

- `Data/Scripts/Custom/Core`
- `Data/Scripts/Custom/Progression`
- `Data/Scripts/Custom/PvE`
- `Data/Scripts/Custom/PvP`
- `Data/Scripts/Custom/Combat`
- `Data/Scripts/Custom/Magic`
- `Data/Scripts/Custom/Economy`
- `Data/Scripts/Custom/Crafting`
- `Data/Scripts/Custom/Housing`
- `Data/Scripts/Custom/Travel`
- `Data/Scripts/Custom/Government`
- `Data/Scripts/Custom/Events`
- `Data/Scripts/Custom/StaffTools`
- `Data/Scripts/Custom/Integrations`
- `Data/Scripts/Custom/Legacy`
- `Data/Scripts/Custom/ThirdParty`

Completion gate:

- Each proposed folder has a clear ownership rule.

## Subphase 12.3: Existing Root Preservation

Decide what should remain under:

- `Items`
- `Mobiles`
- `Magic`
- `System`
- `Trades`
- `Quests`

Reasons to keep in place:

- core framework role;
- save type stability;
- conventional RunUO location;
- large content family;
- namespace expectations;
- project organization already clear.

Completion gate:

- The plan does not blindly move stable framework code.

## Subphase 12.4: Move Proposal Table

Every move proposal must include:

| Field | Meaning |
| --- | --- |
| `CurrentPath` | Existing file/folder. |
| `TargetPath` | Proposed destination. |
| `System` | Owning system. |
| `Reason` | Why the move helps. |
| `NamespaceChange` | None, proposed, or forbidden. |
| `SaveRisk` | Safe, caution, high, do not move. |
| `ProjectUpdate` | Required include changes. |
| `DocsUpdate` | Pages to update. |
| `Verification` | Build/check commands. |
| `Rollback` | How to undo safely. |

Completion gate:

- No move lacks a rollback and verification plan.

## Subphase 12.5: Third-Party And Imported Systems

For imported systems:

1. Identify package boundaries.
2. Preserve upstream folder structure when it aids maintenance.
3. Document local modifications.
4. Avoid style-only rewrites.
5. Isolate in `ThirdParty` only when it reduces confusion.

Completion gate:

- Imported systems remain understandable.

## Subphase 12.6: Namespace Plan

Classify namespace changes:

- no change;
- future preferred namespace;
- unsafe due to serialization;
- unsafe due to reflection/config;
- requires migration.

Completion gate:

- Namespace cleanup does not accidentally become save-breaking.

## Subphase 12.7: Documentation Move Plan

For each move:

- update source traces;
- update wiki links;
- update system cards;
- update project truth;
- update dependency graph evidence.

Completion gate:

- Docs stay accurate after reorganization.

## Review Checklist

- Target folders have ownership rules.
- Existing roots are respected where appropriate.
- Move proposals include save risk.
- Project updates included.
- Documentation updates included.
- Rollback plan included.

## Exit Criteria

Phase 12 is complete when the reorganization proposal is detailed enough to
execute in small, reviewable batches without guessing about build, save, or docs
impact.
