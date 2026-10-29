# Decision pending: territorial champion or ordinary fleeing creature

No implementation approval or production behavior change is claimed here.

## Observed conflict

The running-enabled reference010 reaches Mephitis's final fifth of health at about19 minutes. At a later observation near27 minutes the actor remains alive/full while the boss remains around its final fifth. It ultimately stops alive/incomplete at2261.441 seconds, with4,841 running requests. No supplied resource, ability, health or AI override was used. Client memory/search reacquires the boss, but repeated low-health escape and arena return consume time without equivalent combat progress. This is not accepted as a complete pacing sample.

Separately, the actual historical copied Zanthus completes route002 in277.502 seconds using the unchanged original macro/helper with explicit idle walking assistance. That earned-power stress result is not a veteran-reference sample, and it demonstrates that the current boss can be killed. Neither observation establishes that a melee player cannot overcome the existing fleeing behavior.

Source explains a behavior worth deciding deliberately. Mephitis selects AI_Melee. MeleeAI.DoActionCombat can enter Flee below20% health for qualifying uncontrolled creatures; DoActionFlee continues until above50%. The outbreak footprint guard can return a creature that leaves its allowed area. Together these existing rules can produce a chase/return cycle. More efficient player interception remains possible; these observations do not prove that ordinary fleeing is impossible to overcome.

The specification allows automatic tuning of health, damage and web interval only. Changing the boss's behavior is a design revision requiring the user's decision, not an automatic tuning pass.

## Concrete recommendation

Make **only outbreak Mephitis defend its territory**. Preserve ordinary Mephitis, guardians, defenders, attacks, damage, poison, webs, player capabilities and rewards. Use the existing normal combat action dispatch rather than a new free attack.

Proposed source change, still unapplied: in ArachnidOutbreakChampion.OnThink, after the existing controller/bounds guards and before base.OnThink, normalize the ordinary flee action:

    if (AIObject != null && AIObject.Action == ActionType.Flee)
        AIObject.Action = ActionType.Combat;

This is a planning-state change, not an attack call. It needs actual runtime/AP verification; the snippet alone does not establish correct scheduling. MeleeAI can select Flee again at the end of DoActionCombat; verify that subsequent normal dispatch restores combat before flee movement without starving ordinary attacks. BaseCreature.CheckFlee is a separate timed flee path, so overriding it alone would not establish this behavior. No serialization field/type or global AI change is proposed.

After approval: add a targeted low-health comparison, verify ordinary Mephitis remains unchanged, rerun outbreak/legacy regressions and forced runtime compilation, verify no off-turn attack/AP bypass, and collect three complete comparable reference runs before judging the20–35 minute median. Keep the same finite legal player capabilities. Preserve all failed/partial pilots and leave production disabled.

## Alternatives

1. Keep ordinary fleeing and improve pursuit/interception or evaluate legitimate counter-tools before reconsidering the reference. This preserves existing AI but can retain a prolonged chase.
2. Design a deliberate retreat phase with readable counterplay. This could fit the world's identity, but adds mechanics and needs a new specification rather than a concealed behavior patch.

The larger question is what a champion is defending. A nest guardian standing its ground fits this event's brood/discovery structure. Other future champion types may deserve explicit retreat behavior. Neither choice should be achieved by weakening earned character power or by treating a raw movement loop as challenge.

## Source and evidence

- [Mephitis.cs](../../Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Mephitis.cs): constructor selects AI_Melee.
- [Behavior.cs](../../Data/Scripts/Mobiles/Base/Behavior.cs): MeleeAI.DoActionCombat/DoActionFlee; BaseAI.Think dispatch and ProcessTurnBasedPulse.
- [ArachnidOutbreakCreatures.cs](../../Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakCreatures.cs): guarded owned champion OnThink.
- [ArachnidOutbreakController.cs](../../Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakController.cs): KeepCreatureInBounds.
- [SPECIFICATION.md](SPECIFICATION.md): approved behavior,20–35 minute reference, earned-power preservation and automatic tuning limits.
- [BENCHMARK.md](BENCHMARK.md): prior retained failures, ordinary running request proof, frozen policy and separate pacing gate. Reference010's terminal partial and preservation are retained under D:/ConficturaUOTestTools/outbreak-benchmark-20261029-010/evidence.
- [ARCHMAGE_STRESS.md](ARCHMAGE_STRESS.md): separate original-macro route002 full clear and preservation, without veteran pacing credit.
