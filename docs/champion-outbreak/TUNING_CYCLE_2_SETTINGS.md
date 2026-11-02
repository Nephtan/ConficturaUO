# Arachnid tuning and participation controls

Candidate implementation on `ChampRevamp`, not production activation. `ArachnidOutbreakController.DeliveryReady` remains false. See [cycle plan](TUNING_CYCLE_2_PLAN.md) for scope and evidence boundaries.

## Administrator workflow

`[ArachnidOutbreak settings <controller serial>` prints the persisted settings and current queen/backup/reward diagnostics. The command requires Administrator access and validates the caller and controller lookup. Administrator `[Props` exposes the ten controls below. All setters require the controller's phase to be `Disabled`; an active edit is rejected without partial mutation. Stopping an outbreak is a separate explicit action that removes its owned encounter entities. Editing a setting never stops it automatically.

Settings belong to a controller, survive saves, and govern its next attempt. They are not global character-level or shard scheduler settings. A returned `Tuning` object is a copy, not a live editing bypass. This restriction keeps a running comparison reproducible and avoids silently changing creatures already fighting.

| Control | Default | Allowed | Meaning and interactions |
| --- | --- | --- | --- |
| `InnerPopulationPercent` |45 |20-100 | Total inner residents are floor(previous3/5/7 targets × percentage), minimum1: defaults1/2/3. The same percentage spaces guardian-bearing cells, rather than turning every one-resident outer inner-band cell into a guardian. Root retains its guardian.100 provides the previous full-density control. It does not expand the300-by-300 footprint or alter creature damage. |
| `ReplacementSeconds` |30 |5-180 | Minimum interval after admitting a normal resident. Also affects how quickly a new cell fills out.5 reproduces the previous refill interval. Killing a guardian or reclaiming a frontier foothold permanently suppresses its patch for that attempt. |
| `FrontierPerception` |8 |4-12 | Ordinary tier0 AI acquisition range. This does not bypass visibility, LOS, staff, honor, legality or current control effects. |
| `FrontierPursuit` |24 |12-40 | A frontier target must stay within this square range of the spider's home. Must exceed perception by at least4. This is a target/pursuit boundary, not teleportation or AI freezing. |
| `LocalWarningSeconds` |10 |5-30 | Delay before the local2/3/4 defensive waves. The existing120-second recent-kill window,3/6/10 thresholds, cooldown and nine-birth nest allowance remain fixed policy. |
| `QueenRetreatPercent` |45 |20-70 | Low-health withdrawal threshold. Recent substantial damage or being outnumbered while injured can also signal a losing fight. |
| `QueenReengagePercent` |70 |45-95 | Recovery needed to leave withdrawal. Must exceed retreat by at least15 percentage points. There is also a commitment interval and a readable challenge tell; crossing a single threshold does not cause oscillation. |
| `QueenBackupCount` |6 |0-12 | Lifetime backup births for the attempt, normally up to three per call.0 disables new queen backup. Spent births are saved and never refunded by recovery or killing guards. Distinct from each outskirts nest's nine-birth budget. |
| `QueenBackupDelaySeconds` |10 |5-30 | Warning window at a visible living rally guardian. Destroying that source cancels pending births. Existing guards remain real; they are not erased. A call expires if safe birth ground or eligible nearby participants cannot be found. |
| `FrontierSuppliesEnabled` |true |true/false | One small supply claim per qualifying character per attempt after a frontier reclamation. This is not a rare-item lottery or a power reward scaled to level. |

## Queen decisions and limits

`ArachnidQueenAdvantage` uses the queen's current health fraction, actual damage over the recent ten seconds, nearby living brood and player/pet counts, and her opponent's health. This is a bounded tactical heuristic, not learned AI or a claim to know each build's damage potential. Egg sacs, staff, blessed targets and dead bonded pets do not count as combat strength. Charmed/controlled brood do not count as her allies.

The queen gives a three-second challenge before advancing. Engagement uses normal melee AI within a24-tile local pursuit bound and occupied territory. Losing prompts withdrawal along an ordinary path to a nearby occupied point away from the opponent. She remains attackable, receives no free healing, and respects ordinary movement/control constraints. Reconsideration runs at most every two seconds; commitment lasts12 seconds on engagement and15 on retreat, with an emergency-health exception. A blocked or exhausted escape can still fail.

