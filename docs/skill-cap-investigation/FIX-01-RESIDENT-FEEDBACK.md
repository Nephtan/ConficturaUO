# FIX-01 Resident feedback

2026-10-18. Sanitized maintainer guidance for implementation commit `1b49df74e8b972eb53805a790797f1f4872ab001`. The implementation is committed locally, not pushed or deployed. This evidence batch accompanies it. These observations are not approved training examples; no model training was performed.

## Reliable mechanics and source traces

| Mechanic | Source at the implementation commit | Safe interpretation |
| --- | --- | --- |
| Region validation preserves existing progression. Null/deleted inputs and Counselor-or-higher characters return early. | [PlayerMobile.cs](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs), `SkillVerification`, lines 1779–1840; [BaseRegion.cs](../../Data/Scripts/System/Regions/BaseRegion.cs), `OnEnter`, line 255. | A diagnostic is an observation, not a correction, sanction or restoration. Do not infer cheating from an unrecognized legacy cap. Not every region override invokes this path. |
| Real skill quantities are integer tenths. The verifier independently sums bases and compares that sum with the cached total and cap. | `PlayerMobile.SkillVerification`, lines 1790–1819; [Skills.cs](../../Data/System/Source/Skills.cs), `Skill.BaseFixedPoint` and `Base`, lines 556–590. | Keep real sum, displayed/cached total, cap and components distinct. A cache mismatch alone does not establish how much progress a character legitimately earned. |
| Diagnostics are best effort: five minutes per character and one global pending background write. Output errors are contained. | `ReportSkillVerification`, `WriteSkillVerificationDiagnostic`, `AppendSkillVerificationDiagnostic`, PlayerMobile lines 1842–1896. | Missing log output does not prove validation did not run. Busy, failed or interrupted writes may be lost. Log units are tenths; the callback region can describe the previous region. |
| Ordinary gain checks and load-time cap recomputation are unchanged. | [SkillCheck.cs](../../Data/Scripts/System/Skills/SkillCheck.cs), `Gain`, lines 645–697; `PlayerMobile.Deserialize`, line 4654. | FIX-01 does not fix false cache/player state, direct grants or every gain edge case. Consistent components roundtripped; arbitrary inconsistent caps are not promised to survive load unchanged. FIX-02/03/04 remain separate. |

## Observable responses

- Confirm fresh character identity, connection and actual destination before judging a movement or interaction. Client labels and model narration are insufficient evidence of an effect.
- Compare available skill/stat observations before and after travel. A privileged test evaluator supplied independent checks in this acceptance run; the model received client observations only. Do not imply ordinary Resident access to hidden server state.
- Treat explicit server refusal, stale observations or missing confirmation as a failed or unconfirmed action. Refresh observations and use an appropriate ordinary retry only when its prerequisites are established.
- Preserve evidence and seek an authorized recovery decision when progress or inventory differs. Do not reset skills, normalize components, replace items or change player identity/cache to manufacture a successful comparison.

## Positive bounded cases

The isolated ordinary Player completed 30 client-driven BaseRegion crossings covering 500/1200, exact/over cap, two profession mismatches and high/low caches. All 58 skill bases/caps/locks, raw stats/stat locks, profession, keys, totals and cap components were retained. Each controlled scenario produced one diagnostic despite repeated crossings; the fixture reset the diagnostic deadline only between scenarios.

Normal Arms Lore gained 0.1 below cap; eight attempts at cap with no Down donor gained zero and produced no new excess. Ordinary Recall, same-map gates and server MapIndex 1 to 6 to 1 travel retained progress. Those maps share client facet 1. Actual logout, save/reload and reconnect retained consistent progression state. Three local Qwen choices executed crossing, return and inspect under supervision with autonomy disabled; this is not a sustained autonomous run. Generated speech was not transmitted.

## Negative observations and limits

- **Gate reporting defect:** Resident labeled an out-of-range refusal as `succeeded`. A closer ordinary retry reached the destination but was labeled `dispatched`. Independent destination/progress checks established the outcomes. The failed attempt is retained; this reporting behavior was not repaired by FIX-01.
- **Inventory lifecycle observation:** Exact in-memory inventory comparison failed: the original bank-box serial was absent after reload, and a candle moved from equipment to the backpack during play. The cause is unestablished. Progress restoration passed, while the separate exact inventory comparison did not. Complete input saves and client profiles were then restored and hash-verified.
- No live production acceptance, different-client-facet acceptance, automated player recovery, broader grant policy or model-training claim follows from these cases.

See the [FIX-01 report](FIX-01-REPORT.md) for verification and policy boundaries. Detailed local evidence remains under `D:\ConficturaUOTestTools\fix01-20261018\client-acceptance-20261018`, especially `REPORT.md`, `evidence/acceptance-validation.json`, `evidence/actual-client.jsonl`, `evidence/restore-inventory-difference.json` and `evidence/file-restoration.json`. Raw prompts, private configuration, save data and privileged fixtures are intentionally excluded from this document and the documentation commit.
