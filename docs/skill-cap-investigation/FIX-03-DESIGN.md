# FIX-03: consistent caps and repeatable origin selection

Started 2026-10-20 on `main` after accepted FIX-02 commit `e27a34891466ab85c27d3b60c0768ed716730a30`. This is the implementation and acceptance plan, not a passing-test claim.

Current results and limitations are in the [acceptance report](FIX-03-REPORT.md). The22:10Z client pause was explicitly lifted at23:01Z; subsequent isolated client001 acceptance and independent review passed. The design below remains the source of the acceptance criteria.

## Resolved policy

The user clarified that characters normally cannot return to the starting area after choosing an origin, but staff may return them. Those characters must be able to choose again and receive that origin's existing resets. Do not impose a once-only marker, novice threshold, age, discovery or prior-origin restriction. Do not add a save migration for origin eligibility. Preserve the explicit alien skill/backpack/clothing reset and other existing origin-specific equipment, profession, story and discovery behavior. A cap reduction alone preserves bases; the accepted FIX-02 grant boundary blocks positive grants while the real total exceeds the cap.

The earlier unanswered eligibility question and usage-saving pause are superseded by the user's clarification and explicit resume. A fresh eligible origin selection is distinct from replaying an already-consumed gump response or awarding Ether twice.

## Source boundary

- `MyServerSettings` in `Data/Scripts/System/Misc/Settings.cs`: one calculation for the four source-defined origins (10000/11000/13000/40000), supported boost units (0..10000 in1000 increments), and Ether (0 or5000), all in tenths. Use it for origin setup, display, load reconciliation, diagnostics and Ether completion. Origin selection retains the current configured boost reset; loading retains the existing supported saved-boost floor rather than reducing it when configuration falls.
- `PlayerMobile.SkillVerification`: replace the numeric cap whitelist with component-based supported-cap diagnostics. Fugitive profession diagnostics depend on the origin component, not a coincidentally equal boosted total. Preserve all progress and bounded asynchronous reporting.
- `PlayerMobile.Deserialize`: preserve every saved field/version/read order. Move existing recognized legacy inference and supported cap reconciliation into the shared helper. Preserve unrecognized component/cap records for review instead of inventing an origin or deleting points. Component-derived cap reductions retain bases; no destructive travel correction returns.
- `GypsyTarotGump` in `Mobiles/Civilized/ShardGreeter.cs`: bind the response to its actor, displayed page, opening map/location and race; validate current page eligibility and basic live/backpack state; consume each response once. Navigation creates a fresh gump. Reopening after a staff return remains available at any skill total, origin or Ether state. Do not add a permanent completion marker or hard-coded start-area entitlement rule.
- `PlayerSettings.SetSavage/SetSpaceMan/SetWanted`: validate the live player/backpack and successful cap setup before their existing side effects. Preserve source-defined resets and region behavior without adding novice restrictions.
- `ApproachObsidian.OnMoveOver`: validate the owned completed tip and current player state at completion, compute the resulting cap from components, and publish the saved Ether completion marker before consumptive/reward effects. A repeat cannot increase the cap or award another reward. Preserve the existing acquisition stat-cap rule and also recognize the Ether marker in `ObeliskOnCorpse`.

No new source file, saved field/type, namespace, project include or core-source change is planned. The accepted tracked core remains unchanged unless implementation proves a core change is necessary; any such change requires a matching validated executable in its source commit.

## Acceptance

Test every origin/boost/Ether combination through initialization, supported load reconciliation and non-destructive verification. Cover recognized legacy caps, unsupported records, configuration increases/decreases, real totals above a reduced cap and rejection of further grants. Compare serialized write/read sequences and exercise a full disposable world save/reload. Test fresh and staff-returned origin selections, intended alien resets, current eligibility, malformed/stale/replayed/other-actor responses and actual ordinary Player rendering/interactions. Complete Ether once with the real tip/tile, reject repeated/stale completion with exact cap/resource assertions, and verify persistence/reconnect.

Use newly pinned disposable copies under `D:\ConficturaUOTestTools\ordered-repairs-20261018\fix03`, the verified client004 configuration lineage and existing guarded ClassicAssist/local Qwen workflow. Keep Qwen input client-visible, record failures independently, and do not label supervised choices autonomous play. Recheck source, protected inputs, copies, process ownership and loopback ports before runtime launch. Restore full copied saves/profiles, stop only owned runtimes, independently review, reconcile the existing tracker record, and commit locally before FIX-04. No push, production restart/save change or live Mira control.
