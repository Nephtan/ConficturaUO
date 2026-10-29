# Archmage stress evidence

Status: native preparation accepted; stationary combat and route-assisted full clear remain open. Production stays disabled.

## Actual inputs and privacy

Read-only discovery found the selected archmage-overlord entry in the personal ClassicAssist profile. Its inline Macro field is empty because FilePath points to an external script. The imported archmage_combat helper was found alongside it. Both scripts and the selected profile have private hash-preserved snapshots under outbreak-zanthus-source-20261029-001. The personal client was never launched or modified.

Matching-serializer inspection of the historical copied save finds exactly one Zanthus, already a living unblessed Player. Private copied skill/gear observations agree with the helper's principal ancient damage, casting and mana-cost assumptions. This establishes which character and inputs to test; it does not establish the current live character, performance, or successful spell execution. Character details, inventories, profile credentials, macro code and journals remain outside this repository.

The original profile disables GetFriendEnemyUsesIgnoreList although the macro's diagnostic labels it required. Preparation preserves this setting. Its combat consequences remain untested; no silent configuration correction is counted as original behavior.

## Native maintenance and retained failures

ArachnidArchmagePreparation selects the actual copied character, records its original capabilities privately and routes only the unsaved copy to the private login account. It stages the actor without changing skills, equipment, health, mana or supplies. The fixture creates no encounter. Original-source and copied-save preservation remain mandatory.

The first fixture mistakenly blessed the actor for preparation. Spell.Cast explicitly rejects blessed casters at Data/Scripts/Magic/Base/Spell.cs:627. Copy001 therefore completes the macro's maintenance loop without observing the required pools, Wraith Form or follower. Timers reported by the macro do not establish actual effects. This is an invalid fixture observation rather than evidence that the original macro cannot work. Copy002 is retained server setup only, stopped before any client or macro execution after the source rule was identified.

Corrected copy003 remains unblessed. The unchanged original macro and helper execute through ordinary copied-client commands. The wrapper preserves preparation, captures private diagnostics/journal, then withholds the first subsequent enemy scan and uses the macro's existing loop limit to stop before combat. It does not disable control, summons, Mass Death or another combat ability. This bounded observation is not the full combat script.

Forced runtime compile/startup and native preparation pass in003. Server observations match the original macro's expected pools and show Wraith Form, a follower and the immortality jewel. The jewel was already present; no new Withstand Death cast is inferred from its presence. The aggregate observations do not isolate every maintenance spell's effect or validate control/damage/recovery under pressure. All6,742 production inputs, accepted core,20 private save files, protected originals and acknowledged forced no-save teardown pass. Personal profile/macro/module contents remain unchanged.

## Reproduction and source traces

- [ArachnidArchmagePreparation.cs](../../tests/ArachnidArchmagePreparation.cs): exact private-root and disabled-production guards, unique copied character, account routing, initial staging, private before/after manifests, no-save acknowledgement.
- [PrepareNativeAcceptance.py](../../tools/champion-outbreak/PrepareNativeAcceptance.py): separately selected archmage-preparation fixture; no production script-tree insertion.
- [PrepareArchmageClient.py](../../tools/champion-outbreak/PrepareArchmageClient.py): original hash checks, private script copies, gameplay Options/Friends only, personal autologin exclusion, sole bounded autostart wrapper and private input freeze.
- [ArchmagePreparationClient.py](../../tools/champion-outbreak/ArchmagePreparationClient.py): process/root guards, share-delete observation, unchanged original execution, native diagnostics/journal and first post-maintenance scan boundary.
- [VerifyArchmagePreparation.py](../../tools/champion-outbreak/VerifyArchmagePreparation.py): original identity/capability checks, expected effects read from the private macro rather than published character values, preserved partial/setup-only outcomes, source/save/profile checks and forced teardown. Run copy003 with --require-ready; copy001 with --historical-fixture; copy002 with --historical-fixture --setup-only.
- [Spell.Cast](../../Data/Scripts/Magic/Base/Spell.cs): blessed-player rejection. [ResearchSpell](../../Data/Scripts/Magic/Research/ResearchSpell.cs): actual ancient casting/damage formulas and initial mana gate. Its GetMana scaling and Spell.CheckSequence scaling explain the helper's two-stage mana arithmetic; no spell-system change is authorized or made here.

Full stress work must retain actual earned advantages, distinguish a stationary macro from externally assisted walking, observe control/recovery/summons rather than trusting diagnostic timers, and report survival, clear time and actual resources without imposing the veteran benchmark's minimum duration. [SPECIFICATION.md](SPECIFICATION.md) remains controlling.
