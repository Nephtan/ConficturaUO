# Pet stabling, safe logout, and weekly billing

This is the canonical description of animal trainer stabling, automatic shelter, and stable billing. Source behavior is described here; deployment must be confirmed separately.

## Player behavior

- Each new stay has seven free real days. After that, each stabled pet costs **30 gold per week**, plus **one gold per stone of pack cargo**.
- Cargo is the recursive weight of a carrier's `StrongBackpack` contents, excluding the carrying backpack itself. The weekly rate is fixed for that stay.
- Loaded pack animals are accepted by manual stables. Existing manual eligibility and capacity checks still apply.
- Instant logout shelters eligible controlled followers even when normal stable capacity is full. This includes public areas, qualifying homes, and bedroll logout. Existing follower coverage has no distance requirement.
- Summons retain their existing cleanup rules. Ridden mounts retain the existing rider lifecycle.
- Automatic returns respect follower capacity and ghost state. A blocked return stays pending for a later login or explicit claim.
- The billing timer withdraws bank gold and bank checks. Explicit retrieval can also use backpack gold; an insufficient combined balance is left untouched.
- Unpaid pets and cargo stay sheltered indefinitely. They are never released, sold, or deleted for unpaid fees. Retrieval requires settling outstanding fees.
- There is no upfront payment. Retrieving a pet ends its stay; stabling it again begins a new seven-day grace period.
- Pets already stabled when this feature is first loaded receive seven free days from adoption, with no retroactive debt.
- Weekly billing includes empty auto-stabled pets and mounts temporarily sheltered by no-mount areas.

### Bedroll logout

Successfully laying out a bedroll makes instant logout available immediately while the owner is within three paces of that live bed. The existing bed lasts 100 seconds and the ten-minute placement cooldown still applies. The owner must be alive, on the same world map, within 16 units of height, and have line of sight to the bed. Camping must be allowed, away from boats and nearby hostile creatures, with no criminal status, combatant, or recent incoming/outgoing aggression. No campfire is required by this bedroll workflow.

Walking out of range, rolling up or deleting the bed, expiration, or unsafe conditions revoke bedroll instant logout. Returning to the still-active safe camp restores eligibility. The disconnect handler checks this live eligibility before sheltering pets; no persistent bedroll grant is set. A successful instant bedroll logout therefore protects loaded followers beyond normal stable capacity. A failed placement or cooldown rejection grants no protection. Existing laid-out beds roll up on world loading; they do not carry a logout grant across a restart.

## Commands and interfaces

Animal trainers and shepherds retain speech keywords `stable` (keyword `0x0008`) and `claim` (`0x0009`), including `claim <pet name>`. Context menus and existing claim gumps remain available. Stable stones and city stables also use the shared service. Summoning balls settle outstanding stable fees before retrieving a stabled pet or consuming a charge.

Claim-list pet buttons use ID `i + 1`; responses convert to `ButtonID - 1` and bounds-check before delegating retrieval. Existing bonding-status displays remain unchanged. Stable interactions report the weekly rate, next billing deadline in UTC, and outstanding fees. Trainer/manual retrieval validates the actor, ownership list, range/map, alive state, follower capacity, and payment. No new staff command is introduced.

## Mobile Definition

`AnimalTrainer` derives from `BaseVendor`, uses the title `"the animal trainer"`, and belongs to `NpcGuild.DruidsGuild`.

On construction the trainer receives these skill ranges:

| Skill | Minimum | Maximum |
| --- | ---: | ---: |
| `Druidism` | 64.0 | 100.0 |
| `Taming` | 90.0 | 100.0 |
| `Veterinary` | 65.0 | 88.0 |

`InitSBInfo()` picks the regional animal-trainer shop list from the trainer's current world or crypt state, then always adds `SBAnimalTrainer` and `SBBuyArtifacts`.

| Region/world gate | Added shop list |
| --- | --- |
| `"the Serpent Island"` | `SBGargoyleAnimalTrainer` |
| `"the Land of Lodoria"` | `SBElfAnimalTrainer` |
| `"the Isles of Dread"` | `SBBarbarianAnimalTrainer` |
| `"the Savaged Empire"` | `SBOrkAnimalTrainer` |
| `Worlds.IsCrypt(Location, Map)` | `SBDeadAnimalTrainer` |
| Fallback | `SBHumanAnimalTrainer` |

