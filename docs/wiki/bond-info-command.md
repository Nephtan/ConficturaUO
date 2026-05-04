# System Name: Bond Info Command

## Overview

`BondInfoCommand` adds the player command `[BondInfo`. The command targets a controlled pet and reports whether that pet is already bonded, has not started bonding, is still counting down to bonding, or is ready to bond on the next qualifying feed.

This system is a read-only command helper. It does not create pets, alter bonding state, serialize its own data, or add XMLSpawner hooks.

---

## Player Use

1. Type `[BondInfo`.
2. Target one of your controlled pets.
3. Read the status message returned by the command.

Valid targets are `BaseCreature` instances whose `ControlMaster` is the player using the command. Invalid creature targets reopen the target cursor and send `That is not your pet!`; non-creature targets reopen the target cursor and send `That is not a pet!`.

---

## Code-Verified Behavior

* **Command registration:** `Initialize()` registers `BondInfo` at `AccessLevel.Player`.
* **Targeting:** `BondInfo_OnCommand` starts an unlimited-range, non-harmful target and prompts `Target the pet you wish to know the bonding timer of`.
* **Already bonded:** If `BaseCreature.IsBonded == true`, the player receives `Bonded`.
* **Bonding not started:** If `BaseCreature.BondingBegin == DateTime.MinValue`, the player receives `Your pet hasn't started to bond yet, please feed it and try again.`
* **Countdown active:** The command calculates `willbebonded = BondingBegin + BondingDelay`, subtracts `DateTime.Now`, and displays `BondingBegin` plus remaining days, hours, and minutes.
* **Timer elapsed:** If the remaining `TimeSpan` has no positive days, hours, or minutes, the command sends `Ready to bond!`. The command does not set `IsBonded`; the actual bond is completed by the pet-feeding code.

---

## Technical Trace

* Wiki claimed `[BondInfo` lets players target a pet -> traced `BondInfoCommand.Initialize` and `BondInfo_OnCommand` -> code registers `BondInfo` for players, starts a target cursor, and prompts for a pet.
* Wiki claimed only controlled pets work -> traced `BondInfo_OnTarget` -> code accepts only `BaseCreature` targets where `ControlMaster == from`, then re-prompts for non-owned creatures or non-pets.
* Wiki claimed bonded, not-started, and timer states -> traced `BondInfo_OnTarget` -> code sends `Bonded`, the feed-to-start message, a countdown, or `Ready to bond!`.
* Wiki claimed the formula `(BondingBegin + BondingDelay) - current time` -> traced `BondInfo_OnTarget` and `BaseCreature.BondingDelay` -> code uses `DateTime.Now`, `BondingBegin + BondingDelay`, and `TimeSpan.FromDays(Server.Misc.MyServerSettings.BondDays())`.
* XMLSpawner references -> traced this system by class and command name -> no XMLSpawner configuration, attachment, or spawn-entry integration is used by `BondInfoCommand`.

---

## Persistence

`BondInfoCommand` has no `Serialize` or `Deserialize` methods. The bonding fields it reads are persisted by `BaseCreature`, which writes and reads the version 10 bonding state:

* `m_IsDeadPet`
* `m_IsBonded`
* `m_BondingBegin`
* `m_OwnerAbandonTime`

---

## Known Code Issue

`BaseCreature.BondingDelay` delegates to `MyServerSettings.BondDays()`. The settings loader reads `S_BondDays`, but the compiled `BondDays()` helper initializes a local `days` value to `7` and only changes it when `S_BondDays > 30` or `S_BondDays < 0`. Normal in-range configured values are therefore ignored, so the `[BondInfo` countdown effectively uses a 7-day delay except for the clamp edge cases.
