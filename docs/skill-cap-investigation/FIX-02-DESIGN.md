# FIX-02 transactional skill grants: design and acceptance

Started 2026-10-19 after accepted FIX-05 implementation `69cc69b6ac2f7e474a9e2aee4ca5dead7bb95fb7` and documentation closeout `ed918c456c0c83839b7b3516184d568d1066bf8e`. This preserves the pre-implementation design. Current implementation, acceptance and limits are in the [FIX-02 report](FIX-02-REPORT.md). FIX-03 and FIX-04 remain later ordered batches.

## Confirmed defect and boundary

`SkillCheck.Gain` currently lowers a Down donor before calculating the final Alacrity increment. Its later total-cap rejection can retain that loss without granting anything. `Skill.BaseFixedPoint` updates one base/cache pair and immediately invokes invalidation/network/mobile callbacks; a prevalidated loop of ordinary setters would still expose partial transfers to those callbacks.

The proposed common core boundary validates the independent sum of base values, target membership, Up lock, individual cap, complete donor losses and final aggregate cap before changing any base. Donors must belong to the same collection, be Down, differ from the target, and have sufficient points. Total donor loss must not exceed the actual grant. Already excessive players receive no positive grant, and rejection removes no existing points. Publish all changed bases and the resulting total before notifications. Reject nested grants during notification, contain/report notification faults after commitment, and reconcile the cache after trusted callback changes. This is a logical game-thread transaction, not a replacement for the world's threading model.

Low-level raw setters, NPC initialization, character creation, deserialization and explicit staff restoration remain separately classified trusted paths. No serialization order/version, saved type, namespace or project inclusion change is planned. The new boundary must not infer an ordinary player exemption from a mutable `Mobile.Player` flag.

## Caller preservation and open review

- Normal gain: determine the final accelerated amount and individual headroom before planning a donor. Preserve gain/stat probabilities and the existing single-donor selection policy; never lower a donor for a rejected final grant.
- Transcendence scroll: preserve individual-cap clamping and existing full-increment single-donor transfer behavior; consume only after a committed grant.
- Soulstone and teaching: preserve their current target values, eligibility/payment rules and aggregate Down-point planning; commit the complete transfer before callbacks.
- Election bonus: preserve the configured bonus and repeat-voter rule, with both caps and locks enforced. Revalidate the live target at response.
- XML SKILL assignment: a positive absolute assignment must succeed in full within both caps and the Up lock or reject without partial award/donor loss. Generic reflection protection is coordinated with FIX-04; no arbitrary public-setter clamp is planned.
- TestCenter and staff/clone restoration: inventory their deliberate exceptions and existing audit/access boundaries before choosing any changed path.
- SkillStone: independent source review found no declared lock exception. Apply the ordinary Up-lock rule, retain the existing absolute-target affordability check and per-entry purchases, and revalidate actor/backpack/assignment at response. Correct its Cancel button to the cancellation ID and reject replay/invalid entries. This tightens the previously missing lock and response guards without changing stone prices.

## Required verification

Exercise production methods against a freshly pinned disposable candidate: real/cache totals, all lock states, individual/aggregate boundaries, fractional fixed-point values, rejection with unchanged donors, correct multi-donor conservation, final accelerated increments, stale/replayed/deleted resources, callback observation/reentrancy/failure, staff/NPC/load exceptions, and valid ordinary gains. Preserve existing FIX-01 progress protection and FIX-05 networking. Build the operational core and force runtime script compilation. Use full-copy world/save-load and actual guarded Player/client/Qwen acceptance where needed to establish the changed gameplay effects; component fixtures alone are not real-client proof.

Before any runtime launch, verify protected inputs, fresh source identity, copied settings, process identities and loopback ports. Restore complete copied saves/profiles and stop only owned runtimes afterward. Record feedback, negative attempts and independent review, reconcile the active tracker, and make a focused local commit only after the final matrix passes. No push, live restart, production-save operation or original Mira/Resident/Shardmind modification.