`InitOutfit()` calls the base vendor outfit setup and then equips either a `QuarterStaff` or a `ShepherdsCrook`.

## Player Interaction Surface

### Context Menus

`GetContextMenuEntries()` always adds three custom entries after base vendor entries:

| Entry class | Caller gate | Result |
| --- | --- | --- |
| `ClaimingGumpEntry` | Caller must be `PlayerMobile` | Calls `BeginClaimList()` to open the claim-list `Gump`. |
| `SpeechGumpEntry` | Caller must be `PlayerMobile`; no existing `SpeechGump` | Opens `SpeechGump(mobile, "Animal Companions", SpeechFunctions.SpeechText(..., "Pets"))`. |
| `RidingGumpEntry` | Caller must be `PlayerMobile`; no existing `Veterinarian.RidingGump` | Opens `Veterinarian.RidingGump`. |

`AddCustomContextEntries()` adds the stable entry only when the caller is alive. It also adds `ClaimAllEntry` when `from.Stabled.Count > 0`.

| Entry class | Gate | Result |
| --- | --- | --- |
| `StableEntry` | Caller alive | Calls `BeginStable()`. |
| `ClaimAllEntry` | Caller alive and `from.Stabled.Count > 0` | Calls `Claim(from)`, attempting to claim all eligible stabled pets. |

### Speech Keywords

`HandlesOnSpeech(Mobile from)` returns `true`, so nearby speech events that reach the trainer are passed to `OnSpeech()`.

| Keyword ID | Commented keyword | Speech shape | Result |
| --- | --- | --- | --- |
| `0x0008` | `*stable*` | Speech containing the encoded stable keyword | Closes any `ClaimListGump` and calls `BeginStable()`. |
| `0x0009` | `*claim*` | `claim` | Closes any `ClaimListGump` and calls `Claim(from)`. |
| `0x0009` | `*claim*` | `claim <pet name>` | Passes the substring after the first space to `Claim(from, petName)`. |

There are no bracket commands such as `[stable` or `[claim`; this is speech-keyword and context-menu driven.

## Stable Slot Formula

`GetMaxStabled(Mobile from)` uses the caller's `Taming`, `Druidism`, `Veterinary`, and `Herding` values.

First it sums the four skill values:

```text
skillSum = Taming + Druidism + Veterinary + Herding
```

Then it assigns the base stable capacity:

| `skillSum` gate | Base slots |
| ---: | ---: |
| `>= 400.0` | 7 |
| `>= 300.0` | 6 |
| `>= 240.0` | 5 |
| `>= 200.0` | 4 |
| `>= 160.0` | 3 |
| `< 160.0` | 2 |

After that, each individual skill at or above `100.0` adds:

```text
(int)((skillValue - 90.0) / 10)
```

The cast truncates fractional values toward zero. For example, a skill from `100.0` through `109.9` adds `1`, `110.0` through `119.9` adds `2`, and so on.

The final value adds `MyServerSettings.ExtraStableSlots()`. That setting returns `S_Stables` clamped to the range `0..20`. There is no final cap after the individual skill bonuses and extra-slot setting are added.

## Ownership and capacity

`AnimalTrainer.GetMaxStabled()` retains its skill-based formula: Taming, Druidism, Veterinary and Herding, including skill bonuses and the configured extra slots. City stables retain their existing manual capacity formula.

Protected automatic shelter can exceed that capacity. These pets remain in the owner's existing `Stabled` list and count toward subsequent manual capacity checks. Protection is not unlimited active follower capacity.

The service internalizes a pet before clearing its control master, records the owner on the stable list, marks it stabled, and preserves its contents. Automatic returns are registered after master removal because `BaseCreature.RemoveFollowers()` also removes automatic-return entries. Repeated protection does not duplicate pets or reset a stay.

## Runtime hooks and guards

