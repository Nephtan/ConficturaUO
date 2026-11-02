# Level, archetype and actual readiness

Candidate diagnostic work for [tuning cycle2](TUNING_CYCLE_2_PLAN.md). This audit does not change the canonical Level formula, saved character fields, random-encounter aliases, skill caps or global progression. Player inventories and identifying survey records stay in private TestTools evidence.

## Current Level behavior and consumers

`CharacterLevelService.GetOverallLevel` converts normalized power into level1-100. For players, power is60% skill power,25% raw-stat/cap progress and15% fame/absolute-karma progress. Skill power is75% strongest adventure lane and25% total skill/cap progress. Each individual lane input uses effective skill divided by100, clamped to0-1. A skill above100 can therefore still matter greatly in combat while adding no further lane credit.

`GetArchetypeLevel` uses weighted skill/identity/karma lanes; it does not establish actual access to abilities. `GetDiagnostics` reports all lanes and can select a noncombat lane as its best archetype. Ties follow enum iteration rather than a loadout-based primary-role decision. Hybrids with many100-level skills can look similar despite very different equipment, prepared spells or available recovery.

Source-verified consumers:

- `Data/Scripts/System/Misc/Players.cs`: `GetPlayerLevel` delegates to the canonical service; `GetPlayerDifficulty` buckets that overall value. Player-facing information uses these results.
- `Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs`: player encounter aliases use `GetEncounterLevel`; non-player mobiles keep `GetLegacyMobileLevel`.
- `Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs`: GM `CharLevel` and `CharLevelTarget` expose diagnostics. The latter uses the existing12-tile target path.
- `Data/Scripts/Mobiles/Base/Behavior.cs`, `IntelligentAction`: creature-facing level calculations remain separate. They are not silently replaced by the player service.

Changing the canonical formula would affect more than champion balance. It needs its own reviewed consumer analysis and representative outcome comparisons. That migration is not necessary to reduce this outbreak's density or make its queen react to combat.

## What the added profile means

`CharacterCapabilityService.Inspect` adds read-only evidence to the existing GM diagnostic commands. It has no event hook, serializer, damage multiplier, reward gate or encounter-scaling caller.

| Evidence | Meaning | Limitation |
| --- | --- | --- |
| Nominal level | Existing canonical overall score | Not a measured solo combat rating |
| Training band | Highest base skill: newcomer<25, novice25-49.9, journeyman50-79.9, master80-99.9, veteran100+ | Highest skill can be noncombat; do not use this as an enemy-strength setting |
| Role tags | Relevant base-skill training in melee, archery, casting, taming, bard, support and crafting | Multiple tags indicate possible hybrid roles, not proof all abilities are usable |
| Current attack skill | The equipped weapon's actual `GetUsedSkill` result and effective skill | Skills trained for another weapon do not rescue the current loadout; weapon properties still need audit |
| Recovery counts | Carried bandages and selected healing/cure potions, plus immortality-jewel count | Amounts do not prove cooldown, required skill, charges, poison interactions or client cache visibility |
| Nearby companions | Real living creatures within24 tiles with this character as control/summon master; excludes dead bonded pets | Stabled/off-map pets are not current nearby strength. Ordinary login can return auto-stabled pets |
| Uncertainty flags | Missing matching attack skill, tithing, counted supplies or nearby pets; hybrid and support/caster caveats | Flags guide an audit; they do not declare a character unable to play |

The nearby query uses real creature/master references and releases its pooled enumeration. The survey separately records `Stabled` and `AutoStabled` counts. `PlayerMobile.ClaimAutoStabledPets`, invoked on login, explains why a saved tamer can have no nearby pets offline and seven after ordinary login. Offline absence must not be promoted to a no-pet combat result. `IsDeadBondedPet` matters because a fallen bonded companion can still have `Alive=true`.

## Saved-build selection

An offline native `World.Load` survey inspected1384 saved Player builds. It did not initialize all scripts, start listeners, save, or prove gameplay. Evidence is under `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/surveys/`; the current selection rules and identifying output are `SelectCohorts.py` and `selected-builds.private.json` beside it.

| Cohort beyond the previous four | Selection evidence | Test implication |
| --- | --- | --- |
| Journeyman melee | Level44, base Swordsmanship61.3, equipped effective sword skill about84, Healing32.8,696 bandages, zero tithing | Test real weapon combat and finite ordinary healing. Do not cast Knightship recovery with unavailable tithing. |
| Experienced archer | Level97, Marksmanship100 and a heavy crossbow, broad hybrid training,5629 bandages; four backpack bolts plus214 in the equipped quiver | This is an archer loadout test, not a pure-archetype character. The first backpack-only count understated ammunition. Audit quiver effects and actual finite consumption; an existing composite bow and498 arrows are a possible carried alternative. No ammunition grants. |
| Bard support | Level71, Music/Discordance/Provocation100 and Peacemaking98.2; actual instrument; weak current unarmed skill | Exercise real control/support with the instrument. A solo punch-until-death loop is a driver defect, not a useful bard balance test. |
| Melee/caster hybrid | Level85, sword/mage/Knightship training100, effective equipped sword110,148 bandages and84 tithing | Use legal current abilities with finite resources; audit actual books before casting. |
| Alternate tamer candidate | Taming93.6, Veterinary100, stored pets and993 bandages | Confirm returned pets/slots and actual veterinary actions in a real client before treating this as ready strength. |

The original novice, intermediate, Sargar and Zanthus remain separate reference cases. Zanthus is Jeremy's top solo mage benchmark and must actually fight with the unchanged original macro, normal safety validation and finite saved kit. Consumption of a death-prevention item is reported separately from final death events. Characters are not required to win.

These are selected candidates and test requirements, not claimed completed native cases. Existing natural growth, client-driver fidelity, outcomes, resource use and any failures are recorded separately as the cycle proceeds.

## Why this is useful without false precision

Use the level/lane vector to find contrasting saved builds, then use actual readiness evidence to choose a fair activity and driver. Test a novice's ability to kill a few hostile frontier creatures and reclaim a cell; test a bard's support effect; test an archer with the ammunition actually available; test Zanthus actively against the queen. Compare observed pressure, control, resource costs and outcomes. Do not make every player fight the same core, or scale a shared overworld queen independently for each viewer from a lossy level number.

The broader questions remain explicit: what should reputation contribute to a combat rating; how should over100 mastery be represented; how should stored pets, equipment effects and support be expressed; and should there be a combat-readiness index separate from overall character development? This cycle supplies evidence and diagnostics rather than silently deciding those global policies.
