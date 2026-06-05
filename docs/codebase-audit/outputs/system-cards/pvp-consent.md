# System: PvP Consent

## Classification

GameplayLayer

## Summary

Combat consent and event-gate policy surface with player and staff-facing implications.

## Source Files

Matched source files: 6.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/PvPConsent/GoddessOfProtection.cs | StartupWiring | CustomTimerSubclass;OnMovement;OnSpeech | Yes | SendGump |
| Data/Scripts/Custom/PvPConsent/NONPKEventMoongate.cs | StartupWiring | CustomTimerSubclass | Yes | OnResponse;SendGump |
| Data/Scripts/Custom/PvPConsent/NONPKSword.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/PvPConsent/PKSword.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/PvPConsent/Gumps/PKNONPKGUMP.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/Mobiles/Base/PlayerMobile.cs | StartupWiring | EventSink;Initialize;OnLogin;OnLogout;OnSpeech;Timer.DelayCall | Yes | OnResponse;SendGump |

## Data Files

No XML/config/text/json references were found in Phase 1 string-reference markers.

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/PvPConsent/Gumps/PKNONPKGUMP.cs; Line=89; LikelySystem=Custom:PvPConsent; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/PvPConsent/Gumps/PKNONPKGUMP.cs:89 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;OnMovement;OnSpeech;EventSink;Initialize;OnLogin;OnLogout;OnSpeech;Timer.DelayCall;Initialize

## Serialized State

Serialized marker files: 5. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

PlayerMobile; Notoriety; Regions; event gates; harmful and beneficial action checks

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/PvP_Consent_System.md;docs/wiki/pvp-consent-system.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