| Entry point | Trigger and behavior |
| --- | --- |
| `PlayerMobile.OnDisconnected()` | Captures effective instant logout and shelters pets before other disconnect subscribers, including save-on-disconnect. |
| `PlayerMobile.OnLogout()` / `AutoStablePets()` | Idempotent fallback; delayed logout retains its capacity and loaded-carrier exclusions. Snapshots followers and validates current ownership/deleted state. |
| `PlayerMobile.CheckPets()` | Startup recovery checks offline players with active followers without comparing unrelated automatic-stable counts. Uses captured protection or saved region/home eligibility. |
| `PlayerMobile.ClaimAutoStabledPets()` | Login attempts pending returns. Null/deleted entries are cleaned safely; unpaid, ghost and follower-limited returns remain pending. |
| `StableService.Initialize()` | Adopts existing stable records and starts a five-minute billing timer. |
| `StableService.TryReturn()` | Shared payment/ownership/movement guard for automatic return, manual claim, mount restoration and summoning. |

Failures are logged with owner/pet serials. A failed pet operation does not prevent the remaining pets from being processed. A partially internalized pet retains its stable ownership record; a failed return keeps already paid fees paid.

## Billing and persistence

Deadlines use UTC. Billing accrues every elapsed whole seven-day boundary, including downtime, and collection is retried without duplicating invoices. Merely opening a claim menu displays billing without withdrawing gold. Recurring collection uses bank funds only. City-origin stays credit their original city's treasury while it remains valid; otherwise payments are a gold sink.

`PlayerMobile` version **38** prefixes the unchanged version-37 payload with:

1. Captured instant-logout protection flag.
2. Billing-record count.
3. Each inline billing record: version `0`, pet mobile reference, weekly rate (`int`), next UTC deadline (`DateTime` ticks), outstanding balance (`long`), optional city item reference.

Version 37 and older reads retain their original order. Missing billing records are adopted with seven days of grace. Creature serializers, namespaces and type identities are unchanged. The normal `Mobile.Stabled` and `PlayerMobile.AutoStabled` lists remain authoritative for ownership and pending return.

Before deployment, back up the full saves. Rollback requires compatible code **and** the pre-update save backup; old code must not load version-38 player records.

## Source traces

- [StableService.cs](../../Data/Scripts/Custom/PetStabling/StableService.cs): intake, cargo rate, billing records, reconciliation, timer, atomic insufficient-funds checks, shared return and claim handling.
- [PlayerMobile.cs](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs): disconnect/logout/login hooks, startup recovery, automatic lists, version 38.
- [Bedroll.cs](../../Data/Scripts/Items/Explorers/Bedroll.cs): Player double-click, Camping check, placement, cooldown and nearby owned-bed guard. [BedrolledOut.cs](../../Data/Scripts/Items/Explorers/BedrolledOut.cs): `IsSafeCamp`, `CanLogout`, 100-second lifetime, roll-up and unchanged version-0 serializer. [Kindling.cs](../../Data/Scripts/Items/Explorers/Kindling.cs): shared camping region and hostile-creature rules.
- [AnimalTrainer.cs](../../Data/Scripts/Mobiles/Civilized/Vendors/AnimalTrainer.cs): trainer interfaces, normal capacity, no-mount shelter/restoration.
- [Shepherd.cs](../../Data/Scripts/Mobiles/Civilized/Vendors/Shepherd.cs), [StableStone.cs](../../Data/Scripts/Items/Explorers/StableStone.cs), [CityStableStone.cs](../../Data/Scripts/Custom/Government%20System/Items/Stones/CityStableStone.cs): alternate stable entry and retrieval paths.
- [BallOfSummoning.cs](../../Data/Scripts/Items/Special/Solen%20Items/BallOfSummoning.cs): paid retrieval before charge consumption.
- [BaseCreature.cs](../../Data/Scripts/Mobiles/Base/BaseCreature.cs): control/follower callbacks, stable flags, loyalty and deletion timers.
- [Talk.cs](../../Data/Scripts/System/Misc/Talk.cs): player-facing stable explanation.
- [StableRegression.cs](../../tests/StableRegression.cs), [StableWorldRoundtrip.cs](../../tests/StableWorldRoundtrip.cs), [Test-Stabling.ps1](../../scripts/Test-Stabling.ps1): production-method, serializer and isolated verification.

Run `scripts/Test-Stabling.ps1` from the repository root. Record actual component, copied-world and startup results separately; source inspection and project builds alone do not establish deployed player behavior.

See [bedroll logout verification](../verification/2026-11-05-bedroll-logout.md) for the isolated native Player acceptance and its evidence limits.
