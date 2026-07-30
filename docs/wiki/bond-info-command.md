# System Name: Bond Info Command

## Overview

`BondInfoCommand` adds the player command `[BondInfo`. The command targets a controlled pet and reports whether that pet is already bonded, has not started bonding, is still counting down to bonding, or is ready to bond on the next qualifying feed.

Code-Verified: 2026-05-06

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
* **Timer elapsed:** If the remaining `TimeSpan` has no positive days, hours, or minutes, the command sends `Ready to bond!`. The command does not set `IsBonded`; `BaseCreature.OnDragDrop` completes the bond only when the owner feeds the pet again after the delay has elapsed.

---

## Technical Trace

* Wiki claimed `[BondInfo` lets players target a pet -> traced `BondInfoCommand.Initialize` and `BondInfo_OnCommand` -> code registers `BondInfo` for players, starts a target cursor, and prompts for a pet.
* Wiki claimed only controlled pets work -> traced `BondInfo_OnTarget` -> code accepts only `BaseCreature` targets where `ControlMaster == from`, then re-prompts for non-owned creatures or non-pets.
* Wiki claimed bonded, not-started, and timer states -> traced `BondInfo_OnTarget` -> code sends `Bonded`, the feed-to-start message, a countdown, or `Ready to bond!`.
* Wiki claimed the formula `(BondingBegin + BondingDelay) - current time` -> traced `BondInfo_OnTarget` and `BaseCreature.BondingDelay` -> code uses `DateTime.Now`, `BondingBegin + BondingDelay`, and `TimeSpan.FromDays(Server.Misc.MyServerSettings.BondDays())`.
* Wiki claimed the pet becomes bonded after the countdown -> traced `BaseCreature.OnDragDrop` -> code only marks `IsBonded = true` when the owner feeds the pet again after the countdown has elapsed; `[BondInfo` is read-only.
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

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0079.
- Backlog rows: RB-06668.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/BondInfo/BondInfo.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/BaseCreature.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=1; Initialize=2; Movement=3; Speech=3; Timer=6.
- Data/Scripts/Custom/BondInfo/BondInfo.cs:L12 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/BondInfo/BondInfo.cs:L14 Command CommandSystem.Register access=Unknown
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L487 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L914 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L950 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L5879 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L8605 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L8609 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L8612 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L8790 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L8792 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L8800 Movement OnMovement access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 1.
- Data/Scripts/Mobiles/Base/BaseCreature.cs:Server.Mobiles.BaseCreature version=19 serialize=L6507 deserialize=L6665 alignment=TypeMismatch:#30:Write=bool/Read=Double;#32:Write=DeltaTime/Read=Bool;#33:Write=int/Read=DeltaTime;#37:Write=int/Read=Mobile;#53:Write=bool/Read=StrongMobileList;#55:Write=DateTime/Read=Bool;#57:Write=bool/Read=DateTime;#61:Write=bool/Read=StrongMobileList;#62:Write=int/Read=Bool

### Project And Runtime Coverage

- Data/Scripts/Custom/BondInfo/BondInfo.cs=Keep
- Data/Scripts/Custom/BondInfo/BondInfo.cs=Keep
- Data/Scripts/Mobiles/Base/BaseCreature.cs=Keep
- Data/Scripts/Mobiles/Base/BaseCreature.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