A retreat can call up to three guards from a nearby living inner guardian, subject to a60-second call cooldown and the saved total allowance. The source flashes and announces itself. Births use the existing shared admission machinery and participant-filtered, loot-free defender type. The source must remain alive, eligible contributing players must remain nearby, and births avoid appearing within10 tiles of players/pets. Pending calls are transient and cancelled on recovery rather than replayed; existing guards and their principal references persist normally.

## Reclamation and loot progression

| Participation | Concrete reward/progress | Bounds |
| --- | --- | --- |
| Weak outskirts hunting | Killing a resident in a non-sac frontier cell retires that cell and clears its webs. A sac cell requires destroying its sac. Established daughters remain independent. | No reclamation supply claim for an administrative deletion, founder/NPC-only kill or reinforcement farming. Ordinary corpse loot is separate. Existing active creatures are preserved. |
| First qualifying reclamation | Pending claim containing20 bandages, one lesser healing potion and one lesser cure potion. | Ordinary damage/looting rights, responsible pet masters and32-tile proximity; once per character per attempt. Separate saved paid receipts prevent collecting/deleting a claim from resetting eligibility. |
| Suppression milestones and support | Existing venom-gland stage claims, including existing validated healing, poison prevention and bard credit. | Stage sealing and existing qualification stay intact. A small damage tag is not general stage qualification. |
| Craft/trade preparation | Existing Alchemy recipe uses venom glands to make brood antivenom; the current recipe requires80-100 Alchemy and produces two treatments. | No new universal self-sufficiency rule. The treatment is protection against future wild poison, not a cure for existing poison. |
| Qualified queen victory | Existing skull entitlement, a possible artifact claim and distinct25×600-gold shower with animation. | The inherited artifact roll is5% unique,10% shared and15% decorative; otherwise no artifact. Real qualified victory only; no PowerScroll replacement lottery, no victory claims/shower for NPC-only victory, no forced victory in testing. |

The small starter package is useful progress, not proof that the economy is balanced. Native tests must record actual looting, claim contents, consumables spent and opportunity costs. Gear/material progression and cosmetic trophies remain extension candidates; a new named power-item pool is not silently added in this cycle.

## Save and source map

- `ArachnidOutbreakController` advances version3 to4. All version0-3 fields keep their types and order. Version4 appends tuning, queen-backup expenditure and a strong-mobile list of frontier-paid recipients. Older versions receive validated defaults, zero expenditure and an empty receipt list.
- `ArachnidOutbreakTuning` is a value serializer, version0: population, replacement, frontier perception/pursuit, local warning, queen retreat/reengage, backup allowance/delay, supplies flag. Validation runs on read. It is not a new world Item or Mobile type.
- `ArachnidOutbreakClaimKind.FrontierSupplies` is appended to the enum, retaining existing numeric values and the existing claim serializer. The new package uses existing Bag/Bandage/potion types.
- `ArachnidInfestationPatch` remains version2; creature and reinforcement serializers retain their existing layouts. Tactical planning, recent damage and pending calls are rebuilt, not replayed.
- Source: `Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakTuning.cs`, `ArachnidQueenTactics.cs`, `ArachnidQueenShelter.cs`, `ArachnidTerritoryPolicy.cs`, `ArachnidInfestation.cs`, `ArachnidInfestationActors.cs`, `ArachnidFrontierRewards.cs`, `ArachnidOutbreakClaim.cs`, `ArachnidBroodResistance.cs`, `ArachnidOutbreakController.cs`; existing recipe `Data/Scripts/Trades/Crafting/DefAlchemy.cs` and treatment `BroodAntivenom.cs`.
- Existing artifact probability/pools: `Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakCreatures.cs`, `ArachnidOutbreakChampion.RollVictoryArtifact`; `Data/Scripts/Custom/Champions/System/Mobiles/BaseChampion.cs`, `GetArtifact`; `Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Mephitis.cs`, the nonempty unique/shared/decorative lists. A missing artifact after a qualified victory is therefore a possible ordinary outcome.

Current component/runtime compile results do not establish natural-client balance, resumed live recovery, civic guard respawn, final visual approval or economic acceptance. Existing visible/engaged actors from an older density remain protected; no instantaneous visual thinning is promised.

When editing linked thresholds, preserve validity at each step: raise reengagement before raising retreat, or lower retreat before lowering reengagement. Likewise, raise pursuit before perception when necessary. A rejected edit leaves the existing settings unchanged. Stopping a controller ends its attempt and removes its owned encounter entities; it is not a pause/resume or a harmless way to retune a live battle.
