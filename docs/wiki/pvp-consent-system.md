# System Name: PvP Consent System

## Overview

PvP Consent System stores player consent in `PlayerMobile.NONPK` and enforces it through shard-wide combat, notoriety, and region checks. The selector gump writes one of four enum values:

* `NONPK.Null` for neutral players
* `NONPK.PK` for PvP players
* `NONPK.NONPK` for PvE players
* `NONPK.NONPKinEvent` for PvE players who have been temporarily flipped by the event moongate

This is not a region-only or item-only feature. The chosen mode is serialized with the player, read by `Notoriety.Mobile_AllowBeneficial`, `Notoriety.Mobile_AllowHarmful`, `Notoriety.MobileNotoriety`, and `BaseRegion.AllowHarmful`, and then surfaced to players through the selection gump, the goddess NPC, and sample status-locked items.

## Script Inventory

* `Data/Scripts/Custom/PvPConsent/Gumps/PKNONPKGUMP.cs`
* `Data/Scripts/Custom/PvPConsent/GoddessOfProtection.cs`
* `Data/Scripts/Custom/PvPConsent/NONPKEventMoongate.cs`
* `Data/Scripts/Custom/PvPConsent/PKSword.cs`
* `Data/Scripts/Custom/PvPConsent/NONPKSword.cs`
* `Data/Scripts/Mobiles/Base/PlayerMobile.cs`
* `Data/Scripts/System/Misc/Notoriety.cs`
* `Data/Scripts/System/Regions/BaseRegion.cs`
* `Data/Scripts/Items/Potions/Standard/Conflagration Potions/BaseConflagrationPotion.cs`
* `Data/Scripts/Items/Potions/Standard/Frostbite Potions/BaseFrostbitePotion.cs`
* `Data/Scripts/Items/Potions/Special/MonsterSplatter.cs`

## Mode Selection Flow

1. A neutral player can open the selector gump with `[ChoosePvP`.
2. `ChoosePvPCommand` only opens the gump while `PlayerMobile.NONPK == NONPK.Null`.
3. `PKNONPK.OnResponse(NetState sender, RelayInfo info)` handles three reply IDs:
   * `0`: set `NONPK.Null`, clear `Title`, and announce neutrality
   * `1`: set `NONPK.PK`, set `Title = "[PvP]"`, and announce PvP
   * `2`: set `NONPK.NONPK`, set `Title = "[PvE]"`, and announce PvE
4. The goddess NPC can open the same gump again through speech, even after a player has already chosen a path.

## Goddess Of Protection Behavior

`GoddessOfProtection` is a blessed, immobile `BaseCreature` named `Thuvia`.

* Walking within 8 tiles triggers a greeting once per global 8-second cooldown.
* Saying `hail goddess`, `hail`, `pvp`, `pve`, `pk`, or `nonpk` prompts the player to say `choose`.
* Saying `help` explains the three paths in plain text.
* Saying `status` reports the current mode.
* Saying `choose` opens `PKNONPK`.

There is no separate registration command for the NPC. It must exist in the world as a placed mobile.

## Code-Verified Consent Matrix

| Mode | Stored enum | Title written by selector | Harmful actions against players and player-owned pets | Beneficial actions on players | Notes |
| --- | --- | --- | --- | --- | --- |
| Neutral | `NONPK.Null` | `null` | Can attack `PK`, other neutral players, and `NONPKinEvent`; cannot attack `NONPK` | Allowed on all player modes | Neutral is the only state that `[ChoosePvP` accepts directly. |
| PvP | `NONPK.PK` | `[PvP]` | Can attack `PK`, neutral, and `NONPKinEvent`; cannot attack `NONPK` | Allowed on all player modes | Standard opt-in PvP state. |
| PvE | `NONPK.NONPK` | `[PvE]` | Cannot initiate harmful actions against players or player-owned pets | Cannot perform beneficial actions on `PK`; can beneficial neutral, `NONPK`, and `NONPKinEvent` | Can still attack NPCs and creatures not owned by a `PlayerMobile`. |
| Event PvE | `NONPK.NONPKinEvent` | unchanged | Can attack `PK`, neutral, and other `NONPKinEvent`; cannot attack `NONPK` | Allowed on all player modes | Only the event moongate sets this state, and only from `NONPK`. |

## Harmful-Action Details

* `Notoriety.Mobile_AllowHarmful` is the main consent gate.
* Player-owned pets inherit consent from their controlling `PlayerMobile`.
* PvE players cannot start attacks against players or player-owned pets.
* Other players and player-owned pets cannot attack PvE players or PvE-owned pets.
* PvE players can still attack NPCs and creatures that are not owned by a `PlayerMobile`.
* NPCs and unowned creatures can still attack players regardless of consent state.
* `BaseRegion.AllowHarmful` duplicates direct player-versus-player blocks so the region layer still denies attacks on PvE players even before later combat code runs.
* `Notoriety.MobileNotoriety` returns `Invulnerable` whenever the source or target is in `NONPK.NONPK`, so PvE players show protected notoriety to other players.

## Beneficial-Action Details

