# FIX-05 Resident feedback

**Current FIX-05 checkpoint (2026-10-19):** All required isolated acceptance gates now passed on candidate004, including client011 real house customization entry/rendering/normal exit. Final independent review, tracker reconciliation and the local acceptance commit are being completed. All owned runtimes are stopped; saves/profiles and protected inputs are restored and verified. FIX-02/03/04 remain unstarted. See [current validation](FIX-05-RESUME-VALIDATION.md).

2026-10-18. Candidate guidance for the [FIX-05 repair](FIX-05-REPORT.md). This is maintainer feedback, not training data or permission to change Resident/Shardmind. No new model training is part of this task. Current runtime/client acceptance passed with the explicit limits in the validation record; source-derived guidance and observed examples remain separate.

## Mechanics and safe responses

| Source-supported mechanic | Observable symptom and safe response |
| --- | --- |
| `CreateFoodSpell.PlaceConjuredItem` uses backpack-only placement and deletes rejected temporary output. Ordinary food and water can succeed independently. | Read the spell's actual delivery message and inspect the backpack before counting a successful result. A partial delivery is not two delivered objects. Do not repeatedly cast into a full pack or assume objects appeared on the ground. |
| `NetState.SendWorldItem` admits world items into a bounded queue; the timer sends complete item packet groups in order. | A dense scene may populate over time. Wait for fresh observations and the requested interaction's actual result before deciding it succeeded. A stale or partial scene is not proof that an item was deleted. |
| World item dispatch rechecks visibility, map, range, parent and deletion state. Old view generations are invalidated on clear-screen changes. | After travel or reconnect, refresh location and nearby observations. Do not continue an action against an item from a previous view solely because its serial remains in memory. |
| Hard work, packet or group limits can disconnect the connection. Diagnostics are bounded and best effort. | A disconnect has several possible causes; it does not establish cheating, excessive skill progress or item loss. Stop a repeated failure loop and preserve the location/time and visible error for staff. Missing diagnostic output is not proof that no overload occurred. |
| Existing staff serial/property commands can set a verified Internal character's next-login destination. | Ordinary Player/Resident actions do not acquire staff privileges. Request authorized assistance for a persistent reconnect trap; do not approach the dense area with another character, delete a pile or reset progress as a default workaround. |

Source traces: [CreateFood.cs](../../Data/Scripts/Magic/Magery/Magery%201st/CreateFood.cs), `PlaceConjuredItem`, `OnCast`; [NetState.cs](../../Data/System/Source/Network/NetState.cs), `SendWorldItem`, `SendWorldItemGroup`, `ReportWorldSend`; [Mobile.cs](../../Data/System/Source/Mobile.cs), `ClearScreen`, `LogoutTimer`, `LogoutLocation`, `LogoutMap`; [SerialCommandImplementor.cs](../../Data/Scripts/System/Commands/Implementors/SerialCommandImplementor.cs), `Execute`.

## Acceptance cases to retain separately

- Positive demonstrations require observed Player connection, fresh location, complete admitted view and successful ordinary interaction. Compare actual effects independently of Resident's status label.
- Crowded login, walk-out/return, map changes and reconnect are different cases. A successful login does not certify all of them.
- Staff recovery requires the verified character serial, completed logout to Internal, readback of both destination properties, command audit evidence and actual Player reconnect at that destination. A successful staff command alone is insufficient.
- Keep permission denials, rejected actions, timeouts, false success labels and first-draft defects in negative evidence. Do not mix them into good examples or erase them after a later successful retry.
- The FIX-01 gate-label and inventory lifecycle observations remain unresolved historical limits. A later successful network test does not establish their cause or repair.

The previous no-demonstration statement is superseded by the client007/client009 bounded evidence below. The external `D:\ConficturaUOTestTools\ordered-repairs-20261018` package records source identity, isolated fixtures, raw results, save/profile restoration and controller lifecycle. Only sanitized outcomes belong in this repository.


## Final resumed checkpoint

**Final checkpoint: held at manual client acceptance.** No authenticated Nephtest connection or manual Play confirmation arrived, and no gameplay helper operation or supervised Qwen choice executed. All task-owned client003 supervisors stopped; cleanup at 2026-10-18T16:23:36Z verified 14 protected input records unchanged, disposable saves/profiles restored, recorded process identities absent and ports 25215/25291/25292/21447 free. Native control was never activated. No new commit was made. FIX-02/03/04 remain unstarted.

See [resumed validation](FIX-05-RESUME-VALIDATION.md) for completed evidence and the exact next gate.


## 2026-10-19 observed feedback

- Actual candidate004 reconnect/recovery, queue completion, native movement/rendering and house-sign interaction passed in isolated copies. Keep each case and source identity distinct.
- Local Qwen chose the observed disposable corpse. The bridge returned dispatched with no confirmed change; native inspection independently showed its populated container. A dispatch label alone remains insufficient. No loot transfer or autonomous run was tested.
- Crowded Resident travel in client006/007 exceeded the unchanged observation lease (about4.83s environment preparation versus3s lease); movement did not occur. Retain that negative example. Native walking and later one-step sign approach do not erase it.
- The Price 0 disposable-house menu lacked customization. Normal placement supplies30500 for0x13EC; the corrected fixture is compiled/reviewed but customization entry/exit remain NOT RUN because supported native activation failed. No production housing policy was relaxed.
- Discovery and screenshots working do not establish input availability. Stop native input after the documented recovery retry fails, preserve the exact error, and do not repeat permission setup.

## Client011 customization and native retry

The 2026-10-19 retry completed saved login and the exact named guarded macro without changing permissions. A fresh client-observed house menu/button entered customization; native designer rendering and independent server DesignContext agreed. Normal native close removed the editor and cleared DesignContext with unchanged progression and empty queues. Full restoration/cleanup passed. No new Qwen inference ran; C9 supervised corpse choice remains the relevant model evidence.

The failed client010 activation is retained with unknown cause. C11 also showed one stale accessibility index and a vertical screenshot/input coordinate discrepancy: refresh/reselect resolved the index; an initial right-click above the editor and Erase-mode selection made no house modification. Confirm actual UI state after every action, and do not label a mode selection or dispatched command as a completed effect. These are harness/UI observations, not shard defects or approved training examples. Evidence: client011 native-customization-result, actual-client, response XML and cleanup files under the external run root.
