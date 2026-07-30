# Phase 11: Inline Code Documentation

## Purpose

Phase 11 adds comments in source only where they prevent future mistakes. The
goal is not to narrate C#. The goal is to preserve non-obvious gameplay rules,
save compatibility constraints, global hook reasoning, and cross-system
dependencies.

## Required Inputs

- Comment Target Register.
- Serialization Register.
- Runtime Hook Map.
- Dependency Graph.
- Risk-specific findings.
- Local coding conventions.

## Required Outputs

- Reviewed comment target list.
- Source comments for approved targets.
- Rejected comment list for unnecessary comments.
- Verification notes.

## Comment Target Table

| Field | Meaning |
| --- | --- |
| `File` | Source file. |
| `Location` | Type, method, field, or block. |
| `CommentType` | Serialization, global hook, gameplay exception, dependency, XML schema, pooled enumerable, staff-event assumption, legacy. |
| `Reason` | Mistake prevented. |
| `DraftComment` | Proposed text. |
| `Approved` | Yes/no. |

## Subphase 11.1: Serialization Comments

Add comments where:

- field order is fragile;
- old version reads look strange;
- fields are consumed but discarded;
- linked objects are repaired after load;
- timers are restarted after deserialize;
- move/rename risk is high.

Do not comment every version integer unless the local context is fragile.

Completion gate:

- Future serializer edits have warnings where needed.

## Subphase 11.2: Global Hook Comments

Add comments where:

- startup registration affects the whole shard;
- event order matters;
- duplicate subscription is avoided intentionally;
- staff tools are process-wide;
- packet overrides replace or extend core behavior.

Completion gate:

- Non-obvious global behavior is explained at registration or handler boundary.

## Subphase 11.3: Gameplay Exception Comments

Add comments where a system intentionally bypasses normal rules:

- PvE event gate behavior.
- Government city-ban combat exceptions.
- XMLPoints event overrides.
- protected or staff-only behavior.
- special map or region rules.

Completion gate:

- Exceptions are visibly intentional.

## Subphase 11.4: Cross-System Dependency Comments

Add comments where direct code references are not enough to explain the
dependency:

- Character Level to Random Encounters.
- PvP Consent to Notoriety and Regions.
- XMLSpawner attachments to cleanup systems.
- Government to PlayerMobile city fields.
- Spell registry to high-ID spell families.

Completion gate:

- Maintainers know why the dependency exists.

## Subphase 11.5: XML And Config Comments

Add comments where:

- config nodes are intentionally ignored or deprecated;
- defaults are dangerous;
- hot reload behavior matters;
- schema is implicit in parser code.

Completion gate:

- Data-driven behavior can be safely edited.

## Subphase 11.6: Pooled Enumerable Comments

Add comments only where ownership is non-obvious:

- enumerables passed across helpers;
- early returns require cleanup;
- nested scans complicate ownership.

Completion gate:

- Future edits do not reintroduce leaks.

## Subphase 11.7: Comment Rejection

Reject comments that:

- restate method names;
- explain basic C# syntax;
- describe obvious assignments;
- duplicate nearby documentation;
- freeze stale assumptions.

Completion gate:

- Comments remain high-signal.

## Review Checklist

- Comments follow local style.
- Comments are near the risky code.
- Comments are brief.
- Comments explain why, not what.
- No broad formatting churn.

## Exit Criteria

Phase 11 is complete when approved high-risk comment targets are documented in
source and unnecessary comments have been rejected rather than added.