* `Notoriety.Mobile_AllowBeneficial` is the main beneficial gate.
* The only direct consent-specific beneficial block is `NONPK.NONPK -> NONPK.PK`.
* PvP and neutral players can perform beneficial actions on any player mode.
* PvE players can beneficial neutral players, other PvE players, and `NONPKinEvent` players.
* NPCs are allowed to perform beneficial actions toward any target.

## Event And Override Paths

### XML Event Overrides

Consent is not absolute inside the shard's XML event helpers.

* `XmlPoints.AreChallengers(attacker, target)` returns `true` before the PvE harmful checks, so XML event challengers can harm each other even when normal consent would block it.
* `XmlPoints.AreInAnyGame(target)` makes beneficial actions depend on `XmlPoints.AreTeamMembers(from, target)` before the normal consent checks run.

### Event Moongate

`NONPKEventMoongate` is a separate item-based override path.

* Double-clicking it at range 1 or walking over it starts a 1-second delay timer.
* If the source location is guarded town and the destination is not, the script opens `NONPKEventMoongateConfirmGump` before moving the player.
* `UseGate` rejects sigil carriers, young players going to `Map.Lodor`, casters with an active spell, and murderers traveling anywhere except `Map.Lodor`.
* Successful travel teleports pets, moves the mobile, plays the moongate sound, and then calls `OnGateUsed`.
* `OnGateUsed` changes `NONPK.NONPK` to `NONPK.NONPKinEvent`.
* Reusing the same type of gate while already in `NONPK.NONPKinEvent` changes the player back to `NONPK.NONPK` and clears `Combatant`.
* Neutral and PvP players are never changed by `OnGateUsed`.

`NONPKEventConfirmationMoongate` is also defined in the same file, but it derives from `Moongate`, not `NONPKEventMoongate`, so it does not toggle `PlayerMobile.NONPK`.

## Status-Locked Example Items

The folder includes two example `BaseSword` derivatives:

* `PKSword` only equips when `PlayerMobile.NONPK == NONPK.PK`.
* `NONPKSword` only equips when `PlayerMobile.NONPK == NONPK.NONPK`.

These checks happen only in `OnEquip`. The items do not auto-unequip when a player's consent mode changes later.

## Supplemental Splash-Damage Checks

Some harmful items duplicate the consent matrix instead of relying only on `Notoriety`.

* `BaseConflagrationPotion`
* `BaseFrostbitePotion`
* `MonsterSplatter`

Those scripts independently allow player damage only when the thrower/target combination matches the same broad rules as the main consent system:

* PvE players only hit themselves
* PvP players hit PvP and neutral players
* Neutral players hit PvP and neutral players

## Technical Trace

* Wiki claim: the selector is one-time only -> traced `ChoosePvP_OnCommand` and `GoddessOfProtection.CheckSpeech` -> `[ChoosePvP` only opens for `NONPK.Null`, but the goddess NPC can reopen the same gump later through `choose`.
* Wiki claim: PvE players cannot participate in player combat -> traced `Notoriety.Mobile_AllowHarmful` and `BaseRegion.AllowHarmful` -> direct attacks and pet attacks against players or player-owned pets are blocked, but attacks against NPCs and unowned creatures still succeed.
* Wiki claim: PvE players cannot help PvP players -> traced `Notoriety.Mobile_AllowBeneficial` -> only the `NONPK.NONPK -> NONPK.PK` beneficial path is blocked by consent itself.
* Wiki claim: the event gate creates temporary PvP participation for PvE players -> traced `NONPKEventMoongate.OnGateUsed` plus the harmful/beneficial checks -> the gate flips PvE players to `NONPKinEvent`, and the relaxed behavior works because the main consent code only special-cases `Null`, `PK`, and `NONPK`.
* Wiki claim: sample status weapons enforce the path choice -> traced `PKSword.OnEquip` and `NONPKSword.OnEquip` -> they gate equipping only and do not re-check the state after later mode changes.

## Persistence

`PlayerMobile` serializes version `37` and writes both `m_NONPK` and `Title`. That means the selected consent mode survives saves, logouts, and restarts like normal player data.

`NONPKEventMoongate` serializes version `1` and writes:

* `Target`
* `TargetMap`
* `Dispellable`

`GoddessOfProtection`, `PKSword`, and `NONPKSword` each serialize version `0` only. `NONPKEventConfirmationMoongate` also serializes its warning-gump configuration separately from the actual consent-state toggle.

## Observed Implementation Gaps

* `NONPKinEvent` is persisted in `PlayerMobile`, but no login, logout, region, or timer code clears it. The only scripted reset is using an event moongate again, so a PvE player can remain in event mode indefinitely if they save or leave without the return gate.
* `PKNONPK` uses `PlayerMobile.Title` as the consent label and clears it on neutral selection instead of layering a separate indicator.
* `NONPKEventMoongate` changes the enum but never updates the title, so a player can still display `[PvE]` while actually operating under `NONPKinEvent` rules.

## XMLSpawner

This system does not use XMLSpawner attachments or XMLSpawner packet hooks. It is driven by:

* a player command
* an in-world `BaseCreature`
* a custom `Item` moongate
* direct hooks in `PlayerMobile`, `Notoriety`, region logic, and a few harmful-item scripts
